/*

	OmniMIDI v15+ (Rewrite) for Windows NT

	This file contains the required code to run the driver under Windows 7 SP1 and later.
	This file is useful only if you want to compile the driver under Windows, it's not needed for Linux/macOS porting.

*/

#ifndef _WDMENTRY_H
#define _WDMENTRY_H

#pragma once

#include <Windows.h>
#include <Shlwapi.h>
#include <devguid.h>
#include <newdev.h>
#include <regstr.h>
#include <setupapi.h>
#include "ErrSys.h"
#include "WDMDrv.h"
#include "KDMAPI.h"
#include "BASSSynth.h"
#include "FluidSynth.h"
#include "XSynthM.h"
#include "TSFSynth.h"
#include "StreamPlayer.h"

#ifdef DEFINE_DEVPROPKEY
#undef DEFINE_DEVPROPKEY
#endif
#define DEFINE_DEVPROPKEY(name, l, w1, w2, b1, b2, b3, b4, b5, b6, b7, b8, pid) EXTERN_C const DEVPROPKEY DECLSPEC_SELECTANY name = { { l, w1, w2, { b1, b2,  b3,  b4,  b5,  b6,  b7,  b8 } }, pid }

// Copied from devpkey.h in the WinDDK
DEFINE_DEVPROPKEY(DEVPKEY_Device_BusReportedDeviceDesc, 0x540b947e, 0x8b40, 0x45bc, 0xa8, 0xa2, 0x6a, 0x0b, 0x89, 0x4c, 0xbd, 0xa2, 4);	// DEVPROP_TYPE_STRING
DEFINE_DEVPROPKEY(DEVPKEY_Device_ContainerId, 0x8c7ed206, 0x3f8a, 0x4827, 0xb3, 0xab, 0xae, 0x9e, 0x1f, 0xae, 0xfc, 0x6c, 2);			// DEVPROP_TYPE_GUID
DEFINE_DEVPROPKEY(DEVPKEY_Device_FriendlyName, 0xa45c254e, 0xdf1c, 0x4efd, 0x80, 0x20, 0x67, 0xd1, 0x46, 0xa8, 0x50, 0xe0, 14);			// DEVPROP_TYPE_STRING
DEFINE_DEVPROPKEY(DEVPKEY_DeviceDisplay_Category, 0x78c34fc8, 0x104a, 0x4aca, 0x9e, 0xa4, 0x52, 0x4d, 0x52, 0x99, 0x6e, 0x57, 0x5a);	// DEVPROP_TYPE_STRING_LIST
DEFINE_DEVPROPKEY(DEVPKEY_Device_LocationInfo, 0xa45c254e, 0xdf1c, 0x4efd, 0x80, 0x20, 0x67, 0xd1, 0x46, 0xa8, 0x50, 0xe0, 15);			// DEVPROP_TYPE_STRING
DEFINE_DEVPROPKEY(DEVPKEY_Device_Manufacturer, 0xa45c254e, 0xdf1c, 0x4efd, 0x80, 0x20, 0x67, 0xd1, 0x46, 0xa8, 0x50, 0xe0, 13);			// DEVPROP_TYPE_STRING
DEFINE_DEVPROPKEY(DEVPKEY_Device_SecuritySDS, 0xa45c254e, 0xdf1c, 0x4efd, 0x80, 0x20, 0x67, 0xd1, 0x46, 0xa8, 0x50, 0xe0, 26);			// DEVPROP_TYPE_SECURITY_DESCRIPTOR_STRING

const GUID DevGUID = GUID_DEVCLASS_MEDIA;
const wchar_t DEVICE_NAME_MEDIA[] = L"MEDIA";
const wchar_t DEVICE_DESCRIPTION[] = L"OmniMIDI for Windows NT";
const wchar_t DRIVER_PROVIDER_NAME[] = L"Keppy's Software";
const wchar_t DRIVER_CLASS_PROP_DRIVER_DESC[] = L"DriverDesc";
const wchar_t DRIVER_CLASS_PROP_PROVIDER_NAME[] = L"ProviderName";
const wchar_t DRIVER_CLASS_SUBKEY_DRIVERS[] = L"Drivers";
const wchar_t DRIVER_CLASS_PROP_SUBCLASSES[] = L"SubClasses";
const wchar_t DRIVER_CLASS_SUBCLASSES[] = L"midi";
const wchar_t DRIVER_SUBCLASS_SUBKEYS[] = L"midi\\OmniMIDI.dll";
const wchar_t DRIVER_SUBCLASS_PROP_DRIVER[] = L"Driver";
const wchar_t DRIVER_SUBCLASS_PROP_DESCRIPTION[] = L"Description";
const wchar_t DRIVER_SUBCLASS_PROP_ALIAS[] = L"Alias";
const wchar_t SHAKRA_DRIVER_NAME[] = L"OmniMIDI.dll";

