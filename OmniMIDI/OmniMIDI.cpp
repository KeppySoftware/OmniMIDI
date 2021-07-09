/*
OmniMIDI, a fork of BASSMIDI Driver

Thank you Kode54 for allowing me to fork your awesome driver.
*/

#pragma once

typedef unsigned __int64 QWORD;
typedef long NTSTATUS;

// KDMAPI version
#define CUR_MAJOR	3
#define CUR_MINOR	0
#define CUR_BUILD	0
#define CUR_REV		0

#define _SILENCE_ALL_CXX17_DEPRECATION_WARNINGS
#define STRICT
#define VC_EXTRALEAN
#define WIN32_LEAN_AND_MEAN
#define __STDC_LIMIT_MACROS
#define _CRT_SECURE_CPP_OVERLOAD_STANDARD_NAMES 1

#define BASSASIODEF(f) (WINAPI *f)
#define BASSDEF(f) (WINAPI *f)
#define BASSENCDEF(f) (WINAPI *f)
#define BASSMIDIDEF(f) (WINAPI *f)
#define BASSWASAPIDEF(f) (WINAPI *f)
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
#include <assert.h>
#include "Resource.h"
#include "OmniMIDI.h"
#include "sound_out.h"

// BASS headers
#include <bass.h>
#include <bassmidi.h>
#include <bassenc.h>
#include <bassasio.h>
#include <basswasapi.h>
#include <bass_vst.h>

// Important
#include "LockSystem.h"
#include "Values.h"
#include "Debug.h"

// Shakra backport
#include "ShakraRegSystem.h"

// F**k Sleep() tbh
void NTSleep(__int64 usec) {
	__int64 neg = (usec * -1);
	NtDelayExecution(FALSE, &neg);
}

// Predefined sleep values, useful for redundancy
#define _FWAIT NTSleep(1)								// Fast wait
#define _WAIT NTSleep(100)								// Normal wait
#define _SWAIT NTSleep(5000)							// Slow wait
#define _CFRWAIT NTSleep(16667)							// Cap framerate wait

// Variables
#include "BASSErrors.h"

// OmniMIDI vital parts
#include "SoundFontLoader.h"
#include "BufferSystem.h"
#include "Settings.h"
#include "BlacklistSystem.h"
#include "DriverInit.h"
#include "KDMAPI.h"

