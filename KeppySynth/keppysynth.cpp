/*
Keppy's Synthesizer, a fork of BASSMIDI Driver

Thank you Kode54 for allowing me to fork your awesome driver.
*/

#if !_WIN32
#error The driver only works on 32-bit and 64-bit versions of Windows x86. ARM is not supported.
#endif

#define VC_EXTRALEAN

#pragma comment(lib,"Version.lib")

#include "sha256.h"
#include "stdafx.h"
#include <dbghelp.h>
#include <Psapi.h>
#include <Shlwapi.h>
#include <Tlhelp32.h>
#include <assert.h>
#include <atlbase.h>
#include <atlstr.h>
#include <cctype>
#include <comdef.h>
#include <fstream>
#include <iostream>
#include <limits>
#include <list>
#include <mmddk.h>
#include <mmsystem.h>
#include <process.h>
#include <process.h>
#include <shellapi.h>
#include <shlobj.h>
#include <signal.h>
#include <sstream>
#include <stdarg.h>
#include <stdio.h>
#include <stdlib.h>
#include <string.h>
#include <string>
#include <tchar.h>
#include <time.h>
#include <vector>
#include <winbase.h>
#include <windows.h>
#include <winnt.h>
#include "Resource.h"

// BLiNX best game

#define BASSASIODEF(f) (WINAPI *f)
#define BASSDEF(f) (WINAPI *f)
#define BASSENCDEF(f) (WINAPI *f)	
#define BASSMIDIDEF(f) (WINAPI *f)	
#define BASSWASAPIDEF(f) (WINAPI *f)
#define BASS_FXDEF(f) (WINAPI *f)
#define BASS_VSTDEF(f) (WINAPI *f)
#define LOADBASSASIOFUNCTION(f) *((void**)&f)=GetProcAddress(bassasio,#f)
#define LOADBASSENCFUNCTION(f) *((void**)&f)=GetProcAddress(bassenc,#f)
#define LOADBASSFUNCTION(f) *((void**)&f)=GetProcAddress(bass,#f)
#define LOADBASSMIDIFUNCTION(f) *((void**)&f)=GetProcAddress(bassmidi,#f)
#define LOADBASSWASAPIFUNCTION(f) *((void**)&f)=GetProcAddress(basswasapi,#f)
#define LOADBASS_FXFUNCTION(f) *((void**)&f)=GetProcAddress(bass_fx,#f)
#define LOADBASS_VSTFUNCTION(f) *((void**)&f)=GetProcAddress(bass_vst,#f)
#define Between(value, a, b) (value <= b && value >= a)

#define ERRORCODE 0
#define CAUSE 1

static CRITICAL_SECTION midiparsing;

#include <bass.h>
#include <bass_fx.h>
#include <bassmidi.h>
#include <bassenc.h>
#include <bassasio.h>
#include <bass_vst.h>
#include <basswasapi.h>

#define MAX_DRIVERS 256
#define MAX_CLIENTS 256 // Per driver

struct Driver_Client {
	int allocated;
	DWORD_PTR instance;
	DWORD flags;
	DWORD_PTR callback;
};

struct Driver {
	int open;
	int clientCount;
	HDRVR hdrvr;
	struct Driver_Client clients[MAX_CLIENTS];
} drivers[MAX_DRIVERS + 1];

static int driverCount = 0;

static volatile int OpenCount = 0;
static volatile int modm_closed = 1;

static volatile int stop_thread = 0;
static volatile int stop_athread = 0;
static volatile int stop_sthread = 0;
static volatile int stop_rtthread = 0;
static volatile int reset_synth = 0;
static DWORD processPriority;
static HANDLE load_sfevent = NULL;

static int KSStream = 0;
static BASS_INFO info;
static BASS_WASAPI_DEVICEINFO infoDW;
static BASS_WASAPI_INFO infoW;

static BOOL com_initialized = FALSE;
static BOOL sound_out_float = FALSE;
static float sound_out_volume_float = 1.0;
static int sound_out_volume_int = 0x1000;

// Threads
static clock_t start1, start2, start3, start4;
static float Thread1Usage, Thread2Usage, Thread3Usage, Thread4Usage;
static HANDLE hCalcThread = NULL;
static HANDLE hThread2 = NULL;
static HANDLE hThread3 = NULL;
static HANDLE hThread4 = NULL;
static unsigned int thrdaddrC;
static unsigned int thrdaddr2;
static unsigned int thrdaddr3;
static unsigned int thrdaddr4;
static bool hThread2Running = FALSE, hThread3Running = FALSE, hThread4Running = FALSE;

// Variables
#include "basserr.h"
#include "val.h"

