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
#include "EvBuf_t.h"
#include "ErrSys.h"
#include "Utils.h"
#include <nlohmann\json.hpp>

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

	struct Settings {
		// Global settings
		unsigned int MaxVoices = 1000;
		unsigned int MaxCPU = 100;
		int AudioEngine = WASAPI;

		// WASAPI
		float WASAPIBuf = 32.0f;

		Settings() {
			// When you initialize Settings(), load OM's own settings by default
			WinUtils::SysPath Utils;
			wchar_t OMPath[MAX_PATH] = { 0 };

			if (Utils.GetFolderPath(FOLDERID_Profile, OMPath, sizeof(OMPath))) {
				swprintf_s(OMPath, L"%s\\OmniMIDI\\settings.json\0", OMPath);
				LoadJSON(OMPath);
			}
		}

		// Here you can load your own JSON, it will be tied to ChangeSetting()
		void LoadJSON(wchar_t* Path) {
			std::fstream st;
			st.open(Path);

			if (st.is_open()) {
				// Read the JSON data from there
				auto json = nlohmann::json::parse(st, nullptr, false, true);

				if (json != nullptr) {
					auto JsonData = json["BASSSynth"];

					if (!(JsonData == nullptr)) {
						MaxVoices = (unsigned int)JsonData["MaxVoices"];
						MaxCPU = (unsigned int)JsonData["MaxCPU"];
						AudioEngine = (int)JsonData["AudioEngine"];
						WASAPIBuf = (float)JsonData["WASAPIBuffer"];
					}
				}
			}
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

		signed long long onenano = -1;
		unsigned int (WINAPI* NanoSleep)(unsigned char, signed long long*) = nullptr;
		unsigned int AudioStream = 0;
		std::vector<BASS_MIDI_FONTEX> SoundFonts;

		Settings* SynthSettings = nullptr;
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