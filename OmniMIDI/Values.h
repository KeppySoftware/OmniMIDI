// OmniMIDI Values

// Device status
#define DEVICE_UNAVAILABLE 0
#define DEVICE_AVAILABLE 1

// Things
UINT CPUThreadsAvailable = 0;

#define LOADLIBFUNCTION(l, f) *((void**)&f)=GetProcAddress(l,#f)

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

#define MMI(f)			{&MM##f, #f}		// OWINMM

// path
#define NTFS_MAX_PATH	32767

// Settings
FILE* DebugLog = nullptr;
BOOL SettingsManagedByClient;
Settings ManagedSettings = Settings();
DebugInfo ManagedDebugInfo = DebugInfo();

// Settings managed by client
BOOL AlreadyStartedOnce = FALSE;

// EVBuffer
typedef struct EventsBuffer {
	DWORD*				Buffer;
	QWORD				BufSize;
	volatile ULONGLONG	ReadHead;
	volatile ULONGLONG	WriteHead;
};
// The buffer's structure
EventsBuffer EVBuffer;							// The buffer
DWORD LastRunningStatus = 0;				// Last running status
QWORD EvBufferSize = 4096;
DWORD EvBufferMultRatio = 1;
DWORD GetEvBuffSizeFromRAM = 0;

// BASS lib
typedef struct OMLib {
	HMODULE Lib = NULL;
	BOOL AppOwnDLL = FALSE;
};
OMLib BASS = {}, BASSWASAPI = {}, BASSASIO = {}, BASSENC = {}, BASSMIDI = {}, BASS_VST = {};
HPLUGIN bassflac = NULL, basswv = NULL, bassopus = NULL;

// Cooked player struct
typedef struct CookedPlayer
{
	MIDIHDR* MIDIHeaderQueue;			// MIDIHDR buffer
	BOOL Paused;						// Is the player paused?
	DWORD Tempo;						// Player tempo
	DWORD TimeDiv;						// Player time division
	DWORD TempoMulti;					// Player time multiplier
	DWORD TimeAccumulator;				// ?
	DWORD ByteAccumulator;				// ?
	DWORD TickAccumulator;				// ?
	LockSystem Lock;					// LockSystem
	DWORD_PTR dwInstance;
} CookedPlayer, *LPCookedPlayer;
// CookedPlayer
BOOL CookedPlayerHasToGo = FALSE;
CookedPlayer* OMCookedPlayer;

// Device stuff
const GUID OMCLSID = { 0x62F3192B, 0xA961, 0x456D, { 0xAB, 0xCA, 0xA5, 0xC9, 0x5A, 0x14, 0xB9, 0xAA } };
static ULONGLONG TickStart = 0;			// For TGT64
static HSTREAM OMStream = NULL;
static HANDLE OMReady = NULL, LiveChanges = NULL, ATThreadDone = NULL, EPThreadDone = NULL;
static HMIDI OMHMIDI = NULL, OMFeedback = NULL;
static HDRVR OMHDRVR = NULL;
static DWORD_PTR OMCallback = NULL;
static DWORD_PTR OMInstance = NULL;

// Important stuff
const std::locale UTF8Support(std::locale(), new std::codecvt_utf8<wchar_t>);
DOUBLE SpeedHack = 1.0;
BOOL DriverInitStatus = FALSE;
BOOL AlreadyInitializedViaKDMAPI = FALSE;
BOOL BASSLoadedToMemory = FALSE;
BOOL KDMAPIEnabled = FALSE;
BOOL IsKDMAPIViaWinMM = FALSE;
BOOL HostSessionMode = FALSE;

// Check if init
BOOL ASIOInit = FALSE;

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
BOOL stop_svthread = FALSE;

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
BASS_FX_VOLUME_PARAM ChVolumeStruct;	// Volume
HFX ChVolume;						// Volume
DWORD RestartValue = 0;				// For AudToWAV
BOOL UnlimitedChannels = 0;			// For KDMAPI

// XA Engine
static sound_out* SndDrv = 0;
static float* SndBuf = 0;
int SamplesPerFrame = ManagedSettings.XASamplesPerFrame * 2;

