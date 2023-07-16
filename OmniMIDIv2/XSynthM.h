/*

	OmniMIDI v15+ (Rewrite) for Windows NT

	This file contains the required code to run the driver under Windows 7 SP1 and later.
	This file is useful only if you want to compile the driver under Windows, it's not needed for Linux/macOS porting.

*/

#ifndef _XSYNTHM_H
#define _XSYNTHM_H

// Not supported on ARM Thumb-2!

#ifdef _WIN32
#include <Windows.h>
#endif

#ifndef _M_ARM

#include <xsynth.h>
#include "NtFuncs.h"
#include "EvBuf_t.h"
#include "SynthMain.h"

namespace OmniMIDI {
	class XSynthSettings : public SynthSettings {
	private:
#pragma clang diagnostic push
#pragma clang diagnostic ignored "-Wunused-private-field"
		ErrorSystem::WinErr SetErr;
#pragma clang diagnostic pop

	public:
		double BufSize = 5.0;

		XSynthSettings() {
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
					{ "XSynth", {
						JSONGetVal(BufSize)
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
						auto& JsonData = json["XSynth"];

						if (!(JsonData == nullptr)) {
							JSONSetVal(double, BufSize);
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

	class XSynth : public SynthModule {
	private:
		ErrorSystem::WinErr SynErr;

		Lib* XLib = nullptr;

		LibImport FLibImports[5] = {
			// BASS
			ImpFunc(StartModule),
			ImpFunc(StopModule),
			ImpFunc(SendData),
			ImpFunc(LoadSoundFont),
			ImpFunc(ResetModule)
		};

		XSynthSettings* Settings = nullptr;
		bool Running = false;

	public:
		bool LoadSynthModule();
		bool UnloadSynthModule();
		bool StartSynthModule();
		bool StopSynthModule();
		bool SettingsManager(unsigned int setting, bool get, void* var, size_t size) { return false; }
		bool IsSynthInitialized() { return 0; }
		int SynthID() { return 0x9AF3812A; }

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