/*

	OmniMIDI v15+ (Rewrite) for Windows NT

	This file contains the required code to run the driver under Windows 7 SP1 and later.
	This file is useful only if you want to compile the driver under Windows, it's not needed for Linux/macOS porting.

*/

#define TSF_IMPLEMENTATION
#include "TSFSynth.h"

void OmniMIDI::TinySFSynth::EventsThread() {
	// Spin while waiting for the stream to go online
	while (!IsSynthInitialized())
		NtDelayExecution(-1);

	while (IsSynthInitialized()) {
		if (!ProcessEvBuf())
			NtDelayExecution(-1);
	}
}

bool OmniMIDI::TinySFSynth::ProcessEvBuf() {
	unsigned int tev = 0;
	unsigned int sysev = 0;

	if (!Events->Pop(tev) || !IsSynthInitialized())
		return false;

	if (CHKLRS(GETSTATUS(tev)) != 0) LastRunningStatus = GETSTATUS(tev);
	else tev = tev << 8 | LastRunningStatus;

	unsigned int ev = 0;
	unsigned char status = GETSTATUS(tev);
	unsigned char cmd = GETCMD(tev);
	unsigned char ch = GETCHANNEL(tev);
	unsigned char param1 = GETFP(tev);
	unsigned char param2 = GETSP(tev);

	unsigned int len = 3;

	SDL_LockMutex(g_Mutex);

	switch (cmd) {
	case MIDI_NOTEON:
		tsf_channel_note_on(g_TinySoundFont, ch, param1, ((float)param2) / 128.0f);
		break;
	case MIDI_NOTEOFF:
		tsf_channel_note_off(g_TinySoundFont, ch, param1);
		break;
	case MIDI_PROGCHAN:
		tsf_channel_set_presetnumber(g_TinySoundFont, ch, param1, param2);
		break;
	case MIDI_CMC:
		tsf_channel_midi_control(g_TinySoundFont, ch, param1, param2);
		break;
	case MIDI_PITCHWHEEL:
		tsf_channel_set_pitchwheel(g_TinySoundFont, ch, param2 << 7 | param1);
		break;
	default:
		switch (status) {
		case MIDI_SYSRESET:
			for (int i = 0; i < 16; i++)
				tsf_channel_sounds_off_all(g_TinySoundFont, i);
		}
		// UNSUPPORTED!
		break;
	}

	SDL_UnlockMutex(g_Mutex);

	return true;
}

bool OmniMIDI::TinySFSynth::LoadSynthModule() {
	PrepZwDelayExecution();

	if (!SDLLib)
		SDLLib = new Lib(L"SDL2");

	if (!Settings)
		Settings = new TinySFSettings;

	if (!SDLLib->LoadLib())
		return false;

	for (int i = 0; i < sizeof(SLibImports) / sizeof(SLibImports[0]); i++)
	{
		void* ptr = (void*)GetProcAddress(SDLLib->Ptr(), SLibImports[i].GetName());
		if (ptr)
		{
			SLibImports[i].SetPtr(ptr);
			continue;
		}
	}

	Events = new EvBuf(Settings->EvBufSize);
	_EvtThread = std::jthread(&TinySFSynth::EventsThread, this);

	return true;
}

bool OmniMIDI::TinySFSynth::UnloadSynthModule() {
	if (Events) {
		delete Events;
		Events = nullptr;
	}

	return true;
}

