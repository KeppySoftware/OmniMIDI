/*
OmniMIDI, a fork of BASSMIDI Driver

Thank you Kode54 for allowing me to fork your awesome driver.
*/
#pragma once

#define DriverSettingsCase(Setting, Mode, Type, SettingStruct, Value, cbValue) case Setting: if (cbValue != sizeof(Type)) return FALSE; if (Mode = OM_SET) SettingStruct = *(Type*)Value; else if (Mode = OM_GET) *(Type*)Value = SettingStruct; else return FALSE; break;

// F**k WinMM and Microsoft
typedef VOID(CALLBACK*WMMC)(HMIDIOUT, DWORD, DWORD_PTR, DWORD_PTR, DWORD_PTR);
static DWORD_PTR WMMCI;
static WMMC CustomCallback = 0;

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

extern "C" BOOL KDMAPI ReturnKDMAPIVer(LPDWORD Major, LPDWORD Minor, LPDWORD Build, LPDWORD Revision) {
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

extern "C" BOOL KDMAPI IsKDMAPIAvailable()  {
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

extern "C" BOOL KDMAPI InitializeKDMAPIStream() {
	if (!AlreadyInitializedViaKDMAPI && !bass_initialized) {
		PrintMessageToDebugLog("KDMAPI_IKS", "The app requested the driver to initialize its audio stream.");

		// The client manually called a KDMAPI init call, KDMAPI is available no matter what
		AlreadyInitializedViaKDMAPI = TRUE;
		KDMAPIEnabled = TRUE;

		// PrintMessageToDebugLog("KDMAPI_IKS", "Initializing OmniMIDICrashHandler...");
		// AddVectoredExceptionHandler(0, OmniMIDICrashHandler);

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

extern "C" BOOL KDMAPI TerminateKDMAPIStream() {
	// If the driver is already initialized, close it
	if (AlreadyInitializedViaKDMAPI && bass_initialized) {
		PrintMessageToDebugLog("KDMAPI_TKS", "The app requested the driver to terminate its audio stream.");
		DoStopClient();
		AlreadyInitializedViaKDMAPI = FALSE;
		KDMAPIEnabled = FALSE;
		// PrintMessageToDebugLog("KDMAPI_IKS", "Removing OmniMIDICrashHandler...");
		// RemoveVectoredExceptionHandler(OmniMIDICrashHandler);
		PrintMessageToDebugLog("KDMAPI_TKS", "KDMAPI is now in sleep mode.");

		return TRUE;
	}
	else if (!AlreadyInitializedViaKDMAPI && bass_initialized) 
		PrintMessageToDebugLog("KDMAPI_TKS", "You cannot call TerminateKDMAPIStream if OmniMIDI has been initialized through WinMM.");
	else 
		PrintMessageToDebugLog("KDMAPI_TKS", "TerminateKDMAPIStream called, even though the driver is already sleeping.");

	return FALSE;
}

extern "C" VOID KDMAPI ResetKDMAPIStream() {
	// Redundant
	if (bass_initialized) ResetSynth(FALSE);
}

extern "C" BOOL KDMAPI SendCustomEvent(DWORD eventtype, DWORD chan, DWORD param) {
	return BASS_MIDI_StreamEvent(OMStream, chan, eventtype, param);
}

extern "C" MMRESULT KDMAPI SendDirectData(DWORD dwMsg) {
	// Send it to the pointed ParseData function (Either ParseData or ParseDataHyper)
	return _PrsData(MODM_DATA, dwMsg);
}

extern "C" MMRESULT KDMAPI SendDirectDataNoBuf(DWORD dwMsg) {
	// Send the data directly to BASSMIDI, bypassing the buffer altogether
	_StoBASSMIDI(0, dwMsg);
	return MMSYSERR_NOERROR;
}

extern "C" MMRESULT KDMAPI PrepareLongData(MIDIHDR* IIMidiHdr) {
	if (!IIMidiHdr) {
		PrintMessageToDebugLog("PrepareLongData", "The buffer doesn't exist, or hasn't been allocated.");
		return DebugResult(MMSYSERR_INVALPARAM, "The buffer doesn't exist, or hasn't been allocated.");		// Buffer doesn't exist
	}
	if (IIMidiHdr->dwBufferLength > LONGMSG_MAXSIZE) {
		PrintMessageToDebugLog("PrepareLongData", "The given stream buffer is greater than 64K.");
		return DebugResult(MMSYSERR_INVALPARAM, "The given stream buffer is greater than 64K.");			// Buffer is bigger than 64K
	}
	if (IIMidiHdr->dwFlags & MHDR_PREPARED) {
		PrintMessageToDebugLog("PrepareLongData", "The buffer is already prepared.");
		return MMSYSERR_NOERROR;																			// Already prepared, everything is fine
	}

	PrintMessageToDebugLog("PrepareLongData", "Locking buffer...");
	// Lock the MIDIHDR buffer, to prevent the MIDI app from accidentally writing to it
	if (!VirtualLock(IIMidiHdr->lpData, sizeof(IIMidiHdr->lpData)))
	{
		PrintMessageToDebugLog("PrepareLongData", "VirtualLock failed to lock the buffer!");
		return DebugResult(MMSYSERR_NOMEM, "VirtualLock failed to lock the buffer!");
	}
	PrintMessageToDebugLog("PrepareLongData", "Buffer is locked.");

	// Mark the buffer as prepared, and say that everything is oki-doki
	PrintMessageToDebugLog("PrepareLongData", "Marking as prepared...");
	IIMidiHdr->dwFlags |= MHDR_PREPARED;

	PrintMessageToDebugLog("PrepareLongData", "Function succeded.");
	return MMSYSERR_NOERROR;
}

extern "C" MMRESULT KDMAPI UnprepareLongData(MIDIHDR* IIMidiHdr) {
	// Check if the MIDIHDR buffer is valid
	if (!IIMidiHdr) {
		PrintMessageToDebugLog("UnprepareLongData", "The buffer doesn't exist, or hasn't been allocated.");
		return DebugResult(MMSYSERR_INVALPARAM, "The buffer doesn't exist, or hasn't been allocated.");		// The buffer doesn't exist, invalid parameter
	}
	if (!(IIMidiHdr->dwFlags & MHDR_PREPARED)) {
		PrintMessageToDebugLog("UnprepareLongData", "The buffer is already unprepared.");
		return MMSYSERR_NOERROR;																			// Already unprepared, everything is fine
	}
	if (IIMidiHdr->dwFlags & MHDR_INQUEUE) {
		PrintMessageToDebugLog("UnprepareLongData", "The buffer is still in queue.");
		return DebugResult(MIDIERR_STILLPLAYING, "The buffer is still in queue.");							// The buffer is currently being played from the driver, cannot unprepare
	}

	PrintMessageToDebugLog("UnprepareLongData", "Unlocking buffer...");
	// Unlock the buffer, and say that everything is oki-doki
	if (!VirtualUnlock(IIMidiHdr->lpData, sizeof(IIMidiHdr->lpData)))
	{
		// The buffer isn't locked
		PrintMessageToDebugLog("UnprepareLongData", "The buffer is still already unlocked.");
	}

	PrintMessageToDebugLog("UnprepareLongData", "Marking as unprepared...");
	IIMidiHdr->dwFlags &= ~MHDR_PREPARED;																	// Mark the buffer as unprepared

	PrintMessageToDebugLog("UnprepareLongData", "Function succeded.");
	return MMSYSERR_NOERROR;
}

extern "C" MMRESULT KDMAPI SendDirectLongData(MIDIHDR* IIMidiHdr) {
	if (!bass_initialized) return DebugResult(MIDIERR_NOTREADY, "BASS hasn't been initialized yet");					// The driver isn't ready
	if (!IIMidiHdr) return DebugResult(MMSYSERR_INVALPARAM, "The buffer doesn't exist, or hasn't been allocated.");		// The buffer doesn't exist, invalid parameter
	if (!(IIMidiHdr->dwFlags & MHDR_PREPARED)) return DebugResult(MIDIERR_UNPREPARED, "The buffer is not prepared");	// The buffer is not prepared
	
	// Mark the buffer as in queue
	IIMidiHdr->dwFlags &= ~MHDR_DONE;
	IIMidiHdr->dwFlags |= MHDR_INQUEUE;

	// Do the stuff with it
	SendLongToBASSMIDI(IIMidiHdr);

	// Mark the buffer as done
	IIMidiHdr->dwFlags &= ~MHDR_INQUEUE;
	IIMidiHdr->dwFlags |= MHDR_DONE;

	// Tell the app that the buffer has been played
	return MMSYSERR_NOERROR;
}

extern "C" MMRESULT KDMAPI SendDirectLongDataNoBuf(MIDIHDR* IIMidiHdr) {
	PrintMessageToDebugLog("KDMAPI_SDLDNBuf", "Deprecated command, please use SendDirectLongData instead.");
	return SendDirectLongData(IIMidiHdr);
}

extern "C" BOOL KDMAPI DriverSettings(DWORD Setting, DWORD Mode, LPVOID Value, UINT cbValue) {
	BOOL DontMissNotesTemp = ManagedSettings.DontMissNotes;

	switch (Setting) {
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
		DriverSettingsCase(OM_MT32MODE, Mode, BOOL, ManagedSettings.MT32Mode, Value, cbValue);
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

	default:
		MessageBox(NULL, L"Unknown setting passed to DriverSettings.", L"OmniMIDI - KDMAPI ERROR", MB_OK | MB_ICONERROR | MB_SYSTEMMODAL);
		return FALSE;

	}

	if (Mode == OM_SET) {
		PrintMessageToDebugLog("KDMAPI_DS", "Applying new settings to the driver...");

		// The new value is different from the temporary one, reset the synth
		// to avoid stuck notes or crashes
		if (DontMissNotesTemp != ManagedSettings.DontMissNotes) ResetSynth(TRUE);

		// Stuff lol
		if (!Between(ManagedSettings.MinVelIgnore, 1, 127)) ManagedSettings.MinVelIgnore = 1;
		if (!Between(ManagedSettings.MaxVelIgnore, 1, 127)) ManagedSettings.MaxVelIgnore = 1;

		// Parse the new volume value, and set it
		SynthVolume = (float)ManagedSettings.OutputVolume / 10000.0f;
		ChVolumeStruct.fCurrent = 1.0f;
		ChVolumeStruct.fTarget = SynthVolume;
		ChVolumeStruct.fTime = 0.0f;
		ChVolumeStruct.lCurve = 0;

		if (AlreadyInitializedViaKDMAPI) {
			BASS_FXSetParameters(ChVolume, &ChVolumeStruct);
			CheckUp(FALSE, ERRORCODE, L"Stream Volume FX Set", FALSE);

			// Set the rendering time threshold, if the driver's own panic system is disabled
			BASS_ChannelSetAttribute(OMStream, BASS_ATTRIB_MIDI_CPU, ManagedSettings.MaxRenderingTime);

			// Set the stream's attributes
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

		PrintMessageToDebugLog("KDMAPI_DS", "Done.");
	}
	return TRUE;
}

extern "C" DebugInfo* KDMAPI GetDriverDebugInfo() {
	// Parse the debug info, and return them to the app.
	PrintMessageToDebugLog("KDMAPI_GDDI", "Passed pointer to DebugInfo to the KDMAPI-ready application.");
	return &ManagedDebugInfo;
}

extern "C" BOOL KDMAPI LoadCustomSoundFontsList(LPWSTR Directory) {
	// Load the SoundFont from the specified path (It can be a sf2/sfz or a sflist)
	if (!AlreadyInitializedViaKDMAPI) {
		MessageBox(NULL, L"Initialize OmniMIDI before loading a SoundFont!", L"KDMAPI ERROR", MB_OK | MB_ICONERROR | MB_SYSTEMMODAL);
		return FALSE;
	}
	else return FontLoader(Directory);
}

extern "C" DWORD64 KDMAPI timeGetTime64() {
	static LARGE_INTEGER frequency = { {0,0} };
	LARGE_INTEGER startingTime;

	if (frequency.QuadPart == 0)
		QueryPerformanceFrequency(&frequency);

	QueryPerformanceCounter(&startingTime);
	return (1000 * (startingTime.QuadPart % frequency.QuadPart) / frequency.QuadPart) + (1000 * (startingTime.QuadPart / frequency.QuadPart));
}