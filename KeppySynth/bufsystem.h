/*
Keppy's Synthesizer buffer system
*/

#define SETVELOCITY(evento, newvelocity) evento = (DWORD(evento) & 0xFF00FFFF) | ((DWORD(newvelocity) & 0xFF) << 16)
#define SETNOTE(evento, newnote) evento = (DWORD(evento) & 0xFFFF00FF) | ((DWORD(newnote) & 0xFF) << 8)
#define SETSTATUS(evento, newstatus) evento = (DWORD(evento) & 0xFFFFFF00) | (DWORD(newstatus) & 0xFF)

int BufferCheck(void){
	int retval;
	int retvalemu;
	if (improveperf == 0) EnterCriticalSection(&midiparsing);
	retval = (evbrpoint != evbwpoint) ? ~0 : 0;
	retvalemu = evbcount;
	if (improveperf == 0) LeaveCriticalSection(&midiparsing);

	return vms2emu ? retvalemu : retval;
}

void SendToBASSMIDI(DWORD dwParam1) {
	BYTE statusv = (BYTE)dwParam1;
	DWORD len = 0;

	int status = (dwParam1 & 0x000000FF);
	int note = (dwParam1 & 0x0000FF00) >> 8;
	int velocity = (dwParam1 & 0x00FF0000) >> 16;
	int channel = (dwParam1 & 0xFF000000) >> 24;

	if ((statusv >= 0xC0 && statusv <= 0xDF) || statusv == 0xF1 || statusv == 0xF3)	len = 2;
	else if (statusv < 0xF0 || statusv == 0xF2)	len = 3;
	else len = 1;

	BASS_MIDI_StreamEvents(KSStream, BASS_MIDI_EVENTS_RAW, &dwParam1, len);
	PrintEventToConsole(FOREGROUND_GREEN, dwParam1, FALSE, "Parsed normal MIDI event.", channel, status, note, velocity);
}

void SendLongToBASSMIDI(DWORD dwParam1) {
	MIDIHDR* rhdr;
	rhdr = (MIDIHDR*)dwParam1;

	BASS_MIDI_StreamEvents(KSStream, BASS_MIDI_EVENTS_RAW, rhdr->lpData, rhdr->dwBytesRecorded);
	PrintEventToConsole(FOREGROUND_GREEN, (DWORD)rhdr->lpData, TRUE, "Parsed SysEx MIDI event.", 0, 0, 0, 0);
}

int PlayBufferedData(void){
	if (allnotesignore) {
		return 0;
	}
	else {
		UINT uMsg;
		DWORD_PTR	dwParam1;
		DWORD_PTR   dwParam2;
		UINT evbpoint;
		int exlen;
		unsigned char *sysexbuffer;

		if (!BufferCheck()){
			return ~0;
		}

		while (vms2emu ? InterlockedDecrement64(&evbcount) : BufferCheck()) {
			if (improveperf == 0) EnterCriticalSection(&midiparsing);
			evbpoint = evbrpoint;

			if (++evbrpoint >= evbuffsize) {			
				evbrpoint -= evbuffsize;
			}

			uMsg = evbuf[evbpoint].uMsg;
			dwParam1 = evbuf[evbpoint].dwParam1;
			dwParam2 = evbuf[evbpoint].dwParam2;
			exlen = evbuf[evbpoint].exlen;
			sysexbuffer = evbuf[evbpoint].sysexbuffer;
			if (improveperf == 0) LeaveCriticalSection(&midiparsing);

			switch (uMsg) {
			case MODM_DATA:
				SendToBASSMIDI(dwParam1);
				break;
			case MODM_LONGDATA:
				if (sysresetignore != 1) SendLongToBASSMIDI(dwParam1);
				else PrintToConsole(FOREGROUND_RED, dwParam1, "Ignored SysEx MIDI event.");
				break;
			}
		}

		return 0;
	}
}

BOOL CheckIfEventIsToIgnore(DWORD dwParam1, int status, int note, int velocity, int channel) {
	if (ignorenotes1) {
		if (((LOWORD(dwParam1) & 0xF0) == 128 || (LOWORD(dwParam1) & 0xF0) == 144)
			&& ((HIWORD(dwParam1) & 0xFF) >= lovel && (HIWORD(dwParam1) & 0xFF) <= hivel)) {
			PrintToConsole(FOREGROUND_RED, dwParam1, "Ignored NoteON/NoteOFF MIDI event.");
			return TRUE;
		}
		else {
			if (limit88) {
				if ((Between(status, 0x80, 0x8f) && (status != 0x89)) || (Between(status, 0x90, 0x9f) && (status != 0x99))) {
					if (!(note >= 21 && note <= 108)) {
						PrintToConsole(FOREGROUND_RED, dwParam1, "Ignored NoteON/NoteOFF MIDI event.");
						return TRUE;
					}
				}
			}
		}
	}
	else {
		if (limit88 == 1) {
			if ((Between(status, 0x80, 0x8f) && (status != 0x89)) || (Between(status, 0x90, 0x9f) && (status != 0x99))) {
				if (!(note >= 21 && note <= 108)) {
					PrintToConsole(FOREGROUND_RED, dwParam1, "Ignored NoteON/NoteOFF MIDI event.");
					return TRUE;
				}
			}
		}
	}

	return FALSE;
}

DWORD ReturnEditedEvent(DWORD dwParam1, int status, int note, int velocity, int channel) {
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

	return dwParam1;
}

MMRESULT ParseData(BOOL direct, LONG evbpoint, UINT uMsg, UINT uDeviceID, DWORD_PTR dwParam1, DWORD_PTR dwParam2, int exlen, unsigned char *sysexbuffer) {
	if (ksdirectenabled != direct && ksdirectenabled != TRUE) ksdirectenabled = direct;

	int status = (dwParam1 & 0x000000FF);
	int note = (dwParam1 & 0x0000FF00) >> 8;
	int velocity = (dwParam1 & 0x00FF0000) >> 16;
	int channel = (dwParam1 & 0xFF000000) >> 24;

	if (CheckIfEventIsToIgnore(dwParam1, status, note, velocity, channel)) return MMSYSERR_NOERROR;

	if (improveperf == 0) EnterCriticalSection(&midiparsing);

	evbpoint = evbwpoint;
	if (++evbwpoint >= evbuffsize) {
		evbwpoint -= evbuffsize;
	}

	dwParam1 = ReturnEditedEvent(dwParam1, status, note, velocity, channel);

	evbuf[evbpoint].uMsg = uMsg;
	evbuf[evbpoint].dwParam1 = dwParam1;
	evbuf[evbpoint].dwParam2 = dwParam2;
	evbuf[evbpoint].exlen = exlen;
	evbuf[evbpoint].sysexbuffer = sysexbuffer;

	if (improveperf == 0) LeaveCriticalSection(&midiparsing);

	if (vms2emu == 1) {
		if (InterlockedIncrement64(&evbcount) >= evbuffsize) {
			do { usleep(1); } while (evbcount >= evbuffsize);
		}
	}

	return MMSYSERR_NOERROR;
}

void AudioRender() {
	DWORD decoded;
	decoded = BASS_ChannelGetData(KSStream, sndbf, BASS_DATA_FLOAT + newsndbfvalue * sizeof(float));
}