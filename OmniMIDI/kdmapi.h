// KDMAPI calls

BOOL StreamHealthCheck(BOOL& Initialized) {
	// If BASS is forbidden from initializing itself, then abort immediately
	if (block_bassinit) return FALSE;

	// Dummy call
	BASS_ChannelIsActive(OMStream);

	// Check if the call failed
	if ((BASS_ErrorGetCode() == 5 || ManagedSettings.LiveChanges)) {
		SetConsoleTextAttribute(hConsole, FOREGROUND_RED);
		PrintToConsole(FOREGROUND_RED, 1, "Restarting audio stream...");

		// It did, reload the settings and reallocate the memory for the buffer
		CloseThreads(FALSE);
		LoadSettings();
		AllocateMemory(TRUE);

		// Initialize the BASS output device, and set up the streams
		if (InitializeBASS(TRUE)) {
			SetUpStream();
			LoadSoundFontsToStream();

			// Done, now initialize the threads
			Initialized = CreateThreads(TRUE);
		}
		return FALSE;
	}
	else {
		if (stop_thread || ATThread.ThreadHandle == NULL) CreateThreads(FALSE);
	}

	return TRUE;
}

DWORD WINAPI StreamHealthAndSettings(LPVOID lpV) {
	try {
		// Check system
		PrintToConsole(FOREGROUND_RED, 1, "Checking for settings changes or hotkeys...");

		while (!stop_thread) {
			// Start the timer, which calculates 
			// how much time it takes to do its stuff
			if (!HyperMode) start1 = TimeNow();

			// Do registry stuff

			LoadSettingsRT();			// Load real-time settings
			LoadCustomInstruments();	// Load custom instrument values from the registry
			keybindings();				// Check for keystrokes (ALT+1, INS, etc..)
			WatchdogCheck();			// Check current active voices, rendering time, etc..
			mixervoid();				// Send dB values to the mixer
			RevbNChor();				// Check if custom reverb/chorus values are enabled


			// Check if the threads and streams are still alive
			StreamHealthCheck(bass_initialized);

			// Check the current output volume
			CheckVolume(FALSE);

			// I SLEEP
			Sleep(10);
		}

		// Release the SoundFonts and the stream
		FreeFonts();
		FreeUpStream();
	}
	catch (...) {
		CrashMessage(L"SettingsAndHealthThread");
	}

	// Close the thread
	PrintToConsole(FOREGROUND_RED, 1, "Closing health thread...");
	CloseHandle(HealthThread.ThreadHandle);
	HealthThread.ThreadHandle = NULL;
	return 0;
}

void DoStartClient() {
	if (modm_closed == TRUE && BannedSystemProcess() != TRUE) {
		// Load the selected driver priority value from the registry
		OpenRegistryKey(MainKey, L"Software\\OmniMIDI");
		RegQueryValueEx(MainKey.Address, L"DriverPriority", NULL, &dwType, (LPBYTE)&ManagedSettings.DriverPriority, &dwSize);

		// Parse the app name, and start the debug pipe to the debug window
		AppName();
		if (!AlreadyStartedOnce) StartDebugPipe(FALSE);

		// Create an event, to load the default SoundFonts synchronously
		load_sfevent = CreateEvent(
			NULL,               // default security attributes
			TRUE,               // manual-reset event
			FALSE,              // initial state is nonsignaled
			TEXT("SoundFontEvent")  // object name
		);

		// Initialize the stream
		bass_initialized = FALSE;
		while (!bass_initialized) {
			// Load the settings, and allocate the memory for the EVBuffer
			LoadSettings();
			AllocateMemory(FALSE);

			// Load the BASS functions
			if (!BASSLoadedToMemory) BASSLoadedToMemory = load_bassfuncs();

			// Initialize the BASS output device, and set up the streams
			SetConsoleTextAttribute(hConsole, FOREGROUND_RED);
			if (InitializeBASS(FALSE)) {
				SetUpStream();
				LoadSoundFontsToStream();

				// Done, now initialize the threads
				bass_initialized = CreateThreads(TRUE);
			}
		}

		// Create the main thread
		CheckIfThreadClosed(HealthThread.ThreadHandle);
		HealthThread.ThreadHandle = CreateThread(NULL, 0, (LPTHREAD_START_ROUTINE)StreamHealthAndSettings, NULL, 0, (LPDWORD)HealthThread.ThreadAddress);
		SetThreadPriority(HealthThread.ThreadHandle, prioval[ManagedSettings.DriverPriority]);

		// Wait for the SoundFonts to load, then close the event's handle
		if (WaitForSingleObject(load_sfevent, INFINITE) == WAIT_OBJECT_0)
			CloseHandle(load_sfevent);

		// Ok, everything's ready, do not open more debug pipes from now on
		modm_closed = FALSE;
		AlreadyStartedOnce = TRUE;
	}
}

