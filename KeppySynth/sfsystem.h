/*
Keppy's Synthesizer soundfont lists loading system
*/

char * tocharpls(const TCHAR * widechar)
{
	int size = 0;
	while ((const char *)widechar[size] != ""){
		size++;
	}
	size++;
	char * charpointer = new char[size];
	wcstombs(charpointer, widechar, size);
	return charpointer;
}

static void FreeFonts(UINT uDeviceID)
{
	unsigned i;
	if (_soundFonts[uDeviceID].size())
	{
		for (auto it = _soundFonts[uDeviceID].begin(); it != _soundFonts[uDeviceID].end(); ++it)
		{
			BASS_MIDI_FontFree(*it);
		}
		_soundFonts[uDeviceID].resize(0);
		presetList[uDeviceID].resize(0);
	}
}

static void checksferror(LPCWSTR name) {
	if (BASS_ErrorGetCode() != 0) {
		TCHAR errormessage[MAX_PATH] = L"Could not load soundfont \"";
		TCHAR clickokmsg[420] = L"\".\n\nThe soundfont might be of an unknown format, its functions might be unsupported by BASSMIDI, or it might not exist in memory.\nPlease check if the path to the soundfont is correct, and ultimately check if the functions (If using a SFZ soundfont) are supported by BASSMIDI.\nIf they're not, contact Ian Luck about the issue at Un4seen forums.\n\nThe soundfont will be disabled. Press OK to continue the loading process.";
		lstrcat(errormessage, name);
		lstrcat(errormessage, clickokmsg);
		MessageBox(NULL, errormessage, _T("Keppy's Synthesizer - Soundfont error"), MB_ICONERROR | MB_OK | MB_SYSTEMMODAL);
	}
}

static bool load_font_item(unsigned uDeviceID, const TCHAR * in_path)
{
	const DWORD bass_flags =
#ifdef UNICODE
		BASS_UNICODE
#else
		0
#endif
		;
	const TCHAR * ext = _T("");
	const TCHAR * dot = _tcsrchr(in_path, _T('.'));
	if (dot != 0) ext = dot + 1;
	if (!_tcsicmp(ext, _T("sf2"))
		|| !_tcsicmp(ext, _T("sf2pack"))
		|| !_tcsicmp(ext, _T("sfz"))
		)
	{
		HSOUNDFONT font = BASS_MIDI_FontInit(in_path, bass_flags);
		if (!font)
		{
			return false;
		}
		_soundFonts[uDeviceID].push_back(font);
		BASS_MIDI_FONTEX fex = { font, -1, -1, -1, 0, 0 };
		presetList[uDeviceID].push_back(fex);
		return true;
	}
	else if (!_tcsicmp(ext, _T("sflist")))
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
							checksferror(nameptr);
						}
						break;

						case '&':
						{
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
						checksferror(nameptr);
					}
				}
				else
				{
					BASS_MIDI_FONTEX fex = { 0, -1, -1, -1, 0, 0 };
					presets.push_back(fex);
					nameptr = name;
					checksferror(nameptr);
				}
				if ((isalpha(nameptr[0]) && nameptr[1] == _T(':')) || nameptr[0] == '\\')
				{
					_tcscpy(temp, nameptr);
				}
				else
				{
					_tcscat(temp, nameptr);
				}
				if (name[0] != '@') {
					int sbank;
					int spreset;
					HSOUNDFONT font = BASS_MIDI_FontInit(temp, bass_flags);
					for (auto it = presets.begin(); it != presets.end(); ++it)
					{
						sbank = it->sbank;
						spreset = it->spreset;
						if (preload)
							BASS_MIDI_FontLoad(font, it->spreset, it->sbank);
						it->font = font;
						presetList[uDeviceID].push_back(*it);
					}
					_soundFonts[uDeviceID].push_back(font);
					std::wstring appdatapath = L"This error might have been caused by a missing instrument/preset in the SoundFont.\nPlease check if the instrument/preset you selected exists inside the SoundFont, then try again.\n\nAffected SoundFont: ";
					appdatapath += nameptr;
					appdatapath += L"\nAffected bank and preset: Bank " + std::to_wstring(sbank) + L", Preset " + std::to_wstring(spreset);
					CheckUp(CAUSE, (wchar_t *)appdatapath.c_str());
				}
				else {
					continue;
				}
			}
			fclose(fl);
			return true;
		}
	}
	return false;
}

void LoadFonts(UINT uDeviceID, const TCHAR * name)
{
	FreeFonts(uDeviceID);

	if (name && *name)
	{
		const TCHAR * ext = _tcsrchr(name, _T('.'));
		if (ext) ext++;
		if (!_tcsicmp(ext, _T("sf2")) || !_tcsicmp(ext, _T("sf2pack")) || !_tcsicmp(ext, _T("sfz")))
		{
			if (!load_font_item(uDeviceID, name))
			{
				FreeFonts(uDeviceID);
				return;
			}
		}
		else if (!_tcsicmp(ext, _T("sflist")))
		{
			if (!load_font_item(uDeviceID, name))
			{
				FreeFonts(uDeviceID);
				return;
			}
		}

		std::vector< BASS_MIDI_FONTEX > fonts;
		for (unsigned long i = 0, j = presetList[uDeviceID].size(); i < j; ++i)
		{
			fonts.push_back(presetList[uDeviceID][j - i - 1]);
		}
		BASS_MIDI_StreamSetFonts(KSStream, &fonts[0], (unsigned int)fonts.size() | BASS_MIDI_FONT_EX);
	}
}