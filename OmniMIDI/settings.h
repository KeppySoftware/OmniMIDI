/*
OmniMIDI settings loading system
*/
#pragma once

void ResetSynth(BOOL SwitchingBufferMode) {
	if (SwitchingBufferMode) {	
		EVBuffer.ReadHead = 0;
		EVBuffer.WriteHead = 0;
		memset(EVBuffer.Buffer, -1, sizeof(EVBuffer.Buffer));
	}
	BASS_ChannelSetAttribute(OMStream, BASS_ATTRIB_MIDI_CHANS, 16);
	BASS_MIDI_StreamEvent(OMStream, 0, MIDI_EVENT_SYSTEM, MIDI_SYSTEM_DEFAULT);
	BASS_MIDI_StreamEvent(OMStream, 9, MIDI_EVENT_DRUMS, 1);
}

void OpenRegistryKey(RegKey &hKey, LPCWSTR hKeyDir, BOOL Mandatory) {
	// If the key isn't ready, open it again
	if (hKey.Status != KEY_READY && !hKey.Address) {
		// Open the key
		hKey.Status = RegOpenKeyEx(HKEY_CURRENT_USER, hKeyDir, 0, KEY_ALL_ACCESS, &hKey.Address);

		// If the key failed to open, throw a crash (If needed)
		if (hKey.Status != KEY_READY && Mandatory) CrashMessage("hKeyOpen");
	}
}

void CloseRegistryKey(RegKey &hKey) {
	if (hKey.Address) {
		// Try to flush the key
		LSTATUS Action = RegFlushKey(hKey.Address);
		// If the key can't be flushed, throw a crash
		if (Action != ERROR_SUCCESS) CrashMessage("hKeyFlush");

		// Try to close the key
		Action = RegCloseKey(hKey.Address);
		// If the key can't be closed, throw a crash
		if (Action != ERROR_SUCCESS) CrashMessage("hKeyClose");

		// Everything is fine, mark the key as closed
		hKey.Status = KEY_CLOSED;
		hKey.Address = NULL;
	}
}

BOOL CloseThread(HANDLE thread) {
	// Wait for the thread to finish its job
	if (thread) {
		PrintMessageToDebugLog("CloseThread", "Waiting for passed thread to finish...");
		WaitForSingleObject(thread, INFINITE);

		// And mark it as NULL
		PrintMessageToDebugLog("CloseThread", "Cleaning up...");
		thread = NULL;

		PrintMessageToDebugLog("CloseThread", "Thread is down.");
		return TRUE;
	}

	PrintMessageToDebugLog("CloseThread", "The passed thread doesn't exist.");
	return FALSE;
}

void DLLLoadError(LPWSTR dll) {
	TCHAR Error[NTFS_MAX_PATH] = { 0 };

	// Print to log
	PrintCurrentTime();
	if (DebugLog) fprintf(DebugLog, "ERROR | Unable to load the following DLL: %s\n", dll);

	// Show error message
	swprintf_s(Error, L"An error has occurred while loading the following library: %s\n\nClick OK to close the program.", dll);
	MessageBoxW(NULL, Error, L"OmniMIDI - DLL load error", MB_ICONERROR | MB_SYSTEMMODAL);
	exit(0);
}

long long TimeNow() {
	LARGE_INTEGER now;
	LARGE_INTEGER s_frequency;
	QueryPerformanceCounter(&now);
	QueryPerformanceFrequency(&s_frequency);
	return (1000LL * now.QuadPart) / s_frequency.QuadPart;
}

void LoadSoundfont(int whichsf) {
	DWORD CurrentList = (whichsf + 1);
	wchar_t ListToLoad[NTFS_MAX_PATH] = { 0 };

	if (!SHGetFolderPathW(NULL, CSIDL_PROFILE, NULL, 0, ListToLoad)) {
		PrintMessageToDebugLog("LoadSoundFontFunc", "Loading soundfont list...");

		OpenRegistryKey(SFDynamicLoader, L"Software\\OmniMIDI\\Watchdog", TRUE);
		RegSetValueExW(SFDynamicLoader.Address, L"currentsflist", 0, REG_DWORD, (LPBYTE)&CurrentList, sizeof(CurrentList));
		swprintf_s(ListToLoad + wcslen(ListToLoad), NTFS_MAX_PATH, OMFileTemplate, L"lists", OMLetters[whichsf], L"omlist");

		if (FontLoader(ListToLoad)) PrintMessageToDebugLog("LoadSoundFontFunc", "Done!");
	}
}

bool LoadSoundfontStartup() {
	wchar_t CurrentAppList[NTFS_MAX_PATH] = { 0 };
	wchar_t CurrentString[NTFS_MAX_PATH] = { 0 };

	if (!SHGetFolderPath(NULL, CSIDL_PROFILE, NULL, 0, CurrentAppList)) {
		for (int i = 0; i <= 15; ++i) {
			swprintf_s(CurrentAppList + wcslen(CurrentAppList), NTFS_MAX_PATH, OMFileTemplate, L"applists", OMLetters[i], L"applist");

			std::wifstream AppList(CurrentAppList);
			if (AppList) {
				AppList.imbue(UTF8Support);
				while (AppList.getline(CurrentString, sizeof(CurrentString) / sizeof(*CurrentString)))
				{
					if (!_wcsicmp(AppNameW, CurrentString) && !_wcsicmp(AppPathW, CurrentString)) {
						PrintMessageToDebugLog("LoadSoundfontStartup", "Found list. Loading...");
						LoadSoundfont(i);
						return TRUE;
					}
				}
			}
		}

		PrintMessageToDebugLog("LoadSoundfontStartup", "No default startup list found. Continuing...");
		return FALSE;
	}
}

void LoadDriverModule(HMODULE * Target, wchar_t * InstallPath, wchar_t * RequestedLib, BOOL Mandatory) 
{
	wchar_t DLLPath[MAX_PATH] = { 0 };

	if (!(*Target = LoadLibrary(RequestedLib))) {
		PrintLoadedDLLToDebugLog(RequestedLib, "No library has been found in memory. The driver will now load the DLL...");
		wcscat(DLLPath, InstallPath);
		wcscat(DLLPath, L"\\");
		wcscat(DLLPath, RequestedLib);
		if (!(*Target = LoadLibraryEx(DLLPath, NULL, 0))) {
			if (Mandatory) {
				DLLLoadError(DLLPath);
				exit(0);
			}
			else PrintLoadedDLLToDebugLog(DLLPath, "Failed to load requested library. It probably requires some missing dependencies.");
		}
		else PrintLoadedDLLToDebugLog(RequestedLib, "The library is now in memory.");
	}
	else PrintLoadedDLLToDebugLog(RequestedLib, "The library is already in memory. The HMODULE will be a pointer to that address.");
}

