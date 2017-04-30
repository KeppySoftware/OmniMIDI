/*
Keppy's Synthesizer, a fork of BASSMIDI Driver

Thank you Kode54 for allowing me to fork your awesome driver.
*/

#pragma comment(lib,"Version.lib")

#include "sha256.h"
#include "stdafx.h"
#include <Dbghelp.h>
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
#include "Resource.h"

#define BASSDEF(f) (WINAPI *f)
#define BASSMIDIDEF(f) (WINAPI *f)	
#define BASSENCDEF(f) (WINAPI *f)	
#define BASS_VSTDEF(f) (WINAPI *f)
#define BASSXADEF(f) (WINAPI *f)
#define LOADBASSFUNCTION(f) *((void**)&f)=GetProcAddress(bass,#f)
#define LOADBASSMIDIFUNCTION(f) *((void**)&f)=GetProcAddress(bassmidi,#f)
#define LOADBASSENCFUNCTION(f) *((void**)&f)=GetProcAddress(bassenc,#f)
#define LOADBASS_VSTFUNCTION(f) *((void**)&f)=GetProcAddress(bass_vst,#f)
#define LOADBASSXAFUNCTION(f) *((void**)&f)=GetProcAddress(bassxa,#f)
#define Between(value, a, b) (value <= b && value >= a)

#define ERRORCODE 0
#define CAUSE 1

#include <bass.h>
#include <bassmidi.h>
#include <bassenc.h>
#include <bass_vst.h>
#include <bassxa.h>

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

static CRITICAL_SECTION mim_section;
static volatile int stop_thread = 0;
static volatile int stop_rtthread = 0;
static volatile int reset_synth = 0;
static DWORD processPriority;
static HANDLE load_sfevent = NULL;

static HSTREAM KSStream = 0;
static BASS_INFO info;

static BOOL com_initialized = FALSE;
static BOOL sound_out_float = FALSE;
static float sound_out_volume_float = 1.0;
static int sound_out_volume_int = 0x1000;

// Threads
static HANDLE hCalcThread = NULL;
static HANDLE hThread = NULL;
static HANDLE hThread2 = NULL;
static HANDLE hThread3 = NULL;
static HANDLE hThread4 = NULL;
static unsigned int thrdaddr1;
static unsigned int thrdaddr2;
static unsigned int thrdaddr3;
static unsigned int thrdaddr4;

// Variables
#include "basserr.h"
#include "val.h"

