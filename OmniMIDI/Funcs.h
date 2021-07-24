/*
OmniMIDI funcs (this is a mess I know)
*/
#pragma once

// Predefined sleep values, useful for redundancy
#define _FWAIT NTSleep(1)								// Fast wait
#define _WAIT NTSleep(100)								// Normal wait
#define _SWAIT NTSleep(5000)							// Slow wait
#define _CFRWAIT NTSleep(16667)							// Cap framerate wait

static BOOL InfoAlreadyGot = FALSE;

// F**k Sleep() tbh
void NTSleep(__int64 usec) {
	__int64 neg = (usec * -1);
	NtDelayExecution(FALSE, &neg);
}

void GetAppName() {
	if (!InfoAlreadyGot)
	{
		GetModuleFileName(NULL, AppPathW, 32768);
		wcstombs(AppPath, AppPathW, sizeof(AppPath));

		wcsncpy(AppNameW, PathFindFileNameW(AppPathW), MAX_PATH);
		wcstombs(AppName, AppNameW, sizeof(AppName));

		InfoAlreadyGot = TRUE;
	}
}

BOOL GetFolderPath(const GUID FolderID, const int CSIDL, wchar_t* P, size_t PS) {
#ifdef XP
	if (typedef HRESULT(WINAPI* SHGKP)(REFKNOWNFOLDERID, DWORD, HANDLE, PWSTR*); true) {
		SHGKP SHGetKnownFolderPath = (SHGKP)GetProcAddress(GetModuleHandle(L"shell32"), "SHGetKnownFolderPath");

		if (SHGetKnownFolderPath && FolderID != GUID_NULL) {
#endif
			PWSTR Dir;

			if (SUCCEEDED(SHGetKnownFolderPath(FolderID, 0, NULL, &Dir))) {
				swprintf_s(P, PS, L"%s", Dir);
				CoTaskMemFree(Dir);
				return TRUE;
			}

			CoTaskMemFree(Dir);
#ifdef XP
		}
		else {
			LPWSTR Dir;

			if (SUCCEEDED(SHGetFolderPath(NULL, CSIDL, NULL, SHGFP_TYPE_CURRENT, Dir))) {
				swprintf_s(P, PS, L"%s", Dir);
				return TRUE;
			}
		}
	}
#endif

	return FALSE;
}

static VOID MakeMiniDump(LPEXCEPTION_POINTERS exc) {
	TCHAR CurrentTime[MAX_PATH];
	TCHAR DumpDir[MAX_PATH];

	auto hDbgHelp = LoadLibraryA("dbghelp");
	if (hDbgHelp == nullptr)
		return;
	auto pMiniDumpWriteDump = (decltype(&MiniDumpWriteDump))GetProcAddress(hDbgHelp, "MiniDumpWriteDump");
	if (pMiniDumpWriteDump == nullptr)
		return;

	// Get the debug info first
	GetAppName();

	// Get user profile's path
	GetFolderPath(FOLDERID_Profile, CSIDL_PROFILE, DumpDir, sizeof(DumpDir));

	// Append "\OmniMIDI\dumpfiles\" to "%userprofile%"
	wcscat_s(DumpDir, MAX_PATH, L"\\OmniMIDI\\dumpfiles\\");

	// Create "%userprofile%\OmniMIDI\dumpfiles\", in case it doesn't exist
	CreateDirectory(DumpDir, NULL);

	// Append the app's filename to the output file's path
	wcscat_s(DumpDir, MAX_PATH, AppNameW);

	// Parse current time, and append it
	struct tm* sTm;
	time_t now = time(0);
	sTm = gmtime(&now);
	wcsftime(CurrentTime, sizeof(CurrentTime), L" - %d-%m-%Y %H.%M.%S", sTm);
	wcscat_s(DumpDir, MAX_PATH, CurrentTime);

	// Append file extension, and that's it
	wcscat_s(DumpDir, MAX_PATH, _T(" (OmniMIDI Minidump).mdmp"));

	auto hFile = CreateFileW(DumpDir, GENERIC_WRITE, FILE_SHARE_READ, 0, CREATE_ALWAYS, FILE_ATTRIBUTE_NORMAL, 0);
	if (hFile == INVALID_HANDLE_VALUE)
		return;

	MINIDUMP_EXCEPTION_INFORMATION exceptionInfo;
	exceptionInfo.ThreadId = GetCurrentThreadId();
	exceptionInfo.ExceptionPointers = exc;
	exceptionInfo.ClientPointers = FALSE;

	auto dumped = pMiniDumpWriteDump(
		GetCurrentProcess(),
		GetCurrentProcessId(),
		hFile,
		MINIDUMP_TYPE(MiniDumpWithFullMemory | MiniDumpIgnoreInaccessibleMemory),
		exc ? &exceptionInfo : nullptr,
		NULL,
		NULL);

	CloseHandle(hFile);
	return;
}

void CrashMessage(LPCWSTR part) {
	WCHAR ErrorMessage[32768] = { 0 };
	DWORD ErrorID = GetLastError();

	fwprintf(DebugLog, L"(Error at \"%s\", Code 0x%08x) - Fatal error during the execution of the driver.", part, ErrorID);

	swprintf(ErrorMessage, L"An error has been detected while executing the following function: %s\n", part);

	//Get the error message, if any.
	if (ErrorID != 0) {
		TCHAR* ERR;
		if (FormatMessageW(FORMAT_MESSAGE_ALLOCATE_BUFFER | FORMAT_MESSAGE_FROM_SYSTEM,
			NULL,
			ErrorID,
			MAKELANGID(LANG_NEUTRAL, SUBLANG_DEFAULT),
			(LPWSTR)&ERR,
			0,
			NULL))
		{
			swprintf(
				ErrorMessage + wcslen(ErrorMessage),
				L"\nError code: 0x%08X - %s\nPlease take a screenshot of this messagebox (ALT+PRINT), and create a GitHub issue.\n",
				ErrorID, ERR
			);

			LocalFree(ERR);
		}
	}

	swprintf(
		ErrorMessage + wcslen(ErrorMessage),
		L"\nClick OK to close the program."
	);

	MessageBoxW(NULL, ErrorMessage, L"OmniMIDI - Fatal execution error", MB_ICONERROR | MB_SYSTEMMODAL);

	block_bassinit = TRUE;
	stop_svthread = TRUE;

	MakeMiniDump(nullptr);

	exit(ErrorID);
}