void basserrconsole(int color, TCHAR * error, TCHAR * desc) {
	if (debugmode == 1) {
		// Set color
		SetConsoleTextAttribute(hConsole, color);

		// Get time
		char buff[20];
		struct tm *sTm;
		time_t now = time(0);
		sTm = gmtime(&now);
		strftime(buff, sizeof(buff), "%Y-%m-%d %H:%M:%S", sTm);

		// Get error
		char errorC[MAX_PATH];
		char descC[MAX_PATH];
		wcstombs(errorC, error, wcslen(error) + 1);
		wcstombs(descC, desc, wcslen(desc) + 1);
		std::cout << std::endl;
		std::cout << std::endl << buff << " - Keppy's Synthesizer encountered the following error: " << errorC;
		std::cout << std::endl << buff << " - Description: " << descC;
		std::cout << std::endl;
	}
}

void ShowError(int error, int mode, TCHAR* engine, TCHAR* codeline) {
	TCHAR title[MAX_PATH];
	TCHAR main[33354];

	ZeroMemory(title, MAX_PATH);
	ZeroMemory(main, 33354);

	lstrcat(title, L"Keppy's Synthesizer - ");
	lstrcat(title, engine);
	lstrcat(title, L" execution error");

	int e = error + 1;
	std::wstring ernumb = std::to_wstring(error);

	lstrcat(main, engine);
	lstrcat(main, L" encountered the following error: ");
	if (e >= 0 && e <= 48) {
		lstrcat(main, BASSErrorCode[e]);
		basserrconsole(FOREGROUND_RED, BASSErrorCode[e], BASSErrorCode[e]);
	}
	else if (e >= 5000 && e <= 5001) {
		lstrcat(main, BASSWASAPIErrorCode[e - 5000]);
		basserrconsole(FOREGROUND_RED, BASSWASAPIErrorCode[e - 5000], BASSErrorDesc[e - 5000]);
	}
	lstrcat(main, L" (E");
	lstrcat(main, ernumb.c_str());
	lstrcat(main, L")");

	if (mode == 0) {
		lstrcat(main, L"\n\nCode line error: ");
		lstrcat(main, codeline);
	}

	lstrcat(main, L"\n\nExplanation: ");
	if (e >= 0 && e <= 48) {
		lstrcat(main, BASSErrorDesc[e]);
	}
	else if (e >= 5000 && e <= 5001) {
		lstrcat(main, BASSWASAPIErrorDesc[e - 5000]);
	}

	if (mode == 1) lstrcat(main, L"\n\nWhat might have caused this error:\n\n");
	else {
		lstrcat(main, L"\n\nPossible fixes:\n");
		if (e >= 0 && e <= 48)
			lstrcat(main, BASSErrorFix[e]);
		else if (e >= 5000 && e <= 5001)
			lstrcat(main, BASSWASAPIErrorFix[e - 5000]);
	}

	lstrcat(main, L"\n\nIf you're unsure about what this means, please take a screenshot, and give it to KaleidonKep99.");
	if (isoverrideenabled == 1) lstrcat(main, L"\n\n(This might be caused by using old BASS libraries through the DLL override function.)");

	if (engine == L"ASIO") {
		lstrcat(main, L"\n\nChange the device through the configurator, then try again.\nTo change it, please open the configurator, and go to \"More settings > Advanced audio settings > Change default audio output\"");
	}

	MessageBox(NULL, main, title, MB_OK | MB_ICONERROR);

	if (error == -1 ||
		error >= 2 && error <= 10 ||
		error == 19 ||
		error >= 24 && error <= 26 ||
		error == 44)
	{
		exit(error);
	}
}

void CheckUp(int mode, TCHAR * codeline) {
	int error = BASS_ErrorGetCode();
	if (error != 0) {
		ShowError(error, mode, L"BASS", codeline);
	}
}

void CheckUpASIO(int mode, TCHAR * codeline) {
	int error = BASS_ASIO_ErrorGetCode();
	if (error != 0) {
		ShowError(error, mode, L"BASSASIO", codeline);
	}
}

bool GetVersionInfo(
	LPCTSTR filename,
	int &major,
	int &minor,
	int &build,
	int &revision)
{
	DWORD   verBufferSize;
	char    verBuffer[2048];

	//  Get the size of the version info block in the file
	verBufferSize = GetFileVersionInfoSize(filename, NULL);
	if (verBufferSize > 0 && verBufferSize <= sizeof(verBuffer))
	{
		//  get the version block from the file
		if (TRUE == GetFileVersionInfo(filename, NULL, verBufferSize, verBuffer))
		{
			UINT length;
			VS_FIXEDFILEINFO *verInfo = NULL;

			//  Query the version information for neutral language
			if (TRUE == VerQueryValue(
				verBuffer,
				_T("\\"),
				reinterpret_cast<LPVOID*>(&verInfo),
				&length))
			{
				//  Pull the version values.
				major = HIWORD(verInfo->dwProductVersionMS);
				minor = LOWORD(verInfo->dwProductVersionMS);
				build = HIWORD(verInfo->dwProductVersionLS);
				revision = LOWORD(verInfo->dwProductVersionLS);
				return true;
			}
		}
	}

	return false;
}

