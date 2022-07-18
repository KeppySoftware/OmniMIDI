/*
OmniMIDI stream init
*/
#pragma once

void SetNoteValuesFromSettings() {
	if (HyperMode) return;

	if (ManagedSettings.OverrideInstruments) {
		for (int i = 0; i <= 15; ++i) {
			_BMSE(OMStream, i, MIDI_EVENT_BANK, cbank[i]);
			_BMSE(OMStream, i, MIDI_EVENT_PROGRAM, cpreset[i]);
		}
	}

	for (int i = 0; i <= 15; ++i) {
		if (pitchshiftchan[i]) {
			_BMSE(OMStream, i, MIDI_EVENT_FINETUNE, ManagedSettings.ConcertPitch);
		}
	}
}

void TightenTimings() {
	return;

	TIMECAPS TC;
	
	if (timeGetDevCaps(&TC, sizeof(TC)) != TIMERR_NOERROR)
		PrintMessageToDebugLog("TightenTimings", "timeGetDevCaps failed! Timings might not be as tight as requested.");

	TimerResolution = min(max(TC.wPeriodMin, 1), TC.wPeriodMax);

	if (timeBeginPeriod(TimerResolution) == TIMERR_NOERROR)
		PrintVarToDebugLog("TightenTimings", "timeBeginPeriod set! New resolution (ms)", &TimerResolution, PRINT_UINT32);
	else 
		PrintMessageToDebugLog("TightenTimings", "timeBeginPeriod failed! Timings might not be as tight as requested.");

	if (NT_SUCCESS(NtQueryTimerResolution(&Min, &Max, &Org))) {
		PrintMessageToDebugLog("TightenTimings", "Queried NtQueryTimerResolution.");
		if (NT_SUCCESS(NtSetTimerResolution(Max, true, &Org))) {
			PrintVarToDebugLog("TightenTimings", "Timings tightened through NtSetTimerResolution! New resolution (ms)", &Max, PRINT_UINT32);
		}
		else PrintMessageToDebugLog("TightenTimings", "NtSetTimerResolution failed! Timings might not be as tight as requested.");
	}
	else PrintMessageToDebugLog("TightenTimings", "NtQueryTimerResolution failed! Timings might not be as tight as requested.");
}

void ResetTimings() {
	return;

	if (Org) {
		if (NT_SUCCESS(NtSetTimerResolution(Org, true, &Dummy))) {
			PrintMessageToDebugLog("ResetTimings", "Timings reset to normal through NtSetTimerResolution.");
		}
		else PrintMessageToDebugLog("ResetTimings", "NtSetTimerResolution failed?!");
	}

	if (timeEndPeriod(1)) PrintMessageToDebugLog("ResetTimings", "timeEndPeriod failed?!");
	else PrintMessageToDebugLog("ResetTimings", "timeEndPeriod called with previous value sent to timeBeginPeriod.");
}

BOOL EnableMIDIFeedbackMode() {
	// If minimal playback is enabled, abort the feedback initialization process,
	// since the required functions aren't called in this mode.
	if (HyperMode)
		return FALSE;

	try {
		// Initialize feedback device info
		DWORD NumDevs = 0;
		MIDIOUTCAPSW CapsW;
		BOOL FeedbackEnabled = FALSE;
		wchar_t FeedbackDevice[32] = L"Microsoft GS Wavetable Synth\0";

		// Initialize registry values
		DWORD dwType = REG_DWORD;
		DWORD dwSize = sizeof(DWORD);
		DWORD ASType = REG_SZ;
		DWORD ASSize = sizeof(FeedbackDevice);

		// Open the registry, and get the name of the selected feedback device
		PrintMessageToDebugLog("EnableMIDIFeedbackMode", "Importing default MIDI feedback device from registry...");
		OpenRegistryKey(Configuration, L"Software\\OmniMIDI\\Configuration", TRUE);
		RegQueryValueEx(Configuration.Address, L"FeedbackEnabled", NULL, &dwType, (LPBYTE)&FeedbackEnabled, &dwSize);
		RegQueryValueEx(Configuration.Address, L"FeedbackDevice", NULL, &ASType, (LPBYTE)&FeedbackDevice, &ASSize);

		// If the process has been blacklisted through KDMAPI,
		// or the feedback mode isn't enabled, return and don't load OWINMM
		if (!FeedbackEnabled || FeedbackBlacklisted)
			return FeedbackEnabled;

		if (!MIDIFeedbackWhitelist())
		{
			PrintMessageToDebugLog("EnableMIDIFeedbackMode", "Feedback mode is enabled, but the app isn't allowed to use it.");
			return FALSE;
		}

		if (!OWINMM) {
			// First check if the user patched the app using WinMMWRP 4.5+
			PrintMessageToDebugLog("EnableMIDIFeedbackMode", "Checking if Windows Multimedia Wrapper is loaded...");
			/*
			GetOWINMM = (GO)GetProcAddress(GetModuleHandle(L"winmm"), "GetOWINMM");
			SystemGetVersion = (SGV)GetProcAddress(GetModuleHandle(L"winmm"), "mmsystemGetVersion");
			*/

			/*
			if (SystemGetVersion() == 0x0502U)
			{
				PrintMessageToDebugLog("EnableMIDIFeedbackMode", "Unsupported version of the Windows Multimedia Wrapper detected.");
				MessageBox(NULL, L"This version of the Windows Multimedia Wrapper is not supported.\nPlease upgrade it by repatching the application.\n\nThe MIDI feedback mode will not be enabled.\n\nPress OK to continue", L"OmniMIDI - ERROR", MB_OK | MB_ICONERROR | MB_SYSTEMMODAL);
				return FALSE;
			}
			*/

			// If they didn't, load WINMM from the system directory
			// (CAUTION: THIS IS NOT RECOMMENDED WHEN USING WINMMWRP 4.0 OR EARLIER!)
			/*
			if (!GetOWINMM) {
				PrintMessageToDebugLog("EnableMIDIFeedbackMode", "Loading Windows Multimedia API (OWINMM) from System32... (This is to avoid issues with WinMMWRP)");

				wchar_t SystemDirectory[MAX_PATH];
				GetSystemDirectoryW(SystemDirectory, MAX_PATH);
				wcscat(SystemDirectory, L"\\winmm.dll");

				owinmm = LoadLibraryW(SystemDirectory);
			}
			// If they did, then, use the new GetOWINMM function
			// to get the original WINMM from the wrapper itself
			else owinmm = GetOWINMM();
			*/

			// If there's no OWINMM, abort the feedback initialization process
			// and mark the feedback mode as not loaded
			/*
			if (!owinmm)
			{
				PrintMessageToDebugLog("EnableMIDIFeedbackMode", "Something went wrong while loading OWINMM! Feedback mode disabled.");
				return FALSE;
			}
			*/
			// Import the required functions from the OWINMM lib
			/*
			VMMmidiOutOpen = (MOO)GetProcAddress(owinmm, "midiOutOpen");
			VMMmidiOutClose = (MOC)GetProcAddress(owinmm, "midiOutClose");
			VMMmidiOutGetNumDevs = (MOGND)GetProcAddress(owinmm, "midiOutGetNumDevs");
			VMMmidiOutShortMsg = (MOSM)GetProcAddress(owinmm, "midiOutShortMsg");
			VMMmidiOutLongMsg = (MOLM)GetProcAddress(owinmm, "midiOutLongMsg");
			VMMmidiOutGetDevCapsW = (MOGDCW)GetProcAddress(owinmm, "midiOutGetDevCapsW");
			*/

			// If one of the functions fails to load,
			// abort the feedback initialization process and mark the feedback mode as not loaded
			if (!MMmidiOutOpen || !MMmidiOutClose || !MMmidiOutGetNumDevs || !MMmidiOutShortMsg || !MMmidiOutLongMsg || !MMmidiOutGetDevCapsW) {
				PrintMessageToDebugLog("EnableMIDIFeedbackMode", "One of the functions from OWINMM failed to be parsed! Feedback mode disabled.");
				return FALSE;
			}
		}

		PrintMessageToDebugLog("EnableMIDIFeedbackMode", "Feedback mode enabled. Searching for feedback target device...");

		// Get the total number of devices from OWINMM
		NumDevs = MMmidiOutGetNumDevs();

		// Iterate through the available devices
		for (DWORD CurrentDevice = 0; CurrentDevice < NumDevs; CurrentDevice++)
		{
			// Get device caps
			if (MMmidiOutGetDevCapsW(CurrentDevice, &CapsW, sizeof(CapsW)) == MMSYSERR_NOERROR) {
				// If the device name is equals to the registry name, and the name isn't "OmniMIDI",
				// then proceed to the initialization process
				if (wcscmp(FeedbackDevice, CapsW.szPname) == 0 && wcscmp(FeedbackDevice, L"OmniMIDI\0") != 0)
				{
					PrintMessageToDebugLog("EnableMIDIFeedbackMode", "Match found, opening device...");
					// Open the MIDI output device
					if (MMmidiOutOpen((LPHMIDIOUT)&OMFeedback, CurrentDevice, 0, 0, CALLBACK_NULL) != MMSYSERR_NOERROR) {
						// It failed, mark the feedback mode as not loaded
						PrintMessageToDebugLog("midiOutOpen", "Failed to open feedback device.");
						OMFeedback = NULL;
						return FALSE;
					}
					else {
						// Everything went hunky-dory, return TRUE
						PrintMessageToDebugLog("EnableMIDIFeedbackMode", "Device is ready to receive events.");
						return TRUE;
					}
				}
			}
		}

		// Otherwise, nothing
		PrintMessageToDebugLog("EnableMIDIFeedbackMode", "No device found, or the selected device was \"OmniMIDI\" itself, which is illegal.");
	}
	catch (...) {
		_THROWCRASH;
	}
}

