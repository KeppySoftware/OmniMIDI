/*
OmniMIDI, a fork of BASSMIDI Driver - Declarations

Thank you Kode54 for allowing me to fork your awesome driver.
*/
#pragma once

#include <wtypes.h>

#ifdef OMNIMIDI_EXPORTS
#define KDMAPI __declspec(dllexport)
#else
#define KDMAPI __declspec(dllimport)
#endif

// Audio engines
#define AUDTOWAV 0
#define DSOUND_ENGINE 1
#define ASIO_ENGINE 2
#define WASAPI_ENGINE 3

typedef struct Settings
{
	BOOL AlternativeCPU = FALSE;			// Autopanic switch (DEPRECATED)
	BOOL CapFramerate = FALSE;				// Cap input framerate
	BOOL DebugMode = FALSE;					// Debug console
	BOOL DisableNotesFadeOut = 0;			// Disable fade-out
	BOOL DontMissNotes = FALSE;				// Slow down instead of missing notes

	BOOL EnableSFX = TRUE;					// Enable or disable FXs
	BOOL FastHotkeys = FALSE;				// Enable/Disable fast hotkeys
	BOOL FullVelocityMode = FALSE;			// Enable full velocity mode
	BOOL IgnoreNotesBetweenVel = FALSE;		// Ignore notes in between two velocity values
	BOOL IgnoreAllEvents = FALSE;			// Ignore all MIDI events
	BOOL IgnoreSysEx = FALSE;				// Ignore SysEx events
	BOOL IgnoreSysReset = FALSE;			// Ignore sysex messages
	BOOL LimitTo88Keys = FALSE;				// Limit to 88 keys
	BOOL LiveChanges = FALSE;				// Live changes
	BOOL MT32Mode = FALSE;					// Roland MT-32 mode
	BOOL MonoRendering = TRUE;				// Mono rendering (Instead of stereo by default)
	BOOL NoBlacklistMessage = TRUE;			// Disable blacklist message (DEPRECATED)
	BOOL NoteOff1 = 0;						// Note cut INT
	BOOL NotesCatcherWithAudio = FALSE;		// For old-ass PCs
	BOOL OverrideInstruments = TRUE;		// Override channel instruments
	BOOL PreloadSoundFonts = TRUE;			// Soundfont preloading
	BOOL SincInter = FALSE;					// Sinc
	BOOL SleepStates = TRUE;				// Reduce CPU overhead
	BOOL VolumeMonitor = TRUE;				// Volume monitoring

	DWORD AudioBitDepth = 1;				// Floating poDWORD audio
	DWORD AudioFrequency = 48000;			// Audio frequency
	DWORD AudioOutputReg = 0;				// Audio output (All devices except AudToWAV and ASIO)
	DWORD BufferLength = 0;					// Default
	DWORD CurrentEngine = WASAPI_ENGINE;	// Current engine
	DWORD DefaultSFList = 1;				// Default soundfont list
	DWORD DriverPriority = 0;				// Process priority
	DWORD Extra8Lists = 0;					// Enable extra 8 SoundFont lists
	DWORD MaxRenderingTime = 75;			// CPU usage INT
	DWORD MaxVelIgnore = 1;					// Ignore notes in between two velocity values
	DWORD MinVelIgnore = 1;					// Ignore notes in between two velocity values
	DWORD OutputVolume = 10000;				// Volume
	DWORD TransposeValue = 127;				// Pitch shift (127 = None)
	DWORD MaxVoices = 500;					// Voices limit
	DWORD SincConv = 2;						// Sinc

	// Add more down here
	// ------------------
};

typedef struct DebugInfo
{
	FLOAT RenderingTime = 0.0f;
	DWORD ActiveVoices[16];

	// Add more down here
	// ------------------
};

// Return the KDMAPI version from OmniMIDI as the following output: Major.Minor.Build.Revision (eg. 1.30.0 Rev. 51).
extern "C" KDMAPI BOOL ReturnKDMAPIVer(DWORD &Major, DWORD &Minor, DWORD &Build, DWORD &Revision);

// Checks if KDMAPI is available. You can ignore the output if you want, but you should give the user the choice between WinMM and KDMAPI.
extern "C" KDMAPI BOOL IsKDMAPIAvailable();

// Initializes OmniMIDI through KDMAPI. (Like midiOutOpen)
extern "C" KDMAPI VOID InitializeKDMAPIStream();

// Closes OmniMIDI through KDMAPI. (Like midiOutClose)
extern "C" KDMAPI VOID TerminateKDMAPIStream();

// Resets OmniMIDI and all its MIDI channels through KDMAPI. (Like midiOutReset)
extern "C" KDMAPI VOID ResetKDMAPIStream();

// Send short messages through KDMAPI. (Like midiOutShortMsg)
extern "C" KDMAPI MMRESULT SendDirectData(DWORD dwMsg);

// Send short messages through KDMAPI like SendDirectData, but bypasses the buffer. (Like midiOutShortMsg)
extern "C" KDMAPI MMRESULT SendDirectDataNoBuf(DWORD dwMsg);

// Send long messages through KDMAPI. (Like midiOutLongMsg)
extern "C" KDMAPI MMRESULT SendDirectLongData(MIDIHDR* IIMidiHdr);

// Send long messages through KDMAPI like SendDirectLongData, but bypasses the buffer. (Like midiOutLongMsg)
extern "C" KDMAPI MMRESULT SendDirectLongDataNoBuf(MIDIHDR* IIMidiHdr);

// Prepares the long data, and locks its memory to prevent apps from writing to it.
extern "C" KDMAPI MMRESULT PrepareLongData(MIDIHDR* IIMidiHdr);

// Unlocks the memory, and unprepares the long data.
extern "C" KDMAPI MMRESULT UnprepareLongData(MIDIHDR* IIMidiHdr);

// Push your own settings to the driver through the Settings struct.
extern "C" KDMAPI VOID ChangeDriverSettings(const Settings* Struct, DWORD StructSize);

// Load a custom sflist. (You can also load SF2 and SFZ files)
extern "C" KDMAPI VOID LoadCustomSoundFontsList(const TCHAR* Directory);

// Get a pointer to the debug info of the driver.
extern "C" KDMAPI DebugInfo* GetDriverDebugInfo();