// Settings and debug
wchar_t ListToLoad[NTFS_MAX_PATH] = { 0 };
typedef struct SoundFontList
{
	int EnableState;
	int Preload;
	wchar_t Path[NTFS_MAX_PATH];
	int SourcePreset;
	int SourceBank;
	int DestinationPreset;
	int DestinationBank;
	int DestinationBankLSB;
	int XGBankMode;
	int NoLimits;
	int LinearDecayVol;
	int LinearAttackVol;
	int NoRampIn;
};

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
	_T("RtkAudUService32.exe"),
	_T("RtkAudUService64.exe"),
	_T("smss.exe"),
	_T("spoolsv.exe"),
	_T("VBoxSDS.exe"),
	_T("VBoxSVC.exe"),
	_T("vcpkgsrv.exe"),
	_T("VirtualBox.exe"),
	_T("VirtualBoxVM.exe"),
	_T("vmware-hostd.exe"),
	_T("vmware-vmx.exe"),
	_T("wininit.exe"),
	_T("winlogon.exe"),
	_T("wlanext.exe"),
};

// Per channel values
DWORD cvalues[16];		// Volume setting per channel.
DWORD cbank[16];			// MIDI bank setting per channel.
DWORD cpreset[16];		// MIDI preset setting for... you guess it!

DWORD SynthType = MOD_MIDIPORT;
const WORD SynthNamesTypes[7] =
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
const wchar_t* OMLetters[7] = { L"A", L"B", L"C", L"D", L"E", L"F", L"G" };

// -----------------------------------------------------------------------

std::vector<HSOUNDFONT> SoundFontHandles;
std::vector<BASS_MIDI_FONTEX> SoundFontPresets;

// -----------------------------------------------------------------------

DWORD pitchshiftchan[16];

// -----------------------------------------------------------------------

// NTSTATUS
#define NT_SUCCESS(StatCode) ((NTSTATUS)(StatCode) == 0)
#define NTAPI __stdcall
// these functions have identical prototypes
typedef NTSTATUS(NTAPI* NDE)(BOOLEAN, INT64*);
typedef NTSTATUS(NTAPI* NQST)(QWORD*);
typedef NTSTATUS(NTAPI* DDP)(DWORD, HANDLE, UINT, LONG, LONG);

NDE NtDelayExecution = 0;
NQST NtQuerySystemTime = 0;
DDP DefDriverProcImp = 0;

// Critical sections but handled by OmniMIDI functions because f**k Windows
DWORD DummyPlayBufData() noexcept { return 0; };
VOID DummyPrepareForBASSMIDI(DWORD, DWORD_PTR) noexcept { return; };
MMRESULT DummyParseData(DWORD_PTR) noexcept { return MMSYSERR_NOERROR; };
BOOL WINAPI DummyBMSE(HSTREAM, DWORD, DWORD, DWORD) noexcept { return TRUE; };
DWORD CALLBACK DummyProcData(void*, DWORD, void*) noexcept { return 0; };

// Hyper switch
BOOL HyperMode = 0;
MMRESULT(*_PrsData)(DWORD_PTR dwParam1) = DummyParseData;
VOID(*_PforBASSMIDI)(DWORD LastRunningStatus, DWORD_PTR dwParam1) = DummyPrepareForBASSMIDI;
DWORD(*_PlayBufData)(void) = DummyPlayBufData;
DWORD(*_PlayBufDataChk)(void) = DummyPlayBufData;
BOOL(WINAPI* _BMSE)(HSTREAM handle, DWORD chan, DWORD event, DWORD param) = DummyBMSE;
DWORD(CALLBACK* _ProcData)(void* buffer, DWORD length, void* user) = DummyProcData;
// What does it do? It gets rid of the useless functions,
// and passes the events without checking for anything

// ----------------------------------------------------------------------

// OWINMM
typedef HMODULE(WINAPI* GO)();
typedef UINT(WINAPI* SGV)();
typedef MMRESULT(WINAPI* MOC)(HMIDIOUT);
typedef MMRESULT(WINAPI* MOO)(LPHMIDIOUT, UINT, DWORD_PTR, DWORD_PTR, DWORD);
typedef MMRESULT(WINAPI* MOSM)(HMIDIOUT, DWORD);
typedef MMRESULT(WINAPI* MOLM)(HMIDIOUT, LPMIDIHDR, UINT);
typedef DWORD(WINAPI* MOGND)();
typedef MMRESULT(WINAPI* MOGDCW)(UINT_PTR, LPMIDIOUTCAPSW, UINT);

BOOL FeedbackBlacklisted = FALSE;
HMODULE owinmm = NULL;					// ?
GO GetOWINMM = 0;
SGV SystemGetVersion = 0;
MOC MMmidiOutClose = 0;
MOO MMmidiOutOpen = 0;
MOSM MMmidiOutShortMsg = 0;
MOLM MMmidiOutLongMsg = 0;
MOGND MMmidiOutGetNumDevs = 0;
MOGDCW MMmidiOutGetDevCapsW = 0;