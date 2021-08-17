/*
OmniMIDI blacklist system
*/

BOOL BannedProcesses() {
	// These processes are PERMANENTLY banned because of some internal bugs inside them.
	GetAppName();

	// Check if the current process is banned
	for (int i = 0; i < (sizeof(BuiltInBlacklist) / sizeof(*BuiltInBlacklist)); i++) {
		// It's a match, the process is banned
		if (!_wcsicmp(AppNameW, BuiltInBlacklist[i])) return TRUE;
	}

	// Also check if it's blacklisted from using CookedPlayer
	for (int i = 0; i < (sizeof(CookedPlayerBlacklist) / sizeof(*CookedPlayerBlacklist)); i++) {
		// It's a match, the process is banned
		if (!_wcsicmp(AppNameW, CookedPlayerBlacklist[i])) CPBlacklisted = TRUE;
	}

	// All good, go on
	return FALSE;
}

DWORD BlackListSystem(){
	// Blacklist system init
	std::wstring DBLDir;
	std::wstring UBLDir;

	wchar_t SysDir32[MAX_PATH] = { 0 };
	wchar_t UserProfile[MAX_PATH] = { 0 };
	wchar_t TempString[NTFS_MAX_PATH] = { 0 };

	// Start the system
	if (!GetFolderPath(FOLDERID_SystemX86, CSIDL_SYSTEMX86, SysDir32, sizeof(SysDir32)))
		return DEVICE_UNAVAILABLE;
	
	if (!GetFolderPath(FOLDERID_Profile, CSIDL_PROFILE, UserProfile, sizeof(UserProfile))) 
		return DEVICE_UNAVAILABLE;

	DBLDir.append(SysDir32);
	DBLDir.append(_T("\\OmniMIDI\\OmniMIDI.dbl\0"));
	UBLDir.append(UserProfile);
	UBLDir.append(_T("\\OmniMIDI\\blacklist\\OmniMIDI.blacklist\0"));

	try {
		if (PathFileExistsW(DBLDir.c_str())) {
			std::wifstream file(DBLDir.c_str());

			if (file) {
				file.imbue(UTF8Support);

				// The default blacklist exists, continue
				while (file.getline(TempString, sizeof(TempString) / sizeof(*TempString)))
				{
					if (_wcsicmp(AppNameW, TempString) == 0) return DEVICE_UNAVAILABLE;
				}
			}
			else {
				MessageBoxW(NULL, L"Failed to parse the default blacklist!\nThe driver will refuse to run.\n\nPlease reinstall OmniMIDI to restore it.\nPress OK to continue.", L"OmniMIDI - ERROR", MB_OK | MB_ICONEXCLAMATION | MB_SYSTEMMODAL);
				return DEVICE_UNAVAILABLE;
			}
		}
		else {
			MessageBoxW(NULL, L"The default blacklist is missing, or the driver is not installed properly!\nThe driver will refuse to run.\n\nPlease reinstall OmniMIDI to restore it.\nPress OK to continue.", L"OmniMIDI - ERROR", MB_OK | MB_ICONEXCLAMATION | MB_SYSTEMMODAL);
			return DEVICE_UNAVAILABLE;
		}

		if (PathFileExistsW(UBLDir.c_str())) {
			std::wifstream file(UBLDir.c_str());

			if (file) {
				file.imbue(UTF8Support);
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
		_THROWCRASH;
	}
}

BOOL MIDIFeedbackWhitelist() {
	// Blacklist system init
	std::wstring UBLDir;

	wchar_t UserProfile[MAX_PATH] = { 0 };
	wchar_t TempString[NTFS_MAX_PATH] = { 0 };

	// Start the system
	GetFolderPath(FOLDERID_Profile, CSIDL_PROFILE, UserProfile, sizeof(UserProfile));

	UBLDir.append(UserProfile);
	UBLDir.append(_T("\\OmniMIDI\\MIDIFeedback.whitelist"));

	try {
		if (PathFileExistsW(UBLDir.c_str())) {
			std::wifstream file(UBLDir.c_str());

			if (file) {
				file.imbue(UTF8Support);
				while (file.getline(TempString, sizeof(TempString) / sizeof(*TempString)))
				{
					if (_wcsicmp(AppNameW, TempString) == 0 ||
						_wcsicmp(AppPathW, TempString) == 0)
						return TRUE;
				}
			}
		}

		return FALSE;
	}
	catch (...) {
		_THROWCRASH;
	}
}