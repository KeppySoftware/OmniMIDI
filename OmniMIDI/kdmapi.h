// KDMAPI calls

void keepstreamsalive(int& opend) {
	BASS_ChannelIsActive(OMStream);
	if (BASS_ErrorGetCode() == 5 || ManagedSettings.LiveChanges) {
		PrintToConsole(FOREGROUND_RED, 1, "Restarting audio stream...");
		CloseThreads(FALSE);
		LoadSettings(TRUE);
		SetConsoleTextAttribute(hConsole, FOREGROUND_RED);
		if (InitializeBASS(FALSE)) {
			SetUpStream();
			LoadSoundFontsToStream();
			opend = CreateThreads(TRUE);
		}
	}
}

DWORD WINAPI DriverHeart(LPVOID lpV) {
	try {
		if (BannedSystemProcess() == TRUE) {
			ExitThread(0);
			return 0;
		}
		else {
			int opend = 0;
			while (opend == 0) {
				LoadSettings(FALSE);
				AllocateMemory();
				load_bassfuncs();
				SetConsoleTextAttribute(hConsole, FOREGROUND_RED);
				if (InitializeBASS(FALSE)) {
					SetUpStream();
					LoadSoundFontsToStream();
					opend = CreateThreads(TRUE);
				}
			}
			PrintToConsole(FOREGROUND_RED, 1, "Checking for settings changes or hotkeys...");
			while (stop_rtthread == FALSE) {
				start1 = TimeNow();
				keepstreamsalive(opend);
				LoadCustomInstruments();
				CheckVolume(FALSE);
				ParseDebugData();
				Sleep(10);
			}
			FreeFonts();
			FreeUpStream();
			PrintToConsole(FOREGROUND_RED, 1, "Closing main thread...");
			CloseHandle(MainThread);
			MainThread = NULL;
			return 0;
		}
	}
	catch (...) {
		CrashMessage(L"DriverHeartThread");
		throw;
	}
}

void DoStartClient() {
	if (modm_closed == TRUE) {
		timeBeginPeriod(1);

		HKEY hKey;
		long lResult;
		DWORD dwType = REG_DWORD;
		DWORD dwSize = sizeof(DWORD);
		int One = 0;
		lResult = RegOpenKeyEx(HKEY_CURRENT_USER, L"Software\\OmniMIDI", 0, KEY_ALL_ACCESS, &hKey);
		RegQueryValueEx(hKey, L"DriverPriority", NULL, &dwType, (LPBYTE)&ManagedSettings.DriverPriority, &dwSize);
		RegCloseKey(hKey);
		lResult = RegOpenKeyEx(HKEY_CURRENT_USER, L"Software\\OmniMIDI\\Configuration", 0, KEY_ALL_ACCESS, &hKey);
		RegQueryValueEx(hKey, L"HyperPlayback", NULL, &dwType, (LPBYTE)&HyperMode, &dwSize);
		RegCloseKey(hKey);

		if (HyperMode && !HyperCheckedAlready) {
			MessageBox(NULL, L"Hyper-playback mode is enabled!", L"OmniMIDI", MB_ICONWARNING | MB_OK | MB_SYSTEMMODAL);
			_PrsData = ParseDataHyper;
			_PlayBufData = PlayBufferedDataHyper;
			_PlayBufDataChk = PlayBufferedDataChunkHyper;
		}
		else {
			_PrsData = ParseData;
			_PlayBufData = PlayBufferedData;
			_PlayBufDataChk = PlayBufferedDataChunk;
		}
		HyperCheckedAlready = TRUE;

		AppName();
		if (!AlreadyStartedOnce) StartDebugPipe(FALSE);

		processPriority = GetPriorityClass(GetCurrentProcess());
		SetPriorityClass(GetCurrentProcess(), NORMAL_PRIORITY_CLASS);
		load_sfevent = CreateEvent(
			NULL,               // default security attributes
			TRUE,               // manual-reset event
			FALSE,              // initial state is nonsignaled
			TEXT("SoundFontEvent")  // object name
		);
		MainThread = CreateThread(NULL, 0, (LPTHREAD_START_ROUTINE)DriverHeart, NULL, 0, (LPDWORD)MainThreadAddress);
		SetThreadPriority(MainThread, prioval[ManagedSettings.DriverPriority]);

		if (WaitForSingleObject(load_sfevent, INFINITE) == WAIT_OBJECT_0)
			CloseHandle(load_sfevent);

		modm_closed = FALSE;
		AlreadyStartedOnce = TRUE;
	}
}

void DoStopClient() {
	if (modm_closed == FALSE) {
		CloseThreads(TRUE);
		FreeUpMemory();
		modm_closed = TRUE;
		SetPriorityClass(GetCurrentProcess(), processPriority);
		timeEndPeriod(1);
	}
}

