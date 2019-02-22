// OmniMapper: MIDI mapper for OmniMIDI

#include <Windows.h>
#include <Shlwapi.h>
#include <shlobj_core.h>
#include <stdio.h>
#include <locale.h>
#include <fstream>
#include <codecvt>
#include "mmddk.h"

static const CHAR SynthNameA[MAXPNAMELEN] = "OmniMapper\0";
static DWORD SNAType = REG_SZ, SNASize = sizeof(SynthNameA);

static const WCHAR SynthNameW[MAXPNAMELEN] = L"OmniMapper\0";
static DWORD SNWType = REG_SZ, SNWSize = sizeof(SynthNameW);

static const WCHAR IAmDummy[MAXPNAMELEN] = L"I'm OmniMapper, don't read me!\0";

static WCHAR AppPath[32767];
static WCHAR AppName[MAX_PATH];
static HMIDI Target = NULL;
static HDRVR MapperDevice = NULL;
static DWORD SelectedDevice = 2;
static BOOL StreamMode = FALSE;

// Blacklist system from OM
#include "BlackListSystem.h"

void ShowFatalError(LPCSTR Error) {
	MessageBox(NULL, Error, "OmniMapper - FATAL ERROR", MB_ICONHAND | MB_SYSTEMMODAL);
	exit(-1);
}

BOOL DllMain(HINSTANCE hInstDLL, DWORD fdwReason, LPVOID fImpLoad) {
	if (fdwReason == DLL_PROCESS_ATTACH) {
		if (BannedProcesses() || BlackListSystem()) return FALSE;
	}
	return TRUE;
}

STDAPI_(LONG_PTR) DriverProc(DWORD_PTR dwDriverId, HDRVR hdrvr, UINT uMsg, LPARAM lParam1, LPARAM lParam2)
{
	switch (uMsg) {
	case DRV_CONFIGURE:
		return DRVCNF_OK;
	case DRV_OPEN:
		MapperDevice = hdrvr;
		return DRV_OK;
	case DRV_CLOSE:
		MapperDevice = NULL;
		return DRV_OK;
	case DRV_QUERYCONFIGURE:
	case DRV_LOAD:
	case DRV_ENABLE:
	case DRV_REMOVE:
	case DRV_FREE:
		return DRV_OK;
	default:
		return DefDriverProc(dwDriverId, hdrvr, uMsg, lParam1, lParam2);
	}
}

DWORD GiveMapperCaps(MIDIOUTCAPS* capsPtr, DWORD capsSize) {
	if (capsPtr->szPname) {
		// Avoid getting stuck in a circular reference
		if (!lstrcmpW(((MIDIOUTCAPSW*)capsPtr)->szPname, IAmDummy))
			return MMSYSERR_NOTENABLED;
	}

	WCHAR TrgtSynthW[MAXPNAMELEN] = L"Microsoft GS Wavetable Synth\0";

	// Get number of devices!
	DWORD DevCount = midiOutGetNumDevs();
	MIDIOUTCAPSW* TempCapsW = (MIDIOUTCAPSW*)calloc(DevCount, sizeof(MIDIOUTCAPSW));
	for (DWORD i = 0; i < DevCount; i++) {
		wcsncpy(TempCapsW[i].szPname, IAmDummy, MAXPNAMELEN);

		if (!midiOutGetDevCapsW(i, &TempCapsW[i], sizeof(MIDIOUTCAPSW)))
			memcpy(&TempCapsW[i], &TempCapsW[i], sizeof(MIDIOUTCAPSW));
	}

	// Get real devices!
	DWORD ValidDevices = 0;
	MIDIOUTCAPSW* DeviceCapsW = (MIDIOUTCAPSW*)calloc(DevCount, sizeof(MIDIOUTCAPSW));
	for (DWORD i = 0; i < DevCount; i++) {
		if (TempCapsW[i].szPname == NULL || !lstrcmpW(((MIDIOUTCAPSW*)capsPtr)->szPname, IAmDummy)) continue;

		memcpy(&DeviceCapsW[ValidDevices], &TempCapsW[i], sizeof(MIDIOUTCAPSW));
		ValidDevices++;
	}

	free(TempCapsW);

	// First check if registry key exists
	HKEY MapperKey;
	LSTATUS s = RegOpenKeyEx(HKEY_CURRENT_USER, "Software\\OmniMIDI\\Mapper", 0, KEY_ALL_ACCESS, &MapperKey);
	if (s == ERROR_NO_MATCH || s == ERROR_FILE_NOT_FOUND) {
		// If it doesn't, create it
		s = RegCreateKeyEx(HKEY_CURRENT_USER, "Software\\OmniMIDI\\Mapper", 0L, NULL, REG_OPTION_NON_VOLATILE, KEY_ALL_ACCESS, NULL, &MapperKey, NULL);
	}
	
	// Operation succesfull, get data from registry
	if (!s) RegQueryValueExW(MapperKey, L"TrgtSynth", NULL, &SNWType, (LPBYTE)&TrgtSynthW, &SNWSize);
	RegCloseKey(MapperKey);

	BOOL Found = FALSE;
	for (DWORD i = 0; i < DevCount; i++) {
		if (!lstrcmpW(DeviceCapsW[i].szPname, TrgtSynthW))
		{
			SelectedDevice = i;
			Found = TRUE;
			break;
		}
	}

	if (!Found) {
		MessageBox(NULL, "Can not find the device set in the OmniMIDI control panel!\nPlease set up the MIDI mapper in the OmniMIDI configurator.\n\nPress OK to continue.", "OmniMapper - ERROR", MB_ICONERROR | MB_SYSTEMMODAL);
		wcsncpy(TrgtSynthW, L"Microsoft GS Wavetable Synth\0", MAXPNAMELEN);
	}

	switch (capsSize) {
	case (sizeof(MIDIOUTCAPSA)):
	case (sizeof(MIDIOUTCAPS2A)):
	{
		MIDIOUTCAPSA MIDICapsA;
		strncpy(MIDICapsA.szPname, SynthNameA, MAXPNAMELEN);
		MIDICapsA.dwSupport = DeviceCapsW[SelectedDevice].dwSupport;
		MIDICapsA.wChannelMask = DeviceCapsW[SelectedDevice].wChannelMask;
		MIDICapsA.wMid = DeviceCapsW[SelectedDevice].wMid;
		MIDICapsA.wPid = DeviceCapsW[SelectedDevice].wPid;
		MIDICapsA.wTechnology = MOD_MAPPER;
		MIDICapsA.wVoices = DeviceCapsW[SelectedDevice].wVoices;
		MIDICapsA.vDriverVersion = DeviceCapsW[SelectedDevice].vDriverVersion;
		memcpy(capsPtr, &MIDICapsA, min(sizeof(MIDICapsA), capsSize));
		break;
	}

	case (sizeof(MIDIOUTCAPSW)):
	case (sizeof(MIDIOUTCAPS2W)):
	{
		MIDIOUTCAPSW MIDICapsW;
		wcsncpy(MIDICapsW.szPname, SynthNameW, MAXPNAMELEN);
		MIDICapsW.dwSupport = DeviceCapsW[SelectedDevice].dwSupport;
		MIDICapsW.wChannelMask = DeviceCapsW[SelectedDevice].wChannelMask;
		MIDICapsW.wMid = DeviceCapsW[SelectedDevice].wMid;
		MIDICapsW.wPid = DeviceCapsW[SelectedDevice].wPid;
		MIDICapsW.wTechnology = MOD_MAPPER;
		MIDICapsW.wVoices = DeviceCapsW[SelectedDevice].wVoices;
		MIDICapsW.vDriverVersion = DeviceCapsW[SelectedDevice].vDriverVersion;
		memcpy(capsPtr, &MIDICapsW, min(sizeof(MIDICapsW), capsSize));
		break;
	}
	}

	free(DeviceCapsW);

	return MMSYSERR_NOERROR;
}

