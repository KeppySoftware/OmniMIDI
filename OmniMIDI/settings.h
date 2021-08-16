/*
OmniMIDI settings loading system
*/
#pragma once

void ResetSynth(BOOL SwitchingBufferMode, BOOL ModeReset) {
	if (SwitchingBufferMode) {	
		EVBuffer.ReadHead = 0;
		EVBuffer.WriteHead = 0;
		memset(EVBuffer.Buffer, 0, sizeof(EVBuffer.Buffer));
		PrintMessageToDebugLog("ResetSynth", "EVBuffer has been reset.");
	}

	if (ModeReset) {
		// Wait for the heads to align, to avoid crashes
		if (!BufferCheck()) return;

		BASS_MIDI_StreamEvent(OMStream, 0, MIDI_EVENT_SYSTEMEX, MIDI_SYSTEM_XG);
		PrintMessageToDebugLog("ResetSynth", "Sent SysEx to BASSMIDI.");
	}
	else {
		for (int ch = 0; ch < 16; ch++) {
			BASS_MIDI_StreamEvent(OMStream, ch, MIDI_EVENT_NOTESOFF, NULL);
			BASS_MIDI_StreamEvent(OMStream, ch, MIDI_EVENT_SOUNDOFF, NULL);
		}
		PrintMessageToDebugLog("ResetSynth", "Sent NoteOFFs to all MIDI channels.");
	}
}

void OpenRegistryKey(RegKey &hKey, LPCWSTR hKeyDir, BOOL Mandatory) {
	// If the key isn't ready, open it again
	if (hKey.Status != KEY_READY && !hKey.Address) {
		// Open the key
		hKey.Status = RegOpenKeyEx(HKEY_CURRENT_USER, hKeyDir, 0, KEY_ALL_ACCESS, &hKey.Address);

		// If the key failed to open, throw a crash (If needed)
		if (hKey.Status != KEY_READY && Mandatory) _THROWCRASH;
	}
}

void CloseRegistryKey(RegKey &hKey) {
	if (hKey.Address) {
		// Try to flush the key
		LSTATUS Action = RegFlushKey(hKey.Address);
		// If the key can't be flushed, throw a crash
		if (Action != ERROR_SUCCESS) _THROWCRASH;

		// Try to close the key
		Action = RegCloseKey(hKey.Address);
		// If the key can't be closed, throw a crash
		if (Action != ERROR_SUCCESS) _THROWCRASH;

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
		if (thread->ThreadHandle)
			if (!CloseHandle(thread->ThreadHandle))
				_THROWCRASH;
		thread->ThreadHandle = NULL;
		thread->ThreadAddress = 0;

		PrintMessageToDebugLog("CloseThread", "Thread is down.");
		return TRUE;
	}

	PrintMessageToDebugLog("CloseThread", "The passed thread doesn't exist.");
	return FALSE;
}

void DLLLoadError(LPWSTR DLL, int ErrCode, BOOL Mandatory) {
	TCHAR Error[NTFS_MAX_PATH] = { 0 };

	// Print to log
	if (DebugLog != nullptr) {
		PrintCurrentTime();
		fprintf(DebugLog, "ERROR | Unable to load the following DLL: %s\n", DLL);
	}

	// Show error message
	swprintf_s(Error, L"An error has occurred while loading the following library: %s\n\nClick OK to close the program.", DLL);
	MessageBoxW(NULL, Error, L"OmniMIDI - DLL load error", MB_ICONERROR | MB_SYSTEMMODAL | MB_OK);
	if (Mandatory) exit(ErrCode);
}

long long TimeNow() {
	LARGE_INTEGER now;
	LARGE_INTEGER s_frequency;
	QueryPerformanceCounter(&now);
	QueryPerformanceFrequency(&s_frequency);
	return (1000LL * now.QuadPart) / s_frequency.QuadPart;
}

BOOL LoadSoundfont(int whichsf) {
	BOOL RET = FALSE;
	BOOL V = (!whichsf) ? TRUE : FALSE;
	DWORD CurrentList = (whichsf + 1);

	memset(ListToLoad, 0, sizeof(ListToLoad));

	if (GetFolderPath(V ? FOLDERID_RoamingAppData : FOLDERID_Profile, V ? CSIDL_APPDATA : CSIDL_PROFILE, ListToLoad, sizeof(ListToLoad))) {
		PrintMessageToDebugLog("LoadSoundFontFunc", "Loading soundfont list...");

		if (V) swprintf_s(ListToLoad + wcslen(ListToLoad), NTFS_MAX_PATH, CSFFileTemplate);
		else swprintf_s(ListToLoad + wcslen(ListToLoad), NTFS_MAX_PATH, OMFileTemplate, L"lists", OMLetters[whichsf - 1], L"omlist");
		
		if (PathFileExists(ListToLoad)) {
			PrintMessageWToDebugLog(L"LoadSoundFontFunc", ListToLoad);

			OpenRegistryKey(SFDynamicLoader, L"Software\\OmniMIDI\\Watchdog", TRUE);
			RegSetValueExW(SFDynamicLoader.Address, L"currentsflist", 0, REG_DWORD, (LPBYTE)&CurrentList, sizeof(CurrentList));

			RET = FontLoader(ListToLoad);
		}

		if (RET) {
			ManagedDebugInfo.CurrentSFList = CurrentList;
			PrintMessageToDebugLog("LoadSoundFontFunc", "Done!");
		}
	}

	return RET;
}

