// Windows Multimedia original functions, used to redirect unedited functions without relying on definition files

// WinMM funcs, just replace MM with "midiOut" to get the real version
HMODULE OWINMM = NULL;
HMODULE KERNEL32 = NULL;
DWORD(WINAPI* INtimeGetTime)() = 0;

// Wine check
const char* (WINAPI* WGBI)(void) = 0;
INT WDummy = 0xFFFFF;
HMIDI OMDummy = (HMIDI)0x1001;
DWORD_PTR OMUser;

// MIDI out stuff
MMRESULT(WINAPI* MMmidiStreamClose)(HMIDISTRM) = 0;
MMRESULT(WINAPI* MMmidiStreamOpen)(LPHMIDISTRM, LPUINT, DWORD, DWORD_PTR, DWORD_PTR, DWORD) = 0;
MMRESULT(WINAPI* MMmidiStreamOut)(HMIDISTRM, LPMIDIHDR, UINT) = 0;
MMRESULT(WINAPI* MMmidiStreamPause)(HMIDISTRM) = 0;
MMRESULT(WINAPI* MMmidiStreamPosition)(HMIDISTRM, LPMMTIME, UINT) = 0;
MMRESULT(WINAPI* MMmidiStreamProperty)(HMIDISTRM, LPBYTE, DWORD) = 0;
MMRESULT(WINAPI* MMmidiStreamRestart)(HMIDISTRM) = 0;
MMRESULT(WINAPI* MMmidiStreamStop)(HMIDISTRM) = 0;
MMRESULT(WINAPI* MMmidiOutCacheDrumPatches)(HMIDIOUT, UINT, LPWORD, UINT) = 0;
MMRESULT(WINAPI* MMmidiOutCachePatches)(HMIDIOUT, UINT, LPWORD, UINT) = 0;
MMRESULT(WINAPI* MMmidiOutClose)(HMIDIOUT) = 0;
MMRESULT(WINAPI* MMmidiOutGetDevCapsA)(UINT_PTR, LPMIDIOUTCAPSA, UINT) = 0;
MMRESULT(WINAPI* MMmidiOutGetDevCapsW)(UINT_PTR, LPMIDIOUTCAPSW, UINT) = 0;
MMRESULT(WINAPI* MMmidiOutGetErrorTextA)(MMRESULT, LPSTR, UINT) = 0;
MMRESULT(WINAPI* MMmidiOutGetErrorTextW)(MMRESULT, LPWSTR, UINT) = 0;
MMRESULT(WINAPI* MMmidiOutGetID)(HMIDIOUT, LPUINT) = 0;
MMRESULT(WINAPI* MMmidiOutGetVolume)(HMIDIOUT, LPDWORD) = 0;
MMRESULT(WINAPI* MMmidiOutLongMsg)(HMIDIOUT, LPMIDIHDR, UINT) = 0;
MMRESULT(WINAPI* MMmidiOutMessage)(HMIDIOUT, UINT, DWORD_PTR, DWORD_PTR) = 0;
MMRESULT(WINAPI* MMmidiOutOpen)(LPHMIDIOUT, UINT, DWORD_PTR, DWORD_PTR, DWORD) = 0;
MMRESULT(WINAPI* MMmidiOutPrepareHeader)(HMIDIOUT, LPMIDIHDR, UINT) = 0;
MMRESULT(WINAPI* MMmidiOutReset)(HMIDIOUT) = 0;
MMRESULT(WINAPI* MMmidiOutSetVolume)(HMIDIOUT, DWORD) = 0;
MMRESULT(WINAPI* MMmidiOutShortMsg)(HMIDIOUT, DWORD) = 0;
MMRESULT(WINAPI* MMmidiOutUnprepareHeader)(HMIDIOUT, LPMIDIHDR, UINT) = 0;
UINT(WINAPI* MMmidiOutGetNumDevs)() = 0;
// MIDI out stuff

// MIDI in stuff
MMRESULT(WINAPI* MMmidiConnect)(HMIDI, HMIDIIN, LPVOID) = 0;
MMRESULT(WINAPI* MMmidiDisconnect)(HMIDI, HMIDIIN, LPVOID) = 0;
MMRESULT(WINAPI* MMmidiInAddBuffer)(HMIDIIN, LPMIDIHDR, UINT) = 0;
MMRESULT(WINAPI* MMmidiInClose)(HMIDIIN) = 0;
MMRESULT(WINAPI* MMmidiInGetDevCapsA)(UINT_PTR, LPMIDIINCAPSA, UINT) = 0;
MMRESULT(WINAPI* MMmidiInGetDevCapsW)(UINT_PTR, LPMIDIINCAPSW, UINT) = 0;
MMRESULT(WINAPI* MMmidiInGetErrorTextA)(MMRESULT, LPSTR, UINT) = 0;
MMRESULT(WINAPI* MMmidiInGetErrorTextW)(MMRESULT, LPWSTR, UINT) = 0;
MMRESULT(WINAPI* MMmidiInGetID)(HMIDIIN, LPUINT) = 0;
MMRESULT(WINAPI* MMmidiInMessage)(HMIDIIN, UINT, DWORD_PTR, DWORD_PTR) = 0;
MMRESULT(WINAPI* MMmidiInOpen)(LPHMIDIIN, UINT, DWORD_PTR, DWORD_PTR, DWORD) = 0;
MMRESULT(WINAPI* MMmidiInPrepareHeader)(HMIDIIN, LPMIDIHDR, UINT) = 0;
MMRESULT(WINAPI* MMmidiInReset)(HMIDIIN) = 0;
MMRESULT(WINAPI* MMmidiInStart)(HMIDIIN) = 0;
MMRESULT(WINAPI* MMmidiInStop)(HMIDIIN) = 0;
MMRESULT(WINAPI* MMmidiInUnprepareHeader)(HMIDIIN, LPMIDIHDR, UINT) = 0;
UINT(WINAPI* MMmidiInGetNumDevs)() = 0;
// MIDI in stuff

// Time related stuff
DWORD(WINAPI* MMtimeGetTime)() = 0;
MMRESULT(WINAPI* MMtimeBeginPeriod)(UINT) = 0;
MMRESULT(WINAPI* MMtimeEndPeriod)(UINT) = 0;
MMRESULT(WINAPI* MMtimeGetDevCaps)(LPTIMECAPS, UINT) = 0;
MMRESULT(WINAPI* MMtimeGetSystemTime)(LPMMTIME, UINT) = 0;
MMRESULT(WINAPI* MMtimeKillEvent)(UINT) = 0;
MMRESULT(WINAPI* MMtimeSetEvent)(UINT, UINT, LPTIMECALLBACK, DWORD_PTR, UINT) = 0;
// Time related stuff

// Driver stuff

HDRVR(WINAPI* MMOpenDriver)(_In_ LPCWSTR, _In_ LPCWSTR, _In_ LPARAM) = 0;
LRESULT(WINAPI* MMCloseDriver)(_In_ HDRVR, _In_ LPARAM, _In_ LPARAM) = 0;
LRESULT(WINAPI* MMSendDriverMessage)(_In_ HDRVR, _In_ UINT, _Inout_ LPARAM, _Inout_ LPARAM) = 0;
BOOL(WINAPI* MMmmDrvInstall)(LPCSTR, LPCSTR, BOOL) = 0;
DWORD(WINAPI* MMmmGetCurrentTask)() = 0;
HMODULE(WINAPI* MMDrvGetModuleHandle)(_In_ HDRVR) = 0;
HMODULE(WINAPI* MMGetDriverModuleHandle)(_In_ HDRVR) = 0;
LRESULT(WINAPI* MMDefDriverProc)(DWORD_PTR, HDRVR, UINT, LONG, LONG) = 0;
LRESULT(WINAPI* MMDriverCallback)(DWORD, DWORD, HDRVR, DWORD, DWORD, DWORD, DWORD) = 0;
// Driver stuff

// Game-related stuff
MMRESULT(WINAPI* MMjoyConfigChanged)(DWORD) = 0;
MMRESULT(WINAPI* MMjoyGetDevCapsA)(UINT, LPJOYCAPSA, UINT) = 0;
MMRESULT(WINAPI* MMjoyGetDevCapsW)(UINT, LPJOYCAPSW, UINT) = 0;
MMRESULT(WINAPI* MMjoyGetPos)(UINT, LPJOYINFO) = 0;
MMRESULT(WINAPI* MMjoyGetPosEx)(UINT, LPJOYINFOEX) = 0;
MMRESULT(WINAPI* MMjoyGetThreshold)(UINT, LPUINT) = 0;
MMRESULT(WINAPI* MMjoySetThreshold)(UINT, UINT) = 0;
MMRESULT(WINAPI* MMjoyReleaseCapture)(UINT) = 0;
MMRESULT(WINAPI* MMjoySetCapture)(HWND, UINT, UINT, BOOL) = 0;
UINT(WINAPI* MMjoyGetNumDevs)() = 0;
// Game related stuff

