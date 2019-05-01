/*
OmniMIDI debug functions
*/

#define CurrentError(Title, Message, Error, Text) case Error: sprintf_s(Title, 512, "%s", #Error); sprintf_s(Message, 512, "%s", #Text); break
#define arre(wat) case wat: sprintf(MessageBuf + strlen(MessageBuf), "\nCode: %s", #wat); break

static HANDLE ExceptionHandler = nullptr;
static const char hex[] = "0123456789ABCDEF";
static std::mutex DebugMutex;
static BOOL IntroAlreadyShown = FALSE;
static BOOL InfoAlreadyGot = FALSE;

double GetThreadUsage(Thread* Thread) {
	if (!(WaitForSingleObject(Thread->ThreadHandle, 0) != WAIT_OBJECT_0)) return 0.0;

	FILETIME ftime, fsys, fuser;
	ULARGE_INTEGER now, sys, user;
	double percent;

	GetSystemTimeAsFileTime(&ftime);
	memcpy(&now, &ftime, sizeof(FILETIME));

	GetThreadTimes(Thread->ThreadHandle, &ftime, &ftime, &fsys, &fuser);
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

std::wstring GetErrorAsString(DWORD ErrorID)
{
	//Get the error message, if any.
	if (ErrorID == 0)
		return std::wstring(L"No error detected."); //No error message has been recorded

	LPWSTR messageBuffer = nullptr;
	DWORD size = FormatMessageW(FORMAT_MESSAGE_ALLOCATE_BUFFER | FORMAT_MESSAGE_FROM_SYSTEM | FORMAT_MESSAGE_IGNORE_INSERTS,
		NULL, ErrorID, MAKELANGID(LANG_NEUTRAL, SUBLANG_DEFAULT), (LPWSTR)&messageBuffer, 0, NULL);

	std::wstring message(messageBuffer, size);

	//Free the buffer.
	LocalFree(messageBuffer);

	return message;
}

void GetAppName() {
	if (!InfoAlreadyGot)
	{
		try {
			ZeroMemory(AppPath, sizeof(AppPath));
			ZeroMemory(AppPathW, sizeof(AppPathW));
			ZeroMemory(AppName, sizeof(AppName));
			ZeroMemory(AppNameW, sizeof(AppNameW));

			GetModuleFileNameW(NULL, AppPathW, NTFS_MAX_PATH);
			wcstombs(AppPath, AppPathW, wcslen(AppPathW) + 1);

			TCHAR * TempPoint = PathFindFileName(AppPathW);
			wcsncpy(AppNameW, TempPoint, MAX_PATH);
			wcstombs(AppName, AppNameW, wcslen(AppNameW) + 1);

			InfoAlreadyGot = TRUE;
		}
		catch (...) { }
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
		MINIDUMP_TYPE(MiniDumpWithIndirectlyReferencedMemory | MiniDumpScanMemory),
		exc ? &exceptionInfo : nullptr,
		NULL,
		NULL);

	CloseHandle(hFile);
	return;
}

void CrashMessage(LPCSTR part) {
	std::wstringstream ErrorMessage;
	DWORD ErrorID = GetLastError();

	fprintf(stdout, "(Error at \"%s\", Code 0x%08x) - Fatal error during the execution of the driver.", part, ErrorID);

	ErrorMessage << L"An error has been detected while executing the following function: " << part << "\n";
	if (ErrorID != 0) {
		ErrorMessage << L"\nError code: 0x" << std::uppercase << std::hex << ErrorID << L" - " << GetErrorAsString(ErrorID);
		ErrorMessage << L"\nPlease take a screenshot of this messagebox (ALT+PRINT), and create a GitHub issue.\n";
	}
	ErrorMessage << L"\nClick OK to close the program.";

	MessageBox(NULL, ErrorMessage.str().c_str(), L"OmniMIDI - Fatal execution error", MB_ICONERROR | MB_SYSTEMMODAL);

	block_bassinit = TRUE;
	stop_thread = TRUE;

	MakeMiniDump(nullptr);

	exit(ErrorID);
}

static __declspec(noinline) void ToHex32(char* Target, DWORD val)
{
	sprintf(Target + strlen(Target), "%08X", val);
}

static __declspec(noinline) void ToHex64(char* Target, QWORD valin)
{
	DWORD* valp = (DWORD*)&valin;
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
	BYTE* StackTrace[1024];
	DWORD ret = CaptureStackBackTrace(0, 1024, (PVOID*)StackTrace, 0);

	char MessageBuf[NTFS_MAX_PATH];
	char NameBuf[NTFS_MAX_PATH];

	sprintf(MessageBuf, "The program performed an illegal operation!\n\nIf you're the developer, check the stacktrace to see where the crash occured.\nIf you're a normal user, report this issue to either KaleidonKep99 on GitHub or the app developer.\n\n");
	if (ret) {
		sprintf(MessageBuf + strlen(MessageBuf), "== Stacktrace ==");
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
	else sprintf(MessageBuf + strlen(MessageBuf), " * No stack trace available\n\n");

	sprintf(MessageBuf + strlen(MessageBuf), "\n== Exception ==");

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
		sprintf(MessageBuf + strlen(MessageBuf), "\nCode: unk"); 
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

	sprintf(MessageBuf + strlen(MessageBuf), "== RegDump ==");
#ifdef _M_AMD64
	sprintf(MessageBuf + strlen(MessageBuf), "\nPC : "); WritePointer(MessageBuf, exc->ContextRecord->Rip);
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
	sprintf(MessageBuf + strlen(MessageBuf), "\nEDI: "); WritePointer(MessageBuf, exc->ContextRecord->Edi);
	sprintf(MessageBuf + strlen(MessageBuf), " ESI: "); WritePointer(MessageBuf, exc->ContextRecord->Esi);
	sprintf(MessageBuf + strlen(MessageBuf), "\nEBX: "); WritePointer(MessageBuf, exc->ContextRecord->Ebx);
	sprintf(MessageBuf + strlen(MessageBuf), " EDX: "); WritePointer(MessageBuf, exc->ContextRecord->Edx);
	sprintf(MessageBuf + strlen(MessageBuf), "\nEXC: "); WritePointer(MessageBuf, exc->ContextRecord->Ecx);
	sprintf(MessageBuf + strlen(MessageBuf), " EAX: "); WritePointer(MessageBuf, exc->ContextRecord->Eax);
	sprintf(MessageBuf + strlen(MessageBuf), "\nEBP: "); WritePointer(MessageBuf, exc->ContextRecord->Ebp);
#else
	sprintf(MessageBuf + strlen(MessageBuf), " * Regdumps are not supported on this platform");
#endif
#endif
#endif

	MakeMiniDump(exc);
	MessageBoxA(NULL, MessageBuf, "OmniMIDI - Unhandled Exception", MB_ICONERROR | MB_OK | MB_SYSTEMMODAL);

	return EXCEPTION_EXECUTE_HANDLER;
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
		TCHAR MainLibrary[MAX_PATH];
		TCHAR DebugDir[MAX_PATH];
		TCHAR CurrentTime[MAX_PATH];

		if (!DisableChime)
		{
			Beep(440, 100);
			Beep(440, 100);
		}

		// Get the debug info first
		GetAppName();

		// Get user profile's path
		SHGetFolderPath(NULL, CSIDL_PROFILE, NULL, 0, DebugDir);

		// Append "\OmniMIDI\debug\" to "%userprofile%"
		wcscat_s(DebugDir, MAX_PATH, L"\\OmniMIDI\\debug\\");

		// Create "%userprofile%\OmniMIDI\debug\", in case it doesn't exist
		CreateDirectory(DebugDir, NULL);

		// Append the app's filename to the output file's path
		wcscat_s(DebugDir, MAX_PATH, AppNameW);

		// Parse current time, and append it
		struct tm *sTm;
		time_t now = time(0);
		sTm = gmtime(&now);
		wcsftime(CurrentTime, sizeof(CurrentTime), L" - %d-%m-%Y %H.%M.%S", sTm);
		wcscat_s(DebugDir, MAX_PATH, CurrentTime);

		// Append file extension, and that's it
		wcscat_s(DebugDir, MAX_PATH, _T(" (Debug output).txt"));

		// Parse OmniMIDI's current version
		GetModuleFileName(hinst, MainLibrary, MAX_PATH);
		PathRemoveFileSpec(MainLibrary);
		wcscat_s(MainLibrary, MAX_PATH, L"\\OmniMIDI.dll");
		int major, minor, build, revision;
		GetVersionInfo(MainLibrary, major, minor, build, revision);

		// Open the debug output's file
		if (ManagedSettings.DebugMode == 1)
			_wfreopen(DebugDir, L"w", stdout);
		else
			fprintf(stdout, "Enabled file-less debug log.\n\n");

		std::lock_guard<std::mutex> lock(DebugMutex);

		// Begin writing to it
		fprintf(stdout, "Those who cannot change their minds cannot change anything.\n\n");
		fprintf(stdout, "OmniMIDI %d.%d.%d CR%d (KDMAPI %d.%d.%d, Revision %d)\n", major, minor, build, revision, CUR_MAJOR, CUR_MINOR, CUR_BUILD, CUR_REV);
		fprintf(stdout, "%d threads available to the ASIO engine\n", std::thread::hardware_concurrency());
		fprintf(stdout, "Copyright(C) 2013 - KaleidonKep99\n\n");
		IntroAlreadyShown = TRUE;
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
	fprintf(stdout, "%02d-%02d-%04d %02d:%02d:%02d.%03d - ",
		stime.wDay, stime.wMonth, stime.wYear, stime.wHour, stime.wMinute, stime.wSecond, stime.wMilliseconds);
}

void PrintMMToDebugLog(UINT uDID, UINT uMsg, DWORD_PTR dwUser, DWORD_PTR dwParam1, DWORD_PTR dwParam2) {
	if (ManagedSettings.DebugMode) {
		char Msg[NTFS_MAX_PATH] = { 0 };

		// Debug log is busy now
		std::lock_guard<std::mutex> lock(DebugMutex);

		// Print to log
		PrintCurrentTime();
		try {
			sprintf(Msg, "Stage <<modMessage>> | uDeviceID-> %d, uMsg-> %d, dwUser-> %d (LPVOID: %d), dwParam1-> %d, dwParam2-> %d", 
				uDID, uMsg, (DWORD)dwUser, *(DWORD_PTR*)dwUser, (DWORD)dwParam1, (DWORD)dwParam2);
		}
		catch (...) {
			try {
				sprintf(Msg, "Stage <<modMessage>> | uDeviceID-> %d, uMsg-> %d, dwUser-> %d (LPVOID: %d), dwParam1-> %d, dwParam2-> %d",
					uDID, uMsg, (DWORD)dwUser, (DWORD_PTR*)dwUser, (DWORD)dwParam1, (DWORD)dwParam2);
			}
			catch (...) {
				sprintf(Msg, "Stage <<modMessage>> | uDeviceID-> %d, uMsg-> %d, dwUser-> %d (LPVOID: FAIL), dwParam1-> %d, dwParam2-> %d",
					uDID, uMsg, (DWORD)dwUser, (DWORD)dwParam1, (DWORD)dwParam2);
			}
		}
	
		fprintf(stdout, Msg);
		OutputDebugStringA(Msg);

		// Flush buffer
		fflush(stdout);
	}
}

void PrintLoadedDLLToDebugLog(LPCWSTR LibraryW, LPCSTR Status) {
	if (ManagedSettings.DebugMode) {
		char Msg[NTFS_MAX_PATH] = { 0 };
		char LibraryA[MAX_PATH] = { 0 };
		wcstombs(LibraryA, LibraryW, wcslen(LibraryW) + 1);

		// Debug log is busy now
		std::lock_guard<std::mutex> lock(DebugMutex);

		// Print to log
		PrintCurrentTime();
		sprintf(Msg, "Library <<%s>> | %s\n", LibraryA, Status);
		fprintf(stdout, Msg);
		OutputDebugStringA(Msg);

		// Flush buffer
		fflush(stdout);
	}
}

void PrintSoundFontToDebugLog(LPCWSTR SoundFontW, LPCSTR Status) {
	if (ManagedSettings.DebugMode) {
		char Msg[NTFS_MAX_PATH] = { 0 };
		char SoundFontA[MAX_PATH] = { 0 };
		char * SoundFontNameA;
		wcstombs(SoundFontA, SoundFontW, wcslen(SoundFontW) + 1);
		SoundFontNameA = PathFindFileNameA(SoundFontA);

		// Debug log is busy now
		std::lock_guard<std::mutex> lock(DebugMutex);

		// Print to log
		PrintCurrentTime();
		sprintf(Msg, "Stage <<NewSFLoader>> | SoundFont \"%s\" -> %s\n", SoundFontNameA, Status);
		fprintf(stdout, Msg);
		OutputDebugStringA(Msg);

		// Flush buffer
		fflush(stdout);
	}
}

void PrintMessageToDebugLog(LPCSTR Stage, LPCSTR Status) {
	if (ManagedSettings.DebugMode) {
		char Msg[NTFS_MAX_PATH] = { 0 };

		// Debug log is busy now
		std::lock_guard<std::mutex> lock(DebugMutex);

		// Print to log
		PrintCurrentTime();
		sprintf(Msg, "Stage <<%s>> | %s\n", Stage, Status);
		fprintf(stdout, Msg);
		OutputDebugStringA(Msg);

		// Flush buffer
		fflush(stdout);
	}
}

void PrintStreamValueToDebugLog(LPCSTR Stage, LPCSTR ValueName, DWORD Value) {
	if (ManagedSettings.DebugMode) {
		char Msg[NTFS_MAX_PATH] = { 0 };

		// Debug log is busy now
		std::lock_guard<std::mutex> lock(DebugMutex);

		// Print to log
		PrintCurrentTime();
		sprintf(Msg, "Stage <<%s>> | %s: %d\n", Stage, ValueName, Value);
		fprintf(stdout, Msg);
		OutputDebugStringA(Msg);

		// Flush buffer
		fflush(stdout);
	}
}

void PrintBASSErrorMessageToDebugLog(LPCWSTR ErrorTitle, LPCWSTR ErrorDesc) {
	if (ManagedSettings.DebugMode) {
		wchar_t Msg[NTFS_MAX_PATH] = { 0 };

		// Debug log is busy now
		std::lock_guard<std::mutex> lock(DebugMutex);

		// Print to log
		PrintCurrentTime();
		swprintf(Msg, L"BASS error <<%s>> encountered | %s\n", ErrorTitle, ErrorDesc);
		fwprintf(stdout, Msg);
		OutputDebugStringW(Msg);

		// Flush buffer
		fflush(stdout);
	}
}

void PrintMemoryMessageToDebugLog(LPCSTR Stage, LPCSTR Status, BOOL IsRatio, ULONGLONG Memory) {
	if (ManagedSettings.DebugMode) {
		char Msg[NTFS_MAX_PATH] = { 0 };

		// Debug log is busy now
		std::lock_guard<std::mutex> lock(DebugMutex);

		// Print to log
		PrintCurrentTime();
		sprintf(Msg, "Stage <<%s>> | %s: %u\n", Stage, Status, Memory);
		fprintf(stdout, Msg);
		OutputDebugStringA(Msg);

		// Flush buffer
		fflush(stdout);
	}
}

void PrintMIDIOPENDESCToDebugLog(LPCSTR Stage, MIDIOPENDESC* MIDIOD, DWORD Flags) {
	if (ManagedSettings.DebugMode) {
		char Msg[NTFS_MAX_PATH] = { 0 };

		// Debug log is busy now
		std::lock_guard<std::mutex> lock(DebugMutex);

		// Print to log
		PrintCurrentTime();
		sprintf(Msg, "Stage <<%s>> | HMIDI: %08X - dwCallback: %08X - dwInstance: %08X - OMFlags: %08X\n", Stage, MIDIOD->hMidi, MIDIOD->dwCallback, MIDIOD->dwInstance, Flags);

		fprintf(stdout, Msg);
		OutputDebugStringA(Msg);

		// Flush buffer
		fflush(stdout);
	}
}

void PrintMIDIHDRToDebugLog(LPCSTR Stage, MIDIHDR* IIMidiHdr) {
	if (ManagedSettings.DebugMode) {
		char Msg[NTFS_MAX_PATH] = { 0 };

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

		fprintf(stdout, Msg);
		OutputDebugStringA(Msg);

		// Flush buffer
		fflush(stdout);
	}
}

void PrintEventToDebugLog(DWORD dwParam) {
	if (ManagedSettings.DebugMode) {
		char Msg[NTFS_MAX_PATH] = { 0 };

		// Debug log is busy now
		std::lock_guard<std::mutex> lock(DebugMutex);

		// Print to log
		PrintCurrentTime();
		sprintf(Msg, "Stage <<MIDIEvent | %08X>> | Event ID: 0x%X, Channel: %u, Note: %u, Velocity: %u\n", 
			dwParam, dwParam & 0xFF, GETCHANNEL(dwParam), GETVELOCITY(dwParam), GETNOTE(dwParam));

		fprintf(stdout, Msg);
		OutputDebugStringA(Msg);

		// Flush buffer
		fflush(stdout);
	}
}

void PrintLongMessageToDebugLog(BOOL IsRecognized, MIDIHDR* IIMidiHdr) {
	if (ManagedSettings.DebugMode) {
		char Msg[NTFS_MAX_PATH] = { 0 };

		// Debug log is busy now
		std::lock_guard<std::mutex> lock(DebugMutex);

		// Print to log
		PrintCurrentTime();
		sprintf(Msg, "Stage <<%s>> | %s long message: ", (IsRecognized ? "ParsedLongMsg" : "UnknownLongMsg"), (IsRecognized ? "Parsed" : "Unknown"));

		for (int i = 0; i < IIMidiHdr->dwBytesRecorded; i++)
			sprintf(Msg + strlen(Msg), "%02X", (BYTE)(IIMidiHdr->lpData[i]));

		sprintf(Msg + strlen(Msg), " (Recorded bytes: %u)", IIMidiHdr->dwBytesRecorded);

		fprintf(stdout, Msg);
		OutputDebugStringA(Msg);

		// Flush buffer
		fflush(stdout);
	}
}

BOOL EnableBuiltInHandler(LPCSTR Stage) {
	if (ManagedSettings.DebugMode) {
		PrintMessageToDebugLog(Stage, "Initializing OmniMIDICrashHandler...");
		if (NULL == (ExceptionHandler = AddVectoredExceptionHandler(1, OmniMIDICrashHandler))) {
			MessageBoxA(NULL, "An error has occured while initializing the built-in crash handler.", "OmniMIDI - ERROR", MB_ICONERROR | MB_OK | MB_SYSTEMMODAL);
			return FALSE;
		}
	}

	return TRUE;
}

BOOL DisableBuiltInHandler(LPCSTR Stage) {
	if (ExceptionHandler != nullptr) {
		PrintMessageToDebugLog(Stage, "Removing OmniMIDICrashHandler...");
		if (!RemoveVectoredExceptionHandler(ExceptionHandler)) {
			CrashMessage("DIsableBuiltInHandlerFail");
			return FALSE;
		}
	}

	return TRUE;
}

std::string GetLastErrorAsString()
{
	//Get the error message, if any.
	DWORD errorMessageID = ::GetLastError();
	if (errorMessageID == 0)
		return std::string(); //No error message has been recorded

	LPSTR messageBuffer = nullptr;
	size_t size = FormatMessageW(FORMAT_MESSAGE_ALLOCATE_BUFFER | FORMAT_MESSAGE_FROM_SYSTEM | FORMAT_MESSAGE_IGNORE_INSERTS,
		NULL, errorMessageID, MAKELANGID(LANG_NEUTRAL, SUBLANG_ENGLISH_US), (LPWSTR)&messageBuffer, 0, NULL);

	std::string message(messageBuffer, size);

	//Free the buffer.
	LocalFree(messageBuffer);

	return message;
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
			{
				std::string Error = GetLastErrorAsString();
				CrashMessage(Error.c_str());
			}
			else PipeVal++;
		}
	}
}

