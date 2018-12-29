/*
OmniMIDI stream init
*/
#pragma once

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
	PrintMessageToDebugLog("DebugThread", "Initializing debug pipe thread...");
	while (true) {
		// Parse the debug info (Active voices, rendering time etc..)
		ParseDebugData();

		// Send the debug info to the pipes
		SendDebugDataToPipe();

		// Wait
		_DBGWAIT;
	}
	PrintMessageToDebugLog("DebugThread", "Closing debug pipe thread...");
	CloseHandle(DThread.ThreadHandle);
	DThread.ThreadHandle = NULL;
	return 0;
}

DWORD WINAPI EventsProcesser(LPVOID lpV) {
	PrintMessageToDebugLog("EventsProcesser", "Initializing notes catcher thread...");
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
			if (_PlayBufData() && !stop_thread) _FWAIT;
			if (ManagedSettings.CapFramerate) _CFRWAIT;
		}
	}
	catch (...) {
		CrashMessage("NotesCatcherThread");
	}

	PrintMessageToDebugLog("EventsProcesser", "Closing notes catcher thread...");
	CloseHandle(EPThread.ThreadHandle);
	EPThread.ThreadHandle = NULL;
	return 0;
}

DWORD WINAPI EventsProcesserHP(LPVOID lpV) {
	PrintMessageToDebugLog("EventsProcesserHP", "Initializing notes catcher thread...");
	try {
		while (!stop_thread) {
			// If the notes catcher thread is supposed to run together with the audio thread,
			// break from the EventProcesser's loop, and close the thread, and move the processing to AudioThread
			if (ManagedSettings.NotesCatcherWithAudio) break;

			// Parse the notes until the audio thread is done
			if (_PlayBufData()) _FWAIT;
		}
	}
	catch (...) {
		CrashMessage("NotesCatcherThreadHP");
	}

	PrintMessageToDebugLog("EventsProcesserHP", "Closing notes catcher thread...");
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
	PrintMessageToDebugLog("AudioEngine", "Initializing audio rendering thread...");
	try {
		if (ManagedSettings.CurrentEngine != ASIO_ENGINE) {
			while (!stop_thread) {
				// Check if HyperMode has been disabled
				if (HyperMode) break;

				// Start the timer, which calculates 
				// how much time it takes to do its stuff
				start2 = TimeNow();

				// If the EventProcesser is disabled, then process the events from the audio thread instead
				if (ManagedSettings.NotesCatcherWithAudio) {
					MT32SetInstruments();
					_PlayBufDataChk();
				}
				// Else, open the EventProcesser thread
				else if (!EPThread.ThreadHandle) InitializeNotesCatcherThread();

				// If the current engine is ".WAV mode", then use AudioRender()
				if (ManagedSettings.CurrentEngine == AUDTOWAV) BASS_ChannelGetData(OMStream, sndbf, BASS_DATA_FLOAT + sndbflen * sizeof(float));
				else BASS_ChannelUpdate(OMStream, ManagedSettings.ChannelUpdateLength);

				_FWAIT;
			}
		}
	}
	catch (...) {
		CrashMessage("AudioEngineThread");
	}

	PrintMessageToDebugLog("AudioEngine", "Closing audio rendering thread...");
	CloseHandle(ATThread.ThreadHandle);
	ATThread.ThreadHandle = NULL;
	return 0;
}

