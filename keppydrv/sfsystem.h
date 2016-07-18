/*
Keppy's Driver soundfont lists loading system
*/

static unsigned int font_count[8] = { 0, 0, 0, 0, 0, 0, 0, 0 };
static HSOUNDFONT * hFonts[8] = { NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL };

std::vector<HSOUNDFONT> _soundFonts[8];
std::vector<BASS_MIDI_FONTEX> presetList[8];

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
					_tcscat(temp, nameptr);
				}
				if (name[0] != '@') {
					HSOUNDFONT font = BASS_MIDI_FontInit(temp, bass_flags);
					for (auto it = presets.begin(); it != presets.end(); ++it)
					{
						if (preload)
							BASS_MIDI_FontLoad(font, it->spreset, it->sbank);
						it->font = font;
						presetList[uDeviceID].push_back(*it);
					}
					_soundFonts[uDeviceID].push_back(font);
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
	KillWatchdog();

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
		BASS_MIDI_StreamSetFonts(hStream, &fonts[0], (unsigned int)fonts.size() | BASS_MIDI_FONT_EX);
	}

	RunWatchdog();
}