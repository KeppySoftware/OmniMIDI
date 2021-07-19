/*
OmniMIDI debug functions
*/

#define CurrentError(Message, Error, Text) case Error: sprintf_s(Message, NTFS_MAX_PATH, "Error %s:\n%s", #Error, #Text); break
#define DefError(Message, Error, Text) sprintf_s(Message, NTFS_MAX_PATH, "Error %s:\n%s", #Error, #Text); break
#define arre(wat) case wat: sprintf(MessageBuf + strlen(MessageBuf), "\nCode: %s", #wat); break

static HANDLE ExceptionHandler = nullptr;
static const char hex[] = "0123456789ABCDEF";
static std::mutex DebugMutex;
static BOOL IntroAlreadyShown = FALSE;
static BOOL InfoAlreadyGot = FALSE;

double GetThreadUsage(Thread* Thread) {
	if (WaitForSingleObject(Thread->ThreadHandle, 0) == WAIT_OBJECT_0) return 0.0;

	SYSTEM_INFO sysInfo;
	FILETIME ftime, fsys, fuser;
	ULARGE_INTEGER now, sys, user;
	double percent;

	GetSystemTimeAsFileTime(&ftime);
	memcpy(&now, &ftime, sizeof(FILETIME));

	GetThreadTimes(Thread->ThreadHandle, &ftime, &ftime, &fsys, &fuser);
	GetSystemInfo(&sysInfo);
	CPUThreadsAvailable = sysInfo.dwNumberOfProcessors;

	memcpy(&sys, &fsys, sizeof(FILETIME));
	memcpy(&user, &fuser, sizeof(FILETIME));
	percent = (sys.QuadPart - Thread->KernelCPU.QuadPart) +
		(user.QuadPart - Thread->UserCPU.QuadPart);
	percent /= (now.QuadPart - Thread->CPU.QuadPart);
	percent /= CPUThreadsAvailable;
	Thread->CPU = now;
	Thread->UserCPU = user;
	Thread->KernelCPU = sys;

	return percent * 100;
}

