/*
OmniMIDI, a fork of BASSMIDI Driver

Thank you Kode54 for allowing me to fork your awesome driver.
*/

#pragma once
#pragma comment(lib,"shlwapi.lib")

#if !_WIN32
#error The driver only works on 32-bit and 64-bit versions of Windows x86. ARM is not supported.
#endif

typedef unsigned __int64 QWORD;

// KDMAPI version
#define CUR_MAJOR	1
#define CUR_MINOR	47
#define CUR_BUILD	2
#define CUR_REV		8

#define STRICT
#define VC_EXTRALEAN
#define WIN32_LEAN_AND_MEAN
#define __STDC_LIMIT_MACROS
#define _CRT_SECURE_CPP_OVERLOAD_STANDARD_NAMES 1

#define BASSASIODEF(f) (WINAPI *f)
#define BASSDEF(f) (WINAPI *f)
#define BASSENCDEF(f) (WINAPI *f)	
#define BASSMIDIDEF(f) (WINAPI *f)	
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
#include "Resource.h"
#include "OmniMIDI.h"

// BASS headers
#include <bass.h>
#include <bassmidi.h>
#include <bassenc.h>
#include <bassasio.h>
// #include <basswasapi.h>

// Sleep
typedef LONG NTSTATUS;

// NTSTATUS
#define NTAPI __stdcall
// these functions have identical prototypes
typedef NTSTATUS(NTAPI* NLVM)(IN HANDLE process, IN OUT void** baseAddress, IN OUT ULONG* size, IN ULONG flags);
typedef NTSTATUS(NTAPI* NULVM)(IN HANDLE process, IN OUT void** baseAddress, IN OUT ULONG* size, IN ULONG flags);
typedef NTSTATUS(NTAPI* NDE)(BOOLEAN dwAlertable, PLARGE_INTEGER dwDelayInterval);

static NDE NtDelayExecution = 0;
static NLVM NtLockVirtualMemory = 0;
static NULVM NtUnlockVirtualMemory = 0;

// Blinx best game
static HMODULE bass = NULL, bassasio = NULL, bassenc = NULL, bassmidi = NULL;	// BASS libs handles
static HPLUGIN bassflac = NULL, bassopus = NULL, basswv = NULL;					// BASS codecs handles
#define LOADBASSASIOFUNCTION(f) *((void**)&f)=GetProcAddress(bassasio,#f)
#define LOADBASSENCFUNCTION(f) *((void**)&f)=GetProcAddress(bassenc,#f)
#define LOADBASSFUNCTION(f) *((void**)&f)=GetProcAddress(bass,#f)
#define LOADBASSMIDIFUNCTION(f) *((void**)&f)=GetProcAddress(bassmidi,#f)
// #define LOADBASSWASAPIFUNCTION(f) *((void**)&f) = GetProcAddress(basswasapi, #f)

// F**k Sleep() tbh
void NTSleep(__int64 usec) {
	LARGE_INTEGER ft;
	ft.QuadPart = usec;
	NtDelayExecution(FALSE, &ft);
}

MMRESULT DummyParseData(UINT dwMsg, DWORD_PTR dwParam1, DWORD_PTR dwParam2) {
	return MIDIERR_NOTREADY;
}

// Hyper switch
static BOOL HyperMode = 0;
static MMRESULT(*_PrsData)(UINT uMsg, DWORD_PTR dwParam1, DWORD_PTR dwParam2) = DummyParseData;
static DWORD(*_PlayBufData)(void) = 0;
static DWORD(*_PlayBufDataChk)(void) = 0;
// What does it do? It gets rid of the useless functions,
// and passes the events without checking for anything

// Predefined sleep values, useful for redundancy
#define _DBGWAIT NTSleep(-10000)											// Normal wait
#define _FWAIT NTSleep(ManagedSettings.SleepStates ? -100 : 0)				// Fast wait
#define _LWAIT NTSleep(ManagedSettings.SleepStates ? -1000 : 0)				// Slow wait
#define _VLWAIT NTSleep(-200000)											// Very slow wait
#define _CFRWAIT NTSleep(ManagedSettings.SleepStates ? -15667 : -16667)		// Cap framerate wait

// LightweightLock by Brad Wilson
#include "LwL.h"

// Variables
#include "Values.h"
#include "BASSErrors.h"
#include "Debug.h"