BOOL DisableMIDIFeedbackMode() {
	try {
		MMmidiOutShortMsg = DummymidiOutShortMsg;
		MMmidiOutLongMsg = DummymidiOutLongMsg;

		if (OMFeedback) {
			PrintMessageToDebugLog("DisableMIDIFeedbackMode", "Disabling feedback mode...");

			switch (MMmidiOutClose((HMIDIOUT)OMFeedback)) {
			case MMSYSERR_NOMEM:
				PrintMessageToDebugLog("midiOutClose", "FATAL ERROR!");
				throw;
			case MMSYSERR_INVALHANDLE:
				PrintMessageToDebugLog("midiOutClose", "Invalid handle passed to midiOutClose, marking as closed anyway.");
				break;
			case MMSYSERR_NOERROR:
			default:						
				PrintMessageToDebugLog("midiOutClose", "Device closed.");
				break;
			}

			OMFeedback = NULL;
			PrintMessageToDebugLog("DisableMIDIFeedbackMode", "Disabled.");
		}

		/*
		if (owinmm && !GetOWINMM) {
			if (!FreeLibrary(owinmm))
				throw;
		}
		
		owinmm = NULL;
		*/

		return TRUE;
	}
	catch (...) {
		_THROWCRASH;
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
	thread->ThreadHandle = NULL;
	thread->ThreadAddress = 0;
	if (T) _endthreadex(excode);
}

void DebugPipe(LPVOID lpV) {
	// Thread DPThread;

	PrintMessageToDebugLog("DebugPipe", "Initializing debug pipe thread...");
	while (!stop_svthread) {
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
		while (ConnectNamedPipe(hPipe, NULL) ? TRUE : (GetLastError() == ERROR_PIPE_CONNECTED) /* && DPThread.ThreadHandle */) {
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
			// Parse the notes until the audio thread is done
			_PlayBufData();
		}
	}
	catch (...) {
		_THROWCRASH;
	}

	PrintMessageToDebugLog("EventsProcesser", "Closing notes catcher thread...");
	SetEvent(EPThreadDone);
	TerminateThread(&EPThread, TRUE, 0);
}

void InitializeEventsProcesserThreads() {
	if (ManagedSettings.NotesCatcherWithAudio) return;

	// If the EventProcesser thread is not valid, then open a new one
	if (!EPThread.ThreadHandle) {
		if (ManagedSettings.CurrentEngine == ASIO_ENGINE && ManagedSettings.ASIODirectFeed) return;

		EPThread.ThreadHandle = (HANDLE)_beginthreadex(NULL, 0, (_beginthreadex_proc_type)EventsProcesser, 0, 0, &EPThread.ThreadAddress);
		SetThreadPriority(EPThread.ThreadHandle, prioval[ManagedSettings.DriverPriority]);
		PrintMessageToDebugLog("InitializeEventsProcesserThreads", "Done!");
	}
}

void AudioEngine(LPVOID lpParam) {
	DWORD DataLength = 0;
	QWORD QDataLength = 0;

	PrintMessageToDebugLog("AudioEngine", "Initializing audio rendering thread...");
	// Skip if ASIO isn't using the direct feed mode
	if (ManagedSettings.CurrentEngine == WASAPI_ENGINE ||
		(ManagedSettings.CurrentEngine == ASIO_ENGINE && !ManagedSettings.ASIODirectFeed));
	else {
		try {
			while (!stop_thread) {
				// If the current engine is ".WAV mode", then use AudioRender()
				switch (ManagedSettings.CurrentEngine) {
				case XAUDIO_ENGINE:
				{
					_PlayBufDataChk();
					
					if ((DataLength = BASS_ChannelGetData(OMStream, FSndBuf, BASS_DATA_FLOAT + SamplesPerFrame * sizeof(float))) != -1)
						SndDrv->WriteFrame(FSndBuf, DataLength / sizeof(float));
					
					_FWAIT;
					continue;
				}
				case BASS_OUTPUT:
//				case DXAUDIO_ENGINE:
				{
					_PlayBufDataChk();
					BASS_ChannelUpdate(OMStream, /*(ManagedSettings.CurrentEngine != DXAUDIO_ENGINE) ?*/ ManagedSettings.ChannelUpdateLength /*: 0*/);

					_FWAIT;
					continue;
				}
				case ASIO_ENGINE:
				{
					_PlayBufData();

					_FWAIT;
					continue;
				}
				case AUDTOWAV:
				{
					// Parse some notes for the WAV mode
					_PlayBufDataChk();

					QDataLength = BASS_ChannelSeconds2Bytes(OMStream, 0.016);
					BASS_ChannelGetData(OMStream, FSndBuf, AudioRenderingType(FALSE, ManagedSettings.AudioBitDepth) | QDataLength);
					BASS_Encode_Write(OMStream, FSndBuf, QDataLength);

					continue;
				}
				default:
					break;
				}

				// If we're here, then something went wrong.
				break;
			}
		}
		catch (...) {
			_THROWCRASH;
		}
	}

	PrintMessageToDebugLog("AudioEngine", "Closing audio rendering thread...");
	SetEvent(ATThreadDone);
	TerminateThread(&ATThread, TRUE, 0);
}

DWORD CALLBACK ProcData(void* buffer, DWORD length, void* user)
{
	// Get the processed audio data, and send it to the WASAPI device
	DWORD data = BASS_ChannelGetData(OMStream, buffer, length);
	return (data == -1) ? NULL : data;
}

DWORD CALLBACK ProcDataSameThread(void* buffer, DWORD length, void* user)
{
	// Process the events from the audio thread
	_PlayBufDataChk();

	// Get the processed audio data, and send it to the engine
	return ProcData(buffer, length, user);
}

DWORD CALLBACK WASAPIProc(void* buffer, DWORD length, void* user) 
{
	// Get the processed audio data, and send it to the engine
	return _ProcData(buffer, length, user);
}

DWORD CALLBACK ASIOProc(BOOL input, DWORD channel, void* buffer, DWORD length, void* user)
{
	// Get the processed audio data, and send it to the engine
	return _ProcData(buffer, length, user);
}

// Extremely useful to check if a thread is alive
BOOL IsThisThreadActive(HANDLE thread) {
	// Check if the thread is still alive
	PrintMessageToDebugLog("IsThisThreadActiveFunc", "Checking if previous thread passed to this function is still alive...");
	BOOL result = (WaitForSingleObject(thread, 0) != WAIT_OBJECT_0);

	result ? PrintMessageToDebugLog("IsThisThreadActiveFunc", "It is!") : PrintMessageToDebugLog("IsThisThreadActiveFunc", "It's not.");
	return result;
}

BOOL CheckIfThreadClosed(Thread* thread) {
	if (!thread->ThreadHandle) {
		PrintMessageToDebugLog("CheckIfThreadClosedFunc", "The address of the given thread is null, so it's not online.");
		return FALSE;
	}

	// Check if the thread is still alive
	PrintMessageToDebugLog("CheckIfThreadClosedFunc", "Checking if thread passed to this function is still alive...");
	BOOL result = (WaitForSingleObject(thread, 0) != WAIT_OBJECT_0);

	PrintMessageToDebugLog("CheckIfThreadClosedFunc", result ? "It is! I'm now waiting for it to stop..." : "It's not! Starting thread again...");
	return result;
}

void CloseThreads() {
	stop_thread = TRUE;

	// Wait for each thread to close, and free their handles
	PrintMessageToDebugLog("CloseThreadsFunc", "Closing audio thread...");
	if (CloseThread(&ATThread))
		ResetEvent(ATThreadDone);
	else
		PrintMessageToDebugLog("CloseThreadsFunc", "Audio thread is already closed.");

	PrintMessageToDebugLog("CloseThreadsFunc", "Closing events processer thread...");
	if (CloseThread(&EPThread))
		ResetEvent(EPThreadDone);
	else
		PrintMessageToDebugLog("CloseThreadsFunc", "Events processer thread is already closed.");

	PrintMessageToDebugLog("CloseThreadsFunc", "Threads closed.");
	stop_thread = FALSE;
}

void CreateThreads() {
	PrintMessageToDebugLog("CreateThreadsFunc", "Closing existing threads, if they're open...");
	CloseThreads();

	PrintMessageToDebugLog("CreateThreadsFunc", "Creating threads...");

	// Open the audio thread
	PrintMessageToDebugLog("CreateThreadsFunc", "Opening audio thread...");

	// Check if the audio thread is already present
	if (CheckIfThreadClosed(&ATThread))
		CloseThread(&ATThread);

	// Recreate it from scratch, and give it a priority
	ATThread.ThreadHandle = (HANDLE)_beginthreadex(NULL, 0, (_beginthreadex_proc_type)AudioEngine, NULL, 0, &ATThread.ThreadAddress);
	SetThreadPriority(ATThread.ThreadHandle, prioval[ManagedSettings.DriverPriority]);
	
	// Check if the debug thread is working
	if (!DThread.ThreadHandle)
	{
		PrintMessageToDebugLog("CreateThreadsFunc", "Opening debug thread...");

		// It's not, initialize it and give it a priority
		DThread.ThreadHandle = (HANDLE)_beginthreadex(NULL, 0, (_beginthreadex_proc_type)DebugPipe, NULL, 0, &DThread.ThreadAddress);
		SetThreadPriority(DThread.ThreadHandle, THREAD_PRIORITY_NORMAL);

		PrintMessageToDebugLog("CreateThreadsFunc", "Done!");
	}

	PrintMessageToDebugLog("CreateThreadsFunc", "Threads are now active!");
}

void InitializeBASSVST() {
	wchar_t InstallPath[MAX_PATH] = { 0 };
	wchar_t LoudMax[MAX_PATH] = { 0 };

	// Get the user profile path
	GetFolderPath(FOLDERID_Profile, CSIDL_PROFILE, LoudMax, sizeof(LoudMax));

	// Append the right DLL name to the path
#if defined(_M_AMD64)
	PathAppend(LoudMax, _T("\\OmniMIDI\\LoudMax\\LoudMax64.dll"));
#elif defined(_M_IX86)
	PathAppend(LoudMax, _T("\\OmniMIDI\\LoudMax\\LoudMax32.dll"));
#endif

	// If the DLL exists, begin the loading process
	if (PathFileExists(LoudMax)) {
		// Initialize BASS_VST
		if (LoadDriverModule(&BASS_VST, L"bass_vst.dll")) {
			LOADLIBFUNCTION(BASS_VST.Lib, BASS_VST_ChannelSetDSP);
			LOADLIBFUNCTION(BASS_VST.Lib, BASS_VST_ChannelFree);
			LOADLIBFUNCTION(BASS_VST.Lib, BASS_VST_ChannelCreate);
			LOADLIBFUNCTION(BASS_VST.Lib, BASS_VST_ProcessEvent);
			LOADLIBFUNCTION(BASS_VST.Lib, BASS_VST_ProcessEventRaw);

			BASS_VST_ChannelSetDSP(OMStream, LoudMax, BASS_UNICODE, 1);
		}
		else { /* Nothing, probably on ARM64 */ }
	}
}

BOOL InitializeStream(INT32 mixfreq) {
	PrintMessageToDebugLog("InitializeStreamFunc", "Creating stream...");

	// If the current audio engine is DS or WASAPI, then it's not a decoding channel, else it is
	// bool isdecode = !(ManagedSettings.CurrentEngine == DXAUDIO_ENGINE || ManagedSettings.CurrentEngine == OLD_WASAPI);
		
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
		(ManagedSettings.BASSDSMode ? BASS_MIDI_ASYNC : 0) |
		(ManagedSettings.CurrentEngine != BASS_OUTPUT ? BASS_STREAM_DECODE : 0) |
		(ManagedSettings.IgnoreSysReset ? BASS_MIDI_NOSYSRESET : 0) |
		(ManagedSettings.MonoRendering ? BASS_SAMPLE_MONO : 0) |
		AudioRenderingType(TRUE, ManagedSettings.AudioBitDepth) | 
		(ManagedSettings.NoteOff1 ? BASS_MIDI_NOTEOFF1 : 0) | 
		(ManagedSettings.EnableSFX ? 0 : BASS_MIDI_NOFX) | 
		(ManagedSettings.SincInter ? BASS_MIDI_SINCINTER : 0),
		mixfreq);

	if (!OMStream)
	{
		if (CheckUp(FALSE, ERRORCODE, "MIDI Stream Initialization", TRUE)) {
			MessageBox(NULL, L"BASS reported no error during the initialization, but the stream handle is NULL.\n\nThis is a clear sign of DLL hell.\nPlease report the issue to the app developer, or check if there are any BASS libraries inside the app's folder.\n\nCan not continue, press OK to quit.", L"OmniMIDI - FATAL ERROR", MB_ICONERROR | MB_OK | MB_SYSTEMMODAL);
			exit(ERROR_INVALID_HANDLE);
		}
	}

	BMSEsFlags = (ManagedSettings.BASSDSMode ? BASS_MIDI_EVENTS_ASYNC : 0) | BASS_MIDI_EVENTS_STRUCT | BASS_MIDI_EVENTS_TIME | BASS_MIDI_EVENTS_CANCEL;
	BMSEsRAWFlags = (ManagedSettings.BASSDSMode ? BASS_MIDI_EVENTS_ASYNC : 0) | BASS_MIDI_EVENTS_RAW;
	
	PrintMessageToDebugLog("InitializeStreamFunc", "Stream is now active!");
	return TRUE;
}

