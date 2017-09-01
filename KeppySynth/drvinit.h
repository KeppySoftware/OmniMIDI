/*
Keppy's Synthesizer stream init
*/

DWORD WINAPI notescatcher(LPVOID lpV) {
	hThread4Running = TRUE;
	PrintToConsole(FOREGROUND_RED, 1, "Initializing notes catcher thread...");
	while (stop_thread == 0) {
		try {
			start4 = clock();
			bmsyn_play_some_data();

			if (capframerate == 1) Sleep(16); else Sleep(1);
			if (currentengine < 2) { if (oldbuffermode == 1) break; }
		}
		catch (...) {
			crashmessage(L"NotesCatcher");
			throw;
		}
	}
	PrintToConsole(FOREGROUND_RED, 1, "Closing notes catcher thread...");
	hThread4Running = FALSE;
	ExitThread(0);
	return 0;
}

unsigned WINAPI settingsload(LPVOID lpV) {
	hThread3Running = TRUE;
	PrintToConsole(FOREGROUND_RED, 1, "Initializing settings thread...");
	while (stop_thread == 0) {
		try {
			start3 = clock();
			realtime_load_settings();
			Panic();
			WatchdogCheck();
			mixervoid();
			RevbNChor();
			Sleep(100);
		}
		catch (...) {
			crashmessage(L"SettingsLoad");
			throw;
		}
	}
	PrintToConsole(FOREGROUND_RED, 1, "Closing settings thread...");
	hThread3Running = FALSE;
	ExitThread(0);
	return 0;
}

void InitializeNotesCatcherThread() {
	if (hThread4 == NULL) {
		hThread4 = CreateThread(NULL, 0, (LPTHREAD_START_ROUTINE)notescatcher, NULL, 0, NULL);
		SetThreadPriority(hThread4, prioval[driverprio]);
	}
}

unsigned WINAPI audioengine(LPVOID lpParam) {
	hThread2Running = TRUE;
	PrintToConsole(FOREGROUND_RED, 1, "Initializing audio rendering thread for DS/Enc...");
	while (stop_thread == 0) {
		try {
			start2 = clock();
			if (currentengine < 2) {
				if (reset_synth != 0) {
					reset_synth = 0;
					BASS_MIDI_StreamEvent(KSStream, 0, MIDI_EVENT_SYSTEM, MIDI_SYSTEM_DEFAULT);
				}

				if (oldbuffermode == 1) bmsyn_play_some_data();
				else InitializeNotesCatcherThread();

				if (currentengine == 0) AudioRender();
				else {
					if (DSoutput != 0) {
						BASS_ChannelUpdate(KSStream, 0);
					}
					Sleep(rco);
				}
			}
			else break;
		}
		catch (...) {
			crashmessage(L"AudioEngineRender");
			throw;
		}
	}
	PrintToConsole(FOREGROUND_RED, 1, "Closing audio rendering thread for DS/Enc...");
	hThread2Running = FALSE;
	ExitThread(0);
	return 0;
}

DWORD CALLBACK ASIOProc(BOOL input, DWORD channel, void *buffer, DWORD length, void *user)
{
	start2 = clock();
	DWORD data = BASS_ChannelGetData(KSStream, buffer, length);
	if (data == -1) return 0;
	return data;
}

DWORD CALLBACK WASAPIProc(void *buffer, DWORD length, void *user)
{
	start2 = clock();
	DWORD data = BASS_ChannelGetData(KSStream, buffer, length);
	if (data == -1) return 0;
	return data;
}

void InitializeStreamForExternalEngine(INT32 mixfreq) {
	bool isdecode = FALSE;

	PrintToConsole(FOREGROUND_RED, 1, "Working...");

	if (currentengine == 1) {
		if (defaultoutput == 1) isdecode = TRUE;
		else isdecode = FALSE;
	}
	else {
		isdecode = TRUE;
	}

	if (KSStream) BASS_StreamFree(KSStream);
	KSStream = BASS_MIDI_StreamCreate(16, (isdecode ? BASS_STREAM_DECODE : 0) | (sysresetignore ? BASS_MIDI_NOSYSRESET : 0) | (monorendering ? BASS_SAMPLE_MONO : 0) | AudioRenderingType(floatrendering) | (noteoff1 ? BASS_MIDI_NOTEOFF1 : 0) | (nofx ? BASS_MIDI_NOFX : 0) | (sinc ? BASS_MIDI_SINCINTER : 0), mixfreq);
	CheckUp(ERRORCODE, L"KSStreamCreateDEC");
	
	PrintToConsole(FOREGROUND_RED, 1, "External engine stream enabled.");
}

