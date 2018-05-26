/*
Keppy's Synthesizer errors list
*/

static TCHAR * BASSErrorCode[48] =
{
	L"BASS_ERROR_UNKNOWN",																						// Error -1
	L"BASS_OK",																									// Error 0
	L"BASS_ERROR_MEM",																							// Error 1
	L"BASS_ERROR_FILEOPEN",																						// Error 2
	L"BASS_ERROR_DRIVER",																						// Error 3
	L"BASS_ERROR_BUFLOST",																						// Error 4
	L"BASS_ERROR_HANDLE",																						// Error 5
	L"BASS_ERROR_FORMAT",																						// Error 6
	L"BASS_ERROR_POSITION",																						// Error 7
	L"BASS_ERROR_INIT",																							// Error 8
	L"BASS_ERROR_START",																						// Error 9
	L"BASS_ERROR_SSL",																							// Error 10
	L"Unknown",																									// Error 11
	L"Unknown",																									// Error 12
	L"Unknown",																									// Error 13
	L"BASS_ERROR_ALREADY",																						// Error 14
	L"Unknown",																									// Error 15
	L"Unknown",																									// Error 16
	L"Unknown",																									// Error 17
	L"BASS_ERROR_NOCHAN",																						// Error 18
	L"BASS_ERROR_ILLTYPE",																						// Error 19
	L"BASS_ERROR_ILLPARAM",																						// Error 20
	L"BASS_ERROR_NO3D",																							// Error 21
	L"BASS_ERROR_NOEAX",																						// Error 22
	L"BASS_ERROR_DEVICE",																						// Error 23
	L"BASS_ERROR_NOPLAY",																						// Error 24
	L"BASS_ERROR_FREQ",																							// Error 25
	L"Unknown",																									// Error 26
	L"BASS_ERROR_NOTFILE",																						// Error 27
	L"Unknown",																									// Error 28
	L"BASS_ERROR_NOHW",																							// Error 29
	L"Unknown",																									// Error 30
	L"BASS_ERROR_EMPTY",																						// Error 31
	L"BASS_ERROR_NONET",																						// Error 32
	L"BASS_ERROR_CREATE",																						// Error 33
	L"BASS_ERROR_NOFX",																							// Error 34
	L"BASS_ERROR_PLAYING",																						// Error 35
	L"Unknown",																									// Error 36
	L"BASS_ERROR_NOTAVAIL",																						// Error 37
	L"BASS_ERROR_DECODE",																						// Error 38
	L"BASS_ERROR_DX",																							// Error 39
	L"BASS_ERROR_TIMEOUT",																						// Error 40
	L"BASS_ERROR_FILEFORM",																						// Error 41
	L"BASS_ERROR_SPEAKER",																						// Error 42
	L"BASS_ERROR_VERSION",																						// Error 43
	L"BASS_ERROR_CODEC",																						// Error 44
	L"BASS_ERROR_ENDED",																						// Error 45
	L"BASS_ERROR_BUSY",																							// Error 46
};

static TCHAR * BASSWASAPIErrorCode[2] =
{
	L"BASS_ERROR_WASAPI",																						// Error 5200
	L"BASS_ERROR_WASAPI_BUFFER",																				// Error 5201
};

