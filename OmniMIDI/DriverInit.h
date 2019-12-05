/*
OmniMIDI stream init
*/
#pragma once

void SetInstruments() {
	if (ManagedSettings.OverrideInstruments) {
		for (int i = 0; i <= 15; ++i) {
			BASS_MIDI_StreamEvent(OMStream, i, MIDI_EVENT_BANK, cbank[i]);
			BASS_MIDI_StreamEvent(OMStream, i, MIDI_EVENT_PROGRAM, cpreset[i]);
		}
	}
}

/*
DWORD WINAPI DebugParser(Thread* DPThread) {
	PrintMessageToDebugLog("DebugParser", "Initializing debug pipe helper thread...");
	while (DriverInitStatus) {
		// Parse debug data, that's it...
		ParseDebugData();
		_FWAIT;
	}
	PrintMessageToDebugLog("DebugParser", "Closing debug pipe helper thread...");
	CloseHandle(DPThread->ThreadHandle);
	DPThread->ThreadHandle = NULL;
	return 0;
}
*/

void TerminateThread(Thread* thread, BOOL T, DWORD excode) {
	CloseHandle(thread->ThreadHandle);
	thread->ThreadHandle = nullptr;
	thread->ThreadAddress = 0;
	if (T) _endthreadex(excode);
}

void DebugPipe(LPVOID lpV) {
	// Thread DPThread;

	PrintMessageToDebugLog("DebugPipe", "Initializing debug pipe thread...");
	while (true) {
		/*
		// Check if parser is present
		if (!DPThread.ThreadHandle)
		{
			// Not present! Start it now.
			PrintMessageToDebugLog("DebugPipe", "Opening debug helper thread...");
			DPThread.ThreadHandle = CreateThread(NULL, 0, (LPTHREAD_START_ROUTINE)DebugParser, (LPVOID*)&DPThread, 0, (LPDWORD)DPThread.ThreadAddress);
			SetThreadPriority(DPThread.ThreadHandle, THREAD_PRIORITY_NORMAL);
			PrintMessageToDebugLog("DebugPipe", "Done!");
		}
		*/

		// Wait for someone to connect to the pipe
		while (ConnectNamedPipe(hPipe, NULL) ? TRUE : (GetLastError() == ERROR_PIPE_CONNECTED) /* && DPThread.ThreadHandle */)
		{
			// Parse debug data
			ParseDebugData();

			// Send the debug info to the pipes
			SendDebugDataToPipe();

			// Wait
			_FWAIT;
		}

		// Disconnect from pipe
		DisconnectNamedPipe(hPipe);
	}
	PrintMessageToDebugLog("DebugPipe", "Closing debug pipe thread...");
	TerminateThread(&DThread, TRUE, 0);
}

void EventsProcesser(LPVOID lpV) {
	DWORD TI = (DWORD)lpV;
	
	PrintMessageToDebugLog("EventsProcesser", "Initializing notes catcher thread...");
	try {
		while (!stop_thread) {
			// If the notes catcher thread is supposed to run together with the audio thread,
			// break from the EventProcesser's loop, and close the thread, and move the processing to AudioThread
			if (ManagedSettings.NotesCatcherWithAudio) break;

			SetInstruments();

			// Parse the notes until the audio thread is done
			if (_PlayBufData() && !stop_thread) _FWAIT;
			if (ManagedSettings.CapFramerate) _CFRWAIT;
		}
	}
	catch (...) {
		CrashMessage(L"EventsProcesserThread");
	}

	PrintMessageToDebugLog("EventsProcesser", "Closing notes catcher thread...");
	TerminateThread(&EPThread, TRUE, 0);
}

void FastEventsProcesser(LPVOID lpV) {
	DWORD TI = (DWORD)lpV;

	PrintMessageToDebugLog("FastEventsProcesser", "Initializing notes catcher thread...");
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
		CrashMessage(L"FastEventsProcesserThread");
	}

	PrintMessageToDebugLog("FastEventsProcesser", "Closing notes catcher thread...");
	TerminateThread(&EPThread, TRUE, 0);
}

