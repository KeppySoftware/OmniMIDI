
/*

	OmniMIDI v15+ (Rewrite) for Windows NT

	This file contains the required code to run the driver under Windows 7 SP1 and later.
	This file is useful only if you want to compile the driver under Windows, it's not needed for Linux/macOS porting.

*/

#ifndef _BASSSYNTH_H
#define _BASSSYNTH_H

#include <bass\bass.h>
#include <bass\bassmidi.h>
#include <bass\bass_vst.h>
#include <bass\bassasio.h>
#include <bass\basswasapi.h>
#include <future>
#include "EvBuf_t.h"
#include "ErrSys.h"
#include "SynthMain.h"

// Engines
#define INVALID_ENGINE		-1
#define BASS_INTERNAL		0
#define WASAPI				1
#define XAUDIO_2_9			2
#define ASIO				3
#define KERNEL_STREAM		4

#define EvBuf				EvBuf_t
#define JSONGetVal(f)		{ #f, f }
#define JSONSetVal(t, f)	f = JsonData[#f].is_null() ? f : (t)JsonData[#f]

#define SettingsManagerCase(choice, get, type, setting, var, size) \
	case choice: \
		if (size != sizeof(type)) return false; \
		if (get) *(type*)var = setting; \
		else setting = *(type*)var; \
		break;

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
		ImpFunc(BASS_MIDI_StreamGetChannel),

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

		// BASSVST
		ImpFunc(BASS_VST_ChannelSetDSP),

		// BASSASIO
		ImpFunc(BASS_ASIO_CheckRate),
		ImpFunc(BASS_ASIO_ChannelEnable),
		ImpFunc(BASS_ASIO_ChannelEnableBASS),
		ImpFunc(BASS_ASIO_ChannelEnableMirror),
		ImpFunc(BASS_ASIO_ChannelGetLevel),
		ImpFunc(BASS_ASIO_ChannelJoin),
		ImpFunc(BASS_ASIO_ChannelSetFormat),
		ImpFunc(BASS_ASIO_ChannelSetRate),
		ImpFunc(BASS_ASIO_ControlPanel),
		ImpFunc(BASS_ASIO_ErrorGetCode),
		ImpFunc(BASS_ASIO_Free),
		ImpFunc(BASS_ASIO_GetDevice),
		ImpFunc(BASS_ASIO_GetDeviceInfo),
		ImpFunc(BASS_ASIO_GetLatency),
		ImpFunc(BASS_ASIO_GetRate),
		ImpFunc(BASS_ASIO_Init),
		ImpFunc(BASS_ASIO_SetRate),
		ImpFunc(BASS_ASIO_Start),
		ImpFunc(BASS_ASIO_Stop),
		ImpFunc(BASS_ASIO_IsStarted)
	};
}

namespace OmniMIDI {
	class BASSSettings : public SynthSettings {
	private:
		ErrorSystem::WinErr SetErr;

	public:
		// Global settings
		unsigned int EvBufSize = 32768;
		unsigned int AudioFrequency = 48000;
		unsigned int MaxVoices = 1000;
		unsigned int MaxCPU = 95;
		int AudioEngine = WASAPI;
		bool LoudMax = false;

		// WASAPI
		float WASAPIBuf = 32.0f;

		// ASIO
		std::string ASIODevice = "None";

		BASSSettings() {
			// When you initialize Settings(), load OM's own settings by default
			WinUtils::SysPath Utils;
			wchar_t OMPath[MAX_PATH] = { 0 };

			if (Utils.GetFolderPath(FOLDERID_Profile, OMPath, sizeof(OMPath))) {
				swprintf_s(OMPath, L"%s\\OmniMIDI\\settings.json\0", OMPath);
				LoadJSON(OMPath);
			}
		}

		void CreateJSON(wchar_t* Path) {
			std::fstream st;
			st.open(Path, std::fstream::out | std::ofstream::trunc);
			if (st.is_open()) {
				nlohmann::json defset = {
					{ "BASSSynth", {
						JSONGetVal(ASIODevice),
						JSONGetVal(AudioEngine),
						JSONGetVal(AudioFrequency),
						JSONGetVal(EvBufSize),
						JSONGetVal(LoudMax),
						JSONGetVal(MaxCPU),
						JSONGetVal(MaxVoices),
						JSONGetVal(WASAPIBuf)
					}}
				};

				std::string dump = defset.dump(1);
				st.write(dump.c_str(), dump.length());
				st.close();
			}
		}

		// Here you can load your own JSON, it will be tied to ChangeSetting()
		void LoadJSON(wchar_t* Path) {
			std::fstream st;
			st.open(Path, std::fstream::in);

			if (st.is_open()) {
				try {
					// Read the JSON data from there
					auto json = nlohmann::json::parse(st, nullptr, false, true);

					if (json != nullptr) {
						auto JsonData = json["BASSSynth"];

						if (!(JsonData == nullptr)) {
							JSONSetVal(bool, LoudMax);
							JSONSetVal(float, WASAPIBuf);
							JSONSetVal(int, AudioEngine);
							JSONSetVal(std::string, ASIODevice);
							JSONSetVal(unsigned int, AudioFrequency);
							JSONSetVal(unsigned int, EvBufSize);
							JSONSetVal(unsigned int, MaxCPU);
							JSONSetVal(unsigned int, MaxVoices);
						}
					}
					else throw nlohmann::json::type_error::create(667, "json structure is not valid", nullptr);
				}
				catch (nlohmann::json::type_error ex) {
					st.close();
					LOG(SetErr, "The JSON is corrupted or malformed!\n\nnlohmann::json says: %s", ex.what());
					CreateJSON(Path);
					return;
				}
				st.close();
			}
		}
	};

	class BASSSynth : public SynthModule {
	private:
		ErrorSystem::WinErr SynErr;

		Lib* BAudLib = nullptr;
		Lib* BMidLib = nullptr;
		Lib* BWasLib = nullptr;
		Lib* BVstLib = nullptr;
		Lib* BAsiLib = nullptr;

		std::jthread _AudThread;
		std::jthread _EvtThread;
		EvBuf* Events;

		signed long long onenano = -1;
		unsigned int (WINAPI* NanoSleep)(unsigned char, signed long long*) = nullptr;
		unsigned int AudioStream = 0;
		std::vector<BASS_MIDI_FONTEX> SoundFonts;

		BASSSettings* Settings = nullptr;
		bool Fail = false;
		bool RestartSynth = false;
		char LastRunningStatus = 0x0;

		// Threads func
		void AudioThread();
		void EventsThread();

		// BASS system
		bool LoadFuncs();
		bool UnloadFuncs();
		void StreamSettings(bool restart);
		bool ProcessEvBuf();

	public:
		bool LoadSynthModule();
		bool UnloadSynthModule();
		bool StartSynthModule();
		bool StopSynthModule();
		bool SettingsManager(unsigned int setting, bool get, void* var, size_t size);
		bool IsSynthInitialized() { return (AudioStream != 0 && Fail != true); }

		// Event handling system
		SynthResult PlayShortEvent(unsigned int ev);
		SynthResult UPlayShortEvent(unsigned int ev);

		SynthResult PlayLongEvent(char* ev, unsigned int size);
		SynthResult UPlayLongEvent(char* ev, unsigned int size);

		int TalkToSynthDirectly(unsigned int evt, unsigned int chan, unsigned int param);
	};
}

#endif