// MCI stuff
BOOL(WINAPI* MMmciDriverNotify)(HWND, UINT, UINT) = 0;
BOOL(WINAPI* MMmciExecute)(LPCSTR) = 0;
BOOL(WINAPI* MMmciFreeCommandResource)(UINT) = 0;
BOOL(WINAPI* MMmciGetErrorStringA)(DWORD, LPTSTR, UINT) = 0;
BOOL(WINAPI* MMmciGetErrorStringW)(DWORD, LPWSTR, UINT) = 0;
BOOL(WINAPI* MMmciSetDriverData)(UINT, DWORD) = 0;
DWORD(WINAPI* MMmciGetDriverData)(UINT) = 0;
HANDLE(WINAPI* MMmciGetCreatorTask)(MCIDEVICEID) = 0;
MCIDEVICEID(WINAPI* MMmciGetDeviceIDA)(LPCTSTR) = 0;
MCIDEVICEID(WINAPI* MMmciGetDeviceIDFromElementIDA)(DWORD, LPCTSTR) = 0;
MCIDEVICEID(WINAPI* MMmciGetDeviceIDFromElementIDW)(DWORD, LPCWSTR) = 0;
MCIDEVICEID(WINAPI* MMmciGetDeviceIDW)(LPCWSTR) = 0;
MCIERROR(WINAPI* MMmciSendCommandA)(MCIDEVICEID, UINT, DWORD_PTR, DWORD_PTR) = 0;
MCIERROR(WINAPI* MMmciSendCommandW)(MCIDEVICEID, UINT, DWORD_PTR, DWORD_PTR) = 0;
MCIERROR(WINAPI* MMmciSendStringA)(LPCTSTR, LPTSTR, UINT, HANDLE) = 0;
MCIERROR(WINAPI* MMmciSendStringW)(LPCWSTR, LPWSTR, UINT, HANDLE) = 0;
UINT(WINAPI* MMmciDriverYield)(UINT) = 0;
UINT(WINAPI* MMmciLoadCommandResource)(HINSTANCE, LPCWSTR, UINT) = 0;
UINT(WINAPI* MMmciSetYieldProc)(MCIDEVICEID, YIELDPROC, DWORD) = 0;
YIELDPROC(WINAPI* MMmciGetYieldProc)(MCIDEVICEID, LPDWORD) = 0;
// MCI stuff

// Mixer stuff
DWORD(WINAPI* MMmixerMessage)(HMIXER, UINT, DWORD_PTR, DWORD_PTR) = 0;
MMRESULT(WINAPI* MMmixerClose)(HMIXER) = 0;
MMRESULT(WINAPI* MMmixerGetControlDetailsA)(HMIXEROBJ, LPMIXERCONTROLDETAILS, DWORD) = 0;
MMRESULT(WINAPI* MMmixerGetControlDetailsW)(HMIXEROBJ, LPMIXERCONTROLDETAILS, DWORD) = 0;
MMRESULT(WINAPI* MMmixerGetDevCapsA)(UINT_PTR, LPMIXERCAPSA, UINT) = 0;
MMRESULT(WINAPI* MMmixerGetDevCapsW)(UINT_PTR, LPMIXERCAPSW, UINT) = 0;
MMRESULT(WINAPI* MMmixerGetID)(HMIXEROBJ, UINT FAR*, DWORD) = 0;
MMRESULT(WINAPI* MMmixerGetLineControlsA)(HMIXEROBJ, LPMIXERLINECONTROLSA, DWORD) = 0;
MMRESULT(WINAPI* MMmixerGetLineControlsW)(HMIXEROBJ, LPMIXERLINECONTROLSW, DWORD) = 0;
MMRESULT(WINAPI* MMmixerGetLineInfoA)(HMIXEROBJ, LPMIXERLINEA, DWORD) = 0;
MMRESULT(WINAPI* MMmixerGetLineInfoW)(HMIXEROBJ, LPMIXERLINEW, DWORD) = 0;
MMRESULT(WINAPI* MMmixerOpen)(LPHMIXER, UINT, DWORD_PTR, DWORD_PTR, DWORD) = 0;
MMRESULT(WINAPI* MMmixerSetControlDetails)(HMIXEROBJ, LPMIXERCONTROLDETAILS, DWORD) = 0;
UINT(WINAPI* MMmixerGetNumDevs)() = 0;
// Mixer stuff

// MMIO stuff
FOURCC(WINAPI* MMmmioStringToFOURCCA)(LPCTSTR, UINT) = 0;
FOURCC(WINAPI* MMmmioStringToFOURCCW)(LPCWSTR, UINT) = 0;
LONG(WINAPI* MMmmioRead)(HMMIO, HPSTR, LONG) = 0;
LONG(WINAPI* MMmmioSeek)(HMMIO, LONG, INT) = 0;
LONG(WINAPI* MMmmioWrite)(HMMIO, char _huge*, LONG) = 0;
LPMMIOPROC(WINAPI* MMmmioInstallIOProcA)(FOURCC, LPMMIOPROC, DWORD) = 0;
LPMMIOPROC(WINAPI* MMmmioInstallIOProcW)(FOURCC, LPMMIOPROC, DWORD) = 0;
LRESULT(WINAPI* MMmmioSendMessage)(HMMIO, UINT, LPARAM, LPARAM) = 0;
MMRESULT(WINAPI* MMmmioAdvance)(HMMIO, LPMMIOINFO, UINT) = 0;
MMRESULT(WINAPI* MMmmioAscend)(HMMIO, LPMMCKINFO, UINT) = 0;
MMRESULT(WINAPI* MMmmioClose)(HMMIO, UINT) = 0;
MMRESULT(WINAPI* MMmmioCreateChunk)(HMMIO, LPMMCKINFO, UINT) = 0;
MMRESULT(WINAPI* MMmmioDescend)(HMMIO, LPMMCKINFO, const MMCKINFO*, UINT) = 0;
MMRESULT(WINAPI* MMmmioFlush)(HMMIO, UINT) = 0;
MMRESULT(WINAPI* MMmmioGetInfo)(HMMIO, LPMMIOINFO, UINT) = 0;
MMRESULT(WINAPI* MMmmioOpenA)(LPTSTR, LPMMIOINFO, DWORD) = 0;
MMRESULT(WINAPI* MMmmioOpenW)(LPWSTR, LPMMIOINFO, DWORD) = 0;
MMRESULT(WINAPI* MMmmioRenameA)(LPCTSTR, LPCTSTR, const LPMMIOINFO, DWORD);
MMRESULT(WINAPI* MMmmioRenameW)(LPCWSTR, LPCWSTR, const LPMMIOINFO, DWORD);
MMRESULT(WINAPI* MMmmioSetBuffer)(HMMIO, LPSTR, LONG, UINT) = 0;
MMRESULT(WINAPI* MMmmioSetInfo)(HMMIO, LPMMIOINFO, UINT) = 0;
// MMIO stuff

// MM stuff
BOOL(WINAPI* MMmmTaskSignal)(DWORD) = 0;
UINT(WINAPI* MMmmTaskCreate)(LPTASKCALLBACK, HANDLE, DWORD_PTR) = 0;
VOID(WINAPI* MMmmTaskBlock)(DWORD) = 0;
VOID(WINAPI* MMmmTaskYield)() = 0;
// MM stuff

// PlaySound stuff
BOOL(WINAPI* MMPlaySound)(LPCTSTR, HMODULE, DWORD) = 0;
BOOL(WINAPI* MMPlaySoundA)(LPCTSTR, HMODULE, DWORD) = 0;
BOOL(WINAPI* MMPlaySoundW)(LPCWSTR, HMODULE, DWORD) = 0;
BOOL(WINAPI* MMsndPlaySoundA)(LPCWSTR, UINT) = 0;
BOOL(WINAPI* MMsndPlaySoundW)(LPCTSTR, UINT) = 0;
// PlaySound stuff

// Aux stuff
DWORD(WINAPI* MMauxOutMessage)(UINT, UINT, DWORD_PTR, DWORD_PTR) = 0;
MMRESULT(WINAPI* MMauxGetDevCapsA)(UINT_PTR, LPAUXCAPSA, UINT) = 0;
MMRESULT(WINAPI* MMauxGetDevCapsW)(UINT_PTR, LPAUXCAPSW, UINT) = 0;
MMRESULT(WINAPI* MMauxGetVolume)(UINT, LPDWORD) = 0;
MMRESULT(WINAPI* MMauxSetVolume)(UINT, DWORD) = 0;
UINT(WINAPI* MMauxGetNumDevs)() = 0;
// Aux stuff