void InitializeBASSEnc() {
	PrintToConsole(FOREGROUND_RED, 1, "Opening BASSenc stream...");

	// Cast restart values
	std::wostringstream rv;
	rv << restartvalue;

	typedef std::basic_string<TCHAR> tstring;
	TCHAR encpath[MAX_PATH];
	TCHAR confpath[MAX_PATH];
	TCHAR buffer[MAX_PATH] = { 0 };
	TCHAR * out;
	DWORD bufSize = sizeof(buffer) / sizeof(*buffer);
	if (GetModuleFileName(NULL, buffer, bufSize) == bufSize) {}
	out = PathFindFileName(buffer);
	std::wstring stemp = tstring(out) + L" - Keppy's Synthesizer Output File (Restart N°" + rv.str() + L").wav";
	LPCWSTR result2 = stemp.c_str();
	HKEY hKey = 0;
	DWORD cbValueLength = sizeof(confpath);
	DWORD dwType = REG_SZ;
	RegOpenKeyEx(HKEY_CURRENT_USER, L"Software\\Keppy's Synthesizer\\Settings", 0, KEY_ALL_ACCESS, &hKey);
	if (RegQueryValueEx(hKey, L"lastexportfolder", NULL, &dwType, reinterpret_cast<LPBYTE>(&confpath), &cbValueLength) == ERROR_FILE_NOT_FOUND) {
		if (SUCCEEDED(SHGetFolderPath(NULL, CSIDL_DESKTOP, NULL, 0, encpath)))
		{
			PathAppend(encpath, result2);
		}
	}
	else {
		PathAppend(encpath, confpath);
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
		CheckUp(ERRORCODE, L"EncoderStart");
		break;
	case IDNO:
		TCHAR configuratorapp[MAX_PATH];
		if (SUCCEEDED(SHGetFolderPath(NULL, CSIDL_SYSTEMX86, NULL, 0, configuratorapp)))
		{
			PathAppend(configuratorapp, _T("\\keppydrv\\KeppyDriverConfigurator.exe"));
			ShellExecute(NULL, L"open", configuratorapp, L"/AT", NULL, SW_SHOWNORMAL);
			delete configuratorapp;
		}
	}

	PrintToConsole(FOREGROUND_RED, 1, "BASSenc ready.");
}

void GetWASAPIDevice() {
	HKEY hKey;
	long lResult;
	DWORD dwType = REG_DWORD;
	DWORD dwSize = sizeof(DWORD);
	lResult = RegOpenKeyEx(HKEY_CURRENT_USER, L"Software\\Keppy's Synthesizer\\Settings", 0, KEY_ALL_ACCESS, &hKey);
	RegQueryValueEx(hKey, L"defaultWdev", NULL, &dwType, (LPBYTE)&defaultWoutput, &dwSize);
	RegCloseKey(hKey);
}

void ASIOControlPanel() {
	if (GetAsyncKeyState(VK_MENU) & GetAsyncKeyState(0x39) & 0x8000) {
		if (currentengine == 2) {
			BASS_ASIO_ControlPanel();
		}
	}
}

