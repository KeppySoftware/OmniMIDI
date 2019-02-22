/*
OmniMIDI blacklist system
*/

static const std::locale UTF8Support(std::locale(), new std::codecvt_utf8<wchar_t>);

static const LPCWSTR BuiltInBlacklist[] =
{
	L"Battle.net Launcher.exe",
	L"Discord.exe",
	L"DiscordCanary.exe",
	L"Fortnite.exe",
	L"ICEsoundService64.exe",
	L"LogonUI.exe",
	L"NVDisplay.Container.exe",
	L"NVIDIA Share.exe",
	L"NVIDIA Web Helper.exe",
	L"RainbowSix.exe",
	L"RuntimeBroker.exe",
	L"RustClient.exe",
	L"SearchUI.exe",
	L"SecurityHealthService.exe",
	L"SecurityHealthSystray.exe",
	L"ShellExperienceHost.exe",
	L"SndVol.exe",
	L"WUDFHost.exe",
	L"conhost.exe",
	L"consent.exe",
	L"csrss.exe",
	L"ctfmon.exe",
	L"dwm.exe",
	L"explorer.exe",
	L"fontdrvhost.exe",
	L"lsass.exe",
	L"mstsc.exe",
	L"nvcontainer.exe",
	L"nvsphelper64.exe",
	L"smss.exe",
	L"spoolsv.exe",
	L"vcpkgsrv.exe",
	L"vmware-hostd.exe",
	L"wininit.exe",
	L"winlogon.exe"
};

BOOL BannedProcesses() {
	memset(AppPath, 0, sizeof(AppPath));
	memset(AppName, 0, sizeof(AppName));

	GetModuleFileNameW(NULL, AppPath, 32767);

	LPWSTR TempPoint = PathFindFileNameW(AppPath);
	lstrcpyW(AppName, TempPoint);

	for (int i = 0; i < (sizeof(BuiltInBlacklist) / sizeof(*BuiltInBlacklist)); i++) {
		// It's a match, the process is banned
		if (!lstrcmpiW(AppName, BuiltInBlacklist[i])) return TRUE;
	}

	return FALSE;
}

BOOL BlackListSystem() {
	// Blacklist system init
	std::wstring DBLDir;
	std::wstring UBLDir;
	wchar_t SysDir32[MAX_PATH] = { 0 };
	wchar_t UserProfile[MAX_PATH] = { 0 };
	wchar_t TempString[32767] = { 0 };

	// Start the system
	SHGetFolderPathW(NULL, CSIDL_SYSTEMX86, NULL, 0, SysDir32);
	SHGetFolderPathW(NULL, CSIDL_PROFILE, NULL, 0, UserProfile);

	DBLDir.append(SysDir32);
	DBLDir.append(L"\\OmniMIDI\\OmniMIDI.dbl");
	UBLDir.append(UserProfile);
	UBLDir.append(L"\\OmniMIDI\\blacklist\\OmniMIDI.blacklist");

	try {
		if (PathFileExistsW(DBLDir.c_str())) {
			std::wifstream file(DBLDir.c_str());

			if (file) {
				file.imbue(UTF8Support);

				// The default blacklist exists, continue
				while (file.getline(TempString, sizeof(TempString) / sizeof(*TempString)))
				{
					if (_wcsicmp(AppName, TempString) == 0) return TRUE;
				}
			}
			else {
				MessageBoxW(NULL, L"Failed to parse the default blacklist!\nThe driver will refuse to run.\n\nPlease reinstall OmniMIDI to restore it.\nPress OK to continue.", L"OmniMIDI - ERROR", MB_OK | MB_ICONEXCLAMATION | MB_SYSTEMMODAL);
				return TRUE;
			}
		}
		else {
			MessageBoxW(NULL, L"The default blacklist is missing, or the driver is not installed properly!\nThe driver will refuse to run.\n\nPlease reinstall OmniMIDI to restore it.\nPress OK to continue.", L"OmniMIDI - ERROR", MB_OK | MB_ICONEXCLAMATION | MB_SYSTEMMODAL);
			return TRUE;
		}

		if (PathFileExistsW(UBLDir.c_str())) {
			std::wifstream file(UBLDir.c_str());

			if (file) {
				file.imbue(UTF8Support);
				while (file.getline(TempString, sizeof(TempString) / sizeof(*TempString)))
				{
					if (_wcsicmp(AppName, TempString) == 0 ||
						_wcsicmp(AppPath, TempString) == 0)
						return TRUE;
				}
			}
		}

		return FALSE;
	}
	catch (...) { return TRUE; }
}