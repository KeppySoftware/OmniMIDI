/*
OmniMIDI v15+ (Rewrite) for Windows NT

This file contains the required code to run the driver under Windows 7 SP1 and later.
This file is useful only if you want to compile the driver under Windows, it's not needed for Linux/macOS porting.
*/

#include "pch.h"
#include "SynthMain.h"

bool OmniMIDI::SynthModule::LoadLib(Lib* Target) {
	wchar_t SysDir[MAX_PATH] = { 0 };
	wchar_t DLLPath[MAX_PATH] = { 0 };
	WIN32_FIND_DATA FD = { 0 };

	// Check if library is loaded
	if (Target->Library == nullptr) {
		// If not, begin loading process
		// LOG(SynErr, L"Loading library...");
		// LOG(SynErr, Target->Path);

		// Check if the host MIDI application has a version of the library
		// in memory already.
		if ((Target->Library = GetModuleHandle(Target->Path)) != nullptr)
		{
			// (TODO) Make it so we can load our own version of it
			// For now, just make the driver try and use that instead
			return (Target->AppSelfHosted = true);
		}
		// Otherwise, let's load our own
		else {
			// Let's get the system path
			PWSTR Dir;
			LSTATUS Success = SHGetKnownFolderPath(FOLDERID_System, 0, NULL, &Dir);
			bool Good = SUCCEEDED(Success);
			assert(Good == true);

			if (Good)
			{
				Success = StringCchPrintfW(SysDir, sizeof(SysDir), L"%ws", Dir);
				Good = SUCCEEDED(Success);
				assert(Good == true);
			}

			CoTaskMemFree((LPVOID)Dir);

			LOG(SynErr, SysDir);

			// If we failed to get a system path,
			// return false
			if (!Good)
				return false;

			int swp = swprintf_s(DLLPath, MAX_PATH, L"%s\\OmniMIDI\\%s.dll\0", SysDir, Target->Path);
			assert(swp != -1);

			// LOG(SynErr, DLLPath);

			if (FindFirstFile(DLLPath, &FD) == INVALID_HANDLE_VALUE)
				return false;
			else {
				Target->Library = LoadLibrary(DLLPath);
				assert(Target->Library != 0);

				if (!Target->Library)
					return false;
			}
		}
	}

	return true;
}

bool OmniMIDI::SynthModule::UnloadLib(Lib* Target) {
	// Check if library is already loaded
	if (Target->Library != nullptr) {
		// Check if the library was originally loaded by the
		// hosting MIDI application, this happens sometimes.
		if (Target->AppSelfHosted)
		{
			// It was, set Lib to nullptr and return true
			Target->AppSelfHosted = false;
			Target->Library = nullptr;
			return true;
		}

		Target->Initialized = false;
		Target->LoadFailed = false;

		bool r = FreeLibrary(Target->Library);
		assert(r == true);

		Target->Library = nullptr;
	}

	// It is, return true
	return true;
}

void OmniMIDI::SynthModule::LoadFunc(Lib* Target, void* Func, const char* FuncName) {
	// If target is already initialized, just return
	if (Target->Initialized)
		return;

	if (Target->Library == nullptr)
	{
		if (!LoadLib(Target))
			Target->LoadFailed = true;
	}

	if (Target->Library != nullptr) {
		// Load the func (This macro makes it so much easier)
		*((void**)&Func) = GetProcAddress(Target->Library, FuncName);

		assert(*((void**)&Func) != 0);

		// Check if the function does in fact exist, if it doesn't then die
		if (!(*((void**)&Func) != 0))
			Target->LoadFailed = true;
	}
}

bool OmniMIDI::SynthModule::LoadFuncs() {
	if (!BAudLib.Path)
		BAudLib.Path = L"BASS";

	if (!BMidLib.Path)
		BMidLib.Path = L"BASSMIDI";

	int limit = sizeof(LibImports) / sizeof(LibImports[0]);

	for (int i = 0; i < limit; i++)
	{
		if (!LoadLib(&BAudLib))
			return false;

		*(LibImports[i].ptr) = (void*)GetProcAddress(BAudLib.Library, LibImports[i].name);
		if (*(LibImports[i].ptr))
			continue;

		if (!LoadLib(&BMidLib))
			return false;

		*(LibImports[i].ptr) = (void*)GetProcAddress(BMidLib.Library, LibImports[i].name);
		if (*(LibImports[i].ptr))
			continue;
	}

	// Sanity check
	for (int i = 0; i < limit; i++)
	{
		if (!*(LibImports[i].ptr))
		{
			LOG(SynErr, L"A function could not be loaded.");
			return false;
		}
	}

	return !(BAudLib.LoadFailed && BMidLib.LoadFailed);
}

