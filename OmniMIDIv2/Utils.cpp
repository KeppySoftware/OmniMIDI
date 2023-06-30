#pragma once

#include "Utils.h"

bool WinUtils::SysPath::GetFolderPath(const GUID FolderID, wchar_t* P, size_t PS) {
	PWSTR Dir;
	HRESULT SGKFP = SHGetKnownFolderPath(FolderID, 0, NULL, &Dir);
	bool Success = SUCCEEDED(SGKFP);

	if (Success)
		StringCchPrintfW(P, PS, L"%ws", Dir);

	CoTaskMemFree((LPVOID)Dir);

	return Success;
}