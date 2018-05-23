// Keppy's Synthesizer Values

// Audio engines
#define AUDTOWAV 0
#define DSOUND_ENGINE 1
#define ASIO_ENGINE 2
#define WASAPI_ENGINE 3

// Things
#define SizeOfArray(type) sizeof(type)/sizeof(type[0])

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
static DWORD_PTR KSCallback = NULL;
static DWORD_PTR KSInstance = NULL;
static DWORD KSFlags = NULL;
static HDRVR KSDevice = NULL;

// Important stuff
static volatile BOOL modm_closed = TRUE;
static volatile BOOL reset_synth = FALSE;
static DWORD processPriority = NORMAL_PRIORITY_CLASS;
static HANDLE load_sfevent = NULL;

// Stream
static DWORD KSStream = NULL;
static BASS_INFO info;
static FLOAT sound_out_volume_float = 1.0;

// Threads
static volatile BOOL stop_thread = FALSE;
static volatile BOOL stop_rtthread = FALSE;
static ULONGLONG start1, start2, start3, start4;
static FLOAT Thread1Usage, Thread2Usage, Thread3Usage, Thread4Usage;
static HANDLE hCalcThread = NULL, hThread2 = NULL, hThread3 = NULL, hThread4 = NULL, hThreadDBG = NULL;
static ULONG thrdaddrC = NULL, thrdaddr2 = NULL, thrdaddr3 = NULL, thrdaddr4 = NULL, thrdaddrDBG = NULL;
static BOOL hThread2Running = FALSE, hThread3Running = FALSE, hThread4Running = FALSE, hThreadDBGRunning = FALSE;

// Mandatory values
static HINSTANCE hinst = NULL;							// main DLL handle

static CHAR modulename[MAX_PATH];		// debug info
static CHAR bitapp[MAX_PATH];			// debug info
static HANDLE hPipe = INVALID_HANDLE_VALUE;	// debug info

// Potato
static BOOL bufferinitialized = FALSE;
static BOOL ksdirectenabled = FALSE;
static BOOL bufferoverload = FALSE;
static FLOAT currentcpuusage0;
static DWORD isoverrideenabled = 0;
static ULONGLONG evbuffsize = 4096;
static ULONGLONG sevbuffsize = evbuffsize;
static DWORD evbuffratio = 1;
static DWORD evbuffbyram = 0;

// Main values
static BASS_FX_VOLUME_PARAM ChVolumeStruct;	// Volume (whole)
static HFX ChVolume;						// Volume (whole)

static HANDLE hConsole;						// Debug console
static FLOAT *sndbf;						// Cake
static INT AudioOutput = 0;					// Audio output (All devices except AudToWAV and WASAPI)
static DWORD allhotkeys = 0;				// Enable/Disable all the hotkeys
static DWORD allnotesignore = 0;			// Ignore all MIDI events
static DWORD alreadyshown = 0;				// Check if the info about the drivers have been already shown.
static DWORD autopanic = 0;					// Autopanic switch
static DWORD bassoutputfinal = 0;			// DO NOT TOUCH
static DWORD capframerate = 0;				// Cap input framerate
static DWORD currentengine = WASAPI_ENGINE;	// Current engine
static DWORD debugmode = 0;					// Debug console
static DWORD defaultAoutput = 0;			// Default audio output (ASIO)
static DWORD defaultmidiindev = 0;			// MIDI Input device
static DWORD defaultmidiout = 0;			// Set as default MIDI out device for 8.x or newer
static DWORD defaultoutput = 0;				// Default audio output (DSound)
static DWORD defaultsflist = 1;				// Default soundfont list
static DWORD driverprio = 0;				// Process priority
static DWORD fadeoutdisable = 0;			// Disable fade-out
static DWORD floatrendering = 1;			// Floating postatic DWORD audio
static DWORD frames = 0;					// Default
static DWORD frequency = 0;					// Audio frequency
static DWORD fullvelocity = 0;				// Enable full velocity mode
static DWORD ignorenotes1 = 0;				// Ignores notes with velocity of 1
static DWORD ischangingbuffermode = 0;		// Stuff
static DWORD limit88 = 0;					// Limit to 88 keys
static DWORD livechange = 0;				// Live changes
static DWORD maxcpu = 0;					// CPU usage INT
static DWORD midiinenabled = 0;				// MIDI Input
static DWORD midivoices = 0;				// Max voices INT
static DWORD midivolumeoverride = 0;		// MIDI track volume override
static DWORD monorendering = 0;				// Mono rendering (Instead of stereo by default)
static DWORD mt32mode = 0;					// Roland MT-32 mode
static DWORD newsndbfvalue = 128;			// DO NOT TOUCH
static DWORD noblacklistmsg = 0;			// Disable blacklist message
static DWORD noFLOAT = 1;					// Enable or disable the FLOAT engine
static DWORD nofx = 0;						// Enable or disable FXs
static DWORD noteoff1 = 0;					// Note cut INT
static BOOL oldbuffermode = TRUE;			// For old-ass PCs
static DWORD overrideinstruments = 0;		// Override channel instruments
static DWORD pitchshift = 127;				// Pitch shift
static DWORD preload = 0;					// Soundfont preloading
static DWORD rco = 0;						// Reduce CPU overhead
static DWORD restartvalue = 0;				// How many times you changed the settings in real-time
static DWORD shortname = 0;					// Use short name or nah
static DWORD sinc = 0;						// Sinc
static DWORD sincconv = 2;					// Sinc
static DWORD sysexignore = 0;				// Ignore SysEx events
static DWORD sysresetignore = 0;			// Ignore sysex messages
static DWORD vms2emu = 0;					// VirtualMIDISynth 2.x buffer emulation
static DWORD volume = 0;					// Volume limit
static DWORD volumehotkeys = 1;				// Enable/Disable volume hotkeys
static DWORD volumemon = 1;					// Volume monitoring

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

