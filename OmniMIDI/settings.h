/*
OmniMIDI settings loading system
*/

void OpenRegistryKey(RegKey &hKey, LPCWSTR hKeyDir) {
	// If the key isn't ready, open it again
	if (hKey.Status != KEY_READY) {
		// Open the key
		hKey.Status = RegOpenKeyEx(HKEY_CURRENT_USER, hKeyDir, 0, KEY_ALL_ACCESS, &hKey.Address);

		// If the key failed to open, throw a crash
		if (hKey.Status != KEY_READY) CrashMessage(L"hKeyOpen");
	}
}

void CloseRegistryKey(RegKey &hKey) {
	// Try to close the key
	LSTATUS Action = RegCloseKey(hKey.Address);

	// If the key can't be closed, throw a crash
	if (Action != ERROR_SUCCESS) CrashMessage(L"hKeyClose");

	// Everything is fine, mark the key as closed
	hKey.Status = KEY_CLOSED;
}

void CloseThread(HANDLE thread) {
	// Wait for the thread to finish its job
	WaitForSingleObject(thread, INFINITE);

	// Close its handle
	CloseHandle(thread);

	// And mark it as NULL
	thread = NULL;
}

void DLLLoadError(LPCWSTR dll) {
	TCHAR errormessage[MAX_PATH] = L"There was an error while trying to load the DLL for the driver!\nFaulty/missing DLL: ";
	TCHAR clickokmsg[MAX_PATH] = L"\n\nClick OK to close the program.";
	lstrcat(errormessage, dll);
	lstrcat(errormessage, clickokmsg);
	SetConsoleTextAttribute(hConsole, FOREGROUND_RED);
	std::cout << "(Invalid DLL: " << dll << ") " << " - Fatal error during the loading process of the following DLL." << std::endl;

	const int result = MessageBoxW(NULL, errormessage, L"OmniMIDI - DLL load error", MB_ICONERROR | MB_SYSTEMMODAL);
	switch (result)
	{
	default:
		exit(0);
		return;
	}
}

long long TimeNow() {
	LARGE_INTEGER now;
	LARGE_INTEGER s_frequency;
	QueryPerformanceCounter(&now);
	QueryPerformanceFrequency(&s_frequency);
	return (1000LL * now.QuadPart) / s_frequency.QuadPart;
}

void CopyToClipboard(const std::string &s) {
	OpenClipboard(0);
	EmptyClipboard();
	HGLOBAL hg = GlobalAlloc(GMEM_MOVEABLE, s.size());
	if (!hg) {
		CloseClipboard();
		return;
	}
	memcpy(GlobalLock(hg), s.c_str(), s.size());
	GlobalUnlock(hg);
	SetClipboardData(CF_TEXT, hg);
	CloseClipboard();
	GlobalFree(hg);
}

void ResetSynth(int ischangingbuffermode){
	reset_synth = 1;
	if (ischangingbuffermode == 1) {
		writehead = 0;
		readhead = 0;
		eventcount = 0;
	}
	BASS_MIDI_StreamEvent(OMStream, 0, MIDI_EVENT_SYSTEM, MIDI_SYSTEM_DEFAULT);
	BASS_MIDI_StreamEvent(OMStream, 0, MIDI_EVENT_SYSTEMEX, MIDI_SYSTEM_DEFAULT);
	reset_synth = 0;
}

void LoadSoundfont(int whichsf){
	try {
		PrintToConsole(FOREGROUND_RED, whichsf, "Freeing previous soundfont list...");
		FreeFonts();

		PrintToConsole(FOREGROUND_RED, whichsf, "Loading soundfont list...");
		TCHAR config[MAX_PATH];
		BASS_MIDI_FONT * mf;

		OpenRegistryKey(Watchdog, L"Software\\OmniMIDI\\Watchdog");
		RegSetValueEx(Watchdog.Address, L"currentsflist", 0, REG_DWORD, (LPBYTE)&whichsf, sizeof(whichsf));

		LoadFonts(sflistloadme[whichsf - 1]);
		BASS_MIDI_StreamLoadSamples(OMStream);

		PrintToConsole(FOREGROUND_RED, whichsf, "Done.");
	}
	catch (...) {
		CrashMessage(L"ListLoad");
	}
}

bool LoadSoundfontStartup() {
	try {
		int done = 0;
		TCHAR modulename[MAX_PATH];
		TCHAR fullmodulename[MAX_PATH];
		GetModuleFileName(NULL, modulename, MAX_PATH);
		GetModuleFileName(NULL, fullmodulename, MAX_PATH);
		PathStripPath(modulename);

		for (int i = 0; i <= 15; ++i) {
			SHGetFolderPath(NULL, CSIDL_PROFILE, NULL, 0, listsloadme[i]);
			SHGetFolderPath(NULL, CSIDL_PROFILE, NULL, 0, sflistloadme[i]);
			_tcscat(sflistloadme[i], sfdirs[i]);
			_tcscat(listsloadme[i], listsanalyze[i]);
			std::wifstream file(listsloadme[i]);
			if (file) {
				TCHAR defaultstring[MAX_PATH];
				while (file.getline(defaultstring, sizeof(defaultstring) / sizeof(*defaultstring)))
				{
					if (_wcsicmp(modulename, defaultstring) && _wcsicmp(fullmodulename, defaultstring) == 0) {
						LoadSoundfont(i + 1);
						done = 1;
						PrintToConsole(FOREGROUND_RED, i, "Found it");
					}
				}
			}
			file.close();
		}

		if (done == 1) {
			return TRUE;
		}
		else {
			return FALSE;
		}
	}
	catch (...) {
		CrashMessage(L"ListLoadStartUp");
	}
}

