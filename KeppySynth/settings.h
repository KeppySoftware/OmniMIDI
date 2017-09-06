/*
Keppy's Synthesizer settings loading system
*/

struct evbuf_t{
	UINT   uMsg;
	DWORD_PTR	dwParam1;
};

static struct evbuf_t * evbuf;
static long long evbwpoint = 0;
static long long evbrpoint = 0;
static volatile long long evbcount = 0;
static UINT evbsysexpoint;

void crashmessage(LPCWSTR part) {
	TCHAR errormessage[MAX_PATH] = L"An error has been detected while trying to execute the following action: ";
	TCHAR clickokmsg[MAX_PATH] = L"\nPlease take a screenshot of this messagebox (ALT+PRINT), and create a GitHub issue.\n\nClick OK to close the program.";
	lstrcat(errormessage, part);
	lstrcat(errormessage, clickokmsg);
	SetConsoleTextAttribute(hConsole, FOREGROUND_RED);
	std::cout << "(Error at \"" << part << "\") - Fatal error during the execution of the driver." << std::endl;

	const int result = MessageBox(NULL, errormessage, L"Keppy's Synthesizer - Fatal execution error", MB_ICONERROR | MB_SYSTEMMODAL);
	switch (result)
	{
	default:
		exit(0);
		return;
	}
}

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
	std::cout << "(BASS_VST ERROR: " << dll << ") " << " - Can not load DLL. VC++ 2010 is missing. Skipping..." << std::endl;
}

bool CheckXAudioInstallation() {
	TCHAR xaudio[MAX_PATH];
	ZeroMemory(xaudio, sizeof(TCHAR) * MAX_PATH);
	SHGetFolderPath(NULL, CSIDL_SYSTEM, NULL, 0, xaudio);
	PathAppend(xaudio, _T("XAudio2_7.dll"));
	if (!PathFileExists(xaudio)) {
		return FALSE;
	}
	return TRUE;
}

float GetUsage(clock_t start, clock_t end) {
	return (float)(end - start);
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
		if (vstimode == FALSE)
		{
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
	}
	catch (...) {
		crashmessage(L"SFLoad");
		throw;
	}
}