// Wave out stuff
DWORD(WINAPI* MMwaveOutMessage)(HWAVEOUT, UINT, DWORD_PTR, DWORD_PTR) = 0;
MMRESULT(WINAPI* MMwaveOutAddBuffer)(HWAVEOUT, LPWAVEHDR, UINT) = 0;
MMRESULT(WINAPI* MMwaveOutBreakLoop)(HWAVEOUT) = 0;
MMRESULT(WINAPI* MMwaveOutClose)(HWAVEOUT) = 0;
MMRESULT(WINAPI* MMwaveOutGetDevCapsA)(UINT_PTR, LPWAVEOUTCAPSA, UINT) = 0;
MMRESULT(WINAPI* MMwaveOutGetDevCapsW)(UINT_PTR, LPWAVEOUTCAPSW, UINT) = 0;
MMRESULT(WINAPI* MMwaveOutGetErrorTextA)(MMRESULT, LPTSTR, UINT) = 0;
MMRESULT(WINAPI* MMwaveOutGetErrorTextW)(MMRESULT, LPWSTR, UINT) = 0;
MMRESULT(WINAPI* MMwaveOutGetID)(HWAVEOUT, LPUINT) = 0;
MMRESULT(WINAPI* MMwaveOutGetPitch)(HWAVEOUT, LPDWORD) = 0;
MMRESULT(WINAPI* MMwaveOutGetPlaybackRate)(HWAVEOUT, LPDWORD) = 0;
MMRESULT(WINAPI* MMwaveOutGetPosition)(HWAVEOUT, LPMMTIME, UINT) = 0;
MMRESULT(WINAPI* MMwaveOutGetVolume)(HWAVEOUT, LPDWORD) = 0;
MMRESULT(WINAPI* MMwaveOutOpen)(LPHWAVEOUT, UINT, LPCWAVEFORMATEX, DWORD_PTR, DWORD_PTR, DWORD) = 0;
MMRESULT(WINAPI* MMwaveOutPrepareHeader)(HWAVEOUT, LPWAVEHDR, UINT) = 0;
MMRESULT(WINAPI* MMwaveOutReset)(HWAVEOUT) = 0;
MMRESULT(WINAPI* MMwaveOutRestart)(HWAVEOUT) = 0;
MMRESULT(WINAPI* MMwaveOutPause)(HWAVEOUT) = 0;
MMRESULT(WINAPI* MMwaveOutSetPitch)(HWAVEOUT, DWORD) = 0;
MMRESULT(WINAPI* MMwaveOutSetPlaybackRate)(HWAVEOUT, DWORD) = 0;
MMRESULT(WINAPI* MMwaveOutSetVolume)(HWAVEOUT, DWORD) = 0;
MMRESULT(WINAPI* MMwaveOutStart)(HWAVEOUT) = 0;
MMRESULT(WINAPI* MMwaveOutStop)(HWAVEOUT) = 0;
MMRESULT(WINAPI* MMwaveOutUnprepareHeader)(HWAVEOUT, LPWAVEHDR, UINT) = 0;
MMRESULT(WINAPI* MMwaveOutWrite)(HWAVEOUT, LPWAVEHDR, UINT) = 0;
UINT(WINAPI* MMwaveOutGetNumDevs)() = 0;
// Wave out stuff

// Wave in stuff
DWORD(WINAPI* MMwaveInMessage)(HWAVEIN, UINT, DWORD_PTR, DWORD_PTR) = 0;
MMRESULT(WINAPI* MMwaveInAddBuffer)(HWAVEIN, LPWAVEHDR, UINT) = 0;
MMRESULT(WINAPI* MMwaveInClose)(HWAVEIN) = 0;
MMRESULT(WINAPI* MMwaveInGetDevCapsA)(UINT_PTR, LPWAVEINCAPSA, UINT) = 0;
MMRESULT(WINAPI* MMwaveInGetDevCapsW)(UINT_PTR, LPWAVEINCAPSW, UINT) = 0;
MMRESULT(WINAPI* MMwaveInGetErrorTextA)(MMRESULT, LPTSTR, UINT) = 0;
MMRESULT(WINAPI* MMwaveInGetErrorTextW)(MMRESULT, LPWSTR, UINT) = 0;
MMRESULT(WINAPI* MMwaveInGetID)(HWAVEIN, LPUINT) = 0;
MMRESULT(WINAPI* MMwaveInGetPosition)(HWAVEIN, LPMMTIME, UINT) = 0;
MMRESULT(WINAPI* MMwaveInOpen)(LPHWAVEIN, UINT, LPCWAVEFORMATEX, DWORD_PTR, DWORD_PTR, DWORD) = 0;
MMRESULT(WINAPI* MMwaveInPrepareHeader)(HWAVEIN, LPWAVEHDR, UINT) = 0;
MMRESULT(WINAPI* MMwaveInReset)(HWAVEIN) = 0;
MMRESULT(WINAPI* MMwaveInStart)(HWAVEIN) = 0;
MMRESULT(WINAPI* MMwaveInStop)(HWAVEIN) = 0;
MMRESULT(WINAPI* MMwaveInUnprepareHeader)(HWAVEIN, LPWAVEHDR, UINT) = 0;
UINT(WINAPI* MMwaveInGetNumDevs)() = 0;
// Wave in stuff

#ifdef _M_IX86
// Legacy 16-bit functions
MMRESULT(WINAPI* MMaux32Message)(UINT_PTR, UINT, DWORD_PTR, DWORD_PTR, DWORD_PTR) = 0;
MMRESULT(WINAPI* MMjoy32Message)(UINT_PTR, UINT, DWORD_PTR, DWORD_PTR, DWORD_PTR) = 0;
MMRESULT(WINAPI* MMmci32Message)(UINT_PTR, UINT, DWORD_PTR, DWORD_PTR, DWORD_PTR) = 0;
MMRESULT(WINAPI* MMmid32Message)(UINT_PTR, UINT, DWORD_PTR, DWORD_PTR, DWORD_PTR) = 0;
MMRESULT(WINAPI* MMmod32Message)(UINT_PTR, UINT, DWORD_PTR, DWORD_PTR, DWORD_PTR) = 0;
MMRESULT(WINAPI* MMmxd32Message)(UINT_PTR, UINT, DWORD_PTR, DWORD_PTR, DWORD_PTR) = 0;
MMRESULT(WINAPI* MMtid32Message)(UINT_PTR, UINT, DWORD_PTR, DWORD_PTR, DWORD_PTR) = 0;
MMRESULT(WINAPI* MMwid32Message)(UINT_PTR, UINT, DWORD_PTR, DWORD_PTR, DWORD_PTR) = 0;
MMRESULT(WINAPI* MMwod32Message)(UINT_PTR, UINT, DWORD_PTR, DWORD_PTR, DWORD_PTR) = 0;
// Legacy 16-bit functions
#endif

// Quickity quackity
#define MMI(f) {(void**)& MM##f, #f}

struct MMImport
{
	void** ptr;
	const char* name;
}