// OmniMIDI vital parts
#include "SoundFontLoader.h"
#include "BufferSystem.h"
#include "Settings.h"
#include "BlacklistSystem.h"
#include "DriverInit.h"
#include "KDMAPI.h"

// OmniMIDI GUID
// {62F3192B-A961-456D-ABCA-A5C95A14B9AA}
static const GUID OMCLSID = { 0x62f3192b, 0xa961, 0x456d, { 0xab, 0xca, 0xa5, 0xc9, 0x5a, 0x14, 0xb9, 0xaa } };

extern "C" BOOL APIENTRY DllMain(HANDLE hinstDLL, DWORD fdwReason, LPVOID lpvReserved)
{
	switch (fdwReason) {
	case DLL_PROCESS_ATTACH:
		if (lpvReserved != NULL) OutputDebugString(L"The driver has been dynamically loaded");

		hinst = (HINSTANCE)hinstDLL;
		NtDelayExecution = (NDE)GetProcAddress(GetModuleHandle(L"ntdll"), "NtDelayExecution");
		NtLockVirtualMemory = (NLVM)GetProcAddress(GetModuleHandle(L"ntdll"), "NtLockVirtualMemory");
		NtUnlockVirtualMemory = (NULVM)GetProcAddress(GetModuleHandle(L"ntdll"), "NtUnlockVirtualMemory");
		if (!NtDelayExecution || !NtLockVirtualMemory || !NtUnlockVirtualMemory) {
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
	case DRV_QUERYCONFIGURE:
		return DRV_OK;
	case DRV_CONFIGURE:
		TCHAR configuratorapp[MAX_PATH];
		if (SUCCEEDED(SHGetFolderPath(NULL, CSIDL_SYSTEMX86, NULL, 0, configuratorapp)))
		{
			PathAppend(configuratorapp, _T("\\OmniMIDI\\OmniMIDIConfigurator.exe"));
			ShellExecute(NULL, L"open", configuratorapp, L"/AST", NULL, SW_SHOWNORMAL);
			return DRVCNF_OK;
		}
		return DRVCNF_CANCEL;
	case DRV_LOAD:
		return DRV_OK;
	case DRV_ENABLE:
		return DRV_OK;
	case DRV_REMOVE:
		return DRV_OK;
	case DRV_FREE:
		return DRV_OK;
	case DRV_OPEN:
		OMDevice = hdrvr;
		return DRV_OK;
	case DRV_CLOSE:
		OMDevice = NULL;
		return DRV_OK;
	default:
		return DefDriverProc(dwDriverId, hdrvr, uMsg, lParam1, lParam2);
	}
}

DWORD modGetCaps(PVOID capsPtr, DWORD capsSize) {
	try {
		// Create temp caps
		static MIDIOUTCAPS2 MIDICaps = { 0 };
		
		// Initialize values
		wchar_t SysDir32[MAX_PATH];
		WORD maximumvoices = 0xFFFF;
		WORD maximumnotes = 0xFFFF;
		DWORD CapsSupport = MIDICAPS_VOLUME;
		DWORD Technology = NULL;

		WORD VID = 0x0000;
		WORD PID = 0x0000;

		OpenRegistryKey(Configuration, L"Software\\OmniMIDI\\Configuration", FALSE);
		RegQueryValueEx(Configuration.Address, L"SynthType", NULL, &dwType, (LPBYTE)&SynthType, &dwSize);
		RegQueryValueEx(Configuration.Address, L"DebugMode", NULL, &dwType, (LPBYTE)&ManagedSettings.DebugMode, &dwSize);
		RegQueryValueEx(Configuration.Address, L"VID", NULL, &dwType, (LPBYTE)&VID, &dwSize);
		RegQueryValueEx(Configuration.Address, L"PID", NULL, &dwType, (LPBYTE)&PID, &dwSize);
		RegQueryValueEx(Configuration.Address, L"SynthName", NULL, &SNType, (LPBYTE)&SynthNameW, &SNSize);

		// If the synth type ID is bigger than the size of the synth types array,
		// set it automatically to MOD_MIDIPORT
		if (SynthType >= ((sizeof(SynthNamesTypes) / sizeof(SynthNamesTypes[0]))))
			Technology = MOD_MIDIPORT;
		// Else, load the requested value
		else Technology = SynthNamesTypes[SynthType];

		// If the debug mode is enabled, and the process isn't banned, create the debug log
		RtlZeroMemory(SysDir32, sizeof(SysDir32));
		SHGetFolderPathW(NULL, CSIDL_SYSTEMX86, NULL, 0, SysDir32);

		if (ManagedSettings.DebugMode && (!BannedSystemProcess() | !BlackListSystem(SysDir32)))
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
		if (!MIDICaps.wMid) {
			wcsncpy(MIDICaps.szPname, SynthNameW, MAXPNAMELEN);
			MIDICaps.ManufacturerGuid = OMCLSID;
			MIDICaps.NameGuid = OMCLSID;
			MIDICaps.ProductGuid = OMCLSID;
			MIDICaps.dwSupport = CapsSupport;
			MIDICaps.wChannelMask = 0xffff;
			MIDICaps.wMid = VID;
			MIDICaps.wNotes = maximumnotes;
			MIDICaps.wPid = PID;
			MIDICaps.wTechnology = Technology;
			MIDICaps.wVoices = maximumvoices;
			MIDICaps.vDriverVersion = 0x0501;
			PrintMessageToDebugLog("MODM_GETDEVCAPS", "Done sharing MIDI device caps.");
		}

		// Copy the item to the app's caps
		memcpy(capsPtr, &MIDICaps, min(sizeof(MIDICaps), capsSize));
		return MMSYSERR_NOERROR;
	}
	catch (...) {
		CrashMessage("MIDICapsException");
	}
}

STDAPI_(DWORD) modMessage(UINT uDeviceID, UINT uMsg, DWORD_PTR dwUser, DWORD_PTR dwParam1, DWORD_PTR dwParam2){
	MMRESULT RetVal = MMSYSERR_NOERROR;

	switch (uMsg) {
	case MODM_DATA:
		// Parse the data lol
		return _PrsData(uMsg, dwParam1, dwParam2);
	case MODM_LONGDATA: // case MODM_STRMDATA:
		// Pass it to a KDMAPI function
		RetVal = SendDirectLongData((MIDIHDR*)dwParam1);

		// Tell the app that the buffer has been played
		DriverCallback(OMCallback, OMFlags, OMDevice, MOM_DONE, OMInstance, dwParam1, 0);
		return RetVal;
	case MODM_STRMDATA:
		/* 
		// Reference the MIDIHDR
		IIMidiHdr = (MIDIHDR*)dwParam1;

		if (!IIMidiHdr || !(IIMidiHdr->dwFlags & MHDR_ISSTRM)) return MMSYSERR_INVALPARAM;		// The buffer doesn't exist, invalid 
		if (!(IIMidiHdr->dwFlags & MHDR_PREPARED)) return MIDIERR_UNPREPARED;					// The buffer is not prepared
		if (IIMidiHdr->dwFlags & MHDR_INQUEUE) return MIDIERR_STILLPLAYING;						// The buffer is already being played

		// Mark the buffer as in queue
		IIMidiHdr->dwFlags &= ~MHDR_DONE;
		IIMidiHdr->dwFlags |= MHDR_INQUEUE;

		// Todo, I don't know how to play the stream yet.		

		// Mark the buffer as done
		IIMidiHdr->dwFlags &= ~MHDR_INQUEUE;
		IIMidiHdr->dwFlags |= MHDR_DONE;

		// Tell the app that the buffer has been played
		DriverCallback(KSCallback, KSFlags, KSDevice, MOM_DONE, KSInstance, dwParam1, 0);
		return MMSYSERR_NOERROR;
		*/

		PrintMessageToDebugLog("MODM_STRMDATA", "MIDI_IO_COOKED not supported by OmniMIDI.");
		return DebugResult(MMSYSERR_NOTSUPPORTED);
	case MODM_PREPARE:
		// Pass it to a KDMAPI function
		return PrepareLongData((MIDIHDR*)dwParam1);
	case MODM_UNPREPARE:
		// Pass it to a KDMAPI function
		return UnprepareLongData((MIDIHDR*)dwParam1);
	case MODM_GETNUMDEVS:
		// Return "1" if the process isn't blacklisted, otherwise the driver doesn't exist OwO
		return BlackListInit();
	case MODM_GETDEVCAPS:
		// Return OM's caps to the app
		return modGetCaps((PVOID)dwParam1, (DWORD)dwParam2);
	case MODM_GETVOLUME:
		// Tell the app the current output volume of the driver
		PrintMessageToDebugLog("MODM_GETVOLUME", "The app wants to know the current output volume of the driver.");
		*(LONG*)dwParam1 = (LONG)(sound_out_volume_float * 0xFFFF);
		PrintMessageToDebugLog("MODM_GETVOLUME", "The app knows the volume now.");
		return MMSYSERR_NOERROR;
	case MODM_SETVOLUME: 
		// The app isn't allowed to set the volume, everything's fine anyway
		PrintMessageToDebugLog("MODM_SETVOLUME", "Dummy, the app has no control over the driver's audio output.");
		return MMSYSERR_NOERROR;
	case MODM_RESET:
		// Stop all the current active voices
		PrintMessageToDebugLog("MODM_RESET", "The app sent a reset command.");
		ResetSynth(FALSE);
		return MMSYSERR_NOERROR;
	case MODM_STOP:
		// Not needed for OmniMIDI
		PrintMessageToDebugLog("MODM_STOP", "Dummy, MIDI_IO_COOKED not supported by OmniMIDI.");
		return MMSYSERR_NOERROR;
	case MODM_OPEN:
		// The driver doesn't support stream mode
		PrintMessageToDebugLog("MODM_OPEN", "The app requested the driver to initialize its audio stream.");

		if ((DWORD)dwParam2 & MIDI_IO_COOKED) {
			PrintMessageToDebugLog("MODM_OPEN", "MIDI_IO_COOKED not supported by OmniMIDI.");
			return DebugResult(MMSYSERR_NOTENABLED);
		}

		if (!AlreadyInitializedViaKDMAPI && !bass_initialized) {
			// Parse callback and instance
			PrintMessageToDebugLog("MODM_OPEN", "Preparing callback data (If present)...");
			OMCallback = ((MIDIOPENDESC*)dwParam1)->dwCallback;
			OMInstance = ((MIDIOPENDESC*)dwParam1)->dwInstance;
			OMFlags = HIWORD((DWORD)dwParam2);

			// Open the driver
			PrintMessageToDebugLog("MODM_OPEN", "Initializing driver...");
			DoStartClient();

			// Tell the app that the driver is ready
			ResetSynth(TRUE);
			PrintMessageToDebugLog("MODM_OPEN", "Sending callback data to app (If present)...");
			DriverCallback(OMCallback, OMFlags, OMDevice, MOM_OPEN, OMInstance, 0, 0);

			PrintMessageToDebugLog("MODM_OPEN", "Everything is fine.");
		}
		else PrintMessageToDebugLog("MODM_OPEN", "The driver has already been initialized. Cannot initialize it twice!");

		return MMSYSERR_NOERROR;
	case MODM_CLOSE:
		PrintMessageToDebugLog("MODM_CLOSE", "The app requested the driver to terminate its audio stream.");
		ResetSynth(TRUE);
		if (CloseStreamMidiOutClose && !AlreadyInitializedViaKDMAPI) {
			// The app wants us to close the driver
			// Only close the stream if the user has chosen to
			PrintMessageToDebugLog("MODM_CLOSE", "Terminating driver...");
			if (bass_initialized) DoStopClient();			
		}

		PrintMessageToDebugLog("MODM_CLOSE", "Sending callback data to app (If present)...");
		DriverCallback(OMCallback, OMFlags, OMDevice, MOM_CLOSE, OMInstance, 0, 0);

		PrintMessageToDebugLog("MODM_CLOSE", "Everything is fine.");
		return MMSYSERR_NOERROR;
	case DRV_QUERYDEVICEINTERFACESIZE:
		// Not needed for OmniMIDI
		PrintMessageToDebugLog("modMessage", "DRV_QUERYDEVICEINTERFACESIZE not supported by OmniMIDI.");
		return MMSYSERR_NOTSUPPORTED;
	case DRV_QUERYDEVICEINTERFACE:
		// Not needed for OmniMIDI
		PrintMessageToDebugLog("modMessage", "DRV_QUERYDEVICEINTERFACE not supported by OmniMIDI.");
		return MMSYSERR_NOTSUPPORTED;
	}
}