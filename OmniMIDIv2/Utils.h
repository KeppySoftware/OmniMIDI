/*

	OmniMIDI v15+ (Rewrite) for Windows NT

	This file contains the required code to run the driver under Windows 7 SP1 and later.

*/

#pragma once

#ifndef _UTILS_H
#define _UTILS_H

#ifdef _WIN32
#include <Windows.h>
#include <guiddef.h>
#include <ShlObj_core.h>
#include <strsafe.h>
#endif

namespace Utils {
	enum FIDs {
		System,
		UserFolder	
	};

	class SysPath {
	public:
		bool GetFolderPath(const FIDs FolderID, wchar_t* String, size_t StringLen);
	};
}

#endif