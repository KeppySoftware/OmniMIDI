// KSDAPI calls
#define KSDAPIVer "1.0A (YHLBBJ)"

void keepstreamsalive(int& opend) {
	BASS_ChannelIsActive(KSStream);
	if (BASS_ErrorGetCode() == 5 || livechange == 1) {
		PrintToConsole(FOREGROUND_RED, 1, "Restarting audio stream...");
		CloseThreads();
		LoadSettings(TRUE);
		if (!com_initialized) { if (!FAILED(CoInitialize(NULL))) com_initialized = TRUE; }
		SetConsoleTextAttribute(hConsole, FOREGROUND_RED);
		if (InitializeBASS(FALSE)) {
			SetUpStream();
			opend = CreateThreads(TRUE);
			LoadSoundFontsToStream();
		}
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
					opend = CreateThreads(TRUE);
					LoadSoundFontsToStream();
				}
			}
			PrintToConsole(FOREGROUND_RED, 1, "Checking for settings changes or hotkeys...");
			while (stop_rtthread == FALSE) {
				start1 = TimeNow();
				keepstreamsalive(opend);
				LoadCustomInstruments();
				CheckVolume();
				ParseDebugData();
				Sleep(1);
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

void DoCallback(int clientNum, DWORD msg, DWORD_PTR param1, DWORD_PTR param2) {
	struct Driver_Client *client = &drivers[0].clients[clientNum];
	DriverCallback(client->callback, client->flags, drivers[0].hdrvr, msg, client->instance, param1, param2);
}

void DoStartClient() {
	if (modm_closed == TRUE) {
		HKEY hKey;
		long lResult;
		DWORD dwType = REG_DWORD;
		DWORD dwSize = sizeof(DWORD);
		int One = 0;
		lResult = RegOpenKeyEx(HKEY_CURRENT_USER, L"Software\\Keppy's Synthesizer", 0, KEY_ALL_ACCESS, &hKey);
		RegQueryValueEx(hKey, L"driverprio", NULL, &dwType, (LPBYTE)&driverprio, &dwSize);
		RegCloseKey(hKey);
		lResult = RegOpenKeyEx(HKEY_CURRENT_USER, L"Software\\Keppy's Synthesizer\\Settings", 0, KEY_ALL_ACCESS, &hKey);
		RegQueryValueEx(hKey, L"improveperf", NULL, &dwType, (LPBYTE)&improveperf, &dwSize);
		RegCloseKey(hKey);

		StartDebugPipe(FALSE);

		InitializeCriticalSection(&midiparsing);
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
	DeleteCriticalSection(&midiparsing);
}

void DoResetClient(UINT uDeviceID) {
	reset_synth = 1;
	ResetSynth(0);
}

char const* WINAPI ReturnKSDAPIVer()
{
	return KSDAPIVer;
}

void InitializeKSStream() {
	DoStartClient();
}

void TerminateKSStream() {
	DoStopClient();
}

MMRESULT WINAPI SendDirectData(DWORD dwMsg)
{
	MMRESULT returnme = ParseData(TRUE, 0, MODM_DATA, 0, dwMsg, 0, 0, 0);
	return returnme;
}

MMRESULT WINAPI SendDirectDataNoBuf(DWORD dwMsg)
{
	try {
		if (ksdirectenabled != TRUE) ksdirectenabled = TRUE;
		SendToBASSMIDI(dwMsg);
		return MMSYSERR_NOERROR;
	}
	catch (...) { return MMSYSERR_INVALPARAM; }
}