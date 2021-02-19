/*
OmniMIDI, a fork of BASSMIDI Driver - Declarations

Thank you Kode54 for allowing me to fork your awesome driver.

KDMAPI_OMONLY = Used internally by OmniMIDI
KDMAPI_ONLYSTRUCTS = Used by MIDI apps who want to use the KDMAPI functions
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
#define DXAUDIO_ENGINE 1
#define ASIO_ENGINE 2
#define WASAPI_ENGINE 3
#define OLD_WASAPI 69420

// Settings
#define OM_SET						0x0
#define OM_GET						0x1
#define OM_MANAGE					0x2
#define OM_LEAVE					0x3

// DO NOT USE THOSE, THEY ARE USED INTERNALLY BY OMNIMIDI
#define ON1I8F97TJ6S5SI07LDPJBSB	0xFFFFE
#define INVC2MDUBR3YR8DWOF2L55WL	0xFFFFF
// DO NOT USE THOSE, THEY ARE USED INTERNALLY BY OMNIMIDI

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

#define OM_CHANUPDLENGTH			0x10032

#define OM_UNLOCKCHANS				0x10033

// The debug info struct, you can set the default values by assigning DEFAULT_DEBUG
typedef struct
{
	FLOAT RenderingTime = 0.0f;				// Current BASS rendering time
	DWORD ActiveVoices[16] = { 0 };			// Active voices for each channel

	// ASIO debug info (DO NOT USE)
	DOUBLE ASIOInputLatency = 0.0f;
	DOUBLE ASIOOutputLatency = 0.0f;

	// Threads info
	DOUBLE HealthThreadTime = 0.0;
	DOUBLE ATThreadTime = 0.0;
	DOUBLE EPThreadTime = 0.0;
	DOUBLE CookedThreadTime = 0.0;

	// SoundFonts list
	DWORD CurrentSFList;

	// Audio latency
	DOUBLE AudioLatency = 0.0f;
	DWORD AudioBufferSize = 0;

	// Add more down here
	// ------------------
} DebugInfo;

#ifdef KDMAPI_OMONLY
// The settings struct, you can initialize it with the defaults value through by assigning DEFAULT_SETTINGS
typedef struct
{
	BOOL CapFramerate = FALSE;				// Cap input framerate
	BOOL DelayNoteOff = FALSE;				// Delay note off events
	BOOL DisableCookedPlayer = FALSE;		// Disable CookedPlayer
	BOOL DisableNotesFadeOut = FALSE;		// Disable fade-out
	BOOL DontMissNotes = FALSE;				// Slow down instead of missing notes
	BOOL EnableSFX = TRUE;					// Enable or disable FXs
	BOOL Extra8Lists = FALSE;				// DEPRECATED
	BOOL FastHotkeys = FALSE;				// Enable/Disable fast hotkeys
	BOOL FullVelocityMode = FALSE;			// Enable full velocity mode
	BOOL IgnoreAllEvents = FALSE;			// Ignore all MIDI events
	BOOL IgnoreNotesBetweenVel = FALSE;		// Ignore notes in between two velocity values
	BOOL IgnoreSysReset = FALSE;			// Ignore SysEx Reset events
	BOOL LimitTo88Keys = FALSE;				// Limit to 88 keys
	BOOL LiveChanges = FALSE;				// Live changes
	BOOL MT32Mode = FALSE;					// Roland MT-32 mode (DEPRECATED)
	BOOL MonoRendering = FALSE;				// Mono rendering (Instead of stereo by default)
	BOOL NoteOff1 = FALSE;					// Note cut INT
	BOOL NotesCatcherWithAudio = FALSE;		// For old-ass PCs
	BOOL OverrideInstruments = FALSE;		// Override channel instruments
	BOOL OverrideNoteLength = FALSE;		// Override note length
	BOOL PreloadSoundFonts = FALSE;			// Soundfont preloading
	BOOL SincInter = FALSE;					// Sinc
	BOOL VolumeMonitor = FALSE;				// Volume monitoring

	DWORD AudioBitDepth = 0;				// Floating point audio
	DWORD AudioFrequency = 44100;			// Audio frequency
	DWORD AudioOutputReg = 0;				// Audio output (All devices except AudToWAV and ASIO)
	DWORD BufferLength = 30;				// Default
	DWORD ChannelUpdateLength = 0;			// Length of buffer in BASS_ChannelUpdate
	DWORD CurrentEngine = WASAPI_ENGINE;	// Current engine
	DWORD DebugMode = 0;					// Debug console
	DWORD DelayNoteOffValue = 5;			// Length of the delay
	DWORD DriverPriority = 0;				// Process priority
	DWORD MaxRenderingTime = 75;			// CPU usage INT
	DWORD MaxVelIgnore = 1;					// Ignore notes in between two velocity values
	DWORD MaxVoices = 500;					// Voices limit
	DWORD MinVelIgnore = 1;					// Ignore notes in between two velocity values
	DWORD NoteLengthValue = 5;				// Length of the note overridden
	DWORD OutputVolume = 10000;				// Volume
	DWORD SincConv = 2;						// Sinc
	DWORD TransposeValue = 127;				// Pitch shift (127 = None)

	BOOL FollowDefaultAudioDevice = FALSE;	// Follow the default audio device whenever it's changed
	BOOL ReduceBootUpDelay = FALSE;			// Reduce boot-up delay when using DirectSound

	DWORD ConcertPitch = 8192;				// Concert pitch

	BOOL WASAPIExclusive = FALSE;			// WASAPI Exclusive Mode
	BOOL OldWASAPIMode = FALSE;				// Old WASAPI mode
	BOOL WASAPIRAWMode = FALSE;				// WASAPI raw mode
	BOOL WASAPIDoubleBuf = TRUE;			// WASAPI double buffer (for volume monitoring)

	BOOL AudioRampIn = TRUE;				// Enables the audio ramp-in, to gracefully play new notes without audible clicks
} Settings;
#endif

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

// timeGetTime, but 64-bit
DWORD64 KDMAPI(timeGetTime64)();
#endif