static DWORD selectedtype = 4;
static DWORD SynthNamesTypes[7] =
{
	MOD_FMSYNTH,
	MOD_SYNTH,
	MOD_WAVETABLE,
	MOD_MAPPER,
	MOD_MIDIPORT,
	MOD_SWSYNTH,
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
static DWORD extra8lists = 0;
static DWORD lovel = 1;
static DWORD hivel = 1;

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
	L"\\Keppy's Synthesizer\\lists\\keppymidi.sflist",
	L"\\Keppy's Synthesizer\\lists\\keppymidib.sflist",
	L"\\Keppy's Synthesizer\\lists\\keppymidic.sflist",
	L"\\Keppy's Synthesizer\\lists\\keppymidid.sflist",
	L"\\Keppy's Synthesizer\\lists\\keppymidie.sflist",
	L"\\Keppy's Synthesizer\\lists\\keppymidif.sflist",
	L"\\Keppy's Synthesizer\\lists\\keppymidig.sflist",
	L"\\Keppy's Synthesizer\\lists\\keppymidih.sflist",
	L"\\Keppy's Synthesizer\\lists\\keppymidii.sflist",
	L"\\Keppy's Synthesizer\\lists\\keppymidij.sflist",
	L"\\Keppy's Synthesizer\\lists\\keppymidik.sflist",
	L"\\Keppy's Synthesizer\\lists\\keppymidil.sflist",
	L"\\Keppy's Synthesizer\\lists\\keppymidim.sflist",
	L"\\Keppy's Synthesizer\\lists\\keppymidin.sflist",
	L"\\Keppy's Synthesizer\\lists\\keppymidio.sflist",
	L"\\Keppy's Synthesizer\\lists\\keppymidip.sflist"
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
	L"\\Keppy's Synthesizer\\applists\\keppymidi.applist",
	L"\\Keppy's Synthesizer\\applists\\keppymidib.applist",
	L"\\Keppy's Synthesizer\\applists\\keppymidic.applist",
	L"\\Keppy's Synthesizer\\applists\\keppymidid.applist",
	L"\\Keppy's Synthesizer\\applists\\keppymidie.applist",
	L"\\Keppy's Synthesizer\\applists\\keppymidif.applist",
	L"\\Keppy's Synthesizer\\applists\\keppymidig.applist",
	L"\\Keppy's Synthesizer\\applists\\keppymidih.applist",
	L"\\Keppy's Synthesizer\\applists\\keppymidii.applist",
	L"\\Keppy's Synthesizer\\applists\\keppymidij.applist",
	L"\\Keppy's Synthesizer\\applists\\keppymidik.applist",
	L"\\Keppy's Synthesizer\\applists\\keppymidil.applist",
	L"\\Keppy's Synthesizer\\applists\\keppymidim.applist",
	L"\\Keppy's Synthesizer\\applists\\keppymidin.applist",
	L"\\Keppy's Synthesizer\\applists\\keppymidio.applist",
	L"\\Keppy's Synthesizer\\applists\\keppymidip.applist"
};

// -----------------------------------------------------------------------

std::vector<DWORD> _soundFonts[16];
std::vector<BASS_MIDI_FONTEX> presetList[16];

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