bool LoadSoundfontStartup() {
	wchar_t CurrentAppList[NTFS_MAX_PATH] = { 0 };
	wchar_t CurrentString[NTFS_MAX_PATH] = { 0 };

	for (int i = 0; i < 7; ++i) {
		memset(CurrentAppList, 0, sizeof(CurrentAppList));
		if (GetFolderPath(FOLDERID_Profile, CSIDL_PROFILE, CurrentAppList, sizeof(CurrentAppList))) {
			swprintf_s(CurrentAppList + wcslen(CurrentAppList), NTFS_MAX_PATH, OMFileTemplate, L"applists", OMLetters[i], L"applist");

			PrintMessageWToDebugLog(L"LoadSoundfontStartup", CurrentAppList);

			std::wifstream AppList(CurrentAppList);
			if (AppList) {
				AppList.imbue(UTF8Support);
				while (AppList.getline(CurrentString, sizeof(CurrentString) / sizeof(*CurrentString)))
				{
					PrintMessageWToDebugLog(L"LoadSoundfontStartup", CurrentString);

					if (!_wcsicmp(AppNameW, CurrentString) || !_wcsicmp(AppPathW, CurrentString)) {
						PrintMessageToDebugLog("LoadSoundfontStartup", "Found list. Loading...");
						LoadSoundfont(i + 1);
						return TRUE;
					}
				}
			}
		}
		else break;
	}

	PrintMessageToDebugLog("LoadSoundfontStartup", "No default startup list found. Continuing...");
	return FALSE;
}

void LoadDriverModule(OMLib* Target, wchar_t* RequestedLib, BOOL Mandatory, BOOL Opt) {
	HMODULE Temp = NULL;
	wchar_t SysDir[MAX_PATH] = { 0 };
	wchar_t DLLPath[MAX_PATH] = { 0 };
	wchar_t Msg[1024] = { 0 };
	WIN32_FIND_DATA FD = { 0 };

	if (Target->Lib == nullptr) {
		PrintLoadedDLLToDebugLog(RequestedLib, "No library has been found in memory. The driver will now load the DLL...");

		// Check if another DLL is already in memory
		Target->Lib = GetModuleHandle(RequestedLib);
		PrintLoadedDLLToDebugLog(RequestedLib, "Checking through GetModuleHandle...");

		// It is, sigh
		if (Target->Lib != NULL)
		{
			PrintLoadedDLLToDebugLog(RequestedLib, "OmniMIDI will use the app's own library. This could cause issues...");
			Target->AppOwnDLL = TRUE;
			return;
		}
		else Target->AppOwnDLL = FALSE;	

		if (GetFolderPath(FOLDERID_System, NULL, SysDir, sizeof(SysDir))) {
			swprintf_s(DLLPath, MAX_PATH, Opt ? L"%s\\OmniMIDI\\opt\\%s\0" : L"%s\\OmniMIDI\\%s\0", SysDir, RequestedLib);

			if (FindFirstFile(DLLPath, &FD) == INVALID_HANDLE_VALUE)
			{
				PrintLoadedDLLToDebugLog(RequestedLib, "OmniMIDI couldn't find the required library!!!");
				DLLLoadError(DLLPath, ERROR_PATH_NOT_FOUND, Mandatory);
			}
			else {
				if (!(Target->Lib = LoadLibrary(DLLPath))) {
					PrintLoadedDLLToDebugLog(DLLPath, "Failed to load requested library. It's either missing or requires some missing dependencies.");
					DLLLoadError(DLLPath, ERROR_BAD_FORMAT, Mandatory);
				}
				else PrintLoadedDLLToDebugLog(RequestedLib, "The library is now in memory.");
			}
		}
		else DLLLoadError(DLLPath, ERROR_PATH_NOT_FOUND, Mandatory);
	}
	else PrintLoadedDLLToDebugLog(RequestedLib, "The library is already in memory. The HMODULE will be a pointer to that address.");
}

void LoadPluginModule(HPLUGIN* Target, wchar_t* RequestedLib, BOOL Mandatory) {
	wchar_t SysDir[MAX_PATH] = { 0 };
	wchar_t DLLPath[MAX_PATH] = { 0 };

	if (!(*Target)) {
		PrintLoadedDLLToDebugLog(RequestedLib, "No plugin has been found in memory. The driver will now load the DLL...");

		if (GetFolderPath(FOLDERID_System, NULL, SysDir, sizeof(SysDir))) {
			swprintf_s(DLLPath, MAX_PATH, L"%s\\OmniMIDI\\%s", SysDir, RequestedLib);

			(*Target) = BASS_PluginLoad((char*)&DLLPath, BASS_UNICODE);
			if (BASS_ErrorGetCode() != 0) {
				if (Mandatory) DLLLoadError(DLLPath, ERROR_BAD_FORMAT, Mandatory);
				else PrintLoadedDLLToDebugLog(DLLPath, "Failed to load requested plugin. It's either missing, requires some missing dependencies or isn't supported by this version of BASS.");
			}
			else PrintLoadedDLLToDebugLog(RequestedLib, "The plugin is now in memory.");
		}
		else DLLLoadError(DLLPath, ERROR_PATH_NOT_FOUND, Mandatory);
	}
	else PrintLoadedDLLToDebugLog(RequestedLib, "The plugin is already in memory. The HPLUGIN will be a pointer to that address.");
}