STDAPI_(DWORD) modMessage(UINT uDeviceID, UINT uMsg, DWORD_PTR dwUser, DWORD_PTR dwParam1, DWORD_PTR dwParam2) {
	switch (uMsg) {
	case MODM_DATA:
		return midiOutShortMsg((HMIDIOUT)Target, dwParam1);
	case MODM_LONGDATA:
		return midiOutLongMsg((HMIDIOUT)Target, (MIDIHDR*)dwParam1, dwParam2);
	case MODM_STRMDATA:
		return midiStreamOut((HMIDISTRM)Target, (MIDIHDR*)dwParam1, dwParam2);
	case MODM_PREPARE:
		return midiOutPrepareHeader((HMIDIOUT)Target, (MIDIHDR*)dwParam1, dwParam2);
	case MODM_UNPREPARE:
		return midiOutUnprepareHeader((HMIDIOUT)Target, (MIDIHDR*)dwParam1, dwParam2);
	case MODM_OPEN:
		StreamMode = (DWORD)dwParam2 & 0x00000002L;
		if (StreamMode) return midiStreamOpen((LPHMIDISTRM)&Target, (LPUINT)SelectedDevice, 1, ((MIDIOPENDESC*)dwParam1)->dwCallback, ((MIDIOPENDESC*)dwParam1)->dwInstance, dwParam2);
		else return midiOutOpen((LPHMIDIOUT)&Target, SelectedDevice, ((MIDIOPENDESC*)dwParam1)->dwCallback, ((MIDIOPENDESC*)dwParam1)->dwInstance, dwParam2);
	case MODM_CLOSE:
		if (StreamMode) return midiStreamClose((HMIDISTRM)Target);
		else return midiOutClose((HMIDIOUT)Target);
	case MODM_PROPERTIES:
		return midiStreamProperty((HMIDISTRM)Target, (LPBYTE)&dwParam1, dwParam2);
	case MODM_GETPOS:
		return midiStreamPosition((HMIDISTRM)Target, (LPMMTIME)&dwParam1, (UINT)dwParam2);
	case MODM_RESET:
		return midiOutReset((HMIDIOUT)Target);
	case MODM_RESTART:
		return midiStreamRestart((HMIDISTRM)Target);
	case MODM_STOP:
		return midiStreamStop((HMIDISTRM)Target);
	case MODM_PAUSE:
		return midiStreamPause((HMIDISTRM)Target);
	case MODM_SETVOLUME:
		return midiOutSetVolume((HMIDIOUT)Target, dwParam1);
	case MODM_GETVOLUME:
		return midiOutGetVolume((HMIDIOUT)Target, (LPDWORD)&dwParam1);
	case MODM_GETDEVCAPS:
		return GiveMapperCaps((MIDIOUTCAPS*)dwParam1, (DWORD)dwParam2);
	case MODM_GETNUMDEVS:
		return TRUE;
	case MODM_CACHEDRUMPATCHES:
	{
		WORD pwkya = HIWORD(dwParam1);
		return midiOutCacheDrumPatches((HMIDIOUT)Target, LOWORD(dwParam1), &pwkya, dwParam2);
	}
	case MODM_CACHEPATCHES:
	{
		WORD pwpa = HIWORD(dwParam1);
		return midiOutCachePatches((HMIDIOUT)Target, LOWORD(dwParam1), &pwpa, dwParam2);
	}
	}
}