/*

	OmniMIDI v15+ (Rewrite) for Windows NT

	This file contains the required code to run the driver under Windows 7 SP1 and later.
	This file is useful only if you want to compile the driver under Windows, it's not needed for Linux/macOS porting.

*/

#ifndef _OFLUIDSYNTH_H
#define _OFLUIDSYNTH_H

#ifdef _WIN32
#include <Windows.h>
#endif

#include <fluidsynth.h>
#include "NtFuncs.h"
#include "EvBuf_t.h"
#include "SynthMain.h"

namespace OmniMIDI {
	class FluidSettings : public SynthSettings {
	private:
#pragma clang diagnostic push
#pragma clang diagnostic ignored "-Wunused-private-field"
		ErrorSystem::WinErr SetErr;
#pragma clang diagnostic pop

	public:
		// Global settings
		unsigned int EvBufSize = 32768;
		unsigned int AudioFrequency = 48000;
		unsigned int MaxVoices = 1024;
		unsigned int PeriodSize = 64;
		unsigned int Periods = 2;
		unsigned int ThreadsCount = 1;
		unsigned int MinimumNoteLength = 10;
		double OverflowVolume = 10000.0;
		double OverflowPercussion = 10000.0;
		double OverflowReleased = -10000.0;
		double OverflowImportant = 0.0;
		std::string Driver = "wasapi";
		std::string SampleFormat = "float";

		FluidSettings() {
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
					{ "FluidSynth", {
						JSONGetVal(AudioFrequency),
						JSONGetVal(EvBufSize),
						JSONGetVal(MaxVoices),
						JSONGetVal(PeriodSize),
						JSONGetVal(Periods),
						JSONGetVal(ThreadsCount),
						JSONGetVal(MinimumNoteLength),
						JSONGetVal(OverflowVolume),
						JSONGetVal(OverflowPercussion),
						JSONGetVal(OverflowReleased),
						JSONGetVal(OverflowImportant),
						JSONGetVal(Driver),
						JSONGetVal(SampleFormat)
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
						auto& JsonData = json["FluidSynth"];

						if (!(JsonData == nullptr)) {
							JSONSetVal(unsigned int, AudioFrequency);
							JSONSetVal(unsigned int, EvBufSize);
							JSONSetVal(unsigned int, MaxVoices);
							JSONSetVal(unsigned int, PeriodSize);
							JSONSetVal(unsigned int, Periods);
							JSONSetVal(unsigned int, ThreadsCount);
							JSONSetVal(unsigned int, MinimumNoteLength);
							JSONSetVal(double, OverflowVolume);
							JSONSetVal(double, OverflowPercussion);
							JSONSetVal(double, OverflowReleased);
							JSONSetVal(double, OverflowImportant);
							JSONSetVal(std::string, Driver);
							JSONSetVal(std::string, SampleFormat);
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

	class FluidSynth : public SynthModule {
	private:
		ErrorSystem::WinErr SynErr;
		NT::Funcs NTFuncs;

		Lib* FluiLib = nullptr;

		LibImport FLibImports[24] = {
			// BASS
			ImpFunc(new_fluid_synth),
			ImpFunc(new_fluid_settings),
			ImpFunc(delete_fluid_synth),
			ImpFunc(delete_fluid_settings),
			ImpFunc(fluid_synth_all_notes_off),
			ImpFunc(fluid_synth_all_sounds_off),
			ImpFunc(fluid_synth_noteoff),
			ImpFunc(fluid_synth_noteon),
			ImpFunc(fluid_synth_cc),
			ImpFunc(fluid_synth_channel_pressure),
			ImpFunc(fluid_synth_key_pressure),
			ImpFunc(fluid_synth_pitch_bend),
			ImpFunc(fluid_synth_pitch_wheel_sens),
			ImpFunc(fluid_synth_program_change),
			ImpFunc(fluid_synth_program_reset),
			ImpFunc(fluid_synth_program_select),
			ImpFunc(fluid_synth_sysex),
			ImpFunc(fluid_synth_system_reset),
			ImpFunc(fluid_synth_sfload),
			ImpFunc(fluid_settings_setint),
			ImpFunc(fluid_settings_setnum),
			ImpFunc(fluid_settings_setstr),
			ImpFunc(new_fluid_audio_driver),
			ImpFunc(delete_fluid_audio_driver)
		};

		std::jthread _EvtThread;
		EvBuf* Events;

		FluidSettings* Settings = nullptr;
		fluid_settings_t* fSet = nullptr;
		fluid_synth_t* fSyn = nullptr;
		fluid_audio_driver_t* fDrv = nullptr;
		std::vector<int> SoundFonts;

		char LastRunningStatus = 0x0;

		void EventsThread();
		bool ProcessEvBuf();

	public:
		bool LoadSynthModule();
		bool UnloadSynthModule();
		bool StartSynthModule();
		bool StopSynthModule();
		bool SettingsManager(unsigned int setting, bool get, void* var, size_t size) { return false; }
		bool IsSynthInitialized() { return (fDrv != nullptr); }
		int SynthID() { return 0x6F704EC6; }

		// Event handling system
		SynthResult PlayShortEvent(unsigned int ev);
		SynthResult UPlayShortEvent(unsigned int ev);

		SynthResult PlayLongEvent(char* ev, unsigned int size);
		SynthResult UPlayLongEvent(char* ev, unsigned int size);

		// Not supported in FluidSynth
		int TalkToSynthDirectly(unsigned int evt, unsigned int chan, unsigned int param) { return 0; }
	};
}

#endif