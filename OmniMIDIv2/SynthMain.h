/*
OmniMIDI v15+ (Rewrite) for Windows NT

This file contains the required code to run the driver under Windows 7 SP1 and later.
This file is useful only if you want to compile the driver under Windows, it's not needed for Linux/macOS porting.
*/

#ifndef _SYNTHMAIN_H
#define _SYNTHMAIN_H

#pragma once

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

#define fv2fn(f)			(#f)
#define ImpFunc(f)			{(void**)&##f, #f}
#define EvBuf				SonoBuf_t

#include <Windows.h>
#include <bass.h>
#include <bassmidi.h>
#include <thread>
#include <atomic>
#include <vector>
#include <ShlObj_core.h>
#include <strsafe.h>
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

	struct SonoBufEv {
		DWORD Event;
		DWORD Align[15];
	};

	// Sono's buffer
	class SonoBuf_t {
	private:
		SonoBufEv* Buffer;
		size_t Size { 0 };

		// Written by reader thread
		alignas(64) volatile size_t	ReadHead { 0 };
		size_t WriteHeadCached { 0 };

		// Written by writer thread
		alignas(64) volatile size_t	WriteHead { 0 };
		size_t ReadHeadCached { 0 };

	#ifdef _DEBUG
		size_t EventsSent { 0 };
		size_t EventsSkipped { 0 };
	#endif

	public:
		SonoBuf_t(size_t ReqSize) {
			Buffer = new SonoBufEv[ReqSize];
			Size = ReqSize;
		}

		~SonoBuf_t() {
			Size = 0;
			delete[] Buffer;
		}

		bool Push(unsigned int ev) {
			size_t NextWriteHead = WriteHead + 1;
			if (NextWriteHead >= Size) NextWriteHead = 0;

			Buffer[WriteHead].Event = ev;

	#ifdef _DEBUG
			EventsSent++;
	#endif

			if (NextWriteHead == ReadHeadCached) {
				ReadHeadCached = ReadHead;
				if (NextWriteHead == ReadHeadCached) {
	#ifdef _DEBUG
					EventsSkipped++;
	#endif
					return false;
				}
			}
			WriteHead = NextWriteHead;

			return true;
		}

		bool Pop(unsigned int& ev) {
			if (ReadHead == WriteHeadCached) {
				WriteHeadCached = WriteHead;
				if (ReadHead == WriteHeadCached) {
					return false;
				}
			}

			ev = Buffer[ReadHead].Event;

			if (++ReadHead >= Size) ReadHead = 0;

			return true;
		}
	};

	// EVBuffer
	class EvBuf_t {
	private:
		std::vector<unsigned int> Buffer{};
		alignas(64) size_t Size { 0 };
		alignas(64) std::atomic<size_t> ReadHead{ 0 };
		alignas(64) size_t WriteHeadCached { 0 };
		alignas(64) std::atomic<size_t> WriteHead{ 0 };
		alignas(64) size_t ReadHeadCached { 0 };

	public:
		EvBuf_t(size_t ReqSize) : Buffer(ReqSize, 0) {
			Size = ReqSize;
		}

		~EvBuf_t() {
			Size = 0;
			Buffer.clear();
		}

		bool Push(unsigned int ev) {
			if (Size == 0)
				return false;

			auto const tWriteHead = WriteHead.load(std::memory_order_relaxed);
			auto tNextWritePos = tWriteHead + 1;
			if (tNextWritePos == Size) {
				tNextWritePos = 0;
			}
			if (tNextWritePos == ReadHeadCached) {
				ReadHeadCached = ReadHead.load(std::memory_order_acquire);
				if (tNextWritePos == ReadHeadCached) {
					return false;
				}
			}
			Buffer[tWriteHead] = ev;
			WriteHead.store(tNextWritePos, std::memory_order_release);
			return true;
		}

		bool Pop(unsigned int& ev) {
			if (Size == 0)
				return false;

			auto const tReadHead = ReadHead.load(std::memory_order_relaxed);
			if (tReadHead == WriteHeadCached) {
				WriteHeadCached = WriteHead.load(std::memory_order_acquire);
				if (ReadHead == WriteHeadCached) {
					return false;
				}
			}
			ev = Buffer[tReadHead];
			auto tNextReadPos = tReadHead + 1;
			if (tNextReadPos == Size) {
				tNextReadPos = 0;
			}
			ReadHead.store(tNextReadPos, std::memory_order_release);
			return true;
		}
	};

	class SynthModule {
	private:
		ErrorSystem::WinErr SynErr;

		Lib BAudLib;
		Lib BMidLib;

		std::thread _AudThread;
		std::thread _EvtThread;
		EvBuf* Events;

		signed long long onenano = -1;
		unsigned int (WINAPI* NanoSleep)(unsigned char, signed long long*);
		unsigned int AudioStream;
		BASS_MIDI_FONTEX* SoundFonts;

		char LastRunningStatus = 0x0;
		const unsigned char XGReset[9] = { 0xF0, 0x43, 0x10, 0x4C, 0x00, 0x00, 0x7E, 0x00, 0xF7 };

		bool LoadLib(Lib* Target);
		bool UnloadLib(Lib* Target);
		bool LoadFuncs();
		bool UnloadFuncs();
		
		void AudioThread();
		void EventsThread();
		bool ProcessEvBuf();

	public:
		bool IsSynthInitialized() { return (BAudLib.Initialized && BMidLib.Initialized); }
		bool LoadSynthModule();
		bool UnloadSynthModule();
		bool StartSynthModule();
		bool StopSynthModule();
		bool ChangeSetting(unsigned int setting, void* var, size_t size);

		// Event handling system
		SynthResult PlayShortEvent(unsigned int ev);
		SynthResult UPlayShortEvent(unsigned int ev);
		SynthResult PlayLongEvent(char* ev, unsigned int size);
		SynthResult UPlayLongEvent(char* ev, unsigned int size);
	};
}

#endif