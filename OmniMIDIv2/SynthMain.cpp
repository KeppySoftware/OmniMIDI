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

			// If we failed to get a system path,
			// return false
			if (!Good)
				return false;

			int swp = swprintf_s(DLLPath, MAX_PATH, L"%s.dll\0", Target->Path);
			assert(swp != -1);

			Target->Library = LoadLibrary(DLLPath);

			if (!Target->Library)
			{
				swp = swprintf_s(DLLPath, MAX_PATH, L"%s\\OmniMIDI\\%s.dll\0", SysDir, Target->Path);
				assert(swp != -1);

				Target->Library = LoadLibrary(DLLPath);
				assert(Target->Library != 0);

				if (!Target->Library)
					return false;
			}
		}
	}

	Target->Initialized = true;
	return true;
}

bool OmniMIDI::SynthModule::UnloadLib(Lib* Target) {
	// Check if library is already loaded
	if (Target->Library != nullptr) {
		// Set all flags to false
		Target->Initialized = false;
		Target->LoadFailed = false;

		// Check if the library was originally loaded by the
		// hosting MIDI application, this happens sometimes.
		if (Target->AppSelfHosted)
		{
			// It was, set Lib to nullptr and return true
			Target->AppSelfHosted = false;
		}
		else {
			bool r = FreeLibrary(Target->Library);
			assert(r == true);
		}

		Target->Library = nullptr;
	}

	// It is, return true
	return true;
}

bool OmniMIDI::SynthModule::LoadFuncs() {
	if (!BAudLib.Path)
		BAudLib.Path = L"BASS";

	if (!BMidLib.Path)
		BMidLib.Path = L"BASSMIDI";

	if (!BWasLib.Path)
		BWasLib.Path = L"BASSWASAPI";

	int limit = sizeof(LibImports) / sizeof(LibImports[0]);

	if (!NanoSleep)
		NanoSleep = (unsigned int (WINAPI*)(unsigned char, signed long long*))GetProcAddress(GetModuleHandle(L"ntdll"), "NtDelayExecution");

	for (int i = 0; i < limit; i++)
	{
		if (!LoadLib(&BAudLib))
			return false;

		if (!LoadLib(&BMidLib))
			return false;

		*(LibImports[i].ptr) = (void*)GetProcAddress(BAudLib.Library, LibImports[i].name);
		if (*(LibImports[i].ptr))
			continue;

		*(LibImports[i].ptr) = (void*)GetProcAddress(BMidLib.Library, LibImports[i].name);
		if (*(LibImports[i].ptr))
			continue;

		if (AudioEngine == WASAPI)
		{
			if (!LoadLib(&BWasLib))
				return false;

			*(LibImports[i].ptr) = (void*)GetProcAddress(BWasLib.Library, LibImports[i].name);
			if (*(LibImports[i].ptr))
				continue;
		}
	}

	return !(BAudLib.LoadFailed && BMidLib.LoadFailed && BWasLib.LoadFailed);
}

bool OmniMIDI::SynthModule::UnloadFuncs() {
	if (!UnloadLib(&BMidLib))
		return false;

	if (!UnloadLib(&BWasLib))
		return false;

	if (!UnloadLib(&BAudLib))
		return false;

	return true;
}

void OmniMIDI::SynthModule::AudioThread() {
	while (IsSynthInitialized()) {
		BASS_ChannelUpdate(AudioStream, 0);
		NanoSleep(0, &onenano);
	}
}

void OmniMIDI::SynthModule::EventsThread() {
	// Spin while waiting for the stream to go online
	while (AudioStream == 0)
		NanoSleep(0, &onenano);

	while (IsSynthInitialized()) {
		if (!ProcessEvBuf())
			NanoSleep(0, &onenano);
	}
}

