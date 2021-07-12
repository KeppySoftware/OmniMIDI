/*
OmniMIDI, a fork of BASSMIDI Driver

Thank you Kode54 for allowing me to fork your awesome driver.
*/
#pragma once

#define DriverSettingsCase(Setting, Mode, Type, SettingStruct, Value, cbValue) \
	case Setting: \
		if (!SettingsManagedByClient) PrintMessageToDebugLog(#Setting, "Please send OM_MANAGE first!!!"); return FALSE; \
		if (cbValue != sizeof(Type)) return FALSE; \
		if (Mode = OM_SET) SettingStruct = *(Type*)Value; \
		else if (Mode = OM_GET) *(Type*)Value = SettingStruct; \
		else return FALSE; \
		break;

// F**k WinMM and Microsoft
typedef VOID(CALLBACK* WMMC)(HMIDIOUT, DWORD, DWORD_PTR, DWORD_PTR, DWORD_PTR);
DWORD OMCallbackMode = CALLBACK_NULL | MIDI_IO_PACKED;
BOOL OMCookedMode = FALSE;

// For callbacks
void DoCallback(DWORD M, DWORD_PTR P1, DWORD_PTR P2) {
	BOOL R = FALSE;

	switch (OMCallbackMode & CALLBACK_TYPEMASK) {
	case CALLBACK_FUNCTION:
	{
		(*(WMMC)OMCallback)((HMIDIOUT)OMHMIDI, M, OMInstance, P1, P2);
		PrintMessageToDebugLog("DoCallback w/ CALLBACK_FUNCTION", "The callback function has been called.");
		break;
	}
	case CALLBACK_EVENT:
	{
		R = SetEvent((HANDLE)OMCallback);
		PrintMessageToDebugLog("DoCallback w/ CALLBACK_EVENT", R ? "The event has been set." : "The event is null or SetEvent failed to set it.");
		break;
	}
	case CALLBACK_THREAD:
	{
		R = PostThreadMessageW((DWORD)OMCallback, M, P1, P2);
		PrintMessageToDebugLog("DoCallback w/ CALLBACK_THREAD", R ? "The message has been sent to the thread handle." : "Failed to send message to thread handle.");
		break;
	}
	case CALLBACK_WINDOW:
	{
		R = PostMessageW((HWND)OMCallback, M, P1, P2);
		PrintMessageToDebugLog("DoCallback w/ CALLBACK_WINDOW", R ? "The message has been sent to the window handle." : "Failed to send message to window handle.");
		break;
	}
	default:
		PrintMessageToDebugLog("DoCallback w/ CALLBACK_NULL", "Function called, but no callback has been requested by the app.");
		break;
	}
}

// CookedPlayer system
VOID KillOldCookedPlayer() {
	if (IsThisThreadActive(CookedThread.ThreadHandle)) {
		CookedPlayerHasToGo = TRUE;
		if (WaitForSingleObject(CookedThread.ThreadHandle, INFINITE) == WAIT_OBJECT_0) {
			CloseHandle(CookedThread.ThreadHandle);
			CookedThread.ThreadHandle = nullptr;
			delete OMCookedPlayer;
			CookedPlayerHasToGo = FALSE;
		}
	}
}

// Code by SonoSooS
void CookedPlayerSystem(CookedPlayer* Player)
{
	QWORD ticker = 0;
	QWORD tickdiff = 0;
	int sleeptime = 0;
	int oldsleep = 0;
	int deltasleep = 0;

	DWORD delaytick = 0;
	BOOL barrier = TRUE;			// This is horrible :s

	constexpr DWORD maxdelay = 100000;	// Adjust responsiveness here
	constexpr DWORD adaption = 100000;	// Adaptive timer nice time >:3

	PrintMessageToDebugLog("CookedPlayerSystem", "Thread is alive!");

	NtQuerySystemTime(&tickdiff);

	while (!CookedPlayerHasToGo)
	{
		if (Player->Paused || !Player->MIDIHeaderQueue)
		{
			PrintMessageToDebugLog("CookedPlayerSystem", "Waiting for unpause and/or header...");
			while (Player->Paused || !Player->MIDIHeaderQueue)
			{
				ticker = (QWORD)-(INT64)maxdelay;
				NtDelayExecution(TRUE, (INT64*)&ticker);
				NtQuerySystemTime(&tickdiff);				// Reset timer
				deltasleep = 0;								// Reset drift
				oldsleep = 0;
				if (CookedPlayerHasToGo) break;
			}
			PrintMessageToDebugLog("CookedPlayerThread", "Playback started!");
			continue;
		}

		if (delaytick)
		{
			NtQuerySystemTime(&ticker);
			DWORD tdiff = (DWORD)(ticker - tickdiff);		// Calculate elapsed time
			tickdiff = ticker;
			int delt = (int)(tdiff - oldsleep);				// Calculate drift
			deltasleep += delt;								// Accumlate drift

			sleeptime = (delaytick * Player->TempoMulti);	// TODO: can overflow
			//sleeptime *= speedcontrol;
			oldsleep = sleeptime;

			Player->TimeAccumulator += sleeptime;

			if (deltasleep > 0)								// Can underflow, don't speed up if we pushed too hard
				sleeptime -= deltasleep;					// Adjust for time drift

			if (0) //if(sleeptime > maxdelay)
			{ // Yes, this is very coarse, but the adaptive timer will keep it in sync
				sleeptime = maxdelay;
				DWORD acc = maxdelay / Player->TempoMulti;	// Time to ticks
				if (!acc) acc = 1;

				if (sleeptime <= 0)							// Overloaded
				{
					if (deltasleep < adaption);
					else deltasleep = adaption;				// Don't overpush
				}
				else
				{
					INT64 usl = -((INT64)sleeptime);
					NtDelayExecution(FALSE, &usl);
				}

				delaytick -= acc;
				if (delaytick >> 31)
					PrintMessageToDebugLog("CookedPlayerSystem", "Warning: DelayTick integer underflow!");
				Player->TickAccumulator += acc;

				continue;
			}
			else
			{
				if (sleeptime <= 0)							// Overloaded
				{
					if (deltasleep < adaption);
					else deltasleep = adaption;				// Don't overpush
				}
				else
				{
					INT64 usl = -((INT64)sleeptime);
					NtDelayExecution(FALSE, &usl);
				}

				Player->TickAccumulator += delaytick;
				delaytick = 0;

				continue;
			}
		}

		LPMIDIHDR hdr = Player->MIDIHeaderQueue;

		if (hdr->dwFlags & MHDR_DONE)
		{
			LockForWriting(&Player->Lock);
			Player->MIDIHeaderQueue = hdr->lpNext;
			UnlockForWriting(&Player->Lock);
			continue;
		}

		while (!Player->Paused)
		{
			if (hdr->dwOffset >= hdr->dwBytesRecorded)
			{
				LockForWriting(&Player->Lock);
				hdr->dwFlags |= MHDR_DONE;
				hdr->dwFlags &= ~MHDR_INQUEUE;
				LPMIDIHDR nexthdr = hdr->lpNext;
				UnlockForWriting(&Player->Lock);

				Player->MIDIHeaderQueue = nexthdr;

				DoCallback(MOM_DONE, (DWORD_PTR)hdr, 0);

				hdr->dwOffset = 0;
				hdr = nexthdr;

				break;
			}

			MIDIEVENT* evt = (MIDIEVENT*)(hdr->lpData + hdr->dwOffset);

			if (barrier)
			{
				barrier = FALSE;
				delaytick = evt->dwDeltaTime;

				if (delaytick) break;
			}

			// Reset barrier
			barrier = TRUE;

			if (evt->dwEvent & MEVT_F_CALLBACK)
			{
				PrintMessageToDebugLog("CookedPlayerSystem", "Reached MEVT_F_CALLBACK! Let's warn the app about it.");
				DoCallback(MOM_POSITIONCB, (DWORD_PTR)hdr, 0);
			}

			BYTE evid = (evt->dwEvent >> 24) & 0xBF;

			/*
			if(evid != MEVT_NOP && evid != MEVT_VERSION)
			{
				CrashMessage(L"CookedPlayerThread | evid not NOP", nullptr);
			}
			*/

			switch (evid) {
			case MEVT_SHORTMSG:
				_PrsData(evt->dwEvent);
				break;
			case MEVT_LONGMSG:
				BASS_MIDI_StreamEvents(OMStream, BASS_MIDI_EVENTS_RAW, evt->dwParms, evt->dwEvent & 0xFFFFFF);
				break;
			case MEVT_TEMPO:
				Player->Tempo = evt->dwEvent & 0xFFFFFF;
				Player->TempoMulti = (DWORD)((Player->Tempo * 10) / Player->TimeDiv);
				break;
			default:
				break;
			}

			if (evt->dwEvent & MEVT_F_LONG)
			{
				DWORD acc = ((evt->dwEvent & 0xFFFFFF) + 3) & ~3;	// PAD
				Player->ByteAccumulator += acc;
				hdr->dwOffset += acc;
			}

			Player->ByteAccumulator += 0xC;
			hdr->dwOffset += 0xC;
		}
	}

	// Close the thread
	PrintMessageToDebugLog("CookedPlayerSystem", "Closing CookedPlayer thread...");
	TerminateThread(&CookedThread, TRUE, 0);
}

BOOL PrepareDriver() {
	PrintMessageToDebugLog("StartDriver", "Initializing driver...");

	// Load the settings, and allocate the memory for the EVBuffer
	LoadSettings(FALSE, FALSE);

	// Initialize the BASS output device, and set up the streams
	if (!InitializeBASS(FALSE))
		return FALSE;

	SetUpStream();
	LoadSoundFontsToStream();

	// Done, now initialize the threads
	CreateThreads();

	SetEvent(OMReady);

	return TRUE;
}

// KDMAPI calls
BOOL StreamHealthCheck() {
	// If BASS is forbidden from initializing itself, then abort immediately
	if (block_bassinit) return FALSE;

	// Check if the call failed
	if ((BASS_ChannelIsActive(OMStream) == BASS_ACTIVE_STOPPED || ManagedSettings.LiveChanges)) {
		PrintMessageToDebugLog("StreamWatchdog", "Stream is down! Restarting audio stream...");

		// It did, reload the settings and reallocate the memory for the buffer
		CloseThreads(FALSE);
		LoadSettings(TRUE, FALSE);

		// Initialize the BASS output device, and set up the streams
		if (InitializeBASS(TRUE)) {
			SetUpStream();
			LoadSoundFontsToStream();

			// Done, now initialize the threads
			CreateThreads();
		}
		else PrintMessageToDebugLog("StreamWatchdog", "Failed to initialize stream! Retrying...");

		return FALSE;
	}
	else { 
		if (stop_thread || 
			(!ATThread.ThreadHandle && 
				(ManagedSettings.CurrentEngine != WASAPI_ENGINE || 
				(ManagedSettings.CurrentEngine == ASIO_ENGINE && ManagedSettings.ASIODirectFeed))))
			CreateThreads();
	}

	return TRUE;
}

void Supervisor(LPVOID lpV) {
	try {
		// Parse the app name, and start the debug pipe to the debug window
		PrintMessageToDebugLog("StreamWatchdog", "Checking if app is allowed to use RTSS OSD...");
		if (!AlreadyStartedOnce) StartDebugPipe(FALSE);

		// Load the BASS functions
		if (!LoadBASSFunctions())
			// If BASS is still unavailable, commit suicide
			CrashMessage(L"NoBASSFound");

		// Ok, everything's ready, do not open more debug pipes from now on
		DriverInitStatus = TRUE;
		AlreadyStartedOnce = TRUE;
		bass_initialized = TRUE;

		if (!PrepareDriver())
			CrashMessage(L"0xDEADDEAD");

		// Check system
		PrintMessageToDebugLog("StreamWatchdog", "Checking for settings changes or hotkeys...");

		while (!stop_thread) {
			// Check if the threads and streams are still alive
			if (StreamHealthCheck());
			{
				// It's alive, do registry stuff

				LoadSettings(FALSE, TRUE);			// Load real-time settings
				LoadCustomInstruments();			// Load custom instrument values from the registry
				KeyShortcuts();						// Check for keystrokes (ALT+1, INS, etc..)
				SFDynamicLoaderCheck();				// Check current active voices, rendering time, etc..
				MixerCheck();						// Send dB values to the mixer
				RevbNChor();						// Check if custom reverb/chorus values are enabled
				InitializeEventsProcesserThreads(); // Check if the user wants to parse the notes through a separate thread

				// Check the current output volume
				CheckVolume(FALSE);

				// Assign ProcData
				_ProcData = ManagedSettings.NotesCatcherWithAudio ? ProcDataSameThread : ProcData;
			}

			// I SLEEP
			Sleep(333);
		}
	}
	catch (...) {
		CrashMessage(L"SettingsAndHealthThread");
	}

	// Wait for the other threads to finish
	PrintMessageToDebugLog("StreamWatchdog", "Waiting for events processer thread...");
	if (!ManagedSettings.NotesCatcherWithAudio)
	{
		if (WaitForSingleObject(EPThreadDone, INFINITE) == WAIT_OBJECT_0)
			ResetEvent(EPThreadDone);
	}

	if (ManagedSettings.CurrentEngine != WASAPI_ENGINE &&
		ManagedSettings.CurrentEngine != ASIO_ENGINE) {
		PrintMessageToDebugLog("StreamWatchdog", "Waiting for audio renderer thread...");
		if (WaitForSingleObject(ATThreadDone, INFINITE) == WAIT_OBJECT_0)
			ResetEvent(ATThreadDone);
	}

	// Unload BASS functions
	FreeFonts();
	FreeUpStream();
	UnloadBASSFunctions();

	// Free memory
	FreeUpMemory();

	// Close registry keys
	PrintMessageToDebugLog("StreamWatchdog", "Closing registry keys...");
	CloseRegistryKey(MainKey);
	PrintMessageToDebugLog("StreamWatchdog", "Closed MainKey...");
	CloseRegistryKey(Configuration);
	PrintMessageToDebugLog("StreamWatchdog", "Closed Configuration...");
	CloseRegistryKey(Channels);
	PrintMessageToDebugLog("StreamWatchdog", "Closed Channels...");
	CloseRegistryKey(ChanOverride);
	PrintMessageToDebugLog("StreamWatchdog", "Closed ChanOverride...");
	CloseRegistryKey(SFDynamicLoader);
	PrintMessageToDebugLog("StreamWatchdog", "Closed SFDynamicLoader...");

	SetEvent(OMReady);

	// Close the thread
	PrintMessageToDebugLog("StreamWatchdog", "Closing health thread...");
	TerminateThread(&HealthThread, TRUE, 0);
}

BOOL DoStartClient() {
	if (!DriverInitStatus && !block_bassinit) {
		// Create an event, to wait for the driver to be ready
		if (!OMReady)
			OMReady = CreateEvent(NULL, TRUE, FALSE, L"OMReady");

		if (!ATThreadDone)
			ATThreadDone = CreateEvent(NULL, TRUE, FALSE, L"ATThreadDone");

		if (!EPThreadDone)
			EPThreadDone = CreateEvent(NULL, TRUE, FALSE, L"EPThreadDone");

		// Create the main thread
		PrintMessageToDebugLog("StartDriver", "Starting main watchdog thread...");
		CheckIfThreadClosed(&HealthThread);
		HealthThread.ThreadHandle = (HANDLE)_beginthreadex(NULL, 0, (_beginthreadex_proc_type)Supervisor, NULL, 0, &HealthThread.ThreadAddress);
		SetThreadPriority(HealthThread.ThreadHandle, THREAD_PRIORITY_NORMAL);
		PrintMessageToDebugLog("StartDriver", "Done!");

		// Wait for the SoundFonts to load, then close the event's handle
		PrintMessageToDebugLog("StartDriver", "Waiting for the driver to signal if it's ready...");
		if (WaitForSingleObject(OMReady, INFINITE) == WAIT_OBJECT_0)
			ResetEvent(OMReady);

		PrintMessageToDebugLog("StartDriver", "Driver initialized.");
		return TRUE;
	}

	return FALSE;
}

BOOL DoStopClient() {
	if (DriverInitStatus) 
	{
		PrintMessageToDebugLog("StopDriver", "Terminating driver...");

		// Close the threads and free up the allocated memory
		PrintMessageToDebugLog("StopDriver", "Freeing memory...");
		CloseThreads(TRUE);

		bass_initialized = FALSE;

		PrintMessageToDebugLog("StartDriver", "Waiting for the threads to go offline..");
		if (WaitForSingleObject(OMReady, INFINITE) == WAIT_OBJECT_0)
			ResetEvent(OMReady);

		PrintMessageToDebugLog("StopDriver", "Deleting handles...");
		if (OMReady)
		{
			CloseHandle(OMReady);
			OMReady = NULL;
		}		
		if (ATThreadDone)
		{
			CloseHandle(ATThreadDone);
			ATThreadDone = NULL;
		}
		if (EPThreadDone)
		{
			CloseHandle(EPThreadDone);
			EPThreadDone = NULL;
		}

		// Boopers
		DriverInitStatus = FALSE;
		PrintMessageToDebugLog("StopDriver", "Driver terminated.");
	}
	else PrintMessageToDebugLog("StopDriver", "The driver is not initialized.");

	return TRUE;
}

BOOL KDMAPI ReturnKDMAPIVer(LPDWORD Major, LPDWORD Minor, LPDWORD Build, LPDWORD Revision) {
	if (Major == NULL || Minor == NULL || Build == NULL || Revision == NULL) {
		PrintMessageToDebugLog("KDMAPI_RKV", "One of the pointers passed to the RKV function is invalid.");
		MessageBox(NULL, L"One of the pointers passed to the ReturnKDMAPIVer function is invalid!", L"KDMAPI ERROR", MB_OK | MB_ICONHAND | MB_SYSTEMMODAL);
		return FALSE;
	}

	PrintMessageToDebugLog("KDMAPI_RKV", "The app wants to know what version of KDMAPI is currently available.");
	*Major = CUR_MAJOR; *Minor = CUR_MINOR; *Build = CUR_BUILD; *Revision = CUR_REV;
	PrintMessageToDebugLog("KDMAPI_RKV", "Now they know.");
	return TRUE;
}

BOOL KDMAPI IsKDMAPIAvailable() {
	// Parse the current state of the KDMAPI
	OpenRegistryKey(Configuration, L"Software\\OmniMIDI\\Configuration", TRUE);

	PrintMessageToDebugLog("KDMAPI_IKA", "Interrogating registry about KDMAPI status...");
	long lResult = RegQueryValueEx(Configuration.Address, L"KDMAPIEnabled", NULL, &dwType, (LPBYTE)& KDMAPIEnabled, &dwSize);
	PrintMessageToDebugLog("KDMAPI_IKA", "Done!");

	// If the state is not available or it hasn't been set, keep it enabled by default
	if (lResult != ERROR_SUCCESS)
		KDMAPIEnabled = TRUE;

	// Return the state
	return KDMAPIEnabled;
}

BOOL KDMAPI InitializeKDMAPIStream() {
	if (!AlreadyInitializedViaKDMAPI && !bass_initialized) {
		PrintMessageToDebugLog("KDMAPI_IKS", "The app requested the driver to initialize its audio stream.");

		// The client manually called a KDMAPI init call, KDMAPI is available no matter what
		AlreadyInitializedViaKDMAPI = TRUE;
		KDMAPIEnabled = TRUE;
		EnableBuiltInHandler("KDMAPI_IKS");

		// Enable the debug log, if the process isn't banned
		OpenRegistryKey(Configuration, L"Software\\OmniMIDI\\Configuration", FALSE);
		RegQueryValueEx(Configuration.Address, L"DebugMode", NULL, &dwType, (LPBYTE)&ManagedSettings.DebugMode, &dwSize);
		if (ManagedSettings.DebugMode) CreateConsole();

		// Start the driver's engine
		if (!DoStartClient()) {
			PrintMessageToDebugLog("KDMAPI_IKS", "KDMAPI failed to initialize.");
			return FALSE;
		}

		PrintMessageToDebugLog("KDMAPI_IKS", "KDMAPI is now active.");
		return TRUE;
	}

	PrintMessageToDebugLog("KDMAPI_IKS", "InitializeKDMAPIStream called, even though the driver is already active.");
	return FALSE;
}

BOOL KDMAPI TerminateKDMAPIStream() {
	// If the driver is already initialized, close it
	if (AlreadyInitializedViaKDMAPI && bass_initialized) {
		// Prevent BASS from reinitializing itself
		block_bassinit = TRUE;

		PrintMessageToDebugLog("KDMAPI_TKS", "The app requested the driver to terminate its audio stream.");
		DoStopClient();
		AlreadyInitializedViaKDMAPI = FALSE;
		KDMAPIEnabled = FALSE;
		DisableBuiltInHandler("KDMAPI_TKS");
		PrintMessageToDebugLog("KDMAPI_TKS", "KDMAPI is now in sleep mode.");

		// OK now it's fine
		block_bassinit = FALSE;

		return TRUE;
	}
	else {
		if (!AlreadyInitializedViaKDMAPI && bass_initialized)
			PrintMessageToDebugLog("KDMAPI_TKS", "You cannot call TerminateKDMAPIStream if OmniMIDI has been initialized through WinMM.");
		else
			PrintMessageToDebugLog("KDMAPI_TKS", "TerminateKDMAPIStream called, even though the driver is already sleeping.");
	}

	return FALSE;
}

BOOL KDMAPI InitializeCallbackFeatures(HMIDI OMHM, DWORD_PTR OMCB, DWORD_PTR OMI, DWORD_PTR OMU, DWORD OMCM) {
	// Copy values to memory
	const BOOL NV = ((OMCM != NULL) && (!OMCB && !OMI));

	OMHMIDI = OMHM;
	OMCallback = NV ? NULL : OMCB;
	OMInstance = NV ? NULL : OMI;
	OMCallbackMode = NV ? NULL : OMCM;

	if ((OMCM & CALLBACK_TYPEMASK) == CALLBACK_WINDOW) 
	{
		if (OMCallback && !IsWindow((HWND)OMCallback))
		{
			PrintMessageToDebugLog("ICF", "The application requested a CALLBACK_WINDOW, but the HWND passed to OMCallback isn't valid.");
			return FALSE;
		}
	}

	if (NV) PrintMessageToDebugLog("ICF", "The application requested the driver to use callbacks, but no callback address has been given.");

	if ((OMCallbackMode & MIDI_IO_COOKED)) {
		PrintMessageToDebugLog("ICF", "MIDI_IO_COOKED requested.");

		// Prepare registry for CookedPlayer
		OpenRegistryKey(Configuration, L"Software\\OmniMIDI\\Configuration", FALSE);
		RegQueryValueEx(Configuration.Address, L"DisableCookedPlayer", NULL, &dwType, (LPBYTE)&ManagedSettings.DisableCookedPlayer, &dwSize);

		if (ManagedSettings.DisableCookedPlayer) {
			PrintMessageToDebugLog("ICF", "CookedPlayer has been disabled in the configurator.");
			return TRUE;
		}

		// Prepare the CookedPlayer
		PrintMessageToDebugLog("ICF", "Preparing CookedPlayer struct...");

		OMCookedPlayer = new (std::nothrow) CookedPlayer();
		if (OMCookedPlayer != nullptr)
		{
			OMCookedPlayer->Paused = TRUE;
			OMCookedPlayer->Tempo = 500000;
			OMCookedPlayer->TimeDiv = 384;
			OMCookedPlayer->TempoMulti = ((OMCookedPlayer->Tempo * 10) / OMCookedPlayer->TimeDiv);
			PrintStreamValueToDebugLog("ICF", "TempoMulti", OMCookedPlayer->TempoMulti);

			PrintMessageToDebugLog("ICF", "CookedPlayer struct prepared.");

			// Create player thread
			PrintMessageToDebugLog("ICF", "Preparing thread for CookedPlayer...");
			CookedThread.ThreadHandle = (HANDLE)_beginthreadex(NULL, 0, (_beginthreadex_proc_type)CookedPlayerSystem, OMCookedPlayer, 0, &CookedThread.ThreadAddress);

			PrintMessageToDebugLog("ICF", "Thread is running. The driver is now ready to receive MIDI headers for the CookedPlayer.");

			OMCookedMode = TRUE;

			return TRUE;
		}

		PrintMessageToDebugLog("ICF", "An error has occured while preparing CookedPlayer.");
		return FALSE;
	}		

	return TRUE;
}

VOID KDMAPI RunCallbackFunction(DWORD Msg, DWORD_PTR P1, DWORD_PTR P2) {
	DoCallback(Msg, P1, P2);
}

VOID KDMAPI ResetKDMAPIStream() {
	// Redundant
	if (bass_initialized)
		ResetSynth(FALSE, TRUE);
}

BOOL KDMAPI SendCustomEvent(DWORD eventtype, DWORD chan, DWORD param) noexcept {
	return _BMSE(OMStream, chan, eventtype, param);
}

MMRESULT KDMAPI SendDirectData(DWORD dwMsg) noexcept {
	// Send it to the pointed ParseData function (Either ParseData or ParseDataHyper)
	return _PrsData(dwMsg);
}

MMRESULT KDMAPI SendDirectDataNoBuf(DWORD dwMsg) noexcept {
	// Send the data directly to BASSMIDI, bypassing the buffer altogether
	_PforBASSMIDI(0, dwMsg);
	return MMSYSERR_NOERROR;
}

MMRESULT KDMAPI PrepareLongData(MIDIHDR * IIMidiHdr) {
	// Check if the MIDIHDR buffer is valid
	if (!IIMidiHdr)														// Buffer doesn't exist
		return DebugResult("PrepareLongData", MMSYSERR_INVALPARAM, "The buffer doesn't exist, or hasn't been allocated.");

	if (IIMidiHdr->dwBufferLength > LONGMSG_MAXSIZE)					// Buffer is bigger than 64K
		return DebugResult("PrepareLongData", MMSYSERR_INVALPARAM, "The given stream buffer is greater than 64K.");

	if (IIMidiHdr->dwBytesRecorded > IIMidiHdr->dwBufferLength ||		// The recorded buffer is bigger than the actual buffer? How.
		IIMidiHdr->dwBytesRecorded < 1)									// Buffer is smaller tha one?
		return DebugResult("PrepareLongData", MMSYSERR_INVALPARAM, "Invalid buffer size passed to dwBytesRecorded.");

	if (IIMidiHdr->dwFlags & MHDR_PREPARED)								// Already prepared, everything is fine
		return DebugResult("PrepareLongData", MMSYSERR_NOERROR, "The buffer is already prepared.");

	// Lock the buffer
	if (!VirtualLock(&IIMidiHdr->lpData, IIMidiHdr->dwBytesRecorded))	// Unable to lock
		return DebugResult("PrepareLongData", MMSYSERR_NOMEM, "VirtualLock failed to lock the buffer to the virtual address space. Not enough memory available.");
	else PrintMessageToDebugLog("PrepareLongData", "Buffer locked to virtual address space.");

	PrintStreamValueToDebugLog("PrepareLongData", "IIMidiHdr Address", (DWORD)IIMidiHdr);

	// Mark the buffer as prepared, and say that everything is hunky-dory
	PrintMessageToDebugLog("PrepareLongData", "Marking as prepared...");
	IIMidiHdr->dwFlags |= MHDR_PREPARED;

	PrintMessageToDebugLog("PrepareLongData", "Function succeded.");
	return MMSYSERR_NOERROR;
}

MMRESULT KDMAPI UnprepareLongData(MIDIHDR * IIMidiHdr) {
	// Check if the MIDIHDR buffer is valid
	if (!IIMidiHdr)													// Buffer doesn't exist
		return DebugResult("UnprepareLongData", MMSYSERR_INVALPARAM, "The buffer doesn't exist, or hasn't been allocated.");

	if (IIMidiHdr->dwBufferLength > LONGMSG_MAXSIZE)				// Buffer is bigger than 64K
		return DebugResult("UnprepareLongData", MMSYSERR_INVALPARAM, "The given stream buffer is greater than 64K.");

	if (IIMidiHdr->dwBytesRecorded > IIMidiHdr->dwBufferLength ||	// The recorded buffer is bigger than the actual buffer? How.
		IIMidiHdr->dwBytesRecorded < 1)								// Buffer is smaller tha one?
		return DebugResult("UnprepareLongData", MMSYSERR_INVALPARAM, "Invalid buffer size passed to dwBytesRecorded.");

	if (IIMidiHdr->dwFlags & MHDR_INQUEUE)							// The buffer is currently being played from the driver, cannot unprepare
		return DebugResult("UnprepareLongData", MIDIERR_STILLPLAYING, "The buffer is still in queue.");

	// Unlock the buffer
	if (!VirtualUnlock(&IIMidiHdr->lpData, IIMidiHdr->dwBytesRecorded))
	{
		DWORD e = GetLastError();

		// 0x9E == Buffer already unlocked
		if (e != 0x9E)
			// If that's not the error, then something wrong happened
			CrashMessage(L"VirtualUnlock on long data");
		else PrintMessageToDebugLog("UnprepareLongData", "The buffer is already unlocked.");
	}
	else PrintMessageToDebugLog("UnprepareLongData", "Buffer unlocked from virtual address space.");

	PrintStreamValueToDebugLog("UnprepareLongData", "IIMidiHdr Address", (DWORD)IIMidiHdr);

	// Mark the buffer as unprepared
	PrintMessageToDebugLog("UnprepareLongData", "Marking as unprepared...");
	IIMidiHdr->dwFlags &= ~MHDR_PREPARED;

	PrintMessageToDebugLog("UnprepareLongData", "Function succeded.");
	return MMSYSERR_NOERROR;
}

MMRESULT KDMAPI SendDirectLongData(MIDIHDR * IIMidiHdr) {
	// Check if the MIDIHDR buffer is valid and if the stream is alive
	while (!bass_initialized)					// The driver isn't ready
		/* return */ DebugResult("SendDirectLongData", MIDIERR_NOTREADY, "BASS hasn't been initialized yet.");
		// Since some apps don't listen to the MIDIERR_NOTREADY return value,
		// I'm forced to make OmniMIDI spinlock until BASS is ready. (Thank you VanBasco.)
		_FWAIT;

	if (!IIMidiHdr)													// The buffer doesn't exist, invalid parameter
		return DebugResult("SendDirectLongData", MMSYSERR_INVALPARAM, "The buffer doesn't exist, or hasn't been allocated.");

	if (IIMidiHdr->dwBufferLength > LONGMSG_MAXSIZE)				// Buffer is bigger than 64K
		return DebugResult("SendDirectLongData", MMSYSERR_INVALPARAM, "The given stream buffer is greater than 64K.");

	if (IIMidiHdr->dwBytesRecorded > IIMidiHdr->dwBufferLength ||	// The recorded buffer is bigger than the actual buffer? How.
		IIMidiHdr->dwBytesRecorded < 1)								// Buffer is smaller tha one?
		return DebugResult("SendDirectLongData", MMSYSERR_INVALPARAM, "Invalid buffer size passed to dwBytesRecorded.");

	if (!(IIMidiHdr->dwFlags & MHDR_PREPARED))						// The buffer is not prepared
		return DebugResult("SendDirectLongData", MIDIERR_UNPREPARED, "The buffer is not prepared");

	// Mark the buffer as in queue
	IIMidiHdr->dwFlags &= ~MHDR_DONE;
	IIMidiHdr->dwFlags |= MHDR_INQUEUE;

	// Do the stuff with it
	BOOL res = SendLongToBASSMIDI(IIMidiHdr);

	// Mark the buffer as done
	IIMidiHdr->dwFlags &= ~MHDR_INQUEUE;
	IIMidiHdr->dwFlags |= MHDR_DONE;

	return MMSYSERR_NOERROR;
}

MMRESULT KDMAPI SendDirectLongDataNoBuf(LPSTR MidiHdrData, DWORD MidiHdrDataLen) {
	if (!MidiHdrData)
		// The buffer doesn't exist, invalid parameter
		return DebugResult("SendDirectLongDataNoBuf", MMSYSERR_INVALPARAM, "No pointer has been passed to MidiHdrData.");

	if (!MidiHdrDataLen < 1 || MidiHdrDataLen > 65535)
		// Invalid buffer size
		return DebugResult("SendDirectLongDataNoBuf", MMSYSERR_INVALPARAM, "Invalid value passed for MidiHdrDataLen.");

	MIDIHDR mhdr;
	mhdr.dwFlags |= MHDR_PREPARED;
	mhdr.lpData = MidiHdrData;
	mhdr.dwBufferLength = MidiHdrDataLen;
	mhdr.dwBytesRecorded = MidiHdrDataLen;

	BOOL res = SendLongToBASSMIDI(&mhdr);

	// Tell the app that the buffer has failed to be played
	if (!res) {
		char Msg[NTFS_MAX_PATH] = { 0 };

		sprintf(Msg, "The long buffer (MIDIHDR) sent to OmniMIDI wasn't able to be recognized.\n\nUnrecognized sequence: ");

		for (int i = 0; i < MidiHdrDataLen; i++)
			sprintf(Msg + strlen(Msg), "%02X", (BYTE)(MidiHdrData[i]));

		sprintf(Msg + strlen(Msg), "\n");

		return DebugResult("SendDirectLongDataNoBuf", MMSYSERR_INVALPARAM, Msg);
	}

	return MMSYSERR_NOERROR;
}

BOOL KDMAPI DriverSettings(DWORD Setting, DWORD Mode, LPVOID Value, UINT cbValue) {
	switch (Setting) 
	{

	case OM_MANAGE:
	{
		SettingsManagedByClient = TRUE;
		return TRUE;
	}

	case OM_LEAVE:
	{
		SettingsManagedByClient = FALSE;
		return TRUE;
	}

	case ON1I8F97TJ6S5SI07LDPJBSB:
	{
		IsKDMAPIViaWinMM = FALSE;
		return TRUE;
	}

	case INVC2MDUBR3YR8DWOF2L55WL:
	{
		IsKDMAPIViaWinMM = TRUE;
		return TRUE;
	}

	DriverSettingsCase(OM_CAPFRAMERATE, Mode, BOOL, ManagedSettings.CapFramerate, Value, cbValue);
	DriverSettingsCase(OM_DEBUGMMODE, Mode, DWORD, ManagedSettings.DebugMode, Value, cbValue);
	DriverSettingsCase(OM_DISABLEFADEOUT, Mode, BOOL, ManagedSettings.DisableNotesFadeOut, Value, cbValue);
	DriverSettingsCase(OM_DONTMISSNOTES, Mode, BOOL, ManagedSettings.DontMissNotes, Value, cbValue);
	DriverSettingsCase(OM_ENABLESFX, Mode, BOOL, ManagedSettings.EnableSFX, Value, cbValue);
	DriverSettingsCase(OM_FULLVELOCITY, Mode, BOOL, ManagedSettings.FullVelocityMode, Value, cbValue);
	DriverSettingsCase(OM_IGNOREVELOCITYRANGE, Mode, BOOL, ManagedSettings.IgnoreNotesBetweenVel, Value, cbValue);
	DriverSettingsCase(OM_IGNOREALLEVENTS, Mode, BOOL, ManagedSettings.IgnoreAllEvents, Value, cbValue);
	DriverSettingsCase(OM_IGNORESYSRESET, Mode, BOOL, ManagedSettings.IgnoreSysReset, Value, cbValue);
	DriverSettingsCase(OM_LIMITRANGETO88, Mode, BOOL, ManagedSettings.LimitTo88Keys, Value, cbValue);
	DriverSettingsCase(OM_MONORENDERING, Mode, BOOL, ManagedSettings.MonoRendering, Value, cbValue);
	DriverSettingsCase(OM_NOTEOFF1, Mode, BOOL, ManagedSettings.NoteOff1, Value, cbValue);
	DriverSettingsCase(OM_EVENTPROCWITHAUDIO, Mode, BOOL, ManagedSettings.NotesCatcherWithAudio, Value, cbValue);
	DriverSettingsCase(OM_SINCINTER, Mode, BOOL, ManagedSettings.SincInter, Value, cbValue);
	DriverSettingsCase(OM_AUDIOBITDEPTH, Mode, DWORD, ManagedSettings.AudioBitDepth, Value, cbValue);
	DriverSettingsCase(OM_AUDIOFREQ, Mode, DWORD, ManagedSettings.AudioFrequency, Value, cbValue);
	DriverSettingsCase(OM_CURRENTENGINE, Mode, DWORD, ManagedSettings.CurrentEngine, Value, cbValue);
	DriverSettingsCase(OM_BUFFERLENGTH, Mode, DWORD, ManagedSettings.BufferLength, Value, cbValue);
	DriverSettingsCase(OM_MAXRENDERINGTIME, Mode, DWORD, ManagedSettings.MaxRenderingTime, Value, cbValue);
	DriverSettingsCase(OM_MINIGNOREVELRANGE, Mode, DWORD, ManagedSettings.MinVelIgnore, Value, cbValue);
	DriverSettingsCase(OM_MAXIGNOREVELRANGE, Mode, DWORD, ManagedSettings.MaxVelIgnore, Value, cbValue);
	DriverSettingsCase(OM_OUTPUTVOLUME, Mode, DWORD, ManagedSettings.OutputVolume, Value, cbValue);
	DriverSettingsCase(OM_TRANSPOSE, Mode, DWORD, ManagedSettings.TransposeValue, Value, cbValue);
	DriverSettingsCase(OM_MAXVOICES, Mode, DWORD, ManagedSettings.MaxVoices, Value, cbValue);
	DriverSettingsCase(OM_SINCINTERCONV, Mode, DWORD, ManagedSettings.SincConv, Value, cbValue);
	DriverSettingsCase(OM_OVERRIDENOTELENGTH, Mode, BOOL, ManagedSettings.OverrideNoteLength, Value, cbValue);
	DriverSettingsCase(OM_NOTELENGTH, Mode, DWORD, ManagedSettings.NoteLengthValue, Value, cbValue);
	DriverSettingsCase(OM_ENABLEDELAYNOTEOFF, Mode, BOOL, ManagedSettings.DelayNoteOff, Value, cbValue);
	DriverSettingsCase(OM_DELAYNOTEOFFVAL, Mode, DWORD, ManagedSettings.DelayNoteOffValue, Value, cbValue);
	DriverSettingsCase(OM_CHANUPDLENGTH, Mode, DWORD, ManagedSettings.ChannelUpdateLength, Value, cbValue);
	DriverSettingsCase(OM_UNLOCKCHANS, Mode, BOOL, UnlimitedChannels, Value, cbValue);

	default:
	{
		MessageBox(NULL, L"Unknown setting passed to DriverSettings.", L"OmniMIDI - KDMAPI ERROR", MB_OK | MB_ICONERROR | MB_SYSTEMMODAL);
		return FALSE;
	}

	}

	if (Mode == OM_SET) 
	{
		if (AlreadyInitializedViaKDMAPI)
		{
			PrintMessageToDebugLog("KDMAPI_DS", "Applying new settings to the driver...");
			LoadSettings(FALSE, TRUE);
			PrintMessageToDebugLog("KDMAPI_DS", "Done.");
		}
		else PrintMessageToDebugLog("KDMAPI_DS", "The new settings will be applied once the driver is started.");
	}
	else PrintMessageToDebugLog("KDMAPI_DS", "Copied required setting to pointed value.");

	return TRUE;
}

DebugInfo* KDMAPI GetDriverDebugInfo() {
	// Parse the debug info, and return them to the app.
	PrintMessageToDebugLog("KDMAPI_GDDI", "Passed pointer to DebugInfo to the KDMAPI-ready application.");
	return &ManagedDebugInfo;
}

BOOL KDMAPI LoadCustomSoundFontsList(LPWSTR Directory) {
	// Load the SoundFont from the specified path (It can be a sf2/sfz or a sflist)
	if (!AlreadyInitializedViaKDMAPI) {
		MessageBox(NULL, L"Initialize OmniMIDI before loading a SoundFont!", L"KDMAPI ERROR", MB_OK | MB_ICONERROR | MB_SYSTEMMODAL);
		return FALSE;
	}
	
	return FontLoader(Directory);
}

DWORD64 KDMAPI timeGetTime64() {
	ULONGLONG CurrentTime;
	NtQuerySystemTime(&CurrentTime);
	return (((CurrentTime) - TickStart) * (SpeedHack / 10000.0));
}