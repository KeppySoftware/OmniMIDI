/*
OmniMIDI, a fork of BASSMIDI Driver

Thank you Kode54 for allowing me to fork your awesome driver.
*/

#pragma once

typedef unsigned __int64 QWORD;
typedef long NTSTATUS;

// KDMAPI version
#define CUR_MAJOR	1
#define CUR_MINOR	60
#define CUR_BUILD	0
#define CUR_REV		0

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
#define Between(value, a, b) ((value) >= a && (value) <= b)

#define ERRORCODE		0
#define CAUSE			1
#define LONGMSG_MAXSIZE	65535

#define STATUS_SUCCESS 0
#define STATUS_TIMER_RESOLUTION_NOT_SET 0xC0000245

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
#include <assert.h>
#include <stdlib.h>
#include <time.h>
#include <vector>
#include <algorithm>
#include <windows.h>
#include <Dbghelp.h>
#include <mmsystem.h>
#include <dsound.h>
#include "Resource.h"
#include "OmniMIDI.h"
#include "RTSSOSD.h"

// BASS headers
#include <bass.h>
#include <bassmidi.h>
#include <bassenc.h>
#include <bassasio.h>
#include <bass_vst.h>

// Important
#include "Values.h"
#include "Debug.h"

// NTSTATUS
#define NT_SUCCESS(StatCode) ((NTSTATUS)(StatCode) == 0)
#define NTAPI __stdcall
// these functions have identical prototypes
typedef NTSTATUS(NTAPI* NDE)(BOOLEAN, INT64*);
typedef NTSTATUS(NTAPI* NQST)(QWORD*);
typedef NTSTATUS(NTAPI* DDP)(DWORD, HANDLE, UINT, LONG, LONG);

static NDE NtDelayExecution = 0;
static NQST NtQuerySystemTime = 0;
static DDP DefDriverProcImp = 0;

// Blinx best game
static HMODULE bass = NULL, bassasio = NULL, bassenc = NULL, bassmidi = NULL, bass_vst = NULL;	// BASS libs handles

#define LOADLIBFUNCTION(l, f) *((void**)&f)=GetProcAddress(l,#f)

// F**k Sleep() tbh
void NTSleep(__int64 usec) {
	__int64 neg = (usec * -1);
	NtDelayExecution(FALSE, &neg);
}

// Critical sections but handled by OmniMIDI functions because f**k Windows
static DWORD DummyPlayBufData() { return 0; }
static VOID DummyPrepareForBASSMIDI(DWORD LastRunningStatus, DWORD dwParam1) { return; }
static MMRESULT DummyParseData(DWORD dwParam1) { return MIDIERR_NOTREADY; }
static BOOL WINAPI DummyBMSE(HSTREAM handle, DWORD chan, DWORD event, DWORD param) { return FALSE; }

// Hyper switch
static BOOL HyperMode = 0;
static MMRESULT(*_PrsData)(DWORD dwParam1) = DummyParseData;
static VOID(*_PforBASSMIDI)(DWORD LastRunningStatus, DWORD dwParam1) = DummyPrepareForBASSMIDI;
static DWORD(*_PlayBufData)(void) = DummyPlayBufData;
static DWORD(*_PlayBufDataChk)(void) = DummyPlayBufData;
static BOOL(WINAPI*_BMSE)(HSTREAM handle, DWORD chan, DWORD event, DWORD param) = DummyBMSE;
// What does it do? It gets rid of the useless functions,
// and passes the events without checking for anything

// Predefined sleep values, useful for redundancy
#define _FWAIT NTSleep(1)								// Fast wait
#define _WAIT NTSleep(100)								// Normal wait
#define _SWAIT NTSleep(5000)							// Slow wait
#define _CFRWAIT NTSleep(16667)							// Cap framerate wait

// Variables
#include "BASSErrors.h"
#include "LockSystem.h"

// OmniMIDI vital parts
#include "SoundFontLoader.h"
#include "BufferSystem.h"
#include "Settings.h"
#include "BlacklistSystem.h"
#include "DriverInit.h"
#include "KDMAPI.h"
#include "CookedPlayer.h"

// OmniMIDI GUID
// {62F3192B-A961-456D-ABCA-A5C95A14B9AA}
static const GUID OMCLSID = { 0x62F3192B, 0xA961, 0x456D, { 0xAB, 0xCA, 0xA5, 0xC9, 0x5A, 0x14, 0xB9, 0xAA } };