LPCWSTR ReturnAppName(void) {
	// Get app name
	TCHAR buffer[MAX_PATH];
	TCHAR * out;
	GetModuleFileName(NULL, buffer, MAX_PATH);
	out = PathFindFileName(buffer);

	TCHAR final[MAX_PATH];
	_stprintf(final, _T("%s  - Debug Output.txt"), out);

	return final;
}

void CreateConsole() {
	if (alreadyshown != 1) {
		MessageBox(NULL, L"You're running the driver in debug mode.", L"Keppy's Synthesizer - Notice", MB_ICONWARNING | MB_OK);

		// Create file and start console output
		LPCWSTR appname = ReturnAppName();
		TCHAR installpath[MAX_PATH];
		TCHAR pathfortext[MAX_PATH];
		SHGetFolderPath(NULL, CSIDL_PROFILE, NULL, 0, pathfortext);
		PathAppend(pathfortext, _T("\\Keppy's Synthesizer\\debug\\"));
		CreateDirectory(pathfortext, NULL);
		PathAppend(pathfortext, appname);
		GetModuleFileName(hinst, installpath, MAX_PATH);
		PathRemoveFileSpec(installpath);
		lstrcat(installpath, L"\\keppysynth.dll");
		int major, minor, build, revision;
		GetVersionInfo(installpath, major, minor, build, revision);
		_wfreopen(pathfortext, L"w", stdout);
		hConsole = GetStdHandle(STD_OUTPUT_HANDLE);
		SetConsoleTitle(L"Keppy's Synthesizer Debug Console");
		std::cout << "Be the change that you wish to see in the world.";
		std::cout << std::endl;
		std::cout << "Keppy's Synthesizer Version " << major << "." << minor << "." << build << "." << revision;
		std::cout << std::endl << "Copyright 2014-2017 - KaleidonKep99";
		std::cout << std::endl;
		alreadyshown = 1;
	}
}

inline bool DebugFileExists(const std::string& name) {
	if (FILE *file = fopen(name.c_str(), "r")) {
		fclose(file);
		return true;
	}
	else {
		return false;
	}
}

void StatusType(int status, char* &statustoprint) {
	std::string statusstring = "";

	if (Between(status, 0x80, 0xEF)) {
		if (Between(status, 0x80, 0x8F)) {
			statusstring += "Note OFF event on channel ";
			statusstring += std::to_string((status - 0x80));
		}
		else if (Between(status, 0x90, 0x9F)) {
			statusstring += "Note ON event on channel ";
			statusstring += std::to_string((status - 0x90));
		}
		else if (Between(status, 0xA0, 0xAF)) {
			statusstring += "Polyphonic aftertouch event on channel ";
			statusstring += std::to_string((status - 0xA0));
		}
		else if (Between(status, 0xB0, 0xBF)) {
			statusstring += "Channel reset on channel ";
			statusstring += std::to_string((status - 0xB0));
		}
		else if (Between(status, 0xC0, 0xCF)) {
			statusstring += "Program change on channel ";
			statusstring += std::to_string((status - 0xC0));
		}
		else if (Between(status, 0xD0, 0xDF)) {
			statusstring += "Channel aftertouch event on channel ";
			statusstring += std::to_string((status - 0xD0));
		}
		else if (Between(status, 0xE0, 0xEF)) {
			statusstring += "Pitch change on channel ";
			statusstring += std::to_string((status - 0xE0));
		}

		statustoprint = strdup(statusstring.c_str());
	}
	else if (status == 0xF0) statustoprint = "System Exclusive\0";
	else if (status == 0xF1) statustoprint = "System Common - undefined\0";
	else if (status == 0xF2) statustoprint = "Sys Com Song Position Pntr\0";
	else if (status == 0xF3) statustoprint = "Sys Com Song Select\0";
	else if (status == 0xF4) statustoprint = "System Common - undefined\0";
	else if (status == 0xF5) statustoprint = "System Common - undefined\0";
	else if (status == 0xF6) statustoprint = "Sys Com Tune Request\0";
	else if (status == 0xF7) statustoprint = "Sys Com-end of SysEx (EOX)\0";
	else if (status == 0xF8) statustoprint = "Sys Real Time Timing Clock\0";
	else if (status == 0xF9) statustoprint = "Sys Real Time - undefined\0";
	else if (status == 0xFA) statustoprint = "Sys Real Time Start\0";
	else if (status == 0xFB) statustoprint = "Sys Real Time Continue\0";
	else if (status == 0xFC) statustoprint = "Sys Real Time Stop\0";
	else if (status == 0xFD) statustoprint = "Sys Real Time - undefined\0";
	else if (status == 0xFE) statustoprint = "Sys Real Time Active Sensing\0";
	else if (status == 0xFF) statustoprint = "Sys Real Time Sys Reset\0";
	else statustoprint = "Unknown event\0";
}

void PrintToConsole(int color, long stage, const char* text) {
	if (debugmode == 1 && printimportant == 1) {
		// Set color
		SetConsoleTextAttribute(hConsole, color);

		// Get time
		char buff[20];
		struct tm *sTm;
		time_t now = time(0);
		sTm = gmtime(&now);
		strftime(buff, sizeof(buff), "%Y-%m-%d %H:%M:%S", sTm);

		// Print to log
		std::cout << std::endl << buff << " - (" << stage << ") - " << text;
	}
}

