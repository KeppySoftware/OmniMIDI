/*
OmniMIDI, a fork of BASSMIDI Driver

Thank you Kode54 for allowing me to fork your awesome driver.
*/
#pragma once

// KDMAPI calls
BOOL StreamHealthCheck(BOOL& Initialized) {
	// If BASS is forbidden from initializing itself, then abort immediately
	if (block_bassinit) return FALSE;

	// Check if the call failed
	if ((BASS_ChannelIsActive(OMStream) == BASS_ACTIVE_STOPPED || ManagedSettings.LiveChanges)) {
		PrintMessageToDebugLog("StreamWatchdog", "Stream is down! Restarting audio stream...");

		// It did, reload the settings and reallocate the memory for the buffer
		CloseThreads(FALSE);
		LoadSettings(TRUE);

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

DWORD WINAPI Watchdog(LPVOID lpV) {
	try {
		// Check system
		PrintMessageToDebugLog("StreamWatchdog", "Checking for settings changes or hotkeys...");

		while (!stop_thread) {
			// Start the timer, which calculates 
			// how much time it takes to do its stuff
			if (!HyperMode) start1 = TimeNow();

			// Check if the threads and streams are still alive
			if (StreamHealthCheck(bass_initialized));
			{
				// It's alive, do registry stuff

				LoadSettingsRT();			// Load real-time settings
				LoadCustomInstruments();	// Load custom instrument values from the registry
				keybindings();				// Check for keystrokes (ALT+1, INS, etc..)
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
		CrashMessage("SettingsAndHealthThread");
	}

	// Close the thread
	PrintMessageToDebugLog("StreamWatchdog", "Closing health thread...");
	CloseHandle(HealthThread.ThreadHandle);
	HealthThread.ThreadHandle = NULL;
	return 0;
}

BOOL DoStartClient() {
	if (!DriverInitStatus) {
		GetAppName();

		PrintMessageToDebugLog("StartDriver", "Initializing driver...");

		// Load the selected driver priority value from the registry
		OpenRegistryKey(MainKey, L"Software\\OmniMIDI", TRUE);
		RegQueryValueEx(MainKey.Address, L"DriverPriority", NULL, &dwType, (LPBYTE)&ManagedSettings.DriverPriority, &dwSize);

		// Parse the app name, and start the debug pipe to the debug window
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
			// Load the BASS functions
			LoadBASSFunctions();

			// If BASS is still unavailable, commit suicide
			if (!BASSLoadedToMemory) CrashMessage("NoBASSFound");

			// Load the settings, and allocate the memory for the EVBuffer
			LoadSettings(FALSE);

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
		CheckIfThreadClosed(HealthThread.ThreadHandle);
		HealthThread.ThreadHandle = CreateThread(NULL, 0, (LPTHREAD_START_ROUTINE)Watchdog, NULL, 0, (LPDWORD)HealthThread.ThreadAddress);
		SetThreadPriority(HealthThread.ThreadHandle, prioval[ManagedSettings.DriverPriority]);
		PrintMessageToDebugLog("StartDriver", "Done!");

		// Wait for the SoundFonts to load, then close the event's handle
		PrintMessageToDebugLog("StartDriver", "Waiting for the SoundFonts to load...");
		if (WaitForSingleObject(load_sfevent, INFINITE) == WAIT_OBJECT_0)
			CloseHandle(load_sfevent);

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
		PrintMessageToDebugLog("StopDriver", "Terminating driver...");

		// Prevent BASS from reinitializing itself
		block_bassinit = TRUE;

		// Close the threads and free up the allocated memory
		PrintMessageToDebugLog("StopDriver", "Freeing memory...");
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
		bass_initialized = FALSE;

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

BOOL KDMAPI IsKDMAPIAvailable()  {
	// Parse the current state of the KDMAPI
	OpenRegistryKey(Configuration, L"Software\\OmniMIDI\\Configuration", TRUE);

	PrintMessageToDebugLog("KDMAPI_IKA", "Interrogating registry about KDMAPI status...");
	long lResult = RegQueryValueEx(Configuration.Address, L"KDMAPIEnabled", NULL, &dwType, (LPBYTE)&KDMAPIEnabled, &dwSize);
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
	try {
		// If the driver is already initialized, close it
		if (AlreadyInitializedViaKDMAPI && bass_initialized) {
			PrintMessageToDebugLog("KDMAPI_TKS", "The app requested the driver to terminate its audio stream.");
			DoStopClient();
			AlreadyInitializedViaKDMAPI = FALSE;
			KDMAPIEnabled = FALSE;
			PrintMessageToDebugLog("KDMAPI_TKS", "KDMAPI is now in sleep mode.");
		}
		else PrintMessageToDebugLog("KDMAPI_TKS", "TerminateKDMAPIStream called, even though the driver is already sleeping.");

		return TRUE;
	}
	catch (...) {
		// Uh oh, the driver did a bad!
		PrintMessageToDebugLog("KDMAPI_TKS", "Unhandled exception while terminating the stream.");
		MessageBox(
			NULL,
			L"An error has occured in the TerminateKDMAPIStream() function.",
			L"KDMAPI ERROR",
			MB_OK | MB_ICONHAND | MB_SYSTEMMODAL);

		return FALSE;
	}
}

VOID KDMAPI ResetKDMAPIStream() {
	// Redundant
	if (bass_initialized) ResetSynth(FALSE);
}

VOID KDMAPI SendCustomEvent(DWORD eventtype, DWORD chan, DWORD param) {
	BASS_MIDI_StreamEvent(OMStream, chan, eventtype, param);
}

MMRESULT KDMAPI SendDirectData(DWORD dwMsg) {
	// Send it to the pointed ParseData function (Either ParseData or ParseDataHyper)
	return _PrsData(MODM_DATA, dwMsg, 0);
}

MMRESULT KDMAPI SendDirectDataNoBuf(DWORD dwMsg) {
	// Send the data directly to BASSMIDI, bypassing the buffer altogether
	if (EVBuffReady && AlreadyInitializedViaKDMAPI) {
		_StoBASSMIDI(0, dwMsg);
		return MMSYSERR_NOERROR;
	}
	return DebugResult(MIDIERR_NOTREADY, TRUE);
}

MMRESULT KDMAPI PrepareLongData(MIDIHDR* IIMidiHdr) {
	if (!IIMidiHdr ||
		!(IIMidiHdr->dwBytesRecorded <= IIMidiHdr->dwBufferLength) ||							// The buffer either doesn't exist, it's too big or
		sizeof(IIMidiHdr->lpData) >= LONGMSG_MAXSIZE) {
		PrintMessageToDebugLog("PrepareLongData", "The buffer is too big, or it's invalid.");
		return DebugResult(MMSYSERR_INVALPARAM, TRUE);											// the given size is invalid, invalid parameter
	}
	if (IIMidiHdr->dwFlags & MHDR_PREPARED) {
		PrintMessageToDebugLog("PrepareLongData", "The buffer is already prepared.");
		return MMSYSERR_NOERROR;																// Already prepared, everything is fine
	}

	PrintMessageToDebugLog("PrepareLongData", "Preparing to lock buffer...");
	VOID* m = IIMidiHdr->lpData;
	ULONG s = sizeof(IIMidiHdr->lpData);

	PrintMessageToDebugLog("PrepareLongData", "Locking...");
	// Lock the MIDIHDR buffer, to prevent the MIDI app from accidentally writing to it
	if (!NtLockVirtualMemory(GetCurrentProcess(), &m, &s, LOCK_VM_IN_WORKING_SET | LOCK_VM_IN_RAM))
	{
		PrintMessageToDebugLog("PrepareLongData", "NtLockVirtualMemory failed to lock the buffer!");
		return DebugResult(MMSYSERR_NOMEM, TRUE);
	}
	PrintMessageToDebugLog("PrepareLongData", "Buffer is locked.");

	// Mark the buffer as prepared, and say that everything is oki-doki
	PrintMessageToDebugLog("PrepareLongData", "Marking as prepared...");
	IIMidiHdr->dwFlags |= MHDR_PREPARED;

	PrintMessageToDebugLog("PrepareLongData", "Function succeded.");
	return MMSYSERR_NOERROR;
}

MMRESULT KDMAPI UnprepareLongData(MIDIHDR* IIMidiHdr) {
	// Check if the MIDIHDR buffer is valid
	if (!IIMidiHdr) {
		PrintMessageToDebugLog("UnprepareLongData", "The buffer passed to this function is invalid.");
		return DebugResult(MMSYSERR_INVALPARAM, TRUE);													// The buffer doesn't exist, invalid parameter
	}
	if (!(IIMidiHdr->dwFlags & MHDR_PREPARED)) {
		PrintMessageToDebugLog("UnprepareLongData", "The buffer is already unprepared.");
		return MMSYSERR_NOERROR;																		// Already unprepared, everything is fine
	}
	if (IIMidiHdr->dwFlags & MHDR_INQUEUE) {
		PrintMessageToDebugLog("UnprepareLongData", "The buffer is still in queue.");
		return DebugResult(MIDIERR_STILLPLAYING, TRUE);													// The buffer is currently being played from the driver, cannot unprepare
	}

	PrintMessageToDebugLog("UnprepareLongData", "Preparing to unlock buffer...");
	VOID* m = IIMidiHdr->lpData;
	ULONG s = sizeof(IIMidiHdr->lpData);

	PrintMessageToDebugLog("UnprepareLongData", "Unlocking...");
	// Unlock the buffer, and say that everything is oki-doki
	if (!NtUnlockVirtualMemory(GetCurrentProcess(), &m, &s, LOCK_VM_IN_WORKING_SET | LOCK_VM_IN_RAM))
	{
		PrintMessageToDebugLog("UnprepareLongData", "FATAL ERROR: Failed to unlock buffer! Access violation exception.");
		CrashMessage("UnlockMIDIHDR");
	}

	PrintMessageToDebugLog("UnprepareLongData", "Marking as unprepared...");
	IIMidiHdr->dwFlags &= ~MHDR_PREPARED;																// Mark the buffer as unprepared

	PrintMessageToDebugLog("UnprepareLongData", "Function succeded.");
	return MMSYSERR_NOERROR;
}

MMRESULT KDMAPI SendDirectLongData(MIDIHDR* IIMidiHdr) {
	if (!bass_initialized) return DebugResult(MIDIERR_NOTREADY, TRUE);							// The driver isn't ready
	if (!IIMidiHdr) return DebugResult(MMSYSERR_INVALPARAM, TRUE);								// The buffer doesn't exist, invalid parameter
	if (!(IIMidiHdr->dwFlags & MHDR_PREPARED)) return DebugResult(MIDIERR_UNPREPARED, TRUE);	// The buffer is not prepared
	
	// Mark the buffer as in queue
	IIMidiHdr->dwFlags &= ~MHDR_DONE;
	IIMidiHdr->dwFlags |= MHDR_INQUEUE;

	// Do the stuff with it, if it's not to be ignored
	if (!ManagedSettings.IgnoreSysEx) SendLongToBASSMIDI(IIMidiHdr);
	// It has to be ignored, send info to console
	else PrintMessageToDebugLog("KDMAPI_SDLD", "Ignored SysEx MIDI event...");

	// Mark the buffer as done
	IIMidiHdr->dwFlags &= ~MHDR_INQUEUE;
	IIMidiHdr->dwFlags |= MHDR_DONE;

	// Tell the app that the buffer has been played
	return MMSYSERR_NOERROR;
}

MMRESULT KDMAPI SendDirectLongDataNoBuf(MIDIHDR* IIMidiHdr) {
	PrintMessageToDebugLog("KDMAPI_SDLDNBuf", "Deprecated command, please use SendDirectLongData instead.");
	return SendDirectLongData(IIMidiHdr);
}

VOID KDMAPI GetCurrentDriverSettings(Settings* Struct) {
	if (!Struct) {
		// The app returned an invalid pointer, or "nullptr" on purpose
		// Fallback to the registry
		PrintMessageToDebugLog("KDMAPI_GCDS", "The app passed a nullptr. Invalid argument.");
		return;
	}

	PrintMessageToDebugLog("KDMAPI_GCDS", "Copying current settings to app's \"Settings\" struct...");
	memcpy(Struct, &ManagedSettings, min(sizeof(Settings), sizeof(ManagedSettings)));

	PrintMessageToDebugLog("KDMAPI_GCDS", "Done.");
}

VOID KDMAPI ChangeDriverSettings(Settings* Struct, DWORD StructSize){
	if (!Struct) {
		// The app returned an invalid pointer, or "nullptr" on purpose
		// Fallback to the registry
		PrintMessageToDebugLog("KDMAPI_CDS", "The app passed a nullptr. Fallback to registry enabled.");
		SettingsManagedByClient = FALSE;
		return;
	}
	else if (StructSize < sizeof(ManagedSettings)) {
		PrintMessageToDebugLog("KDMAPI_CDS", "The struct size is invalid.");
		return;
	}

	PrintMessageToDebugLog("KDMAPI_CDS", "The app passed a valid pointer. Disabled live settings from registry.");

	// Temp setting we need to keep
	BOOL DontMissNotesTemp = ManagedSettings.DontMissNotes;

	// Copy the struct from the app to the driver
	PrintMessageToDebugLog("KDMAPI_CDS", "Copying app's settings to driver's struct...");
	memcpy(&ManagedSettings, Struct, min(sizeof(Settings), StructSize));
	SettingsManagedByClient = TRUE;
	PrintMessageToDebugLog("KDMAPI_CDS", "Done, the settings are now managed by the app.");

	// The new value is different from the temporary one, reset the synth
	// to avoid stuck notes or crashes
	if (DontMissNotesTemp != ManagedSettings.DontMissNotes) {
		ResetSynth(TRUE);
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

	if (AlreadyInitializedViaKDMAPI) {
		PrintMessageToDebugLog("KDMAPI_CDS", "Applying new settings to the driver...");

		BASS_FXSetParameters(ChVolume, &ChVolumeStruct);
		CheckUp(FALSE, ERRORCODE, L"Stream Volume FX Set", FALSE);

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

		PrintMessageToDebugLog("KDMAPI_CDS", "Done!");
	}
}

VOID KDMAPI LoadCustomSoundFontsList(LPWSTR Directory) {
	// Load the SoundFont from the specified path (It can be a sf2/sfz or a sflist)
	if (!AlreadyInitializedViaKDMAPI) MessageBox(NULL, L"Initialize OmniMIDI before loading a SoundFont!", L"KDMAPI ERROR", MB_OK | MB_ICONERROR | MB_SYSTEMMODAL);
	else FontLoader(Directory);
}

DebugInfo* KDMAPI GetDriverDebugInfo() {
	// Parse the debug info, and return them to the app.
	PrintMessageToDebugLog("KDMAPI_GDDI", "Passed pointer to DebugInfo to the KDMAPI-ready application.");
	return &ManagedDebugInfo;
}