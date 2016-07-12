/*
Keppy's Driver, a fork of BASSMIDI Driver
*/

#define STRICT

#if __DMC__
unsigned long _beginthreadex(void *security, unsigned stack_size,
	unsigned(__stdcall *start_address)(void *), void *arglist,
	unsigned initflag, unsigned *thrdaddr);
void _endthreadex(unsigned retval);
#endif

#define _CRT_SECURE_CPP_OVERLOAD_STANDARD_NAMES 1
#define _CRT_SECURE_NO_WARNINGS 1
#include "stdafx.h"
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
#include <shlobj.h>
#include <signal.h>
#include <sstream>
#include <stdarg.h>
#include <stdio.h>
#include <stdlib.h>
#include <string.h>
#include <string>
#include <tchar.h>
#include <vector>
#include <winbase.h>
#include <windows.h>

#define BASSDEF(f) (WINAPI *f)	// define the BASS/BASSMIDI functions as pointers
#define BASSMIDIDEF(f) (WINAPI *f)	
#define BASSENCDEF(f) (WINAPI *f)	
#define LOADBASSFUNCTION(f) *((void**)&f)=GetProcAddress(bass,#f)
#define LOADBASSMIDIFUNCTION(f) *((void**)&f)=GetProcAddress(bassmidi,#f)
#define LOADBASSENCFUNCTION(f) *((void**)&f)=GetProcAddress(bassenc,#f)

#include <bass.h>
#include <bassmidi.h>
#include <bassenc.h>

#include "sound_out.h"

#define MAX_DRIVERS 255
#define MAX_CLIENTS 255 // Per driver

#ifndef _LOADRESTRICTIONS_OFF
#define _LOADRESTRICTIONS_ON
#endif

struct Driver_Client {
	int allocated;
	DWORD_PTR instance;
	DWORD flags;
	DWORD_PTR callback;
};

//Note: drivers[0] is not used (See OnDriverOpen).
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
static volatile int reset_synth = 0;
static HANDLE hCalcThread = NULL;
static DWORD processPriority;
static HANDLE load_sfevent = NULL;

static unsigned int font_count[8] = { 0, 0, 0, 0, 0, 0, 0, 0 };
static HSOUNDFONT * hFonts[8] = { NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL };
static HSTREAM hStream = 0;

static BOOL com_initialized = FALSE;
static BOOL sound_out_float = FALSE;
static float sound_out_volume_float = 1.0;
static int sound_out_volume_int = 0x1000;

static BYTE gs_part_to_ch[16];
static BYTE drum_channels[16];

// Variables
static float *sndbf;
static int allhotkeys = 1; // Enable/Disable all the hotkeys
static int availableports = 4; // How many ports are available
static int defaultsflist = 1; // Default soundfont list
static int encmode = 0; // Encoder mode
static int frames = 0; // Default
static int frequency = 0; // Audio frequency
static int legacybuf = 0; // Enable or disable the legacy buffer system from BASSMIDI Driver 1.x
static int maxcpu = 0; // CPU usage INT
static int midivoices = 0; // Max voices INT
static int midivolumeoverride = 0; // MIDI track volume override
static int newsndbfvalue; // DO NOT TOUCH
static int nofloat = 1; // Enable or disable the float engine
static int nofx = 0; // Enable or disable FXs
static int noteoff1 = 0; // Note cut INT
static int preload = 0; // Soundfont preloading
static int sfdisableconf = 0; // Enable/Disable that annoying confirmation popup that asks you if you want to change the sf list lel
static int sinc = 0; // Sinc
static int sysresetignore = 0; //Ignore sysex messages
static int tracks = 0; // Tracks limit
static int vmsemu = 0; // VirtualMIDISynth buffer emulation
static int volume = 0; // Volume limit
static int volumehotkeys = 1; // Enable/Disable volume hotkeys
static int volumemon = 1; // Volume monitoring
static int xaudiodisabled = 0; // Override the default engine