void InitializeEventsProcesserThreads() {
	// If the EventProcesser thread is not valid, then open a new one
	if (!EPThread.ThreadHandle) {
		EPThread.ThreadHandle = (HANDLE)_beginthreadex(NULL, 0, (_beginthreadex_proc_type)(HyperMode ? FastEventsProcesser : EventsProcesser), (void*)1, 0, &EPThread.ThreadAddress);
		SetThreadPriority(EPThread.ThreadHandle, prioval[ManagedSettings.DriverPriority]);
	}
}

void AudioEngine(LPVOID lpParam) {
	PrintMessageToDebugLog("AudioEngine", "Initializing audio rendering thread...");
	try {
		if (ManagedSettings.CurrentEngine != ASIO_ENGINE) {
			while (!stop_thread) {
				// Check if HyperMode has been disabled
				if (HyperMode) break;

				// If the EventProcesser is disabled, then process the events from the audio thread instead
				if (ManagedSettings.NotesCatcherWithAudio) {
					SetInstruments();
					_PlayBufDataChk();
				}
				// Else, open the EventProcesser thread
				else if (!EPThread.ThreadHandle) InitializeEventsProcesserThreads();

				// If the current engine is ".WAV mode", then use AudioRender()
				if (ManagedSettings.CurrentEngine == AUDTOWAV) BASS_ChannelGetData(OMStream, sndbf, AudioRenderingType(FALSE, ManagedSettings.AudioBitDepth) + sndbflen * sizeof(float));
				else BASS_ChannelUpdate(OMStream, (ManagedSettings.CurrentEngine != DXAUDIO_ENGINE) ? ManagedSettings.ChannelUpdateLength : 0);

				_WAIT;
			}
		}
	}
	catch (...) {
		CrashMessage(L"AudioEngineThread");
	}

	PrintMessageToDebugLog("AudioEngine", "Closing audio rendering thread...");
	TerminateThread(&ATThread, TRUE, 0);
}

void FastAudioEngine(LPVOID lpParam) {
	PrintMessageToDebugLog("FastAudioEngine", "Initializing audio rendering thread for DirectX Audio/WASAPI/.WAV mode...");
	try {
		if (ManagedSettings.CurrentEngine != ASIO_ENGINE) {
			while (!stop_thread) {
				// Check if HyperMode has been disabled
				if (!HyperMode) break;

				// If the current engine is ".WAV mode", then use AudioRender()
				if (ManagedSettings.CurrentEngine == AUDTOWAV) BASS_ChannelGetData(OMStream, sndbf, AudioRenderingType(FALSE, ManagedSettings.AudioBitDepth) + sndbflen * sizeof(float));
				else BASS_ChannelUpdate(OMStream, (ManagedSettings.CurrentEngine != DXAUDIO_ENGINE) ? ManagedSettings.ChannelUpdateLength : 0);

				// If the EventProcesser is disabled, then process the events from the audio thread instead
				if (ManagedSettings.NotesCatcherWithAudio) {
					_PlayBufDataChk();
				}
				// Else, open the EventProcesser thread
				else if (!EPThread.ThreadHandle) InitializeEventsProcesserThreads();

				_WAIT;
			}
		}
	}
	catch (...) {
		CrashMessage(L"FastAudioEngineThread");
	}

	PrintMessageToDebugLog("FastAudioEngine", "Closing audio rendering thread for DirectX Audio/WASAPI/.WAV mode...");
	TerminateThread(&ATThread, TRUE, 0);
}

