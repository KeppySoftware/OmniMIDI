/*
OmniMIDI errors list
*/
#pragma once

#define Case(Error) case Error: return L#Error;

LPCWSTR ReturnBASSError(INT ErrorCode) {
	switch (ErrorCode) {
	Case(BASS_ERROR_ALREADY)
	Case(BASS_ERROR_BUFLOST)
	Case(BASS_ERROR_BUSY)
	Case(BASS_ERROR_CODEC)
	Case(BASS_ERROR_CREATE)
	Case(BASS_ERROR_DECODE)
	Case(BASS_ERROR_DEVICE)
	Case(BASS_ERROR_DRIVER)
	Case(BASS_ERROR_DX)
	Case(BASS_ERROR_EMPTY)
	Case(BASS_ERROR_ENDED)
	Case(BASS_ERROR_FILEFORM)
	Case(BASS_ERROR_FILEOPEN)
	Case(BASS_ERROR_FORMAT)
	Case(BASS_ERROR_FREQ)
	Case(BASS_ERROR_HANDLE)
	Case(BASS_ERROR_ILLPARAM)
	Case(BASS_ERROR_ILLTYPE)
	Case(BASS_ERROR_INIT)
	Case(BASS_ERROR_MEM)
	Case(BASS_ERROR_MIDI_INCLUDE)
	Case(BASS_ERROR_NO3D)
	Case(BASS_ERROR_NOCHAN)
	Case(BASS_ERROR_NOEAX)
	Case(BASS_ERROR_NOFX)
	Case(BASS_ERROR_NOHW)
	Case(BASS_ERROR_NONET)
	Case(BASS_ERROR_NOPLAY)
	Case(BASS_ERROR_NOTAVAIL)
	Case(BASS_ERROR_NOTFILE)
	Case(BASS_ERROR_PLAYING)
	Case(BASS_ERROR_POSITION)
	Case(BASS_ERROR_SPEAKER)
	Case(BASS_ERROR_SSL)
	Case(BASS_ERROR_START)
	Case(BASS_ERROR_TIMEOUT)
	Case(BASS_ERROR_UNKNOWN)
	Case(BASS_ERROR_VERSION)
	Case(BASS_ERROR_WASAPI)
	Case(BASS_ERROR_WASAPI_BUFFER)
	Case(BASS_ERROR_WASAPI_RAW)
	Case(BASS_ERROR_WASAPI_DENIED)
	Case(BASS_OK)
	default: return L"Unknown error.";
	}
}

LPCWSTR ReturnBASSErrorDesc(INT ErrorCode) {
	switch (ErrorCode) {
	case -1: return L"Unknown error.";
	case 0: return L"No error detected.";
	case 1: return L"The app is out of memory.";
	case 2: return L"The file could not be opened.";
	case 3: return L"There is no available device driver. The device may already be in use.";
	case 4: return L"The sample buffer was lost.";
	case 5: return L"An invalid handle has been used.";
	case 6: return L"The sample format is not supported by the output device.";
	case 7: return L"The requested position is invalid, eg. it is beyond the end or the download has not yet reached it.";
	case 8: return L"BASS_Init has not been called yet.";
	case 9: return L"BASS_Start has not been called yet.";
	case 10: return L"SSL/HTTPS support isn't available. Are you using Windows 95?";
	case 14: return L"Stream already initialized, or BASS_Init already called.";
	case 18: return L"No free channels are available.";
	case 19: return L"An illegal definition was specified.";
	case 20: return L"An illegal parameter was specified.";
	case 21: return L"The selected DirectSound output doesn't support DirectSound3D.";
	case 22: return L"The selected DirectSound output doesn't support hardware EAX.";
	case 23: return L"Invalid device ID.";
	case 24: return L"The stream is not playing.";
	case 25: return L"Invalid audio frequency selected.";
	case 27: return L"The stream is not a file stream.";
	case 29: return L"No hardware voices available.";
	case 31: return L"The MOD sequence has no data.";
	case 32: return L"No Internet connection could be made.";
	case 33: return L"Couldn't create the file.";
	case 34: return L"Effects are not available with the selected device.";
	case 35: return L"The stream is already playing.";
	case 37: return L"The requested data is not available yet.";
	case 38: return L"The stream is a \"decoding stream\"";
	case 39: return L"DirectX8 is not installed.";
	case 40: return L"Connection timed out.";
	case 41: return L"Unsupported file format.";
	case 42: return L"Speakers configuration unavailable.";
	case 43: return L"BASS version mismatch.";
	case 44: return L"Codec is not available or supported.";
	case 45: return L"The stream has ended.";
	case 46: return L"The device is busy. (eg. in 'exclusive' use by another process)";
	case 5000: return L"WASAPI is not available in this system.";
	case 5001: return L"WASAPI buffer size is not valid.";
	case 5002: return L"RAW mode is unsupported on this output device.";
	case 5003: return L"Couldn't open the WASAPI device, access denied.";
	default: return L"No description available.";
	}
}

