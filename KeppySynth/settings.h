/*
Keppy's Synthesizer settings loading system
*/

struct evbuf_t{
	UINT   uMsg;
	DWORD_PTR	dwParam1;
	DWORD_PTR	dwParam2;
	int exlen;
	unsigned char *sysexbuffer;
};

static struct evbuf_t * evbuf;
static long long evbwpoint = 0;
static long long evbrpoint = 0;
static volatile long long evbcount = 0;

void DLLLoadError(LPCWSTR dll) {
	TCHAR errormessage[MAX_PATH] = L"There was an error while trying to load the DLL for the driver!\nFaulty/missing DLL: ";
	TCHAR clickokmsg[MAX_PATH] = L"\n\nClick OK to close the program.";
	lstrcat(errormessage, dll);
	lstrcat(errormessage, clickokmsg);
	SetConsoleTextAttribute(hConsole, FOREGROUND_RED);
	std::cout << "(Invalid DLL: " << dll << ") " << " - Fatal error during the loading process of the following DLL." << std::endl;

	const int result = MessageBox(NULL, errormessage, L"Keppy's Synthesizer - DLL load error", MB_ICONERROR | MB_SYSTEMMODAL);
	switch (result)
	{
	default:
		exit(0);
		return;
	}
}

void DLLLoadErrorVST(LPCWSTR dll) {
	SetConsoleTextAttribute(hConsole, FOREGROUND_RED);
	std::cout << "(Invalid DLL: " << dll << ") " << " - The required runtime library is missing!" << std::endl;

	MessageBox(NULL,
		L"An error has occurred during the initialization of BASS_VST!\nPlease install Visual C++ 2010 to get rid of this error!\n\nClick OK to continue.",
		L"Keppy's Synthesizer - DLL load error", MB_ICONERROR | MB_SYSTEMMODAL);
}

long long TimeNow() {
	static LARGE_INTEGER s_frequency;
	static BOOL s_use_qpc = QueryPerformanceFrequency(&s_frequency);
	if (s_use_qpc) {
		LARGE_INTEGER now;
		QueryPerformanceCounter(&now);
		return (1000LL * now.QuadPart) / s_frequency.QuadPart;
	}
	else {
		return GetTickCount();
	}
}

void CopyToClipboard(const std::string &s) {
	OpenClipboard(0);
	EmptyClipboard();
	HGLOBAL hg = GlobalAlloc(GMEM_MOVEABLE, s.size());
	if (!hg) {
		CloseClipboard();
		return;
	}
	memcpy(GlobalLock(hg), s.c_str(), s.size());
	GlobalUnlock(hg);
	SetClipboardData(CF_TEXT, hg);
	CloseClipboard();
	GlobalFree(hg);

}

void ResetSynth(int ischangingbuffermode){
	reset_synth = 1;
	if (ischangingbuffermode == 1) {
		evbwpoint = 0;
		evbrpoint = 0;
		evbcount = 0;
	}
	BASS_MIDI_StreamEvent(KSStream, 0, MIDI_EVENT_SYSTEMEX, MIDI_SYSTEM_DEFAULT);
	reset_synth = 0;
}

void LoadSoundfont(int whichsf){
	try {
		PrintToConsole(FOREGROUND_RED, whichsf, "Loading soundfont list...");
		TCHAR config[MAX_PATH];
		BASS_MIDI_FONT * mf;
		HKEY hKey;
		long lResult;
		DWORD dwType = REG_DWORD;
		DWORD dwSize = sizeof(DWORD);
		lResult = RegOpenKeyEx(HKEY_CURRENT_USER, L"Software\\Keppy's Synthesizer\\Watchdog", 0, KEY_ALL_ACCESS, &hKey);
		FreeFonts(0);
		RegSetValueEx(hKey, L"currentsflist", 0, dwType, (LPBYTE)&whichsf, sizeof(whichsf));
		RegCloseKey(hKey);
		LoadFonts(0, sflistloadme[whichsf - 1]);
		BASS_MIDI_StreamLoadSamples(KSStream);
		PrintToConsole(FOREGROUND_RED, whichsf, "Done.");
	}
	catch (...) {
		CrashMessage(L"SFLoad");
		throw;
	}
}

bool LoadSoundfontStartup() {
	try {
		int done = 0;
		TCHAR modulename[MAX_PATH];
		TCHAR fullmodulename[MAX_PATH];
		GetModuleFileName(NULL, modulename, MAX_PATH);
		GetModuleFileName(NULL, fullmodulename, MAX_PATH);
		PathStripPath(modulename);

		for (int i = 0; i <= 15; ++i) {
			SHGetFolderPath(NULL, CSIDL_PROFILE, NULL, 0, listsloadme[i]);
			SHGetFolderPath(NULL, CSIDL_PROFILE, NULL, 0, sflistloadme[i]);
			_tcscat(sflistloadme[i], sfdirs[i]);
			_tcscat(listsloadme[i], listsanalyze[i]);
			std::wifstream file(listsloadme[i]);
			if (file) {
				TCHAR defaultstring[MAX_PATH];
				while (file.getline(defaultstring, sizeof(defaultstring) / sizeof(*defaultstring)))
				{
					if (_tcsicmp(modulename, defaultstring) && _tcsicmp(fullmodulename, defaultstring) == 0) {
						LoadSoundfont(i + 1);
						done = 1;
						PrintToConsole(FOREGROUND_RED, i, "Found it");
					}
				}
			}
			file.close();
		}

		if (done == 1) {
			return TRUE;
		}
		else {
			return FALSE;
		}
	}
	catch (...) {
		CrashMessage(L"SFLoadStartup");
		throw;
	}
}

