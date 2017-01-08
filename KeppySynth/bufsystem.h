/*
Keppy's Synthesizer buffer system
*/

#define SETVELOCITY(evento, newstatus) evento = (DWORD(evento) & 0xFF00FFFF) | ((DWORD(newstatus) & 0xFF) << 16)
#define SETNOTE(evento, newnote) evento = (DWORD(evento) & 0xFFFF00FF) | ((DWORD(newnote) & 0xFF) << 8)
#define SETSTATUS(evento, newvelocity) evento = (DWORD(evento) & 0xFFFFFF00) | (DWORD(newvelocity) & 0xFF)

static void ResetDrumChannels()
{
	static const BYTE part_to_ch[16] = { 9, 0, 1, 2, 3, 4, 5, 6, 7, 8, 10, 11, 12, 13, 14, 15 };

	unsigned i;

	memset(drum_channels, 0, sizeof(drum_channels));
	drum_channels[9] = 1;

	memcpy(gs_part_to_ch, part_to_ch, sizeof(gs_part_to_ch));

	if (hStream)
	{
		for (i = 0; i < 16; ++i)
		{
			BASS_MIDI_StreamEvent(hStream, i, MIDI_EVENT_DRUMS, drum_channels[i]);
		}
	}
}

int bmsyn_buf_check(void){
	int retval;
	int retvalemu;
	EnterCriticalSection(&mim_section);
	retval = (evbrpoint != evbwpoint) ? ~0 : 0;
	retvalemu = evbcount;
	LeaveCriticalSection(&mim_section);
	if (vms2emu == 1) {
		return retvalemu;
	}
	else {
		return retval;
	}
}

bool depends() {
	if (vms2emu == 1) {
		return InterlockedDecrement(&evbcount);
	}
	else {
		return bmsyn_buf_check();
	}
}

void playnotes(int status, int note, int velocity, DWORD_PTR dwParam1, DWORD_PTR dwParam2, int exlen) {
	SETSTATUS(dwParam1, status);
	if (mainkey != 12) {
		if (Between(status, 0x80, 0x8f) || Between(status, 0x90, 0x9f)) {
			int newnote = (note - 12) + mainkey;
			if (newnote > 127) { newnote = 127; }
			else if (newnote < 0) { newnote = 0; }
			SETNOTE(dwParam1, newnote);
		}
	}
	else {
		SETNOTE(dwParam1, note);
	}
	SETVELOCITY(dwParam1, velocity);

	dwParam2 = dwParam1 & 0xF0;
	exlen = (dwParam2 >= 0xF8 && dwParam2 <= 0xFF) ? 1 : ((dwParam2 == 0xC0 || dwParam2 == 0xD0) ? 2 : 3);
	BASS_MIDI_StreamEvents(hStream, BASS_MIDI_EVENTS_RAW, &dwParam1, exlen);

	CheckUp();

	if (debugmode == 1) {
		PrintEventToConsole(FOREGROUND_GREEN, dwParam1, "Parsed normal MIDI event.", status, note, velocity);
	}
}

