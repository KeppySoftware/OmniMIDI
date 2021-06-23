/*
OmniMIDI buffer system
Some code has been optimized by Sono (SonoSooS), the old one has been commented out
*/
#pragma once
#define SMALLBUFFER 2

int __inline BufferCheck(void) {
	return (EVBuffer.ReadHead != EVBuffer.WriteHead);
}

BOOL __inline CheckIfEventIsToIgnore(DWORD dwParam1)
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

DWORD __inline ReturnEditedEvent(DWORD dwParam1) {
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
		if (!((dwParam1 - 0x80) & 0xE0))
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

void __inline SendToBASSMIDI(DWORD dwParam1) {
	DWORD len = 3;

	// PrintEventToDebugLog(dwParam1);

	switch (GETCMD(dwParam1)) {
	case MIDI_NOTEON:
		BASS_MIDI_StreamEvent(OMStream, dwParam1 & 0xF, MIDI_EVENT_NOTE, dwParam1 >> 8);
		return;
	case MIDI_NOTEOFF:
		BASS_MIDI_StreamEvent(OMStream, dwParam1 & 0xF, MIDI_EVENT_NOTE, (BYTE)(dwParam1 >> 8));
		return;
	case MIDI_POLYAFTER:
		BASS_MIDI_StreamEvent(OMStream, dwParam1 & 0xF, MIDI_EVENT_KEYPRES, dwParam1 >> 8);
		return;
	case MIDI_PROGCHAN:
		BASS_MIDI_StreamEvent(OMStream, dwParam1 & 0xF, MIDI_EVENT_PROGRAM, (BYTE)(dwParam1 >> 8));
		return;
	case MIDI_CHANAFTER:
		BASS_MIDI_StreamEvent(OMStream, dwParam1 & 0xF, MIDI_EVENT_CHANPRES, (BYTE)(dwParam1 >> 8));
		return;
	default:
		if (!(dwParam1 - 0x80 & 0xC0))
		{
			BASS_MIDI_StreamEvents(OMStream, BASS_MIDI_EVENTS_RAW, &dwParam1, 3);
			return;
		}

		if (!((dwParam1 - 0xC0) & 0xE0)) len = 2;
		else if ((dwParam1 & 0xF0) == 0xF0)
		{
			switch (dwParam1 & 0xF)
			{
			case 3:
				len = 2;
				break;
			default:
				// Not supported by OmniMIDI!
				return;
			}
		}

		BASS_MIDI_StreamEvents(OMStream, BASS_MIDI_EVENTS_RAW, &dwParam1, len);
		return;
	}
}

void __inline PrepareForBASSMIDI(DWORD LastRunningStatus, DWORD_PTR dwParam1) {
	if (!(dwParam1 & 0x80))
		dwParam1 = dwParam1 << 8 | LastRunningStatus;

	if (ManagedSettings.FullVelocityMode || ManagedSettings.TransposeValue != 0x7F)
		dwParam1 = ReturnEditedEvent(dwParam1);

	if (ManagedSettings.OverrideNoteLength || ManagedSettings.DelayNoteOff) {
		if (((dwParam1& 0xF0) == 0x90 && ((dwParam1 >> 16) & 0xFF))) {
			BASS_MIDI_EVENT e[2] = { 0, 0 };

			e[0].event = MIDI_EVENT_NOTE;
			e[0].param = MAKEWORD(HIBYTE(LOWORD(dwParam1)), LOBYTE(HIWORD(dwParam1)));
			e[0].chan = dwParam1 & 0xF;
			e[0].pos = 0;
			e[0].tick = 0;

			if (ManagedSettings.OverrideNoteLength)
			{
				e[1].event = MIDI_EVENT_NOTE;
				e[1].param = MAKEWORD(HIBYTE(LOWORD(dwParam1)), 0);
				e[1].chan = dwParam1 & 0xF;
				e[1].pos = BASS_ChannelSeconds2Bytes(OMStream, ((double)ManagedSettings.NoteLengthValue / 1000.0) + (ManagedSettings.DelayNoteOff ? ((double)ManagedSettings.DelayNoteOffValue / 1000.0) : 0));
				e[1].tick = 0;
			}

			BASS_MIDI_StreamEvents(OMStream, BASS_MIDI_EVENTS_STRUCT | BASS_MIDI_EVENTS_TIME, &e, ManagedSettings.OverrideNoteLength ? 2 : 1);
			return;
		}
		else if ((dwParam1 & 0xF0) == 0x80) {
			if (!ManagedSettings.OverrideNoteLength && ManagedSettings.DelayNoteOff) {
				BASS_MIDI_EVENT e;

				e.event = MIDI_EVENT_NOTE;
				e.param = MAKEWORD(HIBYTE(LOWORD(dwParam1)), 0);
				e.chan = dwParam1 & 0xF;
				e.pos = BASS_ChannelSeconds2Bytes(OMStream, ((double)ManagedSettings.DelayNoteOffValue / 1000.0));
				e.tick = 0;

				BASS_MIDI_StreamEvents(OMStream, BASS_MIDI_EVENTS_STRUCT | BASS_MIDI_EVENTS_TIME, &e, 1);
				return;
			}
			else return;
		}
	}

	SendToBASSMIDI(dwParam1);
}

void __inline PrepareForBASSMIDIHyper(DWORD LastRunningStatus, DWORD_PTR dwParam1) {
	if (!(dwParam1 & 0x80))
		dwParam1 = dwParam1 << 8 | LastRunningStatus;

	SendToBASSMIDI(dwParam1);
}

BOOL __inline SendLongToBASSMIDI(LPMIDIHDR IIMidiHdr) {
	// The buffer doesn't exist or isn't ready
	if (!IIMidiHdr || !(IIMidiHdr->dwFlags & MHDR_PREPARED)) return FALSE;

	BOOL rec = (BASS_MIDI_StreamEvents(OMStream, BASS_MIDI_EVENTS_RAW, IIMidiHdr->lpData, IIMidiHdr->dwBufferLength) != -1);
	PrintLongMessageToDebugLog(rec, IIMidiHdr);

	return rec;
}

BOOL __inline SendLongToBASSMIDI(LPSTR MidiHdrData, DWORD MidiHdrDataLen) {
	return (BASS_MIDI_StreamEvents(OMStream, BASS_MIDI_EVENTS_RAW, MidiHdrData, MidiHdrDataLen) != -1);
}

// PBufData and PBufDataHyper have been merged,
// since Running Status is a MANDATORY feature in MIDI drivers
// and can not be skipped
void __inline PBufData(void) {
	DWORD dwParam1 = EVBuffer.Buffer[EVBuffer.ReadHead];
	if (dwParam1 & 0x80)
		LastRunningStatus = (BYTE)dwParam1;

	if (++EVBuffer.ReadHead >= EvBufferSize) EVBuffer.ReadHead = 0;

	_PforBASSMIDI(LastRunningStatus, dwParam1);
}

unsigned int __inline PSmallBufData(void)
{
	auto HeadStart = EVBuffer.ReadHead;
	auto HeadPos = HeadStart; //fast volatile
	DWORD dwParam1 = EVBuffer.Buffer[HeadPos];

	if (!~dwParam1) return 1;

	for (;;)
	{
		EVBuffer.Buffer[HeadPos] = ~0;

		if (++HeadPos >= EvBufferSize)
			HeadPos = 0;

		EVBuffer.ReadHead = HeadPos;

		if (~dwParam1)
		{
			if (dwParam1 & 0x80)
				LastRunningStatus = (BYTE)dwParam1;

			_PforBASSMIDI(LastRunningStatus, dwParam1);
		}
		else return 0;

		if (HeadPos == HeadStart) return 0;

		dwParam1 = EVBuffer.Buffer[HeadPos];
		if (!~dwParam1) return 0;
	}
}

DWORD __inline PlayBufferedData(void) {
	if (ManagedSettings.IgnoreAllEvents) return 1;
	
	if (EvBufferSize >= SMALLBUFFER)
	{
		if (!BufferCheck()) return 1;

		do PBufData();
		while (BufferCheck());

		return 0;
	}
	else return PSmallBufData();
}

DWORD __inline PlayBufferedDataHyper(void) {
	if (!BufferCheck()) return 1;

	do PBufData();
	while (EVBuffer.ReadHead != EVBuffer.WriteHead);

	return 0;
}

DWORD __inline PlayBufferedDataChunk(void) {
	if (ManagedSettings.IgnoreAllEvents || !BufferCheck()) return 1;

	if (EvBufferSize >= SMALLBUFFER)
	{
		ULONGLONG whe = EVBuffer.WriteHead;
		do PBufData();
		while (EVBuffer.ReadHead != whe);

		return 0;
	}
	else return PSmallBufData();
}

DWORD __inline PlayBufferedDataChunkHyper(void) {
	if (!BufferCheck()) return 1;

	ULONGLONG whe = EVBuffer.WriteHead;
	do PBufData();
	while (EVBuffer.ReadHead != whe);

	return 0;
}

MMRESULT __inline ParseData(DWORD_PTR dwParam1) {
	// Some checks
	if (CheckIfEventIsToIgnore(dwParam1))
		return MMSYSERR_NOERROR;

	// The buffer is not ready yet
	if (!EVBuffer.Buffer) return DebugResult("ParseData", MIDIERR_NOTREADY, "The events buffer isn't ready yet!");

	// Prepare the event in the buffer
	// LockForWriting(&EPThreadsL);

	auto NextWriteHead = EVBuffer.WriteHead + 1;
	if (NextWriteHead >= EvBufferSize) NextWriteHead = 0;

	if (NextWriteHead != EVBuffer.ReadHead)
	{
		EVBuffer.Buffer[EVBuffer.WriteHead] = dwParam1;
		EVBuffer.WriteHead = NextWriteHead;
	}
	else if (!ManagedSettings.DontMissNotes)
	{
		EVBuffer.Buffer[EVBuffer.WriteHead] = dwParam1;
		//do NOT advance the WriteHead for a more sensical note skipping
		//EVBuffer.WriteHead = NextWriteHead;
	}
	else
	{
		if (NextWriteHead >= EvBufferSize)
			NextWriteHead = EVBuffer.ReadHead; //guaranteed to be always in bounds

		if(EvBufferSize >= SMALLBUFFER)
			while (NextWriteHead == EVBuffer.ReadHead) /*do sleep or something*/;
		else
		{
			volatile DWORD* dwVolatileEvent = EVBuffer.Buffer; // + NextWriteHead;
			/* EvBuffer resize resets both heads to 0, so checking
				anything but the first event in the buffer will softlock completely*/
			while (~*dwVolatileEvent) /*do sleep or something*/;
		}

		EVBuffer.Buffer[EVBuffer.WriteHead] = dwParam1;
		EVBuffer.WriteHead = NextWriteHead;
	}

	PrintEventToDebugLog(dwParam1);

	// UnlockForWriting(&EPThreadsL);

	// Go!
	return MMSYSERR_NOERROR;
}

MMRESULT __inline ParseDataHyper(DWORD_PTR dwParam1)
{
	// LockForWriting(&EPThreadsL);

	auto NextWriteHead = EVBuffer.WriteHead + 1;
	if (NextWriteHead >= EvBufferSize) NextWriteHead = 0;

	EVBuffer.Buffer[EVBuffer.WriteHead] = dwParam1;

	if (NextWriteHead != EVBuffer.ReadHead)
		EVBuffer.WriteHead = NextWriteHead; //skip notes properly

	// UnlockForWriting(&EPThreadsL);

	// Go!
	return MMSYSERR_NOERROR;
}