VOID LoadBASSFunctions()
{
	wchar_t InstallPath[MAX_PATH] = { 0 };

	try {
		if (!BASSLoadedToMemory) {
			if (GetModuleFileName(hinst, InstallPath, MAX_PATH))
			{
				PathRemoveFileSpec(InstallPath);

				PrintMessageToDebugLog("ImportBASS", "Importing BASS DLLs to memory...");

				LoadDriverModule(&bass, InstallPath, L"bass.dll", TRUE);
				LoadDriverModule(&bassmidi, InstallPath, L"bassmidi.dll", TRUE);
				LoadDriverModule(&bassenc, InstallPath, L"bassenc.dll", TRUE);
				LoadDriverModule(&bassasio, InstallPath, L"bassasio.dll", TRUE);

				PrintMessageToDebugLog("ImportBASS", "DLLs loaded into memory. Importing functions...");

				// Load all the functions into memory
				LOADLIBFUNCTION(bassasio, BASS_ASIO_ChannelEnable);
				LOADLIBFUNCTION(bassasio, BASS_ASIO_ChannelEnableMirror);
				LOADLIBFUNCTION(bassasio, BASS_ASIO_ChannelGetLevel);
				LOADLIBFUNCTION(bassasio, BASS_ASIO_ChannelJoin);
				LOADLIBFUNCTION(bassasio, BASS_ASIO_ChannelReset);
				LOADLIBFUNCTION(bassasio, BASS_ASIO_ChannelSetFormat);
				LOADLIBFUNCTION(bassasio, BASS_ASIO_ChannelSetRate);
				LOADLIBFUNCTION(bassasio, BASS_ASIO_ChannelSetVolume);
				LOADLIBFUNCTION(bassasio, BASS_ASIO_ControlPanel);
				LOADLIBFUNCTION(bassasio, BASS_ASIO_ErrorGetCode);
				LOADLIBFUNCTION(bassasio, BASS_ASIO_Free);
				LOADLIBFUNCTION(bassasio, BASS_ASIO_GetDeviceInfo);
				LOADLIBFUNCTION(bassasio, BASS_ASIO_GetLatency);
				LOADLIBFUNCTION(bassasio, BASS_ASIO_GetRate);
				LOADLIBFUNCTION(bassasio, BASS_ASIO_Init);
				LOADLIBFUNCTION(bassasio, BASS_ASIO_SetRate);
				LOADLIBFUNCTION(bassasio, BASS_ASIO_Start);
				LOADLIBFUNCTION(bassasio, BASS_ASIO_Stop);
				LOADLIBFUNCTION(bassasio, BASS_ASIO_ChannelPause);
				LOADLIBFUNCTION(bassenc, BASS_Encode_Start);
				LOADLIBFUNCTION(bassenc, BASS_Encode_Stop);
				LOADLIBFUNCTION(bass, BASS_ChannelFlags);
				LOADLIBFUNCTION(bass, BASS_ChannelGetAttribute);
				LOADLIBFUNCTION(bass, BASS_ChannelGetData);
				LOADLIBFUNCTION(bass, BASS_ChannelGetLevelEx);
				LOADLIBFUNCTION(bass, BASS_ChannelIsActive);
				LOADLIBFUNCTION(bass, BASS_ChannelPlay);
				LOADLIBFUNCTION(bass, BASS_ChannelRemoveFX);
				LOADLIBFUNCTION(bass, BASS_ChannelSeconds2Bytes);
				LOADLIBFUNCTION(bass, BASS_ChannelSeconds2Bytes);
				LOADLIBFUNCTION(bass, BASS_ChannelSetAttribute);
				LOADLIBFUNCTION(bass, BASS_ChannelSetDevice);
				LOADLIBFUNCTION(bass, BASS_ChannelSetFX);
				LOADLIBFUNCTION(bass, BASS_ChannelSetSync);
				LOADLIBFUNCTION(bass, BASS_ChannelStop);
				LOADLIBFUNCTION(bass, BASS_ChannelUpdate);
				LOADLIBFUNCTION(bass, BASS_ErrorGetCode);
				LOADLIBFUNCTION(bass, BASS_FXSetParameters);
				LOADLIBFUNCTION(bass, BASS_Free);
				LOADLIBFUNCTION(bass, BASS_GetDevice);
				LOADLIBFUNCTION(bass, BASS_GetDeviceInfo);
				LOADLIBFUNCTION(bass, BASS_GetInfo);
				LOADLIBFUNCTION(bass, BASS_Init);
				LOADLIBFUNCTION(bass, BASS_PluginLoad);
				LOADLIBFUNCTION(bass, BASS_SetConfig);
				LOADLIBFUNCTION(bass, BASS_SetDevice);
				LOADLIBFUNCTION(bass, BASS_SetVolume);
				LOADLIBFUNCTION(bass, BASS_Stop);
				LOADLIBFUNCTION(bass, BASS_StreamFree);
				LOADLIBFUNCTION(bass, BASS_Update);
				LOADLIBFUNCTION(bassmidi, BASS_MIDI_FontFree);
				LOADLIBFUNCTION(bassmidi, BASS_MIDI_FontInit);
				LOADLIBFUNCTION(bassmidi, BASS_MIDI_FontLoad);
				LOADLIBFUNCTION(bassmidi, BASS_MIDI_StreamCreate);
				LOADLIBFUNCTION(bassmidi, BASS_MIDI_StreamEvent);
				LOADLIBFUNCTION(bassmidi, BASS_MIDI_StreamEvents);
				LOADLIBFUNCTION(bassmidi, BASS_MIDI_StreamGetEvent);
				LOADLIBFUNCTION(bassmidi, BASS_MIDI_StreamLoadSamples);
				LOADLIBFUNCTION(bassmidi, BASS_MIDI_StreamSetFonts);

				PrintMessageToDebugLog("ImportBASS", "Function pointers loaded into memory.");
			}
			else {
				PrintMessageToDebugLog("ImportBASS", "Failed to locate main directory for libs.");
				BASSLoadedToMemory = FALSE;
				return;
			}
		}
		else PrintMessageToDebugLog("ImportBASS", "BASS has been already loaded by the driver.");

		BASSLoadedToMemory = TRUE;
	}
	catch (...) {
		CrashMessage("BASSLibLoad");
	}
}

VOID UnloadBASSFunctions() {
	try {
		if (BASSLoadedToMemory) {
			PrintMessageToDebugLog("UnloadBASS", "Freeing BASS libraries...");
			FreeLibrary(bass);
			FreeLibrary(bassmidi);
			FreeLibrary(bassenc);
			FreeLibrary(bassasio);
			if (bass_vst) FreeLibrary(bass_vst);
			PrintMessageToDebugLog("UnloadBASS", "The BASS libraries have been freed from the app's working set.");
		}
		else PrintMessageToDebugLog("UnloadBASS", "BASS hasn't been loaded by the driver yet.");

		BASSLoadedToMemory = FALSE;
	}
	catch (...) {
		CrashMessage("BASSLibUnload");
	}
}

void ResetEVBufferSettings() {
	EvBufferSize = 4096;
	EvBufferMultRatio = 1;

	OpenRegistryKey(Configuration, L"Software\\OmniMIDI\\Configuration", TRUE);
	RegSetValueEx(Configuration.Address, L"EvBufferSize", 0, REG_QWORD, (LPBYTE)&EvBufferSize, sizeof(EvBufferSize));
	RegSetValueEx(Configuration.Address, L"EvBufferMultRatio", 0, REG_DWORD, (LPBYTE)&EvBufferMultRatio, sizeof(EvBufferMultRatio));
}