static TCHAR * BASSErrorDesc[48] =
{
	L"Unknown error.",																							// Description of error -1
	L"Nothing's wrong.",																						// Description of error 0
	L"There is insufficient memory.",																			// Description of error 1
	L"The file could not be opened.",																			// Description of error 2
	L"There is no available device driver. The device may already be in use.",									// Description of error 3
	L"The sample buffer was lost.",																				// Description of error 4
	L"An invalid handle has been used.",																		// Description of error 5
	L"The sample format is not supported by the device/drivers.",												// Description of error 6
	L"The requested position is invalid, eg. it is beyond the end or the download has not yet reached it.",		// Description of error 7
	L"BASS_Init has not been successfully called.",																// Description of error 8
	L"BASS_Start has not been successfully called.",															// Description of error 9
	L"SSL/HTTPS support isn't available.",																		// Description of error 10
	L"No description available.",																				// Description of error 11
	L"No description available.",																				// Description of error 12
	L"No description available.",																				// Description of error 13
	L"Stream already initialized.",																				// Description of error 14
	L"No description available.",																				// Description of error 15
	L"No description available.",																				// Description of error 16
	L"No description available.",																				// Description of error 17
	L"Can't get a free channel.",																				// Description of error 18
	L"An illegal type was specified.",																			// Description of error 19
	L"An illegal parameter was specified.",																		// Description of error 20
	L"No 3D support.",																							// Description of error 21
	L"No EAX support.",																							// Description of error 22
	L"Illegal device number.",																					// Description of error 23
	L"The stream is not playing.",																				// Description of error 24
	L"Illegal sample rate.",																					// Description of error 25
	L"No description available.",																				// Description of error 26
	L"The stream is not a file stream.",																		// Description of error 27
	L"No description available.",																				// Description of error 28
	L"No hardware voices available.",																			// Description of error 29
	L"No description available.",																				// Description of error 30
	L"The MOD music has no sequence data.",																		// Description of error 31
	L"No internet connection could be opened.",																	// Description of error 32
	L"Couldn't create the file.",																				// Description of error 33
	L"Effects are not available.",																				// Description of error 34
	L"The channel is playing.",																					// Description of error 35
	L"No description available.",																				// Description of error 36
	L"Requested data is not available.",																		// Description of error 37
	L"The channel is a \"decoding channel\".",																	// Description of error 38
	L"A sufficient DirectX version is not installed.",															// Description of error 39
	L"Connection timed out.",																					// Description of error 40
	L"Unsupported file format.",																				// Description of error 41
	L"Unavailable speaker.",																					// Description of error 42
	L"Invalid BASS version.",																					// Description of error 43
	L"Codec is not available/supported.",																		// Description of error 44
	L"The channel/file has ended.",																				// Description of error 45
	L"The device is busy (eg. in 'exclusive' use by another process).",											// Description of error 46
};

static TCHAR * BASSWASAPIErrorDesc[2] =
{
	L"WASAPI is unavailable, or its DLL is missing.",															// Description of error 5200
	L"The buffer is too large. You need a really small buffer value for exclusive mode.",						// Description of error 5201
};