bool OmniMIDI::SynthModule::UnloadFuncs() {
	int limit = sizeof(LibImports) / sizeof(LibImports[0]);

	for (int i = 0; i < limit; i++)
		*(LibImports[i].ptr) = nullptr;

	if (!UnloadLib(&BAudLib))
		return false;

	if (!UnloadLib(&BMidLib))
		return false;

	return true;
}

bool OmniMIDI::SynthModule::LoadSynthModule() {
	// LOG(SynErr, L"LoadSynthModule called.");
	if (LoadFuncs()) {
		// LOG(SynErr, L"LoadFuncs succeeded.");
		// TODO
		return true;
	}
	
	return false;
}

bool OmniMIDI::SynthModule::UnloadSynthModule() {
	// LOG(SynErr, L"UnloadSynthModule called.");
	if (UnloadFuncs()) {
		// LOG(SynErr, L"UnloadFuncs succeeded.");
		// TODO
		return true;
	}

	return false;
}

bool OmniMIDI::SynthModule::StartSynthModule() {
	if (BASS_Init(-1, 48000, BASS_DEVICE_STEREO | BASS_DEVICE_SOFTWARE, nullptr, nullptr))
	{
		AudioStream = BASS_MIDI_StreamCreate(16, BASS_SAMPLE_FLOAT | BASS_MIDI_ASYNC, 0);
		if (!AudioStream) 
		{
			LOG(SynErr, L"BASS_MIDI_StreamCreate failed!");
			return false;
		}

		SoundFonts = (BASS_MIDI_FONTEX*)malloc(1 * sizeof(BASS_MIDI_FONTEX));
		assert(SoundFonts != 0);

		if (!SoundFonts) {
			LOG(SynErr, L"SoundFonts failed!");
			StopSynthModule();
			return false;
		}

		SoundFonts[0].font = BASS_MIDI_FontInit(L"gm.sf2", BASS_UNICODE | BASS_MIDI_FONT_NOLIMITS | BASS_MIDI_FONT_MMAP);
		SoundFonts[0].spreset = -1; // all presets
		SoundFonts[0].sbank = -1; // all banks
		SoundFonts[0].dpreset = -1; // all presets
		SoundFonts[0].dbank = 0; // default banks
		SoundFonts[0].dbanklsb = 0; // destination bank LSB 0

		BASS_MIDI_FontLoad(SoundFonts[0].font, SoundFonts[0].spreset, SoundFonts[0].sbank);
		BASS_MIDI_StreamSetFonts(AudioStream, SoundFonts, 1 | BASS_MIDI_FONT_EX);

		BASS_SetConfig(BASS_CONFIG_UPDATEPERIOD, 5);
		BASS_SetConfig(BASS_CONFIG_UPDATETHREADS, 4);

		BASS_ChannelSetAttribute(AudioStream, BASS_ATTRIB_BUFFER, 0);

		if (!BASS_ChannelPlay(AudioStream, false))
		{
			LOG(SynErr, L"BASS_ChannelPlay failed.");
			return false;
		}

		return true;
	}

	LOG(SynErr, L"BASS_Init failed.");
	return false;
}

bool OmniMIDI::SynthModule::StopSynthModule() {
	if (SoundFonts && SoundFonts[0].font) {
		BASS_MIDI_FontFree(SoundFonts[0].font);
		free(SoundFonts);
		LOG(SynErr, L"SoundFonts freed.");
	}

	BASS_ChannelStop(AudioStream);
	BASS_StreamFree(AudioStream);
	AudioStream = 0;
	LOG(SynErr, L"BASS_StreamFree called!");

	BASS_Free();
	LOG(SynErr, L"BASS freed.");

	return true;
}