BOOL load_bassfuncs()
{
	try {
		TCHAR installpath[MAX_PATH] = { 0 };
		TCHAR bassencpath[MAX_PATH] = { 0 };
		TCHAR bassencpathalt[MAX_PATH] = { 0 };
		TCHAR bassasiopath[MAX_PATH] = { 0 };
		TCHAR bassasiopathalt[MAX_PATH] = { 0 };
		TCHAR bassmidipath[MAX_PATH] = { 0 };
		TCHAR bassmidipathalt[MAX_PATH] = { 0 };
		TCHAR bassmixpath[MAX_PATH] = { 0 };
		TCHAR bassmixpathalt[MAX_PATH] = { 0 };
		TCHAR basspath[MAX_PATH] = { 0 };
		TCHAR basspathalt[MAX_PATH] = { 0 };
		TCHAR bassvstpath[MAX_PATH] = { 0 };
		TCHAR bassvstpathalt[MAX_PATH] = { 0 };
		TCHAR bassxapath[MAX_PATH] = { 0 };
		TCHAR bassxapathalt[MAX_PATH] = { 0 };
		TCHAR bassfxpath[MAX_PATH] = { 0 };
		TCHAR bassfxpathalt[MAX_PATH] = { 0 };

		int installpathlength;

		GetModuleFileName(hinst, installpath, MAX_PATH);
		PathRemoveFileSpec(installpath);

		SHGetFolderPath(NULL, CSIDL_PROFILE, NULL, 0, basspathalt);
		SHGetFolderPath(NULL, CSIDL_PROFILE, NULL, 0, bassmidipathalt);
		SHGetFolderPath(NULL, CSIDL_PROFILE, NULL, 0, bassencpathalt);
		SHGetFolderPath(NULL, CSIDL_PROFILE, NULL, 0, bassasiopathalt);
		SHGetFolderPath(NULL, CSIDL_PROFILE, NULL, 0, bassvstpathalt);
		SHGetFolderPath(NULL, CSIDL_PROFILE, NULL, 0, bassxapathalt);
		SHGetFolderPath(NULL, CSIDL_PROFILE, NULL, 0, bassfxpathalt);

#if defined(_WIN64)
		PathAppend(basspathalt, _T("\\Keppy's Synthesizer\\dlloverride\\64\\bass.dll"));
		PathAppend(bassmidipathalt, _T("\\Keppy's Synthesizer\\dlloverride\\64\\bassmidi.dll"));
		PathAppend(bassmixpathalt, _T("\\Keppy's Synthesizer\\dlloverride\\64\\bassmix.dll"));
		PathAppend(bassencpathalt, _T("\\Keppy's Synthesizer\\dlloverride\\64\\bassenc.dll"));
		PathAppend(bassasiopathalt, _T("\\Keppy's Synthesizer\\dlloverride\\64\\bassasio.dll"));
		PathAppend(bassvstpathalt, _T("\\Keppy's Synthesizer\\dlloverride\\64\\bass_vst.dll"));
		PathAppend(bassxapathalt, _T("\\Keppy's Synthesizer\\dlloverride\\64\\bassxa.dll"));
		PathAppend(bassfxpathalt, _T("\\Keppy's Synthesizer\\dlloverride\\64\\bass_fx.dll"));
#elif defined(_WIN32)
		PathAppend(basspathalt, _T("\\Keppy's Synthesizer\\dlloverride\\32\\bass.dll"));
		PathAppend(bassmidipathalt, _T("\\Keppy's Synthesizer\\dlloverride\\32\\bassmidi.dll"));
		PathAppend(bassmixpathalt, _T("\\Keppy's Synthesizer\\dlloverride\\64\\bassmix.dll"));
		PathAppend(bassencpathalt, _T("\\Keppy's Synthesizer\\dlloverride\\32\\bassenc.dll"));
		PathAppend(bassasiopathalt, _T("\\Keppy's Synthesizer\\dlloverride\\32\\bassasio.dll"));
		PathAppend(bassvstpathalt, _T("\\Keppy's Synthesizer\\dlloverride\\32\\bass_vst.dll"));
		PathAppend(bassxapathalt, _T("\\Keppy's Synthesizer\\dlloverride\\32\\bassxa.dll"));
		PathAppend(bassfxpathalt, _T("\\Keppy's Synthesizer\\dlloverride\\32\\bass_fx.dll"));
#endif

		SetConsoleTextAttribute(hConsole, FOREGROUND_RED);
		PrintToConsole(FOREGROUND_RED, 1, "Allocating memory for BASS DLLs...");

		// BASS
		if (PathFileExists(basspathalt)) {
			if (!(bass = LoadLibrary(basspathalt))) {
				DLLLoadError(basspathalt);
				exit(0);
			}
			isoverrideenabled = 1;
		}
		else {
			lstrcat(basspath, installpath);
			lstrcat(basspath, L"\\bass.dll");
			if (!(bass = LoadLibrary(basspath))) {
				DLLLoadError(basspath);
				exit(0);
			}
		}

		// BASSMIDI
		if (PathFileExists(bassmidipathalt)) {
			if (!(bassmidi = LoadLibrary(bassmidipathalt))) {
				DLLLoadError(bassmidipathalt);
				exit(0);
			}
			isoverrideenabled = 1;
		}
		else {
			lstrcat(bassmidipath, installpath);
			lstrcat(bassmidipath, L"\\bassmidi.dll");
			if (!(bassmidi = LoadLibrary(bassmidipath))) {
				DLLLoadError(bassmidipath);
				exit(0);
			}
		}

		/* 
		// BASSMix
		if (PathFileExists(bassmixpathalt)) {
			if (!(bassmix = LoadLibrary(bassmixpathalt))) {
				DLLLoadError(bassmixpathalt);
				exit(0);
			}
			isoverrideenabled = 1;
		}
		else {
			lstrcat(bassmixpath, installpath);
			lstrcat(bassmixpath, L"\\bassmix.dll");
			if (!(bassmix = LoadLibrary(bassmixpath))) {
				DLLLoadError(bassmixpath);
				exit(0);
			}
		} 
		*/

		// BASSenc
		if (PathFileExists(bassencpathalt)) {
			if (!(bassenc = LoadLibrary(bassencpathalt))) {
				DLLLoadError(bassencpathalt);
				exit(0);
			}
			isoverrideenabled = 1;
		}
		else {
			lstrcat(bassencpath, installpath);
			lstrcat(bassencpath, L"\\bassenc.dll");
			if (!(bassenc = LoadLibrary(bassencpath))) {
				DLLLoadError(bassencpath);
				exit(0);
			}
		}

		// BASSASIO
		if (PathFileExists(bassasiopathalt)) {
			if (!(bassasio = LoadLibrary(bassasiopathalt))) {
				DLLLoadError(bassasiopathalt);
				exit(0);
			}
			isoverrideenabled = 1;
		}
		else {
			lstrcat(bassasiopath, installpath);
			lstrcat(bassasiopath, L"\\bassasio.dll");
			if (!(bassasio = LoadLibrary(bassasiopath))) {
				DLLLoadError(bassasiopath);
				exit(0);
			}
		}

		// BASS_FX
		if (PathFileExists(bassfxpathalt)) {
			if (!(bass_fx = LoadLibrary(bassfxpathalt))) {
				DLLLoadError(bassfxpathalt);
				exit(0);
			}
			isoverrideenabled = 1;
		}
		else {
			lstrcat(bassfxpath, installpath);
			lstrcat(bassfxpath, L"\\bass_fx.dll");
			if (!(bass_fx = LoadLibrary(bassfxpath))) {
				DLLLoadError(bassfxpath);
				exit(0);
			}
		}

		// BASS_VST
		if (PathFileExists(bassvstpathalt)) {
			if (!(bass_vst = LoadLibrary(bassvstpathalt))) {
				isbassvstloaded = 0;
				DLLLoadErrorVST(bassvstpathalt);
			}
			else {
				isbassvstloaded = 1;
			}
			isoverrideenabled = 1;
		}
		else {
			lstrcat(bassvstpath, installpath);
			lstrcat(bassvstpath, L"\\bass_vst.dll");
			if (!(bass_vst = LoadLibrary(bassvstpath))) {
				isbassvstloaded = 0;
				DLLLoadErrorVST(bassvstpath);
			}
			else {
				isbassvstloaded = 1;
			}
		}

		PrintToConsole(FOREGROUND_RED, 1, "Done loading BASS DLLs.");

		/* "load" all the BASS functions that are to be used */
		PrintToConsole(FOREGROUND_RED, 1, "Loading BASS functions...");
		LOADBASSASIOFUNCTION(BASS_ASIO_ChannelEnable);
		LOADBASSASIOFUNCTION(BASS_ASIO_ChannelGetLevel);
		LOADBASSASIOFUNCTION(BASS_ASIO_ChannelJoin);
		LOADBASSASIOFUNCTION(BASS_ASIO_ChannelReset);
		LOADBASSASIOFUNCTION(BASS_ASIO_ChannelSetFormat);
		LOADBASSASIOFUNCTION(BASS_ASIO_ChannelSetRate);
		LOADBASSASIOFUNCTION(BASS_ASIO_ChannelSetVolume);
		LOADBASSASIOFUNCTION(BASS_ASIO_ChannelEnableMirror);
		LOADBASSASIOFUNCTION(BASS_ASIO_ControlPanel);
		LOADBASSASIOFUNCTION(BASS_ASIO_GetRate);
		LOADBASSASIOFUNCTION(BASS_ASIO_GetLatency);
		LOADBASSASIOFUNCTION(BASS_ASIO_ErrorGetCode);
		LOADBASSASIOFUNCTION(BASS_ASIO_Free);
		LOADBASSASIOFUNCTION(BASS_ASIO_GetCPU);
		LOADBASSASIOFUNCTION(BASS_ASIO_Init);
		LOADBASSASIOFUNCTION(BASS_ASIO_SetDSD);
		LOADBASSASIOFUNCTION(BASS_ASIO_SetRate);
		LOADBASSASIOFUNCTION(BASS_ASIO_Start);
		LOADBASSASIOFUNCTION(BASS_ASIO_Stop);
		LOADBASSENCFUNCTION(BASS_Encode_Start);
		LOADBASSENCFUNCTION(BASS_Encode_Stop);
		LOADBASSFUNCTION(BASS_ChannelFlags);
		LOADBASSFUNCTION(BASS_ChannelGetAttribute);
		LOADBASSFUNCTION(BASS_ChannelGetData);
		LOADBASSFUNCTION(BASS_ChannelGetLevelEx);
		LOADBASSFUNCTION(BASS_ChannelIsActive);
		LOADBASSFUNCTION(BASS_ChannelPlay);
		LOADBASSFUNCTION(BASS_ChannelRemoveFX);
		LOADBASSFUNCTION(BASS_ChannelSeconds2Bytes);
		LOADBASSFUNCTION(BASS_ChannelSetAttribute);
		LOADBASSFUNCTION(BASS_ChannelSetDevice);
		LOADBASSFUNCTION(BASS_ChannelSetFX);
		LOADBASSFUNCTION(BASS_ChannelSetSync);
		LOADBASSFUNCTION(BASS_ChannelStop);
		LOADBASSFUNCTION(BASS_ChannelUpdate);
		LOADBASSFUNCTION(BASS_Update);
		LOADBASSFUNCTION(BASS_ErrorGetCode);
		LOADBASSFUNCTION(BASS_Free);
		LOADBASSFUNCTION(BASS_Stop);
		LOADBASSFUNCTION(BASS_GetDevice);
		LOADBASSFUNCTION(BASS_GetDeviceInfo);
		LOADBASSFUNCTION(BASS_GetInfo);
		LOADBASSFUNCTION(BASS_Init);
		LOADBASSFUNCTION(BASS_PluginLoad);
		LOADBASSFUNCTION(BASS_SetConfig);
		LOADBASSFUNCTION(BASS_SetDevice);
		LOADBASSFUNCTION(BASS_SetVolume);
		LOADBASSFUNCTION(BASS_StreamFree);
		LOADBASSFUNCTION(BASS_FXSetParameters);
		LOADBASSMIDIFUNCTION(BASS_MIDI_FontFree);
		LOADBASSMIDIFUNCTION(BASS_MIDI_FontInit);
		LOADBASSMIDIFUNCTION(BASS_MIDI_FontLoad);
		LOADBASSMIDIFUNCTION(BASS_MIDI_StreamCreate);
		LOADBASSMIDIFUNCTION(BASS_MIDI_StreamEvent);
		LOADBASSMIDIFUNCTION(BASS_MIDI_StreamEvents);
		LOADBASSMIDIFUNCTION(BASS_MIDI_StreamGetEvent);
		LOADBASSMIDIFUNCTION(BASS_MIDI_StreamLoadSamples);
		LOADBASSMIDIFUNCTION(BASS_MIDI_StreamSetFonts);
		// LOADBASSMIXFUNCTION(BASS_Mixer_StreamCreate);
		// LOADBASSMIXFUNCTION(BASS_Mixer_StreamAddChannel);
		LOADBASS_FXFUNCTION(BASS_FX_TempoCreate);

		if (isbassvstloaded == 1) {
			LOADBASS_VSTFUNCTION(BASS_VST_ChannelSetDSP);
			LOADBASS_VSTFUNCTION(BASS_VST_ChannelFree);
			LOADBASS_VSTFUNCTION(BASS_VST_ChannelCreate);
			LOADBASS_VSTFUNCTION(BASS_VST_ProcessEvent);
			LOADBASS_VSTFUNCTION(BASS_VST_ProcessEventRaw);
		}
		PrintToConsole(FOREGROUND_RED, 1, "BASS functions succesfully loaded.");
		return TRUE;
	}
	catch (...) {
		CrashMessage(L"BASSDefLoad");
		throw;
	}
}

