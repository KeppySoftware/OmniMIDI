/*
OmniMIDI soundfont lists loading system
Don't bother understanding this stuff, as long as it works
*/
#pragma once

static void FreeFonts()
{
	try {
		if (SoundFontHandles.size())
		{
			for (auto it = SoundFontHandles.begin(); it != SoundFontHandles.end(); ++it)
			{
				BASS_MIDI_FontFree(*it);
				PrintMessageToDebugLog("NewSFLoader", "Freed SoundFont...");
			}
			SoundFontHandles.resize(0);
			SoundFontPresets.resize(0);
			PrintMessageToDebugLog("NewSFLoader", "Vectors resized to zero.");
		}
	}
	catch (...) {
		CrashMessage("FreeFonts");
	}
}

LONGLONG FileSize(const wchar_t* name)
{
	WIN32_FILE_ATTRIBUTE_DATA fad;
	if (!GetFileAttributesEx(name, GetFileExInfoStandard, &fad))
		return -1; // error condition, could call GetLastError to find out more
	LARGE_INTEGER size;
	size.HighPart = fad.nFileSizeHigh;
	size.LowPart = fad.nFileSizeLow;
	return (LONGLONG)size.QuadPart;
}

static void SoundFontError(LPWSTR Cause, LPCWSTR Path) {
	TCHAR Message[NTFS_MAX_PATH];
	memset(Message, 0, sizeof(Message));

	DWORD err = BASS_ErrorGetCode();
	wsprintf(Message, L"%s\n\nBASS error code: %s\nError description: %s\n\nAffected SoundFont: %s\n\nSolution:\nThe soundfont might be of an unknown format, its functions might be unsupported by BASSMIDI, or it might not exist in memory.\nPlease check if the path to the soundfont is correct, and ultimately check if the functions (If using a SFZ soundfont) are supported by BASSMIDI.\nIf they're not, contact Ian Luck about the issue at Un4seen forums.\n\nThe soundfont will not be loaded. Press OK to continue the loading process.", 
		Cause, ReturnBASSError(err), ReturnBASSErrorDesc(err), Path);
	MessageBox(NULL, Message, L"OmniMIDI - SoundFont error", MB_OK | MB_ICONERROR);
}

static void SoundFontTooBig(LPWSTR Path) {
	TCHAR Message[NTFS_MAX_PATH];
	memset(Message, 0, sizeof(Message));

	wsprintf(Message, L"The SoundFont is too big for this 32-bit app to load!\n\nAffected SoundFont: %s\n\nSolution:\nSwitch to the 64-bit version of this app (if it exists), or switch to another MIDI app that offers a 64-bit release.\n\nThe SoundFont will not be preloaded. Press OK to continue the loading process.", Path);
	MessageBox(NULL, Message, L"OmniMIDI - SoundFont error", MB_OK | MB_ICONWARNING);
}