DWORD CALLBACK ASIOProc(BOOL input, DWORD channel, void *buffer, DWORD length, void *user) {
	// If the EventProcesser is disabled, then process the events from the audio thread instead
	if (ManagedSettings.NotesCatcherWithAudio) {
		SetInstruments();
		_PlayBufDataChk();
	}
	// Else, open the EventProcesser thread
	else if (!EPThread.ThreadHandle) InitializeEventsProcesserThreads();

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
	BOOL result = (WaitForSingleObject(thread, 0) != WAIT_OBJECT_0);

	result ? PrintMessageToDebugLog("IsThisThreadActiveFunc", "It is!") : PrintMessageToDebugLog("IsThisThreadActiveFunc", "It's not.");
	return result;
}

void CheckIfThreadClosed(Thread* thread) {
	if (!thread->ThreadHandle) {
		PrintMessageToDebugLog("CheckIfThreadClosedFunc", "The address of the given thread is null, so it's not online.");
		return;
	}

	// Check if the thread is still alive
	PrintMessageToDebugLog("CheckIfThreadClosedFunc", "Checking if previous thread passed to this function is still alive...");
	BOOL result = (WaitForSingleObject(thread, 0) != WAIT_OBJECT_0);

	// Oh no it is!
	if (result) {
		// KILL IT. DO IT.
		PrintMessageToDebugLog("CheckIfThreadClosedFunc", "It is! I'm now waiting for it to stop...");

		WaitForSingleObject(thread, INFINITE);
		thread->ThreadAddress = NULL;
		thread->ThreadHandle = NULL;

		PrintMessageToDebugLog("CheckIfThreadClosedFunc", "It stopped! Starting thread again...");
		return;
	}

	PrintMessageToDebugLog("CheckIfThreadClosedFunc", "It's not! Starting thread again...");
}

void CloseThreads(BOOL MainClose) {
	stop_thread = TRUE;

	// Wait for each thread to close, and free their handles
	PrintMessageToDebugLog("CloseThreadsFunc", "Closing audio thread...");
	if (!CloseThread(&ATThread))
		PrintMessageToDebugLog("CloseThreadsFunc", "Audio thread is already closed.");

	PrintMessageToDebugLog("CloseThreadsFunc", "Closing events processer thread...");
	if (!CloseThread(&EPThread))
		PrintMessageToDebugLog("CloseThreadsFunc", "Events processer thread is already closed.");

	if (MainClose)
	{
		// Close main as well
		PrintMessageToDebugLog("CloseThreadsFunc", "Closing main thread...");
		if (!CloseThread(&HealthThread))
			PrintMessageToDebugLog("CloseThreadsFunc", "Main thread is already closed.");
	}

	PrintMessageToDebugLog("CloseThreadsFunc", "Threads closed.");
	stop_thread = FALSE;
}

BOOL CreateThreads(BOOL startup) {
	PrintMessageToDebugLog("CreateThreadsFunc", "Closing existing threads, if they're open...");
	CloseThreads(FALSE);

	PrintMessageToDebugLog("CreateThreadsFunc", "Creating threads...");

	// Check if the current engine is ASIO
	if (ManagedSettings.CurrentEngine != ASIO_ENGINE) {
		// It's not, we need a separate thread to render the audio
		PrintMessageToDebugLog("CreateThreadsFunc", "Opening audio thread...");

		// Check if the audio thread is already present, and close it
		CheckIfThreadClosed(&ATThread);

		// Recreate it from scratch, and give it a priority
		ATThread.ThreadHandle = (HANDLE)_beginthreadex(NULL, 0, (_beginthreadex_proc_type)(HyperMode ? FastAudioEngine : AudioEngine), NULL, 0, &ATThread.ThreadAddress);
		SetThreadPriority(ATThread.ThreadHandle, prioval[ManagedSettings.DriverPriority]);

		PrintMessageToDebugLog("CreateThreadsFunc", "Done!");
	}

	// Check if the debug thread is working
	if (!DThread.ThreadHandle)
	{
		PrintMessageToDebugLog("CreateThreadsFunc", "Opening debug thread...");

		// It's not, initialize it and give it a priority
		DThread.ThreadHandle = (HANDLE)_beginthreadex(NULL, 0, (_beginthreadex_proc_type)DebugPipe, NULL, 0, &DThread.ThreadAddress);
		SetThreadPriority(DThread.ThreadHandle, THREAD_PRIORITY_NORMAL);

		PrintMessageToDebugLog("CreateThreadsFunc", "Done!");
	}

	// The threads are ready!
	if (startup) SetEvent(OMReady);

	PrintMessageToDebugLog("CreateThreadsFunc", "Threads are now active!");
	return TRUE;
}

