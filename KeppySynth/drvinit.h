/*
Keppy's Synthesizer stream init
*/

void usleep(__int64 usec)
{
	HANDLE timer;
	LARGE_INTEGER ft;

	ft.QuadPart = -(10 * usec);

	timer = CreateWaitableTimer(NULL, TRUE, NULL);
	SetWaitableTimer(timer, &ft, 0, NULL, NULL, 0);
	WaitForSingleObject(timer, INFINITE);
	CloseHandle(timer);
}

void MT32SetInstruments() {
	if (mt32mode == 1) {
		BASS_MIDI_StreamEvent(KSStream, 0, MIDI_EVENT_PROGRAM, 0);
		BASS_MIDI_StreamEvent(KSStream, 1, MIDI_EVENT_PROGRAM, 36);
		BASS_MIDI_StreamEvent(KSStream, 2, MIDI_EVENT_PROGRAM, 48);
		BASS_MIDI_StreamEvent(KSStream, 3, MIDI_EVENT_PROGRAM, 61);
		BASS_MIDI_StreamEvent(KSStream, 4, MIDI_EVENT_PROGRAM, 66);
		BASS_MIDI_StreamEvent(KSStream, 5, MIDI_EVENT_PROGRAM, 96);
		BASS_MIDI_StreamEvent(KSStream, 6, MIDI_EVENT_PROGRAM, 76);
		BASS_MIDI_StreamEvent(KSStream, 7, MIDI_EVENT_PROGRAM, 76);
		BASS_MIDI_StreamEvent(KSStream, 8, MIDI_EVENT_PROGRAM, 55);
		BASS_MIDI_StreamEvent(KSStream, 9, MIDI_EVENT_PROGRAM, 35);
		BASS_MIDI_StreamEvent(KSStream, 10, MIDI_EVENT_PROGRAM, 0);
		BASS_MIDI_StreamEvent(KSStream, 11, MIDI_EVENT_PROGRAM, 0);
		BASS_MIDI_StreamEvent(KSStream, 12, MIDI_EVENT_PROGRAM, 0);
		BASS_MIDI_StreamEvent(KSStream, 13, MIDI_EVENT_PROGRAM, 0);
		BASS_MIDI_StreamEvent(KSStream, 14, MIDI_EVENT_PROGRAM, 0);
		BASS_MIDI_StreamEvent(KSStream, 15, MIDI_EVENT_PROGRAM, 0);
	}
	else {
		if (overrideinstruments == 1) {
			for (int i = 0; i <= 15; ++i) {
				BASS_MIDI_StreamEvent(KSStream, i, MIDI_EVENT_BANK, cbank[i]);
				BASS_MIDI_StreamEvent(KSStream, i, MIDI_EVENT_PROGRAM, cpreset[i]);
			}
		}
	}
}

DWORD WINAPI pipesfill(LPVOID lpV) {
	hThreadDBGRunning = TRUE;
	PrintToConsole(FOREGROUND_RED, 1, "Initializing debug pipe thread...");
	while (stop_thread == FALSE) {
		try {
			SendDebugDataToPipe();
		}
		catch (...) {
			CrashMessage(L"DbgPipe");
			throw;
		}
		usleep(100);
	}
	PrintToConsole(FOREGROUND_RED, 1, "Closing debug pipe thread...");
	hThreadDBGRunning = FALSE;
	CloseHandle(hThreadDBG);
	hThreadDBG = NULL;
	return 0;
}

DWORD WINAPI notescatcher(LPVOID lpV) {
	hThread4Running = TRUE;
	PrintToConsole(FOREGROUND_RED, 1, "Initializing notes catcher thread...");
	while (stop_thread == FALSE) {
		try {
			start4 = TimeNow();

			MT32SetInstruments();
			PlayBufferedData();

			if (capframerate == 1) usleep(16666); else usleep(1);
			if (currentengine == 1 && oldbuffermode == 1) { break; }
		}
		catch (...) {
			CrashMessage(L"NotesCatcher");
			throw;
		}
	}
	PrintToConsole(FOREGROUND_RED, 1, "Closing notes catcher thread...");
	hThread4Running = FALSE;
	CloseHandle(hThread4);
	hThread4 = NULL;
	return 0;
}

