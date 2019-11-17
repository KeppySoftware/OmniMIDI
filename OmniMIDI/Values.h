// OmniMIDI Values

// Device status
#define DEVICE_UNAVAILABLE 0
#define DEVICE_AVAILABLE 1

// Things
UINT CPUThreadsAvailable = 0;

#define GETCMD(f) (f & 0xF0)
#define GETSP(f) ((f & 0xFF0000) >> 16)
#define GETFP(f) ((f & 0x00FF00) >> 8)
#define GETCHANNEL(f) (f & 0xF)

#define SETVELOCITY(f, nf) f = (f & 0xFF00FFFF) | ((DWORD(nf) & 0xFF) << 16)
#define SETNOTE(f, nf) f = (f & 0xFFFF00FF) | ((DWORD(nf) & 0xFF) << 8)
#define SETSTATUS(f, nf) f = (f & 0xFFFFFF00) | (DWORD(nf) & 0xFF)

#define MIDI_NOTEOFF	0x80
#define MIDI_NOTEON		0x90
#define MIDI_POLYAFTER	0xA0
#define MIDI_CMC		0xB0
#define MIDI_PROGCHAN	0xC0
#define MIDI_CHANAFTER	0xD0
#define MIDI_PITCHWHEEL	0xE0

#define MIDI_IO_PACKED	0x00000000L			// Legacy mode, used by most MIDI apps
#define MIDI_IO_COOKED	0x00000002L			// Stream mode, used by some old MIDI apps (Such as GZDoom)

// path
#define NTFS_MAX_PATH	32767

// Settings managed by client
BOOL AlreadyStartedOnce = FALSE;

// EVBuffer
typedef struct EventsBuffer {
	DWORD* Buffer;
	volatile ULONGLONG		ReadHead;
	ULONGLONG				WriteHead;
};
// The buffer's structure
EventsBuffer EVBuffer;							// The buffer
DWORD LastRunningStatus = 0;				// Last running status
QWORD EvBufferSize = 4096;
DWORD EvBufferMultRatio = 1;
DWORD GetEvBuffSizeFromRAM = 0;

// Cooked player struct
static BOOL CookedPlayerHasToGo = FALSE;
typedef struct CookedPlayer
{
	LPMIDIHDR MIDIHeaderQueue;			// MIDIHDR buffer
	BOOL Paused;						// Is the player paused?
	DWORD Tempo;						// Player tempo
	DWORD TimeDiv;						// Player time division
	DWORD TempoMulti;					// Player time multiplier
	DWORD TimeAccumulator;				// ?
	DWORD ByteAccumulator;				// ?
	DWORD TickAccumulator;				// ?
	LockSystem Lock;					// LockSystem
	DWORD_PTR dwInstance;
};

// Device stuff
const GUID OMCLSID = { 0x62F3192B, 0xA961, 0x456D, { 0xAB, 0xCA, 0xA5, 0xC9, 0x5A, 0x14, 0xB9, 0xAA } };
ULONGLONG TickStart = 0;			// For TGT64
HSTREAM OMStream = NULL;
HANDLE OMReady = NULL;
HMIDI OMHMIDI = NULL;
HDRVR OMHDRVR = NULL;
DWORD_PTR OMCallback = NULL;
DWORD_PTR OMInstance = NULL;
DWORD OMFlags = NULL;

// Important stuff
const std::locale UTF8Support(std::locale(), new std::codecvt_utf8<wchar_t>);
BOOL DriverInitStatus = FALSE;
BOOL AlreadyInitializedViaKDMAPI = FALSE;
BOOL BASSLoadedToMemory = FALSE;
BOOL ASIOReady = FALSE;
BOOL DisableChime = FALSE;
BOOL KDMAPIEnabled = FALSE;
BOOL IsKDMAPIViaWinMM = FALSE;
WCHAR SynthNameW[MAXPNAMELEN];		// Synthesizer name
CHAR SynthName[MAXPNAMELEN];			// Synthesizer name, but ASCII

// Stream
BASS_INFO info;
FLOAT SynthVolume = 1.0;

// Registry system
#define KEY_READY	ERROR_SUCCESS
#define KEY_CLOSED	ERROR_INVALID_HANDLE

typedef struct RegKey
{
	HKEY Address = NULL;
	LSTATUS Status = KEY_CLOSED;
};

RegKey MainKey, Configuration, Channels, ChanOverride, SFDynamicLoader;

DWORD Blank = NULL;
DWORD dwType = REG_DWORD, dwSize = sizeof(DWORD);
DWORD qwType = REG_QWORD, qwSize = sizeof(QWORD);
DWORD SNType = REG_SZ, SNSize = sizeof(SynthNameW);

// Threads
typedef struct Thread
{
	HANDLE ThreadHandle = NULL;
	UINT ThreadAddress = NULL;
	FILETIME Time, Kernel, User;
	ULARGE_INTEGER CPU, KernelCPU, UserCPU;
	BOOL DebugAvailable;					// <<<<<<<< USED INTERNALLY BY OMNIMIDI!
};

BOOL bass_initialized = FALSE;
BOOL block_bassinit = FALSE;
BOOL stop_thread = FALSE;

Thread HealthThread, ATThread, EPThread, DThread, CookedThread;
LockSystem EPThreadsL;

