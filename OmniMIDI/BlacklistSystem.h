/*
OmniMIDI blacklist system
*/

BOOL BannedSystemProcess() {
	// These processes are PERMANENTLY banned because of some internal bugs inside them.

	// Check if the current process is banned
	for (int i = 0; i < (sizeof(BuiltInBlacklist) / sizeof(BuiltInBlacklist[0])); i++) {
		// It's a match, the process is banned
		if (!_wcsicmp(AppNameW, BuiltInBlacklist[i])) return TRUE;
	}

	// All good, go on
	return FALSE;
}

BOOL BlackListSystem(TCHAR * SysDir32){
	// If one of the pointers is invalid, the device is available because it has been initialized through KDMAPI
	if (!SysDir32) return DEVICE_AVAILABLE;

	// Blacklist system init
	std::wstring DBLDir;
	std::wstring UBLDir;
	wchar_t UserProfile[MAX_PATH];
	wchar_t TempString[NTFS_MAX_PATH];

	// Clears all the tchars
	RtlZeroMemory(UserProfile, sizeof(UserProfile));
	RtlZeroMemory(TempString, sizeof(TempString));

	// Start the system
	SHGetFolderPathW(NULL, CSIDL_PROFILE, NULL, 0, UserProfile);
	DBLDir.append(SysDir32);
	DBLDir.append(_T("\\OmniMIDI\\OmniMIDI.dbl"));
	UBLDir.append(UserProfile);
	UBLDir.append(_T("\\OmniMIDI\\blacklist\\OmniMIDI.blacklist"));

	try {
		if (PathFileExistsW(DBLDir.c_str())) {
			std::wifstream file(DBLDir.c_str());

			if (file) {
				// The default blacklist exists, continue
				while (file.getline(TempString, sizeof(TempString) / sizeof(*TempString)))
				{
					if (_wcsicmp(AppNameW, TempString) == 0) return DEVICE_UNAVAILABLE;
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
		if (PathFileExistsW(UBLDir.c_str())) {
			std::wifstream file(UBLDir.c_str());

			if (file) {
				while (file.getline(TempString, sizeof(TempString) / sizeof(*TempString)))
				{
					if (_wcsicmp(AppNameW, TempString) == 0 ||
						_wcsicmp(AppPathW, TempString) == 0)
						return DEVICE_UNAVAILABLE;
				}
			}
		}
		return DEVICE_AVAILABLE;
	}
	catch (...) {
		CrashMessage("BlacklistCheckUp");
	}
}

BOOL BlackListInit() {
	// First, the VMS blacklist system, then the main one
	std::wstring BASSMIDIDrv;
	std::wstring VMSDLL;
	std::wstring VMS2EXE;
	wchar_t SysDir32[MAX_PATH];
	wchar_t SysDir64[MAX_PATH];

	RtlZeroMemory(SysDir32, sizeof(SysDir32));
	RtlZeroMemory(SysDir64, sizeof(SysDir64));

	// Here we go
	SHGetFolderPathW(NULL, CSIDL_SYSTEMX86, NULL, 0, SysDir32);
#if defined(_WIN64)
	SHGetFolderPathW(NULL, CSIDL_SYSTEM, NULL, 0, SysDir64);
#elif defined(_WIN32)
	SHGetFolderPathW(NULL, CSIDL_SYSTEMX86, NULL, 0, SysDir64);
#endif

	VMS2EXE.append(SysDir32);
	VMS2EXE.append(_T("\\VirtualMIDISynth\\VirtualMIDISynth.exe"));

	BASSMIDIDrv.append(SysDir64);
	VMSDLL.append(SysDir64);
	BASSMIDIDrv.append(_T("\\bassmididrv\\bassmididrv.dll"));
	VMSDLL.append(_T("\\VirtualMIDISynth\\VirtualMIDISynth.dll"));

	GetAppName();

	try {
		if (BannedSystemProcess()) return DEVICE_UNAVAILABLE;
		else {
			if (PathFileExistsW(VMSDLL.c_str())) {
				if (PathFileExistsW(VMS2EXE.c_str())) return BlackListSystem(SysDir32);
				else {
					if (!_wcsicmp(AppNameW, _T("sndvol.exe")))
						return DEVICE_UNAVAILABLE;
					else {
						if (MessageBoxW(0, L"Please uninstall VirtualMIDISynth 1.x before using this driver.\n\nPress No if you want to use OmniMIDI anyway, or Yes to unload it from the application.\n\n(VirtualMIDISynth's outdated DLLs could cause performance degradation while using OmniMIDI)", L"OmniMIDI", MB_YESNO | MB_ICONWARNING | MB_SYSTEMMODAL) == IDYES)
							return DEVICE_UNAVAILABLE;
						else
							return BlackListSystem(SysDir32);
					}
				}
			}
			else if (PathFileExistsW(BASSMIDIDrv.c_str())) {
				MessageBoxW(0, L"OmniMIDI will refuse to start until you uninstall BASSMIDI Driver.\n\nClick OK to continue.", L"OmniMIDI", MB_OK | MB_ICONERROR | MB_SYSTEMMODAL);
				return DEVICE_UNAVAILABLE;
			}

			return BlackListSystem(SysDir32);
		}
	}
	catch (...) {
		CrashMessage("BlacklistInit");
	}
}