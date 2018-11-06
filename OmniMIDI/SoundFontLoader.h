/*
OmniMIDI soundfont lists loading system
Don't bother understanding this stuff, as long as it works
*/

static void FreeFonts()
{
	try {
		if (_soundFonts.size())
		{
			for (auto it = _soundFonts.begin(); it != _soundFonts.end(); ++it)
			{
				BASS_MIDI_FontFree(*it);
				PrintMessageToDebugLog("NewSFLoader", "Freed SoundFont...");
			}
			_soundFonts.resize(0);
			presetList.resize(0);
			PrintMessageToDebugLog("NewSFLoader", "Vectors resized to zero.");
		}
	}
	catch (...) {
		CrashMessage("FreeFonts");
	}
}

static void checksferror(LPCWSTR name) {
	if (BASS_ErrorGetCode() != 0) {
		TCHAR errormessage[MAX_PATH] = L"Could not load soundfont \"";
		TCHAR clickokmsg[420] = L"\".\n\nThe soundfont might be of an unknown format, its functions might be unsupported by BASSMIDI, or it might not exist in memory.\nPlease check if the path to the soundfont is correct, and ultimately check if the functions (If using a SFZ soundfont) are supported by BASSMIDI.\nIf they're not, contact Ian Luck about the issue at Un4seen forums.\n\nThe soundfont will be disabled. Press OK to continue the loading process.";
		lstrcat(errormessage, name);
		lstrcat(errormessage, clickokmsg);
		MessageBox(NULL, errormessage, _T("OmniMIDI - Soundfont error"), MB_ICONERROR | MB_OK | MB_SYSTEMMODAL);
	}
}