void PrintEventToConsole(int color, int stage, bool issysex, const char* text, int channel, int status, int note, int velocity) {
	if (debugmode == 1 && printmidievent == 1) {
		// Set color
		SetConsoleTextAttribute(hConsole, color);

		// Get time
		char buff[20];
		struct tm *sTm;
		time_t now = time(0);
		sTm = gmtime(&now);
		strftime(buff, sizeof(buff), "%Y-%m-%d %H:%M:%S", sTm);

		// Get status
		char* statustoprint = { 0 };
		StatusType(status, statustoprint);

		// Print to log
		if (issysex) {
			std::cout << std::endl << buff << " - (" << stage << ") - " << text << " ~ Type = SysEx event";
		}
		else {
			std::cout << std::endl << buff << " - (" << stage << ") - " << text << " ~ Channel = " << statustoprint << " | Type = " << statustoprint << " | Note = " << note << " | Velocity = " << velocity;
		}
	}
}

// Keppy's Synthesizer vital parts
#include "sfsystem.h"
#include "settings.h"
#include "bufsystem.h"
#include "bansystem.h"
#include "drvinit.h"

static void DoStopClient();

BOOL WINAPI DllMain(HINSTANCE hinstDLL, DWORD fdwReason, LPVOID lpvReserved){

	if (fdwReason == DLL_PROCESS_ATTACH){
		hinst = hinstDLL;
		DisableThreadLibraryCalls(hinstDLL);
	}
	else if (fdwReason == DLL_PROCESS_DETACH){
		DoStopClient();
	}
	return TRUE;
}

LRESULT DoDriverLoad() {
	memset(drivers, 0, sizeof(drivers));
	driverCount = 0;
	return DRV_OK;
}

LRESULT DoDriverOpen(HDRVR hdrvr, LPCWSTR driverName, LONG lParam) {
	int driverNum;
	if (driverCount == MAX_DRIVERS) {
		return 0;
	}
	else {
		for (driverNum = 1; driverNum < MAX_DRIVERS; driverNum++) {
			if (!drivers[driverNum].open) {
				break;
			}
		}
		if (driverNum == MAX_DRIVERS) {
			return 0;
		}
	}
	drivers[driverNum].open = 1;
	drivers[driverNum].clientCount = 0;
	drivers[driverNum].hdrvr = hdrvr;
	driverCount++;
	return driverNum;
}

LRESULT DoDriverClose(DWORD_PTR dwDriverId, HDRVR hdrvr, LONG lParam1, LONG lParam2) {
	int i;
	for (i = 0; i < MAX_DRIVERS; i++) {
		if (drivers[i].open && drivers[i].hdrvr == hdrvr) {
			drivers[i].open = 0;
			--driverCount;
			return DRV_OK;
		}
	}
	return DRV_CANCEL;
}

LRESULT DoDriverConfiguration() {
	TCHAR configuratorapp[MAX_PATH];
	if (SUCCEEDED(SHGetFolderPath(NULL, CSIDL_SYSTEMX86, NULL, 0, configuratorapp)))
	{
		PathAppend(configuratorapp, _T("\\keppysynth\\KeppySynthConfigurator.exe"));
		ShellExecute(NULL, L"open", configuratorapp, L"/AST", NULL, SW_SHOWNORMAL);
		return DRVCNF_OK;
	}
	return DRVCNF_CANCEL;
}

STDAPI_(LRESULT) DriverProc(DWORD_PTR dwDriverId, HDRVR hdrvr, UINT uMsg, LPARAM lParam1, LPARAM lParam2)
{
	switch (uMsg) {
	case DRV_QUERYCONFIGURE:
		return DRV_CANCEL;
	case DRV_CONFIGURE:
		return DoDriverConfiguration();
	case DRV_LOAD:
		return DoDriverLoad();
	case DRV_FREE:
		return DRV_OK;
	case DRV_OPEN:
		return DoDriverOpen(hdrvr, reinterpret_cast<LPCWSTR>(lParam1), static_cast<LONG>(lParam2));
	case DRV_CLOSE:
		return DoDriverClose(dwDriverId, hdrvr, static_cast<LONG>(lParam1), static_cast<LONG>(lParam2));
	default:
		return DRV_OK;
	}
}

