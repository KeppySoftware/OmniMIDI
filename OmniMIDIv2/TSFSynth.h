/*

	OmniMIDI v15+ (Rewrite) for Windows NT

	This file contains the required code to run the driver under Windows 7 SP1 and later.
	This file is useful only if you want to compile the driver under Windows, it's not needed for Linux/macOS porting.

*/

#ifndef _TSFSYNTH_H
#define _TSFSYNTH_H

// Not supported on ARM Thumb-2!

#ifdef _WIN32
#include <Windows.h>
#endif

#ifndef _M_ARM

#include <tsf/minisdl_audio.h>
#include <tsf.h>
#include "NtFuncs.h"
#include "EvBuf_t.h"
#include "SynthMain.h"

namespace OmniMIDI {
	class TinySFSettings : public SynthSettings {
	private:
#pragma clang diagnostic push
#pragma clang diagnostic ignored "-Wunused-private-field"
		ErrorSystem::WinErr SetErr;
#pragma clang diagnostic pop

	public:
		unsigned int EvBufSize = 32768;
		unsigned int AudioFrequency = 48000;
		unsigned int Samples = 4096;

		TinySFSettings() {
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
					{ "TinySFSynth", {
						JSONGetVal(EvBufSize),
						JSONGetVal(AudioFrequency),
						JSONGetVal(Samples)
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
						auto& JsonData = json["TinySFSynth"];

						if (!(JsonData == nullptr)) {
							JSONSetVal(unsigned int, EvBufSize);
							JSONSetVal(unsigned int, AudioFrequency);
							JSONSetVal(unsigned int, Samples);
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

	class TinySFSynth : public SynthModule {
	private:
		ErrorSystem::WinErr SynErr;

		Lib* SDLLib = nullptr;

		LibImport SLibImports[9] = {
			ImpFunc(SDL_AudioInit),
			ImpFunc(SDL_AudioQuit),
			ImpFunc(SDL_CreateMutex),
			ImpFunc(SDL_DestroyMutex),
			ImpFunc(SDL_OpenAudioDevice),
			ImpFunc(SDL_PauseAudioDevice),
			ImpFunc(SDL_LockMutex),
			ImpFunc(SDL_UnlockMutex),
			ImpFunc(SDL_CloseAudioDevice)
		};

		std::jthread _EvtThread;
		EvBuf* Events = nullptr;
		char LastRunningStatus = 0x0;

		// Holds the global instance pointer
		tsf* g_TinySoundFont = nullptr;

		// A Mutex so we don't call note_on/note_off while rendering audio samples
		SDL_AudioDeviceID dev = 0;
		SDL_mutex* g_Mutex = nullptr;

		TinySFSettings* Settings = nullptr;
		bool Running = false;

		static void cCallback(void* data, Uint8* stream, int len) {
			static_cast<TinySFSynth*>(data)->Callback(reinterpret_cast<float*>(stream), len);
		}

		void Callback(float* stream, int len) {
			// Render the audio samples in float format
			int SampleCount = (len / (2 * sizeof(float))); //2 output channels
			SDL_LockMutex(g_Mutex); //get exclusive lock
			tsf_render_float(g_TinySoundFont, stream, SampleCount, 0);
			SDL_UnlockMutex(g_Mutex);
		}

		void EventsThread();
		bool ProcessEvBuf();

	public:
		bool LoadSynthModule();
		bool UnloadSynthModule();
		bool StartSynthModule();
		bool StopSynthModule();
		bool SettingsManager(unsigned int setting, bool get, void* var, size_t size) { return false; }
		bool IsSynthInitialized() { return (g_TinySoundFont != nullptr); }
		int SynthID() { return 0xA0A0A0A0; }

		// Event handling system
		SynthResult PlayShortEvent(unsigned int ev);
		SynthResult UPlayShortEvent(unsigned int ev);

		SynthResult PlayLongEvent(char* ev, unsigned int size);
		SynthResult UPlayLongEvent(char* ev, unsigned int size);

		// Not supported in XSynth
		int TalkToSynthDirectly(unsigned int evt, unsigned int chan, unsigned int param) { return 0; }
	};
}

#endif

#endif