/*
Keppy's Driver watchdog system
*/

void RunWatchdog()
{
	try
	{
		HKEY hKey;
		long lResult;
		TCHAR watchdog[MAX_PATH];
		DWORD dwType = REG_DWORD;
		DWORD dwSize = sizeof(DWORD);
		DWORD one = 1;
		DWORD zero = 0;
		lResult = RegOpenKeyEx(HKEY_CURRENT_USER, L"Software\\Keppy's Driver\\Watchdog", 0, KEY_ALL_ACCESS, &hKey);
		RegSetValueEx(hKey, L"closewatchdog", 0, dwType, (LPBYTE)&zero, sizeof(zero));
		RegSetValueEx(hKey, L"wdrun", 0, dwType, (LPBYTE)&one, sizeof(one));
		if (SUCCEEDED(SHGetFolderPath(NULL, CSIDL_SYSTEMX86, NULL, 0, watchdog)))
		{
			PathAppend(watchdog, _T("\\keppydrv\\KeppyDriverWatchdog.exe"));
			ShellExecute(NULL, L"open", watchdog, NULL, NULL, SW_SHOWNORMAL);
		}
		RegCloseKey(hKey);
	}
	catch (int e)
	{

	}
}

void KillWatchdog()
{
	try
	{
		HANDLE hSnapShot = CreateToolhelp32Snapshot(TH32CS_SNAPALL, NULL);
		PROCESSENTRY32W pEntry;
		pEntry.dwSize = sizeof(pEntry);
		BOOL hRes = Process32First(hSnapShot, &pEntry);
		HKEY hKey;
		long lResult;
		DWORD dwType = REG_DWORD;
		DWORD dwSize = sizeof(DWORD);
		DWORD one = 1;
		DWORD zero = 0;
		lResult = RegOpenKeyEx(HKEY_CURRENT_USER, L"Software\\Keppy's Driver\\Watchdog", 0, KEY_ALL_ACCESS, &hKey);
		RegSetValueEx(hKey, L"closewatchdog", 0, dwType, (LPBYTE)&one, sizeof(one));
		RegSetValueEx(hKey, L"wdrun", 0, dwType, (LPBYTE)&zero, sizeof(zero));
		RegCloseKey(hKey);
		while (hRes)
		{
			if (wcscmp(pEntry.szExeFile, L"KeppyDriverWatchdog.exe") == 0)
			{
				HANDLE hProcess = OpenProcess(PROCESS_TERMINATE, 0,
					(DWORD)pEntry.th32ProcessID);
				if (hProcess != NULL)
				{
					CloseHandle(hProcess);
				}
			}
			hRes = Process32Next(hSnapShot, &pEntry);
		}
		CloseHandle(hSnapShot);
	}
	catch (int e)
	{

	}
}