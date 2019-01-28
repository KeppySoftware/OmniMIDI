/*
OmniMIDI buffer system
Some code has been optimized by Sono (MarcusD), the old one has been commented out
*/
#pragma once

#define SETVELOCITY(evento, newvelocity) evento = (DWORD(evento) & 0xFF00FFFF) | ((DWORD(newvelocity) & 0xFF) << 16)
#define SETNOTE(evento, newnote) evento = (DWORD(evento) & 0xFFFF00FF) | ((DWORD(newnote) & 0xFF) << 8)
#define SETSTATUS(evento, newstatus) evento = (DWORD(evento) & 0xFFFFFF00) | (DWORD(newstatus) & 0xFF)

int BufferCheck(void) {
	return ManagedSettings.DontMissNotes ? EVBuffer.EventsCount : (EVBuffer.ReadHead != EVBuffer.WriteHead) ? ~0 : 0;
}

int BufferCheckHyper(void) {
	return (EVBuffer.ReadHead != EVBuffer.WriteHead) ? ~0 : 0;
}

void SendToBASSMIDI(DWORD LastRunningStatus, DWORD dwParam1) {
	if (!(dwParam1 & 0x80))
		dwParam1 = dwParam1 << 8 | LastRunningStatus;

	if (ManagedSettings.OverrideNoteLength || ManagedSettings.DelayNoteOff) {
		if ((((dwParam1 & 0xFF) & 0xF0) == 0x90 && ((dwParam1 >> 16) & 0xFF))) {
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
		else if (((dwParam1 & 0xFF) & 0xF0) == 0x80) {
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

	BASS_MIDI_StreamEvents(
		OMStream, BASS_MIDI_EVENTS_RAW,
		&dwParam1, ((dwParam1 & 0xF0) >= 0xF8 && (dwParam1 & 0xF0) <= 0xFF) ? 1 : (((dwParam1 & 0xF0) == 0xC0 || (dwParam1 & 0xF0) == 0xD0) ? 2 : 3)
	);
	
	// PrintEventToConsole(FOREGROUND_GREEN, dwParam1, FALSE, "Parsed normal MIDI event.");
}

void SendToBASSMIDIHyper(DWORD LastRunningStatus, DWORD dwParam1) {
	DWORD len = 3;

	switch (GETSTATUS(dwParam1)) {
	case MIDI_NOTEON:
		BASS_MIDI_StreamEvent(OMStream, dwParam1 & 0xF, MIDI_EVENT_NOTE, dwParam1 >> 8);
		return;
	case MIDI_NOTEOFF:
		BASS_MIDI_StreamEvent(OMStream, dwParam1 & 0xF, MIDI_EVENT_NOTE, (BYTE)(dwParam1 >> 8));
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
				len = 1;
				break;
			}
		}

		BASS_MIDI_StreamEvents(OMStream, BASS_MIDI_EVENTS_RAW, &dwParam1, len);
		return;
	}
}

void SendLongToBASSMIDI(MIDIHDR* IIMidiHdr) {
	// The buffer doesn't exist or isn't ready
	if (!IIMidiHdr && !(IIMidiHdr->dwFlags & MHDR_PREPARED)) return;								

	PrintSysExMessageToDebugLog(
		BASS_MIDI_StreamEvents(OMStream, BASS_MIDI_EVENTS_RAW, IIMidiHdr->lpData, IIMidiHdr->dwBufferLength),
		IIMidiHdr);
	// PrintEventToConsole(FOREGROUND_GREEN, 0, TRUE, "Parsed SysEx MIDI event.");
}

void __inline PBufData(void) {
	EnterProtectedZone(&EVBufferLock);

	DWORD dwParam1 = EVBuffer.Buffer[EVBuffer.ReadHead];
	if (dwParam1 & 0x80) LastRunningStatus = (BYTE)dwParam1;
	DWORD TempLRS = LastRunningStatus;

	if (++EVBuffer.ReadHead >= EvBufferSize) EVBuffer.ReadHead = 0;
	LeaveProtectedZone(&EVBufferLock);

	_StoBASSMIDI(TempLRS, dwParam1);
}

void __inline PBufDataHyper(void) {
	EnterProtectedZone(&EVBufferLock);
	DWORD dwParam1 = EVBuffer.Buffer[EVBuffer.ReadHead];
	if (++EVBuffer.ReadHead >= EvBufferSize) EVBuffer.ReadHead = 0;
	LeaveProtectedZone(&EVBufferLock);

	_StoBASSMIDI(0, dwParam1);
}

DWORD __inline PlayBufferedData(void) {
	if (ManagedSettings.IgnoreAllEvents || !BufferCheck()) return 1;

	do PBufData();
	while (ManagedSettings.DontMissNotes ? InterlockedDecrement64(&EVBuffer.EventsCount) : ((EVBuffer.ReadHead != EVBuffer.WriteHead) ? ~0 : 0));

	return 0;
}

DWORD __inline PlayBufferedDataHyper(void) {
	if (!BufferCheckHyper()) return 1;

	do PBufDataHyper();
	while ((EVBuffer.ReadHead != EVBuffer.WriteHead) ? ~0 : 0);

	return 0;
}

DWORD __inline PlayBufferedDataChunk(void) {
	if (ManagedSettings.IgnoreAllEvents || !BufferCheck()) return 1;

	ULONGLONG whe = EVBuffer.WriteHead;
	do PBufData();
	while (ManagedSettings.DontMissNotes ? InterlockedDecrement64(&EVBuffer.EventsCount) : ((EVBuffer.ReadHead != whe) ? ~0 : 0));
}

DWORD __inline PlayBufferedDataChunkHyper(void) {
	if (!BufferCheckHyper()) return 1;

	ULONGLONG whe = EVBuffer.WriteHead;
	do PBufDataHyper();
	while ((EVBuffer.ReadHead != whe) ? ~0 : 0);
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
	if (CheckIfEventIsToIgnore(dwParam1))
		return MMSYSERR_NOERROR;

	if (ManagedSettings.FullVelocityMode || ManagedSettings.TransposeValue != 0x7F)
		dwParam1 = ReturnEditedEvent(dwParam1);

	if (!EVBuffer.Buffer) return DebugResult(MIDIERR_NOTREADY, TRUE);

	// Prepare the event in the buffer
	EnterProtectedZone(&EVBufferLock);

	EVBuffer.Buffer[EVBuffer.WriteHead] = dwParam1;
	if (++EVBuffer.WriteHead >= EvBufferSize) EVBuffer.WriteHead = 0;

	LeaveProtectedZone(&EVBufferLock);

	// Some checks
	if (ManagedSettings.DontMissNotes && InterlockedIncrement64(&EVBuffer.EventsCount) >= EvBufferSize) do { /* Absolutely nothing */ } while (EVBuffer.EventsCount >= EvBufferSize);

	// Haha everything is fine
	return MMSYSERR_NOERROR;
}

MMRESULT ParseDataHyper(UINT uMsg, DWORD_PTR dwParam1, DWORD_PTR dwParam2) {
	// Prepare the event in the buffer
	EnterProtectedZone(&EVBufferLock);

	EVBuffer.Buffer[EVBuffer.WriteHead] = dwParam1;
	if (++EVBuffer.WriteHead >= EvBufferSize) EVBuffer.WriteHead = 0;

	LeaveProtectedZone(&EVBufferLock);

	// Haha everything is fine
	return MMSYSERR_NOERROR;
}