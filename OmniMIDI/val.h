// OmniMIDI Values

// Audio engines
#define AUDTOWAV 0
#define DSOUND_ENGINE 1
#define ASIO_ENGINE 2
#define WASAPI_ENGINE 3

// MIDI output technology IDs
#define MM_MIDI_MAPPER					1		/* MIDI Mapper */
#define MM_SNDBLST_MIDIOUT				3		/* Sound Blaster MIDI output port */
#define MM_SNDBLST_SYNTH				5		/* Sound Blaster internal synthesizer */
#define MM_ADLIB						9		/* Ad Lib-compatible synthesizer */
#define MM_MPU401_MIDIOUT				10		/* MPU401-compatible MIDI output port */
#define MM_MSFT_WSS_OEM_FMSYNTH_STEREO	20		/* MS OEM Audio Board Stereo FM Synth */. 
#define MM_MSFT_GENERIC_MIDIOUT			26      /*  MS Vanilla driver MIDI  external out  */
#define MM_MSFT_GENERIC_MIDISYNTH		27      /*  MS Vanilla driver MIDI synthesizer  */

// Things
#define SizeOfArray(type) sizeof(type)/sizeof(type[0])

// Settings managed by client
static BOOL AlreadyStartedOnce = FALSE;

typedef struct Settings
{
	BOOL AlternativeCPU = FALSE;			// Autopanic switch
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
	BOOL NoBlacklistMessage = TRUE;			// Disable blacklist message
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

// EVBuffer
struct evbuf_t {
	UINT			uMsg;
	DWORD_PTR		dwParam1;
	DWORD_PTR		dwParam2;
};	// The buffer's structure

static LightweightLock LockSystem;			// LockSystem
static evbuf_t * evbuf;						// The buffer
static volatile ULONGLONG writehead = 0;	// Current write position in the buffer
static volatile ULONGLONG readhead = 0;		// Current read position in the buffer
static volatile LONGLONG eventcount = 0;	// Total events present in the buffer

// Device stuff
static DWORD_PTR OMCallback = NULL;
static DWORD_PTR OMInstance = NULL;
static DWORD OMFlags = NULL;
static HDRVR OMDevice = NULL;

// Important stuff
static volatile BOOL modm_closed = TRUE;
static volatile BOOL reset_synth = FALSE;
static DWORD processPriority = NORMAL_PRIORITY_CLASS;
static HANDLE load_sfevent = NULL;

// Stream
static HSTREAM OMStream = NULL;
static BASS_INFO info;
static FLOAT sound_out_volume_float = 1.0;

// Threads
static volatile BOOL stop_thread = FALSE;
static volatile BOOL stop_rtthread = FALSE;
static ULONGLONG start1, start2, start3, start4;
static FLOAT Thread1Usage, Thread2Usage, Thread3Usage, Thread4Usage;

static HANDLE MainThread = NULL, ATThread = NULL, RTSThread = NULL, EPThread = NULL, DThread = NULL;
static ULONG MainThreadAddress = NULL, ATThreadAddress = NULL, RTSThreadAddress = NULL, EPThreadAddress = NULL, DThreadAddress = NULL;

// Mandatory values
static HINSTANCE hinst = NULL;							// main DLL handle
static HINSTANCE ntdll = NULL;							// ?

static CHAR modulename[MAX_PATH];		// debug info
static CHAR bitapp[MAX_PATH];			// debug info
static HANDLE hPipe = INVALID_HANDLE_VALUE;	// debug info

// Potato
static BOOL EVBuffReady = FALSE;
static BOOL KDMAPIEnabled = FALSE;
static FLOAT RenderingTime = 0.0f;
static ULONGLONG EvBufferSize = 4096;
static ULONGLONG TempEvBufferSize = EvBufferSize;
static DWORD EvBufferMultRatio = 1;
static DWORD GetEvBuffSizeFromRAM = 0;
static WCHAR SynthNameW[MAXPNAMELEN];		// Synthesizer name

// Main values
static INT AudioOutput = -1;				// Audio output (All devices except AudToWAV and ASIO)
static BASS_FX_VOLUME_PARAM ChVolumeStruct;	// Volume
static HFX ChVolume;						// Volume
static DWORD RestartValue = 0;				// For AudToWAV

static HANDLE hConsole;						// Debug console
static FLOAT *sndbf;						// AudToWAV

// Settings and debug
static BOOL SettingsManagedByClient = FALSE;
static Settings ManagedSettings;
static DebugInfo ManagedDebugInfo;

// Priority values
static DWORD prioval[7] =
{
	15,
	15,
	2,
	1,
	0,
	-1,
	-2
};

static DWORD callprioval[7] =
{
	0x00000100,
	0x00000100,
	0x00000080,
	0x00008000,
	0x0000020,
	0x00004000,
	0x0000040
};

// Built-in blacklist
static LPCWSTR builtinblacklist[10] =
{
	_T("Battle.net Launcher.exe"),
	_T("consent.exe"),
	_T("csrss.exe"),
	_T("explorer.exe"),
	_T("mstsc.exe"),
	_T("RustClient.exe"),
	_T("NVIDIA Share.exe"),
	_T("ShellExperienceHost.exe"),
	_T("SndVol.exe"),
	_T("vmware-hostd.exe")
};

// Channels volume
static LPCWSTR cnames[16] =
{
	L"ch1", L"ch2", L"ch3", L"ch4", L"ch5", L"ch6", L"ch7", L"ch8",
	L"ch9", L"ch10", L"ch11", L"ch12", L"ch13", L"ch14", L"ch15", L"ch16"
};

static DWORD cvvalues[16] =
{
	0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0
};

// Channels instruments/banks
static LPCWSTR cbankname[16] =
{
	L"bc1", L"bc2", L"bc3", L"bc4", L"bc5", L"bc6", L"bc7", L"bc8",
	L"bc9", L"bcd", L"bc11", L"bc12", L"bc13", L"bc14", L"bc15", L"bc16"
};

static LPCWSTR cpresetname[16] =
{
	L"pc1", L"pc2", L"pc3", L"pc4", L"pc5", L"pc6", L"pc7", L"pc8",
	L"pc9", L"pcd", L"pc11", L"pc12", L"pc13", L"pc14", L"pc15", L"pc16"
};

static DWORD cbank[16] =
{
	0, 0, 0, 0, 0, 0, 0, 0,
	0, 0, 0, 0, 0, 0, 0, 0
};

static DWORD cpreset[16] =
{
	0, 0, 0, 0, 0, 0, 0, 0,
	0, 0, 0, 0, 0, 0, 0, 0
};

static DWORD SynthType = 5;
static DWORD SynthNamesTypes[11] =
{
	MM_MIDI_MAPPER,
	MOD_SWSYNTH,
	MOD_WAVETABLE,
	MM_MPU401_MIDIOUT,
	MM_SNDBLST_MIDIOUT,
	MOD_MIDIPORT,
	MOD_SYNTH,
	MM_ADLIB,
	MM_SNDBLST_SYNTH,
	MOD_FMSYNTH,
	MOD_SQSYNTH
};

// Channels
static DWORD cvalues[16] =
{
	100, 100, 100, 100, 100, 100, 100, 100,
	100, 100, 100, 100, 100, 100, 100, 100
};

// Reverb and chorus
static DWORD reverb = 64;					// Reverb
static DWORD chorus = 64;					// Chorus

// Watchdog stuff
static LPCWSTR rnames[16] =
{
	L"rel1", L"rel2", L"rel3", L"rel4", L"rel5", L"rel6", L"rel7", L"rel8",
	L"rel9", L"rel10", L"rel11", L"rel2", L"rel13", L"rel14", L"rel15", L"rel16"
};

static DWORD rvalues[16] =
{
	0, 0, 0, 0, 0, 0, 0, 0,
	0, 0, 0, 0, 0, 0, 0, 0
};

// Other
static DWORD buffull = 0;

// Soundfont lists
static TCHAR userprofile[MAX_PATH];

// -----------------------------------------------------------------------

static TCHAR sfdir1[MAX_PATH];
static TCHAR sfdir2[MAX_PATH];
static TCHAR sfdir3[MAX_PATH];
static TCHAR sfdir4[MAX_PATH];
static TCHAR sfdir5[MAX_PATH];
static TCHAR sfdir6[MAX_PATH];
static TCHAR sfdir7[MAX_PATH];
static TCHAR sfdir8[MAX_PATH];
static TCHAR sfdir9[MAX_PATH];
static TCHAR sfdir10[MAX_PATH];
static TCHAR sfdir11[MAX_PATH];
static TCHAR sfdir12[MAX_PATH];
static TCHAR sfdir13[MAX_PATH];
static TCHAR sfdir14[MAX_PATH];
static TCHAR sfdir15[MAX_PATH];
static TCHAR sfdir16[MAX_PATH];

static TCHAR * sflistloadme[16] =
{
	sfdir1, sfdir2, sfdir3, sfdir4, sfdir5, sfdir6, sfdir7, sfdir8,
	sfdir9, sfdir10, sfdir11, sfdir12, sfdir13, sfdir14, sfdir15, sfdir16
};

static TCHAR * sfdirs[16] =
{
	L"\\OmniMIDI\\lists\\OmniMIDI_A.sflist",
	L"\\OmniMIDI\\lists\\OmniMIDI_B.sflist",
	L"\\OmniMIDI\\lists\\OmniMIDI_C.sflist",
	L"\\OmniMIDI\\lists\\OmniMIDI_D.sflist",
	L"\\OmniMIDI\\lists\\OmniMIDI_E.sflist",
	L"\\OmniMIDI\\lists\\OmniMIDI_F.sflist",
	L"\\OmniMIDI\\lists\\OmniMIDI_G.sflist",
	L"\\OmniMIDI\\lists\\OmniMIDI_H.sflist",
	L"\\OmniMIDI\\lists\\OmniMIDI_I.sflist",
	L"\\OmniMIDI\\lists\\OmniMIDI_L.sflist",
	L"\\OmniMIDI\\lists\\OmniMIDI_M.sflist",
	L"\\OmniMIDI\\lists\\OmniMIDI_N.sflist",
	L"\\OmniMIDI\\lists\\OmniMIDI_O.sflist",
	L"\\OmniMIDI\\lists\\OmniMIDI_P.sflist",
	L"\\OmniMIDI\\lists\\OmniMIDI_Q.sflist",
	L"\\OmniMIDI\\lists\\OmniMIDI_R.sflist"
};

// -----------------------------------------------------------------------

static TCHAR listsloadme1[MAX_PATH];
static TCHAR listsloadme2[MAX_PATH];
static TCHAR listsloadme3[MAX_PATH];
static TCHAR listsloadme4[MAX_PATH];
static TCHAR listsloadme5[MAX_PATH];
static TCHAR listsloadme6[MAX_PATH];
static TCHAR listsloadme7[MAX_PATH];
static TCHAR listsloadme8[MAX_PATH];
static TCHAR listsloadme9[MAX_PATH];
static TCHAR listsloadme10[MAX_PATH];
static TCHAR listsloadme11[MAX_PATH];
static TCHAR listsloadme12[MAX_PATH];
static TCHAR listsloadme13[MAX_PATH];
static TCHAR listsloadme14[MAX_PATH];
static TCHAR listsloadme15[MAX_PATH];
static TCHAR listsloadme16[MAX_PATH];

static TCHAR * listsloadme[16] =
{
	listsloadme1, listsloadme2, listsloadme3, listsloadme4, listsloadme5, listsloadme6, listsloadme7, listsloadme8,
	listsloadme9, listsloadme10, listsloadme11, listsloadme12, listsloadme13, listsloadme14, listsloadme15, listsloadme16
};

static TCHAR * listsanalyze[16] =
{
	L"\\OmniMIDI\\applists\\OmniMIDI_A.applist",
	L"\\OmniMIDI\\applists\\OmniMIDI_B.applist",
	L"\\OmniMIDI\\applists\\OmniMIDI_C.applist",
	L"\\OmniMIDI\\applists\\OmniMIDI_D.applist",
	L"\\OmniMIDI\\applists\\OmniMIDI_E.applist",
	L"\\OmniMIDI\\applists\\OmniMIDI_F.applist",
	L"\\OmniMIDI\\applists\\OmniMIDI_G.applist",
	L"\\OmniMIDI\\applists\\OmniMIDI_H.applist",
	L"\\OmniMIDI\\applists\\OmniMIDI_I.applist",
	L"\\OmniMIDI\\applists\\OmniMIDI_L.applist",
	L"\\OmniMIDI\\applists\\OmniMIDI_M.applist",
	L"\\OmniMIDI\\applists\\OmniMIDI_N.applist",
	L"\\OmniMIDI\\applists\\OmniMIDI_O.applist",
	L"\\OmniMIDI\\applists\\OmniMIDI_P.applist",
	L"\\OmniMIDI\\applists\\OmniMIDI_Q.applist",
	L"\\OmniMIDI\\applists\\OmniMIDI_R.applist"
};

// -----------------------------------------------------------------------

static std::vector<DWORD> _soundFonts;
static std::vector<BASS_MIDI_FONTEX> presetList;

// -----------------------------------------------------------------------

static DWORD pitchshiftchan[16] = { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };
static LPCWSTR pitchshiftname[16] =
{
	L"ch1pshift", L"ch2pshift", L"ch3pshift", L"ch4pshift", L"ch5pshift",
	L"ch6pshift", L"ch7pshift", L"ch8pshift", L"ch9pshift", L"ch10pshift",
	L"ch11pshift", L"ch12pshift", L"ch13pshift", L"ch14pshift", L"ch15pshift", 
	L"ch16pshift"
};

//

BOOL CheckNullChar(CHAR* scanme) {
	for (int i = 0; i != sizeof(scanme); i++) {
		if (scanme[i] == '\0') {
			return TRUE;
			break;
		}
	}
	return FALSE;
}

BOOL CheckNullWChar(WCHAR* scanme) {
	for (int i = 0; i != sizeof(scanme); i++) {
		if (scanme[i] == L'\0') {
			return TRUE;
			break;
		}
	}
	return FALSE;
}