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