void AppName() {
	try {
		ZeroMemory(modulename, MAX_PATH * sizeof(char));
		ZeroMemory(bitapp, MAX_PATH * sizeof(char));
		GetModuleFileNameExA(GetCurrentProcess(), NULL, modulename, MAX_PATH);
#if defined(_WIN64)
		strcpy(bitapp, "64-bit");
#elif defined(_WIN32)
		strcpy(bitapp, "32-bit");
#endif
	}
	catch (...) {
		CrashMessage(L"AppNameWrite");
		throw;
	}
}

void allocate_memory() {
	try {
		PrintToConsole(FOREGROUND_BLUE, 1, "Allocating memory for EV buffer and audio buffer...");

		// EVBUFF
		MEMORYSTATUSEX status;
		status.dwLength = sizeof(status);
		GlobalMemoryStatusEx(&status);

		if (evbuffbyram == 1) {
			sevbuffsize = status.ullTotalPhys;
			if (evbuffratio == 1) evbuffratio = 128;
		}
		else {
			if (sevbuffsize > status.ullTotalPhys) sevbuffsize = status.ullTotalPhys;
		}

#if _WIN32 || _WIN64
#if _WIN64
		// Nothing
#else
		if (sevbuffsize > 2147483647) {
			PrintToConsole(FOREGROUND_BLUE, 1, "EV buffer is too big, limiting to 2GB...");
			sevbuffsize = 2147483647;
		}
#endif
#endif
		std::ostringstream st;
		std::ostringstream nd;
		std::ostringstream rd;

		PrintToConsole(FOREGROUND_BLUE, 1, "Calculating ratio...");
		evbuffsize = (unsigned long long)sevbuffsize / (unsigned long long)evbuffratio;

		st << "EV buffer size: " << sevbuffsize;
		nd << "EV buffer ratio: " << evbuffratio;
		rd << "EV buffer final size: " << evbuffsize;

		PrintToConsole(FOREGROUND_BLUE, 1, st.str().c_str());
		PrintToConsole(FOREGROUND_BLUE, 1, nd.str().c_str());
		PrintToConsole(FOREGROUND_BLUE, 1, rd.str().c_str());

		PrintToConsole(FOREGROUND_BLUE, 1, "Calculating ratio...");
		PrintToConsole(FOREGROUND_BLUE, 1, "Calculating ratio...");

		PrintToConsole(FOREGROUND_BLUE, 1, "Allocating EV buffer...");
		evbuf = (evbuf_t *)malloc((unsigned long long)evbuffsize * sizeof(evbuf_t));
		PrintToConsole(FOREGROUND_BLUE, 1, "Zeroing EV buffer...");
		memset(evbuf, 0, sizeof(evbuf_t));
		PrintToConsole(FOREGROUND_BLUE, 1, "EV buffer allocated.");
		// EVBUFF

		PrintToConsole(FOREGROUND_BLUE, 1, "Allocating audio buffer...");
		sndbf = (float *)malloc(newsndbfvalue * sizeof(float));
		PrintToConsole(FOREGROUND_BLUE, 1, "Audio buffer allocated.");
	}
	catch (...) {
		CrashMessage(L"MemAlloc");
		throw;
	}
}