bool OmniMIDI::TinySFSynth::StartSynthModule() {
	WinUtils::SysPath Utils;
	wchar_t OMPath[MAX_PATH] = { 0 };

	if (Running)
		return true;

	if (!Settings)
		return false;

	// Define the desired audio output format we request
	SDL_AudioSpec OutputAudioSpec;
	SDL_AudioSpec FinalAudioSpec;
	memset(&OutputAudioSpec, 0, sizeof(OutputAudioSpec));
	memset(&FinalAudioSpec, 0, sizeof(FinalAudioSpec));
	OutputAudioSpec.freq = Settings->AudioFrequency;
	OutputAudioSpec.format = AUDIO_F32;
	OutputAudioSpec.channels = 2;
	OutputAudioSpec.samples = Settings->Samples;
	OutputAudioSpec.callback = TinySFSynth::cCallback;
	OutputAudioSpec.userdata = this;

	// Initialize the audio system
	if (SDL_AudioInit(0) < 0)
		return false;

	if (Utils.GetFolderPath(FOLDERID_Profile, OMPath, sizeof(OMPath))) {
		swprintf_s(OMPath, L"%s\\OmniMIDI\\lists\\OmniMIDI_A.json\0", OMPath);

		std::fstream sfs;
		sfs.open(OMPath);

		if (sfs.is_open()) {
			try {
				// Read the JSON data from there
				auto json = nlohmann::json::parse(sfs, nullptr, false, true);

				if (json != nullptr) {
					auto& JsonData = json["SoundFonts"];

					if (!(JsonData == nullptr || JsonData.size() < 1)) {
						for (int i = 0; i < JsonData.size(); i++) {
							bool enabled = true;
							nlohmann::json subitem = JsonData[i];

							// Is item valid?
							if (subitem != nullptr) {
								std::string sfpath = subitem["path"].is_null() ? "\0" : subitem["path"];
								enabled = subitem["enabled"].is_null() ? enabled : (bool)subitem["enabled"];

								if (enabled) {
									if (GetFileAttributesA(sfpath.c_str()) != INVALID_FILE_ATTRIBUTES) {
										g_TinySoundFont = tsf_load_filename(sfpath.c_str());
										if (g_TinySoundFont) {
											tsf_channel_set_bank_preset(g_TinySoundFont, 9, 128, 0);
											break;
										}						

										continue;
									}
									else NERROR(SynErr, "The SoundFont \"%s\" could not be found!", false, sfpath.c_str());
								}
							}

							// If it's not, then let's loop until the end of the JSON struct
						}
					}
					else NERROR(SynErr, "\"%s\" does not contain a valid \"SoundFonts\" JSON structure.", false, OMPath);
				}
				else NERROR(SynErr, "Invalid JSON structure!", false);
			}
			catch (nlohmann::json::type_error ex) {
				NERROR(SynErr, "The SoundFont JSON is corrupted or malformed!\n\nnlohmann::json says: %s", ex.what());
			}
			sfs.close();
		}
		else NERROR(SynErr, "SoundFonts JSON does not exist.", false);
	}

	tsf_set_output(g_TinySoundFont, TSF_STEREO_INTERLEAVED, OutputAudioSpec.freq, 0);

	g_Mutex = SDL_CreateMutex();

	// Request the desired audio output format
	dev = SDL_OpenAudioDevice(0, 0, &OutputAudioSpec, &FinalAudioSpec, SDL_AUDIO_ALLOW_FORMAT_CHANGE);
	if (!dev)
		return false;

	LOG(SynErr, "Op: freq %d, ch %d, samp %d\nGot: freq %d, ch %d, samp %d", 
		OutputAudioSpec.freq, OutputAudioSpec.channels, OutputAudioSpec.samples, 
		FinalAudioSpec.freq, FinalAudioSpec.channels, FinalAudioSpec.samples);

	SDL_PauseAudioDevice(dev, 0);

	Running = true;
	return true;
}

bool OmniMIDI::TinySFSynth::StopSynthModule() {
	if (IsSynthInitialized()) {
		SDL_PauseAudioDevice(dev, 1);
		SDL_CloseAudioDevice(dev);
		SDL_DestroyMutex(g_Mutex);
		SDL_AudioQuit();
		tsf_close(g_TinySoundFont);
		g_TinySoundFont = nullptr;
	}

	return true;
}

SynthResult OmniMIDI::TinySFSynth::PlayShortEvent(unsigned int ev) {
	if (!Events || !IsSynthInitialized())
		return SYNTH_NOTINIT;

	return UPlayShortEvent(ev);
}

SynthResult OmniMIDI::TinySFSynth::UPlayShortEvent(unsigned int ev) {
	return Events->Push(ev) ? SYNTH_OK : SYNTH_INVALPARAM;
}

SynthResult OmniMIDI::TinySFSynth::PlayLongEvent(char* ev, unsigned int size) {
	return SYNTH_OK;
}

SynthResult OmniMIDI::TinySFSynth::UPlayLongEvent(char* ev, unsigned int size) {
	return SYNTH_OK;
}