BOOL LoadBASSFunctions()
{
	try {
		if (!BASSLoadedToMemory) {
			PrintMessageToDebugLog("ImportBASS", "Importing BASS DLLs to memory...");

			// Load modules
			// RegQueryValueEx(Configuration.Address, L"FastLibs", NULL, &dwType, (LPBYTE)&ManagedSettings.FastLibs, &dwSize);

			LoadDriverModule(&BASS, L"bass.dll", TRUE, FALSE);
			LoadDriverModule(&BASSMIDI, L"bassmidi.dll", TRUE, FALSE);
			LoadDriverModule(&BASSASIO, L"bassasio.dll", TRUE, FALSE);
			LoadDriverModule(&BASSWASAPI, L"basswasapi.dll", TRUE, FALSE);
			LoadDriverModule(&BASSENC, L"bassenc.dll", TRUE, FALSE);

			PrintMessageToDebugLog("ImportBASS", "DLLs loaded into memory. Importing functions...");

			// Load all the functions into memory
			LOADLIBFUNCTION(BASSASIO.Lib, BASS_ASIO_CheckRate);
			LOADLIBFUNCTION(BASSASIO.Lib, BASS_ASIO_ChannelEnable);
			LOADLIBFUNCTION(BASSASIO.Lib, BASS_ASIO_ChannelEnableBASS);
			LOADLIBFUNCTION(BASSASIO.Lib, BASS_ASIO_ChannelEnableMirror);
			LOADLIBFUNCTION(BASSASIO.Lib, BASS_ASIO_ChannelGetLevel);
			LOADLIBFUNCTION(BASSASIO.Lib, BASS_ASIO_ChannelJoin);
			LOADLIBFUNCTION(BASSASIO.Lib, BASS_ASIO_ChannelSetFormat);
			LOADLIBFUNCTION(BASSASIO.Lib, BASS_ASIO_ChannelSetRate);
			LOADLIBFUNCTION(BASSASIO.Lib, BASS_ASIO_ControlPanel);
			LOADLIBFUNCTION(BASSASIO.Lib, BASS_ASIO_ErrorGetCode);
			LOADLIBFUNCTION(BASSASIO.Lib, BASS_ASIO_Free);
			LOADLIBFUNCTION(BASSASIO.Lib, BASS_ASIO_GetDevice);
			LOADLIBFUNCTION(BASSASIO.Lib, BASS_ASIO_GetDeviceInfo);
			LOADLIBFUNCTION(BASSASIO.Lib, BASS_ASIO_GetLatency);
			LOADLIBFUNCTION(BASSASIO.Lib, BASS_ASIO_GetRate);
			LOADLIBFUNCTION(BASSASIO.Lib, BASS_ASIO_Init);
			LOADLIBFUNCTION(BASSASIO.Lib, BASS_ASIO_SetRate);
			LOADLIBFUNCTION(BASSASIO.Lib, BASS_ASIO_Start);
			LOADLIBFUNCTION(BASSASIO.Lib, BASS_ASIO_Stop);
			LOADLIBFUNCTION(BASSASIO.Lib, BASS_ASIO_IsStarted);
			LOADLIBFUNCTION(BASSENC.Lib, BASS_Encode_Start);
			LOADLIBFUNCTION(BASSENC.Lib, BASS_Encode_Stop);
			LOADLIBFUNCTION(BASSENC.Lib, BASS_Encode_Write);
			LOADLIBFUNCTION(BASSENC.Lib, BASS_Encode_SetPaused);
			LOADLIBFUNCTION(BASS.Lib, BASS_ChannelFlags);
			LOADLIBFUNCTION(BASS.Lib, BASS_ChannelGetAttribute);
			LOADLIBFUNCTION(BASS.Lib, BASS_ChannelGetData);
			LOADLIBFUNCTION(BASS.Lib, BASS_ChannelGetLevelEx);
			LOADLIBFUNCTION(BASS.Lib, BASS_ChannelIsActive);
			LOADLIBFUNCTION(BASS.Lib, BASS_ChannelPlay);
			LOADLIBFUNCTION(BASS.Lib, BASS_ChannelRemoveFX);
			LOADLIBFUNCTION(BASS.Lib, BASS_ChannelSeconds2Bytes);
			LOADLIBFUNCTION(BASS.Lib, BASS_ChannelSetAttribute);
			LOADLIBFUNCTION(BASS.Lib, BASS_ChannelSetDevice);
			LOADLIBFUNCTION(BASS.Lib, BASS_ChannelSetFX);
			LOADLIBFUNCTION(BASS.Lib, BASS_ChannelStop);
			LOADLIBFUNCTION(BASS.Lib, BASS_ChannelUpdate);
			LOADLIBFUNCTION(BASS.Lib, BASS_ErrorGetCode);
			LOADLIBFUNCTION(BASS.Lib, BASS_FXSetParameters);
			LOADLIBFUNCTION(BASS.Lib, BASS_Free);
			LOADLIBFUNCTION(BASS.Lib, BASS_GetDevice);
			LOADLIBFUNCTION(BASS.Lib, BASS_GetDeviceInfo);
			LOADLIBFUNCTION(BASS.Lib, BASS_GetInfo);
			LOADLIBFUNCTION(BASS.Lib, BASS_Init);
			LOADLIBFUNCTION(BASS.Lib, BASS_PluginFree);
			LOADLIBFUNCTION(BASS.Lib, BASS_PluginLoad);
			LOADLIBFUNCTION(BASS.Lib, BASS_SetConfig);
			LOADLIBFUNCTION(BASS.Lib, BASS_Stop);
			LOADLIBFUNCTION(BASS.Lib, BASS_StreamFree);
			LOADLIBFUNCTION(BASSWASAPI.Lib, BASS_WASAPI_Init);
			LOADLIBFUNCTION(BASSWASAPI.Lib, BASS_WASAPI_Free);
			LOADLIBFUNCTION(BASSWASAPI.Lib, BASS_WASAPI_IsStarted);
			LOADLIBFUNCTION(BASSWASAPI.Lib, BASS_WASAPI_Start);
			LOADLIBFUNCTION(BASSWASAPI.Lib, BASS_WASAPI_Stop);
			LOADLIBFUNCTION(BASSWASAPI.Lib, BASS_WASAPI_GetDeviceInfo);
			LOADLIBFUNCTION(BASSWASAPI.Lib, BASS_WASAPI_GetInfo);
			LOADLIBFUNCTION(BASSWASAPI.Lib, BASS_WASAPI_GetDevice);
			LOADLIBFUNCTION(BASSWASAPI.Lib, BASS_WASAPI_GetLevelEx);
			LOADLIBFUNCTION(BASSMIDI.Lib, BASS_MIDI_FontFree);
			LOADLIBFUNCTION(BASSMIDI.Lib, BASS_MIDI_FontInit);
			LOADLIBFUNCTION(BASSMIDI.Lib, BASS_MIDI_FontLoad);
			LOADLIBFUNCTION(BASSMIDI.Lib, BASS_MIDI_StreamCreate);
			LOADLIBFUNCTION(BASSMIDI.Lib, BASS_MIDI_StreamEvent);
			LOADLIBFUNCTION(BASSMIDI.Lib, BASS_MIDI_StreamEvents);
			LOADLIBFUNCTION(BASSMIDI.Lib, BASS_MIDI_StreamGetEvent);
			LOADLIBFUNCTION(BASSMIDI.Lib, BASS_MIDI_StreamLoadSamples);
			LOADLIBFUNCTION(BASSMIDI.Lib, BASS_MIDI_StreamSetFonts);

			_BMSE = BASS_MIDI_StreamEvent;

			// Load plugins
			LoadPluginModule(&bassflac, L"bassflac.dll", TRUE);
			LoadPluginModule(&basswv, L"basswv.dll", FALSE);
			LoadPluginModule(&bassopus, L"bassopus.dll", FALSE);

			PrintMessageToDebugLog("ImportBASS", "Function pointers loaded into memory.");
		}
		else PrintMessageToDebugLog("ImportBASS", "BASS has been already loaded by the driver.");

		PrintMessageToDebugLog("ImportBASS", "Setting BASS flag to true.");
		BASSLoadedToMemory = TRUE;

		return TRUE;
	}
	catch (...) {
		_THROWCRASH;
	}
}

