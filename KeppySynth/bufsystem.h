/*
Keppy's Synthesizer buffer system
*/

#define SETVELOCITY(evento, newvelocity) evento = (DWORD(evento) & 0xFF00FFFF) | ((DWORD(newvelocity) & 0xFF) << 16)
#define SETNOTE(evento, newnote) evento = (DWORD(evento) & 0xFFFF00FF) | ((DWORD(newnote) & 0xFF) << 8)
#define SETSTATUS(evento, newstatus) evento = (DWORD(evento) & 0xFFFFFF00) | (DWORD(newstatus) & 0xFF)

int bmsyn_buf_check(void){
	int retval;
	int retvalemu;
	if (improveperf == 0) EnterCriticalSection(&midiparsing);
	retval = (evbrpoint != evbwpoint) ? ~0 : 0;
	retvalemu = evbcount;
	if (improveperf == 0) LeaveCriticalSection(&midiparsing);
	if (vms2emu == 1) {
		return retvalemu;
	}
	else {
		return retval;
	}
}

bool depends() {
	if (vms2emu == 1) {
		return InterlockedDecrement64(&evbcount);
	}
	else {
		return bmsyn_buf_check();
	}
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

		if (!bmsyn_buf_check()){
			return ~0;
		}
		do{
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

			MIDIHDR *hdr = (MIDIHDR*)dwParam1;
			BYTE statusv = (BYTE)dwParam1;

			DWORD len;

			int status = (dwParam1 & 0x000000FF);
			int note = (dwParam1 & 0x0000FF00) >> 8;
			int velocity = (dwParam1 & 0x00FF0000) >> 16;
			int channel = (dwParam1 & 0xFF000000) >> 24;

			switch (uMsg) {
			case MODM_DATA:
				if ((statusv >= 0xc0 && statusv <= 0xdf) || statusv == 0xf1 || statusv == 0xf3)	len = 2;
				else if (statusv < 0xf0 || statusv == 0xf2)	len = 3;
				else len = 1;

				if (ignorenotes1) {
					if (((LOWORD(dwParam1) & 0xF0) == 128 || (LOWORD(dwParam1) & 0xF0) == 144)
						&& ((HIWORD(dwParam1) & 0xFF) >= lovel && (HIWORD(dwParam1) & 0xFF) <= hivel)) {
						PrintToConsole(FOREGROUND_RED, dwParam1, "Ignored NoteON/NoteOFF MIDI event.");
					}
					else {
						if (limit88 == 1) {
							if ((Between(status, 0x80, 0x8f) && (status != 0x89)) || (Between(status, 0x90, 0x9f) && (status != 0x99))) {
								if (note >= 21 && note <= 108) {
									if (vstimode == TRUE) BASS_VST_ProcessEventRaw(KSStream, &dwParam1, len);
									else BASS_MIDI_StreamEvents(KSStream, BASS_MIDI_EVENTS_RAW, &dwParam1, len);
									PrintEventToConsole(FOREGROUND_GREEN, dwParam1, FALSE, "Parsed normal MIDI event.", channel, status, note, velocity);
								}
							}
							else {
								if (vstimode == TRUE) BASS_VST_ProcessEventRaw(KSStream, &dwParam1, len);
								else BASS_MIDI_StreamEvents(KSStream, BASS_MIDI_EVENTS_RAW, &dwParam1, len);
								PrintEventToConsole(FOREGROUND_GREEN, dwParam1, FALSE, "Parsed normal MIDI event.", channel, status, note, velocity);
							}
						}
						else {
							if (vstimode == TRUE) BASS_VST_ProcessEventRaw(KSStream, &dwParam1, len);
							else BASS_MIDI_StreamEvents(KSStream, BASS_MIDI_EVENTS_RAW, &dwParam1, len);
							PrintEventToConsole(FOREGROUND_GREEN, dwParam1, FALSE, "Parsed normal MIDI event.", channel, status, note, velocity);
						}
					}
					break;
				}
				else {
					if (limit88 == 1) {
						if ((Between(status, 0x80, 0x8f) && (status != 0x89)) || (Between(status, 0x90, 0x9f) && (status != 0x99))) {
							if (note >= 21 && note <= 108) {
								if (vstimode == TRUE) BASS_VST_ProcessEventRaw(KSStream, &dwParam1, len);
								else BASS_MIDI_StreamEvents(KSStream, BASS_MIDI_EVENTS_RAW, &dwParam1, len);
								PrintEventToConsole(FOREGROUND_GREEN, dwParam1, FALSE, "Parsed normal MIDI event.", channel, status, note, velocity);
							}
							else PrintToConsole(FOREGROUND_RED, dwParam1, "Ignored NoteON/NoteOFF MIDI event.");
						}
						else {
							if (vstimode == TRUE) BASS_VST_ProcessEventRaw(KSStream, &dwParam1, len);
							else BASS_MIDI_StreamEvents(KSStream, BASS_MIDI_EVENTS_RAW, &dwParam1, len);
							PrintEventToConsole(FOREGROUND_GREEN, dwParam1, FALSE, "Parsed normal MIDI event.", channel, status, note, velocity);
						}
					}
					else {
						if (vstimode == TRUE) BASS_VST_ProcessEventRaw(KSStream, &dwParam1, len);
						else BASS_MIDI_StreamEvents(KSStream, BASS_MIDI_EVENTS_RAW, &dwParam1, len);
						PrintEventToConsole(FOREGROUND_GREEN, dwParam1, FALSE, "Parsed normal MIDI event.", channel, status, note, velocity);
					}
				}
				break;
			case MODM_LONGDATA:
				if (sysresetignore != 1) {
					if (vstimode == TRUE) BASS_VST_ProcessEventRaw(KSStream, hdr->lpData, hdr->dwBytesRecorded);
					else BASS_MIDI_StreamEvents(KSStream, BASS_MIDI_EVENTS_RAW, hdr->lpData, hdr->dwBytesRecorded);
					PrintEventToConsole(FOREGROUND_GREEN, (DWORD)hdr->lpData, TRUE, "Parsed SysEx MIDI event.", 0, 0, 0, 0);
				}
				break;
			}
		} while (depends());
		return 0;
	}
}