MMImports[] =
{
	MMI(CloseDriver),
	MMI(DefDriverProc),
	MMI(DriverCallback),
	MMI(DrvGetModuleHandle),
	MMI(GetDriverModuleHandle),
	MMI(OpenDriver),
	MMI(PlaySound),
	MMI(PlaySoundA),
	MMI(PlaySoundW),
	MMI(SendDriverMessage),
	MMI(auxGetDevCapsA),
	MMI(auxGetDevCapsW),
	MMI(auxGetNumDevs),
	MMI(auxGetVolume),
	MMI(auxOutMessage),
	MMI(auxSetVolume),
	MMI(joyConfigChanged),
	MMI(joyGetDevCapsA),
	MMI(joyGetDevCapsW),
	MMI(joyGetNumDevs),
	MMI(joyGetPos),
	MMI(joyGetPosEx),
	MMI(joyGetThreshold),
	MMI(joyReleaseCapture),
	MMI(joySetCapture),
	MMI(joySetThreshold),
	MMI(mciDriverNotify),
	MMI(mciDriverYield),
	MMI(mciExecute),
	MMI(mciFreeCommandResource),
	MMI(mciGetCreatorTask),
	MMI(mciGetDeviceIDA),
	MMI(mciGetDeviceIDFromElementIDA),
	MMI(mciGetDeviceIDFromElementIDW),
	MMI(mciGetDeviceIDW),
	MMI(mciGetDriverData),
	MMI(mciGetErrorStringA),
	MMI(mciGetErrorStringW),
	MMI(mciGetYieldProc),
	MMI(mciLoadCommandResource),
	MMI(mciSendCommandA),
	MMI(mciSendCommandW),
	MMI(mciSendStringA),
	MMI(mciSendStringW),
	MMI(mciSetDriverData),
	MMI(mciSetYieldProc),
	MMI(midiConnect),
	MMI(midiDisconnect),
	MMI(midiInAddBuffer),
	MMI(midiInClose),
	MMI(midiInGetDevCapsA),
	MMI(midiInGetDevCapsW),
	MMI(midiInGetErrorTextA),
	MMI(midiInGetErrorTextW),
	MMI(midiInGetID),
	MMI(midiInGetNumDevs),
	MMI(midiInMessage),
	MMI(midiInOpen),
	MMI(midiInPrepareHeader),
	MMI(midiInReset),
	MMI(midiInStart),
	MMI(midiInStop),
	MMI(midiInUnprepareHeader),
	MMI(midiOutCacheDrumPatches),
	MMI(midiOutCachePatches),
	MMI(midiOutClose),
	MMI(midiOutGetDevCapsA),
	MMI(midiOutGetDevCapsW),
	MMI(midiOutGetErrorTextA),
	MMI(midiOutGetErrorTextW),
	MMI(midiOutGetID),
	MMI(midiOutGetNumDevs),
	MMI(midiOutGetVolume),
	MMI(midiOutLongMsg),
	MMI(midiOutMessage),
	MMI(midiOutOpen),
	MMI(midiOutPrepareHeader),
	MMI(midiOutReset),
	MMI(midiOutSetVolume),
	MMI(midiOutShortMsg),
	MMI(midiOutUnprepareHeader),
	MMI(midiStreamClose),
	MMI(midiStreamOpen),
	MMI(midiStreamOut),
	MMI(midiStreamPause),
	MMI(midiStreamPosition),
	MMI(midiStreamProperty),
	MMI(midiStreamRestart),
	MMI(midiStreamStop),
	MMI(mixerClose),
	MMI(mixerGetControlDetailsA),
	MMI(mixerGetControlDetailsW),
	MMI(mixerGetDevCapsA),
	MMI(mixerGetDevCapsW),
	MMI(mixerGetID),
	MMI(mixerGetLineControlsA),
	MMI(mixerGetLineControlsW),
	MMI(mixerGetLineInfoA),
	MMI(mixerGetLineInfoW),
	MMI(mixerGetNumDevs),
	MMI(mixerMessage),
	MMI(mixerOpen),
	MMI(mixerSetControlDetails),
	MMI(mmGetCurrentTask),
	MMI(mmTaskBlock),
	MMI(mmTaskCreate),
	MMI(mmTaskSignal),
	MMI(mmTaskYield),
	MMI(mmioAdvance),
	MMI(mmioAscend),
	MMI(mmioClose),
	MMI(mmioCreateChunk),
	MMI(mmioDescend),
	MMI(mmioFlush),
	MMI(mmioGetInfo),
	MMI(mmioInstallIOProcA),
	MMI(mmioInstallIOProcW),
	MMI(mmioOpenA),
	MMI(mmioOpenW),
	MMI(mmioRead),
	MMI(mmioRenameA),
	MMI(mmioRenameW),
	MMI(mmioSeek),
	MMI(mmioSendMessage),
	MMI(mmioSetBuffer),
	MMI(mmioSetInfo),
	MMI(mmioStringToFOURCCA),
	MMI(mmioStringToFOURCCW),
	MMI(mmioWrite),
	MMI(sndPlaySoundA),
	MMI(sndPlaySoundW),
	MMI(timeBeginPeriod),
	MMI(timeEndPeriod),
	MMI(timeGetDevCaps),
	MMI(timeGetSystemTime),
	MMI(timeGetTime),
	MMI(timeKillEvent),
	MMI(timeSetEvent),
	MMI(waveInAddBuffer),
	MMI(waveInClose),
	MMI(waveInGetDevCapsA),
	MMI(waveInGetDevCapsW),
	MMI(waveInGetErrorTextA),
	MMI(waveInGetErrorTextW),
	MMI(waveInGetID),
	MMI(waveInGetNumDevs),
	MMI(waveInGetPosition),
	MMI(waveInMessage),
	MMI(waveInOpen),
	MMI(waveInPrepareHeader),
	MMI(waveInReset),
	MMI(waveInStart),
	MMI(waveInStop),
	MMI(waveInUnprepareHeader),
	MMI(waveOutBreakLoop),
	MMI(waveOutClose),
	MMI(waveOutGetDevCapsA),
	MMI(waveOutGetDevCapsW),
	MMI(waveOutGetErrorTextA),
	MMI(waveOutGetErrorTextW),
	MMI(waveOutGetID),
	MMI(waveOutGetNumDevs),
	MMI(waveOutGetPitch),
	MMI(waveOutGetPlaybackRate),
	MMI(waveOutGetPosition),
	MMI(waveOutGetVolume),
	MMI(waveOutMessage),
	MMI(waveOutOpen),
	MMI(waveOutPause),
	MMI(waveOutPrepareHeader),
	MMI(waveOutReset),
	MMI(waveOutRestart),
	MMI(waveOutSetPitch),
	MMI(waveOutSetPlaybackRate),
	MMI(waveOutSetVolume),
	MMI(waveOutUnprepareHeader),
	MMI(waveOutWrite),

#ifdef _M_IX86
	MMI(aux32Message),
	MMI(joy32Message),
	MMI(mci32Message),
	MMI(mid32Message),
	MMI(mod32Message),
	MMI(mxd32Message),
	MMI(tid32Message),
	MMI(wid32Message),
	MMI(wod32Message)
#endif
};

// Functions start -> HERE <-

HDRVR WINAPI WINMM_OpenDriver(_In_ LPCWSTR lpDN, _In_ LPCWSTR lpSN, _In_ LPARAM lp) {
	return MMOpenDriver(lpDN, lpSN, lp);
}

LRESULT WINAPI WINMM_CloseDriver(_In_ HDRVR drv, _In_ LPARAM lP1, _In_ LPARAM lP2) {
	return MMCloseDriver(drv, lP1, lP2);
}

LRESULT WINAPI WINMM_SendDriverMessage(_In_ HDRVR drv, _In_ UINT msg, _Inout_ LPARAM lP1, _Inout_ LPARAM lP2) {
	return MMSendDriverMessage(drv, msg, lP1, lP2);
}

DWORD WINAPI WINMM_mmGetCurrentTask() {
	return MMmmGetCurrentTask();
}

HMODULE WINAPI WINMM_DrvGetModuleHandle(_In_ HDRVR drv) {
	return MMDrvGetModuleHandle(drv);
}

HMODULE WINAPI WINMM_GetDriverModuleHandle(_In_ HDRVR drv) {
	return MMGetDriverModuleHandle(drv);
}

LRESULT WINAPI WINMM_DefDriverProc(DWORD_PTR dwDId, HDRVR drv, UINT msg, LONG lP1, LONG lP2) {
	return MMDefDriverProc(dwDId, drv, msg, lP1, lP2);
}

LRESULT WINAPI WINMM_DriverCallback(DWORD dwC, DWORD dwF, HDRVR drv, DWORD msg, DWORD dwU, DWORD dwP1, DWORD dwP2) {
	return MMDriverCallback(dwC, dwF, drv, msg, dwU, dwP1, dwP2);
}

MMRESULT WINAPI WINMM_midiOutGetErrorTextA(MMRESULT err, LPSTR pszT, UINT cchT) {
	return MMmidiOutGetErrorTextA(err, pszT, cchT);
}

MMRESULT WINAPI WINMM_midiOutGetErrorTextW(MMRESULT err, LPWSTR pszT, UINT cchT) {
	return MMmidiOutGetErrorTextW(err, pszT, cchT);
}

MMRESULT WINAPI WINMM_midiConnect(HMIDI hS, HMIDIIN hM, LPVOID lpV) {
	return MMmidiConnect(hS, hM, lpV);
}

MMRESULT WINAPI WINMM_midiDisconnect(HMIDI hS, HMIDIIN hM, LPVOID lpV) {
	return MMmidiDisconnect(hS, hM, lpV);
}

MMRESULT WINAPI WINMM_midiInAddBuffer(HMIDIIN hM, LPMIDIHDR buf, UINT bufsize) {
	return MMmidiInAddBuffer(hM, buf, bufsize);
}

MMRESULT WINAPI WINMM_midiInPrepareHeader(HMIDIIN hM, LPMIDIHDR buf, UINT bufsize) {
	return MMmidiInPrepareHeader(hM, buf, bufsize);
}

MMRESULT WINAPI WINMM_midiInUnprepareHeader(HMIDIIN hM, LPMIDIHDR buf, UINT bufsize) {
	return MMmidiInPrepareHeader(hM, buf, bufsize);
}

