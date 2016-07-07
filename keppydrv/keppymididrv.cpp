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
#include <assert.h>
#include <atlbase.h>
#include <atlstr.h>
#include <stdarg.h>
#include <stdio.h>
#include <stdlib.h>
#include <windows.h>
#include <process.h>
#include <Shlwapi.h>
#include <mmddk.h>
#include <mmsystem.h>
#include <tchar.h>
#include <limits>
#include "stdafx.h"
#include <vector>
#include <signal.h>
#include <list>
#include <sstream>
#include <string>
#include <shlobj.h>
#include <fstream>
#include <iostream>
#include <cctype>
#include <process.h>
#include <Tlhelp32.h>
#include <winbase.h>
#include <string.h>
#include <comdef.h>

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

// Variables
static float *sndbf;
static int allhotkeys = 1; // Enable/Disable all the hotkeys
static int availableports = 4; // How many ports are available
static int defaultsflist = 1; // Default soundfont list
static int encmode = 0; // Encoder mode
static int frames = 0; // Default
static int frequency = 0; // Audio frequency
static int maxcpu = 0; // CPU usage INT
static int midivoices = 0; // Max voices INT
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
static int midivolumeoverride = 0; // MIDI track volume override
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

std::vector<HSOUNDFONT> _soundFonts[8];
std::vector<BASS_MIDI_FONTEX> presetList[8];

static void FreeFonts(UINT uDeviceID)
{
	unsigned i;
	if (_soundFonts[uDeviceID].size())
	{
		for (auto it = _soundFonts[uDeviceID].begin(); it != _soundFonts[uDeviceID].end(); ++it)
		{
			BASS_MIDI_FontFree(*it);
		}
		_soundFonts[uDeviceID].resize(0);
		presetList[uDeviceID].resize(0);
	}
}

static bool load_font_item(unsigned uDeviceID, const TCHAR * in_path)
{
	const DWORD bass_flags =
#ifdef UNICODE
		BASS_UNICODE
#else
		0
#endif
		;
	const TCHAR * ext = _T("");
	const TCHAR * dot = _tcsrchr(in_path, _T('.'));
	if (dot != 0) ext = dot + 1;
	if (!_tcsicmp(ext, _T("sf2"))
		|| !_tcsicmp(ext, _T("sf2pack"))
		|| !_tcsicmp(ext, _T("sfz"))
		)
	{
		HSOUNDFONT font = BASS_MIDI_FontInit(in_path, bass_flags);
		if (!font)
		{
			return false;
		}
		_soundFonts[uDeviceID].push_back(font);
		BASS_MIDI_FONTEX fex = { font, -1, -1, -1, 0, 0 };
		presetList[uDeviceID].push_back(fex);
		return true;
	}
	else if (!_tcsicmp(ext, _T("sflist")))
	{
		FILE * fl = _tfopen(in_path, _T("r, ccs=UTF-8"));
		if (fl)
		{
			TCHAR path[32768], temp[32768];
			TCHAR name[32768];
			TCHAR *nameptr;
			const TCHAR * slash = _tcsrchr(in_path, _T('\\'));
			if (slash != 0) _tcsncpy(path, in_path, slash - in_path + 1);
			while (!feof(fl))
			{
				std::vector<BASS_MIDI_FONTEX> presets;

				if (!_fgetts(name, 32767, fl)) break;
				name[32767] = 0;
				TCHAR * cr = _tcschr(name, _T('\n'));
				if (cr) *cr = 0;
				cr = _tcschr(name, _T('\r'));
				if (cr) *cr = 0;
				cr = _tcschr(name, '|');
				if (cr)
				{
					bool valid = true;
					TCHAR *endchr;
					nameptr = cr + 1;
					*cr = 0;
					cr = name;
					while (*cr && valid)
					{
						switch (*cr++)
						{
						case 'p':
						{
							// patch override - "p[db#,]dp#=[sb#,]sp#" ex. "p0,5=0,1"
							// may be used once per preset group
							long dbank = 0;
							long dpreset = _tcstol(cr, &endchr, 10);
							if (endchr == cr)
							{
								valid = false;
								break;
							}
							if (*endchr == ',')
							{
								dbank = dpreset;
								cr = endchr + 1;
								dpreset = _tcstol(cr, &endchr, 10);
								if (endchr == cr)
								{
									valid = false;
									break;
								}
							}
							if (*endchr != '=')
							{
								valid = false;
								break;
							}
							cr = endchr + 1;
							long sbank = -1;
							long spreset = _tcstol(cr, &endchr, 10);
							if (endchr == cr)
							{
								valid = false;
								break;
							}
							if (*endchr == ',')
							{
								sbank = spreset;
								cr = endchr + 1;
								spreset = _tcstol(cr, &endchr, 10);
								if (endchr == cr)
								{
									valid = false;
									break;
								}
							}
							if (*endchr && *endchr != ';' && *endchr != '&')
							{
								valid = false;
								break;
							}
							cr = endchr;
							BASS_MIDI_FONTEX fex = { 0, (int)spreset, (int)sbank, (int)dpreset, (int)dbank, 0 };
							presets.push_back(fex);
						}
						break;

						case '&':
						{
						}
						break;

						case ';':
							// separates preset items
							break;

						default:
							// invalid command character
							valid = false;
							break;
						}
					}
					if (!valid)
					{
						presets.clear();
						BASS_MIDI_FONTEX fex = { 0, -1, -1, -1, 0, 0 };
						presets.push_back(fex);
					}
				}
				else
				{
					BASS_MIDI_FONTEX fex = { 0, -1, -1, -1, 0, 0 };
					presets.push_back(fex);
					nameptr = name;
				}
				if ((isalpha(nameptr[0]) && nameptr[1] == _T(':')) || nameptr[0] == '\\')
				{
					_tcscpy(temp, nameptr);
				}
				else
				{
					_tcscat(temp, nameptr);
				}
				if (name[0] != '@') {
					HSOUNDFONT font = BASS_MIDI_FontInit(temp, bass_flags);
					for (auto it = presets.begin(); it != presets.end(); ++it)
					{
						if (preload)
							BASS_MIDI_FontLoad(font, it->spreset, it->sbank);
						it->font = font;
						presetList[uDeviceID].push_back(*it);
					}
					_soundFonts[uDeviceID].push_back(font);
				}
				else {
					continue;
				}
			}
			fclose(fl);
			return true;
		}
	}
	return false;
}

