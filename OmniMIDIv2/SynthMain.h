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
#define GETCMD(f)			(f & 0xF0)
#define GETCHANNEL(f)		(f & 0xF)
#define GETSTATUS(f)		(f & 0xFF)
#define GETSP(f)			(f >> 16)
#define GETFP(f)			(f >> 8)

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

#include <Windows.h>
#include <bass.h>
#include <bassmidi.h>
#include <ShlObj_core.h>
#include <strsafe.h>
#include "ErrSys.h"

using namespace std;

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
		bool Initialized = true;
		bool LoadFailed = false;
		bool AppSelfHosted = false;
	};

	class SynthModule {
	private:
		ErrorSystem::WinErr SynErr;

		Lib BAudLib;
		Lib BMidLib;

		unsigned int AudioStream;
		BASS_MIDI_FONTEX* SoundFonts;

		char LastRunningStatus = 0x0;

		void LoadFunc(Lib* target, void* func, const char* funcName);
		bool LoadLib(Lib* Target);
		bool UnloadLib(Lib* Target);
		bool LoadFuncs();
		bool UnloadFuncs();

	public:
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