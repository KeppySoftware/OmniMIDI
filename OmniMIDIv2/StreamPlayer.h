#pragma once

#ifdef _WIN32

#ifndef _COOKEDPLAYER_H
#define _COOKEDPLAYER_H

#define MAX_WAIT 10000

#include "WDMEntry.h"
#include <mmeapi.h>

namespace OmniMIDI {
	class StreamPlayer {
	private:
		ErrorSystem::WinErr StrmErr;
		NT::Funcs NTFuncs;
		OmniMIDI::SynthModule* synthModule;
		WinDriver::DriverCallback* drvCallback;

		MIDIHDR* mhdrQueue = 0;
		bool paused = true;
		bool goToBed = false;

		// tempo is microseconds per quarter note,
		// while timeDiv is the time division (lower 15 bits)
		unsigned int tempo = 500000;
		unsigned int ticksPerQN = 0x30;
		unsigned int bpm = 60000000 / tempo;
		unsigned long long timeAcc = 0;
		unsigned long long byteAcc = 0;
		unsigned long long tickAcc = 0;

		std::jthread _CooThread;
		void PlayerThread();

	public:
		void Start() { paused = false; }
		void Stop() { paused = true; }

		bool AddToQueue(MIDIHDR* hdr);
		bool EmptyQueue();
		bool IsQueueEmpty() { return (!mhdrQueue); }

		void SetTempo(unsigned int ntempo);
		void SetTicksPerQN(unsigned int ntimeDiv);
		unsigned int GetTempo() { return tempo; }
		unsigned int GetTicksPerQN() { return ticksPerQN; }
		void GetPosition(MMTIME* mmtime);

		StreamPlayer(OmniMIDI::SynthModule* sptr, WinDriver::DriverCallback* dptr);
		~StreamPlayer();
	};
}

#endif
#endif