void FreeUpBASS() {
	// Remove effects
	if (ChVolume)
	{
		BASS_ChannelRemoveFX(OMStream, ChVolume);
		ChVolume = NULL;
		PrintMessageToDebugLog("FreeUpBASSFunc", "Removed ChVolume from OMStream.");
	}

	if (ChReverb)
	{
		BASS_ChannelRemoveFX(OMStream, ChReverb);
		ChReverb = NULL;
		PrintMessageToDebugLog("FreeUpBASSFunc", "Removed ChReverb from OMStream.");
	}

	if (ChChorus)
	{
		BASS_ChannelRemoveFX(OMStream, ChChorus);
		ChChorus = NULL;
		PrintMessageToDebugLog("FreeUpBASSFunc", "Removed ChChorus from OMStream.");
	}

	if (ChEcho)
	{
		BASS_ChannelRemoveFX(OMStream, ChEcho);
		ChEcho = NULL;
		PrintMessageToDebugLog("FreeUpBASSFunc", "Removed ChEcho from OMStream.");
	}

	// Deinitialize the BASS stream, then the output and free the library, since we need to restart it
	BASS_StreamFree(OMStream);
	PrintMessageToDebugLog("FreeUpBASSFunc", "BASS stream freed.");

	//BASS_PluginFree(0);
	//PrintMessageToDebugLog("FreeUpBASSFunc", "Plug-ins freed.");
	if (!HostSessionMode) {
		BASS_Stop();
		PrintMessageToDebugLog("FreeUpBASSFunc", "BASS stopped.");
		BASS_Free();
		PrintMessageToDebugLog("FreeUpBASSFunc", "BASS freed.");
	}
}