bool LoadSoundfontStartup() {
	try {
		if (vstimode == FALSE)
		{
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
	}
	catch (...) {
		crashmessage(L"SFLoadStartup");
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
		TCHAR basspath[MAX_PATH] = { 0 };
		TCHAR basspathalt[MAX_PATH] = { 0 };
		TCHAR bassvstpath[MAX_PATH] = { 0 };
		TCHAR bassvstpathalt[MAX_PATH] = { 0 };
		TCHAR bassxapath[MAX_PATH] = { 0 };
		TCHAR bassxapathalt[MAX_PATH] = { 0 };
		TCHAR bassfxpath[MAX_PATH] = { 0 };
		TCHAR bassfxpathalt[MAX_PATH] = { 0 };
		TCHAR basswasapipath[MAX_PATH] = { 0 };
		TCHAR basswasapipathalt[MAX_PATH] = { 0 };

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
		SHGetFolderPath(NULL, CSIDL_PROFILE, NULL, 0, basswasapipathalt);

#if defined(_WIN64)
		PathAppend(basspathalt, _T("\\Keppy's Synthesizer\\dlloverride\\64\\bass.dll"));
		PathAppend(bassmidipathalt, _T("\\Keppy's Synthesizer\\dlloverride\\64\\bassmidi.dll"));
		PathAppend(bassencpathalt, _T("\\Keppy's Synthesizer\\dlloverride\\64\\bassenc.dll"));
		PathAppend(bassasiopathalt, _T("\\Keppy's Synthesizer\\dlloverride\\64\\bassasio.dll"));
		PathAppend(bassvstpathalt, _T("\\Keppy's Synthesizer\\dlloverride\\64\\bass_vst.dll"));
		PathAppend(bassxapathalt, _T("\\Keppy's Synthesizer\\dlloverride\\64\\bassxa.dll"));
		PathAppend(bassfxpathalt, _T("\\Keppy's Synthesizer\\dlloverride\\64\\bass_fx.dll"));
		PathAppend(basswasapipathalt, _T("\\Keppy's Synthesizer\\dlloverride\\64\\basswasapi.dll"));
#elif defined(_WIN32)
		PathAppend(basspathalt, _T("\\Keppy's Synthesizer\\dlloverride\\32\\bass.dll"));
		PathAppend(bassmidipathalt, _T("\\Keppy's Synthesizer\\dlloverride\\32\\bassmidi.dll"));
		PathAppend(bassencpathalt, _T("\\Keppy's Synthesizer\\dlloverride\\32\\bassenc.dll"));
		PathAppend(bassasiopathalt, _T("\\Keppy's Synthesizer\\dlloverride\\32\\bassasio.dll"));
		PathAppend(bassvstpathalt, _T("\\Keppy's Synthesizer\\dlloverride\\32\\bass_vst.dll"));
		PathAppend(bassxapathalt, _T("\\Keppy's Synthesizer\\dlloverride\\32\\bassxa.dll"));
		PathAppend(bassfxpathalt, _T("\\Keppy's Synthesizer\\dlloverride\\32\\bass_fx.dll"));
		PathAppend(basswasapipathalt, _T("\\Keppy's Synthesizer\\dlloverride\\32\\basswasapi.dll")); 
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

		// BASSWASAPI
		if (PathFileExists(basswasapipathalt)) {
			if (!(basswasapi = LoadLibrary(basswasapipathalt))) {
				DLLLoadError(basswasapipathalt);
				exit(0);
			}
			isoverrideenabled = 1;
		}
		else {
			lstrcat(basswasapipath, installpath);
			lstrcat(basswasapipath, L"\\basswasapi.dll");
			if (!(basswasapi = LoadLibrary(basswasapipath))) {
				DLLLoadError(basswasapipath);
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
		LOADBASSMIDIFUNCTION(BASS_MIDI_FontFree);
		LOADBASSMIDIFUNCTION(BASS_MIDI_FontInit);
		LOADBASSMIDIFUNCTION(BASS_MIDI_FontLoad);
		LOADBASSMIDIFUNCTION(BASS_MIDI_StreamCreate);
		LOADBASSMIDIFUNCTION(BASS_MIDI_StreamEvent);
		LOADBASSMIDIFUNCTION(BASS_MIDI_StreamEvents);
		LOADBASSMIDIFUNCTION(BASS_MIDI_StreamGetEvent);
		LOADBASSMIDIFUNCTION(BASS_MIDI_StreamLoadSamples);
		LOADBASSMIDIFUNCTION(BASS_MIDI_StreamSetFonts);
		LOADBASSWASAPIFUNCTION(BASS_WASAPI_Free);
		LOADBASSWASAPIFUNCTION(BASS_WASAPI_GetCPU);
		LOADBASSWASAPIFUNCTION(BASS_WASAPI_GetDevice);
		LOADBASSWASAPIFUNCTION(BASS_WASAPI_GetDeviceInfo);
		LOADBASSWASAPIFUNCTION(BASS_WASAPI_GetInfo);
		LOADBASSWASAPIFUNCTION(BASS_WASAPI_GetLevelEx);
		LOADBASSWASAPIFUNCTION(BASS_WASAPI_Init);
		LOADBASSWASAPIFUNCTION(BASS_WASAPI_PutData);
		LOADBASSWASAPIFUNCTION(BASS_WASAPI_SetVolume);
		LOADBASSWASAPIFUNCTION(BASS_WASAPI_Start);
		LOADBASSWASAPIFUNCTION(BASS_WASAPI_Stop);
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
		crashmessage(L"BASSDefLoad");
		throw;
	}
}

void appname() {
	try {
		HKEY hKey;
		long lResult;
		DWORD dwType = REG_SZ;
		DWORD dwSize = sizeof(DWORD);
		TCHAR modulename[MAX_PATH];
		TCHAR bitapp[MAX_PATH];
		ZeroMemory(modulename, MAX_PATH * sizeof(TCHAR));
		ZeroMemory(bitapp, MAX_PATH * sizeof(TCHAR));
		GetModuleFileNameEx(GetCurrentProcess(), NULL, modulename, MAX_PATH);
		lResult = RegOpenKeyEx(HKEY_CURRENT_USER, L"Software\\Keppy's Synthesizer\\Watchdog", 0, KEY_ALL_ACCESS, &hKey);
#if defined(_WIN64)
		wcscpy(bitapp, L"64-bit");
#elif defined(_WIN32)
		wcscpy(bitapp, L"32-bit");
#endif
		RegSetValueEx(hKey, L"currentapp", 0, dwType, (LPBYTE)&modulename, sizeof(modulename));
		RegSetValueEx(hKey, L"bit", 0, dwType, (LPBYTE)&bitapp, sizeof(bitapp));
		RegCloseKey(hKey);
	}
	catch (...) {
		crashmessage(L"AppNameWrite");
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
		crashmessage(L"MemAlloc");
		throw;
	}
}
void load_settings(bool streamreload)
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
			RegQueryValueEx(hKey, L"sndbfvalue", NULL, &dwType, (LPBYTE)&newsndbfvalue, &dwSize);
		}
		RegQueryValueEx(hKey, L"alternativecpu", NULL, &dwType, (LPBYTE)&autopanic, &dwSize);
		RegQueryValueEx(hKey, L"allhotkeys", NULL, &dwType, (LPBYTE)&allhotkeys, &dwSize);
		RegQueryValueEx(hKey, L"buflen", NULL, &dwType, (LPBYTE)&frames, &dwSize);
		RegQueryValueEx(hKey, L"capframerate", NULL, &dwType, (LPBYTE)&capframerate, &dwSize);
		RegQueryValueEx(hKey, L"cpu", NULL, &dwType, (LPBYTE)&maxcpu, &dwSize);
		RegQueryValueEx(hKey, L"defaultsflist", NULL, &dwType, (LPBYTE)&defaultsflist, &dwSize);
		RegQueryValueEx(hKey, L"fullvelocity", NULL, &dwType, (LPBYTE)&fullvelocity, &dwSize);
		RegQueryValueEx(hKey, L"limit88", NULL, &dwType, (LPBYTE)&limit88, &dwSize);
		RegQueryValueEx(hKey, L"fadeoutdisable", NULL, &dwType, (LPBYTE)&fadeoutdisable, &dwSize);
		RegQueryValueEx(hKey, L"defaultdev", NULL, &dwType, (LPBYTE)&defaultoutput, &dwSize);
		RegQueryValueEx(hKey, L"defaultAdev", NULL, &dwType, (LPBYTE)&defaultAoutput, &dwSize);
		RegQueryValueEx(hKey, L"defaultWdev", NULL, &dwType, (LPBYTE)&defaultWoutput, &dwSize);
		RegQueryValueEx(hKey, L"defaultmidiindev", NULL, &dwType, (LPBYTE)&defaultmidiindev, &dwSize);
		RegQueryValueEx(hKey, L"driverprio", NULL, &dwType, (LPBYTE)&driverprio, &dwSize);
		RegQueryValueEx(hKey, L"midiinenabled", NULL, &dwType, (LPBYTE)&midiinenabled, &dwSize);
		RegQueryValueEx(hKey, L"pitchshift", NULL, &dwType, (LPBYTE)&pitchshift, &dwSize);
		RegQueryValueEx(hKey, L"32bit", NULL, &dwType, (LPBYTE)&floatrendering, &dwSize);
		RegQueryValueEx(hKey, L"frequency", NULL, &dwType, (LPBYTE)&frequency, &dwSize);
		RegQueryValueEx(hKey, L"ignorenotes1", NULL, &dwType, (LPBYTE)&ignorenotes1, &dwSize);
		RegQueryValueEx(hKey, L"midivolumeoverride", NULL, &dwType, (LPBYTE)&midivolumeoverride, &dwSize);
		RegQueryValueEx(hKey, L"extra8lists", NULL, &dwType, (LPBYTE)&extra8lists, &dwSize);
		RegQueryValueEx(hKey, L"polyphony", NULL, &dwType, (LPBYTE)&midivoices, &dwSize);
		RegQueryValueEx(hKey, L"monorendering", NULL, &dwType, (LPBYTE)&monorendering, &dwSize);
		RegQueryValueEx(hKey, L"preload", NULL, &dwType, (LPBYTE)&preload, &dwSize);
		RegQueryValueEx(hKey, L"oldbuffermode", NULL, &dwType, (LPBYTE)&oldbuffermode, &dwSize);
		RegQueryValueEx(hKey, L"rco", NULL, &dwType, (LPBYTE)&rco, &dwSize);
		RegQueryValueEx(hKey, L"lovelign", NULL, &dwType, (LPBYTE)&lovel, &dwSize);
		RegQueryValueEx(hKey, L"hivelign", NULL, &dwType, (LPBYTE)&hivel, &dwSize);
		RegQueryValueEx(hKey, L"vmsemu", NULL, &dwType, (LPBYTE)&vmsemu, &dwSize);
		RegQueryValueEx(hKey, L"vms2emu", NULL, &dwType, (LPBYTE)&vms2emu, &dwSize);
		RegQueryValueEx(hKey, L"volume", NULL, &dwType, (LPBYTE)&volume, &dwSize);
		RegQueryValueEx(hKey, L"volumehotkeys", NULL, &dwType, (LPBYTE)&volumehotkeys, &dwSize);
		RegQueryValueEx(hKey, L"sysexignore", NULL, &dwType, (LPBYTE)&sysexignore, &dwSize);
		RegQueryValueEx(hKey, L"allnotesignore", NULL, &dwType, (LPBYTE)&allnotesignore, &dwSize);
		RegQueryValueEx(hKey, L"xaudiodisabled", NULL, &dwType, (LPBYTE)&currentengine, &dwSize);
		RegQueryValueEx(hKey, L"wasapiex", NULL, &dwType, (LPBYTE)&wasapiex, &dwSize);
		RegQueryValueEx(hKey, L"sinc", NULL, &dwType, (LPBYTE)&sinc, &dwSize);
		RegSetValueEx(hKey, L"livechange", 0, dwType, (LPBYTE)&zero, sizeof(zero));

		if (lovel < 1) { lovel = 1; }
		if (hivel > 127) { hivel = 127; }

		RegCloseKey(hKey);

		appname();

		frequencynew = frequency;

		sound_out_volume_float = (float)volume / 10000.0f;
		sound_out_volume_int = (int)(sound_out_volume_float * (float)0x1000);

		PrintToConsole(FOREGROUND_BLUE, 1, "Done loading settings from registry.");
	}
	catch (...) {
		crashmessage(L"RegSetLoad");
		throw;
	}
}

