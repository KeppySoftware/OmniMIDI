/*
OmniMIDI debug functions
*/

static BOOL IntroAlreadyShown = FALSE;
static LightweightLock DebugLogLockSystem;	// LockSystem

void Pointer(LPCWSTR Msg) {
	MessageBoxW(NULL, Msg, L"Debug pointer", MB_OK | MB_SYSTEMMODAL | MB_ICONINFORMATION);
}

BOOL InfoAlreadyGot = FALSE;
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
		catch (...) {
			CrashMessage("AppAnalysis");
		}
	}
}

void CreateConsole() {
	if (!IntroAlreadyShown) {
		TCHAR MainLibrary[MAX_PATH];
		TCHAR DebugDir[MAX_PATH];
		TCHAR CurrentTime[MAX_PATH];

		Beep(440, 100);
		Beep(440, 100);

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
		_wfreopen(DebugDir, L"w", stdout);

		// Begin writing to it
		printf("Those who cannot change their minds cannot change anything.\n\n");
		printf("OmniMIDI %d.%d.%d CR%d (KDMAPI %d.%d.%d, Revision %d)\n", major, minor, build, revision, CUR_MAJOR, CUR_MINOR, CUR_BUILD, CUR_REV);
		printf("%d threads available to the ASIO engine\n", std::thread::hardware_concurrency());
		printf("Copyright(C) 2013 - KaleidonKep99\n\n");
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
	printf("%02d-%02d-%04d %02d:%02d:%02d.%03d - ", 
		stime.wDay, stime.wMonth, stime.wYear, stime.wHour, stime.wMinute, stime.wSecond, stime.wMilliseconds);
}

void PrintLoadedDLLToDebugLog(LPCWSTR LibraryW, LPCSTR Status) {
	if (ManagedSettings.DebugMode) {
		char LibraryA[MAX_PATH] = { 0 };
		wcstombs(LibraryA, LibraryW, wcslen(LibraryW) + 1);

		// Wait while debug log is busy
		while (DebugLogLockSystem.GetWriterCount() > 0) {}

		// Debug log is busy now
		DebugLogLockSystem.LockForWriting();

		// Print to log
		PrintCurrentTime();
		printf("Library <<%s>> | %s\n", LibraryA, Status);

		// Debug log is free now
		DebugLogLockSystem.UnlockForWriting();

		// Flush buffer
		fflush(stdout);
	}
}

void PrintMessageToDebugLog(LPCSTR Stage, LPCSTR Status) {
	if (ManagedSettings.DebugMode) {
		// Wait while debug log is busy
		while (DebugLogLockSystem.GetWriterCount() > 0) {}

		// Debug log is busy now
		DebugLogLockSystem.LockForWriting();

		// Print to log
		PrintCurrentTime();
		printf("Stage <<%s>> | %s\n", Stage, Status);

		// Debug log is free now
		DebugLogLockSystem.UnlockForWriting();

		// Flush buffer
		fflush(stdout);
	}
}

void PrintMemoryMessageToDebugLog(LPCSTR Stage, LPCSTR Status, BOOL IsRatio, ULONGLONG Memory) {
	if (ManagedSettings.DebugMode) {
		// Wait while debug log is busy
		while (DebugLogLockSystem.GetWriterCount() > 0) { }

		// Debug log is busy now
		DebugLogLockSystem.LockForWriting();

		// Print to log
		PrintCurrentTime();
		printf("Stage <<%s>> | %s: %u\n", Stage, Status, Memory);

		// Debug log is free now
		DebugLogLockSystem.UnlockForWriting();

		// Flush buffer
		fflush(stdout);
	}
}

void PrintSysExMessageToDebugLog(BOOL IsRecognized, MIDIHDR* IIMidiHdr) {
	if (ManagedSettings.DebugMode) {
		DWORD DataLen = 0;

		// Wait while debug log is busy
		while (DebugLogLockSystem.GetWriterCount() > 0) {}

		// Debug log is busy now
		DebugLogLockSystem.LockForWriting();

		// Print to log
		PrintCurrentTime();
		printf("Stage %s ", (IsRecognized ? "<<UnrecognizedSysEx>> | Unrecognized SysEx event:" : "<<ParsedSysEx>> | Parsed SysEx event:"));

		for (DataLen = 0; DataLen < IIMidiHdr->dwBufferLength; ++DataLen)
			printf("%X", IIMidiHdr->lpData[DataLen]);

		printf(" (%u bytes)\n", DataLen);

		// Debug log is free now
		DebugLogLockSystem.UnlockForWriting();

		// Flush buffer
		fflush(stdout);
	}
}

/*
/ Unused, but still here for people who wants to mess around with it

void StatusType(int status, char* &statustoprint) {
	// Pretty self explanatory

	std::string statusstring = "";

	if (Between(status, 0x80, 0xEF)) {
		if (Between(status, 0x80, 0x8F)) {
			statusstring += "Note OFF event on channel ";
			statusstring += std::to_string((status - 0x80));
		}
		else if (Between(status, 0x90, 0x9F)) {
			statusstring += "Note ON event on channel ";
			statusstring += std::to_string((status - 0x90));
		}
		else if (Between(status, 0xA0, 0xAF)) {
			statusstring += "Polyphonic aftertouch event on channel ";
			statusstring += std::to_string((status - 0xA0));
		}
		else if (Between(status, 0xB0, 0xBF)) {
			statusstring += "Channel reset on channel ";
			statusstring += std::to_string((status - 0xB0));
		}
		else if (Between(status, 0xC0, 0xCF)) {
			statusstring += "Program change on channel ";
			statusstring += std::to_string((status - 0xC0));
		}
		else if (Between(status, 0xD0, 0xDF)) {
			statusstring += "Channel aftertouch event on channel ";
			statusstring += std::to_string((status - 0xD0));
		}
		else if (Between(status, 0xE0, 0xEF)) {
			statusstring += "Pitch change on channel ";
			statusstring += std::to_string((status - 0xE0));
		}

		statustoprint = strdup(statusstring.c_str());
	}
	else if (status == 0xF0) statustoprint = "System Exclusive\0";
	else if (status == 0xF1) statustoprint = "System Common - undefined\0";
	else if (status == 0xF2) statustoprint = "Sys Com Song Position Pntr\0";
	else if (status == 0xF3) statustoprint = "Sys Com Song Select\0";
	else if (status == 0xF4) statustoprint = "System Common - undefined\0";
	else if (status == 0xF5) statustoprint = "System Common - undefined\0";
	else if (status == 0xF6) statustoprint = "Sys Com Tune Request\0";
	else if (status == 0xF7) statustoprint = "Sys Com-end of SysEx (EOX)\0";
	else if (status == 0xF8) statustoprint = "Sys Real Time Timing Clock\0";
	else if (status == 0xF9) statustoprint = "Sys Real Time - undefined\0";
	else if (status == 0xFA) statustoprint = "Sys Real Time Start\0";
	else if (status == 0xFB) statustoprint = "Sys Real Time Continue\0";
	else if (status == 0xFC) statustoprint = "Sys Real Time Stop\0";
	else if (status == 0xFD) statustoprint = "Sys Real Time - undefined\0";
	else if (status == 0xFE) statustoprint = "Sys Real Time Active Sensing\0";
	else if (status == 0xFF) statustoprint = "Sys Real Time Sys Reset\0";
	else statustoprint = "Unknown event\0";
}

*/

/*
/ Unused, but still here for people who wants to mess around with it

void PrintEventToConsole(int color, int stage, bool issysex, const char* text) {
	if (debugmode) {
		if (printmidievent) {
			// Set color
			SetConsoleTextAttribute(hConsole, color);

			// Get time
			char buff[20];
			struct tm *sTm;
			time_t now = time(0);
			sTm = gmtime(&now);
			strftime(buff, sizeof(buff), "%Y-%m-%d %H:%M:%S", sTm);

			// Print to log
			if (issysex) std::cout << std::endl << buff << " - (" << stage << ") - " << text << " ~ Type = SysEx event";
			else {
				// Get status
				char* statustoprint = { 0 };
				StatusType(stage & 0xFF, statustoprint);
				std::cout << std::endl << buff << " - (" << stage << ") - " << text << " ~ Channel = " << (stage & 0xF) << " | Type = " << statustoprint << " | Note = " << ((stage >> 8) & 0xFF) << " | Velocity = " << ((stage >> 16) & 0xFF);
			}
		}
	}
}

*/

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
		DisconnectNamedPipe(hPipe);
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
		hPipe = CreateNamedPipe(PipeDes,
			PIPE_ACCESS_DUPLEX | FILE_FLAG_FIRST_PIPE_INSTANCE | FILE_FLAG_OVERLAPPED,
			PIPE_TYPE_MESSAGE | PIPE_WAIT | PIPE_REJECT_REMOTE_CLIENTS,
			PIPE_UNLIMITED_INSTANCES,
			NTFS_MAX_PATH,
			0,
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

MMRESULT DebugResult(MMRESULT ErrorToDisplay) {
	const DWORD MaxSize = 512;
	CHAR ErrorTitle[MaxSize] = { 0 };
	CHAR ErrorString[MaxSize] = { 0 };

	switch (ErrorToDisplay) {
	case MMSYSERR_NOTENABLED:
		sprintf_s(ErrorTitle, MaxSize, "MMSYSERR_NOTENABLED");
		sprintf_s(ErrorString, MaxSize, "OmniMIDI has not been initialized yet, or has failed to.");
	case MMSYSERR_NOTSUPPORTED:
		sprintf_s(ErrorTitle, MaxSize, "MMSYSERR_NOTSUPPORTED");
		sprintf_s(ErrorString, MaxSize, "OmniMIDI does not support the requested function.");
	case MMSYSERR_INVALPARAM:
		sprintf_s(ErrorTitle, MaxSize, "MMSYSERR_INVALPARAM");
		sprintf_s(ErrorString, MaxSize, "The app passed an invalid parameter to OmniMIDI.");
	case MIDIERR_NOTREADY:
		sprintf_s(ErrorTitle, MaxSize, "MIDIERR_NOTREADY");
		sprintf_s(ErrorString, MaxSize, "OmniMIDI is not ready to accept the MIDIHDR.");
	case MIDIERR_UNPREPARED:	
		sprintf_s(ErrorTitle, MaxSize, "MIDIERR_UNPREPARED");
		sprintf_s(ErrorString, MaxSize, "The MIDIHDR buffer passed to OmniMIDI hasn't been prepared yet.");
	case MIDIERR_STILLPLAYING:	
		sprintf_s(ErrorTitle, MaxSize, "MIDIERR_STILLPLAYING");
		sprintf_s(ErrorString, MaxSize, "A MIDIHDR buffer is still being played by OmniMIDI.");
	default:
		sprintf_s(ErrorTitle, MaxSize, "MMSYSERR_ERROR");
		sprintf_s(ErrorString, MaxSize, "Windows Multimedia encountered an unknown error.");
	}

	MessageBoxA(NULL, ErrorString, "OmniMIDI - WinMM error", MB_OK | MB_ICONHAND | MB_SYSTEMMODAL);
	PrintMessageToDebugLog(ErrorTitle, ErrorString);

	return ErrorToDisplay;
}