void InitializeBASSVST() {
	wchar_t InstallPath[MAX_PATH] = { 0 };
	wchar_t LoudMax[MAX_PATH] = { 0 };

	// Get the user profile path
	SHGetFolderPath(NULL, CSIDL_PROFILE, NULL, 0, LoudMax);

	// Append the right DLL name to the path
#if defined(_M_AMD64)
	PathAppend(LoudMax, _T("\\OmniMIDI\\LoudMax\\LoudMax64.dll"));
#elif defined(_M_IX86)
	PathAppend(LoudMax, _T("\\OmniMIDI\\LoudMax\\LoudMax32.dll"));
#endif

	// If the DLL exists, begin the loading process
	if (PathFileExists(LoudMax)) {
		// Initialize BASS_VST
		LoadDriverModule(&bass_vst, L"bass_vst.dll", FALSE);

		// If BASS_VST has been loaded succesfully, load the functions too
		if (bass_vst)
		{
			LOADLIBFUNCTION(bass_vst, BASS_VST_ChannelSetDSP);
			LOADLIBFUNCTION(bass_vst, BASS_VST_ChannelFree);
			LOADLIBFUNCTION(bass_vst, BASS_VST_ChannelCreate);
			LOADLIBFUNCTION(bass_vst, BASS_VST_ProcessEvent);
			LOADLIBFUNCTION(bass_vst, BASS_VST_ProcessEventRaw);

			BASS_VST_ChannelSetDSP(OMStream, LoudMax, BASS_UNICODE, 1);
		}
		else { /* Nothing, probably on ARM64 */ }
	}
}

void InitializeStream(INT32 mixfreq) {
	PrintMessageToDebugLog("InitializeStreamFunc", "Creating stream...");

	// If the current audio engine is DS or WASAPI, then it's not a decoding channel, else it is
	bool isdecode = ((ManagedSettings.CurrentEngine == DXAUDIO_ENGINE || ManagedSettings.CurrentEngine == WASAPI_ENGINE) ? FALSE : TRUE);
		
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

	CheckUp(FALSE, ERRORCODE, "MIDI Stream Initialization", TRUE);
	
	PrintMessageToDebugLog("InitializeStreamFunc", "Stream is now active!");
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
#if !defined(_M_ARM64) // Only do this if on x86 and x64

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

DWORD ASIODevicesCount() {
	// Initialize BASSASIO device info
	DWORD count = 0;
	BASS_ASIO_DEVICEINFO info;

	// Count the devices
	for (/* NULL */; BASS_ASIO_GetDeviceInfo(count, &info); count++);

	// Return the count
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
		CrashMessage(L"ASIODetectID");
	}
}