static TCHAR * BASSErrorFix[48] =
{
	/* -1 */ L"The cause of the error is unknown, no description is available.",
	/* 0 */ L"Everything's fine.",
	/* 1 */ L"There's not enough available memory for the driver.\nIt might be caused by a really big SoundFont, or by the app itself.\n\nTry using a smaller SoundFont, or switch to the 64-bit version of the app, if available.",
	/* 2 */ L"Ensure the file you selected actually exists, and if the drive hosting the file is online.",
	/* 3 */ L"Another app might've took exclusive use of the selected audio device. Try closing all the other audio applications, then try again. Ensure you're not running another exclusive-mode instance of Keppy's Synthesizer.",
	/* 4 */ L"The sound card might've timed out, or the buffer size might not be big enough for it to handle. Try increasing the buffer size, or switch to another audio device.",
	/* 5 */ L"This is a serious error, please restart the application.\nIf it happens again, contact KaleidonKep99.",
	/* 6 */ L"You're using an audio frequency that isn't supported by the device. If it still works after pressing OK, then ignore this message, otherwise change the frequency in the configurator.",
	/* 7 */ L"Not used by Keppy's Synthesizer, contact KaleidonKep99.",
	/* 8 */ L"This is a serious error, please restart the application.\nIf it happens again, contact KaleidonKep99.",
	/* 9 */ L"This is a serious error, please restart the application.\nIf it happens again, contact KaleidonKep99.",
	/* 10 */ L"Not used by Keppy's Synthesizer, contact KaleidonKep99.",
	/* 11 */ L"The cause of the error is unknown, no description is available.",
	/* 12 */ L"The cause of the error is unknown, no description is available.",
	/* 13 */ L"The cause of the error is unknown, no description is available.",
	/* 14 */ L"This is a serious error, please restart the application.\nIf it happens again, contact KaleidonKep99.",
	/* 15 */ L"The cause of the error is unknown, no description is available.",
	/* 16 */ L"The cause of the error is unknown, no description is available.",
	/* 17 */ L"The cause of the error is unknown, no description is available.",
	/* 18 */ L"BASS or BASSMIDI are unable to allocate a stream channel. If you're using VirtualMIDISynth 1.x, please uninstall it, otherwise restart the application.",
	/* 19 */ L"This is a serious error, please restart the application.\nIf it happens again, contact KaleidonKep99.",
	/* 20 */ L"The ASIO/WASAPI device might be incompatible with a certain function you enabled in the settings. Try disabling that specific function, otherwise switch to another device",
	/* 21 */ L"Not used by Keppy's Synthesizer, contact KaleidonKep99.",
	/* 22 */ L"Not used by Keppy's Synthesizer, contact KaleidonKep99.",
	/* 23 */ L"The device you selected doesn't exist. Check the selected device in the configurator.",
	/* 24 */ L"The driver encountered an error, and called the same function twice. Restart the application.",
	/* 25 */ L"You're using an audio frequency that isn't supported by the device. Change the frequency in the configurator.",
	/* 26 */ L"Not used by Keppy's Synthesizer, contact KaleidonKep99.",
	/* 27 */ L"Not used by Keppy's Synthesizer, contact KaleidonKep99.",
	/* 28 */ L"Not used by Keppy's Synthesizer, contact KaleidonKep99.",
	/* 29 */ L"Not used by Keppy's Synthesizer, contact KaleidonKep99.",
	/* 30 */ L"Not used by Keppy's Synthesizer, contact KaleidonKep99.",
	/* 31 */ L"Not used by Keppy's Synthesizer, contact KaleidonKep99.",
	/* 32 */ L"Not used by Keppy's Synthesizer, contact KaleidonKep99.",
	/* 33 */ L"You might not have the required permissions to write the file, or BASS might have encountered an error while creating it.",
	/* 34 */ L"Not used by Keppy's Synthesizer, contact KaleidonKep99.",
	/* 35 */ L"The driver encountered an error, and called the same function twice. Restart the application.",
	/* 36 */ L"Not used by Keppy's Synthesizer, contact KaleidonKep99.",
	/* 37 */ L"The audio data wasn't ready yet to be picked up. This can be caused by a timeout in the buffer system, or by a dead audio stream. Try restarting the application.",
	/* 38 */ L"This is a serious error, please restart the application.\nIf it happens again, contact KaleidonKep99.",
	/* 39 */ L"Not used by Keppy's Synthesizer, contact KaleidonKep99.",
	/* 40 */ L"Not used by Keppy's Synthesizer, contact KaleidonKep99.",
	/* 41 */ L"Not used by Keppy's Synthesizer, contact KaleidonKep99.",
	/* 42 */ L"BASS is unable to use the speakers. Make sure nothing is having exclusive control over the selected audio device.",
	/* 43 */ L"This is a serious error, please reisntall the driver.\nIf it happens again, contact KaleidonKep99.",
	/* 44 */ L"Not used by Keppy's Synthesizer, contact KaleidonKep99.",
	/* 45 */ L"Not used by Keppy's Synthesizer, contact KaleidonKep99.",
	/* 46 */  L"Another app might've took exclusive use of the selected audio device. Try closing all the other audio applications, then try again. Ensure you're not running another exclusive-mode instance of Keppy's Synthesizer.",
};

static TCHAR * BASSWASAPIErrorFix[2] =
{
	L"You might be using a OS without WASAPI support, or the required DLL file might be missing from the driver's directory.\n\nTry reinstalling the driver.",
	L"Big buffer values aren't supported in exclusive mode.\n\nTry using a smaller size.",
};


void basserrconsole(int color, TCHAR * error, TCHAR * desc) {
	if (DebugMode) {
		// Set color
		SetConsoleTextAttribute(hConsole, color);

		// Get time
		char buff[20];
		struct tm *sTm;
		time_t now = time(0);
		sTm = gmtime(&now);
		strftime(buff, sizeof(buff), "%Y-%m-%d %H:%M:%S", sTm);

		// Get error
		char errorC[MAX_PATH];
		char descC[MAX_PATH];
		wcstombs(errorC, error, wcslen(error) + 1);
		wcstombs(descC, desc, wcslen(desc) + 1);
		std::cout << std::endl;
		std::cout << std::endl << buff << " - Keppy's Synthesizer encountered the following error: " << errorC;
		std::cout << std::endl << buff << " - Description: " << descC;
		std::cout << std::endl;
	}
}