DWORD WINAPI AudioEngineHP(LPVOID lpParam) {
	PrintMessageToDebugLog("AudioEngineHP", "Initializing audio rendering thread for DirectX Audio/WASAPI/.WAV mode...");
	try {
		if (ManagedSettings.CurrentEngine != ASIO_ENGINE) {
			while (!stop_thread) {
				// Check if HyperMode has been disabled
				if (!HyperMode) break;

				// If the current engine is ".WAV mode", then use AudioRender()
				if (ManagedSettings.CurrentEngine == AUDTOWAV) BASS_ChannelGetData(OMStream, sndbf, AudioRenderingType(FALSE, ManagedSettings.AudioBitDepth) + sndbflen * sizeof(float));
				else BASS_ChannelUpdate(OMStream, ManagedSettings.ChannelUpdateLength);

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
		CrashMessage("AudioEngineThreadHP");
	}

	PrintMessageToDebugLog("AudioEngineHP", "Closing audio rendering thread for DirectX Audio/WASAPI/.WAV mode...");
	CloseHandle(ATThread.ThreadHandle);
	ATThread.ThreadHandle = NULL;
	return 0;
}

DWORD CALLBACK ASIOProc(BOOL input, DWORD channel, void *buffer, DWORD length, void *user)
{
	// Start the timer, which calculates 
	// how much time it takes to do its stuff
	start2 = TimeNow();

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

// Extremely useful to check if a thread is alive
BOOL IsThisThreadActive(HANDLE thread) {
	// Check if the thread is still alive
	PrintMessageToDebugLog("IsThisThreadActiveFunc", "Checking if previous thread passed to this function is still alive...");
	DWORD result = WaitForSingleObject(thread, 0);

	// Oh no it is!
	if (result != WAIT_OBJECT_0) {
		PrintMessageToDebugLog("CheckIfThreadClosedFunc", "It is!");
		return TRUE;
	}
	else {
		PrintMessageToDebugLog("CheckIfThreadClosedFunc", "It's not.");
		return TRUE;
	}
}

void CheckIfThreadClosed(HANDLE thread) {
	// Check if the thread is still alive
	PrintMessageToDebugLog("CheckIfThreadClosedFunc", "Checking if previous thread passed to this function is still alive...");
	DWORD result = WaitForSingleObject(thread, 0);

	// Oh no it is!
	if (result != WAIT_OBJECT_0) {
		// KILL IT. DO IT.
		PrintMessageToDebugLog("CheckIfThreadClosedFunc", "It is! I'm now waiting for it to stop...");
		WaitForSingleObject(thread, INFINITE);
		CloseHandle(thread);
		thread = NULL;

		PrintMessageToDebugLog("CheckIfThreadClosedFunc", "It stopped! Starting thread again...");
		return;
	}

	PrintMessageToDebugLog("CheckIfThreadClosedFunc", "It's not! Starting thread again...");
}

void CloseThreads(BOOL MainClose) {
	stop_thread = TRUE;

	// Wait for each thread to close, and free their handles
	PrintMessageToDebugLog("CloseThreadsFunc", "Closing audio thread...");
	CloseThread(ATThread.ThreadHandle);

	PrintMessageToDebugLog("CloseThreadsFunc", "Closing events processer thread...");
	CloseThread(EPThread.ThreadHandle);

	if (MainClose)
	{
		// Close main as well
		PrintMessageToDebugLog("CloseThreadsFunc", "Closing main thread...");
		CloseThread(HealthThread.ThreadHandle);
	}

	PrintMessageToDebugLog("CloseThreadsFunc", "Threads closed.");
	stop_thread = FALSE;
}

BOOL CreateThreads(BOOL startup) {
	PrintMessageToDebugLog("CreateThreadsFunc", "Closing existing threads, if they're open...");
	CloseThreads(FALSE);

	PrintMessageToDebugLog("CreateThreadsFunc", "Creating threads...");

	if (ManagedSettings.CurrentEngine != ASIO_ENGINE) {
		PrintMessageToDebugLog("CreateThreadsFunc", "Opening audio thread...");
		CheckIfThreadClosed(ATThread.ThreadHandle);
		ATThread.ThreadHandle = CreateThread(NULL, 0, (LPTHREAD_START_ROUTINE)(HyperMode ? AudioEngineHP : AudioEngine), NULL, 0, (LPDWORD)ATThread.ThreadAddress);
		SetThreadPriority(ATThread.ThreadHandle, prioval[ManagedSettings.DriverPriority]);
		PrintMessageToDebugLog("CreateThreadsFunc", "Done!");
	}

	if (!DThread.ThreadHandle)
	{
		PrintMessageToDebugLog("CreateThreadsFunc", "Opening debug thread...");
		DThread.ThreadHandle = CreateThread(NULL, 0, (LPTHREAD_START_ROUTINE)DebugThread, NULL, 0, (LPDWORD)DThread.ThreadAddress);
		SetThreadPriority(DThread.ThreadHandle, prioval[ManagedSettings.DriverPriority]);
		PrintMessageToDebugLog("CreateThreadsFunc", "Done!");
	}

	// The threads are ready!
	if (startup == TRUE) SetEvent(OMReady);

	PrintMessageToDebugLog("CreateThreadsFunc", "Threads are now active!");
	return TRUE;
}

void InitializeBASSVST() {
#if defined(_M_ARM64)
	return;
#endif

	wchar_t InstallPath[MAX_PATH] = { 0 };
	wchar_t LoudMax[MAX_PATH] = { 0 };

	SHGetFolderPath(NULL, CSIDL_PROFILE, NULL, 0, LoudMax);
#if defined(_M_AMD64)
	PathAppend(LoudMax, _T("\\OmniMIDI\\LoudMax\\LoudMax64.dll"));
#elif defined(_M_IX86)
	PathAppend(LoudMax, _T("\\OmniMIDI\\LoudMax\\LoudMax32.dll"));
#endif

	if (PathFileExists(LoudMax)) {
		if (GetModuleFileName(hinst, InstallPath, MAX_PATH))
		{
			PathRemoveFileSpec(InstallPath);

			LoadDriverModule(&bass_vst, InstallPath, L"bass_vst.dll", FALSE);

			if (bass_vst)
			{
				LOADLIBFUNCTION(bass_vst, BASS_VST_ChannelSetDSP);
				LOADLIBFUNCTION(bass_vst, BASS_VST_ChannelFree);
				LOADLIBFUNCTION(bass_vst, BASS_VST_ChannelCreate);
				LOADLIBFUNCTION(bass_vst, BASS_VST_ProcessEvent);
				LOADLIBFUNCTION(bass_vst, BASS_VST_ProcessEventRaw);

				BASS_VST_ChannelSetDSP(OMStream, LoudMax, BASS_UNICODE, 1);
			}
		}
	}
}

void InitializeStream(INT32 mixfreq) {
	bool isdecode = FALSE;
	PrintMessageToDebugLog("InitializeStreamFunc", "Creating stream...");

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
		PrintMessageToDebugLog("InitializeStreamFunc", "Existing BASS stream stopped...");
		BASS_StreamFree(OMStream);
		PrintMessageToDebugLog("InitializeStreamFunc", "Existing BASS stream freed...");
	}

	// Create the stream with 16 MIDI channels, and the various settings
	OMStream = BASS_MIDI_StreamCreate(16,
		(isdecode ? BASS_STREAM_DECODE : 0) | (ManagedSettings.IgnoreSysReset ? BASS_MIDI_NOSYSRESET : 0) | (ManagedSettings.MonoRendering ? BASS_SAMPLE_MONO : 0) |
		AudioRenderingType(TRUE, ManagedSettings.AudioBitDepth) | (ManagedSettings.NoteOff1 ? BASS_MIDI_NOTEOFF1 : 0) | (ManagedSettings.EnableSFX ? 0 : BASS_MIDI_NOFX) | (ManagedSettings.SincInter ? BASS_MIDI_SINCINTER : 0),
		mixfreq);

	CheckUp(FALSE, ERRORCODE, L"MIDI Stream Initialization", TRUE);
	
	PrintMessageToDebugLog("InitializeStreamFunc", "Stream is now active!");
}

void InitializeBASSEnc() {
	PrintMessageToDebugLog("InitializeBASSEncFunc", "Initializing BASSenc output...");

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
		CheckUp(FALSE, ERRORCODE, L"Encoder Start", TRUE);
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

	PrintMessageToDebugLog("InitializeBASSEncFunc", "BASSenc is now ready!");
}

void FreeUpBASS() {
	// Deinitialize the BASS stream, then the output and free the library, since we need to restart it
	BASS_StreamFree(OMStream);
	PrintMessageToDebugLog("FreeUpBASSFunc", "BASS stream freed.");
	//BASS_PluginFree(0);
	//PrintMessageToDebugLog("FreeUpBASSFunc", "Plug-ins freed.");
	BASS_Stop();
	PrintMessageToDebugLog("FreeUpBASSFunc", "BASS stopped.");
	BASS_Free();
	PrintMessageToDebugLog("FreeUpBASSFunc", "BASS freed.");
}

void FreeUpBASSASIO() {
#if !defined(_M_ARM64)
	if (BASSLoadedToMemory) {
		// Free up ASIO before doing anything
		BASS_ASIO_Stop();
		PrintMessageToDebugLog("FreeUpBASSASIOFunc", "BASSASIO stopped.");
		BASS_ASIO_Free();
		PrintMessageToDebugLog("FreeUpBASSASIOFunc", "BASSASIO freed.");
		ASIOReady = FALSE;
	}
#endif
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
		CrashMessage("WASAPIDetectID");
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
		PrintMessageToDebugLog("ASIODetectIDFunc", "Importing default ASIO device from registry...");
		OpenRegistryKey(Configuration, L"Software\\OmniMIDI\\Configuration", TRUE);
		RegQueryValueExA(Configuration.Address, "ASIOOutput", NULL, &ASType, (LPBYTE)&OutputName, &ASSize);

		// Iterate through the available audio devices
		for (DWORD CurrentDevice = 0; BASS_ASIO_GetDeviceInfo(CurrentDevice, &info); CurrentDevice++)
		{
			// Return the correct ID when found
			if (strcmp(OutputName, info.name) == 0)
			{
				PrintMessageToDebugLog("ASIODetectIDFunc", "It's a match! Initializing matched ASIO device...");
				return CurrentDevice;
			}
		}

		// Otherwise, return the first ASIO device
		PrintMessageToDebugLog("ASIODetectIDFunc", "No matches found, initializing first ASIO device available...");
		return -1;
	}
	catch (...) {
		CrashMessage("ASIODetectID");
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
		CheckUp(FALSE, ERRORCODE, L"Channel Play", TRUE);
		
		// If using WASAPI, disable playback buffering
		if (ManagedSettings.CurrentEngine == WASAPI_ENGINE) {
			BASS_ChannelSetAttribute(OMStream, BASS_ATTRIB_NOBUFFER, 1);
			CheckUp(FALSE, ERRORCODE, L"Disable Stream Buffering", TRUE);
		}
	}
}

BOOL InitializeBASSLibrary() {
	// If DS or WASAPI are selected, then the final stream will not be a decoding channel
	BOOL isds = (ManagedSettings.CurrentEngine == DSOUND_ENGINE || ManagedSettings.CurrentEngine == WASAPI_ENGINE);
	// Stream flags
	BOOL flags = BASS_DEVICE_STEREO | ((ManagedSettings.CurrentEngine == DSOUND_ENGINE) ? BASS_DEVICE_DSOUND : 0);
	// DWORDs on the registry are unsigned, so parse the value and subtract 1 to get the selected audio device
	AudioOutput = ManagedSettings.AudioOutputReg - 1;

	PrintMessageToDebugLog("InitializeBASSLibraryFunc", "Initializing BASS...");
	BOOL init = BASS_Init(isds ? AudioOutput : 0, ManagedSettings.AudioFrequency, flags, 0, NULL);
	CheckUp(FALSE, ERRORCODE, L"BASS Lib Initialization", TRUE);

	//load_bassaddons();

	return init;
}

BOOL ApplyStreamSettings() {
	if (!OMStream) {
		// Free BASS and BASSASIO
		FreeUpBASSASIO();
		FreeUpBASS();

		// The synth failed to open the output
		OMStream = 0;
		PrintMessageToDebugLog("InitializeBASSFunc", "Failed to open BASS stream.");
		return FALSE;
	}
	else {
		// Load the settings to BASS
		BASS_ChannelFlags(OMStream, ManagedSettings.EnableSFX ? 0 : BASS_MIDI_NOFX, BASS_MIDI_NOFX);
		CheckUp(FALSE, ERRORCODE, L"Stream Attributes 1", TRUE);
		BASS_ChannelFlags(OMStream, ManagedSettings.NoteOff1 ? BASS_MIDI_NOTEOFF1 : 0, BASS_MIDI_NOTEOFF1);
		CheckUp(FALSE, ERRORCODE, L"Stream Attributes 2", TRUE);
		BASS_ChannelFlags(OMStream, ManagedSettings.IgnoreSysReset ? BASS_MIDI_NOSYSRESET : 0, BASS_MIDI_NOSYSRESET);
		CheckUp(FALSE, ERRORCODE, L"Stream Attributes 3", TRUE);
		BASS_ChannelFlags(OMStream, ManagedSettings.SincInter ? BASS_MIDI_SINCINTER : 0, BASS_MIDI_SINCINTER);
		CheckUp(FALSE, ERRORCODE, L"Stream Attributes 4", TRUE);
		BASS_ChannelSetAttribute(OMStream, BASS_ATTRIB_SRC, ManagedSettings.SincConv);
		CheckUp(FALSE, ERRORCODE, L"Stream Attributes 5", TRUE);
		BASS_ChannelSetAttribute(OMStream, BASS_ATTRIB_MIDI_VOICES, ManagedSettings.MaxVoices);
		CheckUp(FALSE, ERRORCODE, L"Stream Attributes 6", TRUE);
		BASS_ChannelSetAttribute(OMStream, BASS_ATTRIB_MIDI_CPU, ManagedSettings.MaxRenderingTime);
		CheckUp(FALSE, ERRORCODE, L"Stream Attributes 7", TRUE);
		BASS_ChannelSetAttribute(OMStream, BASS_ATTRIB_MIDI_KILL, ManagedSettings.DisableNotesFadeOut);
		CheckUp(FALSE, ERRORCODE, L"Stream Attributes 8", TRUE);
	}
	return TRUE;
}

void PrepareVolumeKnob() {
	// Enable the volume knob in the configurator
	ChVolume = BASS_ChannelSetFX(OMStream, BASS_FX_VOLUME, 1);
	ChVolumeStruct.fCurrent = 1.0f;
	ChVolumeStruct.fTarget = sound_out_volume_float;
	ChVolumeStruct.fTime = 0.0f;
	ChVolumeStruct.lCurve = 0;
	BASS_FXSetParameters(ChVolume, &ChVolumeStruct);
	CheckUp(FALSE, ERRORCODE, L"Stream Volume FX Preparation", FALSE);
}

void FallbackToWASAPIEngine() {
	// Something failed, fallback to WASAPI
	ManagedSettings.CurrentEngine = WASAPI_ENGINE;

	FreeUpBASSASIO();
	FreeUpBASS();

	if (InitializeBASSLibrary()) 
		InitializeBASSOutput();
	else CrashMessage("WASAPIFallback");
}

/*

// Legacy stuff, used for BASSWASAPI

void InitializeWASAPI() {
	ManagedSettings.CurrentEngine = WASAPI_ENGINE;

	BASS_WASAPI_INFO infoW;
	BASS_WASAPI_DEVICEINFO infoDW;
	LONG DeviceID = WASAPIDetectID();

	BASS_WASAPI_Init(DeviceID, 0, 0, BASS_WASAPI_BUFFER, 0, 0, NULL, NULL);
	CheckUp(FALSE, ERRORCODE, L"OMWASAPIInitInfo");
	BASS_WASAPI_GetDeviceInfo(BASS_WASAPI_GetDevice(), &infoDW);
	CheckUp(FALSE, ERRORCODE, L"OMWASAPIGetDeviceInfo");
	BASS_WASAPI_GetInfo(&infoW);
	CheckUp(FALSE, ERRORCODE, L"OMWASAPIGetBufInfo");
	BASS_WASAPI_Free();
	CheckUp(FALSE, ERRORCODE, L"OMWASAPIFreeInfo");

	InitializeStream(infoDW.mixfreq);

	if (BASS_WASAPI_Init(DeviceID, 0, 2,
		BASS_WASAPI_BUFFER | BASS_WASAPI_EVENT,
		(wasapiex ? ((float)frames / 1000.0f) : infoW.buflen + 5),
		0, WASAPIProc, NULL)) {
		CheckUp(FALSE, ERRORCODE, L"OMInitWASAPI");
		BASS_WASAPI_Start();
		CheckUp(FALSE, ERRORCODE, L"OMStartStreamWASAPI");
	}
	else {
		MessageBox(NULL, L"WASAPI is unavailable with the current device.\n\nChange the device through the configurator, then try again.\nTo change it, please open the configurator, and go to \"More settings > Advanced audio settings > Change default audio output\", then, after you're done, restart the MIDI application.\n\nFalling back to BASSEnc...\nPress OK to continue.", L"OmniMIDI - Can not open WASAPI device", MB_OK | MB_ICONERROR);
		ManagedSettings.CurrentEngine = AUDTOWAV;
		InitializeStream(ManagedSettings.AudioFrequency);
		InitializeBASSEnc();
		CheckUp(FALSE, ERRORCODE, L"OMInitEnc");
	}
}
*/

void InitializeWAVEnc() {
	// Initialize the ".WAV mode"
	InitializeStream(ManagedSettings.AudioFrequency);
	InitializeBASSEnc();
	CheckUp(FALSE, ERRORCODE, L"Encoder Initialization", TRUE);
}

void InitializeASIO() {
	// Free BASSASIO again, just to be sure
	FreeUpBASSASIO();

	// Chec how many ASIO devices are available
	if (ASIODevicesCount() < 1) {
		// If no devices are available, return an error, and switch to WASAPI
		PrintMessageToDebugLog("InitializeASIOFunc", "No ASIO devices available.");
		MessageBox(NULL, L"No ASIO devices available!\n\nPress OK to fallback to WASAPI.", L"OmniMIDI - Error", MB_ICONERROR | MB_OK | MB_SYSTEMMODAL);
		FallbackToWASAPIEngine();
		return;
	}

	// Check if driver is supposed to run in a separate thread (TURNED ON PERMANENTLY ATM)
	// BOOL ASIOSeparateThread = TRUE;
	// OpenRegistryKey(Configuration, L"Software\\OmniMIDI\\Configuration", TRUE);
	// RegQueryValueEx(Configuration.Address, L"ASIOSeparateThread", NULL, &dwType, (LPBYTE)&ASIOSeparateThread, &dwSize);

	// Get ASIO device to use
	LONG DeviceToUse = ASIODetectID();
	if (DeviceToUse == -1) {
		// If no devices are available, return an error, and switch to WASAPI
		PrintMessageToDebugLog("InitializeASIOFunc", "Failed to get a valid ASIO device.");
		MessageBox(NULL, L"Failed to get a valid ASIO device.\n\nPress OK to fallback to WASAPI.", L"OmniMIDI - Error", MB_ICONERROR | MB_OK | MB_SYSTEMMODAL);
		FallbackToWASAPIEngine();
		return;
	}

	// If ASIO is successfully initialized, go on with the initialization process
	PrintMessageToDebugLog("InitializeASIOFunc", "Initializing BASSASIO...");
	if (BASS_ASIO_Init(DeviceToUse, BASS_ASIO_THREAD | BASS_ASIO_JOINORDER)) {
		CheckUp(TRUE, ERRORCODE, L"ASIO Initialized", TRUE);

		// Set the audio frequency
		BASS_ASIO_SetRate(ManagedSettings.AudioFrequency);
		CheckUp(TRUE, ERRORCODE, L"ASIO Device Frequency Set", TRUE);

		// Set the bit depth for the left channel (ASIO only supports 32-bit float, on Vista+)
		BASS_ASIO_ChannelSetFormat(FALSE, 0, BASS_ASIO_FORMAT_FLOAT);
		CheckUp(TRUE, ERRORCODE, L"ASIO Device Format Set", TRUE);

		// If mono rendering is enabled, set the audio frequency of the channels to half the value of the frequency selected
		if (ManagedSettings.MonoRendering == 1) BASS_ASIO_ChannelSetRate(FALSE, 0, ManagedSettings.AudioFrequency / 2);
		// Else, set it to the default frequency
		else BASS_ASIO_ChannelSetRate(FALSE, 0, ManagedSettings.AudioFrequency);
		CheckUp(TRUE, ERRORCODE, L"ASIO Channel Frequency Set", TRUE);

		// Enable the channels
		BASS_ASIO_ChannelEnable(FALSE, 0, ASIOProc, NULL);
		CheckUp(TRUE, ERRORCODE, L"ASIO Channel Enable", TRUE);

		// If mono rendering is enabled, mirror the left channel to the right one as well
		if (ManagedSettings.MonoRendering) {
			BASS_ASIO_ChannelEnableMirror(1, FALSE, 0);
			CheckUp(TRUE, ERRORCODE, L"ASIO Device Mono Mode", TRUE);
		}
		// Else, go for stereo
		else {
			BASS_ASIO_ChannelJoin(FALSE, 1, 0);
			CheckUp(TRUE, ERRORCODE, L"ASIO Channel Join", TRUE);
			// Set the bit depth for the right channel as well
			BASS_ASIO_ChannelSetFormat(FALSE, 0, BASS_ASIO_FORMAT_FLOAT);
			CheckUp(TRUE, ERRORCODE, L"ASIO Channel Bit Depth Set", TRUE);
		}

		// And start the ASIO output
		BASS_ASIO_Start(0, std::thread::hardware_concurrency());
		CheckUp(TRUE, ERRORCODE, L"ASIO Start Output", TRUE);

		// Initialize the stream
		InitializeStream(ManagedSettings.AudioFrequency);

		PrintMessageToDebugLog("InitializeASIOFunc", "Done!");
	}
	// Else, something is wrong
	else CheckUp(TRUE, ERRORCODE, L"ASIO Initialization", TRUE);

	ASIOReady = TRUE;
}

bool InitializeBASS(BOOL restart) {
	PrintMessageToDebugLog("InitializeBASSFunc", "The driver is now initializing BASS. Please wait...");

	// The user restarted the synth, add 1 to RestartValue, for the ".WAV mode"
	if (restart == TRUE) {
		PrintMessageToDebugLog("InitializeBASSFunc", "The driver had to restart the stream.");
		if (ManagedSettings.CurrentEngine == AUDTOWAV) RestartValue++;

		FreeUpBASSASIO();
		FreeUpBASS();
	}

	// Initialize BASS
	BOOL init = InitializeBASSLibrary();

	if (init)
	{
		// If ".WAV mode" is selected, initialize the decoding channel
		if (ManagedSettings.CurrentEngine == AUDTOWAV)
			InitializeWAVEnc();
		// Else, initialize the default stream
		else if (ManagedSettings.CurrentEngine == DSOUND_ENGINE || ManagedSettings.CurrentEngine == WASAPI_ENGINE)
			InitializeBASSOutput();
#if !defined(_M_ARM64)
		// Or else, initialize ASIO
		else if (ManagedSettings.CurrentEngine == ASIO_ENGINE)
			InitializeASIO();
#endif
		else {
			PrintMessageToDebugLog("InitializeBASSFunc", "Unknown engine, falling back to WASAPI...");
			ManagedSettings.CurrentEngine = WASAPI_ENGINE;
			RegSetValueEx(Configuration.Address, L"CurrentEngine", NULL, dwType, (LPBYTE)&ManagedSettings.CurrentEngine, sizeof(DWORD));
			InitializeBASSOutput();
		}

		if (!ApplyStreamSettings()) return FALSE;

		// Enable the volume knob in the configurator
		PrepareVolumeKnob();
		
		// Apply LoudMax, if requested
		InitializeBASSVST();
	}

	return init;
}

void LoadSoundFontsToStream() {
	PrintMessageToDebugLog("LoadSoundFontsToStreamFunc", "Loading soundfonts...");

	// If app has a personal SoundFont list, then load it
	if (!LoadSoundfontStartup() == TRUE) {
		// Otherwise, fallback to default SoundFont
		LoadSoundfont(0);
	}
}

void SetUpStream() {
	// Initialize the MIDI channels
	PrintMessageToDebugLog("SetUpStreamFunc", "Preparing MIDI channels...");
	BASS_ChannelSetAttribute(OMStream, BASS_ATTRIB_MIDI_CHANS, 16);
	BASS_MIDI_StreamEvent(OMStream, 0, MIDI_EVENT_SYSTEM, MIDI_SYSTEM_DEFAULT);
	BASS_MIDI_StreamEvent(OMStream, 9, MIDI_EVENT_DRUMS, 1);
	PrintMessageToDebugLog("SetUpStreamFunc", "MIDI channels are now ready to receive events.");
}

void FreeUpStream() {
	if (BASSLoadedToMemory) {
		// Reset synth
		ResetSynth(0);

#if !defined(_M_ARM64)
		// Free up ASIO before doing anything
		BASS_ASIO_Stop();
		PrintMessageToDebugLog("FreeUpStreamFunc", "BASSASIO stopped.");
		BASS_ASIO_Free();
		PrintMessageToDebugLog("FreeUpStreamFunc", "BASSASIO freed.");
#endif

		// Stop the stream and free it as well
		BASS_ChannelStop(OMStream);
		PrintMessageToDebugLog("FreeUpStreamFunc", "BASS stream stopped.");
		BASS_StreamFree(OMStream);
		PrintMessageToDebugLog("FreeUpStreamFunc", "BASS stream freed.");

		// Deinitialize the BASS output and free it, since we need to restart it
		BASS_Stop();
		PrintMessageToDebugLog("FreeUpStreamFunc", "BASS stopped.");
		BASS_Free();
		PrintMessageToDebugLog("FreeUpStreamFunc", "BASS freed.");
	}

	// Send dummy values to the mixer
	CheckVolume(TRUE);
}