void FreeUpMemory() {
	// Free up the memory, since it's not needed or it has to be reinitialized
	PrintMessageToDebugLog("FreeUpMemoryFunc", "Freeing EV buffer...");
	RtlSecureZeroMemory(EVBuffer.Buffer, sizeof(EVBuffer.Buffer));
	free(EVBuffer.Buffer);
	EVBuffer.Buffer = NULL;
	PrintMessageToDebugLog("FreeUpMemoryFunc", "Freed.");

	PrintMessageToDebugLog("FreeUpMemoryFunc", "Freeing audio buffer...");
	RtlSecureZeroMemory(sndbf, sizeof(sndbf));
	free(sndbf);
	sndbf = NULL;
	PrintMessageToDebugLog("FreeUpMemoryFunc", "Freed.");
}

void AllocateMemory(BOOL restart) {
	try {
		PrintMessageToDebugLog("AllocateMemoryFunc", "Allocating memory for EV buffer and audio buffer");

		// Check how much RAM is available
		ULONGLONG TempEvBufferSize = EvBufferSize;
		MEMORYSTATUSEX status;
		status.dwLength = sizeof(status);
		GlobalMemoryStatusEx(&status);

		// Check if the user has chose to get the EVBuffer size from the RAM
		if (GetEvBuffSizeFromRAM == 1) {
			// He did, do a calculation to get the size
			TempEvBufferSize = status.ullTotalPhys;
			if (EvBufferMultRatio < 2) EvBufferMultRatio = 128;
		}
		else {
			// He didn't, check if the selected EVBuffer size doesn't exceed the maximum amount of RAM available
			if (TempEvBufferSize >= status.ullTotalPhys) {
				MessageBox(NULL, L"The events buffer cannot allocate more than the total RAM available!\nIts size will now default to 16384 bytes.\n\nThe EVBuffer settings have been reset.", L"OmniMIDI - Illegal memory amount defined", MB_OK | MB_ICONEXCLAMATION | MB_SYSTEMMODAL);
				ResetEVBufferSettings();
				TempEvBufferSize = EvBufferSize;
			}
		}

#if !_M_AMD64
		// !! ONLY FOR x86 APPS !!

		// Check if the EVBuffer size goes above 512MB of RAM
		// Each 32-bit app is limited to a 2GB working set size
		if (TempEvBufferSize > 134217728) {
			// It is, limit the EVBuffer to 512MB
			PrintMessageToDebugLog("AllocateMemoryFunc", "EV buffer is too big, limiting to 512MB...");
			TempEvBufferSize = 134217728;
		}
#endif

		// Calculate the ratio
		EvBufferSize = TempEvBufferSize / (unsigned long long)EvBufferMultRatio;

		if (EvBufferSize < 1) {
			MessageBox(NULL, L"The size of the buffer cannot be 0!\nIts size will now default to 16384 bytes.\n\nThe settings have been reset.", L"OmniMIDI - Illegal memory amount defined", MB_OK | MB_ICONEXCLAMATION | MB_SYSTEMMODAL);
			ResetEVBufferSettings();
			TempEvBufferSize = EvBufferSize;
		}

		// Print the values to the log
		PrintMessageToDebugLog("AllocateMemoryFunc", "Final EV buffer settings: ");
		PrintMemoryMessageToDebugLog("AllocateMemoryFunc", "EV buffer size (in amount of DWORDs)", FALSE, TempEvBufferSize);
		PrintMemoryMessageToDebugLog("AllocateMemoryFunc", "EV buffer division ratio", TRUE, EvBufferMultRatio);
		PrintMemoryMessageToDebugLog("AllocateMemoryFunc", "EV buffer final size (in bytes, one DWORD is 4 bytes)", FALSE, EvBufferSize * 4);

		if (restart) FreeUpMemory();

		// Begin allocating the EVBuffer
		if (EVBuffer.Buffer != NULL) PrintMessageToDebugLog("AllocateMemoryFunc", "EV buffer already allocated.");
		else {
			PrintMessageToDebugLog("AllocateMemoryFunc", "Allocating EV buffer...");
			EVBuffer.Buffer = (DWORD*)calloc(EvBufferSize, sizeof(DWORD));
			if (!EVBuffer.Buffer) {
				MessageBox(NULL, L"An error has occured while allocating the events buffer!\nIt will now default to 4096 bytes.\n\nThe EVBuffer settings have been reset.", L"OmniMIDI - Error allocating memory", MB_OK | MB_ICONEXCLAMATION | MB_SYSTEMMODAL);
				ResetEVBufferSettings();
				EVBuffer.Buffer = (DWORD*)calloc(EvBufferSize, sizeof(DWORD));
				if (!EVBuffer.Buffer) {
					MessageBox(NULL, L"Fatal error while allocating the events buffer.\n\nPress OK to quit.", L"OmniMIDI - Fatal error", MB_OK | MB_ICONERROR | MB_SYSTEMMODAL);
					exit(0x3623);
				}
			}
			PrintMessageToDebugLog("AllocateMemoryFunc", "EV buffer allocated.");
		}

		// Done, now allocate the buffer for the ".WAV mode"
		if (sndbf != NULL) PrintMessageToDebugLog("AllocateMemoryFunc", "Audio buffer already allocated.");
		else {
			PrintMessageToDebugLog("AllocateMemoryFunc", "Allocating audio buffer...");
			sndbf = (float *)calloc(sndbflen, sizeof(float));
			if (!sndbf) {
				PrintMessageToDebugLog("AllocateMemoryFunc", "An error has occurred while allocating the audio buffer.");
				MessageBox(NULL, L"Fatal error while allocating the sound buffer.\n\nPress OK to quit.", L"OmniMIDI - Fatal error", MB_OK | MB_ICONERROR | MB_SYSTEMMODAL);
				exit(0x3623);
			}
			PrintMessageToDebugLog("AllocateMemoryFunc", "Audio buffer allocated.");
		}
	}
	catch (...) {
		CrashMessage("EVBufAlloc");
	}
}