MMRESULT WINAPI WINMM_midiInOpen(LPHMIDIIN lphM, UINT uDID, DWORD_PTR dwC, DWORD_PTR dwCI, DWORD dwF) {
	return MMmidiInOpen(lphM, uDID, dwC, dwCI, dwF);
}

MMRESULT WINAPI WINMM_midiInStart(HMIDIIN hM) {
	return MMmidiInStart(hM);
}

MMRESULT WINAPI WINMM_midiInReset(HMIDIIN hM) {
	return MMmidiInReset(hM);
}

MMRESULT WINAPI WINMM_midiInStop(HMIDIIN hM) {
	return MMmidiInStop(hM);
}

MMRESULT WINAPI WINMM_midiInClose(HMIDIIN hM) {
	return MMmidiInClose(hM);
}

MMRESULT WINAPI WINMM_midiInGetID(HMIDIIN hM, LPUINT lpU) {
	return MMmidiInGetID(hM, lpU);
}

MMRESULT WINAPI WINMM_midiInGetDevCapsA(UINT_PTR uP, LPMIDIINCAPSA LPMIC, UINT u) {
	return MMmidiInGetDevCapsA(uP, LPMIC, u);
}

MMRESULT WINAPI WINMM_midiInGetDevCapsW(UINT_PTR uP, LPMIDIINCAPSW LPMIC, UINT u) {
	return MMmidiInGetDevCapsW(uP, LPMIC, u);
}

MMRESULT WINAPI WINMM_midiInGetErrorTextA(MMRESULT mmr, LPSTR str, UINT u) {
	return MMmidiInGetErrorTextA(mmr, str, u);
}

MMRESULT WINAPI WINMM_midiInGetErrorTextW(MMRESULT mmr, LPWSTR str, UINT u) {
	return MMmidiInGetErrorTextW(mmr, str, u);
}

MMRESULT WINAPI WINMM_midiInMessage(HMIDIIN hM, UINT u, DWORD_PTR dwP1, DWORD_PTR dwP2) {
	return MMmidiInMessage(hM, u, dwP1, dwP2);
}

MMRESULT WINAPI WINMM_midiInGetNumDevs() {
	return MMmidiInGetNumDevs();
}

MMRESULT WINAPI WINMM_joyConfigChanged(DWORD dwF) {
	return MMjoyConfigChanged(dwF);
}

MMRESULT WINAPI WINMM_joyGetDevCapsA(UINT uDID, LPJOYCAPSA LPJC, UINT size) {
	return MMjoyGetDevCapsA(uDID, LPJC, size);
}

MMRESULT WINAPI WINMM_joyGetDevCapsW(UINT uDID, LPJOYCAPSW LPJC, UINT size) {
	return MMjoyGetDevCapsW(uDID, LPJC, size);
}

MMRESULT WINAPI WINMM_joyGetPos(UINT uDID, LPJOYINFO LPJI) {
	return MMjoyGetPos(uDID, LPJI);
}

MMRESULT WINAPI WINMM_joyGetPosEx(UINT uDID, LPJOYINFOEX LPJI) {
	return MMjoyGetPosEx(uDID, LPJI);
}

MMRESULT WINAPI WINMM_joyGetThreshold(UINT uDID, LPUINT val) {
	return MMjoyGetThreshold(uDID, val);
}

MMRESULT WINAPI WINMM_joySetThreshold(UINT uDID, UINT val) {
	return MMjoySetThreshold(uDID, val);
}

MMRESULT WINAPI WINMM_joyReleaseCapture(UINT uDID) {
	return MMjoyReleaseCapture(uDID);
}

MMRESULT WINAPI WINMM_joySetCapture(HWND h, UINT uDID, UINT uP, BOOL fC) {
	return MMjoySetCapture(h, uDID, uP, fC);
}

UINT WINAPI WINMM_joyGetNumDevs() {
	return MMjoyGetNumDevs();
}

BOOL WINAPI WINMM_mciDriverNotify(HWND hC, UINT wDID, UINT uS) {
	return MMmciDriverNotify(hC, wDID, uS);
}

BOOL WINAPI WINMM_mciExecute(LPCSTR pC) {
	return MMmciExecute(pC);
}

UINT WINAPI WINMM_mciLoadCommandResource(HANDLE hI, LPCWSTR lpRN, UINT wT) {
	return MMmciLoadCommandResource((HINSTANCE)hI, lpRN, wT);
}

BOOL WINAPI WINMM_mciFreeCommandResource(UINT wT) {
	return MMmciFreeCommandResource(wT);
}

BOOL WINAPI WINMM_mciGetErrorStringA(DWORD MCIE, LPTSTR pT, UINT cT) {
	return MMmciGetErrorStringA(MCIE, pT, cT);
}

BOOL WINAPI WINMM_mciGetErrorStringW(DWORD MCIE, LPWSTR pT, UINT cT) {
	return MMmciGetErrorStringW(MCIE, pT, cT);
}

BOOL WINAPI WINMM_mciSetDriverData(UINT wDID, DWORD dwD) {
	return MMmciSetDriverData(wDID, dwD);
}

BOOL WINAPI WINMM_mciGetDriverData(UINT wDID) {
	return MMmciGetDriverData(wDID);
}

HANDLE WINAPI WINMM_mciGetCreatorTask(MCIDEVICEID IDD) {
	return MMmciGetCreatorTask(IDD);
}

MCIDEVICEID WINAPI WINMM_mciGetDeviceIDA(LPCTSTR pszD) {
	return MMmciGetDeviceIDA(pszD);
}

MCIDEVICEID WINAPI WINMM_mciGetDeviceIDW(LPCWSTR pszD) {
	return MMmciGetDeviceIDW(pszD);
}

MCIDEVICEID WINAPI WINMM_mciGetDeviceIDFromElementIDA(DWORD dwEID, LPCTSTR pszD) {
	return MMmciGetDeviceIDFromElementIDA(dwEID, pszD);
}

MCIDEVICEID WINAPI WINMM_mciGetDeviceIDFromElementIDW(DWORD dwEID, LPCWSTR pszD) {
	return MMmciGetDeviceIDFromElementIDW(dwEID, pszD);
}

MCIERROR WINAPI WINMM_mciSendCommandA(MCIDEVICEID uDID, UINT msg, DWORD_PTR dwP1, DWORD_PTR dwP2) {
	return MMmciSendCommandA(uDID, msg, dwP1, dwP2);
}

MCIERROR WINAPI WINMM_mciSendCommandW(MCIDEVICEID uDID, UINT msg, DWORD_PTR dwP1, DWORD_PTR dwP2) {
	return MMmciSendCommandW(uDID, msg, dwP1, dwP2);
}

MCIERROR WINAPI WINMM_mciSendStringA(LPCTSTR lpszC, LPTSTR lpszR, UINT cchR, HANDLE hC) {
	return MMmciSendStringA(lpszC, lpszR, cchR, hC);
}

MCIERROR WINAPI WINMM_mciSendStringW(LPCWSTR lpszC, LPWSTR lpszR, UINT cchR, HANDLE hC) {
	return MMmciSendStringW(lpszC, lpszR, cchR, hC);
}

UINT WINAPI WINMM_mciDriverYield(MCIDEVICEID wDID) {
	return MMmciDriverYield(wDID);
}

UINT WINAPI WINMM_mciSetYieldProc(MCIDEVICEID wDID, YIELDPROC fpYP, DWORD dwYD) {
	return MMmciSetYieldProc(wDID, fpYP, dwYD);
}

YIELDPROC WINAPI WINMM_mciGetYieldProc(MCIDEVICEID wDID, LPDWORD lpdwYD) {
	return MMmciGetYieldProc(wDID, lpdwYD);
}

DWORD WINAPI WINMM_mixerMessage(HMIXER dID, UINT msg, DWORD_PTR dwP1, DWORD_PTR dwP2) {
	return MMmixerMessage(dID, msg, dwP1, dwP2);
}

MMRESULT WINAPI WINMM_mixerClose(HMIXER dID) {
	return MMmixerClose(dID);
}

MMRESULT WINAPI WINMM_mixerGetLineInfoA(HMIXEROBJ dIDO, LPMIXERLINEA LPML, DWORD size) {
	return MMmixerGetLineInfoA(dIDO, LPML, size);
}

MMRESULT WINAPI WINMM_mixerGetLineInfoW(HMIXEROBJ dIDO, LPMIXERLINEW LPML, DWORD size) {
	return MMmixerGetLineInfoW(dIDO, LPML, size);
}

MMRESULT WINAPI WINMM_mixerGetLineControlsA(HMIXEROBJ dIDO, LPMIXERLINECONTROLSA LPMLC, DWORD size) {
	return MMmixerGetLineControlsA(dIDO, LPMLC, size);
}

