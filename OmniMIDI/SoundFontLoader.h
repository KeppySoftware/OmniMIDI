/*
OmniMIDI soundfont lists loading system
Don't bother understanding this stuff, as long as it works
*/

static void FreeFonts()
{
	try {
		unsigned i;
		if (_soundFonts.size())
		{
			for (auto it = _soundFonts.begin(); it != _soundFonts.end(); ++it)
			{
				BASS_MIDI_FontFree(*it);
			}
			_soundFonts.resize(0);
			presetList.resize(0);
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

static BOOL LegacyFontLoader(const TCHAR * in_path)
{
	try {
		BOOL XGDrumsMode = FALSE;
		const TCHAR * ext = _T("");
		const TCHAR * dot = _tcsrchr(in_path, _T('.'));
		if (dot != 0) ext = dot + 1;
		if (!_wcsicmp(ext, _T("sflist")))
		{
			FILE * fl = _tfopen(in_path, _T("r, ccs=UTF-8"));
			if (fl)
			{
				TCHAR path[32768], temp[32768];
				TCHAR name[32768];
				TCHAR *nameptr;
				const TCHAR * slash = _tcsrchr(in_path, _T('\\'));
				if (slash != 0) _tcsncpy(path, in_path, slash - in_path + 1);
				while (!feof(fl))
				{
					std::vector<BASS_MIDI_FONTEX> presets;

					if (!_fgetts(name, 32767, fl)) break;
					name[32767] = 0;
					TCHAR * cr = _tcschr(name, _T('\n'));
					if (cr) *cr = 0;
					cr = _tcschr(name, _T('\r'));
					if (cr) *cr = 0;
					cr = _tcschr(name, '|');
					if (cr)
					{
						bool valid = true;
						TCHAR *endchr;
						nameptr = cr + 1;
						*cr = 0;
						cr = name;
						while (*cr && valid)
						{
							switch (*cr++)
							{
							case 'p':
							{
								// patch override - "p[db#,]dp#=[sb#,]sp#" ex. "p0,5=0,1"
								// may be used once per preset group
								long dbank = 0;
								long dpreset = _tcstol(cr, &endchr, 10);
								if (endchr == cr)
								{
									valid = false;
									break;
								}
								if (*endchr == ',')
								{
									dbank = dpreset;
									cr = endchr + 1;
									dpreset = _tcstol(cr, &endchr, 10);
									if (endchr == cr)
									{
										valid = false;
										break;
									}
								}
								if (*endchr != '=')
								{
									valid = false;
									break;
								}
								cr = endchr + 1;
								long sbank = -1;
								long spreset = _tcstol(cr, &endchr, 10);
								if (endchr == cr)
								{
									valid = false;
									break;
								}
								if (*endchr == ',')
								{
									sbank = spreset;
									cr = endchr + 1;
									spreset = _tcstol(cr, &endchr, 10);
									if (endchr == cr)
									{
										valid = false;
										break;
									}
								}
								if (*endchr && *endchr != ';' && *endchr != '&')
								{
									valid = false;
									break;
								}
								cr = endchr;
								BASS_MIDI_FONTEX fex = { 0, (int)spreset, (int)sbank, (int)dpreset, (int)dbank, 0 };
								presets.push_back(fex);
							}
							break;

							case ';':
								// separates preset items
								break;

							default:
								// invalid command character
								valid = false;
								break;
							}
						}
						if (!valid)
						{
							presets.clear();
							BASS_MIDI_FONTEX fex = { 0, -1, -1, -1, 0, 0 };
							presets.push_back(fex);
						}
					}
					else
					{
						BASS_MIDI_FONTEX fex = { 0, -1, -1, -1, 0, 0 };
						presets.push_back(fex);
						nameptr = name;
					}
					if ((isalpha(nameptr[0]) && nameptr[1] == _T(':')) || nameptr[0] == '\\')
					{
						_tcscpy(temp, nameptr);
					}
					else
					{
						_tcscpy(temp, path);
						_tcscat(temp, nameptr);
					}
					if (name[0] != '@') {
						if (PathFileExists(nameptr)) {
							int sbank;
							int spreset;
							HSOUNDFONT font = BASS_MIDI_FontInit(temp, BASS_UNICODE);
							if (!font)
							{
								fclose(fl);
								return FALSE;
							}
							for (auto it = presets.begin(); it != presets.end(); ++it)
							{
								sbank = it->sbank;
								spreset = it->spreset;

								if (!font)
								{
									fclose(fl);
									return FALSE;
								}

								if (ManagedSettings.PreloadSoundFonts)
									BASS_MIDI_FontLoad(font, it->spreset, it->sbank);

								std::wstring appdatapath = L"This error might have been caused by a missing instrument/preset in the SoundFont.\nPlease check if the instrument/preset you selected exists inside the SoundFont, then try again.\n\nAffected SoundFont: ";
								appdatapath += nameptr;
								appdatapath += L"\nAffected bank and preset: Bank " + std::to_wstring(sbank) + L", Preset " + std::to_wstring(spreset);
								CheckUp(CAUSE, (wchar_t *)appdatapath.c_str(), TRUE);

								it->font = font;
								presetList.push_back(*it);
							}
							_soundFonts.push_back(font);
						}
						else {
							std::wstring appdatapath = L"The following SoundFont does not exist.\n\nAffected SoundFont: ";
							appdatapath += nameptr;
							appdatapath += L"\n\nSolution:\nCheck if the SoundFont actually exists in its folder, and if it hasn't accidentally been renamed, moved or deleted.\n\nThe SoundFont will be skipped from the loading process.";
							MessageBox(NULL, appdatapath.c_str(), L"OmniMIDI - SoundFont error", MB_OK | MB_ICONERROR);
						}
					}
				}
				fclose(fl);
				return TRUE;
			}
		}
		return FALSE;
	}
	catch (...) {
		CrashMessage("LoadFontItem");
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
				SoundFontList TempSF {
					TRUE,		// Enable state
					NULL,		// SF path
					-1,			// Source preset
					-1,			// Source bank
					-1,			// Destination preset
					0,			// Destination bank
					FALSE		// XG drumset mode
				};

				for (std::wstring TempLine; std::getline(SFList, TempLine);)
				{
					if (TempLine.find(L"sf.start") == 0) 
					{
						if (AlreadyInitialized) continue;

						// It begins...
						AlreadyInitialized = TRUE;
						PrintMessageToDebugLog("NewSFLoader", "Begin loading SoundFont item...");
						continue;
					}
					else if (TempLine.find(L"sf.end") == 0) 
					{
						if (!AlreadyInitialized) continue;

						// We've found the enable state! Crush it!
						AlreadyInitialized = FALSE;
						TempSoundFonts.push_back(TempSF);
						TempSF = SoundFontList();
						PrintMessageToDebugLog("NewSFLoader", "Ended loading SF. Searching for a new one...");
						continue;
					}
					else if (TempLine.find(L"sf.path") == 0)
					{
						if (!AlreadyInitialized) continue;

						// We've found the path! Parse it.
						ZeroMemory(TempSF.Path, MAX_PATH);
						memcpy(TempSF.Path, TempLine.substr(TempLine.find(L"= ") + 2).c_str(), MAX_PATH);
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
					else {
						// SoundFont is invalid, let's try parsing it with the old font loader
						SFList.close();
						return LegacyFontLoader(in_path);
					}
				}
				SFList.close();
			}

			for (int i = 0; i < TempSoundFonts.size(); i++)
			{
				if (!TempSoundFonts[i].EnableState) {
					PrintMessageToDebugLog("NewSFLoader", "SoundFont disabled, skipping to next one...");
					continue;
				}

				if (PathFileExists(TempSoundFonts[i].Path))
				{
					PrintMessageToDebugLog("NewSFLoader", "Initializing SoundFont...");
					HSOUNDFONT font = BASS_MIDI_FontInit(TempSoundFonts[i].Path, (TempSoundFonts[i].XGBankMode ? BASS_MIDI_FONT_XGDRUMS : 0) | BASS_UNICODE);
					if (!font) {
						PrintMessageToDebugLog("NewSFLoader", "An error has occurred while initializing the SoundFont.");
						CheckUp(ERRORCODE, L"BASSMIDI Font Initialization", TRUE);
					}

					PrintMessageToDebugLog("NewSFLoader", "Preparing BASS_MIDI_FONTEX...");
					BASS_MIDI_FONTEX FEX = { font, TempSoundFonts[i].SourcePreset, TempSoundFonts[i].SourceBank, TempSoundFonts[i].DestinationPreset, TempSoundFonts[i].DestinationBank, 0 };

					PrintMessageToDebugLog("NewSFLoader", "Pushing it back inside the vector array...");
					_soundFonts.push_back(font);
					presetList.push_back(FEX);

					if (ManagedSettings.PreloadSoundFonts) {
						PrintMessageToDebugLog("NewSFLoader", "Preloading SoundFont...");
						BASS_MIDI_FontLoad(font, TempSoundFonts[i].SourcePreset, TempSoundFonts[i].SourceBank);
					}
				}
				else {
					TCHAR Message[MAX_PATH];
					ZeroMemory(Message, MAX_PATH);
					wsprintf(Message, L"The following SoundFont does not exist.\n\nAffected SoundFont: %s\n\nSolution:\nCheck if the SoundFont actually exists in its folder, and if it hasn't accidentally been renamed, moved or deleted.\n\nThe SoundFont will be skipped from the loading process.", TempSoundFonts[i].Path);
					MessageBox(NULL, Message, L"OmniMIDI - SoundFont error", MB_OK | MB_ICONERROR);
				}
			}
			return TRUE;
		}
		else if (!_wcsicmp(Extension, _T(".sflist"))) return LegacyFontLoader(in_path);

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
				!_wcsicmp(Extension, _T(".sflist")) || 
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