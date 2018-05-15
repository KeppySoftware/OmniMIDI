// KSDAPI calls

void keepstreamsalive(int& opend) {
	BASS_ChannelIsActive(KSStream);
	if (BASS_ErrorGetCode() == 5 || livechange) {
		PrintToConsole(FOREGROUND_RED, 1, "Restarting audio stream...");
		CloseThreads();
		LoadSettings(TRUE);
		if (!com_initialized) { if (!FAILED(CoInitialize(NULL))) com_initialized = TRUE; }
		SetConsoleTextAttribute(hConsole, FOREGROUND_RED);
		if (InitializeBASS(FALSE)) {
			SetUpStream();
			LoadSoundFontsToStream();
			opend = CreateThreads(TRUE);
		}
		streaminitialized = TRUE;
	}
}

DWORD WINAPI threadfunc(LPVOID lpV) {
	try {
		if (BannedSystemProcess() == TRUE) {
			_endthread();
			return 0;
		}
		else {
			int opend = 0;
			while (opend == 0) {
				LoadSettings(FALSE);
				allocate_memory();
				load_bassfuncs();
				if (!com_initialized) {
					if (FAILED(CoInitialize(NULL))) continue;
					com_initialized = TRUE;
				}
				SetConsoleTextAttribute(hConsole, FOREGROUND_RED);
				if (InitializeBASS(FALSE)) {
					SetUpStream();
					LoadSoundFontsToStream();
					opend = CreateThreads(TRUE);
				}
				streaminitialized = TRUE;
			}
			PrintToConsole(FOREGROUND_RED, 1, "Checking for settings changes or hotkeys...");
			while (stop_rtthread == FALSE) {
				start1 = TimeNow();
				keepstreamsalive(opend);
				LoadCustomInstruments();
				CheckVolume();
				ParseDebugData();
				Sleep(10);
			}
			stop_rtthread = FALSE;
			FreeUpLibraries();
			PrintToConsole(FOREGROUND_RED, 1, "Closing main thread...");
			ExitThread(0);
			return 0;
		}
	}
	catch (...) {
		CrashMessage(L"DrvMainThread");
		ExitThread(0);
		throw;
		return 0;
	}
}

void DoCallback(int driverNum, int clientNum, DWORD msg, DWORD_PTR param1, DWORD_PTR param2) {
	struct Driver_Client *client = &drivers[driverNum].clients[clientNum];
	DriverCallback(client->callback, client->flags, drivers[driverNum].hdrvr, msg, client->instance, param1, param2);
}

void DoStartClient() {
	if (modm_closed == TRUE) {
		InitializeCriticalSection(&mim_section);

		HKEY hKey;
		long lResult;
		DWORD dwType = REG_DWORD;
		DWORD dwSize = sizeof(DWORD);
		int One = 0;
		lResult = RegOpenKeyEx(HKEY_CURRENT_USER, L"Software\\Keppy's Synthesizer", 0, KEY_ALL_ACCESS, &hKey);
		RegQueryValueEx(hKey, L"driverprio", NULL, &dwType, (LPBYTE)&driverprio, &dwSize);
		RegCloseKey(hKey);
		lResult = RegOpenKeyEx(HKEY_CURRENT_USER, L"Software\\Keppy's Synthesizer\\Settings", 0, KEY_ALL_ACCESS, &hKey);
		RegQueryValueEx(hKey, L"hypermode", NULL, &dwType, (LPBYTE)&HyperMode, &dwSize);
		RegCloseKey(hKey);

		if (HyperMode && !HyperCheckedAlready) {
			MessageBox(NULL, L"Hyper-playback mode is enabled!", L"Keppy's Synthesizer", MB_ICONWARNING | MB_OK | MB_SYSTEMMODAL);
			_PrsData = ParseDataHyper;
			_SndBASSMIDI = SendToBASSMIDIHyper;
			_SndLongBASSMIDI = SendLongToBASSMIDIHyper;
			_PlayBufData = PlayBufferedDataHyper;
			_PlayBufDataChk = PlayBufferedDataChunkHyper;
		}
		else {
			_PrsData = ParseData;
			_SndBASSMIDI = SendToBASSMIDI;
			_SndLongBASSMIDI = SendLongToBASSMIDI;
			_PlayBufData = PlayBufferedData;
			_PlayBufDataChk = PlayBufferedDataChunk;
		}
		HyperCheckedAlready = TRUE;

		AppName();
		StartDebugPipe(FALSE);

		DWORD result;
		processPriority = GetPriorityClass(GetCurrentProcess());
		SetPriorityClass(GetCurrentProcess(), NORMAL_PRIORITY_CLASS);
		load_sfevent = CreateEvent(
			NULL,               // default security attributes
			TRUE,               // manual-reset event
			FALSE,              // initial state is nonsignaled
			TEXT("SoundFontEvent")  // object name
		);
		hCalcThread = CreateThread(NULL, 0, (LPTHREAD_START_ROUTINE)threadfunc, NULL, 0, (LPDWORD)thrdaddrC);
		SetThreadPriority(hCalcThread, prioval[driverprio]);
		result = WaitForSingleObject(load_sfevent, INFINITE);
		if (result == WAIT_OBJECT_0)
		{
			CloseHandle(load_sfevent);
		}
		modm_closed = FALSE;
	}
}

