/*
Keppy's Synthesizer buffer system
Some code has been optimized by Sono (MarcusD), the old one has been commented out
*/

#define SETVELOCITY(evento, newvelocity) evento = (DWORD(evento) & 0xFF00FFFF) | ((DWORD(newvelocity) & 0xFF) << 16)
#define SETNOTE(evento, newnote) evento = (DWORD(evento) & 0xFFFF00FF) | ((DWORD(newnote) & 0xFF) << 8)
#define SETSTATUS(evento, newstatus) evento = (DWORD(evento) & 0xFFFFFF00) | (DWORD(newstatus) & 0xFF)

int BufferCheck(void) {
	return DontMissNotes ? eventcount : (readhead != writehead) ? ~0 : 0;
}

int BufferCheckHyper(void) {
	return (readhead != writehead) ? ~0 : 0;
}

void SendToBASSMIDI(DWORD dwParam1) {
	if (!(dwParam1 - 0x80 & 0xC0))
	{
		BASS_MIDI_StreamEvents(KSStream, BASS_MIDI_EVENTS_RAW, &dwParam1, 3);
		return;
	}

	DWORD len = 3;

	if (!((dwParam1 - 0xC0) & 0xE0)) len = 2;
	else if ((dwParam1 & 0xF0) == 0xF0)
	{
		switch (dwParam1 & 0xF)
		{
		case 3:
			len = 2;
			break;
		default:
			len = 1;
			break;
		}
	}

	BASS_MIDI_StreamEvents(KSStream, BASS_MIDI_EVENTS_RAW, &dwParam1, len);
	// PrintEventToConsole(FOREGROUND_GREEN, dwParam1, FALSE, "Parsed normal MIDI event.");
}

void SendLongToBASSMIDI(const void* sysexbuffer, DWORD exlen) {
	BASS_MIDI_StreamEvents(KSStream, BASS_MIDI_EVENTS_RAW, sysexbuffer, exlen);
	// PrintEventToConsole(FOREGROUND_GREEN, 0, TRUE, "Parsed SysEx MIDI event.");
}

DWORD __inline PlayBufferedData(void) {
	if (IgnoreAllEvents || !BufferCheck()) return 1;

	do {
		LockSystem.LockForReading();
		ULONGLONG tempevent = readhead;
		if (++readhead >= EvBufferSize) readhead = 0;
		evbuf_t TempBuffer = *(evbuf + tempevent);
		LockSystem.UnlockForReading();

		SendToBASSMIDI(TempBuffer.dwParam1);
	} while (DontMissNotes ? InterlockedDecrement64(&eventcount) : ((readhead != writehead) ? ~0 : 0));

	return 0;
}

DWORD __inline PlayBufferedDataHyper(void) {
	if (!BufferCheckHyper()) return 1;

	do {
		LockSystem.LockForReading();
		ULONGLONG tempevent = readhead;
		if (++readhead >= EvBufferSize) readhead = 0;
		evbuf_t TempBuffer = *(evbuf + tempevent);
		LockSystem.UnlockForReading();

		SendToBASSMIDI(TempBuffer.dwParam1);
	} while ((readhead != writehead) ? ~0 : 0);

	return 0;
}

DWORD __inline PlayBufferedDataChunk(void) {
	if (IgnoreAllEvents || !BufferCheck()) return 1;

	ULONGLONG whe = writehead;
	do {
		LockSystem.LockForReading();
		ULONGLONG tempevent = readhead;
		if (++readhead >= EvBufferSize) readhead = 0;
		evbuf_t TempBuffer = *(evbuf + tempevent);
		LockSystem.UnlockForReading();

		SendToBASSMIDI(TempBuffer.dwParam1);
	} while (DontMissNotes ? InterlockedDecrement64(&eventcount) : ((readhead != whe) ? ~0 : 0));
}

DWORD __inline PlayBufferedDataChunkHyper(void) {
	if (!BufferCheckHyper()) return 1;

	ULONGLONG whe = writehead;
	do {
		LockSystem.LockForReading();
		ULONGLONG tempevent = readhead;
		if (++readhead >= EvBufferSize) readhead = 0;
		evbuf_t TempBuffer = *(evbuf + tempevent);
		LockSystem.UnlockForReading();

		SendToBASSMIDI(TempBuffer.dwParam1);
	} while ((readhead != whe) ? ~0 : 0);
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

	if (IgnoreNotesBetweenVel)
	{
		if (!((dwParam1 - 0x80) & 0xE0)
			&& ((HIWORD(dwParam1) & 0xFF) >= MinVelIgnore && (HIWORD(dwParam1) & 0xFF) <= MaxVelIgnore))
		{
			PrintToConsole(FOREGROUND_RED, dwParam1, "Ignored NoteON/NoteOFF MIDI event.");
			return TRUE;
		}
	}
	if (LimitTo88Keys)
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

	if (TransposeValue != 0x7F)
	{
		if (!((dwParam1 - 0x80) & 0xE0) && (dwParam1 & 0xF) != 9)
		{
			if (pitchshiftchan[dwParam1 & 0xF])
			{
				int newnote = (((dwParam1 >> 8) & 0xFF) - 0x7F) + TransposeValue;
				if (newnote > 0x7F) { newnote = 0x7F; }
				else if (newnote < 0) { newnote = 0; }
				SETNOTE(dwParam1, newnote);
			}
		}
	}
	if (FullVelocityMode && (((dwParam1 & 0xFF) & 0xF0) == 0x90 && ((dwParam1 >> 16) & 0xFF)))
		SETVELOCITY(dwParam1, 0x7F);

	return dwParam1;
}

MMRESULT ParseData(UINT uMsg, DWORD_PTR dwParam1, DWORD dwParam2) {
	if (!EVBuffReady || (uMsg != MODM_LONGDATA && CheckIfEventIsToIgnore(dwParam1)))
		return MMSYSERR_NOERROR;

	if ((uMsg != MODM_LONGDATA) && (FullVelocityMode || TransposeValue != 0x7F))
		dwParam1 = ReturnEditedEvent(dwParam1);

	// Prepare the event in the buffer
	LockSystem.LockForWriting();
	long long tempevent = writehead;
	if (++writehead >= EvBufferSize) writehead = 0;

	evbuf_t evtobuf{
		uMsg = uMsg,
		dwParam1 = dwParam1,
		dwParam2 = dwParam2,
	};

	evbuf[tempevent] = evtobuf;
	LockSystem.UnlockForWriting();

	// Some checks
	if (DontMissNotes && InterlockedIncrement64(&eventcount) >= EvBufferSize) do { /* Absolutely nothing */ } while (eventcount >= EvBufferSize);

	// Haha everything is fine
	return MMSYSERR_NOERROR;
}

MMRESULT ParseDataHyper(UINT uMsg, DWORD_PTR dwParam1, DWORD dwParam2) {
	// Prepare the event in the buffer
	LockSystem.LockForWriting();
	long long tempevent = writehead;
	if (++writehead >= EvBufferSize) writehead = 0;

	evbuf_t evtobuf{
		uMsg = uMsg,
		dwParam1 = dwParam1,
		dwParam2 = dwParam2,
	};

	evbuf[tempevent] = evtobuf;
	LockSystem.UnlockForWriting();

	// Haha everything is fine
	return MMSYSERR_NOERROR;
}

void AudioRender() {
	BASS_ChannelGetData(KSStream, sndbf, BASS_DATA_FLOAT + 256.0f * sizeof(float));
}