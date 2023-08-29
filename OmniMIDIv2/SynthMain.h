/*

	OmniMIDI v15+ (Rewrite) for Windows NT

	This file contains the required code to run the driver under Windows 7 SP1 and later.
	This file is useful only if you want to compile the driver under Windows, it's not needed for Linux/macOS porting.

*/

#ifndef _SYNTHMAIN_H
#define _SYNTHMAIN_H

#pragma once

// Uncomment this if you want stats to be shown once the synth is closed
// #define _STATSDEV

typedef	unsigned int SynthResult;

#define CHKLRS(f)			(f & 0x80)
#define GETSTATUS(f)		(f & 0xFF)
#define GETCMD(f)			(f & 0xF0)
#define GETCHANNEL(f)		(f & 0xF)
#define GETFP(f)			((f >> 8) & 0xFF)
#define GETSP(f)			((f >> 16) & 0xFF)

//
#define MIDI_NOTEOFF		0x80
#define MIDI_NOTEON			0x90
#define MIDI_POLYAFTER		0xA0
#define MIDI_CMC			0xB0
#define MIDI_PROGCHAN		0xC0
#define MIDI_CHANAFTER		0xD0
#define MIDI_PITCHWHEEL		0xE0

// TBD
#define MIDI_SYSEXBEG		0xF0
#define MIDI_SYSEXEND		0xF7
#define MIDI_COOKPLPLAY		0xFA
#define MIDI_COOKPLCONT		0xFB
#define MIDI_COOKPLSTOP		0xFC
#define MIDI_SENSING		0xFE
#define MIDI_SYSRESET		0xFF

// ERRORS
#define SYNTH_OK			0x00
#define SYNTH_NOTINIT		0x01
#define SYNTH_INITERR		0x02
#define SYNTH_LIBOFFLINE	0x03
#define SYNTH_INVALPARAM	0x04

// Renderers
#define EXTERNAL			-1
#define BASSMIDI			0
#define FLUIDSYNTH			1
#define XSYNTH				2
#define TINYSF				3

#define fv2fn(f)			(#f)
#define ImpFunc(f)			LibImport((void**)&##f, #f)

#define JSONGetVal(f)		{ #f, f }
#define JSONSetVal(t, f)	f = JsonData[#f].is_null() ? f : JsonData[#f].get<t>()

#define SettingsManagerCase(choice, get, type, setting, var, size) \
	case choice: \
		if (size != sizeof(type)) return false; \
		if (get) *(type*)var = setting; \
		else setting = *(type*)var; \
		break;

#define EMPTYMODULE			0xDEADBEEF

#include <Windows.h>
#include <strsafe.h>
#include <thread>
#include <atomic>
#include <algorithm>
#include <iostream>
#include <fstream>
#include <sstream>
#include <vector>
#include <codecvt>
#include <locale>
#include "ErrSys.h"
#include "Utils.h"
#include "KDMAPI.h"
#include <nlohmann\json.hpp>

namespace OmniMIDI {
	class LibImport
	{
	private:
		void** funcptr = nullptr;
		const char* funcname = nullptr;

	public:
		LibImport(void** pptr, const char* pfuncname) {
			funcptr = pptr;
			funcname = pfuncname;
		}

		~LibImport() {
			*(funcptr) = nullptr;
			funcptr = nullptr;
			funcname = nullptr;
		}

		void SetName(const char* pfuncname) { funcname = pfuncname; }
		const char* GetName() { return funcname; }

		void SetPtr(void* pfuncptr) { *(funcptr) = pfuncptr; }
		void* GetPtr() { return *(funcptr); }
	};

	class Lib {
	private:
		const wchar_t* Name;
		HMODULE Library = nullptr;
		bool Initialized = false;
		bool LoadFailed = false;
		bool AppSelfHosted = false;
		ErrorSystem::WinErr LibErr;

	public:
		HMODULE Ptr() { return Library; }
		bool IsOnline() { return (Library != nullptr && Initialized && !LoadFailed); }

