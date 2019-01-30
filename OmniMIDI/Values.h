// OmniMIDI Values

// Device status
#define DEVICE_UNAVAILABLE 0
#define DEVICE_AVAILABLE 1

// Things
#define GETSTATUS(f) (f & 0xF0)
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
#define NTFS_MAX_PATH 32767

// Settings managed by client
static BOOL AlreadyStartedOnce = FALSE;

// EVBuffer
struct EventsBuffer {
	DWORD*					Buffer;
	volatile ULONGLONG		ReadHead;
	volatile ULONGLONG		WriteHead;
};
// The buffer's structure

static volatile short EVBufferLock;				// LockSystem
static EventsBuffer EVBuffer;					// The buffer
static DWORD LastRunningStatus = 0;				// Last running status
static QWORD EvBufferSize = 4096;
static DWORD EvBufferMultRatio = 1;
static DWORD GetEvBuffSizeFromRAM = 0;

// Device stuff
static HSTREAM OMStream = NULL;
static HANDLE OMReady = NULL;
static HMIDI OMHMIDI = NULL;
static DWORD_PTR OMCallback = NULL;
static DWORD_PTR OMInstance = NULL;
static DWORD OMFlags = NULL;
static HDRVR OMDevice = NULL;

// Important stuff
static const std::locale UTF8Support(std::locale(), new std::codecvt_utf8<wchar_t>);
static BOOL DriverInitStatus = FALSE;
static BOOL AlreadyInitializedViaKDMAPI = FALSE;
static BOOL BASSLoadedToMemory = FALSE;
static BOOL ASIOReady = FALSE;
static BOOL DisableChime = FALSE;
static BOOL KDMAPIEnabled = FALSE;
static WCHAR SynthNameW[MAXPNAMELEN];		// Synthesizer name

// Stream
static BASS_INFO info;
static FLOAT SynthVolume = 1.0;

// Registry system
#define KEY_READY	ERROR_SUCCESS
#define KEY_CLOSED	ERROR_INVALID_HANDLE

typedef struct RegKey
{
	HKEY Address = NULL;
	LSTATUS Status = KEY_CLOSED;
};

static RegKey MainKey, Configuration, Channels, ChanOverride, SFDynamicLoader;

static DWORD Blank = NULL;
static DWORD dwType = REG_DWORD, dwSize = sizeof(DWORD);
static DWORD qwType = REG_QWORD, qwSize = sizeof(QWORD);
static DWORD SNType = REG_SZ, SNSize = sizeof(SynthNameW);

// Threads
typedef struct Thread
{
	HANDLE ThreadHandle = NULL;
	ULONG ThreadAddress = NULL;
};

static BOOL bass_initialized = FALSE;
static BOOL block_bassinit = FALSE;
static BOOL stop_thread = FALSE;

static Thread HealthThread, ATThread, EPThread, DThread, CookedThread;

// Mandatory values
static HINSTANCE hinst = NULL;				// main DLL handle
static HINSTANCE ntdll = NULL;				// ?

static CHAR AppPath[NTFS_MAX_PATH];			// debug info
static TCHAR AppPathW[NTFS_MAX_PATH];		// debug info
static CHAR AppName[MAX_PATH];				// debug info
static TCHAR AppNameW[MAX_PATH];			// debug info

static HANDLE hPipe = INVALID_HANDLE_VALUE;	// debug info

// Main values
static INT AudioOutput = -1;				// Audio output (All devices except AudToWAV and ASIO)
static BASS_FX_VOLUME_PARAM ChVolumeStruct;	// Volume
static HFX ChVolume;						// Volume
static DWORD RestartValue = 0;				// For AudToWAV

static const FLOAT sndbflen = 256.0f;		// AudToWAV
static FLOAT *sndbf;						// AudToWAV

// Settings and debug
struct SoundFontList
{
	int EnableState;
	wchar_t Path[NTFS_MAX_PATH];
	int SourcePreset;
	int SourceBank;
	int DestinationPreset;
	int DestinationBank;
	int XGBankMode;
} SFLIST, *PSFLIST;

static BOOL SettingsManagedByClient;
static FLOAT RenderingTime = 0.0f;
static Settings ManagedSettings = Settings();
static DebugInfo ManagedDebugInfo = DEFAULT_DEBUG;

// Priority values
static const DWORD prioval[] =
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
static const LPCWSTR BuiltInBlacklist[] =
{
	_T("Battle.net Launcher.exe"),
	_T("LogonUI.exe"),
	_T("NVDisplay.Container.exe"),
	_T("NVIDIA Share.exe"),
	_T("NVIDIA Web Helper.exe"),
	_T("RustClient.exe"),
	_T("SearchUI.exe"),
	_T("Fortnite.exe"),
	_T("RainbowSix.exe"),
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
	_T("lsass.exe"),
	_T("mstsc.exe"),
	_T("nvcontainer.exe"),
	_T("nvsphelper64.exe"),
	_T("smss.exe"),
	_T("spoolsv.exe"),
	_T("vcpkgsrv.exe"),
	_T("vmware-hostd.exe")
};

// Per channel values
static DWORD cvvalues[16];		// Active voices count per channel.
static DWORD cvalues[16];		// Volume setting per channel.
static DWORD cbank[16];			// MIDI bank setting per channel.
static DWORD cpreset[16];		// MIDI preset setting for... you guess it!

static DWORD SynthType = MOD_MIDIPORT;
static const DWORD SynthNamesTypes[7] =
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
static DWORD reverb = 64;					// Reverb
static DWORD chorus = 64;					// Chorus

// Watchdog stuff
static DWORD rvalues[16];

// -----------------------------------------------------------------------

static const wchar_t * OMPipeTemplate = L"\\\\.\\pipe\\OmniMIDIDbg%u";
static const wchar_t * OMFileTemplate = L"%s\\OmniMIDI\\%s\\OmniMIDI_%s.%s";
static const wchar_t * OMLetters[16] = { L"A", L"B", L"C", L"D", L"E", L"F", L"G", L"H", L"I", L"L", L"M", L"N", L"O", L"P", L"Q", L"R" };

// -----------------------------------------------------------------------

static std::vector<HSOUNDFONT> SoundFontHandles;
static std::vector<BASS_MIDI_FONTEX> SoundFontPresets;

// -----------------------------------------------------------------------

static DWORD pitchshiftchan[16];