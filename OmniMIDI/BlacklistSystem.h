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

BOOL BlackListSystem(){
	// If the process is in the permanent ban list, return 0
	if (BannedSystemProcess()) return DEVICE_UNAVAILABLE;

	// Blacklist system init
	std::wstring DBLDir;
	std::wstring UBLDir;
	wchar_t SysDir32[MAX_PATH] = { 0 };
	wchar_t UserProfile[MAX_PATH] = { 0 };
	wchar_t TempString[NTFS_MAX_PATH] = { 0 };

	// Start the system
	SHGetFolderPathW(NULL, CSIDL_SYSTEMX86, NULL, 0, SysDir32);
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