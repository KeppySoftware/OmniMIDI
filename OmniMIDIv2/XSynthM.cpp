/*

	OmniMIDI v15+ (Rewrite) for Windows NT

	This file contains the required code to run the driver under Windows 7 SP1 and later.
	This file is useful only if you want to compile the driver under Windows, it's not needed for Linux/macOS porting.

*/

#ifndef _M_ARM

#include "XSynthM.h"

bool OmniMIDI::XSynth::LoadSynthModule() {
	if (!Settings)
		Settings = new XSynthSettings;

	if (!XLib)
		XLib = new Lib(L"xsynth");

	if (XLib->IsOnline())
		return true;

	if (!XLib->LoadLib())
		return false;

	void* ptr = nullptr;
	for (int i = 0; i < sizeof(FLibImports) / sizeof(FLibImports[0]); i++) {
		if (FLibImports[i].SetPtr(XLib->Ptr(), FLibImports[i].GetName()))
			continue;
	}

	return true;
}

bool OmniMIDI::XSynth::UnloadSynthModule() {
	if (!XLib)
		return true;

	if (!XLib->UnloadLib())
		return false;

	return true;
}

bool OmniMIDI::XSynth::StartSynthModule() {
	Utils::SysPath Utils;
	wchar_t OMPath[MAX_PATH] = { 0 };

	if (Running)
		return true;

	if (!Settings)
		return false;

	if (StartModule(Settings->BufSize))
	{
		std::vector<OmniMIDI::SoundFont>* SFv = SFSystem.LoadList();
		if (SFv != nullptr) {
			std::vector<SoundFont>& dSFv = *SFv;

			for (int i = 0; i < SFv->size(); i++) {
				LoadSoundFont(dSFv[i].path.c_str());
				break;
			}
		}

		Running = true;
		return true;
	}

	return false;
}

bool OmniMIDI::XSynth::StopSynthModule() {
	if (StopModule()) {
		// FIXME: Waiting for Arduano to fix a ghost thread
		Sleep(4000);
		return true;
	}

	return false;
}

SynthResult OmniMIDI::XSynth::PlayShortEvent(unsigned int ev) {
	return UPlayShortEvent(ev);
}

SynthResult OmniMIDI::XSynth::UPlayShortEvent(unsigned int ev) {
	if (XLib->IsOnline()) 
		SendData(ev);

	return SYNTH_OK;
}

SynthResult OmniMIDI::XSynth::PlayLongEvent(char* ev, unsigned int size) {
	return SYNTH_OK;
}

SynthResult OmniMIDI::XSynth::UPlayLongEvent(char* ev, unsigned int size) {
	return SYNTH_OK;
}

#endif