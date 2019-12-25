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
		PostThreadMessageW((DWORD)OMCallback, M, P1, P2);
		PrintMessageToDebugLog("DoCallback w/ CALLBACK_THREAD", R ? "The message has been sent to the thread handle." : "Failed to send message to thread handle.");
		break;
	}
	case CALLBACK_WINDOW:
	{
		PostMessageW((HWND)OMCallback, M, P1, P2);
		PrintMessageToDebugLog("DoCallback w/ CALLBACK_WINDOW", R ? "The message has been sent to the window handle." : "Failed to send message to window handle.");
		break;
	}
	default:
		PrintMessageToDebugLog("DoCallback w/ CALLBACK_NULL", "Function called, but no callback has been requested by the app.");
		break;
	}
}

// CookedPlayer system
VOID KillOldCookedPlayer(DWORD_PTR dwUser) {
	if (IsThisThreadActive(CookedThread.ThreadHandle)) {
		CookedPlayerHasToGo = TRUE;
		if (WaitForSingleObject(CookedThread.ThreadHandle, INFINITE) == WAIT_OBJECT_0) {
			CloseHandle(CookedThread.ThreadHandle);
			CookedThread.ThreadHandle = nullptr;
			CookedThread.ThreadAddress = NULL;
			CookedPlayerHasToGo = FALSE;
		}
	}
}

void CookedPlayerSystem(CookedPlayer* Player)
{
	QWORD ticker = 0;
	QWORD tickdiff = 0;
	int sleeptime = 0;
	int oldsleep = 0;
	int deltasleep = 0;

	DWORD delaytick = 0;
	BOOL barrier = TRUE;			// This is horrible :s

	const DWORD maxdelay = 10e4;	// Adjust responsiveness here
	const DWORD adaption = 1e5;		// Adaptive timer nice time >:3

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

// KDMAPI calls
BOOL StreamHealthCheck(BOOL & Initialized) {
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
			Initialized = CreateThreads(TRUE);
		}
		else PrintMessageToDebugLog("StreamWatchdog", "Failed to initialize stream! Retrying...");

		return FALSE;
	}
	else { 
		if (stop_thread || (!ATThread.ThreadHandle && ManagedSettings.CurrentEngine != ASIO_ENGINE)) CreateThreads(FALSE);
	}

	return TRUE;
}

void Supervisor(LPVOID lpV) {
	try {
		// Check system
		PrintMessageToDebugLog("StreamWatchdog", "Checking for settings changes or hotkeys...");

		while (!stop_thread) {
			// Check if the threads and streams are still alive
			if (StreamHealthCheck(bass_initialized));
			{
				// It's alive, do registry stuff

				LoadSettings(FALSE, TRUE);	// Load real-time settings
				LoadCustomInstruments();	// Load custom instrument values from the registry
				KeyShortcuts();				// Check for keystrokes (ALT+1, INS, etc..)
				SFDynamicLoaderCheck();		// Check current active voices, rendering time, etc..
				MixerCheck();				// Send dB values to the mixer
				RevbNChor();				// Check if custom reverb/chorus values are enabled

				// Check the current output volume
				CheckVolume(FALSE);
			}

			// I SLEEP
			Sleep(30);
		}
	}
	catch (...) {
		CrashMessage(L"SettingsAndHealthThread");
	}

	// Close the thread
	PrintMessageToDebugLog("StreamWatchdog", "Closing health thread...");
	TerminateThread(&HealthThread, TRUE, 0);
}