void basserr(int error, int mode, TCHAR * codeline) {
	TCHAR buffer[MAX_PATH];
	wsprintfW(buffer, L"%d", error);
	error++;
	TCHAR part1[MAX_PATH] = L"BASS encountered the error number ";
	TCHAR part2[MAX_PATH] = L": \"";
	TCHAR part3[MAX_PATH] = L"\"\n\nExplanation: ";
	TCHAR part4[MAX_PATH] = L"\n\nIf you're unsure about what this means, please take a screenshot, and give it to KaleidonKep99.";
	TCHAR partE[MAX_PATH] = L"\n\n(This might be caused by using old BASS libraries through the DLL override function.)";
	TCHAR partA[MAX_PATH];

	if (mode == 1)
		wcscpy(partA, L"\n\nWhat might have caused this error:\n\n");
	else
		wcscpy(partA, L"\n\nCode line error: ");

	lstrcat(part1, buffer);
	lstrcat(part2, errname[error]);
	lstrcat(part3, errdesc[error - 1]);
	lstrcat(part1, part2);
	lstrcat(part1, part3);
	lstrcat(part1, part4);
	if (isoverrideenabled == 1) {
		lstrcat(part1, partE);
	}
	lstrcat(part1, partA);
	lstrcat(part1, codeline);
	MessageBox(NULL, part1, L"Keppy's Synthesizer - BASS execution error", MB_OK | MB_ICONERROR);

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
		basserr(error, mode, codeline);
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

LPCWSTR ReturnAppName() {
	// Get app name
	typedef std::basic_string<TCHAR> tstring;
	TCHAR buffer[MAX_PATH] = { 0 };
	TCHAR * out;
	DWORD bufSize = sizeof(buffer) / sizeof(*buffer);
	if (GetModuleFileName(NULL, buffer, bufSize) == bufSize) {}
	out = PathFindFileName(buffer);
	std::wstring stemp = tstring(out) + L" - Debug Output.txt";
	return stemp.c_str();
}

void CreateConsole() {
	// Create file and start console output
	LPCWSTR appname = ReturnAppName();
	TCHAR installpath[MAX_PATH] = { 0 };
	TCHAR pathfortext[MAX_PATH];
	char pathfortextchar[500];
	SHGetFolderPath(NULL, CSIDL_PROFILE, NULL, 0, pathfortext);
	PathAppend(pathfortext, _T("\\Keppy's Synthesizer\\debug\\"));
	CreateDirectory(pathfortext, NULL);
	PathAppend(pathfortext, appname);
	GetModuleFileName(hinst, installpath, MAX_PATH);
	PathRemoveFileSpec(installpath);
	lstrcat(installpath, L"\\keppysynth.dll");
	int major2;
	int minor2;
	int build;
	int revision;
	int number = 1;
	GetVersionInfo(installpath, major2, minor2, build, revision);
	if (alreadyshown != 1) {
		wcstombs(pathfortextchar, pathfortext, 500);
		freopen(pathfortextchar, "w", stdout);
		hConsole = GetStdHandle(STD_OUTPUT_HANDLE);
		SetConsoleTitle(L"Keppy's Synthesizer Debug Console");
		std::cout << "Keppy's Synthesizer Version " << major2 << "." << minor2 << "." << build << "." << revision;
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

char* StatusType(int status) {
	if (status == 128) { return "Note Off"; }
	else if (status == 144) { return "Note On"; }
	else if (status == 176) { return "Polyphonic Key Pressure"; }
	else if (Between(status, 177, 191)) { return "Channel Reset"; }
	else { return "Unknown"; }
}

void PrintToConsole(int color, int stage, const char* text) {
	if (debugmode == 1) {
		SetConsoleTextAttribute(hConsole, color);
		std::cout << std::endl << "(" << stage << ") - " << text;
	}
}

void PrintEventToConsole(int color, int stage, const char* text, int status, int note, int velocity) {
	if (debugmode == 1) {
		SetConsoleTextAttribute(hConsole, color);
		std::cout << std::endl << "(" << stage << ") - " << text << " ~ Type = " << StatusType(status) << " | Note = " << note << " | Velocity = " << velocity;
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
		;
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
		PathAppend(configuratorapp, _T("\\keppydrv\\KeppyDriverConfigurator.exe"));
		ShellExecute(NULL, L"open", configuratorapp, L"/AT", NULL, SW_SHOWNORMAL);
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
		dwType = REG_SZ;
		RegQueryValueExA(hKey, "synthname", NULL, &dwType, (LPBYTE)&SynthName, &dwSizeA);
		RegQueryValueExW(hKey, L"synthname", NULL, &dwType, (LPBYTE)&SynthNameW, &dwSizeW);
		RegCloseKey(hKey);

		if (defaultmidiout == 1)
			defaultmode = MOD_SWSYNTH;
		else
		{
			if (selectedtype < 0 || selectedtype > 6)
				selectedtype = 4;
		}

		defaultmode = SynthNamesTypes[selectedtype];

		if (debugmode == 1 && (!BannedSystemProcess() | !BlackListSystem())) {
			CreateConsole();
		}

		if (SynthName == NULL || SynthName == "\0") {
			ZeroMemory(SynthName, MAXPNAMELEN);
			strncpy(SynthName, "Keppy's Synthesizer\0", MAXPNAMELEN);
		}

		if (SynthNameW == NULL || SynthNameW == L"\0") {
			ZeroMemory(SynthNameW, MAXPNAMELEN);
			wcsncpy(SynthNameW, L"Keppy's Synthesizer\0", MAXPNAMELEN);
		}

		PrintToConsole(FOREGROUND_BLUE, 1, "Sharing MIDI caps with application...");

		MIDIOUTCAPSA * myCapsA;
		MIDIOUTCAPSW * myCapsW;
		MIDIOUTCAPS2A * myCaps2A;
		MIDIOUTCAPS2W * myCaps2W;

		const GUID CLSIDKEPSYNTH = { 0x318fa900, 0xf7de, 0x4ec6,{ 0x84, 0x8f, 0x0f, 0x28, 0xea, 0x37, 0x88, 0x9f } };

		switch (capsSize) {
		case (sizeof(MIDIOUTCAPSA)):
			myCapsA = (MIDIOUTCAPSA *)capsPtr;
			myCapsA->wMid = 0xffff; //MM_UNMAPPED
			myCapsA->wPid = 0xffff; //MM_PID_UNMAPPED
			memcpy(myCapsA->szPname, SynthName, sizeof(SynthName));
			myCapsA->wVoices = 65535;
			myCapsA->wNotes = 65535;
			myCapsA->wTechnology = defaultmode;
			myCapsA->wChannelMask = 0xffff;
			myCapsA->dwSupport = MIDICAPS_VOLUME | MIDICAPS_CACHE;
			PrintToConsole(FOREGROUND_BLUE, 1, "Done sharing caps. (MIDIOUTCAPSA)");
			return MMSYSERR_NOERROR;

		case (sizeof(MIDIOUTCAPSW)):
			myCapsW = (MIDIOUTCAPSW *)capsPtr;
			myCapsW->wMid = 0xffff;
			myCapsW->wPid = 0xffff;
			memcpy(myCapsW->szPname, SynthNameW, sizeof(SynthNameW));
			myCapsW->wVoices = 65535;
			myCapsW->wNotes = 65535;
			myCapsW->wTechnology = defaultmode;
			myCapsW->wChannelMask = 0xffff;
			myCapsW->dwSupport = MIDICAPS_VOLUME | MIDICAPS_CACHE;
			PrintToConsole(FOREGROUND_BLUE, 1, "Done sharing caps. (MIDIOUTCAPSW)");
			return MMSYSERR_NOERROR;

		case (sizeof(MIDIOUTCAPS2A)):
			myCaps2A = (MIDIOUTCAPS2A *)capsPtr;
			myCaps2A->wMid = 0xffff; //MM_UNMAPPED
			myCaps2A->wPid = 0xffff; //MM_PID_UNMAPPED
			memcpy(myCaps2A->szPname, SynthName, sizeof(SynthName));
			myCaps2A->ManufacturerGuid = CLSIDKEPSYNTH;
			myCaps2A->ProductGuid = CLSIDKEPSYNTH;
			myCaps2A->NameGuid = CLSIDKEPSYNTH;
			myCaps2A->wVoices = 65535;
			myCaps2A->wNotes = 65535;
			myCaps2A->wTechnology = defaultmode;
			myCaps2A->wChannelMask = 0xffff;
			myCaps2A->dwSupport = MIDICAPS_VOLUME | MIDICAPS_CACHE;
			PrintToConsole(FOREGROUND_BLUE, 1, "Done sharing caps. (MIDIOUTCAPS2A)");
			return MMSYSERR_NOERROR;

		case (sizeof(MIDIOUTCAPS2W)):
			myCaps2W = (MIDIOUTCAPS2W *)capsPtr;
			myCaps2W->wMid = 0xffff;
			myCaps2W->wPid = 0xffff;
			memcpy(myCaps2W->szPname, SynthNameW, sizeof(SynthNameW));
			myCaps2W->ManufacturerGuid = CLSIDKEPSYNTH;
			myCaps2W->ProductGuid = CLSIDKEPSYNTH;
			myCaps2W->NameGuid = CLSIDKEPSYNTH;
			myCaps2W->wVoices = 65535;
			myCaps2W->wNotes = 65535;
			myCaps2W->wTechnology = defaultmode;
			myCaps2W->wChannelMask = 0xffff;
			myCaps2W->dwSupport = MIDICAPS_VOLUME | MIDICAPS_CACHE;
			PrintToConsole(FOREGROUND_BLUE, 1, "Done sharing caps. (MIDIOUTCAPS2W)");
			return MMSYSERR_NOERROR;

		default:
			try {
				PrintToConsole(FOREGROUND_BLUE, 1, "App is not asking for specific caps. Trying to give Unicode caps...");
				myCapsW = (MIDIOUTCAPSW *)capsPtr;
				myCapsW->wMid = 0xffff;
				myCapsW->wPid = 0xffff;
				memcpy(myCapsW->szPname, SynthNameW, sizeof(SynthNameW));
				myCapsW->wVoices = 65535;
				myCapsW->wNotes = 65535;
				myCapsW->wTechnology = defaultmode;
				myCapsW->wChannelMask = 0xffff;
				myCapsW->dwSupport = MIDICAPS_VOLUME | MIDICAPS_CACHE;
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
		PrintToConsole(FOREGROUND_BLUE, 1, "Fatal error while sharing MIDI caps!!!");
		exit(-1);
	}

}

void CALLBACK MidiInProc(DWORD device, double time, const BYTE *buffer, DWORD length, void *user)
{
	BASS_MIDI_StreamEvents(KSStream, BASS_MIDI_EVENTS_RAW, buffer, length); // forward the data to the MIDI stream
}

unsigned WINAPI threadfunc(LPVOID lpV){
	try {
		if (BannedSystemProcess() == TRUE) {
			_endthreadex(0);
			return 0;
		}
		else {
			int opend = 0;
			while (opend == 0) {
				load_settings();
				load_bassfuncs();
				if (!com_initialized) {
					if (FAILED(CoInitialize(NULL))) continue;
					com_initialized = TRUE;
				}
				SetConsoleTextAttribute(hConsole, FOREGROUND_RED);
				InitializeAudioStream();
				if (BASS_Init(bassoutputfinal, frequency, xaudiodisabled ? BASS_DEVICE_LATENCY : 1, 0, NULL)) {
					InitializeBASS(info);
					InitializeBASSVST();
					if (encmode == 1) {
						InitializeBASSEnc();
					}
					SetUpStream();
					LoadSoundFontsToStream();
					opend = CreateThreads();
				}
			}
			PrintToConsole(FOREGROUND_RED, 1, "Checking for settings changes or hotkeys...");
			while (stop_rtthread == 0){
				Sleep(10);
				debug_info();
				CheckVolume();
			}
			stop_rtthread = 0;
			if (KSStream)
			{
				ResetSynth(0);
				BASS_StreamFree(KSStream);
				KSStream = 0;
			}
			if (bassmidi) {
				ResetSynth(0);
				FreeFonts(0);
				FreeLibrary(bassmidi);
				bassmidi = 0;
			}
			if (bass) {
				ResetSynth(0);
				BASS_Free();
				FreeLibrary(bass);
				bass = 0;
			}
			if (sound_driver) {
				ResetSynth(0);
				BASSXA_TerminateAudioStream(sound_driver);
			}
			if (com_initialized) {
				CoUninitialize();
				com_initialized = FALSE;
			}
			PrintToConsole(FOREGROUND_RED, 1, "Closing main thread...");
			_endthreadex(0);
			return 0;
		}
	}
	catch (...) {
		crashmessage(L"DrvMainThread");
		_endthreadex(0);
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
		DWORD result;
		InitializeCriticalSection(&mim_section);
		processPriority = GetPriorityClass(GetCurrentProcess());
		SetPriorityClass(GetCurrentProcess(), NORMAL_PRIORITY_CLASS);
		load_sfevent = CreateEvent(
			NULL,               // default security attributes
			TRUE,               // manual-reset event
			FALSE,              // initial state is nonsignaled
			TEXT("SoundFontEvent")  // object name
			);
		hCalcThread = (HANDLE)_beginthreadex(NULL, 0, threadfunc, 0, 0, &thrdaddr1);
		SetPriorityClass(hCalcThread, callprioval[driverprio]);
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
	RegSetValueEx(hKey, L"ramusage", 0, dwType, (LPBYTE)&One, sizeof(One));
	RegSetValueEx(hKey, L"handlecount", 0, dwType, (LPBYTE)&One, sizeof(One));
	RegSetValueEx(hKey, L"rightvol", 0, dwType, (LPBYTE)&One, 1);
	RegSetValueEx(hKey, L"leftvol", 0, dwType, (LPBYTE)&One, 1);
	for (int i = 0; i <= 15; ++i) {
		RegSetValueEx(hKey, cvnames[i], 0, dwType, (LPBYTE)&One, sizeof(One));
	}

	RegSetValueEx(hKey, L"buffull", 0, dwType, (LPBYTE)&One, 1);
	RegSetValueEx(hKey, L"int", 0, dwType, (LPBYTE)&One, 1);
	RegCloseKey(hKey);
	lResult = RegOpenKeyEx(HKEY_CURRENT_USER, L"Software\\Keppy's Synthesizer\\Watchdog", 0, KEY_ALL_ACCESS, &hKey);
	RegSetValueEx(hKey, L"currentapp", 0, dwType, (LPBYTE)&One, 1);
	RegSetValueEx(hKey, L"bit", 0, dwType, (LPBYTE)&One, 1);
	RegCloseKey(hKey);
	if (modm_closed == 0){
		stop_thread = 1;
		WaitForSingleObject(hCalcThread, INFINITE);
		CloseHandle(hCalcThread);
		modm_closed = 1;
		SetPriorityClass(GetCurrentProcess(), processPriority);
	}
	DeleteCriticalSection(&mim_section);
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
	UINT evbpoint;
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
			crashmessage(L"LongMODMData");
		}
	case MODM_DATA:
		try {
			ParseData(evbpoint, MODM_DATA, uDeviceID, dwParam1, dwParam2, exlen, sysexbuffer);
			break;
		}
		catch (...) {
			crashmessage(L"MODMData");
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