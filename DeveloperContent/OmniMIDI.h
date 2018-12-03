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

#define DEFAULT_SETTINGS { 0, 0, 0, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 0, 0, 0, 0, 44100, 0, 30, WASAPI_ENGINE, 1, 0, 0, 75, 1, 1, 10000, 127, 500, 2, 0, 5, 0, 5 }
#define DEFAULT_DEBUG { 0.0f, { 0 } }

// Settings
#define OM_SET						0x0
#define OM_GET						0x1

#define OM_CAPFRAMERATE				0x10000
#define OM_DEBUGMMODE				0x10001
#define OM_DISABLEFADEOUT			0x10002
#define OM_DONTMISSNOTES			0x10003

#define OM_ENABLESFX				0x10004
#define OM_FULLVELOCITY				0x10005
#define OM_IGNOREVELOCITYRANGE		0x10006
#define OM_IGNOREALLEVENTS			0x10007
#define OM_IGNORESYSEX				0x10008
#define OM_IGNORESYSRESET			0x10009
#define OM_LIMITRANGETO88			0x10010
#define OM_MT32MODE					0x10011
#define OM_MONORENDERING			0x10012
#define OM_NOTEOFF1					0x10013
#define OM_EVENTPROCWITHAUDIO		0x10014
#define OM_SINCINTER				0x10015
#define OM_SLEEPSTATES				0x10016

#define OM_AUDIOBITDEPTH			0x10017
#define OM_AUDIOFREQ				0x10018
#define OM_CURRENTENGINE			0x10019
#define OM_BUFFERLENGTH				0x10020
#define OM_MAXRENDERINGTIME			0x10021
#define OM_MINIGNOREVELRANGE		0x10022
#define OM_MAXIGNOREVELRANGE		0x10023
#define OM_OUTPUTVOLUME				0x10024
#define OM_TRANSPOSE				0x10025
#define OM_MAXVOICES				0x10026
#define OM_SINCINTERCONV			0x10027

#define OM_OVERRIDENOTELENGTH		0x10028
#define OM_NOTELENGTH				0x10029
#define OM_ENABLEDELAYNOTEOFF		0x10030
#define OM_DELAYNOTEOFFVAL			0x10031

// The settings struct, you can initialize it with the defaults value through by assigning DEFAULT_SETTINGS
typedef struct
{
	BOOL CapFramerate;				// Cap input framerate
	DWORD DebugMode;				// Debug console
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

	DWORD AudioBitDepth;			// Floating point audio
	DWORD AudioFrequency;			// Audio frequency
	DWORD AudioOutputReg;			// Audio output (All devices except AudToWAV and ASIO)
	DWORD BufferLength;				// Default
	DWORD CurrentEngine;			// Current engine
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
	BOOL OverrideNoteLength;		// Override note length
	DWORD NoteLengthValue;			// Length of the note overridden
	BOOL DelayNoteOff;				// Delay note off events
	DWORD DelayNoteOffValue;		// Length of the delay
} Settings;

// The debug info struct, you can set the default values by assigning DEFAULT_DEBUG
typedef struct
{
	FLOAT RenderingTime;			// Current BASS rendering time
	DWORD ActiveVoices[16];			// Active voices for each channel

	// ASIO debug info
	DOUBLE ASIOInputLatency;
	DOUBLE ASIOOutputLatency;

	// Add more down here
	// ------------------
} DebugInfo;

#ifndef KDMAPI_ONLYSTRUCTS
// Return the KDMAPI version from OmniMIDI as the following output: Major.Minor.Build.Revision (eg. 1.30.0 Rev. 51).
BOOL KDMAPI(ReturnKDMAPIVer)(LPDWORD Major, LPDWORD Minor, LPDWORD Build, LPDWORD Revision);

// Checks if KDMAPI is available. You can ignore the output if you want, but you should give the user the choice between WinMM and KDMAPI.
BOOL KDMAPI(IsKDMAPIAvailable)();

// Initializes OmniMIDI through KDMAPI. (Like midiOutOpen)
BOOL KDMAPI(InitializeKDMAPIStream)();

// Closes OmniMIDI through KDMAPI. (Like midiOutClose)
BOOL KDMAPI(TerminateKDMAPIStream)();

// Resets OmniMIDI and all its MIDI channels through KDMAPI. (Like midiOutReset)
VOID KDMAPI(ResetKDMAPIStream)();

// Send short messages through KDMAPI. (Like midiOutShortMsg)
UINT KDMAPI(SendCustomEvent)(DWORD eventtype, DWORD chan, DWORD param);

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

// Get or set the current settings for the driver.
BOOL KDMAPI(DriverSettings)(DWORD Setting, DWORD Mode, LPVOID Value, UINT cbValue);

// Get a pointer to the debug info of the driver.
DebugInfo* KDMAPI(GetDriverDebugInfo)();

// Load a custom sflist. (You can also load SF2 and SFZ files)
VOID KDMAPI(LoadCustomSoundFontsList)(LPWSTR Directory);
#endif