int bmsyn_play_some_data(void){
	if (allnotesignore) {
		return 0;
	}
	else {
		UINT uDeviceID;
		UINT uMsg;
		DWORD_PTR	dwParam1;
		DWORD_PTR   dwParam2;

		UINT evbpoint;
		int exlen;
		unsigned char *sysexbuffer;

		if (!bmsyn_buf_check()){
			return ~0;
		}
		do{
			EnterCriticalSection(&mim_section);
			evbpoint = evbrpoint;

			if (++evbrpoint >= newevbuffvalue) {
				evbrpoint -= newevbuffvalue;
			}

			uDeviceID = evbuf[evbpoint].uDeviceID;
			uMsg = evbuf[evbpoint].uMsg;
			dwParam1 = evbuf[evbpoint].dwParam1;
			dwParam2 = evbuf[evbpoint].dwParam2;
			exlen = evbuf[evbpoint].exlen;
			sysexbuffer = evbuf[evbpoint].sysexbuffer;

			int status = (dwParam1 & 0x000000FF);
			int note = (dwParam1 & 0x0000FF00) >> 8;
			int velocity = (dwParam1 & 0x00FF0000) >> 16;

			LeaveCriticalSection(&mim_section);
			switch (uMsg) {
			case MODM_DATA:
				if (ignorenotes1 == 1) {	
					if ((LOWORD(dwParam1) & 0xF0) == 144 && ((HIWORD(dwParam1) & 0xFF) >= lovel && (HIWORD(dwParam1) & 0xFF) <= hivel)) {
						PrintToConsole(FOREGROUND_RED, dwParam1, "Ignored NoteON MIDI event.");
					}
					else {
						playnotes(status, note, velocity, dwParam1, dwParam2, exlen);
					}
				}
				else {
					playnotes(status, note, velocity, dwParam1, dwParam2, exlen);
				}
				break;
			case MODM_LONGDATA:
				if (sysresetignore == 1) {
					BASS_MIDI_StreamEvents(hStream, BASS_MIDI_EVENTS_RAW, sysexbuffer, exlen);
					CheckUp();
					if ((exlen == _countof(sysex_gm_reset) && !memcmp(sysexbuffer, sysex_gm_reset, _countof(sysex_gm_reset))) ||
						(exlen == _countof(sysex_gs_reset) && !memcmp(sysexbuffer, sysex_gs_reset, _countof(sysex_gs_reset))) ||
						(exlen == _countof(sysex_xg_reset) && !memcmp(sysexbuffer, sysex_xg_reset, _countof(sysex_xg_reset)))) {
						ResetDrumChannels();
					}
					if (debugmode == 1) {
						PrintToConsole(FOREGROUND_RED, dwParam1, "Parsed SysEx MIDI event.");
					}
				}
				free(sysexbuffer);
				break;
			}
		} while (depends());
		return 0;
	}
}

bool modmdata(UINT evbpoint, UINT uMsg, UINT uDeviceID, DWORD_PTR dwParam1, DWORD_PTR dwParam2, int exlen, unsigned char *sysexbuffer) {
	EnterCriticalSection(&mim_section);
	evbpoint = evbwpoint;
	if (++evbwpoint >= newevbuffvalue)
		evbwpoint -= newevbuffvalue;
	evbuf[evbpoint].uDeviceID = !!uDeviceID;
	evbuf[evbpoint].uMsg = uMsg;
	evbuf[evbpoint].dwParam1 = dwParam1;
	evbuf[evbpoint].dwParam2 = dwParam2;
	evbuf[evbpoint].exlen = exlen;
	evbuf[evbpoint].sysexbuffer = sysexbuffer;
	LeaveCriticalSection(&mim_section);
	if (vms2emu == 1) {
		if (InterlockedIncrement(&evbcount) >= newevbuffvalue) {
			do
			{
				if (debugmode == 1) {
					std::cout << "Buffer is full, slowing down..." << std::endl << std::flush;;
				}
				Sleep(1);
			} while (evbcount >= newevbuffvalue);
		}
	}
	return MMSYSERR_NOERROR;
}

bool longmodmdata(MIDIHDR *IIMidiHdr, UINT uDeviceID, DWORD_PTR dwParam1, DWORD_PTR dwParam2, int exlen, unsigned char *sysexbuffer) {
	IIMidiHdr = (MIDIHDR *)dwParam1;
	if (!(IIMidiHdr->dwFlags & MHDR_PREPARED)) return MIDIERR_UNPREPARED;
	IIMidiHdr->dwFlags &= ~MHDR_DONE;
	IIMidiHdr->dwFlags |= MHDR_INQUEUE;
	exlen = (int)IIMidiHdr->dwBufferLength;
	if (NULL == (sysexbuffer = (unsigned char *)malloc(exlen * sizeof(char)))) {
		return MMSYSERR_NOMEM;
	}
	else {
		memcpy(sysexbuffer, IIMidiHdr->lpData, exlen);
	}
	IIMidiHdr->dwFlags &= ~MHDR_INQUEUE;
	IIMidiHdr->dwFlags |= MHDR_DONE;
}

void AudioRender() {
	decoded = BASS_ChannelGetData(hStream, sndbf, BASS_DATA_FLOAT + newsndbfvalue * sizeof(float));
	if (encmode == 1) {

	}
	else if (encmode == 0) {
		for (unsigned i = 0, j = decoded / sizeof(float); i < j; i++) {
			sndbf[i] *= sound_out_volume_float;
		}
		sound_driver->write_frame(sndbf, decoded / sizeof(float), true);
	}
}