BOOL DoStartClient() {
	if (!DriverInitStatus) {
		PrintMessageToDebugLog("StartDriver", "Checking if app is allowed to use RTSS OSD...");
		GetAppName();
		CheckIfAppIsAllowedToUseOSD();

		PrintMessageToDebugLog("StartDriver", "Initializing driver...");

		// Load the selected driver priority value from the registry
		OpenRegistryKey(MainKey, L"Software\\OmniMIDI", TRUE);
		RegQueryValueEx(MainKey.Address, L"DriverPriority", NULL, &dwType, (LPBYTE)&ManagedSettings.DriverPriority, &dwSize);

		// Parse the app name, and start the debug pipe to the debug window
		if (!AlreadyStartedOnce) StartDebugPipe(FALSE);

		// Create an event, to load the default SoundFonts synchronously
		OMReady = CreateEvent(
			NULL,               // default security attributes
			TRUE,               // manual-reset event
			FALSE,              // initial state is nonsignaled
			TEXT("OMReady")		// object name
		);

		// Initialize the stream
		bass_initialized = FALSE;
		while (!bass_initialized) {
			// Load the BASS functions
			LoadBASSFunctions();

			// If BASS is still unavailable, commit suicide
			if (!BASSLoadedToMemory) CrashMessage(L"NoBASSFound");

			// Load the settings, and allocate the memory for the EVBuffer
			LoadSettings(FALSE, FALSE);

			// Initialize the BASS output device, and set up the streams
			if (InitializeBASS(FALSE)) {
				SetUpStream();
				LoadSoundFontsToStream();

				// Done, now initialize the threads
				bass_initialized = CreateThreads(TRUE);
			}
		}

		// Create the main thread
		PrintMessageToDebugLog("StartDriver", "Starting main watchdog thread...");
		CheckIfThreadClosed(&HealthThread);
		HealthThread.ThreadHandle = (HANDLE)_beginthreadex(NULL, 0, (_beginthreadex_proc_type)Supervisor, NULL, 0, &HealthThread.ThreadAddress);
		SetThreadPriority(HealthThread.ThreadHandle, THREAD_PRIORITY_NORMAL);
		PrintMessageToDebugLog("StartDriver", "Done!");

		// Wait for the SoundFonts to load, then close the event's handle
		PrintMessageToDebugLog("StartDriver", "Waiting for the SoundFonts to load...");
		if (WaitForSingleObject(OMReady, INFINITE) == WAIT_OBJECT_0)
			CloseHandle(OMReady);

		// Ok, everything's ready, do not open more debug pipes from now on
		DriverInitStatus = TRUE;
		AlreadyStartedOnce = TRUE;

		PrintMessageToDebugLog("StartDriver", "Driver initialized.");
		return TRUE;
	}

	return FALSE;
}