VOID UnloadBASSFunctions() {
	try {
		if (BASSLoadedToMemory) {
			// Set the functions back to dummies, to avoid issues
			_PrsData = DummyParseData;
			_PforBASSMIDI = DummyPrepareForBASSMIDI;
			_PlayBufData = DummyPlayBufData;
			_PlayBufDataChk = DummyPlayBufData;
			_BMSE = DummyBMSE;

			PrintMessageToDebugLog("UnloadBASS", "Freeing BASS libraries...");

			if (!BASS_PluginFree(bassflac))
				_THROWCRASH;
			bassflac = NULL;

			if (!BASS_PluginFree(basswv))
				_THROWCRASH;
			basswv = NULL;

			if (!BASS_PluginFree(bassopus))
				_THROWCRASH;
			bassopus = NULL;

			if (!BASS.AppOwnDLL)
			{
				if (!FreeLibrary(BASS.Lib))
					_THROWCRASH;
			}
			BASS.Lib = nullptr;

			if (!BASSMIDI.AppOwnDLL)
			{
				if (!FreeLibrary(BASSMIDI.Lib))
					_THROWCRASH;
			}
			BASSMIDI.Lib = nullptr;

			if (!BASSENC.AppOwnDLL)
			{
				if (!FreeLibrary(BASSENC.Lib))
					_THROWCRASH;
			}
			BASSENC.Lib = nullptr;

			if (!BASSASIO.AppOwnDLL)
			{
				if (!FreeLibrary(BASSASIO.Lib))
					_THROWCRASH;
			}
			BASSASIO.Lib = nullptr;

			if (!BASSWASAPI.AppOwnDLL)
			{
				if (!FreeLibrary(BASSWASAPI.Lib))
					_THROWCRASH;
			}
			BASSWASAPI.Lib = nullptr;

			if (!BASS_VST.AppOwnDLL)
			{
				if (BASS_VST.Lib)
					if (!FreeLibrary(BASS_VST.Lib))
						_THROWCRASH;
			}
			BASS_VST.Lib = nullptr;

			PrintMessageToDebugLog("UnloadBASS", "The BASS libraries have been freed from the app's working set.");
		}
		else PrintMessageToDebugLog("UnloadBASS", "BASS hasn't been loaded by the driver yet.");

		BASSLoadedToMemory = FALSE;
	}
	catch (...) {
		_THROWCRASH;
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
	if (EVBuffer.Buffer)
	{
		if (VirtualUnlock(EVBuffer.Buffer, EVBuffer.BufSize * sizeof(DWORD)))
			PrintMessageToDebugLog("AllocateMemoryFunc", "Unlocked buffer from RAM.");

		if (!VirtualFree(EVBuffer.Buffer, 0, MEM_RELEASE))
			_THROWCRASH;

		EVBuffer.Buffer = NULL;
		EVBuffer.BufSize = 0;
		EVBuffer.WriteHead = 0;
		EVBuffer.ReadHead = 0;
	}

	PrintMessageToDebugLog("FreeUpMemoryFunc", "Freed.");
}

void AllocateMemory(BOOL restart) {
	try {
		PrintMessageToDebugLog("AllocateMemoryFunc", "Allocating memory for EV buffer and audio buffer");

		// Check how much RAM is available
		ULONGLONG TempEvBufferSize = EvBufferSize;
		SIZE_T MinSize = 0, MaxSize = 0;
		MEMORYSTATUSEX status;
		status.dwLength = sizeof(status);
		GlobalMemoryStatusEx(&status);

		// Check if the user has chosen to get the EVBuffer size from the RAM
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

		if (restart) {
			if (EvBufferSize != EVBuffer.BufSize) {
				// Set them to dummy temporarily to avoid problems
				_PrsData = DummyParseData;
				_PforBASSMIDI = DummyPrepareForBASSMIDI;
				_PlayBufData = DummyPlayBufData;
				_PlayBufDataChk = DummyPlayBufData;
				_BMSE = DummyBMSE;

				FreeUpMemory();
			}

			else return;
		}

		if (EvBufferSize < 1) {
			MessageBox(NULL, L"The size of the buffer cannot be 0!\nIts size will now default to 16384 bytes.\n\nThe settings have been reset.", L"OmniMIDI - Illegal memory amount defined", MB_OK | MB_ICONEXCLAMATION | MB_SYSTEMMODAL);
			ResetEVBufferSettings();
			TempEvBufferSize = EvBufferSize;
		}

		// Begin allocating the EVBuffer
		if (EVBuffer.Buffer != NULL) PrintMessageToDebugLog("AllocateMemoryFunc", "EV buffer already allocated.");
		else {
			// Print the values to the log
			PrintMemoryMessageToDebugLog("AllocateMemoryFunc", "EV buffer size (in amount of DWORDs)", FALSE, TempEvBufferSize);
			PrintMemoryMessageToDebugLog("AllocateMemoryFunc", "EV buffer division ratio", TRUE, EvBufferMultRatio);
			PrintMemoryMessageToDebugLog("AllocateMemoryFunc", "EV buffer final size (in bytes, one DWORD is 4 bytes)", FALSE, EvBufferSize * 4);
			PrintMemoryMessageToDebugLog("AllocateMemoryFunc", "Total RAM available (in bytes)", FALSE, status.ullTotalPhys);

			PrintMessageToDebugLog("AllocateMemoryFunc", "Allocating EV buffer...");
			EVBuffer.BufSize = EvBufferSize;
			EVBuffer.Buffer = (DWORD*)VirtualAlloc(NULL, EVBuffer.BufSize * sizeof(DWORD), MEM_COMMIT | MEM_RESERVE, PAGE_READWRITE);
			if (EVBuffer.Buffer == NULL) {
				MessageBox(NULL, L"The driver failed to allocate the events buffer!\n\nNot enough memory, press OK to quit.", L"OmniMIDI - FATAL ERREOR", MB_OK | MB_ICONERROR | MB_SYSTEMMODAL);
				exit(ERROR_NOT_ENOUGH_MEMORY);
			}

			if (!VirtualLock(EVBuffer.Buffer, EVBuffer.BufSize * sizeof(DWORD)))
				PrintMessageToDebugLog("AllocateMemoryFunc", "VirtualLock failed to lock the events buffer to the CPU cache/RAM. This could reduce performance!");
			else
				PrintMessageToDebugLog("AllocateMemoryFunc", "VirtualLock successfully locked the events buffer to the CPU cache/RAM.");

			PrintMessageToDebugLog("AllocateMemoryFunc", "EV buffer allocated.");
		}

		// Set heads to 0 and store buffer size
		EVBuffer.WriteHead = 0;
		EVBuffer.ReadHead = 0;
		PrintMessageToDebugLog("AllocateMemoryFunc", "Set heads to 0.");

		if (restart) {
			_PrsData = HyperMode ? ParseDataHyper : ParseData;
			_PforBASSMIDI = HyperMode ? PrepareForBASSMIDIHyper : PrepareForBASSMIDI;
			_PlayBufData = HyperMode ? PlayBufferedDataHyper : PlayBufferedData;
			_PlayBufDataChk = ManagedSettings.NotesCatcherWithAudio ? (HyperMode ? PlayBufferedDataChunkHyper : PlayBufferedDataChunk) : DummyPlayBufData;

			_BMSE = BASS_MIDI_StreamEvent;
		}
	}
	catch (...) {
		_THROWCRASH;
	}
}

void LoadSettings(BOOL Restart, BOOL RT)
{	
	// Initialize the temp values
	DWORD
		TempSC = ManagedSettings.SincConv,
		TempOV = ManagedSettings.OutputVolume,
		TempHP = HyperMode,
		TempNCWA = ManagedSettings.NotesCatcherWithAudio,
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

	DOUBLE
		TSpeedHack = SpeedHack;

	DWORD
		RSH = 100000000;

	try {
		// If the driver is booting up, then return true
		BOOL IsBootUp = (!RT && !Restart);
		if (!RT) PrintMessageToDebugLog("LoadSettingsFuncs", "Loading settings from registry...");

		// Load the settings from the registry
		OpenRegistryKey(Configuration, L"Software\\OmniMIDI\\Configuration", TRUE);

		// These settings should NOT be loaded in real-time
		if (!RT) {
			// Load the selected driver priority value from the registry
			OpenRegistryKey(MainKey, L"Software\\OmniMIDI", TRUE);
			RegQueryValueEx(MainKey.Address, L"DriverPriority", NULL, &dwType, (LPBYTE)&ManagedSettings.DriverPriority, &dwSize);

			RegQueryValueEx(Configuration.Address, L"ASIODirectFeed", NULL, &dwType, (LPBYTE)&ManagedSettings.ASIODirectFeed, &dwSize);
			RegQueryValueEx(Configuration.Address, L"DisableASIOFreqWarn", NULL, &dwType, (LPBYTE)&ManagedSettings.DisableASIOFreqWarn, &dwSize);
			RegQueryValueEx(Configuration.Address, L"LeaveASIODeviceFreq", NULL, &dwType, (LPBYTE)&ManagedSettings.LeaveASIODeviceFreq, &dwSize);
			RegQueryValueEx(Configuration.Address, L"AudioBitDepth", NULL, &dwType, (LPBYTE)&ManagedSettings.AudioBitDepth, &dwSize);
			RegQueryValueEx(Configuration.Address, L"AudioFrequency", NULL, &dwType, (LPBYTE)&ManagedSettings.AudioFrequency, &dwSize);
			RegQueryValueEx(Configuration.Address, L"AudioOutput", NULL, &dwType, (LPBYTE)&ManagedSettings.AudioOutputReg, &dwSize);
			RegQueryValueEx(Configuration.Address, L"CurrentEngine", NULL, &dwType, (LPBYTE)&ManagedSettings.CurrentEngine, &dwSize);
			RegQueryValueEx(Configuration.Address, L"DriverPriority", NULL, &dwType, (LPBYTE)&ManagedSettings.DriverPriority, &dwSize);
			RegQueryValueEx(Configuration.Address, L"EvBufferMultRatio", NULL, &dwType, (LPBYTE)&TEvBufferMultRatio, &dwSize);
			RegQueryValueEx(Configuration.Address, L"EvBufferSize", NULL, &qwType, (LPBYTE)&TEvBufferSize, &qwSize);
			RegQueryValueEx(Configuration.Address, L"Extra8Lists", NULL, &dwType, (LPBYTE)&ManagedSettings.Extra8Lists, &dwSize);
			RegQueryValueEx(Configuration.Address, L"FollowDefaultAudioDevice", NULL, &dwType, (LPBYTE)&ManagedSettings.FollowDefaultAudioDevice, &dwSize);
			RegQueryValueEx(Configuration.Address, L"GetEvBuffSizeFromRAM", NULL, &dwType, (LPBYTE)&GetEvBuffSizeFromRAM, &dwSize);
			RegQueryValueEx(Configuration.Address, L"MonoRendering", NULL, &dwType, (LPBYTE)&ManagedSettings.MonoRendering, &dwSize);
			RegQueryValueEx(Configuration.Address, L"VolumeMonitor", NULL, &dwType, (LPBYTE)&ManagedSettings.VolumeMonitor, &dwSize);
			RegQueryValueEx(Configuration.Address, L"WASAPIExclusive", NULL, &dwType, (LPBYTE)&ManagedSettings.WASAPIExclusive, &dwSize);
			RegQueryValueEx(Configuration.Address, L"WASAPIRAWMode", NULL, &dwType, (LPBYTE)&ManagedSettings.WASAPIRAWMode, &dwSize);
			RegQueryValueEx(Configuration.Address, L"WASAPIDoubleBuf", NULL, &dwType, (LPBYTE)&ManagedSettings.WASAPIDoubleBuf, &dwSize);
			RegQueryValueEx(Configuration.Address, L"ReduceBootUpDelay", NULL, &dwType, (LPBYTE)&ManagedSettings.ReduceBootUpDelay, &dwSize);
			RegQueryValueEx(Configuration.Address, L"XASamplesPerFrame", NULL, &dwType, (LPBYTE)&ManagedSettings.XASamplesPerFrame, &dwSize);
			RegQueryValueEx(Configuration.Address, L"XASPFSweepRate", NULL, &dwType, (LPBYTE)&ManagedSettings.XASPFSweepRate, &dwSize);
			RegQueryValueEx(Configuration.Address, L"LogarithmVol", NULL, &dwType, (LPBYTE)&LogarithmVol, &dwSize);
			
			if (ManagedSettings.CurrentEngine != AUDTOWAV) RegQueryValueEx(Configuration.Address, L"NotesCatcherWithAudio", NULL, &dwType, (LPBYTE)&TempNCWA, &dwSize);
			else ManagedSettings.NotesCatcherWithAudio = TRUE;
		
			SamplesPerFrame = ManagedSettings.XASamplesPerFrame * (ManagedSettings.MonoRendering ? 1 : 2);
		}

		RegQueryValueEx(Configuration.Address, L"WinMMSpeed", NULL, &dwType, (LPBYTE)&RSH, &dwSize);
		RegQueryValueEx(Configuration.Address, L"BufferLength", NULL, &dwType, (LPBYTE)&ManagedSettings.BufferLength, &dwSize);
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
		RegQueryValueEx(Configuration.Address, L"CPitchValue", NULL, &dwType, (LPBYTE)&ManagedSettings.ConcertPitch, &dwSize);
		RegQueryValueEx(Configuration.Address, L"VolumeMonitor", NULL, &dwType, (LPBYTE)&ManagedSettings.VolumeMonitor, &dwSize);
		RegQueryValueEx(Configuration.Address, L"AudioRampIn", NULL, &dwType, (LPBYTE)&ManagedSettings.AudioRampIn, &dwSize);
		RegQueryValueEx(Configuration.Address, L"LinAttMod", NULL, &dwType, (LPBYTE)&ManagedSettings.LinAttMod, &dwSize);
		RegQueryValueEx(Configuration.Address, L"LinDecVol", NULL, &dwType, (LPBYTE)&ManagedSettings.LinDecVol, &dwSize);
		RegQueryValueEx(Configuration.Address, L"NoSFGenLimits", NULL, &dwType, (LPBYTE)&ManagedSettings.NoSFGenLimits, &dwSize);
		
		// Stuff that works so don't bother
		if (!Between(ManagedSettings.MinVelIgnore, 1, 127)) { ManagedSettings.MinVelIgnore = 1; }
		if (!Between(ManagedSettings.MaxVelIgnore, 1, 127)) { ManagedSettings.MaxVelIgnore = 1; }

		TSpeedHack = (double)RSH / 100000000.0;

		if (TSpeedHack != SpeedHack) {
			if (NT_SUCCESS(NtQuerySystemTime(&TickStart))) {
				PrintMessageToDebugLog("LoadSettingsFuncs", "SpeedHack updated.");
			}
			SpeedHack = TSpeedHack;
		}

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
			if (RT) ResetSynth(TRUE, FALSE);
		}

		// Check if the value is different from the temporary one
		if (TempHP != HyperMode || SettingsManagedByClient) {
			if (!SettingsManagedByClient) HyperMode = TempHP;

			// Close the threads for safety reasons
			if (RT) stop_thread = TRUE;

			// Check if "Hyper-playback" mode has been enabled
			_PrsData = HyperMode ? ParseDataHyper : ParseData;
			_PforBASSMIDI = HyperMode ? PrepareForBASSMIDIHyper : PrepareForBASSMIDI;
			_PlayBufData = HyperMode ? PlayBufferedDataHyper : PlayBufferedData;
			_PlayBufDataChk = ManagedSettings.NotesCatcherWithAudio ? (HyperMode ? PlayBufferedDataChunkHyper : PlayBufferedDataChunk) : DummyPlayBufData;

			// Restart threads
			if (RT) stop_thread = FALSE;
		}

		if (TempNCWA != ManagedSettings.NotesCatcherWithAudio || SettingsManagedByClient) {
			if (!SettingsManagedByClient) ManagedSettings.NotesCatcherWithAudio = TempNCWA;
		}

		if (IsBootUp || (TEvBufferSize != EvBufferSize || TEvBufferMultRatio != EvBufferMultRatio)) {
			EvBufferSize = TEvBufferSize;
			EvBufferMultRatio = TEvBufferMultRatio;
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
			if (RT) BASS_ChannelFlags(OMStream, ManagedSettings.IgnoreSysReset ? BASS_MIDI_NOSYSRESET : 0, BASS_MIDI_NOSYSRESET);
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
		_THROWCRASH;
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
		_THROWCRASH;
	}
}

int AudioRenderingType(BOOLEAN IsItStreamCreation, INT RegistryVal) {
	switch (ManagedSettings.CurrentEngine) {
	case XAUDIO_ENGINE:
	case ASIO_ENGINE:
	case WASAPI_ENGINE:
		return IsItStreamCreation ? BASS_SAMPLE_FLOAT : BASS_DATA_FLOAT;
	default:
		if (RegistryVal == 0)
			return IsItStreamCreation ? BASS_SAMPLE_FLOAT : BASS_DATA_FLOAT;
		else if (RegistryVal == 1)
			return 0;
		else
			return IsItStreamCreation ? BASS_SAMPLE_8BITS : 0;
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
		_THROWCRASH;
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
					if (ManagedSettings.WASAPIDoubleBuf)
						BASS_WASAPI_GetLevelEx(levels, (ManagedSettings.MonoRendering ? 0.01f : 0.02f), (ManagedSettings.MonoRendering ? BASS_LEVEL_MONO : BASS_LEVEL_STEREO));
					else {
						left = 0;	// the left level
						right = 0;	// the right level
					}

					break;
					
				case BASS_OUTPUT:
				// case DXAUDIO_ENGINE:
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
		_THROWCRASH;
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

void FillContentDebug() {
	GetAppName();

	std::locale::global(std::locale::classic());	// DO NOT REMOVE

	// For debug window
	std::wstring PipeContent;
	DWORD bytesWritten;								// Needed for Windows 7 apparently...
	DWORD handleCount;

	PROCESS_MEMORY_COUNTERS_EX pmc;
	GetProcessMemoryInfo(GetCurrentProcess(), (PROCESS_MEMORY_COUNTERS*)&pmc, sizeof(pmc));
	GetProcessHandleCount(GetCurrentProcess(), &handleCount);
	SIZE_T RU = pmc.WorkingSetSize;
	QWORD ramusageint = static_cast<QWORD>(RU);

	PipeContent.append(L"OMDebugInfo");
	PipeContent.append(L"|CurrentApp = ");
	PipeContent.append(AppPathW);
#if defined(_M_AMD64)
	PipeContent.append(L"|BitApp = AMD64");
#elif defined(_M_IX86)
	PipeContent.append(L"|BitApp = i386");
#elif defined(_M_ARM64)
	PipeContent.append(L"|BitApp = AArch64");
#endif

	for (int i = 0; i <= 15; ++i) 
		PipeContent.append(L"|CV" + std::to_wstring(i) + L" = " + std::to_wstring(ManagedDebugInfo.ActiveVoices[i]));

	PipeContent.append(L"|CurCPU = " + std::to_wstring(ManagedDebugInfo.RenderingTime));
	PipeContent.append(L"|Handles = " + std::to_wstring(handleCount));
	PipeContent.append(L"|RAMUsage = " + std::to_wstring(static_cast<QWORD>(RU)));
	PipeContent.append(L"|OMDirect = " + std::to_wstring(KDMAPIEnabled));
	PipeContent.append(L"|WinMMKDMAPI = " + std::to_wstring(IsKDMAPIViaWinMM));
	PipeContent.append(L"|EVBufferSize = " + std::to_wstring(EVBuffer.BufSize));
	PipeContent.append(L"|EVReadHead = " + std::to_wstring(EVBuffer.ReadHead));
	PipeContent.append(L"|EVWriteHead = " + std::to_wstring(EVBuffer.WriteHead));
	PipeContent.append(L"|AudioBufSize = " + std::to_wstring(ManagedDebugInfo.AudioBufferSize));
	PipeContent.append(L"|AudioLatency = " + std::to_wstring(ManagedDebugInfo.AudioLatency));
	PipeContent.append(L"|SFsList = " + std::to_wstring(ManagedDebugInfo.CurrentSFList));

	PipeContent.append(L"\n\0");

	const WCHAR* PCW = PipeContent.c_str();
	WriteFile(hPipe, (LPVOID)PCW, wcslen(PCW) * sizeof(wchar_t), &bytesWritten, NULL);
	if (hPipe == INVALID_HANDLE_VALUE || (GetLastError() != ERROR_SUCCESS && GetLastError() != ERROR_PIPE_LISTENING)) StartDebugPipe(TRUE);
}

void ParseDebugData() {
	DWORD ASIOTempOutLatency;

	if (BASSLoadedToMemory && bass_initialized) {
		BASS_ChannelGetAttribute(OMStream, BASS_ATTRIB_CPU, &ManagedDebugInfo.RenderingTime);

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
		ManagedDebugInfo.RenderingTime = 0.0f;
		ManagedDebugInfo.AudioLatency = 0.0;
		for (int i = 0; i <= 15; ++i) ManagedDebugInfo.ActiveVoices[i] = 0;
	}
}

void SendDebugDataToPipe() {
	try {
		FillContentDebug();

		FlushFileBuffers(hPipe);
	}
	catch (...) {
		_THROWCRASH;
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
		_THROWCRASH;
	}
}

void RevbNChor() {
	if (!ManagedSettings.EnableSFX)
		return;
	
	try {
		BOOL Reverb = FALSE;
		BOOL Chorus = FALSE;
		OpenRegistryKey(Configuration, L"Software\\OmniMIDI\\Configuration", TRUE);

		RegQueryValueEx(Configuration.Address, L"ReverbOverride", NULL, &dwType, (LPBYTE)&Reverb, &dwSize);
		RegQueryValueEx(Configuration.Address, L"ChorusOverride", NULL, &dwType, (LPBYTE)&Chorus, &dwSize);

		if (Reverb) {
			RegQueryValueEx(Configuration.Address, L"Reverb", NULL, &dwType, (LPBYTE)&reverb, &dwSize);

			for (int i = 0; i <= 15; ++i)
				BASS_MIDI_StreamEvent(OMStream, i, MIDI_EVENT_REVERB, reverb);
		}

		if (Chorus) {
			RegQueryValueEx(Configuration.Address, L"Chorus", NULL, &dwType, (LPBYTE)&chorus, &dwSize);

			for (int i = 0; i <= 15; ++i)
				BASS_MIDI_StreamEvent(OMStream, i, MIDI_EVENT_CHORUS, chorus);
		}
	}
	catch (...) {
		_THROWCRASH;
	}
}

void ReloadSFList(DWORD whichsflist){
	try {	
		if (LoadSoundfont(whichsflist))
			ResetSynth(FALSE, FALSE);
	}
	catch (...) {
		_THROWCRASH;
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
			for (int i = 0; i < sizeof(Keys); i++)
				Keys[i] = GetAsyncKeyState(i);

			// Hotkey ALT
			if (Keys[VK_MENU])
			{
				// ALT + 1
				if (Keys[0x31])
				{
					ReloadSFList(0);
					return;
				}

				// ALT + 2
				if (Keys[0x32]) 
				{
					ReloadSFList(1);
					return;
				}

				// ALT + 3
				if (Keys[0x33]) 
				{
					ReloadSFList(2);
					return;
				}

				// ALT + 4
				if (Keys[0x34]) 
				{
					ReloadSFList(3);
					return;
				}

				// ALT + 5
				if (Keys[0x35])
				{
					ReloadSFList(4);
					return;
				}

				// ALT + 6
				if (Keys[0x36])
				{
					ReloadSFList(5);
					return;
				}

				// ALT + 7
				if (Keys[0x37])
				{
					ReloadSFList(6);
					return;
				}

				// ALT + 8
				if (Keys[0x38])
				{
					ReloadSFList(7);
					return;
				}

				// ALT + 9
				if (Keys[0x39])
				{
					if (ManagedSettings.CurrentEngine == ASIO_ENGINE) {
						if (BASSLoadedToMemory && bass_initialized) BASS_ASIO_ControlPanel();
					}
					else {
						if (GetFolderPath(FOLDERID_SystemX86, CSIDL_SYSTEMX86, OMConfiguratorDir, sizeof(OMConfiguratorDir)))
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
					if (GetFolderPath(FOLDERID_SystemX86, CSIDL_SYSTEMX86, OMConfiguratorDir, sizeof(OMConfiguratorDir)))
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
				ResetSynth(FALSE, Keys[VK_LSHIFT] ? TRUE : FALSE);
				return;
			}
		}
	}
	catch (...) {
		_THROWCRASH;
	}
}