void LoadSettings(BOOL restart)
{
	try {
		DWORD64 TEvBufferSize, TEvBufferMultRatio;

		PrintMessageToDebugLog("LoadSettingsFuncs", "Loading settings from registry...");

		// Load the settings from the registry
		OpenRegistryKey(Configuration, L"Software\\OmniMIDI\\Configuration", TRUE);

		RegQueryValueEx(Configuration.Address, L"AudioBitDepth", NULL, &dwType, (LPBYTE)&ManagedSettings.AudioBitDepth, &dwSize);
		RegQueryValueEx(Configuration.Address, L"AudioFrequency", NULL, &dwType, (LPBYTE)&ManagedSettings.AudioFrequency, &dwSize);
		RegQueryValueEx(Configuration.Address, L"AudioOutput", NULL, &dwType, (LPBYTE)&ManagedSettings.AudioOutputReg, &dwSize);
		RegQueryValueEx(Configuration.Address, L"BufferLength", NULL, &dwType, (LPBYTE)&ManagedSettings.BufferLength, &dwSize);
		RegQueryValueEx(Configuration.Address, L"CapFramerate", NULL, &dwType, (LPBYTE)&ManagedSettings.CapFramerate, &dwSize);
		RegQueryValueEx(Configuration.Address, L"ChannelUpdateLength", NULL, &dwType, (LPBYTE)&ManagedSettings.ChannelUpdateLength, &dwSize);
		RegQueryValueEx(Configuration.Address, L"CurrentEngine", NULL, &dwType, (LPBYTE)&ManagedSettings.CurrentEngine, &dwSize);
		RegQueryValueEx(Configuration.Address, L"DelayNoteOff", NULL, &dwType, (LPBYTE)&ManagedSettings.DelayNoteOff, &dwSize);
		RegQueryValueEx(Configuration.Address, L"DelayNoteOffValue", NULL, &dwType, (LPBYTE)&ManagedSettings.DelayNoteOffValue, &dwSize);
		RegQueryValueEx(Configuration.Address, L"DisableChime", NULL, &dwType, (LPBYTE)&DisableChime, &dwSize);
		RegQueryValueEx(Configuration.Address, L"DisableNotesFadeOut", NULL, &dwType, (LPBYTE)&ManagedSettings.DisableNotesFadeOut, &dwSize);
		RegQueryValueEx(Configuration.Address, L"DontMissNotes", NULL, &dwType, (LPBYTE)&ManagedSettings.DontMissNotes, &dwSize);
		RegQueryValueEx(Configuration.Address, L"DriverPriority", NULL, &dwType, (LPBYTE)&ManagedSettings.DriverPriority, &dwSize);
		RegQueryValueEx(Configuration.Address, L"EnableSFX", NULL, &dwType, (LPBYTE)&ManagedSettings.EnableSFX, &dwSize);
		RegQueryValueEx(Configuration.Address, L"EvBufferMultRatio", NULL, &dwType, (LPBYTE)&TEvBufferMultRatio, &dwSize);
		RegQueryValueEx(Configuration.Address, L"EvBufferSize", NULL, &qwType, (LPBYTE)&TEvBufferSize, &qwSize);
		RegQueryValueEx(Configuration.Address, L"Extra8Lists", NULL, &dwType, (LPBYTE)&ManagedSettings.Extra8Lists, &dwSize);
		RegQueryValueEx(Configuration.Address, L"FastHotkeys", NULL, &dwType, (LPBYTE)&ManagedSettings.FastHotkeys, &dwSize);
		RegQueryValueEx(Configuration.Address, L"FullVelocityMode", NULL, &dwType, (LPBYTE)&ManagedSettings.FullVelocityMode, &dwSize);
		RegQueryValueEx(Configuration.Address, L"GetEvBuffSizeFromRAM", NULL, &dwType, (LPBYTE)&GetEvBuffSizeFromRAM, &dwSize);
		RegQueryValueEx(Configuration.Address, L"HyperPlayback", NULL, &dwType, (LPBYTE)&HyperMode, &dwSize);
		RegQueryValueEx(Configuration.Address, L"IgnoreAllEvents", NULL, &dwType, (LPBYTE)&ManagedSettings.IgnoreAllEvents, &dwSize);
		RegQueryValueEx(Configuration.Address, L"IgnoreNotesBetweenVel", NULL, &dwType, (LPBYTE)&ManagedSettings.IgnoreNotesBetweenVel, &dwSize);
		RegQueryValueEx(Configuration.Address, L"IgnoreSysReset", NULL, &dwType, (LPBYTE)&ManagedSettings.IgnoreSysReset, &dwSize);
		RegQueryValueEx(Configuration.Address, L"LimitTo88Keys", NULL, &dwType, (LPBYTE)&ManagedSettings.LimitTo88Keys, &dwSize);
		RegQueryValueEx(Configuration.Address, L"MT32Mode", NULL, &dwType, (LPBYTE)&ManagedSettings.MT32Mode, &dwSize);
		RegQueryValueEx(Configuration.Address, L"MaxRenderingTime", NULL, &dwType, (LPBYTE)&ManagedSettings.MaxRenderingTime, &dwSize);
		RegQueryValueEx(Configuration.Address, L"MaxVelIgnore", NULL, &dwType, (LPBYTE)&ManagedSettings.MaxVelIgnore, &dwSize);
		RegQueryValueEx(Configuration.Address, L"MaxVoices", NULL, &dwType, (LPBYTE)&ManagedSettings.MaxVoices, &dwSize);
		RegQueryValueEx(Configuration.Address, L"MinVelIgnore", NULL, &dwType, (LPBYTE)&ManagedSettings.MinVelIgnore, &dwSize);
		RegQueryValueEx(Configuration.Address, L"MonoRendering", NULL, &dwType, (LPBYTE)&ManagedSettings.MonoRendering, &dwSize);
		RegQueryValueEx(Configuration.Address, L"NoteLengthValue", NULL, &dwType, (LPBYTE)&ManagedSettings.NoteLengthValue, &dwSize);
		RegQueryValueEx(Configuration.Address, L"NoteOff1", NULL, &dwType, (LPBYTE)&ManagedSettings.NoteOff1, &dwSize);
		RegQueryValueEx(Configuration.Address, L"OutputVolume", NULL, &dwType, (LPBYTE)&ManagedSettings.OutputVolume, &dwSize);
		RegQueryValueEx(Configuration.Address, L"OverrideNoteLength", NULL, &dwType, (LPBYTE)&ManagedSettings.OverrideNoteLength, &dwSize);
		RegQueryValueEx(Configuration.Address, L"PreloadSoundFonts", NULL, &dwType, (LPBYTE)&ManagedSettings.PreloadSoundFonts, &dwSize);
		RegQueryValueEx(Configuration.Address, L"SincConv", NULL, &dwType, (LPBYTE)&ManagedSettings.SincConv, &dwSize);
		RegQueryValueEx(Configuration.Address, L"SincInter", NULL, &dwType, (LPBYTE)&ManagedSettings.SincInter, &dwSize);
		RegQueryValueEx(Configuration.Address, L"TransposeValue", NULL, &dwType, (LPBYTE)& ManagedSettings.TransposeValue, &dwSize);
		RegQueryValueEx(Configuration.Address, L"FollowDefaultAudioDevice", NULL, &dwType, (LPBYTE)&ManagedSettings.FollowDefaultAudioDevice, &dwSize);

		if (ManagedSettings.CurrentEngine != AUDTOWAV) RegQueryValueEx(Configuration.Address, L"NotesCatcherWithAudio", NULL, &dwType, (LPBYTE)&ManagedSettings.NotesCatcherWithAudio, &dwSize);
		else ManagedSettings.NotesCatcherWithAudio = FALSE;

		RegSetValueEx(Configuration.Address, L"LiveChanges", 0, REG_DWORD, (LPBYTE)&Blank, sizeof(Blank));

		// Stuff that works, don't bother
		if (!Between(ManagedSettings.MinVelIgnore, 1, 127)) { ManagedSettings.MinVelIgnore = 1; }
		if (!Between(ManagedSettings.MaxVelIgnore, 1, 127)) { ManagedSettings.MaxVelIgnore = 1; }
		SynthVolume = (float)ManagedSettings.OutputVolume / 10000.0f;

		// Check if "Hyper-playback" mode has been enabled
		if (HyperMode && !DisableChime) {
			// It's enabled, do some beeps to notify the user (If the chime is enabled)
			Beep(440, 100);
			Beep(687, 100);
		}

		// Assign the pointed functions
		_PrsData = HyperMode ? ParseDataHyper : ParseData;
		_PforBASSMIDI = HyperMode ? PrepareForBASSMIDIHyper : PrepareForBASSMIDI;
		_PlayBufData = HyperMode ? PlayBufferedDataHyper : PlayBufferedData;
		_PlayBufDataChk = HyperMode ? PlayBufferedDataChunkHyper : PlayBufferedDataChunk;

		if (!restart || (TEvBufferSize != EvBufferSize || TEvBufferMultRatio != EvBufferMultRatio)) {
			EvBufferSize = TEvBufferSize;
			EvBufferMultRatio = TEvBufferMultRatio;
			AllocateMemory(restart);
		}

		PrintMessageToDebugLog("LoadSettingsFuncs", "Settings loaded.");
	}
	catch (...) {
		CrashMessage("LoadSettings");
	}
}

