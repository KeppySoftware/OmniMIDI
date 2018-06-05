/*
OmniMIDI stream init
*/

void MT32SetInstruments() {
	if (ManagedSettings.MT32Mode) {
		BASS_MIDI_StreamEvent(OMStream, 0, MIDI_EVENT_PROGRAM, 0);
		BASS_MIDI_StreamEvent(OMStream, 1, MIDI_EVENT_PROGRAM, 36);
		BASS_MIDI_StreamEvent(OMStream, 2, MIDI_EVENT_PROGRAM, 48);
		BASS_MIDI_StreamEvent(OMStream, 3, MIDI_EVENT_PROGRAM, 61);
		BASS_MIDI_StreamEvent(OMStream, 4, MIDI_EVENT_PROGRAM, 66);
		BASS_MIDI_StreamEvent(OMStream, 5, MIDI_EVENT_PROGRAM, 96);
		BASS_MIDI_StreamEvent(OMStream, 6, MIDI_EVENT_PROGRAM, 76);
		BASS_MIDI_StreamEvent(OMStream, 7, MIDI_EVENT_PROGRAM, 76);
		BASS_MIDI_StreamEvent(OMStream, 8, MIDI_EVENT_PROGRAM, 55);
		BASS_MIDI_StreamEvent(OMStream, 9, MIDI_EVENT_PROGRAM, 35);
		BASS_MIDI_StreamEvent(OMStream, 10, MIDI_EVENT_PROGRAM, 0);
		BASS_MIDI_StreamEvent(OMStream, 11, MIDI_EVENT_PROGRAM, 0);
		BASS_MIDI_StreamEvent(OMStream, 12, MIDI_EVENT_PROGRAM, 0);
		BASS_MIDI_StreamEvent(OMStream, 13, MIDI_EVENT_PROGRAM, 0);
		BASS_MIDI_StreamEvent(OMStream, 14, MIDI_EVENT_PROGRAM, 0);
		BASS_MIDI_StreamEvent(OMStream, 15, MIDI_EVENT_PROGRAM, 0);
	}
	else {
		if (ManagedSettings.OverrideInstruments) {
			for (int i = 0; i <= 15; ++i) {
				BASS_MIDI_StreamEvent(OMStream, i, MIDI_EVENT_BANK, cbank[i]);
				BASS_MIDI_StreamEvent(OMStream, i, MIDI_EVENT_PROGRAM, cpreset[i]);
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

			if (ManagedSettings.NotesCatcherWithAudio) break;

			MT32SetInstruments();
			if (_PlayBufData()) _LWAIT;

			if (ManagedSettings.CapFramerate) _CFRWAIT;
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
			if (SettingsManagedByClient) break;

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
		SetThreadPriority(EPThread, prioval[ManagedSettings.DriverPriority]);
	}
}

DWORD WINAPI AudioThread(LPVOID lpParam) {
	PrintToConsole(FOREGROUND_RED, 1, "Initializing audio rendering thread for DS/Enc...");
	try {
		if (ManagedSettings.CurrentEngine != ASIO_ENGINE) {
			while (!stop_thread) {
				start2 = TimeNow();
				if (reset_synth) {
					reset_synth = 0;
					BASS_MIDI_StreamEvent(OMStream, 0, MIDI_EVENT_SYSTEM, MIDI_SYSTEM_DEFAULT);
				}

				if (ManagedSettings.CurrentEngine == AUDTOWAV) AudioRender();
				else BASS_ChannelUpdate(OMStream, 0);

				if (ManagedSettings.NotesCatcherWithAudio) {
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
		BASS_MIDI_StreamEvent(OMStream, 0, MIDI_EVENT_SYSTEM, MIDI_SYSTEM_DEFAULT);
	}

	DWORD data = BASS_ChannelGetData(OMStream, buffer, length);

	if (ManagedSettings.NotesCatcherWithAudio) {
		MT32SetInstruments();
		_PlayBufDataChk();
	}
	else if (!EPThread) InitializeNotesCatcherThread();

	if (data == -1) return 0;
	return data;
}

/*
DWORD CALLBACK WASAPIProc(void *buffer, DWORD length, void *user)
{
	start2 = TimeNow();
	if (reset_synth) {
		reset_synth = 0;
		BASS_MIDI_StreamEvent(OMStream, 0, MIDI_EVENT_SYSTEM, MIDI_SYSTEM_DEFAULT);
	}

	DWORD data = BASS_ChannelGetData(OMStream, buffer, length);

	if (ManagedSettings.NotesCatcherWithAudio) {
		MT32SetInstruments();
		_PlayBufDataChk();
	}
	else if (!EPThread) InitializeNotesCatcherThread();

	if (data == -1) return 0;
	return data;
}
*/

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

	PrintToConsole(FOREGROUND_RED, 1, "Creating stream...");
	if (ManagedSettings.CurrentEngine == DSOUND_ENGINE || ManagedSettings.CurrentEngine == WASAPI_ENGINE) isdecode = FALSE;
	else isdecode = TRUE;

	if (OMStream) BASS_StreamFree(OMStream);

	OMStream = BASS_MIDI_StreamCreate(16,
		(isdecode ? BASS_STREAM_DECODE : 0) | (ManagedSettings.IgnoreSysReset ? BASS_MIDI_NOSYSRESET : 0) | (ManagedSettings.MonoRendering ? BASS_SAMPLE_MONO : 0) |
		AudioRenderingType(ManagedSettings.AudioBitDepth) | (ManagedSettings.NoteOff1 ? BASS_MIDI_NOTEOFF1 : 0) | (ManagedSettings.EnableSFX ? 0 : BASS_MIDI_NOFX) | (ManagedSettings.SincInter ? BASS_MIDI_SINCINTER : 0),
		mixfreq);
	// BASS_MIDI_StreamSetFilter(OMStream, TRUE, MidiFilterProc, NULL);
	CheckUp(ERRORCODE, L"OMStreamCreate", TRUE);
	
	PrintToConsole(FOREGROUND_RED, 1, "Stream enabled.");
}

void InitializeBASSEnc() {
	PrintToConsole(FOREGROUND_RED, 1, "Opening BASSenc output...");

	// Cast restart values
	std::wostringstream rv;
	rv << RestartValue;

	typedef std::basic_string<TCHAR> tstring;
	TCHAR encpath[MAX_PATH];
	TCHAR confpath[MAX_PATH];
	TCHAR buffer[MAX_PATH] = { 0 };
	TCHAR * out;
	DWORD bufSize = sizeof(buffer) / sizeof(*buffer);
	if (GetModuleFileName(NULL, buffer, bufSize) == bufSize) {}
	out = PathFindFileName(buffer);
	std::wstring stemp = tstring(out) + L" - OmniMIDI Output File (Restart N�" + rv.str() + L").wav";
	LPCWSTR result2 = stemp.c_str();
	HKEY hKey = 0;
	DWORD cbValueLength = sizeof(confpath);
	DWORD dwType = REG_SZ;
	RegOpenKeyEx(HKEY_CURRENT_USER, L"Software\\OmniMIDI\\Configuration", 0, KEY_ALL_ACCESS, &hKey);
	if (RegQueryValueEx(hKey, L"AudToWAVFolder", NULL, &dwType, reinterpret_cast<LPBYTE>(&confpath), &cbValueLength) == ERROR_FILE_NOT_FOUND) {
		if (SUCCEEDED(SHGetFolderPath(NULL, CSIDL_DESKTOP, NULL, 0, encpath))) PathAppend(encpath, result2);
	}
	else {
		PathAppend(encpath, confpath);
		PathAppend(encpath, result2);
	}
	RegCloseKey(hKey);
	_bstr_t b(encpath);
	const char* c = b;
	const int result = MessageBox(NULL, L"You've enabled the \"Output to WAV\" mode.\n\nPress YES to confirm, or press NO to open the configurator\nand disable it.", L"OmniMIDI", MB_ICONINFORMATION | MB_YESNO | MB_SYSTEMMODAL);
	switch (result)
	{
	case IDYES:
		BASS_Encode_Start(OMStream, c, BASS_ENCODE_PCM | BASS_ENCODE_LIMIT, NULL, 0);
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

/* 
LONG WASAPIDetectID() {
	try {
		BASS_WASAPI_DEVICEINFO info;
		char OutputName[MAX_PATH] = "None";

		HKEY hKey;
		LSTATUS lResultA;
		LSTATUS lResultB;
		DWORD dwType = REG_SZ;
		DWORD dwSize = sizeof(OutputName);

		lResultA = RegOpenKeyEx(HKEY_CURRENT_USER, L"Software\\OmniMIDI\\Configuration", 0, KEY_READ, &hKey);
		lResultB = RegQueryValueExA(hKey, "WASAPIOutput", NULL, &dwType, (LPBYTE)&OutputName, &dwSize);

		for (DWORD CurrentDevice = 0; BASS_WASAPI_GetDeviceInfo(CurrentDevice, &info); CurrentDevice++)
		{
			if ((strcmp(OutputName, info.name) == 0) &&
				!(info.flags & BASS_DEVICE_LOOPBACK) &&
				!(info.flags & BASS_DEVICE_INPUT) &&
				!(info.flags & BASS_DEVICE_UNPLUGGED) &&
				!(info.flags & BASS_DEVICE_DISABLED))
			{
				RegCloseKey(hKey);
				return CurrentDevice;
			}
		}

		RegCloseKey(hKey);
		return 0;
	}
	catch (...) {
		CrashMessage(L"WASAPIDetectID");
		throw;
	}
}
*/

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

		lResultA = RegOpenKeyEx(HKEY_CURRENT_USER, L"Software\\OmniMIDI\\Configuration", 0, KEY_READ, &hKey);
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
		throw;
	}
}

void InitializeBASSFinal() {
	BASS_SetConfig(BASS_CONFIG_UPDATETHREADS, 0);
	BASS_SetConfig(BASS_CONFIG_UPDATEPERIOD, 0);
	BASS_GetInfo(&info);
	BASS_SetConfig(BASS_CONFIG_BUFFER, ManagedSettings.BufferLength);
	InitializeStream(ManagedSettings.AudioFrequency);
	if (AudioOutput != NULL)
	{
		BASS_ChannelPlay(OMStream, false);
		CheckUp(ERRORCODE, L"KSChannelPlay", TRUE);
	}
}

/*
void InitializeWASAPI() {
	ManagedSettings.CurrentEngine = WASAPI_ENGINE;

	BASS_WASAPI_INFO infoW;
	BASS_WASAPI_DEVICEINFO infoDW;
	LONG DeviceID = WASAPIDetectID();

	BASS_WASAPI_Init(DeviceID, 0, 0, BASS_WASAPI_BUFFER, 0, 0, NULL, NULL);
	CheckUp(ERRORCODE, L"KSWASAPIInitInfo");
	BASS_WASAPI_GetDeviceInfo(BASS_WASAPI_GetDevice(), &infoDW);
	CheckUp(ERRORCODE, L"KSWASAPIGetDeviceInfo");
	BASS_WASAPI_GetInfo(&infoW);
	CheckUp(ERRORCODE, L"KSWASAPIGetBufInfo");
	BASS_WASAPI_Free();
	CheckUp(ERRORCODE, L"KSWASAPIFreeInfo");

	InitializeStream(infoDW.mixfreq);

	if (BASS_WASAPI_Init(DeviceID, 0, 2,
		BASS_WASAPI_BUFFER | BASS_WASAPI_EVENT,
		(wasapiex ? ((float)frames / 1000.0f) : infoW.buflen + 5),
		0, WASAPIProc, NULL)) {
		CheckUp(ERRORCODE, L"KSInitWASAPI");
		BASS_WASAPI_Start();
		CheckUp(ERRORCODE, L"KSStartStreamWASAPI");
	}
	else {
		MessageBox(NULL, L"WASAPI is unavailable with the current device.\n\nChange the device through the configurator, then try again.\nTo change it, please open the configurator, and go to \"More settings > Advanced audio settings > Change default audio output\", then, after you're done, restart the MIDI application.\n\nFalling back to BASSEnc...\nPress OK to continue.", L"OmniMIDI - Can not open WASAPI device", MB_OK | MB_ICONERROR);
		ManagedSettings.CurrentEngine = AUDTOWAV;
		InitializeStream(ManagedSettings.AudioFrequency);
		InitializeBASSEnc();
		CheckUp(ERRORCODE, L"KSInitEnc");
	}
}
*/

void InitializeWAVEnc() {
	InitializeStream(ManagedSettings.AudioFrequency);
	InitializeBASSEnc();
	CheckUp(ERRORCODE, L"KSInitEnc", TRUE);
}

void InitializeASIO() {
	if (ASIODevicesCount() < 1) {
		MessageBox(NULL, L"No ASIO devices available!\n\nPress OK to fallback to WASAPI.", L"OmniMIDI - Error", MB_ICONERROR | MB_OK | MB_SYSTEMMODAL);
		ManagedSettings.CurrentEngine = WASAPI_ENGINE;
		BASS_Free();
		PrintToConsole(FOREGROUND_RED, 1, "ASIO devices not available, using WASAPI...");
		BASS_Init(AudioOutput, ManagedSettings.AudioFrequency, BASS_DEVICE_STEREO, 0, NULL);
		CheckUp(ERRORCODE, L"BASSInit", TRUE);
		InitializeBASSFinal();
		return;
	}

	InitializeStream(ManagedSettings.AudioFrequency);
	if (BASS_ASIO_Init(ASIODetectID(), BASS_ASIO_THREAD | BASS_ASIO_JOINORDER)) {
		BASS_ASIO_SetRate(ManagedSettings.AudioFrequency);
		CheckUpASIO(ERRORCODE, L"KSFormatASIO", FALSE);
		BASS_ASIO_ChannelSetFormat(FALSE, 0, BASS_ASIO_FORMAT_FLOAT);
		if (ManagedSettings.MonoRendering == 1) BASS_ASIO_ChannelEnableMirror(1, FALSE, 0);
		CheckUpASIO(ERRORCODE, L"KSChanSetMono", TRUE);
		BASS_ASIO_ChannelSetFormat(FALSE, 1, BASS_ASIO_FORMAT_FLOAT);
		CheckUpASIO(ERRORCODE, L"KSChanSetFormatASIO", TRUE);
		if (ManagedSettings.MonoRendering == 1) BASS_ASIO_ChannelSetRate(FALSE, 0, ManagedSettings.AudioFrequency / 2);
		else BASS_ASIO_ChannelSetRate(FALSE, 0, ManagedSettings.AudioFrequency);
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
	BOOL isds = (ManagedSettings.CurrentEngine == DSOUND_ENGINE || ManagedSettings.CurrentEngine == WASAPI_ENGINE);
	DWORD flags = BASS_DEVICE_STEREO | ((ManagedSettings.CurrentEngine == DSOUND_ENGINE) ? BASS_DEVICE_DSOUND : 0);
	AudioOutput = ManagedSettings.AudioOutputReg - 1;

	PrintToConsole(FOREGROUND_RED, 1, "Settings are valid, continue...");

	if (restart == TRUE) {
		PrintToConsole(FOREGROUND_RED, 1, "The driver requested to restart the stream.");
		if (ManagedSettings.CurrentEngine == AUDTOWAV) RestartValue++;
	}

	// Free BASS
	BASS_ASIO_Stop();
	PrintToConsole(FOREGROUND_RED, 1, "BASSASIO stopped.");

	BASS_ASIO_Free();
	PrintToConsole(FOREGROUND_RED, 1, "BASSASIO freed.");

	BASS_ChannelStop(OMStream);
	PrintToConsole(FOREGROUND_RED, 1, "BASS stream stopped.");

	BASS_StreamFree(OMStream);
	PrintToConsole(FOREGROUND_RED, 1, "BASS stream freed.");

	BASS_Stop();
	PrintToConsole(FOREGROUND_RED, 1, "BASS stopped.");

	BASS_Free();
	PrintToConsole(FOREGROUND_RED, 1, "BASS freed.");

	Retry:
	PrintToConsole(FOREGROUND_RED, 1, "Initializing BASS...");
	init = BASS_Init(isds ? AudioOutput : 0, ManagedSettings.AudioFrequency, flags, 0, NULL);
	CheckUp(ERRORCODE, L"BASSInit", TRUE);

	if (ManagedSettings.CurrentEngine == AUDTOWAV) {
		InitializeWAVEnc();
	}
	else if (ManagedSettings.CurrentEngine == DSOUND_ENGINE || ManagedSettings.CurrentEngine == WASAPI_ENGINE) {
		InitializeBASSFinal();
	}
	else if (ManagedSettings.CurrentEngine == ASIO_ENGINE) {
		InitializeASIO();
	}

	if (!OMStream) {
		// Free BASS
		BASS_ASIO_Stop();
		PrintToConsole(FOREGROUND_RED, 1, "BASSASIO stopped.");

		BASS_ASIO_Free();
		PrintToConsole(FOREGROUND_RED, 1, "BASSASIO terminated.");

		BASS_StreamFree(OMStream);
		PrintToConsole(FOREGROUND_RED, 1, "BASS stream freed.");

		BASS_Free();
		PrintToConsole(FOREGROUND_RED, 1, "BASS freed.");

		OMStream = 0;
		PrintToConsole(FOREGROUND_RED, 1, "Failed to open BASS stream.");
		return false;
	}
	else {
		BASS_ChannelSetAttribute(OMStream, BASS_ATTRIB_MIDI_VOICES, ManagedSettings.MaxVoices);
		CheckUp(ERRORCODE, L"KSAttributes1", TRUE);
		BASS_ChannelSetAttribute(OMStream, BASS_ATTRIB_MIDI_CPU, ManagedSettings.MaxRenderingTime);
		CheckUp(ERRORCODE, L"KSAttributes2", TRUE);
		BASS_ChannelSetAttribute(OMStream, BASS_ATTRIB_MIDI_KILL, ManagedSettings.DisableNotesFadeOut);
		CheckUp(ERRORCODE, L"KSAttributes3", FALSE);
	}

	ChVolume = BASS_ChannelSetFX(OMStream, BASS_FX_VOLUME, 1);
	CheckUp(ERRORCODE, L"KSVolFX", FALSE);

	return init;
}

void CloseThreads(BOOL MainClose) {
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

	if (MainClose) {
		stop_rtthread = TRUE;

		WaitForSingleObject(DThread, INFINITE);
		CloseHandle(DThread);
		DThread = NULL;

		SendDummyDataToPipe();

		WaitForSingleObject(MainThread, INFINITE);
		CloseHandle(MainThread);
		MainThread = NULL;

		stop_rtthread = FALSE;
	}

	stop_thread = FALSE;
}

int CreateThreads(bool startup) {
	if (startup == TRUE) SetEvent(load_sfevent);

	PrintToConsole(FOREGROUND_RED, 1, "Creating threads...");

	reset_synth = 0;
	ATThread = CreateThread(NULL, 0, (LPTHREAD_START_ROUTINE)AudioThread, NULL, 0, (LPDWORD)ATThreadAddress);
	SetThreadPriority(ATThread, prioval[ManagedSettings.DriverPriority]);
	RTSThread = CreateThread(NULL, 0, (LPTHREAD_START_ROUTINE)RTSettings, NULL, 0, (LPDWORD)RTSThreadAddress);
	SetThreadPriority(RTSThread, prioval[ManagedSettings.DriverPriority]);
	if (!DThread)
	{
		DThread = CreateThread(NULL, 0, (LPTHREAD_START_ROUTINE)DebugThread, NULL, 0, (LPDWORD)DThreadAddress);
		SetThreadPriority(DThread, prioval[ManagedSettings.DriverPriority]);
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
		LoadSoundfont(ManagedSettings.DefaultSFList);
		PrintToConsole(FOREGROUND_RED, 1, "Default global list loaded.");
	}
}

void SetUpStream() {
	PrintToConsole(FOREGROUND_RED, 1, "Preparing stream...");
	BASS_ChannelSetAttribute(OMStream, BASS_ATTRIB_MIDI_CHANS, 16);
	BASS_MIDI_StreamEvent(OMStream, 0, MIDI_EVENT_SYSTEM, MIDI_SYSTEM_DEFAULT);
	BASS_MIDI_StreamEvent(OMStream, 9, MIDI_EVENT_DRUMS, 1);
}

void FreeUpStream() {
	CheckVolume(TRUE);

	if (OMStream)
	{
		ResetSynth(0);
		BASS_ASIO_ChannelReset(FALSE, -1, BASS_ASIO_RESET_ENABLE | BASS_ASIO_RESET_JOIN);
		BASS_ASIO_Stop();
		BASS_StreamFree(OMStream);
		CheckUp(ERRORCODE, L"OMStreamFreeBASS", TRUE);
		BASS_Encode_Stop(OMStream);
		BASS_ASIO_Free();
		BASS_Free();
		CheckUp(ERRORCODE, L"KSFreeBASS", TRUE);
	}
}