void LoadSettings(bool streamreload)
{
	try {
		PrintToConsole(FOREGROUND_BLUE, 1, "Loading settings from registry...");
		int zero = 0;
		HKEY hKey;
		long lResult;
		DWORD dwType = REG_DWORD;
		DWORD dwSize = sizeof(DWORD);
		DWORD qwType = REG_QWORD;
		DWORD qwSize = sizeof(QWORD);
		lResult = RegOpenKeyEx(HKEY_CURRENT_USER, L"Software\\Keppy's Synthesizer\\Settings", 0, KEY_ALL_ACCESS, &hKey);
		if (!streamreload) {
			RegQueryValueEx(hKey, L"evbuffsize", NULL, &qwType, (LPBYTE)&sevbuffsize, &qwSize);
			RegQueryValueEx(hKey, L"evbuffbyram", NULL, &dwType, (LPBYTE)&evbuffbyram, &dwSize);
			RegQueryValueEx(hKey, L"evbuffratio", NULL, &dwType, (LPBYTE)&evbuffratio, &dwSize);
		}
		RegQueryValueEx(hKey, L"32bit", NULL, &dwType, (LPBYTE)&floatrendering, &dwSize);
		RegQueryValueEx(hKey, L"allhotkeys", NULL, &dwType, (LPBYTE)&allhotkeys, &dwSize);
		RegQueryValueEx(hKey, L"allnotesignore", NULL, &dwType, (LPBYTE)&allnotesignore, &dwSize);
		RegQueryValueEx(hKey, L"alternativecpu", NULL, &dwType, (LPBYTE)&autopanic, &dwSize);
		RegQueryValueEx(hKey, L"buflen", NULL, &dwType, (LPBYTE)&frames, &dwSize);
		RegQueryValueEx(hKey, L"capframerate", NULL, &dwType, (LPBYTE)&capframerate, &dwSize);
		RegQueryValueEx(hKey, L"cpu", NULL, &dwType, (LPBYTE)&maxcpu, &dwSize);
		RegQueryValueEx(hKey, L"defaultAdev", NULL, &dwType, (LPBYTE)&defaultAoutput, &dwSize);
		RegQueryValueEx(hKey, L"defaultWdev", NULL, &dwType, (LPBYTE)&defaultWoutput, &dwSize);
		RegQueryValueEx(hKey, L"defaultdev", NULL, &dwType, (LPBYTE)&defaultoutput, &dwSize);
		RegQueryValueEx(hKey, L"defaultmidiindev", NULL, &dwType, (LPBYTE)&defaultmidiindev, &dwSize);
		RegQueryValueEx(hKey, L"defaultsflist", NULL, &dwType, (LPBYTE)&defaultsflist, &dwSize);
		RegQueryValueEx(hKey, L"driverprio", NULL, &dwType, (LPBYTE)&driverprio, &dwSize);
		RegQueryValueEx(hKey, L"extra8lists", NULL, &dwType, (LPBYTE)&extra8lists, &dwSize);
		RegQueryValueEx(hKey, L"fadeoutdisable", NULL, &dwType, (LPBYTE)&fadeoutdisable, &dwSize);
		RegQueryValueEx(hKey, L"frequency", NULL, &dwType, (LPBYTE)&frequency, &dwSize);
		RegQueryValueEx(hKey, L"fullvelocity", NULL, &dwType, (LPBYTE)&fullvelocity, &dwSize);
		RegQueryValueEx(hKey, L"hivelign", NULL, &dwType, (LPBYTE)&hivel, &dwSize);
		RegQueryValueEx(hKey, L"ignorenotes1", NULL, &dwType, (LPBYTE)&ignorenotes1, &dwSize);
		RegQueryValueEx(hKey, L"limit88", NULL, &dwType, (LPBYTE)&limit88, &dwSize);
		RegQueryValueEx(hKey, L"lovelign", NULL, &dwType, (LPBYTE)&lovel, &dwSize);
		RegQueryValueEx(hKey, L"midiinenabled", NULL, &dwType, (LPBYTE)&midiinenabled, &dwSize);
		RegQueryValueEx(hKey, L"midivolumeoverride", NULL, &dwType, (LPBYTE)&midivolumeoverride, &dwSize);
		RegQueryValueEx(hKey, L"monorendering", NULL, &dwType, (LPBYTE)&monorendering, &dwSize);
		RegQueryValueEx(hKey, L"mt32mode", NULL, &dwType, (LPBYTE)&mt32mode, &dwSize);
		RegQueryValueEx(hKey, L"oldbuffermode", NULL, &dwType, (LPBYTE)&oldbuffermode, &dwSize);
		RegQueryValueEx(hKey, L"pitchshift", NULL, &dwType, (LPBYTE)&pitchshift, &dwSize);
		RegQueryValueEx(hKey, L"polyphony", NULL, &dwType, (LPBYTE)&midivoices, &dwSize);
		RegQueryValueEx(hKey, L"preload", NULL, &dwType, (LPBYTE)&preload, &dwSize);
		if (currentengine == 1) RegQueryValueEx(hKey, L"rco", NULL, &dwType, (LPBYTE)&rco, &dwSize);
		else rco = 0;
		RegQueryValueEx(hKey, L"sinc", NULL, &dwType, (LPBYTE)&sinc, &dwSize);
		RegQueryValueEx(hKey, L"sincconv", NULL, &dwType, (LPBYTE)&sincconv, &dwSize);
		RegQueryValueEx(hKey, L"sysexignore", NULL, &dwType, (LPBYTE)&sysexignore, &dwSize);
		RegQueryValueEx(hKey, L"vms2emu", NULL, &dwType, (LPBYTE)&vms2emu, &dwSize);
		RegQueryValueEx(hKey, L"vmsemu", NULL, &dwType, (LPBYTE)&vmsemu, &dwSize);
		RegQueryValueEx(hKey, L"volume", NULL, &dwType, (LPBYTE)&volume, &dwSize);
		RegQueryValueEx(hKey, L"volumehotkeys", NULL, &dwType, (LPBYTE)&volumehotkeys, &dwSize);
		RegQueryValueEx(hKey, L"xaudiodisabled", NULL, &dwType, (LPBYTE)&currentengine, &dwSize);
		RegSetValueEx(hKey, L"livechange", 0, dwType, (LPBYTE)&zero, sizeof(zero));

		if (lovel < 1) { lovel = 1; }
		if (hivel > 127) { hivel = 127; }

		RegCloseKey(hKey);

		frequencynew = frequency;
		sound_out_volume_float = (float)volume / 10000.0f;

		PrintToConsole(FOREGROUND_BLUE, 1, "Done loading settings from registry.");
	}
	catch (...) {
		CrashMessage(L"RegSetLoad");
		throw;
	}
}

