/*
OmniMIDI soundfont lists loading system
Don't bother understanding this stuff, as long as it works
*/

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

static void SoundFontError(LPCWSTR Cause, LPCWSTR Path) {
	TCHAR Message[NTFS_MAX_PATH];
	RtlZeroMemory(Message, sizeof(Message));
	wsprintf(Message, L"%s\n\nAffected SoundFont: %s\n\nSolution:\nThe soundfont might be of an unknown format, its functions might be unsupported by BASSMIDI, or it might not exist in memory.\nPlease check if the path to the soundfont is correct, and ultimately check if the functions (If using a SFZ soundfont) are supported by BASSMIDI.\nIf they're not, contact Ian Luck about the issue at Un4seen forums.\n\nThe soundfont will not be loaded. Press OK to continue the loading process.", Cause, Path);
	MessageBox(NULL, Message, L"OmniMIDI - SoundFont error", MB_OK | MB_ICONERROR);
}

static BOOL FontLoader(LPCWSTR in_path) {
	try {
		if (in_path == NULL && *in_path == NULL) return FALSE;

		LPCWSTR Extension = PathFindExtension(in_path);
		if (!_wcsicmp(Extension, _T(".sf2")) ||
			!_wcsicmp(Extension, _T(".sf2pack")) ||
			!_wcsicmp(Extension, _T(".sfz")))
		{
			SoundFontList TempItem;
			ZeroMemory(TempItem.Path, sizeof(TempItem.Path));
			wcsncpy(TempItem.Path, in_path, NTFS_MAX_PATH);

			if (PathFileExists(in_path))
			{
				TempItem.SourcePreset = -1;
				TempItem.SourceBank = -1;
				TempItem.DestinationPreset = -1;
				TempItem.DestinationBank = 0;

				PrintMessageToDebugLog("NewSFLoader", "Initializing SoundFont...");
				HSOUNDFONT SF = BASS_MIDI_FontInit(in_path, BASS_UNICODE);
				if (!SF) {
					PrintMessageToDebugLog("NewSFLoader", "An error has occurred while initializing the SoundFont.");
					SoundFontError(L"An error has occurred while initializing the SoundFont.", TempItem.Path);
					return FALSE;
				}

				PrintMessageToDebugLog("NewSFLoader", "Preparing BASS_MIDI_FONTEX...");
				BASS_MIDI_FONTEX SFConf = { SF, TempItem.SourcePreset, TempItem.SourceBank, TempItem.DestinationPreset, TempItem.DestinationBank, 0 };

				if (ManagedSettings.PreloadSoundFonts) {
					PrintMessageToDebugLog("NewSFLoader", "Preloading SoundFont...");
					if (!BASS_MIDI_FontLoad(SF, TempItem.SourcePreset, TempItem.SourceBank)) {
						PrintMessageToDebugLog("NewSFLoader", "An error has occurred while preloading the SoundFont.");
						SoundFontError(L"An error has occurred while preloading the SoundFont.", TempItem.Path);
						return FALSE;
					}
				}

				SoundFontHandles.push_back(SF);
				SoundFontPresets.push_back(SFConf);
				BASS_MIDI_StreamSetFonts(OMStream, &SoundFontPresets[0], (unsigned int)SoundFontPresets.size() | BASS_MIDI_FONT_EX);
				PrintMessageToDebugLog("NewSFLoader", "SoundFont(s) loaded into memory.");
				return TRUE;
			}
			else {
				SoundFontError(L"Unable to load SoundFont!\nThe file does not exist.", TempItem.Path);
			}
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
				wcsncpy(TempSF.Path, L"\0", NTFS_MAX_PATH);
				TempSF.EnableState = FALSE;
				TempSF.SourcePreset = -1;
				TempSF.SourceBank = -1;
				TempSF.DestinationPreset = -1;
				TempSF.DestinationBank = 0;
				TempSF.XGBankMode = FALSE;

				for (std::wstring TempLine; std::getline(SFList, TempLine);)
				{
					if (TempLine.find(L"sf.start") == 0)
					{
						if (AlreadyInitialized) continue;

						// It begins...
						AlreadyInitialized = TRUE;

						// Initialize TempSF struct
						TempSF = SoundFontList();
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
						if (!AlreadyInitialized) continue;

						// We've found the enable state! Crush it!
						AlreadyInitialized = FALSE;
						TempSoundFonts.push_back(TempSF);

						PrintMessageToDebugLog("NewSFLoader", "Ended loading SF. Searching for a new one...");
						continue;
					}
					else if (TempLine.find(L"sf.path") == 0 && TempSF.Path[0] == _T('\0'))
					{
						if (!AlreadyInitialized) continue;

						// We've found the path! Parse it.
						ZeroMemory(TempSF.Path, sizeof(TempSF.Path));
						wcsncpy(TempSF.Path, TempLine.substr(TempLine.find(L"= ") + 2).c_str(), NTFS_MAX_PATH);
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
				SFList.close();

				if (AlreadyInitialized) {
					AlreadyInitialized = FALSE;
					TempSoundFonts.push_back(TempSF);
				}
			}
			else return FALSE;

			// Free fonts, to prepare the arrays
			FreeFonts();

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
						SoundFontError(L"An error has occurred while initializing the SoundFont.", obj->Path);
						continue;
					}

					PrintMessageToDebugLog("NewSFLoader", "Preparing BASS_MIDI_FONTEX...");
					BASS_MIDI_FONTEX FEX = { font, obj->SourcePreset, obj->SourceBank, obj->DestinationPreset, obj->DestinationBank, 0 };

					if (ManagedSettings.PreloadSoundFonts) {
						PrintMessageToDebugLog("NewSFLoader", "Preloading SoundFont...");
						if (!BASS_MIDI_FontLoad(font, obj->SourcePreset, obj->SourceBank)) {
							PrintMessageToDebugLog("NewSFLoader", "An error has occurred while preloading the SoundFont.");
							SoundFontError(L"An error has occurred while preloading the SoundFont.", obj->Path);
							continue;
						}
					}

					PrintMessageToDebugLog("NewSFLoader", "Everything seems to be OK. Pushing it back inside the vector array...");
					SoundFontHandles.push_back(font);
					SoundFontPresets.push_back(FEX);
				}
				else {
					SoundFontError(L"Unable to load SoundFont!\nThe file does not exist.", obj->Path);
				}
			}

			std::reverse(SoundFontPresets.begin(), SoundFontPresets.end());
			BASS_MIDI_StreamSetFonts(OMStream, &SoundFontPresets[0], (unsigned int)SoundFontPresets.size() | BASS_MIDI_FONT_EX);
			PrintMessageToDebugLog("NewSFLoader", "SoundFont(s) loaded into memory.");

			return TRUE;
		}
		else if (!_wcsicmp(Extension, _T(".sflist"))) 
			MessageBox(NULL, L"SFLISTs are not supported by OmniMIDI anymore.\nSorry about that.", L"OmniMIDI - ERROR", MB_OK | MB_ICONERROR | MB_SYSTEMMODAL);
		else 
			MessageBox(NULL, L"Unrecognized SoundFont format/list system.\n\nCannot load the file.", L"OmniMIDI - ERROR", MB_OK | MB_ICONERROR | MB_SYSTEMMODAL);

		return FALSE;
	}
	catch (...) {
		CrashMessage("LoadFontItem");
	}
}