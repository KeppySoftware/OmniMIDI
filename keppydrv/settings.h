/*
Keppy's Driver settings loading system
*/



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

// Other
static int buffull = 0;

struct evbuf_t{
	UINT   uDeviceID;
	UINT   uMsg;
	DWORD_PTR	dwParam1;
	DWORD_PTR	dwParam2;
	int exlen;
	unsigned char *sysexbuffer;
};

static struct evbuf_t evbuf[32768];
static UINT  evbwpoint = 0;
static UINT  evbrpoint = 0;
static volatile LONG evbcount = 0;
static UINT evbsysexpoint;

void crashhandler(int e) {
	std::wstring s = std::to_wstring(e);
	std::wstring stemp = L"Fatal error during the execution of the driver!\n\nError code: " + s;
	const WCHAR * text = stemp.c_str();
	MessageBox(NULL, text, L"Keppy's Driver - Fatal execution error", MB_ICONERROR | MB_SYSTEMMODAL);
	exit(0);
}

void DLLLoadError(LPCWSTR dll) {
	TCHAR errormessage[MAX_PATH] = L"There was an error while trying to load the DLL for the driver!\nFaulty/missing DLL: ";
	TCHAR clickokmsg[MAX_PATH] = L"\n\nClick OK to close the program.";
	lstrcat(errormessage, dll);
	lstrcat(errormessage, clickokmsg);
	MessageBox(NULL, errormessage, L"Keppy's Driver - DLL load error", MB_ICONERROR | MB_SYSTEMMODAL);
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
	RegCloseKey(hKey);
	return nofx;
}

