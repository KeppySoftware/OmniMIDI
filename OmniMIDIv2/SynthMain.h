/*
OmniMIDI v15+ (Rewrite) for Windows NT

This file contains the required code to run the driver under Windows 7 SP1 and later.
This file is useful only if you want to compile the driver under Windows, it's not needed for Linux/macOS porting.
*/

#ifndef _SYNTHMAIN_H
#define _SYNTHMAIN_H

#pragma once

// Uncomment this if you want stats to be shown once the synth is closed
// #define _STATSDEV

typedef	unsigned int SynthResult;

#define CHKLRS(f)			(f & 0x80)
#define GETSTATUS(f)		(f & 0xFF)
#define GETCMD(f)			(f & 0xF0)
#define GETCHANNEL(f)		(f & 0xF)
#define GETFP(f)			((f >> 8) & 0xFF)
#define GETSP(f)			((f >> 16) & 0xFF)

//
#define MIDI_NOTEOFF		0x80
#define MIDI_NOTEON			0x90
#define MIDI_POLYAFTER		0xA0
#define MIDI_CMC			0xB0
#define MIDI_PROGCHAN		0xC0
#define MIDI_CHANAFTER		0xD0
#define MIDI_PITCHWHEEL		0xE0

// TBD
#define MIDI_SYSEXBEG		0xF0
#define MIDI_SYSEXEND		0xF7
#define MIDI_COOKPLPLAY		0xFA
#define MIDI_COOKPLCONT		0xFB
#define MIDI_COOKPLSTOP		0xFC
#define MIDI_SENSING		0xFE
#define MIDI_SYSRESET		0xFF

// ERRORS
#define SYNTH_OK			0x00
#define SYNTH_NOTINIT		0x01
#define SYNTH_INITERR		0x02
#define SYNTH_INVALPARAM	0x03

// Engines
#define INVALID_ENGINE		-1
#define BASS_INTERNAL		0
#define WASAPI				1
#define XAUDIO_2_9			2
#define ASIO				3
#define KERNEL_STREAM		4

#define fv2fn(f)			(#f)
#define ImpFunc(f)			{(void**)&##f, #f}
#define EvBuf				EvBuf_t

#include <Windows.h>
#include <ShlObj_core.h>
#include <strsafe.h>
#include <bass.h>
#include <basswasapi.h>
#include <bassmidi.h>
#include <thread>
#include <atomic>
#include <algorithm>
#include <iostream>
#include <fstream>
#include <sstream>
#include <vector>
#include <codecvt>
#include <locale>
#include <nlohmann\json.hpp>
#include "ErrSys.h"

namespace {
	struct LibImport
	{
		void** ptr;
		const char* name;
	}

	LibImports[] = {
		// BASS
		ImpFunc(BASS_ChannelFlags),
		ImpFunc(BASS_ChannelGetAttribute),
		ImpFunc(BASS_ChannelGetData),
		ImpFunc(BASS_ChannelGetLevelEx),
		ImpFunc(BASS_ChannelIsActive),
		ImpFunc(BASS_ChannelPlay),
		ImpFunc(BASS_ChannelRemoveFX),
		ImpFunc(BASS_ChannelSeconds2Bytes),
		ImpFunc(BASS_ChannelSetAttribute),
		ImpFunc(BASS_ChannelSetDevice),
		ImpFunc(BASS_ChannelSetFX),
		ImpFunc(BASS_ChannelStop),
		ImpFunc(BASS_ChannelUpdate),
		ImpFunc(BASS_ErrorGetCode),
		ImpFunc(BASS_FXSetParameters),
		ImpFunc(BASS_Free),
		ImpFunc(BASS_GetDevice),
		ImpFunc(BASS_GetDeviceInfo),
		ImpFunc(BASS_GetInfo),
		ImpFunc(BASS_Init),
		ImpFunc(BASS_PluginFree),
		ImpFunc(BASS_SetConfig),
		ImpFunc(BASS_Stop),
		ImpFunc(BASS_StreamFree),

		// BASSWASAPI
		ImpFunc(BASS_WASAPI_Init),
		ImpFunc(BASS_WASAPI_Free),
		ImpFunc(BASS_WASAPI_IsStarted),
		ImpFunc(BASS_WASAPI_Start),
		ImpFunc(BASS_WASAPI_Stop),
		ImpFunc(BASS_WASAPI_GetDeviceInfo),
		ImpFunc(BASS_WASAPI_GetInfo),
		ImpFunc(BASS_WASAPI_GetDevice),
		ImpFunc(BASS_WASAPI_GetLevelEx),

		// BASSMIDI
		ImpFunc(BASS_MIDI_FontFree),
		ImpFunc(BASS_MIDI_FontInit),
		ImpFunc(BASS_MIDI_FontLoad),
		ImpFunc(BASS_MIDI_StreamCreate),
		ImpFunc(BASS_MIDI_StreamEvent),
		ImpFunc(BASS_MIDI_StreamEvents),
		ImpFunc(BASS_MIDI_StreamGetEvent),
		ImpFunc(BASS_MIDI_StreamLoadSamples),
		ImpFunc(BASS_MIDI_StreamSetFonts),
		ImpFunc(BASS_MIDI_StreamGetChannel)
	};
}