BOOL InitializeBASSLibrary() {
	// If DS or WASAPI are selected, then the final stream will not be a decoding channel
	BOOL isds = (ManagedSettings.CurrentEngine == DXAUDIO_ENGINE || ManagedSettings.CurrentEngine == WASAPI_ENGINE);
	
	// Stream flags
	BOOL flags = BASS_DEVICE_STEREO | ((ManagedSettings.CurrentEngine == DXAUDIO_ENGINE) ? BASS_DEVICE_DSOUND : 0);
	
	// DWORDs on the registry are unsigned, so parse the value and subtract 1 to get the selected audio device
	AudioOutput = ManagedSettings.AudioOutputReg - 1;

	PrintMessageToDebugLog("InitializeBASSLibraryFunc", "Initializing BASS...");

	if (isds && ManagedSettings.FollowDefaultAudioDevice) {
		// If the audio output is set to -1 (Default device),
		// force BASS to follow Windows' default device whenever it's changed
		if (AudioOutput == -1) BASS_SetConfig(BASS_CONFIG_DEV_DEFAULT, 1);
	}
	
	BOOL init = BASS_Init(
		isds ? AudioOutput : 0, 
		ManagedSettings.AudioFrequency, 
		(isds ? ((ManagedSettings.ReduceBootUpDelay ? 0 : BASS_DEVICE_LATENCY) | BASS_DEVICE_CPSPEAKERS) : NULL) | flags,
		GetActiveWindow(), 
		NULL);
	CheckUp(FALSE, ERRORCODE, "BASS Lib Initialization", TRUE);

	/*
	if (isds) {
		IDirectSound* ds = (IDirectSound*)BASS_GetDSoundObject(BASS_OBJECT_DS);
		CheckUp(FALSE, ERRORCODE, L"BASS Get DSound Object", TRUE);

		if (ds) IDirectSound_SetSpeakerConfig(ds, DSSPEAKER_STEREO);
	}
	*/

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
		CheckUp(FALSE, ERRORCODE, "Stream Attributes 1", TRUE);

		BASS_ChannelFlags(OMStream, ManagedSettings.NoteOff1 ? BASS_MIDI_NOTEOFF1 : 0, BASS_MIDI_NOTEOFF1);
		CheckUp(FALSE, ERRORCODE, "Stream Attributes 2", TRUE);

		BASS_ChannelFlags(OMStream, ManagedSettings.IgnoreSysReset ? BASS_MIDI_NOSYSRESET : 0, BASS_MIDI_NOSYSRESET);
		CheckUp(FALSE, ERRORCODE, "Stream Attributes 3", TRUE);

		BASS_ChannelFlags(OMStream, ManagedSettings.SincInter ? BASS_MIDI_SINCINTER : 0, BASS_MIDI_SINCINTER);
		CheckUp(FALSE, ERRORCODE, "Stream Attributes 4", TRUE);

		BASS_ChannelSetAttribute(OMStream, BASS_ATTRIB_SRC, ManagedSettings.SincConv);
		CheckUp(FALSE, ERRORCODE, "Stream Attributes 5", TRUE);

		BASS_ChannelSetAttribute(OMStream, BASS_ATTRIB_MIDI_VOICES, ManagedSettings.MaxVoices);
		CheckUp(FALSE, ERRORCODE, "Stream Attributes 6", TRUE);

		BASS_ChannelSetAttribute(OMStream, BASS_ATTRIB_MIDI_CPU, ManagedSettings.MaxRenderingTime);
		CheckUp(FALSE, ERRORCODE, "Stream Attributes 7", TRUE);

		BASS_ChannelSetAttribute(OMStream, BASS_ATTRIB_MIDI_KILL, ManagedSettings.DisableNotesFadeOut);
		CheckUp(FALSE, ERRORCODE, "Stream Attributes 8", TRUE);
	}
	return TRUE;
}

void PrepareVolumeKnob() {
	// Enable the volume knob in the configurator
	ChVolume = BASS_ChannelSetFX(OMStream, BASS_FX_VOLUME, 1);
	ChVolumeStruct.fCurrent = 1.0f;
	ChVolumeStruct.fTarget = SynthVolume;
	ChVolumeStruct.fTime = 0.0f;
	ChVolumeStruct.lCurve = 0;
	BASS_FXSetParameters(ChVolume, &ChVolumeStruct);
	CheckUp(FALSE, ERRORCODE, "Stream Volume FX Preparation", FALSE);
}

