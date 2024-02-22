/*

	OmniMIDI v15+ (Rewrite) for Windows NT

	This file contains the required code to run the driver under Windows 7 SP1 and later.

*/

#include "Utils.h"

bool Utils::SysPath::GetFolderPath(const FIDs FolderID, wchar_t* P, size_t PS) {
#ifdef _WIN32 
	GUID id = GUID_NULL;

	switch (FolderID) {
	case System:
		id = FOLDERID_System;
		break;
	case UserFolder:
		id = FOLDERID_Profile;
		break;
	default:
		break;
	}

	if (id != GUID_NULL) {
		PWSTR Dir;
		HRESULT SGKFP = SHGetKnownFolderPath(id, 0, NULL, &Dir);
		bool Success = SUCCEEDED(SGKFP);

		if (Success)
			StringCchPrintfW(P, PS, L"%ws", Dir);

		CoTaskMemFree((LPVOID)Dir);

		return true;
	}
#endif

	return false;
}