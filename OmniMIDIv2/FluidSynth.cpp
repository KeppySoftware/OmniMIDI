/*

	OmniMIDI v15+ (Rewrite) for Windows NT

	This file contains the required code to run the driver under Windows 7 SP1 and later.
	This file is useful only if you want to compile the driver under Windows, it's not needed for Linux/macOS porting.

*/

// Not supported on ARM Thumb-2!

#include "FluidSynth.h"

#ifndef _M_ARM

void OmniMIDI::FluidSynth::EventsThread() {
	// Spin while waiting for the stream to go online
	while (!fDrv)
		NtDelayExecution(-1);

	while (IsSynthInitialized()) {
		if (!ProcessEvBuf())
			NtDelayExecution(-1);
	}
}

bool OmniMIDI::FluidSynth::ProcessEvBuf() {
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

	if (!Events->Pop(tev) || !fSyn)
		return false;

	if (CHKLRS(GETSTATUS(tev)) != 0) LastRunningStatus = GETSTATUS(tev);
	else tev = tev << 8 | LastRunningStatus;

	unsigned char st = GETSTATUS(tev);
	unsigned char cmd = GETCMD(tev);
	unsigned char ch = GETCHANNEL(tev);
	unsigned char param1 = GETFP(tev);
	unsigned char param2 = GETSP(tev);

	unsigned int len = 3;

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
	case MIDI_PROGCHAN:
		fluid_synth_program_change(fSyn, ch, param1);
		break;
	case MIDI_CHANAFTER:
		fluid_synth_channel_pressure(fSyn, ch, param1);
		break;
	case MIDI_PITCHWHEEL:
		fluid_synth_pitch_bend(fSyn, ch, param1 << 8 | param2);
	default:
		switch (st) {
		case 0xFF:
			for (int i = 0; i < 16; i++)
			{
				fluid_synth_all_notes_off(fSyn, i);
				fluid_synth_all_sounds_off(fSyn, i);
			}
		}
		break;
	}

	return true;
}

bool OmniMIDI::FluidSynth::LoadSynthModule() {
	if (!Settings) {
		Settings = new FluidSettings;

		PrepZwDelayExecution();

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
		fluid_settings_setint(fSet, "synth.polyphony", 1024);

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
		delete_fluid_synth(fSyn);
		fDrv = nullptr;
		fSyn = nullptr;
	}

	return true;
}

SynthResult OmniMIDI::FluidSynth::UPlayShortEvent(unsigned int ev) {
	return Events->Push(ev) ? SYNTH_OK : SYNTH_INVALPARAM;
}

SynthResult OmniMIDI::FluidSynth::PlayShortEvent(unsigned int ev) {
	if (!Events)
		return SYNTH_NOTINIT;

	return UPlayShortEvent(ev);
}

SynthResult OmniMIDI::FluidSynth::UPlayLongEvent(char* ev, unsigned int size) {
	int r = fluid_synth_sysex(fSyn, (const char*)ev, size, nullptr, nullptr, nullptr, 0);
	return (r != -1) ? SYNTH_OK : SYNTH_INVALPARAM;
}

SynthResult OmniMIDI::FluidSynth::PlayLongEvent(char* ev, unsigned int size) {
	if (!FluiLib || !FluiLib->IsOnline())
		return SYNTH_LIBOFFLINE;

	// The size has to be between 1B and 64KB!
	if (size < 1 || size > 65536)
		return SYNTH_INVALPARAM;

	return UPlayLongEvent(ev, size);
}

#endif