bool InitializeBASS(BOOL restart) {
	PrintMessageToDebugLog("InitializeBASSFunc", "The driver is now initializing BASS. Please wait...");

	// The user restarted the synth, add 1 to RestartValue, for the ".WAV mode"
	if (restart) {
		PrintMessageToDebugLog("InitializeBASSFunc", "The driver had to restart the stream.");
		if (ManagedSettings.CurrentEngine == AUDTOWAV) RestartValue++;

		FreeUpBASSASIO();
		FreeUpBASS();
	}

	// Initialize BASS
	BOOL init = InitializeBASSLibrary();

	if (init)
	{
		SWITCHCHECK:
		switch (ManagedSettings.CurrentEngine) {

		case AUDTOWAV:
		{
			InitializeStream(ManagedSettings.AudioFrequency);

			PrintMessageToDebugLog("InitializeBASSEncFunc", "Initializing BASSenc output...");

			// Cast restart values
			std::wostringstream rv;
			rv << RestartValue;

			// Initialize the values
			typedef std::basic_string<TCHAR> tstring;
			TCHAR encpath[MAX_PATH];
			TCHAR confpath[MAX_PATH];
			TCHAR buffer[MAX_PATH] = { 0 };
			TCHAR* out;
			DWORD bufSize = sizeof(buffer) / sizeof(*buffer);

			// Get name of the current app using OmniMIDI
			if (GetModuleFileName(NULL, buffer, bufSize) == bufSize);
			out = PathFindFileName(buffer);

			// Append it to a temporary string, along with how many times it got restarted
			// (Ex. "Dummy.exe - OmniMIDI Output File (Restart number 4).wav")

			std::wstring stemp = tstring(out) + L" - OmniMIDI Output File (Restart number " + rv.str() + L").wav";
			LPCWSTR result2 = stemp.c_str();

			// Open the registry key, and check the current output path set in the configurator
			DWORD cbValueLength = sizeof(confpath);
			DWORD dwType = REG_SZ;
			OpenRegistryKey(Configuration, L"Software\\OmniMIDI\\Configuration", TRUE);

			if (!RegQueryValueEx(Configuration.Address, L"AudToWAVFolder", NULL, &dwType, (LPBYTE)& confpath, &cbValueLength)) {
				// If the folder exists, then set the path to that
				PathAppend(encpath, confpath);
				PathAppend(encpath, result2);
				MessageBox(NULL, encpath, L"DEBUG", MB_OK);
			}
			// Otherwise, set the default path to the desktop
			else if (SUCCEEDED(SHGetFolderPath(NULL, CSIDL_DESKTOP, NULL, 0, encpath))) PathAppend(encpath, result2);

			// Convert some values for the following MsgBox
			const int result = MessageBox(NULL, L"You've enabled the \"Output to WAV\" mode.\n\nPress YES to confirm, or press NO to open the configurator\nand disable it.", L"OmniMIDI", MB_ICONINFORMATION | MB_YESNO | MB_SYSTEMMODAL);
			switch (result)
			{
			case IDYES:
				// If the user chose to output to WAV, then continue initializing BASSEnc
				BASS_Encode_Start(OMStream, (const char*)encpath, BASS_ENCODE_PCM | BASS_ENCODE_LIMIT | BASS_UNICODE, NULL, 0);
				// Error handling
				CheckUp(FALSE, ERRORCODE, "Encoder Start", TRUE);
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
			break;
		}

		case WASAPI_ENGINE:
		case DXAUDIO_ENGINE:
		{
			// Final BASS initialization, set some settings
			PrintMessageToDebugLog("InitializeBASSOutput", "Configuring stream...");
			BASS_SetConfig(BASS_CONFIG_UPDATETHREADS, 0);
			BASS_SetConfig(BASS_CONFIG_UPDATEPERIOD, 0);

			DWORD minbuf = 0;
			if (ManagedSettings.CurrentEngine == DXAUDIO_ENGINE) {
				if (!ManagedSettings.ReduceBootUpDelay) {
					PrintMessageToDebugLog("InitializeBASSOutput", "Getting buffer info...");
					BASS_GetInfo(&info);

					// The safest value is usually minbuf - 10
					minbuf = info.minbuf - 10;
				}
				else minbuf = 0;
			}

			// If the minimum buffer is bigger than the requested buffer length, use the minimum buffer value instead
			PrintMessageToDebugLog("InitializeBASSOutput", "Setting buffer...");
			BASS_SetConfig(BASS_CONFIG_BUFFER, ((minbuf > ManagedSettings.BufferLength) ? minbuf : ManagedSettings.BufferLength));

			// Print debug info
			PrintMemoryMessageToDebugLog("InitializeBASSOutput", "Buffer length from BASS_GetInfo (in ms)", false, minbuf);
			PrintMemoryMessageToDebugLog("InitializeBASSOutput", "Buffer length from registry (in ms)", false, ManagedSettings.BufferLength);

			PrintMessageToDebugLog("InitializeBASSOutput", "Initializing stream...");
			InitializeStream(ManagedSettings.AudioFrequency);

			if (AudioOutput != NULL)
			{
				// If using WASAPI, disable playback buffering
				if (ManagedSettings.CurrentEngine == WASAPI_ENGINE) {
					PrintMessageToDebugLog("InitializeBASSOutput", "Disabling buffering, this should only be visible when using WASAPI...");
					if (BASS_ChannelSetAttribute(OMStream, BASS_ATTRIB_BUFFER, 0))
						BASS_ChannelSetAttribute(OMStream, BASS_ATTRIB_NOBUFFER, 1);
				}

				// And finally, open the stream
				PrintMessageToDebugLog("InitializeBASSOutput", "Starting stream...");
				BASS_ChannelPlay(OMStream, FALSE);
				CheckUp(FALSE, ERRORCODE, "Channel Play", TRUE);
			}
			break;
		}

#if !defined(_M_ARM64)
		case ASIO_ENGINE:
		{
			// Free BASSASIO again, just to be sure
			FreeUpBASSASIO();

			// Chec how many ASIO devices are available
			if (ASIODevicesCount() < 1) {
				// If no devices are available, return an error, and switch to WASAPI
				ManagedSettings.CurrentEngine = WASAPI_ENGINE;
				FreeUpBASSASIO();
				FreeUpBASS();
				PrintMessageToDebugLog("InitializeASIOFunc", "No ASIO devices available.");
				MessageBox(NULL, L"No ASIO devices available!\n\nPress OK to fallback to WASAPI.", L"OmniMIDI - Error", MB_ICONERROR | MB_OK | MB_SYSTEMMODAL);
				goto SWITCHCHECK;
			}

			// Check if driver is supposed to run in a separate thread (TURNED ON PERMANENTLY ATM)
			// BOOL ASIOSeparateThread = TRUE;
			// OpenRegistryKey(Configuration, L"Software\\OmniMIDI\\Configuration", TRUE);
			// RegQueryValueEx(Configuration.Address, L"ASIOSeparateThread", NULL, &dwType, (LPBYTE)&ASIOSeparateThread, &dwSize);

			// Get ASIO device to use
			LONG DeviceToUse = ASIODetectID();
			if (DeviceToUse == -1) {
				// If no devices are available, return an error, and switch to WASAPI
				ManagedSettings.CurrentEngine = WASAPI_ENGINE;
				FreeUpBASSASIO();
				FreeUpBASS();
				PrintMessageToDebugLog("InitializeASIOFunc", "Failed to get a valid ASIO device.");
				MessageBox(NULL, L"Failed to get a valid ASIO device.\n\nPress OK to fallback to WASAPI.", L"OmniMIDI - Error", MB_ICONERROR | MB_OK | MB_SYSTEMMODAL);
				goto SWITCHCHECK;
			}

			// If ASIO is successfully initialized, go on with the initialization process
			PrintMessageToDebugLog("InitializeASIOFunc", "Initializing BASSASIO...");
			if (BASS_ASIO_Init(DeviceToUse, BASS_ASIO_THREAD | BASS_ASIO_JOINORDER)) {
				CheckUp(TRUE, ERRORCODE, "ASIO Initialized", TRUE);

				// Set the audio frequency
				BASS_ASIO_SetRate(ManagedSettings.AudioFrequency);
				CheckUp(TRUE, ERRORCODE, "ASIO Device Frequency Set", TRUE);

				// Set the bit depth for the left channel (ASIO only supports 32-bit float, on Vista+)
				BASS_ASIO_ChannelSetFormat(FALSE, 0, BASS_ASIO_FORMAT_FLOAT);
				CheckUp(TRUE, ERRORCODE, "ASIO Device Format Set", TRUE);

				// If mono rendering is enabled, set the audio frequency of the channels to half the value of the frequency selected
				if (ManagedSettings.MonoRendering == 1) BASS_ASIO_ChannelSetRate(FALSE, 0, ManagedSettings.AudioFrequency / 2);
				// Else, set it to the default frequency
				else BASS_ASIO_ChannelSetRate(FALSE, 0, ManagedSettings.AudioFrequency);
				CheckUp(TRUE, ERRORCODE, "ASIO Channel Frequency Set", TRUE);

				// Enable the channels
				BASS_ASIO_ChannelEnable(FALSE, 0, ASIOProc, NULL);
				CheckUp(TRUE, ERRORCODE, "ASIO Channel Enable", TRUE);

				// If mono rendering is enabled, mirror the left channel to the right one as well
				if (ManagedSettings.MonoRendering) {
					BASS_ASIO_ChannelEnableMirror(1, FALSE, 0);
					CheckUp(TRUE, ERRORCODE, "ASIO Device Mono Mode", TRUE);
				}
				// Else, go for stereo
				else {
					BASS_ASIO_ChannelJoin(FALSE, 1, 0);
					CheckUp(TRUE, ERRORCODE, "ASIO Channel Join", TRUE);
					// Set the bit depth for the right channel as well
					BASS_ASIO_ChannelSetFormat(FALSE, 0, BASS_ASIO_FORMAT_FLOAT);
					CheckUp(TRUE, ERRORCODE, "ASIO Channel Bit Depth Set", TRUE);
				}

				// And start the ASIO output
				BASS_ASIO_Start(0, std::thread::hardware_concurrency());
				CheckUp(TRUE, ERRORCODE, "ASIO Start Output", TRUE);

				// Initialize the stream
				InitializeStream(ManagedSettings.AudioFrequency);

				PrintMessageToDebugLog("InitializeASIOFunc", "Done!");
			}
			// Else, something is wrong
			else CheckUp(TRUE, ERRORCODE, "ASIO Initialization", TRUE);

			ASIOReady = TRUE;

			break;
		}
#endif

		default:
		{
			PrintMessageToDebugLog("InitializeBASSFunc", "Unknown engine, falling back to WASAPI...");
			ManagedSettings.CurrentEngine = WASAPI_ENGINE;
			RegSetValueEx(Configuration.Address, L"CurrentEngine", NULL, dwType, (LPBYTE)& ManagedSettings.CurrentEngine, sizeof(DWORD));
			goto SWITCHCHECK;
		}

		}

		if (!ApplyStreamSettings()) return FALSE;

		// Enable the volume knob in the configurator
		if (ManagedSettings.CurrentEngine != AUDTOWAV) PrepareVolumeKnob();
		
#if !defined(_M_ARM64)
		// Apply LoudMax, if requested
		InitializeBASSVST();
#endif
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