/*

	OmniMIDI v15+ (Rewrite) for Windows NT

	This file contains the required code to run the driver under Windows 7 SP1 and later.
	This file is useful only if you want to compile the driver under Windows, it's not needed for Linux/macOS porting.

*/

#include "WDMEntry.h"

static OmniMIDI::WDMSettings* KDMSettings = nullptr;
static OmniMIDI::SynthModule* SynthModule = nullptr;
static ErrorSystem::WinErr KDMErr;

extern "C" {
	__declspec(dllexport)
	int KDMAPI IsKDMAPIAvailable() {
		return 1;
	}

	__declspec(dllexport)
	int KDMAPI InitializeKDMAPIStream() {
		if (!SynthModule)
			SynthModule = new OmniMIDI::SynthModule;

		if (SynthModule->SynthID() == EMPTYMODULE) {
			if (KDMSettings != nullptr)
				delete KDMSettings;

			delete SynthModule;
			KDMSettings = new OmniMIDI::WDMSettings();
			switch (KDMSettings->Renderer) {
			case BASSMIDI:
				SynthModule = new OmniMIDI::BASSSynth;
				break;
			case FLUIDSYNTH:
				SynthModule = new OmniMIDI::FluidSynth;
				break;
			case XSYNTH:
				SynthModule = new OmniMIDI::XSynth;
				break;
			default:
				SynthModule = new OmniMIDI::SynthModule;
				break;
			}

			if (SynthModule->LoadSynthModule()) {
				if (SynthModule->StartSynthModule()) {
					return 1;
				}
				SynthModule->StopSynthModule();
			}
			SynthModule->UnloadSynthModule();
		}

		LOG(KDMErr, "InitializeKDMAPIStream failed.");
		return 0;
	}

	__declspec(dllexport)
	int KDMAPI TerminateKDMAPIStream() {
		if (!SynthModule)
			SynthModule = new OmniMIDI::SynthModule;

		if (SynthModule->SynthID() != EMPTYMODULE) {
			if (SynthModule->StopSynthModule()) {
				if (SynthModule->UnloadSynthModule()) {
					delete SynthModule;
					SynthModule = new OmniMIDI::SynthModule;

					return 1;
				}
			}
		}

		LOG(KDMErr, "TerminateKDMAPIStream failed.");
		return 0;
	}

	__declspec(dllexport)
	void KDMAPI ResetKDMAPIStream() {
		SynthModule->PlayShortEvent(0x010101FF);
	}

	__declspec(dllexport)
	void KDMAPI SendDirectData(unsigned int ev) {
		SynthModule->PlayShortEvent(ev);
	}

	__declspec(dllexport)
	void KDMAPI SendDirectDataNoBuf(unsigned int) {
		// Unsupported.
	}

	__declspec(dllexport)
	int KDMAPI SendCustomEvent(unsigned int evt, unsigned int chan, unsigned int param) {
		return SynthModule->TalkToSynthDirectly(evt, chan, param);
	}

	__declspec(dllexport)
	int KDMAPI DriverSettings(unsigned int setting, unsigned int mode, void* value, unsigned int cbValue) {
		if (!SynthModule)
			return 0;

		return SynthModule->SettingsManager(setting, (bool)mode, value, (size_t)cbValue);
	}
}