void FreeUpBASSWASAPI() {
	if (BASSLoadedToMemory) {
		// If it isn't started, then go away
		if (!BASS_WASAPI_IsStarted()) return;

		// Free up WASAPI before doing anything
		BASS_WASAPI_Stop(TRUE);
		PrintMessageToDebugLog("FreeUpBASSWASAPIFunc", "BASSWASAPI stopped.");
		BASS_WASAPI_Free();
		PrintMessageToDebugLog("FreeUpBASSWASAPIFunc", "BASSWASAPI freed.");
	}
}

void FreeUpBASSASIO() {
#if !defined(_M_ARM64) // Only do this if on x86 and x64

	if (BASSLoadedToMemory) {
		// If it isn't started, then go away
		if (!BASS_ASIO_IsStarted()) return;

		// Free up ASIO before doing anything
		BASS_ASIO_Stop();
		PrintMessageToDebugLog("FreeUpBASSASIOFunc", "BASSASIO stopped.");
		BASS_ASIO_Free();
		PrintMessageToDebugLog("FreeUpBASSASIOFunc", "BASSASIO freed.");

		// Wait for the ASIO thread to exit, to avoid issues with BASSASIO not freeing itself later
		// <https://www.un4seen.com/forum/?topic=19461.msg136033#msg136033>
		Sleep(200);
	}

#endif
}