namespace OmniMIDI {
	struct Lib {
		const wchar_t* Path;
		HMODULE Library = nullptr;
		bool Initialized = false;
		bool LoadFailed = false;
		bool AppSelfHosted = false;
	};

	struct Ev {
		DWORD Event;
		DWORD Align[15];
	};

	// Sono's buffer
	class EvBuf_t {
	private:
		Ev* Buffer;
		size_t Size{ 0 };

		// Written by reader thread
		alignas(64) volatile size_t	ReadHead { 0 };
		alignas(64) size_t WriteHeadCached{ 0 };

		// Written by writer thread
		alignas(64) volatile size_t	WriteHead { 0 };
		alignas(64) size_t ReadHeadCached{ 0 };

#ifdef _STATSDEV
		size_t EventsSent{ 0 };
		size_t EventsSkipped{ 0 };
#endif

	public:
		EvBuf_t(size_t ReqSize) {
			Buffer = new Ev[ReqSize];
			Size = ReqSize;
		}

		~EvBuf_t() {
			Size = 0;
			ReadHead = 0;
			WriteHead = 0;
			ReadHeadCached = 0;
			WriteHeadCached = 0;
#ifdef _STATSDEV
			EventsSent = 0;
			EventsSkipped = 0;
#endif
			delete[] Buffer;
		}

		void GetStats() {
#ifdef _STATSDEV
			char asdf[1024] = {};
			snprintf(asdf, sizeof(asdf), "%llu of %llu events skipped", EventsSkipped, EventsSent);
			MessageBoxA(NULL, asdf, "", 0);
#else
			// Absolutely nothing.
#endif
		}

		bool Push(unsigned int ev) {
			size_t LocalWriteHead = WriteHead;
			size_t NextWriteHead = LocalWriteHead + 1;

			if (NextWriteHead >= Size)
				NextWriteHead = 0;

			if (NextWriteHead == ReadHeadCached)
			{
				ReadHeadCached = ReadHead;
				if (NextWriteHead == ReadHeadCached) {
#ifdef _STATSDEV
					EventsSkipped++;
#endif
					return false;
				}
			}

			Buffer[LocalWriteHead].Event = ev;
			WriteHead = NextWriteHead;

			return true;
		}

		bool Pop(unsigned int& ev) {
			size_t LocalReadHead = ReadHead;
			if (LocalReadHead == WriteHeadCached)
			{
				WriteHeadCached = WriteHead;
				if (LocalReadHead == WriteHeadCached)
					return false;
			}

			size_t NextReadHead = LocalReadHead + 1;
			if (NextReadHead >= Size)
				NextReadHead = 0;

			ev = Buffer[LocalReadHead].Event;
			ReadHead = NextReadHead;

			return true;
		}

		void Peek(unsigned int& ev) {
			if (ReadHead == WriteHead) {
				return;
			}

			ev = Buffer[ReadHead].Event;
		}
	};

	class SynthModule {
	private:
		ErrorSystem::WinErr SynErr;

		Lib BAudLib = { .Path = L"BASS" };
		Lib BMidLib = { .Path = L"BASSMIDI" };
		Lib BWasLib = { .Path = L"BASSWASAPI" };

		std::thread _AudThread;
		std::thread _EvtThread;
		EvBuf* Events;

		nlohmann::json JsonData;
		signed long long onenano = -1;
		unsigned int (WINAPI* NanoSleep)(unsigned char, signed long long*);
		unsigned int AudioStream;
		std::vector<BASS_MIDI_FONTEX> SoundFonts;

		int AudioEngine = WASAPI;
		char LastRunningStatus = 0x0;

		bool LoadLib(Lib* Target);
		bool UnloadLib(Lib* Target);
		bool LoadFuncs();
		bool UnloadFuncs();
		
		void AudioThread();
		void EventsThread();
		bool ProcessEvBuf();

	public:
		bool LoadSynthModule();
		bool UnloadSynthModule();
		bool StartSynthModule();
		bool StopSynthModule();
		bool ChangeSetting(unsigned int setting, void* var, size_t size);
		bool IsSynthInitialized() { return (AudioStream != 0); }

		// Event handling system
		SynthResult PlayShortEvent(unsigned int ev);
		SynthResult UPlayShortEvent(unsigned int ev);

		SynthResult PlayLongEvent(char* ev, unsigned int size);
		SynthResult UPlayLongEvent(char* ev, unsigned int size);

		int TalkToBASSMIDI(unsigned int evt, unsigned int chan, unsigned int param);
	};
}

#endif