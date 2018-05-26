/*
Keppy's Synthesizer blacklist system
*/

BOOL BlackListSystem(){
	// Blacklist system init
	TCHAR defaultstring[MAX_PATH];
	TCHAR userstring[MAX_PATH];
	TCHAR defaultblacklistdirectory[MAX_PATH];
	TCHAR userblacklistdirectory[MAX_PATH];
	TCHAR modulename[MAX_PATH];
	TCHAR fullmodulename[MAX_PATH];
	// Clears all the tchars
	ZeroMemory(defaultstring, MAX_PATH);
	ZeroMemory(userstring, MAX_PATH);
	ZeroMemory(defaultblacklistdirectory, MAX_PATH);
	ZeroMemory(userblacklistdirectory, MAX_PATH);
	ZeroMemory(modulename, MAX_PATH);
	ZeroMemory(fullmodulename,MAX_PATH);
	// Start the system
	SHGetFolderPath(NULL, CSIDL_SYSTEMX86, NULL, 0, defaultblacklistdirectory);
	_tcscat(defaultblacklistdirectory, L"\\keppysynth\\keppysynth.dbl");
	GetModuleFileName(NULL, modulename, MAX_PATH);
	GetModuleFileName(NULL, fullmodulename, MAX_PATH);
	PathStripPath(modulename);
	try {
		if (PathFileExists(defaultblacklistdirectory)) {
			std::wifstream file(defaultblacklistdirectory);
			if (file) {
				// The default blacklist exists, continue
				OutputDebugString(defaultblacklistdirectory);
				while (file.getline(defaultstring, sizeof(defaultstring) / sizeof(*defaultstring)))
				{
					if (_tcsicmp(modulename, defaultstring) == 0) return 0x0;
				}
			}
			else {
				MessageBox(NULL, L"The default blacklist is missing, or the driver is not installed properly!\nFatal error, can not continue!\n\nPress OK to quit.", L"Keppy's Synthesizer - FATAL ERROR", MB_OK | MB_ICONERROR | MB_SYSTEMMODAL);
				exit(0);
			}
		}
		else {
			MessageBox(NULL, L"The default blacklist is missing, or the driver is not installed properly!\nFatal error, can not continue!\n\nPress OK to qu'it.", L"Keppy's Synthesizer - FATAL ERROR", MB_OK | MB_ICONERROR | MB_SYSTEMMODAL);
			exit(0);
		}
		if (SUCCEEDED(SHGetFolderPath(NULL, CSIDL_PROFILE, NULL, 0, userblacklistdirectory))) {
			HKEY hKey;
			long lResult;
			DWORD dwType = REG_DWORD;
			DWORD dwSize = sizeof(DWORD);
			lResult = RegOpenKeyEx(HKEY_CURRENT_USER, L"Software\\Keppy's Synthesizer\\Configuration", 0, KEY_ALL_ACCESS, &hKey);
			RegQueryValueEx(hKey, L"NoBlacklistMessage", NULL, &dwType, (LPBYTE)&NoBlacklistMessage, &dwSize);
			RegCloseKey(hKey);

			PathAppend(userblacklistdirectory, _T("\\Keppy's Synthesizer\\blacklist\\keppymididrv.blacklist"));
			std::wifstream file(userblacklistdirectory);
			OutputDebugString(userblacklistdirectory);
			while (file.getline(userstring, sizeof(userstring) / sizeof(*userstring)))
			{
				if (_tcsicmp(modulename, userstring) == 0 || _tcsicmp(fullmodulename, userstring) == 0) {
					if (NoBlacklistMessage != 1) {
						std::wstring modulenamelpcwstr(modulename);
						std::wstring concatted_stdstr = L"Keppy's Synthesizer - " + modulenamelpcwstr + L" is blacklisted";
						LPCWSTR messageboxtitle = concatted_stdstr.c_str();
						MessageBox(NULL, L"This program has been manually blacklisted.\nThe driver will be automatically unloaded.\n\nPress OK to continue.", messageboxtitle, MB_OK | MB_ICONEXCLAMATION | MB_SYSTEMMODAL);
					}
					return 0x0;
				}
			}
		}
		return 0x1;
	}
	catch (...) {
		CrashMessage(L"BlacklistCheckUp");
		throw;
	}
}