void RunWatchdog()
{
	HKEY hKey;
	long lResult;
	TCHAR watchdog[MAX_PATH];
	DWORD dwType = REG_DWORD;
	DWORD dwSize = sizeof(DWORD);
	DWORD one = 1;
	lResult = RegOpenKeyEx(HKEY_CURRENT_USER, L"Software\\Keppy's Driver\\Watchdog", 0, KEY_ALL_ACCESS, &hKey);
	RegSetValueEx(hKey, L"wdrun", 0, dwType, (LPBYTE)&one, sizeof(one));
	if (SUCCEEDED(SHGetFolderPath(NULL, CSIDL_SYSTEMX86, NULL, 0, watchdog)))
	{
		PathAppend(watchdog, _T("\\keppydrv\\KeppyDriverWatchdog.exe"));
		ShellExecute(NULL, L"open", watchdog, NULL, NULL, SW_SHOWNORMAL);
	}
}

void KillWatchdog()
{
	HKEY hKey;
	long lResult;
	DWORD dwType = REG_DWORD;
	DWORD dwSize = sizeof(DWORD);
	DWORD one = 1;
	DWORD zero = 0;
	lResult = RegOpenKeyEx(HKEY_CURRENT_USER, L"Software\\Keppy's Driver\\Watchdog", 0, KEY_ALL_ACCESS, &hKey);
	RegSetValueEx(hKey, L"closewatchdog", 0, dwType, (LPBYTE)&one, sizeof(one));
	RegSetValueEx(hKey, L"wdrun", 0, dwType, (LPBYTE)&zero, sizeof(zero));
}