HRESULT modGetCaps(UINT uDeviceID, MIDIOUTCAPS* capsPtr, DWORD capsSize) {
	try {
		HKEY hKey;
		long lResult;
		int defaultmode;
		WORD VID = 0x0000;
		WORD PID = 0x0000;
		CHAR SynthName[MAXPNAMELEN];
		WCHAR SynthNameW[MAXPNAMELEN];
		DWORD dwType = REG_DWORD;
		DWORD dwSize = sizeof(DWORD);
		DWORD dwSizeA = sizeof(SynthName);
		DWORD dwSizeW = sizeof(SynthNameW);
		lResult = RegOpenKeyEx(HKEY_CURRENT_USER, L"Software\\Keppy's Synthesizer\\Settings", 0, KEY_ALL_ACCESS, &hKey);
		RegQueryValueEx(hKey, L"shortname", NULL, &dwType, (LPBYTE)&shortname, &dwSize);
		RegQueryValueEx(hKey, L"defaultmidiout", NULL, &dwType, (LPBYTE)&defaultmidiout, &dwSize);
		RegQueryValueEx(hKey, L"synthtype", NULL, &dwType, (LPBYTE)&selectedtype, &dwSize);
		RegQueryValueEx(hKey, L"debugmode", NULL, &dwType, (LPBYTE)&debugmode, &dwSize);
		dwType = REG_DWORD;
		RegQueryValueEx(hKey, L"vid", NULL, &dwType, (LPBYTE)&VID, &dwSize);
		RegQueryValueEx(hKey, L"pid", NULL, &dwType, (LPBYTE)&PID, &dwSize);
		dwType = REG_SZ;
		RegQueryValueExA(hKey, "synthname", NULL, &dwType, (LPBYTE)&SynthName, &dwSizeA);
		RegQueryValueExW(hKey, L"synthname", NULL, &dwType, (LPBYTE)&SynthNameW, &dwSizeW);
		RegCloseKey(hKey);

		if (defaultmidiout == 1) defaultmode = MOD_SWSYNTH;
		else if (selectedtype < 0 || selectedtype > 6) selectedtype = 4;
		else defaultmode = MOD_SWSYNTH;

		defaultmode = SynthNamesTypes[selectedtype];

		if (debugmode == 1 && (!BannedSystemProcess() | !BlackListSystem())) CreateConsole();

		if (strlen(SynthName) < 1) {
			ZeroMemory(SynthName, MAXPNAMELEN);
			strncpy(SynthName, "Keppy's Synthesizer\0", MAXPNAMELEN);
		}

		if (wcslen(SynthNameW) < 1) {
			ZeroMemory(SynthNameW, MAXPNAMELEN);
			wcsncpy(SynthNameW, L"Keppy's Synthesizer\0", MAXPNAMELEN);
		}

		PrintToConsole(FOREGROUND_BLUE, 1, "Sharing MIDI caps with application...");

		MIDIOUTCAPSA * myCapsA;
		MIDIOUTCAPSW * myCapsW;
		MIDIOUTCAPS2A * myCaps2A;

		MIDIOUTCAPS2W * myCaps2W;
		WORD maximumvoices = 0xffff;
		WORD maximumnotes = 0xffff;
		DWORD CapsSupport = MIDICAPS_VOLUME | MIDICAPS_LRVOLUME | MIDICAPS_CACHE;

		const GUID CLSIDKEPSYNTH = { 0x318fa900, 0xf7de, 0x4ec6,{ 0x84, 0x8f, 0x0f, 0x28, 0xea, 0x37, 0x88, 0x9f } };

		switch (capsSize) {
		case (sizeof(MIDIOUTCAPSA)):
			myCapsA = (MIDIOUTCAPSA *)capsPtr;
			myCapsA->wMid = VID;
			myCapsA->wPid = PID;
			memcpy(myCapsA->szPname, SynthName, sizeof(SynthName));
			myCapsA->wVoices = maximumvoices;
			myCapsA->wNotes = maximumnotes;
			myCapsA->wTechnology = defaultmode;
			myCapsA->wChannelMask = 0xffff;
			myCapsA->dwSupport = CapsSupport;
			PrintToConsole(FOREGROUND_BLUE, 1, "Done sharing caps. (MIDIOUTCAPSA)");
			return MMSYSERR_NOERROR;

		case (sizeof(MIDIOUTCAPSW)):
			myCapsW = (MIDIOUTCAPSW *)capsPtr;
			myCapsW->wMid = VID;
			myCapsW->wPid = PID;
			memcpy(myCapsW->szPname, SynthNameW, sizeof(SynthNameW));
			myCapsW->wVoices = maximumvoices;
			myCapsW->wNotes = maximumnotes;
			myCapsW->wTechnology = defaultmode;
			myCapsW->wChannelMask = 0xffff;
			myCapsW->dwSupport = CapsSupport;
			PrintToConsole(FOREGROUND_BLUE, 1, "Done sharing caps. (MIDIOUTCAPSW)");
			return MMSYSERR_NOERROR;

		case (sizeof(MIDIOUTCAPS2A)):
			myCaps2A = (MIDIOUTCAPS2A *)capsPtr;
			myCaps2A->wMid = VID;
			myCaps2A->wPid = PID;
			memcpy(myCaps2A->szPname, SynthName, sizeof(SynthName));
			myCaps2A->ManufacturerGuid = CLSIDKEPSYNTH;
			myCaps2A->ProductGuid = CLSIDKEPSYNTH;
			myCaps2A->NameGuid = CLSIDKEPSYNTH;
			myCaps2A->wVoices = maximumvoices;
			myCaps2A->wNotes = maximumnotes;
			myCaps2A->wTechnology = defaultmode;
			myCaps2A->wChannelMask = 0xffff;
			myCaps2A->dwSupport = CapsSupport;
			PrintToConsole(FOREGROUND_BLUE, 1, "Done sharing caps. (MIDIOUTCAPS2A)");
			return MMSYSERR_NOERROR;

		case (sizeof(MIDIOUTCAPS2W)):
			myCaps2W = (MIDIOUTCAPS2W *)capsPtr;
			myCaps2W->wMid = VID;
			myCaps2W->wPid = PID;
			memcpy(myCaps2W->szPname, SynthNameW, sizeof(SynthNameW));
			myCaps2W->ManufacturerGuid = CLSIDKEPSYNTH;
			myCaps2W->ProductGuid = CLSIDKEPSYNTH;
			myCaps2W->NameGuid = CLSIDKEPSYNTH;
			myCaps2W->wVoices = maximumvoices;
			myCaps2W->wNotes = maximumnotes;
			myCaps2W->wTechnology = defaultmode;
			myCaps2W->wChannelMask = 0xffff;
			myCaps2W->dwSupport = CapsSupport;
			PrintToConsole(FOREGROUND_BLUE, 1, "Done sharing caps. (MIDIOUTCAPS2W)");
			return MMSYSERR_NOERROR;

		default:
			try {
				PrintToConsole(FOREGROUND_BLUE, 1, "App is not asking for specific caps. Trying to give Unicode caps...");
				myCapsW = (MIDIOUTCAPSW *)capsPtr;
				myCapsW->wMid = VID;
				myCapsW->wPid = PID;
				memcpy(myCapsW->szPname, SynthNameW, sizeof(SynthNameW));
				myCapsW->wVoices = 0;
				myCapsW->wNotes = 0;
				myCapsW->wTechnology = defaultmode;
				myCapsW->wChannelMask = 0xffff;
				myCapsW->dwSupport = CapsSupport;
				PrintToConsole(FOREGROUND_BLUE, 1, "Done sharing caps. (MIDIOUTCAPSW)");
				return MMSYSERR_NOERROR;
			}
			catch (...) {
				PrintToConsole(FOREGROUND_BLUE, 1, "Error while sharing MIDI caps.");
				return MMSYSERR_NOTSUPPORTED;
			}
			break;
		}
	}
	catch (...) {
		PrintToConsole(FOREGROUND_BLUE, 1, "Error while sharing MIDI caps.");
		return MMSYSERR_NOTSUPPORTED;
	}
}