		Lib(const wchar_t* pName) {
			Name = pName;
		}

		bool LoadLib(wchar_t* CustomPath = nullptr) {
			WinUtils::SysPath Utils;

			char CName[MAX_PATH] = { 0 };
			wchar_t SysDir[MAX_PATH] = { 0 };
			wchar_t DLLPath[MAX_PATH] = { 0 };
			int swp = 0;

			// Check if library is loaded
			if (Library == nullptr) {
				// If not, begin loading process

				// Check if the host MIDI application has a version of the library
				// in memory already.
				if ((Library = GetModuleHandle(Name)) != nullptr)
				{
					// (TODO) Make it so we can load our own version of it
					// For now, just make the driver try and use that instead
					return (AppSelfHosted = true);
				}
				// Otherwise, let's load our own
				else {
					// Let's get the system path
					if (CustomPath != nullptr) {
						swp = swprintf_s(DLLPath, MAX_PATH, L"%s\\%s.dll\0", CustomPath, Name);
						assert(swp != -1);

						if (swp != -1) {
							Library = LoadLibrary(DLLPath);

							if (!Library)
								return false;
						}
						else return false;
					}
					else {
						if (Utils.GetFolderPath(FOLDERID_System, SysDir, sizeof(SysDir))) {
							swp = swprintf_s(DLLPath, MAX_PATH, L"%s.dll\0", Name);
							assert(swp != -1);

							if (swp != -1) {
								Library = LoadLibrary(DLLPath);

								if (!Library)
								{
									swp = swprintf_s(DLLPath, MAX_PATH, L"%s\\OmniMIDI\\%s.dll\0", SysDir, Name);
									assert(swp != -1);
									if (swp != -1) {
										Library = LoadLibrary(DLLPath);
										assert(Library != 0);

										if (!Library) {
											wcstombs_s(nullptr, CName, Name, MAX_PATH);
											NERROR(LibErr, "The required library \"%s\" could not be loaded or found.\nThis is required for the synthesizer to work.", true, CName);
											return false;
										}
											
									}
									else return false;
								}
							}
							else return false;
						}
						else return false;
					}
				}
			}

			Initialized = true;
			return true;
		}

		bool UnloadLib() {
			// Check if library is already loaded
			if (Library != nullptr) {
				// Set all flags to false
				Initialized = false;
				LoadFailed = false;

				// Check if the library was originally loaded by the
				// hosting MIDI application, this happens sometimes.
				if (AppSelfHosted)
				{
					// It was, set Lib to nullptr and return true
					AppSelfHosted = false;
				}
				else {
					bool r = FreeLibrary(Library);
					assert(r == true);
					if (!r) {
						throw;
					}
				}

				Library = nullptr;
			}

			// It is, return true
			return true;
		}
	};

	class SynthSettings {
	public:
		SynthSettings() {}
	};

	class SynthModule {
	public:
		virtual ~SynthModule() {}
		virtual bool LoadSynthModule() { return true; }
		virtual bool UnloadSynthModule() { return true; }
		virtual bool StartSynthModule() { return true; }
		virtual bool StopSynthModule() { return true; }
		virtual bool SettingsManager(unsigned int setting, bool get, void* var, size_t size) { return true; }
		virtual bool IsSynthInitialized() { return true; }
		virtual int SynthID() { return EMPTYMODULE; }

		// Event handling system
		virtual SynthResult PlayShortEvent(unsigned int ev) { return 0; }
		virtual SynthResult UPlayShortEvent(unsigned int ev) { return 0; }

		virtual SynthResult PlayLongEvent(char* ev, unsigned int size) { return 0; }
		virtual SynthResult UPlayLongEvent(char* ev, unsigned int size) { return 0; }

		virtual int TalkToSynthDirectly(unsigned int evt, unsigned int chan, unsigned int param) { return 0; }
	};
}

#endif