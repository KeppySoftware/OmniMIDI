/*
OmniMIDI settings loading system
*/
#pragma once

void CheckIfAppIsAllowedToUseOSD() {
	// OSD system init
	std::wstring OSDDir;

	wchar_t UserProfile[MAX_PATH] = { 0 };
	wchar_t TempString[NTFS_MAX_PATH] = { 0 };

	// Start the system
	SHGetFolderPathW(NULL, CSIDL_PROFILE, NULL, 0, UserProfile);

	OSDDir.append(UserProfile);
	OSDDir.append(_T("\\OmniMIDI\\lists\\OmniMIDI.osdlist"));

	try {
		if (PathFileExistsW(OSDDir.c_str())) {
			std::wifstream file(OSDDir.c_str());

			if (file) {
				file.imbue(UTF8Support);

				while (file.getline(TempString, sizeof(TempString) / sizeof(*TempString)))
				{
					if (_wcsicmp(AppNameW, TempString) == 0) CanUseOSD = TRUE;
				}
			}
		}
	}
	catch (...) {
		CrashMessage("OSDCheckUp");
	}
}

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

BOOL CloseThread(Thread* thread) {
	// Wait for the thread to finish its job
	if (thread->ThreadHandle) {
		PrintMessageToDebugLog("CloseThread", "Waiting for passed thread to finish...");
		WaitForSingleObject(thread->ThreadHandle, INFINITE);

		// And mark it as NULL
		PrintMessageToDebugLog("CloseThread", "Cleaning up...");
		CloseHandle(thread->ThreadHandle);
		thread->ThreadHandle = NULL;
		thread->ThreadAddress = NULL;

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

BOOL LoadSoundfont(int whichsf) {
	BOOL RET = false;
	DWORD CurrentList = (whichsf + 1);

	if (!SHGetFolderPathW(NULL, whichsf ? CSIDL_PROFILE : CSIDL_APPDATA, NULL, SHGFP_TYPE_CURRENT, ListToLoad)) {
		PrintMessageToDebugLog("LoadSoundFontFunc", "Loading soundfont list...");

		OpenRegistryKey(SFDynamicLoader, L"Software\\OmniMIDI\\Watchdog", TRUE);
		RegSetValueExW(SFDynamicLoader.Address, L"currentsflist", 0, REG_DWORD, (LPBYTE)& CurrentList, sizeof(CurrentList));
		
		if (!whichsf) swprintf_s(ListToLoad + wcslen(ListToLoad), NTFS_MAX_PATH, CSFFileTemplate);
		else swprintf_s(ListToLoad + wcslen(ListToLoad), NTFS_MAX_PATH, OMFileTemplate, L"lists", OMLetters[whichsf], L"omlist");
		
		RET = FontLoader(ListToLoad);
		if (RET) PrintMessageToDebugLog("LoadSoundFontFunc", "Done!");
	}

	return RET;
}

bool LoadSoundfontStartup() {
	wchar_t CurrentAppList[NTFS_MAX_PATH] = { 0 };
	wchar_t CurrentString[NTFS_MAX_PATH] = { 0 };

	if (!SHGetFolderPath(NULL, CSIDL_PROFILE, NULL, 0, CurrentAppList)) {
		for (int i = 0; i < 15; ++i) {
			swprintf_s(CurrentAppList + wcslen(CurrentAppList), NTFS_MAX_PATH, OMFileTemplate, L"applists", OMLetters[i], L"applist");

			std::wifstream AppList(CurrentAppList);
			if (AppList) {
				AppList.imbue(UTF8Support);
				while (AppList.getline(CurrentString, sizeof(CurrentString) / sizeof(*CurrentString)))
				{
					if (!_wcsicmp(AppNameW, CurrentString) && !_wcsicmp(AppPathW, CurrentString)) {
						PrintMessageToDebugLog("LoadSoundfontStartup", "Found list. Loading...");
						LoadSoundfont(i + 1);
						return TRUE;
					}
				}
			}
		}

		PrintMessageToDebugLog("LoadSoundfontStartup", "No default startup list found. Continuing...");
		return FALSE;
	}
}

void LoadDriverModule(HMODULE * Target, LPWSTR RequestedLib, BOOL Mandatory) {
	wchar_t InstallPath[MAX_PATH] = { 0 };
	wchar_t DLLPath[MAX_PATH] = { 0 };

	if (!(*Target = LoadLibrary(RequestedLib))) {
		PrintLoadedDLLToDebugLog(RequestedLib, "No library has been found in memory. The driver will now load the DLL...");
		if (GetModuleFileName(hinst, InstallPath, MAX_PATH))
		{
			PathRemoveFileSpec(InstallPath);
			swprintf_s(DLLPath, MAX_PATH, L"%s\\%s", InstallPath, RequestedLib);
			if (!(*Target = LoadLibraryEx(DLLPath, NULL, 0))) {
				if (Mandatory) {
					DLLLoadError(DLLPath);
					exit(0);
				}
				else PrintLoadedDLLToDebugLog(DLLPath, "Failed to load requested library. It probably requires some missing dependencies.");
			}
			else PrintLoadedDLLToDebugLog(RequestedLib, "The library is now in memory.");
		}
	}
	else PrintLoadedDLLToDebugLog(RequestedLib, "The library is already in memory. The HMODULE will be a pointer to that address.");
}

void LoadPluginModule(HPLUGIN* Target, LPWSTR RequestedLib, BOOL Mandatory) {
	wchar_t InstallPath[MAX_PATH] = { 0 };
	wchar_t DLLPath[MAX_PATH] = { 0 };

	if (!(*Target)) {
		PrintLoadedDLLToDebugLog(RequestedLib, "No plugin has been found in memory. The driver will now load the DLL...");
		if (GetModuleFileName(hinst, InstallPath, MAX_PATH))
		{
			PathRemoveFileSpec(InstallPath);
			swprintf_s(DLLPath, MAX_PATH, L"%s\\%s", InstallPath, RequestedLib);
			*Target = BASS_PluginLoad((char*)&DLLPath, BASS_UNICODE);
			if (BASS_ErrorGetCode() != 0) {
				if (Mandatory) {
					DLLLoadError(DLLPath);
					exit(0);
				}
				else PrintLoadedDLLToDebugLog(DLLPath, "Failed to load requested plugin. It probably requires some missing dependencies or isn't supported by this version of BASS.");
			}
			else PrintLoadedDLLToDebugLog(RequestedLib, "The plugin is now in memory.");
		}
	}
	else PrintLoadedDLLToDebugLog(RequestedLib, "The plugin is already in memory. The HPLUGIN will be a pointer to that address.");
}

VOID LoadBASSFunctions()
{
	try {
		if (!BASSLoadedToMemory) {
			PrintMessageToDebugLog("ImportBASS", "Importing BASS DLLs to memory...");

			// Load modules
			LoadDriverModule(&bass, L"bass.dll", TRUE);
			LoadDriverModule(&bassmidi, L"bassmidi.dll", TRUE);
			LoadDriverModule(&bassenc, L"bassenc.dll", TRUE);
			LoadDriverModule(&bassasio, L"bassasio.dll", TRUE);

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
			LOADLIBFUNCTION(bass, BASS_PluginFree);
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

			_BMSE = BASS_MIDI_StreamEvent;

			// Load plugins
			LoadPluginModule(&bassflac, L"bassflac.dll", TRUE);

			PrintMessageToDebugLog("ImportBASS", "Function pointers loaded into memory.");
		}
		else PrintMessageToDebugLog("ImportBASS", "BASS has been already loaded by the driver.");

		PrintMessageToDebugLog("ImportBASS", "Setting BASS flag to true.");
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
			_BMSE = DummyBMSE;
			BASS_PluginFree(bassflac);
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
		if (restart) FreeUpMemory();

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
		PrintMemoryMessageToDebugLog("AllocateMemoryFunc", "Total RAM available (in bytes)", FALSE, status.ullTotalPhys);

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

void LoadSettings(BOOL Restart, BOOL RT)
{	
	// Initialize the temp values
	DWORD
		TempSC = ManagedSettings.SincConv,
		TempOV = ManagedSettings.OutputVolume,
		TempHP = HyperMode,
		TempMV = ManagedSettings.MaxVoices;

	BOOL
		TempESFX = ManagedSettings.EnableSFX,
		TempNOFF1 = ManagedSettings.NoteOff1,
		TempISR = ManagedSettings.IgnoreSysReset,
		TempSI = ManagedSettings.SincInter,
		TempDNFO = ManagedSettings.DisableNotesFadeOut,
		TempDMN = ManagedSettings.DontMissNotes;

	DWORD64
		TEvBufferSize = EvBufferSize,
		TEvBufferMultRatio = EvBufferMultRatio;

	try {
		// If the driver is booting up, then return true
		BOOL IsBootUp = (!RT && !Restart);
		if (!RT) PrintMessageToDebugLog("LoadSettingsFuncs", "Loading settings from registry...");

		// Load the settings from the registry
		OpenRegistryKey(Configuration, L"Software\\OmniMIDI\\Configuration", TRUE);

		// These settings should NOT be loaded in real-time
		if (!RT) {
			RegQueryValueEx(Configuration.Address, L"AudioBitDepth", NULL, &dwType, (LPBYTE)&ManagedSettings.AudioBitDepth, &dwSize);
			RegQueryValueEx(Configuration.Address, L"AudioFrequency", NULL, &dwType, (LPBYTE)&ManagedSettings.AudioFrequency, &dwSize);
			RegQueryValueEx(Configuration.Address, L"AudioOutput", NULL, &dwType, (LPBYTE)&ManagedSettings.AudioOutputReg, &dwSize);
			RegQueryValueEx(Configuration.Address, L"CurrentEngine", NULL, &dwType, (LPBYTE)&ManagedSettings.CurrentEngine, &dwSize);
			RegQueryValueEx(Configuration.Address, L"DisableChime", NULL, &dwType, (LPBYTE)&DisableChime, &dwSize);
			RegQueryValueEx(Configuration.Address, L"DriverPriority", NULL, &dwType, (LPBYTE)&ManagedSettings.DriverPriority, &dwSize);
			RegQueryValueEx(Configuration.Address, L"EvBufferMultRatio", NULL, &dwType, (LPBYTE)&TEvBufferMultRatio, &dwSize);
			RegQueryValueEx(Configuration.Address, L"EvBufferSize", NULL, &qwType, (LPBYTE)&TEvBufferSize, &qwSize);
			RegQueryValueEx(Configuration.Address, L"Extra8Lists", NULL, &dwType, (LPBYTE)&ManagedSettings.Extra8Lists, &dwSize);
			RegQueryValueEx(Configuration.Address, L"FollowDefaultAudioDevice", NULL, &dwType, (LPBYTE)&ManagedSettings.FollowDefaultAudioDevice, &dwSize);
			RegQueryValueEx(Configuration.Address, L"GetEvBuffSizeFromRAM", NULL, &dwType, (LPBYTE)&GetEvBuffSizeFromRAM, &dwSize);
			RegQueryValueEx(Configuration.Address, L"LiveChanges", NULL, &dwType, (LPBYTE)&ManagedSettings.LiveChanges, &dwSize);
			RegQueryValueEx(Configuration.Address, L"MonoRendering", NULL, &dwType, (LPBYTE)&ManagedSettings.MonoRendering, &dwSize);
			RegQueryValueEx(Configuration.Address, L"VolumeMonitor", NULL, &dwType, (LPBYTE)&ManagedSettings.VolumeMonitor, &dwSize);
		}

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

		if (!RT) RegSetValueEx(Configuration.Address, L"LiveChanges", 0, REG_DWORD, (LPBYTE)& Blank, sizeof(Blank));

		// Volume
		if (TempOV != ManagedSettings.OutputVolume || SettingsManagedByClient) {
			if (!SettingsManagedByClient) ManagedSettings.OutputVolume = TempOV;
			SynthVolume = (float)ManagedSettings.OutputVolume / 10000.0f;

			if (RT) {
				ChVolumeStruct.fCurrent = 1.0f;
				ChVolumeStruct.fTarget = SynthVolume;
				ChVolumeStruct.fTime = 0.0f;
				ChVolumeStruct.lCurve = 0;
				BASS_FXSetParameters(ChVolume, &ChVolumeStruct);
				CheckUp(FALSE, ERRORCODE, "Stream Volume FX Set", FALSE);
			}
		}

		// Check if the value is different from the temporary one
		if (TempDMN != ManagedSettings.DontMissNotes || SettingsManagedByClient) {
			// It is different, reset the synth
			// to avoid stuck notes or crashes
			if (!SettingsManagedByClient) ManagedSettings.DontMissNotes = TempDMN;
			if (RT) ResetSynth(TRUE);
		}

		if (IsBootUp && (HyperMode && !DisableChime)) {
			// It's enabled, do some beeps to notify the user (If the chime is enabled)
			Beep(440, 100);
			Beep(687, 100);
		}

		// Check if the value is different from the temporary one
		if (IsBootUp || TempHP != HyperMode || SettingsManagedByClient) {
			if (!SettingsManagedByClient) HyperMode = TempHP;

			// Close the threads for safety reasons
			if (RT) stop_thread = TRUE;

			// Check if "Hyper-playback" mode has been enabled
			_PrsData = HyperMode ? ParseDataHyper : ParseData;
			_PforBASSMIDI = HyperMode ? PrepareForBASSMIDIHyper : PrepareForBASSMIDI;
			_PlayBufData = HyperMode ? PlayBufferedDataHyper : PlayBufferedData;
			_PlayBufDataChk = HyperMode ? PlayBufferedDataChunkHyper : PlayBufferedDataChunk;

			// Restart threads
			if (RT) stop_thread = FALSE;
		}

		if (IsBootUp || (TEvBufferSize != EvBufferSize || TEvBufferMultRatio != EvBufferMultRatio)) {
			EvBufferSize = TEvBufferSize;
			EvBufferMultRatio = TEvBufferMultRatio;
			AllocateMemory(Restart);
		}

		// Load the settings by comparing the temporary values to the driver's ones, to prevent overhead
		if (TempESFX != ManagedSettings.EnableSFX || SettingsManagedByClient) {
			if (!SettingsManagedByClient) ManagedSettings.EnableSFX = TempESFX;
			if (RT) BASS_ChannelFlags(OMStream, ManagedSettings.EnableSFX ? 0 : BASS_MIDI_NOFX, BASS_MIDI_NOFX);
		}

		if (TempNOFF1 != ManagedSettings.NoteOff1 || SettingsManagedByClient) {
			if (!SettingsManagedByClient) ManagedSettings.NoteOff1 = TempNOFF1;
			if (RT) BASS_ChannelFlags(OMStream, ManagedSettings.NoteOff1 ? BASS_MIDI_NOTEOFF1 : 0, BASS_MIDI_NOTEOFF1);
		}

		if (TempISR != ManagedSettings.IgnoreSysReset || SettingsManagedByClient) {
			if (!SettingsManagedByClient) ManagedSettings.IgnoreSysReset = TempNOFF1;
			BASS_ChannelFlags(OMStream, ManagedSettings.IgnoreSysReset ? BASS_MIDI_NOSYSRESET : 0, BASS_MIDI_NOSYSRESET);
		}

		if (TempSI != ManagedSettings.SincInter || TempSC != ManagedSettings.SincConv || SettingsManagedByClient) {
			if (!SettingsManagedByClient) {
				ManagedSettings.SincInter = TempSI;
				ManagedSettings.SincConv = TempSC;
			}

			if (RT) {
				BASS_ChannelFlags(OMStream, ManagedSettings.SincInter ? BASS_MIDI_SINCINTER : 0, BASS_MIDI_SINCINTER);
				BASS_ChannelSetAttribute(OMStream, BASS_ATTRIB_SRC, ManagedSettings.SincConv);
			}
		}

		if (TempDNFO != ManagedSettings.DisableNotesFadeOut || SettingsManagedByClient) {
			if (!SettingsManagedByClient) ManagedSettings.DisableNotesFadeOut = TempDNFO;
			if (RT) BASS_ChannelSetAttribute(OMStream, BASS_ATTRIB_MIDI_KILL, ManagedSettings.DisableNotesFadeOut);
		}

		if (TempMV != ManagedSettings.MaxVoices || SettingsManagedByClient) {
			if (!SettingsManagedByClient) ManagedSettings.MaxVoices = TempMV;
			if (RT) BASS_ChannelSetAttribute(OMStream, BASS_ATTRIB_MIDI_VOICES, ManagedSettings.MaxVoices);
		}

		if (RT) BASS_ChannelSetAttribute(OMStream, BASS_ATTRIB_MIDI_CHANS, UnlimitedChannels ? 128.0f : 16.0f);

		if (!RT) PrintMessageToDebugLog("LoadSettingsFuncs", "Settings loaded.");
	}
	catch (...) {
		CrashMessage("LoadSettings");
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
	switch (ManagedSettings.CurrentEngine) {
	case ASIO_ENGINE:
		return IsItStreamCreation ? BASS_SAMPLE_FLOAT : BASS_DATA_FLOAT;
	default:
		if (RegistryVal == 2 || RegistryVal == 0)
			return IsItStreamCreation ? 0 : BASS_DATA_FIXED;
		else if (RegistryVal == 3)
			return IsItStreamCreation ? BASS_SAMPLE_8BITS : BASS_DATA_FIXED;
		else
			return IsItStreamCreation ? BASS_SAMPLE_FLOAT : BASS_DATA_FLOAT;
	}
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
				float levels[2] = { -0.1f, -0.1f };
				DWORD left = 0, right = 0;

				switch (ManagedSettings.CurrentEngine) {
				case WASAPI_ENGINE:
				case DXAUDIO_ENGINE:
					BASS_ChannelGetLevelEx(OMStream, levels, (ManagedSettings.MonoRendering ? 0.01f : 0.02f), (ManagedSettings.MonoRendering ? BASS_LEVEL_MONO : BASS_LEVEL_STEREO));
					break;
				case ASIO_ENGINE:
					levels[0] = BASS_ASIO_ChannelGetLevel(FALSE, 0);
					levels[1] = BASS_ASIO_ChannelGetLevel(FALSE, 1);
					break;
				default:
					break;
				}

				if (levels[0] > -0.1f && levels[1] > -0.1f) {
					DWORD level = MAKELONG((WORD)(min(levels[0], 1) * 32768), (WORD)(min(levels[1], 1) * 32768));
					left = LOWORD(level);	// the left level
					right = HIWORD(level);	// the right level
				}

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

template <typename T>
std::string draw_number(const T a_value, int prec, DWORD compar, LPCSTR c1, LPCSTR c2)
{
	std::ostringstream out;
	out.precision(prec);

	if (a_value > compar)
		out << c1;
	else
		out << c2;

	out << std::fixed << a_value;
	return out.str();
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

	// For debug window
	std::wstring PipeContent;
	DWORD bytesWritten;								// Needed for Windows 7 apparently...

	PipeContent.append(L"OMDebugInfo");
	PipeContent.append(L"|CurrentApp = ");
	PipeContent.append(AppPathW);
#if defined(_M_AMD64)
	PipeContent.append(L"|BitApp = 64-bit");
#elif defined(_M_IX86)
	PipeContent.append(L"|BitApp = 32-bit");
#elif defined(_M_ARM64)
	PipeContent.append(L"|BitApp = 64-bit (ARM)");
#endif

	ManagedDebugInfo.RenderingTime = CCUI0;

	for (int i = 0; i <= 15; ++i) 
		PipeContent.append(L"|CV" + std::to_wstring(i) + L" = " + std::to_wstring(ManagedDebugInfo.ActiveVoices[i]));

	PipeContent.append(L"|CurCPU = " + std::to_wstring(CCUI0));
	PipeContent.append(L"|Handles = " + std::to_wstring(HC));
	PipeContent.append(L"|RAMUsage = " + std::to_wstring(RUI));
	PipeContent.append(L"|OMDirect = " + std::to_wstring(KDMAPIStatus));
	PipeContent.append(L"|ASIOInLat = " + std::to_wstring(IL));
	PipeContent.append(L"|ASIOOutLat = " + std::to_wstring(OL));

	/*
	PipeContent += L"|BufferOverload = " + std::to_wstring(BUFOVD);
	PipeContent += L"|HealthThreadTime = " + std::to_wstring(GetThreadUsage(&HealthThread));
	PipeContent += L"|ATThreadTime = " + std::to_wstring(GetThreadUsage(&ATThread));
	PipeContent += L"|EPThreadTime = " + std::to_wstring(GetThreadUsage(&EPThread));
	PipeContent += L"|CookedThreadTime = " + std::to_wstring(GetThreadUsage(&CookedThread));
	*/

	PipeContent.append(L"\n\0");

	const WCHAR* PCW = PipeContent.c_str();
	WriteFile(hPipe, (LPVOID)PCW, wcslen(PCW) * sizeof(wchar_t), &bytesWritten, NULL);
	if (hPipe == INVALID_HANDLE_VALUE || (GetLastError() != ERROR_SUCCESS && GetLastError() != ERROR_PIPE_LISTENING)) StartDebugPipe(TRUE);

	// Check if RTSS OSD is available
	if (IsOSDAvailable()) {
		// It is, go push some data fam
		std::string RTSSContent;

		DWORD ActiveVoices = 0;
		for (int i = 0; i <= 15; ++i)
			ActiveVoices += ManagedDebugInfo.ActiveVoices[i];

		RTSSContent += "<A0=-5><A1=4><C0=FFA0A0><C1=FF0000><C2=FFFFFF><C3=33FF33><C4=FF3333><C5=FFFF00><S0=-50><S1=50>";
		RTSSContent += "<C0>Rendering time:<S>	 <A0>" + draw_number(CCUI0, 1, ManagedSettings.MaxRenderingTime, "<C1>", "<C>") + "%<A>\n";
		RTSSContent += "<C0>Active voices:<S>	 <A0>" + draw_number(ActiveVoices, 0, ManagedSettings.MaxVoices, "<C1>", "<C>") + "<A>\n";
		RTSSContent += "<C0>Init mode:<S><C>	 <A0>";
		if (KDMAPIStatus) {
			if (IsKDMAPIViaWinMM)
				RTSSContent += "<C5>WinMMWRP\n<A>\n";
			else
				RTSSContent += "<C3>KDMAPI\n<A>\n";
		} 
		else RTSSContent += "<C4>WinMM\n<A>\n";
		RTSSContent += "<C2>Framerate (<APP>):<C>	 <A0><FR><A>";

		// Send the data to RTSS
		UpdateOSD(RTSSContent.c_str());
	}
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
			if (temp != -1) ManagedDebugInfo.ActiveVoices[i] = temp;
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
		for (int i = 0; i <= 15; ++i) ManagedDebugInfo.ActiveVoices[i] = 0;
	}
}

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

void MixerCheck() {
	wchar_t TempCh[MAXPNAMELEN] = { 0 };
	wchar_t TempPs[MAXPNAMELEN] = { 0 };

	try {
		OpenRegistryKey(Channels, L"Software\\OmniMIDI\\Channels", TRUE);
		for (int i = 0; i <= 15; ++i) 
		{
			swprintf_s(TempCh, MAXPNAMELEN, L"ch%d", i + 1);
			swprintf_s(TempPs, MAXPNAMELEN, L"ch%dpshift\0", i + 1);

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

void KeyShortcuts()
{
	bool ControlPressed;
	bool Keys[256];
	wchar_t OMConfiguratorDir[MAX_PATH];

	try 
	{
		if (ManagedSettings.FastHotkeys == 1) 
		{
			// Check if CONTROL is pressed together with ALT
			ControlPressed = (GetAsyncKeyState(VK_CONTROL) & (1 << 15));

			// Get all keys pressed at the time
			for (int i = 0; i < 256; i++)
				Keys[i] = GetAsyncKeyState(i);

			// Hotkey ALT
			if (Keys[VK_MENU])
			{
				// ALT + 1
				if (Keys[0x31])
				{
					ReloadSFList((ControlPressed && ManagedSettings.Extra8Lists) ? 8 : 0);
					return;
				}

				// ALT + 2
				if (Keys[0x32]) 
				{
					ReloadSFList((ControlPressed && ManagedSettings.Extra8Lists) ? 9 : 1);
					return;
				}

				// ALT + 3
				if (Keys[0x33]) 
				{
					ReloadSFList((ControlPressed && ManagedSettings.Extra8Lists) ? 10 : 2);
					return;
				}

				// ALT + 4
				if (Keys[0x34]) 
				{
					ReloadSFList((ControlPressed && ManagedSettings.Extra8Lists) ? 11 : 3);
					return;
				}

				// ALT + 5
				if (Keys[0x35])
				{
					ReloadSFList((ControlPressed && ManagedSettings.Extra8Lists) ? 12 : 4);
					return;
				}

				// ALT + 6
				if (Keys[0x36])
				{
					ReloadSFList((ControlPressed && ManagedSettings.Extra8Lists) ? 13 : 5);
					return;
				}

				// ALT + 7
				if (Keys[0x37])
				{
					ReloadSFList((ControlPressed && ManagedSettings.Extra8Lists) ? 14 : 6);
					return;
				}

				// ALT + 8
				if (Keys[0x38])
				{
					ReloadSFList((ControlPressed && ManagedSettings.Extra8Lists) ? 15 : 7);
					return;
				}

				// ALT + 9
				if (Keys[0x39])
				{
					if (ManagedSettings.CurrentEngine == ASIO_ENGINE) {
						if (BASSLoadedToMemory && bass_initialized) BASS_ASIO_ControlPanel();
					}
					else {
						if (SUCCEEDED(SHGetFolderPath(NULL, CSIDL_SYSTEMX86, NULL, 0, OMConfiguratorDir)))
						{
							PathAppend(OMConfiguratorDir, _T("\\OmniMIDI\\OmniMIDIMixerWindow.exe"));
							ShellExecute(NULL, L"open", OMConfiguratorDir, NULL, NULL, SW_SHOWNORMAL);
							Sleep(10);
						}
					}
					return;
				}

				// ALT + 0
				if (Keys[0x30])
				{
					if (SUCCEEDED(SHGetFolderPath(NULL, CSIDL_SYSTEMX86, NULL, 0, OMConfiguratorDir)))
					{
						PathAppend(OMConfiguratorDir, _T("\\OmniMIDI\\OmniMIDIDebugWindow.exe"));
						ShellExecute(NULL, L"open", OMConfiguratorDir, NULL, NULL, SW_SHOWNORMAL);
						Sleep(10);
					}
					return;
				}
			}

			// INSERT
			if (Keys[VK_INSERT])
			{
				ResetSynth(FALSE);
				return;
			}
		}
	}
	catch (...) {
		CrashMessage("HotKeysCheck");
	}
}