void keepstreamsalive(int& opend) {
	BASS_ChannelIsActive(KSStream);
	if (BASS_ErrorGetCode() == 5 || livechange == 1) {
		PrintToConsole(FOREGROUND_RED, 1, "Restarting audio stream...");
		CloseThreads();
		load_settings(TRUE);
		if (!com_initialized) { if (!FAILED(CoInitialize(NULL))) com_initialized = TRUE; }
		SetConsoleTextAttribute(hConsole, FOREGROUND_RED);
		if (InitializeBASS(FALSE)) {
			InitializeBASSVST();
			SetUpStream();
			LoadSoundFontsToStream();
			opend = CreateThreads(TRUE);
		}
	}
}

BOOL CALLBACK EnumWindowsProc(HWND hWnd, LPARAM lParam) {
	DWORD process;
	GetWindowThreadProcessId(hWnd, &process);
	if (GetCurrentProcessId() == process) {
		MessageBox(hWnd, L"I loaded your dll!", L"it's me", MB_OK);
		return TRUE;
	}
	return FALSE;
}

DWORD WINAPI threadfunc(LPVOID lpV){
	try {
		if (BannedSystemProcess() == TRUE) {
			_endthread();
			return 0;
		}
		else {
			int opend = 0;
			while (opend == 0) {
				load_settings(FALSE);
				allocate_memory();
				load_bassfuncs();
				if (!com_initialized) {
					if (FAILED(CoInitialize(NULL))) continue;
					com_initialized = TRUE;
				}
				SetConsoleTextAttribute(hConsole, FOREGROUND_RED);
				if (InitializeBASS(FALSE)) {
					InitializeBASSVST();
					SetUpStream();
					LoadSoundFontsToStream();
					opend = CreateThreads(TRUE);
				}
			}
			PrintToConsole(FOREGROUND_RED, 1, "Checking for settings changes or hotkeys...");
			while (stop_rtthread == 0){
				start1 = clock();
				keepstreamsalive(opend);
				debug_info();
				LoadCustomInstruments();
				keybindings();
				CheckVolume();
				Sleep(10);
			}
			stop_rtthread = 0;
			FreeUpLibraries();
			PrintToConsole(FOREGROUND_RED, 1, "Closing main thread...");
			ExitThread(0);
			return 0;
		}
	}
	catch (...) {
		crashmessage(L"DrvMainThread");
		ExitThread(0);
		throw;
		return 0;
	}
}

