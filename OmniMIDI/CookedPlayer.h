/*
OmniMIDI MIDI_IO_COOKED player (Ported from the mmidi project by Sono)
*/
#pragma once

// Cooked player struct
static BOOL CookedPlayerHasToGo = FALSE;
struct CookedPlayer
{
	LPMIDIHDR MIDIHeaderQueue;			// MIDIHDR buffer
	BOOL Paused;						// Is the player paused?
	DWORD Tempo;						// Player tempo
	DWORD TimeDiv;						// Player time division
	DWORD TempoMulti;					// Player time multiplier
	DWORD TimeAccumulator;				// ?
	DWORD ByteAccumulator;				// ?
	DWORD TickAccumulator;				// ?
	LockSystem Lock;					// LockSystem
	DWORD_PTR dwInstance;
};

VOID KillOldCookedPlayer(DWORD_PTR dwUser) {
	if (IsThisThreadActive(CookedThread.ThreadHandle))
	{
		CookedPlayerHasToGo = TRUE;
		CloseThread(CookedThread.ThreadHandle);
		CookedPlayerHasToGo = FALSE;
		if (dwUser) free((*(CookedPlayer * *)dwUser));
	}
}

DWORD WINAPI CookedPlayerSystem(CookedPlayer* Player)
{
	QWORD ticker = 0;
	QWORD tickdiff = 0;
	int sleeptime = 0;
	int oldsleep = 0;
	int deltasleep = 0;

	DWORD delaytick = 0;
	BOOL barrier = TRUE;			// This is horrible :s

	const DWORD maxdelay = 10e4;	// Adjust responsiveness here
	const DWORD adaption = 1e5;		// Adaptive timer nice time >:3

	PrintMessageToDebugLog("CookedPlayerSystem", "Thread is alive!");

	NtQuerySystemTime(&tickdiff);

	while (!CookedPlayerHasToGo)
	{
		if (Player->Paused || !Player->MIDIHeaderQueue)
		{
			PrintMessageToDebugLog("CookedPlayerSystem", "Waiting for unpause and/or header...");
			while (Player->Paused || !Player->MIDIHeaderQueue)
			{
				ticker = (QWORD)-(INT64)maxdelay;
				NtDelayExecution(TRUE, (INT64*)&ticker);
				NtQuerySystemTime(&tickdiff);				// Reset timer
				deltasleep = 0;								// Reset drift
				oldsleep = 0;
				if (CookedPlayerHasToGo) break;
			}
			PrintMessageToDebugLog("CookedPlayerThread", "Playback started!");
			continue;
		}

		if (delaytick)
		{
			NtQuerySystemTime(&ticker);
			DWORD tdiff = (DWORD)(ticker - tickdiff);		// Calculate elapsed time
			tickdiff = ticker;
			int delt = (int)(tdiff - oldsleep);				// Calculate drift
			deltasleep += delt;								// Accumlate drift

			sleeptime = (delaytick * Player->TempoMulti);	// TODO: can overflow
			//sleeptime *= speedcontrol;
			oldsleep = sleeptime;

			Player->TimeAccumulator += sleeptime;

			if (deltasleep > 0)								// Can underflow, don't speed up if we pushed too hard
				sleeptime -= deltasleep;					// Adjust for time drift

			if (0) //if(sleeptime > maxdelay)
			{ // Yes, this is very coarse, but the adaptive timer will keep it in sync
				sleeptime = maxdelay;
				DWORD acc = maxdelay / Player->TempoMulti;	// Time to ticks
				if (!acc) acc = 1;

				if (sleeptime <= 0)							// Overloaded
				{
					if (deltasleep < adaption);
					else deltasleep = adaption;				// Don't overpush
				}
				else
				{
					INT64 usl = -((INT64)sleeptime);
					NtDelayExecution(FALSE, &usl);
				}

				delaytick -= acc;
				if (delaytick >> 31)
					PrintMessageToDebugLog("CookedPlayerSystem", "Warning: DelayTick integer underflow!");
				Player->TickAccumulator += acc;

				continue;
			}
			else
			{
				if (sleeptime <= 0)							// Overloaded
				{
					if (deltasleep < adaption);
					else deltasleep = adaption;				// Don't overpush
				}
				else
				{
					INT64 usl = -((INT64)sleeptime);
					NtDelayExecution(FALSE, &usl);
				}

				Player->TickAccumulator += delaytick;
				delaytick = 0;

				continue;
			}
		}

		LPMIDIHDR hdr = Player->MIDIHeaderQueue;

		if (hdr->dwFlags & MHDR_DONE)
		{
			LockForWriting(&Player->Lock);
			Player->MIDIHeaderQueue = hdr->lpNext;
			UnlockForWriting(&Player->Lock);
			continue;
		}

		while (!Player->Paused)
		{
			if (hdr->dwOffset >= hdr->dwBytesRecorded)
			{
				LockForWriting(&Player->Lock);
				hdr->dwFlags |= MHDR_DONE;
				hdr->dwFlags &= ~MHDR_INQUEUE;
				LPMIDIHDR nexthdr = hdr->lpNext;
				UnlockForWriting(&Player->Lock);

				Player->MIDIHeaderQueue = nexthdr;

				CustomCallback((HMIDIOUT)OMHMIDI, MM_MOM_DONE, WMMCI, (DWORD_PTR)hdr, 0);

				hdr->dwOffset = 0;
				hdr = nexthdr;

				break;
			}

			MIDIEVENT* evt = (MIDIEVENT*)(hdr->lpData + hdr->dwOffset);

			if (barrier)
			{
				barrier = FALSE;
				delaytick = evt->dwDeltaTime;

				if (delaytick) break;
			}

			// Reset barrier
			barrier = TRUE;

			if (evt->dwEvent & MEVT_F_CALLBACK)
			{
				PrintMessageToDebugLog("CookedPlayerSystem", "Reached MEVT_F_CALLBACK! Let's warn the app about it.");
				CustomCallback((HMIDIOUT)OMHMIDI, MM_MOM_POSITIONCB, WMMCI, (DWORD_PTR)hdr, 0);
			}

			BYTE evid = (evt->dwEvent >> 24) & 0xBF;

			/*
			if(evid != MEVT_NOP && evid != MEVT_VERSION)
			{
				CrashMessage("CookedPlayerThread | evid not NOP");
			}
			*/

			switch (evid) {
			case MEVT_SHORTMSG:
				_PrsData(evt->dwEvent);
				break;
			case MEVT_LONGMSG:
				BASS_MIDI_StreamEvents(OMStream, BASS_MIDI_EVENTS_RAW, evt->dwParms, evt->dwEvent & 0xFFFFFF);
				break;
			case MEVT_TEMPO:
				Player->Tempo = evt->dwEvent & 0xFFFFFF;
				Player->TempoMulti = (DWORD)((Player->Tempo * 10) / Player->TimeDiv);
				break;
			default:
				break;
			}

			if (evt->dwEvent & MEVT_F_LONG)
			{
				DWORD acc = ((evt->dwEvent & 0xFFFFFF) + 3) & ~3;	// PAD
				Player->ByteAccumulator += acc;
				hdr->dwOffset += acc;
			}

			Player->ByteAccumulator += 0xC;
			hdr->dwOffset += 0xC;
		}
	}

	// Close the thread
	PrintMessageToDebugLog("CookedPlayerSystem", "Closing CookedPlayer thread...");
	CloseHandle(CookedThread.ThreadHandle);
	CookedThread.ThreadHandle = NULL;
	return 0;
}