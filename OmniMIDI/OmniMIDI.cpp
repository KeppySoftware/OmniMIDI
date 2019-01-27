/*
OmniMIDI, a fork of BASSMIDI Driver

Thank you Kode54 for allowing me to fork your awesome driver.
*/

#pragma once

typedef unsigned __int64 QWORD;
typedef long NTSTATUS;

// KDMAPI version
#define CUR_MAJOR	1
#define CUR_MINOR	50
#define CUR_BUILD	1
#define CUR_REV		17

#define STRICT
#define VC_EXTRALEAN
#define WIN32_LEAN_AND_MEAN
#define __STDC_LIMIT_MACROS
#define _CRT_SECURE_CPP_OVERLOAD_STANDARD_NAMES 1

#define BASSASIODEF(f) (WINAPI *f)
#define BASSDEF(f) (WINAPI *f)
#define BASSENCDEF(f) (WINAPI *f)	
#define BASSMIDIDEF(f) (WINAPI *f)	
#define BASS_VSTDEF(f) (WINAPI *f)
// #define BASSWASAPIDEF(f) (WINAPI *f)
#define Between(value, a, b) (value <= b && value >= a)

#define ERRORCODE		0
#define CAUSE			1
#define LONGMSG_MAXSIZE	65536

#define LOCK_VM_IN_WORKING_SET 1
#define LOCK_VM_IN_RAM 2

#include "stdafx.h"
#include <Psapi.h>
#include <atlbase.h>
#include <cstdint>
#include <comdef.h>
#include <fstream>
#include <iostream>
#include <codecvt>
#include <string>
#include <future>
#include <mmddk.h>
#include <process.h>
#include <shlobj.h>
#include <sstream>
#include <stdio.h>
#include <stdlib.h>
#include <time.h>
#include <vector>
#include <algorithm>
#include <windows.h>
#include <Dbghelp.h>
#include <mmsystem.h>
#include "Resource.h"
#include "OmniMIDI.h"

// BASS headers
#include <bass.h>
#include <bassmidi.h>
#include <bassenc.h>
#include <bassasio.h>
#include <bass_vst.h>

// NTSTATUS
#define NTAPI __stdcall
// these functions have identical prototypes
typedef NTSTATUS(NTAPI* NLVM)(IN HANDLE, IN OUT VOID**, IN OUT ULONG*, IN ULONG);
typedef NTSTATUS(NTAPI* NULVM)(IN HANDLE, IN OUT VOID**, IN OUT ULONG*, IN ULONG);
typedef NTSTATUS(NTAPI* NDE)(BOOLEAN, INT64*);
typedef NTSTATUS(NTAPI* NQST)(QWORD*);

static NLVM NtLockVirtualMemory = 0;
static NULVM NtUnlockVirtualMemory = 0;
static NDE NtDelayExecution = 0;
static NQST NtQuerySystemTime = 0;

// Blinx best game
static HMODULE bass = NULL, bassasio = NULL, bassenc = NULL, bassmidi = NULL, bass_vst = NULL;	// BASS libs handles

#define LOADLIBFUNCTION(l, f) *((void**)&f)=GetProcAddress(l,#f)
// #define LOADBASSWASAPIFUNCTION(f) *((void**)&f) = GetProcAddress(basswasapi, #f)

// F**k Sleep() tbh
void NTSleep(__int64 usec) {
	NtDelayExecution(FALSE, &usec);
}

DWORD DummyPlayBufData() { return 0; }
VOID DummySendToBASSMIDI(DWORD LastRunningStatus, DWORD dwParam1) { return; }
MMRESULT DummyParseData(UINT dwMsg, DWORD_PTR dwParam1, DWORD_PTR dwParam2) { return MIDIERR_NOTREADY; }

// Hyper switch
static BOOL HyperMode = 0;
static MMRESULT(*_PrsData)(UINT uMsg, DWORD_PTR dwParam1, DWORD_PTR dwParam2) = DummyParseData;
static VOID(*_StoBASSMIDI)(DWORD LastRunningStatus, DWORD dwParam1) = DummySendToBASSMIDI;
static DWORD(*_PlayBufData)(void) = DummyPlayBufData;
static DWORD(*_PlayBufDataChk)(void) = DummyPlayBufData;
// What does it do? It gets rid of the useless functions,
// and passes the events without checking for anything