BOOL load_bassfuncs()
{
	try {
		TCHAR installpath[MAX_PATH] = { 0 };
		TCHAR bassencpath[MAX_PATH] = { 0 };
		TCHAR bassasiopath[MAX_PATH] = { 0 };
		TCHAR bassmidipath[MAX_PATH] = { 0 };
		TCHAR bassmixpath[MAX_PATH] = { 0 };
		TCHAR basspath[MAX_PATH] = { 0 };
		TCHAR bassfxpath[MAX_PATH] = { 0 };

		int installpathlength;

		GetModuleFileName(hinst, installpath, MAX_PATH);
		PathRemoveFileSpec(installpath);

		SetConsoleTextAttribute(hConsole, FOREGROUND_RED);
		PrintToConsole(FOREGROUND_RED, 1, "Allocating memory for BASS DLLs...");

		// BASS
		lstrcat(basspath, installpath);
		lstrcat(basspath, L"\\bass.dll");
		if (!(bass = LoadLibrary(basspath))) {
			DLLLoadError(basspath);
			exit(0);
		}

		// BASSMIDI
		lstrcat(bassmidipath, installpath);
		lstrcat(bassmidipath, L"\\bassmidi.dll");
		if (!(bassmidi = LoadLibrary(bassmidipath))) {
			DLLLoadError(bassmidipath);
			exit(0);
		}

		// BASSenc
		lstrcat(bassencpath, installpath);
		lstrcat(bassencpath, L"\\bassenc.dll");
		if (!(bassenc = LoadLibrary(bassencpath))) {
			DLLLoadError(bassencpath);
			exit(0);
		}

		// BASSASIO
		lstrcat(bassasiopath, installpath);
		lstrcat(bassasiopath, L"\\bassasio.dll");
		if (!(bassasio = LoadLibrary(bassasiopath))) {
			DLLLoadError(bassasiopath);
			exit(0);
		}

		PrintToConsole(FOREGROUND_RED, 1, "Done loading BASS DLLs.");

		// Load all the functions into memory
		PrintToConsole(FOREGROUND_RED, 1, "Loading BASS functions...");
		LOADBASSASIOFUNCTION(BASS_ASIO_ChannelEnable);
		LOADBASSASIOFUNCTION(BASS_ASIO_ChannelGetLevel);
		LOADBASSASIOFUNCTION(BASS_ASIO_ChannelJoin);
		LOADBASSASIOFUNCTION(BASS_ASIO_ChannelReset);
		LOADBASSASIOFUNCTION(BASS_ASIO_ChannelSetFormat);
		LOADBASSASIOFUNCTION(BASS_ASIO_ChannelSetRate);
		LOADBASSASIOFUNCTION(BASS_ASIO_ChannelSetVolume);
		LOADBASSASIOFUNCTION(BASS_ASIO_ChannelEnableMirror);
		LOADBASSASIOFUNCTION(BASS_ASIO_ControlPanel);
		LOADBASSASIOFUNCTION(BASS_ASIO_GetRate);
		LOADBASSASIOFUNCTION(BASS_ASIO_GetLatency);
		LOADBASSASIOFUNCTION(BASS_ASIO_GetDeviceInfo);
		LOADBASSASIOFUNCTION(BASS_ASIO_ErrorGetCode);
		LOADBASSASIOFUNCTION(BASS_ASIO_Free);
		LOADBASSASIOFUNCTION(BASS_ASIO_GetCPU);
		LOADBASSASIOFUNCTION(BASS_ASIO_Init);
		LOADBASSASIOFUNCTION(BASS_ASIO_SetDSD);
		LOADBASSASIOFUNCTION(BASS_ASIO_SetRate);
		LOADBASSASIOFUNCTION(BASS_ASIO_Start);
		LOADBASSASIOFUNCTION(BASS_ASIO_Stop);
		LOADBASSENCFUNCTION(BASS_Encode_Start);
		LOADBASSENCFUNCTION(BASS_Encode_Stop);
		LOADBASSFUNCTION(BASS_ChannelFlags);
		LOADBASSFUNCTION(BASS_ChannelGetAttribute);
		LOADBASSFUNCTION(BASS_ChannelGetData);
		LOADBASSFUNCTION(BASS_ChannelGetLevelEx);
		LOADBASSFUNCTION(BASS_ChannelIsActive);
		LOADBASSFUNCTION(BASS_ChannelPlay);
		LOADBASSFUNCTION(BASS_ChannelRemoveFX);
		LOADBASSFUNCTION(BASS_ChannelSeconds2Bytes);
		LOADBASSFUNCTION(BASS_ChannelSetAttribute);
		LOADBASSFUNCTION(BASS_ChannelSetDevice);
		LOADBASSFUNCTION(BASS_ChannelSetFX);
		LOADBASSFUNCTION(BASS_ChannelSetSync);
		LOADBASSFUNCTION(BASS_ChannelStop);
		LOADBASSFUNCTION(BASS_ChannelUpdate);
		LOADBASSFUNCTION(BASS_Update);
		LOADBASSFUNCTION(BASS_ErrorGetCode);
		LOADBASSFUNCTION(BASS_Free);
		LOADBASSFUNCTION(BASS_Stop);
		LOADBASSFUNCTION(BASS_GetDevice);
		LOADBASSFUNCTION(BASS_GetDeviceInfo);
		LOADBASSFUNCTION(BASS_GetInfo);
		LOADBASSFUNCTION(BASS_Init);
		LOADBASSFUNCTION(BASS_PluginLoad);
		LOADBASSFUNCTION(BASS_SetConfig);
		LOADBASSFUNCTION(BASS_SetDevice);
		LOADBASSFUNCTION(BASS_SetVolume);
		LOADBASSFUNCTION(BASS_StreamFree);
		LOADBASSFUNCTION(BASS_FXSetParameters);
		LOADBASSMIDIFUNCTION(BASS_MIDI_FontFree);
		LOADBASSMIDIFUNCTION(BASS_MIDI_FontInit);
		LOADBASSMIDIFUNCTION(BASS_MIDI_FontLoad);
		LOADBASSMIDIFUNCTION(BASS_MIDI_StreamCreate);
		LOADBASSMIDIFUNCTION(BASS_MIDI_StreamEvent);
		LOADBASSMIDIFUNCTION(BASS_MIDI_StreamEvents);
		LOADBASSMIDIFUNCTION(BASS_MIDI_StreamGetEvent);
		LOADBASSMIDIFUNCTION(BASS_MIDI_StreamLoadSamples);
		LOADBASSMIDIFUNCTION(BASS_MIDI_StreamSetFonts);
		// LOADBASSMIDIFUNCTION(BASS_MIDI_StreamSetFilter);		// Not needed

		PrintToConsole(FOREGROUND_RED, 1, "BASS functions succesfully loaded.");

		// Load audio extensions
		BASS_PluginLoad("bassflac.dll", BASS_UNICODE);
		BASS_PluginLoad("bassopus.dll", BASS_UNICODE);
		BASS_PluginLoad("bass_mpc.dll", BASS_UNICODE);
		BASS_PluginLoad("basswv.dll", BASS_UNICODE);

		return TRUE;
	}
	catch (...) {
		CrashMessage(L"BASSLibLoad");
	}
}

