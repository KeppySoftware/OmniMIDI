/*

	OmniMIDI v15+ (Rewrite) for Windows NT

	This file contains the required code to run the driver under Windows 7 SP1 and later.
	This file is useful only if you want to compile the driver under Windows, it's not needed for Linux/macOS porting.

*/

#ifndef _UTILS_H
#define _UTILS_H

#pragma once

#include <Windows.h>
#include <guiddef.h>
#include <ShlObj_core.h>
#include <strsafe.h>

namespace WinUtils {
	class SysPath {
	public:
		bool GetFolderPath(const GUID FolderID, wchar_t* String, size_t StringLen);
	};
}

#endif