// Predefined sleep values, useful for redundancy
#define _DBGWAIT NTSleep(-16667)											// Normal wait
#define _FWAIT NTSleep(ManagedSettings.SleepStates ? -100 : 0)				// Fast wait
#define _LWAIT NTSleep(ManagedSettings.SleepStates ? -1000 : 0)				// Slow wait
#define _VLWAIT NTSleep(-200000)											// Very slow wait
#define _CFRWAIT NTSleep(ManagedSettings.SleepStates ? -16667 : -16567)		// Cap framerate wait

// LightweightLock by Brad Wilson
#include "LwL.h"

// Variables
#include "Values.h"
#include "Debug.h"
#include "BASSErrors.h"

// OmniMIDI vital parts
#include "SoundFontLoader.h"
#include "BufferSystem.h"
#include "Settings.h"
#include "BlacklistSystem.h"
#include "DriverInit.h"
#include "CookedPlayer.h"
#include "KDMAPI.h"

// OmniMIDI GUID
// {62F3192B-A961-456D-ABCA-A5C95A14B9AA}
static const GUID OMCLSID = { 0x62F3192B, 0xA961, 0x456D, { 0xAB, 0xCA, 0xA5, 0xC9, 0x5A, 0x14, 0xB9, 0xAA } };

extern "C" BOOL APIENTRY DllMain(HANDLE hinstDLL, DWORD fdwReason, LPVOID lpvReserved)
{
	switch (fdwReason) {
	case DLL_PROCESS_ATTACH:
		if (BannedProcesses()) {
			OutputDebugString(L"You can't load me!");
			return FALSE;
		}

		hinst = (HINSTANCE)hinstDLL;
		NtDelayExecution = (NDE)GetProcAddress(GetModuleHandle(L"ntdll"), "NtDelayExecution");
		NtLockVirtualMemory = (NLVM)GetProcAddress(GetModuleHandle(L"ntdll"), "NtLockVirtualMemory");
		NtUnlockVirtualMemory = (NULVM)GetProcAddress(GetModuleHandle(L"ntdll"), "NtUnlockVirtualMemory");
		NtQuerySystemTime = (NQST)GetProcAddress(GetModuleHandle(L"ntdll"), "NtQuerySystemTime");
		if (!NtDelayExecution || !NtLockVirtualMemory || !NtUnlockVirtualMemory || !NtQuerySystemTime) {
			OutputDebugString(L"Failed to parse functions from NTDLL!\nThe driver will not be loaded.");
			return FALSE;
		}

		DisableThreadLibraryCalls((HMODULE)hinstDLL);
		break;
	case DLL_PROCESS_DETACH:
	case DLL_THREAD_ATTACH:
	case DLL_THREAD_DETACH: 
		break;
	}

	return TRUE;
}