void LoadSettingsRT()
{
	try {
		int zero = 0;
		HKEY hKey;
		long lResult;
		DWORD dwType = REG_DWORD;
		DWORD dwSize = sizeof(DWORD);
		int vms2emutemp = vms2emu;
		int frequencyttemp = frequency;
		int potato;
		lResult = RegOpenKeyEx(HKEY_CURRENT_USER, L"Software\\Keppy's Synthesizer\\Settings", 0, KEY_ALL_ACCESS, &hKey);
		RegQueryValueEx(hKey, L"allhotkeys", NULL, &dwType, (LPBYTE)&allhotkeys, &dwSize);
		RegQueryValueEx(hKey, L"allnotesignore", NULL, &dwType, (LPBYTE)&allnotesignore, &dwSize);
		RegQueryValueEx(hKey, L"alternativecpu", NULL, &dwType, (LPBYTE)&autopanic, &dwSize);
		RegQueryValueEx(hKey, L"capframerate", NULL, &dwType, (LPBYTE)&capframerate, &dwSize);
		RegQueryValueEx(hKey, L"cpu", NULL, &dwType, (LPBYTE)&maxcpu, &dwSize);
		RegQueryValueEx(hKey, L"fadeoutdisable", NULL, &dwType, (LPBYTE)&fadeoutdisable, &dwSize);
		RegQueryValueEx(hKey, L"frequency", NULL, &dwType, (LPBYTE)&frequency, &dwSize);
		RegQueryValueEx(hKey, L"fullvelocity", NULL, &dwType, (LPBYTE)&fullvelocity, &dwSize);
		RegQueryValueEx(hKey, L"hivelign", NULL, &dwType, (LPBYTE)&hivel, &dwSize);
		RegQueryValueEx(hKey, L"ignorenotes1", NULL, &dwType, (LPBYTE)&ignorenotes1, &dwSize);
		RegQueryValueEx(hKey, L"limit88", NULL, &dwType, (LPBYTE)&limit88, &dwSize);
		RegQueryValueEx(hKey, L"livechange", NULL, &dwType, (LPBYTE)&livechange, &dwSize);
		RegQueryValueEx(hKey, L"lovelign", NULL, &dwType, (LPBYTE)&lovel, &dwSize);
		RegQueryValueEx(hKey, L"midivolumeoverride", NULL, &dwType, (LPBYTE)&midivolumeoverride, &dwSize);
		RegQueryValueEx(hKey, L"mt32mode", NULL, &dwType, (LPBYTE)&mt32mode, &dwSize);
		RegQueryValueEx(hKey, L"nofx", NULL, &dwType, (LPBYTE)&nofx, &dwSize);
		RegQueryValueEx(hKey, L"noteoff", NULL, &dwType, (LPBYTE)&noteoff1, &dwSize);
		RegQueryValueEx(hKey, L"oldbuffermode", NULL, &dwType, (LPBYTE)&oldbuffermode, &dwSize);
		RegQueryValueEx(hKey, L"pitchshift", NULL, &dwType, (LPBYTE)&pitchshift, &dwSize);
		RegQueryValueEx(hKey, L"polyphony", NULL, &dwType, (LPBYTE)&midivoices, &dwSize);
		RegQueryValueEx(hKey, L"preload", NULL, &dwType, (LPBYTE)&preload, &dwSize);
		if (currentengine == 1) RegQueryValueEx(hKey, L"rco", NULL, &dwType, (LPBYTE)&rco, &dwSize);
		else rco = 0;
		RegQueryValueEx(hKey, L"sinc", NULL, &dwType, (LPBYTE)&sinc, &dwSize);
		RegQueryValueEx(hKey, L"sincconv", NULL, &dwType, (LPBYTE)&sincconv, &dwSize);
		RegQueryValueEx(hKey, L"sysexignore", NULL, &dwType, (LPBYTE)&sysexignore, &dwSize);
		RegQueryValueEx(hKey, L"sysresetignore", NULL, &dwType, (LPBYTE)&sysresetignore, &dwSize);
		RegQueryValueEx(hKey, L"vms2emu", NULL, &dwType, (LPBYTE)&vms2emu, &dwSize);
		RegQueryValueEx(hKey, L"volume", NULL, &dwType, (LPBYTE)&volume, &dwSize);
		RegQueryValueEx(hKey, L"volumehotkeys", NULL, &dwType, (LPBYTE)&volumehotkeys, &dwSize);
		RegQueryValueEx(hKey, L"volumemon", NULL, &dwType, (LPBYTE)&volumemon, &dwSize);

		RegQueryValueEx(hKey, L"printmidievent", NULL, &dwType, (LPBYTE)&printmidievent, &dwSize);
		RegQueryValueEx(hKey, L"printimportant", NULL, &dwType, (LPBYTE)&printimportant, &dwSize);

		if (vms2emutemp != vms2emu) {
			ResetSynth(1);
		}
		if (lovel < 1) { lovel = 1; }
		if (hivel > 127) { hivel = 127; }
		RegCloseKey(hKey);

		//cake
		sound_out_volume_float = (float)volume / 10000.0f;
		ChVolumeStruct.fCurrent = 1.0f;
		ChVolumeStruct.fTarget = sound_out_volume_float;
		ChVolumeStruct.fTime = 0.0f;
		ChVolumeStruct.lCurve = 0;
		BASS_FXSetParameters(ChVolume, &ChVolumeStruct);
		CheckUp(ERRORCODE, L"KSVolFXSet", FALSE);

		// stuff
		if (autopanic != 1) BASS_ChannelSetAttribute(KSStream, BASS_ATTRIB_MIDI_CPU, maxcpu);

		BASS_ChannelSetAttribute(KSStream, BASS_ATTRIB_SRC, sincconv);
		BASS_ChannelSetAttribute(KSStream, BASS_ATTRIB_MIDI_KILL, fadeoutdisable);
		if (noteoff1) {
			BASS_ChannelFlags(KSStream, BASS_MIDI_NOTEOFF1, BASS_MIDI_NOTEOFF1);
		}
		else {
			BASS_ChannelFlags(KSStream, 0, BASS_MIDI_NOTEOFF1);
		}
		if (nofx) {
			BASS_ChannelFlags(KSStream, BASS_MIDI_NOFX, BASS_MIDI_NOFX);
		}
		else {
			BASS_ChannelFlags(KSStream, 0, BASS_MIDI_NOFX);
		}
		if (sysresetignore) {
			BASS_ChannelFlags(KSStream, BASS_MIDI_NOSYSRESET, BASS_MIDI_NOSYSRESET);
		}
		else {
			BASS_ChannelFlags(KSStream, 0, BASS_MIDI_NOSYSRESET);
		}
		if (sinc) {
			BASS_ChannelFlags(KSStream, BASS_MIDI_SINCINTER, BASS_MIDI_SINCINTER);
		}
		else {
			BASS_ChannelFlags(KSStream, 0, BASS_MIDI_SINCINTER);
		}
	}
	catch (...) {
		CrashMessage(L"RTSetLoad");
		throw;
	}
}