void LoadSettingsRT() {
	if (!SettingsManagedByClient) {
		try {
			// Initialize the temp values
			DWORD TempSC, TempOV, TempHP, TempMV;
			BOOL TempESFX, TempNOFF1, TempISR, TempSI, TempDNFO, TempDMN;

			// Load the settings
			OpenRegistryKey(Configuration, L"Software\\OmniMIDI\\Configuration", TRUE);

			RegQueryValueEx(Configuration.Address, L"BufferLength", NULL, &dwType, (LPBYTE)&ManagedSettings.BufferLength, &dwSize);
			RegQueryValueEx(Configuration.Address, L"CapFramerate", NULL, &dwType, (LPBYTE)&ManagedSettings.CapFramerate, &dwSize);
			RegQueryValueEx(Configuration.Address, L"ChannelUpdateLength", NULL, &dwType, (LPBYTE)&ManagedSettings.ChannelUpdateLength, &dwSize);
			RegQueryValueEx(Configuration.Address, L"DelayNoteOff", NULL, &dwType, (LPBYTE)&ManagedSettings.DelayNoteOff, &dwSize);
			RegQueryValueEx(Configuration.Address, L"DelayNoteOffValue", NULL, &dwType, (LPBYTE)&ManagedSettings.DelayNoteOffValue, &dwSize);
			RegQueryValueEx(Configuration.Address, L"DisableNotesFadeOut", NULL, &dwType, (LPBYTE)&TempDNFO, &dwSize);
			RegQueryValueEx(Configuration.Address, L"DontMissNotes", NULL, &dwType, (LPBYTE)&TempDMN, &dwSize);
			RegQueryValueEx(Configuration.Address, L"EnableSFX", NULL, &dwType, (LPBYTE)&TempESFX, &dwSize);
			RegQueryValueEx(Configuration.Address, L"FastHotkeys", NULL, &dwType, (LPBYTE)&ManagedSettings.FastHotkeys, &dwSize);
			RegQueryValueEx(Configuration.Address, L"FullVelocityMode", NULL, &dwType, (LPBYTE)&ManagedSettings.FullVelocityMode, &dwSize);
			RegQueryValueEx(Configuration.Address, L"HyperPlayback", NULL, &dwType, (LPBYTE)&TempHP, &dwSize);
			RegQueryValueEx(Configuration.Address, L"IgnoreAllEvents", NULL, &dwType, (LPBYTE)&ManagedSettings.IgnoreAllEvents, &dwSize);
			RegQueryValueEx(Configuration.Address, L"IgnoreNotesBetweenVel", NULL, &dwType, (LPBYTE)&ManagedSettings.IgnoreNotesBetweenVel, &dwSize);
			RegQueryValueEx(Configuration.Address, L"IgnoreSysReset", NULL, &dwType, (LPBYTE)&TempISR, &dwSize);
			RegQueryValueEx(Configuration.Address, L"LimitTo88Keys", NULL, &dwType, (LPBYTE)&ManagedSettings.LimitTo88Keys, &dwSize);
			RegQueryValueEx(Configuration.Address, L"LiveChanges", NULL, &dwType, (LPBYTE)&ManagedSettings.LiveChanges, &dwSize);
			RegQueryValueEx(Configuration.Address, L"MT32Mode", NULL, &dwType, (LPBYTE)&ManagedSettings.MT32Mode, &dwSize);
			RegQueryValueEx(Configuration.Address, L"MaxRenderingTime", NULL, &dwType, (LPBYTE)&ManagedSettings.MaxRenderingTime, &dwSize);
			RegQueryValueEx(Configuration.Address, L"MaxVelIgnore", NULL, &dwType, (LPBYTE)&ManagedSettings.MaxVelIgnore, &dwSize);
			RegQueryValueEx(Configuration.Address, L"MaxVoices", NULL, &dwType, (LPBYTE)&TempMV, &dwSize);
			RegQueryValueEx(Configuration.Address, L"MinVelIgnore", NULL, &dwType, (LPBYTE)&ManagedSettings.MinVelIgnore, &dwSize);
			RegQueryValueEx(Configuration.Address, L"NoteLengthValue", NULL, &dwType, (LPBYTE)&ManagedSettings.NoteLengthValue, &dwSize);
			RegQueryValueEx(Configuration.Address, L"NoteOff1", NULL, &dwType, (LPBYTE)&TempNOFF1, &dwSize);
			RegQueryValueEx(Configuration.Address, L"OutputVolume", NULL, &dwType, (LPBYTE)&TempOV, &dwSize);
			RegQueryValueEx(Configuration.Address, L"OverrideNoteLength", NULL, &dwType, (LPBYTE)&ManagedSettings.OverrideNoteLength, &dwSize);
			RegQueryValueEx(Configuration.Address, L"PreloadSoundFonts", NULL, &dwType, (LPBYTE)&ManagedSettings.PreloadSoundFonts, &dwSize);
			RegQueryValueEx(Configuration.Address, L"SincConv", NULL, &dwType, (LPBYTE)&TempSC, &dwSize);
			RegQueryValueEx(Configuration.Address, L"SincInter", NULL, &dwType, (LPBYTE)&TempSI, &dwSize);
			RegQueryValueEx(Configuration.Address, L"TransposeValue", NULL, &dwType, (LPBYTE)&ManagedSettings.TransposeValue, &dwSize);
			RegQueryValueEx(Configuration.Address, L"VolumeMonitor", NULL, &dwType, (LPBYTE)&ManagedSettings.VolumeMonitor, &dwSize);

			if (ManagedSettings.CurrentEngine != AUDTOWAV) RegQueryValueEx(Configuration.Address, L"NotesCatcherWithAudio", NULL, &dwType, (LPBYTE)&ManagedSettings.NotesCatcherWithAudio, &dwSize);
			else ManagedSettings.NotesCatcherWithAudio = FALSE;

			// Stuff that works so don't bother
			if (!Between(ManagedSettings.MinVelIgnore, 1, 127)) { ManagedSettings.MinVelIgnore = 1; }
			if (!Between(ManagedSettings.MaxVelIgnore, 1, 127)) { ManagedSettings.MaxVelIgnore = 1; }

			// Volume
			if (TempOV != ManagedSettings.OutputVolume) {
				ManagedSettings.OutputVolume = TempOV;
				SynthVolume = (float)ManagedSettings.OutputVolume / 10000.0f;
				ChVolumeStruct.fCurrent = 1.0f;
				ChVolumeStruct.fTarget = SynthVolume;
				ChVolumeStruct.fTime = 0.0f;
				ChVolumeStruct.lCurve = 0;
				BASS_FXSetParameters(ChVolume, &ChVolumeStruct);
				CheckUp(FALSE, ERRORCODE, L"Stream Volume FX Set", FALSE);
			}

			// Check if the value is different from the temporary one
			if (TempDMN != ManagedSettings.DontMissNotes) {
				// It is different, reset the synth
				// to avoid stuck notes or crashes
				ManagedSettings.DontMissNotes = TempDMN;
				ResetSynth(TRUE);
			}

			// Check if the value is different from the temporary one
			if (TempHP != HyperMode) {
				HyperMode = TempHP;

				// Close the threads for safety reasons
				stop_thread = TRUE;

				// Check if "Hyper-playback" mode has been enabled
				_PrsData = HyperMode ? ParseDataHyper : ParseData;
				_PforBASSMIDI = HyperMode ? PrepareForBASSMIDIHyper : PrepareForBASSMIDI;
				_PlayBufData = HyperMode ? PlayBufferedDataHyper : PlayBufferedData;
				_PlayBufDataChk = HyperMode ? PlayBufferedDataChunkHyper : PlayBufferedDataChunk;

				// Restart threads
				stop_thread = FALSE;
			}

			// Load the settings by comparing the temporary values to the driver's ones, to prevent overhead
			if (TempESFX != ManagedSettings.EnableSFX) {
				ManagedSettings.EnableSFX = TempESFX;
				BASS_ChannelFlags(OMStream, ManagedSettings.EnableSFX ? 0 : BASS_MIDI_NOFX, BASS_MIDI_NOFX);
			}

			if (TempNOFF1 != ManagedSettings.NoteOff1) {
				ManagedSettings.NoteOff1 = TempNOFF1;
				BASS_ChannelFlags(OMStream, ManagedSettings.NoteOff1 ? BASS_MIDI_NOTEOFF1 : 0, BASS_MIDI_NOTEOFF1);
			}

			if (TempISR != ManagedSettings.IgnoreSysReset) {
				ManagedSettings.IgnoreSysReset = TempNOFF1;
				BASS_ChannelFlags(OMStream, ManagedSettings.IgnoreSysReset ? BASS_MIDI_NOSYSRESET : 0, BASS_MIDI_NOSYSRESET);
			}

			if (TempSI != ManagedSettings.SincInter || TempSC != ManagedSettings.SincConv) {
				ManagedSettings.SincInter = TempSI;
				ManagedSettings.SincConv = TempSC;
				BASS_ChannelFlags(OMStream, ManagedSettings.SincInter ? BASS_MIDI_SINCINTER : 0, BASS_MIDI_SINCINTER);
				BASS_ChannelSetAttribute(OMStream, BASS_ATTRIB_SRC, ManagedSettings.SincConv);
			}

			if (TempDNFO != ManagedSettings.DisableNotesFadeOut) {
				ManagedSettings.DisableNotesFadeOut = TempDNFO;
				BASS_ChannelSetAttribute(OMStream, BASS_ATTRIB_MIDI_KILL, ManagedSettings.DisableNotesFadeOut);
			}

			if (TempMV != ManagedSettings.MaxVoices) {
				ManagedSettings.MaxVoices = TempMV;
				BASS_ChannelSetAttribute(OMStream, BASS_ATTRIB_MIDI_VOICES, ManagedSettings.MaxVoices);
			}
		}
		catch (...) {
			CrashMessage("LoadSettingsRT");
		}
	}
}