BOOL APIENTRY DllMain(HMODULE hModule, DWORD CallReason, LPVOID lpReserved)
{
	switch (CallReason) {
	case DLL_PROCESS_ATTACH:
	{
		BOOL pbDP = FALSE;

		if (BannedProcesses()) {
			OutputDebugStringA("Process is banned! Bye!");
			return FALSE;
		}

		if (CheckRemoteDebuggerPresent(GetCurrentProcess(), &pbDP))
		{
			if (pbDP) {
				MessageBoxA(NULL, "You're running OmniMIDI under a debugger!\n\nKeep in mind that the debugger might slow down the driver, making it perform and sound worse.\n\nPress OK to continue.", "OmniMIDI - WARNING", MB_ICONWARNING | MB_SYSTEMMODAL);
			}
		}

		hinst = hModule;
		DisableThreadLibraryCalls(hinst);
		BASS_MIDI_StreamEvent = DummyBMSE;

		if (!NtDelayExecution || !NtQuerySystemTime) {
			NtDelayExecution = (NDE)GetProcAddress(GetModuleHandleW(L"ntdll"), "NtDelayExecution");
			NtQuerySystemTime = (NQST)GetProcAddress(GetModuleHandleW(L"ntdll"), "NtQuerySystemTime");

			if (!NtDelayExecution || !NtQuerySystemTime) {
				MessageBoxA(NULL, "Failed to parse NT functions from NTDLL!\nPress OK to stop the loading process of OmniMIDI.", "OmniMIDI - ERROR", MB_ICONERROR | MB_SYSTEMMODAL);
				return FALSE;
			}

			if (!NT_SUCCESS(NtQuerySystemTime(&TickStart))) {
				MessageBoxA(NULL, "Failed to parse starting tick through NtQuerySystemTime!\nPress OK to stop the loading process of OmniMIDI.", "OmniMIDI - ERROR", MB_ICONERROR | MB_SYSTEMMODAL);
				return FALSE;
			}

			// Loaded!
			return TRUE;
		}

		break;
	}
	case DLL_PROCESS_DETACH:
	{
		hinst = NULL;
		break;
	}
	case DLL_THREAD_ATTACH:
	case DLL_THREAD_DETACH: 
		break;
	}

	return TRUE;
}

LONG WINAPI DriverProc(DWORD dwDriverIdentifier, HANDLE hdrvr, UINT uMsg, LONG lParam1, LONG lParam2)
{
	switch (uMsg) {
	case DRV_LOAD:
	case DRV_FREE:
		return DRVCNF_OK;

	case DRV_OPEN:
	case DRV_CLOSE:
		return DRVCNF_OK;

	case DRV_QUERYCONFIGURE:
		return DRVCNF_OK;

	case DRV_CONFIGURE:
		TCHAR configuratorapp[MAX_PATH];
		if (SUCCEEDED(SHGetFolderPath(NULL, CSIDL_SYSTEMX86, NULL, 0, configuratorapp)))
		{
			PathAppend(configuratorapp, _T("\\OmniMIDI\\OmniMIDIConfigurator.exe"));
			ShellExecute(NULL, L"open", configuratorapp, L"/AST", NULL, SW_SHOWNORMAL);
			return DRVCNF_OK;
		}
		return DRVCNF_CANCEL;

	case DRV_ENABLE:
	case DRV_DISABLE:
		return DRVCNF_OK;

	case DRV_INSTALL:
		return DRVCNF_OK;

	case DRV_REMOVE:
		return DRVCNF_OK;
	}

	if (!winmm) {
		winmm = GetModuleHandleA("winmm");
		if (!winmm) {
			winmm = LoadLibraryA("winmm");
			if (!winmm) {
				MessageBoxA(NULL, "Failed to load WinMM!\nPress OK to stop the loading process of OmniMIDI.", "OmniMIDI - ERROR", MB_ICONERROR | MB_SYSTEMMODAL);
				return FALSE;
			}
		}

		DefDriverProcImp = (DDP)GetProcAddress(winmm, "DefDriverProc");
		if (!DefDriverProcImp) {
			MessageBoxA(NULL, "Failed to parse DefDriverProc function from WinMM!\nPress OK to stop the loading process of OmniMIDI.", "OmniMIDI - ERROR", MB_ICONERROR | MB_SYSTEMMODAL);
			return FALSE;
		}
	}

	return DefDriverProcImp(dwDriverIdentifier, (HDRVR)hdrvr, uMsg, lParam1, lParam2);
}

