/*
Keppy's Synthesizer buffer system
Some code has been optimized by Sono (MarcusD), the old one has been commented out
*/

#define SETVELOCITY(evento, newvelocity) evento = (DWORD(evento) & 0xFF00FFFF) | ((DWORD(newvelocity) & 0xFF) << 16)
#define SETNOTE(evento, newnote) evento = (DWORD(evento) & 0xFFFF00FF) | ((DWORD(newnote) & 0xFF) << 8)
#define SETSTATUS(evento, newstatus) evento = (DWORD(evento) & 0xFFFFFF00) | (DWORD(newstatus) & 0xFF)

int BufferCheck(void) {
	int retval;
	EnterCriticalSection(&mim_section);
	retval = vms2emu ? eventcount : (readhead != writehead) ? ~0 : 0;
	LeaveCriticalSection(&mim_section);
	return retval;
}

int BufferCheckHyper(void) {
	int retval;
	EnterCriticalSection(&mim_section);
	retval = (readhead != writehead) ? ~0 : 0;
	LeaveCriticalSection(&mim_section);
	return retval;
}

void SendToBASSMIDI(DWORD dwParam1) {
	if (!(dwParam1 - 0x80 & 0xC0))
	{
		BASS_MIDI_StreamEvents(KSStream, BASS_MIDI_EVENTS_RAW, &dwParam1, 3);
		return;
	}

	DWORD dwParam2 = dwParam1 & 0xF0;
	DWORD len = (dwParam2 >= 0xF8 && dwParam2 <= 0xFF) ? 1 : ((dwParam2 == 0xC0 || dwParam2 == 0xD0) ? 2 : 3);

	BASS_MIDI_StreamEvents(KSStream, BASS_MIDI_EVENTS_RAW, &dwParam1, len);
	PrintEventToConsole(FOREGROUND_GREEN, dwParam1, FALSE, "Parsed normal MIDI event.");
}

void SendLongToBASSMIDI(MIDIHDR* IIMidiHdr) {
	BASS_MIDI_StreamEvents(KSStream, BASS_MIDI_EVENTS_RAW, (void*)IIMidiHdr->lpData, IIMidiHdr->dwBufferLength);
	PrintEventToConsole(FOREGROUND_GREEN, 0, TRUE, "Parsed SysEx MIDI event.");
}

void SendToBASSMIDIHyper(DWORD dwParam1) {
	if (!(dwParam1 - 0x80 & 0xC0))
	{
		BASS_MIDI_StreamEvents(KSStream, BASS_MIDI_EVENTS_RAW, &dwParam1, 3);
		return;
	}

	DWORD dwParam2 = dwParam1 & 0xF0;
	DWORD len = (dwParam2 >= 0xF8 && dwParam2 <= 0xFF) ? 1 : ((dwParam2 == 0xC0 || dwParam2 == 0xD0) ? 2 : 3);

	BASS_MIDI_StreamEvents(KSStream, BASS_MIDI_EVENTS_RAW, &dwParam1, len);
}

void SendLongToBASSMIDIHyper(MIDIHDR* IIMidiHdr) {
	BASS_MIDI_StreamEvents(KSStream, BASS_MIDI_EVENTS_RAW, (void*)IIMidiHdr->lpData, IIMidiHdr->dwBufferLength);
}

int __inline PlayBufferedData(void) {
	if (allnotesignore || !BufferCheck()) return 1;

	do {
		evbuf_t TempBuffer = *(evbuf + readhead);
		if (++readhead >= evbuffsize) readhead = 0;

		switch (TempBuffer.uMsg) {
		case MODM_DATA:
			_SndBASSMIDI(TempBuffer.dwParam1);
			break;
		case MODM_LONGDATA:
			BASS_MIDI_StreamEvents(KSStream, BASS_MIDI_EVENTS_RAW, (void*)TempBuffer.dwParam1, TempBuffer.dwParam2);
			break;
		}
	} while (vms2emu ? InterlockedDecrement64(&eventcount) : (readhead != writehead));

	return 0;
}

int __inline PlayBufferedDataHyper(void) {
	if (!BufferCheckHyper()) return 1;

	do {
		evbuf_t TempBuffer = *(evbuf + readhead);
		if (++readhead >= evbuffsize) readhead = 0;

		switch (TempBuffer.uMsg) {
		case MODM_DATA:
			_SndBASSMIDI(TempBuffer.dwParam1);
			break;
		case MODM_LONGDATA:
			BASS_MIDI_StreamEvents(KSStream, BASS_MIDI_EVENTS_RAW, (void*)TempBuffer.dwParam1, TempBuffer.dwParam2);
			break;
		}
	} while (readhead != writehead);

	return 0;
}