void DoCallback(int driverNum, int clientNum, DWORD msg, DWORD_PTR param1, DWORD_PTR param2) {
	struct Driver_Client *client = &drivers[driverNum].clients[clientNum];
	DriverCallback(client->callback, client->flags, drivers[driverNum].hdrvr, msg, client->instance, param1, param2);
}

void DoStartClient() {
	if (modm_closed == 1) {
		HKEY hKey;
		long lResult;
		DWORD dwType = REG_DWORD;
		DWORD dwSize = sizeof(DWORD);
		int One = 0;
		lResult = RegOpenKeyEx(HKEY_CURRENT_USER, L"Software\\Keppy's Synthesizer", 0, KEY_ALL_ACCESS, &hKey);
		RegQueryValueEx(hKey, L"driverprio", NULL, &dwType, (LPBYTE)&driverprio, &dwSize);
		RegCloseKey(hKey);
		lResult = RegOpenKeyEx(HKEY_CURRENT_USER, L"Software\\Keppy's Synthesizer\\Settings", 0, KEY_ALL_ACCESS, &hKey);
		RegQueryValueEx(hKey, L"improveperf", NULL, &dwType, (LPBYTE)&improveperf, &dwSize);
		RegCloseKey(hKey);

		if (improveperf == 0) InitializeCriticalSection(&midiparsing);
		DWORD result;
		processPriority = GetPriorityClass(GetCurrentProcess());
		SetPriorityClass(GetCurrentProcess(), NORMAL_PRIORITY_CLASS);
		load_sfevent = CreateEvent(
			NULL,               // default security attributes
			TRUE,               // manual-reset event
			FALSE,              // initial state is nonsignaled
			TEXT("SoundFontEvent")  // object name
			);
		hCalcThread = CreateThread(NULL, 0, (LPTHREAD_START_ROUTINE)threadfunc, NULL, 0, (LPDWORD)thrdaddrC);
		SetThreadPriority(hCalcThread, prioval[driverprio]);
		result = WaitForSingleObject(load_sfevent, INFINITE);
		if (result == WAIT_OBJECT_0)
		{
			CloseHandle(load_sfevent);
		}
		modm_closed = 0;
	}
}

void DoStopClient() {
	HKEY hKey;
	long lResult;
	DWORD dwType = REG_DWORD;
	DWORD dwSize = sizeof(DWORD);
	int One = 0;
	lResult = RegOpenKeyEx(HKEY_CURRENT_USER, L"Software\\Keppy's Synthesizer", 0, KEY_ALL_ACCESS, &hKey);
	RegSetValueEx(hKey, L"currentcpuusage0", 0, dwType, (LPBYTE)&One, 1);
	RegSetValueEx(hKey, L"currentcpuusageE0", 0, dwType, (LPBYTE)&One, 1);
	RegSetValueEx(hKey, L"ramusage", 0, dwType, (LPBYTE)&One, sizeof(One));
	RegSetValueEx(hKey, L"handlecount", 0, dwType, (LPBYTE)&One, sizeof(One));
	RegSetValueEx(hKey, L"rightvol", 0, dwType, (LPBYTE)&One, 1);
	RegSetValueEx(hKey, L"leftvol", 0, dwType, (LPBYTE)&One, 1);
	for (int i = 0; i <= 15; ++i) {
		RegSetValueEx(hKey, cvnames[i], 0, dwType, (LPBYTE)&One, sizeof(One));
	}

	RegSetValueEx(hKey, L"buffull", 0, dwType, (LPBYTE)&One, 1);
	RegSetValueEx(hKey, L"int", 0, dwType, (LPBYTE)&One, 1);
	RegSetValueEx(hKey, L"td1", 0, dwType, (LPBYTE)&One, 1);
	RegSetValueEx(hKey, L"td2", 0, dwType, (LPBYTE)&One, 1);
	RegSetValueEx(hKey, L"td3", 0, dwType, (LPBYTE)&One, 1);
	RegSetValueEx(hKey, L"td4", 0, dwType, (LPBYTE)&One, 1);
	RegSetValueEx(hKey, L"asioinlatency", 0, dwType, (LPBYTE)&One, sizeof(One));
	RegSetValueEx(hKey, L"asiooutlatency", 0, dwType, (LPBYTE)&One, sizeof(One));
	RegCloseKey(hKey);
	lResult = RegOpenKeyEx(HKEY_CURRENT_USER, L"Software\\Keppy's Synthesizer\\Watchdog", 0, KEY_ALL_ACCESS, &hKey);
	RegSetValueEx(hKey, L"currentapp", 0, dwType, (LPBYTE)&One, 1);
	RegSetValueEx(hKey, L"bit", 0, dwType, (LPBYTE)&One, 1);
	RegCloseKey(hKey);
	if (modm_closed == 0){
		stop_thread = 1;
		stop_rtthread = 1;
		WaitForSingleObject(hCalcThread, INFINITE);
		CloseHandle(hCalcThread);
		modm_closed = 1;
		SetPriorityClass(GetCurrentProcess(), processPriority);
	}
	if (improveperf == 0) DeleteCriticalSection(&midiparsing);
}

