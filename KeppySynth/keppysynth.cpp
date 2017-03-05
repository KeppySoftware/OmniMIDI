/*
Keppy's Synthesizer, a fork of BASSMIDI Driver

Thank you Kode54 for allowing me to fork your awesome driver.
*/

#pragma comment(linker,"\"/manifestdependency:type='win32' \
name='Microsoft.Windows.Common-Controls' version='6.0.0.0' \
processorArchitecture='*' publicKeyToken='6595b64144ccf1df' language='*'\"")

#pragma comment(lib,"Version.lib")

#include "stdafx.h"
#include <Shlwapi.h>
#include <Tlhelp32.h>
#include <assert.h>
#include <Dbghelp.h>
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
#include <shlobj.h>
#include <signal.h>
#include <shellapi.h>
#include <sstream>
#include <stdarg.h>
#include <stdio.h>
#include <stdlib.h>
#include <string.h>
#include <string>
#include <tchar.h>
#include <Psapi.h>
#include <vector>
#include <winbase.h>
#include <windows.h>

#define BASSDEF(f) (WINAPI *f)	// define the BASS/BASSMIDI functions as pointers
#define BASSMIDIDEF(f) (WINAPI *f)	
#define BASSENCDEF(f) (WINAPI *f)	
#define BASS_VSTDEF(f) (WINAPI *f)
#define LOADBASSFUNCTION(f) *((void**)&f)=GetProcAddress(bass,#f)
#define LOADBASSMIDIFUNCTION(f) *((void**)&f)=GetProcAddress(bassmidi,#f)
#define LOADBASSENCFUNCTION(f) *((void**)&f)=GetProcAddress(bassenc,#f)
#define LOADBASS_VSTFUNCTION(f) *((void**)&f)=GetProcAddress(bass_vst,#f)
#define Between(value, a, b) (value <= b && value >= a)

#include <bass.h>
#include <bassmidi.h>
#include <bassenc.h>
#include <bass_vst.h>

#include "sound_out.h"

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

static BOOL com_initialized = FALSE;
static BOOL sound_out_float = FALSE;
static float sound_out_volume_float = 1.0;
static int sound_out_volume_int = 0x1000;

// Threads
static HANDLE hCalcThread = NULL;
static HANDLE hThread = NULL;
static HANDLE hThread2 = NULL;
static unsigned int thrdaddr1;
static unsigned int thrdaddr2;
static unsigned int thrdaddr3;

// Variables
#include "basserr.h"
#include "val.h"

class message_window
{
	HWND m_hWnd;
	ATOM class_atom;

public:
	message_window() {
		static const TCHAR * class_name = _T("Keppy's Synthesizer Message Window");
		WNDCLASSEX cls = { 0 };
		cls.cbSize = sizeof(cls);
		cls.lpfnWndProc = DefWindowProc;
		cls.hInstance = hinst;
		cls.lpszClassName = class_name;
		class_atom = RegisterClassEx(&cls);
		if (class_atom) {
			m_hWnd = CreateWindowEx(0, (LPCTSTR)class_atom, _T("keppysynth"), 0, 0, 0, 0, 0, HWND_MESSAGE, NULL, hinst, NULL);
		}
		else {
			m_hWnd = NULL;
		}
	}

	~message_window()
	{
		if (IsWindow(m_hWnd)) DestroyWindow(m_hWnd);
		if (class_atom) UnregisterClass((LPCTSTR)class_atom, hinst);
	}

	HWND get_hwnd() const { return m_hWnd; }
};

message_window * g_msgwnd = NULL;