void DoStopClient() {
	if (modm_closed == FALSE) {
		// Prevent BASS from reinitializing itself
		block_bassinit = TRUE;

		// Close the threads and free up the allocated memory
		FreeFonts();
		FreeUpStream();
		CloseThreads(TRUE);
		FreeUpMemory();
		modm_closed = TRUE;

		// Close registry keys
		CloseRegistryKey(MainKey);
		CloseRegistryKey(Configuration);
		CloseRegistryKey(Channels);
		CloseRegistryKey(ChanOverride);
		CloseRegistryKey(Watchdog);

		// OK now it's fine
		block_bassinit = FALSE;
	}
}

void DoResetClient() {
	// The app requested a complete reset of the synth
	ResetSynth(0);
}

char const* WINAPI ReturnKDMAPIVer() {
	// Dummy value, set by Kep himself
	return "v1.30 (Release)";
}

BOOL WINAPI IsKDMAPIAvailable()  {
	// Parse the current state of the KDMAPI
	OpenRegistryKey(Configuration, L"Software\\OmniMIDI\\Configuration");

	long lResult = RegQueryValueEx(Configuration.Address, L"KDMAPIEnabled", NULL, &dwType, (LPBYTE)&KDMAPIEnabled, &dwSize);

	// If the state is not available or it hasn't been set, keep it enabled by default
	if (lResult != ERROR_SUCCESS) 
		KDMAPIEnabled = TRUE;

	// Return the state
	return KDMAPIEnabled;
}

void InitializeKDMAPIStream() {
	// The client manually called a KDMAPI init call, KDMAPI is available no matter what
	KDMAPIEnabled = TRUE;

	// Start the driver's engine
	DoStartClient();
	DoResetClient();
}

void TerminateKDMAPIStream() {
	// If the driver is supposed to terminate the stream, then do so
	if (CloseStreamMidiOutClose) 
		DoStopClient();
}

void ResetKDMAPIStream() {
	// Redundant
	DoResetClient();
}

MMRESULT WINAPI SendDirectData(DWORD dwMsg) {
	// Send it to the pointed ParseData function (Either ParseData or ParseDataHyper)
	return _PrsData(MODM_DATA, dwMsg, 0);
}

MMRESULT WINAPI SendDirectDataNoBuf(DWORD dwMsg) {
	try {
		// Send the data directly to BASSMIDI, bypassing the buffer altogether
		if (EVBuffReady) SendToBASSMIDI(dwMsg);
		return MMSYSERR_NOERROR;
	}
	catch (...) {
		// Something died, invalid parameter!
		return MMSYSERR_INVALPARAM;
	}
}

MMRESULT WINAPI PrepareLongData(MIDIHDR* IIMidiHdr) {
	if (!IIMidiHdr || sizeof(IIMidiHdr->lpData) > LONGMSG_MAXSIZE) return MMSYSERR_INVALPARAM;			// The buffer doesn't exist or is too big, invalid parameter

	// Lock the MIDIHDR buffer, to prevent the MIDI app from accidentally writing to it
	if (!VirtualLock(IIMidiHdr->lpData, sizeof(IIMidiHdr->lpData)))
		return MMSYSERR_NOMEM;

	// Mark the buffer as prepared, and say that everything is oki-doki
	IIMidiHdr->dwFlags |= MHDR_PREPARED;
	return MMSYSERR_NOERROR;
}

MMRESULT WINAPI UnprepareLongData(MIDIHDR* IIMidiHdr) {
	// Check if the MIDIHDR buffer is valid
	if (!IIMidiHdr) return MMSYSERR_INVALPARAM;								// The buffer doesn't exist, invalid parameter
	if (!(IIMidiHdr->dwFlags & MHDR_PREPARED)) return MMSYSERR_NOERROR;		// Already unprepared, everything is fine
	if (IIMidiHdr->dwFlags & MHDR_INQUEUE) return MIDIERR_STILLPLAYING;		// The buffer is currently being played from the driver, cannot unprepare

	IIMidiHdr->dwFlags &= ~MHDR_PREPARED;									// Mark the buffer as unprepared

	// Unlock the buffer, and say that everything is oki-doki
	VirtualUnlock(IIMidiHdr->lpData, sizeof(IIMidiHdr->lpData));
	return MMSYSERR_NOERROR;
}