void LoadCustomInstruments() {
	wchar_t TempPc[MAXPNAMELEN] = { 0 };
	wchar_t TempBc[MAXPNAMELEN] = { 0 };

	try {
		OpenRegistryKey(ChanOverride, L"Software\\OmniMIDI\\ChanOverride", TRUE);

		RegQueryValueEx(ChanOverride.Address, L"overrideinstruments", NULL, &dwType, (LPBYTE)&ManagedSettings.OverrideInstruments, &dwSize);
		for (int i = 0; i <= 15; ++i) {
			swprintf_s(TempPc, MAXPNAMELEN, L"pc%d", i + 1);
			swprintf_s(TempBc, MAXPNAMELEN, L"bc%d", i + 1);

			// Load the custom bank/instrument for each channel
			RegQueryValueEx(ChanOverride.Address, TempPc, NULL, &dwType, (LPBYTE)&cpreset[i], &dwSize);
			RegQueryValueEx(ChanOverride.Address, TempBc, NULL, &dwType, (LPBYTE)&cbank[i], &dwSize);
		}
	}
	catch (...) {
		CrashMessage("LoadCustomInstruments");
	}
}

int AudioRenderingType(BOOLEAN IsItStreamCreation, INT RegistryVal) {
	if (RegistryVal == 1 || ManagedSettings.CurrentEngine == ASIO_ENGINE)
		return IsItStreamCreation ? BASS_SAMPLE_FLOAT : BASS_DATA_FLOAT;
	else if (RegistryVal == 2 || RegistryVal == 0)
		return IsItStreamCreation ? 0 : BASS_DATA_FIXED;
	else if (RegistryVal == 3)
		return IsItStreamCreation ? BASS_SAMPLE_8BITS : BASS_DATA_FIXED;
	else
		return IsItStreamCreation ? BASS_SAMPLE_FLOAT : BASS_DATA_FLOAT;
}

void SFDynamicLoaderCheck() {
	wchar_t TempRe[MAXPNAMELEN] = { 0 };

	try {
		// Used to check which SoundFont list has been loaded through the configurator
		OpenRegistryKey(SFDynamicLoader, L"Software\\OmniMIDI\\Watchdog", TRUE);

		// Check each value, to see if they're true or not
		for (int i = 0; i <= 15; ++i) {
			swprintf_s(TempRe, MAXPNAMELEN, L"rel%d", i + 1);

			RegQueryValueEx(SFDynamicLoader.Address, TempRe, NULL, &dwType, (LPBYTE)&rvalues[i], &dwSize);

			// Value "i" is true, reload the specific SoundFont list
			if (rvalues[i]) {
				RegSetValueEx(SFDynamicLoader.Address, TempRe, 0, REG_DWORD, (LPBYTE)&Blank, sizeof(Blank));
				LoadSoundfont(i);
			}
		}
	}
	catch (...) {
		CrashMessage("SFDynamicLoaderCheck");
	}
}

void CheckVolume(BOOL Closing) {
	try {
		// Self explanatory
		OpenRegistryKey(MainKey, L"Software\\OmniMIDI", TRUE);

		if (!Closing && !stop_thread && BASSLoadedToMemory && bass_initialized) {
			if (ManagedSettings.VolumeMonitor == TRUE && ManagedSettings.CurrentEngine > AUDTOWAV) {
				float levels[2];
				DWORD left, right;

				if (ManagedSettings.CurrentEngine == DXAUDIO_ENGINE || ManagedSettings.CurrentEngine == WASAPI_ENGINE) {
					BASS_ChannelGetLevelEx(OMStream, levels, (ManagedSettings.MonoRendering ? 0.01f : 0.02f), (ManagedSettings.MonoRendering ? BASS_LEVEL_MONO : BASS_LEVEL_STEREO));
				}
				else if (ManagedSettings.CurrentEngine == ASIO_ENGINE)
				{
					levels[0] = BASS_ASIO_ChannelGetLevel(FALSE, 0);
					levels[1] = BASS_ASIO_ChannelGetLevel(FALSE, 1);
				}

				DWORD level = MAKELONG((WORD)(min(levels[0], 1) * 32768), (WORD)(min(levels[1], 1) * 32768));
				left = LOWORD(level);	// the left level
				right = HIWORD(level);	// the right level

				RegSetValueEx(MainKey.Address, L"leftvol", 0, REG_DWORD, (LPBYTE)&left, sizeof(left));
				RegSetValueEx(MainKey.Address, L"rightvol", 0, REG_DWORD, (LPBYTE)&right, sizeof(right));
			}
		}
		else {
			RegSetValueEx(MainKey.Address, L"leftvol", 0, REG_DWORD, (LPBYTE)&Blank, sizeof(Blank));
			RegSetValueEx(MainKey.Address, L"rightvol", 0, REG_DWORD, (LPBYTE)&Blank, sizeof(Blank));
		}
	}
	catch (...) {
		CrashMessage("VolumeMonitor");
	}
}