int IsNoteOff1TurnedOn(){
	long lResult;
	HKEY hKey;
	DWORD dwType = REG_DWORD;
	DWORD dwSize = sizeof(DWORD);
	lResult = RegOpenKeyEx(HKEY_CURRENT_USER, L"Software\\Keppy's Driver\\Settings", 0, KEY_ALL_ACCESS, &hKey);
	RegQueryValueEx(hKey, L"noteoff", NULL, &dwType, (LPBYTE)&noteoff1, &dwSize);
	RegCloseKey(hKey);
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

void LoadSoundfont(int whichsf){
	try {
		TCHAR config[MAX_PATH];
		BASS_MIDI_FONT * mf;
		HKEY hKey;
		long lResult;
		DWORD dwType = REG_DWORD;
		DWORD dwSize = sizeof(DWORD);
		lResult = RegOpenKeyEx(HKEY_CURRENT_USER, L"Software\\Keppy's Driver\\Watchdog", 0, KEY_ALL_ACCESS, &hKey);
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
			RegSetValueEx(hKey, L"currentsflist", 0, dwType, (LPBYTE)&whichsf, sizeof(whichsf));
			RegCloseKey(hKey);
		}
		RegCloseKey(hKey);
		LoadFonts(0, config);
		BASS_MIDI_StreamLoadSamples(hStream);
	}
	catch (int e) {
		crashhandler(e);
	}
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
	LOADBASSFUNCTION(BASS_ChannelStop);
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

void load_settings()
{
	try {
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
		RegQueryValueEx(hKey, L"newevbuffvalue", NULL, &dwType, (LPBYTE)&newevbuffvalue, &dwSize);
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

		frequencynew = frequency;

		sndbf = (float *)malloc(newsndbfvalue*sizeof(float));
		memset(evbuf, newevbuffvalue, sizeof(newevbuffvalue));

		sound_out_volume_float = (float)volume / 10000.0f;
		sound_out_volume_int = (int)(sound_out_volume_float * (float)0x1000);
	}
	catch (int e) {
		crashhandler(e);
	}
}

void realtime_load_settings()
{
	try {
		HKEY hKey;
		long lResult;
		DWORD dwType = REG_DWORD;
		DWORD dwSize = sizeof(DWORD);
		lResult = RegOpenKeyEx(HKEY_CURRENT_USER, L"Software\\Keppy's Driver\\Settings", 0, KEY_ALL_ACCESS, &hKey);
		RegQueryValueEx(hKey, L"allhotkeys", NULL, &dwType, (LPBYTE)&allhotkeys, &dwSize);
		RegQueryValueEx(hKey, L"cpu", NULL, &dwType, (LPBYTE)&maxcpu, &dwSize);
		RegQueryValueEx(hKey, L"midivolumeoverride", NULL, &dwType, (LPBYTE)&midivolumeoverride, &dwSize);
		RegQueryValueEx(hKey, L"nofx", NULL, &dwType, (LPBYTE)&nofx, &dwSize);
		RegQueryValueEx(hKey, L"noteoff", NULL, &dwType, (LPBYTE)&noteoff1, &dwSize);
		RegQueryValueEx(hKey, L"polyphony", NULL, &dwType, (LPBYTE)&midivoices, &dwSize);
		RegQueryValueEx(hKey, L"preload", NULL, &dwType, (LPBYTE)&preload, &dwSize);
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
		BASS_MIDI_StreamEvent(hStream, 9, MIDI_EVENT_DRUMS, 1);
		BASS_ChannelSetAttribute(hStream, BASS_ATTRIB_MIDI_CHANS, trackslimit);
		BASS_ChannelSetAttribute(hStream, BASS_ATTRIB_MIDI_VOICES, maxmidivoices);
		BASS_ChannelSetAttribute(hStream, BASS_ATTRIB_MIDI_CPU, maxcpu);
		if (noteoff1) {
			BASS_ChannelFlags(hStream, BASS_MIDI_NOTEOFF1, BASS_MIDI_NOTEOFF1);
		}
		else {
			BASS_ChannelFlags(hStream, 0, BASS_MIDI_NOTEOFF1);
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
	catch (int e) {
		crashhandler(e);
	}
}

void WatchdogCheck()
{
	try {
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
	catch (int e) {
		crashhandler(e);
	}
}

void debug_info() {
	try {
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
		RegSetValueEx(hKey, L"buffull", 0, dwType, (LPBYTE)&buffull, sizeof(buffull));

		// OTHER THINGS
		RegSetValueEx(hKey, L"int", 0, dwType, (LPBYTE)&decoded, sizeof(decoded));
		RegCloseKey(hKey);
	}
	catch (int e) {
		crashhandler(e);
	}
}

void mixervoid() {
	try {
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
		RegCloseKey(hKey);
	}
	catch (int e) {
		crashhandler(e);
	}
}

void Volume(char* UpOrDown) {
	try {
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
		RegCloseKey(hKey);
	}
	catch (int e) {
		crashhandler(e);
	}
}

void ResetSynth(){
	BASS_MIDI_StreamEvent(hStream, 0, MIDI_EVENT_SYSTEMEX, MIDI_SYSTEM_DEFAULT);
}

void ReloadSFList(DWORD whichsflist){
	try {
		if (xaudiodisabled == 1) {
			BASS_ChannelSetAttribute(hStream, BASS_ATTRIB_VOL, 0.0f / 10000.0f);
		}
		else {
			sound_out_volume_float = 0.0f / 10000.0f;
		}
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
		if (xaudiodisabled == 1) {
			BASS_ChannelSetAttribute(hStream, BASS_ATTRIB_VOL, (float)volume / 10000.0f);
		}
		else {
			sound_out_volume_float = (float)volume / 10000.0f;
		}
	}
	catch (int e) {
		crashhandler(e);
	}
}

void keybindings()
{
	try {
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
				BOOL run = TRUE;
				if (SUCCEEDED(SHGetFolderPath(NULL, CSIDL_SYSTEMX86, NULL, 0, configuratorapp)))
				{
					PathAppend(configuratorapp, _T("\\keppydrv\\KeppyDriverConfigurator.exe"));
					ShellExecute(NULL, L"open", configuratorapp, NULL, NULL, SW_SHOWNORMAL);
					Sleep(10);
					return;
				}
			}
			else if (GetAsyncKeyState(VK_MENU) & GetAsyncKeyState(0x30) & 0x8000) {
				TCHAR configuratorapp[MAX_PATH];
				BOOL run = TRUE;
				if (SUCCEEDED(SHGetFolderPath(NULL, CSIDL_SYSTEMX86, NULL, 0, configuratorapp)))
				{
					PathAppend(configuratorapp, _T("\\keppydrv\\KeppyDriverConfigurator.exe"));
					ShellExecute(NULL, L"open", configuratorapp, L"/AT", NULL, SW_SHOWNORMAL);
					Sleep(10);
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
	catch (int e) {
		crashhandler(e);
	}
}