void ShowError(int error, int mode, TCHAR* engine, TCHAR* codeline, BOOL showerror) {
	TCHAR main[33354];
	ZeroMemory(main, 33354);

	int e = error + 1;

	lstrcat(main, engine);
	lstrcat(main, L" encountered the following error: ");
	if (e >= 0 && e <= 48) {
		lstrcat(main, BASSErrorCode[e]);
		basserrconsole(FOREGROUND_RED, BASSErrorCode[e], BASSErrorCode[e]);
	}
	else if (e >= 5000 && e <= 5001) {
		lstrcat(main, BASSWASAPIErrorCode[e - 5000]);
		basserrconsole(FOREGROUND_RED, BASSWASAPIErrorCode[e - 5000], BASSErrorDesc[e - 5000]);
	}

	if (showerror) {
		TCHAR title[MAX_PATH];
		ZeroMemory(title, MAX_PATH);

		std::wstring ernumb = std::to_wstring(error);

		lstrcat(title, L"Keppy's Synthesizer - ");
		lstrcat(title, engine);
		lstrcat(title, L" execution error");

		lstrcat(main, L" (E");
		lstrcat(main, ernumb.c_str());
		lstrcat(main, L")");

		if (mode == 0) {
			lstrcat(main, L"\n\nCode line error: ");
			lstrcat(main, codeline);
		}

		lstrcat(main, L"\n\nExplanation: ");
		if (e >= 0 && e <= 48) {
			lstrcat(main, BASSErrorDesc[e]);
		}
		else if (e >= 5000 && e <= 5001) {
			lstrcat(main, BASSWASAPIErrorDesc[e - 5000]);
		}

		if (mode == 1) {
			lstrcat(main, L"\n\nWhat might have caused this error:\n");
			lstrcat(main, codeline);
		}
		else {
			lstrcat(main, L"\n\nPossible fixes:\n");
			if (e >= 0 && e <= 48)
				lstrcat(main, BASSErrorFix[e]);
			else if (e >= 5000 && e <= 5001)
				lstrcat(main, BASSWASAPIErrorFix[e - 5000]);
		}

		lstrcat(main, L"\n\nIf you're unsure about what this means, please take a screenshot, and give it to KaleidonKep99.");

		if (engine == L"ASIO") {
			lstrcat(main, L"\n\nChange the device through the configurator, then try again.\nTo change it, please open the configurator, and go to \"More settings > Advanced audio settings > Change default audio output\"");
		}

		MessageBox(NULL, main, title, MB_OK | MB_ICONERROR);
	}

	if (error == -1 ||
		error >= 2 && error <= 10 ||
		error == 19 ||
		error >= 24 && error <= 26 ||
		error == 44)
	{
		exit(error);
	}
}

void CrashMessage(LPCWSTR part) {
	std::wstring ErrorMessage;
	ErrorMessage.append(L"An error has been detected while trying to execute the following action: ");
	ErrorMessage.append(part);
	ErrorMessage.append(L"\nPlease take a screenshot of this messagebox (ALT+PRINT), and create a GitHub issue.\n\nClick OK to close the program.");

	SetConsoleTextAttribute(hConsole, FOREGROUND_RED);
	std::cout << "(Error at \"" << part << "\) - Fatal error during the execution of the driver." << std::endl;

	MessageBox(NULL, ErrorMessage.c_str() , L"Keppy's Synthesizer - Fatal execution error", MB_ICONERROR | MB_SYSTEMMODAL);
	exit(0);
	return;
}

BOOL CheckUp(int mode, TCHAR * codeline, bool showerror) {
	int error = BASS_ErrorGetCode();
	if (error != 0) {
		ShowError(error, mode, L"BASS", codeline, showerror);
		return FALSE;
	}
	return TRUE;
}

BOOL CheckUpASIO(int mode, TCHAR * codeline, bool showerror) {
	int error = BASS_ASIO_ErrorGetCode();
	if (error != 0) {
		ShowError(error, mode, L"BASSASIO", codeline, showerror);
		return FALSE;
	}
	return TRUE;
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