BOOL DoStopClient() {
	if (DriverInitStatus) {
		PrintMessageToDebugLog("StopDriver", "Releasing RTSS OSD...");
		ReleaseOSD();

		PrintMessageToDebugLog("StopDriver", "Terminating driver...");

		// Prevent BASS from reinitializing itself
		block_bassinit = TRUE;

		// Close the threads and free up the allocated memory
		PrintMessageToDebugLog("StopDriver", "Freeing memory...");
		bass_initialized = FALSE;
		FreeFonts();
		FreeUpStream();
		CloseThreads(TRUE);
		FreeUpMemory();

		// Close registry keys
		PrintMessageToDebugLog("StopDriver", "Closing registry keys...");
		CloseRegistryKey(MainKey);
		PrintMessageToDebugLog("StopDriver", "Closed MainKey...");
		CloseRegistryKey(Configuration);
		PrintMessageToDebugLog("StopDriver", "Closed Configuration...");
		CloseRegistryKey(Channels);
		PrintMessageToDebugLog("StopDriver", "Closed Channels...");
		CloseRegistryKey(ChanOverride);
		PrintMessageToDebugLog("StopDriver", "Closed ChanOverride...");
		CloseRegistryKey(SFDynamicLoader);
		PrintMessageToDebugLog("StopDriver", "Closed SFDynamicLoader...");

		// OK now it's fine
		PrintMessageToDebugLog("StopDriver", "Just a few more things...");
		block_bassinit = FALSE;

		// Unload BASS functions
		UnloadBASSFunctions();

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
		RegQueryValueEx(Configuration.Address, L"DebugMode", NULL, &dwType, (LPBYTE)& ManagedSettings.DebugMode, &dwSize);
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
		PrintMessageToDebugLog("KDMAPI_TKS", "The app requested the driver to terminate its audio stream.");
		DoStopClient();
		AlreadyInitializedViaKDMAPI = FALSE;
		KDMAPIEnabled = FALSE;
		DisableBuiltInHandler("KDMAPI_TKS");
		PrintMessageToDebugLog("KDMAPI_TKS", "KDMAPI is now in sleep mode.");

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

VOID KDMAPI InitializeCallbackFeatures(HMIDI OMHM, DWORD_PTR OMCB, DWORD_PTR OMI, DWORD_PTR OMU, DWORD OMCM) {
	// Copy values to memory
	BOOL NV = ((OMCM != NULL) && (!OMCB && !OMI));

	OMHMIDI = OMHM;
	OMCallback = NV ? NULL : OMCB;
	OMInstance = NV ? NULL : OMI;
	OMCallbackMode = NV ? NULL : OMCM;

	if (NV) PrintMessageToDebugLog("ICF", "The application requested the driver to use callbacks, but no callback address has been given.");

	if ((OMCallbackMode & MIDI_IO_COOKED)) {
		PrintMessageToDebugLog("ICF", "MIDI_IO_COOKED requested.");

		// Prepare registry for CookedPlayer
		OpenRegistryKey(Configuration, L"Software\\OmniMIDI\\Configuration", FALSE);
		RegQueryValueEx(Configuration.Address, L"DisableCookedPlayer", NULL, &dwType, (LPBYTE)&ManagedSettings.DisableCookedPlayer, &dwSize);

		if (ManagedSettings.DisableCookedPlayer) {
			PrintMessageToDebugLog("ICF", "CookedPlayer has been disabled in the configurator.");
			return;
		}

		// Prepare the CookedPlayer
		PrintMessageToDebugLog("ICF", "Preparing CookedPlayer struct...");

		*(CookedPlayer**)OMU = (CookedPlayer*)malloc(sizeof(CookedPlayer));
		memset(*(CookedPlayer**)OMU, 0, sizeof(**(CookedPlayer**)OMU));

		(*(CookedPlayer**)OMU)->Paused = TRUE;
		(*(CookedPlayer**)OMU)->Tempo = 500000;
		(*(CookedPlayer**)OMU)->TimeDiv = 384;
		(*(CookedPlayer**)OMU)->TempoMulti = (((*(CookedPlayer**)OMU)->Tempo * 10) / (*(CookedPlayer**)OMU)->TimeDiv);
		PrintStreamValueToDebugLog("ICF", "TempoMulti", (*(CookedPlayer**)OMU)->TempoMulti);

		PrintMessageToDebugLog("ICF", "CookedPlayer struct prepared.");

		// Create player thread
		PrintMessageToDebugLog("ICF", "Preparing thread for CookedPlayer...");
		CookedThread.ThreadHandle = (HANDLE)_beginthreadex(NULL, 0, (_beginthreadex_proc_type)CookedPlayerSystem, *(LPVOID*)OMU, 0, &CookedThread.ThreadAddress);

		PrintMessageToDebugLog("ICF", "Thread is running. The driver is now ready to receive MIDI headers for the CookedPlayer.");
	}		
}

VOID KDMAPI RunCallbackFunction(DWORD Msg, DWORD_PTR P1, DWORD_PTR P2) {
	DoCallback(Msg, P1, P2);
}

VOID KDMAPI ResetKDMAPIStream() {
	// Redundant
	if (bass_initialized) ResetSynth(FALSE);
}

BOOL KDMAPI SendCustomEvent(DWORD eventtype, DWORD chan, DWORD param) {
	return _BMSE(OMStream, chan, eventtype, param);
}

MMRESULT KDMAPI SendDirectData(DWORD dwMsg) {
	// Send it to the pointed ParseData function (Either ParseData or ParseDataHyper)
	return _PrsData(dwMsg);
}

MMRESULT KDMAPI SendDirectDataNoBuf(DWORD dwMsg) {
	// Send the data directly to BASSMIDI, bypassing the buffer altogether
	_PforBASSMIDI(0, dwMsg);
	return MMSYSERR_NOERROR;
}

MMRESULT KDMAPI PrepareLongData(MIDIHDR * IIMidiHdr) {
	// Check if the MIDIHDR buffer is valid
	if (!bass_initialized)								// The driver isn't ready
		return DebugResult("PrepareLongData", MIDIERR_NOTREADY, "BASS hasn't been initialized yet.");

	if (!IIMidiHdr)										// Buffer doesn't exist
		return DebugResult("PrepareLongData", MMSYSERR_INVALPARAM, "The buffer doesn't exist, or hasn't been allocated.");

	if (IIMidiHdr->dwBufferLength > LONGMSG_MAXSIZE)	// Buffer is bigger than 64K
		return DebugResult("PrepareLongData", MMSYSERR_INVALPARAM, "The given stream buffer is greater than 64K.");

	if (IIMidiHdr->dwFlags & MHDR_PREPARED)				// Already prepared, everything is fine
		return DebugResult("PrepareLongData", MMSYSERR_NOERROR, "The buffer is already prepared.");

	// Mark the buffer as prepared, and say that everything is oki-doki
	PrintMessageToDebugLog("PrepareLongData", "Marking as prepared...");
	IIMidiHdr->dwFlags |= MHDR_PREPARED;

	PrintMessageToDebugLog("PrepareLongData", "Function succeded.");
	return MMSYSERR_NOERROR;
}

MMRESULT KDMAPI UnprepareLongData(MIDIHDR * IIMidiHdr) {
	// Check if the MIDIHDR buffer is valid
	if (!bass_initialized)						// The driver isn't ready
		return DebugResult("UnprepareLongData", MIDIERR_NOTREADY, "BASS hasn't been initialized yet.");

	if (!IIMidiHdr)								// The buffer doesn't exist, invalid parameter
		return DebugResult("UnprepareLongData", MMSYSERR_INVALPARAM, "The buffer doesn't exist, or hasn't been allocated.");

	if (!(IIMidiHdr->dwFlags & MHDR_PREPARED))	// Already unprepared, everything is fine
		return DebugResult("UnprepareLongData", MMSYSERR_NOERROR, "The buffer is already unprepared.");

	if (IIMidiHdr->dwFlags & MHDR_INQUEUE)		// The buffer is currently being played from the driver, cannot unprepare
		return DebugResult("UnprepareLongData", MIDIERR_STILLPLAYING, "The buffer is still in queue.");

	// Mark the buffer as unprepared
	PrintMessageToDebugLog("UnprepareLongData", "Marking as unprepared...");
	IIMidiHdr->dwFlags &= ~MHDR_PREPARED;

	PrintMessageToDebugLog("UnprepareLongData", "Function succeded.");
	return MMSYSERR_NOERROR;
}

MMRESULT KDMAPI SendDirectLongData(MIDIHDR * IIMidiHdr) {
	// Check if the MIDIHDR buffer is valid and if the stream is alive
	if (!bass_initialized)						// The driver isn't ready
		return DebugResult("SendDirectLongData", MIDIERR_NOTREADY, "BASS hasn't been initialized yet.");

	if (!IIMidiHdr)								// The buffer doesn't exist, invalid parameter
		return DebugResult("SendDirectLongData", MMSYSERR_INVALPARAM, "The buffer doesn't exist, or hasn't been allocated.");

	if (!(IIMidiHdr->dwFlags & MHDR_PREPARED))	// The buffer is not prepared
		return DebugResult("SendDirectLongData", MIDIERR_UNPREPARED, "The buffer is not prepared");

	// Mark the buffer as in queue
	IIMidiHdr->dwFlags &= ~MHDR_DONE;
	IIMidiHdr->dwFlags |= MHDR_INQUEUE;

	// Do the stuff with it
	BOOL res = SendLongToBASSMIDI(IIMidiHdr);

	// Mark the buffer as done
	IIMidiHdr->dwFlags &= ~MHDR_INQUEUE;
	IIMidiHdr->dwFlags |= MHDR_DONE;

	// Tell the app that the buffer has failed to be played
	if (!res) {
		char Msg[NTFS_MAX_PATH] = { 0 };

		sprintf(Msg, "The long buffer (MIDIHDR) sent to OmniMIDI wasn't able to be recognized.\n\nUnrecognized sequence: ");

		for (int i = 0; i < IIMidiHdr->dwBytesRecorded; i++)
			sprintf(Msg + strlen(Msg), "%02X", (BYTE)(IIMidiHdr->lpData[i]));

		sprintf(Msg + strlen(Msg), "\n");

		return DebugResult("SendDirectLongData", MMSYSERR_INVALPARAM, Msg);
	}

	return MMSYSERR_NOERROR;
}

MMRESULT KDMAPI SendDirectLongDataNoBuf(MIDIHDR * IIMidiHdr) {
	PrintMessageToDebugLog("KDMAPI_SDLDNBuf", "Deprecated command, please use SendDirectLongData instead.");
	return SendDirectLongData(IIMidiHdr);
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
	return (CurrentTime - TickStart) * (1.0 / 10000.0);
}