void realtime_load_settings()
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
		RegQueryValueEx(hKey, L"frequency", NULL, &dwType, (LPBYTE)&frequency, &dwSize);
		RegQueryValueEx(hKey, L"alternativecpu", NULL, &dwType, (LPBYTE)&autopanic, &dwSize);
		RegQueryValueEx(hKey, L"allhotkeys", NULL, &dwType, (LPBYTE)&allhotkeys, &dwSize);
		RegQueryValueEx(hKey, L"capframerate", NULL, &dwType, (LPBYTE)&capframerate, &dwSize);
		RegQueryValueEx(hKey, L"cpu", NULL, &dwType, (LPBYTE)&maxcpu, &dwSize);
		RegQueryValueEx(hKey, L"pitchshift", NULL, &dwType, (LPBYTE)&pitchshift, &dwSize);
		RegQueryValueEx(hKey, L"midivolumeoverride", NULL, &dwType, (LPBYTE)&midivolumeoverride, &dwSize);
		RegQueryValueEx(hKey, L"nofx", NULL, &dwType, (LPBYTE)&nofx, &dwSize);
		RegQueryValueEx(hKey, L"fullvelocity", NULL, &dwType, (LPBYTE)&fullvelocity, &dwSize);
		RegQueryValueEx(hKey, L"fadeoutdisable", NULL, &dwType, (LPBYTE)&fadeoutdisable, &dwSize);
		RegQueryValueEx(hKey, L"noteoff", NULL, &dwType, (LPBYTE)&noteoff1, &dwSize);
		RegQueryValueEx(hKey, L"limit88", NULL, &dwType, (LPBYTE)&limit88, &dwSize);
		RegQueryValueEx(hKey, L"polyphony", NULL, &dwType, (LPBYTE)&midivoices, &dwSize);
		RegQueryValueEx(hKey, L"vms2emu", NULL, &dwType, (LPBYTE)&vms2emu, &dwSize);
		RegQueryValueEx(hKey, L"ignorenotes1", NULL, &dwType, (LPBYTE)&ignorenotes1, &dwSize);
		RegQueryValueEx(hKey, L"preload", NULL, &dwType, (LPBYTE)&preload, &dwSize);
		RegQueryValueEx(hKey, L"oldbuffermode", NULL, &dwType, (LPBYTE)&oldbuffermode, &dwSize);
		RegQueryValueEx(hKey, L"rco", NULL, &dwType, (LPBYTE)&rco, &dwSize);
		RegQueryValueEx(hKey, L"sinc", NULL, &dwType, (LPBYTE)&sinc, &dwSize);
		RegQueryValueEx(hKey, L"sysresetignore", NULL, &dwType, (LPBYTE)&sysresetignore, &dwSize);
		RegQueryValueEx(hKey, L"volume", NULL, &dwType, (LPBYTE)&volume, &dwSize);
		RegQueryValueEx(hKey, L"lovelign", NULL, &dwType, (LPBYTE)&lovel, &dwSize);
		RegQueryValueEx(hKey, L"hivelign", NULL, &dwType, (LPBYTE)&hivel, &dwSize);
		RegQueryValueEx(hKey, L"volumehotkeys", NULL, &dwType, (LPBYTE)&volumehotkeys, &dwSize);
		RegQueryValueEx(hKey, L"volumemon", NULL, &dwType, (LPBYTE)&volumemon, &dwSize);
		RegQueryValueEx(hKey, L"sysexignore", NULL, &dwType, (LPBYTE)&sysexignore, &dwSize);
		RegQueryValueEx(hKey, L"allnotesignore", NULL, &dwType, (LPBYTE)&allnotesignore, &dwSize);
		RegQueryValueEx(hKey, L"livechange", NULL, &dwType, (LPBYTE)&livechange, &dwSize);
		if (vms2emutemp != vms2emu) {
			ResetSynth(1);
		}
		if (lovel < 1) { lovel = 1; }
		if (hivel > 127) { hivel = 127; }
		RegCloseKey(hKey);

		//cake
		sound_out_volume_float = (float)volume / 10000.0f;
		sound_out_volume_int = (int)(sound_out_volume_float * (float)0x1000);
		if (currentengine == 1) BASS_SetConfig(BASS_CONFIG_GVOL_STREAM, volume);
		else if (currentengine == 2) BASS_ASIO_ChannelSetVolume(FALSE, -1, sound_out_volume_float);
		else if (currentengine == 3) BASS_WASAPI_SetVolume(BASS_WASAPI_VOL_SESSION, sound_out_volume_float);

		// stuff
		if (autopanic != 1) BASS_ChannelSetAttribute(KSStream, BASS_ATTRIB_MIDI_CPU, maxcpu);

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
		appname();
	}
	catch (...) {
		crashmessage(L"RTSetLoad");
		throw;
	}
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
		crashmessage(L"ConfigCheck");
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

			if (currentengine == 1)
			{
				BASS_ChannelGetLevelEx(KSStream, levels, (monorendering ? 0.01f : 0.02f), (monorendering ? BASS_LEVEL_MONO : BASS_LEVEL_STEREO));
			}
			else if (currentengine == 2)
			{
				levels[0] = BASS_ASIO_ChannelGetLevel(FALSE, 0);
				levels[1] = BASS_ASIO_ChannelGetLevel(FALSE, 1);
			}
			else if (currentengine == 3)
			{
				BASS_WASAPI_GetLevelEx(levels, (monorendering ? 0.01f : 0.02f), (monorendering ? BASS_LEVEL_MONO : BASS_LEVEL_STEREO));
			}

			DWORD level = MAKELONG((WORD)(min(levels[0], 1) * 32768), (WORD)(min(levels[1], 1) * 32768));
			left = LOWORD(level); // the left level
			right = HIWORD(level); // the right level

			RegSetValueEx(hKey, L"leftvol", 0, dwType, (LPBYTE)&left, sizeof(left));
			RegSetValueEx(hKey, L"rightvol", 0, dwType, (LPBYTE)&right, sizeof(right));
			RegCloseKey(hKey);
		}
	}
	catch (...) {
		crashmessage(L"VolumeMonWrite");
		throw;
	}
}

