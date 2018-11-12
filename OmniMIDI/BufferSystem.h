/*
OmniMIDI buffer system
Some code has been optimized by Sono (MarcusD), the old one has been commented out
*/

#define SETVELOCITY(evento, newvelocity) evento = (DWORD(evento) & 0xFF00FFFF) | ((DWORD(newvelocity) & 0xFF) << 16)
#define SETNOTE(evento, newnote) evento = (DWORD(evento) & 0xFFFF00FF) | ((DWORD(newnote) & 0xFF) << 8)
#define SETSTATUS(evento, newstatus) evento = (DWORD(evento) & 0xFFFFFF00) | (DWORD(newstatus) & 0xFF)

void ResetDrumChannels()
{
	unsigned i;

	memset(drum_channels, 0, sizeof(drum_channels));
	drum_channels[9] = 1;

	memcpy(gs_part_to_ch, part_to_ch, sizeof(gs_part_to_ch));

	for (i = 0; i < 16; ++i) BASS_MIDI_StreamEvent(OMStream, i, MIDI_EVENT_DRUMS, drum_channels[i]);
}

int BufferCheck(void) {
	return ManagedSettings.DontMissNotes ? eventcount : (readhead != writehead) ? ~0 : 0;
}

int BufferCheckHyper(void) {
	return (readhead != writehead) ? ~0 : 0;
}

void SendToBASSMIDI(DWORD dwParam1) {
	/*

	THIS IS A WIP

	if (OverrideNoteOff) {
		if ((((dwParam1 & 0xFF) & 0xF0) == 0x90 && ((dwParam1 >> 16) & 0xFF))) {
			BASS_MIDI_EVENT e[2];
			memset(e, 0, sizeof(e));

			e[0].event = MIDI_EVENT_NOTE;
			e[0].param = MAKEWORD(HIBYTE(LOWORD(dwParam1)), LOBYTE(HIWORD(dwParam1)));
			e[0].chan = dwParam1 & 0xF;
			e[0].pos = 0;
			e[0].tick = 0;

			e[1].event = MIDI_EVENT_NOTE;
			e[1].param = MAKEWORD(HIBYTE(LOWORD(dwParam1)), 0);
			e[1].chan = dwParam1 & 0xF;
			e[1].pos = BASS_ChannelSeconds2Bytes(OMStream, 0.25);
			e[1].tick = 0;

			BASS_MIDI_StreamEvents(OMStream, BASS_MIDI_EVENTS_STRUCT | BASS_MIDI_EVENTS_TIME, &e, 2);
			return;
		}
		else if (((dwParam1 & 0xFF) & 0xF0) == 0x80) return;
	}

	*/

	DWORD dwParam2 = dwParam1 & 0xF0;
	DWORD len = (dwParam2 >= 0xF8 && dwParam2 <= 0xFF) ? 1 : ((dwParam2 == 0xC0 || dwParam2 == 0xD0) ? 2 : 3);

	BASS_MIDI_StreamEvents(OMStream, BASS_MIDI_EVENTS_RAW, &dwParam1, len);
	// PrintEventToConsole(FOREGROUND_GREEN, dwParam1, FALSE, "Parsed normal MIDI event.");
}

void SendToBASSMIDIHyper(DWORD dwParam1) {
	BASS_MIDI_StreamEvents(
		OMStream, BASS_MIDI_EVENTS_RAW, 
		&dwParam1, ((dwParam1 & 0xF0) >= 0xF8 && (dwParam1 & 0xF0) <= 0xFF) ? 1 : (((dwParam1 & 0xF0) == 0xC0 || (dwParam1 & 0xF0) == 0xD0) ? 2 : 3)
	);
}

void SendLongToBASSMIDI(MIDIHDR* IIMidiHdr) {
	PrintSysExMessageToDebugLog(
		BASS_MIDI_StreamEvents(OMStream, BASS_MIDI_EVENTS_RAW, IIMidiHdr->lpData, (int)IIMidiHdr->dwBufferLength),
		IIMidiHdr);
	// PrintEventToConsole(FOREGROUND_GREEN, 0, TRUE, "Parsed SysEx MIDI event.");
}

DWORD __inline PlayBufferedData(void) {
	if (ManagedSettings.IgnoreAllEvents || !BufferCheck()) return 1;

	do {
		LockSystem.LockForReading();
		ULONGLONG tempevent = readhead;
		if (++readhead >= EvBufferSize) readhead = 0;
		evbuf_t TempBuffer = *(evbuf + tempevent);
		LockSystem.UnlockForReading();

		_StoBASSMIDI(TempBuffer.dwParam1);
	} while (ManagedSettings.DontMissNotes ? InterlockedDecrement64(&eventcount) : ((readhead != writehead) ? ~0 : 0));

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

		_StoBASSMIDI(TempBuffer.dwParam1);
	} while ((readhead != writehead) ? ~0 : 0);

	return 0;
}