void LoadCustomInstruments() {
	HKEY hKey;
	long lResult;
	DWORD dwType = REG_DWORD;
	DWORD dwSize = sizeof(DWORD);
	lResult = RegOpenKeyEx(HKEY_CURRENT_USER, L"Software\\Keppy's Synthesizer\\ChanOverride", 0, KEY_ALL_ACCESS, &hKey);
	RegQueryValueEx(hKey, L"overrideinstruments", NULL, &dwType, (LPBYTE)&overrideinstruments, &dwSize);
	for (int i = 0; i <= 15; ++i) {
		RegQueryValueEx(hKey, cbankname[i], NULL, &dwType, (LPBYTE)&cbank[i], &dwSize);
		RegQueryValueEx(hKey, cpresetname[i], NULL, &dwType, (LPBYTE)&cpreset[i], &dwSize);
	}
	RegCloseKey(hKey);
}

void Panic() {
	//Panic system
	if (autopanic == 1) {
		BASS_ChannelSetAttribute(KSStream, BASS_ATTRIB_MIDI_CPU, 100.0f);
		float maxcpuf = (float)maxcpu;

		if (currentcpuusage0 >= (maxcpuf - 3.0f)) {
			int newmidivoices = midivoices - ((midivoices / 8) * (int)(currentcpuusage0 - maxcpuf));

			if (newmidivoices < 1) {
				newmidivoices = 1;
			}

			BASS_ChannelSetAttribute(KSStream, BASS_ATTRIB_MIDI_VOICES, newmidivoices);
		}
		else {
			BASS_ChannelSetAttribute(KSStream, BASS_ATTRIB_MIDI_VOICES, midivoices);
		}
	}
	else {
		BASS_ChannelSetAttribute(KSStream, BASS_ATTRIB_MIDI_VOICES, midivoices);
	}
}

int AudioRenderingType(int value) {
	if (currentengine == 2 || currentengine == 3) return BASS_SAMPLE_FLOAT;
	else {
		if (value == 1)
			return BASS_SAMPLE_FLOAT;
		else if (value == 2 || value == 0)
			return 0;
		else if (value == 3)
			return BASS_SAMPLE_8BITS;
		else
			return BASS_SAMPLE_FLOAT;
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
		lResult = RegOpenKeyEx(HKEY_CURRENT_USER, L"Software\\Keppy's Synthesizer\\Watchdog", 0, KEY_ALL_ACCESS, &hKey);

		for (int i = 0; i <= 15; ++i) {
			RegQueryValueEx(hKey, rnames[i], NULL, &dwType, (LPBYTE)&rvalues[i], &dwSize);
			if (rvalues[i] == 1) {
				LoadSoundfont(i + 1);
				RegSetValueEx(hKey, rnames[i], 0, dwType, (LPBYTE)&zero, sizeof(zero));
			}
		}

		RegCloseKey(hKey);
	}
	catch (...) {
		CrashMessage(L"ConfigCheck");
		throw;
	}
}

