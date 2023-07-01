/*

	OmniMIDI v15+ (Rewrite) for Windows NT

	This file contains the required code to run the driver under Windows 7 SP1 and later.
	This file is useful only if you want to compile the driver under Windows, it's not needed for Linux/macOS porting.

*/

#include <bass_vst.h>

DWORD(WINAPI* BASS_VST_ChannelSetDSP)(DWORD chHandle, const void* dllFile, DWORD flags, int priority) = 0;