DWORD GiveOmniMIDICaps(PVOID capsPtr, DWORD capsSize) {
	// Initialize values
	static BOOL LoadedOnce = FALSE;

	MIDIOUTCAPSA* MIDICapsA;
	MIDIOUTCAPSW* MIDICapsW;
	MIDIOUTCAPS2A* MIDICaps2A;
	MIDIOUTCAPS2W* MIDICaps2W;

	DWORD StreamCapable = NULL;
	DWORD Technology = NULL;
	WORD MID = 0x0000;
	WORD PID = 0x0000;

	try {
		PrintMessageToDebugLog("MODM_GETDEVCAPS", "The MIDI app sent a MODM_GETDEVCAPS request to the driver.");

		if (!LoadedOnce) {
			PrintMessageToDebugLog("MODM_GETDEVCAPS", "Loading settings from the registry...");
			OpenRegistryKey(Configuration, L"Software\\OmniMIDI\\Configuration", FALSE);
			RegQueryValueEx(Configuration.Address, L"DisableChime", NULL, &dwType, (LPBYTE)& DisableChime, &dwSize);
			RegQueryValueEx(Configuration.Address, L"SynthType", NULL, &dwType, (LPBYTE)& SynthType, &dwSize);
			RegQueryValueEx(Configuration.Address, L"DebugMode", NULL, &dwType, (LPBYTE)& ManagedSettings.DebugMode, &dwSize);
			RegQueryValueEx(Configuration.Address, L"VID", NULL, &dwType, (LPBYTE)& MID, &dwSize);
			RegQueryValueEx(Configuration.Address, L"PID", NULL, &dwType, (LPBYTE)& PID, &dwSize);
			RegQueryValueEx(Configuration.Address, L"SynthName", NULL, &SNType, (LPBYTE)& SynthNameW, &SNSize);
			RegQueryValueEx(Configuration.Address, L"DisableCookedPlayer", NULL, &dwType, (LPBYTE)& ManagedSettings.DisableCookedPlayer, &dwSize);

			// If the synth type ID is bigger than the size of the synth types array,
			// set it automatically to MOD_MIDIPORT
			if (SynthType >= ((sizeof(SynthNamesTypes) / sizeof(*SynthNamesTypes))))
				Technology = MOD_MIDIPORT;
			// Else, load the requested value
			else Technology = SynthNamesTypes[SynthType];

			// If the debug mode is enabled, and the process isn't banned, create the debug log
			if (ManagedSettings.DebugMode && BlackListSystem())
				CreateConsole();

			// If the synthname length is less than 1, or if it's just a space, use the default name
			PrintMessageToDebugLog("MODM_GETDEVCAPS", "Checking if SynthNameW is valid...");
			if (wcslen(SynthNameW) < 1 || (wcslen(SynthNameW) == 1 && iswspace(SynthNameW[0]))) {
				RtlSecureZeroMemory(SynthNameW, sizeof(SynthNameW));
				wcsncpy(SynthNameW, L"OmniMIDI\0", MAXPNAMELEN);
			}

			PrintMessageToDebugLog("MODM_GETDEVCAPS", "Converting SynthNameW to SynthNameA...");
			wcstombs(SynthName, SynthNameW, MAXPNAMELEN);

			StreamCapable = (!ManagedSettings.DisableCookedPlayer && !CPBlacklisted);

			LoadedOnce = TRUE;
		}

		PrintMessageToDebugLog("MODM_GETDEVCAPS", "Sharing MIDI device caps with application...");

		// Prepare the caps item
		switch (capsSize) {
		case (sizeof(MIDIOUTCAPSA)):
		{
			MIDICapsA = (MIDIOUTCAPSA*)capsPtr;
			strncpy(MIDICapsA->szPname, SynthName, MAXPNAMELEN);
			MIDICapsA->dwSupport = (StreamCapable ? MIDICAPS_STREAM : 0) | MIDICAPS_VOLUME;
			MIDICapsA->wChannelMask = 0xFFFF;
			MIDICapsA->wMid = MID;
			MIDICapsA->wPid = PID;
			MIDICapsA->wTechnology = Technology;
			MIDICapsA->wVoices = 65535;
			MIDICapsA->vDriverVersion = MAKEWORD(6, 0);
			PrintMessageToDebugLog("MODM_GETDEVCAPS (ASCII, Type 1)", "Done sharing MIDI device caps.");
			break;
		}
		case (sizeof(MIDIOUTCAPSW)):
		{
			MIDICapsW = (MIDIOUTCAPSW*)capsPtr;
			wcsncpy(MIDICapsW->szPname, SynthNameW, MAXPNAMELEN);
			MIDICapsW->dwSupport = (StreamCapable ? MIDICAPS_STREAM : 0) | MIDICAPS_VOLUME;
			MIDICapsW->wChannelMask = 0xFFFF;
			MIDICapsW->wMid = MID;
			MIDICapsW->wPid = PID;
			MIDICapsW->wTechnology = Technology;
			MIDICapsW->wVoices = 65535;
			MIDICapsW->vDriverVersion = MAKEWORD(6, 0);
			PrintMessageToDebugLog("MODM_GETDEVCAPS (Unicode, Type 1)", "Done sharing MIDI device caps.");
			break;
		}
		case (sizeof(MIDIOUTCAPS2A)):
		{
			MIDICaps2A = (MIDIOUTCAPS2A*)capsPtr;
			strncpy(MIDICaps2A->szPname, SynthName, MAXPNAMELEN);
			MIDICaps2A->ManufacturerGuid = OMCLSID;
			MIDICaps2A->NameGuid = OMCLSID;
			MIDICaps2A->ProductGuid = OMCLSID;
			MIDICaps2A->dwSupport = (StreamCapable ? MIDICAPS_STREAM : 0) | MIDICAPS_VOLUME;
			MIDICaps2A->wChannelMask = 0xFFFF;
			MIDICaps2A->wMid = MID;
			MIDICaps2A->wPid = PID;
			MIDICaps2A->wTechnology = Technology;
			MIDICaps2A->wVoices = 65535;
			MIDICaps2A->vDriverVersion = MAKEWORD(6, 0);
			PrintMessageToDebugLog("MODM_GETDEVCAPS (ASCII, Type 2)", "Done sharing MIDI device caps.");
			break;
		}
		case (sizeof(MIDIOUTCAPS2W)):
		{
			MIDICaps2W = (MIDIOUTCAPS2W*)capsPtr;
			wcsncpy(MIDICaps2W->szPname, SynthNameW, MAXPNAMELEN);
			MIDICaps2W->ManufacturerGuid = OMCLSID;
			MIDICaps2W->NameGuid = OMCLSID;
			MIDICaps2W->ProductGuid = OMCLSID;
			MIDICaps2W->dwSupport = (StreamCapable ? MIDICAPS_STREAM : 0) | MIDICAPS_VOLUME;
			MIDICaps2W->wChannelMask = 0xFFFF;
			MIDICaps2W->wMid = MID;
			MIDICaps2W->wPid = PID;
			MIDICaps2W->wTechnology = Technology;
			MIDICaps2W->wVoices = 65535;
			MIDICaps2W->vDriverVersion = MAKEWORD(6, 0);
			PrintMessageToDebugLog("MODM_GETDEVCAPS (Unicode, Type 2)", "Done sharing MIDI device caps.");
			break;
		}
		}

		return MMSYSERR_NOERROR;
	}
	catch (...) {
		return MMSYSERR_NOTENABLED;
	}
}

