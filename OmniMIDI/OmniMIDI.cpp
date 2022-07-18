/*
OmniMIDI, a fork of BASSMIDI Driver

Thank you Kode54 for allowing me to fork your awesome driver.
*/

#pragma once

typedef unsigned __int64 QWORD;
typedef long NTSTATUS;

#define _SILENCE_ALL_CXX17_DEPRECATION_WARNINGS
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
#include <fstream>
#include <iostream>
#include <codecvt>
#include <string>
#include <future>
#include <mmddk.h>
#include <shlobj.h>
#include <sstream>
#include <stdio.h>
#include <vector>
#include <Dbghelp.h>
#include <assert.h>
#include <strsafe.h>
#include <VersionHelpers.h>
#include "NTDLLDummy.h"
#include "Resource.h"
#include "OmniMIDI.h"
#include "sound_out.h"

// BASS headers
#include <bass.h>
#include <bass_fx.h>
#include <bassmidi.h>
#include <bassenc.h>
#include <bassasio.h>
#include <basswasapi.h>
#include <bass_vst.h>

// Important
#include "LockSystem.h"
#include "Values.h"
#include "Funcs.h"
#include "Debug.h"
#include "WinMMWRP/WinMM.h"

// Shakra backport
#include "ShakraRegSystem.h"

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
		BASS_MIDI_StreamEvents = DummyBMSEs;

		if (!NT_SUCCESS(NtQuerySystemTime(&TickStart))) {
			OutputDebugStringA("Failed to parse starting tick through NtQuerySystemTime! OmniMIDI will not load.");
			return FALSE;
		}

		if (!InitializeWinMM()) {
			OutputDebugStringA("Failed to initialize WinMM.");
			return FALSE;
		}

		return TRUE;
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
		if (GetFolderPath(FOLDERID_SystemX86, CSIDL_SYSTEMX86, configuratorapp, sizeof(configuratorapp)))
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

	try {
		PrintMessageToDebugLog("MODM_GETDEVCAPS", "The MIDI app sent a MODM_GETDEVCAPS request to the driver.");

		if (!LoadedOnce) {
			OpenRegistryKey(Configuration, L"Software\\OmniMIDI\\Configuration", FALSE);
			RegQueryValueEx(Configuration.Address, L"DebugMode", NULL, &dwType, (LPBYTE)&ManagedSettings.DebugMode, &dwSize);

			// If the debug mode is enabled, and the process isn't banned, create the debug log
			if (ManagedSettings.DebugMode && BlackListSystem())
				CreateConsole();

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
			MIDICaps.dwSupport = MIDICAPS_CACHE;
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
			MIDICaps.dwSupport = MIDICAPS_CACHE;
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
			MIDICaps.dwSupport = MIDICAPS_CACHE;
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
			MIDICaps.dwSupport = MIDICAPS_CACHE;
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

extern "C" MMRESULT WINAPI modMessage(UINT uDeviceID, UINT uMsg, DWORD_PTR dwUser, DWORD_PTR dwParam1, DWORD_PTR dwParam2)
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
		_PrsData(dwParam1);
		return MMSYSERR_NOERROR;
	case MODM_LONGDATA:
		// Pass it to a KDMAPI function
		RetVal = SendDirectLongData((MIDIHDR*)dwParam1, dwParam2);

		// Tell the app that the buffer has been played
		DoCallback(MOM_DONE, dwParam1, 0);
		// if (CustomCallback) CustomCallback((HMIDIOUT)OMMOD.hMidi, MM_MOM_DONE, WMMCI, dwParam1, 0);

		SendLongMIDIFeedback((MIDIHDR*)dwParam1, dwParam2);

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
				PrintVarToDebugLog("MODM_PROPERTIES", "Received Tempo", &OMCookedPlayer->Tempo, PRINT_UINT32);
				OMCookedPlayer->TempoMulti = ((OMCookedPlayer->Tempo * 10) / OMCookedPlayer->TimeDiv);
				PrintVarToDebugLog("MODM_PROPERTIES", "New TempoMulti", &OMCookedPlayer->TempoMulti, PRINT_UINT32); 
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
				PrintVarToDebugLog("MODM_PROPERTIES", "Received TimeDiv", &OMCookedPlayer->TimeDiv, PRINT_UINT32);
				OMCookedPlayer->TempoMulti = ((OMCookedPlayer->Tempo * 10) / OMCookedPlayer->TimeDiv);
				PrintVarToDebugLog("MODM_PROPERTIES", "New TempoMulti", &OMCookedPlayer->TempoMulti, PRINT_UINT32);
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
		return PrepareLongData(MIDIHeader, dwParam2);
	}
	case MODM_UNPREPARE:
	{
		MIDIHDR* MIDIHeader = (MIDIHDR*)dwParam1;

		// Pass it to a KDMAPI function
		return UnprepareLongData(MIDIHeader, dwParam2);
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

			if (DriverInitStatus) {
				// Prevent BASS from reinitializing itself
				block_bassinit = TRUE;

				PrintMessageToDebugLog("MODM_CLOSE", "The app requested the driver to terminate its audio stream.");
				ResetSynth(TRUE, TRUE);

				PrintMessageToDebugLog("MODM_CLOSE", "Terminating driver...");
				KillOldCookedPlayer();
				DoStopClient();
				DisableBuiltInHandler("MODM_CLOSE");

				// OK now it's fine
				block_bassinit = FALSE;

				DoCallback(MOM_CLOSE, 0, 0);
			}

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

// test
UINT WINAPI KDMAPI_midiOutGetNumDevs(void) {
	return 1;
}

MMRESULT WINAPI KDMAPI_midiOutGetDevCapsW(UINT_PTR uDeviceID, LPMIDIOUTCAPSW lpCaps, UINT uSize) {
	if (lpCaps == NULL || uSize != sizeof(MIDIOUTCAPSW)) return MMSYSERR_INVALPARAM;

	return GiveOmniMIDICaps(lpCaps, uSize);
}

MMRESULT WINAPI KDMAPI_midiOutGetDevCapsA(UINT_PTR uDeviceID, LPMIDIOUTCAPSA lpCaps, UINT uSize) {
	// Return the output device, but ASCII/Multibyte
	if (lpCaps == NULL || uSize != sizeof(MIDIOUTCAPSA)) return MMSYSERR_INVALPARAM;

	// Parse info in Unicode
	MIDIOUTCAPSW myCapsW;
	UINT ret;

#ifdef _DAWRELEASE
	ret = KDMAPI_midiOutGetDevCapsW(uDeviceID, &myCapsW, sizeof(myCapsW));
#else
	ret = GiveOmniMIDICaps(&myCapsW, uSize);
#endif

	// Translate them to ASCII/Multibyte
	if (ret == MMSYSERR_NOERROR) {
		MIDIOUTCAPSA myCapsA;
		myCapsA.wMid = myCapsW.wMid;
		myCapsA.wPid = myCapsW.wPid;
		myCapsA.vDriverVersion = myCapsW.vDriverVersion;
		wcstombs(myCapsA.szPname, myCapsW.szPname, wcslen(myCapsW.szPname) + 1);
		myCapsA.wTechnology = myCapsW.wTechnology;
		myCapsA.wVoices = myCapsW.wVoices;
		myCapsA.wNotes = myCapsW.wNotes;
		myCapsA.wChannelMask = myCapsW.wChannelMask;
		myCapsA.dwSupport = myCapsW.dwSupport;
		memcpy(lpCaps, &myCapsA, min(uSize, sizeof(myCapsA)));
	}
	return ret;
}

MMRESULT WINAPI KDMAPI_midiOutShortMsg(HMIDIOUT hMidiOut, DWORD dwMsg) {
	SendDirectData(dwMsg);
	return MMSYSERR_NOERROR;
}

MMRESULT WINAPI KDMAPI_midiOutOpen(LPHMIDIOUT lphmo, UINT uDeviceID, DWORD_PTR dwCallback, DWORD_PTR dwCallbackInstance, DWORD dwFlags) {
	if (!AlreadyInitializedViaKDMAPI) {
		// Initialize a dummy out device
		*lphmo = (HMIDIOUT)OMDummy;

		// Setup the Callback (If there's one) - NEEDED FOR VANBASCO!
		if (!InitializeCallbackFeatures((HMIDI)(*lphmo), dwCallback, dwCallbackInstance, OMUser, dwFlags))
		{
			MessageBox(NULL, L"ICF failed!", L"KDMAPI ERROR", MB_SYSTEMMODAL | MB_ICONERROR);
			return MMSYSERR_INVALPARAM;
		}

		// Close any stream, just to be safe
		TerminateKDMAPIStream();

		// Initialize MIDI out
		if (!InitializeKDMAPIStream())
			return MMSYSERR_ALLOCATED;

		DriverSettings(0xFFFFF, NULL, NULL, NULL);

		RunCallbackFunction(MM_MOM_OPEN, 0, 0);

		AlreadyInitializedViaKDMAPI = TRUE;
		return MMSYSERR_NOERROR;
	}

	return MMSYSERR_ALLOCATED;
}

MMRESULT WINAPI KDMAPI_midiOutClose(HMIDIOUT hMidiOut) {
	// Close OM
	if (AlreadyInitializedViaKDMAPI) {
		if (!TerminateKDMAPIStream()) return MMSYSERR_NOMEM;

		DriverSettings(0xFFFFE, NULL, NULL, NULL);

		RunCallbackFunction(MM_MOM_CLOSE, 0, 0);

		hMidiOut = (HMIDIOUT)0;
		AlreadyInitializedViaKDMAPI = FALSE;
	}

	return MMSYSERR_NOERROR;
}

MMRESULT WINAPI KDMAPI_midiOutReset(HMIDIOUT hMidiOut) {
	ResetKDMAPIStream();
	return MMSYSERR_NOERROR;
}

MMRESULT WINAPI KDMAPI_midiOutPrepareHeader(HMIDIOUT hMidiOut, MIDIHDR* lpMidiOutHdr, UINT uSize) {
	return PrepareLongData(lpMidiOutHdr, uSize);
}

MMRESULT WINAPI KDMAPI_midiOutUnprepareHeader(HMIDIOUT hMidiOut, MIDIHDR* lpMidiOutHdr, UINT uSize) {
	return UnprepareLongData(lpMidiOutHdr, uSize);
}

MMRESULT WINAPI KDMAPI_midiOutLongMsg(HMIDIOUT hMidiOut, MIDIHDR* lpMidiOutHdr, UINT uSize) {
	// Forward the buffer to KDMAPI
	MMRESULT Ret = SendDirectLongData(lpMidiOutHdr, uSize);

	// Inform the app that the driver successfully received the long message (Required for vanBasco to work), and return the MMRESULT
	RunCallbackFunction(MM_MOM_DONE, (DWORD_PTR)hMidiOut, (DWORD_PTR)lpMidiOutHdr);

	return Ret;
}

MMRESULT WINAPI KDMAPI_midiOutCachePatches(HMIDIOUT hMidiOut, UINT wPatch, LPWORD lpPatchArray, UINT wFlags) {
	// Dummy, OmniMIDI uses SoundFonts
	return MMSYSERR_NOERROR;
}

MMRESULT WINAPI KDMAPI_midiOutCacheDrumPatches(HMIDIOUT hMidiOut, UINT wPatch, LPWORD lpKeyArray, UINT wFlags) {
	// Dummy, OmniMIDI uses SoundFonts
	return MMSYSERR_NOERROR;
}

MMRESULT WINAPI KDMAPI_midiOutMessage(HMIDIOUT hMidiOut, UINT uMsg, DWORD_PTR dw1, DWORD_PTR dw2) {
	// Dummy, OmniMIDI uses SoundFonts
	return MMSYSERR_NOERROR;
}

MMRESULT WINAPI KDMAPI_midiOutSetVolume(HMIDIOUT hMidiOut, DWORD dwVolume) {
	// Set the volume, even though it won't work lol
	return modMessage(0, MODM_SETVOLUME, OMUser, dwVolume, 0);
}

MMRESULT WINAPI KDMAPI_midiOutGetVolume(HMIDIOUT hMidiOut, LPDWORD lpdwVolume) {
	// Get the volume
	return modMessage(0, MODM_GETVOLUME, OMUser, (DWORD_PTR)lpdwVolume, 0);
}

MMRESULT WINAPI KDMAPI_midiOutGetID(HMIDIOUT hMidiOut, LPUINT puDeviceID) {
	// Dummy, device is always 0
	*puDeviceID = 0;
	return MMSYSERR_NOERROR;
}

UINT WINAPI KDMAPI_mmsystemGetVersion(void) {
	// Dummy, not needed
	return 0x0600U;
}

MMRESULT WINAPI KDMAPI_midiStreamOpen(LPHMIDISTRM lphStream, LPUINT puDeviceID, DWORD cMidi, DWORD_PTR dwCallback, DWORD_PTR dwCallbackInstance, DWORD fdwOpen) {
	MMRESULT retval = MMSYSERR_NOERROR;

	if (cMidi != 1 || lphStream == NULL || puDeviceID == NULL) return MMSYSERR_INVALPARAM;

	if (!AlreadyInitializedViaKDMAPI) {
		// Setup the Callback
		if (!InitializeCallbackFeatures((HMIDI)(*lphStream), dwCallback, dwCallbackInstance, OMUser, fdwOpen | 0x00000002L))
		{
			MessageBox(NULL, L"ICF failed!", L"KDMAPI ERROR", MB_SYSTEMMODAL | MB_ICONERROR);
			return MMSYSERR_INVALPARAM;
		}

		// Close any stream, just to be safe
		TerminateKDMAPIStream();

		// Initialize a dummy out device
		*lphStream = (HMIDISTRM)OMDummy;

		// Initialize MIDI out
		if (!InitializeKDMAPIStream())
			return MMSYSERR_ALLOCATED;

		DriverSettings(0xFFFFF, NULL, NULL, NULL);

		RunCallbackFunction(MM_MOM_OPEN, (DWORD_PTR)*lphStream, 0);

		AlreadyInitializedViaKDMAPI = TRUE;
		return MMSYSERR_NOERROR;
	}

	return MMSYSERR_ALLOCATED;
}

MMRESULT WINAPI KDMAPI_midiStreamClose(HMIDISTRM hStream) {
	// Terminate CookedPlayer, free up hStream and return 0
	if (AlreadyInitializedViaKDMAPI) {
		if (TerminateKDMAPIStream())
			RunCallbackFunction(MM_MOM_CLOSE, (DWORD_PTR)hStream, 0);

		AlreadyInitializedViaKDMAPI = FALSE;
		return MMSYSERR_NOERROR;
	}

	return MMSYSERR_INVALHANDLE;
}

MMRESULT WINAPI KDMAPI_midiStreamOut(HMIDISTRM hStream, LPMIDIHDR lpMidiOutHdr, UINT uSize) {
	// Give stream data to CookedPlayer
	return modMessage(0, MODM_STRMDATA, OMUser, (DWORD_PTR)lpMidiOutHdr, uSize);
}

MMRESULT WINAPI KDMAPI_midiStreamPause(HMIDISTRM hStream) {
	// Pause CookedPlayer
	return modMessage(0, MODM_PAUSE, OMUser, 0, 0);
}

MMRESULT WINAPI KDMAPI_midiStreamRestart(HMIDISTRM hStream) {
	// Play CookedPlayer
	return modMessage(0, MODM_RESTART, OMUser, 0, 0);
}

MMRESULT WINAPI KDMAPI_midiStreamStop(HMIDISTRM hStream) {
	// Stop CookedPlayer
	return modMessage(0, MODM_STOP, OMUser, 0, 0);
}

MMRESULT WINAPI KDMAPI_midiStreamProperty(HMIDISTRM hStream, LPBYTE lppropdata, DWORD dwProperty) {
	// Pass the prop. data to modMessage
	return modMessage(0, MODM_PROPERTIES, OMUser, (DWORD_PTR)lppropdata, dwProperty);
}

MMRESULT WINAPI KDMAPI_midiStreamPosition(HMIDISTRM hStream, LPMMTIME pmmt, UINT cbmmt) {
	// Give CookedPlayer position to MIDI app
	return modMessage(0, MODM_GETPOS, OMUser, (DWORD_PTR)pmmt, cbmmt);
}

VOID WINAPI KDMAPI_poweredByKeppy() {
	MessageBox(NULL, L"With love by Keppy.", L"Windows Multimedia Wrapper", MB_OK | MB_ICONINFORMATION | MB_SYSTEMMODAL);
}

DWORD WINAPI FINAL_timeGetTime() {
	return timeGetTime64();
}