void FreeUpXA() {
	// Free up XA before doing anything
	if (SndDrv) {
		delete SndDrv;
		SndDrv = 0;
	}
}

void SetUpStream() {
	// Initialize the MIDI channels
	PrintMessageToDebugLog("SetUpStreamFunc", "Preparing MIDI channels...");
	BASS_ChannelSetAttribute(OMStream, BASS_ATTRIB_MIDI_CHANS, 16);
	_BMSE(OMStream, 0, MIDI_EVENT_SYSTEM, MIDI_SYSTEM_DEFAULT);
	_BMSE(OMStream, 9, MIDI_EVENT_DRUMS, 1);
	PrintMessageToDebugLog("SetUpStreamFunc", "MIDI channels are now ready to receive events.");
}

void FreeUpStream() {
	if (BASSLoadedToMemory) {
		// Reset synth
		ResetSynth(FALSE, TRUE);

		// Stop the stream and free it as well
		FreeUpBASSWASAPI();
		FreeUpBASSASIO();
		FreeUpBASS();
		FreeUpXA();

		if (SndDrv) {
			delete SndDrv;
			SndDrv = 0;
		}

		if (ISndBuf) {
			delete[] ISndBuf;
			ISndBuf = 0;
		}

		if (FSndBuf) {
			delete[] FSndBuf;
			FSndBuf = 0;
		}
	}

	// Send dummy values to the mixer
	CheckVolume(TRUE);
}

DWORD BASSDevicesCount() {
	// Initialize BASS device info
	DWORD count = 0;
	BASS_DEVICEINFO info;

	// Count the devices
	for (/* NULL */; BASS_GetDeviceInfo(count, &info); count++);

	// Return the count
	return count;
}