void InitializeWASAPI() {
	currentengine = 3;

	GetWASAPIDevice();

	BASS_WASAPI_Init(WASAPIoutput, 0, 0, BASS_WASAPI_BUFFER, 0, 0, NULL, NULL);
	CheckUp(ERRORCODE, L"KSWASAPIInitInfo");
	BASS_WASAPI_GetDeviceInfo(BASS_WASAPI_GetDevice(), &infoDW);
	CheckUp(ERRORCODE, L"KSWASAPIGetDeviceInfo");
	BASS_WASAPI_GetInfo(&infoW);
	CheckUp(ERRORCODE, L"KSWASAPIGetBufInfo");
	BASS_WASAPI_Free();
	CheckUp(ERRORCODE, L"KSWASAPIFreeInfo");
	
	InitializeStreamForExternalEngine(infoDW.mixfreq);

	if (BASS_WASAPI_Init(WASAPIoutput, 0, 2,
		BASS_WASAPI_BUFFER | (wasapiex ? BASS_WASAPI_EXCLUSIVE : BASS_WASAPI_EVENT),
		(wasapiex ? ((float)frames / 1000.0f) : infoW.buflen + 5),
		0, WASAPIProc, NULL)) {
		CheckUp(ERRORCODE, L"KSInitWASAPI");
		BASS_WASAPI_Start();
		CheckUp(ERRORCODE, L"KSStartStreamWASAPI");
	}
	else {
		MessageBox(NULL, L"WASAPI is unavailable with the current device.\n\nChange the device through the configurator, then try again.\nTo change it, please open the configurator, and go to \"More settings > Advanced audio settings > Change default audio output\", then, after you're done, restart the MIDI application.\n\nFalling back to BASSEnc...\nPress OK to continue.", L"Keppy's Synthesizer - Can not open WASAPI device", MB_OK | MB_ICONERROR);
		currentengine = 0;
		InitializeStreamForExternalEngine(frequency);
		InitializeBASSEnc();
		CheckUp(ERRORCODE, L"KSInitEnc");
	}
}

void InitializeDirectSound() {
	currentengine = 1;

	BASS_SetConfig(BASS_CONFIG_UPDATETHREADS, 0);
	BASS_SetConfig(BASS_CONFIG_UPDATEPERIOD, 0);
	BASS_GetInfo(&info);
	BASS_SetConfig(BASS_CONFIG_BUFFER, frames);
	InitializeStreamForExternalEngine(frequency);
	CheckUp(ERRORCODE, L"KSStreamCreateDS");
	if (DSoutput != 0)
	{
		BASS_ChannelPlay(KSStream, false);
		CheckUp(ERRORCODE, L"KSChannelPlayDS");
	}
}

void InitializeWAVEnc() {
	currentengine = 0;

	InitializeStreamForExternalEngine(frequency);
	InitializeBASSEnc();
	CheckUp(ERRORCODE, L"KSInitEnc");
}

void InitializeASIO() {
	currentengine = 2;

	InitializeStreamForExternalEngine(frequency);
	if (BASS_ASIO_Init(defaultAoutput, BASS_ASIO_THREAD | BASS_ASIO_JOINORDER)) {
		CheckUpASIO(ERRORCODE, L"KSInitASIO");
		BASS_ASIO_SetRate(frequency);
		CheckUpASIO(ERRORCODE, L"KSFormatASIO");
		BASS_ASIO_ChannelSetFormat(FALSE, 0, BASS_ASIO_FORMAT_FLOAT);
		if (monorendering == 1) BASS_ASIO_ChannelEnableMirror(1, FALSE, 0);
		BASS_ASIO_ChannelSetFormat(FALSE, 1, BASS_ASIO_FORMAT_FLOAT);
		CheckUpASIO(ERRORCODE, L"KSChanSetFormatASIO");
		if (monorendering == 1) BASS_ASIO_ChannelSetRate(FALSE, 0, frequency / 2);
		else BASS_ASIO_ChannelSetRate(FALSE, 0, frequency);
		CheckUpASIO(ERRORCODE, L"KSChanSetFreqASIO");
		BASS_ASIO_ChannelEnable(FALSE, 0, ASIOProc, 0);
		BASS_ASIO_ChannelJoin(FALSE, 1, 0);
		CheckUpASIO(ERRORCODE, L"KSChanEnableASIO");
		BASS_ASIO_Start(0, 2);
		CheckUpASIO(ERRORCODE, L"KSStartASIO");
	}
	else {
		CheckUpASIO(ERRORCODE, L"KSInitASIO");
		MessageBox(NULL, L"ASIO is unavailable with the current device.\n\nChange the device through the configurator, then try again.\nTo change it, please open the configurator, and go to \"More settings > Advanced audio settings > Change default audio output\", then, after you're done, restart the MIDI application.\n\nFalling back to WASAPI...\nPress OK to continue.", L"Keppy's Synthesizer - Can not open ASIO device", MB_OK | MB_ICONERROR);
		InitializeWASAPI();
	}
}

