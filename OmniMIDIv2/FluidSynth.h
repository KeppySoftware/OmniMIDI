/*

	OmniMIDI v15+ (Rewrite) for Windows NT

	This file contains the required code to run the driver under Windows 7 SP1 and later.
	This file is useful only if you want to compile the driver under Windows, it's not needed for Linux/macOS porting.

*/

#ifndef _FLUIDSYNTH_H
#define _FLUIDSYNTH_H

// Not supported on ARM Thumb-2!
#ifndef _M_ARM

#ifdef _WIN32
#include <Windows.h>
#define FLUIDSYNTH_API		WINAPI
#endif

#define FLUID_DEPRECATED	[[deprecated]]

#include <fluidsynth/types.h>
#include <fluidsynth/audio.h>
#include <fluidsynth/midi.h>
#include <fluidsynth/sfont.h>
#include <fluidsynth/synth.h>

#endif

#endif