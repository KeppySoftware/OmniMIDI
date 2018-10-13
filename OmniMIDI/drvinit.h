/*
OmniMIDI stream init
*/

void MT32SetInstruments() {
	if (ManagedSettings.MT32Mode) {
		// Send the default MT32 instruments to the channels
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
		// Send the overridden instruments to the channels
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
	while (true) {
		// Parse the debug info (Active voices, rendering time etc..)
		ParseDebugData();

		// Send the debug info to the pipes
		SendDebugDataToPipe();

		// Wait
		_DBGWAIT;
	}
	PrintToConsole(FOREGROUND_RED, 1, "Closing debug pipe thread...");
	CloseHandle(DThread.ThreadHandle);
	DThread.ThreadHandle = NULL;
	return 0;
}

DWORD WINAPI EventsProcesser(LPVOID lpV) {
	PrintToConsole(FOREGROUND_RED, 1, "Initializing notes catcher thread...");
	try {
		while (!stop_thread) {
			// Start the timer, which calculates 
			// how much time it takes to do its stuff
			start4 = TimeNow();

			// If the notes catcher thread is supposed to run together with the audio thread,
			// break from the EventProcesser's loop, and close the thread, and move the processing to AudioThread
			if (ManagedSettings.NotesCatcherWithAudio) break;

			MT32SetInstruments();

			// Parse the notes until the audio thread is done
			if (_PlayBufData() && !stop_thread) _LWAIT;
			if (ManagedSettings.CapFramerate) _CFRWAIT;
		}
	}
	catch (...) {
		CrashMessage(L"NotesCatcherThread");
	}
	PrintToConsole(FOREGROUND_RED, 1, "Closing notes catcher thread...");
	CloseHandle(EPThread.ThreadHandle);
	EPThread.ThreadHandle = NULL;
	return 0;
}

DWORD WINAPI EventsProcesserHP(LPVOID lpV) {
	PrintToConsole(FOREGROUND_RED, 1, "Initializing notes catcher thread...");
	try {
		while (!stop_thread) {
			// If the notes catcher thread is supposed to run together with the audio thread,
			// break from the EventProcesser's loop, and close the thread, and move the processing to AudioThread
			if (ManagedSettings.NotesCatcherWithAudio) break;

			// Parse the notes until the audio thread is done
			if (_PlayBufData()) NTSleep(-1000);
		}
	}
	catch (...) {
		CrashMessage(L"NotesCatcherThreadHP");
	}
	PrintToConsole(FOREGROUND_RED, 1, "Closing notes catcher thread...");
	CloseHandle(EPThread.ThreadHandle);
	EPThread.ThreadHandle = NULL;
	return 0;
}

void InitializeNotesCatcherThread() {
	// If the EventProcesser thread is not valid, then open a new one
	if (EPThread.ThreadHandle == NULL) {
		EPThread.ThreadHandle = CreateThread(NULL, 0, (LPTHREAD_START_ROUTINE)(HyperMode ? EventsProcesserHP : EventsProcesser), NULL, 0, (LPDWORD)EPThread.ThreadAddress);
		SetThreadPriority(EPThread.ThreadHandle, prioval[ManagedSettings.DriverPriority]);
	}
}

DWORD WINAPI AudioEngine(LPVOID lpParam) {
	PrintToConsole(FOREGROUND_RED, 1, "Initializing audio rendering thread for DS/Enc...");
	try {
		if (ManagedSettings.CurrentEngine != ASIO_ENGINE) {
			while (!stop_thread) {
				// Check if HyperMode has been disabled
				if (HyperMode) break;

				// Start the timer, which calculates 
				// how much time it takes to do its stuff
				start2 = TimeNow();
				
				// If the app sent a SysEx Reset message, or if the user pressed INS,
				// then reset the channels
				if (reset_synth) {
					reset_synth = 0;
					BASS_MIDI_StreamEvent(OMStream, 0, MIDI_EVENT_SYSTEM, MIDI_SYSTEM_DEFAULT);
				}

				// If the current engine is ".WAV mode", then use AudioRender()
				if (ManagedSettings.CurrentEngine == AUDTOWAV) AudioRender();
				else BASS_ChannelUpdate(OMStream, 0);

				// If the EventProcesser is disabled, then process the events from the audio thread instead
				if (ManagedSettings.NotesCatcherWithAudio) {
					MT32SetInstruments();
					_PlayBufDataChk();
				}
				// Else, open the EventProcesser thread
				else if (!EPThread.ThreadHandle) InitializeNotesCatcherThread();

				_FWAIT;
			}
		}
	}
	catch (...) {
		CrashMessage(L"AudioEngineThread");
	}

	PrintToConsole(FOREGROUND_RED, 1, "Closing audio rendering thread for DS/Enc...");
	CloseHandle(ATThread.ThreadHandle);
	ATThread.ThreadHandle = NULL;

	return 0;
}

DWORD WINAPI AudioEngineHP(LPVOID lpParam) {
	PrintToConsole(FOREGROUND_RED, 1, "Initializing audio rendering thread for DS/Enc...");
	try {
		if (ManagedSettings.CurrentEngine != ASIO_ENGINE) {
			while (!stop_thread) {
				// Check if HyperMode has been disabled
				if (!HyperMode) break;

				// If the app sent a SysEx Reset message, or if the user pressed INS,
				// then reset the channels
				if (reset_synth) {
					reset_synth = 0;
					BASS_MIDI_StreamEvent(OMStream, 0, MIDI_EVENT_SYSTEM, MIDI_SYSTEM_DEFAULT);
				}

				// If the current engine is ".WAV mode", then use AudioRender()
				if (ManagedSettings.CurrentEngine == AUDTOWAV) AudioRender();
				else BASS_ChannelUpdate(OMStream, 0);

				// If the EventProcesser is disabled, then process the events from the audio thread instead
				if (ManagedSettings.NotesCatcherWithAudio) {
					_PlayBufDataChk();
				}
				// Else, open the EventProcesser thread
				else if (!EPThread.ThreadHandle) InitializeNotesCatcherThread();

				_FWAIT;
			}
		}
	}
	catch (...) {
		CrashMessage(L"AudioEngineThreadHP");
	}

	PrintToConsole(FOREGROUND_RED, 1, "Closing audio rendering thread for DS/Enc...");
	CloseHandle(ATThread.ThreadHandle);
	ATThread.ThreadHandle = NULL;

	return 0;
}

DWORD CALLBACK ASIOProc(BOOL input, DWORD channel, void *buffer, DWORD length, void *user)
{
	// Start the timer, which calculates 
	// how much time it takes to do its stuff
	start2 = TimeNow();

	// If the app sent a SysEx Reset message, or if the user pressed INS,
	// then reset the channels
	if (reset_synth) {
		reset_synth = 0;
		BASS_MIDI_StreamEvent(OMStream, 0, MIDI_EVENT_SYSTEM, MIDI_SYSTEM_DEFAULT);
	}

	// If the EventProcesser is disabled, then process the events from the audio thread instead
	if (ManagedSettings.NotesCatcherWithAudio) {
		MT32SetInstruments();
		_PlayBufDataChk();
	}
	// Else, open the EventProcesser thread
	else if (!EPThread.ThreadHandle) InitializeNotesCatcherThread();

	// Get the processed audio data, and send it to the ASIO device
	DWORD data = BASS_ChannelGetData(OMStream, buffer, length);

	// If no data is available, then return NULL
	if (data == -1) return NULL;
	return data;
}

// Extremely useful in case the thread is still not dead
void CheckIfThreadClosed(HANDLE thread) {
	// Check if the thread is still alive
	PrintToConsole(FOREGROUND_RED, 1, "Checking if previous thread is still alive...");
	DWORD result = WaitForSingleObject(thread, 0);

	// Oh no it is!
	if (thread != WAIT_OBJECT_0) {
		// KILL IT. DO IT.
		PrintToConsole(FOREGROUND_RED, 1, "It is! Killing...");
		WaitForSingleObject(thread, INFINITE);
		CloseHandle(thread);
		thread = NULL;
		PrintToConsole(FOREGROUND_RED, 1, "Killed! Starting thread...");
		return;
	}

	PrintToConsole(FOREGROUND_RED, 1, "It's not. Starting thread...");
}

void CloseThreads(BOOL MainClose) {
	stop_thread = TRUE;

	// Wait for each thread to close, and free their handles
	PrintToConsole(FOREGROUND_RED, 1, "Closing audio thread...");
	CloseThread(ATThread.ThreadHandle);

	PrintToConsole(FOREGROUND_RED, 1, "Closing events processer thread...");
	CloseThread(EPThread.ThreadHandle);

	if (MainClose)
	{
		// Close main as well
		PrintToConsole(FOREGROUND_RED, 1, "Closing main thread...");
		CloseThread(HealthThread.ThreadHandle);
	}

	PrintToConsole(FOREGROUND_RED, 1, "Threads closed.");
	stop_thread = FALSE;
}

BOOL CreateThreads(BOOL startup) {
	// Load the SoundFont on startup
	if (startup == TRUE) SetEvent(load_sfevent);

	PrintToConsole(FOREGROUND_RED, 1, "Closing existing threads, if they're open...");
	CloseThreads(FALSE);

	PrintToConsole(FOREGROUND_RED, 1, "Creating threads...");
	reset_synth = 0;

	if (ManagedSettings.CurrentEngine != ASIO_ENGINE) {
		PrintToConsole(FOREGROUND_RED, 1, "Opening audio thread...");
		CheckIfThreadClosed(ATThread.ThreadHandle);
		ATThread.ThreadHandle = CreateThread(NULL, 0, (LPTHREAD_START_ROUTINE)(HyperMode ? AudioEngineHP : AudioEngine), NULL, 0, (LPDWORD)ATThread.ThreadAddress);
		SetThreadPriority(ATThread.ThreadHandle, prioval[ManagedSettings.DriverPriority]);
		PrintToConsole(FOREGROUND_RED, 1, "Done...");
	}

	if (!DThread.ThreadHandle)
	{
		DThread.ThreadHandle = CreateThread(NULL, 0, (LPTHREAD_START_ROUTINE)DebugThread, NULL, 0, (LPDWORD)DThread.ThreadAddress);
		SetThreadPriority(DThread.ThreadHandle, prioval[ManagedSettings.DriverPriority]);
	}

	PrintToConsole(FOREGROUND_RED, 1, "Threads are now active.");
	return TRUE;
}

void InitializeStream(INT32 mixfreq) {
	bool isdecode = FALSE;
	PrintToConsole(FOREGROUND_RED, 1, "Creating stream...");

	// If the current audio engine is DS or WASAPI, then it's not a decoding channel
	if (ManagedSettings.CurrentEngine == DSOUND_ENGINE || 
		ManagedSettings.CurrentEngine == WASAPI_ENGINE) 
	{ isdecode = FALSE; }
	// Else, it is
	else
	{ isdecode = TRUE; }
		
	// If the stream is still active, free it up again
	if (OMStream) {
		// Stop the stream and free it as well
		BASS_ChannelStop(OMStream);
		PrintToConsole(FOREGROUND_RED, 1, "BASS stream stopped.");
		BASS_StreamFree(OMStream);
		PrintToConsole(FOREGROUND_RED, 1, "BASS stream freed.");
	}

	// Create the stream with 16 MIDI channels, and the various settings
	OMStream = BASS_MIDI_StreamCreate(16,
		(isdecode ? BASS_STREAM_DECODE : 0) | (ManagedSettings.IgnoreSysReset ? BASS_MIDI_NOSYSRESET : 0) | (ManagedSettings.MonoRendering ? BASS_SAMPLE_MONO : 0) |
		AudioRenderingType(ManagedSettings.AudioBitDepth) | (ManagedSettings.NoteOff1 ? BASS_MIDI_NOTEOFF1 : 0) | (ManagedSettings.EnableSFX ? 0 : BASS_MIDI_NOFX) | (ManagedSettings.SincInter ? BASS_MIDI_SINCINTER : 0),
		mixfreq);

	CheckUp(ERRORCODE, L"MIDI Stream Initialization", TRUE);
	
	PrintToConsole(FOREGROUND_RED, 1, "Stream enabled.");
}

void InitializeBASSEnc() {
	PrintToConsole(FOREGROUND_RED, 1, "Opening BASSenc output...");

	// Cast restart values
	std::wostringstream rv;
	rv << RestartValue;

	// Initialize the values
	typedef std::basic_string<TCHAR> tstring;
	TCHAR encpath[MAX_PATH];
	TCHAR confpath[MAX_PATH];
	TCHAR buffer[MAX_PATH] = { 0 };
	TCHAR * out;
	DWORD bufSize = sizeof(buffer) / sizeof(*buffer);

	// Get name of the current app using OmniMIDI
	if (GetModuleFileName(NULL, buffer, bufSize) == bufSize) {}
	out = PathFindFileName(buffer);

	// Append it to a temporary string, along with how many times it got restarted
	// (Ex. "Dummy.exe - OmniMIDI Output File (Restart number 4).wav")
	std::wstring stemp = tstring(out) + L" - OmniMIDI Output File (Restart number" + rv.str() + L").wav";
	LPCWSTR result2 = stemp.c_str();

	// Open the registry key, and check the current output path set in the configurator
	DWORD cbValueLength = sizeof(confpath);
	DWORD dwType = REG_SZ;
	OpenRegistryKey(Configuration, L"Software\\OmniMIDI\\Configuration", TRUE);

	if (RegQueryValueEx(Configuration.Address, L"AudToWAVFolder", NULL, &dwType, reinterpret_cast<LPBYTE>(&confpath), &cbValueLength) == ERROR_FILE_NOT_FOUND) {
		// If the folder exists, then set the path to that
		if (SUCCEEDED(SHGetFolderPath(NULL, CSIDL_DESKTOP, NULL, 0, encpath))) PathAppend(encpath, result2);
	}
	else {
		// Otherwise, set the default path to the desktop
		PathAppend(encpath, confpath);
		PathAppend(encpath, result2);
	}

	// Convert some values for the following MsgBox
	_bstr_t b(encpath);
	const char* c = b;
	const int result = MessageBox(NULL, L"You've enabled the \"Output to WAV\" mode.\n\nPress YES to confirm, or press NO to open the configurator\nand disable it.", L"OmniMIDI", MB_ICONINFORMATION | MB_YESNO | MB_SYSTEMMODAL);
	switch (result)
	{
	case IDYES:
		// If the user chose to output to WAV, then continue initializing BASSEnc
		BASS_Encode_Start(OMStream, c, BASS_ENCODE_PCM | BASS_ENCODE_LIMIT, NULL, 0);
		// Error handling
		CheckUp(ERRORCODE, L"Encoder Start", TRUE);
		break;
	case IDNO:
		// Otherwise, open the configurator
		TCHAR configuratorapp[MAX_PATH];
		if (SUCCEEDED(SHGetFolderPath(NULL, CSIDL_SYSTEMX86, NULL, 0, configuratorapp)))
		{
			PathAppend(configuratorapp, _T("\\OmniMIDI\\OmniMIDIConfigurator.exe"));
			ShellExecute(NULL, L"open", configuratorapp, L"/AT", NULL, SW_SHOWNORMAL);
			delete configuratorapp;
		}
	}

	PrintToConsole(FOREGROUND_RED, 1, "BASSenc ready.");
}

void FreeUpBASS() {
	// Deinitialize the BASS stream, then the output and free the library, since we need to restart it
	BASS_StreamFree(OMStream);
	PrintToConsole(FOREGROUND_RED, 1, "BASS stream freed.");
	BASS_Stop();
	PrintToConsole(FOREGROUND_RED, 1, "BASS stopped.");
	BASS_Free();
	PrintToConsole(FOREGROUND_RED, 1, "BASS freed.");
}

void FreeUpBASSASIO() {
	// Free up ASIO before doing anything
	BASS_ASIO_Stop();
	PrintToConsole(FOREGROUND_RED, 1, "BASSASIO stopped.");
	BASS_ASIO_Free();
	PrintToConsole(FOREGROUND_RED, 1, "BASSASIO freed.");
	ASIOReady = FALSE;
}

/*

// Legacy stuff, used for BASSWASAPI

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
		// Initialize BASSASIO info
		BASS_ASIO_DEVICEINFO info;
		char OutputName[MAX_PATH] = "None";

		// Initialize registry values
		DWORD ASType = REG_SZ;
		DWORD ASSize = sizeof(OutputName);

		// Open the registry, and get the name of the selected ASIO device
		OpenRegistryKey(Configuration, L"Software\\OmniMIDI\\Configuration", TRUE);
		RegQueryValueExA(Configuration.Address, "ASIOOutput", NULL, &ASType, (LPBYTE)&OutputName, &ASSize);

		// Iterate through the available audio devices
		for (DWORD CurrentDevice = 0; BASS_ASIO_GetDeviceInfo(CurrentDevice, &info); CurrentDevice++)
		{
			// Return the correct ID when found
			if (strcmp(OutputName, info.name) == 0)
				return CurrentDevice;
		}

		// Otherwise, return the first ASIO device
		return 0;
	}
	catch (...) {
		CrashMessage(L"ASIODetectID");
	}
}

void InitializeBASSOutput() {
	// Final BASS initialization, set some settings
	BASS_SetConfig(BASS_CONFIG_UPDATETHREADS, 0);
	BASS_SetConfig(BASS_CONFIG_UPDATEPERIOD, 0);
	BASS_GetInfo(&info);
	BASS_SetConfig(BASS_CONFIG_BUFFER, ManagedSettings.BufferLength);
	InitializeStream(ManagedSettings.AudioFrequency);
	if (AudioOutput != NULL)
	{
		// And finally, open the stream
		BASS_ChannelPlay(OMStream, false);
		CheckUp(ERRORCODE, L"Channel Play", TRUE);
		
		// If using WASAPI, disable playback buffering
		if (ManagedSettings.CurrentEngine == WASAPI_ENGINE) {
			BASS_ChannelSetAttribute(OMStream, BASS_ATTRIB_NOBUFFER, 1);
			CheckUp(ERRORCODE, L"Disable Stream Buffering", TRUE);
		}
	}
}

/*

// Legacy stuff, used for BASSWASAPI

void InitializeWASAPI() {
	ManagedSettings.CurrentEngine = WASAPI_ENGINE;

	BASS_WASAPI_INFO infoW;
	BASS_WASAPI_DEVICEINFO infoDW;
	LONG DeviceID = WASAPIDetectID();

	BASS_WASAPI_Init(DeviceID, 0, 0, BASS_WASAPI_BUFFER, 0, 0, NULL, NULL);
	CheckUp(ERRORCODE, L"OMWASAPIInitInfo");
	BASS_WASAPI_GetDeviceInfo(BASS_WASAPI_GetDevice(), &infoDW);
	CheckUp(ERRORCODE, L"OMWASAPIGetDeviceInfo");
	BASS_WASAPI_GetInfo(&infoW);
	CheckUp(ERRORCODE, L"OMWASAPIGetBufInfo");
	BASS_WASAPI_Free();
	CheckUp(ERRORCODE, L"OMWASAPIFreeInfo");

	InitializeStream(infoDW.mixfreq);

	if (BASS_WASAPI_Init(DeviceID, 0, 2,
		BASS_WASAPI_BUFFER | BASS_WASAPI_EVENT,
		(wasapiex ? ((float)frames / 1000.0f) : infoW.buflen + 5),
		0, WASAPIProc, NULL)) {
		CheckUp(ERRORCODE, L"OMInitWASAPI");
		BASS_WASAPI_Start();
		CheckUp(ERRORCODE, L"OMStartStreamWASAPI");
	}
	else {
		MessageBox(NULL, L"WASAPI is unavailable with the current device.\n\nChange the device through the configurator, then try again.\nTo change it, please open the configurator, and go to \"More settings > Advanced audio settings > Change default audio output\", then, after you're done, restart the MIDI application.\n\nFalling back to BASSEnc...\nPress OK to continue.", L"OmniMIDI - Can not open WASAPI device", MB_OK | MB_ICONERROR);
		ManagedSettings.CurrentEngine = AUDTOWAV;
		InitializeStream(ManagedSettings.AudioFrequency);
		InitializeBASSEnc();
		CheckUp(ERRORCODE, L"OMInitEnc");
	}
}
*/

void InitializeWAVEnc() {
	// Initialize the ".WAV mode"
	InitializeStream(ManagedSettings.AudioFrequency);
	InitializeBASSEnc();
	CheckUp(ERRORCODE, L"Encoder Initialization", TRUE);
}

void InitializeASIO() {
	// Chec how many ASIO devices are available
	if (ASIODevicesCount() < 1) {
		// If no devices are available, return an error, and switch to WASAPI
		MessageBox(NULL, L"No ASIO devices available!\n\nPress OK to fallback to WASAPI.", L"OmniMIDI - Error", MB_ICONERROR | MB_OK | MB_SYSTEMMODAL);
		ManagedSettings.CurrentEngine = WASAPI_ENGINE;
		BASS_Free();
		PrintToConsole(FOREGROUND_RED, 1, "ASIO devices not available, using WASAPI...");
		BASS_Init(AudioOutput, ManagedSettings.AudioFrequency, BASS_DEVICE_STEREO, 0, NULL);
		CheckUp(ERRORCODE, L"BASS Lib Initialization", TRUE);
		InitializeBASSOutput();
		return;
	}

	LONG ADID = ASIODetectID();

	// Else, initialize the stream and proceed to initialize ASIO as well
	InitializeStream(ManagedSettings.AudioFrequency);

	// If ASIO is successfully initialized, go on with the initialization process
	if (BASS_ASIO_Init(ADID, BASS_ASIO_THREAD | BASS_ASIO_JOINORDER)) {
		// Set the audio frequency
		BASS_ASIO_SetRate(ManagedSettings.AudioFrequency);
		CheckUpASIO(ERRORCODE, L"ASIO Device Frequency Set", TRUE);

		// Set the bit depth for the left channel (ASIO only supports 32-bit float, on Vista+)
		BASS_ASIO_ChannelSetFormat(FALSE, 0, BASS_ASIO_FORMAT_FLOAT);

		// If mono rendering is enabled, mirror the left channel to the right one as well
		if (ManagedSettings.MonoRendering == 1) {
			BASS_ASIO_ChannelEnableMirror(1, FALSE, 0);
			CheckUpASIO(ERRORCODE, L"ASIO Device Mono Mode", TRUE);
		}

		// Set the bit depth for the right channel as well
		BASS_ASIO_ChannelSetFormat(FALSE, 1, BASS_ASIO_FORMAT_FLOAT);
		CheckUpASIO(ERRORCODE, L"ASIO Channel Bit Depth Set", TRUE);

		// If mono rendering is enabled, set the audio frequency of the channels to half the value of the frequency selected
		if (ManagedSettings.MonoRendering == 1) BASS_ASIO_ChannelSetRate(FALSE, 0, ManagedSettings.AudioFrequency / 2);
		// Else, set it to the default frequency
		else BASS_ASIO_ChannelSetRate(FALSE, 0, ManagedSettings.AudioFrequency);
		CheckUpASIO(ERRORCODE, L"ASIO Channel Frequency Set", TRUE);

		// Enable the channels
		BASS_ASIO_ChannelEnable(FALSE, 0, ASIOProc, NULL);
		CheckUpASIO(ERRORCODE, L"ASIO Channel Enable", TRUE);
		BASS_ASIO_ChannelJoin(FALSE, 1, 0);
		CheckUpASIO(ERRORCODE, L"ASIO Channel Join", TRUE);

		// And start the ASIO output
		BASS_ASIO_Start(0, 2);
		CheckUpASIO(ERRORCODE, L"ASIO Start Output", TRUE);
	}
	// Else, something is wrong
	else CheckUpASIO(ERRORCODE, L"ASIO Initialization", TRUE);

	ASIOReady = TRUE;
}

bool InitializeBASS(BOOL restart) {
	PrintToConsole(FOREGROUND_RED, 1, "The driver is now initializing BASS. Please wait...");

	// Initialize values
	BOOL init;
	// If DS or WASAPI are selected, then the final stream will not be a decoding channel
	BOOL isds = (ManagedSettings.CurrentEngine == DSOUND_ENGINE || ManagedSettings.CurrentEngine == WASAPI_ENGINE);
	// Parse the flags, and check if DS is selected
	DWORD flags = BASS_DEVICE_STEREO | ((ManagedSettings.CurrentEngine == DSOUND_ENGINE) ? BASS_DEVICE_DSOUND : 0);
	// DWORDs on the registry are unsigned, so parse the value and subtract 1 to get the selected audio device
	AudioOutput = ManagedSettings.AudioOutputReg - 1;

	PrintToConsole(FOREGROUND_RED, 1, "Settings are valid, continue...");

	// The user restarted the synth, add 1 to RestartValue, for the ".WAV mode"
	if (restart == TRUE) {
		PrintToConsole(FOREGROUND_RED, 1, "The driver requested to restart the stream.");
		if (ManagedSettings.CurrentEngine == AUDTOWAV) RestartValue++;
	}

	FreeUpBASSASIO();
	FreeUpBASS();

Retry:
	// Initialize BASS
	PrintToConsole(FOREGROUND_RED, 1, "Initializing BASS...");
	init = BASS_Init(isds ? AudioOutput : 0, ManagedSettings.AudioFrequency, flags, 0, NULL);
	CheckUp(ERRORCODE, L"BASS Lib Initialization", TRUE);

	// If ".WAV mode" is selected, initialize the decoding channel
	if (ManagedSettings.CurrentEngine == AUDTOWAV)
		InitializeWAVEnc();
	// Else, initialize the default stream
	else if (ManagedSettings.CurrentEngine == DSOUND_ENGINE || ManagedSettings.CurrentEngine == WASAPI_ENGINE)
		InitializeBASSOutput();
	// Or else, initialize ASIO
	else if (ManagedSettings.CurrentEngine == ASIO_ENGINE)
		InitializeASIO();

	if (!OMStream) {
		// Free BASS and BASSASIO
		FreeUpBASSASIO();
		FreeUpBASS();

		// The synth failed to open the output
		OMStream = 0;
		PrintToConsole(FOREGROUND_RED, 1, "Failed to open BASS stream.");
		return false;
	}
	else {
		// Load the settings to BASS
		BASS_ChannelFlags(OMStream, ManagedSettings.EnableSFX ? 0 : BASS_MIDI_NOFX, BASS_MIDI_NOFX);
		CheckUp(ERRORCODE, L"Stream Attributes 1", TRUE);
		BASS_ChannelFlags(OMStream, ManagedSettings.NoteOff1 ? BASS_MIDI_NOTEOFF1 : 0, BASS_MIDI_NOTEOFF1);
		CheckUp(ERRORCODE, L"Stream Attributes 2", TRUE);
		BASS_ChannelFlags(OMStream, ManagedSettings.IgnoreSysReset ? BASS_MIDI_NOSYSRESET : 0, BASS_MIDI_NOSYSRESET);
		CheckUp(ERRORCODE, L"Stream Attributes 3", TRUE);
		BASS_ChannelFlags(OMStream, ManagedSettings.SincInter ? BASS_MIDI_SINCINTER : 0, BASS_MIDI_SINCINTER);
		CheckUp(ERRORCODE, L"Stream Attributes 4", TRUE);
		BASS_ChannelSetAttribute(OMStream, BASS_ATTRIB_SRC, ManagedSettings.SincConv);
		CheckUp(ERRORCODE, L"Stream Attributes 5", TRUE);
		BASS_ChannelSetAttribute(OMStream, BASS_ATTRIB_MIDI_VOICES, ManagedSettings.MaxVoices);
		CheckUp(ERRORCODE, L"Stream Attributes 6", TRUE);
		BASS_ChannelSetAttribute(OMStream, BASS_ATTRIB_MIDI_CPU, ManagedSettings.MaxRenderingTime);
		CheckUp(ERRORCODE, L"Stream Attributes 7", TRUE);
		BASS_ChannelSetAttribute(OMStream, BASS_ATTRIB_MIDI_KILL, ManagedSettings.DisableNotesFadeOut);
		CheckUp(ERRORCODE, L"Stream Attributes 8", FALSE);
	}

	// Enable the volume knob in the configurator
	ChVolume = BASS_ChannelSetFX(OMStream, BASS_FX_VOLUME, 1);
	ChVolumeStruct.fCurrent = 1.0f;
	ChVolumeStruct.fTarget = sound_out_volume_float;
	ChVolumeStruct.fTime = 0.0f;
	ChVolumeStruct.lCurve = 0;
	BASS_FXSetParameters(ChVolume, &ChVolumeStruct);
	CheckUp(ERRORCODE, L"Stream Volume FX Preparation", FALSE);

	return init;
}

void LoadSoundFontsToStream() {
	PrintToConsole(FOREGROUND_RED, 1, "Loading soundfonts...");

	// If app has a personal SoundFont list, then load it
	if (LoadSoundfontStartup() == TRUE) {
		PrintToConsole(FOREGROUND_RED, 1, "Default list for app loaded.");
	}
	// Else, load list 1
	else {
		LoadSoundfont(ManagedSettings.DefaultSFList);
		PrintToConsole(FOREGROUND_RED, 1, "Default global list loaded.");
	}
}

void SetUpStream() {
	// Initialize the MIDI channels
	PrintToConsole(FOREGROUND_RED, 1, "Preparing stream...");
	BASS_ChannelSetAttribute(OMStream, BASS_ATTRIB_MIDI_CHANS, 16);
	BASS_MIDI_StreamEvent(OMStream, 0, MIDI_EVENT_SYSTEM, MIDI_SYSTEM_DEFAULT);
	BASS_MIDI_StreamEvent(OMStream, 9, MIDI_EVENT_DRUMS, 1);
}

void FreeUpStream() {
	// Send dummy values to the mixer
	CheckVolume(TRUE);

	// Reset synth
	ResetSynth(0);

	// Free BASSASIO
	BASS_ASIO_Stop();
	PrintToConsole(FOREGROUND_RED, 1, "BASSASIO stopped.");
	BASS_ASIO_Free();
	PrintToConsole(FOREGROUND_RED, 1, "BASSASIO freed.");

	// Stop the stream and free it as well
	BASS_ChannelStop(OMStream);
	PrintToConsole(FOREGROUND_RED, 1, "BASS stream stopped.");
	BASS_StreamFree(OMStream);
	PrintToConsole(FOREGROUND_RED, 1, "BASS stream freed.");

	// Deinitialize the BASS output and free it, since we need to restart it
	BASS_Stop();
	PrintToConsole(FOREGROUND_RED, 1, "BASS stopped.");
	BASS_Free();
	PrintToConsole(FOREGROUND_RED, 1, "BASS freed.");
}