DWORD WINAPI settingsload(LPVOID lpV) {
	hThread3Running = TRUE;
	PrintToConsole(FOREGROUND_RED, 1, "Initializing settings thread...");
	while (stop_thread == FALSE) {
		try {
			start3 = TimeNow();
			LoadSettingsRT();
			Panic();
			keybindings();
			WatchdogCheck();
			mixervoid();
			RevbNChor();
			usleep(50000);
		}
		catch (...) {
			CrashMessage(L"SettingsLoad");
			throw;
		}
	}
	PrintToConsole(FOREGROUND_RED, 1, "Closing settings thread...");
	hThread3Running = FALSE;
	CloseHandle(hThread3);
	hThread3 = NULL;
	return 0;
}

void InitializeNotesCatcherThread() {
	if (hThread4 == NULL) {
		hThread4 = CreateThread(NULL, 0, (LPTHREAD_START_ROUTINE)notescatcher, NULL, 0, (LPDWORD)thrdaddr4);
		SetThreadPriority(hThread4, prioval[driverprio]);
	}
}

DWORD WINAPI audioengine(LPVOID lpParam) {
	hThread2Running = TRUE;
	PrintToConsole(FOREGROUND_RED, 1, "Initializing audio rendering thread for DS/Enc...");
	while (stop_thread == FALSE) {
		try {
			start2 = TimeNow();
			if (currentengine < 2) {
				if (reset_synth != 0) {
					reset_synth = 0;
					BASS_MIDI_StreamEvent(KSStream, 0, MIDI_EVENT_SYSTEM, MIDI_SYSTEM_DEFAULT);
				}

				if (currentengine == 0) AudioRender();
				else {
					BASS_ChannelUpdate(KSStream, 0);
					usleep(rco);
				}

				if (oldbuffermode == 1) {
					MT32SetInstruments();
					PlayBufferedData();
				}
				else InitializeNotesCatcherThread();
			}
			else break;
		}
		catch (...) {
			CrashMessage(L"AudioEngineRender");
			throw;
		}
	}
	PrintToConsole(FOREGROUND_RED, 1, "Closing audio rendering thread for DS/Enc...");
	hThread2Running = FALSE;
	CloseHandle(hThread2);
	hThread2 = NULL;
	return 0;
}

DWORD CALLBACK ASIOProc(BOOL input, DWORD channel, void *buffer, DWORD length, void *user)
{
	start2 = clock();
	DWORD data = BASS_ChannelGetData(KSStream, buffer, length);
	if (data == -1) return 0;
	return data;
}