BOOL APIENTRY DllMain(HMODULE hModule, DWORD CallReason, LPVOID lpReserved)
{
	switch (CallReason) {
	case DLL_PROCESS_ATTACH:
	{
		if (BannedProcesses()) {
			OutputDebugStringA("Process is banned. OmniMIDI will not load.");
			return FALSE;
		}

		hinst = hModule;
		BASS_MIDI_StreamEvent = DummyBMSE;

		if (!NtDelayExecution || !NtQuerySystemTime) {
			NtDelayExecution = (NDE)GetProcAddress(GetModuleHandleW(L"ntdll"), "NtDelayExecution");
			NtQuerySystemTime = (NQST)GetProcAddress(GetModuleHandleW(L"ntdll"), "NtQuerySystemTime");

			if (!NtDelayExecution || !NtQuerySystemTime) {
				OutputDebugStringA("Failed to parse NT functions from NTDLL! OmniMIDI will not load.");
				return FALSE;
			}

			if (!NT_SUCCESS(NtQuerySystemTime(&TickStart))) {
				OutputDebugStringA("Failed to parse starting tick through NtQuerySystemTime! OmniMIDI will not load.");
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
		DriverRegistration(NULL, hinst, "RegisterDrv", FALSE);
		return DRVCNF_OK;

	case DRV_REMOVE:
		DriverRegistration(NULL, hinst, "UnregisterDrv", FALSE);
		return DRVCNF_OK;
	}

	if (!winmm) {
		winmm = GetModuleHandle(L"winmm");
		if (!winmm) {
			winmm = LoadLibrary(L"winmm");
			if (!winmm) {
				MessageBoxA(NULL, "Failed to load Windows Multimedia API!\nPress OK to stop the loading process of OmniMIDI.", "OmniMIDI - ERROR", MB_ICONERROR | MB_SYSTEMMODAL);
				return FALSE;
			}
		}

		DefDriverProcImp = (DDP)GetProcAddress(winmm, "DefDriverProc");
		if (!DefDriverProcImp) {
			MessageBoxA(NULL, "Failed to parse DefDriverProc function from Windows Multimedia API!\nPress OK to stop the loading process of OmniMIDI.", "OmniMIDI - ERROR", MB_ICONERROR | MB_SYSTEMMODAL);
			return FALSE;
		}
	}

	return DefDriverProcImp(dwDriverIdentifier, (HDRVR)hdrvr, uMsg, lParam1, lParam2);
}

DWORD GiveOmniMIDICaps(PVOID capsPtr, DWORD capsSize) {
	// Initialize values
	static BOOL LoadedOnce = FALSE;

	DWORD StreamCapable = NULL;

	try {
		PrintMessageToDebugLog("MODM_GETDEVCAPS", "The MIDI app sent a MODM_GETDEVCAPS request to the driver.");

		if (!LoadedOnce) {
			OpenRegistryKey(Configuration, L"Software\\OmniMIDI\\Configuration", FALSE);
			RegQueryValueEx(Configuration.Address, L"DebugMode", NULL, &dwType, (LPBYTE)&ManagedSettings.DebugMode, &dwSize);
			RegQueryValueEx(Configuration.Address, L"DisableCookedPlayer", NULL, &dwType, (LPBYTE)&ManagedSettings.DisableCookedPlayer, &dwSize);

			// If the debug mode is enabled, and the process isn't banned, create the debug log
			if (ManagedSettings.DebugMode && BlackListSystem())
				CreateConsole();

			StreamCapable = !(ManagedSettings.DisableCookedPlayer && CPBlacklisted) ? MIDICAPS_STREAM : 0;
			if (!StreamCapable)
				PrintMessageToDebugLog("MODM_GETDEVCAPS", "Either the app is blacklisted, or the user requested to disable CookedPlayer globally.");

			LoadedOnce = TRUE;
		}

		PrintMessageToDebugLog("MODM_GETDEVCAPS", "Sharing MIDI device caps with application...");

		// Prepare the caps item
		switch (capsSize) {
		case (sizeof(MIDIOUTCAPSA)):
		{
			if (capsPtr == NULL || capsSize != sizeof(MIDIOUTCAPSA))
				return MMSYSERR_INVALPARAM;

			MIDIOUTCAPSA MIDICaps;
			
			strcpy_s(MIDICaps.szPname, sizeof(MIDICaps.szPname), "OmniMIDI\0");
			MIDICaps.dwSupport = StreamCapable | MIDICAPS_VOLUME;
			MIDICaps.wChannelMask = 0xFFFF;
			MIDICaps.wMid = 0xFFFF;
			MIDICaps.wPid = 0x000A;
			MIDICaps.wTechnology = MOD_MIDIPORT;
			MIDICaps.wNotes = 0;
			MIDICaps.wVoices = 0;
			MIDICaps.vDriverVersion = MAKEWORD(6, 0);

			memcpy((LPMIDIOUTCAPSA)capsPtr, &MIDICaps, min(capsSize, sizeof(MIDICaps)));
			PrintMessageToDebugLog("MODM_GETDEVCAPS (ANSI, Type 1)", "Done sharing MIDI device caps.");

			break;
		}
		case (sizeof(MIDIOUTCAPSW)):
		{
			if (capsPtr == NULL || capsSize != sizeof(MIDIOUTCAPSW))
				return MMSYSERR_INVALPARAM;

			MIDIOUTCAPSW MIDICaps;

			wcscpy_s(MIDICaps.szPname, sizeof(MIDICaps.szPname), L"OmniMIDI\0");
			MIDICaps.dwSupport = StreamCapable | MIDICAPS_VOLUME;
			MIDICaps.wChannelMask = 0xFFFF;
			MIDICaps.wMid = 0xFFFF;
			MIDICaps.wPid = 0x000A;
			MIDICaps.wTechnology = MOD_MIDIPORT;
			MIDICaps.wNotes = 0;
			MIDICaps.wVoices = 0;
			MIDICaps.vDriverVersion = MAKEWORD(6, 0);

			memcpy((LPMIDIOUTCAPSW)capsPtr, &MIDICaps, min(capsSize, sizeof(MIDICaps)));
			PrintMessageToDebugLog("MODM_GETDEVCAPS (Unicode, Type 1)", "Done sharing MIDI device caps.");

			break;
		}
		case (sizeof(MIDIOUTCAPS2A)):
		{
			if (capsPtr == NULL || capsSize != sizeof(MIDIOUTCAPS2A))
				return MMSYSERR_INVALPARAM;

			MIDIOUTCAPS2A MIDICaps;

			strcpy_s(MIDICaps.szPname, sizeof(MIDICaps.szPname), "OmniMIDI\0");
			MIDICaps.ManufacturerGuid = OMCLSID;
			MIDICaps.NameGuid = OMCLSID;
			MIDICaps.ProductGuid = OMCLSID;
			MIDICaps.dwSupport = StreamCapable | MIDICAPS_VOLUME;
			MIDICaps.wChannelMask = 0xFFFF;
			MIDICaps.wMid = 0xFFFF;
			MIDICaps.wPid = 0x000A;
			MIDICaps.wTechnology = MOD_MIDIPORT;
			MIDICaps.wNotes = 0;
			MIDICaps.wVoices = 0;
			MIDICaps.vDriverVersion = MAKEWORD(6, 0);

			memcpy((LPMIDIOUTCAPS2A)capsPtr, &MIDICaps, min(capsSize, sizeof(MIDICaps)));
			PrintMessageToDebugLog("MODM_GETDEVCAPS (ANSI, Type 2)", "Done sharing MIDI device caps.");

			break;
		}
		case (sizeof(MIDIOUTCAPS2W)):
		{
			if (capsPtr == NULL || capsSize != sizeof(MIDIOUTCAPS2W))
				return MMSYSERR_INVALPARAM;

			MIDIOUTCAPS2W MIDICaps;

			wcscpy_s(MIDICaps.szPname, sizeof(MIDICaps.szPname), L"OmniMIDI\0");
			MIDICaps.ManufacturerGuid = OMCLSID;
			MIDICaps.NameGuid = OMCLSID;
			MIDICaps.ProductGuid = OMCLSID;
			MIDICaps.dwSupport = StreamCapable | MIDICAPS_VOLUME;
			MIDICaps.wChannelMask = 0xFFFF;
			MIDICaps.wMid = 0xFFFF;
			MIDICaps.wPid = 0x000A;
			MIDICaps.wTechnology = MOD_MIDIPORT;
			MIDICaps.wNotes = 0;
			MIDICaps.wVoices = 0;
			MIDICaps.vDriverVersion = MAKEWORD(6, 0);

			memcpy((LPMIDIOUTCAPS2W)capsPtr, &MIDICaps, min(capsSize, sizeof(MIDICaps)));
			PrintMessageToDebugLog("MODM_GETDEVCAPS (Unicode, Type 2)", "Done sharing MIDI device caps.");

			break;
		}
		default:
		{
			PrintMessageToDebugLog("MODM_GETDEVCAPS (Invalid)", "The MIDI app passed a caps pointer, but the pointer didn't match with anything.");
			return MMSYSERR_INVALPARAM;
		}
		}

		return MMSYSERR_NOERROR;
	}
	catch (...) {
		return MMSYSERR_NOTENABLED;
	}
}

MMRESULT DequeueMIDIHDRs()
{
	if (OMCookedPlayer == nullptr)
		return DebugResult("DequeueMIDIHDRs", MMSYSERR_INVALPARAM, "dwUser is not valid.");

	for (LPMIDIHDR hdr = OMCookedPlayer->MIDIHeaderQueue; hdr; hdr = hdr->lpNext)
	{
		LockForWriting(&OMCookedPlayer->Lock);
		PrintMessageToDebugLog("MODM_RESET", "Marking buffer as done and not in queue anymore...");
		hdr->dwFlags &= ~MHDR_INQUEUE;
		hdr->dwFlags |= MHDR_DONE;
		UnlockForWriting(&OMCookedPlayer->Lock);

		DoCallback(MOM_DONE, (DWORD_PTR)hdr, 0);
	}

	return MMSYSERR_NOERROR;
}

MMRESULT modMessage(UINT uDeviceID, UINT uMsg, DWORD_PTR dwUser, DWORD_PTR dwParam1, DWORD_PTR dwParam2)
{
	static BOOL PreventInit = FALSE;

	// Return value
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
	case MODM_LONGDATA:
		// Pass it to a KDMAPI function
		RetVal = SendDirectLongData((MIDIHDR*)dwParam1);

		// Tell the app that the buffer has been played
		DoCallback(MOM_DONE, dwParam1, 0);
		// if (CustomCallback) CustomCallback((HMIDIOUT)OMMOD.hMidi, MM_MOM_DONE, WMMCI, dwParam1, 0);

		return RetVal;
	case MODM_STRMDATA:
	{
		MIDIHDR* MIDIHeader = (MIDIHDR*)dwParam1;
		DWORD HeaderLength = (DWORD)dwParam2;

		if (!bass_initialized || OMCookedPlayer == nullptr)
			return DebugResult("MODM_STRMDATA", MIDIERR_NOTREADY, "You can't call midiStreamData with a normal MIDI stream, or the driver isn't ready.");

		if (HeaderLength < offsetof(MIDIHDR, dwOffset) ||
			!MIDIHeader || !MIDIHeader->lpData ||
			MIDIHeader->dwBufferLength < MIDIHeader->dwBytesRecorded ||
			MIDIHeader->dwBytesRecorded % 4)
		{
			return DebugResult("MODM_STRMDATA", MMSYSERR_INVALPARAM, "The buffer doesn't exist, hasn't been allocated or is not valid.");
		}
		
		if (!(MIDIHeader->dwFlags & MHDR_PREPARED))
			return DebugResult("MODM_STRMDATA", MIDIERR_UNPREPARED, "The buffer is not prepared.");

		if (!(MIDIHeader->dwFlags & MHDR_DONE))
		{
			if (MIDIHeader->dwFlags & MHDR_INQUEUE)
				return DebugResult("MODM_STRMDATA", MIDIERR_STILLPLAYING, "The buffer is still being played.");
		}

		PrintMessageToDebugLog("MODM_STRMDATA", "Locking for writing...");
		LockForWriting(&OMCookedPlayer->Lock);

		PrintMessageToDebugLog("MODM_STRMDATA", "Copying pointer of buffer...");

		PrintMIDIHDRToDebugLog("MODM_STRMDATA", MIDIHeader);

		MIDIHeader->dwFlags &= ~MHDR_DONE;
		MIDIHeader->dwFlags |= MHDR_INQUEUE;
		MIDIHeader->lpNext = 0;
		MIDIHeader->dwOffset = 0;
		if (OMCookedPlayer->MIDIHeaderQueue)
		{
			PrintMessageToDebugLog("MODM_STRMDATA", "Another buffer is already present. Adding it to queue...");
			MIDIHDR* PMIDIHeader = OMCookedPlayer->MIDIHeaderQueue;
			PrintMessageToDebugLog("MODM_STRMDATA", "nig");

			if (PMIDIHeader == MIDIHeader) {
				PrintMessageToDebugLog("MODM_STRMDATA", "Unlocking...");
				UnlockForWriting(&OMCookedPlayer->Lock);
				return MIDIERR_STILLPLAYING;
			}

			while (PMIDIHeader->lpNext)
			{
				PMIDIHeader = PMIDIHeader->lpNext;
				if (PMIDIHeader == MIDIHeader)
				{
					PrintMessageToDebugLog("MODM_STRMDATA", "Unlocking...");
					UnlockForWriting(&OMCookedPlayer->Lock);
					return MIDIERR_STILLPLAYING;
				}
			}

			PMIDIHeader->lpNext = MIDIHeader;
		}
		else OMCookedPlayer->MIDIHeaderQueue = MIDIHeader;
		PrintMessageToDebugLog("MODM_STRMDATA", "Copied.");

		PrintMessageToDebugLog("MODM_STRMDATA", "Unlocking...");
		UnlockForWriting(&OMCookedPlayer->Lock);

		PrintMessageToDebugLog("MODM_STRMDATA", "All done!");
		return MMSYSERR_NOERROR;
	}
	case MODM_PROPERTIES:
	{
		MIDIPROPTIMEDIV* MPropTimeDiv = (MIDIPROPTIMEDIV*)dwParam1;
		MIDIPROPTEMPO* MPropTempo = (MIDIPROPTEMPO*)dwParam1;
		DWORD MPropFlags = (DWORD)dwParam2;

		if (!bass_initialized || OMCookedPlayer == nullptr)
			return DebugResult("MODM_PROPERTIES", MIDIERR_NOTREADY, "You can't call midiStreamProperties with a normal MIDI stream, or the driver isn't ready.");

		if (!(MPropFlags & (MIDIPROP_GET | MIDIPROP_SET)))
			return DebugResult("MODM_PROPERTIES", MMSYSERR_INVALPARAM, "The MIDI application is confused, and didn't specify if it wanted to get the properties or set them.");

		if (MPropFlags & MIDIPROP_TEMPO)
		{
			if (MPropTempo->cbStruct != sizeof(MIDIPROPTEMPO)) {
				return DebugResult("MODM_PROPERTIES", MMSYSERR_INVALPARAM, "Invalid pointer to MIDIPROPTEMPO struct.");
			}
			else if (MPropFlags & MIDIPROP_SET) {
				PrintMessageToDebugLog("MODM_PROPERTIES", "CookedPlayer's tempo set to received value.");
				OMCookedPlayer->Tempo = MPropTempo->dwTempo;
				PrintStreamValueToDebugLog("MODM_PROPERTIES", "Received Tempo", OMCookedPlayer->Tempo);
				OMCookedPlayer->TempoMulti = ((OMCookedPlayer->Tempo * 10) / OMCookedPlayer->TimeDiv);
				PrintStreamValueToDebugLog("MODM_PROPERTIES", "New TempoMulti", OMCookedPlayer->TempoMulti);
			}
			else if (MPropFlags & MIDIPROP_GET) {
				PrintMessageToDebugLog("MODM_PROPERTIES", "CookedPlayer's tempo sent to MIDI application.");
				MPropTempo->dwTempo = OMCookedPlayer->Tempo;
			}
		}
		else if (MPropFlags & MIDIPROP_TIMEDIV) {

			if (MPropTimeDiv->cbStruct != sizeof(MIDIPROPTIMEDIV)) {
				return DebugResult("MODM_PROPERTIES", MMSYSERR_INVALPARAM, "Invalid pointer to MIDIPROPTIMEDIV struct.");
			}
			else if (MPropFlags & MIDIPROP_SET) {
				PrintMessageToDebugLog("MODM_PROPERTIES", "CookedPlayer's time division set to received value.");
				OMCookedPlayer->TimeDiv = MPropTimeDiv->dwTimeDiv;
				PrintStreamValueToDebugLog("MODM_PROPERTIES", "Received TimeDiv", OMCookedPlayer->TimeDiv);
				OMCookedPlayer->TempoMulti = ((OMCookedPlayer->Tempo * 10) / OMCookedPlayer->TimeDiv);
				PrintStreamValueToDebugLog("MODM_PROPERTIES", "New TempoMulti", OMCookedPlayer->TempoMulti);
			}
			else if (MPropFlags & MIDIPROP_GET) {
				PrintMessageToDebugLog("MODM_PROPERTIES", "CookedPlayer's time division sent to MIDI application.");
				MPropTimeDiv->dwTimeDiv = OMCookedPlayer->TimeDiv;
			}
		}
		else return DebugResult("MODM_PROPERTIES", MMSYSERR_INVALPARAM, "Invalid properties.");

		return MMSYSERR_NOERROR;
	}
	case MODM_GETPOS:
	{
		MMTIME* MMTime = (MMTIME*)dwParam1;

		if (!bass_initialized || OMCookedPlayer == nullptr)
			return DebugResult("MODM_GETPOS", MIDIERR_NOTREADY, "You can't call midiStreamPosition with a normal MIDI stream, or the driver isn't ready.");

		if (!dwParam1 || !dwParam2)
			return DebugResult("MODM_GETPOS", MMSYSERR_INVALPARAM, "Invalid parameters.");

		PrintMessageToDebugLog("MODM_GETPOS", "The app wants to know the current position of the stream.");

		RESET:
		switch (MMTime->wType) {
		case TIME_BYTES:
			PrintMessageToDebugLog("TIME_BYTES", "The app wanted it in bytes.");
			MMTime->u.cb = OMCookedPlayer->ByteAccumulator;
			break;
		case TIME_MS:
			PrintMessageToDebugLog("TIME_MS", "The app wanted it in milliseconds.");
			MMTime->u.ms = OMCookedPlayer->TimeAccumulator / 10000;
			break;
		case TIME_TICKS:
			PrintMessageToDebugLog("TIME_TICKS", "The app wanted it in ticks.");
			MMTime->u.ticks = OMCookedPlayer->TickAccumulator;
			break;
		default:
			PrintMessageToDebugLog("TIME_UNK", "Unrecognized wType. Parsing in the default format of ticks.");
			MMTime->wType = TIME_TICKS;
			goto RESET;
			break;
		}

		PrintMessageToDebugLog("MODM_GETPOS", "The app now knows the position.");
		return MMSYSERR_NOERROR;
	}
	case MODM_RESTART:
	{
		if (!bass_initialized || OMCookedPlayer == nullptr)
			return DebugResult("MODM_RESTART", MMSYSERR_NOTENABLED, "You can't call midiStreamRestart with a normal MIDI stream, or the driver isn't ready.");

		if (OMCookedPlayer->Paused) {
			OMCookedPlayer->Paused = FALSE;
			PrintMessageToDebugLog("MODM_RESTART", "CookedPlayer is now playing.");
		}

		return MMSYSERR_NOERROR;
	}
	case MODM_PAUSE:
	{
		if (!bass_initialized || OMCookedPlayer == nullptr)
			return DebugResult("MODM_PAUSE", MMSYSERR_NOTENABLED, "You can't call midiStreamPause with a normal MIDI stream, or the driver isn't ready.");

		if (!OMCookedPlayer->Paused) {
			OMCookedPlayer->Paused = TRUE;
			ResetSynth(FALSE, FALSE);
			PrintMessageToDebugLog("MODM_PAUSE", "CookedPlayer is now paused.");
		}

		return MMSYSERR_NOERROR;
	}
	case MODM_STOP:
	{
		if (!bass_initialized || OMCookedPlayer == nullptr)
			return DebugResult("MODM_STOP", MIDIERR_NOTREADY, "You can't call midiStreamStop with a normal MIDI stream, or the driver isn't ready.");

		PrintMessageToDebugLog("MODM_STOP", "The app requested OmniMIDI to stop CookedPlayer.");
		OMCookedPlayer->Paused = TRUE;

		ResetSynth(FALSE, TRUE);
		RetVal = DequeueMIDIHDRs();

		if (!RetVal) PrintMessageToDebugLog("MODM_STOP", "CookedPlayer is now stopped.");
		return RetVal;
	}
	case MODM_RESET:
		// Stop all the current active voices
		ResetSynth(FALSE, OMCookedMode ? TRUE : FALSE);

		PrintMessageToDebugLog("MODM_RESET", (OMCookedPlayer != nullptr ? "The app requested OmniMIDI to reset CookedPlayer." : "The app sent a reset command."));
		return (OMCookedMode ? DequeueMIDIHDRs() : MMSYSERR_NOERROR);
	case MODM_PREPARE:
	{
		MIDIHDR* MIDIHeader = (MIDIHDR*)dwParam1;

		// Pass it to a KDMAPI function
		return PrepareLongData(MIDIHeader);
	}
	case MODM_UNPREPARE:
	{
		MIDIHDR* MIDIHeader = (MIDIHDR*)dwParam1;

		// Pass it to a KDMAPI function
		return UnprepareLongData(MIDIHeader);
	}
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
		// SynthVolume = (int)(10000 + ((*(LONG*)dwParam1) - 65535) * (double)(0 - 10000) / (0 - 65535));
		return MMSYSERR_NOTSUPPORTED;
	}
	case MODM_OPEN:
	{
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
			LPMIDIOPENDESC OMMPD = reinterpret_cast<MIDIOPENDESC*>(dwParam1);
			PrintMIDIOPENDESCToDebugLog("MODM_OPEN", OMMPD, dwUser, (DWORD)dwParam2);

			if (!InitializeCallbackFeatures(
				OMMPD->hMidi,
				OMMPD->dwCallback,
				OMMPD->dwInstance,
				NULL,
				(DWORD)dwParam2))
			{
				PrintMessageToDebugLog("MODM_OPEN", "InitializeCallbackFeatures failed. Unable to complete MODM_OPEN.");
				return MMSYSERR_INVALPARAM;
			}

			// Enable handler if required
			EnableBuiltInHandler("MODM_OPEN");

			// Open the driver
			PrintMessageToDebugLog("MODM_OPEN", "Initializing driver...");
			if (!DoStartClient()) {
				PrintMessageToDebugLog("MODM_OPEN", "The driver failed to initialize.");
				return MMSYSERR_ERROR;
			}

			// Tell the app that the driver is ready
			PrintMessageToDebugLog("MODM_OPEN", "Sending callback data to app. if needed...");
			DoCallback(MOM_OPEN, 0, 0);

			PrintMessageToDebugLog("MODM_OPEN", "Everything is fine.");
			PreventInit = FALSE;

			return MMSYSERR_NOERROR;
		}

		PreventInit = FALSE;
		PrintMessageToDebugLog("MODM_OPEN", "The driver has already been initialized.");

		return MMSYSERR_ALLOCATED;
	}
	case MODM_CLOSE: {
		if (PreventInit) DebugResult("MODM_CLOSE", MMSYSERR_ALLOCATED, "The driver is currently being initialized or closed. Wait before closing it again!");

		if (!AlreadyInitializedViaKDMAPI) {
			// Prevent the app from calling MODM_CLOSE again...
			PreventInit = TRUE;

			// Prevent BASS from reinitializing itself
			block_bassinit = TRUE;

			PrintMessageToDebugLog("MODM_CLOSE", "The app requested the driver to terminate its audio stream.");
			ResetSynth(TRUE, TRUE);

			if (bass_initialized) {
				PrintMessageToDebugLog("MODM_CLOSE", "Terminating driver...");
				KillOldCookedPlayer();
				DoStopClient();
				DisableBuiltInHandler("MODM_CLOSE");
			}

			// OK now it's fine
			block_bassinit = FALSE;

			DoCallback(MOM_CLOSE, 0, 0);

			PrintMessageToDebugLog("MODM_CLOSE", "Everything is fine.");
		}
		else PrintMessageToDebugLog("MODM_CLOSE", "The driver is already in use via KDMAPI. Cannot terminate it!");

		PreventInit = FALSE;
		return DebugResult("MODM_CLOSE", MMSYSERR_NOERROR, "The driver has been stopped.");
	}
	case MODM_CACHEPATCHES:
	case MODM_CACHEDRUMPATCHES:
	case DRV_QUERYDEVICEINTERFACESIZE:
	case DRV_QUERYDEVICEINTERFACE:
		return MMSYSERR_NOERROR;
	default: {
		// Unrecognized uMsg
		char* Msg = new char[NTFS_MAX_PATH];
		sprintf(Msg,
			"The application sent an unknown message! ID: 0x%08x - dwUser: 0x%08x - dwParam1: 0x%08x - dwParam2: 0x%08x",
			uMsg, dwUser, dwParam1, dwParam2);
		RetVal = DebugResult("modMessage", MMSYSERR_ERROR, Msg);
		delete[] Msg;
		return RetVal;
	}
	}
}