bool InitializeBASS(bool restart) {
	PrintToConsole(FOREGROUND_RED, 1, "The driver is now initializing BASS. Please wait...");

	bool init = FALSE;
	bool isds = FALSE;

    WASAPIoutput = defaultWoutput - 1;
	DSoutput = defaultoutput - 1;

	if (currentengine == 1) isds = TRUE; // DirectSound, init BASS for output device
	else if (currentengine == 3)  {
		if (monorendering != 0) {
			monorendering = 0;
			MessageBox(NULL, L"WASAPI doesn't support monophonic rendering.\n\nClick OK to continue in stereophonic mode.", L"Keppy's Synthesizer - WASAPI error", MB_OK | MB_ICONERROR);
		}
	}

	PrintToConsole(FOREGROUND_RED, 1, "Settings are valid, continue...");

	if (restart == TRUE) {
		PrintToConsole(FOREGROUND_RED, 1, "The driver requested to restart the stream.");
		if (currentengine == 0) restartvalue++;
	}

	// Free BASS
	BASS_WASAPI_Stop(TRUE);
	PrintToConsole(FOREGROUND_RED, 1, "BASSWASAPI stopped.");

	BASS_WASAPI_Free();
	PrintToConsole(FOREGROUND_RED, 1, "BASSWASAPI freed.");

	BASS_ASIO_Stop();
	PrintToConsole(FOREGROUND_RED, 1, "BASSASIO stopped.");

	BASS_ASIO_Free();
	PrintToConsole(FOREGROUND_RED, 1, "BASSASIO freed.");

	BASS_ChannelStop(KSStream);
	PrintToConsole(FOREGROUND_RED, 1, "BASS stream stopped.");

	BASS_StreamFree(KSStream);
	PrintToConsole(FOREGROUND_RED, 1, "BASS stream freed.");

	BASS_Stop();
	PrintToConsole(FOREGROUND_RED, 1, "BASS stopped.");

	BASS_Free();
	PrintToConsole(FOREGROUND_RED, 1, "BASS freed.");

	// Init BASS
	PrintToConsole(FOREGROUND_RED, 1, "Initializing BASS...");
	init = BASS_Init(isds ? DSoutput : 0, frequency, 0, 0, NULL);
	CheckUp(ERRORCODE, L"BASSInit");

	if (currentengine == 0) {
		InitializeWAVEnc();
	}
	else if (currentengine == 1) {
		InitializeDirectSound();
	}
	else if (currentengine == 2) {
		InitializeASIO();
	}
	else if (currentengine == 3) {
		InitializeWASAPI();
	}
	
	if (!KSStream) {
		// Free BASS
		BASS_WASAPI_Stop(TRUE);
		PrintToConsole(FOREGROUND_RED, 1, "BASSWASAPI stopped.");

		BASS_WASAPI_Free();
		PrintToConsole(FOREGROUND_RED, 1, "BASSWASAPI terminated.");

		BASS_ASIO_Stop();
		PrintToConsole(FOREGROUND_RED, 1, "BASSASIO stopped.");

		BASS_ASIO_Free();
		PrintToConsole(FOREGROUND_RED, 1, "BASSASIO terminated.");

		BASS_StreamFree(KSStream);
		PrintToConsole(FOREGROUND_RED, 1, "BASS stream freed.");

		BASS_Free();
		PrintToConsole(FOREGROUND_RED, 1, "BASS freed.");

		KSStream = 0;
		PrintToConsole(FOREGROUND_RED, 1, "Failed to open BASS stream.");
		return false;
	}
	else {
		BASS_ChannelSetAttribute(KSStream, BASS_ATTRIB_MIDI_VOICES, midivoices);
		CheckUp(ERRORCODE, L"KSAttributes1");
		BASS_ChannelSetAttribute(KSStream, BASS_ATTRIB_MIDI_CPU, maxcpu);
		CheckUp(ERRORCODE, L"KSAttributes2");
		BASS_ChannelSetAttribute(KSStream, BASS_ATTRIB_MIDI_KILL, fadeoutdisable);
		CheckUp(ERRORCODE, L"KSAttributes3");
	}
	return init;
}

void InitializeBASSVST() {
	USES_CONVERSION;
	TCHAR loudmaxdll[MAX_PATH];
	TCHAR loudmaxdll64[MAX_PATH];
	SHGetFolderPath(NULL, CSIDL_PROFILE, NULL, 0, loudmaxdll);
	SHGetFolderPath(NULL, CSIDL_PROFILE, NULL, 0, loudmaxdll64);
	PathAppend(loudmaxdll, _T("\\Keppy's Synthesizer\\LoudMax.dll"));
	PathAppend(loudmaxdll64, _T("\\Keppy's Synthesizer\\LoudMax64.dll"));
    char *LMDLL = T2A(loudmaxdll);
	char *LMDLL64 = T2A(loudmaxdll64);
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
}

