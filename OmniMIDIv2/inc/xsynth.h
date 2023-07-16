/*

	OmniMIDI v15+ (Rewrite) for Windows NT

	This file contains the required code to run the driver under Windows 7 SP1 and later.
	This file is useful only if you want to compile the driver under Windows, it's not needed for Linux/macOS porting.

*/

#ifndef _XSYNTH_H
#define _XSYNTH_H

#include <Windows.h>
#include <string>

extern void (WINAPI* LoadSoundFont)(const char* path);
extern int (WINAPI* StartModule)(double f);
extern int (WINAPI* StopModule)();
extern void (WINAPI* ResetModule)();
extern unsigned int (WINAPI* SendData)(unsigned int ev);

#endif