void FreeUpMemory() {
	// Free up the memory, since it's not needed or it has to be reinitialized
	memset(evbuf, 0, sizeof(evbuf_t));
	free(evbuf);
	evbuf = NULL;
	memset(sndbf, 0, sizeof(float));
	free(sndbf);
	sndbf = NULL;
}

void AllocateMemory(BOOL restart) {
	try {
		PrintToConsole(FOREGROUND_BLUE, 1, "Allocating memory for EV buffer and audio buffer...");

		// Check how much RAM is available
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
			if (TempEvBufferSize > status.ullTotalPhys) TempEvBufferSize = status.ullTotalPhys;
		}

#if !_WIN64
		// !! ONLY FOR x86 APPS !!

		// Check if the EVBuffer size goes above 1GB of RAM
		// Each 32-bit app is limited to a 2GB working set size
		if (TempEvBufferSize > 1073741824) {
			// It is, limit the EVBuffer to 1GB
			PrintToConsole(FOREGROUND_BLUE, 1, "EV buffer is too big, limiting to 1GB...");
			TempEvBufferSize = 1073741824;
		}
#endif

		// Calculate the ratio
		PrintToConsole(FOREGROUND_BLUE, 1, "Calculating ratio...");
		EvBufferSize = TempEvBufferSize / (unsigned long long)EvBufferMultRatio;

		// Print the values to the registry
		std::ostringstream st;
		std::ostringstream nd;
		std::ostringstream rd;
		st << "EV buffer size: " << TempEvBufferSize;
		nd << "EV buffer ratio: " << EvBufferMultRatio;
		rd << "EV buffer final size: " << EvBufferSize;

		PrintToConsole(FOREGROUND_BLUE, 1, st.str().c_str());
		PrintToConsole(FOREGROUND_BLUE, 1, nd.str().c_str());
		PrintToConsole(FOREGROUND_BLUE, 1, rd.str().c_str());

		PrintToConsole(FOREGROUND_BLUE, 1, "Calculating ratio...");
		PrintToConsole(FOREGROUND_BLUE, 1, "Calculating ratio...");

		// Begin allocating the EVBuffer
		if (evbuf != NULL) PrintToConsole(FOREGROUND_BLUE, 1, "EV buffer already allocated.");
		else {
			if (restart) {
				PrintToConsole(FOREGROUND_BLUE, 1, "Freeing EV buffer...");
				memset(evbuf, 0, sizeof(evbuf));
				free(evbuf);
				evbuf = NULL;
				PrintToConsole(FOREGROUND_BLUE, 1, "Freed.");
			}

			PrintToConsole(FOREGROUND_BLUE, 1, "Allocating EV buffer...");
			evbuf = (evbuf_t *)calloc((unsigned long long)EvBufferSize, sizeof(evbuf_t));
			PrintToConsole(FOREGROUND_BLUE, 1, "EV buffer allocated.");
			EVBuffReady = TRUE;
		}

		// Done, now allocate the buffer for the ".WAV mode"
		if (sndbf != NULL) PrintToConsole(FOREGROUND_BLUE, 1, "Audio buffer already allocated.");
		else {
			if (restart) {
				PrintToConsole(FOREGROUND_BLUE, 1, "Freeing audio buffer...");
				memset(sndbf, 0, sizeof(sndbf));
				free(sndbf);
				sndbf = NULL;
				PrintToConsole(FOREGROUND_BLUE, 1, "Freed.");
			}

			PrintToConsole(FOREGROUND_BLUE, 1, "Allocating audio buffer...");
			sndbf = (float *)calloc(256.0f, sizeof(float));
			PrintToConsole(FOREGROUND_BLUE, 1, "Audio buffer allocated.");
		}
	}
	catch (...) {
		CrashMessage(L"EVBufAlloc");
	}
}