STDAPI_(LONG_PTR) DriverProc(DWORD_PTR dwDriverId, HDRVR hdrvr, UINT uMsg, LPARAM lParam1, LPARAM lParam2)
{
	switch (uMsg) {
	case DRV_CONFIGURE:
		TCHAR configuratorapp[MAX_PATH];
		if (SUCCEEDED(SHGetFolderPath(NULL, CSIDL_SYSTEMX86, NULL, 0, configuratorapp)))
		{
			PathAppend(configuratorapp, _T("\\OmniMIDI\\OmniMIDIConfigurator.exe"));
			ShellExecute(NULL, L"open", configuratorapp, L"/AST", NULL, SW_SHOWNORMAL);
			return DRVCNF_OK;
		}
		return DRVCNF_CANCEL;
	case DRV_OPEN:
		OMDevice = hdrvr;
		return DRV_OK;
	case DRV_CLOSE:
		OMDevice = NULL;
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

DWORD GiveOmniMIDICaps(PVOID capsPtr, DWORD capsSize) {
	try {
		// Create temp caps
		MIDIOUTCAPS2 MIDICaps;
		
		// Initialize values
		DWORD Technology = NULL;
		WORD MID = 0x0000;
		WORD PID = 0x0000;

		OpenRegistryKey(Configuration, L"Software\\OmniMIDI\\Configuration", FALSE);
		RegQueryValueEx(Configuration.Address, L"DisableChime", NULL, &dwType, (LPBYTE)&DisableChime, &dwSize);
		RegQueryValueEx(Configuration.Address, L"SynthType", NULL, &dwType, (LPBYTE)&SynthType, &dwSize);
		RegQueryValueEx(Configuration.Address, L"DebugMode", NULL, &dwType, (LPBYTE)&ManagedSettings.DebugMode, &dwSize);
		RegQueryValueEx(Configuration.Address, L"VID", NULL, &dwType, (LPBYTE)&MID, &dwSize);
		RegQueryValueEx(Configuration.Address, L"PID", NULL, &dwType, (LPBYTE)&PID, &dwSize);
		RegQueryValueEx(Configuration.Address, L"SynthName", NULL, &SNType, (LPBYTE)&SynthNameW, &SNSize);
		RegQueryValueEx(Configuration.Address, L"DisableCookedPlayer", NULL, &dwType, (LPBYTE)&ManagedSettings.DisableCookedPlayer, &dwSize);

		// If the synth type ID is bigger than the size of the synth types array,
		// set it automatically to MOD_MIDIPORT
		if (SynthType >= ((sizeof(SynthNamesTypes) / sizeof(SynthNamesTypes[0]))))
			Technology = MOD_MIDIPORT;
		// Else, load the requested value
		else Technology = SynthNamesTypes[SynthType];

		// If the debug mode is enabled, and the process isn't banned, create the debug log
		if (ManagedSettings.DebugMode && BlackListSystem())
			CreateConsole();

		// If the synthname length is less than 1, or if it's just a space, use the default name
		if (wcslen(SynthNameW) < 1 || (wcslen(SynthNameW) == 1 && iswspace(SynthNameW[0]))) {
			RtlSecureZeroMemory(SynthNameW, sizeof(SynthNameW));
			wcsncpy(SynthNameW, L"OmniMIDI\0", MAXPNAMELEN);
		}

		PrintMessageToDebugLog("MODM_GETDEVCAPS", "Sharing MIDI device caps with application...");

		// Not yet
		// CapsSupport |= MIDICAPS_STREAM;

		// Prepare the caps item
		wcsncpy(MIDICaps.szPname, SynthNameW, MAXPNAMELEN);
		MIDICaps.ManufacturerGuid = OMCLSID;
		MIDICaps.NameGuid = OMCLSID;
		MIDICaps.ProductGuid = OMCLSID;
		MIDICaps.dwSupport = (ManagedSettings.DisableCookedPlayer ? 0 : MIDICAPS_STREAM) | MIDICAPS_VOLUME;
		MIDICaps.wChannelMask = 0xFFFF;
		MIDICaps.wMid = MID;
		MIDICaps.wPid = PID;
		MIDICaps.wTechnology = Technology;
		MIDICaps.wVoices = 65535;
		MIDICaps.vDriverVersion = MAKEWORD(6, 0);
		PrintMessageToDebugLog("MODM_GETDEVCAPS", "Done sharing MIDI device caps.");

		// Copy the item to the app's caps
		memcpy(capsPtr, &MIDICaps, min(sizeof(MIDICaps), capsSize));
		return MMSYSERR_NOERROR;
	}
	catch (...) {
		CrashMessage("MIDICapsException");
	}
}

STDAPI_(DWORD) modMessage(UINT uDeviceID, UINT uMsg, DWORD_PTR dwUser, DWORD_PTR dwParam1, DWORD_PTR dwParam2) {
	MMRESULT RetVal = MMSYSERR_NOERROR;
	
	/*
	char Msg[NTFS_MAX_PATH] = { 0 };
	sprintf(Msg, "Received modMessage(%u, %u, %X, %X, %X)", uDeviceID, uMsg, dwUser, dwParam1, dwParam2);
	PrintMessageToDebugLog("MOD_MESSAGE", Msg);
	*/

	switch (uMsg) {
	case MODM_DATA:
		// Parse the data lol
		return _PrsData(uMsg, dwParam1, dwParam2);
	case MODM_LONGDATA: {
		// Pass it to a KDMAPI function
		RetVal = SendDirectLongData((MIDIHDR*)dwParam1);

		// Tell the app that the buffer has been played
		DriverCallback(OMCallback, OMFlags, OMDevice, MOM_DONE, OMInstance, dwParam1, 0);
		return RetVal;
	}
	case MODM_STRMDATA: {
		if (!bass_initialized || !dwUser) {
			PrintMessageToDebugLog("MODM_STRMDATA", "You can't call midiStreamData with a normal MIDI stream, or the driver isn't ready.");
			return DebugResult(MMSYSERR_INVALPARAM, TRUE);
		}

		if ((DWORD)dwParam2 < offsetof(MIDIHDR, dwOffset) ||
			!((MIDIHDR*)dwParam1) || !((MIDIHDR*)dwParam1)->lpData ||
			((MIDIHDR*)dwParam1)->dwBufferLength < ((MIDIHDR*)dwParam1)->dwBytesRecorded ||
			((MIDIHDR*)dwParam1)->dwBytesRecorded % 4)
		{
			PrintMessageToDebugLog("MODM_STRMDATA", "You can't call midiStreamOut with a normal MIDI stream, or the driver isn't ready.");
			return DebugResult(MMSYSERR_INVALPARAM, TRUE);
		}

		if (!(((MIDIHDR*)dwParam1)->dwFlags & MHDR_PREPARED)) {
			PrintMessageToDebugLog("MODM_STRMDATA", "The buffer is not prepared.");
			return DebugResult(MIDIERR_UNPREPARED, TRUE);
		}

		if (!(((MIDIHDR*)dwParam1)->dwFlags & MHDR_DONE)) {
			if (((MIDIHDR*)dwParam1)->dwFlags & MHDR_INQUEUE) {
				PrintMessageToDebugLog("MODM_STRMDATA", "The buffer is still being played.");
				return DebugResult(MIDIERR_STILLPLAYING, TRUE);
			}
		}

		PrintMessageToDebugLog("MODM_STRMDATA", "Locking for writing...");
		((CookedPlayer*)dwUser)->Lock.LockForWriting();

		PrintMessageToDebugLog("MODM_STRMDATA", "Copying pointer of buffer...");

		PrintMIDIHDRToDebugLog("MODM_STRMDATA", (MIDIHDR*)dwParam1);

		((MIDIHDR*)dwParam1)->dwFlags &= ~MHDR_DONE;
		((MIDIHDR*)dwParam1)->dwFlags |= MHDR_INQUEUE;
		((MIDIHDR*)dwParam1)->lpNext = 0;
		((MIDIHDR*)dwParam1)->dwOffset = 0;
		if (((CookedPlayer*)dwUser)->MIDIHeaderQueue)
		{
			PrintMessageToDebugLog("MODM_STRMDATA", "Another buffer is already present. Adding it to queue...");
			LPMIDIHDR phdr = ((CookedPlayer*)dwUser)->MIDIHeaderQueue;
			while (phdr->lpNext)
				phdr = phdr->lpNext;
			phdr->lpNext = (MIDIHDR*)dwParam1;
		}
		else ((CookedPlayer*)dwUser)->MIDIHeaderQueue = (MIDIHDR*)dwParam1;
		PrintMessageToDebugLog("MODM_STRMDATA", "Copied.");

		PrintMessageToDebugLog("MODM_STRMDATA", "Unlocking...");
		((CookedPlayer*)dwUser)->Lock.UnlockForWriting();

		PrintMessageToDebugLog("MODM_STRMDATA", "All done!");
		return MMSYSERR_NOERROR;
	}
	case MODM_PROPERTIES: {
		if (!bass_initialized || !dwUser) {
			PrintMessageToDebugLog("MODM_PROPERTIES", "You can't call midiStreamProperties with a normal MIDI stream, or the driver isn't ready.");
			return DebugResult(MMSYSERR_INVALPARAM, TRUE);
		}
		else if (!((DWORD)dwParam2 & (MIDIPROP_GET | MIDIPROP_SET))) {
			PrintMessageToDebugLog("MODM_PROPERTIES", "The MIDI application is confused, and didn't specify if it wanted to get the properties or set them.");
			return DebugResult(MMSYSERR_INVALPARAM, TRUE);
		}
		else if ((DWORD)dwParam2 & MIDIPROP_TEMPO) {
			MIDIPROPTEMPO* MPropTempo = (MIDIPROPTEMPO*)dwParam1;

			if (sizeof(MIDIPROPTEMPO) != MPropTempo->cbStruct) {
				PrintMessageToDebugLog("MODM_PROPERTIES", "Invalid pointer to MIDIPROPTEMPO struct.");
				return DebugResult(MMSYSERR_INVALPARAM, TRUE);
			}
			else if ((DWORD)dwParam2 & MIDIPROP_SET) {
				PrintMessageToDebugLog("MODM_PROPERTIES", "CookedPlayer's tempo set to received value.");
				((CookedPlayer*)dwUser)->Tempo = MPropTempo->dwTempo;
				PrintStreamValueToDebugLog("MODM_PROPERTIES", "Received Tempo", ((CookedPlayer*)dwUser)->Tempo);
				((CookedPlayer*)dwUser)->TempoMulti = ((((CookedPlayer*)dwUser)->Tempo * 10) / ((CookedPlayer*)dwUser)->TimeDiv);
				PrintStreamValueToDebugLog("MODM_PROPERTIES", "New TempoMulti", ((CookedPlayer*)dwUser)->TempoMulti);
			}
			else if ((DWORD)dwParam2 & MIDIPROP_GET) {
				PrintMessageToDebugLog("MODM_PROPERTIES", "CookedPlayer's tempo sent to MIDI application.");
				MPropTempo->dwTempo = ((CookedPlayer*)dwUser)->Tempo;
			}
		}
		else if ((DWORD)dwParam2 & MIDIPROP_TIMEDIV) {
			MIDIPROPTIMEDIV* MPropTimeDiv = (MIDIPROPTIMEDIV*)dwParam1;

			if (sizeof(MIDIPROPTIMEDIV) != MPropTimeDiv->cbStruct) {
				PrintMessageToDebugLog("MODM_PROPERTIES", "Invalid pointer to MIDIPROPTIMEDIV struct.");
				return DebugResult(MMSYSERR_INVALPARAM, TRUE);
			}
			else if ((DWORD)dwParam2 & MIDIPROP_SET) {
				PrintMessageToDebugLog("MODM_PROPERTIES", "CookedPlayer's time division set to received value.");
				((CookedPlayer*)dwUser)->TimeDiv = MPropTimeDiv->dwTimeDiv;
				PrintStreamValueToDebugLog("MODM_PROPERTIES", "Received TimeDiv", ((CookedPlayer*)dwUser)->TimeDiv);
				((CookedPlayer*)dwUser)->TempoMulti = ((((CookedPlayer*)dwUser)->Tempo * 10) / ((CookedPlayer*)dwUser)->TimeDiv);
				PrintStreamValueToDebugLog("MODM_PROPERTIES", "New TempoMulti", ((CookedPlayer*)dwUser)->TempoMulti);
			}
			else if ((DWORD)dwParam2 & MIDIPROP_GET) {
				PrintMessageToDebugLog("MODM_PROPERTIES", "CookedPlayer's time division sent to MIDI application.");
				MPropTimeDiv->dwTimeDiv = ((CookedPlayer*)dwUser)->TimeDiv;
			}
		}
		else {
			PrintMessageToDebugLog("MODM_PROPERTIES", "Invalid properties.");
			return DebugResult(MMSYSERR_INVALPARAM, TRUE);
		}

		return MMSYSERR_NOERROR;
	}
	case MODM_GETPOS: {
		if (!bass_initialized || !dwUser) return DebugResult(MIDIERR_NOTREADY, TRUE);				// The driver isn't ready
		if (!dwParam1 || !dwParam2) return DebugResult(MMSYSERR_INVALPARAM, TRUE);					// Invalid parameters

		PrintMessageToDebugLog("MODM_GETPOS", "The app wants to know the current position of the stream.");
		switch (((MMTIME*)dwParam1)->wType) {
		case TIME_BYTES:
			PrintMessageToDebugLog("TIME_BYTES", "The app wanted it in bytes.");
			((MMTIME*)dwParam1)->u.cb = ((CookedPlayer*)dwUser)->ByteAccumulator;
			break;
		case TIME_MIDI:
			PrintMessageToDebugLog("TIME_MIDI", "The app wanted it in MIDI time.");
			((MMTIME*)dwParam1)->u.midi.songptrpos = ((CookedPlayer*)dwUser)->TimeAccumulator / 10;
			break;
		case TIME_MS:
			PrintMessageToDebugLog("TIME_MS", "The app wanted it in milliseconds.");
			((MMTIME*)dwParam1)->u.ms = ((CookedPlayer*)dwUser)->TimeAccumulator / 10000;
			break;
		case TIME_TICKS:
			PrintMessageToDebugLog("TIME_TICKS", "The app wanted it in ticks.");
			((MMTIME*)dwParam1)->u.ticks = ((CookedPlayer*)dwUser)->TickAccumulator;
			break;
		default:
			PrintMessageToDebugLog("MODM_GETPOS", "Unrecognized wType. Parsing in the default format of milliseconds.");
			((MMTIME*)dwParam1)->u.ms = ((CookedPlayer*)dwUser)->TimeAccumulator / 10000;
			break;
		}

		PrintMessageToDebugLog("MODM_GETPOS", "The app now knows the position.");
		return MMSYSERR_NOERROR;
	}
	case MODM_RESTART: {
		if (!bass_initialized || !dwUser) return DebugResult(MIDIERR_NOTREADY, TRUE);				// The driver isn't ready

		if (((CookedPlayer*)dwUser)->Paused != FALSE) {
			((CookedPlayer*)dwUser)->Paused = FALSE;
			PrintMessageToDebugLog("MODM_RESTART", "CookedPlayer is now playing.");
		}
		else PrintMessageToDebugLog("MODM_RESTART", "CookedPlayer is already playing.");

		return MMSYSERR_NOERROR;
	}
	case MODM_PAUSE: {
		if (!bass_initialized || !dwUser) return DebugResult(MIDIERR_NOTREADY, TRUE);				// The driver isn't ready

		if (((CookedPlayer*)dwUser)->Paused != TRUE) {
			((CookedPlayer*)dwUser)->Paused = TRUE;
			ResetSynth(FALSE);
			PrintMessageToDebugLog("MODM_RESTART", "CookedPlayer is now paused.");
		}
		else PrintMessageToDebugLog("MODM_RESTART", "CookedPlayer is already paused.");

		return MMSYSERR_NOERROR;
	}
	case MODM_STOP: {
		if (!bass_initialized || !dwUser) return DebugResult(MIDIERR_NOTREADY, TRUE);				// The driver isn't ready

		PrintMessageToDebugLog("MODM_STOP", "The app requested OmniMIDI to stop CookedPlayer.");
		((CookedPlayer*)dwUser)->Paused = TRUE;

		LPMIDIHDR hdr = ((CookedPlayer*)dwUser)->MIDIHeaderQueue;
		while (hdr)
		{
			((CookedPlayer*)dwUser)->Lock.LockForWriting();
			PrintMessageToDebugLog("MODM_STRMDATA", "Marking buffer as done and not in queue anymore...");
			hdr->dwFlags &= ~MHDR_INQUEUE;
			hdr->dwFlags |= MHDR_DONE;
			((CookedPlayer*)dwUser)->Lock.UnlockForWriting();

			DriverCallback(OMCallback, OMFlags, OMDevice, MOM_DONE, OMInstance, (DWORD_PTR)hdr, 0);
			hdr = hdr->lpNext;
		}

		ResetSynth(FALSE);

		PrintMessageToDebugLog("MODM_STOP", "CookedPlayer is now stopped.");
		return MMSYSERR_NOERROR;
	}
	case MODM_PREPARE:
		// Pass it to a KDMAPI function
		return PrepareLongData((MIDIHDR*)dwParam1);
	case MODM_UNPREPARE:
		// Pass it to a KDMAPI function
		return UnprepareLongData((MIDIHDR*)dwParam1);
	case MODM_GETNUMDEVS:
		// Return "1" if the process isn't blacklisted, otherwise the driver doesn't exist OwO
		return BlackListSystem();
	case MODM_GETDEVCAPS:
		// Return OM's caps to the app
		return GiveOmniMIDICaps((PVOID)dwParam1, (DWORD)dwParam2);
	case MODM_GETVOLUME: {
		// Tell the app the current output volume of the driver
		PrintMessageToDebugLog("MODM_GETVOLUME", "The app wants to know the current output volume of the driver.");
		*(LONG*)dwParam1 = (LONG)(SynthVolume * 0xFFFF);
		PrintMessageToDebugLog("MODM_GETVOLUME", "The app knows the volume now.");
		return MMSYSERR_NOERROR;
	}
	case MODM_SETVOLUME: {
		// The app isn't allowed to set the volume, everything's fine anyway
		PrintMessageToDebugLog("MODM_SETVOLUME", "Dummy, the app has no control over the driver's audio output.");
		return MMSYSERR_NOERROR;
	}
	case MODM_RESET: {
		// Stop all the current active voices
		PrintMessageToDebugLog("MODM_RESET", "The app sent a reset command.");
		ResetSynth(FALSE);
		return MMSYSERR_NOERROR;
	}
	case MODM_OPEN: {
		// The driver doesn't support stream mode
		PrintMessageToDebugLog("MODM_OPEN", "The app requested the driver to initialize its audio stream.");

		if (!AlreadyInitializedViaKDMAPI && !bass_initialized) {
			// Parse callback and instance
			// AddVectoredExceptionHandler(1, OmniMIDICrashHandler);
			PrintMessageToDebugLog("MODM_OPEN", "Preparing callback data (If present)...");
			OMHMIDI = ((MIDIOPENDESC*)dwParam1)->hMidi;
			OMCallback = ((MIDIOPENDESC*)dwParam1)->dwCallback;
			OMInstance = ((MIDIOPENDESC*)dwParam1)->dwInstance;
			OMFlags = HIWORD((DWORD)dwParam2);

			// Open the driver
			PrintMessageToDebugLog("MODM_OPEN", "Initializing driver...");
			DoStartClient();
			ResetSynth(TRUE);

			// Prepare registry for CookedPlayer
			OpenRegistryKey(Configuration, L"Software\\OmniMIDI\\Configuration", FALSE);
			RegQueryValueEx(Configuration.Address, L"DisableCookedPlayer", NULL, &dwType, (LPBYTE)&ManagedSettings.DisableCookedPlayer, &dwSize);

			if ((DWORD)dwParam2 & MIDI_IO_COOKED && !ManagedSettings.DisableCookedPlayer) {
				// CookedPlayer only supports CALLBACK_FUNCTION
				if (!((DWORD)dwParam2 & ~(MIDI_IO_COOKED | CALLBACK_FUNCTION)))
				{
					PrintMessageToDebugLog("MODM_OPEN", "MIDI_IO_COOKED requested.");

					PrintMessageToDebugLog("MODM_OPEN", "Checking if old CookedPlayer thread is alive...");
					KillOldCookedPlayer();

					// Prepare the CookedPlayer
					PrintMessageToDebugLog("MODM_OPEN", "Preparing CookedPlayer struct...");

					CookedPlayer* TPlayer = (CookedPlayer*)malloc(sizeof(CookedPlayer));
					RtlZeroMemory(TPlayer, sizeof(*TPlayer));

					TPlayer->Paused = TRUE;
					TPlayer->Tempo = 5000000;
					TPlayer->TimeDiv = 348;
					TPlayer->TempoMulti = ((TPlayer->Tempo * 10) / TPlayer->TimeDiv);
					PrintStreamValueToDebugLog("MODM_OPEN", "TempoMulti", TPlayer->TempoMulti);

					PrintMessageToDebugLog("MODM_OPEN", "CookedPlayer struct prepared.");

					// Transfer player to dwUser
					*(CookedPlayer**)dwUser = TPlayer;
					PrintMessageToDebugLog("MODM_OPEN", "Passed struct to dwUser.");

					// Create player thread
					PrintMessageToDebugLog("MODM_OPEN", "Preparing thread for CookedPlayer...");
					CookedThread.ThreadHandle = CreateThread(NULL, NULL, (LPTHREAD_START_ROUTINE)CookedPlayerSystem, *(LPVOID*)dwUser, NULL, (LPDWORD)CookedThread.ThreadAddress);
					while (!TPlayer->IsThreadReady)
					{ 
						PrintMessageToDebugLog("MODM_OPEN", "Waiting for thread to start...");
						Sleep(1);
					}

					PrintMessageToDebugLog("MODM_OPEN", "Thread is running.");

					PrintMessageToDebugLog("MODM_OPEN", "The driver is now ready to receive MIDI headers for the CookedPlayer.");
				}
				else {
					PrintMessageToDebugLog("MODM_OPEN", "MIDI_IO_COOKED is only supported with CALLBACK_FUNCTION! Preparation aborted.");
					DoStopClient();
					return DebugResult(MMSYSERR_NOTSUPPORTED, TRUE);
				}
			}
			else if (ManagedSettings.DisableCookedPlayer) {
				PrintMessageToDebugLog("MODM_OPEN", "CookedPlayer has been disabled in the configurator.");
				DoStopClient();
				return DebugResult(MMSYSERR_NOTSUPPORTED, TRUE);
			}

			// Tell the app that the driver is ready
			PrintMessageToDebugLog("MODM_OPEN", "Sending callback data to app (If present)...");
			DriverCallback(OMCallback, OMFlags, OMDevice, MOM_OPEN, OMInstance, 0, 0);

			PrintMessageToDebugLog("MODM_OPEN", "Everything is fine.");
		}
		else {
			PrintMessageToDebugLog("MODM_OPEN", "The driver has already been initialized. Cannot initialize it twice!");
			return DebugResult(MMSYSERR_ALLOCATED, TRUE);
		}

		return MMSYSERR_NOERROR;
	}
	case MODM_CLOSE: {
		if (!AlreadyInitializedViaKDMAPI) {
			// RemoveVectoredExceptionHandler(OmniMIDICrashHandler);
			PrintMessageToDebugLog("MODM_CLOSE", "The app requested the driver to terminate its audio stream.");
			ResetSynth(TRUE);

			if (bass_initialized) {
				PrintMessageToDebugLog("MODM_CLOSE", "Terminating driver...");
				KillOldCookedPlayer();
				DoStopClient();
			}

			PrintMessageToDebugLog("MODM_CLOSE", "Sending callback data to app (If present)...");
			DriverCallback(OMCallback, OMFlags, OMDevice, MOM_CLOSE, OMInstance, 0, 0);

			PrintMessageToDebugLog("MODM_CLOSE", "Everything is fine.");
		}
		else PrintMessageToDebugLog("MODM_OPEN", "The driver is already in use via KDMAPI. Cannot terminate it!");

		return MMSYSERR_NOERROR;
	}
	case MODM_CACHEPATCHES:
		// Not needed for OmniMIDI
		PrintMessageToDebugLog("modMessage", "MODM_CACHEPATCHES is not required by OmniMIDI, since it uses SoundFonts.");
		return MMSYSERR_NOERROR;
	case MODM_CACHEDRUMPATCHES:
		// Not needed for OmniMIDI
		PrintMessageToDebugLog("modMessage", "MODM_CACHEDRUMPATCHES is not required by OmniMIDI, since it uses SoundFonts.");
		return MMSYSERR_NOERROR;
	case DRV_QUERYDEVICEINTERFACESIZE:
		// Not needed for OmniMIDI
		PrintMessageToDebugLog("modMessage", "DRV_QUERYDEVICEINTERFACESIZE not supported by OmniMIDI.");
		return DebugResult(MMSYSERR_NOTSUPPORTED, FALSE);
	case DRV_QUERYDEVICEINTERFACE:
		// Not needed for OmniMIDI
		PrintMessageToDebugLog("modMessage", "DRV_QUERYDEVICEINTERFACE not supported by OmniMIDI.");
		return DebugResult(MMSYSERR_NOTSUPPORTED, FALSE);
	}
}