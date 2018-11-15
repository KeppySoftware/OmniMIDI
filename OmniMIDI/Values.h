// OmniMIDI Values

// Device status
#define DEVICE_UNAVAILABLE 0
#define DEVICE_AVAILABLE 1

// Things
#define GETSTATUS(f) (f & 0xF0)
#define MIDI_NOTEOFF	0x80
#define MIDI_NOTEON		0x90

#define MIDI_IO_PACKED	0x00000000L			// Legacy mode, used by all MIDI apps
#define MIDI_IO_COOKED	0x00000002L			// Stream mode, used by some apps (Such as Pinball 3D), NOT SUPPORTED

// path
#define NTFS_MAX_PATH 32767

// Settings managed by client
static BOOL AlreadyStartedOnce = FALSE;

// EVBuffer
struct evbuf_t {
	UINT			uMsg;
	DWORD_PTR		dwParam1;
	DWORD_PTR		dwParam2;

	evbuf_t() :
		uMsg(0),
		dwParam1(0),
		dwParam2(0){}

	evbuf_t(UINT uMsg, DWORD_PTR dwParam1, DWORD_PTR dwParam2):
		uMsg(uMsg),
		dwParam1(dwParam1),
		dwParam2(dwParam2){}
};	
// The buffer's structure

static LightweightLock LockSystem;				// LockSystem
static evbuf_t * evbuf;							// The buffer
static volatile ULONGLONG writehead = 0;		// Current write position in the buffer
static volatile ULONGLONG readhead = 0;			// Current read position in the buffer
static volatile LONGLONG eventcount = 0;		// Total events present in the buffer
static QWORD EvBufferSize = 4096;
static DWORD EvBufferMultRatio = 1;
static DWORD GetEvBuffSizeFromRAM = 0;

// Device stuff
static DWORD_PTR OMCallback = NULL;
static DWORD_PTR OMInstance = NULL;
static DWORD OMFlags = NULL;
static HDRVR OMDevice = NULL;

// Important stuff
static BOOL DriverInitStatus = FALSE;
static BOOL AlreadyInitializedViaKDMAPI = FALSE;
static BOOL BASSLoadedToMemory = FALSE;
static HANDLE load_sfevent = NULL;
static BOOL ASIOReady = FALSE;
static BOOL EVBuffReady = FALSE;
static BOOL KDMAPIEnabled = FALSE;
static WCHAR SynthNameW[MAXPNAMELEN];		// Synthesizer name

// Stream
static HSTREAM OMStream = NULL;
static BASS_INFO info;
static FLOAT sound_out_volume_float = 1.0;

// GM/GS/XG reset values
static BYTE gs_part_to_ch[16];
static BYTE drum_channels[16];
static const BYTE part_to_ch[16] = { 9, 0, 1, 2, 3, 4, 5, 6, 7, 8, 10, 11, 12, 13, 14, 15 };
static const char sysex_gm_reset[] = { 0xF0, 0x7E, 0x7F, 0x09, 0x01, 0xF7 };
static const char sysex_gs_reset[] = { 0xF0, 0x41, 0x10, 0x42, 0x12, 0x40, 0x00, 0x7F, 0x00, 0x41, 0xF7 };
static const char sysex_xg_reset[] = { 0xF0, 0x43, 0x10, 0x4C, 0x00, 0x00, 0x7E, 0x00, 0xF7 };

// Registry system
#define KEY_READY ERROR_SUCCESS
#define KEY_CLOSED ERROR_INVALID_HANDLE

typedef struct RegKey
{
	HKEY Address = NULL;
	LSTATUS Status = KEY_CLOSED;
};

static RegKey MainKey, Configuration, Channels, ChanOverride, SFDynamicLoader;

static DWORD Blank = 0;
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
static ULONGLONG start1 = 0, start2 = 0, start3 = 0, start4 = 0;
static FLOAT Thread1Usage = 0.0f, Thread2Usage = 0.0f, Thread3Usage = 0.0f, Thread4Usage = 0.0f;

static Thread HealthThread, ATThread, EPThread, DThread;

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

static FLOAT sndbflen = 64.0f;				// AudToWAV
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

static BOOL SettingsManagedByClient = FALSE;
static FLOAT RenderingTime = 0.0f;
static Settings ManagedSettings = DEFAULT_SETTINGS;
static DebugInfo ManagedDebugInfo = DEFAULT_DEBUG;

// Priority values
static const DWORD prioval[7] =
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
static LPCWSTR BuiltInBlacklist[26] =
{
	_T("Battle.net Launcher.exe"),
	_T("LogonUI.exe"),
	_T("NVDisplay.Container.exe"),
	_T("NVIDIA Share.exe"),
	_T("NVIDIA Web Helper.exe"),
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
static DWORD SynthNamesTypes[7] =
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