BOOL BlackListInit(){
	// First, the VMS blacklist system, then the main one
	TCHAR modulename[MAX_PATH];
	TCHAR sndvol[MAX_PATH];
	TCHAR bassmididrv[MAX_PATH];
	TCHAR vmidisynthdll[MAX_PATH];
	TCHAR vmidisynth2exe[MAX_PATH];
	// Clears all the tchars
	ZeroMemory(modulename, sizeof(TCHAR) * MAX_PATH);
	ZeroMemory(sndvol, sizeof(TCHAR) * MAX_PATH);
	ZeroMemory(bassmididrv, sizeof(TCHAR) * MAX_PATH);
	ZeroMemory(vmidisynthdll, sizeof(TCHAR) * MAX_PATH);
	ZeroMemory(vmidisynth2exe, sizeof(TCHAR) * MAX_PATH);
	// Here we go
#if defined(_WIN64)
	SHGetFolderPath(NULL, CSIDL_SYSTEM, NULL, 0, bassmididrv);
	SHGetFolderPath(NULL, CSIDL_SYSTEM, NULL, 0, vmidisynthdll);
	SHGetFolderPath(NULL, CSIDL_SYSTEMX86, NULL, 0, vmidisynth2exe);
#elif defined(_WIN32)
	SHGetFolderPath(NULL, CSIDL_SYSTEMX86, NULL, 0, bassmididrv);
	SHGetFolderPath(NULL, CSIDL_SYSTEMX86, NULL, 0, vmidisynthdll);
	SHGetFolderPath(NULL, CSIDL_SYSTEMX86, NULL, 0, vmidisynth2exe);
#endif
	PathAppend(bassmididrv, _T("\\bassmididrv\\bassmididrv.dll"));
	PathAppend(vmidisynthdll, _T("\\VirtualMIDISynth\\VirtualMIDISynth.dll"));
	PathAppend(vmidisynth2exe, _T("\\VirtualMIDISynth\\VirtualMIDISynth.exe"));
	GetModuleFileName(NULL, modulename, MAX_PATH);
	PathStripPath(modulename);
	// Lel stuff
	_tcscpy_s(sndvol, _countof(sndvol), _T("sndvol.exe"));
	try {
		if (PathFileExists(vmidisynthdll)) {
			if (PathFileExists(vmidisynth2exe)) return BlackListSystem();
			else {
				if (!_tcsicmp(modulename, sndvol))
					return 0x0;
				else {
					if (MessageBox(0, L"Please uninstall VirtualMIDISynth 1.x before using this driver.\n\nPress No if you want to use Keppy's Synthesizer anyway, or Yes to unload it from the application.\n\n(VirtualMIDISynth's outdated DLLs could cause performance degradation while using Keppy's Synthesizer)", L"Keppy's Synthesizer", MB_YESNO | MB_ICONWARNING | MB_SYSTEMMODAL) == IDYES)
						return 0x0;
					else 
						return BlackListSystem();
				}
			}
		}
		else if (PathFileExists(bassmididrv)) {
			MessageBox(0, L"Keppy's Synthesizer will refuse to start until you uninstall BASSMIDI Driver.\n\nClick OK to continue.", L"Keppy's Synthesizer", MB_OK | MB_ICONERROR | MB_SYSTEMMODAL);
			return 0x0;
		}
		return BlackListSystem();
	}
	catch (...) {
		CrashMessage(L"BlacklistInit");
		throw;
	}
}

BOOL BannedSystemProcess() {
	// These processes are PERMANENTLY banned because of some internal bugs inside them.
	TCHAR modulename[MAX_PATH];
	GetModuleFileName(NULL, modulename, MAX_PATH);

	for (int i = 0; i < SizeOfArray(builtinblacklist); i++) {
		if (!_tcsicmp(modulename, builtinblacklist[i])) return TRUE;
	}

	return FALSE;
}