void InitializeStreamForExternalEngine(INT32 mixfreq) {
	bool isdecode = FALSE;

	PrintToConsole(FOREGROUND_RED, 1, "Creating stream for external engine...");
	if (currentengine == 1 || currentengine == 3) {
		if (defaultoutput == 1) {
			isdecode = TRUE;
		}
		else isdecode = FALSE;

	}
	else {
		isdecode = TRUE;
	}

	if (KSStream) BASS_StreamFree(KSStream);

	KSStream = BASS_MIDI_StreamCreate(16,
		(isdecode ? BASS_STREAM_DECODE : 0) | (sysresetignore ? BASS_MIDI_NOSYSRESET : 0) | (monorendering ? BASS_SAMPLE_MONO : 0) |
		AudioRenderingType(floatrendering) | (noteoff1 ? BASS_MIDI_NOTEOFF1 : 0) | (nofx ? BASS_MIDI_NOFX : 0) | (sinc ? BASS_MIDI_SINCINTER : 0),
		mixfreq);
	CheckUp(ERRORCODE, L"KSStreamCreateDEC", TRUE);
	
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
		CheckUp(ERRORCODE, L"EncoderStart", TRUE);
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

void InitializeBASSFinal() {
	currentengine = 1;

	BASS_SetConfig(BASS_CONFIG_UPDATETHREADS, 0);
	BASS_SetConfig(BASS_CONFIG_UPDATEPERIOD, 0);
	BASS_GetInfo(&info);
	BASS_SetConfig(BASS_CONFIG_BUFFER, frames);
	InitializeStreamForExternalEngine(frequency);
	CheckUp(ERRORCODE, L"KSStreamCreateDS", TRUE);
	if (DSoutput != 0)
	{
		BASS_ChannelPlay(KSStream, false);
		CheckUp(ERRORCODE, L"KSChannelPlayDS", TRUE);
	}
}

void InitializeWAVEnc() {
	currentengine = 0;

	InitializeStreamForExternalEngine(frequency);
	InitializeBASSEnc();
	CheckUp(ERRORCODE, L"KSInitEnc", TRUE);
}

void InitializeASIO() {
	currentengine = 2;

	InitializeStreamForExternalEngine(frequency);
	if (BASS_ASIO_Init(defaultAoutput, BASS_ASIO_THREAD | BASS_ASIO_JOINORDER)) {
		BASS_ASIO_SetRate(frequency);
		CheckUpASIO(ERRORCODE, L"KSFormatASIO", FALSE);
		BASS_ASIO_ChannelSetFormat(FALSE, 0, BASS_ASIO_FORMAT_FLOAT);
		if (monorendering == 1) BASS_ASIO_ChannelEnableMirror(1, FALSE, 0);
		CheckUpASIO(ERRORCODE, L"KSChanSetMono", TRUE);
		BASS_ASIO_ChannelSetFormat(FALSE, 1, BASS_ASIO_FORMAT_FLOAT);
		CheckUpASIO(ERRORCODE, L"KSChanSetFormatASIO", TRUE);
		if (monorendering == 1) BASS_ASIO_ChannelSetRate(FALSE, 0, frequency / 2);
		else BASS_ASIO_ChannelSetRate(FALSE, 0, frequency);
		CheckUpASIO(ERRORCODE, L"KSChanSetFreqASIO", TRUE);
		BASS_ASIO_ChannelEnable(FALSE, 0, ASIOProc, 0);
		BASS_ASIO_ChannelJoin(FALSE, 1, 0);
		CheckUpASIO(ERRORCODE, L"KSChanEnableASIO", TRUE);
		BASS_ASIO_Start(0, 2);
		CheckUpASIO(ERRORCODE, L"KSStartASIO", TRUE);
	}
	else {
		CheckUpASIO(ERRORCODE, L"KSInitASIO", TRUE);
		InitializeBASSFinal();
	}
}

bool InitializeBASS(bool restart) {
	PrintToConsole(FOREGROUND_RED, 1, "The driver is now initializing BASS. Please wait...");

	bool init = FALSE;
	bool isds = FALSE;

    WASAPIoutput = defaultWoutput - 1;
	DSoutput = defaultoutput - 1;

	if (currentengine == 1 || currentengine == 3) isds = TRUE; // DirectSound or WASAPI internal, init BASS for output device
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
	int flags;
	if (currentengine == 1) flags = BASS_DEVICE_DSOUND;
	else flags = 0;

	Retry:
	PrintToConsole(FOREGROUND_RED, 1, "Initializing BASS...");
	init = BASS_Init(isds ? DSoutput : 0, frequency, flags, 0, NULL);
	CheckUp(ERRORCODE, L"BASSInit", TRUE);

	if (currentengine == 0) {
		InitializeWAVEnc();
	}
	else if (currentengine == 1 || currentengine == 3) {
		InitializeBASSFinal();
	}
	else if (currentengine == 2) {
		InitializeASIO();
	}

	if (!KSStream) {
		// Free BASS
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
		CheckUp(ERRORCODE, L"KSAttributes1", TRUE);
		BASS_ChannelSetAttribute(KSStream, BASS_ATTRIB_MIDI_CPU, maxcpu);
		CheckUp(ERRORCODE, L"KSAttributes2", TRUE);
		BASS_ChannelSetAttribute(KSStream, BASS_ATTRIB_MIDI_KILL, fadeoutdisable);
		CheckUp(ERRORCODE, L"KSAttributes3", FALSE);
	}

	ChVolume = BASS_ChannelSetFX(KSStream, BASS_FX_VOLUME, 1);
	CheckUp(ERRORCODE, L"KSVolFX", FALSE);

	return init;
}

void CloseThreads() {
	stop_thread = TRUE;

	WaitForSingleObject(hThread2, INFINITE);
	CloseHandle(hThread2);
	hThread2 = NULL;

	WaitForSingleObject(hThread3, INFINITE);
	CloseHandle(hThread3);
	hThread3 = NULL;

	WaitForSingleObject(hThread4, INFINITE);
	CloseHandle(hThread4);
	hThread4 = NULL;

	stop_thread = FALSE;
}

int CreateThreads(bool startup) {
	if (startup == TRUE) SetEvent(load_sfevent);

	PrintToConsole(FOREGROUND_RED, 1, "Creating threads...");

	reset_synth = 0;
	hThread2 = CreateThread(NULL, 0, (LPTHREAD_START_ROUTINE)audioengine, NULL, 0, (LPDWORD)thrdaddr2);
	SetThreadPriority(hThread2, prioval[driverprio]);
	hThread3 = CreateThread(NULL, 0, (LPTHREAD_START_ROUTINE)settingsload, NULL, 0, (LPDWORD)thrdaddr3);
	SetThreadPriority(hThread3, prioval[driverprio]);
	if (currentengine > 1) {
		hThread4 = CreateThread(NULL, 0, (LPTHREAD_START_ROUTINE)notescatcher, NULL, 0, (LPDWORD)thrdaddr4);
		SetThreadPriority(hThread4, prioval[driverprio]);
	}
	hThreadDBG = CreateThread(NULL, 0, (LPTHREAD_START_ROUTINE)pipesfill, NULL, 0, (LPDWORD)thrdaddrDBG);
	SetThreadPriority(hThreadDBG, prioval[driverprio]);

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
	FillContentDebug(TRUE, 0.0f, 0, 0, FALSE, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0);
	FlushFileBuffers(hPipe);
	CloseHandle(hPipe);

	if (KSStream)
	{
		ResetSynth(0);
		BASS_ASIO_ChannelReset(FALSE, -1, BASS_ASIO_RESET_ENABLE | BASS_ASIO_RESET_JOIN);
		CheckUp(ERRORCODE, L"KSResetChannelASIO", TRUE);
		BASS_ASIO_Stop();
		CheckUp(ERRORCODE, L"KSStopASIO", TRUE);
		BASS_StreamFree(KSStream);
		CheckUp(ERRORCODE, L"KSStreamFreeBASS", TRUE);
		KSStream = 0;
	}
	if (bassmidi) {
		FreeFonts(0);
		FreeLibrary(bassmidi);
		bassmidi = 0;
	}
	if (bassenc) {
		BASS_Encode_Stop(KSStream);
		CheckUp(ERRORCODE, L"KSFreeBASSenc", TRUE);
		FreeLibrary(bassenc);
		bassenc = 0;
	}
	if (bass) {
		BASS_Free();
		CheckUp(ERRORCODE, L"KSFreeBASS", TRUE);
		FreeLibrary(bass);
		bass = 0;
	}
	if (bassasio) {
		BASS_ASIO_Free();
		CheckUp(ERRORCODE, L"KSFreeASIO", TRUE);
		FreeLibrary(bassasio);
		bassasio = 0;
	}
	if (com_initialized) {
		CoUninitialize();
		com_initialized = FALSE;
	}
}