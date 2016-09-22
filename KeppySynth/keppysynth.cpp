/*
Keppy's Synthesizer, a fork of BASSMIDI Driver
*/

#pragma comment(linker,"\"/manifestdependency:type='win32' \
name='Microsoft.Windows.Common-Controls' version='6.0.0.0' \
processorArchitecture='*' publicKeyToken='6595b64144ccf1df' language='*'\"")

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
static HANDLE hCalcThread = NULL;
static DWORD processPriority;
static HANDLE load_sfevent = NULL;

static HSTREAM hStream = 0;

static BOOL com_initialized = FALSE;
static BOOL sound_out_float = FALSE;
static float sound_out_volume_float = 1.0;
static int sound_out_volume_int = 0x1000;

// Variables
static float *sndbf;
static int allhotkeys = 0; // Enable/Disable all the hotkeys
static int autopanic = 1; // Autopanic switch
static int bassoutputfinal = 0; // DO NOT TOUCH
static int defaultsflist = 1; // Default soundfont list
static int encmode = 0; // Encoder mode
static int frames = 0; // Default
static int frequency = 0; // Audio frequency
static int frequencynew = 0; // Audio frequency
static int maxcpu = 0; // CPU usage INT
static int midivoices = 0; // Max voices INT
static int midivolumeoverride = 0; // MIDI track volume override
static int newsndbfvalue; // DO NOT TOUCH
static int newevbuffvalue = 64; // DO NOT TOUCH
static int oldbuffermode = 0; // For old-ass PCs
static int nofloat = 1; // Enable or disable the float engine
static int nofx = 0; // Enable or disable FXs
static int noteoff1 = 0; // Note cut INT
static int preload = 0; // Soundfont preloading
static int defaultmidiout = 0; // Set as default MIDI out device for 8.x or newer
static int sinc = 0; // Sinc
static int sysresetignore = 0; //Ignore sysex messages
static int tracks = 0; // Tracks limit
static int vmsemu = 0; // VirtualMIDISynth buffer emulation
static int vms2emu = 0; // VirtualMIDISynth 2.x buffer emulation
static int volume = 0; // Volume limit
static int volumehotkeys = 1; // Enable/Disable volume hotkeys
static int volumemon = 1; // Volume monitoring
static int xaudiodisabled = 0; // Override the default engine

// Other stuff
static int decoded;

static sound_out * sound_driver = NULL;

static HINSTANCE hinst = NULL;             //main DLL handle

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
			m_hWnd = CreateWindowEx(0, (LPCTSTR)class_atom, _T("keppydrv"), 0, 0, 0, 0, 0, HWND_MESSAGE, NULL, hinst, NULL);
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

static HINSTANCE bass = 0;			// bass handle
static HINSTANCE bassmidi = 0;			// bassmidi handle
static HINSTANCE bassenc = 0;			// bassenc handle
static HINSTANCE bass_vst = 0;			// bass_vst handle

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

