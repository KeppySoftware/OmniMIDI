/*

	OmniMIDI v15+ (Rewrite) for Windows NT

	This file contains the required code to run the driver under Windows 7 SP1 and later.
	This file is useful only if you want to compile the driver under Windows, it's not needed for Linux/macOS porting.

*/

#include <bass\bassmidi.h>

DWORD(WINAPI* BASS_MIDI_GetVersion)(void) = 0;

HSTREAM(WINAPI* BASS_MIDI_StreamCreate)(DWORD channels, DWORD flags, DWORD freq) = 0;
HSTREAM(WINAPI* BASS_MIDI_StreamCreateFile)(BOOL mem, const void* file, QWORD offset, QWORD length, DWORD flags, DWORD freq) = 0;
HSTREAM(WINAPI* BASS_MIDI_StreamCreateURL)(const char* url, DWORD offset, DWORD flags, DOWNLOADPROC* proc, void* user, DWORD freq) = 0;
HSTREAM(WINAPI* BASS_MIDI_StreamCreateFileUser)(DWORD system, DWORD flags, const BASS_FILEPROCS* procs, void* user, DWORD freq) = 0;
HSTREAM(WINAPI* BASS_MIDI_StreamCreateEvents)(const BASS_MIDI_EVENT* events, DWORD ppqn, DWORD flags, DWORD freq) = 0;
BOOL(WINAPI* BASS_MIDI_StreamGetMark)(HSTREAM handle, DWORD type, DWORD index, BASS_MIDI_MARK* mark) = 0;
DWORD(WINAPI* BASS_MIDI_StreamGetMarks)(HSTREAM handle, int track, DWORD type, BASS_MIDI_MARK* marks) = 0;
BOOL(WINAPI* BASS_MIDI_StreamSetFonts)(HSTREAM handle, const void* fonts, DWORD count) = 0;
DWORD(WINAPI* BASS_MIDI_StreamGetFonts)(HSTREAM handle, void* fonts, DWORD count) = 0;
BOOL(WINAPI* BASS_MIDI_StreamLoadSamples)(HSTREAM handle) = 0;
BOOL(WINAPI* BASS_MIDI_StreamEvent)(HSTREAM handle, DWORD chan, DWORD event, DWORD param) = 0;
DWORD(WINAPI* BASS_MIDI_StreamEvents)(HSTREAM handle, DWORD mode, const void* events, DWORD length) = 0;
DWORD(WINAPI* BASS_MIDI_StreamGetEvent)(HSTREAM handle, DWORD chan, DWORD event) = 0;
DWORD(WINAPI* BASS_MIDI_StreamGetEvents)(HSTREAM handle, int track, DWORD filter, BASS_MIDI_EVENT* events) = 0;
DWORD(WINAPI* BASS_MIDI_StreamGetEventsEx)(HSTREAM handle, int track, DWORD filter, BASS_MIDI_EVENT* events, DWORD start, DWORD count) = 0;
BOOL(WINAPI* BASS_MIDI_StreamGetPreset)(HSTREAM handle, DWORD chan, BASS_MIDI_FONT* font) = 0;
HSTREAM(WINAPI* BASS_MIDI_StreamGetChannel)(HSTREAM handle, DWORD chan) = 0;
BOOL(WINAPI* BASS_MIDI_StreamSetFilter)(HSTREAM handle, BOOL seeking, MIDIFILTERPROC* proc, void* user) = 0;

HSOUNDFONT(WINAPI* BASS_MIDI_FontInit)(const void* file, DWORD flags) = 0;
HSOUNDFONT(WINAPI* BASS_MIDI_FontInitUser)(const BASS_FILEPROCS* procs, void* user, DWORD flags) = 0;
BOOL(WINAPI* BASS_MIDI_FontFree)(HSOUNDFONT handle) = 0;
BOOL(WINAPI* BASS_MIDI_FontGetInfo)(HSOUNDFONT handle, BASS_MIDI_FONTINFO* info) = 0;
BOOL(WINAPI* BASS_MIDI_FontGetPresets)(HSOUNDFONT handle, DWORD* presets) = 0;
const char* (WINAPI* BASS_MIDI_FontGetPreset)(HSOUNDFONT handle, int preset, int bank) = 0;
BOOL(WINAPI* BASS_MIDI_FontLoad)(HSOUNDFONT handle, int preset, int bank) = 0;
BOOL(WINAPI* BASS_MIDI_FontLoadEx)(HSOUNDFONT handle, int preset, int bank, DWORD length, DWORD flags) = 0;
BOOL(WINAPI* BASS_MIDI_FontUnload)(HSOUNDFONT handle, int preset, int bank) = 0;
BOOL(WINAPI* BASS_MIDI_FontCompact)(HSOUNDFONT handle) = 0;
BOOL(WINAPI* BASS_MIDI_FontPack)(HSOUNDFONT handle, const void* outfile, const void* encoder, DWORD flags) = 0;
BOOL(WINAPI* BASS_MIDI_FontUnpack)(HSOUNDFONT handle, const void* outfile, DWORD flags) = 0;
DWORD(WINAPI* BASS_MIDI_FontFlags)(HSOUNDFONT handle, DWORD flags, DWORD mask) = 0;
BOOL(WINAPI* BASS_MIDI_FontSetVolume)(HSOUNDFONT handle, float volume) = 0;
float (WINAPI* BASS_MIDI_FontGetVolume)(HSOUNDFONT handle) = 0;

DWORD(WINAPI* BASS_MIDI_ConvertEvents)(const BYTE* data, DWORD length, BASS_MIDI_EVENT* events, DWORD count, DWORD flags) = 0;

BOOL(WINAPI* BASS_MIDI_InGetDeviceInfo)(DWORD device, BASS_MIDI_DEVICEINFO* info) = 0;
BOOL(WINAPI* BASS_MIDI_InInit)(DWORD device, MIDIINPROC* proc, void* user) = 0;
BOOL(WINAPI* BASS_MIDI_InFree)(DWORD device) = 0;
BOOL(WINAPI* BASS_MIDI_InStart)(DWORD device) = 0;
BOOL(WINAPI* BASS_MIDI_InStop)(DWORD device) = 0;