// Channels volume
static int ch1 = 16383;
static int ch2 = 16383;
static int ch3 = 16383;
static int ch4 = 16383;
static int ch5 = 16383;
static int ch6 = 16383;
static int ch7 = 16383;
static int ch8 = 16383;
static int ch9 = 16383;
static int ch10 = 16383;
static int ch11 = 16383;
static int ch12 = 16383;
static int ch13 = 16383;
static int ch14 = 16383;
static int ch15 = 16383;
static int ch16 = 16383;
static int tch1 = 16383;
static int tch2 = 16383;
static int tch3 = 16383;
static int tch4 = 16383;
static int tch5 = 16383;
static int tch6 = 16383;
static int tch7 = 16383;
static int tch8 = 16383;
static int tch9 = 16383;
static int tch10 = 16383;
static int tch11 = 16383;
static int tch12 = 16383;
static int tch13 = 16383;
static int tch14 = 16383;
static int tch15 = 16383;
static int tch16 = 16383;

// Watchdog
static int rel1 = 0;
static int rel2 = 0;
static int rel3 = 0;
static int rel4 = 0;
static int rel5 = 0;
static int rel6 = 0;
static int rel7 = 0;
static int rel8 = 0;

// Other stuff
static int decoded;

static sound_out * sound_driver = NULL;

static HINSTANCE bass = 0;			// bass handle
static HINSTANCE bassmidi = 0;			// bassmidi handle
static HINSTANCE bassenc = 0;			// bassmidi handle

//TODO: Can be done with: HMODULE GetDriverModuleHandle(HDRVR hdrvr);  (once DRV_OPEN has been called)
static HINSTANCE hinst = NULL;             //main DLL handle

// Keppy's Driver vital parts
#include "sfsystem.h"
#include "bufsystem.h"
#include "bansystem.h"
#include "settings.h"
#include "watchdog.h"

static void DoStopClient();