void DoResetClient() {
	ResetSynth(0);
}

char const* WINAPI ReturnKDMAPIVer() {
	return "v1.20 (Release)";
}

BOOL WINAPI IsKDMAPIAvailable()  {
	HKEY hKey;
	DWORD dwType = REG_DWORD;
	DWORD dwSize = sizeof(DWORD);
	RegOpenKeyEx(HKEY_CURRENT_USER, L"Software\\OmniMIDI\\Configuration", 0, KEY_READ, &hKey);
	long lResult = RegQueryValueEx(hKey, L"KDMAPIEnabled", NULL, &dwType, (LPBYTE)&KDMAPIEnabled, &dwSize);
	RegCloseKey(hKey);

	if (lResult != ERROR_SUCCESS) 
		KDMAPIEnabled = TRUE;

	return KDMAPIEnabled;
}

void InitializeKDMAPIStream() {
	KDMAPIEnabled = TRUE;
	DoStartClient();
}

void TerminateKDMAPIStream() {
	if (CloseStreamMidiOutClose) DoStopClient();
}

void ResetKDMAPIStream() {
	ResetSynth(0);
}

MMRESULT WINAPI SendDirectData(DWORD dwMsg) {
	return _PrsData(MODM_DATA, dwMsg, 0);
}

MMRESULT WINAPI SendDirectDataNoBuf(DWORD dwMsg) {
	try {
		if (EVBuffReady) SendToBASSMIDI(dwMsg);
		return MMSYSERR_NOERROR;
	}
	catch (...) { return MMSYSERR_INVALPARAM; }
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
		SettingsManagedByClient = FALSE;
		if (RTSThread == NULL) {
			RTSThread = CreateThread(NULL, 0, (LPTHREAD_START_ROUTINE)RTSettings, NULL, 0, (LPDWORD)RTSThreadAddress);
			SetThreadPriority(RTSThread, prioval[ManagedSettings.DriverPriority]);
		}
		return;
	}

	BOOL DontMissNotesTemp = ManagedSettings.DontMissNotes;

	// Copy the struct from the app to the driver
	memcpy(&ManagedSettings, Struct, min(sizeof(Settings), StructSize));
	SettingsManagedByClient = TRUE;

	if (DontMissNotesTemp != ManagedSettings.DontMissNotes) {
		ResetSynth(1);
	}
	if (!Between(ManagedSettings.MinVelIgnore, 1, 127)) { ManagedSettings.MinVelIgnore = 1; }
	if (!Between(ManagedSettings.MaxVelIgnore, 1, 127)) { ManagedSettings.MaxVelIgnore = 1; }

	sound_out_volume_float = (float)ManagedSettings.OutputVolume / 10000.0f;
	ChVolumeStruct.fCurrent = 1.0f;
	ChVolumeStruct.fTarget = sound_out_volume_float;
	ChVolumeStruct.fTime = 0.0f;
	ChVolumeStruct.lCurve = 0;
	BASS_FXSetParameters(ChVolume, &ChVolumeStruct);
	CheckUp(ERRORCODE, L"KSVolFXSet", FALSE);

	if (ManagedSettings.AlternativeCPU != 1) BASS_ChannelSetAttribute(OMStream, BASS_ATTRIB_MIDI_CPU, ManagedSettings.MaxRenderingTime);

	BASS_ChannelFlags(OMStream, ManagedSettings.EnableSFX ? 0 : BASS_MIDI_NOFX, BASS_MIDI_NOFX);
	BASS_ChannelFlags(OMStream, ManagedSettings.NoteOff1 ? BASS_MIDI_NOTEOFF1 : 0, BASS_MIDI_NOTEOFF1);
	BASS_ChannelFlags(OMStream, ManagedSettings.IgnoreSysReset ? BASS_MIDI_NOSYSRESET : 0, BASS_MIDI_NOSYSRESET);
	BASS_ChannelFlags(OMStream, ManagedSettings.SincInter ? BASS_MIDI_SINCINTER : 0, BASS_MIDI_SINCINTER);

	BASS_ChannelSetAttribute(OMStream, BASS_ATTRIB_SRC, ManagedSettings.SincConv);
	BASS_ChannelSetAttribute(OMStream, BASS_ATTRIB_MIDI_KILL, ManagedSettings.DisableNotesFadeOut);
}

VOID WINAPI LoadCustomSoundFontsList(const TCHAR* Directory) {
	LoadFonts(Directory);
}

DebugInfo* WINAPI GetDriverDebugInfo() {
	return &ManagedDebugInfo;
}