MMRESULT WINAPI SendDirectLongData(MIDIHDR* IIMidiHdr) {
	try {
		if (EVBuffReady) {
			if (!(IIMidiHdr->dwFlags & MHDR_PREPARED)) return MIDIERR_UNPREPARED;

			// Mark the buffer as in queue
			IIMidiHdr->dwFlags &= ~MHDR_DONE;
			IIMidiHdr->dwFlags |= MHDR_INQUEUE;

			// Do the stuff with it, if it's not to be ignored
			if (!ManagedSettings.IgnoreSysEx) SendLongToBASSMIDI(IIMidiHdr->lpData, IIMidiHdr->dwBytesRecorded);
			// It has to be ignored, send info to console
			else PrintToConsole(FOREGROUND_RED, (DWORD)IIMidiHdr->lpData, "Ignored SysEx MIDI event.");

			// Mark the buffer as done
			IIMidiHdr->dwFlags &= ~MHDR_INQUEUE;
			IIMidiHdr->dwFlags |= MHDR_DONE;

			// Tell the app that the buffer has been played
			return MMSYSERR_NOERROR;
		}
		else return MIDIERR_NOTREADY;
	}
	catch (...) { return MMSYSERR_INVALPARAM; }
}

MMRESULT WINAPI SendDirectLongDataNoBuf(MIDIHDR* IIMidiHdr) {
	return SendDirectLongData(IIMidiHdr);
}

VOID WINAPI ChangeDriverSettings(const Settings* Struct, DWORD StructSize){
	if (Struct == nullptr) {
		// The app returned an invalid pointer, or "nullptr" on purpose
		// Fallback to the registry
		SettingsManagedByClient = FALSE;
		return;
	}

	// Temp setting we need to keep
	BOOL DontMissNotesTemp = ManagedSettings.DontMissNotes;

	// Copy the struct from the app to the driver
	memcpy(&ManagedSettings, Struct, min(sizeof(Settings), StructSize));
	SettingsManagedByClient = TRUE;

	// The new value is different from the temporary one, reset the synth
	// to avoid stuck notes or crashes
	if (DontMissNotesTemp != ManagedSettings.DontMissNotes) {
		ResetSynth(1);
	}

	// Stuff lol
	if (!Between(ManagedSettings.MinVelIgnore, 1, 127)) { ManagedSettings.MinVelIgnore = 1; }
	if (!Between(ManagedSettings.MaxVelIgnore, 1, 127)) { ManagedSettings.MaxVelIgnore = 1; }

	// Parse the new volume value, and set it
	sound_out_volume_float = (float)ManagedSettings.OutputVolume / 10000.0f;
	ChVolumeStruct.fCurrent = 1.0f;
	ChVolumeStruct.fTarget = sound_out_volume_float;
	ChVolumeStruct.fTime = 0.0f;
	ChVolumeStruct.lCurve = 0;
	BASS_FXSetParameters(ChVolume, &ChVolumeStruct);
	CheckUp(ERRORCODE, L"Stream Volume FX Set", FALSE);

	// Set the rendering time threshold, if the driver's own panic system is disabled
	BASS_ChannelSetAttribute(OMStream, BASS_ATTRIB_MIDI_CPU, ManagedSettings.MaxRenderingTime);

	// Set the stream's settings
	BASS_ChannelFlags(OMStream, ManagedSettings.EnableSFX ? 0 : BASS_MIDI_NOFX, BASS_MIDI_NOFX);
	BASS_ChannelFlags(OMStream, ManagedSettings.NoteOff1 ? BASS_MIDI_NOTEOFF1 : 0, BASS_MIDI_NOTEOFF1);
	BASS_ChannelFlags(OMStream, ManagedSettings.IgnoreSysReset ? BASS_MIDI_NOSYSRESET : 0, BASS_MIDI_NOSYSRESET);
	BASS_ChannelFlags(OMStream, ManagedSettings.SincInter ? BASS_MIDI_SINCINTER : 0, BASS_MIDI_SINCINTER);

	// Set the stream's attributes
	BASS_ChannelSetAttribute(OMStream, BASS_ATTRIB_SRC, ManagedSettings.SincConv);
	BASS_ChannelSetAttribute(OMStream, BASS_ATTRIB_MIDI_KILL, ManagedSettings.DisableNotesFadeOut);
}

VOID WINAPI LoadCustomSoundFontsList(const TCHAR* Directory) {
	// Load the SoundFont from the specified path (It can be a sf2/sfz or a sflist)
	LoadFonts(Directory);
}

DebugInfo* WINAPI GetDriverDebugInfo() {
	// Parse the debug info, and return them to the app
	return &ManagedDebugInfo;
}