int __inline PlayBufferedDataChunk(void) {
	DWORD evt = writehead;

	if (allnotesignore || !BufferCheck()) return 1;
	do {
		evbuf_t TempBuffer = *(evbuf + readhead);
		if (++readhead >= evbuffsize) readhead = 0;

		switch (TempBuffer.uMsg) {
		case MODM_DATA:
			_SndBASSMIDI(TempBuffer.dwParam1);
			break;
		case MODM_LONGDATA:
			BASS_MIDI_StreamEvents(KSStream, BASS_MIDI_EVENTS_RAW, (void*)TempBuffer.dwParam1, TempBuffer.dwParam2);
			break;
		}
	} while (vms2emu ? InterlockedDecrement64(&eventcount) : (readhead != evt));

	return 0;
}

int __inline PlayBufferedDataChunkHyper(void) {
	DWORD evt = writehead;

	if (!BufferCheckHyper()) return 1;
	do {
		evbuf_t TempBuffer = *(evbuf + readhead);
		if (++readhead >= evbuffsize) readhead = 0;

		switch (TempBuffer.uMsg) {
		case MODM_DATA:
			_SndBASSMIDI(TempBuffer.dwParam1);
			break;
		case MODM_LONGDATA:
			BASS_MIDI_StreamEvents(KSStream, BASS_MIDI_EVENTS_RAW, (void*)TempBuffer.dwParam1, TempBuffer.dwParam2);
			break;
		}
	} while (readhead != evt);

	return 0;
}

BOOL CheckIfEventIsToIgnore(DWORD dwParam1) 
{
	/* 
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

	Understandable version of what the following function does
	*/

	if (ignorenotes1)
	{
		if (!((dwParam1 - 0x80) & 0xE0)
			&& ((HIWORD(dwParam1) & 0xFF) >= lovel && (HIWORD(dwParam1) & 0xFF) <= hivel))
		{
			PrintToConsole(FOREGROUND_RED, dwParam1, "Ignored NoteON/NoteOFF MIDI event.");
			return TRUE;
		}
	}
	if (limit88)
	{
		if (!((dwParam1 - 0x80) & 0xE0) && dwParam1 != 0x89)
		{
			if (!(((dwParam1 >> 8) & 0xFF) >= 21 && ((dwParam1 >> 8) & 0xFF) <= 108))
			{
				PrintToConsole(FOREGROUND_RED, dwParam1, "Ignored NoteON/NoteOFF MIDI event.");
				return TRUE;
			}
		}
	}
	return FALSE;
}

DWORD ReturnEditedEvent(DWORD dwParam1) {
	// SETSTATUS(dwParam1, status);

	/* 
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
	
	Understandable version of what the following function does
	*/

	if (pitchshift != 0x7F)
	{
		if (!((dwParam1 - 0x80) & 0xE0) && (dwParam1 & 0xF) != 9)
		{
			if (pitchshiftchan[dwParam1 & 0xF])
			{
				int newnote = (((dwParam1 >> 8) & 0xFF) - 0x7F) + pitchshift;
				if (newnote > 0x7F) { newnote = 0x7F; }
				else if (newnote < 0) { newnote = 0; }
				SETNOTE(dwParam1, newnote);
			}
		}
	}
	if (fullvelocity && (((dwParam1 & 0xFF) & 0xF0) == 0x90 && ((dwParam1 >> 16) & 0xFF)))
		SETVELOCITY(dwParam1, 0x7F);

	return dwParam1;
}

MMRESULT ParseData(UINT uMsg, DWORD_PTR dwParam1, DWORD_PTR dwParam2) {
	if (!streaminitialized || (uMsg != MODM_LONGDATA && (ignorenotes1 || limit88) && CheckIfEventIsToIgnore(dwParam1)))
		return MMSYSERR_NOERROR;

	if ((uMsg != MODM_LONGDATA) && fullvelocity || pitchshift != 0x7F)
		dwParam1 = ReturnEditedEvent(dwParam1);

	EnterCriticalSection(&mim_section);
	long long tempevent = writehead;
	if (++writehead >= evbuffsize) writehead = 0;

	evbuf[tempevent].uMsg = uMsg;
	evbuf[tempevent].dwParam1 = dwParam1;
	evbuf[tempevent].dwParam2 = dwParam2;
	LeaveCriticalSection(&mim_section);

	if (vms2emu)
		if (InterlockedIncrement64(&eventcount) >= evbuffsize) do { /* Absolutely nothing */ } while (eventcount >= evbuffsize);

	return MMSYSERR_NOERROR;
}

MMRESULT ParseDataHyper(UINT uMsg, DWORD_PTR dwParam1, DWORD_PTR dwParam2) {
	EnterCriticalSection(&mim_section);
	long long tempevent = writehead;
	if (++writehead >= evbuffsize) writehead = 0;

	evbuf[tempevent].uMsg = uMsg;
	evbuf[tempevent].dwParam1 = dwParam1;
	evbuf[tempevent].dwParam2 = dwParam2;
	LeaveCriticalSection(&mim_section);

	return MMSYSERR_NOERROR;
}

void AudioRender() {
	DWORD decoded;
	decoded = BASS_ChannelGetData(KSStream, sndbf, BASS_DATA_FLOAT + newsndbfvalue * sizeof(float));
}