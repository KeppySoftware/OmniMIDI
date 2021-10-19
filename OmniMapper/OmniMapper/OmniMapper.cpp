// OmniMapper: MIDI mapper for OmniMIDI

#include <Windows.h>
#include <Shlwapi.h>
#include <shlobj.h>
#include <stdio.h>
#include <locale.h>
#include <fstream>
#include <codecvt>
#include "mmddk.h"
#include "OmniMIDI.h"

static const CHAR SynthNameA[MAXPNAMELEN] = "OmniMapper\0";
static DWORD SNAType = REG_SZ, SNASize = sizeof(SynthNameA);

static const WCHAR SynthNameW[MAXPNAMELEN] = L"OmniMapper\0";
static DWORD SNWType = REG_SZ, SNWSize = sizeof(SynthNameW);

static const WCHAR IAmDummy[MAXPNAMELEN] = L"I'm OmniMapper, don't read me!\0";

static WCHAR AppPath[32767];
static WCHAR AppName[MAX_PATH];
static HANDLE Target = NULL;
static HDRVR MapperDevice = NULL;
static DWORD SelectedDevice = 2;
static BOOL StreamMode = FALSE;

static BOOL OMAlreadyInit = FALSE;
static BOOL KDMAPIMode = FALSE;

static DWORD_PTR OMUser;// Dummy pointer, used for KDMAPI Output