DWORD __inline PlayBufferedDataChunk(void) {
	if (ManagedSettings.IgnoreAllEvents || !BufferCheck()) return 1;

	ULONGLONG whe = writehead;
	do {
		LockSystem.LockForReading();
		ULONGLONG tempevent = readhead;
		if (++readhead >= EvBufferSize) readhead = 0;
		evbuf_t TempBuffer = *(evbuf + tempevent);
		LockSystem.UnlockForReading();

		_StoBASSMIDI(TempBuffer.dwParam1);
	} while (ManagedSettings.DontMissNotes ? InterlockedDecrement64(&eventcount) : ((readhead != whe) ? ~0 : 0));
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

		_StoBASSMIDI(TempBuffer.dwParam1);
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

	if (ManagedSettings.IgnoreNotesBetweenVel && !((dwParam1 - 0x80) & 0xE0)
		&& ((HIWORD(dwParam1) & 0xFF) >= ManagedSettings.MinVelIgnore && (HIWORD(dwParam1) & 0xFF) <= ManagedSettings.MaxVelIgnore))
	{
		PrintMessageToDebugLog("CheckIfEventIsToIgnoreFunc", "Ignored NoteON/NoteOFF MIDI event.");
		return TRUE;
	}

	if (ManagedSettings.LimitTo88Keys && (!((dwParam1 - 0x80) & 0xE0) && dwParam1 != 0x89))
	{
		if (!(((dwParam1 >> 8) & 0xFF) >= 21 && ((dwParam1 >> 8) & 0xFF) <= 108))
		{
			PrintMessageToDebugLog("CheckIfEventIsToIgnoreFunc", "Ignored NoteON/NoteOFF MIDI event.");
			return TRUE;
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

	if (ManagedSettings.TransposeValue != 0x7F)
	{
		if (!((dwParam1 - 0x80) & 0xE0) && (dwParam1 & 0xF) != 9)
		{
			if (pitchshiftchan[dwParam1 & 0xF])
			{
				int newnote = (((dwParam1 >> 8) & 0xFF) - 0x7F) + ManagedSettings.TransposeValue;
				if (newnote > 0x7F) { newnote = 0x7F; }
				else if (newnote < 0) { newnote = 0; }
				SETNOTE(dwParam1, newnote);
			}
		}
	}
	if (ManagedSettings.FullVelocityMode && (((dwParam1 & 0xFF) & 0xF0) == 0x90 && ((dwParam1 >> 16) & 0xFF)))
		SETVELOCITY(dwParam1, 0x7F);

	return dwParam1;
}

MMRESULT ParseData(UINT uMsg, DWORD_PTR dwParam1, DWORD_PTR dwParam2) {
	if (!EVBuffReady || (uMsg != MODM_LONGDATA && CheckIfEventIsToIgnore(dwParam1)))
		return MMSYSERR_NOERROR;

	if ((uMsg != MODM_LONGDATA) && (ManagedSettings.FullVelocityMode || ManagedSettings.TransposeValue != 0x7F))
		dwParam1 = ReturnEditedEvent(dwParam1);

	// Prepare the event in the buffer
	LockSystem.LockForWriting();
	long long tempevent = writehead;
	if (++writehead >= EvBufferSize) writehead = 0;

	evbuf_t evtobuf{
		uMsg,
		dwParam1,
		dwParam2
	};

	evbuf[tempevent] = evtobuf;
	LockSystem.UnlockForWriting();

	// Some checks
	if (ManagedSettings.DontMissNotes && InterlockedIncrement64(&eventcount) >= EvBufferSize) do { /* Absolutely nothing */ } while (eventcount >= EvBufferSize);

	// Haha everything is fine
	return MMSYSERR_NOERROR;
}

MMRESULT ParseDataHyper(UINT uMsg, DWORD_PTR dwParam1, DWORD_PTR dwParam2) {
	// Prepare the event in the buffer
	LockSystem.LockForWriting();
	long long tempevent = writehead;
	if (++writehead >= EvBufferSize) writehead = 0;

	evbuf_t evtobuf{
		uMsg,
		dwParam1,
		dwParam2
	};

	evbuf[tempevent] = evtobuf;
	LockSystem.UnlockForWriting();

	// Haha everything is fine
	return MMSYSERR_NOERROR;
}