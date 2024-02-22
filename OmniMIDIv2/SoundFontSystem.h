#pragma once

#ifndef _SOUNDFONTSYSTEM_H
#define _SOUNDFONTSYSTEM_H

#include "ErrSys.h"
#include "Utils.h"
#include <nlohmann\json.hpp>
#include <fstream>
#include <future>
#include <string>

namespace OmniMIDI {
	class SoundFont {
	public:
		std::string path;

		bool enabled = true;
		bool xgdrums = false;
		bool linattmod = false;
		bool lindecvol = false;
		bool minfx = false;
		bool nolimits = false;
		bool norampin = false;

		int spreset = -1;
		int sbank = -1;
		int dpreset = -1;
		int dbank = 0;
		int dbanklsb = 0;
	};

	class SoundFontSystem {
	private:
		ErrorSystem::WinErr SfErr;
		Utils::SysPath Utils;
		std::vector<SoundFont> SoundFonts;

	public:
		std::vector<OmniMIDI::SoundFont>* LoadList(std::wstring list = L"");
		bool ClearList();
	};
}

#endif