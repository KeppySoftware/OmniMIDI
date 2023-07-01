/*
OmniMIDI v15+ (Rewrite) for Windows NT

This file contains the required code to run the driver under Windows 7 SP1 and later.
This file is useful only if you want to compile the driver under Windows, it's not needed for Linux/macOS porting.
*/

#include "SynthMain.h"

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
			switch (GETCHANNEL(tev))
			{
			case 0x3:
				// This is 0xF3, which is a system reset.
				len = 2;
				break;
			case 0xF:
				// This is 0xFF, which is a system reset.
				BASS_MIDI_StreamEvent(AudioStream, 0, MIDI_EVENT_SYSTEMEX, MIDI_SYSTEM_DEFAULT);
				return true;
			case 0xA:	// Start (CookedPlayer)
			case 0xB:	// Continue (CookedPlayer)
			case 0xC:	// Stop (CookedPlayer)
			case 0xE:	// Sensing
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

bool OmniMIDI::SynthModule::LoadFuncs() {
	int limit = sizeof(LibImports) / sizeof(LibImports[0]);

	if (!NanoSleep)
	{
		auto mod = GetModuleHandleA("ntdll");
		assert(mod != 0);

		if (!mod)
		{
			FNERROR(SynErr, "Could not load NTDLL from memory!!! What's happening???");
			return false;
		}

		NanoSleep = (unsigned int (WINAPI*)(unsigned char, signed long long*))GetProcAddress(mod, "NtDelayExecution");
		assert(NanoSleep != 0);

		if (!NanoSleep)
		{
			FNERROR(SynErr, "Where's NtDelayExecution... Is this Windows 95?");
			return false;
		}
	}

	void* ptr = nullptr;

	// Load required libs
	if (!BAudLib.LoadLib())
		return false;

	if (!BMidLib.LoadLib())
		return false;

	switch (SynthSettings->AudioEngine) {
	case WASAPI:
		if (!BWasLib.LoadLib())
			return false;
		break;

	case ASIO:
		if (!BAsiLib.LoadLib())
			return false;
		break;
	}

#if defined(_M_AMD64) || defined(_M_IX86)
	if (SynthSettings->LoudMax)
	{
		if (!BVstLib.LoadLib())
			return false;
	}
#endif

	for (int i = 0; i < limit; i++)
	{
		ptr = (void*)GetProcAddress(BAudLib.Library, LibImports[i].name);
		if (ptr)
		{
			*(LibImports[i].ptr) = ptr;
			continue;
		}

		ptr = (void*)GetProcAddress(BMidLib.Library, LibImports[i].name);
		if (ptr)
		{
			*(LibImports[i].ptr) = ptr;
			continue;
		}

		switch (SynthSettings->AudioEngine) {
		case WASAPI:
			ptr = (void*)GetProcAddress(BWasLib.Library, LibImports[i].name);
			if (ptr)
			{
				*(LibImports[i].ptr) = ptr;
				continue;
			}
			break;

		case ASIO:
			ptr = (void*)GetProcAddress(BAsiLib.Library, LibImports[i].name);
			if (ptr)
			{
				*(LibImports[i].ptr) = ptr;
				continue;
			}
			break;
		}

		if (BVstLib.Initialized)
		{
			ptr = (void*)GetProcAddress(BVstLib.Library, LibImports[i].name);
			if (ptr)
			{
				*(LibImports[i].ptr) = ptr;
				continue;
			}
		}
	}

	return true;
}

bool OmniMIDI::SynthModule::UnloadFuncs() {
	if (!BAsiLib.UnloadLib())
		return false;

	if (!BWasLib.UnloadLib())
		return false;

	if (!BMidLib.UnloadLib())
		return false;

	if (!BAudLib.UnloadLib())
		return false;

	if (!BVstLib.UnloadLib())
		return false;

	return true;
}

bool OmniMIDI::SynthModule::LoadSynthModule() {
	if (!SynthSettings)
		SynthSettings = new Settings;

	// LOG(SynErr, L"LoadSynthModule called.");
	if (LoadFuncs()) {
		Events = new EvBuf(32768);
		_EvtThread = std::thread(&SynthModule::EventsThread, this);
		// LOG(SynErr, L"LoadFuncs succeeded.");
		// TODO
		return true;
	}
	
	NERROR(SynErr, "Something went wrong here!!!", true);
	return false;
}

bool OmniMIDI::SynthModule::UnloadSynthModule() {
	// LOG(SynErr, L"UnloadSynthModule called.");
	if (UnloadFuncs()) {
		// LOG(SynErr, L"UnloadFuncs succeeded.");
		_EvtThread.join();

		Events->GetStats();
		delete Events;

		SoundFonts.clear();

		delete SynthSettings;
		SynthSettings = nullptr;
		return true;
	}

	NERROR(SynErr, "Something went wrong here!!!", true);
	return false;
}

bool OmniMIDI::SynthModule::StartSynthModule() {
	// SF path
	WinUtils::SysPath Utils;
	wchar_t OMPath[MAX_PATH] = { 0 };

	// BASS stream flags
	BASS_ASIO_DEVICEINFO info;
	unsigned int ASIOCount = 0, ASIODev = 0;
	unsigned int StreamFlags = BASS_SAMPLE_FLOAT | BASS_MIDI_ASYNC;

	// If the audio stream is not 0, then stream is allocated already
	if (AudioStream)
		return true;

	switch (SynthSettings->AudioEngine) {
	case BASS_INTERNAL:
		BASS_SetConfig(BASS_CONFIG_BUFFER, 0);
		BASS_SetConfig(BASS_CONFIG_UPDATEPERIOD, 0);
		BASS_SetConfig(BASS_CONFIG_UPDATETHREADS, 0);

		if (!BASS_Init(-1, 48000, BASS_DEVICE_STEREO, nullptr, nullptr)) {
			NERROR(SynErr, "BASS_Init failed.", false);
			return false;
		}

		AudioStream = BASS_MIDI_StreamCreate(16, StreamFlags, 0);
		if (!AudioStream) {
			NERROR(SynErr, "BASS_MIDI_StreamCreate failed!", false);
			return false;
		}

		BASS_ChannelSetAttribute(AudioStream, BASS_ATTRIB_BUFFER, 0);

		if (!BASS_ChannelPlay(AudioStream, false)) {
			NERROR(SynErr, "BASS_ChannelPlay failed.", false);
			return false;
		}

		_AudThread = std::thread(&SynthModule::AudioThread, this);
		if (!_AudThread.joinable()) {
			NERROR(SynErr, "_AudThread failed.", false);
			return false;
		}

		break;

	case WASAPI:
		StreamFlags |= BASS_STREAM_DECODE;

		if (!BASS_Init(0, 48000, BASS_DEVICE_STEREO, nullptr, nullptr)) {
			NERROR(SynErr, "BASS_Init failed.", false);
			return false;
		}

		AudioStream = BASS_MIDI_StreamCreate(16, StreamFlags, 0);
		if (!AudioStream) {
			NERROR(SynErr, "BASS_MIDI_StreamCreate w/ BASS_STREAM_DECODE failed!", false);
			return false;
		}

		if (!BASS_WASAPI_Init(-1, 0, 0, BASS_WASAPI_ASYNC | BASS_WASAPI_EVENT | BASS_WASAPI_SAMPLES, SynthSettings->WASAPIBuf, 0, WASAPIPROC_BASS, (void*)AudioStream)) {
			NERROR(SynErr, "BASS_WASAPI_Init failed.", false);
			return false;
		}

		if (!BASS_WASAPI_Start()) {
			NERROR(SynErr, "BASS_WASAPI_Start failed.", false);
			return false;
		}

		break;

	case ASIO:
		StreamFlags |= BASS_STREAM_DECODE;

		if (!BASS_Init(0, 48000, BASS_DEVICE_STEREO, nullptr, nullptr)) {
			NERROR(SynErr, "BASS_Init failed.", false);
			return false;
		}

		AudioStream = BASS_MIDI_StreamCreate(16, StreamFlags, 0);
		if (!AudioStream) {
			NERROR(SynErr, "BASS_MIDI_StreamCreate w/ BASS_STREAM_DECODE failed!", false);
			return false;
		}

		// Get the amount of ASIO devices available
		for (0; BASS_ASIO_GetDeviceInfo(ASIOCount, &info); ASIOCount++);

		if (ASIOCount < 1) {
			NERROR(SynErr, "No ASIO devices available!", false);
			return false;
		}

		// Iterate through the available audio devices
		for (0; BASS_ASIO_GetDeviceInfo(ASIODev, &info); ASIODev++)
		{
			// Return the correct ID when found
			if (strcmp(SynthSettings->ASIODevice.c_str(), info.name) == 0)
				break;
		}

		if (!BASS_ASIO_Init(ASIODev, BASS_ASIO_THREAD | BASS_ASIO_JOINORDER)) {
			NERROR(SynErr, "BASS_ASIO_Init failed!", false);
			return false;
		}

		BASS_ASIO_SetRate(0);

		BASS_ASIO_ChannelSetFormat(0, 0, BASS_ASIO_FORMAT_FLOAT);
		BASS_ASIO_ChannelEnableBASS(0, 0, AudioStream, 1);

		if (!BASS_ASIO_Start(0, std::thread::hardware_concurrency())) {
			NERROR(SynErr, "BASS_ASIO_Start failed!", false);
			return false;
		}

		break;

	case INVALID_ENGINE:
	default:
		NERROR(SynErr, "Invalid or unimplemented engine!", false);
		return false;
	}

	if (Utils.GetFolderPath(FOLDERID_Profile, OMPath, sizeof(OMPath))) {
		swprintf_s(OMPath, L"%s\\OmniMIDI\\lists\\OmniMIDI_A.json\0", OMPath);

		std::fstream sfs;
		sfs.open(OMPath);

		if (sfs.is_open()) {
			try {
				// Read the JSON data from there
				auto json = nlohmann::json::parse(sfs, nullptr, false, true);

				if (json != nullptr) {
					auto JsonData = json["SoundFonts"];

					if (!(JsonData == nullptr || JsonData.size() < 1)) {
						for (int i = 0; i < JsonData.size(); i++) {
							BASS_MIDI_FONTEX sf;
							nlohmann::json subitem = JsonData[i];

							// Is item valid?
							if (subitem != nullptr) {
								std::string sfpath = subitem["path"];

								bool xgdrums = subitem["xgdrums"];
								sf.spreset = subitem["spreset"];
								sf.sbank = subitem["sbank"];
								sf.dpreset = subitem["dpreset"];
								sf.dbank = subitem["dbank"];
								sf.dbanklsb = subitem["dbanklsb"];

								sf.font = BASS_MIDI_FontInit(sfpath.c_str(), xgdrums ? BASS_MIDI_FONT_XGDRUMS : 0 | BASS_MIDI_FONT_NOLIMITS | BASS_MIDI_FONT_MMAP);

								// Check if the soundfont loads, if it does then it's valid
								if (BASS_MIDI_FontLoad(sf.font, sf.spreset, sf.sbank))
									SoundFonts.push_back(sf);
							}

							// If it's not, then let's loop until the end of the JSON struct
						}

						std::reverse(SoundFonts.begin(), SoundFonts.end());
						BASS_MIDI_StreamSetFonts(AudioStream, &SoundFonts[0], (unsigned int)SoundFonts.size() | BASS_MIDI_FONT_EX);
					}
					else NERROR(SynErr, "SoundFonts JSON does exist, but it does not contain the required items.", false);
				}
				else NERROR(SynErr, "Invalid JSON structure!", false);


			}
			catch (nlohmann::json::type_error ex) {
				char asdf[1024] = { 0 };
				sprintf_s(asdf, "The JSON is corrupted or malformed!\n\nnlohmann::json says: %s", ex.what());
				NERROR(SynErr, asdf, false);
			}
			sfs.close();
		}
		else NERROR(SynErr, "SoundFonts JSON does not exist.", false);
	}

	BASS_ChannelSetAttribute(AudioStream, BASS_ATTRIB_MIDI_VOICES, SynthSettings->MaxVoices);
	BASS_ChannelSetAttribute(AudioStream, BASS_ATTRIB_MIDI_CPU, SynthSettings->MaxCPU);

#if defined(_M_AMD64) || defined(_M_IX86)
	wchar_t LoudMax[MAX_PATH] = { 0 };
	if (SynthSettings->LoudMax && Utils.GetFolderPath(FOLDERID_Profile, LoudMax, sizeof(LoudMax)))
	{
#if defined(_M_AMD64)
		swprintf_s(LoudMax, L"%s\\OmniMIDI\\LoudMax\\LoudMax64.dll", LoudMax);
#elif defined(_M_IX86)
		swprintf_s(LoudMax, L"%s\\OmniMIDI\\LoudMax\\LoudMax32.dll", LoudMax);
#endif

		if (GetFileAttributes(LoudMax) != INVALID_FILE_ATTRIBUTES)
			BASS_VST_ChannelSetDSP(AudioStream, LoudMax, BASS_UNICODE, 1);
	}
#endif

	return true;
}

bool OmniMIDI::SynthModule::StopSynthModule() {
	if (AudioStream) {
		BASS_StreamFree(AudioStream);
		AudioStream = 0;

		switch (SynthSettings->AudioEngine) {
		case BASS_INTERNAL:
			_AudThread.join();
			break;
		case WASAPI:
			BASS_WASAPI_Stop(true);
			BASS_WASAPI_Free();
			break;
		case ASIO:
			if (BASS_ASIO_IsStarted()) {
				BASS_ASIO_Stop();
				BASS_ASIO_Free();
			}
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

int OmniMIDI::SynthModule::TalkToBASSMIDI(unsigned int evt, unsigned int chan, unsigned int param) {
	if (!BMidLib.Initialized)
		return 0;

	return BASS_MIDI_StreamEvent(AudioStream, chan, evt, param);
}