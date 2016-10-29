// Keppy's Synthesizer Values

// Mandatory values
static int decoded;
static sound_out * sound_driver = NULL;
static HINSTANCE hinst = NULL;			//main DLL handle

static HINSTANCE bass = 0;				// bass handle
static HINSTANCE bassmidi = 0;			// bassmidi handle
static HINSTANCE bassenc = 0;			// bassenc handle
static HINSTANCE bass_vst = 0;			// bass_vst handle

// Main values
static float *sndbf;
static int allhotkeys = 0;				// Enable/Disable all the hotkeys
static int autopanic = 1;				// Autopanic switch
static int bassoutputfinal = 0;			// DO NOT TOUCH
static int defaultsflist = 1;			// Default soundfont list
static int encmode = 0;					// Encoder mode
static int floatrendering = 1;			// Floating point audio
static int frames = 0;					// Default
static int frequency = 0;				// Audio frequency
static int tremoliov = 0;				// Yes
static int frequencynew = 0;			// Audio frequency
static int maxcpu = 0;					// CPU usage INT
static int midivoices = 0;				// Max voices INT
static int midivolumeoverride = 0;		// MIDI track volume override
static int newsndbfvalue;				// DO NOT TOUCH
static int newevbuffvalue = 64;			// DO NOT TOUCH
static int oldbuffermode = 0;			// For old-ass PCs
static int nofloat = 1;					// Enable or disable the float engine
static int nofx = 0;					// Enable or disable FXs
static int rco = 0;						// Reduce CPU overhead
static int shortname = 0;				// Use short name or nah
static int noteoff1 = 0;				// Note cut INT
static int preload = 0;					// Soundfont preloading
static int sysexignore = 0;				// Ignore SysEx events
static int allnotesignore = 0;			// Ignore all MIDI events
static int defaultmidiout = 0;			// Set as default MIDI out device for 8.x or newer
static int sinc = 0;					// Sinc
static int sysresetignore = 0;			//Ignore sysex messages
static int tracks = 0;					// Tracks limit
static int vmsemu = 0;					// VirtualMIDISynth buffer emulation
static int vms2emu = 0;					// VirtualMIDISynth 2.x buffer emulation
static int volume = 0;					// Volume limit
static int volumehotkeys = 1;			// Enable/Disable volume hotkeys
static int volumemon = 1;				// Volume monitoring
static int xaudiodisabled = 0;			// Override the default engine
static int debugmode = 0;				// Debug console
static HANDLE hConsole;					// Debug console

// Channels volume
static LPCWSTR cnames[16] =
{
	L"ch1", L"ch2", L"ch3", L"ch4", L"ch5", L"ch6", L"ch7", L"ch8",
	L"ch9", L"ch10", L"ch11", L"ch2", L"ch13", L"ch14", L"ch15", L"ch16"
};

static int cvalues[16] =
{
	16383, 16383, 16383, 16383, 16383, 16383, 16383, 16383,
	16383, 16383, 16383, 16383, 16383, 16383, 16383, 16383
};

static int tcvalues[16] =
{
	16383, 16383, 16383, 16383, 16383, 16383, 16383, 16383,
	16383, 16383, 16383, 16383, 16383, 16383, 16383, 16383
};

// Watchdog stuff
static LPCWSTR rnames[16] =
{
	L"rel1", L"rel2", L"rel3", L"rel4", L"rel5", L"rel6", L"rel7", L"rel8",
	L"rel9", L"rel10", L"rel11", L"rel2", L"rel13", L"rel14", L"rel15", L"rel16"
};

static int rvalues[16] =
{
	0, 0, 0, 0, 0, 0, 0, 0,
	0, 0, 0, 0, 0, 0, 0, 0
};

// Other
static int buffull = 0;
static int extra8lists = 0;

// Buffer system
static BYTE gs_part_to_ch[16];
static BYTE drum_channels[16];
static const char sysex_gm_reset[] = { 0xF0, 0x7E, 0x7F, 0x09, 0x01, 0xF7 };
static const char sysex_gs_reset[] = { 0xF0, 0x41, 0x10, 0x42, 0x12, 0x40, 0x00, 0x7F, 0x00, 0x41, 0xF7 };
static const char sysex_xg_reset[] = { 0xF0, 0x43, 0x10, 0x4C, 0x00, 0x00, 0x7E, 0x00, 0xF7 };

// Soundfont lists
static TCHAR defaultstring[MAX_PATH];
static TCHAR listanalyze1[MAX_PATH];
static TCHAR listanalyze2[MAX_PATH];
static TCHAR listanalyze3[MAX_PATH];
static TCHAR listanalyze4[MAX_PATH];
static TCHAR listanalyze5[MAX_PATH];
static TCHAR listanalyze6[MAX_PATH];
static TCHAR listanalyze7[MAX_PATH];
static TCHAR listanalyze8[MAX_PATH];
static TCHAR listanalyze9[MAX_PATH];
static TCHAR listanalyze10[MAX_PATH];
static TCHAR listanalyze11[MAX_PATH];
static TCHAR listanalyze12[MAX_PATH];
static TCHAR listanalyze13[MAX_PATH];
static TCHAR listanalyze14[MAX_PATH];
static TCHAR listanalyze15[MAX_PATH];
static TCHAR listanalyze16[MAX_PATH];

static TCHAR * listsanalyze[16] =
{
	listanalyze1, listanalyze2, listanalyze3, listanalyze4, listanalyze5, listanalyze6, listanalyze7, listanalyze8,
	listanalyze9, listanalyze10, listanalyze11, listanalyze12, listanalyze13, listanalyze14, listanalyze15, listanalyze16
};

std::vector<HSOUNDFONT> _soundFonts[16];
std::vector<BASS_MIDI_FONTEX> presetList[16];