void DoStopClient() {
	if (modm_closed == FALSE) {
		stop_thread = TRUE;
		stop_rtthread = TRUE;
		WaitForSingleObject(hCalcThread, INFINITE);
		CloseHandle(hCalcThread);
		modm_closed = TRUE;
		SetPriorityClass(GetCurrentProcess(), processPriority);
	}
	DeleteCriticalSection(&mim_section);
}

void DoResetClient() {
	reset_synth = 1;
	ResetSynth(0);
}

char const* WINAPI ReturnKSDAPIVer() {
	return "v1.10 (Release)";
}

BOOL WINAPI IsKSDAPIAvailable()  {
	HKEY hKey;
	long lResult;
	DWORD dwType = REG_DWORD;
	DWORD dwSize = sizeof(DWORD);
	lResult = RegOpenKeyEx(HKEY_CURRENT_USER, L"Software\\Keppy's Synthesizer\\Settings", 0, KEY_READ, &hKey);
	RegQueryValueEx(hKey, L"allowksdapi", NULL, &dwType, (LPBYTE)&ksdirectenabled, &dwSize);
	RegCloseKey(hKey);

	return ksdirectenabled;
}

void InitializeKSStream() {
	DoStartClient();
}

void TerminateKSStream() {
	DoStopClient();
}

void ResetKSStream() {
	ResetSynth(0);
}

MMRESULT WINAPI SendDirectData(DWORD dwMsg) {
	return _PrsData(MODM_DATA, dwMsg, NULL);
}

MMRESULT WINAPI SendDirectDataNoBuf(DWORD dwMsg) {
	try {
		if (streaminitialized) SendToBASSMIDI(dwMsg);
		return MMSYSERR_NOERROR;
	}
	catch (...) { return MMSYSERR_INVALPARAM; }
}

MMRESULT WINAPI SendDirectLongData(MIDIHDR* IIMidiHdr) {
	try {
		if (streaminitialized) {
			DWORD retval = MMSYSERR_NOERROR;
			if (!(IIMidiHdr->dwFlags & MHDR_PREPARED)) return MIDIERR_UNPREPARED;
			IIMidiHdr->dwFlags &= ~MHDR_DONE;
			IIMidiHdr->dwFlags |= MHDR_INQUEUE;
			if (!sysexignore) {
				void* data = malloc(IIMidiHdr->dwBytesRecorded);
				if (data) {
					memcpy(data, IIMidiHdr->lpData, IIMidiHdr->dwBytesRecorded);
					retval = _PrsData(MODM_LONGDATA, (DWORD_PTR)data, IIMidiHdr->dwBytesRecorded);
				}
				else retval = MMSYSERR_NOMEM;
			}
			else PrintToConsole(FOREGROUND_RED, (DWORD)IIMidiHdr->lpData, "Ignored SysEx MIDI event.");
			IIMidiHdr->dwFlags &= ~MHDR_INQUEUE;
			IIMidiHdr->dwFlags |= MHDR_DONE;
			return retval;
		}
		else return MMSYSERR_NOMEM;
	}
	catch (...) { return MMSYSERR_INVALPARAM; }
}

MMRESULT WINAPI SendDirectLongDataNoBuf(MIDIHDR* IIMidiHdr) {
	try {
		if (streaminitialized) {
			if (!(IIMidiHdr->dwFlags & MHDR_PREPARED)) return MIDIERR_UNPREPARED;
			IIMidiHdr->dwFlags &= ~MHDR_DONE;
			IIMidiHdr->dwFlags |= MHDR_INQUEUE;
			if (!sysexignore) _SndLongBASSMIDI(IIMidiHdr);
			else PrintToConsole(FOREGROUND_RED, (DWORD)IIMidiHdr->lpData, "Ignored SysEx MIDI event.");
			IIMidiHdr->dwFlags &= ~MHDR_INQUEUE;
			IIMidiHdr->dwFlags |= MHDR_DONE;
			return MMSYSERR_NOERROR;
		}
		else return MMSYSERR_NOMEM;
	}
	catch (...) { return MMSYSERR_INVALPARAM; }
}