bool ParseData(LONG evbpoint, UINT uMsg, UINT uDeviceID, DWORD_PTR dwParam1, DWORD_PTR dwParam2, int exlen, unsigned char *sysexbuffer) {
	if (improveperf == 0) EnterCriticalSection(&midiparsing);

	evbpoint = evbwpoint;
	if (++evbwpoint >= evbuffsize)
		evbwpoint -= evbuffsize;

	int status = (dwParam1 & 0x000000FF);
	int note = (dwParam1 & 0x0000FF00) >> 8;
	int velocity = (dwParam1 & 0x00FF0000) >> 16;
	int channel = (dwParam1 & 0xFF000000) >> 24;

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

	evbuf[evbpoint].uMsg = uMsg;
	evbuf[evbpoint].dwParam1 = dwParam1;
	evbuf[evbpoint].dwParam2 = dwParam2;
	evbuf[evbpoint].exlen = exlen;
	evbuf[evbpoint].sysexbuffer = sysexbuffer;

	if (improveperf == 0) LeaveCriticalSection(&midiparsing);

	if (vms2emu == 1) {
		if (InterlockedIncrement64(&evbcount) >= evbuffsize) {
			do { Sleep(1); } while (evbcount >= evbuffsize);
		}
	}

	return MMSYSERR_NOERROR;
}

void AudioRender() {
	DWORD decoded;
	decoded = BASS_ChannelGetData(KSStream, sndbf, BASS_DATA_FLOAT + newsndbfvalue * sizeof(float));
}

#ifndef __BUFSYSTEM_H
#define __BUFSYSTEM_H

extern "C" __declspec(dllexport) void SendDirectData(DWORD dwParam)
{
	ParseData(0, MODM_DATA, NULL, dwParam, NULL, NULL, NULL);
}

extern "C" __declspec(dllexport) void SendDirectLongData(DWORD dwParam)
{
	ParseData(0, MODM_LONGDATA, NULL, dwParam, NULL, NULL, NULL);
}
#endif 