bool OmniMIDI::SynthModule::ProcessEvBuf() {
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

	unsigned int tev = 0;

	if (!Events->Pop(tev))
		return false;

	if (CHKLRS(GETSTATUS(tev)) != 0) LastRunningStatus = GETSTATUS(tev);
	else tev = tev << 8 | LastRunningStatus;

	unsigned int evt = MIDI_SYSTEM_DEFAULT;
	unsigned char status = GETSTATUS(tev);
	unsigned char cmd = GETCMD(tev);
	unsigned char ch = GETCHANNEL(tev);
	unsigned char param1 = GETFP(tev);
	unsigned char param2 = GETSP(tev);

	unsigned int len = 3;

	switch (cmd) {
	case MIDI_NOTEON:
		// param1 is the key, param2 is the velocity
		evt = MIDI_EVENT_NOTE;
		break;
	case MIDI_NOTEOFF:
		// param1 is the key, ignore param2
		evt = MIDI_EVENT_NOTE;
		param2 = 0;
		break;
	case MIDI_POLYAFTER:
		evt = MIDI_EVENT_KEYPRES;
		break;
	case MIDI_PROGCHAN:
		evt = MIDI_EVENT_PROGRAM;
		break;
	case MIDI_CHANAFTER:
		evt = MIDI_EVENT_CHANPRES;
		break;
	default:
		// Some events do not have a specific counter part on BASSMIDI's side, so they have
		// to be directly fed to the library by using the BASS_MIDI_EVENTS_RAW flag.
		if (!(tev - 0x80 & 0xC0))
		{
			BASS_MIDI_StreamEvents(AudioStream, BASS_MIDI_EVENTS_RAW, &tev, 3);
			return true;
		}

		if (!((tev - 0xC0) & 0xE0)) len = 2;
		else if (cmd == 0xF0)
		{
			switch (tev & 0xF)
			{
			case 0x3:
				len = 2;
				break;
			case 0xF:
				BASS_MIDI_StreamEvents(AudioStream, BASS_MIDI_EVENTS_RAW, &XGReset, 9);
				return true;
			default:
				// Not supported by OmniMIDI!
				return true;
			}
		}

		BASS_MIDI_StreamEvents(AudioStream, BASS_MIDI_EVENTS_RAW, &tev, len);
		return true;
	}

	BASS_MIDI_StreamEvent(AudioStream, ch, evt, param2 << 8 | param1);
	return true;
}

bool OmniMIDI::SynthModule::LoadSynthModule() {
	// LOG(SynErr, L"LoadSynthModule called.");
	if (LoadFuncs()) {
		SoundFonts = new BASS_MIDI_FONTEX[1];
		Events = new EvBuf(32768);
		_EvtThread = std::thread(&SynthModule::EventsThread, this);
		// LOG(SynErr, L"LoadFuncs succeeded.");
		// TODO
		return true;
	}
	
	NERROR(SynErr, L"Something went wrong here!!!", true);
	return false;
}

bool OmniMIDI::SynthModule::UnloadSynthModule() {
	// LOG(SynErr, L"UnloadSynthModule called.");
	if (UnloadFuncs()) {
		// LOG(SynErr, L"UnloadFuncs succeeded.");
		_EvtThread.join();

#ifdef _STATSDEV
		Events->GetStats();
#endif
		delete Events;
		delete[] SoundFonts;
		return true;
	}

	NERROR(SynErr, L"Something went wrong here!!!", true);
	return false;
}