MMRESULT WINAPI WINMM_mixerGetLineControlsW(HMIXEROBJ dIDO, LPMIXERLINECONTROLSW LPMLC, DWORD size) {
	return MMmixerGetLineControlsW(dIDO, LPMLC, size);
}

MMRESULT WINAPI WINMM_mixerSetControlDetails(HMIXEROBJ dIDO, LPMIXERCONTROLDETAILS LPMCD, DWORD size) {
	return MMmixerSetControlDetails(dIDO, LPMCD, size);
}

MMRESULT WINAPI WINMM_mixerGetControlDetailsA(HMIXEROBJ dIDO, LPMIXERCONTROLDETAILS LPMCD, DWORD size) {
	return MMmixerGetControlDetailsA(dIDO, LPMCD, size);
}

MMRESULT WINAPI WINMM_mixerGetControlDetailsW(HMIXEROBJ dIDO, LPMIXERCONTROLDETAILS LPMCD, DWORD size) {
	return MMmixerGetControlDetailsW(dIDO, LPMCD, size);
}

MMRESULT WINAPI WINMM_mixerGetDevCapsA(UINT_PTR uDID, LPMIXERCAPSA LPMC, UINT size) {
	return MMmixerGetDevCapsA(uDID, LPMC, size);
}

MMRESULT WINAPI WINMM_mixerGetDevCapsW(UINT_PTR uDID, LPMIXERCAPSW LPMC, UINT size) {
	return MMmixerGetDevCapsW(uDID, LPMC, size);
}

MMRESULT WINAPI WINMM_mixerGetID(HMIXEROBJ dIDO, UINT FAR * puMxId, DWORD fdwld) {
	return MMmixerGetID(dIDO, puMxId, fdwld);
}

MMRESULT WINAPI WINMM_mixerOpen(LPHMIXER lpdID, UINT uMxId, DWORD_PTR dwC, DWORD_PTR dwI, DWORD fdwO) {
	return MMmixerOpen(lpdID, uMxId, dwC, dwI, fdwO);
}

UINT WINAPI WINMM_mixerGetNumDevs() {
	return MMmixerGetNumDevs();
}

FOURCC WINAPI WINMM_mmioStringToFOURCCA(LPCTSTR sz, UINT f) {
	return MMmmioStringToFOURCCA(sz, f);
}

FOURCC WINAPI WINMM_mmioStringToFOURCCW(LPCWSTR sz, UINT f) {
	return MMmmioStringToFOURCCW(sz, f);
}

LONG WINAPI WINMM_mmioRead(HMMIO hm, HPSTR pch, LONG cch) {
	return MMmmioRead(hm, pch, cch);
}

LONG WINAPI WINMM_mmioSeek(HMMIO hm, LONG lO, INT iO) {
	return MMmmioSeek(hm, lO, iO);
}

LONG WINAPI WINMM_mmioWrite(HMMIO hm, char _huge * pch, LONG cch) {
	return MMmmioWrite(hm, pch, cch);
}

LPMMIOPROC WINAPI WINMM_mmioInstallIOProcA(FOURCC fccIOP, LPMMIOPROC pIOP, DWORD dwF) {
	return MMmmioInstallIOProcA(fccIOP, pIOP, dwF);
}

LPMMIOPROC WINAPI WINMM_mmioInstallIOProcW(FOURCC fccIOP, LPMMIOPROC pIOP, DWORD dwF) {
	return MMmmioInstallIOProcW(fccIOP, pIOP, dwF);
}

LRESULT WINAPI WINMM_mmioSendMessage(HMMIO hm, UINT msg, LPARAM lP1, LPARAM lP2) {
	return MMmmioSendMessage(hm, msg, lP1, lP2);
}

MMRESULT WINAPI WINMM_mmioAdvance(HMMIO hm, LPMMIOINFO pmmioi, UINT fuA) {
	return MMmmioAdvance(hm, pmmioi, fuA);
}

MMRESULT WINAPI WINMM_mmioAscend(HMMIO hm, LPMMCKINFO pmmcki, UINT fuA) {
	return MMmmioAscend(hm, pmmcki, fuA);
}

MMRESULT WINAPI WINMM_mmioClose(HMMIO hm, UINT fuC) {
	return MMmmioClose(hm, fuC);
}

MMRESULT WINAPI WINMM_mmioCreateChunk(HMMIO hm, LPMMCKINFO pmmcki, UINT fuC) {
	return MMmmioCreateChunk(hm, pmmcki, fuC);
}

MMRESULT WINAPI WINMM_mmioDescend(HMMIO hm, LPMMCKINFO pmmcki, const MMCKINFO * pmmckiP, UINT fuD) {
	return MMmmioDescend(hm, pmmcki, pmmckiP, fuD);
}

MMRESULT WINAPI WINMM_mmioFlush(HMMIO hm, UINT fuF) {
	return MMmmioFlush(hm, fuF);
}

MMRESULT WINAPI WINMM_mmioGetInfo(HMMIO hm, LPMMIOINFO pmmioi, UINT fuI) {
	return MMmmioGetInfo(hm, pmmioi, fuI);
}

MMRESULT WINAPI WINMM_mmioSetInfo(HMMIO hm, const LPMMIOINFO pmmioi, UINT fuI) {
	return MMmmioSetInfo(hm, pmmioi, fuI);
}

MMRESULT WINAPI WINMM_mmioOpenA(LPTSTR pszFN, LPMMIOINFO pmmioi, DWORD fdwO) {
	return MMmmioOpenA(pszFN, pmmioi, fdwO);
}

MMRESULT WINAPI WINMM_mmioOpenW(LPWSTR pszFN, LPMMIOINFO pmmioi, DWORD fdwO) {
	return MMmmioOpenW(pszFN, pmmioi, fdwO);
}

MMRESULT WINAPI WINMM_mmioRenameA(LPCTSTR pszFN, LPCTSTR pszNFN, const LPMMIOINFO pmmioi, DWORD fdwR) {
	return MMmmioRenameA(pszFN, pszNFN, pmmioi, fdwR);
}

MMRESULT WINAPI WINMM_mmioRenameW(LPCWSTR pszFN, LPCWSTR pszNFN, const LPMMIOINFO pmmioi, DWORD fdwR) {
	return MMmmioRenameW(pszFN, pszNFN, pmmioi, fdwR);
}

MMRESULT WINAPI WINMM_mmioSetBuffer(HMMIO hm, LPSTR pchB, LONG cchB, UINT fuB) {
	return MMmmioSetBuffer(hm, pchB, cchB, fuB);
}

BOOL WINAPI WINMM_mmTaskSignal(DWORD undef) {
	return MMmmTaskSignal(undef);
}

UINT WINAPI WINMM_mmTaskCreate(LPTASKCALLBACK undef1, HANDLE undef2, DWORD_PTR undef3) {
	return MMmmTaskCreate(undef1, undef2, undef3);
}

VOID WINAPI WINMM_mmTaskBlock(DWORD undef) {
	return MMmmTaskBlock(undef);
}

VOID WINAPI WINMM_mmTaskYield() {
	MMmmTaskYield();
}

BOOL WINAPI WINMM_PlaySound(LPCTSTR pszS, HMODULE hmod, DWORD fdwS) {
	return MMPlaySound(pszS, hmod, fdwS);
}

BOOL WINAPI WINMM_PlaySoundA(LPCTSTR pszS, HMODULE hmod, DWORD fdwS) {
	return MMPlaySoundA(pszS, hmod, fdwS);
}

BOOL WINAPI WINMM_PlaySoundW(LPCWSTR pszS, HMODULE hmod, DWORD fdwS) {
	return MMPlaySoundW(pszS, hmod, fdwS);
}

BOOL WINAPI WINMM_sndPlaySound(LPCTSTR pszS, DWORD fuS) {
	return MMsndPlaySoundA(pszS, fuS);
}

BOOL WINAPI WINMM_sndPlaySoundA(LPCTSTR pszS, DWORD fuS) {
	return MMsndPlaySoundA(pszS, fuS);
}

BOOL WINAPI WINMM_sndPlaySoundW(LPCWSTR pszS, DWORD fuS) {
	return MMsndPlaySoundW(pszS, fuS);
}

DWORD WINAPI WINMM_auxOutMessage(UINT uDID, UINT msg, DWORD_PTR dwP1, DWORD_PTR dwP2) {
	return MMauxOutMessage(uDID, msg, dwP1, dwP2);
}

MMRESULT WINAPI WINMM_auxGetDevCapsA(UINT_PTR uDID, LPAUXCAPSA pac, UINT cbac) {
	return MMauxGetDevCapsA(uDID, pac, cbac);
}