void CheckVolume() {
	try {
		if (volumemon == 1) {
			HKEY hKey;
			long lResult;
			float levels[2];
			DWORD dwType = REG_DWORD;
			DWORD dwSize = sizeof(DWORD);
			DWORD left, right;
			lResult = RegOpenKeyEx(HKEY_CURRENT_USER, L"Software\\Keppy's Synthesizer", 0, KEY_ALL_ACCESS, &hKey);

			if (currentengine == 1 || currentengine == 3)
			{
				BASS_ChannelGetLevelEx(KSStream, levels, (monorendering ? 0.01f : 0.02f), (monorendering ? BASS_LEVEL_MONO : BASS_LEVEL_STEREO));
			}
			else if (currentengine == 2)
			{
				levels[0] = BASS_ASIO_ChannelGetLevel(FALSE, 0);
				levels[1] = BASS_ASIO_ChannelGetLevel(FALSE, 1);
			}
			else { /* Nothing */ }

			DWORD level = MAKELONG((WORD)(min(levels[0], 1) * 32768), (WORD)(min(levels[1], 1) * 32768));
			left = LOWORD(level); // the left level
			right = HIWORD(level); // the right level

			RegSetValueEx(hKey, L"leftvol", 0, dwType, (LPBYTE)&left, sizeof(left));
			RegSetValueEx(hKey, L"rightvol", 0, dwType, (LPBYTE)&right, sizeof(right));
			RegCloseKey(hKey);
		}
	}
	catch (...) {
		CrashMessage(L"VolumeMonWrite");
		throw;
	}
}

void FillContentDebug(BOOL close, float CCUI0, int HC, long RUI, bool KSDAPI, double TD1, double TD2, double TD3, double TD4, double IL, double OL) {
	std::string PipeContent;
	DWORD bytesWritten;

	PipeContent += "KSDebugInfo";
	PipeContent += "\nCurrentApp = ";
	PipeContent += close ? "" : modulename;
	PipeContent += "\nBitApp = ";
	PipeContent += close ? "" : bitapp;

	for (int i = 0; i <= 15; ++i) PipeContent += "\nCV" + std::to_string(i) + " = " + std::to_string(cvvalues[i]);

	PipeContent += "\nCurCPU = " + std::to_string(CCUI0);
	PipeContent += "\nHandles = " + std::to_string(HC);
	PipeContent += "\nRAMUsage = " + std::to_string(RUI);
	PipeContent += "\nKSDirect = " + std::to_string(KSDAPI);
	PipeContent += "\nTd1 = " + std::to_string(TD1);
	PipeContent += "\nTd2 = " + std::to_string(TD2);
	PipeContent += "\nTd3 = " + std::to_string(TD3);
	PipeContent += "\nTd4 = " + std::to_string(TD4);
	PipeContent += "\nASIOInLat = " + std::to_string(IL);
	PipeContent += "\nASIOOutLat = " + std::to_string(OL);

	PipeContent += "\n\0";

	if (hPipe != INVALID_HANDLE_VALUE) WriteFile(hPipe, PipeContent.c_str(), PipeContent.length(), &bytesWritten, NULL);
	if (GetLastError() != 0 && GetLastError() != 536) StartDebugPipe(TRUE);
}

clock_t end;
double rate = 0;
double inlatency = 0.0;
double outlatency = 0.0;
void ParseDebugData() {
	BASS_ChannelGetAttribute(KSStream, BASS_ATTRIB_CPU, &currentcpuusage0);

	if (currentengine == 2) {
		rate = BASS_ASIO_GetRate();
		CheckUpASIO(ERRORCODE, L"KSGetRateASIO", TRUE);
		inlatency = (double)BASS_ASIO_GetLatency(TRUE) * 1000.0 / rate;
		CheckUpASIO(ERRORCODE, L"KSGetInputLatencyASIO", TRUE);
		outlatency = (double)BASS_ASIO_GetLatency(FALSE) * 1000.0 / rate;
		CheckUpASIO(ERRORCODE, L"KSGetOutputLatencyASIO", TRUE);
	}

	for (int i = 0; i <= 15; ++i) cvvalues[i] = BASS_MIDI_StreamGetEvent(KSStream, i, MIDI_EVENT_VOICES);
}

void SendDebugDataToPipe() {
	try {
		DWORD handlecount;

		PROCESS_MEMORY_COUNTERS_EX pmc;
		GetProcessMemoryInfo(GetCurrentProcess(), (PROCESS_MEMORY_COUNTERS*)&pmc, sizeof(pmc));
		GetProcessHandleCount(GetCurrentProcess(), &handlecount);
		SIZE_T ramusage = pmc.WorkingSetSize;
		uint64_t ramusageint = static_cast<uint64_t>(ramusage);

		long long TimeDuringDebug = TimeNow();

		FillContentDebug(FALSE, currentcpuusage0, handlecount, static_cast<uint64_t>(pmc.WorkingSetSize), ksdirectenabled,
			TimeDuringDebug - start1, TimeDuringDebug - start2, TimeDuringDebug - start3, oldbuffermode ? 0.0f : TimeDuringDebug - start4,
			inlatency, outlatency);
	}
	catch (...) {
		CrashMessage(L"DebugCheck");
		throw;
	}
}

void mixervoid() {
	try {
		HKEY hKey;
		long lResult;
		DWORD dwType = REG_DWORD;
		DWORD dwSize = sizeof(DWORD);
		lResult = RegOpenKeyEx(HKEY_CURRENT_USER, L"Software\\Keppy's Synthesizer\\Channels", 0, KEY_ALL_ACCESS, &hKey);

		for (int i = 0; i <= 15; ++i) {
			RegQueryValueEx(hKey, cnames[i], NULL, &dwType, (LPBYTE)&cvalues[i], &dwSize);
			BASS_MIDI_StreamEvent(KSStream, i, MIDI_EVENT_MIXLEVEL, cvalues[i]);
			RegQueryValueEx(hKey, pitchshiftname[i], NULL, &dwType, (LPBYTE)&pitchshiftchan[i], &dwSize);
		}

		RegCloseKey(hKey);
	}
	catch (...) {
		CrashMessage(L"MixerCheck");
		throw;
	}
}

void RevbNChor() {
	try {
		int status = 0;
		HKEY hKey;
		long lResult;
		DWORD dwType = REG_DWORD;
		DWORD dwSize = sizeof(DWORD);
		lResult = RegOpenKeyEx(HKEY_CURRENT_USER, L"Software\\Keppy's Synthesizer\\Settings", 0, KEY_ALL_ACCESS, &hKey);

		RegQueryValueEx(hKey, L"rcoverride", NULL, &dwType, (LPBYTE)&status, &dwSize);
		RegQueryValueEx(hKey, L"reverb", NULL, &dwType, (LPBYTE)&reverb, &dwSize);
		RegQueryValueEx(hKey, L"chorus", NULL, &dwType, (LPBYTE)&chorus, &dwSize);

		if (status == 1) {
			for (int i = 0; i <= 15; ++i) {
				BASS_MIDI_StreamEvent(KSStream, i, MIDI_EVENT_REVERB, reverb);
				BASS_MIDI_StreamEvent(KSStream, i, MIDI_EVENT_CHORUS, chorus);
			}
		}

		RegCloseKey(hKey);
	}
	catch (...) {
		CrashMessage(L"RnCCheck");
		throw;
	}
}

