/*
Keppy's Synthesizer stream init
*/

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

DWORD WINAPI DebugThread(LPVOID lpV) {
	PrintToConsole(FOREGROUND_RED, 1, "Initializing debug pipe thread...");
	while (!stop_rtthread) {
		SendDebugDataToPipe();
		_WAIT;
	}
	PrintToConsole(FOREGROUND_RED, 1, "Closing debug pipe thread...");
	CloseHandle(DThread);
	DThread = NULL;
	return 0;
}

DWORD WINAPI EventsProcesser(LPVOID lpV) {
	PrintToConsole(FOREGROUND_RED, 1, "Initializing notes catcher thread...");
	try {
		while (!stop_thread) {
			start4 = TimeNow();

			if (oldbuffermode) break;

			MT32SetInstruments();
			if (_PlayBufData()) _LWAIT;

			if (capframerate) _CFRWAIT;
		}
	}
	catch (...) {
		CrashMessage(L"NotesCatcherThread");
		throw;
	}
	PrintToConsole(FOREGROUND_RED, 1, "Closing notes catcher thread...");
	CloseHandle(EPThread);
	EPThread = NULL;
	return 0;
}

DWORD WINAPI RTSettings(LPVOID lpV) {
	PrintToConsole(FOREGROUND_RED, 1, "Initializing settings thread...");
	try {
		while (!stop_thread) {
			start3 = TimeNow();
			LoadSettingsRT();
			Panic();
			keybindings();
			WatchdogCheck();
			mixervoid();
			RevbNChor();
			_VLWAIT;
		}
	}
	catch (...) {
		CrashMessage(L"RTSettingsThread");
		throw;
	}
	PrintToConsole(FOREGROUND_RED, 1, "Closing settings thread...");
	CloseHandle(RTSThread);
	RTSThread = NULL;
	return 0;
}

void InitializeNotesCatcherThread() {
	if (EPThread == NULL) {
		EPThread = CreateThread(NULL, 0, (LPTHREAD_START_ROUTINE)EventsProcesser, NULL, 0, (LPDWORD)EPThreadAddress);
		SetThreadPriority(EPThread, prioval[driverprio]);
	}
}

DWORD WINAPI AudioThread(LPVOID lpParam) {
	PrintToConsole(FOREGROUND_RED, 1, "Initializing audio rendering thread for DS/Enc...");
	try {
		if (currentengine != ASIO_ENGINE) {
			while (!stop_thread) {
				start2 = TimeNow();
				if (reset_synth) {
					reset_synth = 0;
					BASS_MIDI_StreamEvent(KSStream, 0, MIDI_EVENT_SYSTEM, MIDI_SYSTEM_DEFAULT);
				}

				if (currentengine == AUDTOWAV) AudioRender();
				else BASS_ChannelUpdate(KSStream, 0);

				if (oldbuffermode) {
					MT32SetInstruments();
					_PlayBufDataChk();
				}
				else if (!EPThread) InitializeNotesCatcherThread();

				_FWAIT;
			}
		}
	}
	catch (...) {
		CrashMessage(L"AudioEngineThread");
		throw;
	}
	PrintToConsole(FOREGROUND_RED, 1, "Closing audio rendering thread for DS/Enc...");
	CloseHandle(ATThread);
	ATThread = NULL;
	return 0;
}

DWORD CALLBACK ASIOProc(BOOL input, DWORD channel, void *buffer, DWORD length, void *user)
{
	start2 = TimeNow();
	if (reset_synth) {
		reset_synth = 0;
		BASS_MIDI_StreamEvent(KSStream, 0, MIDI_EVENT_SYSTEM, MIDI_SYSTEM_DEFAULT);
	}

	DWORD data = BASS_ChannelGetData(KSStream, buffer, length);

	if (oldbuffermode) {
		MT32SetInstruments();
		_PlayBufDataChk();
	}
	else if (!EPThread) InitializeNotesCatcherThread();

	if (data == -1) return 0;
	return data;
}

/*
/ Something I could use in Keppy's MIDI Converter
BOOL CALLBACK MidiFilterProc(HSTREAM handle, DWORD track, BASS_MIDI_EVENT *event, BOOL seeking, void *user)
{
	if (event->event == MIDI_EVENT_NOTE) {
		int vel = HIBYTE(event->param);
		int note = LOBYTE(event->param);

		// First check
		if (ignorenotes1)
		{
			if (vel < 10 && vel>0) {
				// Ignored
				return FALSE;
			}
		}

		// Second
		if (limit88)
		{
			if (!(note >= 21 && note <= 108))
			{
				// Ignored
				return FALSE;
			}
		}

		// Third
		if (fullvelocity || pitchshift != 0x7F) {
			if (pitchshift != 0x7F)
			{
				if (pitchshiftchan[event->chan])
				{
					int newnote = note + pitchshift;
					if (newnote > 127) { newnote = 127; }
					else if (newnote < 0) { newnote = 0; }
					event->param = (event->param & 0xFF00) | newnote;
				}
			}
			if (fullvelocity && vel != 0)
				event->param = (event->param & 0x00FF) | 0x7F;
		}

	}
	return TRUE; // process the event
}
*/