LPCWSTR ReturnBASSErrorFix(INT ErrorCode) {
	switch (ErrorCode) {
	case -1:
		return L"The cause of the error is unknown, no description is available.";
	case 0: 
		return L"Nothing wrong happened. You shouldn't be able to see this error.";
	case 1: 
		return L"There's not enough available memory for the driver.\nIt might be caused by a really big SoundFont, or by the app itself.\n\nTry using a smaller SoundFont, or switch to the 64-bit version of the app, if available.";
	case 2: 
		return L"Ensure the file you selected actually exists, and if the drive hosting the file is online.";
	case 3: 
		return L"Another app might've took exclusive use of the selected audio device. Try closing all the other audio applications, then try again. Ensure you're not running another exclusive-mode instance of OmniMIDI.";
	case 4: 
		return L"The sound card might've timed out, or the buffer size might not be big enough for it to handle. Try increasing the buffer size, or switch to another audio device.";
	case 6: case 25:
		return L"You're using an audio frequency that isn't supported by the device. If it still works after pressing OK, then ignore this message, otherwise change the frequency in the configurator.";	
	case 18:
		return L"BASS or BASSMIDI are unable to allocate a stream channel. If you're using VirtualMIDISynth 1.x, please uninstall it, otherwise restart the application.";
	case 20:
		return L"The ASIO/WASAPI device might be incompatible with a certain function you enabled in the settings. Try disabling that specific function, otherwise switch to another device";
	case 23:
		return L"The device you selected doesn't exist. Check the selected device in the configurator.";
	case 24: case 35:
		return L"The driver encountered an error, and called the same function twice. Restart the application.";
	case 33:
		return L"You might not have the required permissions to write the file, or BASS might have encountered an error while creating it.";
	case 37:
		return L"The audio data wasn't ready yet to be picked up. This can be caused by a timeout in the buffer system, or by a dead audio stream. Try restarting the application.";
	case 42:
		return L"BASS is unable to use the selected output. Make sure nothing is having exclusive control over it.";
	case 46:
		return L"Another app might've took exclusive use of the selected audio device. Try closing all the other audio applications, then try again. Ensure you're not running another exclusive-mode instance of OmniMIDI.";
	case 5001:
		return L"The buffer size you specified is either not valid or too small for the device to work. Try specifying another value.";
	case 5002:
		return L"This error could happen if the device driver is meant for Windows 8.0 and older, or if the APO filters don't support RAW mode. Try downgrading the audio driver to the stock Microsoft High Definition Audio driver.";
	case 7000:
		return L"The SoundFont preset might be corrupted or invalid. Contact the original developer of the SoundFont, or use another one instead.";
	case 5: case 8: case 11: case 12: case 13: case 15: case 16: case 17: case 19: case 38: case 43: case 5000: case 5003:
		return L"This is a serious error, please restart the application.\nIf it happens again, contact KaleidonKep99.";
	default: 
		return L"The cause of the error is unknown, no description is available.";
	}
}

void ShowError(int error, int mode, CHAR* engine, CHAR* codeline, int showerror) {
	CHAR title[NTFS_MAX_PATH] = { 0 };
	CHAR main[NTFS_MAX_PATH] = { 0 };

	WCHAR WBE[512] = { 0 };
	WCHAR WBED[512] = { 0 };

	CHAR BE[512] = { 0 };
	CHAR BED[512] = { 0 };
	CHAR BEF[512] = { 0 };

	swprintf_s(WBE, ReturnBASSError(error));
	swprintf_s(WBED, ReturnBASSErrorDesc(error));

	wcstombs(BE, ReturnBASSError(error), 512);
	wcstombs(BED, ReturnBASSErrorDesc(error), 512);
	wcstombs(BEF, ReturnBASSErrorFix(error), 512);

	sprintf_s(main, sizeof(main), "%s encountered the following error: %s", engine, BE);
	PrintBASSErrorMessageToDebugLog(WBE, WBED);

	if (showerror) {
		sprintf_s(title, sizeof(title), "OmniMIDI - %s execution error", engine);
		sprintf_s(main + strlen(main), sizeof(main), " (E%d)", error);

		if (mode == 0) 
			sprintf_s(main + strlen(main), sizeof(main), "\n\nCode line error: %s", codeline);

		sprintf_s(main + strlen(main), sizeof(main), "\n\nExplanation: %s", BED);

		sprintf_s(main + strlen(main), sizeof(main),
			mode ? "\n\nWhat might have caused this error:\n%s" : "\n\nPossible fixes:\n%s", 
			mode ? codeline : BEF
		);

		if (!_stricmp(engine, "BASSASIO") && error != -1) {
			strcat_s(main, sizeof(main), "\n\nChange the device through the configurator, then try again.\nTo change it, please open the configurator, and go to \"More settings > Advanced audio settings > Change default audio output\"");
		}

		strcat_s(main, sizeof(main), "\n\nIf you're unsure about what this means, please take a screenshot, and give it to KaleidonKep99.");

		MessageBoxA(NULL, main, title, MB_OK | MB_ICONERROR);

		if ((error == -1 ||
			error >= 2 && error <= 10 ||
			error == 19 ||
			error >= 24 && error <= 26 ||
			error == 44) && showerror)
		{
			exit(ERROR_INVALID_FUNCTION);
		}
	}
}

BOOL CheckUp(BOOL IsASIO, int mode, CHAR* codeline, bool showerror) {
	int error = IsASIO ? BASS_ASIO_ErrorGetCode() : BASS_ErrorGetCode();
	if (error != 0) {
		ShowError(error, mode, IsASIO ? "BASSASIO" : "BASS", codeline, showerror);
		return FALSE;
	}
	return TRUE;
}