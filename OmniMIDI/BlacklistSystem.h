/*
OmniMIDI blacklist system
*/

BOOL BannedSystemProcess() {
	// These processes are PERMANENTLY banned because of some internal bugs inside them.
	wchar_t modulename[MAX_PATH];
	memset(modulename, 0, MAX_PATH);
	GetModuleFileNameW(NULL, modulename, MAX_PATH);
	PathStripPathW(modulename);

	// Check if the current process is banned
	for (int i = 0; i < (sizeof(BuiltInBlacklist) / sizeof(BuiltInBlacklist[0])); i++) {
		// It's a match, the process is banned
		if (!_wcsicmp(modulename, BuiltInBlacklist[i])) return TRUE;
	}

	// All good, go on
	return FALSE;
}

BOOL BlackListSystem(){
	// Blacklist system init
	wchar_t defaultstring[MAX_PATH];
	wchar_t userstring[MAX_PATH];
	wchar_t defaultblacklistdirectory[MAX_PATH];
	wchar_t userblacklistdirectory[MAX_PATH];
	wchar_t modulename[MAX_PATH];
	wchar_t fullmodulename[MAX_PATH];

	// Clears all the tchars
	RtlSecureZeroMemory(defaultstring, sizeof(wchar_t) * MAX_PATH);
	RtlSecureZeroMemory(userstring, sizeof(wchar_t) * MAX_PATH);
	RtlSecureZeroMemory(defaultblacklistdirectory, sizeof(wchar_t) * MAX_PATH);
	RtlSecureZeroMemory(userblacklistdirectory, sizeof(wchar_t) * MAX_PATH);
	RtlSecureZeroMemory(modulename, sizeof(wchar_t) * MAX_PATH);
	RtlSecureZeroMemory(fullmodulename, sizeof(wchar_t) * MAX_PATH);

	// Start the system
	SHGetFolderPathW(NULL, CSIDL_SYSTEMX86, NULL, 0, defaultblacklistdirectory);
	wcscat(defaultblacklistdirectory, L"\\OmniMIDI\\OmniMIDI.dbl");
	GetModuleFileNameW(NULL, modulename, MAX_PATH);
	GetModuleFileNameW(NULL, fullmodulename, MAX_PATH);
	PathStripPathW(modulename);

	try {
		if (PathFileExistsW(defaultblacklistdirectory)) {
			std::wifstream file(defaultblacklistdirectory);

			if (file) {
				// The default blacklist exists, continue
				OutputDebugStringW(defaultblacklistdirectory);
				while (file.getline(defaultstring, sizeof(defaultstring) / sizeof(*defaultstring)))
				{
					if (_wcsicmp(modulename, defaultstring) == 0) return DEVICE_UNAVAILABLE;
				}
			}
			else {
				MessageBoxW(NULL, L"The default blacklist is missing, or the driver is not installed properly!\nFatal error, can not continue!\n\nPress OK to quit.", L"OmniMIDI - FATAL ERROR", MB_OK | MB_ICONERROR | MB_SYSTEMMODAL);
				exit(0);
			}
		}
		else {
			MessageBoxW(NULL, L"The default blacklist is missing, or the driver is not installed properly!\nFatal error, can not continue!\n\nPress OK to qu'it.", L"OmniMIDI - FATAL ERROR", MB_OK | MB_ICONERROR | MB_SYSTEMMODAL);
			exit(0);
		}
		if (SUCCEEDED(SHGetFolderPathW(NULL, CSIDL_PROFILE, NULL, 0, userblacklistdirectory))) {
			PathAppendW(userblacklistdirectory, _T("\\OmniMIDI\\blacklist\\OmniMIDI.blacklist"));
			std::wifstream file(userblacklistdirectory);
			OutputDebugStringW(userblacklistdirectory);

			while (file.getline(userstring, sizeof(userstring) / sizeof(*userstring)))
			{
				if (_wcsicmp(modulename, userstring) == 0 ||
					_wcsicmp(fullmodulename, userstring) == 0)
					return DEVICE_UNAVAILABLE;
			}
		}
		return DEVICE_AVAILABLE;
	}
	catch (...) {
		CrashMessage("BlacklistCheckUp");
	}
}

BOOL BlackListInit(){
	// First, the VMS blacklist system, then the main one
	wchar_t modulename[MAX_PATH];
	wchar_t bassmididrv[MAX_PATH];
	wchar_t vmidisynthdll[MAX_PATH];
	wchar_t vmidisynth2exe[MAX_PATH];

	// Clears all the tchars
	RtlSecureZeroMemory(modulename, sizeof(wchar_t) * MAX_PATH);
	RtlSecureZeroMemory(bassmididrv, sizeof(wchar_t) * MAX_PATH);
	RtlSecureZeroMemory(vmidisynthdll, sizeof(wchar_t) * MAX_PATH);
	RtlSecureZeroMemory(vmidisynth2exe, sizeof(wchar_t) * MAX_PATH);

	// Here we go
#if defined(_WIN64)
	SHGetFolderPathW(NULL, CSIDL_SYSTEM, NULL, 0, bassmididrv);
	SHGetFolderPathW(NULL, CSIDL_SYSTEM, NULL, 0, vmidisynthdll);
#elif defined(_WIN32)
	SHGetFolderPathW(NULL, CSIDL_SYSTEMX86, NULL, 0, bassmididrv);
	SHGetFolderPathW(NULL, CSIDL_SYSTEMX86, NULL, 0, vmidisynthdll);
#endif
	SHGetFolderPathW(NULL, CSIDL_SYSTEMX86, NULL, 0, vmidisynth2exe);

	PathAppendW(bassmididrv, _T("\\bassmididrv\\bassmididrv.dll"));
	PathAppendW(vmidisynthdll, _T("\\VirtualMIDISynth\\VirtualMIDISynth.dll"));
	PathAppendW(vmidisynth2exe, _T("\\VirtualMIDISynth\\VirtualMIDISynth.exe"));

	GetModuleFileNameW(NULL, modulename, MAX_PATH);
	PathStripPathW(modulename);

	try {
		if (BannedSystemProcess()) return DEVICE_UNAVAILABLE;
		else {
			if (PathFileExistsW(vmidisynthdll)) {
				if (PathFileExistsW(vmidisynth2exe)) return BlackListSystem();
				else {
					if (!_wcsicmp(modulename, _T("sndvol.exe")))
						return DEVICE_UNAVAILABLE;
					else {
						if (MessageBoxW(0, L"Please uninstall VirtualMIDISynth 1.x before using this driver.\n\nPress No if you want to use OmniMIDI anyway, or Yes to unload it from the application.\n\n(VirtualMIDISynth's outdated DLLs could cause performance degradation while using OmniMIDI)", L"OmniMIDI", MB_YESNO | MB_ICONWARNING | MB_SYSTEMMODAL) == IDYES)
							return DEVICE_UNAVAILABLE;
						else
							return BlackListSystem();
					}
				}
			}
			else if (PathFileExistsW(bassmididrv)) {
				MessageBoxW(0, L"OmniMIDI will refuse to start until you uninstall BASSMIDI Driver.\n\nClick OK to continue.", L"OmniMIDI", MB_OK | MB_ICONERROR | MB_SYSTEMMODAL);
				return DEVICE_UNAVAILABLE;
			}

			return BlackListSystem();
		}
	}
	catch (...) {
		CrashMessage("BlacklistInit");
	}
}