void LoadSettings()
{
	try {
		PrintToConsole(FOREGROUND_BLUE, 1, "Loading settings from registry...");

		// Load the settings from the registry
		OpenRegistryKey(Configuration, L"Software\\OmniMIDI\\Configuration");

		RegQueryValueEx(Configuration.Address, L"TransposeValue", NULL, &dwType, (LPBYTE)&ManagedSettings.TransposeValue, &dwSize);
		RegQueryValueEx(Configuration.Address, L"SleepStates", NULL, &dwType, (LPBYTE)&ManagedSettings.SleepStates, &dwSize);
		RegQueryValueEx(Configuration.Address, L"SincInter", NULL, &dwType, (LPBYTE)&ManagedSettings.SincInter, &dwSize);
		RegQueryValueEx(Configuration.Address, L"SincConv", NULL, &dwType, (LPBYTE)&ManagedSettings.SincConv, &dwSize);
		RegQueryValueEx(Configuration.Address, L"PreloadSoundFonts", NULL, &dwType, (LPBYTE)&ManagedSettings.PreloadSoundFonts, &dwSize);
		RegQueryValueEx(Configuration.Address, L"OutputVolume", NULL, &dwType, (LPBYTE)&ManagedSettings.OutputVolume, &dwSize);
		RegQueryValueEx(Configuration.Address, L"NoteOff1", NULL, &dwType, (LPBYTE)&ManagedSettings.NoteOff1, &dwSize);
		RegQueryValueEx(Configuration.Address, L"MonoRendering", NULL, &dwType, (LPBYTE)&ManagedSettings.MonoRendering, &dwSize);
		RegQueryValueEx(Configuration.Address, L"MinVelIgnore", NULL, &dwType, (LPBYTE)&ManagedSettings.MinVelIgnore, &dwSize);
		RegQueryValueEx(Configuration.Address, L"MaxVoices", NULL, &dwType, (LPBYTE)&ManagedSettings.MaxVoices, &dwSize);
		RegQueryValueEx(Configuration.Address, L"MaxVelIgnore", NULL, &dwType, (LPBYTE)&ManagedSettings.MaxVelIgnore, &dwSize);
		RegQueryValueEx(Configuration.Address, L"MaxRenderingTime", NULL, &dwType, (LPBYTE)&ManagedSettings.MaxRenderingTime, &dwSize);
		RegQueryValueEx(Configuration.Address, L"MT32Mode", NULL, &dwType, (LPBYTE)&ManagedSettings.MT32Mode, &dwSize);
		RegQueryValueEx(Configuration.Address, L"LimitTo88Keys", NULL, &dwType, (LPBYTE)&ManagedSettings.LimitTo88Keys, &dwSize);
		RegQueryValueEx(Configuration.Address, L"IgnoreSysReset", NULL, &dwType, (LPBYTE)&ManagedSettings.IgnoreSysReset, &dwSize);
		RegQueryValueEx(Configuration.Address, L"IgnoreSysEx", NULL, &dwType, (LPBYTE)&ManagedSettings.IgnoreSysEx, &dwSize);
		RegQueryValueEx(Configuration.Address, L"IgnoreNotesBetweenVel", NULL, &dwType, (LPBYTE)&ManagedSettings.IgnoreNotesBetweenVel, &dwSize);
		RegQueryValueEx(Configuration.Address, L"IgnoreAllEvents", NULL, &dwType, (LPBYTE)&ManagedSettings.IgnoreAllEvents, &dwSize);
		RegQueryValueEx(Configuration.Address, L"HyperPlayback", NULL, &dwType, (LPBYTE)&HyperMode, &dwSize);
		RegQueryValueEx(Configuration.Address, L"GetEvBuffSizeFromRAM", NULL, &dwType, (LPBYTE)&GetEvBuffSizeFromRAM, &dwSize);
		RegQueryValueEx(Configuration.Address, L"FullVelocityMode", NULL, &dwType, (LPBYTE)&ManagedSettings.FullVelocityMode, &dwSize);
		RegQueryValueEx(Configuration.Address, L"FastHotkeys", NULL, &dwType, (LPBYTE)&ManagedSettings.FastHotkeys, &dwSize);
		RegQueryValueEx(Configuration.Address, L"Extra8Lists", NULL, &dwType, (LPBYTE)&ManagedSettings.Extra8Lists, &dwSize);
		RegQueryValueEx(Configuration.Address, L"EvBufferSize", NULL, &qwType, (LPBYTE)&TempEvBufferSize, &qwSize);
		RegQueryValueEx(Configuration.Address, L"EvBufferMultRatio", NULL, &dwType, (LPBYTE)&EvBufferMultRatio, &dwSize);
		RegQueryValueEx(Configuration.Address, L"EnableSFX", NULL, &dwType, (LPBYTE)&ManagedSettings.EnableSFX, &dwSize);
		RegQueryValueEx(Configuration.Address, L"DriverPriority", NULL, &dwType, (LPBYTE)&ManagedSettings.DriverPriority, &dwSize);
		RegQueryValueEx(Configuration.Address, L"DontMissNotes", NULL, &dwType, (LPBYTE)&ManagedSettings.DontMissNotes, &dwSize);
		RegQueryValueEx(Configuration.Address, L"DisableNotesFadeOut", NULL, &dwType, (LPBYTE)&ManagedSettings.DisableNotesFadeOut, &dwSize);
		RegQueryValueEx(Configuration.Address, L"DefaultSFList", NULL, &dwType, (LPBYTE)&ManagedSettings.DefaultSFList, &dwSize);
		RegQueryValueEx(Configuration.Address, L"CurrentEngine", NULL, &dwType, (LPBYTE)&ManagedSettings.CurrentEngine, &dwSize);
		RegQueryValueEx(Configuration.Address, L"CapFramerate", NULL, &dwType, (LPBYTE)&ManagedSettings.CapFramerate, &dwSize);
		RegQueryValueEx(Configuration.Address, L"BufferLength", NULL, &dwType, (LPBYTE)&ManagedSettings.BufferLength, &dwSize);
		RegQueryValueEx(Configuration.Address, L"AudioOutput", NULL, &dwType, (LPBYTE)&ManagedSettings.AudioOutputReg, &dwSize);
		RegQueryValueEx(Configuration.Address, L"AudioFrequency", NULL, &dwType, (LPBYTE)&ManagedSettings.AudioFrequency, &dwSize);
		RegQueryValueEx(Configuration.Address, L"AudioBitDepth", NULL, &dwType, (LPBYTE)&ManagedSettings.AudioBitDepth, &dwSize);

		if (ManagedSettings.CurrentEngine != AUDTOWAV) RegQueryValueEx(Configuration.Address, L"NotesCatcherWithAudio", NULL, &dwType, (LPBYTE)&ManagedSettings.NotesCatcherWithAudio, &dwSize);
		else ManagedSettings.NotesCatcherWithAudio = FALSE;

		if (ManagedSettings.CurrentEngine != ASIO_ENGINE) RegQueryValueEx(Configuration.Address, L"CloseStreamMidiOutClose", NULL, &dwType, (LPBYTE)&CloseStreamMidiOutClose, &dwSize);
		else CloseStreamMidiOutClose = TRUE;

		RegSetValueEx(Configuration.Address, L"LiveChanges", 0, REG_DWORD, (LPBYTE)&Blank, sizeof(Blank));

		// Stuff that works, don't bother
		if (!Between(ManagedSettings.MinVelIgnore, 1, 127)) { ManagedSettings.MinVelIgnore = 1; }
		if (!Between(ManagedSettings.MaxVelIgnore, 1, 127)) { ManagedSettings.MaxVelIgnore = 1; }
		sound_out_volume_float = (float)ManagedSettings.OutputVolume / 10000.0f;

		// Check if "Hyper-playback" mode has been enabled
		if (HyperMode) {
			// It's enabled, do some beeps to notify the user
			Beep(510, 100);
			Beep(640, 100);
			Beep(760, 100);
			Beep(1000, 100);

			// Assign the pointers to the specific hyper-playback functions
			_PrsData = ParseDataHyper;
			_PlayBufData = PlayBufferedDataHyper;
			_PlayBufDataChk = PlayBufferedDataChunkHyper;
		}
		else {
			// It's disabled, assign the pointers to the normal functions
			_PrsData = ParseData;
			_PlayBufData = PlayBufferedData;
			_PlayBufDataChk = PlayBufferedDataChunk;
		}

		PrintToConsole(FOREGROUND_BLUE, 1, "Done loading settings from registry.");
	}
	catch (...) {
		CrashMessage(L"LoadSettings");
	}
}