void FillContentDebug(
	FLOAT CCUI0,				// Rendering time
	INT HC,						// App's handles
	ULONGLONG RUI,				// App's working size/RAM usage
	BOOL KDMAPIStatus,			// KDMAPI status
	DOUBLE IL,					// ASIO's input latency
	DOUBLE OL,					// ASIO's output latency
	BOOL BUFOVD					// EVBuffer overload
) {
	GetAppName();

	std::locale::global(std::locale::classic());	// DO NOT REMOVE

	std::string PipeContent;
	DWORD bytesWritten;								// Needed for Windows 7 apparently...

	PipeContent += "OMDebugInfo";
	PipeContent += "\nCurrentApp = ";
	PipeContent += AppPath;
#if defined(_M_AMD64)
	PipeContent += "\nBitApp = 64-bit";
#elif defined(_M_IX86)
	PipeContent += "\nBitApp = 32-bit";
#elif defined(_M_ARM64)
	PipeContent += "\nBitApp = 64-bit (ARM)";
#endif

	ManagedDebugInfo.RenderingTime = CCUI0;

	for (int i = 0; i <= 15; ++i) {
		ManagedDebugInfo.ActiveVoices[i] = cvvalues[i];
		PipeContent += "\nCV" + std::to_string(i) + " = " + std::to_string(cvvalues[i]);
	}

	PipeContent += "\nCurCPU = " + std::to_string(CCUI0);
	PipeContent += "\nHandles = " + std::to_string(HC);
	PipeContent += "\nRAMUsage = " + std::to_string(RUI);
	PipeContent += "\nOMDirect = " + std::to_string(KDMAPIStatus);
	PipeContent += "\nASIOInLat = " + std::to_string(IL);
	PipeContent += "\nASIOOutLat = " + std::to_string(OL);

	/*
	PipeContent += "\nBufferOverload = " + std::to_string(BUFOVD);
	PipeContent += "\nHealthThreadTime = " + std::to_string(GetThreadUsage(&HealthThread));
	PipeContent += "\nATThreadTime = " + std::to_string(GetThreadUsage(&ATThread));
	PipeContent += "\nEPThreadTime = " + std::to_string(GetThreadUsage(&EPThread));
	PipeContent += "\nCookedThreadTime = " + std::to_string(GetThreadUsage(&CookedThread));
	*/

	PipeContent += "\n\0";

	WriteFile(hPipe, PipeContent.c_str(), PipeContent.length(), &bytesWritten, NULL);
	if (hPipe == INVALID_HANDLE_VALUE || (GetLastError() != ERROR_SUCCESS && GetLastError() != ERROR_PIPE_LISTENING)) StartDebugPipe(TRUE);
}

void ParseDebugData() {
	DWORD ASIOTempInLatency, ASIOTempOutLatency;
	DOUBLE ASIORate;

	if (BASSLoadedToMemory && bass_initialized) {
		BASS_ChannelGetAttribute(OMStream, BASS_ATTRIB_CPU, &RenderingTime);

		if (ASIOReady != FALSE && ManagedSettings.CurrentEngine == ASIO_ENGINE) {
			ASIOTempInLatency = BASS_ASIO_GetLatency(TRUE);
			if (BASS_ASIO_ErrorGetCode()) ASIOTempInLatency = 0;

			ASIOTempOutLatency = BASS_ASIO_GetLatency(FALSE);
			if (BASS_ASIO_ErrorGetCode()) ASIOTempOutLatency = 0;

			ASIORate = BASS_ASIO_GetRate();
			if (BASS_ASIO_ErrorGetCode()) ASIORate = 0.0;

			// CheckUpASIO(ERRORCODE, L"OMGetRateASIO", TRUE);
			if (ASIORate != 0.0) {
				ManagedDebugInfo.ASIOInputLatency = (ASIOTempInLatency) ? ((DOUBLE)ASIOTempInLatency * 1000.0 / ASIORate) : 0.0;
				ManagedDebugInfo.ASIOOutputLatency = (ASIOTempOutLatency) ? ((DOUBLE)ASIOTempOutLatency * 1000.0 / ASIORate) : 0.0;
			}
		}
		else ManagedDebugInfo.ASIOOutputLatency = ManagedDebugInfo.ASIOInputLatency = 0.0;

		for (int i = 0; i <= 15; ++i) {
			int temp = BASS_MIDI_StreamGetEvent(OMStream, i, MIDI_EVENT_VOICES);
			if (temp != -1) cvvalues[i] = temp;
		}

		/*
		ManagedDebugInfo.HealthThreadTime = GetThreadUsage(&HealthThread);
		ManagedDebugInfo.ATThreadTime = GetThreadUsage(&ATThread);
		ManagedDebugInfo.EPThreadTime = GetThreadUsage(&EPThread);
		ManagedDebugInfo.CookedThreadTime = GetThreadUsage(&CookedThread);
		*/
	}
	else {
		RenderingTime = 0.0f;
		ManagedDebugInfo.ASIOInputLatency = 0;
		ManagedDebugInfo.ASIOOutputLatency = 0;
		for (int i = 0; i <= 15; ++i) cvvalues[i] = 0;
	}
}

/*
void CheckForInfoFromPipe() {
	wchar_t buffer[NTFS_MAX_PATH];
	DWORD dwRead;

	if (ConnectNamedPipe(hPipe, NULL) != FALSE)
	{
		while (ReadFile(hPipe, buffer, sizeof(buffer) - 1, &dwRead, NULL) != FALSE)
		{
			buffer[dwRead] = '\0';
			
			if (wcsicmp(buffer, L"OMRSTSTREAM")) ManagedSettings.LiveChanges = 1;
		}
	}
}
*/

void SendDebugDataToPipe() {
	try {
		DWORD handlecount;

		PROCESS_MEMORY_COUNTERS_EX pmc;
		GetProcessMemoryInfo(GetCurrentProcess(), (PROCESS_MEMORY_COUNTERS*)&pmc, sizeof(pmc));
		GetProcessHandleCount(GetCurrentProcess(), &handlecount);
		SIZE_T ramusage = pmc.WorkingSetSize;
		QWORD ramusageint = static_cast<QWORD>(ramusage);

		long long TimeDuringDebug = HyperMode ? 0 : TimeNow();

		FillContentDebug(RenderingTime, handlecount, static_cast<QWORD>(pmc.WorkingSetSize), KDMAPIEnabled,
			ManagedDebugInfo.ASIOInputLatency, ManagedDebugInfo.ASIOOutputLatency, FALSE /* It's supposed to be a buffer overload check */);

		FlushFileBuffers(hPipe);
	}
	catch (...) {
		CrashMessage("DebugPipePush");
	}
}

