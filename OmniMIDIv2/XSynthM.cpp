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
		ptr = (void*)GetProcAddress(XLib->Ptr(), FLibImports[i].GetName());
		if (ptr)
		{
			FLibImports[i].SetPtr(ptr);
			continue;
		}
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
	WinUtils::SysPath Utils;
	wchar_t OMPath[MAX_PATH] = { 0 };

	if (Running)
		return true;

	if (!Settings)
		return false;

	if (StartModule(Settings->BufSize))
	{
		if (Utils.GetFolderPath(FOLDERID_Profile, OMPath, sizeof(OMPath))) {
			swprintf_s(OMPath, L"%s\\OmniMIDI\\lists\\OmniMIDI_A.json\0", OMPath);

			std::fstream sfs;
			sfs.open(OMPath);

			if (sfs.is_open()) {
				try {
					// Read the JSON data from there
					auto json = nlohmann::json::parse(sfs, nullptr, false, true);

					if (json != nullptr) {
						auto& JsonData = json["SoundFonts"];

						if (!(JsonData == nullptr || JsonData.size() < 1)) {
							for (int i = 0; i < JsonData.size(); i++) {
								bool enabled = true;
								nlohmann::json subitem = JsonData[i];

								// Is item valid?
								if (subitem != nullptr) {
									std::string sfpath = subitem["path"].is_null() ? "\0" : subitem["path"];
									enabled = subitem["enabled"].is_null() ? enabled : (bool)subitem["enabled"];

									if (enabled) {
										if (GetFileAttributesA(sfpath.c_str()) != INVALID_FILE_ATTRIBUTES) {
											LoadSoundFont(sfpath.c_str());
											break;
										}
										else NERROR(SynErr, "The SoundFont \"%s\" could not be found!", false, sfpath.c_str());
									}
								}

								// If it's not, then let's loop until the end of the JSON struct
							}
						}
						else NERROR(SynErr, "\"%s\" does not contain a valid \"SoundFonts\" JSON structure.", false, OMPath);
					}
					else NERROR(SynErr, "Invalid JSON structure!", false);
				}
				catch (nlohmann::json::type_error ex) {
					NERROR(SynErr, "The SoundFont JSON is corrupted or malformed!\n\nnlohmann::json says: %s", ex.what());
				}
				sfs.close();
			}
			else NERROR(SynErr, "SoundFonts JSON does not exist.", false);
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