DWORD WASAPIDevicesCount() {
	// Initialize BASSWASAPI device info
	DWORD count = 0;
	BASS_WASAPI_DEVICEINFO info;

	// Count the devices
	for (/* NULL */; BASS_WASAPI_GetDeviceInfo(count, &info); count++);

	// Return the count
	return count;
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

LONG BASSDetectID() {
	try {
		// Initialize BASS info
		BASS_DEVICEINFO info;
		char OutputName[MAX_PATH] = "default";

		// Initialize registry values
		DWORD ASType = REG_SZ;
		DWORD ASSize = sizeof(OutputName);

		// Open the registry, and get the name of the selected ASIO device
		PrintMessageToDebugLog("BASSDetectIDFunc", "Importing default BASS device from registry...");
		OpenRegistryKey(Configuration, L"Software\\OmniMIDI\\Configuration", TRUE);
		RegQueryValueExA(Configuration.Address, "AudioOutput", NULL, &ASType, (LPBYTE)&OutputName, &ASSize);

		PrintMessageToDebugLog("BASSDetectIDFunc", "Searching for output device...");

		// Iterate through the available audio devices
		if (strcmp(OutputName, "default") == 0)
		{
			PrintMessageToDebugLog("BASSDetectIDFunc", "The user wants OmniMIDI to follow the default audio device.");
			return -1;
		}
		else if (strcmp(OutputName, "nosound") == 0)
		{
			PrintMessageToDebugLog("BASSDetectIDFunc", "The user wants OmniMIDI to make no sound.");
			return 0;
		}
		else 
		{
			for (int CurrentDevice = 1; BASS_GetDeviceInfo(CurrentDevice, &info); CurrentDevice++)
			{
				// Return the correct ID when found
				if (strcmp(OutputName, info.driver) == 0)
				{
					PrintMessageToDebugLog("BASSDetectIDFunc", "It's a match! Initializing matched BASS device...");
					return CurrentDevice;
				}
			}
		}

		// Otherwise, return the default BASS device
		PrintMessageToDebugLog("BASSDetectIDFunc", "No matches found, initializing first BASS device available...");
		return -1;
	}
	catch (...) {
		_THROWCRASH;
	}
}

LONG WASAPIDetectID() {
	try {
		//BASSWASAPI info
		BASS_WASAPI_DEVICEINFO info;
		char OutputName[MAX_PATH] = "None\0";

		// Initialize registry values
		DWORD WSType = REG_SZ;
		DWORD WSSize = sizeof(OutputName);

		// Open the registry, and get the name of the selected ASIO device
		PrintMessageToDebugLog("WASAPIDetectIDFunc", "Importing default WASAPI device from registry...");
		OpenRegistryKey(Configuration, L"Software\\OmniMIDI\\Configuration", TRUE);
		RegQueryValueExA(Configuration.Address, "WASAPIOutput", NULL, &WSType, (LPBYTE)&OutputName, &WSSize);

		// Loop through the available devices
		for (DWORD CurrentDevice = 0; BASS_WASAPI_GetDeviceInfo(CurrentDevice, &info); CurrentDevice++)
		{
			// Found an item, check if it's a playback device
			if ((strcmp(OutputName, info.id) == 0) &&
				!(info.flags & BASS_DEVICE_LOOPBACK) &&
				!(info.flags & BASS_DEVICE_INPUT) &&
				!(info.flags & BASS_DEVICE_UNPLUGGED) &&
				!(info.flags & BASS_DEVICE_DISABLED))
			{
				// It is POG, return the device
				PrintMessageToDebugLog("WASAPIDetectIDFunc", "Device found. Returning its ID...");
				return CurrentDevice;
			}
		}

		// No device found, return -1 for default output device
		PrintMessageToDebugLog("WASAPIDetectIDFunc", "Device not found. Returning -1...");
		return -1;
	}
	catch (...) {
		_THROWCRASH;
	}
}

LONG ASIODetectID() {
	try {
		// Initialize BASSASIO info
		BASS_ASIO_DEVICEINFO info;
		char OutputName[MAX_PATH] = "None\0";

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
		_THROWCRASH;
	}
}

BOOL InitializeBASSLibrary()
{
	if (HostSessionMode) return TRUE;

	// If DS or WASAPI are selected, then the final stream will not be a decoding channel
	BOOL NotDecodeMode = /*(ManagedSettings.CurrentEngine == DXAUDIO_ENGINE ||*/ ManagedSettings.CurrentEngine == BASS_OUTPUT /*)*/;
	
	// Stream flags
	// const BOOL flags = BASS_DEVICE_STEREO | ((ManagedSettings.CurrentEngine == DXAUDIO_ENGINE) ? BASS_DEVICE_DSOUND : 0);
	
	// Output device
	const int OutputID = BASSDetectID();

	PrintMessageToDebugLog("InitializeBASSLibraryFunc", "Initializing BASS...");

	if (NotDecodeMode && ManagedSettings.FollowDefaultAudioDevice) {
		// If the audio output is set to -1 (Default device),
		// force BASS to follow Windows' default device whenever it's changed
		BASS_SetConfig(BASS_CONFIG_DEV_DEFAULT, (OutputID <= 0) ? 1 : 0);
	}

	BOOL init = BASS_Init(
		NotDecodeMode ? OutputID : 0,
		ManagedSettings.AudioFrequency, 
		NotDecodeMode ? (ManagedSettings.ReduceBootUpDelay ? 0 : BASS_DEVICE_LATENCY) | BASS_DEVICE_STEREO : NULL,
		0, 
		NULL);
	DWORD BERR = BASS_ErrorGetCode();

	if (!init && BERR == BASS_ERROR_ALREADY) {
		PrintMessageToDebugLog("InitializeBASSLibraryFunc", "BASS reported an error!");
		PrintVarToDebugLog("InitializeBASSLibraryFunc", "BERR", &BERR, PRINT_INT32);

		if (HostSessionMode != TRUE) 
			HostSessionMode = TRUE;

		return TRUE;
	}
	else CheckUp(FALSE, ERRORCODE, "BASS Lib Initialization", TRUE);

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
		// Free BASS, BASSASIO and BASSWASAPI
		FreeUpBASSWASAPI();
		FreeUpBASSASIO();
		FreeUpBASS();
		FreeUpXA();

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

BOOL InitializeBASS(BOOL restart) {
	BOOL InitializationCompleted = FALSE;

	if (block_bassinit) return FALSE;

	PrintMessageToDebugLog("InitializeBASSFunc", "The driver is now initializing BASS. Please wait...");

	// The user restarted the synth, add 1 to RestartValue, for the ".WAV mode"
	if (restart) {
		PrintMessageToDebugLog("InitializeBASSFunc", "The driver had to restart the stream.");
		if (ManagedSettings.CurrentEngine == AUDTOWAV) RestartValue++;

		// Unload BASS functions
		FreeFonts();
		FreeUpStream();
	}

BEGSWITCH:
	switch (ManagedSettings.CurrentEngine) {
	case AUDTOWAV:
	{
		if (InitializeBASSLibrary()) {
			if (!InitializeStream(ManagedSettings.AudioFrequency))
				break;

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

			if (!RegQueryValueEx(Configuration.Address, L"AudToWAVFolder", NULL, &dwType, (LPBYTE)&confpath, &cbValueLength)) {
				// If the folder exists, then set the path to that
				PathAppend(encpath, confpath);
				PathAppend(encpath, result2);
				MessageBox(NULL, encpath, L"DEBUG", MB_OK);
			}
			// Otherwise, set the default path to the desktop
			else if (GetFolderPath(FOLDERID_Desktop, CSIDL_DESKTOPDIRECTORY, encpath, sizeof(encpath))) PathAppend(encpath, result2);

			// Convert some values for the following MsgBox
			const int result = MessageBox(NULL, L"You've enabled the \"Output to WAV\" mode.\n\nPress YES to confirm, or press NO to open the configurator\nand disable it.", L"OmniMIDI", MB_ICONINFORMATION | MB_YESNO | MB_SYSTEMMODAL);
			switch (result)
			{
			case IDYES:
				// If the user chose to output to WAV, then continue initializing BASSEnc
				BASS_Encode_Start(OMStream, (const char*)encpath, BASS_ENCODE_PCM | BASS_ENCODE_LIMIT | BASS_ENCODE_RF64 | BASS_UNICODE, 0, 0);
				// Error handling
				CheckUp(FALSE, ERRORCODE, "Encoder Start", TRUE);
				// Stop automatic encoding 
				BASS_Encode_SetPaused(OMStream, true);
				// Error handling
				CheckUp(FALSE, ERRORCODE, "Encoder No-Auto", TRUE);
				// Create buffer
				FSndBuf = new float[BASS_ChannelSeconds2Bytes(OMStream, 0.016) / 4];
				break;
			case IDNO:
				// Otherwise, open the configurator
				TCHAR configuratorapp[MAX_PATH];
				if (GetFolderPath(FOLDERID_SystemX86, CSIDL_SYSTEMX86, configuratorapp, sizeof(configuratorapp)))
				{
					PathAppend(configuratorapp, _T("\\OmniMIDI\\OmniMIDIConfigurator.exe"));
					ShellExecute(NULL, L"open", configuratorapp, L"/AT", NULL, SW_SHOWNORMAL);
					delete configuratorapp;
				}
			}

			RestartValue++;
			PrintMessageToDebugLog("InitializeBASSEncFunc", "BASSenc is now ready!");
			InitializationCompleted = TRUE;
		}

		break;
	}

	case BASS_OUTPUT:
	// case DXAUDIO_ENGINE:
	{
		if (InitializeBASSLibrary()) {
			// Final BASS initialization, set some settings
			PrintMessageToDebugLog("InitializeBASSOutput", "Configuring stream...");
			BASS_SetConfig(BASS_CONFIG_UPDATETHREADS, 0);
			BASS_SetConfig(BASS_CONFIG_UPDATEPERIOD, 0);

			DWORD MinimumBuffer = 0;
			DWORD FinalBuffer = 0;

			/*
			if (ManagedSettings.CurrentEngine == DXAUDIO_ENGINE) {
				if (!ManagedSettings.ReduceBootUpDelay) {
					PrintMessageToDebugLog("InitializeBASSOutput", "Getting buffer info...");
					BASS_GetInfo(&info);

					// The safest value is usually minbuf - 10
					MinimumBuffer = info.minbuf - 10;

					if (MinimumBuffer >= ManagedSettings.BufferLength)
						FinalBuffer = MinimumBuffer;
					else
						FinalBuffer = ManagedSettings.BufferLength;
				}
				else FinalBuffer = ManagedSettings.BufferLength;
			}
			*/

			// Store the latency for debug
			ManagedDebugInfo.AudioLatency = (DOUBLE)FinalBuffer;

			// If the minimum buffer is bigger than the requested buffer length, use the minimum buffer value instead
			PrintMessageToDebugLog("InitializeBASSOutput", "Setting buffer...");
			BASS_SetConfig(BASS_CONFIG_BUFFER, FinalBuffer);

			// Print debug info
			PrintMemoryMessageToDebugLog("InitializeBASSOutput", "Buffer length from BASS_GetInfo (in ms)", false, MinimumBuffer);
			PrintMemoryMessageToDebugLog("InitializeBASSOutput", "Buffer length from registry (in ms)", false, ManagedSettings.BufferLength);
			PrintMemoryMessageToDebugLog("InitializeBASSOutput", "Buffer length from calculations (in ms)", false, FinalBuffer);

			PrintMessageToDebugLog("InitializeBASSOutput", "Initializing stream...");

			// Initialize BASS stream
			if (!InitializeStream(ManagedSettings.AudioFrequency))
			{
				PrintMessageToDebugLog("InitializeBASSOutput", "An error has occurred during BASS' initialization! Freeing audio stream...");
				break;
			}

			// And finally, open the stream
			PrintMessageToDebugLog("InitializeBASSOutput", "Starting stream...");
			BASS_ChannelPlay(OMStream, FALSE);
			CheckUp(FALSE, ERRORCODE, "Channel Play", TRUE);

			InitializationCompleted = TRUE;
		}

		break;
	}

	case WASAPI_ENGINE:
	{
		/*
		if (ManagedSettings.OldWASAPIMode)
		{
			ManagedSettings.CurrentEngine = OLD_WASAPI;
			goto BEGSWITCH;
		}
		*/

		if (InitializeBASSLibrary()) {
			// Free UP WASAPI again, just to be sure
			FreeUpBASSWASAPI();

			// Initialize BASS stream
			if (!InitializeStream(ManagedSettings.AudioFrequency))
			{
				PrintMessageToDebugLog("InitializeWASAPIFunc", "An error has occurred during BASS' initialization! Freeing WASAPI...");
				break;
			}

			BASS_WASAPI_INFO infoW;
			LONG DeviceID = WASAPIDetectID();

			// Initialize WASAPI
			BOOL WInit = BASS_WASAPI_Init(DeviceID, ManagedSettings.AudioFrequency, (ManagedSettings.MonoRendering ? 1 : 2),
				(ManagedSettings.WASAPIExclusive) ? BASS_WASAPI_EXCLUSIVE : 0 | 
				(ManagedSettings.WASAPIRAWMode ? BASS_WASAPI_RAW : 0) |
				(ManagedSettings.WASAPIDoubleBuf ? BASS_WASAPI_BUFFER : 0) |
				BASS_WASAPI_EVENT,
				(float)ManagedSettings.BufferLength / 1000.0f,
				0, WASAPIProc, NULL);

			// Check if it's working
			CheckUp(FALSE, ERRORCODE, "WASAPI initialization", TRUE);
			PrintMessageToDebugLog("InitializeWASAPIFunc", "WASAPI initialized.");

			if (WInit)
			{	
				// Start the device
				BASS_WASAPI_Start();
				CheckUp(FALSE, ERRORCODE, "WASAPI start-up", TRUE);

				if (BASS_WASAPI_GetInfo(&infoW))
				{
					// Store the latency for debug
					ManagedDebugInfo.AudioBufferSize = infoW.buflen / 8;
					ManagedDebugInfo.AudioLatency = ((DOUBLE)ManagedDebugInfo.AudioBufferSize * 1000.0f / (DOUBLE)ManagedSettings.AudioFrequency);
					PrintMessageToDebugLog("InitializeWASAPIFunc", "Stored latency information.");
				}
			}
			else 
			{
				// Return an error, and switch to BASS' own WASAPI implementation
				ManagedSettings.CurrentEngine = BASS_OUTPUT;
				FreeUpBASSWASAPI();
				FreeUpBASS();
				PrintMessageToDebugLog("InitializeWASAPIFunc", "Can not initialize BASSWASAPI.");
				MessageBox(NULL, L"An error has occurred while initializing WASAPI.\n\nPress OK to fallback to BASS' default output engine.", L"OmniMIDI - Error", MB_ICONERROR | MB_OK | MB_SYSTEMMODAL);
				goto BEGSWITCH;
			}

			InitializationCompleted = TRUE;
		}

		break;
	}

	// Oh no it's back!
	case XAUDIO_ENGINE:
	{
		const char* XATmp;
		char XAMsgStr[512] = { 0 };
		FreeUpXA();

		if (InitializeBASSLibrary()) {
			// Initialize BASS stream
			if (!InitializeStream(ManagedSettings.AudioFrequency))
			{
				PrintMessageToDebugLog("InitializeXAFunc", "An error has occurred during BASS' initialization! Freeing XAudio2...");
				break;
			}

			PrintMessageToDebugLog("InitializeXAFunc", "Allocationg XAudio2 struct...");
			SndDrv = CreateXAudio2Stream();

			if (SndDrv->IsLoaded()) {
				PrintMessageToDebugLog("InitializeXAFunc", "Opening XAudio2 device...");
				XATmp = 
					SndDrv->OpenStream(
						NULL, 
						ManagedSettings.AudioFrequency,
						ManagedSettings.MonoRendering ? 1 : 2, 
						4,
						SamplesPerFrame,
						ManagedSettings.XASPFSweepRate);
			}

			if (XATmp != NULL) {
				ManagedSettings.CurrentEngine = WASAPI_ENGINE;
				FreeUpBASS();
				FreeUpXA();
				wsprintfA(XAMsgStr, "XAudio2 was unable to initialize.\nError: %s\n\nPress OK to fallback to WASAPI.", XATmp);
				PrintMessageToDebugLog("InitializeXAFunc", XATmp);
				MessageBoxA(NULL, XAMsgStr, "OmniMIDI - XA ERROR", MB_ICONERROR | MB_OK | MB_SYSTEMMODAL);
				goto BEGSWITCH;
			}
			else PrintMessageToDebugLog("InitializeXAFunc", "XAudio2 stream is up and running.");

			// Store the latency for debug
			ManagedDebugInfo.AudioBufferSize = SamplesPerFrame;
			ManagedDebugInfo.AudioLatency = ((DOUBLE)SamplesPerFrame * 1000.0f / (DOUBLE)ManagedSettings.AudioFrequency) * ManagedSettings.XASPFSweepRate;
			PrintMessageToDebugLog("InitializeWASAPIFunc", "Stored latency information.");

			// Prepare audio buffer
			FSndBuf = new float[SamplesPerFrame];

			InitializationCompleted = TRUE;
		}

		break;
	}

#if !defined(_M_ARM64)
	case ASIO_ENGINE:
	{
		if (InitializeBASSLibrary()) {
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
				goto BEGSWITCH;
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
				goto BEGSWITCH;
			}

			// Initialize the stream
			if (!InitializeStream(ManagedSettings.AudioFrequency))
			{
				PrintMessageToDebugLog("InitializeASIOFunc", "An error has occurred during BASS' initialization! Freeing ASIO...");
				FreeUpBASSASIO();
				break;
			}

			// If ASIO is successfully initialized, go on with the initialization process
			PrintMessageToDebugLog("InitializeASIOFunc", "Initializing BASSASIO...");
			if (BASS_ASIO_Init(DeviceToUse, BASS_ASIO_THREAD | BASS_ASIO_JOINORDER)) {
				CheckUp(TRUE, ERRORCODE, "ASIO Initialized", TRUE);

				// Set the audio frequency
				if (!ManagedSettings.LeaveASIODeviceFreq)
				{
					if (!BASS_ASIO_CheckRate(ManagedSettings.AudioFrequency) && !ManagedSettings.DisableASIOFreqWarn)
					{
						WCHAR Warn[255] = { 0 };

						swprintf(Warn,
							L"The ASIO device reported %dHz as an unsupported frequency.\nYou might encounter audio glitches, crackling or other issues.\n\nPress OK to continue.",
							ManagedSettings.AudioFrequency);

						MessageBox(NULL, Warn, L"OmniMIDI - WARNING", MB_OK | MB_ICONWARNING | MB_SYSTEMMODAL);
					}

					BASS_ASIO_SetRate(ManagedSettings.AudioFrequency);
					CheckUp(TRUE, ERRORCODE, "ASIO Device Frequency Set", !ManagedSettings.DisableASIOFreqWarn);
				}
				else BASS_ASIO_SetRate(0);

				// Set the bit depth for the left channel (ASIO only supports 32-bit float, on Vista+)
				BASS_ASIO_ChannelSetFormat(FALSE, 0, BASS_ASIO_FORMAT_FLOAT);
				CheckUp(TRUE, ERRORCODE, "ASIO Device Format Set", TRUE);

				// If mono rendering is enabled, set the audio frequency of the channels to half the value of the frequency selected
				if (ManagedSettings.MonoRendering == 1) BASS_ASIO_ChannelSetRate(FALSE, 0, ManagedSettings.AudioFrequency / 2);
				// Else, set it to the default frequency
				else BASS_ASIO_ChannelSetRate(FALSE, 0, ManagedSettings.AudioFrequency);
				CheckUp(TRUE, ERRORCODE, "ASIO Channel Frequency Set", TRUE);

				// Enable the channels
				if (ManagedSettings.ASIODirectFeed) {
					BASS_ASIO_ChannelEnableBASS(FALSE, 0, OMStream, TRUE);
					CheckUp(TRUE, ERRORCODE, "ASIO DF Channel Enable", TRUE);
				}
				else {
					BASS_ASIO_ChannelEnable(FALSE, 0, ASIOProc, NULL);
					CheckUp(TRUE, ERRORCODE, "ASIO Channel Enable", TRUE);

					if (!ManagedSettings.MonoRendering) {
						BASS_ASIO_ChannelJoin(FALSE, 1, 0);
						CheckUp(TRUE, ERRORCODE, "ASIO Channel Join", TRUE);
						// Set the bit depth for the right channel as well
						BASS_ASIO_ChannelSetFormat(FALSE, 0, BASS_ASIO_FORMAT_FLOAT);
						CheckUp(TRUE, ERRORCODE, "ASIO Channel Bit Depth Set", TRUE);
					}
				}

				// If mono rendering is enabled, mirror the left channel to the right one as well
				if (ManagedSettings.MonoRendering) {
					BASS_ASIO_ChannelEnableMirror(1, FALSE, 0);
					CheckUp(TRUE, ERRORCODE, "ASIO Device Mono Mode", TRUE);
				}

				// And start the ASIO output
				BASS_ASIO_Start(0, std::thread::hardware_concurrency());
				CheckUp(TRUE, ERRORCODE, "ASIO Start Output", TRUE);

				// Store the latency for debug
				ManagedDebugInfo.AudioBufferSize = BASS_ASIO_GetLatency(FALSE);
				ManagedDebugInfo.AudioLatency = ((DOUBLE)ManagedDebugInfo.AudioBufferSize * 1000.0f / BASS_ASIO_GetRate());
				CheckUp(TRUE, ERRORCODE, "ASIO Get Frequency", FALSE);

				PrintMessageToDebugLog("InitializeASIOFunc", "Done!");
			}
			// Else, something is wrong
			else CheckUp(TRUE, ERRORCODE, "ASIO Initialization", TRUE);

			InitializationCompleted = TRUE;
		}

		break;
	}
#endif

	default:
	{
		PrintMessageToDebugLog("InitializeBASSFunc", "Unknown engine, falling back to WASAPI...");
		ManagedSettings.CurrentEngine = WASAPI_ENGINE;
		RegSetValueEx(Configuration.Address, L"CurrentEngine", NULL, dwType, (LPBYTE)&ManagedSettings.CurrentEngine, sizeof(DWORD));
		goto BEGSWITCH;
	}

	}

	if (!ApplyStreamSettings()) return FALSE;

	// Allocate EVBuffer
	AllocateMemory(restart);

	// Check if "Hyper-playback" mode has been enabled
	_PrsData = HyperMode ? ParseDataHyper : ParseData;
	_PforBASSMIDI = HyperMode ? PrepareForBASSMIDIHyper : PrepareForBASSMIDI;
	_PlayBufData = HyperMode ? PlayBufferedDataHyper : PlayBufferedData;
	_PlayBufDataChk = ManagedSettings.NotesCatcherWithAudio ? (HyperMode ? PlayBufferedDataChunkHyper : PlayBufferedDataChunk) : DummyPlayBufData;

#if !defined(_M_ARM64)
	// Apply LoudMax, if requested
	InitializeBASSVST();
	InitializeOrUpdateEffects();
#endif

	return InitializationCompleted;
}

void LoadSoundFontsToStream() {
	PrintMessageToDebugLog("LoadSoundFontsToStreamFunc", "Loading soundfonts...");

	// If app has a personal SoundFont list, then load it
	if (!LoadSoundfontStartup() == TRUE) {
		// Otherwise, fallback to default SoundFont
		LoadSoundfont(0);
	}
}