bool OmniMIDI::SynthModule::StartSynthModule() {
	unsigned int StreamFlags = BASS_SAMPLE_FLOAT | BASS_MIDI_ASYNC;

	if (AudioStream)
		return true;

	switch (AudioEngine) {
	case BASS_INTERNAL:
		BASS_SetConfig(BASS_CONFIG_BUFFER, 0);
		BASS_SetConfig(BASS_CONFIG_UPDATEPERIOD, 0);
		BASS_SetConfig(BASS_CONFIG_UPDATETHREADS, 0);

		if (!BASS_Init(-1, 48000, BASS_DEVICE_STEREO, nullptr, nullptr)) {
			NERROR(SynErr, L"BASS_Init failed.", false);
			return false;
		}

		AudioStream = BASS_MIDI_StreamCreate(16, StreamFlags, 0);
		if (!AudioStream)
		{
			NERROR(SynErr, L"BASS_MIDI_StreamCreate failed!", false);
			return false;
		}

		if (!BASS_ChannelPlay(AudioStream, false)) {
			NERROR(SynErr, L"BASS_ChannelPlay failed.", false);
			return false;
		}

		_AudThread = std::thread(&SynthModule::AudioThread, this);
		if (!_AudThread.joinable()) {
			NERROR(SynErr, L"_AudThread failed.", false);
			return false;
		}

		break;

	case WASAPI:
		StreamFlags |= BASS_STREAM_DECODE;

		if (!BASS_Init(0, 48000, BASS_DEVICE_STEREO, nullptr, nullptr)) {
			NERROR(SynErr, L"BASS_Init failed.", false);
			return false;
		}
		
		AudioStream = BASS_MIDI_StreamCreate(16, StreamFlags, 0);
		if (!AudioStream)
		{
			NERROR(SynErr, L"BASS_MIDI_StreamCreate w/ BASS_STREAM_DECODE failed!", false);
			return false;
		}

		if (!BASS_WASAPI_Init(-1, 0, 0, BASS_WASAPI_ASYNC | BASS_WASAPI_EVENT | BASS_WASAPI_SAMPLES, 32.0f, 0, WASAPIPROC_BASS, (void*)AudioStream)) {
			NERROR(SynErr, L"BASS_WASAPI_Init failed.", false);
			return false;
		}

		if (!BASS_WASAPI_Start()) {
			NERROR(SynErr, L"BASS_WASAPI_Start failed.", false);
			return false;
		}

		break;

	case INVALID_ENGINE:
	default:
		NERROR(SynErr, L"Invalid or unimplemented engine!", false);
		return false;
	}

	SoundFonts[0].font = BASS_MIDI_FontInit(L"E:\\Archive\\MIDIs\\SoundFonts\\Piano_de_Cola_(Grand_Piano).sf2", BASS_UNICODE | BASS_MIDI_FONT_NOLIMITS | BASS_MIDI_FONT_MMAP);
	SoundFonts[0].spreset = -1; // all presets
	SoundFonts[0].sbank = -1; // all banks
	SoundFonts[0].dpreset = -1; // all presets
	SoundFonts[0].dbank = 0; // default banks
	SoundFonts[0].dbanklsb = 0; // destination bank LSB 0

	BASS_MIDI_FontLoad(SoundFonts[0].font, SoundFonts[0].spreset, SoundFonts[0].sbank);
	BASS_MIDI_StreamSetFonts(AudioStream, SoundFonts, 1 | BASS_MIDI_FONT_EX);

	BASS_ChannelSetAttribute(AudioStream, BASS_ATTRIB_BUFFER, 1);
	BASS_ChannelSetAttribute(AudioStream, BASS_ATTRIB_MIDI_VOICES, 1000);
	BASS_ChannelSetAttribute(AudioStream, BASS_ATTRIB_MIDI_CPU, 85);

	return true;
}

bool OmniMIDI::SynthModule::StopSynthModule() {
	if (AudioStream) {
		BASS_StreamFree(AudioStream);
		AudioStream = 0;

		switch (AudioEngine) {
		case BASS_INTERNAL:
			_AudThread.join();
			break;
		case WASAPI:
			BASS_WASAPI_Stop(true);
			BASS_WASAPI_Free();
			break;
		default:
			break;
		}

		BASS_Free();
	}

	return true;
}

SynthResult OmniMIDI::SynthModule::UPlayShortEvent(unsigned int ev) {
	return Events->Push(ev) ? SYNTH_OK : SYNTH_INVALPARAM;
}

SynthResult OmniMIDI::SynthModule::PlayShortEvent(unsigned int ev) {
	if (!BMidLib.Initialized)
		return BMidLib.LoadFailed ? SYNTH_INITERR : SYNTH_NOTINIT;

	return UPlayShortEvent(ev);
}

SynthResult OmniMIDI::SynthModule::UPlayLongEvent(char* ev, unsigned int size) {
	unsigned int r = BASS_MIDI_StreamEvents(AudioStream, BASS_MIDI_EVENTS_RAW, ev, size);
	return (r != -1) ? SYNTH_OK : SYNTH_INVALPARAM;
}

SynthResult OmniMIDI::SynthModule::PlayLongEvent(char* ev, unsigned int size) {
	if (!BMidLib.Initialized)
		return BMidLib.LoadFailed ? SYNTH_INITERR : SYNTH_NOTINIT;

	// The size has to be between 1B and 64KB!
	if (size < 1 || size > 65535)
		return SYNTH_INVALPARAM;

	return UPlayLongEvent(ev, size);
}