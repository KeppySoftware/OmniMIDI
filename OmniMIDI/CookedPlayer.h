/*
OmniMIDI MIDI_IO_COOKED player (Ported from the mmidi project by Sono)
*/
#pragma once

// Cooked player struct
struct CookedPlayer {
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

DWORD WINAPI CookedPlayerThread(CookedPlayer* Player) {
	float qpcfreq = 10.0F;
	BOOL(WINAPI*QPC)(QWORD*) = (BOOL(WINAPI*)(QWORD*))GetProcAddress(GetModuleHandle(L"ntdll"), "RtlQueryPerformanceFrequency");
	if (QPC)
	{
		QWORD freq = 10;
		if (QPC(&freq) && freq)
			qpcfreq = 10000000.0F / freq;
		QPC = (BOOL(WINAPI*)(QWORD*))GetProcAddress(GetModuleHandle(L"ntdll"), "RtlQueryPerformanceCounter");
	}


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

	QPC(&tickdiff);
	tickdiff *= qpcfreq;

	while (!Player->Stopped)
	{
		if (Player->Paused || !Player->MIDIHeaderQueue)
		{
			PrintMessageToDebugLog("CookedPlayerThread", "Waiting for unpause and/or header...");
			while (Player->Paused || !Player->MIDIHeaderQueue)
			{
				ticker = (QWORD)-(INT64)maxdelay;
				NtDelayExecution(TRUE, (INT64*)&ticker);
				QPC(&tickdiff); //reset timer
				tickdiff *= qpcfreq;
				deltasleep = 0; //reset drift
				oldsleep = 0;
			}
			PrintMessageToDebugLog("CookedPlayerThread", "Playback started!");
			continue;
		}

		if (delaytick)
		{
			QPC(&ticker);
			ticker *= qpcfreq;
			DWORD tdiff = (DWORD)(ticker - tickdiff); //calculate elapsed time
			tickdiff = ticker;
			int delt = (int)(tdiff - oldsleep); //calculate drift
			deltasleep += delt; //accumlate drift

			sleeptime = (delaytick * Player->TempoMulti); //TODO: can overflow
			//sleeptime *= speedcontrol;
			oldsleep = sleeptime;

			Player->TimeAccumulator += sleeptime;

			if (deltasleep > 0) //can underflow, don't speed up if we pushed too hard
				sleeptime -= deltasleep; //adjust for time drift

			if (0) //if(sleeptime > maxdelay)
			{ //yes, this is very coarse, but the adaptive timer will keep it in sync
				sleeptime = maxdelay;
				DWORD acc = maxdelay / Player->TempoMulti; //time to ticks
				if (!acc) acc = 1;

				if (sleeptime <= 0) //overloaded
				{
					if (deltasleep < adaption); //trick MSVC
					else deltasleep = adaption; //don't overpush
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
				if (sleeptime <= 0) //overloaded
				{
					if (!(deltasleep < adaption)) //trick MSVC
						deltasleep = adaption; //don't overpush
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
			Player->Lock.LockForWriting();
			Player->MIDIHeaderQueue = hdr->lpNext;
			Player->Lock.UnlockForWriting();
			continue;
		}

		while (!Player->Paused)
		{
			if (hdr->dwOffset >= hdr->dwBytesRecorded)
			{
				PrintMessageToDebugLog("CookedPlayerThread", "MIDIHDR played, sending callback");
				Player->Lock.LockForWriting();
				hdr->dwFlags |= MHDR_DONE;
				hdr->dwFlags &= ~MHDR_INQUEUE;
				Player->MIDIHeaderQueue = hdr->lpNext;
				Player->Lock.UnlockForWriting();
				DriverCallback(OMCallback, OMFlags, (HDRVR)OMHMIDI, MOM_DONE, OMInstance, (DWORD_PTR)hdr, 0);
				hdr->dwOffset = 0;
				break;
			}

			MIDIEVENT* evt = (MIDIEVENT*)(hdr->lpData + hdr->dwOffset);

			if (barrier)
			{
				barrier = FALSE;
				delaytick = evt->dwDeltaTime;
				if (delaytick)
					continue;
			}

			//reset barrier
			barrier = TRUE;

			if (evt->dwEvent & MEVT_F_CALLBACK)
			{
				PrintMessageToDebugLog("CookedPlayerThread", "dwEvent requested callback");
				DriverCallback(OMCallback, OMFlags, (HDRVR)OMHMIDI, MOM_DONE, OMInstance, (DWORD_PTR)hdr, 0);
			}

			BYTE evid = (evt->dwEvent >> 24) & 0xBF;
			if (evid == MEVT_SHORTMSG)
			{ //favor ShortMSG for performance
				//no need to mask away the high byte because it's ignored
				_StoBASSMIDI(0, evt->dwEvent);
			}
			else if (evid == MEVT_TEMPO)
			{ //tempo change spam
				Player->Tempo = evt->dwEvent & 0xFFFFFF;
				Player->TempoMulti = (DWORD)((Player->Tempo * 10) / Player->TimeDiv);
				char asd[256];
				sprintf(asd, "MEVT_TEMPO DEBUG tempo=%i timediv=%i tempomulti=%i\n",
					Player->Tempo, Player->TimeDiv, Player->TempoMulti);
				PrintMessageToDebugLog("CookedPlayerThread", asd);
			}
			else if (evid == MEVT_COMMENT)
			{ //quickly skip over comments
				//TODO
			}
			else if (evid == MEVT_LONGMSG)
			{
				BASS_MIDI_StreamEvents(OMStream, BASS_MIDI_EVENTS_RAW, evt->dwParms, evt->dwEvent & 0xFFFFFF);
			}
			/*else if(evid != MEVT_NOP && evid != MEVT_VERSION)
			{
				CrashMessage("CookedPlayerThread | evid not NOP");
			}*/

			if (evt->dwEvent & MEVT_F_LONG)
			{
				DWORD acc = ((evt->dwEvent & 0xFFFFFF) + 3) & ~3; //PAD
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