void ReloadSFList(DWORD whichsflist){
	try {
		ResetSynth(0);
		Sleep(100);
		LoadSoundfont(whichsflist);
	}
	catch (...) {
		CrashMessage(L"ReloadSFListCheck");
		throw;
	}
}

void keybindings()
{
	try {
		if (allhotkeys == 1) {
			if (extra8lists == 1) {
				BOOL ControlPressed = (GetAsyncKeyState(VK_CONTROL) & (1 << 15));
				if (!ControlPressed && GetAsyncKeyState(VK_MENU) & GetAsyncKeyState(0x31) & 0x8000) {
					ReloadSFList(1);
					return;
				}
				else if (!ControlPressed && GetAsyncKeyState(VK_MENU) & GetAsyncKeyState(0x32) & 0x8000) {
					ReloadSFList(2);
					return;
				}
				else if (!ControlPressed && GetAsyncKeyState(VK_MENU) & GetAsyncKeyState(0x33) & 0x8000) {
					ReloadSFList(3);
					return;
				}
				else if (!ControlPressed && GetAsyncKeyState(VK_MENU) & GetAsyncKeyState(0x34) & 0x8000) {
					ReloadSFList(4);
					return;
				}
				else if (!ControlPressed && GetAsyncKeyState(VK_MENU) & GetAsyncKeyState(0x35) & 0x8000) {
					ReloadSFList(5);
					return;
				}
				else if (!ControlPressed && GetAsyncKeyState(VK_MENU) & GetAsyncKeyState(0x36) & 0x8000) {
					ReloadSFList(6);
					return;
				}
				else if (!ControlPressed && GetAsyncKeyState(VK_MENU) & GetAsyncKeyState(0x37) & 0x8000) {
					ReloadSFList(7);
					return;
				}
				else if (!ControlPressed && GetAsyncKeyState(VK_MENU) & GetAsyncKeyState(0x38) & 0x8000) {
					ReloadSFList(8);
					return;
				}
				else if (GetAsyncKeyState(VK_CONTROL) & GetAsyncKeyState(VK_MENU) & GetAsyncKeyState(0x31) & 0x8000) {
					ReloadSFList(9);
					return;
				}
				else if (GetAsyncKeyState(VK_CONTROL) & GetAsyncKeyState(VK_MENU) & GetAsyncKeyState(0x32) & 0x8000) {
					ReloadSFList(10);
					return;
				}
				else if (GetAsyncKeyState(VK_CONTROL) & GetAsyncKeyState(VK_MENU) & GetAsyncKeyState(0x33) & 0x8000) {
					ReloadSFList(11);
					return;
				}
				else if (GetAsyncKeyState(VK_CONTROL) & GetAsyncKeyState(VK_MENU) & GetAsyncKeyState(0x34) & 0x8000) {
					ReloadSFList(12);
					return;
				}
				else if (GetAsyncKeyState(VK_CONTROL) & GetAsyncKeyState(VK_MENU) & GetAsyncKeyState(0x35) & 0x8000) {
					ReloadSFList(13);
					return;
				}
				else if (GetAsyncKeyState(VK_CONTROL) & GetAsyncKeyState(VK_MENU) & GetAsyncKeyState(0x36) & 0x8000) {
					ReloadSFList(14);
					return;
				}
				else if (GetAsyncKeyState(VK_CONTROL) & GetAsyncKeyState(VK_MENU) & GetAsyncKeyState(0x37) & 0x8000) {
					ReloadSFList(15);
					return;
				}
				else if (GetAsyncKeyState(VK_CONTROL) & GetAsyncKeyState(VK_MENU) & GetAsyncKeyState(0x38) & 0x8000) {
					ReloadSFList(16);
					return;
				}
			}
			else {
				if (GetAsyncKeyState(VK_MENU) & GetAsyncKeyState(0x31) & 0x8000) {
					ReloadSFList(1);
					return;
				}
				if (GetAsyncKeyState(VK_MENU) & GetAsyncKeyState(0x32) & 0x8000) {
					ReloadSFList(2);
					return;
				}
				if (GetAsyncKeyState(VK_MENU) & GetAsyncKeyState(0x33) & 0x8000) {
					ReloadSFList(3);
					return;
				}
				if (GetAsyncKeyState(VK_MENU) & GetAsyncKeyState(0x34) & 0x8000) {
					ReloadSFList(4);
					return;
				}
				if (GetAsyncKeyState(VK_MENU) & GetAsyncKeyState(0x35) & 0x8000) {
					ReloadSFList(5);
					return;
				}
				if (GetAsyncKeyState(VK_MENU) & GetAsyncKeyState(0x36) & 0x8000) {
					ReloadSFList(6);
					return;
				}
				if (GetAsyncKeyState(VK_MENU) & GetAsyncKeyState(0x37) & 0x8000) {
					ReloadSFList(7);
					return;
				}
				if (GetAsyncKeyState(VK_MENU) & GetAsyncKeyState(0x38) & 0x8000) {
					ReloadSFList(8);
					return;
				}
			}
			if (GetAsyncKeyState(VK_MENU) & GetAsyncKeyState(0x39) & 0x8000) {
				if (currentengine == 2) {
					BASS_ASIO_ControlPanel();
				}
				else {
					TCHAR configuratorapp[MAX_PATH];
					BOOL run = TRUE;
					if (SUCCEEDED(SHGetFolderPath(NULL, CSIDL_SYSTEMX86, NULL, 0, configuratorapp)))
					{
						PathAppend(configuratorapp, _T("\\keppysynth\\KeppySynthMixerWindow.exe"));
						ShellExecute(NULL, L"open", configuratorapp, NULL, NULL, SW_SHOWNORMAL);
						Sleep(10);
						return;
					}
				}
			}
			else if (GetAsyncKeyState(VK_MENU) & GetAsyncKeyState(0x30) & 0x8000) {
				TCHAR configuratorapp[MAX_PATH];
				BOOL run = TRUE;
				if (SUCCEEDED(SHGetFolderPath(NULL, CSIDL_SYSTEMX86, NULL, 0, configuratorapp)))
				{
					PathAppend(configuratorapp, _T("\\keppysynth\\KeppySynthDebugWindow.exe"));
					ShellExecute(NULL, L"open", configuratorapp, NULL, NULL, SW_SHOWNORMAL);
					Sleep(10);
					return;
				}
			}
			if (GetAsyncKeyState(VK_INSERT) & 1) {
				ResetSynth(0);
			}
			else {
				// Nothing lel
			}
		}
	}
	catch (...) {
		CrashMessage(L"HotKeysCheck");
		throw;
	}
}