void InitializeStream(INT32 mixfreq) {
	bool isdecode = FALSE;

	PrintToConsole(FOREGROUND_RED, 1, "Creating stream for external engine...");
	if (currentengine == DSOUND_ENGINE || currentengine == WASAPI_ENGINE) {
		if (AudioOutput == 0) {
			isdecode = TRUE;
		}
		else isdecode = FALSE;
	}
	else isdecode = TRUE;

	if (KSStream) BASS_StreamFree(KSStream);

	KSStream = BASS_MIDI_StreamCreate(16,
		(isdecode ? BASS_STREAM_DECODE : 0) | (sysresetignore ? BASS_MIDI_NOSYSRESET : 0) | (monorendering ? BASS_SAMPLE_MONO : 0) |
		AudioRenderingType(floatrendering) | (noteoff1 ? BASS_MIDI_NOTEOFF1 : 0) | (nofx ? BASS_MIDI_NOFX : 0) | (sinc ? BASS_MIDI_SINCINTER : 0),
		mixfreq);
	// BASS_MIDI_StreamSetFilter(KSStream, TRUE, MidiFilterProc, NULL);
	CheckUp(ERRORCODE, L"KSStreamCreate", TRUE);
	
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
	std::wstring stemp = tstring(out) + L" - Keppy's Synthesizer Output File (Restart N�" + rv.str() + L").wav";
	LPCWSTR result2 = stemp.c_str();
	HKEY hKey = 0;
	DWORD cbValueLength = sizeof(confpath);
	DWORD dwType = REG_SZ;
	RegOpenKeyEx(HKEY_CURRENT_USER, L"Software\\Keppy's Synthesizer\\Settings", 0, KEY_ALL_ACCESS, &hKey);
	if (RegQueryValueEx(hKey, L"lastexportfolder", NULL, &dwType, reinterpret_cast<LPBYTE>(&confpath), &cbValueLength) == ERROR_FILE_NOT_FOUND) {
		if (SUCCEEDED(SHGetFolderPath(NULL, CSIDL_DESKTOP, NULL, 0, encpath))) PathAppend(encpath, result2);
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

void ASIOControlPanel() {
	if ((GetAsyncKeyState(VK_MENU) & GetAsyncKeyState(0x39) & 0x8000) && currentengine == ASIO_ENGINE) BASS_ASIO_ControlPanel();	
}

ULONG ASIODevicesCount() {
	int count = 0;
	BASS_ASIO_DEVICEINFO info;
	for (count = 0; BASS_ASIO_GetDeviceInfo(count, &info); count++) { /* I'm counting */ }
	return count;
}

LONG ASIODetectID() {
	try {
		BASS_ASIO_DEVICEINFO info;
		char OutputName[MAX_PATH] = "None";

		HKEY hKey;
		LSTATUS lResultA;
		LSTATUS lResultB;
		DWORD dwType = REG_SZ;
		DWORD dwSize = sizeof(OutputName);

		lResultA = RegOpenKeyEx(HKEY_CURRENT_USER, L"Software\\Keppy's Synthesizer\\Settings", 0, KEY_READ, &hKey);
		lResultB = RegQueryValueExA(hKey, "ASIOOutput", NULL, &dwType, (LPBYTE)&OutputName, &dwSize);

		for (DWORD CurrentDevice = 0; BASS_ASIO_GetDeviceInfo(CurrentDevice, &info); CurrentDevice++)
		{
			if (strcmp(OutputName, info.name) == 0)
			{
				RegCloseKey(hKey);
				return CurrentDevice;
			}
		}

		RegCloseKey(hKey);
		return 0;
	}
	catch (...) {
		CrashMessage(L"ASIODetectID");
		ExitThread(0);
		throw;
	}
}

void InitializeBASSFinal() {
	BASS_SetConfig(BASS_CONFIG_UPDATETHREADS, 0);
	BASS_SetConfig(BASS_CONFIG_UPDATEPERIOD, 0);
	BASS_GetInfo(&info);
	BASS_SetConfig(BASS_CONFIG_BUFFER, frames);
	InitializeStream(frequency);
	if (AudioOutput != NULL)
	{
		BASS_ChannelPlay(KSStream, false);
		CheckUp(ERRORCODE, L"KSChannelPlay", TRUE);
	}
}

void InitializeWAVEnc() {
	InitializeStream(frequency);
	InitializeBASSEnc();
	CheckUp(ERRORCODE, L"KSInitEnc", TRUE);
}

void InitializeASIO() {
	if (ASIODevicesCount() < 1) {
		MessageBox(NULL, L"No ASIO devices available!\n\nPress OK to fallback to WASAPI.", L"Keppy's Synthesizer - Error", MB_ICONERROR | MB_OK | MB_SYSTEMMODAL);
		currentengine = WASAPI_ENGINE;
		BASS_Free();
		PrintToConsole(FOREGROUND_RED, 1, "ASIO devices not available, using WASAPI...");
		BASS_Init(AudioOutput, frequency, BASS_DEVICE_STEREO, 0, NULL);
		CheckUp(ERRORCODE, L"BASSInit", TRUE);
		InitializeBASSFinal();
		return;
	}

	InitializeStream(frequency);
	if (BASS_ASIO_Init(ASIODetectID(), BASS_ASIO_THREAD | BASS_ASIO_JOINORDER)) {
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
	else CheckUpASIO(ERRORCODE, L"KSInitASIO", TRUE);
}

bool InitializeBASS(BOOL restart) {
	PrintToConsole(FOREGROUND_RED, 1, "The driver is now initializing BASS. Please wait...");

	BOOL init;
	BOOL isds = (currentengine == DSOUND_ENGINE || currentengine == WASAPI_ENGINE);
	AudioOutput = defaultoutput - 1;

	PrintToConsole(FOREGROUND_RED, 1, "Settings are valid, continue...");

	if (restart == TRUE) {
		PrintToConsole(FOREGROUND_RED, 1, "The driver requested to restart the stream.");
		if (currentengine == AUDTOWAV) restartvalue++;
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
	DWORD flags = BASS_DEVICE_STEREO;
	if (currentengine == DSOUND_ENGINE) flags |= BASS_DEVICE_DSOUND;

	Retry:
	PrintToConsole(FOREGROUND_RED, 1, "Initializing BASS...");
	init = BASS_Init(isds ? AudioOutput : 0, frequency, flags, 0, NULL);
	CheckUp(ERRORCODE, L"BASSInit", TRUE);

	if (currentengine == AUDTOWAV) {
		InitializeWAVEnc();
	}
	else if (currentengine == DSOUND_ENGINE || currentengine == WASAPI_ENGINE) {
		InitializeBASSFinal();
	}
	else if (currentengine == ASIO_ENGINE) {
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

	WaitForSingleObject(ATThread, INFINITE);
	CloseHandle(ATThread);
	ATThread = NULL;

	WaitForSingleObject(RTSThread, INFINITE);
	CloseHandle(RTSThread);
	RTSThread = NULL;

	WaitForSingleObject(EPThread, INFINITE);
	CloseHandle(EPThread);
	EPThread = NULL;

	stop_thread = FALSE;
}

int CreateThreads(bool startup) {
	if (startup == TRUE) SetEvent(load_sfevent);

	PrintToConsole(FOREGROUND_RED, 1, "Creating threads...");

	reset_synth = 0;
	ATThread = CreateThread(NULL, 0, (LPTHREAD_START_ROUTINE)AudioThread, NULL, 0, (LPDWORD)ATThreadAddress);
	SetThreadPriority(ATThread, prioval[driverprio]);
	RTSThread = CreateThread(NULL, 0, (LPTHREAD_START_ROUTINE)RTSettings, NULL, 0, (LPDWORD)RTSThreadAddress);
	SetThreadPriority(RTSThread, prioval[driverprio]);
	if (!DThread)
	{
		DThread = CreateThread(NULL, 0, (LPTHREAD_START_ROUTINE)DebugThread, NULL, 0, (LPDWORD)DThreadAddress);
		SetThreadPriority(DThread, prioval[driverprio]);
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

void FreeUpStream() {
	FillContentDebug(0.0f, 0, 0, FALSE, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, FALSE);
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
		BASS_Encode_Stop(KSStream);
		CheckUp(ERRORCODE, L"KSFreeBASSenc", TRUE);
		BASS_ASIO_Free();
		CheckUp(ERRORCODE, L"KSFreeASIO", TRUE);
		BASS_Free();
		CheckUp(ERRORCODE, L"KSFreeBASS", TRUE);
	}
}