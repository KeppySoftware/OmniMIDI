/*

	OmniMIDI v15+ (Rewrite) for Windows NT

	This file contains the required code to run the driver under Windows 7 SP1 and later.

*/

#include "SoundFontSystem.h"

std::vector<OmniMIDI::SoundFont>* OmniMIDI::SoundFontSystem::LoadList(std::wstring list) {
	wchar_t OMPath[MAX_PATH] = { 0 };

	if (SoundFonts.size() > 0)
		return &SoundFonts;

	if (Utils.GetFolderPath(Utils::FIDs::UserFolder, OMPath, sizeof(OMPath))) {
		swprintf_s(OMPath, L"%s\\OmniMIDI\\lists\\OmniMIDI_A.json\0", OMPath);

		std::fstream sfs;
		sfs.open(!list.empty() ? list.c_str() : OMPath);

		if (sfs.is_open()) {
			try {
				// Read the JSON data from there
				auto json = nlohmann::json::parse(sfs, nullptr, false, true);

				if (json != nullptr) {
					auto& JsonData = json["SoundFonts"];

					if (!(JsonData == nullptr || JsonData.size() < 1)) {
						SoundFonts.clear();

						for (int i = 0; i < JsonData.size(); i++) {
							SoundFont SF;
							nlohmann::json subitem = JsonData[i];

							// Is item valid?
							if (subitem != nullptr) {
								SF.path = subitem["path"].is_null() ? "\0" : subitem["path"];
								SF.enabled = subitem["enabled"].is_null() ? SF.enabled : (bool)subitem["enabled"];

								if (GetFileAttributesA(SF.path.c_str()) != INVALID_FILE_ATTRIBUTES) {
									SF.xgdrums = subitem["xgdrums"].is_null() ? SF.xgdrums : (bool)subitem["xgdrums"];
									SF.linattmod = subitem["linattmod"].is_null() ? SF.linattmod : (bool)subitem["linattmod"];
									SF.lindecvol = subitem["lindecvol"].is_null() ? SF.lindecvol : (bool)subitem["lindecvol"];
									SF.minfx = subitem["minfx"].is_null() ? SF.minfx : (bool)subitem["minfx"];
									SF.nolimits = subitem["nolimits"].is_null() ? SF.nolimits : (bool)subitem["nolimits"];
									SF.norampin = subitem["norampin"].is_null() ? SF.norampin : (bool)subitem["norampin"];

									SF.spreset = subitem["spreset"].is_null() ? SF.spreset : (int)subitem["spreset"];
									SF.sbank = subitem["sbank"].is_null() ? SF.sbank : (int)subitem["sbank"];
									SF.dpreset = subitem["dpreset"].is_null() ? SF.dpreset : (int)subitem["dpreset"];
									SF.dbank = subitem["dbank"].is_null() ? SF.dbank : (int)subitem["dbank"];
									SF.dbanklsb = subitem["dbanklsb"].is_null() ? SF.dbanklsb : (int)subitem["dbanklsb"];

									SoundFonts.push_back(SF);
								}
								else NERROR(SfErr, "The SoundFont \"%s\" could not be found!", false, SF.path.c_str());
							}

							// If it's not, then let's loop until the end of the JSON struct
						}
					}
					else NERROR(SfErr, "\"%s\" does not contain a valid \"SoundFonts\" JSON structure.", false, OMPath);
				}
				else NERROR(SfErr, "Invalid JSON structure!", false);
			}
			catch (nlohmann::json::type_error ex) {
				NERROR(SfErr, "The SoundFont JSON is corrupted or malformed!nlohmann::json says: %s", ex.what());
			}

			sfs.close();
			return &SoundFonts;
		}
		else NERROR(SfErr, "SoundFonts JSON does not exist.", false);
	}

	return nullptr;
}

bool OmniMIDI::SoundFontSystem::ClearList() {
	if (SoundFonts.size() > 0)
		SoundFonts.clear();

	return true;
}