MMRESULT DebugResult(MMRESULT ErrorToDisplay, LPCSTR ExactError) {
	if (ManagedSettings.DebugMode) {
		if (ErrorToDisplay == MMSYSERR_NOERROR) return MMSYSERR_NOERROR;

		CHAR ErrorTitle[512] = { 0 };
		CHAR ErrorString[512] = { 0 };

		switch (ErrorToDisplay) {
			CurrentError(ErrorTitle, ErrorString, MMSYSERR_NOMEM, "The system is unable to allocate or lock memory.");
			CurrentError(ErrorTitle, ErrorString, MMSYSERR_ALLOCATED, "The driver has been already allocated in a previous midiStreamOpen/midiOutOpen call.");
			CurrentError(ErrorTitle, ErrorString, MMSYSERR_MOREDATA, "The driver has more data to return, but the MIDI application doesn't let it return data quickly enough.");
			CurrentError(ErrorTitle, ErrorString, MMSYSERR_NODRIVERCB, "The driver does not call DriverCallback.");
			CurrentError(ErrorTitle, ErrorString, MMSYSERR_NODRIVER, "No device driver is present.");
			CurrentError(ErrorTitle, ErrorString, MMSYSERR_HANDLEBUSY, "The specified handle is being used simultaneously by another thread.");
			CurrentError(ErrorTitle, ErrorString, MMSYSERR_INVALIDALIAS, "The specified alias was not found.");
			CurrentError(ErrorTitle, ErrorString, MMSYSERR_INVALHANDLE, "The handle of the specified device is invalid.");
			CurrentError(ErrorTitle, ErrorString, MMSYSERR_INVALFLAG, "An invalid flag was passed to modMessage through argument dwParam2.");
			CurrentError(ErrorTitle, ErrorString, MMSYSERR_INVALPARAM, "An invalid parameter was passed to modMessage.");
			CurrentError(ErrorTitle, ErrorString, MMSYSERR_NOTENABLED, "The driver failed to load or initialize.");
			CurrentError(ErrorTitle, ErrorString, MMSYSERR_NOTSUPPORTED, "The function requested by the message is not supported.");
			CurrentError(ErrorTitle, ErrorString, MIDIERR_NOTREADY, "The hardware is busy with other data.");
			CurrentError(ErrorTitle, ErrorString, MIDIERR_UNPREPARED, "The buffer pointed to by lpMidiOutHdr has not been prepared.");
			CurrentError(ErrorTitle, ErrorString, MIDIERR_STILLPLAYING, "Buffers are still in the queue.");
			CurrentError(ErrorTitle, ErrorString, MMSYSERR_BADDEVICEID, "The specified device ID is out of range.");
		default:
			sprintf_s(ErrorTitle, 512, "MMSYSERR_ERROR");
			sprintf_s(ErrorString, 512, "Unspecified error.");
			break;
		}

		if (ExactError) {
			strcat(ErrorString, "\n\nCause: ");
			strcat(ErrorString, ExactError);
		}

		strcat(ErrorString, "\n\nIf you're the developer of this app, please check if all the MIDI calls have been done correctly.");
		PrintMessageToDebugLog(ErrorTitle, ErrorString);
		MessageBoxA(NULL, ErrorString, "OmniMIDI - WinMM API ERROR", MB_OK | MB_ICONHAND | MB_SYSTEMMODAL);

		return ErrorToDisplay;
	}
}