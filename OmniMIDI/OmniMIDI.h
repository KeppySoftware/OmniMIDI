/*
OmniMIDI, a fork of BASSMIDI Driver - Declarations

Thank you Kode54 for allowing me to fork your awesome driver.
*/
#pragma once

#include <wtypes.h>

#ifndef KDMAPI_ONLYSTRUCTS
#define KDMAPI(f) WINAPI f
#else
#define KDMAPI WINAPI
#endif

// Audio engines
#define AUDTOWAV 0
#define DSOUND_ENGINE 1
#define ASIO_ENGINE 2
#define WASAPI_ENGINE 3

#define DEFAULT_SETTINGS { FALSE, FALSE, FALSE, FALSE, FALSE, TRUE, FALSE, FALSE, FALSE, FALSE, FALSE, FALSE, FALSE, FALSE, FALSE, FALSE, FALSE, FALSE, FALSE, FALSE, TRUE, FALSE, FALSE, FALSE, 0, 44100, 0, 30, WASAPI_ENGINE, 1, 0, 0, 75, 1, 1, 10000, 127, 500, 2 }
#define DEFAULT_DEBUG { 0.0f, { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 } }

// The settings struct, you can initialize it with the defaults value through by assigning DEFAULT_SETTINGS
typedef struct
{
	BOOL AlternativeCPU;			// Autopanic switch (DEPRECATED)
	BOOL CapFramerate;				// Cap input framerate
	BOOL DebugMode;					// Debug console
	BOOL DisableNotesFadeOut;		// Disable fade-out
	BOOL DontMissNotes;				// Slow down instead of missing notes

	BOOL EnableSFX;					// Enable or disable FXs
	BOOL FastHotkeys;				// Enable/Disable fast hotkeys
	BOOL FullVelocityMode;			// Enable full velocity mode
	BOOL IgnoreNotesBetweenVel;		// Ignore notes in between two velocity values
	BOOL IgnoreAllEvents;			// Ignore all MIDI events
	BOOL IgnoreSysEx;				// Ignore SysEx events
	BOOL IgnoreSysReset;			// Ignore sysex messages
	BOOL LimitTo88Keys;				// Limit to 88 keys
	BOOL LiveChanges;				// Live changes
	BOOL MT32Mode;					// Roland MT-32 mode
	BOOL MonoRendering;				// Mono rendering (Instead of stereo by default)
	BOOL NoBlacklistMessage;		// Disable blacklist message (DEPRECATED)
	BOOL NoteOff1;					// Note cut INT
	BOOL NotesCatcherWithAudio;		// For old-ass PCs
	BOOL OverrideInstruments;		// Override channel instruments
	BOOL PreloadSoundFonts;			// Soundfont preloading
	BOOL SincInter;					// Sinc
	BOOL SleepStates;				// Reduce CPU overhead
	BOOL VolumeMonitor;				// Volume monitoring

	DWORD AudioBitDepth;			// Floating poDWORD audio
	DWORD AudioFrequency;			// Audio frequency
	DWORD AudioOutputReg;			// Audio output (All devices except AudToWAV and ASIO)
	DWORD BufferLength;				// Default
	DWORD CurrentEngine;			// Current engine
	DWORD DefaultSFList;			// Default soundfont list
	DWORD DriverPriority;			// Process priority
	BOOL Extra8Lists;				// Enable extra 8 SoundFont lists
	DWORD MaxRenderingTime;			// CPU usage INT
	DWORD MaxVelIgnore;				// Ignore notes in between two velocity values
	DWORD MinVelIgnore;				// Ignore notes in between two velocity values
	DWORD OutputVolume;				// Volume
	DWORD TransposeValue;			// Pitch shift (127 = None)
	DWORD MaxVoices;				// Voices limit
	DWORD SincConv;					// Sinc

	// Add more down here
	// ------------------
} Settings;

// The debug info struct, you can set the default values by assigning DEFAULT_DEBUG
typedef struct
{
	FLOAT RenderingTime;			// Current BASS rendering time
	DWORD ActiveVoices[16];			// Active voices for each channel

	// Add more down here
	// ------------------
} DebugInfo;

#ifndef KDMAPI_ONLYSTRUCTS
// Return the KDMAPI version from OmniMIDI as the following output: Major.Minor.Build.Revision (eg. 1.30.0 Rev. 51).
BOOL KDMAPI(ReturnKDMAPIVer)(LPDWORD Major, LPDWORD Minor, LPDWORD Build, LPDWORD Revision);

// Checks if KDMAPI is available. You can ignore the output if you want, but you should give the user the choice between WinMM and KDMAPI.
BOOL KDMAPI(IsKDMAPIAvailable)();

// Initializes OmniMIDI through KDMAPI. (Like midiOutOpen)
VOID KDMAPI(InitializeKDMAPIStream)();

// Closes OmniMIDI through KDMAPI. (Like midiOutClose)
VOID KDMAPI(TerminateKDMAPIStream)();

// Resets OmniMIDI and all its MIDI channels through KDMAPI. (Like midiOutReset)
VOID KDMAPI(ResetKDMAPIStream)();

// Send short messages through KDMAPI. (Like midiOutShortMsg)
UINT KDMAPI(SendDirectData)(DWORD dwMsg);

// Send short messages through KDMAPI like SendDirectData, but bypasses the buffer. (Like midiOutShortMsg)
UINT KDMAPI(SendDirectDataNoBuf)(DWORD dwMsg);

// Send long messages through KDMAPI. (Like midiOutLongMsg)
UINT KDMAPI(SendDirectLongData)(MIDIHDR* IIMidiHdr);

// Send long messages through KDMAPI like SendDirectLongData, but bypasses the buffer. (Like midiOutLongMsg)
UINT KDMAPI(SendDirectLongDataNoBuf)(MIDIHDR* IIMidiHdr);

// Prepares the long data, and locks its memory to prevent apps from writing to it.
UINT KDMAPI(PrepareLongData)(MIDIHDR* IIMidiHdr);

// Unlocks the memory, and unprepares the long data.
UINT KDMAPI(UnprepareLongData)(MIDIHDR* IIMidiHdr);

// Push your own settings to the driver through the Settings struct.
VOID KDMAPI(ChangeDriverSettings)(const Settings* Struct, DWORD StructSize);

// Load a custom sflist. (You can also load SF2 and SFZ files)
VOID KDMAPI(LoadCustomSoundFontsList)(const TCHAR* Directory);

// Get a pointer to the debug info of the driver.
DebugInfo* KDMAPI(GetDriverDebugInfo)();
#endif