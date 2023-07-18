/*

	OmniMIDI v15+ (Rewrite) for Windows NT

	This file contains the required code to run the driver under Windows 7 SP1 and later.
	This file is useful only if you want to compile the driver under Windows, it's not needed for Linux/macOS porting.

*/

// Not supported on ARM Thumb-2!

#include "FluidSynth.h"

void OmniMIDI::FluidSynth::EventsThread() {
	// Spin while waiting for the stream to go online
	while (!fDrv)
		NTFuncs.uSleep(-1);

	fluid_synth_system_reset(fSyn);
	while (IsSynthInitialized()) {
		if (!ProcessEvBuf())
			NTFuncs.uSleep(-1);
	}
}

bool OmniMIDI::FluidSynth::ProcessEvBuf() {
	// SysEx
	int len = 0;
	int handled = 0;
	unsigned int sysev = 0;
	unsigned int tgtev = 0;

	if (!Events->Pop(tgtev) || !fDrv)
		return false;

	if (CHKLRS(GETSTATUS(tgtev)) != 0) LastRunningStatus = GETSTATUS(tgtev);
	else tgtev = tgtev << 8 | LastRunningStatus;

	unsigned char st = GETSTATUS(tgtev);
	unsigned char cmd = GETCMD(tgtev);
	unsigned char ch = GETCHANNEL(tgtev);
	unsigned char param1 = GETFP(tgtev);
	unsigned char param2 = GETSP(tgtev);

	switch (cmd) {
	case MIDI_NOTEON:
		// param1 is the key, param2 is the velocity
		fluid_synth_noteon(fSyn, ch, param1, param2);
		break;

	case MIDI_NOTEOFF:
		// param1 is the key, ignore param2
		fluid_synth_noteoff(fSyn, ch, param1);
		break;

	case MIDI_POLYAFTER:
		fluid_synth_key_pressure(fSyn, ch, param1, param2);
		break;

	case MIDI_CMC:
		fluid_synth_cc(fSyn, ch, param1, param2);
		break;

	case MIDI_PROGCHAN:
		fluid_synth_program_change(fSyn, ch, param1);
		break;

	case MIDI_CHANAFTER:
		fluid_synth_channel_pressure(fSyn, ch, param1);
		break;

	case MIDI_PITCHWHEEL:
		fluid_synth_pitch_bend(fSyn, ch, param2 << 7 | param1);
		break;

	default:
		switch (st) {

		// Let's go!
		case MIDI_SYSEXBEG:
			sysev = tgtev << 8;

			LOG(SynErr, "SysEx Begin: %x", sysev);
			fluid_synth_sysex(fSyn, (const char*)&sysev, 2, 0, &len, &handled, 0);

			while (GETSTATUS(sysev) != MIDI_SYSEXEND) {
				Events->Peek(sysev);

				if (GETSTATUS(sysev) != MIDI_SYSEXEND) {
					Events->Pop(sysev);
					LOG(SynErr, "SysEx Ev: %x", sysev);
					fluid_synth_sysex(fSyn, (const char*)&sysev, 3, 0, &len, &handled, 0);
				}		
			}

			LOG(SynErr, "SysEx End", sysev);		
			break;

		case 0xFF:
			for (int i = 0; i < 16; i++)
			{
				fluid_synth_all_notes_off(fSyn, i);
				fluid_synth_all_sounds_off(fSyn, i);
				fluid_synth_system_reset(fSyn);
			}
		}
		break;
	}

	return true;
}

bool OmniMIDI::FluidSynth::LoadSynthModule() {
	if (!Settings) {
		Settings = new FluidSettings;

		if (!FluiLib)
			FluiLib = new Lib(L"libfluidsynth-3");

		if (!FluiLib->LoadLib())
			return false;

		void* ptr = nullptr;
		for (int i = 0; i < sizeof(FLibImports) / sizeof(FLibImports[0]); i++) {
			ptr = (void*)GetProcAddress(FluiLib->Ptr(), FLibImports[i].GetName());
			if (ptr)
			{
				FLibImports[i].SetPtr(ptr);
				continue;
			}
		}

		fSet = new_fluid_settings();
		if (!fSet)
			NERROR(SynErr, "new_fluid_settings failed to allocate memory for its settings!", true);

		Events = new EvBuf(Settings->EvBufSize);
		_EvtThread = std::jthread(&FluidSynth::EventsThread, this);
	}

	return true;
}