void DoResetClient(UINT uDeviceID) {
	reset_synth = 1;
	ResetSynth(0);
}

LONG DoOpenClient(struct Driver *driver, UINT uDeviceID, LONG* dwUser, MIDIOPENDESC * desc, DWORD flags) {
	int clientNum;
	if (driver->clientCount == 0) {
		DoStartClient();
		DoResetClient(uDeviceID);
		clientNum = 0;
	}
	else if (driver->clientCount == MAX_CLIENTS) {
		return MMSYSERR_ALLOCATED;
	}
	else {
		int i;
		for (i = 0; i < MAX_CLIENTS; i++) {
			if (!driver->clients[i].allocated) {
				break;
			}
		}
		if (i == MAX_CLIENTS) {
			return MMSYSERR_ALLOCATED;
		}
		clientNum = i;
	}
	driver->clients[clientNum].allocated = 1;
	driver->clients[clientNum].flags = HIWORD(flags);
	driver->clients[clientNum].callback = desc->dwCallback;
	driver->clients[clientNum].instance = desc->dwInstance;
	*dwUser = clientNum;
	driver->clientCount++;
	SetPriorityClass(GetCurrentProcess(), processPriority);
	DoCallback(uDeviceID, clientNum, MOM_OPEN, 0, 0);
	return MMSYSERR_NOERROR;
}

LONG DoCloseClient(struct Driver *driver, UINT uDeviceID, LONG dwUser) {
	if (!driver->clients[dwUser].allocated) {
		return MMSYSERR_INVALPARAM;
	}

	driver->clients[dwUser].allocated = 0;
	driver->clientCount--;
	if (driver->clientCount <= 0) {
		DoResetClient(uDeviceID);
		driver->clientCount = 0;
	}
	DoCallback(uDeviceID, dwUser, MOM_CLOSE, 0, 0);
	return MMSYSERR_NOERROR;
}

STDAPI_(DWORD) modMessage(UINT uDeviceID, UINT uMsg, DWORD_PTR dwUser, DWORD_PTR dwParam1, DWORD_PTR dwParam2){
	LONG evbpoint;
	MIDIHDR *IIMidiHdr;
	struct Driver *driver = &drivers[uDeviceID];
	int exlen = 0;
	unsigned char *sysexbuffer = NULL;
	DWORD result = 0;
	switch (uMsg) {
	case MODM_OPEN:
		return DoOpenClient(driver, uDeviceID, reinterpret_cast<LONG*>(dwUser), reinterpret_cast<MIDIOPENDESC*>(dwParam1), static_cast<DWORD>(dwParam2));
	case MODM_PREPARE:
		return MMSYSERR_NOTSUPPORTED;
	case MODM_UNPREPARE:
		return MMSYSERR_NOTSUPPORTED;
	case MODM_GETNUMDEVS:
		return VMSBlackList();
	case MODM_GETDEVCAPS:
		return modGetCaps(uDeviceID, reinterpret_cast<MIDIOUTCAPS*>(dwParam1), static_cast<DWORD>(dwParam2));
	case MODM_LONGDATA:
		try {
			ParseData(evbpoint, MODM_LONGDATA, uDeviceID, dwParam1, dwParam2, exlen, sysexbuffer);
			DoCallback(uDeviceID, static_cast<LONG>(dwUser), MOM_DONE, dwParam1, 0);
			break;
		}
		catch (...) {
			crashmessage(L"LongMODMDataParse");
			throw;
		}
	case MODM_DATA:
		try {
			ParseData(evbpoint, MODM_DATA, uDeviceID, dwParam1, dwParam2, exlen, sysexbuffer);
			break;
		}
		catch (...) {
			crashmessage(L"MODMDataParse");
			throw;
		}
	case MODM_GETVOLUME: {
		*(LONG*)dwParam1 = static_cast<LONG>(sound_out_volume_float * 0xFFFF);
		return MMSYSERR_NOERROR;
	}
	case MODM_SETVOLUME: {
		sound_out_volume_float = LOWORD(dwParam1) / (float)0xFFFF;
		return MMSYSERR_NOERROR;
	}
	case MODM_PAUSE: {
		reset_synth = 1;
		ResetSynth(0);
		return MMSYSERR_NOERROR;
	}
	case MODM_STOP: {
		reset_synth = 1;
		ResetSynth(0);
		return MMSYSERR_NOERROR;
	}
	case MODM_RESET:
		DoResetClient(uDeviceID);
		return MMSYSERR_NOERROR;
	case MODM_CLOSE:
		if (stop_rtthread != 0 || stop_thread != 0) return MIDIERR_STILLPLAYING;
		else return DoCloseClient(driver, uDeviceID, static_cast<LONG>(dwUser));	
		break;
	default:
		return MMSYSERR_NOERROR;
		break;
	}
}

#if defined(_WINMMMODE)
	// WinMM functions
	#include "winmmfunc.h"
#endif