void debug_info() {
	try {
		HKEY hKey;
		long lResult;
		DWORD dwType = REG_DWORD;
		DWORD dwSize = sizeof(DWORD);
		DWORD level, left, right, handlecount;
		lResult = RegOpenKeyEx(HKEY_CURRENT_USER, L"Software\\Keppy's Synthesizer", 0, KEY_ALL_ACCESS, &hKey);
		int tempo;
		BASS_ChannelGetAttribute(KSStream, BASS_ATTRIB_CPU, &currentcpuusage0);
		if (currentengine == 2) currentcpuusageE0 = BASS_ASIO_GetCPU();
		else if (currentengine == 3) currentcpuusageE0 = BASS_WASAPI_GetCPU();

		PROCESS_MEMORY_COUNTERS_EX pmc;
		GetProcessMemoryInfo(GetCurrentProcess(), (PROCESS_MEMORY_COUNTERS*)&pmc, sizeof(pmc));
		GetProcessHandleCount(GetCurrentProcess(), &handlecount);
		SIZE_T ramusage = pmc.WorkingSetSize;
		uint64_t ramusageint = static_cast<uint64_t>(ramusage);

		int currentcpuusageint0 = int(currentcpuusage0);
		int currentcpuusageintE0 = int(currentcpuusageE0);
		clock_t end = clock();
		int64_t td1i = int64_t(GetUsage(start1, end));
		int64_t td2i = int64_t(GetUsage(start2, end));
		int64_t td3i = int64_t(GetUsage(start3, end));
		int64_t td4i = int64_t(GetUsage(start4, end));

		if (oldbuffermode == 1) td4i = 1;

		// Things
		RegSetValueEx(hKey, L"currentcpuusage0", 0, dwType, (LPBYTE)&currentcpuusageint0, sizeof(currentcpuusageint0));
		RegSetValueEx(hKey, L"currentcpuusageE0", 0, dwType, (LPBYTE)&currentcpuusageintE0, sizeof(currentcpuusageintE0));
		RegSetValueEx(hKey, L"handlecount", 0, dwType, (LPBYTE)&handlecount, sizeof(handlecount));

		dwType = REG_QWORD;
		dwSize = sizeof(QWORD);
		RegSetValueEx(hKey, L"ramusage", 0, dwType, (LPBYTE)&ramusageint, sizeof(ramusageint));
		RegSetValueEx(hKey, L"td1", 0, dwType, (LPBYTE)&td1i, sizeof(td1i));
		RegSetValueEx(hKey, L"td2", 0, dwType, (LPBYTE)&td2i, sizeof(td2i));
		RegSetValueEx(hKey, L"td3", 0, dwType, (LPBYTE)&td3i, sizeof(td3i));
		RegSetValueEx(hKey, L"td4", 0, dwType, (LPBYTE)&td4i, sizeof(td4i));

		if (currentengine == 2) {
			double rate = BASS_ASIO_GetRate();
			CheckUpASIO(ERRORCODE, L"KSGetRateASIO");
			long inlatency = BASS_ASIO_GetLatency(TRUE) * 1000 / rate;
			CheckUpASIO(ERRORCODE, L"KSGetInputLatencyASIO");
			long outlatency = BASS_ASIO_GetLatency(FALSE) * 1000 / rate;
			CheckUpASIO(ERRORCODE, L"KSGetOutputLatencyASIO");

			RegSetValueEx(hKey, L"asioinlatency", 0, dwType, (LPBYTE)&inlatency, sizeof(inlatency));
			RegSetValueEx(hKey, L"asiooutlatency", 0, dwType, (LPBYTE)&outlatency, sizeof(outlatency));
		}

		for (int i = 0; i <= 15; ++i) {
			cvvalues[i] = BASS_MIDI_StreamGetEvent(KSStream, i, MIDI_EVENT_VOICES);
			RegSetValueEx(hKey, cvnames[i], 0, dwType, (LPBYTE)&cvvalues[i], sizeof(cvvalues[i]));
		}

		RegCloseKey(hKey);
	}
	catch (...) {
		crashmessage(L"DebugRead");
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
		crashmessage(L"MixerCheck");
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
		crashmessage(L"RnCCheck");
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
		crashmessage(L"ReloadSFListCheck");
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
		crashmessage(L"HotKeysCheck");
		throw;
	}
}