bool OmniMIDI::FluidSynth::UnloadSynthModule() {
	if (!FluiLib || !FluiLib->IsOnline())
		return true;

	if (!fSyn && !fDrv) {
		delete Settings;
		Settings = nullptr;

		delete_fluid_settings(fSet);
		fSet = nullptr;

		delete Events;

		if (!FluiLib->UnloadLib())
		{
			FNERROR(SynErr, "FluiLib->UnloadLib FAILED!!!");
			return false;
		}

		return true;
	}

	NERROR(SynErr, "Call StopSynthModule() first!", true);
	return false;
}

bool OmniMIDI::FluidSynth::StartSynthModule() {
	WinUtils::SysPath Utils;
	wchar_t OMPath[MAX_PATH] = { 0 };

	if (fSet && Settings) {
		if (Settings->ThreadsCount < 1 || Settings->ThreadsCount > std::thread::hardware_concurrency())
			Settings->ThreadsCount = 1;

		fluid_settings_setint(fSet, "synth.cpu-cores", Settings->ThreadsCount);
		fluid_settings_setint(fSet, "audio.period-size", Settings->PeriodSize);
		fluid_settings_setint(fSet, "audio.periods", Settings->Periods);
		fluid_settings_setint(fSet, "synth.device-id", 16);
		fluid_settings_setint(fSet, "synth.min-note-length", Settings->MinimumNoteLength);
		fluid_settings_setint(fSet, "synth.polyphony", Settings->MaxVoices);
		fluid_settings_setint(fSet, "synth.threadsafe-api", Settings->ThreadsCount > 1 ? 0 : 1);
		fluid_settings_setnum(fSet, "synth.sample-rate", Settings->AudioFrequency);
		fluid_settings_setnum(fSet, "synth.overflow.volume", Settings->OverflowVolume);
		fluid_settings_setnum(fSet, "synth.overflow.percussion", Settings->OverflowPercussion);
		fluid_settings_setnum(fSet, "synth.overflow.important", Settings->OverflowImportant);
		fluid_settings_setnum(fSet, "synth.overflow.released", Settings->OverflowReleased);
		fluid_settings_setstr(fSet, "audio.driver", Settings->Driver.c_str());
		fluid_settings_setstr(fSet, "audio.sample-format", Settings->SampleFormat.c_str());
		fluid_settings_setstr(fSet, "synth.midi-bank-select", "xg");

		fSyn = new_fluid_synth(fSet);
		if (!fSyn) {
			NERROR(SynErr, "new_fluid_synth failed!", true);
			goto err;
		}

		fDrv = new_fluid_audio_driver(fSet, fSyn);
		if (!fSyn) {
			NERROR(SynErr, "new_fluid_audio_driver failed!", true);
			goto err;
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
											int sf = fluid_synth_sfload(fSyn, sfpath.c_str(), 1);

											// Check if the soundfont loads, if it does then it's valid
											if (sf)
												SoundFonts.push_back(sf);
											else NERROR(SynErr, "fluid_synth_sfload failed to load \"%s\"!", false, sfpath.c_str());
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

		return true;
	}

err:
	OmniMIDI::FluidSynth::StopSynthModule();
	return false;
}

bool OmniMIDI::FluidSynth::StopSynthModule() {
	if (fSyn && fDrv) {
		delete_fluid_audio_driver(fDrv);
		fDrv = nullptr;
		delete_fluid_synth(fSyn);
		fSyn = nullptr;
	}

	return true;
}

SynthResult OmniMIDI::FluidSynth::UPlayShortEvent(unsigned int ev) {
	return Events->Push(ev) ? SYNTH_OK : SYNTH_INVALPARAM;
}

SynthResult OmniMIDI::FluidSynth::PlayShortEvent(unsigned int ev) {
	if (!Events || !IsSynthInitialized())
		return SYNTH_NOTINIT;

	return UPlayShortEvent(ev);
}

SynthResult OmniMIDI::FluidSynth::UPlayLongEvent(char* ev, unsigned int size) {
	int len = 0;
	int handled = 0;

	int r = fluid_synth_sysex(fSyn, ev, size, 0, &len, &handled, 0);

	return (r != -1 && handled) ? SYNTH_OK : SYNTH_INVALPARAM;
}

SynthResult OmniMIDI::FluidSynth::PlayLongEvent(char* ev, unsigned int size) {
	if (!FluiLib || !FluiLib->IsOnline())
		return SYNTH_LIBOFFLINE;

	if (!IsSynthInitialized())
		return SYNTH_NOTINIT;

	// The size has to be between 1B and 64KB!
	if (size < 1 || size > 65536)
		return SYNTH_INVALPARAM;

	return UPlayLongEvent(ev, size);
}