extern "C" STDAPI_(DWORD) modMessage(UINT uDeviceID, UINT uMsg, DWORD_PTR dwUser, DWORD_PTR dwParam1, DWORD_PTR dwParam2) {
	static BOOL PreventInit = FALSE;
	MMRESULT RetVal = MMSYSERR_NOERROR;
	
	/*
	char* Msg = (char*)malloc(sizeof(char) * NTFS_MAX_PATH);
	sprintf(Msg, "Received modMessage(%u, %u, %X, %X, %X)", uDeviceID, uMsg, dwUser, dwParam1, dwParam2);
	PrintMessageToDebugLog("MOD_MESSAGE", Msg);
	free(Msg);
	*/

	switch (uMsg) {
	case MODM_DATA:
		// Parse the data lol
		return _PrsData(dwParam1);
	case MODM_LONGDATA: {
		// Pass it to a KDMAPI function
		RetVal = SendDirectLongData((MIDIHDR*)dwParam1);

		// Tell the app that the buffer has been played
		if (CustomCallback) CustomCallback((HMIDIOUT)OMHMIDI, MOM_DONE, WMMCI, dwParam1, 0);
		// if (CustomCallback) CustomCallback((HMIDIOUT)OMMOD.hMidi, MM_MOM_DONE, WMMCI, dwParam1, 0);
		return RetVal;
	}
	case MODM_STRMDATA: {
		if (!bass_initialized || !dwUser)
			return DebugResult("MODM_STRMDATA", MIDIERR_NOTREADY, "You can't call midiStreamData with a normal MIDI stream, or the driver isn't ready.");

		if ((DWORD)dwParam2 < offsetof(MIDIHDR, dwOffset) ||
			!((MIDIHDR*)dwParam1) || !((MIDIHDR*)dwParam1)->lpData ||
			((MIDIHDR*)dwParam1)->dwBufferLength < ((MIDIHDR*)dwParam1)->dwBytesRecorded ||
			((MIDIHDR*)dwParam1)->dwBytesRecorded % 4)
		{
			return DebugResult("MODM_STRMDATA", MMSYSERR_INVALPARAM, "The buffer doesn't exist, hasn't been allocated or is not valid.");
		}

		if (!(((MIDIHDR*)dwParam1)->dwFlags & MHDR_PREPARED))
			return DebugResult("MODM_STRMDATA", MIDIERR_UNPREPARED, "The buffer is not prepared.");

		if (!(((MIDIHDR*)dwParam1)->dwFlags & MHDR_DONE)) {
			if (((MIDIHDR*)dwParam1)->dwFlags & MHDR_INQUEUE)
				return DebugResult("MODM_STRMDATA", MIDIERR_STILLPLAYING, "The buffer is still being played.");
		}

		PrintMessageToDebugLog("MODM_STRMDATA", "Locking for writing...");
		LockForWriting(&((CookedPlayer*)dwUser)->Lock);

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

			if (phdr == (MIDIHDR*)dwParam1) {
				PrintMessageToDebugLog("MODM_STRMDATA", "Unlocking...");
				UnlockForWriting(&((CookedPlayer*)dwUser)->Lock);
				return MIDIERR_STILLPLAYING;
			}
			while (phdr->lpNext)
			{
				phdr = phdr->lpNext;
				if (phdr == (MIDIHDR*)dwParam1)
				{
					PrintMessageToDebugLog("MODM_STRMDATA", "Unlocking...");
					UnlockForWriting(&((CookedPlayer*)dwUser)->Lock);
					return MIDIERR_STILLPLAYING;
				}
			}
			phdr->lpNext = (MIDIHDR*)dwParam1;
		}
		else ((CookedPlayer*)dwUser)->MIDIHeaderQueue = (MIDIHDR*)dwParam1;
		PrintMessageToDebugLog("MODM_STRMDATA", "Copied.");

		PrintMessageToDebugLog("MODM_STRMDATA", "Unlocking...");
		UnlockForWriting(&((CookedPlayer*)dwUser)->Lock);

		PrintMessageToDebugLog("MODM_STRMDATA", "All done!");
		return MMSYSERR_NOERROR;
	}
	case MODM_PROPERTIES: {
		if (!bass_initialized || !dwUser)
			return DebugResult("MODM_PROPERTIES", MIDIERR_NOTREADY, "You can't call midiStreamProperties with a normal MIDI stream, or the driver isn't ready.");

		if (!((DWORD)dwParam2 & (MIDIPROP_GET | MIDIPROP_SET)))
			return DebugResult("MODM_PROPERTIES", MMSYSERR_INVALPARAM, "The MIDI application is confused, and didn't specify if it wanted to get the properties or set them.");

		if ((DWORD)dwParam2 & MIDIPROP_TEMPO) {
			MIDIPROPTEMPO* MPropTempo = (MIDIPROPTEMPO*)dwParam1;

			if (sizeof(MIDIPROPTEMPO) != MPropTempo->cbStruct) {
				return DebugResult("MODM_PROPERTIES", MMSYSERR_INVALPARAM, "Invalid pointer to MIDIPROPTEMPO struct.");
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
				return DebugResult("MODM_PROPERTIES", MMSYSERR_INVALPARAM, "Invalid pointer to MIDIPROPTIMEDIV struct.");
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
		else return DebugResult("MODM_PROPERTIES", MMSYSERR_INVALPARAM, "Invalid properties.");

		return MMSYSERR_NOERROR;
	}
	case MODM_GETPOS: {
		if (!bass_initialized || !dwUser)
			return DebugResult("MODM_GETPOS", MIDIERR_NOTREADY, "You can't call midiStreamPosition with a normal MIDI stream, or the driver isn't ready.");

		if (!dwParam1 || !dwParam2)
			return DebugResult("MODM_GETPOS", MMSYSERR_INVALPARAM, "Invalid parameters.");

		PrintMessageToDebugLog("MODM_GETPOS", "The app wants to know the current position of the stream.");
		switch (((MMTIME*)dwParam1)->wType) {
		case TIME_BYTES:
			PrintMessageToDebugLog("TIME_BYTES", "The app wanted it in bytes.");
			((MMTIME*)dwParam1)->u.cb = ((CookedPlayer*)dwUser)->ByteAccumulator;
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
			PrintMessageToDebugLog("TIME_UNK", "Unrecognized wType. Parsing in the default format of ticks.");
			((MMTIME*)dwParam1)->wType = TIME_TICKS;
			((MMTIME*)dwParam1)->u.ticks = ((CookedPlayer*)dwUser)->TickAccumulator;
			break;
		}

		PrintMessageToDebugLog("MODM_GETPOS", "The app now knows the position.");
		return MMSYSERR_NOERROR;
	}
	case MODM_RESTART: {
		if (!bass_initialized || !dwUser)
			return DebugResult("MODM_RESTART", MMSYSERR_NOTENABLED, "You can't call midiStreamRestart with a normal MIDI stream, or the driver isn't ready.");

		if (((CookedPlayer*)dwUser)->Paused) {
			((CookedPlayer*)dwUser)->Paused = FALSE;
			PrintMessageToDebugLog("MODM_RESTART", "CookedPlayer is now playing.");
		}

		return MMSYSERR_NOERROR;
	}
	case MODM_PAUSE: {
		if (!bass_initialized || !dwUser)
			return DebugResult("MODM_PAUSE", MMSYSERR_NOTENABLED, "You can't call midiStreamPause with a normal MIDI stream, or the driver isn't ready.");

		if (!((CookedPlayer*)dwUser)->Paused) {
			((CookedPlayer*)dwUser)->Paused = TRUE;
			ResetSynth(FALSE);
			PrintMessageToDebugLog("MODM_PAUSE", "CookedPlayer is now paused.");
		}

		return MMSYSERR_NOERROR;
	}
	case MODM_STOP: {
		if (!bass_initialized || !dwUser)
			return DebugResult("MODM_STOP", MIDIERR_NOTREADY, "You can't call midiStreamStop with a normal MIDI stream, or the driver isn't ready.");

		PrintMessageToDebugLog("MODM_STOP", "The app requested OmniMIDI to stop CookedPlayer.");
		((CookedPlayer*)dwUser)->Paused = TRUE;

		LPMIDIHDR hdr = ((CookedPlayer*)dwUser)->MIDIHeaderQueue;
		while (hdr)
		{
			LockForWriting(&((CookedPlayer*)dwUser)->Lock);
			PrintMessageToDebugLog("MODM_STOP", "Marking buffer as done and not in queue anymore...");
			hdr->dwFlags &= ~MHDR_INQUEUE;
			hdr->dwFlags |= MHDR_DONE;
			UnlockForWriting(&((CookedPlayer*)dwUser)->Lock);

			CustomCallback((HMIDIOUT)OMHMIDI, MOM_DONE, WMMCI, (DWORD_PTR)hdr, 0);
			hdr = hdr->lpNext;
		}

		ResetSynth(FALSE);

		PrintMessageToDebugLog("MODM_STOP", "CookedPlayer is now stopped.");
		return MMSYSERR_NOERROR;
	}
	case MODM_RESET: {
		// Stop all the current active voices
		if (dwUser)
		{
			PrintMessageToDebugLog("MODM_RESET", "The app requested OmniMIDI to reset CookedPlayer.");

			LPMIDIHDR hdr = ((CookedPlayer*)dwUser)->MIDIHeaderQueue;
			while (hdr)
			{
				LockForWriting(&((CookedPlayer*)dwUser)->Lock);
				PrintMessageToDebugLog("MODM_RESET", "Marking buffer as done and not in queue anymore...");
				hdr->dwFlags &= ~MHDR_INQUEUE;
				hdr->dwFlags |= MHDR_DONE;
				UnlockForWriting(&((CookedPlayer*)dwUser)->Lock);

				CustomCallback((HMIDIOUT)OMHMIDI, MOM_DONE, WMMCI, (DWORD_PTR)hdr, 0);
				hdr = hdr->lpNext;
			}
		}
		else PrintMessageToDebugLog("MODM_RESET", "The app sent a reset command.");

		ResetSynth(FALSE);
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
	case MODM_OPEN: {
		if (PreventInit) {
			PrintMessageToDebugLog("MODM_OPEN", "The app is dumb and requested to open the stream again during the initialization process...");
			return MIDIERR_NOTREADY;
		}

		PrintMessageToDebugLog("MODM_OPEN", "The app requested the driver to initialize its audio stream.");
		if (!AlreadyInitializedViaKDMAPI && !bass_initialized) {
			// Prevent the app from calling MODM_OPEN again...
			PreventInit = TRUE;

			// Parse callback and instance
			// AddVectoredExceptionHandler(1, OmniMIDICrashHandler);
			PrintMessageToDebugLog("MODM_OPEN", "Preparing callback data (If present)...");
			OMHMIDI = ((MIDIOPENDESC*)dwParam1)->hMidi;
			OMCallback = ((MIDIOPENDESC*)dwParam1)->dwCallback;
			OMInstance = ((MIDIOPENDESC*)dwParam1)->dwInstance;
			OMFlags = HIWORD((DWORD)dwParam2);

			PrintMIDIOPENDESCToDebugLog("MODM_OPEN", (MIDIOPENDESC*)dwParam1, dwUser, OMFlags);
			EnableBuiltInHandler("MODM_OPEN");

			// Open the driver
			PrintMessageToDebugLog("MODM_OPEN", "Initializing driver...");
			DoStartClient();
			ResetSynth(TRUE);

			// Prepare registry for CookedPlayer
			OpenRegistryKey(Configuration, L"Software\\OmniMIDI\\Configuration", FALSE);
			RegQueryValueEx(Configuration.Address, L"DisableCookedPlayer", NULL, &dwType, (LPBYTE)&ManagedSettings.DisableCookedPlayer, &dwSize);

			if (((DWORD)dwParam2 != CALLBACK_NULL) && ((DWORD)dwParam2 != CALLBACK_EVENT)) {
				CustomCallback = (WMMC)OMCallback;
				WMMCI = OMInstance;
			}

			if ((DWORD)dwParam2 & MIDI_IO_COOKED && !ManagedSettings.DisableCookedPlayer) {
				// CookedPlayer only supports CALLBACK_FUNCTION
				if (!((DWORD)dwParam2 & ~(MIDI_IO_COOKED | CALLBACK_FUNCTION)))
				{
					PrintMessageToDebugLog("MODM_OPEN", "MIDI_IO_COOKED requested.");

					// Prepare the CookedPlayer
					PrintMessageToDebugLog("MODM_OPEN", "Preparing CookedPlayer struct...");

					*(CookedPlayer**)dwUser = (CookedPlayer*)malloc(sizeof(CookedPlayer));
					memset(*(CookedPlayer**)dwUser, 0, sizeof(**(CookedPlayer**)dwUser));

					(*(CookedPlayer**)dwUser)->Paused = TRUE;
					(*(CookedPlayer**)dwUser)->Tempo = 500000;
					(*(CookedPlayer**)dwUser)->TimeDiv = 384;
					(*(CookedPlayer**)dwUser)->TempoMulti = (((*(CookedPlayer**)dwUser)->Tempo * 10) / (*(CookedPlayer**)dwUser)->TimeDiv);
					PrintStreamValueToDebugLog("MODM_OPEN", "TempoMulti", (*(CookedPlayer**)dwUser)->TempoMulti);

					PrintMessageToDebugLog("MODM_OPEN", "CookedPlayer struct prepared.");

					// Create player thread
					PrintMessageToDebugLog("MODM_OPEN", "Preparing thread for CookedPlayer...");
					CookedThread.ThreadHandle = (HANDLE)_beginthreadex(NULL, 0, (_beginthreadex_proc_type)CookedPlayerSystem, *(LPVOID*)dwUser, 0, &CookedThread.ThreadAddress);

					PrintMessageToDebugLog("MODM_OPEN", "Thread is running. The driver is now ready to receive MIDI headers for the CookedPlayer.");
				}
				else {
					DoStopClient();
					PreventInit = FALSE;
					return DebugResult("MODM_OPEN", MMSYSERR_NOTSUPPORTED, "MIDI_IO_COOKED is only supported with CALLBACK_FUNCTION! Preparation aborted.");
				}
			}
			else if (ManagedSettings.DisableCookedPlayer) {
				DoStopClient();
				PreventInit = FALSE;
				return DebugResult("MODM_OPEN", MMSYSERR_NOTSUPPORTED, "CookedPlayer has been disabled in the configurator.");
			}

			// Tell the app that the driver is ready
			if (CustomCallback) {
				PrintMessageToDebugLog("MODM_OPEN", "Sending callback data to app...");
				CustomCallback((HMIDIOUT)OMHMIDI, MOM_OPEN, WMMCI, 0, 0);
			}

			PrintMessageToDebugLog("MODM_OPEN", "Everything is fine.");
			PreventInit = FALSE;
		}
		else {
			PreventInit = FALSE;
			return DebugResult("MODM_OPEN", MMSYSERR_ALLOCATED, "The driver has already been initialized. Cannot initialize it twice!");
		}

		return MMSYSERR_NOERROR;
	}
	case MODM_CLOSE: {
		if (PreventInit) DebugResult("MODM_CLOSE", MMSYSERR_ALLOCATED, "The driver has already been initialized. Cannot initialize it twice!");

		if (!AlreadyInitializedViaKDMAPI) {
			// Prevent the app from calling MODM_CLOSE again...
			PreventInit = TRUE;

			PrintMessageToDebugLog("MODM_CLOSE", "The app requested the driver to terminate its audio stream.");
			ResetSynth(TRUE);

			if (bass_initialized) {
				PrintMessageToDebugLog("MODM_CLOSE", "Terminating driver...");
				KillOldCookedPlayer(dwUser);
				DoStopClient();
				DisableBuiltInHandler("MODM_CLOSE");
			}

			if (CustomCallback) {
				PrintMessageToDebugLog("MODM_CLOSE", "Sending callback data to app...");
				CustomCallback((HMIDIOUT)OMHMIDI, MOM_CLOSE, WMMCI, 0, 0);
			}

			PrintMessageToDebugLog("MODM_CLOSE", "Everything is fine.");
		}
		else PrintMessageToDebugLog("MODM_CLOSE", "The driver is already in use via KDMAPI. Cannot terminate it!");

		PreventInit = FALSE;
		return DebugResult("MODM_CLOSE", MMSYSERR_NOERROR, "The driver has been stopped");
	}
	case MODM_CACHEPATCHES:
		// Not needed for OmniMIDI
		PrintMessageToDebugLog("modMessage", "MODM_CACHEPATCHES is not required by OmniMIDI, since it uses SoundFonts.");
		return MMSYSERR_NOTSUPPORTED;
	case MODM_CACHEDRUMPATCHES:
		// Not needed for OmniMIDI
		PrintMessageToDebugLog("modMessage", "MODM_CACHEDRUMPATCHES is not required by OmniMIDI, since it uses SoundFonts.");
		return MMSYSERR_NOTSUPPORTED;
	case DRV_QUERYDEVICEINTERFACESIZE:
		// Not needed for OmniMIDI
		PrintMessageToDebugLog("modMessage", "DRV_QUERYDEVICEINTERFACESIZE is not required by OmniMIDI.");
		return MMSYSERR_NOTSUPPORTED;
	case DRV_QUERYDEVICEINTERFACE:
		// Not needed for OmniMIDI
		PrintMessageToDebugLog("modMessage", "DRV_QUERYDEVICEINTERFACE is not required by OmniMIDI.");
		return MMSYSERR_NOTSUPPORTED;
	default: {
		// Unrecognized uMsg
		char* Msg = (char*)malloc(sizeof(char) * NTFS_MAX_PATH);
		sprintf(Msg, 
			"The application sent an unknown message! ID: 0x%08x - dwUser: 0x%08x - dwParam1: 0x%08x - dwParam2: 0x%08x", 
			uMsg, dwUser, dwParam1, dwParam2);
		RetVal = DebugResult("modMessage", MMSYSERR_ERROR, Msg);
		free(Msg);
		return RetVal;
	}
	}
}