// Mandatory values
HMODULE hinst = NULL;					// main DLL handle
HMODULE winmm = NULL;					// ?

HANDLE CSFCheck = NULL;					// Common SoundFonts

CHAR AppPath[NTFS_MAX_PATH] = { 0 };		// debug info
TCHAR AppPathW[NTFS_MAX_PATH] = { 0 };	// debug info
CHAR AppName[MAX_PATH] = { 0 };			// debug info
TCHAR AppNameW[MAX_PATH] = { 0 };		// debug info

HANDLE hPipe = INVALID_HANDLE_VALUE;		// debug info

// Main values
INT AudioOutput = -1;				// Audio output (All devices except AudToWAV and ASIO)
BASS_FX_VOLUME_PARAM ChVolumeStruct;	// Volume
HFX ChVolume;						// Volume
DWORD RestartValue = 0;				// For AudToWAV
BOOL UnlimitedChannels = 0;			// For KDMAPI

const FLOAT sndbflen = 256.0f;		// AudToWAV
FLOAT* sndbf;						// AudToWAV

// Settings and debug
wchar_t ListToLoad[NTFS_MAX_PATH] = { 0 };
typedef struct SoundFontList
{
	int EnableState;
	wchar_t Path[NTFS_MAX_PATH];
	int SourcePreset;
	int SourceBank;
	int DestinationPreset;
	int DestinationBank;
	int XGBankMode;
};

FILE* DebugLog = NULL;
BOOL SettingsManagedByClient;
FLOAT RenderingTime = 0.0f;
Settings ManagedSettings = Settings();
DebugInfo ManagedDebugInfo = DEFAULT_DEBUG;

// Priority values
const DWORD prioval[] =
{
	THREAD_PRIORITY_TIME_CRITICAL,
	THREAD_PRIORITY_TIME_CRITICAL,
	THREAD_PRIORITY_HIGHEST,
	THREAD_PRIORITY_ABOVE_NORMAL,
	THREAD_PRIORITY_NORMAL,
	THREAD_PRIORITY_BELOW_NORMAL,
	THREAD_PRIORITY_HIGHEST
};

// Built-in blacklist
BOOL CPBlacklisted = FALSE;
const LPCWSTR CookedPlayerBlacklist[] =
{
	_T("wmplayer.exe"),
};

const LPCWSTR BuiltInBlacklist[] =
{
	_T("Battle.net Launcher.exe"),
	_T("Discord.exe"),
	_T("DiscordCanary.exe"),
	_T("Fortnite.exe"),
	_T("ICEsoundService64.exe"),
	_T("LogonUI.exe"),
	_T("NVDisplay.Container.exe"),
	_T("NVIDIA Share.exe"),
	_T("NVIDIA Web Helper.exe"),
	_T("RainbowSix.exe"),
	_T("RuntimeBroker.exe"),
	_T("RustClient.exe"),
	_T("SearchUI.exe"),
	_T("SecurityHealthService.exe"),
	_T("SecurityHealthSystray.exe"),
	_T("ShellExperienceHost.exe"),
	_T("SndVol.exe"),
	_T("WUDFHost.exe"),
	_T("conhost.exe"),
	_T("consent.exe"),
	_T("csrss.exe"),
	_T("ctfmon.exe"),
	_T("dwm.exe"),
	_T("explorer.exe"),
	_T("fontdrvhost.exe"),
	_T("lsass.exe"),
	_T("mstsc.exe"),
	_T("nvcontainer.exe"),
	_T("nvsphelper64.exe"),
	_T("smss.exe"),
	_T("spoolsv.exe"),
	_T("vcpkgsrv.exe"),
	_T("vmware-hostd.exe"),
	_T("vmware-vmx.exe"),
	_T("wininit.exe"),
	_T("winlogon.exe"),
};

// Per channel values
DWORD cvalues[16];		// Volume setting per channel.
DWORD cbank[16];			// MIDI bank setting per channel.
DWORD cpreset[16];		// MIDI preset setting for... you guess it!

DWORD SynthType = MOD_MIDIPORT;
const DWORD SynthNamesTypes[7] =
{
	MOD_FMSYNTH,
	MOD_SYNTH,
	MOD_MIDIPORT,
	MOD_WAVETABLE,
	MOD_MAPPER,
	MOD_SWSYNTH,
	MOD_SQSYNTH
};

// Reverb and chorus
DWORD reverb = 64;					// Reverb
DWORD chorus = 64;					// Chorus

// Watchdog stuff
DWORD rvalues[16];

// -----------------------------------------------------------------------

const wchar_t* OMPipeTemplate = L"\\\\.\\pipe\\OmniMIDIDbg%u";
const wchar_t* CSFFileTemplate = L"\\Common SoundFonts\\SoundFontList.csflist";
const wchar_t* OMFileTemplate = L"\\OmniMIDI\\%s\\OmniMIDI_%s.%s";
const wchar_t* OMLetters[15] = { L"A", L"B", L"C", L"D", L"E", L"F", L"G", L"H", L"I", L"L", L"M", L"N", L"O", L"P", L"Q"};

// -----------------------------------------------------------------------

std::vector<HSOUNDFONT> SoundFontHandles;
std::vector<BASS_MIDI_FONTEX> SoundFontPresets;

// -----------------------------------------------------------------------

DWORD pitchshiftchan[16];