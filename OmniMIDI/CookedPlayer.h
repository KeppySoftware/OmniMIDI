/*
OmniMIDI MIDI_IO_COOKED player (Ported from the mmidi project by Sono)
*/
#pragma once

// Cooked player struct
struct CookedPlayer
{
	LPMIDIHDR MIDIHeaderQueue;			// MIDIHDR buffer
	BOOL Paused;						// Is the player paused?
	BOOL Stopped;						// Is the player stopped?
	DWORD Tempo;						// Player tempo
	DWORD TimeDiv;						// Player time division
	DWORD TempoMulti;					// Player time multiplier
	DWORD TimeAccumulator;				// ?
	DWORD ByteAccumulator;				// ?
	DWORD TickAccumulator;				// ?
	LightweightLock Lock;				// LockSystem
	DWORD_PTR dwInstance;
};

DWORD WINAPI CookedPlayerThread(CookedPlayer* Player)
{
	QWORD ticker = 0;
	QWORD tickdiff = 0;
	int sleeptime = 0;
	int oldsleep = 0;
	int deltasleep = 0;

	DWORD delaytick = 0;
	BOOL barrier = TRUE; //this is horrible

	const DWORD maxdelay = 10e4; //adjust responsiveness here
	const DWORD adaption = 1e5; //adaptive timer nice time >:]

	PrintMessageToDebugLog("CookedPlayerThread", "Thread is alive!");

	NtQuerySystemTime(&tickdiff);

	while (!Player->Stopped)
	{
		if (Player->Paused || !Player->MIDIHeaderQueue)
		{
			PrintMessageToDebugLog("CookedPlayerThread", "Waiting for unpause and/or header...");
			while (Player->Paused || !Player->MIDIHeaderQueue)
			{
				ticker = (QWORD)-(INT64)maxdelay;
				NtDelayExecution(TRUE, (INT64*)&ticker);
				NtQuerySystemTime(&tickdiff); //reset timer
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
					if (!(deltasleep < adaption))
						deltasleep = adaption;				// Don't overpush
				}
				else
				{
					INT64 usl = -((INT64)sleeptime);
					NtDelayExecution(FALSE, &usl);
				}

				delaytick -= acc;
				if (delaytick >> 31)
					PrintMessageToDebugLog("CookedPlayerThread", "Warning: delaytick integer underflow");
				Player->TickAccumulator += acc;

				continue;
			}
			else
			{
				if (sleeptime <= 0)							// Overloaded
				{
					if (!(deltasleep < adaption))
						deltasleep = adaption;				// Don't overpush
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
            CrashMessage("CookedPlayerThread | MHDR_DONE is invalid here");
			Player->Lock.LockForWriting();

			Player->MIDIHeaderQueue = hdr->lpNext;

			Player->Lock.UnlockForWriting();
			continue;
		}

		while (!Player->Paused)
		{
			if (hdr->dwOffset >= hdr->dwBytesRecorded)
			{
                Player->Lock.LockForWriting();

                hdr->dwFlags |= MHDR_DONE;
                hdr->dwFlags &= ~MHDR_INQUEUE;

                Player->MIDIHeaderQueue = hdr->lpNext;

                Player->Lock.UnlockForWriting();

                DriverCallback(OMCallback, OMFlags, (HDRVR)OMHMIDI, MOM_DONE, OMInstance, (DWORD_PTR)hdr, 0);
                
				hdr->dwOffset = 0;
                hdr = hdr->lpNext;

				if (hdr) continue;
				else break;
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
				PrintMessageToDebugLog("CookedPlayerThread", "dwEvent requested callback");
				DriverCallback(OMCallback, OMFlags, (HDRVR)OMHMIDI, MOM_DONE, OMInstance, (DWORD_PTR)hdr, 0);
			}

			/*
			if(evid != MEVT_NOP && evid != MEVT_VERSION)
			{
				CrashMessage("CookedPlayerThread | evid not NOP");
			}
			*/

			BYTE evid = (evt->dwEvent >> 24) & 0xBF;
			switch (evid) {
			case MEVT_SHORTMSG:
				_StoBASSMIDI(0, evt->dwEvent);
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
	PrintMessageToDebugLog("CookedPlayerThread", "Closing CookedPlayer thread...");
	CloseHandle(CookedThread.ThreadHandle);
	CookedThread.ThreadHandle = NULL;
	return 0;
}