static const GUID OMCLSID = { 0x62F3192B, 0xA961, 0x456D, { 0xAB, 0xCA, 0xA5, 0xC9, 0x5A, 0x14, 0xB9, 0xAA } };

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

	WCHAR TrgtSynthW[MAXPNAMELEN] = L"KDMAPI Output\0";

	// First check if registry key exists
	HKEY MapperKey;
	LSTATUS s = RegOpenKeyExA(HKEY_CURRENT_USER, "Software\\OmniMIDI\\Mapper", 0, KEY_ALL_ACCESS, &MapperKey);
	if (s == ERROR_NO_MATCH || s == ERROR_FILE_NOT_FOUND) {
		// If it doesn't, create it
		s = RegCreateKeyExA(HKEY_CURRENT_USER, "Software\\OmniMIDI\\Mapper", 0L, NULL, REG_OPTION_NON_VOLATILE, KEY_ALL_ACCESS, NULL, &MapperKey, NULL);
	}

	// Operation succesfull, get data from registry
	if (!s) RegQueryValueExW(MapperKey, L"TrgtSynth", NULL, &SNWType, (LPBYTE)&TrgtSynthW, &SNWSize);
	RegCloseKey(MapperKey);

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
	

	if (wcsicmp(TrgtSynthW, L"KDMAPI Output\0"))
	{
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
			return MMSYSERR_ERROR;
		}
	}
	else {
		DeviceCapsW[0].dwSupport = MIDICAPS_CACHE;
		DeviceCapsW[0].wChannelMask = 0xFFFF;
		DeviceCapsW[0].wMid = 0xFFFF;
		DeviceCapsW[0].wPid = 0x000A;
		DeviceCapsW[0].wTechnology = MOD_MIDIPORT;
		DeviceCapsW[0].wNotes = 0;
		DeviceCapsW[0].wVoices = 0;
		DeviceCapsW[0].vDriverVersion = MAKEWORD(6, 0);
		SelectedDevice = 0;
		KDMAPIMode = TRUE;
	}


	switch (capsSize) {
	case (sizeof(MIDIOUTCAPSA)):
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

	case (sizeof(MIDIOUTCAPS2A)):
	{
		MIDIOUTCAPS2A MIDICapsA;
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
	
	case (sizeof(MIDIOUTCAPS2W)):
	{
		MIDIOUTCAPS2W MIDICapsW;
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

MMRESULT mM(UINT uDeviceID, UINT uMsg, DWORD_PTR dwUser, DWORD_PTR dwParam1, DWORD_PTR dwParam2) {
	MMRESULT ReturnVal = MMSYSERR_NOERROR;

	switch (uMsg) {
	case DRVM_INIT:
	case DRVM_EXIT:
	case DRVM_ENABLE:
	case DRVM_DISABLE:
		// Stub, pretend it's supported
		return MMSYSERR_NOERROR;
	case MODM_DATA:
		KDMAPIMode ? SendDirectData(dwParam1) : ReturnVal = midiOutShortMsg((HMIDIOUT)Target, dwParam1);
		return ReturnVal;
	case MODM_LONGDATA:
		if (KDMAPIMode) {
			ReturnVal = SendDirectLongData((LPMIDIHDR)dwParam1, dwParam2);
			RunCallbackFunction(MM_MOM_DONE, dwParam1, dwParam2);
			return ReturnVal;
		}
		
		return midiOutLongMsg((HMIDIOUT)Target, (LPMIDIHDR)dwParam1, dwParam2);
	case MODM_STRMDATA:
		return KDMAPIMode ? modMessage(0, MODM_STRMDATA, OMUser, dwParam1, dwParam2) : midiStreamOut((HMIDISTRM)Target, (LPMIDIHDR)dwParam1, dwParam2);
	case MODM_PREPARE:
		return KDMAPIMode ? PrepareLongData((LPMIDIHDR)dwParam1, dwParam2) : midiOutPrepareHeader((HMIDIOUT)Target, (LPMIDIHDR)dwParam1, dwParam2);
	case MODM_UNPREPARE:
		return KDMAPIMode ? UnprepareLongData((LPMIDIHDR)dwParam1, dwParam2) : midiOutUnprepareHeader((HMIDIOUT)Target, (LPMIDIHDR)dwParam1, dwParam2);
	case MODM_OPEN:
		if (!KDMAPIMode) {
			StreamMode = (DWORD)dwParam2 & 0x00000002L;

			if (StreamMode)
				return midiStreamOpen((LPHMIDISTRM)&Target, (LPUINT)SelectedDevice, 1, ((MIDIOPENDESC*)dwParam1)->dwCallback, ((MIDIOPENDESC*)dwParam1)->dwInstance, dwParam2);
			else
				return midiOutOpen((LPHMIDIOUT)&Target, SelectedDevice, ((MIDIOPENDESC*)dwParam1)->dwCallback, ((MIDIOPENDESC*)dwParam1)->dwInstance, dwParam2);
		}
		else {
			if (!OMAlreadyInit) {
				// Setup the Callback
				if (!InitializeCallbackFeatures((HMIDI)Target, ((MIDIOPENDESC*)dwParam1)->dwCallback, ((MIDIOPENDESC*)dwParam1)->dwInstance, OMUser, dwParam2))
				{
					MessageBox(NULL, "ICF failed!", "KDMAPI ERROR", MB_SYSTEMMODAL | MB_ICONERROR);
					return MMSYSERR_INVALPARAM;
				}

				// Close any stream, just to be safe
				TerminateKDMAPIStream();

				// Initialize MIDI out
				if (!InitializeKDMAPIStream())
					return MMSYSERR_ALLOCATED;

				DriverSettings(0xFFFFF, NULL, NULL, NULL);

				RunCallbackFunction(MM_MOM_OPEN, 0, 0);

				return MMSYSERR_NOERROR;
			}
			else {
				MessageBox(NULL, "OmniMIDI has been already initialized via KDMAPI! Can not initialize it again!", "KDMAPI ERROR", MB_SYSTEMMODAL | MB_ICONERROR);
				return MMSYSERR_ALLOCATED;
			}
		}
	case MODM_CLOSE:
		if (!KDMAPIMode) {
			if (StreamMode) return midiStreamClose((HMIDISTRM)Target);
			else return midiOutClose((HMIDIOUT)Target);
			StreamMode = 0;
		}
		else {
			// Close OM
			if (OMAlreadyInit) {
				if (!TerminateKDMAPIStream()) 
					return MMSYSERR_NOMEM;

				DriverSettings(0xFFFFE, NULL, NULL, NULL);
				RunCallbackFunction(MM_MOM_CLOSE, 0, 0);
				OMAlreadyInit = FALSE;
			}

			return MMSYSERR_NOERROR;
		}
	case MODM_PROPERTIES:
		return KDMAPIMode ? modMessage(0, uMsg, OMUser, dwParam1, dwParam2) : midiStreamProperty((HMIDISTRM)Target, (LPBYTE)&dwParam1, dwParam2);
	case MODM_GETPOS:
		return KDMAPIMode ? modMessage(0, uMsg, OMUser, dwParam1, dwParam2) : midiStreamPosition((HMIDISTRM)Target, (LPMMTIME)&dwParam1, (UINT)dwParam2);
	case MODM_RESET:
		KDMAPIMode ? ResetKDMAPIStream() : ReturnVal = midiOutReset((HMIDIOUT)Target);
		return ReturnVal;
	case MODM_RESTART:
		return KDMAPIMode ? modMessage(0, uMsg, OMUser, dwParam1, dwParam2) : midiStreamRestart((HMIDISTRM)Target);
	case MODM_STOP:
		return KDMAPIMode ? modMessage(0, uMsg, OMUser, dwParam1, dwParam2) : midiStreamStop((HMIDISTRM)Target);
	case MODM_PAUSE:
		return KDMAPIMode ? modMessage(0, uMsg, OMUser, dwParam1, dwParam2) : midiStreamPause((HMIDISTRM)Target);
	case MODM_SETVOLUME:
		return KDMAPIMode ? modMessage(0, uMsg, OMUser, dwParam1, dwParam2) : midiOutSetVolume((HMIDIOUT)Target, dwParam1);
	case MODM_GETVOLUME:
		return KDMAPIMode ? modMessage(0, uMsg, OMUser, dwParam1, dwParam2) : midiOutGetVolume((HMIDIOUT)Target, (LPDWORD)&dwParam1);
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