static BOOL FontLoader(LPWSTR in_path) {
	try {
		PrintMessageToDebugLog("NewSFLoader", "Preparing FontLoader...");

		if (!in_path && !*in_path) {
			PrintMessageToDebugLog("NewSFLoader", "Invalid path passed to function. No SF loaded.");
			return FALSE;
		}

		PrintMessageToDebugLog("NewSFLoader", "Getting extension of path passed to FontLoader...");
		LPWSTR Extension = PathFindExtensionW(in_path);
		PrintMessageToDebugLog("NewSFLoader", "Path has been parsed...");

		PrintMessageToDebugLog("NewSFLoader", "Checking if it's a SoundFont or a list...");
		if (!_wcsicmp(Extension, _T(".sf2")) ||
			!_wcsicmp(Extension, _T(".sf2pack")) ||
			!_wcsicmp(Extension, _T(".sfz")))
		{
			PrintMessageToDebugLog("NewSFLoader", "It's a SoundFont. Checking if it exists...");
			if (PathFileExists(in_path))
			{
				PrintMessageToDebugLog("NewSFLoader", "Initializing SoundFont...");
				HSOUNDFONT SF = BASS_MIDI_FontInit(in_path, BASS_UNICODE);
				if (!SF) {
					PrintMessageToDebugLog("NewSFLoader", "An error has occurred while initializing the SoundFont.");
					SoundFontError(L"An error has occurred while initializing the SoundFont.", in_path);
					return FALSE;
				}

				PrintMessageToDebugLog("NewSFLoader", "Preparing BASS_MIDI_FONTEX...");
				BASS_MIDI_FONTEX SFConf = { SF, -1, -1, -1, 0, 0 };

				if (ManagedSettings.PreloadSoundFonts) {
					PrintMessageToDebugLog("NewSFLoader", "Preloading SoundFont...");
					if (!BASS_MIDI_FontLoad(SF, -1, -1)) {
						PrintMessageToDebugLog("NewSFLoader", "An error has occurred while preloading the SoundFont.");
						SoundFontError(L"An error has occurred while preloading the SoundFont.", in_path);
						return FALSE;
					}
				}

				SoundFontHandles.push_back(SF);
				SoundFontPresets.push_back(SFConf);
				BASS_MIDI_StreamSetFonts(OMStream, &SoundFontPresets[0], (unsigned int)SoundFontPresets.size() | BASS_MIDI_FONT_EX);
				PrintMessageToDebugLog("NewSFLoader", "SoundFont(s) loaded into memory.");
				return TRUE;
			}
			else SoundFontError(L"Unable to load SoundFont!\nThe file does not exist.", in_path);
		}
		else if (!_wcsicmp(Extension, _T(".omlist")))
		{
			// Open file
			PrintMessageToDebugLog("NewSFLoader", "Opening SoundFont list...");
			wchar_t *end;
			std::vector<SoundFontList> *TempSoundFonts = new std::vector<SoundFontList>;
			std::wifstream SFList(in_path);

			if (SFList) {
				PrintMessageToDebugLog("NewSFLoader", "SoundFont list is valid. Setting UTF-8 encoding...");
				SFList.imbue(UTF8Support);

				PrintMessageToDebugLog("NewSFLoader", "Preparing values...");
				BOOL AlreadyInitialized = FALSE;

				SoundFontList TempSF;
				wcsncpy(TempSF.Path, L"\0", NTFS_MAX_PATH);
				TempSF.EnableState = FALSE;
				TempSF.SourcePreset = -1;
				TempSF.SourceBank = -1;
				TempSF.DestinationPreset = -1;
				TempSF.DestinationBank = 0;
				TempSF.XGBankMode = FALSE;

				PrintMessageToDebugLog("NewSFLoader", "Beginning per-line scan...");
				for (std::wstring TempLine; std::getline(SFList, TempLine);)
				{
					if (TempLine.find(L"sf.start") == 0)
					{
						if (AlreadyInitialized)
						{
							PrintMessageToDebugLog(
								"NewSFLoader", 
								"One of the SoundFont list items didn't end with sf.end, marked as invalid. We'll begin loading the next one."
							);
						}

						AlreadyInitialized = TRUE;

						// Initialize TempSF struct
						memset(TempSF.Path, 0, sizeof(TempSF.Path));
						wcsncpy(TempSF.Path, L"\0", NTFS_MAX_PATH);
						TempSF.EnableState = FALSE;
						TempSF.SourcePreset = -1;
						TempSF.SourceBank = -1;
						TempSF.DestinationPreset = -1;
						TempSF.DestinationBank = 0;
						TempSF.XGBankMode = FALSE;
						PrintMessageToDebugLog("NewSFLoader", "Begin loading SoundFont item...");

						continue;
					}
					else if (TempLine.find(L"sf.end") == 0)
					{
						if (AlreadyInitialized) {
							// We've found the enable state! Crush it!
							AlreadyInitialized = FALSE;
							TempSoundFonts->push_back(TempSF);

							PrintMessageToDebugLog("NewSFLoader", "Ended loading SF. Searching for a new one...");
						}
						continue;
					}
					else if (TempLine.find(L"sf.path") == 0 && TempSF.Path[0] == _T('\0'))
					{
						if (!AlreadyInitialized) continue;

						// We've found the path! Parse it.
						memset(TempSF.Path, 0, sizeof(TempSF.Path));
						wcsncpy(TempSF.Path, TempLine.substr(TempLine.find(L"= ") + 2).c_str(), NTFS_MAX_PATH);

						PrintSoundFontToDebugLog(TempSF.Path, "Loaded SF path to SoundFontList struct.");
						continue;
					}
					else if (TempLine.find(L"sf.enabled") == 0)
					{
						if (AlreadyInitialized)
						{
							// We've found the enable state! Crush it!
							TempSF.EnableState = wcstol(TempLine.substr(TempLine.find(L"= ") + 2).c_str(), &end, 0);

							PrintMessageToDebugLog("NewSFLoader", "Loaded SF's enabled state to SoundFontList struct.");
						}
						continue;
					}
					else if (TempLine.find(L"sf.srcp") == 0)
					{
						if (!AlreadyInitialized) 
						{
							// We've found the source preset! Take it!
							TempSF.SourcePreset = wcstol(TempLine.substr(TempLine.find(L"= ") + 2).c_str(), &end, 0);

							PrintMessageToDebugLog("NewSFLoader", "Loaded SF's source preset to SoundFontList struct.");
						}
						continue;
					}
					else if (TempLine.find(L"sf.srcb") == 0)
					{
						if (!AlreadyInitialized)
						{
							// We've found the source bank! Read it!
							TempSF.SourceBank = wcstol(TempLine.substr(TempLine.find(L"= ") + 2).c_str(), &end, 0);

							PrintMessageToDebugLog("NewSFLoader", "Loaded SF's source bank to SoundFontList struct.");
						}
						continue;
					}
					else if (TempLine.find(L"sf.desp") == 0) // IT'S NOT DESPACITO
					{
						if (!AlreadyInitialized) 
						{
							// We've found the destination preset! Munch it!
							TempSF.DestinationPreset = wcstol(TempLine.substr(TempLine.find(L"= ") + 2).c_str(), &end, 0);

							PrintMessageToDebugLog("NewSFLoader", "Loaded SF's destination preset to SoundFontList struct.");
						}
						continue;
					}
					else if (TempLine.find(L"sf.desb") == 0)
					{
						if (!AlreadyInitialized)
						{
							// We've found the destination bank! Look at it!
							TempSF.DestinationBank = wcstol(TempLine.substr(TempLine.find(L"= ") + 2).c_str(), &end, 0);

							PrintMessageToDebugLog("NewSFLoader", "Loaded SF's destination bank to SoundFontList struct.");
						}
						continue;
					}
					else if (TempLine.find(L"sf.xgdrums") == 0)
					{
						if (!AlreadyInitialized)
						{
							// We've found the destination bank! Look at it!
							TempSF.XGBankMode = wcstol(TempLine.substr(TempLine.find(L"= ") + 2).c_str(), &end, 0);

							PrintMessageToDebugLog("NewSFLoader", "Loaded SF's XG drum setting to SoundFontList struct.");
						}
						continue;
					}
					else continue;
				}
				SFList.close();
			}
			else return FALSE;

			// Free fonts, to prepare the arrays
			FreeFonts();

			for (auto CurrentSF = TempSoundFonts->begin(); CurrentSF != TempSoundFonts->end(); ++CurrentSF)
			{
				if (!CurrentSF->EnableState) {
					PrintSoundFontToDebugLog(CurrentSF->Path, "SoundFont disabled, skipping to next one...");
					continue;
				}

				if (PathFileExists(CurrentSF->Path))
				{
					PrintSoundFontToDebugLog(CurrentSF->Path, "Initializing SoundFont...");
					HSOUNDFONT font = BASS_MIDI_FontInit(CurrentSF->Path, (CurrentSF->XGBankMode ? BASS_MIDI_FONT_XGDRUMS : 0) | BASS_UNICODE);
					if (!font) {
						PrintSoundFontToDebugLog(CurrentSF->Path, "An error has occurred while initializing the SoundFont.");
						SoundFontError(L"An error has occurred while initializing the SoundFont.", CurrentSF->Path);
						continue;
					}

					PrintSoundFontToDebugLog(CurrentSF->Path, "Preparing BASS_MIDI_FONTEX...");
					BASS_MIDI_FONTEX FEX = { font, CurrentSF->SourcePreset, CurrentSF->SourceBank, CurrentSF->DestinationPreset, CurrentSF->DestinationBank, 0 };

					if (ManagedSettings.PreloadSoundFonts) {
						PrintSoundFontToDebugLog(CurrentSF->Path, "Preloading SoundFont...");
#if defined(_M_IX86)
						if (FileSize(CurrentSF->Path) <= 1073741824) {
#endif
							if (!BASS_MIDI_FontLoad(font, CurrentSF->SourcePreset, CurrentSF->SourceBank)) {
								PrintSoundFontToDebugLog(CurrentSF->Path, "An error has occurred while preloading the SoundFont.");
								SoundFontError(L"An error has occurred while preloading the SoundFont.", CurrentSF->Path);
								continue;
							}
#if defined(_M_IX86)
						}
						else {
							MessageBeep(MB_ICONEXCLAMATION);
							PrintSoundFontToDebugLog(CurrentSF->Path, "The SoundFont is too big, it will not be preloaded.");
						}
#endif
					}

					PrintSoundFontToDebugLog(CurrentSF->Path, "Everything seems to be OK. Pushing it back inside the vector array...");
					SoundFontHandles.push_back(font);
					SoundFontPresets.push_back(FEX);
					PrintSoundFontToDebugLog(CurrentSF->Path, "Done.");
				}
				else {
					PrintSoundFontToDebugLog(CurrentSF->Path, "Unable to load SoundFont! The file does not exist.");
					SoundFontError(L"Unable to load SoundFont!\nThe file does not exist.", CurrentSF->Path);
				}
			}

			PrintMessageToDebugLog("NewSFLoader", "Preparing vector array for BASS_MIDI_StreamSetFonts...");
			std::reverse(SoundFontPresets.begin(), SoundFontPresets.end());

			PrintMessageToDebugLog("NewSFLoader", "Loading SoundFont(s) through BASS_MIDI_StreamSetFonts...");
			BASS_MIDI_StreamSetFonts(OMStream, &SoundFontPresets[0], (DWORD)SoundFontPresets.size() | BASS_MIDI_FONT_EX);

			PrintMessageToDebugLog("NewSFLoader", "SoundFont(s) loaded into memory.");
			assert(TempSoundFonts != NULL);
			delete(TempSoundFonts);

			return TRUE;
		}

		return FALSE;
	}
	catch (...) {
		CrashMessage("LoadFontItem");
	}
}