void SendDummyDataToPipe() {
	try {
		FillContentDebug(0.0f, 0, 0, KDMAPIEnabled, 0.0, 0.0, FALSE);
		FlushFileBuffers(hPipe);
	}
	catch (...) {
		CrashMessage("DebugPipeDummyPush");
	}
}

void MixerCheck() {
	wchar_t TempCh[MAXPNAMELEN] = { 0 };
	wchar_t TempPs[MAXPNAMELEN] = { 0 };

	try {
		OpenRegistryKey(Channels, L"Software\\OmniMIDI\\Channels", TRUE);
		for (int i = 0; i <= 15; ++i) 
		{
			swprintf_s(TempCh, MAXPNAMELEN, L"ch%d", i + 1);
			swprintf_s(TempPs, MAXPNAMELEN, L"ch%upshift\0", i + 1);

			RegQueryValueEx(Channels.Address, TempCh, NULL, &dwType, (LPBYTE)&cvalues[i], &dwSize);
			RegQueryValueEx(Channels.Address, TempPs, NULL, &dwType, (LPBYTE)&pitchshiftchan[i], &dwSize);

			BASS_MIDI_StreamEvent(OMStream, i, MIDI_EVENT_MIXLEVEL, cvalues[i]);
		}
	}
	catch (...) {
		CrashMessage("MixerCheck");
	}
}

void RevbNChor() {
	try {
		BOOL RCOverride = FALSE;
		OpenRegistryKey(Configuration, L"Software\\OmniMIDI\\Configuration", TRUE);

		RegQueryValueEx(Configuration.Address, L"RCOverride", NULL, &dwType, (LPBYTE)&RCOverride, &dwSize);
		RegQueryValueEx(Configuration.Address, L"Reverb", NULL, &dwType, (LPBYTE)&reverb, &dwSize);
		RegQueryValueEx(Configuration.Address, L"Chorus", NULL, &dwType, (LPBYTE)&chorus, &dwSize);

		if (RCOverride) {
			for (int i = 0; i <= 15; ++i) {
				BASS_MIDI_StreamEvent(OMStream, i, MIDI_EVENT_REVERB, reverb);
				BASS_MIDI_StreamEvent(OMStream, i, MIDI_EVENT_CHORUS, chorus);
			}
		}
	}
	catch (...) {
		CrashMessage("ReverbAndChorusCheck");
	}
}

void ReloadSFList(DWORD whichsflist){
	try {
		ResetSynth(FALSE);
		LoadSoundfont(whichsflist);
	}
	catch (...) {
		CrashMessage("ReloadListCheck");
	}
}

void keybindings()
{
	try {
		if (ManagedSettings.FastHotkeys == 1) {
			BOOL ControlPressed = (GetAsyncKeyState(VK_CONTROL) & (1 << 15));
			if (!ControlPressed && GetAsyncKeyState(VK_MENU) & GetAsyncKeyState(0x31) & 0x8000) {
				ReloadSFList(0);
				return;
			}
			else if (!ControlPressed && GetAsyncKeyState(VK_MENU) & GetAsyncKeyState(0x32) & 0x8000) {
				ReloadSFList(1);
				return;
			}
			else if (!ControlPressed && GetAsyncKeyState(VK_MENU) & GetAsyncKeyState(0x33) & 0x8000) {
				ReloadSFList(2);
				return;
			}
			else if (!ControlPressed && GetAsyncKeyState(VK_MENU) & GetAsyncKeyState(0x34) & 0x8000) {
				ReloadSFList(3);
				return;
			}
			else if (!ControlPressed && GetAsyncKeyState(VK_MENU) & GetAsyncKeyState(0x35) & 0x8000) {
				ReloadSFList(4);
				return;
			}
			else if (!ControlPressed && GetAsyncKeyState(VK_MENU) & GetAsyncKeyState(0x36) & 0x8000) {
				ReloadSFList(5);
				return;
			}
			else if (!ControlPressed && GetAsyncKeyState(VK_MENU) & GetAsyncKeyState(0x37) & 0x8000) {
				ReloadSFList(6);
				return;
			}
			else if (!ControlPressed && GetAsyncKeyState(VK_MENU) & GetAsyncKeyState(0x38) & 0x8000) {
				ReloadSFList(7);
				return;
			}
			if (ManagedSettings.Extra8Lists == 1) {
				if (ControlPressed & GetAsyncKeyState(VK_MENU) & GetAsyncKeyState(0x31) & 0x8000) {
					ReloadSFList(8);
					return;
				}
				else if (ControlPressed & GetAsyncKeyState(VK_MENU) & GetAsyncKeyState(0x32) & 0x8000) {
					ReloadSFList(9);
					return;
				}
				else if (ControlPressed & GetAsyncKeyState(VK_MENU) & GetAsyncKeyState(0x33) & 0x8000) {
					ReloadSFList(10);
					return;
				}
				else if (ControlPressed & GetAsyncKeyState(VK_MENU) & GetAsyncKeyState(0x34) & 0x8000) {
					ReloadSFList(11);
					return;
				}
				else if (ControlPressed & GetAsyncKeyState(VK_MENU) & GetAsyncKeyState(0x35) & 0x8000) {
					ReloadSFList(12);
					return;
				}
				else if (ControlPressed & GetAsyncKeyState(VK_MENU) & GetAsyncKeyState(0x36) & 0x8000) {
					ReloadSFList(13);
					return;
				}
				else if (ControlPressed & GetAsyncKeyState(VK_MENU) & GetAsyncKeyState(0x37) & 0x8000) {
					ReloadSFList(14);
					return;
				}
				else if (ControlPressed & GetAsyncKeyState(VK_MENU) & GetAsyncKeyState(0x38) & 0x8000) {
					ReloadSFList(15);
					return;
				}
			}

			TCHAR configuratorapp[MAX_PATH];
			if (GetAsyncKeyState(VK_MENU) & GetAsyncKeyState(0x39) & 0x8000) {
				if (ManagedSettings.CurrentEngine == ASIO_ENGINE) {
					if (BASSLoadedToMemory && bass_initialized) BASS_ASIO_ControlPanel();
				}
				else {

					if (SUCCEEDED(SHGetFolderPath(NULL, CSIDL_SYSTEMX86, NULL, 0, configuratorapp)))
					{
						PathAppend(configuratorapp, _T("\\OmniMIDI\\OmniMIDIMixerWindow.exe"));
						ShellExecute(NULL, L"open", configuratorapp, NULL, NULL, SW_SHOWNORMAL);
						Sleep(10);
						return;
					}
				}
			}
			else if (GetAsyncKeyState(VK_MENU) & GetAsyncKeyState(0x30) & 0x8000) {
				if (SUCCEEDED(SHGetFolderPath(NULL, CSIDL_SYSTEMX86, NULL, 0, configuratorapp)))
				{
					PathAppend(configuratorapp, _T("\\OmniMIDI\\OmniMIDIDebugWindow.exe"));
					ShellExecute(NULL, L"open", configuratorapp, NULL, NULL, SW_SHOWNORMAL);
					Sleep(10);
				}
				return;
			}
			if (GetAsyncKeyState(VK_INSERT) & 1) {
				ResetSynth(FALSE);
			}
		}
	}
	catch (...) {
		CrashMessage("HotKeysCheck");
	}
}