MMRESULT WINAPI WINMM_auxGetDevCapsW(UINT_PTR uDID, LPAUXCAPSW pac, UINT cbac) {
	return MMauxGetDevCapsW(uDID, pac, cbac);
}

MMRESULT WINAPI WINMM_auxGetVolume(UINT uDID, LPDWORD lpV) {
	return MMauxGetVolume(uDID, lpV);
}

MMRESULT WINAPI WINMM_auxSetVolume(UINT uDID, DWORD V) {
	return MMauxSetVolume(uDID, V);
}

UINT WINAPI WINMM_auxGetNumDevs() {
	return MMauxGetNumDevs();
}

DWORD WINAPI WINMM_waveOutMessage(HWAVEOUT hw, UINT msg, DWORD_PTR dwP1, DWORD_PTR dwP2) {
	return MMwaveOutMessage(hw, msg, dwP1, dwP2);
}

MMRESULT WINAPI WINMM_waveOutAddBuffer(HWAVEOUT hw, LPWAVEHDR pch, UINT cch) {
	return MMwaveOutAddBuffer(hw, pch, cch);
}

MMRESULT WINAPI WINMM_waveOutBreakLoop(HWAVEOUT hw) {
	return MMwaveOutBreakLoop(hw);
}

MMRESULT WINAPI WINMM_waveOutClose(HWAVEOUT hw) {
	return MMwaveOutClose(hw);
}

MMRESULT WINAPI WINMM_waveOutGetDevCapsA(UINT_PTR uDID, LPWAVEOUTCAPSA LPWOT, UINT size) {
	return MMwaveOutGetDevCapsA(uDID, LPWOT, size);
}

MMRESULT WINAPI WINMM_waveOutGetDevCapsW(UINT_PTR uDID, LPWAVEOUTCAPSW LPWOT, UINT size) {
	return MMwaveOutGetDevCapsW(uDID, LPWOT, size);
}

MMRESULT WINAPI WINMM_waveOutGetErrorTextA(MMRESULT err, LPTSTR pszT, UINT cchT) {
	return MMwaveOutGetErrorTextA(err, pszT, cchT);
}

MMRESULT WINAPI WINMM_waveOutGetErrorTextW(MMRESULT err, LPWSTR pszT, UINT cchT) {
	return MMwaveOutGetErrorTextW(err, pszT, cchT);
}

MMRESULT WINAPI WINMM_waveOutGetID(HWAVEOUT hw, LPUINT puDID) {
	return MMwaveOutGetID(hw, puDID);
}

MMRESULT WINAPI WINMM_waveOutSetPitch(HWAVEOUT hw, DWORD P) {
	return MMwaveOutSetPitch(hw, P);
}

MMRESULT WINAPI WINMM_waveOutGetPitch(HWAVEOUT hw, LPDWORD pP) {
	return MMwaveOutGetPitch(hw, pP);
}

MMRESULT WINAPI WINMM_waveOutSetPlaybackRate(HWAVEOUT hw, DWORD PR) {
	return MMwaveOutSetPlaybackRate(hw, PR);
}

MMRESULT WINAPI WINMM_waveOutGetPlaybackRate(HWAVEOUT hw, LPDWORD pPR) {
	return MMwaveOutGetPlaybackRate(hw, pPR);
}

MMRESULT WINAPI WINMM_waveOutGetPosition(HWAVEOUT hw, LPMMTIME pmmt, UINT cbmmt) {
	return MMwaveOutGetPosition(hw, pmmt, cbmmt);
}

MMRESULT WINAPI WINMM_waveOutSetVolume(HWAVEOUT hw, DWORD V) {
	return MMwaveOutSetVolume(hw, V);
}

MMRESULT WINAPI WINMM_waveOutGetVolume(HWAVEOUT hw, LPDWORD pV) {
	return MMwaveOutGetVolume(hw, pV);
}

MMRESULT WINAPI WINMM_waveOutOpen(LPHWAVEOUT lphw, UINT uDID, LPCWAVEFORMATEX LPWFEX, DWORD_PTR dwC, DWORD_PTR dwI, DWORD fdwO) {
	return MMwaveOutOpen(lphw, uDID, LPWFEX, dwC, dwI, fdwO);
}

MMRESULT WINAPI WINMM_waveOutPrepareHeader(HWAVEOUT hw, LPWAVEHDR pchB, UINT cchB) {
	return MMwaveOutPrepareHeader(hw, pchB, cchB);
}

MMRESULT WINAPI WINMM_waveOutUnprepareHeader(HWAVEOUT hw, LPWAVEHDR pchB, UINT cchB) {
	return MMwaveOutUnprepareHeader(hw, pchB, cchB);
}

MMRESULT WINAPI WINMM_waveOutWrite(HWAVEOUT hw, LPWAVEHDR pchB, UINT cchB) {
	return MMwaveOutWrite(hw, pchB, cchB);
}

MMRESULT WINAPI WINMM_waveOutReset(HWAVEOUT hw) {
	return MMwaveOutReset(hw);
}

MMRESULT WINAPI WINMM_waveOutRestart(HWAVEOUT hw) {
	return MMwaveOutRestart(hw);
}

MMRESULT WINAPI WINMM_waveOutPause(HWAVEOUT hw) {
	return MMwaveOutPause(hw);
}

MMRESULT WINAPI WINMM_waveOutStart(HWAVEOUT hw) {
	return MMwaveOutStart(hw);
}

MMRESULT WINAPI WINMM_waveOutStop(HWAVEOUT hw) {
	return MMwaveOutStop(hw);
}

UINT WINAPI WINMM_waveOutGetNumDevs() {
	return MMwaveOutGetNumDevs();
}

DWORD WINAPI WINMM_waveInMessage(HWAVEIN hw, UINT msg, DWORD_PTR dwP1, DWORD_PTR dwP2) {
	return MMwaveInMessage(hw, msg, dwP1, dwP2);
}

MMRESULT WINAPI WINMM_waveInAddBuffer(HWAVEIN hw, LPWAVEHDR pch, UINT cch) {
	return MMwaveInAddBuffer(hw, pch, cch);
}

MMRESULT WINAPI WINMM_waveInClose(HWAVEIN hw) {
	return MMwaveInClose(hw);
}

MMRESULT WINAPI WINMM_waveInGetDevCapsA(UINT_PTR uDID, LPWAVEINCAPSA LPWOT, UINT size) {
	return MMwaveInGetDevCapsA(uDID, LPWOT, size);
}

MMRESULT WINAPI WINMM_waveInGetDevCapsW(UINT_PTR uDID, LPWAVEINCAPSW LPWOT, UINT size) {
	return MMwaveInGetDevCapsW(uDID, LPWOT, size);
}

MMRESULT WINAPI WINMM_waveInGetErrorTextA(MMRESULT err, LPTSTR pszT, UINT cchT) {
	return MMwaveInGetErrorTextA(err, pszT, cchT);
}

MMRESULT WINAPI WINMM_waveInGetErrorTextW(MMRESULT err, LPWSTR pszT, UINT cchT) {
	return MMwaveInGetErrorTextW(err, pszT, cchT);
}

MMRESULT WINAPI WINMM_waveInGetID(HWAVEIN hw, LPUINT puDID) {
	return MMwaveInGetID(hw, puDID);
}

MMRESULT WINAPI WINMM_waveInGetPosition(HWAVEIN hw, LPMMTIME pmmt, UINT cbmmt) {
	return MMwaveInGetPosition(hw, pmmt, cbmmt);
}

MMRESULT WINAPI WINMM_waveInOpen(LPHWAVEIN lphw, UINT uDID, LPCWAVEFORMATEX LPWFEX, DWORD_PTR dwC, DWORD_PTR dwI, DWORD fdwO) {
	return MMwaveInOpen(lphw, uDID, LPWFEX, dwC, dwI, fdwO);
}

MMRESULT WINAPI WINMM_waveInPrepareHeader(HWAVEIN hw, LPWAVEHDR pchB, UINT cchB) {
	return MMwaveInPrepareHeader(hw, pchB, cchB);
}

MMRESULT WINAPI WINMM_waveInUnprepareHeader(HWAVEIN hw, LPWAVEHDR pchB, UINT cchB) {
	return MMwaveInUnprepareHeader(hw, pchB, cchB);
}

MMRESULT WINAPI WINMM_waveInReset(HWAVEIN hw) {
	return MMwaveInReset(hw);
}

MMRESULT WINAPI WINMM_waveInStart(HWAVEIN hw) {
	return MMwaveInStart(hw);
}

MMRESULT WINAPI WINMM_waveInStop(HWAVEIN hw) {
	return MMwaveInStop(hw);
}

UINT WINAPI WINMM_waveInGetNumDevs() {
	return MMwaveInGetNumDevs();
}