void GetAppName() {
	if (!InfoAlreadyGot)
	{
		GetModuleFileName(NULL, AppPathW, NTFS_MAX_PATH);
		wcstombs(AppPath, AppPathW, sizeof(AppPath));

		wcsncpy(AppNameW, PathFindFileNameW(AppPathW), MAX_PATH);
		wcstombs(AppName, AppNameW, sizeof(AppName));

		InfoAlreadyGot = TRUE;
	}
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
	SHGetFolderPath(NULL, CSIDL_PROFILE, NULL, 0, DumpDir);

	// Append "\OmniMIDI\dumpfiles\" to "%userprofile%"
	wcscat_s(DumpDir, MAX_PATH, L"\\OmniMIDI\\dumpfiles\\");

	// Create "%userprofile%\OmniMIDI\dumpfiles\", in case it doesn't exist
	CreateDirectory(DumpDir, NULL);

	// Append the app's filename to the output file's path
	wcscat_s(DumpDir, MAX_PATH, AppNameW);

	// Parse current time, and append it
	struct tm *sTm;
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
	WCHAR ErrorMessage[NTFS_MAX_PATH] = { 0 };
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

bool GetVersionInfo(
	LPCTSTR filename,
	int &major,
	int &minor,
	int &build,
	int &revision)
{
	DWORD   verBufferSize;
	char    verBuffer[2048];

	//  Get the size of the version info block in the file
	verBufferSize = GetFileVersionInfoSize(filename, NULL);
	if (verBufferSize > 0 && verBufferSize <= sizeof(verBuffer))
	{
		//  get the version block from the file
		if (TRUE == GetFileVersionInfo(filename, NULL, verBufferSize, verBuffer))
		{
			UINT length;
			VS_FIXEDFILEINFO *verInfo = NULL;

			//  Query the version information for neutral language
			if (TRUE == VerQueryValue(
				verBuffer,
				_T("\\"),
				reinterpret_cast<LPVOID*>(&verInfo),
				&length))
			{
				//  Pull the version values.
				major = HIWORD(verInfo->dwProductVersionMS);
				minor = LOWORD(verInfo->dwProductVersionMS);
				build = HIWORD(verInfo->dwProductVersionLS);
				revision = LOWORD(verInfo->dwProductVersionLS);
				return true;
			}
		}
	}

	return false;
}

void CreateConsole() {
	if (!IntroAlreadyShown) {
		TCHAR* MainLibrary = (TCHAR*)malloc(sizeof(TCHAR) * NTFS_MAX_PATH);
		TCHAR* DebugDir = (TCHAR*)malloc(sizeof(TCHAR) * NTFS_MAX_PATH);

		// Get the debug info first
		GetAppName();

		// Get user profile's path
		SHGetFolderPathW(NULL, CSIDL_PROFILE, NULL, 0, DebugDir);

		// Append "\OmniMIDI\debug\" to "%userprofile%"
		wcscat_s(DebugDir, NTFS_MAX_PATH, L"\\OmniMIDI\\debug\\");

		// Create "%userprofile%\OmniMIDI\debug\", in case it doesn't exist
		CreateDirectoryW(DebugDir, NULL);

		// Append the app's filename to the output file's path
		wcscat_s(DebugDir, NTFS_MAX_PATH, AppNameW);
		wcscat_s(DebugDir, NTFS_MAX_PATH, _T(" (Debug output).txt"));

		// Parse OmniMIDI's current version
		GetModuleFileNameW(hinst, MainLibrary, NTFS_MAX_PATH);
		PathRemoveFileSpecW(MainLibrary);
		wcscat_s(MainLibrary, NTFS_MAX_PATH, L"\\OmniMIDI.dll");
		int major, minor, build, revision;
		GetVersionInfo(MainLibrary, major, minor, build, revision);

		// Open the debug output's file
		if (ManagedSettings.DebugMode)
		{
			DebugLog = _wfopen(DebugDir, L"a+");

			std::lock_guard<std::mutex> lock(DebugMutex);

			// Begin writing to it
			fprintf(DebugLog, "=======================================\n");
			fprintf(DebugLog, "OmniMIDI %d.%d.%d ", major, minor, build);
			if (revision) fprintf(DebugLog, "CR%d ", revision);
			fprintf(DebugLog, "(KDMAPI %d.%d.%d, Revision %d)\n", CUR_MAJOR, CUR_MINOR, CUR_BUILD, CUR_REV);
			fprintf(DebugLog, "%d threads available to the ASIO engine\n", std::thread::hardware_concurrency());
			fprintf(DebugLog, "Copyright(C) 2013 - KaleidonKep99\n\n");
			IntroAlreadyShown = TRUE;
		}

		free(MainLibrary);
		free(DebugDir);
	}
}

inline bool DebugFileExists(const std::string& name) {
	// Check if the debug file exists
	if (FILE *file = fopen(name.c_str(), "r")) {
		// It does, close it and return true
		fclose(file);
		return true;
	}

	return false;
}

void PrintCurrentTime() {
	// Get time
	SYSTEMTIME stime;
	FILETIME ltime;
	FILETIME ftTimeStamp;

	GetSystemTimeAsFileTime(&ftTimeStamp); //Gets the current system time
	FileTimeToLocalFileTime(&ftTimeStamp, &ltime); //convert in local time and store in ltime
	FileTimeToSystemTime(&ltime, &stime); //convert in system time and store in stime

	// Print to log
	fprintf(DebugLog, "%02d-%02d-%04d %02d:%02d:%02d.%03d - ",
		stime.wDay, stime.wMonth, stime.wYear, stime.wHour, stime.wMinute, stime.wSecond, stime.wMilliseconds);
}

void PrintCurrentTimeW() {
	// Get time
	SYSTEMTIME stime;
	FILETIME ltime;
	FILETIME ftTimeStamp;

	GetSystemTimeAsFileTime(&ftTimeStamp); //Gets the current system time
	FileTimeToLocalFileTime(&ftTimeStamp, &ltime); //convert in local time and store in ltime
	FileTimeToSystemTime(&ltime, &stime); //convert in system time and store in stime

	// Print to log
	fwprintf(DebugLog, L"%02d-%02d-%04d %02d:%02d:%02d.%03d - ",
		stime.wDay, stime.wMonth, stime.wYear, stime.wHour, stime.wMinute, stime.wSecond, stime.wMilliseconds);
}

void PrintMMToDebugLog(UINT uDID, UINT uMsg, DWORD_PTR dwUser, DWORD_PTR dwParam1, DWORD_PTR dwParam2) {
	if (ManagedSettings.DebugMode) {
		char* Msg = (char*)malloc(sizeof(char) * NTFS_MAX_PATH);

		// Debug log is busy now
		std::lock_guard<std::mutex> lock(DebugMutex);

		// Print to log
		PrintCurrentTime();
		try {
			sprintf(Msg, "Stage <<modMessage>> | uDeviceID-> %d, uMsg-> %d, dwUser-> %d (LPVOID: %d), dwParam1-> %d, dwParam2-> %d", 
				uDID, uMsg, (DWORD)dwUser, *(DWORD_PTR*)dwUser, (DWORD)dwParam1, (DWORD)dwParam2);
		}
		catch (...) {
			sprintf(Msg, "Stage <<modMessage>> | uDeviceID-> %d, uMsg-> %d, dwUser-> %d (LPVOID: FAIL), dwParam1-> %d, dwParam2-> %d",
				uDID, uMsg, (DWORD)dwUser, (DWORD)dwParam1, (DWORD)dwParam2);
		}
	
		fprintf(DebugLog, Msg);
		OutputDebugStringA(Msg);

		free(Msg);

		// Flush buffer
		fflush(DebugLog);
	}
}

void PrintLoadedDLLToDebugLog(LPCWSTR LibraryW, LPCSTR Status) {
	if (ManagedSettings.DebugMode) {
		wchar_t* Lib = (wchar_t*)malloc(sizeof(wchar_t) * NTFS_MAX_PATH);
		char* Msg = (char*)malloc(sizeof(char) * NTFS_MAX_PATH);

		// Debug log is busy now
		std::lock_guard<std::mutex> lock(DebugMutex);

		// Print to log
		PrintCurrentTime();
		swprintf(Lib, L"Library <<%s>> | ", LibraryW);
		fwprintf(DebugLog, Lib);
		OutputDebugStringW(Lib);

		sprintf(Msg, "%s\n", Status);
		fprintf(DebugLog, Msg);
		OutputDebugStringA(Msg);

		free(Lib);
		free(Msg);

		// Flush buffer
		fflush(DebugLog);
	}
}

void PrintSoundFontToDebugLog(LPCWSTR SoundFontW, LPCSTR Status) {
	if (ManagedSettings.DebugMode) {
		char* Msg = (char*)malloc(sizeof(char) * NTFS_MAX_PATH);
		char* SoundFontA = (char*)malloc(sizeof(char) * NTFS_MAX_PATH);

		wcstombs(SoundFontA, SoundFontW, wcslen(SoundFontW) + 1);
		LPSTR SoundFontNameA = PathFindFileNameA(SoundFontA);

		// Debug log is busy now
		std::lock_guard<std::mutex> lock(DebugMutex);

		// Print to log
		PrintCurrentTime();
		sprintf(Msg, "Stage <<NewSFLoader>> | SoundFont \"%s\" -> %s\n", SoundFontNameA, Status);
		fprintf(DebugLog, Msg);
		OutputDebugStringA(Msg);

		free(Msg);
		free(SoundFontA);

		// Flush buffer
		fflush(DebugLog);
	}
}

void PrintCallbackToDebugLog(LPCSTR Stage, HMIDI OMHM, DWORD_PTR OMCB, DWORD_PTR OMI, DWORD_PTR OMU, DWORD OMCM) {
	if (ManagedSettings.DebugMode) {
		char* Msg = (char*)malloc(sizeof(char) * NTFS_MAX_PATH);

		// Debug log is busy now
		std::lock_guard<std::mutex> lock(DebugMutex);

		// Print to log
		PrintCurrentTime();
		
		sprintf(
			Msg, 
			"Stage <<%s>> | OMHM: %08X - OMCB: %08X - OMI: %08X - OMU: %08X - OMCM: %08X\n", 
			Stage, (DWORD)OMHM, (DWORD)OMCB, (DWORD)OMI, (DWORD)OMU, (DWORD)OMCM);

		fprintf(DebugLog, Msg);
		OutputDebugStringA(Msg);

		free(Msg);

		// Flush buffer
		fflush(DebugLog);
	}
}

void PrintMessageToDebugLog(LPCSTR Stage, LPCSTR Status) {
	if (ManagedSettings.DebugMode) {
		char* Msg = (char*)malloc(sizeof(char) * NTFS_MAX_PATH);

		// Debug log is busy now
		std::lock_guard<std::mutex> lock(DebugMutex);

		// Print to log
		PrintCurrentTime();
		sprintf(Msg, "Stage <<%s>> | %s\n", Stage, Status);
		fprintf(DebugLog, Msg);
		OutputDebugStringA(Msg);

		free(Msg);

		// Flush buffer
		fflush(DebugLog);
	}
}

void PrintMessageWToDebugLog(LPCWSTR Stage, LPCWSTR Status) {
	if (ManagedSettings.DebugMode) {
		wchar_t* Msg = (wchar_t*)malloc(sizeof(wchar_t) * NTFS_MAX_PATH);

		// Debug log is busy now
		std::lock_guard<std::mutex> lock(DebugMutex);

		// Print to log
		PrintCurrentTimeW();
		swprintf(Msg, L"Stage <<%s>> | %s\n", Stage, Status);
		fwprintf(DebugLog, Msg);
		OutputDebugStringW(Msg);

		free(Msg);

		// Flush buffer
		fflush(DebugLog);
	}
}

void PrintBoolToDebugLog(LPCSTR Stage, BOOL Status) {
	if (ManagedSettings.DebugMode) {
		char* Msg = (char*)malloc(sizeof(char) * NTFS_MAX_PATH);

		// Debug log is busy now
		std::lock_guard<std::mutex> lock(DebugMutex);

		// Print to log
		PrintCurrentTime();
		sprintf(Msg, "Stage <<%s>> | %s\n", Stage, Status ? "TRUE" : "FALSE");
		fprintf(DebugLog, Msg);
		OutputDebugStringA(Msg);

		free(Msg);

		// Flush buffer
		fflush(DebugLog);
	}
}

void PrintStreamValueToDebugLog(LPCSTR Stage, LPCSTR ValueName, DWORD Value) {
	if (ManagedSettings.DebugMode) {
		char* Msg = (char*)malloc(sizeof(char) * NTFS_MAX_PATH);

		// Debug log is busy now
		std::lock_guard<std::mutex> lock(DebugMutex);

		// Print to log
		PrintCurrentTime();
		sprintf(Msg, "Stage <<%s>> | %s: %d (HEX: %08X)\n", Stage, ValueName, Value, Value);
		fprintf(DebugLog, Msg);
		OutputDebugStringA(Msg);

		free(Msg);

		// Flush buffer
		fflush(DebugLog);
	}
}

void PrintBASSErrorMessageToDebugLog(LPCWSTR BE, LPCWSTR BED) {
	if (ManagedSettings.DebugMode) {
		wchar_t* Msg = (wchar_t*)malloc(sizeof(wchar_t) * NTFS_MAX_PATH);

		// Debug log is busy now
		std::lock_guard<std::mutex> lock(DebugMutex);

		// Print to log
		PrintCurrentTime();
		swprintf(Msg, L"BASS error <<%s>> encountered | %s\n", BE, BED);
		fwprintf(DebugLog, Msg);
		OutputDebugStringW(Msg);

		free(Msg);

		// Flush buffer
		fflush(DebugLog);
	}
}

void PrintMemoryMessageToDebugLog(LPCSTR Stage, LPCSTR Status, BOOL IsRatio, ULONGLONG Memory) {
	if (ManagedSettings.DebugMode) {
		char* Msg = (char*)malloc(sizeof(char) * NTFS_MAX_PATH);

		// Debug log is busy now
		std::lock_guard<std::mutex> lock(DebugMutex);

		// Print to log
		PrintCurrentTime();
		sprintf(Msg, "Stage <<%s>> | %s: %u\n", Stage, Status, Memory);
		fprintf(DebugLog, Msg);
		OutputDebugStringA(Msg);

		free(Msg);

		// Flush buffer
		fflush(DebugLog);
	}
}

void PrintMIDIOPENDESCToDebugLog(LPCSTR Stage, MIDIOPENDESC* MIDIOD, DWORD_PTR dwUser, DWORD Flags) {
	if (ManagedSettings.DebugMode) {
		char* Msg = (char*)malloc(sizeof(char) * NTFS_MAX_PATH);

		// Debug log is busy now
		std::lock_guard<std::mutex> lock(DebugMutex);

		// Print to log
		PrintCurrentTime();
		sprintf(
			Msg, 
			"Stage <<%s>> | HMIDI: %08X - dwCallback: %08X - dwInstance: %08X - dwUser: %08X - OMFlags: %08X\n", 
			Stage, MIDIOD->hMidi, MIDIOD->dwCallback, MIDIOD->dwInstance, dwUser, Flags);

		fprintf(DebugLog, Msg);
		OutputDebugStringA(Msg);

		free(Msg);

		// Flush buffer
		fflush(DebugLog);
	}
}

void PrintMIDIHDRToDebugLog(LPCSTR Stage, MIDIHDR* IIMidiHdr) {
	if (ManagedSettings.DebugMode) {
		char* Msg = (char*)malloc(sizeof(char) * NTFS_MAX_PATH);

		// Debug log is busy now
		std::lock_guard<std::mutex> lock(DebugMutex);

		/*
		// Create dump of MIDIHDR data

		FILE* fo = fopen("mididata.bin", "ab");
		fwrite(IIMidiHdr->lpData, 1, IIMidiHdr->dwBytesRecorded, fo);
		fflush(fo);
		fclose(fo);
		*/

		// Print to log
		PrintCurrentTime();
		sprintf(Msg, "Stage <<%s>> | MIDIHDR data: ", Stage);

		for (int i = 0; i < IIMidiHdr->dwBytesRecorded; i++)
			sprintf(Msg + strlen(Msg), "%02X", (BYTE)(IIMidiHdr->lpData[i]));

		sprintf(Msg + strlen(Msg), " (Recorded bytes: %u)\n", IIMidiHdr->dwBytesRecorded);

		fprintf(DebugLog, Msg);
		OutputDebugStringA(Msg);

		free(Msg);

		// Flush buffer
		fflush(DebugLog);
	}
}

void PrintEventToDebugLog(DWORD dwParam) {
	if (ManagedSettings.DebugMode) {
		char* Msg = (char*)malloc(sizeof(char) * NTFS_MAX_PATH);

		// Debug log is busy now
		std::lock_guard<std::mutex> lock(DebugMutex);

		// Print to log
		PrintCurrentTime();

		sprintf(Msg, "Stage <<MIDIEvent | %06X>> | Event ID: ", dwParam);
		switch (dwParam & 0xF0) {
		case 0x80:
			sprintf(Msg + strlen(Msg), "Note off, Channel: %u, Key: %u\n", GETCHANNEL(dwParam), GETFP(dwParam));
			break;
		case 0x90:
			if (GETSP(dwParam)) sprintf(Msg + strlen(Msg), "Note on, Channel: %u, Key: %u, Velocity: %u\n", GETCHANNEL(dwParam), GETFP(dwParam), GETSP(dwParam));
			else sprintf(Msg + strlen(Msg), "Note off, Channel: %u, Key: %u\n", GETCHANNEL(dwParam), GETFP(dwParam));
			break;
		case 0xA0:
			sprintf(Msg + strlen(Msg), "Polyphonic aftertouch, Channel: %u, Key: %u, Touch: %u\n", GETCHANNEL(dwParam), GETFP(dwParam), GETSP(dwParam));
			break;
		case 0xB0:
			sprintf(Msg + strlen(Msg), "Control mode change, Channel: %u, Controller #: %u, Value: %u\n", GETCHANNEL(dwParam), GETFP(dwParam), GETSP(dwParam));
			break;
		case 0xC0:
			sprintf(Msg + strlen(Msg), "Patch change, Channel: %u, Instrument #: %u\n", GETCHANNEL(dwParam), GETFP(dwParam));
			break;
		case 0xD0:
			sprintf(Msg + strlen(Msg), "Channel aftertouch, Channel: %u, Pressure: %u\n", GETCHANNEL(dwParam), GETFP(dwParam));
			break;
		case 0xE0:
			sprintf(Msg + strlen(Msg), "Pitch wheel, Channel: %u, LSB: %u, MSB: %u\n", GETCHANNEL(dwParam), GETFP(dwParam), GETSP(dwParam));
			break;
		case 0xF0:{
			switch (dwParam & 0xFF) {
			case 0xF0:
				strcat(Msg, "Start of SysEx Msg\n");
				break;
			case 0xF1:
				strcat(Msg, "MIDI Time Code Quarter Frame (SysCm)\n");
				break;
			case 0xF2:
				strcat(Msg, "Song Position Pointer (SysCm)\n");
				break;
			case 0xF3:
				strcat(Msg, "Song Select (SysCm)\n");
				break;
			case 0xF6:
				strcat(Msg, "Tune Request (SysCm)\n");
				break;
			case 0xF7:
				strcat(Msg, "End of SysEx Msg\n");
				break;
			case 0xF8:
				strcat(Msg, "Timing Clock (SysCm)\n");
				break;
			case 0xFA:
				strcat(Msg, "Start (SysRt)\n");
				break;
			case 0xFB:
				strcat(Msg, "Continue (SysRt)\n");
				break;
			case 0xFC:
				strcat(Msg, "Stop (SysRt)\n");
				break;
			case 0xFE:
				strcat(Msg, "Active Sensing (SysRt)\n");
				break;
			case 0xFF:
				strcat(Msg, "System Reset (SysRt)\n");
				break;
			default:
				sprintf(Msg + strlen(Msg), "UNKNOWN (%02X)\n", dwParam & 0xFF);
				break;
			}
			break;
		}
		}

		fprintf(DebugLog, Msg);
		OutputDebugStringA(Msg);

		free(Msg);

		// Flush buffer
		fflush(DebugLog);
	}
}

void PrintLongMessageToDebugLog(BOOL IsRecognized, MIDIHDR* IIMidiHdr) {
	if (ManagedSettings.DebugMode) {
		char* Msg = (char*)malloc(sizeof(char) * NTFS_MAX_PATH);

		// Debug log is busy now
		std::lock_guard<std::mutex> lock(DebugMutex);

		// Print to log
		PrintCurrentTime();
		sprintf(Msg, "Stage <<%s>> | %s long message: ", (IsRecognized ? "ParsedLongMsg" : "UnknownLongMsg"), (IsRecognized ? "Parsed" : "Unknown"));

		for (int i = 0; i < IIMidiHdr->dwBytesRecorded; i++)
			sprintf(Msg + strlen(Msg), "%02X", (BYTE)(IIMidiHdr->lpData[i]));

		sprintf(Msg + strlen(Msg), " (Recorded bytes: %u)\n", IIMidiHdr->dwBytesRecorded);

		fprintf(DebugLog, Msg);
		OutputDebugStringA(Msg);

		free(Msg);

		// Flush buffer
		fflush(DebugLog);
	}
}


static __declspec(noinline) void ToHex32(char* Target, DWORD val)
{
	sprintf(Target + strlen(Target), "%08X", val);
}

static __declspec(noinline) void ToHex64(char* Target, QWORD valin)
{
	DWORD* valp = (DWORD*)& valin;
	ToHex32(Target, valp[1]);
	ToHex32(Target, valp[0]);
}

static __declspec(noinline) void WritePointer(char* Target, size_t valin)
{
	if (sizeof(valin) > 4)
		ToHex64(Target, valin);
	else
		ToHex32(Target, valin);
}

static BOOL IsExceptionValid(DWORD ex) {
	switch (ex)
	{
	case EXCEPTION_ACCESS_VIOLATION:
	case EXCEPTION_ARRAY_BOUNDS_EXCEEDED:
	case EXCEPTION_BREAKPOINT:
	case EXCEPTION_DATATYPE_MISALIGNMENT:
	case EXCEPTION_FLT_DENORMAL_OPERAND:
	case EXCEPTION_FLT_DIVIDE_BY_ZERO:
	case EXCEPTION_FLT_INEXACT_RESULT:
	case EXCEPTION_FLT_INVALID_OPERATION:
	case EXCEPTION_FLT_OVERFLOW:
	case EXCEPTION_FLT_STACK_CHECK:
	case EXCEPTION_FLT_UNDERFLOW:
	case EXCEPTION_ILLEGAL_INSTRUCTION:
	case EXCEPTION_IN_PAGE_ERROR:
	case EXCEPTION_INT_DIVIDE_BY_ZERO:
	case EXCEPTION_INT_OVERFLOW:
	case EXCEPTION_INVALID_DISPOSITION:
	case EXCEPTION_NONCONTINUABLE_EXCEPTION:
	case EXCEPTION_PRIV_INSTRUCTION:
	case EXCEPTION_SINGLE_STEP:
	case EXCEPTION_STACK_OVERFLOW:
		return TRUE;
	default:
		return FALSE;
	}
}

static LONG WINAPI OmniMIDICrashHandler(LPEXCEPTION_POINTERS exc) {
	if (!IsExceptionValid(exc->ExceptionRecord->ExceptionCode)) return 0;

	HANDLE CurrentProcess = GetCurrentProcess();
	MEMORY_BASIC_INFORMATION MBI;
	BYTE* StackTrace[USHRT_MAX];
	DWORD ret = CaptureStackBackTrace(0, USHRT_MAX, (PVOID*)StackTrace, 0);

	char MessageBuf[NTFS_MAX_PATH];
	char NameBuf[NTFS_MAX_PATH];

	sprintf(MessageBuf, "OmniMIDI or the host app has encountered a problem, and needs to be closed.\nHere are some information about the crash.\n\n");
	if (ret) {
		sprintf(MessageBuf + strlen(MessageBuf), "Stacktrace:");
		while (ret--) {
			sprintf(MessageBuf + strlen(MessageBuf), "\n");
			WritePointer(MessageBuf, (size_t)StackTrace[ret]);

			if (!VirtualQuery(StackTrace[ret], &MBI, sizeof(MBI)))
				continue;

			HMODULE mod = (HMODULE)MBI.AllocationBase;

			if (!GetModuleBaseNameA(CurrentProcess, mod, NameBuf, sizeof(NameBuf)))
				continue;

			NameBuf[31] = 0;
			sprintf(MessageBuf + strlen(MessageBuf), " (");
			sprintf(MessageBuf + strlen(MessageBuf), NameBuf);
			sprintf(MessageBuf + strlen(MessageBuf), "+");
			ToHex32(MessageBuf, (DWORD)(StackTrace[ret] - (BYTE*)mod));
			sprintf(MessageBuf + strlen(MessageBuf), ")");
		}
		sprintf(MessageBuf + strlen(MessageBuf), "\n ");
	}
	else sprintf(MessageBuf + strlen(MessageBuf), " * The stacktrace is either empty or contains garbage data.\n\n");

	sprintf(MessageBuf + strlen(MessageBuf), "\nException:");

	switch (exc->ExceptionRecord->ExceptionCode)
	{
		arre(EXCEPTION_ACCESS_VIOLATION);
		arre(EXCEPTION_ARRAY_BOUNDS_EXCEEDED);
		arre(EXCEPTION_BREAKPOINT);
		arre(EXCEPTION_DATATYPE_MISALIGNMENT);
		arre(EXCEPTION_FLT_DENORMAL_OPERAND);
		arre(EXCEPTION_FLT_DIVIDE_BY_ZERO);
		arre(EXCEPTION_FLT_INEXACT_RESULT);
		arre(EXCEPTION_FLT_INVALID_OPERATION);
		arre(EXCEPTION_FLT_OVERFLOW);
		arre(EXCEPTION_FLT_STACK_CHECK);
		arre(EXCEPTION_FLT_UNDERFLOW);
		arre(EXCEPTION_ILLEGAL_INSTRUCTION);
		arre(EXCEPTION_IN_PAGE_ERROR);
		arre(EXCEPTION_INT_DIVIDE_BY_ZERO);
		arre(EXCEPTION_INT_OVERFLOW);
		arre(EXCEPTION_INVALID_DISPOSITION);
		arre(EXCEPTION_NONCONTINUABLE_EXCEPTION);
		arre(EXCEPTION_PRIV_INSTRUCTION);
		arre(EXCEPTION_SINGLE_STEP);
		arre(EXCEPTION_STACK_OVERFLOW);
	default:
		sprintf(MessageBuf + strlen(MessageBuf), "\nUnknown exception code");
		ToHex32(MessageBuf, exc->ExceptionRecord->ExceptionCode);
		break;
	}

	sprintf(MessageBuf + strlen(MessageBuf), "\nAddress: ");
	WritePointer(MessageBuf, (size_t)exc->ExceptionRecord->ExceptionAddress);
	if (exc->ExceptionRecord->NumberParameters)
	{
		sprintf(MessageBuf + strlen(MessageBuf), "\nParam[0]: "); WritePointer(MessageBuf, exc->ExceptionRecord->ExceptionInformation[0]);
		sprintf(MessageBuf + strlen(MessageBuf), "\nParam[1]: "); WritePointer(MessageBuf, exc->ExceptionRecord->ExceptionInformation[1]);
		sprintf(MessageBuf + strlen(MessageBuf), "\nParam[2]: "); WritePointer(MessageBuf, exc->ExceptionRecord->ExceptionInformation[2]);
	}
	sprintf(MessageBuf + strlen(MessageBuf), "\n\n");

	sprintf(MessageBuf + strlen(MessageBuf), "Registers dump:");
#ifdef _M_AMD64
	sprintf(MessageBuf + strlen(MessageBuf), "\nPC : "); WritePointer(MessageBuf, exc->ContextRecord->Rip);
	do
	{
		if (!VirtualQuery((BYTE*)exc->ContextRecord->Rip, &MBI, sizeof(MBI)))
			continue;

		HMODULE mod = (HMODULE)MBI.AllocationBase;

		if (!GetModuleBaseNameA(CurrentProcess, mod, NameBuf, sizeof(NameBuf)))
			continue;

		NameBuf[31] = 0;
		sprintf(MessageBuf + strlen(MessageBuf), " (On address ");
		sprintf(MessageBuf + strlen(MessageBuf), NameBuf);
		sprintf(MessageBuf + strlen(MessageBuf), "+");
		WritePointer(MessageBuf, (DWORD)((BYTE*)exc->ContextRecord->Rip - (BYTE*)mod));
		sprintf(MessageBuf + strlen(MessageBuf), ")");
	} while (0);

	sprintf(MessageBuf + strlen(MessageBuf), "\nRAX: "); WritePointer(MessageBuf, exc->ContextRecord->Rax);
	sprintf(MessageBuf + strlen(MessageBuf), " RCX: "); WritePointer(MessageBuf, exc->ContextRecord->Rcx);
	sprintf(MessageBuf + strlen(MessageBuf), "\nRDX: "); WritePointer(MessageBuf, exc->ContextRecord->Rdx);
	sprintf(MessageBuf + strlen(MessageBuf), " RBX: "); WritePointer(MessageBuf, exc->ContextRecord->Rbx);
	sprintf(MessageBuf + strlen(MessageBuf), "\nRSP: "); WritePointer(MessageBuf, exc->ContextRecord->Rsp);
	sprintf(MessageBuf + strlen(MessageBuf), " RBP: "); WritePointer(MessageBuf, exc->ContextRecord->Rbp);
	sprintf(MessageBuf + strlen(MessageBuf), "\nRSI: "); WritePointer(MessageBuf, exc->ContextRecord->Rsi);
	sprintf(MessageBuf + strlen(MessageBuf), " RDI: "); WritePointer(MessageBuf, exc->ContextRecord->Rdi);
	sprintf(MessageBuf + strlen(MessageBuf), "\nDR0: "); WritePointer(MessageBuf, exc->ContextRecord->Dr0);
	sprintf(MessageBuf + strlen(MessageBuf), " DR1: "); WritePointer(MessageBuf, exc->ContextRecord->Dr1);
	sprintf(MessageBuf + strlen(MessageBuf), "\nDR2: "); WritePointer(MessageBuf, exc->ContextRecord->Dr2);
	sprintf(MessageBuf + strlen(MessageBuf), " DR3: "); WritePointer(MessageBuf, exc->ContextRecord->Dr3);
	sprintf(MessageBuf + strlen(MessageBuf), "\nDR6: "); WritePointer(MessageBuf, exc->ContextRecord->Dr6);
	sprintf(MessageBuf + strlen(MessageBuf), " DR7: "); WritePointer(MessageBuf, exc->ContextRecord->Dr7);
	sprintf(MessageBuf + strlen(MessageBuf), "\nR8 : "); WritePointer(MessageBuf, exc->ContextRecord->R8);
	sprintf(MessageBuf + strlen(MessageBuf), " R9 : "); WritePointer(MessageBuf, exc->ContextRecord->R9);
	sprintf(MessageBuf + strlen(MessageBuf), "\nR10: "); WritePointer(MessageBuf, exc->ContextRecord->R10);
	sprintf(MessageBuf + strlen(MessageBuf), " R11: "); WritePointer(MessageBuf, exc->ContextRecord->R11);
	sprintf(MessageBuf + strlen(MessageBuf), "\nR12: "); WritePointer(MessageBuf, exc->ContextRecord->R12);
	sprintf(MessageBuf + strlen(MessageBuf), " R13: "); WritePointer(MessageBuf, exc->ContextRecord->R13);
	sprintf(MessageBuf + strlen(MessageBuf), "\nR14: "); WritePointer(MessageBuf, exc->ContextRecord->R14);
	sprintf(MessageBuf + strlen(MessageBuf), " R15: "); WritePointer(MessageBuf, exc->ContextRecord->R15);
	sprintf(MessageBuf + strlen(MessageBuf), "\nLBT: "); WritePointer(MessageBuf, exc->ContextRecord->LastBranchToRip);
	sprintf(MessageBuf + strlen(MessageBuf), " LBF: "); WritePointer(MessageBuf, exc->ContextRecord->LastBranchFromRip);
	sprintf(MessageBuf + strlen(MessageBuf), "\nLET: "); WritePointer(MessageBuf, exc->ContextRecord->LastExceptionToRip);
	sprintf(MessageBuf + strlen(MessageBuf), " LEF: "); WritePointer(MessageBuf, exc->ContextRecord->LastExceptionFromRip);
#else
#ifdef _M_ARM64
	sprintf(MessageBuf + strlen(MessageBuf), "\nPC : "); WritePointer(MessageBuf, exc->ContextRecord->Pc);
	sprintf(MessageBuf + strlen(MessageBuf), "\nLR : "); WritePointer(MessageBuf, exc->ContextRecord->Lr);
	sprintf(MessageBuf + strlen(MessageBuf), " SP : "); WritePointer(MessageBuf, exc->ContextRecord->Sp);
	sprintf(MessageBuf + strlen(MessageBuf), "\nFP : "); WritePointer(MessageBuf, exc->ContextRecord->Fp);
	sprintf(MessageBuf + strlen(MessageBuf), " CPS: "); WritePointer(MessageBuf, exc->ContextRecord->Cpsr);
	sprintf(MessageBuf + strlen(MessageBuf), "\nFPS: "); WritePointer(MessageBuf, exc->ContextRecord->Fpsr);
	sprintf(MessageBuf + strlen(MessageBuf), " FPC: "); WritePointer(MessageBuf, exc->ContextRecord->Fpcr);
	sprintf(MessageBuf + strlen(MessageBuf), "\nX0 : "); WritePointer(MessageBuf, exc->ContextRecord->X[0]);
	sprintf(MessageBuf + strlen(MessageBuf), " X1 : "); WritePointer(MessageBuf, exc->ContextRecord->X[1]);
	sprintf(MessageBuf + strlen(MessageBuf), "\nX2 : "); WritePointer(MessageBuf, exc->ContextRecord->X[2]);
	sprintf(MessageBuf + strlen(MessageBuf), " X3 : "); WritePointer(MessageBuf, exc->ContextRecord->X[3]);
	sprintf(MessageBuf + strlen(MessageBuf), "\nX4 : "); WritePointer(MessageBuf, exc->ContextRecord->X[4]);
	sprintf(MessageBuf + strlen(MessageBuf), " X5 : "); WritePointer(MessageBuf, exc->ContextRecord->X[5]);
	sprintf(MessageBuf + strlen(MessageBuf), "\nX6 : "); WritePointer(MessageBuf, exc->ContextRecord->X[6]);
	sprintf(MessageBuf + strlen(MessageBuf), " X7 : "); WritePointer(MessageBuf, exc->ContextRecord->X[7]);
	sprintf(MessageBuf + strlen(MessageBuf), "\nX8 : "); WritePointer(MessageBuf, exc->ContextRecord->X[8]);
	sprintf(MessageBuf + strlen(MessageBuf), " X9 : "); WritePointer(MessageBuf, exc->ContextRecord->X[9]);
	sprintf(MessageBuf + strlen(MessageBuf), "\nX10: "); WritePointer(MessageBuf, exc->ContextRecord->X[10]);
	sprintf(MessageBuf + strlen(MessageBuf), " X11: "); WritePointer(MessageBuf, exc->ContextRecord->X[11]);
	sprintf(MessageBuf + strlen(MessageBuf), "\nX12: "); WritePointer(MessageBuf, exc->ContextRecord->X[12]);
	sprintf(MessageBuf + strlen(MessageBuf), " X13: "); WritePointer(MessageBuf, exc->ContextRecord->X[13]);
	sprintf(MessageBuf + strlen(MessageBuf), "\nX14: "); WritePointer(MessageBuf, exc->ContextRecord->X[14]);
	sprintf(MessageBuf + strlen(MessageBuf), " X15: "); WritePointer(MessageBuf, exc->ContextRecord->X[15]);
	sprintf(MessageBuf + strlen(MessageBuf), "\nX16: "); WritePointer(MessageBuf, exc->ContextRecord->X[16]);
	sprintf(MessageBuf + strlen(MessageBuf), " X17: "); WritePointer(MessageBuf, exc->ContextRecord->X[17]);
	sprintf(MessageBuf + strlen(MessageBuf), "\nX18: "); WritePointer(MessageBuf, exc->ContextRecord->X[18]);
	sprintf(MessageBuf + strlen(MessageBuf), " X19: "); WritePointer(MessageBuf, exc->ContextRecord->X[19]);
	sprintf(MessageBuf + strlen(MessageBuf), "\nX20: "); WritePointer(MessageBuf, exc->ContextRecord->X[20]);
	sprintf(MessageBuf + strlen(MessageBuf), " X21: "); WritePointer(MessageBuf, exc->ContextRecord->X[21]);
	sprintf(MessageBuf + strlen(MessageBuf), "\nX22: "); WritePointer(MessageBuf, exc->ContextRecord->X[22]);
	sprintf(MessageBuf + strlen(MessageBuf), " X23: "); WritePointer(MessageBuf, exc->ContextRecord->X[23]);
	sprintf(MessageBuf + strlen(MessageBuf), "\nX24: "); WritePointer(MessageBuf, exc->ContextRecord->X[24]);
	sprintf(MessageBuf + strlen(MessageBuf), " X25: "); WritePointer(MessageBuf, exc->ContextRecord->X[25]);
	sprintf(MessageBuf + strlen(MessageBuf), "\nX26: "); WritePointer(MessageBuf, exc->ContextRecord->X[26]);
	sprintf(MessageBuf + strlen(MessageBuf), " X27: "); WritePointer(MessageBuf, exc->ContextRecord->X[27]);
	sprintf(MessageBuf + strlen(MessageBuf), "\nX28: "); WritePointer(MessageBuf, exc->ContextRecord->X[28]);
#else
#ifdef _M_IX86
	sprintf(MessageBuf + strlen(MessageBuf), "\nPC : "); WritePointer(MessageBuf, exc->ContextRecord->Eip);
	if (exc->ContextRecord->Eip) {
		do
		{
			if (!VirtualQuery((BYTE*)exc->ContextRecord->Eip, &MBI, sizeof(MBI)))
				continue;

			HMODULE mod = (HMODULE)MBI.AllocationBase;

			if (!GetModuleBaseNameA(CurrentProcess, mod, NameBuf, sizeof(NameBuf)))
				continue;

			NameBuf[31] = 0;
			sprintf(MessageBuf + strlen(MessageBuf), " (On address ");
			sprintf(MessageBuf + strlen(MessageBuf), NameBuf);
			sprintf(MessageBuf + strlen(MessageBuf), "+");
			WritePointer(MessageBuf, (DWORD)((BYTE*)exc->ContextRecord->Eip - (BYTE*)mod));
			sprintf(MessageBuf + strlen(MessageBuf), ")");
		} while (0);
	}

	sprintf(MessageBuf + strlen(MessageBuf), "\nEDI: "); WritePointer(MessageBuf, exc->ContextRecord->Edi);
	sprintf(MessageBuf + strlen(MessageBuf), " ESI: "); WritePointer(MessageBuf, exc->ContextRecord->Esi);
	sprintf(MessageBuf + strlen(MessageBuf), "\nEBX: "); WritePointer(MessageBuf, exc->ContextRecord->Ebx);
	sprintf(MessageBuf + strlen(MessageBuf), " EDX: "); WritePointer(MessageBuf, exc->ContextRecord->Edx);
	sprintf(MessageBuf + strlen(MessageBuf), "\nEXC: "); WritePointer(MessageBuf, exc->ContextRecord->Ecx);
	sprintf(MessageBuf + strlen(MessageBuf), " EAX: "); WritePointer(MessageBuf, exc->ContextRecord->Eax);
	sprintf(MessageBuf + strlen(MessageBuf), "\nEBP: "); WritePointer(MessageBuf, exc->ContextRecord->Ebp);
#else
	sprintf(MessageBuf + strlen(MessageBuf), " * Registry dumps are not supported on this platform");
#endif
#endif
#endif

	PrintMessageToDebugLog("OmniMIDICrashHandler", MessageBuf);
	MessageBoxA(NULL, MessageBuf, "OmniMIDI - An error has occurred", MB_ICONERROR | MB_OK | MB_SYSTEMMODAL);
	MakeMiniDump(exc);

	return EXCEPTION_EXECUTE_HANDLER;
}

BOOL EnableBuiltInHandler(LPCSTR Stage) {
#ifndef _DEBUG
	if (ManagedSettings.DebugMode) {
		PrintMessageToDebugLog(Stage, "Initializing OmniMIDICrashHandler...");
		if (NULL == (ExceptionHandler = AddVectoredExceptionHandler(1, OmniMIDICrashHandler))) {
			MessageBoxA(NULL, "An error has occured while initializing the built-in crash handler.", "OmniMIDI - ERROR", MB_ICONERROR | MB_OK | MB_SYSTEMMODAL);
			return FALSE;
		}
	}
#endif

	return TRUE;
}

BOOL DisableBuiltInHandler(LPCSTR Stage) {
#ifndef _DEBUG
	if (ExceptionHandler != nullptr) {
		PrintMessageToDebugLog(Stage, "Removing OmniMIDICrashHandler...");
		if (!RemoveVectoredExceptionHandler(ExceptionHandler)) {
			CrashMessage(L"DIsableBuiltInHandlerFail");
			return FALSE;
		}
	}
#endif

	return TRUE;
}

void StartDebugPipe(BOOL RestartingPipe) {
	// Initialize the current pipe count and template
	unsigned int PipeVal = 1;
	wchar_t PipeDes[MAX_PATH];

	if (RestartingPipe) {
		FlushFileBuffers(hPipe);
		CloseHandle(hPipe);
		hPipe = NULL;
	}

Retry:
	while (!hPipe || hPipe == INVALID_HANDLE_VALUE) {
		// Clear the WCHAR, since it might contain garbage, 
		// and print the template with PipeVal in it
		// (Ex. "\\\\.\\pipe\\OmniMIDIDbg1")
		ZeroMemory(PipeDes, MAX_PATH);
		swprintf_s(PipeDes, MAX_PATH, OMPipeTemplate, PipeVal);

		// Now create the pipe
		hPipe = CreateNamedPipeW(PipeDes,
			PIPE_ACCESS_DUPLEX | FILE_FLAG_FIRST_PIPE_INSTANCE,
			PIPE_TYPE_BYTE | PIPE_READMODE_BYTE | PIPE_WAIT,
			PIPE_UNLIMITED_INSTANCES,
			NTFS_MAX_PATH,
			NTFS_MAX_PATH,
			NMPWAIT_USE_DEFAULT_WAIT,
			NULL);

		// Check if the pipe failed to be initialized
		if (!hPipe || hPipe == INVALID_HANDLE_VALUE)
		{
			// It did. If the pipe value isn above the maximum instances, throw a crash
			if (PipeVal > PIPE_UNLIMITED_INSTANCES)
				CrashMessage(L"TooManyPipes");
			else
				PipeVal++;
		}
	}
}

MMRESULT DebugResult(LPCSTR Stage, MMRESULT ErrorToDisplay, LPCSTR ExactError) {
	if (!ErrorToDisplay) return MMSYSERR_NOERROR;

	CHAR ErrorString[NTFS_MAX_PATH] = { 0 };
	
	switch (ErrorToDisplay) {
		CurrentError(ErrorString, MMSYSERR_NOMEM, "The system is unable to allocate or lock memory.");
		CurrentError(ErrorString, MMSYSERR_ALLOCATED, "The driver has been already allocated in a previous InitializeKDMAPIStream/midiStreamOpen/midiOutOpen call.");
		CurrentError(ErrorString, MMSYSERR_MOREDATA, "The driver has more data to return, but the MIDI application won't let it return data quickly enough.");
		CurrentError(ErrorString, MMSYSERR_NODRIVERCB, "The driver does not call DriverCallback.");
		CurrentError(ErrorString, MMSYSERR_INVALHANDLE, "The handle of the specified device is invalid.");
		CurrentError(ErrorString, MMSYSERR_INVALFLAG, "An invalid flag was passed to modMessage.");
		CurrentError(ErrorString, MMSYSERR_INVALPARAM, "An invalid parameter was passed to modMessage.");
		CurrentError(ErrorString, MMSYSERR_NOTENABLED, "The driver failed to load or initialize.");
		CurrentError(ErrorString, MMSYSERR_NOTSUPPORTED, "The function requested by the message is not supported.");
		CurrentError(ErrorString, MIDIERR_NOTREADY, "The driver is busy processing other data.");
		CurrentError(ErrorString, MIDIERR_UNPREPARED, "The specified data block has not been prepared.");
		CurrentError(ErrorString, MIDIERR_STILLPLAYING, "There are data blocks still in queue.");
		CurrentError(ErrorString, MMSYSERR_BADDEVICEID, "The specified device ID is out of range.");
	default:
		DefError(ErrorString, MMSYSERR_ERROR, "Unspecified error.");
	}

	if (ExactError) {
		strcat(ErrorString, "\n\nCause: ");
		strcat(ErrorString, ExactError);
	}

	if (ManagedSettings.DebugMode || ErrorToDisplay == MMSYSERR_ALLOCATED) {
		if (ManagedSettings.DebugMode) strcat(ErrorString, "\n\nIf you're the developer of this app, please check if all the MIDI calls have been done correctly.");
		PrintMessageToDebugLog(Stage, ErrorString);
		MessageBoxA(NULL, ErrorString, "OmniMIDI - WinMM API/KDMAPI ERROR", MB_OK | MB_ICONHAND | MB_SYSTEMMODAL);
	}

	return ErrorToDisplay;
}