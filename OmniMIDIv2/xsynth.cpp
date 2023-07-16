/*

	OmniMIDI v15+ (Rewrite) for Windows NT

	This file contains the required code to run the driver under Windows 7 SP1 and later.
	This file is useful only if you want to compile the driver under Windows, it's not needed for Linux/macOS porting.

*/

#include <xsynth.h>

void (WINAPI* LoadSoundFont)(const char* path) = 0;
int (WINAPI* StartModule)(double f) = 0;
int (WINAPI* StopModule)() = 0;
void (WINAPI* ResetModule)() = 0;
unsigned int (WINAPI* SendData)(unsigned int ev) = 0;