DWORD WINAPI WINMM_timeGetTime() {
	return TickStart + (DWORD)((MMtimeGetTime() - TickStart) * SpeedHack);
}

MMRESULT WINAPI WINMM_timeGetDevCaps(LPTIMECAPS ptc, UINT cbtc) {
	return MMtimeGetDevCaps(ptc, cbtc);
}

MMRESULT WINAPI WINMM_timeSetEvent(UINT uDelay, UINT uResolution, LPTIMECALLBACK lpTimeProc, DWORD_PTR dwUser, UINT fuEvent) {
	return MMtimeSetEvent(uDelay, uResolution, lpTimeProc, dwUser, fuEvent);
}

MMRESULT WINAPI WINMM_timeKillEvent(UINT uTimerID) {
	return MMtimeKillEvent(uTimerID);
}

MMRESULT WINAPI WINMM_timeGetSystemTime(LPMMTIME pmmt, UINT cbmmt) {
	return MMtimeGetSystemTime(pmmt, cbmmt);
}

MMRESULT WINAPI WINMM_timeBeginPeriod(UINT uPeriod) {
	return MMtimeBeginPeriod(uPeriod);
}

MMRESULT WINAPI WINMM_timeEndPeriod(UINT uPeriod) {
	return MMtimeEndPeriod(uPeriod);
}

HMODULE WINAPI WINMM_GetOWINMM() {
	return OWINMM;
}

#ifdef _M_IX86
MMRESULT WINAPI WINMM_aux32Message(UINT_PTR uDeviceID, UINT uMsg, DWORD_PTR Handle, DWORD_PTR dwParam1, DWORD_PTR dwParam2) {
	if (MMaux32Message) return MMaux32Message(uDeviceID, uMsg, Handle, dwParam1, dwParam2);
	else return MMSYSERR_NOERROR;
}

MMRESULT WINAPI WINMM_joy32Message(UINT_PTR uDeviceID, UINT uMsg, DWORD_PTR Handle, DWORD_PTR dwParam1, DWORD_PTR dwParam2) {
	if (MMjoy32Message) return MMjoy32Message(uDeviceID, uMsg, Handle, dwParam1, dwParam2);
	else return MMSYSERR_NOERROR;
}

MMRESULT WINAPI WINMM_mci32Message(UINT_PTR uDeviceID, UINT uMsg, DWORD_PTR Handle, DWORD_PTR dwParam1, DWORD_PTR dwParam2) {
	if (MMmci32Message) return MMmci32Message(uDeviceID, uMsg, Handle, dwParam1, dwParam2);
	else return MMSYSERR_NOERROR;
}

MMRESULT WINAPI WINMM_mid32Message(UINT_PTR uDeviceID, UINT uMsg, DWORD_PTR Handle, DWORD_PTR dwParam1, DWORD_PTR dwParam2) {
	if (MMmid32Message) return MMmid32Message(uDeviceID, uMsg, Handle, dwParam1, dwParam2);
	else return MMSYSERR_NOERROR;
}

MMRESULT WINAPI WINMM_mod32Message(UINT_PTR uDeviceID, UINT uMsg, DWORD_PTR Handle, DWORD_PTR dwParam1, DWORD_PTR dwParam2) {
	if (MMmod32Message) return MMmod32Message(uDeviceID, uMsg, Handle, dwParam1, dwParam2);
	else return MMSYSERR_NOERROR;
}

MMRESULT WINAPI WINMM_mxd32Message(UINT_PTR uDeviceID, UINT uMsg, DWORD_PTR Handle, DWORD_PTR dwParam1, DWORD_PTR dwParam2) {
	if (MMmxd32Message) return MMmxd32Message(uDeviceID, uMsg, Handle, dwParam1, dwParam2);
	else return MMSYSERR_NOERROR;
}

MMRESULT WINAPI WINMM_tid32Message(UINT_PTR uDeviceID, UINT uMsg, DWORD_PTR Handle, DWORD_PTR dwParam1, DWORD_PTR dwParam2) {
	if (MMtid32Message) return MMtid32Message(uDeviceID, uMsg, Handle, dwParam1, dwParam2);
	else return MMSYSERR_NOERROR;
}

MMRESULT WINAPI WINMM_wid32Message(UINT_PTR uDeviceID, UINT uMsg, DWORD_PTR hMidi, DWORD_PTR dwParam1, DWORD_PTR dwParam2) {
	if (MMwid32Message) return MMwid32Message(uDeviceID, uMsg, hMidi, dwParam1, dwParam2);
	else return MMSYSERR_NOERROR;
}

MMRESULT WINAPI WINMM_wod32Message(UINT_PTR uDeviceID, UINT uMsg, DWORD_PTR hMidi, DWORD_PTR dwParam1, DWORD_PTR dwParam2) {
	if (MMwod32Message) return MMwod32Message(uDeviceID, uMsg, hMidi, dwParam1, dwParam2);
	else return MMSYSERR_NOERROR;
}
#endif

BOOL ImportFromWinMM(int i, TCHAR* ErrorBuf) {
	*(MMImports[i].ptr) = (void*)GetProcAddress(OWINMM, MMImports[i].name);

	if (!*(MMImports[i].ptr)) {
		swprintf_s(
			ErrorBuf,
			1024,
			L"An error has occured while loading \"%s\" from the Windows Multimedia Extension API library.\n\nFailed to load the required functions, press OK to exit.",
			MMImports[i].name);

		MessageBox(
			NULL,
			ErrorBuf,
			L"KDMAPI ERROR",
			MB_ICONERROR | MB_OK | MB_SYSTEMMODAL
		);
		return FALSE;
	}
}

void GetSpeedHack() {
	char Potato[64] = { 0 };

	HKEY RegKey;
	QWORD TSH = 100000000;
	DWORD dwType = REG_DWORD, dwSize = sizeof(DWORD);

	LSTATUS ROKE = RegOpenKeyExW(HKEY_CURRENT_USER, L"Software\\OmniMIDI\\Configuration", 0, KEY_READ, &RegKey);
	if (!ROKE) {
		RegQueryValueExW(RegKey, L"WinMMSpeed", NULL, &dwType, (LPBYTE)&TSH, &dwSize);
		SpeedHack = ((double)TSH / 100000000.0);
		TickStart = MMtimeGetTime();
	}
	RegCloseKey(RegKey);
}

BOOL InitializeWinMM() {
	if (OWINMM)
		return TRUE;

	if (!OWINMM) {
		// Load WinMM from system directory if copy isn't found in the app's directory
		if (INVALID_FILE_ATTRIBUTES == GetFileAttributes(L"owinmm.dll") && GetLastError() == ERROR_FILE_NOT_FOUND)
		{
			wchar_t SystemDirectory[MAX_PATH];
			GetSystemDirectoryW(SystemDirectory, MAX_PATH);
			wcscat(SystemDirectory, L"\\winmm.dll");

			OWINMM = LoadLibraryW(SystemDirectory);
		}
		// Else load the custom DLL
		else OWINMM = LoadLibraryW(L"owinmm.dll");

		KERNEL32 = GetModuleHandleW(L"kernel32");

		if (!OWINMM) {
			MessageBox(
				NULL,
				L"The wrapper was unable to load WINMM.DLL!\nUnable to initialize the Windows Multimedia Extension API.\n\nPress OK to exit.",
				L"KDMAPI ERROR",
				MB_ICONERROR | MB_OK | MB_SYSTEMMODAL
			);

			return FALSE;
		}

		if (!KERNEL32) {
			MessageBox(
				NULL,
				L"The wrapper was unable to locate KERNEL32.DLL!\nIs this even Windows?\n\nPress OK to exit.",
				L"KDMAPI ERROR",
				MB_ICONERROR | MB_OK | MB_SYSTEMMODAL
			);

			return FALSE;
		}
	}

	TCHAR ErrorBuf[1024];

	// LOAD EVERYTHING!
	for (int i = 0; i < sizeof(MMImports) / sizeof(MMImports[0]); i++)
	{
		if (!_stricmp(MMImports[i].name, "timeBeginPeriod") ||
			!_stricmp(MMImports[i].name, "timeEndPeriod") ||
			!_stricmp(MMImports[i].name, "timeGetDevCaps") ||
			!_stricmp(MMImports[i].name, "timeGetSystemTime") ||
			!_stricmp(MMImports[i].name, "timeGetTime"))
		{
			*(MMImports[i].ptr) = (void*)GetProcAddress(KERNEL32, MMImports[i].name);

			if (!*(MMImports[i].ptr))
				ImportFromWinMM(i, ErrorBuf);
		}
		else ImportFromWinMM(i, ErrorBuf);
	}

	return TRUE;
}