void CloseThreads() {
	stop_thread = 1;

	WaitForSingleObject(hThread2, INFINITE);
	CloseHandle(hThread2);
	hThread2 = NULL;

	WaitForSingleObject(hThread3, INFINITE);
	CloseHandle(hThread3);
	hThread3 = NULL;

	WaitForSingleObject(hThread4, INFINITE);
	CloseHandle(hThread4);
	hThread4 = NULL;

	stop_thread = 0;
}

int CreateThreads(bool startup) {
	if (startup == TRUE) SetEvent(load_sfevent);

	PrintToConsole(FOREGROUND_RED, 1, "Creating threads...");

	reset_synth = 0;
	hThread2 = CreateThread(NULL, 0, (LPTHREAD_START_ROUTINE)audioengine, NULL, 0, NULL);
	SetThreadPriority(hThread2, prioval[driverprio]);
	hThread3 = CreateThread(NULL, 0, (LPTHREAD_START_ROUTINE)settingsload, NULL, 0, NULL);
	SetThreadPriority(hThread3, prioval[driverprio]);
	if (currentengine > 1) {
		hThread4 = CreateThread(NULL, 0, (LPTHREAD_START_ROUTINE)notescatcher, NULL, 0, NULL);
		SetThreadPriority(hThread4, prioval[driverprio]);
	}

	PrintToConsole(FOREGROUND_RED, 1, "Threads are now active.");
	return 1;
}

void LoadSoundFontsToStream() {
	PrintToConsole(FOREGROUND_RED, 1, "Loading soundfonts...");
	if (LoadSoundfontStartup() == TRUE) {
		PrintToConsole(FOREGROUND_RED, 1, "Default list for app loaded.");
	}
	else {
		LoadSoundfont(defaultsflist);
		PrintToConsole(FOREGROUND_RED, 1, "Default global list loaded.");
	}
}

void SetUpStream() {
	PrintToConsole(FOREGROUND_RED, 1, "Preparing stream...");
	BASS_ChannelSetAttribute(KSStream, BASS_ATTRIB_MIDI_CHANS, 16);
	BASS_MIDI_StreamEvent(KSStream, 0, MIDI_EVENT_SYSTEM, MIDI_SYSTEM_DEFAULT);
	BASS_MIDI_StreamEvent(KSStream, 9, MIDI_EVENT_DRUMS, 1);
}

void FreeUpLibraries() {
	if (KSStream)
	{
		ResetSynth(0);
		BASS_WASAPI_Stop(true);
		CheckUp(ERRORCODE, L"KSStopWASAPI");
		BASS_ASIO_ChannelReset(FALSE, -1, BASS_ASIO_RESET_ENABLE | BASS_ASIO_RESET_JOIN);
		CheckUp(ERRORCODE, L"KSResetChannelASIO");
		BASS_ASIO_Stop();
		CheckUp(ERRORCODE, L"KSStopASIO");
		BASS_StreamFree(KSStream);
		CheckUp(ERRORCODE, L"KSStreamFreeBASS");
		KSStream = 0;
	}
	if (bassmidi) {
		FreeFonts(0);
		FreeLibrary(bassmidi);
		bassmidi = 0;
	}
	if (bassenc) {
		BASS_Encode_Stop(KSStream);
		CheckUp(ERRORCODE, L"KSFreeBASSenc");
		FreeLibrary(bassenc);
		bassenc = 0;
	}
	if (bass) {
		BASS_Free();
		CheckUp(ERRORCODE, L"KSFreeBASS");
		FreeLibrary(bass);
		bass = 0;
	}
	if (bassasio) {
		BASS_ASIO_Free();
		CheckUp(ERRORCODE, L"KSFreeASIO");
		FreeLibrary(bassasio);
		bassasio = 0;
	}
	if (basswasapi) {
		BASS_WASAPI_Free();
		CheckUp(ERRORCODE, L"KSFreeWASAPI");
		FreeLibrary(basswasapi);
		bass = 0;
	}
	if (com_initialized) {
		CoUninitialize();
		com_initialized = FALSE;
	}
}