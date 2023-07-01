/*

	OmniMIDI v15+ (Rewrite) for Windows NT

	This file contains the required code to run the driver under Windows 7 SP1 and later.
	This file is useful only if you want to compile the driver under Windows, it's not needed for Linux/macOS porting.

*/

#ifndef _WDMDRV_H
#define _WDMDRV_H

#pragma once

#include <Windows.h>
#include <ShlObj_core.h>
#include <cmmddk.h>
#include <mmeapi.h>
#include <assert.h>
#include "ErrSys.h"

namespace WinDriver {
	typedef VOID(CALLBACK* WMMC)(HMIDIOUT, DWORD, DWORD_PTR, DWORD_PTR, DWORD_PTR);

	struct Callback {
		HMIDI Handle = nullptr;
		DWORD Mode = 0;
		DWORD_PTR Ptr = 0;
		DWORD_PTR Instance = 0;
	};

	class DriverMask {
	private:
#ifdef _DEBUG
		const wchar_t* TemplateName = L"OmniMIDI (Debug)\0";
#else
		const wchar_t* TemplateName = L"OmniMIDI\0";
#endif

		unsigned short ManufacturerID = 0xFFFF;
		unsigned short ProductID = 0xFFFF;
		unsigned short Technology = MOD_SWSYNTH;
		unsigned short Support = MIDICAPS_VOLUME;

		ErrorSystem::WinErr MaskErr;

	public:
		// Change settings
		bool ChangeSettings(short, short, short, short);
		unsigned long GiveCaps(UINT, PVOID, DWORD);
	};

	class DriverCallback {

	private:
		Callback* pCallback = nullptr;
		ErrorSystem::WinErr CallbackErr;

	public:
		// Callbacks
		bool PrepareCallbackFunction(MIDIOPENDESC*, DWORD);
		bool ClearCallbackFunction();
		void CallbackFunction(DWORD, DWORD_PTR, DWORD_PTR);

	};

	class DriverComponent {

	private:
		HDRVR DrvHandle = nullptr;
		HMODULE LibHandle = nullptr;

		ErrorSystem::WinErr DrvErr;

	public:

		// Opening and closing the driver
		bool SetDriverHandle(HDRVR drv);
		bool UnsetDriverHandle();

		// Loading and unloading the library
		bool SetLibraryHandle(HMODULE mod);
		bool UnsetLibraryHandle();

		// Setting the driver's pointer for the app
		bool OpenDriver(MIDIOPENDESC*, DWORD, DWORD_PTR);
		bool CloseDriver();
	};

	class Blacklist {
	private:

	};
}

#endif