class message_window
{
	HWND m_hWnd;
	ATOM class_atom;

public:
	message_window() {
		static const TCHAR * class_name = _T("keppymididrv message window");
		WNDCLASSEX cls = { 0 };
		cls.cbSize = sizeof(cls);
		cls.lpfnWndProc = DefWindowProc;
		cls.hInstance = hinst;
		cls.lpszClassName = class_name;
		class_atom = RegisterClassEx(&cls);
		if (class_atom) {
			m_hWnd = CreateWindowEx(0, (LPCTSTR)class_atom, _T("keppymididrv"), 0, 0, 0, 0, 0, HWND_MESSAGE, NULL, hinst, NULL);
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
	//The DRV_LOAD message is always the first message that a device driver receives. 
	//Notifies the driver that it has been loaded. The driver should make sure that any hardware and supporting drivers it needs to function properly are present.
	memset(drivers, 0, sizeof(drivers));
	driverCount = 0;
	return DRV_OK;
}

LRESULT DoDriverOpen(HDRVR hdrvr, LPCWSTR driverName, LONG lParam) {

	/*
	Remarks

	If the driver returns a nonzero value, the system uses that value as the driver identifier (the dwDriverId parameter)
	in messages it subsequently sends to the driver instance. The driver can return any type of value as the identifier.
	For example, some drivers return memory addresses that point to instance-specific information. Using this method of
	specifying identifiers for a driver instance gives the drivers ready access to the information while they are processing messages.
	*/

	/*
	When the driver's DriverProc function receives a
	DRV_OPEN message, it should:
	1. Allocate memory space for a structure instance.
	2. Add the structure instance to the linked list.
	3. Store instance data in the new list entry.
	4. Specify the entry's number or address as the return value for the DriverProc function.
	Subsequent calls to DriverProc will include the list entry's identifier as its dwDriverID
	argument
	*/
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

LRESULT DoDriverConfigure(DWORD_PTR dwDriverId, HDRVR hdrvr, HWND parent, DRVCONFIGINFO* configInfo) {
	return DRV_CANCEL;
}

/* INFO Installable Driver Reference: http://msdn.microsoft.com/en-us/library/ms709328%28v=vs.85%29.aspx */
/* The original header is LONG DriverProc(DWORD dwDriverId, HDRVR hdrvr, UINT msg, LONG lParam1, LONG lParam2);
but that does not support 64bit. See declaration of DefDriverProc to see where the values come from.
*/
STDAPI_(LRESULT) DriverProc(DWORD_PTR dwDriverId, HDRVR hdrvr, UINT uMsg, LPARAM lParam1, LPARAM lParam2)
{
	switch (uMsg) {
		/* Seems this is only for kernel mode drivers
		case DRV_INSTALL:
		return DoDriverInstall(dwDriverId, hdrvr, static_cast<DRVCONFIGINFO*>(lParam2));
		case DRV_REMOVE:
		DoDriverRemove(dwDriverId, hdrvr);
		return DRV_OK;
		*/
	case DRV_QUERYCONFIGURE:
		//TODO: Until it doesn't have a configuration window, it should return 0.
		return DRV_CANCEL;
	case DRV_CONFIGURE:
		return DoDriverConfigure(dwDriverId, hdrvr, reinterpret_cast<HWND>(lParam1), reinterpret_cast<DRVCONFIGINFO*>(lParam2));

		/* TODO: Study this. It has implications:
		Calling OpenDriver, described in the Win32 SDK. This function calls SendDriverMessage to
		send DRV_LOAD and DRV_ENABLE messages only if the driver has not been previously loaded,
		and then to send DRV_OPEN.
		� Calling CloseDriver, described in the Win32 SDK. This function calls SendDriverMessage to
		send DRV_CLOSE and, if there are no other open instances of the driver, to also send
		DRV_DISABLE and DRV_FREE.
		*/
	case DRV_LOAD:
		return DoDriverLoad();
	case DRV_FREE:
		//The DRV_FREE message is always the last message that a device driver receives. 
		//Notifies the driver that it is being removed from memory. The driver should free any memory and other system resources that it has allocated.
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
	MIDIOUTCAPSA * myCapsA;
	MIDIOUTCAPSW * myCapsW;
	MIDIOUTCAPS2A * myCaps2A;
	MIDIOUTCAPS2W * myCaps2W;
	
	CHAR synthName[] = "Keppy's Driver\0";
	WCHAR synthNameW[] = L"Keppy's Driver\0";

	switch (capsSize) {
	case (sizeof(MIDIOUTCAPSA)) :
		myCapsA = (MIDIOUTCAPSA *)capsPtr;
		myCapsA->wMid = 0xffff;
		myCapsA->wPid = 0xffff;
		memcpy(myCapsA->szPname, synthName, sizeof(synthName));
		myCapsA->wTechnology = MOD_SWSYNTH;
		myCapsA->vDriverVersion = 0x0090;
		myCapsA->wVoices = 9999;
		myCapsA->wNotes = 0;
		myCapsA->wChannelMask = 0xffff;
		myCapsA->dwSupport = MIDICAPS_VOLUME;
		return MMSYSERR_NOERROR;

	case (sizeof(MIDIOUTCAPSW)) :
		myCapsW = (MIDIOUTCAPSW *)capsPtr;
		myCapsW->wMid = 0xffff;
		myCapsW->wPid = 0xffff;
		memcpy(myCapsW->szPname, synthNameW, sizeof(synthNameW));
		myCapsW->wTechnology = MOD_SWSYNTH;
		myCapsW->vDriverVersion = 0x0090;
		myCapsW->wVoices = 9999;
		myCapsW->wNotes = 0;
		myCapsW->wChannelMask = 0xffff;
		myCapsW->dwSupport = MIDICAPS_VOLUME;
		return MMSYSERR_NOERROR;

	case (sizeof(MIDIOUTCAPS2A)) :
		myCaps2A = (MIDIOUTCAPS2A *)capsPtr;
		myCaps2A->wMid = 0xffff;
		myCaps2A->wPid = 0xffff;
		memcpy(myCaps2A->szPname, synthName, sizeof(synthName));
		myCaps2A->wTechnology = MOD_SWSYNTH;
		myCaps2A->vDriverVersion = 0x0090;
		myCaps2A->wVoices = 9999;
		myCaps2A->wNotes = 0;
		myCaps2A->wChannelMask = 0xffff;
		myCaps2A->dwSupport = MIDICAPS_VOLUME;
		return MMSYSERR_NOERROR;

	case (sizeof(MIDIOUTCAPS2W)) :
		myCaps2W = (MIDIOUTCAPS2W *)capsPtr;
		myCaps2W->wMid = 0xffff;
		myCaps2W->wPid = 0xffff;
		memcpy(myCaps2W->szPname, synthNameW, sizeof(synthNameW));
		myCaps2W->wTechnology = MOD_SWSYNTH;
		myCaps2W->vDriverVersion = 0x0090;
		myCaps2W->wVoices = 9999;
		myCaps2W->wNotes = 0;
		myCaps2W->wChannelMask = 0xffff;
		myCaps2W->dwSupport = MIDICAPS_VOLUME;
		return MMSYSERR_NOERROR;

	default:
		return MMSYSERR_ERROR;
	}
}

void DLLLoadError(LPCWSTR dll) {
	TCHAR errormessage[MAX_PATH] = L"There was an error while trying to load the DLL for the driver!\nFaulty/missing DLL: ";
	TCHAR clickokmsg[MAX_PATH] = L"\n\nClick OK to close the program.";
	lstrcat(errormessage, dll);
	lstrcat(errormessage, clickokmsg);
	MessageBox(NULL, errormessage, L"Keppy's Driver - DLL load error", MB_ICONERROR | MB_SYSTEMMODAL);
}

BOOL load_bassfuncs()
{
	TCHAR installpath[MAX_PATH] = { 0 };
	TCHAR basspath[MAX_PATH] = { 0 };
	TCHAR bassmidipath[MAX_PATH] = { 0 };
	TCHAR bassencpath[MAX_PATH] = { 0 };
	TCHAR pluginpath[MAX_PATH] = { 0 };
	WIN32_FIND_DATA fd;
	HANDLE fh;
	int installpathlength;

	GetModuleFileName(hinst, installpath, MAX_PATH);
	PathRemoveFileSpec(installpath);

	lstrcat(basspath, installpath);
	lstrcat(basspath, L"\\bass.dll");
	if (!(bass = LoadLibrary(basspath))) {
		DLLLoadError(basspath);
		exit(0);
	}
	lstrcat(bassmidipath, installpath);
	lstrcat(bassmidipath, L"\\bassmidi.dll");
	if (!(bassmidi = LoadLibrary(bassmidipath))) {
		DLLLoadError(bassmidipath);
		exit(0);
	}
	lstrcat(bassencpath, installpath);
	lstrcat(bassencpath, L"\\bassenc.dll");
	if (!(bassenc = LoadLibrary(bassencpath))) {
		DLLLoadError(bassencpath);
		exit(0);
	}
	/* "load" all the BASS functions that are to be used */
	OutputDebugString(L"Loading BASS functions....");
	LOADBASSENCFUNCTION(BASS_Encode_Start);
	LOADBASSENCFUNCTION(BASS_Encode_Stop);
	LOADBASSFUNCTION(BASS_ChannelFlags);
	LOADBASSFUNCTION(BASS_ChannelGetAttribute);
	LOADBASSFUNCTION(BASS_ChannelGetData);
	LOADBASSFUNCTION(BASS_ChannelGetLevel);
	LOADBASSFUNCTION(BASS_ChannelPlay);
	LOADBASSFUNCTION(BASS_ChannelRemoveFX);
	LOADBASSFUNCTION(BASS_ChannelSetAttribute);
	LOADBASSFUNCTION(BASS_ChannelSetFX);
	LOADBASSFUNCTION(BASS_ChannelUpdate);
	LOADBASSFUNCTION(BASS_ErrorGetCode);
	LOADBASSFUNCTION(BASS_Free);
	LOADBASSFUNCTION(BASS_GetInfo);
	LOADBASSFUNCTION(BASS_Init);
	LOADBASSFUNCTION(BASS_PluginLoad);
	LOADBASSFUNCTION(BASS_SetConfig);
	LOADBASSFUNCTION(BASS_SetVolume);
	LOADBASSFUNCTION(BASS_SetVolume);
	LOADBASSFUNCTION(BASS_StreamFree);
	LOADBASSMIDIFUNCTION(BASS_MIDI_FontFree);
	LOADBASSMIDIFUNCTION(BASS_MIDI_FontInit);
	LOADBASSMIDIFUNCTION(BASS_MIDI_FontLoad);
	LOADBASSMIDIFUNCTION(BASS_MIDI_StreamCreate);
	LOADBASSMIDIFUNCTION(BASS_MIDI_StreamEvent);
	LOADBASSMIDIFUNCTION(BASS_MIDI_StreamEvents);
	LOADBASSMIDIFUNCTION(BASS_MIDI_StreamGetEvent);
	LOADBASSMIDIFUNCTION(BASS_MIDI_StreamLoadSamples);
	LOADBASSMIDIFUNCTION(BASS_MIDI_StreamSetFonts);
	OutputDebugString(L"Done.");

	installpathlength = lstrlen(installpath) + 1;
	lstrcat(pluginpath, installpath);
	lstrcat(pluginpath, L"\\bass*.dll");
	fh = FindFirstFile(pluginpath, &fd);
	if (fh != INVALID_HANDLE_VALUE) {
		do {
			HPLUGIN plug;
			pluginpath[installpathlength] = 0;
			lstrcat(pluginpath, fd.cFileName);
			plug = BASS_PluginLoad((char*)pluginpath, BASS_UNICODE);
		} while (FindNextFile(fh, &fd));
		FindClose(fh);
	}
	return TRUE;
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

void AudioRender(int bassoutput) {
	if (bassoutput == -1) {
		BASS_ChannelUpdate(hStream, 0);
	}
	else {
		decoded = BASS_ChannelGetData(hStream, sndbf, BASS_DATA_FLOAT + newsndbfvalue * sizeof(float));
		if (encmode == 1) {

		}
		else if (encmode == 0) {
			if (decoded < 0) {

			}
			else {
				for (unsigned i = 0, j = decoded / sizeof(float); i < j; i++) {
					float sample = sndbf[i];
					sample *= sound_out_volume_float;
					sndbf[i] = sample;
				}
				sound_driver->write_frame(sndbf, decoded / sizeof(float), false);
			}
		}
	}
}

unsigned __stdcall threadfunc(LPVOID lpV){
	if (BannedSystemProcess() == TRUE) {
		_endthreadex(0);
		return 0;
	}
	else {
		unsigned i;
		int bassoutputfinal = 0;
		int opend = 0;
		BASS_MIDI_FONT * mf;
		BASS_INFO info;
		while (opend == 0 && stop_thread == 0) {
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
			BASS_SetConfig(BASS_CONFIG_MIDI_VOICES, 9999);
			BASS_SetConfig(BASS_CONFIG_UPDATEPERIOD, 0);
			BASS_SetConfig(BASS_CONFIG_UPDATETHREADS, 0);
			OutputDebugString(L"Initializing the stream...");
			if (BASS_Init(bassoutputfinal, frequency, BASS_DEVICE_LATENCY, NULL, NULL)) {
				if (bassoutputfinal == -1) {
					BASS_GetInfo(&info);
					if (vmsemu == 1) {
						BASS_SetConfig(BASS_CONFIG_BUFFER, info.minbuf + 10 + frames); // default buffer size = 'minbuf' + additional buffer size
					}
					else {
						BASS_SetConfig(BASS_CONFIG_BUFFER, info.minbuf + 10 + 1); // default buffer size
					}
					hStream = BASS_MIDI_StreamCreate(tracks, (IgnoreSystemReset() ? BASS_MIDI_NOSYSRESET : sysresetignore) | BASS_SAMPLE_SOFTWARE | BASS_SAMPLE_FLOAT | (IsNoteOff1TurnedOn() ? BASS_MIDI_NOTEOFF1 : noteoff1) | (AreEffectsDisabled() ? BASS_MIDI_NOFX : nofx) | (check_sinc() ? BASS_MIDI_SINCINTER : sinc), 0);
					BASS_ChannelPlay(hStream, false);
				}
				else {
					hStream = BASS_MIDI_StreamCreate(tracks, BASS_STREAM_DECODE | (IgnoreSystemReset() ? BASS_MIDI_NOSYSRESET : sysresetignore) | BASS_SAMPLE_SOFTWARE | BASS_SAMPLE_FLOAT | (IsNoteOff1TurnedOn() ? BASS_MIDI_NOTEOFF1 : noteoff1) | (AreEffectsDisabled() ? BASS_MIDI_NOFX : nofx) | (check_sinc() ? BASS_MIDI_SINCINTER : sinc), 0);
				}
				if (!hStream) {
					BASS_StreamFree(hStream);
					hStream = 0;
					continue;
				}
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
					std::wstring stemp = tstring(out) + L" - Keppy's Driver Output File.wav";
					LPCWSTR result2 = stemp.c_str();
					HKEY hKey = 0;
					DWORD cbValueLength = sizeof(poop);
					DWORD dwType = REG_SZ;
					RegOpenKeyEx(HKEY_CURRENT_USER, L"Software\\Keppy's Driver\\Settings", 0, KEY_ALL_ACCESS, &hKey);
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
					_bstr_t b(encpath);
					const char* c = b;
					const int result = MessageBox(NULL, L"You've enabled the \"Output to WAV\" mode.\n\nPress YES to confirm, or press NO to open the configurator\nand disable it.", L"Keppy's Driver", MB_ICONINFORMATION | MB_YESNO | MB_SYSTEMMODAL);
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
							ShellExecute(NULL, L"open", configuratorapp, L"-advancedtab", NULL, SW_SHOWNORMAL);
							exit(0);
							break;
						}
					}
				}
				// Cake.
				BASS_MIDI_StreamEvent(hStream, 0, MIDI_EVENT_SYSTEM, MIDI_SYSTEM_DEFAULT);
				BASS_ChannelSetAttribute(hStream, BASS_ATTRIB_NOBUFFER, 1);
				BASS_ChannelSetAttribute(hStream, BASS_ATTRIB_MIDI_CHANS, tracks);
				BASS_ChannelSetAttribute(hStream, BASS_ATTRIB_MIDI_VOICES, midivoices);
				BASS_ChannelSetAttribute(hStream, BASS_ATTRIB_MIDI_CPU, maxcpu);
				LoadSoundfont(defaultsflist);
				SetEvent(load_sfevent);
				opend = 1;
				reset_synth = 0;
			}
		}
		while (stop_thread == 0){
			if (reset_synth != 0){
				reset_synth = 0;
				load_settings();
				BASS_MIDI_StreamEvent(hStream, 0, MIDI_EVENT_SYSTEM, MIDI_SYSTEM_DEFAULT);
				BASS_MIDI_StreamLoadSamples(hStream);
			}
			if (legacybuf == 1) {
				bmsyn_play_some_data_old();
			}
			else {
				bmsyn_play_some_data();
			}
			AudioRender(bassoutputfinal);
			realtime_load_settings();
			keybindings();
			debug_info();
			WatchdogCheck();
			mixervoid();
		}
		stop_thread = 0;
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

void DoCallback(int driverNum, int clientNum, DWORD msg, DWORD_PTR param1, DWORD_PTR param2) {
	struct Driver_Client *client = &drivers[driverNum].clients[clientNum];
	DriverCallback(client->callback, client->flags, drivers[driverNum].hdrvr, msg, client->instance, param1, param2);
}

void DoStartClient() {
	if (modm_closed == 1) {
		RunWatchdog();
		DWORD result;
		unsigned int thrdaddr;
		InitializeCriticalSection(&mim_section);
		processPriority = GetPriorityClass(GetCurrentProcess());
		SetPriorityClass(GetCurrentProcess(), REALTIME_PRIORITY_CLASS);
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
	lResult = RegOpenKeyEx(HKEY_CURRENT_USER, L"Software\\Keppy's Driver", 0, KEY_ALL_ACCESS, &hKey);
	RegSetValueEx(hKey, L"currentvoices0", 0, dwType, (LPBYTE)&One, 1);
	RegSetValueEx(hKey, L"currentcpuusage0", 0, dwType, (LPBYTE)&One, 1);
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
	/*
	TODO : If the driver's output queue contains any output buffers (see MODM_LONGDATA) whose contents
	have not been sent to the kernel-mode driver, the driver should set the MHDR_DONE flag and
	clear the MHDR_INQUEUE flag in each buffer's MIDIHDR structure, and then send the client a
	MOM_DONE callback message for each buffer.
	*/
	ResetSynth();
	reset_synth = 1;
}

LONG DoOpenClient(struct Driver *driver, UINT uDeviceID, LONG* dwUser, MIDIOPENDESC * desc, DWORD flags) {
	/*	For the MODM_OPEN message, dwUser is an output parameter.
	The driver creates the instance identifier and returns it in the address specified as
	the argument. The argument is the instance identifier.
	CALLBACK_EVENT Indicates dwCallback member of MIDIOPENDESC is an event handle.
	CALLBACK_FUNCTION Indicates dwCallback member of MIDIOPENDESC is the address of a callback function.
	CALLBACK_TASK Indicates dwCallback member of MIDIOPENDESC is a task handle.
	CALLBACK_WINDOW Indicates dwCallback member of MIDIOPENDESC is a window handle.
	*/
	int clientNum;
	if (driver->clientCount == 0) {
		//TODO: Part of this might be done in DoDriverOpen instead.
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
	//TODO: desc and flags

	DoCallback(uDeviceID, clientNum, MOM_OPEN, 0, 0);

	RunWatchdog();

	return MMSYSERR_NOERROR;
}

LONG DoCloseClient(struct Driver *driver, UINT uDeviceID, LONG dwUser) {
	/*
	If the client has passed data buffers to the user-mode driver by means of MODM_LONGDATA
	messages, and if the user-mode driver hasn't finished sending the data to the kernel-mode driver,
	the user-mode driver should return MIDIERR_STILLPLAYING in response to MODM_CLOSE.
	After the driver closes the device instance it should send a MOM_CLOSE callback message to
	the client.
	*/

	KillWatchdog();

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
/* Audio Device Messages for MIDI http://msdn.microsoft.com/en-us/library/ff536194%28v=vs.85%29 */
STDAPI_(DWORD) modMessage(UINT uDeviceID, UINT uMsg, DWORD_PTR dwUser, DWORD_PTR dwParam1, DWORD_PTR dwParam2){
	UINT evbpoint;
	MIDIHDR *IIMidiHdr;
	struct Driver *driver = &drivers[uDeviceID];
	int exlen = 0;
	char *sysexbufferold = NULL;
	unsigned char *sysexbuffer = NULL;
	DWORD result = 0;
	switch (uMsg) {
	case MODM_OPEN:
		return DoOpenClient(driver, uDeviceID, reinterpret_cast<LONG*>(dwUser), reinterpret_cast<MIDIOPENDESC*>(dwParam1), static_cast<DWORD>(dwParam2));
	case MODM_PREPARE:
		/*If the driver returns MMSYSERR_NOTSUPPORTED, winmm.dll prepares the buffer for use. For
		most drivers, this behavior is sufficient.*/
		return MMSYSERR_NOTSUPPORTED;
	case MODM_UNPREPARE:
		return MMSYSERR_NOTSUPPORTED;
	case MODM_GETNUMDEVS:
		return ProcessBlackList();
	case MODM_GETDEVCAPS:
		return modGetCaps(uDeviceID, reinterpret_cast<MIDIOUTCAPS*>(dwParam1), static_cast<DWORD>(dwParam2));
	case MODM_LONGDATA:
		if (legacybuf == 1) {
			longmodmdata_old(IIMidiHdr, dwParam1, dwParam2, exlen, sysexbufferold);
		}
		else {
			longmodmdata(IIMidiHdr, uDeviceID, dwParam1, dwParam2, exlen, sysexbuffer);
		}
		DoCallback(uDeviceID, static_cast<LONG>(dwUser), MOM_DONE, dwParam1, 0);
	case MODM_DATA:
		if (legacybuf == 1) {
			modmdata_old(evbpoint, uMsg, dwParam1, dwParam2, exlen, sysexbufferold);
		}
		else {
			modmdata(evbpoint, uMsg, uDeviceID, dwParam1, dwParam2, exlen, sysexbuffer);
		}	
		break;
	case MODM_GETVOLUME: {
		*(LONG*)dwParam1 = static_cast<LONG>(sound_out_volume_float * 0xFFFF);
		return MMSYSERR_NOERROR;
	}
	case MODM_SETVOLUME: {
		sound_out_volume_float = LOWORD(dwParam1) / (float)0xFFFF;
		sound_out_volume_int = (int)(sound_out_volume_float * (float)0x1000);
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
		/*
		MODM_GETPOS
		MODM_PAUSE
		//The driver must halt MIDI playback in the current position. The driver must then turn off all notes that are currently on.
		MODM_RESTART
		//The MIDI output device driver must restart MIDI playback at the current position.
		// playback will start on the first MODM_RESTART message that is received regardless of the number of MODM_PAUSE that messages were received.
		//Likewise, MODM_RESTART messages that are received while the driver is already in play mode must be ignored. MMSYSERR_NOERROR must be returned in either case
		MODM_STOP
		//Like reset, without resetting.
		MODM_PROPERTIES
		MODM_STRMDATA
		*/
	case MODM_CLOSE:
		return DoCloseClient(driver, uDeviceID, static_cast<LONG>(dwUser));
		break;

		/*
		MODM_CACHEDRUMPATCHES
		MODM_CACHEPATCHES
		*/

	default:
		return MMSYSERR_NOERROR;
		break;
	}
}