void LoadSettingsRT() {
	if (!SettingsManagedByClient) {
		try {
			// Initialize the temp values
			DWORD TempSC, TempOV, TempHP, TempMV;
			BOOL TempESFX, TempNOFF1, TempISR, TempSI, TempDNFO, TempDMN;

			// Load the settings
			OpenRegistryKey(Configuration, L"Software\\OmniMIDI\\Configuration");

			RegQueryValueEx(Configuration.Address, L"BufferLength", NULL, &dwType, (LPBYTE)&ManagedSettings.BufferLength, &dwSize);
			RegQueryValueEx(Configuration.Address, L"CapFramerate", NULL, &dwType, (LPBYTE)&ManagedSettings.CapFramerate, &dwSize);
			RegQueryValueEx(Configuration.Address, L"DefaultSFList", NULL, &dwType, (LPBYTE)&ManagedSettings.DefaultSFList, &dwSize);
			RegQueryValueEx(Configuration.Address, L"DisableNotesFadeOut", NULL, &dwType, (LPBYTE)&TempDNFO, &dwSize);
			RegQueryValueEx(Configuration.Address, L"DontMissNotes", NULL, &dwType, (LPBYTE)&TempDMN, &dwSize);
			RegQueryValueEx(Configuration.Address, L"EnableSFX", NULL, &dwType, (LPBYTE)&TempESFX, &dwSize);
			RegQueryValueEx(Configuration.Address, L"FastHotkeys", NULL, &dwType, (LPBYTE)&ManagedSettings.FastHotkeys, &dwSize);
			RegQueryValueEx(Configuration.Address, L"FullVelocityMode", NULL, &dwType, (LPBYTE)&ManagedSettings.FullVelocityMode, &dwSize);
			RegQueryValueEx(Configuration.Address, L"HyperPlayback", NULL, &dwType, (LPBYTE)&TempHP, &dwSize);
			RegQueryValueEx(Configuration.Address, L"IgnoreAllEvents", NULL, &dwType, (LPBYTE)&ManagedSettings.IgnoreAllEvents, &dwSize);
			RegQueryValueEx(Configuration.Address, L"IgnoreNotesBetweenVel", NULL, &dwType, (LPBYTE)&ManagedSettings.IgnoreNotesBetweenVel, &dwSize);
			RegQueryValueEx(Configuration.Address, L"IgnoreSysEx", NULL, &dwType, (LPBYTE)&ManagedSettings.IgnoreSysEx, &dwSize);
			RegQueryValueEx(Configuration.Address, L"IgnoreSysReset", NULL, &dwType, (LPBYTE)&TempISR, &dwSize);
			RegQueryValueEx(Configuration.Address, L"LimitTo88Keys", NULL, &dwType, (LPBYTE)&ManagedSettings.LimitTo88Keys, &dwSize);
			RegQueryValueEx(Configuration.Address, L"LiveChanges", NULL, &dwType, (LPBYTE)&ManagedSettings.LiveChanges, &dwSize);
			RegQueryValueEx(Configuration.Address, L"MT32Mode", NULL, &dwType, (LPBYTE)&ManagedSettings.MT32Mode, &dwSize);
			RegQueryValueEx(Configuration.Address, L"MaxRenderingTime", NULL, &dwType, (LPBYTE)&ManagedSettings.MaxRenderingTime, &dwSize);
			RegQueryValueEx(Configuration.Address, L"MaxVelIgnore", NULL, &dwType, (LPBYTE)&ManagedSettings.MaxVelIgnore, &dwSize);
			RegQueryValueEx(Configuration.Address, L"MaxVoices", NULL, &dwType, (LPBYTE)&TempMV, &dwSize);
			RegQueryValueEx(Configuration.Address, L"MinVelIgnore", NULL, &dwType, (LPBYTE)&ManagedSettings.MinVelIgnore, &dwSize);
			RegQueryValueEx(Configuration.Address, L"NoteOff1", NULL, &dwType, (LPBYTE)&TempNOFF1, &dwSize);
			RegQueryValueEx(Configuration.Address, L"OutputVolume", NULL, &dwType, (LPBYTE)&TempOV, &dwSize);
			RegQueryValueEx(Configuration.Address, L"PreloadSoundFonts", NULL, &dwType, (LPBYTE)&ManagedSettings.PreloadSoundFonts, &dwSize);
			RegQueryValueEx(Configuration.Address, L"SincConv", NULL, &dwType, (LPBYTE)&TempSC, &dwSize);
			RegQueryValueEx(Configuration.Address, L"SincInter", NULL, &dwType, (LPBYTE)&TempSI, &dwSize);
			RegQueryValueEx(Configuration.Address, L"SleepStates", NULL, &dwType, (LPBYTE)&ManagedSettings.SleepStates, &dwSize);
			RegQueryValueEx(Configuration.Address, L"TransposeValue", NULL, &dwType, (LPBYTE)&ManagedSettings.TransposeValue, &dwSize);
			RegQueryValueEx(Configuration.Address, L"VolumeMonitor", NULL, &dwType, (LPBYTE)&ManagedSettings.VolumeMonitor, &dwSize);

			if (ManagedSettings.CurrentEngine != AUDTOWAV) RegQueryValueEx(Configuration.Address, L"NotesCatcherWithAudio", NULL, &dwType, (LPBYTE)&ManagedSettings.NotesCatcherWithAudio, &dwSize);
			else ManagedSettings.NotesCatcherWithAudio = FALSE;

			if (ManagedSettings.CurrentEngine != ASIO_ENGINE) RegQueryValueEx(Configuration.Address, L"CloseStreamMidiOutClose", NULL, &dwType, (LPBYTE)&CloseStreamMidiOutClose, &dwSize);
			else CloseStreamMidiOutClose = TRUE;

			// Stuff that works so don't bother
			if (!Between(ManagedSettings.MinVelIgnore, 1, 127)) { ManagedSettings.MinVelIgnore = 1; }
			if (!Between(ManagedSettings.MaxVelIgnore, 1, 127)) { ManagedSettings.MaxVelIgnore = 1; }

			// Volume
			if (TempOV != ManagedSettings.OutputVolume) {
				ManagedSettings.OutputVolume = TempOV;
				sound_out_volume_float = (float)ManagedSettings.OutputVolume / 10000.0f;
				ChVolumeStruct.fCurrent = 1.0f;
				ChVolumeStruct.fTarget = sound_out_volume_float;
				ChVolumeStruct.fTime = 0.0f;
				ChVolumeStruct.lCurve = 0;
				BASS_FXSetParameters(ChVolume, &ChVolumeStruct);
				CheckUp(ERRORCODE, L"Stream Volume FX Set", FALSE);
			}

			// Check if the value is different from the temporary one
			if (TempDMN != ManagedSettings.DontMissNotes) {
				// It is different, reset the synth
				// to avoid stuck notes or crashes
				ManagedSettings.DontMissNotes = TempDMN;
				ResetSynth(1);
			}

			// Check if the value is different from the temporary one
			if (TempHP != HyperMode) {
				HyperMode = TempHP;

				// Close the threads for safety reasons
				stop_thread = TRUE;

				// Check if "Hyper-playback" mode has been enabled
				if (HyperMode) {

					/* Not required in the real-time settings, but you can remove the comment if you want
					// It's enabled, do some beeps to notify the user
					Beep(510, 100);
					Beep(640, 100);
					Beep(760, 100);
					Beep(1000, 100);
					*/

					// Assign the pointers to the specific hyper-playback functions
					_PrsData = ParseDataHyper;
					_PlayBufData = PlayBufferedDataHyper;
					_PlayBufDataChk = PlayBufferedDataChunkHyper;
				}
				else {
					// It's disabled, assign the pointers to the normal functions
					_PrsData = ParseData;
					_PlayBufData = PlayBufferedData;
					_PlayBufDataChk = PlayBufferedDataChunk;
				}

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
			CrashMessage(L"LoadSettingsRT");
		}
	}
}

void LoadCustomInstruments() {
	OpenRegistryKey(ChanOverride, L"Software\\OmniMIDI\\ChanOverride");

	RegQueryValueEx(ChanOverride.Address, L"overrideinstruments", NULL, &dwType, (LPBYTE)&ManagedSettings.OverrideInstruments, &dwSize);
	for (int i = 0; i <= 15; ++i) {
		// Load the custom bank/instrument for each channel
		RegQueryValueEx(ChanOverride.Address, cbankname[i], NULL, &dwType, (LPBYTE)&cbank[i], &dwSize);
		RegQueryValueEx(ChanOverride.Address, cpresetname[i], NULL, &dwType, (LPBYTE)&cpreset[i], &dwSize);
	}
}

int AudioRenderingType(int value) {
	if (ManagedSettings.CurrentEngine == ASIO_ENGINE) return BASS_SAMPLE_FLOAT;
	else {
		if (value == 1)
			return BASS_SAMPLE_FLOAT;
		else if (value == 2 || value == 0)
			return 0;
		else if (value == 3)
			return BASS_SAMPLE_8BITS;
		else
			return BASS_SAMPLE_FLOAT;
	}
}

void WatchdogCheck() {
	try {
		// Used to check which SoundFont list has been loaded through the configurator
		OpenRegistryKey(Watchdog, L"Software\\OmniMIDI\\Watchdog");

		// Check each value, to see if they're true or not
		for (int i = 0; i <= 15; ++i) {
			RegQueryValueEx(Watchdog.Address, rnames[i], NULL, &dwType, (LPBYTE)&rvalues[i], &dwSize);

			// Value "i" is true, reload the specific SoundFont list
			if (rvalues[i] == 1) {
				LoadSoundfont(i + 1);
				RegSetValueEx(Watchdog.Address, rnames[i], 0, REG_DWORD, (LPBYTE)&Blank, sizeof(Blank));
			}
		}
	}
	catch (...) {
		CrashMessage(L"WatchdogCheck");
	}
}

void CheckVolume(BOOL Closing) {
	try {
		// Self explanatory
		OpenRegistryKey(MainKey, L"Software\\OmniMIDI");

		if (!Closing && !stop_thread) {
			if (ManagedSettings.VolumeMonitor == TRUE && ManagedSettings.CurrentEngine > AUDTOWAV) {
				float levels[2];
				DWORD left, right;

				if (ManagedSettings.CurrentEngine == DSOUND_ENGINE || ManagedSettings.CurrentEngine == WASAPI_ENGINE) {
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
		CrashMessage(L"VolumeMonitor");
	}
}

void FillContentDebug(
	float CCUI0,				// Rendering time
	int HC,						// App's handles
	unsigned long long RUI,		// App's working size/RAM usage
	bool KDMAPI,				// KDMAPI status
	double TD1,					// Thread 1's latency
	double TD2,					// Thread 2's latency
	double TD3,					// Thread 3's latency
	double TD4,					// Thread 4's latency
	double IL,					// ASIO's input latency
	double OL,					// ASIO's output latency
	bool BUFOVD					// EVBuffer overload
) {
	std::locale::global(std::locale::classic());	// DO NOT REMOVE

	std::string PipeContent;
	DWORD bytesWritten;								// Needed for Windows 7 apparently...

	PipeContent += "OMDebugInfo";
	PipeContent += "\nCurrentApp = ";
	PipeContent += modulename;
	PipeContent += "\nBitApp = ";
	PipeContent += bitapp;

	for (int i = 0; i <= 15; ++i) {
		ManagedDebugInfo.ActiveVoices[i] = cvvalues[i];
		PipeContent += "\nCV" + std::to_string(i) + " = " + std::to_string(cvvalues[i]);
	}

	ManagedDebugInfo.RenderingTime = CCUI0;
	PipeContent += "\nCurCPU = " + std::to_string(CCUI0);
	PipeContent += "\nHandles = " + std::to_string(HC);
	PipeContent += "\nRAMUsage = " + std::to_string(RUI);
	PipeContent += "\nOMDirect = " + std::to_string(KDMAPI);
	PipeContent += "\nTd1 = " + std::to_string(TD1);
	PipeContent += "\nTd2 = " + std::to_string(TD2);
	PipeContent += "\nTd3 = " + std::to_string(TD3);
	PipeContent += "\nTd4 = " + std::to_string(TD4);
	PipeContent += "\nASIOInLat = " + std::to_string(IL);
	PipeContent += "\nASIOOutLat = " + std::to_string(OL);
	// PipeContent += "\nBufferOverload = " + std::to_string(BUFOVD);

	PipeContent += "\n\0";

	if (hPipe != INVALID_HANDLE_VALUE) WriteFile(hPipe, PipeContent.c_str(), PipeContent.length(), &bytesWritten, NULL);
	if (GetLastError() != ERROR_SUCCESS && GetLastError() != ERROR_PIPE_LISTENING) StartDebugPipe(TRUE);
}

DOUBLE ASIORate = 0;
DWORD ASIODbgErr;
DWORD ASIOInLatency;
DWORD ASIOOutLatency;
DOUBLE inlatency = 0.0;
DOUBLE outlatency = 0.0;

void ParseDebugData() {

	BASS_ChannelGetAttribute(OMStream, BASS_ATTRIB_CPU, &RenderingTime);

	if (ASIOReady != FALSE && ManagedSettings.CurrentEngine == ASIO_ENGINE) {
		ASIOInLatency = BASS_ASIO_GetLatency(TRUE);
		if (BASS_ASIO_ErrorGetCode() != 0) ASIOInLatency = 0;

		ASIOOutLatency = BASS_ASIO_GetLatency(FALSE);
		if (BASS_ASIO_ErrorGetCode() != 0) ASIOOutLatency = 0;

		ASIORate = BASS_ASIO_GetRate();
		if (BASS_ASIO_ErrorGetCode() != 0) ASIORate = 0.0;

		// CheckUpASIO(ERRORCODE, L"OMGetRateASIO", TRUE);
		if (ASIORate != 0.0) {
			inlatency = (ASIOInLatency != 0) ? ((DOUBLE)ASIOInLatency * 1000.0 / ASIORate) : 0.0;
			outlatency = (ASIOOutLatency != 0) ? ((DOUBLE)ASIOOutLatency * 1000.0 / ASIORate) : 0.0;
		}
	}
	else {
		inlatency = 0.0;
		outlatency = 0.0;
	}

	for (int i = 0; i <= 15; ++i) {
		int temp = BASS_MIDI_StreamGetEvent(OMStream, i, MIDI_EVENT_VOICES);
		if (temp != -1) cvvalues[i] = temp;
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

		long long TimeDuringDebug = TimeNow();

		FillContentDebug(RenderingTime, handlecount, static_cast<QWORD>(pmc.WorkingSetSize), KDMAPIEnabled,
			TimeDuringDebug - start1, TimeDuringDebug - start2, TimeDuringDebug - start3, ManagedSettings.NotesCatcherWithAudio ? 0.0f : TimeDuringDebug - start4,
			inlatency, outlatency, FALSE /* It's supposed to be a buffer overload check */);

		FlushFileBuffers(hPipe);
	}
	catch (...) {
		CrashMessage(L"DebugPipePush");
	}
}

void SendDummyDataToPipe() {
	try {
		FillContentDebug(0.0f, 0, 0, KDMAPIEnabled, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, FALSE);
		FlushFileBuffers(hPipe);
	}
	catch (...) {
		CrashMessage(L"DebugPipeDummyPush");
	}
}

void mixervoid() {
	try {
		OpenRegistryKey(Channels, L"Software\\OmniMIDI\\Channels");

		for (int i = 0; i <= SizeOfArray(cnames); ++i) {
			RegQueryValueEx(Channels.Address, cnames[i], NULL, &dwType, (LPBYTE)&cvalues[i], &dwSize);
			BASS_MIDI_StreamEvent(OMStream, i, MIDI_EVENT_MIXLEVEL, cvalues[i]);
			RegQueryValueEx(Channels.Address, pitchshiftname[i], NULL, &dwType, (LPBYTE)&pitchshiftchan[i], &dwSize);
		}
	}
	catch (...) {
		CrashMessage(L"MixerCheck");
	}
}

void RevbNChor() {
	try {
		BOOL RCOverride = FALSE;
		OpenRegistryKey(Configuration, L"Software\\OmniMIDI\\Configuration");

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
		CrashMessage(L"ReverbAndChorusCheck");
	}
}

void ReloadSFList(DWORD whichsflist){
	try {
		ResetSynth(0);
		Sleep(100);
		LoadSoundfont(whichsflist);
	}
	catch (...) {
		CrashMessage(L"ReloadListCheck");
	}
}

void keybindings()
{
	try {
		if (ManagedSettings.FastHotkeys == 1) {
			BOOL ControlPressed = (GetAsyncKeyState(VK_CONTROL) & (1 << 15));
			if (!ControlPressed && GetAsyncKeyState(VK_MENU) & GetAsyncKeyState(0x31) & 0x8000) {
				ReloadSFList(1);
				return;
			}
			else if (!ControlPressed && GetAsyncKeyState(VK_MENU) & GetAsyncKeyState(0x32) & 0x8000) {
				ReloadSFList(2);
				return;
			}
			else if (!ControlPressed && GetAsyncKeyState(VK_MENU) & GetAsyncKeyState(0x33) & 0x8000) {
				ReloadSFList(3);
				return;
			}
			else if (!ControlPressed && GetAsyncKeyState(VK_MENU) & GetAsyncKeyState(0x34) & 0x8000) {
				ReloadSFList(4);
				return;
			}
			else if (!ControlPressed && GetAsyncKeyState(VK_MENU) & GetAsyncKeyState(0x35) & 0x8000) {
				ReloadSFList(5);
				return;
			}
			else if (!ControlPressed && GetAsyncKeyState(VK_MENU) & GetAsyncKeyState(0x36) & 0x8000) {
				ReloadSFList(6);
				return;
			}
			else if (!ControlPressed && GetAsyncKeyState(VK_MENU) & GetAsyncKeyState(0x37) & 0x8000) {
				ReloadSFList(7);
				return;
			}
			else if (!ControlPressed && GetAsyncKeyState(VK_MENU) & GetAsyncKeyState(0x38) & 0x8000) {
				ReloadSFList(8);
				return;
			}
			if (ManagedSettings.Extra8Lists == 1) {
				if (ControlPressed & GetAsyncKeyState(VK_MENU) & GetAsyncKeyState(0x31) & 0x8000) {
					ReloadSFList(9);
					return;
				}
				else if (ControlPressed & GetAsyncKeyState(VK_MENU) & GetAsyncKeyState(0x32) & 0x8000) {
					ReloadSFList(10);
					return;
				}
				else if (ControlPressed & GetAsyncKeyState(VK_MENU) & GetAsyncKeyState(0x33) & 0x8000) {
					ReloadSFList(11);
					return;
				}
				else if (ControlPressed & GetAsyncKeyState(VK_MENU) & GetAsyncKeyState(0x34) & 0x8000) {
					ReloadSFList(12);
					return;
				}
				else if (ControlPressed & GetAsyncKeyState(VK_MENU) & GetAsyncKeyState(0x35) & 0x8000) {
					ReloadSFList(13);
					return;
				}
				else if (ControlPressed & GetAsyncKeyState(VK_MENU) & GetAsyncKeyState(0x36) & 0x8000) {
					ReloadSFList(14);
					return;
				}
				else if (ControlPressed & GetAsyncKeyState(VK_MENU) & GetAsyncKeyState(0x37) & 0x8000) {
					ReloadSFList(15);
					return;
				}
				else if (ControlPressed & GetAsyncKeyState(VK_MENU) & GetAsyncKeyState(0x38) & 0x8000) {
					ReloadSFList(16);
					return;
				}
			}

			TCHAR configuratorapp[MAX_PATH];
			if (GetAsyncKeyState(VK_MENU) & GetAsyncKeyState(0x39) & 0x8000) {
				if (ManagedSettings.CurrentEngine == ASIO_ENGINE) {
					BASS_ASIO_ControlPanel();
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
				ResetSynth(0);
			}
		}
	}
	catch (...) {
		CrashMessage(L"HotKeysCheck");
	}
}