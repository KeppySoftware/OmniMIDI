/*
Keppy's Synthesizer buffer system
*/

#define SETVELOCITY(evento, newvelocity) evento = (DWORD(evento) & 0xFF00FFFF) | ((DWORD(newvelocity) & 0xFF) << 16)
#define SETNOTE(evento, newnote) evento = (DWORD(evento) & 0xFFFF00FF) | ((DWORD(newnote) & 0xFF) << 8)
#define SETSTATUS(evento, newstatus) evento = (DWORD(evento) & 0xFFFFFF00) | (DWORD(newstatus) & 0xFF)

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

void playnotes(int status, int note, int velocity, DWORD_PTR dwParam1, int exlen) {
	// SETSTATUS(dwParam1, status);
	if (pitchshift != 127) {
		if ((Between(status, 0x80, 0x8f) && (status != 0x89)) || (Between(status, 0x90, 0x9f) && (status != 0x99))) {
			if (pitchshiftchan[status - 0x80] == 1 || pitchshiftchan[status - 0x90] == 1) {
				int newnote = (note - 127) + pitchshift;
				if (newnote > 127) { newnote = 127; }
				else if (newnote < 0) { newnote = 0; }
				SETNOTE(dwParam1, newnote);
			}
		}
	}
	if (fullvelocity == 1 && (Between(status, 0x90, 0x9f) && velocity != 0))
		SETVELOCITY(dwParam1, 127);

	BYTE statusv = (BYTE)dwParam1;
	DWORD len;
	if ((statusv >= 0xc0 && statusv <= 0xdf) || statusv == 0xf1 || statusv == 0xf3)	len = 2;
	else if (statusv < 0xf0 || statusv == 0xf2)	len = 3;
	else len = 1;

	if (limit88 == 1) {
		if ((Between(status, 0x80, 0x8f) && (status != 0x89)) || (Between(status, 0x90, 0x9f) && (status != 0x99))) {
			if (note >= 21 && note <= 108) { BASS_MIDI_StreamEvents(KSStream, BASS_MIDI_EVENTS_RAW, &dwParam1, len); }
		}
		else BASS_MIDI_StreamEvents(KSStream, BASS_MIDI_EVENTS_RAW, &dwParam1, len);
	}
	else BASS_MIDI_StreamEvents(KSStream, BASS_MIDI_EVENTS_RAW, &dwParam1, len);

	CheckUp(ERRORCODE, L"DataToAudioStream");

	if (debugmode == 1) {
		PrintEventToConsole(FOREGROUND_GREEN, dwParam1, "Parsed normal MIDI event.", status, note, velocity);
	}
}

int bmsyn_play_some_data(void){
	if (allnotesignore) {
		return 0;
	}
	else {
		UINT uMsg;
		DWORD_PTR	dwParam1;

		UINT evbpoint;
		int exlen;

		if (!bmsyn_buf_check()){
			return ~0;
		}
		do{
			EnterCriticalSection(&mim_section);
			evbpoint = evbrpoint;

			if (++evbrpoint >= newevbuffvalue) {
				evbrpoint -= newevbuffvalue;
			}

			uMsg = evbuf[evbpoint].uMsg;
			dwParam1 = evbuf[evbpoint].dwParam1;

			int status = (dwParam1 & 0x000000FF);
			int note = (dwParam1 & 0x0000FF00) >> 8;
			int velocity = (dwParam1 & 0x00FF0000) >> 16;

			if (fullvelocity)
				velocity = 127;

			LeaveCriticalSection(&mim_section);
			
			switch (uMsg) {
			case MODM_DATA:
				if (ignorenotes1) {	
					if (((LOWORD(dwParam1) & 0xF0) == 128 || (LOWORD(dwParam1) & 0xF0) == 144) 
						&& ((HIWORD(dwParam1) & 0xFF) >= lovel && (HIWORD(dwParam1) & 0xFF) <= hivel)) {
						PrintToConsole(FOREGROUND_RED, dwParam1, "Ignored NoteON/NoteOFF MIDI event.");
					}
					else {
						playnotes(status, note, velocity, dwParam1, exlen);
					}
					break;
				}
				playnotes(status, note, velocity, dwParam1, exlen);
				break;
			case MODM_LONGDATA:
				if (sysresetignore != 1) {
					MIDIHDR *hdr = (MIDIHDR*)dwParam1;
					BASS_MIDI_StreamEvents(KSStream, BASS_MIDI_EVENTS_RAW, hdr->lpData, hdr->dwBytesRecorded);
					CheckUp(ERRORCODE, L"LongDataToAudioStream");
					if (debugmode) {
						PrintToConsole(FOREGROUND_RED, dwParam1, "Parsed SysEx MIDI event.");
					}
				}
				break;
			}
		} while (depends());
		return 0;
	}
}

bool ParseData(UINT evbpoint, UINT uMsg, UINT uDeviceID, DWORD_PTR dwParam1, DWORD_PTR dwParam2, int exlen, unsigned char *sysexbuffer) {
	EnterCriticalSection(&mim_section);
	evbpoint = evbwpoint;
	if (++evbwpoint >= newevbuffvalue)
		evbwpoint -= newevbuffvalue;
	evbuf[evbpoint].uMsg = uMsg;
	evbuf[evbpoint].dwParam1 = dwParam1;
	LeaveCriticalSection(&mim_section);
	if (vms2emu == 1) {
		if (InterlockedIncrement(&evbcount) >= newevbuffvalue) {
			do
			{
				if (debugmode) {
					std::cout << "Buffer is full, slowing down..." << std::endl << std::flush;;
				}
				Sleep(1);
			} while (evbcount >= newevbuffvalue);
		}
	}
	return MMSYSERR_NOERROR;
}

void AudioRender() {
	DWORD decoded;
	decoded = BASS_ChannelGetData(KSStream, sndbf, BASS_DATA_FLOAT + newsndbfvalue * sizeof(float));
	CheckUp(ERRORCODE, L"GetDataFromStream");
	if (encmode == 0) {
		if (xaudiodisabled == 0) {
			for (unsigned i = 0, j = decoded / sizeof(float); i < j; i++) {
				sndbf[i] *= sound_out_volume_float;
			}
			if (!BASSXA_WriteFrame(sound_driver, sndbf, decoded, BASSXA_FRAMEWAIT))
				crashmessage(L"XAWriteFrame");
		}
	}
}