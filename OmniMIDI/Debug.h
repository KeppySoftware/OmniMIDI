/*
OmniMIDI debug functions
*/

static std::mutex DebugMutex;
static BOOL IntroAlreadyShown = FALSE;
static BOOL InfoAlreadyGot = FALSE;

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

	throw ErrorID;
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

void PrintMIDIHDRToDebugLog(LPCSTR Stage, MIDIHDR* IIMidiHdr) {
	if (ManagedSettings.DebugMode) {
		char Msg[NTFS_MAX_PATH] = { 0 };

		// Debug log is busy now
		std::lock_guard<std::mutex> lock(DebugMutex);

		FILE* fo = fopen("mididata.bin", "ab");
		fwrite(IIMidiHdr->lpData, 1, IIMidiHdr->dwBytesRecorded, fo);
		fflush(fo);
		fclose(fo);

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

void PrintSysExMessageToDebugLog(BOOL IsRecognized, MIDIHDR* IIMidiHdr) {
	if (ManagedSettings.DebugMode) {
		char Msg[NTFS_MAX_PATH] = { 0 };

		// Debug log is busy now
		std::lock_guard<std::mutex> lock(DebugMutex);

		// Print to log
		PrintCurrentTime();
		sprintf(Msg, "Stage %s ", (IsRecognized ? "<<UnrecognizedSysEx>> | Unrecognized SysEx event:" : "<<ParsedSysEx>> | Parsed SysEx event:"));

		for (int i = 0; i < IIMidiHdr->dwBytesRecorded; i++)
			sprintf(Msg + strlen(Msg), "%02X", (BYTE)(IIMidiHdr->lpData[i]));

		sprintf(Msg + strlen(Msg), " (Recorded bytes: %u)\n", IIMidiHdr->dwBytesRecorded);

		fprintf(stdout, Msg);
		OutputDebugStringA(Msg);

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
			PIPE_ACCESS_DUPLEX | FILE_FLAG_FIRST_PIPE_INSTANCE,
			PIPE_TYPE_BYTE | PIPE_READMODE_BYTE | PIPE_WAIT,
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

MMRESULT DebugResult(MMRESULT ErrorToDisplay, BOOL ShowError) {
	if (ErrorToDisplay == MMSYSERR_NOERROR) return MMSYSERR_NOERROR;

	const DWORD MaxSize = 512;
	CHAR ErrorTitle[MaxSize] = { 0 };
	CHAR ErrorString[MaxSize] = { 0 };

	switch (ErrorToDisplay) {
	case MMSYSERR_NOMEM:
		sprintf_s(ErrorTitle, MaxSize, "MMSYSERR_NOMEM");
		sprintf_s(ErrorString, MaxSize, "The system is unable to allocate or lock memory.");
		break;
	case MMSYSERR_ALLOCATED:
		sprintf_s(ErrorTitle, MaxSize, "MMSYSERR_ALLOCATED");
		sprintf_s(ErrorString, MaxSize, "The driver has been already allocated in a previous midiOutOpen call.");
		break;
	case MMSYSERR_MOREDATA:
		sprintf_s(ErrorTitle, MaxSize, "MMSYSERR_MOREDATA");
		sprintf_s(ErrorString, MaxSize, "The driver has more data to return, but the MIDI application doesn't let it return data quickly enough.");
		break;
	case MMSYSERR_NODRIVERCB:
		sprintf_s(ErrorTitle, MaxSize, "MMSYSERR_NODRIVERCB");
		sprintf_s(ErrorString, MaxSize, "The driver does not call DriverCallback.");
		break;
	case MMSYSERR_NODRIVER:
		sprintf_s(ErrorTitle, MaxSize, "MMSYSERR_NODRIVERCB");
		sprintf_s(ErrorString, MaxSize, "No device driver is present.");
		break;
	case MMSYSERR_HANDLEBUSY:
		sprintf_s(ErrorTitle, MaxSize, "MMSYSERR_HANDLEBUSY");
		sprintf_s(ErrorString, MaxSize, "The specified handle is being used simultaneously by another thread.");
		break;
	case MMSYSERR_INVALIDALIAS:
		sprintf_s(ErrorTitle, MaxSize, "MMSYSERR_INVALIDALIAS");
		sprintf_s(ErrorString, MaxSize, "The specified alias was not found.");
		break;
	case MMSYSERR_INVALHANDLE:
		sprintf_s(ErrorTitle, MaxSize, "MMSYSERR_INVALHANDLE");
		sprintf_s(ErrorString, MaxSize, "The handle of the specified device is invalid.");
		break;
	case MMSYSERR_INVALFLAG:
		sprintf_s(ErrorTitle, MaxSize, "MMSYSERR_INVALFLAG");
		sprintf_s(ErrorString, MaxSize, "An invalid flag was passed to modMessage.");
		break;
	case MMSYSERR_INVALPARAM:
		sprintf_s(ErrorTitle, MaxSize, "MMSYSERR_INVALPARAM");
		sprintf_s(ErrorString, MaxSize, "An invalid parameter was passed to modMessage.");
		break;
	case MMSYSERR_NOTENABLED:
		sprintf_s(ErrorTitle, MaxSize, "MMSYSERR_NOTENABLED");
		sprintf_s(ErrorString, MaxSize, "The driver failed to load or initialize.");
		break;
	case MMSYSERR_NOTSUPPORTED:
		sprintf_s(ErrorTitle, MaxSize, "MMSYSERR_NOTSUPPORTED");
		sprintf_s(ErrorString, MaxSize, "The function requested by the message is not supported.");
		break;
	case MIDIERR_NOTREADY:
		sprintf_s(ErrorTitle, MaxSize, "MIDIERR_NOTREADY");
		sprintf_s(ErrorString, MaxSize, "The hardware is busy with other data.");
		break;
	case MIDIERR_UNPREPARED:
		sprintf_s(ErrorTitle, MaxSize, "MIDIERR_UNPREPARED");
		sprintf_s(ErrorString, MaxSize, "The buffer pointed to by lpMidiOutHdr has not been prepared.");
		break;
	case MIDIERR_STILLPLAYING:
		sprintf_s(ErrorTitle, MaxSize, "MIDIERR_STILLPLAYING");
		sprintf_s(ErrorString, MaxSize, "Buffers are still in the queue.");
		break;
	case MMSYSERR_BADDEVICEID:
		sprintf_s(ErrorTitle, MaxSize, "MMSYSERR_BADDEVICEID");
		sprintf_s(ErrorString, MaxSize, "The specified device ID is out of range.");
		break;
	default:
		sprintf_s(ErrorTitle, MaxSize, "MMSYSERR_ERROR");
		sprintf_s(ErrorString, MaxSize, "Unspecified error.");
		break;
	}

	if (ManagedSettings.DebugMode)
		strcat(ErrorString, "\n\nIf you're the developer of this app, please check if all the MIDI calls have been done correctly.");

	if (ShowError) MessageBoxA(NULL, ErrorString, "OmniMIDI - WinMM API ERROR", MB_OK | MB_ICONHAND | MB_SYSTEMMODAL);
	PrintMessageToDebugLog(ErrorTitle, ErrorString);

	return ErrorToDisplay;
}