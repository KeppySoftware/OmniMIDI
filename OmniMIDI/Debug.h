/*
OmniMIDI debug functions
*/

static BOOL IntroAlreadyShown = FALSE;
static LightweightLock DebugLogLockSystem;	// LockSystem

void Pointer(LPCWSTR Msg) {
	MessageBoxW(NULL, Msg, L"Debug pointer", MB_OK | MB_SYSTEMMODAL | MB_ICONINFORMATION);
}

void AppName() {
	try {
		ZeroMemory(modulename, MAX_PATH * sizeof(char));
		ZeroMemory(bitapp, MAX_PATH * sizeof(char));
		GetModuleFileNameExA(GetCurrentProcess(), NULL, modulename, MAX_PATH);
		GetModuleFileNameExW(GetCurrentProcess(), NULL, modulenameW, MAX_PATH);
		modulenameWp = PathFindFileName(modulenameW);
#if defined(_WIN64)
		strcpy(bitapp, "64-bit");
#elif defined(_WIN32)
		strcpy(bitapp, "32-bit");
#endif
	}
	catch (...) {
		CrashMessage("AppAnalysis");
	}
}

void CreateConsole() {
	if (!IntroAlreadyShown) {
		Beep(1000, 100);
		Beep(1000, 100);

		// Create file and start console output
		AppName();
		TCHAR installpath[MAX_PATH];
		TCHAR pathfortext[MAX_PATH];
		TCHAR CurrentTime[MAX_PATH];

		// Get user profile's path
		SHGetFolderPath(NULL, CSIDL_PROFILE, NULL, 0, pathfortext);

		// Append "\OmniMIDI\debug\" to "%userprofile%"
		PathAppend(pathfortext, _T("\\OmniMIDI\\debug\\"));

		// Create "%userprofile%\OmniMIDI\debug\", in case it doesn't exist
		CreateDirectory(pathfortext, NULL);

		// Append the app's filename to the output file's path
		lstrcat(pathfortext, modulenameWp);

		// Parse current time, and append it
		struct tm *sTm;
		time_t now = time(0);
		sTm = gmtime(&now);
		wcsftime(CurrentTime, sizeof(CurrentTime), L" - %d-%m-%Y %H.%M.%S", sTm);
		lstrcat(pathfortext, CurrentTime);

		// Append file extension, and that's it
		lstrcat(pathfortext, _T(" (Debug output).txt"));

		// Parse OmniMIDI's current version
		GetModuleFileName(hinst, installpath, MAX_PATH);
		PathRemoveFileSpec(installpath);
		lstrcat(installpath, L"\\OmniMIDI.dll");
		int major, minor, build, revision;
		GetVersionInfo(installpath, major, minor, build, revision);

		// Open the debug output's file
		_wfreopen(pathfortext, L"w", stdout);

		// Begin writing to it
		hConsole = GetStdHandle(STD_OUTPUT_HANDLE);
		SetConsoleTitle(L"OmniMIDI Debug Console");
		std::cout << "Be the change that you wish to see in the world.";
		std::cout << std::endl;
		std::cout << "OmniMIDI Version " << major << "." << minor << "." << build << "." << revision;
		std::cout << std::endl << "Copyright(C) 2013 - KaleidonKep99";
		std::cout << std::endl;
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
	else {
		// It doesn't, return false
		return false;
	}
}

void PrintMessageToDebugLog(LPCSTR Stage, LPCSTR Status) {
	if (ManagedSettings.DebugMode) {
		// Wait while debug log is busy
		while (DebugLogLockSystem.GetWriterConut() > 0) {}

		// Debug log is busy now
		DebugLogLockSystem.LockForWriting();

		// Get time
		SYSTEMTIME stime;
		FILETIME ltime;
		FILETIME ftTimeStamp;
		char TimeStamp[MAX_PATH];

		GetSystemTimeAsFileTime(&ftTimeStamp); //Gets the current system time
		FileTimeToLocalFileTime(&ftTimeStamp, &ltime);//convert in local time and store in ltime
		FileTimeToSystemTime(&ltime, &stime);//convert in system time and store in stime

		sprintf(TimeStamp, "%02d-%02d-%04d %02d:%02d:%02d.%03d", stime.wDay, stime.wMonth, stime.wYear, stime.wHour, stime.wMinute, stime.wSecond,
			stime.wMilliseconds);

		// Print to log
		std::cout << std::endl << TimeStamp << " - Stage <<" << Stage  << ">> | " << Status;

		// Debug log is free now
		DebugLogLockSystem.UnlockForWriting();
	}
}

void PrintMemoryMessageToDebugLog(LPCSTR Stage, LPCSTR Status, BOOL IsRatio, ULONGLONG Memory) {
	if (ManagedSettings.DebugMode) {
		// Wait while debug log is busy
		while (DebugLogLockSystem.GetWriterConut() > 0) { }

		// Debug log is busy now
		DebugLogLockSystem.LockForWriting();

		// Get time
		SYSTEMTIME stime;
		FILETIME ltime;
		FILETIME ftTimeStamp;
		char TimeStamp[MAX_PATH];

		GetSystemTimeAsFileTime(&ftTimeStamp); //Gets the current system time
		FileTimeToLocalFileTime(&ftTimeStamp, &ltime);//convert in local time and store in ltime
		FileTimeToSystemTime(&ltime, &stime);//convert in system time and store in stime

		sprintf(TimeStamp, "%02d-%02d-%04d %02d:%02d:%02d.%03d", stime.wDay, stime.wMonth, stime.wYear, stime.wHour, stime.wMinute, stime.wSecond,
			stime.wMilliseconds);

		// Print to log
		std::cout << std::endl << TimeStamp << 
			" - Stage <<" << Stage << ">> | " << 
			Status << (IsRatio ? " | Ratio: " : " | Memory: ") << Memory << (IsRatio ? "" : " bytes");

		// Debug log is free now
		DebugLogLockSystem.UnlockForWriting();
	}
}

void PrintSysExMessageToDebugLog(BOOL IsRecognized, MIDIHDR* IIMidiHdr) {
	if (ManagedSettings.DebugMode) {
		// Wait while debug log is busy
		while (DebugLogLockSystem.GetWriterConut() > 0) {}

		// Debug log is busy now
		DebugLogLockSystem.LockForWriting();

		// Get time
		SYSTEMTIME stime;
		FILETIME ltime;
		FILETIME ftTimeStamp;
		char TimeStamp[MAX_PATH];

		GetSystemTimeAsFileTime(&ftTimeStamp); //Gets the current system time
		FileTimeToLocalFileTime(&ftTimeStamp, &ltime);//convert in local time and store in ltime
		FileTimeToSystemTime(&ltime, &stime);//convert in system time and store in stime

		sprintf(TimeStamp, "%02d-%02d-%04d %02d:%02d:%02d.%03d", stime.wDay, stime.wMonth, stime.wYear, stime.wHour, stime.wMinute, stime.wSecond,
			stime.wMilliseconds);

		// Print to log
		std::cout << std::endl << TimeStamp << 
			(IsRecognized ? " - Stage <<UnrecognizedSysEx>> | Unrecognized SysEx event: " : " - Stage <<ParsedSysEx>> | Parsed SysEx event: ");

		for (const char* p = IIMidiHdr->lpData; *p; ++p)
			std::cout << std::hex << (int)*p;

		std::cout << std::dec << " (" << IIMidiHdr->dwBufferLength << " bytes)";

		// Debug log is free now
		DebugLogLockSystem.UnlockForWriting();
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

void StartDebugPipe(BOOL restart) {
	// Initialize the current pipe count and template
	static unsigned int PipeVal = 0;
	static const WCHAR PipeName[] = TEXT("\\\\.\\pipe\\OmniMIDIDbg%u");
	WCHAR PipeDes[MAX_PATH];

Retry:
	// If this isn't a restart, add 1 to PipeVal
	if (!restart) PipeVal++;
	// Else, close the pipe and reopen it
	else CloseHandle(hPipe);

	// Clear the WCHAR, since it might contain garbage, 
	// and print the template with PipeVal in it
	// (Ex. "\\\\.\\pipe\\OmniMIDIDbg1")
	ZeroMemory(PipeDes, MAX_PATH);
	swprintf_s(PipeDes, MAX_PATH, PipeName, PipeVal);

	// Now create the pipe
	hPipe = CreateNamedPipe(PipeDes,
		PIPE_ACCESS_DUPLEX | FILE_FLAG_FIRST_PIPE_INSTANCE,
		PIPE_TYPE_BYTE | PIPE_READMODE_BYTE | PIPE_WAIT,
		PIPE_UNLIMITED_INSTANCES,
		1024,
		1024,
		NMPWAIT_USE_DEFAULT_WAIT,
		NULL);

	// Check if the pipe failed to be initialized
	if (hPipe == INVALID_HANDLE_VALUE)
	{
		// It did. If the pipe value isn't above the maximum instances, try again
		if (PipeVal <= PIPE_UNLIMITED_INSTANCES) goto Retry;
		// Else fail, something happened
		else {
			std::string Error = GetLastErrorAsString();
			CrashMessage(Error.c_str());
		}
	}
}

MMRESULT DebugResult(MMRESULT ErrorToDisplay) {
	switch (ErrorToDisplay) {
	case MIDIERR_NOTREADY:
		MessageBox(NULL, L"OmniMIDI is not ready to accept the MIDIHDR!", L"OmniMIDI - Debug Info", MB_OK | MB_ICONHAND | MB_SYSTEMMODAL);
		PrintMessageToDebugLog("MIDIERR_NOTREADY", "OmniMIDI is not ready to accept the MIDIHDR!");
	case MIDIERR_UNPREPARED:	
		MessageBox(NULL, L"The MIDIHDR buffer hasn't been prepared yet!", L"OmniMIDI - Debug Info", MB_OK | MB_ICONHAND | MB_SYSTEMMODAL);
		PrintMessageToDebugLog("MIDIERR_UNPREPARED", "The MIDIHDR buffer hasn't been prepared yet!");
	case MIDIERR_STILLPLAYING:	
		MessageBox(NULL, L"The MIDIHDR buffer is still being played!", L"OmniMIDI - Debug Info", MB_OK | MB_ICONHAND | MB_SYSTEMMODAL);
		PrintMessageToDebugLog("MIDIERR_STILLPLAYING", "The MIDIHDR buffer is still being played!");
	case MMSYSERR_INVALPARAM:
		MessageBox(NULL, L"The pointer to the MIDIHDR is invalid!", L"OmniMIDI - Debug Info", MB_OK | MB_ICONHAND | MB_SYSTEMMODAL);
		PrintMessageToDebugLog("MMSYSERR_INVALPARAM", "The pointer to the MIDIHDR is invalid!");
	}

	return ErrorToDisplay;
}