STDAPI_(LRESULT) DriverProc(DWORD_PTR dwDriverId, HDRVR hdrvr, UINT uMsg, LPARAM lParam1, LPARAM lParam2)
{
	switch (uMsg) {
	case DRV_QUERYCONFIGURE:
		return DRV_CANCEL;
	case DRV_CONFIGURE:
		return DRV_CANCEL;
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
	RegQueryValueEx(hKey, L"defaultmidiout", NULL, &dwType, (LPBYTE)&defaultmidiout, &dwSize);
	RegCloseKey(hKey);

	if (defaultmidiout == 1)
		defaultmode = MOD_SWSYNTH;
	else
		defaultmode = MOD_MIDIPORT;

	MIDIOUTCAPSA * myCapsA;
	MIDIOUTCAPSW * myCapsW;
	MIDIOUTCAPS2A * myCaps2A;
	MIDIOUTCAPS2W * myCaps2W;

	const GUID CLSID_bassmidi_synth = { 0xa675eda2, 0xeb26, 0x4e32, { 0xa3, 0xe7, 0xe4, 0xe6, 0x61, 0xd8, 0xfc, 0x3f } };
	
	CHAR synthName[] = "Keppy's Synthesizer\0";
	WCHAR synthNameW[] = L"Keppy's Synthesizer\0";

	switch (capsSize) {
	case (sizeof(MIDIOUTCAPSA)) :
		myCapsA = (MIDIOUTCAPSA *)capsPtr;
		myCapsA->wMid = 0xffff; //MM_UNMAPPED
		myCapsA->wPid = 0xffff; //MM_PID_UNMAPPED
		memcpy(myCapsA->szPname, synthName, sizeof(synthName));
		myCapsA->wTechnology = defaultmode;
		myCapsA->vDriverVersion = 0x022;
		myCapsA->wVoices = 100000;
		myCapsA->wNotes = 2147483647;
		myCapsA->wChannelMask = 0xffff;
		myCapsA->dwSupport = MIDICAPS_VOLUME;
		return MMSYSERR_NOERROR;

		break;
	case (sizeof(MIDIOUTCAPSW)) :
		myCapsW = (MIDIOUTCAPSW *)capsPtr;
		myCapsW->wMid = 0xffff;
		myCapsW->wPid = 0xffff;
		memcpy(myCapsW->szPname, synthNameW, sizeof(synthNameW));
		myCapsW->wTechnology = defaultmode;
		myCapsW->vDriverVersion = 0x022;
		myCapsW->wVoices = 100000;
		myCapsW->wNotes = 2147483647;
		myCapsW->wChannelMask = 0xffff;
		myCapsW->dwSupport = MIDICAPS_VOLUME;
		return MMSYSERR_NOERROR;

		break;
	case (sizeof(MIDIOUTCAPS2A)) :
		myCaps2A = (MIDIOUTCAPS2A *)capsPtr;
		myCaps2A->wMid = 0xffff;
		myCaps2A->wPid = 0xffff;
		memcpy(myCaps2A->szPname, synthName, sizeof(synthName));
		myCaps2A->wTechnology = defaultmode;
		myCaps2A->vDriverVersion = 0x022;
		myCaps2A->wVoices = 100000;
		myCaps2A->wNotes = 2147483647;
		myCaps2A->wChannelMask = 0xffff;
		myCaps2A->dwSupport = MIDICAPS_VOLUME;
		myCaps2A->ManufacturerGuid = CLSID_bassmidi_synth;
		myCaps2A->ProductGuid = CLSID_bassmidi_synth;
		myCaps2A->NameGuid = CLSID_bassmidi_synth;
		return MMSYSERR_NOERROR;

	case (sizeof(MIDIOUTCAPS2W)) :
		myCaps2W = (MIDIOUTCAPS2W *)capsPtr;
		myCaps2W->wMid = 0xffff;
		myCaps2W->wPid = 0xffff;
		memcpy(myCaps2W->szPname, synthNameW, sizeof(synthNameW));
		myCaps2W->wTechnology = defaultmode;
		myCaps2W->vDriverVersion = 0x022;
		myCaps2W->wVoices = 100000;
		myCaps2W->wNotes = 2147483647;
		myCaps2W->wChannelMask = 0xffff;
		myCaps2W->dwSupport = MIDICAPS_VOLUME;
		myCaps2W->ManufacturerGuid = CLSID_bassmidi_synth;
		myCaps2W->ProductGuid = CLSID_bassmidi_synth;
		myCaps2W->NameGuid = CLSID_bassmidi_synth;
		return MMSYSERR_NOERROR;

	default:
		return MMSYSERR_ERROR;
		break;
	}
}

bool compare_nocase(const std::string& first, const std::string& second)
{
	unsigned int i = 0;
	while ((i<first.length()) && (i<second.length()))
	{
		if (tolower(first[i])<tolower(second[i])) return true;
		else if (tolower(first[i])>tolower(second[i])) return false;
		++i;
	}
	return (first.length() < second.length());
}

unsigned _stdcall notescatcher(LPVOID lpV){
	while (stop_thread == 0){
		Sleep(1);
		bmsyn_play_some_data();
	}
	stop_thread = 0;
	_endthreadex(0);
	return 0;
}

unsigned __stdcall audioengine(LPVOID lpV){
	while (stop_thread == 0){
		if (reset_synth != 0){
			reset_synth = 0;
			load_settings();
			BASS_MIDI_StreamEvent(hStream, 0, MIDI_EVENT_SYSTEM, MIDI_SYSTEM_DEFAULT);
			BASS_MIDI_StreamLoadSamples(hStream);
		}
		if (xaudiodisabled == 1) {
			BASS_ChannelUpdate(hStream, 0);
			Sleep(1);
		}
		else {
			AudioRender();
			Sleep(1);
		}
	}
	stop_thread = 0;
	_endthreadex(0);
	return 0;
}

unsigned __stdcall oldbuffersystemforaldotarving(LPVOID lpV){
	while (stop_thread == 0){
		if (reset_synth != 0){
			reset_synth = 0;
			load_settings();
			BASS_MIDI_StreamEvent(hStream, 0, MIDI_EVENT_SYSTEM, MIDI_SYSTEM_DEFAULT);
			BASS_MIDI_StreamLoadSamples(hStream);
		}
		bmsyn_play_some_data();
		if (xaudiodisabled == 1) {
			BASS_ChannelUpdate(hStream, 0);
			Sleep(1);
		}
		else {
			AudioRender();
			Sleep(1);
		}
	}
	stop_thread = 0;
	_endthreadex(0);
	return 0;
}

unsigned __stdcall threadfunc(LPVOID lpV){
	USES_CONVERSION;
	try {
		if (BannedSystemProcess() == TRUE) {
			_endthreadex(0);
			return 0;
		}
		else {
			HANDLE hThread = NULL;
			HANDLE hThread2 = NULL;
			unsigned int thrdaddr;
			unsigned i;
			int opend = 0;
			BASS_MIDI_FONT * mf;
			BASS_INFO info;
			TCHAR loudmaxdll[MAX_PATH];
			TCHAR loudmaxdll64[MAX_PATH];
			SHGetFolderPath(NULL, CSIDL_PROFILE, NULL, 0, loudmaxdll);
			SHGetFolderPath(NULL, CSIDL_PROFILE, NULL, 0, loudmaxdll64);
			PathAppend(loudmaxdll, _T("\\Keppy's Synthesizer\\LoudMax.dll"));
			PathAppend(loudmaxdll64, _T("\\Keppy's Synthesizer\\LoudMax64.dll"));
			const char *LMDLL = T2A(loudmaxdll);
			const char *LMDLL64 = T2A(loudmaxdll64);
			while (opend == 0) {
				Sleep(100);
				if (!com_initialized) {
					if (FAILED(CoInitialize(NULL))) continue;
					com_initialized = TRUE;
				}
				load_settings();
				if (xaudiodisabled == 1) {
					bassoutputfinal = -1;
					// Do nothing, since now the driver is going to use DirectSound.
				}
				else {
					if (sound_driver == NULL) {
						sound_driver = create_sound_out_xaudio2();
						sound_out_float = TRUE;
						sound_driver->open(g_msgwnd->get_hwnd(), frequency + 100, 2, sound_out_float, newsndbfvalue, frames);
						// Why frequency + 100? There's a bug on XAudio that cause clipping when the MIDI driver's audio frequency is the same has the sound card's max audio frequency.
					}
				}
				load_bassfuncs();
				OutputDebugString(L"Initializing the stream...");
				if (BASS_Init(bassoutputfinal, frequency, xaudiodisabled ? BASS_DEVICE_LATENCY : 1, 0, NULL)) {
					if (bassoutputfinal == -1) {
						BASS_GetInfo(&info);
						if (vmsemu == 1) {
							BASS_SetConfig(BASS_CONFIG_BUFFER, 11 + info.minbuf + frames); // default buffer size = 'minbuf' + additional buffer size
						}
						else {
							BASS_SetConfig(BASS_CONFIG_BUFFER, 11 + info.minbuf); // default buffer size
						}
						BASS_SetConfig(BASS_CONFIG_MIDI_VOICES, 100000);
						BASS_SetConfig(BASS_CONFIG_UPDATEPERIOD, 0);
						BASS_SetConfig(BASS_CONFIG_UPDATETHREADS, 0);
						hStream = BASS_MIDI_StreamCreate(tracks, (IgnoreSystemReset() ? BASS_MIDI_NOSYSRESET : sysresetignore) | BASS_SAMPLE_SOFTWARE | BASS_SAMPLE_FLOAT | (IsNoteOff1TurnedOn() ? BASS_MIDI_NOTEOFF1 : noteoff1) | (AreEffectsDisabled() ? BASS_MIDI_NOFX : nofx) | (check_sinc() ? BASS_MIDI_SINCINTER : sinc), frequency);
						BASS_ChannelSetAttribute(hStream, BASS_ATTRIB_MIDI_VOICES, midivoices);
						BASS_ChannelSetAttribute(hStream, BASS_ATTRIB_MIDI_CPU, maxcpu);
						BASS_ChannelPlay(hStream, false);
					}
					else {
						BASS_SetConfig(BASS_CONFIG_MIDI_VOICES, 100000);
						BASS_SetConfig(BASS_CONFIG_UPDATEPERIOD, 0);
						BASS_SetConfig(BASS_CONFIG_UPDATETHREADS, 0);
						hStream = BASS_MIDI_StreamCreate(tracks, BASS_STREAM_DECODE | (IgnoreSystemReset() ? BASS_MIDI_NOSYSRESET : sysresetignore) | BASS_SAMPLE_SOFTWARE | BASS_SAMPLE_FLOAT | (IsNoteOff1TurnedOn() ? BASS_MIDI_NOTEOFF1 : noteoff1) | (AreEffectsDisabled() ? BASS_MIDI_NOFX : nofx) | (check_sinc() ? BASS_MIDI_SINCINTER : sinc), frequency);
						BASS_ChannelSetAttribute(hStream, BASS_ATTRIB_MIDI_VOICES, midivoices);
						BASS_ChannelSetAttribute(hStream, BASS_ATTRIB_MIDI_CPU, maxcpu);
					}
					if (!hStream) {
						BASS_StreamFree(hStream);
						hStream = 0;
						continue;
					}
					// LoudMax stuff lel
					#if defined(_WIN64)
					if (PathFileExists(loudmaxdll64)) {
						BASS_VST_ChannelSetDSP(hStream, LMDLL64, 0, 1);
					}
					#elif defined(_WIN32)
					if (PathFileExists(loudmaxdll)) {
						BASS_VST_ChannelSetDSP(hStream, LMDLL, 0, 1);
					}
					#endif
					// Encoder code
					if (encmode == 1) {
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
							BASS_Encode_Start(hStream, c, BASS_ENCODE_PCM | BASS_ENCODE_LIMIT, NULL, 0);
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
					}
					// Cake.
					BASS_MIDI_StreamEvent(hStream, 0, MIDI_EVENT_SYSTEM, MIDI_SYSTEM_DEFAULT);
					BASS_MIDI_StreamEvent(hStream, 9, MIDI_EVENT_DRUMS, 1);
					BASS_ChannelSetAttribute(hStream, BASS_ATTRIB_NOBUFFER, 1);
					BASS_ChannelSetAttribute(hStream, BASS_ATTRIB_MIDI_CHANS, tracks);
					if (LoadSoundfontStartup() == TRUE) {
						// Wew
					}
					else {
						LoadSoundfont(defaultsflist);
					}
					SetEvent(load_sfevent);
					opend = 1;
					reset_synth = 0;
					if (oldbuffermode == 1) {
						hThread = (HANDLE)_beginthreadex(NULL, 0, oldbuffersystemforaldotarving, 0, 0, &thrdaddr);
						SetPriorityClass(hThread, REALTIME_PRIORITY_CLASS);
						SetThreadPriority(hThread, THREAD_PRIORITY_TIME_CRITICAL);
					}
					else {
						hThread = (HANDLE)_beginthreadex(NULL, 0, audioengine, 0, 0, &thrdaddr);
						hThread2 = (HANDLE)_beginthreadex(NULL, 0, notescatcher, 0, 0, &thrdaddr);
						SetPriorityClass(hThread, REALTIME_PRIORITY_CLASS);
						SetPriorityClass(hThread2, REALTIME_PRIORITY_CLASS);
						SetThreadPriority(hThread, THREAD_PRIORITY_TIME_CRITICAL);
						SetThreadPriority(hThread2, THREAD_PRIORITY_TIME_CRITICAL);
					}
				}
			}
			while (stop_rtthread == 0){
				Sleep(1);
				realtime_load_settings();
				debug_info();
				keybindings();
				WatchdogCheck();
				mixervoid();
			}
			stop_rtthread = 0;
			if (hStream)
			{
				ResetSynth();
				BASS_StreamFree(hStream);
				hStream = 0;
			}
			if (bassmidi) {
				ResetSynth();
				FreeFonts(0);
				FreeLibrary(bassmidi);
				bassmidi = 0;
			}
			if (bass) {
				ResetSynth();
				BASS_Free();
				FreeLibrary(bass);
				bass = 0;
			}
			if (sound_driver) {
				ResetSynth();
				delete sound_driver;
				sound_driver = NULL;
			}
			if (com_initialized) {
				CoUninitialize();
				com_initialized = FALSE;
			}
			_endthreadex(0);
			return 0;
		}
	}
	catch (int e) {
		crashhandler(e);
	}
}

void DoCallback(int driverNum, int clientNum, DWORD msg, DWORD_PTR param1, DWORD_PTR param2) {
	struct Driver_Client *client = &drivers[driverNum].clients[clientNum];
	DriverCallback(client->callback, client->flags, drivers[driverNum].hdrvr, msg, client->instance, param1, param2);
}

void DoStartClient() {
	if (modm_closed == 1) {
		DWORD result;
		unsigned int thrdaddr;
		InitializeCriticalSection(&mim_section);
		processPriority = GetPriorityClass(GetCurrentProcess());
		SetPriorityClass(GetCurrentProcess(), NORMAL_PRIORITY_CLASS);
		load_sfevent = CreateEvent(
			NULL,               // default security attributes
			TRUE,               // manual-reset event
			FALSE,              // initial state is nonsignaled
			TEXT("SoundFontEvent")  // object name
			);
		hCalcThread = (HANDLE)_beginthreadex(NULL, 0, threadfunc, 0, 0, &thrdaddr);
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
	ResetSynth();
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
		catch (int e) {
			crashhandler(e);
		}
	case MODM_DATA:
		try {
			modmdata(evbpoint, uMsg, uDeviceID, dwParam1, dwParam2, exlen, sysexbuffer);
			break;
		}
		catch (int e) {
			crashhandler(e);
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
		ResetSynth();
		return MMSYSERR_NOERROR;
	}
	case MODM_STOP: {
		reset_synth = 1;
		ResetSynth();
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