void LoadFonts(UINT uDeviceID, const TCHAR * name)
{
	FreeFonts(uDeviceID);

	if (name && *name)
	{
		const TCHAR * ext = _tcsrchr(name, _T('.'));
		if (ext) ext++;
		if (!_tcsicmp(ext, _T("sf2")) || !_tcsicmp(ext, _T("sf2pack")) || !_tcsicmp(ext, _T("sfz")))
		{
			if (!load_font_item(uDeviceID, name))
			{
				FreeFonts(uDeviceID);
				return;
			}
		}
		else if (!_tcsicmp(ext, _T("sflist")))
		{
			if (!load_font_item(uDeviceID, name))
			{
				FreeFonts(uDeviceID);
				return;
			}
		}

		std::vector< BASS_MIDI_FONTEX > fonts;
		for (unsigned long i = 0, j = presetList[uDeviceID].size(); i < j; ++i)
		{
			fonts.push_back(presetList[uDeviceID][j - i - 1]);
		}
		BASS_MIDI_StreamSetFonts(hStream, &fonts[0], (unsigned int)fonts.size() | BASS_MIDI_FONT_EX);
	}
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

struct evbuf_t{
	UINT uDeviceID;
	UINT   uMsg;
	DWORD_PTR	dwParam1;
	DWORD_PTR	dwParam2;
	int exlen;
	unsigned char *sysexbuffer;
};

#define EVBUFF_SIZE 0x3FFFC
static struct evbuf_t evbuf[EVBUFF_SIZE];
static UINT  evbwpoint = 0;
static UINT  evbrpoint = 0;
static volatile LONG evbcount = 0;
static UINT evbsysexpoint;

int bmsyn_buf_check(void){
	int retval;
	EnterCriticalSection(&mim_section);
	retval = evbcount;
	LeaveCriticalSection(&mim_section);
	return retval;
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

int bmsyn_play_some_data(void){
	UINT uDeviceID;
	UINT uMsg;
	DWORD_PTR	dwParam1;
	DWORD_PTR   dwParam2;

	UINT evbpoint;
	int exlen;
	unsigned char *sysexbuffer;
	int played;

	played = 0;
	if (!bmsyn_buf_check()){
		played = ~0;
		return played;
	}
	do{
		EnterCriticalSection(&mim_section);
		evbpoint = evbrpoint;
		if (++evbrpoint >= EVBUFF_SIZE)
			evbrpoint -= EVBUFF_SIZE;

		uDeviceID = evbuf[evbpoint].uDeviceID;
		uMsg = evbuf[evbpoint].uMsg;
		dwParam1 = evbuf[evbpoint].dwParam1;
		dwParam2 = evbuf[evbpoint].dwParam2;
		exlen = evbuf[evbpoint].exlen;
		sysexbuffer = evbuf[evbpoint].sysexbuffer;

		LeaveCriticalSection(&mim_section);
		switch (uMsg) {
		case MODM_DATA:
			dwParam2 = dwParam1 & 0xF0;
			exlen = (dwParam2 >= 0xF8 && dwParam2 <= 0xFF) ? 1 : ((dwParam2 == 0xC0 || dwParam2 == 0xD0) ? 2 : 3);
			BASS_MIDI_StreamEvents(hStream, BASS_MIDI_EVENTS_RAW, &dwParam1, exlen);
			break;
		case MODM_LONGDATA:
#ifdef DEBUG
			FILE * logfile;
			logfile = fopen("c:\\dbglog2.log", "at");
			if (logfile != NULL) {
				for (int i = 0; i < exlen; i++)
					fprintf(logfile, "%x ", sysexbuffer[i]);
				fprintf(logfile, "\n");
			}
			fclose(logfile);
#endif
			BASS_MIDI_StreamEvents(hStream, BASS_MIDI_EVENTS_RAW, sysexbuffer, exlen);
			free(sysexbuffer);
			break;
		}
	} while (InterlockedDecrement(&evbcount));
	return played;
}

void AudioRender(int bassoutput) {
	if (bassoutput == -1) {
		if (vmsemu == 0) {
			BASS_ChannelUpdate(hStream, 0);
		}
		else {
		
		}
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
	LOADBASSMIDIFUNCTION(BASS_MIDI_StreamSetFonts);
	LOADBASSMIDIFUNCTION(BASS_MIDI_StreamLoadSamples);
	LOADBASSMIDIFUNCTION(BASS_MIDI_StreamGetEvent);
	LOADBASSMIDIFUNCTION(BASS_MIDI_StreamEvents);
	LOADBASSMIDIFUNCTION(BASS_MIDI_StreamEvent);
	LOADBASSMIDIFUNCTION(BASS_MIDI_StreamCreate);
	LOADBASSMIDIFUNCTION(BASS_MIDI_FontLoad);
	LOADBASSMIDIFUNCTION(BASS_MIDI_FontInit);
	LOADBASSMIDIFUNCTION(BASS_MIDI_FontFree);
	LOADBASSFUNCTION(BASS_StreamFree);
	LOADBASSFUNCTION(BASS_SetVolume);
	LOADBASSFUNCTION(BASS_SetVolume);
	LOADBASSFUNCTION(BASS_SetConfig);
	LOADBASSFUNCTION(BASS_PluginLoad);
	LOADBASSFUNCTION(BASS_Init);
	LOADBASSFUNCTION(BASS_GetInfo);
	LOADBASSFUNCTION(BASS_Free);
	LOADBASSFUNCTION(BASS_ErrorGetCode);
	LOADBASSFUNCTION(BASS_ChannelUpdate);
	LOADBASSFUNCTION(BASS_ChannelSetFX);
	LOADBASSFUNCTION(BASS_ChannelSetAttribute);
	LOADBASSFUNCTION(BASS_ChannelRemoveFX);
	LOADBASSFUNCTION(BASS_ChannelPlay);
	LOADBASSFUNCTION(BASS_ChannelGetLevel);
	LOADBASSFUNCTION(BASS_ChannelGetData);
	LOADBASSFUNCTION(BASS_ChannelGetAttribute);
	LOADBASSFUNCTION(BASS_ChannelFlags);
	LOADBASSENCFUNCTION(BASS_Encode_Stop);
	LOADBASSENCFUNCTION(BASS_Encode_Start);
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


void load_settings()
{
	HKEY hKey;
	long lResult;
	DWORD dwType = REG_DWORD;
	DWORD dwSize = sizeof(DWORD);
	lResult = RegOpenKeyEx(HKEY_CURRENT_USER, L"Software\\Keppy's Driver\\Settings", 0, KEY_ALL_ACCESS, &hKey);
	RegQueryValueEx(hKey, L"allhotkeys", NULL, &dwType, (LPBYTE)&allhotkeys, &dwSize);
	RegQueryValueEx(hKey, L"buflen", NULL, &dwType, (LPBYTE)&frames, &dwSize);
	RegQueryValueEx(hKey, L"cpu", NULL, &dwType, (LPBYTE)&maxcpu, &dwSize);
	RegQueryValueEx(hKey, L"defaultsflist", NULL, &dwType, (LPBYTE)&defaultsflist, &dwSize);
	RegQueryValueEx(hKey, L"encmode", NULL, &dwType, (LPBYTE)&encmode, &dwSize);
	RegQueryValueEx(hKey, L"frequency", NULL, &dwType, (LPBYTE)&frequency, &dwSize);
	RegQueryValueEx(hKey, L"midivolumeoverride", NULL, &dwType, (LPBYTE)&midivolumeoverride, &dwSize);
	RegQueryValueEx(hKey, L"polyphony", NULL, &dwType, (LPBYTE)&midivoices, &dwSize);
	RegQueryValueEx(hKey, L"preload", NULL, &dwType, (LPBYTE)&preload, &dwSize);
	RegQueryValueEx(hKey, L"sfdisableconf", NULL, &dwType, (LPBYTE)&sfdisableconf, &dwSize);
	RegQueryValueEx(hKey, L"sinc", NULL, &dwType, (LPBYTE)&sinc, &dwSize);
	RegQueryValueEx(hKey, L"sndbfvalue", NULL, &dwType, (LPBYTE)&newsndbfvalue, &dwSize);
	RegQueryValueEx(hKey, L"tracks", NULL, &dwType, (LPBYTE)&tracks, &dwSize);
	RegQueryValueEx(hKey, L"vmsemu", NULL, &dwType, (LPBYTE)&vmsemu, &dwSize);
	RegQueryValueEx(hKey, L"volume", NULL, &dwType, (LPBYTE)&volume, &dwSize);
	RegQueryValueEx(hKey, L"volumehotkeys", NULL, &dwType, (LPBYTE)&volumehotkeys, &dwSize);
	RegQueryValueEx(hKey, L"xaudiodisabled", NULL, &dwType, (LPBYTE)&xaudiodisabled, &dwSize);
	RegCloseKey(hKey);

	sndbf = (float *)malloc(newsndbfvalue*sizeof(float));

	sound_out_volume_float = (float)volume / 10000.0f;
	sound_out_volume_int = (int)(sound_out_volume_float * (float)0x1000);
}

void realtime_load_settings()
{
	HKEY hKey;
	long lResult;
	DWORD dwType = REG_DWORD;
	DWORD dwSize = sizeof(DWORD);
	lResult = RegOpenKeyEx(HKEY_CURRENT_USER, L"Software\\Keppy's Driver\\Settings", 0, KEY_ALL_ACCESS, &hKey);
	RegQueryValueEx(hKey, L"allhotkeys", NULL, &dwType, (LPBYTE)&allhotkeys, &dwSize);
	RegQueryValueEx(hKey, L"buflen", NULL, &dwType, (LPBYTE)&frames, &dwSize);
	RegQueryValueEx(hKey, L"cpu", NULL, &dwType, (LPBYTE)&maxcpu, &dwSize);
	RegQueryValueEx(hKey, L"midivolumeoverride", NULL, &dwType, (LPBYTE)&midivolumeoverride, &dwSize);
	RegQueryValueEx(hKey, L"nofx", NULL, &dwType, (LPBYTE)&nofx, &dwSize);
	RegQueryValueEx(hKey, L"noteoff", NULL, &dwType, (LPBYTE)&noteoff1, &dwSize);
	RegQueryValueEx(hKey, L"polyphony", NULL, &dwType, (LPBYTE)&midivoices, &dwSize);
	RegQueryValueEx(hKey, L"sfdisableconf", NULL, &dwType, (LPBYTE)&sfdisableconf, &dwSize);
	RegQueryValueEx(hKey, L"sinc", NULL, &dwType, (LPBYTE)&sinc, &dwSize);
	RegQueryValueEx(hKey, L"sysresetignore", NULL, &dwType, (LPBYTE)&sysresetignore, &dwSize);
	RegQueryValueEx(hKey, L"tracks", NULL, &dwType, (LPBYTE)&tracks, &dwSize);
	RegQueryValueEx(hKey, L"volume", NULL, &dwType, (LPBYTE)&volume, &dwSize);
	RegQueryValueEx(hKey, L"volumehotkeys", NULL, &dwType, (LPBYTE)&volumehotkeys, &dwSize);
	RegQueryValueEx(hKey, L"volumemon", NULL, &dwType, (LPBYTE)&volumemon, &dwSize);
	RegCloseKey(hKey);
	//cake
	if (xaudiodisabled == 1) {
		BASS_ChannelSetAttribute(hStream, BASS_ATTRIB_VOL, (float)volume / 10000.0f);
	}
	else {
		sound_out_volume_float = (float)volume / 10000.0f;
		sound_out_volume_int = (int)(sound_out_volume_float * (float)0x1000);
	}
	//another cake
	int maxmidivoices = static_cast <int> (midivoices);
	float trackslimit = static_cast <int> (tracks);
	BASS_ChannelSetAttribute(hStream, BASS_ATTRIB_MIDI_CHANS, trackslimit);
	BASS_ChannelSetAttribute(hStream, BASS_ATTRIB_MIDI_VOICES, maxmidivoices);
	BASS_ChannelSetAttribute(hStream, BASS_ATTRIB_MIDI_CPU, maxcpu);
	if (noteoff1) {
		BASS_ChannelFlags(hStream, BASS_MIDI_NOTEOFF1, BASS_MIDI_NOTEOFF1);
	}
	else {
		BASS_ChannelFlags(hStream, 0, BASS_MIDI_NOTEOFF1);
	}
	if (vmsemu == 1) {
		BASS_SetConfig(BASS_CONFIG_UPDATEPERIOD, frames);
	}
	else {
		BASS_SetConfig(BASS_CONFIG_UPDATEPERIOD, 100);
	}
	if (nofx) {
		BASS_ChannelFlags(hStream, BASS_MIDI_NOFX, BASS_MIDI_NOFX);
	}
	else {
		BASS_ChannelFlags(hStream, 0, BASS_MIDI_NOFX);
	}
	if (sysresetignore) {
		BASS_ChannelFlags(hStream, BASS_MIDI_NOSYSRESET, BASS_MIDI_NOSYSRESET);
	}
	else {
		BASS_ChannelFlags(hStream, 0, BASS_MIDI_NOSYSRESET);
	}
	if (sinc) {
		BASS_ChannelFlags(hStream, BASS_MIDI_SINCINTER, BASS_MIDI_SINCINTER);
	}
	else {
		BASS_ChannelFlags(hStream, 0, BASS_MIDI_SINCINTER);
	}
}


void LoadSoundfont(DWORD whichsf){
	TCHAR config[MAX_PATH];
	BASS_MIDI_FONT * mf;
	FreeFonts(0);
	if (SUCCEEDED(SHGetFolderPath(NULL, CSIDL_PROFILE, NULL, 0, config)))
	{
		if (whichsf == 1) {
			PathAppend(config, _T("\\Keppy's Driver\\lists\\keppymidi.sflist"));
		}
		else if (whichsf == 2) {
			PathAppend(config, _T("\\Keppy's Driver\\lists\\keppymidib.sflist"));
		}
		else if (whichsf == 3) {
			PathAppend(config, _T("\\Keppy's Driver\\lists\\keppymidic.sflist"));
		}
		else if (whichsf == 4) {
			PathAppend(config, _T("\\Keppy's Driver\\lists\\keppymidid.sflist"));
		}
		else if (whichsf == 5) {
			PathAppend(config, _T("\\Keppy's Driver\\lists\\keppymidie.sflist"));
		}
		else if (whichsf == 6) {
			PathAppend(config, _T("\\Keppy's Driver\\lists\\keppymidif.sflist"));
		}
		else if (whichsf == 7) {
			PathAppend(config, _T("\\Keppy's Driver\\lists\\keppymidig.sflist"));
		}
		else if (whichsf == 8) {
			PathAppend(config, _T("\\Keppy's Driver\\lists\\keppymidih.sflist"));
		}
	}
	LoadFonts(0, config);
	BASS_MIDI_StreamLoadSamples(hStream);
}

void WatchdogCheck() 
{
	HKEY hKey;
	long lResult;
	DWORD dwType = REG_DWORD;
	DWORD dwSize = sizeof(DWORD);
	DWORD zero = 0;
	lResult = RegOpenKeyEx(HKEY_CURRENT_USER, L"Software\\Keppy's Driver\\Watchdog", 0, KEY_ALL_ACCESS, &hKey);
	RegQueryValueEx(hKey, L"rel1", NULL, &dwType, (LPBYTE)&rel1, &dwSize);
	RegQueryValueEx(hKey, L"rel2", NULL, &dwType, (LPBYTE)&rel2, &dwSize);
	RegQueryValueEx(hKey, L"rel3", NULL, &dwType, (LPBYTE)&rel3, &dwSize);
	RegQueryValueEx(hKey, L"rel4", NULL, &dwType, (LPBYTE)&rel4, &dwSize);
	RegQueryValueEx(hKey, L"rel5", NULL, &dwType, (LPBYTE)&rel5, &dwSize);
	RegQueryValueEx(hKey, L"rel6", NULL, &dwType, (LPBYTE)&rel6, &dwSize);
	RegQueryValueEx(hKey, L"rel7", NULL, &dwType, (LPBYTE)&rel7, &dwSize);
	RegQueryValueEx(hKey, L"rel8", NULL, &dwType, (LPBYTE)&rel8, &dwSize);
	if (rel1 == 1) {
		LoadSoundfont(1);
		RegSetValueEx(hKey, L"rel1", 0, dwType, (LPBYTE)&zero, sizeof(zero));
	}
	if (rel2 == 1) {
		LoadSoundfont(2);
		RegSetValueEx(hKey, L"rel2", 0, dwType, (LPBYTE)&zero, sizeof(zero));
	}
	if (rel3 == 1) {
		LoadSoundfont(3);
		RegSetValueEx(hKey, L"rel3", 0, dwType, (LPBYTE)&zero, sizeof(zero));
	}
	if (rel4 == 1) {
		LoadSoundfont(4);
		RegSetValueEx(hKey, L"rel4", 0, dwType, (LPBYTE)&zero, sizeof(zero));
	}
	if (rel5 == 1) {
		LoadSoundfont(5);
		RegSetValueEx(hKey, L"rel5", 0, dwType, (LPBYTE)&zero, sizeof(zero));
	}
	if (rel6 == 1) {
		LoadSoundfont(6);
		RegSetValueEx(hKey, L"rel6", 0, dwType, (LPBYTE)&zero, sizeof(zero));
	}
	if (rel7 == 1) {
		LoadSoundfont(7);
		RegSetValueEx(hKey, L"rel7", 0, dwType, (LPBYTE)&zero, sizeof(zero));
	}
	if (rel8 == 1) {
		LoadSoundfont(8);
		RegSetValueEx(hKey, L"rel8", 0, dwType, (LPBYTE)&zero, sizeof(zero));
	}
	RegCloseKey(hKey);
}

BOOL IsRunningXP(){
	if (xaudiodisabled == 1)
		return TRUE;
	return FALSE;
}

int AreEffectsDisabled(){
	long lResult;
	HKEY hKey;
	DWORD dwType = REG_DWORD;
	DWORD dwSize = sizeof(DWORD);
	lResult = RegOpenKeyEx(HKEY_CURRENT_USER, L"Software\\Keppy's Driver\\Settings", 0, KEY_ALL_ACCESS, &hKey);
	RegQueryValueEx(hKey, L"nofx", NULL, &dwType, (LPBYTE)&nofx, &dwSize);
	return nofx;
}

int IsNoteOff1TurnedOn(){
	long lResult;
	HKEY hKey;
	DWORD dwType = REG_DWORD;
	DWORD dwSize = sizeof(DWORD);
	lResult = RegOpenKeyEx(HKEY_CURRENT_USER, L"Software\\Keppy's Driver\\Settings", 0, KEY_ALL_ACCESS, &hKey);
	RegQueryValueEx(hKey, L"noteoff", NULL, &dwType, (LPBYTE)&noteoff1, &dwSize);
	return noteoff1;
}

int IgnoreSystemReset()
{
	HKEY hKey;
	long lResult;
	DWORD dwType = REG_DWORD;
	DWORD dwSize = sizeof(DWORD);
	lResult = RegOpenKeyEx(HKEY_CURRENT_USER, L"Software\\Keppy's Driver\\Settings", 0, KEY_ALL_ACCESS, &hKey);
	RegQueryValueEx(hKey, L"sysresetignore", NULL, &dwType, (LPBYTE)&sysresetignore, &dwSize);
	RegCloseKey(hKey);
	return sysresetignore;
}

int check_sinc()
{
	HKEY hKey;
	long lResult;
	DWORD dwType = REG_DWORD;
	DWORD dwSize = sizeof(DWORD);
	lResult = RegOpenKeyEx(HKEY_CURRENT_USER, L"Software\\Keppy's Driver\\Settings", 0, KEY_ALL_ACCESS, &hKey);
	RegQueryValueEx(hKey, L"sinc", NULL, &dwType, (LPBYTE)&sinc, &dwSize);
	RegCloseKey(hKey);
	return sinc;
}

void debug_info() {
	HKEY hKey;
	long lResult;
	DWORD dwType = REG_DWORD;
	DWORD dwSize = sizeof(DWORD);
	DWORD level, left, right;
	lResult = RegOpenKeyEx(HKEY_CURRENT_USER, L"Software\\Keppy's Driver", 0, KEY_ALL_ACCESS, &hKey);
	float currentvoices0;
	float currentcpuusage0;
	int tempo;
	BASS_ChannelGetAttribute(hStream, BASS_ATTRIB_MIDI_VOICES_ACTIVE, &currentvoices0);
	BASS_ChannelGetAttribute(hStream, BASS_ATTRIB_CPU, &currentcpuusage0);
	if (xaudiodisabled == 1) {
		if (volumemon == 1) {
			level = BASS_ChannelGetLevel(hStream);
			left = LOWORD(level); // the left level
			right = HIWORD(level); // the right level
			RegSetValueEx(hKey, L"leftvol", 0, dwType, (LPBYTE)&left, sizeof(left));
			RegSetValueEx(hKey, L"rightvol", 0, dwType, (LPBYTE)&right, sizeof(right));
		}
	}
	int currentvoicesint0 = int(currentvoices0);
	int currentcpuusageint0 = int(currentcpuusage0);
	// Things
	RegSetValueEx(hKey, L"currentvoices0", 0, dwType, (LPBYTE)&currentvoicesint0, sizeof(currentvoicesint0));
	RegSetValueEx(hKey, L"currentcpuusage0", 0, dwType, (LPBYTE)&currentcpuusageint0, sizeof(currentcpuusageint0));

	// OTHER THINGS
	RegSetValueEx(hKey, L"int", 0, dwType, (LPBYTE)&decoded, sizeof(decoded));
	RegCloseKey(hKey);
}

void mixervoid() {
	HKEY hKey;
	long lResult;
	DWORD dwType = REG_DWORD;
	DWORD dwSize = sizeof(DWORD);
	lResult = RegOpenKeyEx(HKEY_CURRENT_USER, L"Software\\Keppy's Driver\\Channels", 0, KEY_ALL_ACCESS, &hKey);
	RegQueryValueEx(hKey, L"ch1", NULL, &dwType, (LPBYTE)&ch1, &dwSize);
	RegQueryValueEx(hKey, L"ch2", NULL, &dwType, (LPBYTE)&ch2, &dwSize);
	RegQueryValueEx(hKey, L"ch3", NULL, &dwType, (LPBYTE)&ch3, &dwSize);
	RegQueryValueEx(hKey, L"ch4", NULL, &dwType, (LPBYTE)&ch4, &dwSize);
	RegQueryValueEx(hKey, L"ch5", NULL, &dwType, (LPBYTE)&ch5, &dwSize);
	RegQueryValueEx(hKey, L"ch6", NULL, &dwType, (LPBYTE)&ch6, &dwSize);
	RegQueryValueEx(hKey, L"ch7", NULL, &dwType, (LPBYTE)&ch7, &dwSize);
	RegQueryValueEx(hKey, L"ch8", NULL, &dwType, (LPBYTE)&ch8, &dwSize);
	RegQueryValueEx(hKey, L"ch9", NULL, &dwType, (LPBYTE)&ch9, &dwSize);
	RegQueryValueEx(hKey, L"ch10", NULL, &dwType, (LPBYTE)&ch10, &dwSize);
	RegQueryValueEx(hKey, L"ch11", NULL, &dwType, (LPBYTE)&ch11, &dwSize);
	RegQueryValueEx(hKey, L"ch12", NULL, &dwType, (LPBYTE)&ch12, &dwSize);
	RegQueryValueEx(hKey, L"ch13", NULL, &dwType, (LPBYTE)&ch13, &dwSize);
	RegQueryValueEx(hKey, L"ch14", NULL, &dwType, (LPBYTE)&ch14, &dwSize);
	RegQueryValueEx(hKey, L"ch15", NULL, &dwType, (LPBYTE)&ch15, &dwSize);
	RegQueryValueEx(hKey, L"ch16", NULL, &dwType, (LPBYTE)&ch16, &dwSize);
	if (midivolumeoverride == 1) {
		BASS_MIDI_StreamEvent(hStream, 0, MIDI_EVENT_VOLUME, ch1);
		BASS_MIDI_StreamEvent(hStream, 1, MIDI_EVENT_VOLUME, ch2);
		BASS_MIDI_StreamEvent(hStream, 2, MIDI_EVENT_VOLUME, ch3);
		BASS_MIDI_StreamEvent(hStream, 3, MIDI_EVENT_VOLUME, ch4);
		BASS_MIDI_StreamEvent(hStream, 4, MIDI_EVENT_VOLUME, ch5);
		BASS_MIDI_StreamEvent(hStream, 5, MIDI_EVENT_VOLUME, ch6);
		BASS_MIDI_StreamEvent(hStream, 6, MIDI_EVENT_VOLUME, ch7);
		BASS_MIDI_StreamEvent(hStream, 7, MIDI_EVENT_VOLUME, ch8);
		BASS_MIDI_StreamEvent(hStream, 8, MIDI_EVENT_VOLUME, ch9);
		BASS_MIDI_StreamEvent(hStream, 9, MIDI_EVENT_VOLUME, ch10);
		BASS_MIDI_StreamEvent(hStream, 10, MIDI_EVENT_VOLUME, ch11);
		BASS_MIDI_StreamEvent(hStream, 11, MIDI_EVENT_VOLUME, ch12);
		BASS_MIDI_StreamEvent(hStream, 12, MIDI_EVENT_VOLUME, ch13);
		BASS_MIDI_StreamEvent(hStream, 13, MIDI_EVENT_VOLUME, ch14);
		BASS_MIDI_StreamEvent(hStream, 14, MIDI_EVENT_VOLUME, ch15);
		BASS_MIDI_StreamEvent(hStream, 15, MIDI_EVENT_VOLUME, ch16);
	}
	else {
		if (ch1 != tch1) {
			BASS_MIDI_StreamEvent(hStream, 0, MIDI_EVENT_VOLUME, ch1);
			tch1 = ch1;
		}
		if (ch2 != tch2) {
			BASS_MIDI_StreamEvent(hStream, 1, MIDI_EVENT_VOLUME, ch2);
			tch2 = ch2;
		}
		if (ch3 != tch3) {
			BASS_MIDI_StreamEvent(hStream, 2, MIDI_EVENT_VOLUME, ch3);
			tch3 = ch3;
		}
		if (ch4 != tch4) {
			BASS_MIDI_StreamEvent(hStream, 3, MIDI_EVENT_VOLUME, ch4);
			tch4 = ch4;
		}
		if (ch5 != tch5) {
			BASS_MIDI_StreamEvent(hStream, 4, MIDI_EVENT_VOLUME, ch5);
			tch5 = ch5;
		}
		if (ch6 != tch6) {
			BASS_MIDI_StreamEvent(hStream, 5, MIDI_EVENT_VOLUME, ch6);
			tch6 = ch6;
		}
		if (ch7 != tch7) {
			BASS_MIDI_StreamEvent(hStream, 6, MIDI_EVENT_VOLUME, ch7);
			tch7 = ch7;
		}
		if (ch8 != tch8) {
			BASS_MIDI_StreamEvent(hStream, 7, MIDI_EVENT_VOLUME, ch8);
			tch8 = ch8;
		}
		if (ch9 != tch9) {
			BASS_MIDI_StreamEvent(hStream, 8, MIDI_EVENT_VOLUME, ch9);
			tch9 = ch9;
		}
		if (ch10 != tch10) {
			BASS_MIDI_StreamEvent(hStream, 9, MIDI_EVENT_VOLUME, ch10);
			tch10 = ch10;
		}
		if (ch11 != tch11) {
			BASS_MIDI_StreamEvent(hStream, 10, MIDI_EVENT_VOLUME, ch11);
			tch11 = ch11;
		}
		if (ch12 != tch12) {
			BASS_MIDI_StreamEvent(hStream, 11, MIDI_EVENT_VOLUME, ch12);
			tch12 = ch12;
		}
		if (ch13 != tch13) {
			BASS_MIDI_StreamEvent(hStream, 12, MIDI_EVENT_VOLUME, ch13);
			tch13 = ch13;
		}
		if (ch14 != tch14) {
			BASS_MIDI_StreamEvent(hStream, 13, MIDI_EVENT_VOLUME, ch14);
			tch14 = ch14;
		}
		if (ch15 != tch15) {
			BASS_MIDI_StreamEvent(hStream, 14, MIDI_EVENT_VOLUME, ch15);
			tch15 = ch15;
		}
		if (ch16 != tch16) {
			BASS_MIDI_StreamEvent(hStream, 15, MIDI_EVENT_VOLUME, ch16);
			tch16 = ch16;
		}
	}
}

void Volume(char* UpOrDown) {
	HKEY hKey;
	long lResult;
	DWORD dwType = REG_DWORD;
	DWORD dwSize = sizeof(DWORD);
	lResult = RegOpenKeyEx(HKEY_CURRENT_USER, L"Software\\Keppy's Driver\\Settings", 0, KEY_ALL_ACCESS, &hKey);
	if (UpOrDown == "UP") {
		int tempvolume = volume + 100;
		if (tempvolume < 0) {
			volume = 0;
			tempvolume = 0;
			RegSetValueEx(hKey, L"volume", 0, dwType, (LPBYTE)&tempvolume, sizeof(tempvolume));
		}
		else if (tempvolume > 10000) {
			volume = 10000;
			tempvolume = 10000;
			RegSetValueEx(hKey, L"volume", 0, dwType, (LPBYTE)&tempvolume, sizeof(tempvolume));
		}
		else {
			RegSetValueEx(hKey, L"volume", 0, dwType, (LPBYTE)&tempvolume, sizeof(tempvolume));
		}
	}
	else if (UpOrDown == "DOWN") {
		int tempvolume = volume - 100;
		if (tempvolume < 0) {
			volume = 0;
			tempvolume = 0;
			RegSetValueEx(hKey, L"volume", 0, dwType, (LPBYTE)&tempvolume, sizeof(tempvolume));
		}
		else if (tempvolume > 10000) {
			volume = 10000;
			tempvolume = 10000;
			RegSetValueEx(hKey, L"volume", 0, dwType, (LPBYTE)&tempvolume, sizeof(tempvolume));
		}
		else {
			RegSetValueEx(hKey, L"volume", 0, dwType, (LPBYTE)&tempvolume, sizeof(tempvolume));
		}
	}
}

BOOL ProcessBlackList(){
	// Blacklist system init
	TCHAR defaultstring[MAX_PATH];
	TCHAR userstring[MAX_PATH];
	TCHAR defaultblacklistdirectory[MAX_PATH];
	TCHAR userblacklistdirectory[MAX_PATH];
	TCHAR modulename[MAX_PATH];
	// VirtualMIDISynth 1.x ban init
	TCHAR vmidisynthpath[MAX_PATH];
	SHGetFolderPath(NULL, CSIDL_SYSTEM, NULL, 0, vmidisynthpath);
	PathAppend(vmidisynthpath, _T("\\VirtualMIDISynth\\VirtualMIDISynth.dll"));
	GetModuleFileName(NULL, modulename, MAX_PATH);
	PathStripPath(modulename);
	try {
		if (PathFileExists(vmidisynthpath)) {
			MessageBox(NULL, L"Please uninstall VirtualMIDISynth 1.x before using this driver.\n\nWhy this? Well, VirtualMIDISynth 1.x causes a DLL Hell while loading the BASS libraries, that's why you need to uninstall it before using my driver.\n\nYou can still use VirtualMIDISynth 2.x, since it doesn't load the DLLs directly into the MIDI application.", L"Keppy's Driver", MB_OK | MB_ICONERROR | MB_SYSTEMMODAL);
			return 0x0;
		}
		else {
			if (GetWindowsDirectory(defaultblacklistdirectory, MAX_PATH)) {
				_tcscat(defaultblacklistdirectory, L"\\keppymididrv.defaultblacklist");
				std::wifstream file(defaultblacklistdirectory);
				if (file) {
					// The default blacklist exists, continue
					OutputDebugString(defaultblacklistdirectory);
					while (file.getline(defaultstring, sizeof(defaultstring) / sizeof(*defaultstring)))
					{
						if (_tcsicmp(modulename, defaultstring) == 0) {
							return 0x0;
						}
					}
				}
				else {
					MessageBox(NULL, L"The default blacklist is missing, or the driver is not installed properly!\nFatal error, can not continue!\n\nPress OK to quit.", L"Keppy's MIDI Driver - FATAL ERROR", MB_OK | MB_ICONERROR | MB_SYSTEMMODAL);
					exit(0);
				}
			}
			if (SUCCEEDED(SHGetFolderPath(NULL, CSIDL_LOCAL_APPDATA, NULL, 0, userblacklistdirectory))) {
				PathAppend(userblacklistdirectory, _T("\\Keppy's Driver\\blacklist\\keppymididrv.blacklist"));
				std::wifstream file(userblacklistdirectory);
				OutputDebugString(userblacklistdirectory);
				while (file.getline(userstring, sizeof(userstring) / sizeof(*userstring)))
				{
					if (_tcsicmp(modulename, userstring) == 0) {
						std::wstring modulenamelpcwstr(modulename);
						std::wstring concatted_stdstr = L"Keppy's Driver - " + modulenamelpcwstr + L" is blacklisted";
						LPCWSTR messageboxtitle = concatted_stdstr.c_str();
						MessageBox(NULL, L"This program has been manually blacklisted.\n\nThe driver will be automatically unloaded by WinMM.", messageboxtitle, MB_OK | MB_ICONEXCLAMATION | MB_SYSTEMMODAL);
						return 0x0;
					}
				}
			}
			return 0x1;
		}
	}
	catch (std::exception & e) {
		OutputDebugStringA(e.what());
		exit;
	}
}

void ResetSynth(){
	BASS_MIDI_StreamEvent(hStream, 0, MIDI_EVENT_SYSTEMEX, MIDI_SYSTEM_DEFAULT);
}

void ReloadSFList(DWORD whichsflist){
	std::wstringstream ss;
	ss << "Do you want to (re)load list n°" << whichsflist << "?";
	std::wstring s = ss.str();
	ResetSynth();
	Sleep(100);
	if (sfdisableconf == 1) {
		LoadSoundfont(whichsflist);
	}
	else {
		const int result = MessageBox(NULL, s.c_str(), L"Keppy's Driver", MB_ICONINFORMATION | MB_YESNO | MB_SYSTEMMODAL);
		switch (result)
		{
		case IDYES:
			LoadSoundfont(whichsflist);
			break;
		case IDNO:
			break;
		}
	}
}

void keybindings()
{
	if (allhotkeys == 1) {
		if (GetAsyncKeyState(VK_MENU) & GetAsyncKeyState(0x31) & 0x8000) {
			ReloadSFList(1);
			return;
		}
		else if (GetAsyncKeyState(VK_MENU) & GetAsyncKeyState(0x32) & 0x8000) {
			ReloadSFList(2);
			return;
		}
		else if (GetAsyncKeyState(VK_MENU) & GetAsyncKeyState(0x33) & 0x8000) {
			ReloadSFList(3);
			return;
		}
		else if (GetAsyncKeyState(VK_MENU) & GetAsyncKeyState(0x34) & 0x8000) {
			ReloadSFList(4);
			return;
		}
		else if (GetAsyncKeyState(VK_MENU) & GetAsyncKeyState(0x35) & 0x8000) {
			ReloadSFList(5);
			return;
		}
		else if (GetAsyncKeyState(VK_MENU) & GetAsyncKeyState(0x36) & 0x8000) {
			ReloadSFList(6);
			return;
		}
		else if (GetAsyncKeyState(VK_MENU) & GetAsyncKeyState(0x37) & 0x8000) {
			ReloadSFList(7);
			return;
		}
		else if (GetAsyncKeyState(VK_MENU) & GetAsyncKeyState(0x38) & 0x8000) {
			ReloadSFList(8);
			return;
		}
		else if (GetAsyncKeyState(VK_MENU) & GetAsyncKeyState(0x39) & 0x8000) {
			TCHAR configuratorapp[MAX_PATH];
			if (SUCCEEDED(SHGetFolderPath(NULL, CSIDL_SYSTEMX86, NULL, 0, configuratorapp)))
			{
				PathAppend(configuratorapp, _T("\\keppydrv\\KeppyDriverConfigurator.exe"));
				ShellExecute(NULL, L"open", configuratorapp, NULL, NULL, SW_SHOWNORMAL);
				return;
			}
		}
		else if (GetAsyncKeyState(VK_MENU) & GetAsyncKeyState(0x30) & 0x8000) {
			TCHAR configuratorapp[MAX_PATH];
			if (SUCCEEDED(SHGetFolderPath(NULL, CSIDL_SYSTEMX86, NULL, 0, configuratorapp)))
			{
				PathAppend(configuratorapp, _T("\\keppydrv\\KeppyDriverConfigurator.exe"));
				ShellExecute(NULL, L"open", configuratorapp, L"/AT", NULL, SW_SHOWNORMAL);
				return;
			}
		}
		if (GetAsyncKeyState(VK_INSERT) & 1) {
			ResetSynth();
		}
		if (volumehotkeys == 1) {
			if (GetAsyncKeyState(VK_SUBTRACT) & 1) {
				Volume("DOWN");
			}
			else if (GetAsyncKeyState(VK_ADD) & 1) {
				Volume("UP");
			}
		}
		else {
			// Nothing lel
		}
	}
}

BOOL BannedSystemProcess() {
	// These processes are PERMANENTLY banned because of some internal bugs inside them.
	TCHAR modulename[MAX_PATH];
	TCHAR bannedconsent[MAX_PATH];
	TCHAR bannedexplorer[MAX_PATH];
	TCHAR bannedcsrss[MAX_PATH];
	TCHAR bannedscratch[MAX_PATH];
	_tcscpy_s(bannedconsent, _countof(bannedconsent), _T("consent.exe"));
	_tcscpy_s(bannedexplorer, _countof(bannedexplorer), _T("explorer.exe"));
	_tcscpy_s(bannedcsrss, _countof(bannedcsrss), _T("csrss.exe"));
	_tcscpy_s(bannedcsrss, _countof(bannedscratch), _T("scratch.exe"));
	GetModuleFileName(NULL, modulename, MAX_PATH);
	PathStripPath(modulename);
	if (!_tcsicmp(modulename, bannedconsent) | !_tcsicmp(modulename, bannedexplorer) | !_tcsicmp(modulename, bannedcsrss) | !_tcsicmp(modulename, bannedscratch)) {
		return TRUE;
		// It's a blacklisted process, so it can NOT create a BASS audio stream.
	}
	else {
		return FALSE;
		// It's not a blacklisted process, so it can create a BASS audio stream.
	}
}

unsigned __stdcall threadfunc(LPVOID lpV){
	if (BannedSystemProcess() == TRUE) {
		_endthreadex(0);
		return 0;
	}
	else {
		unsigned i;
		int pot;
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
			float trackslimit = static_cast <int> (tracks);
			int maxmidivoices = static_cast <int> (midivoices);
			int frequencyvalue = static_cast <int> (frequency);
			BASS_SetConfig(BASS_CONFIG_MIDI_VOICES, 9999);
			OutputDebugString(L"Initializing the stream...");
			if (BASS_Init(bassoutputfinal, frequencyvalue, BASS_DEVICE_LATENCY, NULL, NULL)) {
				if (bassoutputfinal == -1) {
					BASS_GetInfo(&info);
					if (vmsemu == 1) {
						BASS_SetConfig(BASS_CONFIG_UPDATEPERIOD, 5);
						BASS_SetConfig(BASS_CONFIG_UPDATETHREADS, 4);
						BASS_SetConfig(BASS_CONFIG_BUFFER, info.minbuf + frames); // default buffer size = 'minbuf' + additional buffer size
					}
					else {
						BASS_SetConfig(BASS_CONFIG_BUFFER, info.minbuf + 10); // default buffer size
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
				BASS_ChannelSetAttribute(hStream, BASS_ATTRIB_MIDI_CHANS, trackslimit);
				BASS_ChannelSetAttribute(hStream, BASS_ATTRIB_MIDI_VOICES, maxmidivoices);
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
			bmsyn_play_some_data();
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
	KillWatchdog();
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
	MIDIHDR *IIMidiHdr;
	UINT evbpoint;
	struct Driver *driver = &drivers[uDeviceID];
	int exlen = 0;
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
		IIMidiHdr = (MIDIHDR *)dwParam1;
		if (!(IIMidiHdr->dwFlags & MHDR_PREPARED)) return MIDIERR_UNPREPARED;
		IIMidiHdr->dwFlags &= ~MHDR_DONE;
		IIMidiHdr->dwFlags |= MHDR_INQUEUE;
		exlen = (int)IIMidiHdr->dwBufferLength;
		if (NULL == (sysexbuffer = (unsigned char *)malloc(exlen * sizeof(char)))){
			return MMSYSERR_NOMEM;
		}
		else{
			memcpy(sysexbuffer, IIMidiHdr->lpData, exlen);
#ifdef DEBUG
			FILE * logfile;
			logfile = fopen("c:\\dbglog.log", "at");
			if (logfile != NULL) {
				fprintf(logfile, "sysex %d byete\n", exlen);
				for (int i = 0; i < exlen; i++)
					fprintf(logfile, "%x ", sysexbuffer[i]);
				fprintf(logfile, "\n");
			}
			fclose(logfile);
#endif
		}
		/*
		TODO: 	When the buffer contents have been sent, the driver should set the MHDR_DONE flag, clear the
		MHDR_INQUEUE flag, and send the client a MOM_DONE callback message.


		In other words, these three lines should be done when the evbuf[evbpoint] is sent.
		*/
		IIMidiHdr->dwFlags &= ~MHDR_INQUEUE;
		IIMidiHdr->dwFlags |= MHDR_DONE;
		DoCallback(uDeviceID, static_cast<LONG>(dwUser), MOM_DONE, dwParam1, 0);
		//fallthrough
	case MODM_DATA:
		EnterCriticalSection(&mim_section);
		evbpoint = evbwpoint;
		if (++evbwpoint >= EVBUFF_SIZE)
			evbwpoint -= EVBUFF_SIZE;
		evbuf[evbpoint].uDeviceID = uDeviceID;
		evbuf[evbpoint].uMsg = uMsg;
		evbuf[evbpoint].dwParam1 = dwParam1;
		evbuf[evbpoint].dwParam2 = dwParam2;
		evbuf[evbpoint].exlen = exlen;
		evbuf[evbpoint].sysexbuffer = sysexbuffer;
		LeaveCriticalSection(&mim_section);
		if (InterlockedIncrement(&evbcount) >= EVBUFF_SIZE) {
			do
			{

			} while (evbcount >= EVBUFF_SIZE);
		}
		return MMSYSERR_NOERROR;
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