static bool DriverBusy = false;

// MIDI REG
const wchar_t MIDI_REGISTRY_ENTRY_TEMPLATE[] = L"midi%d";

namespace OmniMIDI {
	class WDMSettings : public SynthSettings {
	private:
#pragma clang diagnostic push
#pragma clang diagnostic ignored "-Wunused-private-field"
		ErrorSystem::WinErr SetErr;
#pragma clang diagnostic pop

	public:
		// Global settings
		int Renderer = BASSMIDI;
		bool KDMAPIEnabled = true;
		std::string CustomRenderer = "empty";
		std::vector<std::string> Blacklist;

		WDMSettings() {
			// When you initialize Settings(), load OM's own settings by default
			Utils::SysPath Utils;
			wchar_t OMPath[MAX_PATH] = { 0 };
			wchar_t OMBPath[MAX_PATH] = { 0 };
			wchar_t OMDBPath[MAX_PATH] = { 0 };

			if (Utils.GetFolderPath(Utils::FIDs::UserFolder, OMPath, sizeof(OMPath))) {
				wcscpy_s(OMBPath, OMPath);
				swprintf_s(OMPath, L"%s\\OmniMIDI\\settings.json\0", OMPath);
				swprintf_s(OMBPath, L"%s\\OmniMIDI\\defblacklist.json\0", OMBPath);
				swprintf_s(OMBPath, L"%s\\OmniMIDI\\blacklist.json\0", OMBPath);
				LoadJSON(OMPath);
				LoadBlacklist(OMBPath);
			}
		}

		void CreateJSON(wchar_t* Path) {
			std::fstream st;
			st.open(Path, std::fstream::out | std::ofstream::trunc);
			if (st.is_open()) {
				nlohmann::json defset = {
					{ "WDMInit", {
						JSONGetVal(Renderer),
						JSONGetVal(CustomRenderer)
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
						auto& JsonData = json["WDMInit"];

						if (!(JsonData == nullptr)) {
							JSONSetVal(int, Renderer);
							JSONSetVal(std::string, CustomRenderer);
						}
					}
					else throw nlohmann::json::type_error::create(667, "json structure is not valid", nullptr);
				}
				catch (nlohmann::json::type_error ex) {
					st.close();
					LOG(SetErr, "The JSON is corrupted or malformed! nlohmann::json says: %s", ex.what());
					CreateJSON(Path);
					return;
				}
				st.close();
			}
		}

		void LoadBlacklist(wchar_t* Path) {
			std::fstream st;
			st.open(Path, std::fstream::in);

			if (st.is_open()) {
				try {
					// Read the JSON data from there
					auto JsonData = nlohmann::json::parse(st, nullptr, false, true);

					if (JsonData != nullptr) {
						JSONSetVal(std::vector<std::string>, Blacklist);
					}
					else throw nlohmann::json::type_error::create(667, "json structure is not valid", nullptr);
				}
				catch (nlohmann::json::type_error ex) {
					st.close();
					return;
				}
				st.close();
			}
		}

		bool IsBlacklistedProcess() {
			char szFilePath[MAX_PATH];
			char szFileName[MAX_PATH];

			if (!Blacklist.empty())
			{
				GetModuleFileNameA(NULL, szFilePath, MAX_PATH);
				strncpy_s(szFileName, PathFindFileNameA(szFilePath), MAX_PATH);

				for (int i = 0; i < Blacklist.size(); i++) {
					if (!_stricmp(szFilePath, Blacklist[i].c_str())) {
						return true;
					}

					if (!_stricmp(szFileName, Blacklist[i].c_str())) {
						return true;
					}
				}
			}

			return false;
		}
	};
}
#endif