void basserr(int error) {
	TCHAR buffer[MAX_PATH];
	wsprintfW(buffer, L"%d", error);
	TCHAR part1[MAX_PATH] = L"BASS encountered the error number ";
	TCHAR part2[MAX_PATH] = L": \"";
	TCHAR part3[MAX_PATH] = L"\"\n\nExplanation: ";
	TCHAR part4[MAX_PATH] = L"\n\nIf you're unsure about what this means, please take a screenshot, and give it to KaleidonKep99.";
	TCHAR partE[MAX_PATH] = L"\n\n(This might be caused by using old BASS libraries through the DLL override function.)";
	lstrcat(part1, buffer);
	lstrcat(part2, errname[error + 1]);
	lstrcat(part3, errdesc[error + 1]);
	lstrcat(part1, part2);
	lstrcat(part1, part3);
	lstrcat(part1, part4);
	if (isoverrideenabled == 1) {
		lstrcat(part1, partE);
	}
	MessageBox(NULL, part1, L"Keppy's Synthesizer - BASS execution error", MB_OK | MB_ICONERROR);
}

void CheckUp() {
	int error = BASS_ErrorGetCode();
	if (error != 0) {
		basserr(error);
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

static void DoStopClient();

BOOL WINAPI DllMain(HINSTANCE hinstDLL, DWORD fdwReason, LPVOID lpvReserved){

	if (fdwReason == DLL_PROCESS_ATTACH){
		hinst = hinstDLL;
		DisableThreadLibraryCalls(hinstDLL);
		g_msgwnd = new message_window;
	}
	else if (fdwReason == DLL_PROCESS_DETACH){
		;
		DoStopClient();
		delete g_msgwnd;
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
		return DefDriverProc(dwDriverId, hdrvr, uMsg, lParam1, lParam2);
	}
}

HRESULT modGetCaps(UINT uDeviceID, MIDIOUTCAPS* capsPtr, DWORD capsSize) {
	HKEY hKey;
	long lResult;
	int defaultmode;
	DWORD dwType = REG_DWORD;
	DWORD dwSize = sizeof(DWORD);
	lResult = RegOpenKeyEx(HKEY_CURRENT_USER, L"Software\\Keppy's Synthesizer\\Settings", 0, KEY_ALL_ACCESS, &hKey);
	RegQueryValueEx(hKey, L"shortname", NULL, &dwType, (LPBYTE)&shortname, &dwSize);
	RegQueryValueEx(hKey, L"defaultmidiout", NULL, &dwType, (LPBYTE)&defaultmidiout, &dwSize);
	RegQueryValueEx(hKey, L"newdevicename", NULL, &dwType, (LPBYTE)&selectedname, &dwSize);
	RegQueryValueEx(hKey, L"debugmode", NULL, &dwType, (LPBYTE)&debugmode, &dwSize);
	RegCloseKey(hKey);

	if (defaultmidiout == 1)
		defaultmode = MOD_SWSYNTH;
	else
		defaultmode = SynthNamesTypes[selectedname];

	if (debugmode == 1 && (!BannedSystemProcess() | !BlackListSystem())) {
		CreateConsole();
	}

	PrintToConsole(FOREGROUND_BLUE, 1, "Sharing MIDI caps with application...");

	MIDIOUTCAPSA * myCapsA;
	MIDIOUTCAPSW * myCapsW;
	MIDIOUTCAPS2A * myCaps2A;
    MIDIOUTCAPS2W * myCaps2W;

	const GUID CLSIDKEPSYNTH = { 0x318fa900, 0xf7de, 0x4ec6, { 0x84, 0x8f, 0x0f, 0x28, 0xea, 0x37, 0x88, 0x9f } };

	CHAR SynthName[MAXPNAMELEN];
	WCHAR SynthNameW[MAXPNAMELEN];

	if (selectedname > (defaultarraysize - 1))
		selectedname = defaultarraysize - 1;
	else if (selectedname < 0)
		selectedname = 0;

	strncpy(SynthName, SynthNames[selectedname], MAXPNAMELEN);
	wcsncpy(SynthNameW, SynthNamesW[selectedname], MAXPNAMELEN);

	switch (capsSize) {
	case (sizeof(MIDIOUTCAPSA)) :
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

	case (sizeof(MIDIOUTCAPSW)) :
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

	case (sizeof(MIDIOUTCAPS2A)) :
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

	case (sizeof(MIDIOUTCAPS2W)) :
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
		PrintToConsole(FOREGROUND_BLUE, 1, "Error while sharing MIDI caps.");
		return MMSYSERR_NOTSUPPORTED;
		break;
	}
}

unsigned _stdcall notescatcher(LPVOID lpV){
	try {
		PrintToConsole(FOREGROUND_RED, 1, "Initializing notes catcher thread...");
		while (stop_thread == 0){
			bmsyn_play_some_data();
			if (oldbuffermode == 1)
				break;
			Sleep(1);
		}
		PrintToConsole(FOREGROUND_RED, 1, "Closing notes catcher thread...");
		stop_thread = 0;
		_endthreadex(0);
		return 0;
	}
	catch (...) {
		crashmessage(L"NotesCatcher");
	}
}

void separatethreadfordata() {
	if (hThread2 == NULL) {
		PrintToConsole(FOREGROUND_RED, 1, "Creating thread for the note catcher...");
		hThread2 = (HANDLE)_beginthreadex(NULL, 0, notescatcher, 0, 0, &thrdaddr2);
		SetPriorityClass(hThread, REALTIME_PRIORITY_CLASS);
		SetThreadPriority(hThread, THREAD_PRIORITY_TIME_CRITICAL);
	}
}

unsigned __stdcall audioengine(LPVOID lpV){
	PrintToConsole(FOREGROUND_RED, 1, "Initializing audio rendering thread...");
	while (stop_thread == 0){
		try {
			if (reset_synth != 0){
				reset_synth = 0;
				BASS_MIDI_StreamEvent(KSStream, 0, MIDI_EVENT_SYSTEM, MIDI_SYSTEM_DEFAULT);
				CheckUp();
			}

			if (oldbuffermode == 1) {
				hThread2 = NULL;
				bmsyn_play_some_data();
			}
			else separatethreadfordata();

			if (xaudiodisabled == 1) {
				if (rco == 1) { Sleep(1); }
				BASS_ChannelUpdate(KSStream, 0);
				CheckUp();
			}
			else {
				AudioRender();
			}
		}
		catch (...) {
			crashmessage(L"AudioEngine");
		}
	}
	PrintToConsole(FOREGROUND_RED, 1, "Closing audio rendering thread...");
	stop_thread = 0;
	_endthreadex(0);
	return 0;
}

void CALLBACK MidiInProc(DWORD device, double time, const BYTE *buffer, DWORD length, void *user)
{
	BASS_MIDI_StreamEvents(KSStream, BASS_MIDI_EVENTS_RAW, buffer, length); // forward the data to the MIDI stream
}

unsigned __stdcall threadfunc(LPVOID lpV){
	USES_CONVERSION;
	try {
		if (BannedSystemProcess() == TRUE) {
			_endthreadex(0);
			return 0;
		}
		else {
			unsigned i;
			int check;
			int opend = 0;
			BASS_MIDI_FONT * mf;
			BASS_INFO info;
			BASS_DEVICEINFO dinfo;
			TCHAR loudmaxdll[MAX_PATH];
			TCHAR loudmaxdll64[MAX_PATH];
			SHGetFolderPath(NULL, CSIDL_PROFILE, NULL, 0, loudmaxdll);
			SHGetFolderPath(NULL, CSIDL_PROFILE, NULL, 0, loudmaxdll64);
			PathAppend(loudmaxdll, _T("\\Keppy's Synthesizer\\LoudMax.dll"));
			PathAppend(loudmaxdll64, _T("\\Keppy's Synthesizer\\LoudMax64.dll"));
			const char *LMDLL = T2A(loudmaxdll);
			const char *LMDLL64 = T2A(loudmaxdll64);
			while (opend == 0) {
				load_settings();
				load_bassfuncs();
				if (!com_initialized) {
					if (FAILED(CoInitialize(NULL))) continue;
					com_initialized = TRUE;
				}
				SetConsoleTextAttribute(hConsole, FOREGROUND_RED);
				if (xaudiodisabled == 1) {
					bassoutputfinal = (defaultoutput - 1);
					if (defaultoutput == 0) {
						check = 1;
					}
					else {
						check = bassoutputfinal;
					}
					BASS_GetDeviceInfo(check, &dinfo);
					if (dinfo.name == NULL || defaultoutput == 1) {
						bassoutputfinal = 0;
						noaudiodevices = 1;
						PrintToConsole(FOREGROUND_RED, 1, "Can not open DirectSound device. Switching to \"No sound\"...");
					}
					else {
						PrintToConsole(FOREGROUND_RED, bassoutputfinal, "<-- Default output device");
						PrintToConsole(FOREGROUND_RED, 1, "Opening DirectSound stream...");
					}
				}
				else {
					if (sound_driver == NULL) {
						PrintToConsole(FOREGROUND_RED, 1, "Opening XAudio stream...");
						sound_driver = create_sound_out_xaudio2();
						sound_out_float = TRUE;
						sound_driver->open(g_msgwnd->get_hwnd(), frequency + 100, (monorendering ? 1 : 2), sound_out_float, newsndbfvalue, frames);
						// Why frequency + 100? There's a bug on XAudio that cause clipping when the MIDI driver's audio frequency is the same as the sound card's max audio frequency.
						PrintToConsole(FOREGROUND_RED, 1, "XAudio ready.");
					}
				}
				if (BASS_Init(bassoutputfinal, frequency, xaudiodisabled ? BASS_DEVICE_LATENCY : 1, 0, NULL)) {
					CheckUp();
					PrintToConsole(FOREGROUND_RED, 1, "BASS initialized.");	
					BASS_SetConfig(BASS_CONFIG_UPDATEPERIOD, 0);
					BASS_SetConfig(BASS_CONFIG_UPDATETHREADS, 0);
					if (bassoutputfinal != 0) {
						PrintToConsole(FOREGROUND_RED, 1, "Working...");
						BASS_GetInfo(&info);
						PrintToConsole(FOREGROUND_RED, 1, "Got info about the output device...");
						if (vmsemu == 1) {
							BASS_SetConfig(BASS_CONFIG_BUFFER, 10 + info.minbuf + frames); // default buffer size = 'minbuf' + additional buffer size
						}
						else {
							BASS_SetConfig(BASS_CONFIG_BUFFER, 10 + info.minbuf); // default buffer size
						}
						KSStream = BASS_MIDI_StreamCreate(16, (sysresetignore ? BASS_MIDI_NOSYSRESET : 0) | (monorendering ? BASS_SAMPLE_MONO : 0) | AudioRenderingType(floatrendering) | (noteoff1 ? BASS_MIDI_NOTEOFF1 : 0) | (nofx ? BASS_MIDI_NOFX : 0) | (sinc ? BASS_MIDI_SINCINTER : 0), frequency);
						CheckUp();
						BASS_ChannelSetAttribute(KSStream, BASS_ATTRIB_NOBUFFER, 1);
						BASS_ChannelPlay(KSStream, false);
						CheckUp();
						PrintToConsole(FOREGROUND_RED, 1, "DirectSound stream enabled and running.");
					}
					else {
						PrintToConsole(FOREGROUND_RED, 1, "Working...");
						KSStream = BASS_MIDI_StreamCreate(16, BASS_STREAM_DECODE | (sysresetignore ? BASS_MIDI_NOSYSRESET : 0) | (monorendering ? BASS_SAMPLE_MONO : 0) | AudioRenderingType(floatrendering) | (noteoff1 ? BASS_MIDI_NOTEOFF1 : 0) | (nofx ? BASS_MIDI_NOFX : 0) | (sinc ? BASS_MIDI_SINCINTER : 0), frequency);
						CheckUp();
						if (noaudiodevices != 1) {
							PrintToConsole(FOREGROUND_RED, 1, "XAudio stream enabled.");
						}
						else {
							PrintToConsole(FOREGROUND_RED, 1, "Dummy stream enabled.");
						}
					}
					if (!KSStream) {
						BASS_StreamFree(KSStream);
						CheckUp();
						KSStream = 0;
						PrintToConsole(FOREGROUND_RED, 1, "Failed to open BASS stream.");
						continue;
					}
					else {
						BASS_ChannelSetAttribute(KSStream, BASS_ATTRIB_MIDI_VOICES, midivoices);
						BASS_ChannelSetAttribute(KSStream, BASS_ATTRIB_MIDI_CPU, maxcpu);
						BASS_ChannelSetAttribute(KSStream, BASS_ATTRIB_MIDI_KILL, fadeoutdisable);
						CheckUp();
					}
					// Error handling
					CheckUp();
					// LoudMax stuff lel
					#if defined(_WIN64)
					if (PathFileExists(loudmaxdll64)) {
						if (isbassvstloaded == 1) {
							BASS_VST_ChannelSetDSP(KSStream, LMDLL64, 0, 1);
						}
					}
					#elif defined(_WIN32)
					if (PathFileExists(loudmaxdll)) {
						if (isbassvstloaded == 1) {
							BASS_VST_ChannelSetDSP(KSStream, LMDLL, 0, 1);
						}
					}
					#endif

					// MIDI In code
					if (midiinenabled == 1) {
						// BASS_MIDI_DEVICEINFO* info;
						// BASS_MIDI_InGetDeviceInfo(defaultmidiindev, info);
						// BASS_MIDI_InInit(defaultmidiindev, MidiInProc, NULL);
						// BASS_MIDI_InStart(defaultmidiindev);
						// CheckUp();

						// Working on it, currently disabled.
					}
					// Encoder code
					if (encmode == 1) {
						PrintToConsole(FOREGROUND_RED, 1, "Opening BASSenc stream...");
						typedef std::basic_string<TCHAR> tstring;
						TCHAR encpath[MAX_PATH];
						TCHAR poop[MAX_PATH];
						TCHAR buffer[MAX_PATH] = { 0 };
						TCHAR * out;
						DWORD bufSize = sizeof(buffer) / sizeof(*buffer);
						if (GetModuleFileName(NULL, buffer, bufSize) == bufSize) {}
						out = PathFindFileName(buffer);
						std::wstring stemp = tstring(out) + L" - Keppy's Synthesizer Output File.wav";
						LPCWSTR result2 = stemp.c_str();
						HKEY hKey = 0;
						DWORD cbValueLength = sizeof(poop);
						DWORD dwType = REG_SZ;
						RegOpenKeyEx(HKEY_CURRENT_USER, L"Software\\Keppy's Synthesizer\\Settings", 0, KEY_ALL_ACCESS, &hKey);
						if (RegQueryValueEx(hKey, L"lastexportfolder", NULL, &dwType, reinterpret_cast<LPBYTE>(&poop), &cbValueLength) == ERROR_FILE_NOT_FOUND) {
							if (SUCCEEDED(SHGetFolderPath(NULL, CSIDL_DESKTOP, NULL, 0, encpath)))
							{
								PathAppend(encpath, result2);
							}
						}
						else {
							PathAppend(encpath, CString(poop));
							PathAppend(encpath, result2);
						}
						RegCloseKey(hKey);
						_bstr_t b(encpath);
						const char* c = b;
						const int result = MessageBox(NULL, L"You've enabled the \"Output to WAV\" mode.\n\nPress YES to confirm, or press NO to open the configurator\nand disable it.", L"Keppy's Synthesizer", MB_ICONINFORMATION | MB_YESNO | MB_SYSTEMMODAL);
						switch (result)
						{
						case IDYES:
							BASS_Encode_Start(KSStream, c, BASS_ENCODE_PCM | BASS_ENCODE_LIMIT, NULL, 0);
							// Error handling
							CheckUp();
							break;
						case IDNO:
							TCHAR configuratorapp[MAX_PATH];
							if (SUCCEEDED(SHGetFolderPath(NULL, CSIDL_SYSTEMX86, NULL, 0, configuratorapp)))
							{
								PathAppend(configuratorapp, _T("\\keppydrv\\KeppyDriverConfigurator.exe"));
								ShellExecute(NULL, L"open", configuratorapp, L"/AT", NULL, SW_SHOWNORMAL);
								exit(0);
								break;
							}
						}
						PrintToConsole(FOREGROUND_RED, 1, "BASSenc ready.");
					}
					// Cake.
					PrintToConsole(FOREGROUND_RED, 1, "Preparing stream...");
					BASS_ChannelSetAttribute(KSStream, BASS_ATTRIB_MIDI_CHANS, 16);
					BASS_MIDI_StreamEvent(KSStream, 0, MIDI_EVENT_SYSTEM, MIDI_SYSTEM_DEFAULT);
					BASS_MIDI_StreamEvent(KSStream, 9, MIDI_EVENT_DRUMS, 1);
					PrintToConsole(FOREGROUND_RED, 1, "Loading soundfonts...");
					if (LoadSoundfontStartup() == TRUE) {
						PrintToConsole(FOREGROUND_RED, 1, "Default list for app loaded.");
					}
					else {
						LoadSoundfont(defaultsflist);
						PrintToConsole(FOREGROUND_RED, 1, "Default global list loaded.");
					}
					PrintToConsole(FOREGROUND_RED, 1, "Creating threads...");
					SetEvent(load_sfevent);
					opend = 1;
					reset_synth = 0;
					hThread = (HANDLE)_beginthreadex(NULL, 0, audioengine, 0, 0, &thrdaddr2);
					SetPriorityClass(hThread, REALTIME_PRIORITY_CLASS);
					SetThreadPriority(hThread, THREAD_PRIORITY_TIME_CRITICAL);
					PrintToConsole(FOREGROUND_RED, 1, "Threads are now active.");
				}
			}
			PrintToConsole(FOREGROUND_RED, 1, "Checking for settings changes or hotkeys...");
			while (stop_rtthread == 0){
				Sleep(1);
				realtime_load_settings();
				debug_info();
				Panic();
				keybindings();
				WatchdogCheck();
				CheckVolume();
				mixervoid();
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
				delete sound_driver;
				sound_driver = NULL;
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
	}
}

void DoCallback(int driverNum, int clientNum, DWORD msg, DWORD_PTR param1, DWORD_PTR param2) {
	struct Driver_Client *client = &drivers[driverNum].clients[clientNum];
	DriverCallback(client->callback, client->flags, drivers[driverNum].hdrvr, msg, client->instance, param1, param2);
}

void DoStartClient() {
	if (modm_closed == 1) {
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
		SetPriorityClass(hCalcThread, REALTIME_PRIORITY_CLASS);
		SetThreadPriority(hCalcThread, THREAD_PRIORITY_TIME_CRITICAL);
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
	RegSetValueEx(hKey, L"currentvoices0", 0, dwType, (LPBYTE)&One, 1);
	RegSetValueEx(hKey, L"currentcpuusage0", 0, dwType, (LPBYTE)&One, 1);
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
	ResetSynth(0);
	reset_synth = 1;
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
			longmodmdata(IIMidiHdr, uDeviceID, dwParam1, dwParam2, exlen, sysexbuffer);
			DoCallback(uDeviceID, static_cast<LONG>(dwUser), MOM_DONE, dwParam1, 0);
		}
		catch (...) {
			crashmessage(L"LongMODMData");
		}
	case MODM_DATA:
		try {
			modmdata(evbpoint, uMsg, uDeviceID, dwParam1, dwParam2, exlen, sysexbuffer);
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