static BOOL FontLoader(const TCHAR * in_path) {
	try {
		LPCWSTR Extension = PathFindExtension(in_path);
		if (!_wcsicmp(Extension, _T(".sf2")) ||
			!_wcsicmp(Extension, _T(".sf2pack")) ||
			!_wcsicmp(Extension, _T(".sfz")))
		{
			SoundFontList TempItem;
			memcpy(TempItem.Path, in_path, sizeof(in_path));
			TempItem.SourcePreset = -1;
			TempItem.SourceBank = -1;
			TempItem.DestinationPreset = -1;
			TempItem.DestinationBank = 0;
			TempItem.XGBankMode = FALSE;

			HSOUNDFONT SF = BASS_MIDI_FontInit(in_path, (TempItem.XGBankMode ? BASS_MIDI_FONT_XGDRUMS : 0) | BASS_UNICODE);
			if (!SF) return FALSE;

			BASS_MIDI_FONTEX SFConf = { SF, TempItem.SourcePreset, TempItem.SourceBank, TempItem.DestinationPreset, TempItem.DestinationBank, 0 };

			_soundFonts.push_back(SF);
			presetList.push_back(SFConf);
			BASS_MIDI_StreamSetFonts(OMStream, &presetList[0], (unsigned int)presetList.size() | BASS_MIDI_FONT_EX);
			return TRUE;
		}
		else if (!_wcsicmp(Extension, _T(".omlist")))
		{
			// Open file
			wchar_t *end;
			std::vector<SoundFontList> TempSoundFonts;
			std::wifstream SFList(in_path);

			if (SFList) {
				BOOL AlreadyInitialized = FALSE;

				SoundFontList TempSF;
				for (std::wstring TempLine; std::getline(SFList, TempLine);)
				{
					if (TempLine.find(L"sf.start") == 0)
					{
						if (AlreadyInitialized) continue;

						// It begins...
						AlreadyInitialized = TRUE;

						// Initialize TempSF struct
						TempSF = SoundFontList();
						ZeroMemory(TempSF.Path, NTFS_MAX_PATH);
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
						if (!AlreadyInitialized) continue;

						// We've found the enable state! Crush it!
						AlreadyInitialized = FALSE;
						TempSoundFonts.push_back(TempSF);

						PrintMessageToDebugLog("NewSFLoader", "Ended loading SF. Searching for a new one...");
						continue;
					}
					else if (TempLine.find(L"sf.path") == 0)
					{
						if (!AlreadyInitialized) continue;

						// We've found the path! Parse it.
						const wchar_t* ThePath = TempLine.substr(TempLine.find(L"= ") + 2).c_str();
						wcsncpy(TempSF.Path, ThePath, NTFS_MAX_PATH);
						PrintMessageToDebugLog("NewSFLoader", "Loaded SF path to SoundFontList struct.");
						continue;
					}
					else if (TempLine.find(L"sf.enabled") == 0)
					{
						if (!AlreadyInitialized) continue;

						// We've found the enable state! Crush it!
						TempSF.EnableState = wcstol(TempLine.substr(TempLine.find(L"= ") + 2).c_str(), &end, 0);
						PrintMessageToDebugLog("NewSFLoader", "Loaded SF's enabled state to SoundFontList struct.");
						continue;
					}
					else if (TempLine.find(L"sf.srcp") == 0)
					{
						if (!AlreadyInitialized) continue;

						// We've found the source preset! Take it!
						TempSF.SourcePreset = wcstol(TempLine.substr(TempLine.find(L"= ") + 2).c_str(), &end, 0);
						PrintMessageToDebugLog("NewSFLoader", "Loaded SF's source preset to SoundFontList struct.");
						continue;
					}
					else if (TempLine.find(L"sf.srcb") == 0)
					{
						if (!AlreadyInitialized) continue;

						// We've found the source bank! Read it!
						TempSF.SourceBank = wcstol(TempLine.substr(TempLine.find(L"= ") + 2).c_str(), &end, 0);
						PrintMessageToDebugLog("NewSFLoader", "Loaded SF's source bank to SoundFontList struct.");
						continue;
					}
					else if (TempLine.find(L"sf.desp") == 0)
					{
						if (!AlreadyInitialized) continue;

						// We've found the destination preset! Munch it!
						TempSF.DestinationPreset = wcstol(TempLine.substr(TempLine.find(L"= ") + 2).c_str(), &end, 0);
						PrintMessageToDebugLog("NewSFLoader", "Loaded SF's destination preset to SoundFontList struct.");
						continue;
					}
					else if (TempLine.find(L"sf.desb") == 0)
					{
						if (!AlreadyInitialized) continue;

						// We've found the destination bank! Look at it!
						TempSF.DestinationBank = wcstol(TempLine.substr(TempLine.find(L"= ") + 2).c_str(), &end, 0);
						PrintMessageToDebugLog("NewSFLoader", "Loaded SF's destination bank to SoundFontList struct.");
						continue;
					}
					else if (TempLine.find(L"sf.xgdrums") == 0)
					{
						if (!AlreadyInitialized) continue;

						// We've found the destination bank! Look at it!
						TempSF.XGBankMode = wcstol(TempLine.substr(TempLine.find(L"= ") + 2).c_str(), &end, 0);
						PrintMessageToDebugLog("NewSFLoader", "Loaded SF's XG drum setting to SoundFontList struct.");
						continue;
					}
					else if (TempLine.find(L"//") == 0 || TempLine.find(L"#") || TempLine.empty()) {
						// Comment or emptiness, go on...
						continue;
					}
					else continue;
				}
				if (AlreadyInitialized) {
					MessageBox(NULL, 
						L"An error has occurred while loading the SoundFont list. It might be invalid or corrupted.\nThe SoundFonts will not be loaded to memory\nPress OK to continue.", 
						L"OmniMIDI - ERROR", MB_OK | MB_ICONERROR | MB_SYSTEMMODAL);
					return FALSE;
				}
				SFList.close();
			}

			for (auto obj = TempSoundFonts.begin(); obj != TempSoundFonts.end(); ++obj)
			{
				if (!obj->EnableState) {
					PrintMessageToDebugLog("NewSFLoader", "SoundFont disabled, skipping to next one...");
					continue;
				}

				if (PathFileExists(obj->Path))
				{
					PrintMessageToDebugLog("NewSFLoader", "Initializing SoundFont...");
					HSOUNDFONT font = BASS_MIDI_FontInit(obj->Path, (obj->XGBankMode ? BASS_MIDI_FONT_XGDRUMS : 0) | BASS_UNICODE);
					if (!font) {
						PrintMessageToDebugLog("NewSFLoader", "An error has occurred while initializing the SoundFont.");
						CheckUp(ERRORCODE, L"BASSMIDI Font Initialization", TRUE);
					}

					PrintMessageToDebugLog("NewSFLoader", "Preparing BASS_MIDI_FONTEX...");
					BASS_MIDI_FONTEX FEX = { font, obj->SourcePreset, obj->SourceBank, obj->DestinationPreset, obj->DestinationBank, 0 };

					PrintMessageToDebugLog("NewSFLoader", "Pushing it back inside the vector array...");
					_soundFonts.push_back(font);
					presetList.push_back(FEX);

					if (ManagedSettings.PreloadSoundFonts) {
						PrintMessageToDebugLog("NewSFLoader", "Preloading SoundFont...");
						BASS_MIDI_FontLoad(font, obj->SourcePreset, obj->SourceBank);
					}
				}
				else {
					TCHAR Message[MAX_PATH];
					ZeroMemory(Message, MAX_PATH);
					wsprintf(Message, L"The following SoundFont does not exist.\n\nAffected SoundFont: %s\n\nSolution:\nCheck if the SoundFont actually exists in its folder, and if it hasn't accidentally been renamed, moved or deleted.\n\nThe SoundFont will be skipped from the loading process.", obj->Path);
					MessageBox(NULL, Message, L"OmniMIDI - SoundFont error", MB_OK | MB_ICONERROR);
				}
			}
			return TRUE;
		}
		else if (!_wcsicmp(Extension, _T(".sflist"))) {
			MessageBox(NULL, L"SFLISTs are not supported by OmniMIDI anymore.\nSorry about that.", L"OmniMIDI - ERROR", MB_OK | MB_ICONERROR | MB_SYSTEMMODAL);
		}

		return FALSE;
	}
	catch (...) {
		CrashMessage("LoadFontItem");
	}
}

void LoadFonts(const TCHAR * name)
{
	try {
		if (name != NULL && *name != NULL)
		{
			LPCWSTR Extension = PathFindExtension(name);
			if (!_wcsicmp(Extension, _T(".sf2")) || 
				!_wcsicmp(Extension, _T(".sf2pack")) || 
				!_wcsicmp(Extension, _T(".sfz")) ||
				!_wcsicmp(Extension, _T(".omlist")))
			{
				FreeFonts();

				if (!FontLoader(name))
				{
					FreeFonts();
					return;
				}

				std::vector< BASS_MIDI_FONTEX > fonts;
				for (unsigned long i = 0, j = presetList.size(); i < j; ++i)
				{
					fonts.push_back(presetList[j - i - 1]);
				}
				BASS_MIDI_StreamSetFonts(OMStream, &fonts[0], (unsigned int)fonts.size() | BASS_MIDI_FONT_EX);
			}
		}
	}
	catch (...) {
		CrashMessage("LoadFontToMemory");
	}
}