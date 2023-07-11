/*

	OmniMIDI v15+ (Rewrite) for Windows NT

	This file contains the required code to run the driver under Windows 7 SP1 and later.
	This file is useful only if you want to compile the driver under Windows, it's not needed for Linux/macOS porting.
	This header has been modified to work properly with the new rewrite.

	Original header: BASS_VST 2.4.1.0 C/C++ header file
	(C) Bjoern Petersen Software Design and Development

	See https://github.com/r10s/BASS_VST for more detailed documentation

*/

#include <wtypes.h>

extern DWORD(WINAPI* BASS_VST_ChannelSetDSP)(DWORD chHandle, const void* dllFile, DWORD flags, int priority);