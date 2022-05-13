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

BOOL __inline SendShortMIDIFeedback(DWORD dwParam1) {
	if (OMFeedback) {
		return (MMmidiOutShortMsg((HMIDIOUT)OMFeedback, dwParam1) == MMSYSERR_NOERROR);
	}

	return FALSE;
}

BOOL __inline SendLongMIDIFeedback(LPMIDIHDR MIDIHDR, UINT Size) {
	if (OMFeedback) {
		return (MMmidiOutLongMsg((HMIDIOUT)OMFeedback, MIDIHDR, Size) == MMSYSERR_NOERROR);
	}

	return FALSE;
}

void __inline SendToBASSMIDI(DWORD dwParam1) {
	DWORD len = 3;

	// PrintEventToDebugLog(dwParam1);

	switch (GETCMD(dwParam1)) {
	case MIDI_NOTEON:
		_BMSE(OMStream, dwParam1 & 0xF, MIDI_EVENT_NOTE, dwParam1 >> 8);
		return;
	case MIDI_NOTEOFF:
		_BMSE(OMStream, dwParam1 & 0xF, MIDI_EVENT_NOTE, (BYTE)(dwParam1 >> 8));
		return;
	case MIDI_POLYAFTER:
		_BMSE(OMStream, dwParam1 & 0xF, MIDI_EVENT_KEYPRES, dwParam1 >> 8);
		return;
	case MIDI_PROGCHAN:
		_BMSE(OMStream, dwParam1 & 0xF, MIDI_EVENT_PROGRAM, (BYTE)(dwParam1 >> 8));
		return;
	case MIDI_CHANAFTER:
		_BMSE(OMStream, dwParam1 & 0xF, MIDI_EVENT_CHANPRES, (BYTE)(dwParam1 >> 8));
		return;
	default:
		if (!(dwParam1 - 0x80 & 0xC0))
		{
			_BMSEs(OMStream, BASS_MIDI_EVENTS_RAW, &dwParam1, 3);
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

		_BMSEs(OMStream, BMSEsRAWFlags, &dwParam1, len);
		return;
	}
}

void __inline PrepareForBASSMIDI(DWORD LastRunningStatus, DWORD dwParam1) {
	BASS_MIDI_EVENT Evs[2];

	if (!(dwParam1 & 0x80))
		dwParam1 = dwParam1 << 8 | LastRunningStatus;

	if (ManagedSettings.FullVelocityMode || ManagedSettings.TransposeValue != 0x7F)
		dwParam1 = ReturnEditedEvent(dwParam1);

	SendShortMIDIFeedback(dwParam1);

	if (ManagedSettings.OverrideNoteLength || ManagedSettings.DelayNoteOff) {
		if (((dwParam1 & 0xF0) == MIDI_NOTEON && ((dwParam1 >> 16) & 0xFF))) {

			Evs[0] = { MIDI_EVENT_NOTE, dwParam1 >> 8, dwParam1 & 0xF, 0, 0 };

			if (ManagedSettings.OverrideNoteLength)
				Evs[1] = { MIDI_EVENT_NOTE, (BYTE)(dwParam1 >> 8), dwParam1 & 0xF, FNoteLengthValue, 0 };

			BASS_MIDI_StreamEvents(OMStream, BMSEsFlags, &Evs, ManagedSettings.OverrideNoteLength ? 2 : 1);

			return;
		}
		else if ((dwParam1 & 0xF0) == MIDI_NOTEOFF) {
			if (!ManagedSettings.OverrideNoteLength && ManagedSettings.DelayNoteOff) {
				Evs[0] = { MIDI_EVENT_NOTE, (BYTE)(dwParam1 >> 8), dwParam1 & 0xF, FDelayNoteOff, 0 };

				BASS_MIDI_StreamEvents(OMStream, BMSEsFlags, &Evs, 1);
			}

			return;
		}
	}

	SendToBASSMIDI(dwParam1);
}

void __inline PrepareForBASSMIDIHyper(DWORD LastRunningStatus, DWORD dwParam1) {
	if (!(dwParam1 & 0x80))
		dwParam1 = dwParam1 << 8 | LastRunningStatus;

	SendToBASSMIDI(dwParam1);
}

void __inline SendLongToBASSMIDI(LPMIDIHDR IIMidiHdr) {
	// If dwBytesRecorded is 0, use dwBufferLength instead
	// Thank you MSDN for not telling me about it, 
	// and thanks to Windows Media Player for doing this...
	DWORD FLen = IIMidiHdr->dwBytesRecorded < 1 ? IIMidiHdr->dwBufferLength : IIMidiHdr->dwBytesRecorded;

	_BMSEs(OMStream, BASS_MIDI_EVENTS_RAW, IIMidiHdr->lpData, FLen);
	PrintLongMessageToDebugLog(IIMidiHdr);
}

// PBufData and PBufDataHyper have been merged,
// since Running Status is a MANDATORY feature in MIDI drivers
// and can not be skipped
void __inline PBufData(void) {
	DWORD dwParam1 = EVBuffer.Buffer[EVBuffer.ReadHead].Event;
	if (dwParam1 & 0x80)
		LastRunningStatus = (BYTE)dwParam1;

	if (++EVBuffer.ReadHead >= EVBuffer.BufSize) EVBuffer.ReadHead = 0;

	_PforBASSMIDI(LastRunningStatus, dwParam1);
}

void __inline PSmallBufData(void)
{
	auto HeadStart = EVBuffer.ReadHead;
	auto HeadPos = HeadStart; //fast volatile
	DWORD dwParam1 = EVBuffer.Buffer[HeadPos].Event;

	if (!~dwParam1) return;

	for (;;)
	{
		EVBuffer.Buffer[HeadPos].Event = ~0;

		if (++HeadPos >= EVBuffer.BufSize)
			HeadPos = 0;

		EVBuffer.ReadHead = HeadPos;

		if (~dwParam1)
		{
			if (dwParam1 & 0x80)
				LastRunningStatus = (BYTE)dwParam1;

			_PforBASSMIDI(LastRunningStatus, dwParam1);
		}
		else return;

		if (HeadPos == HeadStart) return;

		dwParam1 = EVBuffer.Buffer[HeadPos].Event;
		if (!~dwParam1) return;
	}
}

void __inline PlayBufferedData(void) {
	if (ManagedSettings.IgnoreAllEvents)
		return;
	
	if (EVBuffer.BufSize >= SMALLBUFFER) {
		if (!BufferCheck()) {
			_FWAIT;
			return;
		}

		do PBufData();
		while (BufferCheck());
	}
	else PSmallBufData();
}

void __inline PlayBufferedDataHyper(void) {
	if (!BufferCheck()) {
		_FWAIT;
		return;
	}

	do PBufData();
	while (EVBuffer.ReadHead != EVBuffer.WriteHead);
}

void __inline PlayBufferedDataChunk(void) {
	if (ManagedSettings.IgnoreAllEvents || !BufferCheck()) return;

	if (EVBuffer.BufSize >= SMALLBUFFER)
	{
		ULONGLONG whe = EVBuffer.WriteHead;
		do PBufData();
		while (EVBuffer.ReadHead != whe);
	}
	else PSmallBufData();
}

void __inline PlayBufferedDataChunkHyper(void) {
	if (!BufferCheck()) return;

	ULONGLONG whe = EVBuffer.WriteHead;
	do PBufData();
	while (EVBuffer.ReadHead != whe);
}

void __inline ParseData(DWORD_PTR dwParam1) {
	// Some checks
	if (CheckIfEventIsToIgnore(dwParam1) || !EVBuffer.Buffer)
		return;

	// Prepare the event in the buffer
	// LockForWriting(&EPThreadsL);

	auto NextWriteHead = EVBuffer.WriteHead + 1;
	if (NextWriteHead >= EVBuffer.BufSize) NextWriteHead = 0;

	if (NextWriteHead != EVBuffer.ReadHead)
	{
		EVBuffer.Buffer[EVBuffer.WriteHead].Event = dwParam1;
		EVBuffer.WriteHead = NextWriteHead;
	}
	else if (!ManagedSettings.DontMissNotes)
	{
		EVBuffer.Buffer[EVBuffer.WriteHead].Event = dwParam1;
		//do NOT advance the WriteHead for a more sensical note skipping
		//EVBuffer.WriteHead = NextWriteHead;
	}
	else
	{
		if (NextWriteHead >= EVBuffer.BufSize)
			NextWriteHead = EVBuffer.ReadHead; //guaranteed to be always in bounds

		if(EVBuffer.BufSize >= SMALLBUFFER)
			while (NextWriteHead == EVBuffer.ReadHead) _FWAIT;

		EVBuffer.Buffer[EVBuffer.WriteHead].Event = dwParam1;
		EVBuffer.WriteHead = NextWriteHead;
	}

	PrintEventToDebugLog(dwParam1);

	// UnlockForWriting(&EPThreadsL);
}

void __inline ParseDataHyper(DWORD_PTR dwParam1)
{
	// LockForWriting(&EPThreadsL);

	ULONGLONG NextWriteHead = EVBuffer.WriteHead + 1;
	if (NextWriteHead >= EVBuffer.BufSize) NextWriteHead = 0;

	EVBuffer.Buffer[EVBuffer.WriteHead].Event = dwParam1;

	if (NextWriteHead != EVBuffer.ReadHead)
		EVBuffer.WriteHead = NextWriteHead; //skip notes properly

	// UnlockForWriting(&EPThreadsL);
}