SynthResult OmniMIDI::SynthModule::UPlayShortEvent(unsigned int ev) {
	/*

	For more info about how an event is structured, read this doc from Microsoft:
	https://learn.microsoft.com/en-us/windows/win32/api/mmeapi/nf-mmeapi-midioutshortmsg

	-
	TL;DR is here though:
	Let's assume that we have an event coming through, of value 0x007F7F95.

	The high-order byte of the high word is ignored.

	The low-order of the high word contains the first part of the data, which, for
	a NoteOn event, is the key number (from 0 to 127).

	The high-order of the low word contains the second part of the data, which, for
	a NoteOn event, is the velocity of the key (again, from 0 to 127).

	The low-order byte of the low word contains two nibbles combined into one.
	The first nibble contains the type of event (1001 is 0x9, which is a NoteOn),
	while the second nibble contains the channel (0101 is 0x5, which is the 5th channel).

	So, in short, we can read it as follows:
	0x957F7F should press the 128th key on the keyboard at full velocity, on MIDI channel 5.

	But hey! We can also receive events with a long running status, which means there's no status byte!
	That event, if we consider the same example from before, will look like this: 0x00007F7F

	Such event will only work if the driver receives a status event, which will be stored in memory.
	So, in a sequence of data like this:
	..
	1. 0x00044A90 (NoteOn on key 75 at velocity 10)
	2. 0x00007F7F (... then on key 128 at velocity 127)
	3. 0x00006031 (... then on key 50 at velocity 96)
	4. 0x0000605F (... and finally on key 95 at velocity 96)
	..
	The same status from event 1 will be applied to all the status-less events from 2 and onwards.
	-

	INFO: ev will be recasted as char in some parts of the code, since those parts
	do not require the high-word part of the unsigned int.

	*/
	
	unsigned int r = 0;
	unsigned int tev = ev;

	if (CHKLRS(GETSTATUS(tev)) != 0) LastRunningStatus = GETSTATUS(tev);
	else tev = ev << 8 | LastRunningStatus;

	unsigned int ch = tev & 0xF;
	unsigned int evt = 0;
	unsigned int param = GETFP(tev);

	unsigned int cmd = GETCMD(tev);
	unsigned int len = 3;
	bool ok = true;

	switch (LastRunningStatus) {
	// Handle 0xFF (GS reset) first!
	default:
		switch (GETCMD(ev)) {
		case MIDI_NOTEON:
			evt = MIDI_EVENT_NOTE;
			break;
		case MIDI_NOTEOFF:
			evt = MIDI_EVENT_NOTE;
			param = (char)param;
			break;
		case MIDI_POLYAFTER:
			evt = MIDI_EVENT_KEYPRES;
			break;
		case MIDI_PROGCHAN:
			evt = MIDI_EVENT_PROGRAM;
			param = (char)param;
			break;
		case MIDI_CHANAFTER:
			evt = MIDI_EVENT_CHANPRES;
			param = (char)param;
			break;
		default:
			if (tev == 0x7FFFFFFF) {
				for (int i = 0; i < 16; i++) {
					if (!BASS_MIDI_StreamEvent(AudioStream, i, MIDI_EVENT_SYSTEM, MIDI_SYSTEM_XG))
						ok = false;
				}
				return ok ? SYNTH_OK : SYNTH_INVALPARAM;
			}

			// Some events do not have a specific counter part on BASSMIDI's side, so they have
			// to be directly fed to the library by using the BASS_MIDI_EVENTS_RAW flag.
			if (!(tev - 0x80 & 0xC0))
			{
				r = BASS_MIDI_StreamEvents(AudioStream, BASS_MIDI_EVENTS_RAW, &tev, 3);
				return (r != -1) ? SYNTH_OK : SYNTH_INVALPARAM;
			}

			if (!((tev - 0xC0) & 0xE0)) len = 2;
			else if (GETCMD(tev) == 0xF0)
			{
				switch (tev & 0xF)
				{
				case 3:
					len = 2;
					break;
				default:
					// Not supported by OmniMIDI!
					return false;
				}
			}

			r = BASS_MIDI_StreamEvents(AudioStream, BASS_MIDI_EVENTS_RAW, &tev, len);
			return (r > -1) ? SYNTH_OK : SYNTH_INVALPARAM;
		}
	}

	r = BASS_MIDI_StreamEvent(AudioStream, ch, evt, param);
	return (r > -1) ? SYNTH_OK : SYNTH_INVALPARAM;
}

SynthResult OmniMIDI::SynthModule::PlayShortEvent(unsigned int ev) {
	if (BMidLib.Initialized && !BMidLib.LoadFailed)
		return UPlayShortEvent(ev);
	else
		return SYNTH_NOTINIT;
}

SynthResult OmniMIDI::SynthModule::UPlayLongEvent(char* ev, unsigned int size) {
	unsigned int r = BASS_MIDI_StreamEvents(AudioStream, BASS_MIDI_EVENTS_RAW, ev, size);
	return (r != -1) ? SYNTH_OK : SYNTH_INVALPARAM;
}

SynthResult OmniMIDI::SynthModule::PlayLongEvent(char* ev, unsigned int size) {
	if (!BMidLib.Initialized)
		return SYNTH_NOTINIT;

	if (BMidLib.LoadFailed)
		return SYNTH_INITERR;

	// No buffer?
	if (size < 1)
		return SYNTH_OK;

	// The maximum size for a long header is 64K!
	if (size > 65535)
		return SYNTH_INVALPARAM;

	return UPlayLongEvent(ev, size);
}