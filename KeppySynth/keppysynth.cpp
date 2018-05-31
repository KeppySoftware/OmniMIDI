/*
Keppy's Synthesizer, a fork of BASSMIDI Driver

Thank you Kode54 for allowing me to fork your awesome driver.
*/

#pragma once

#if !_WIN32
#error The driver only works on 32-bit and 64-bit versions of Windows x86. ARM is not supported.
#endif

#define STRICT
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
#define LONGMSG_MAXSIZE	65535

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
#include <windows.h>
#include "Resource.h"

// BASS headers
#include <bass.h>
#include <bassmidi.h>
#include <bassenc.h>
#include <bassasio.h>
// #include <basswasapi.h>

// Sleep
typedef LONG(NTAPI*NDE)(BOOLEAN dwAlertable, PLARGE_INTEGER dwDelayInterval);
static NDE NtDelayExecution = 0;

// Hyper switch
static DWORD HyperMode = 0;
static DWORD HyperCheckedAlready = FALSE;
static MMRESULT(*_PrsData)(UINT uMsg, DWORD_PTR dwParam1, DWORD dwParam2) = 0;
static DWORD(*_PlayBufData)(void) = 0;
static DWORD(*_PlayBufDataChk)(void) = 0;
// What does it do? It gets rid of the useless functions,
// and passes the events without checking for anything

// Blinx best game
static HMODULE bass = 0;			// bass handle
static HMODULE bassasio = 0;		// bassasio handle
static HMODULE bassenc = 0;			// bassenc handle
static HMODULE bassmidi = 0;		// bassmidi handle
// static HMODULE basswasapi = 0;	// basswasapi handle
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

#define _WAIT NTSleep(-100)													// Normal wait
#define _FWAIT NTSleep(ManagedSettings.SleepStates ? -100 : 0)				// Fast wait
#define _LWAIT NTSleep(ManagedSettings.SleepStates ? -1000 : 0)				// Slow wait
#define _VLWAIT NTSleep(-200000)											// Very slow wait
#define _CFRWAIT NTSleep(ManagedSettings.SleepStates ? -15667 : -16667)		// Cap framerate wait

// LightweightLock by Brad Wilson
#include "LwL.h"

// Variables
#include "val.h"
#include "basserr.h"
#include "dbg.h"

// Keppy's Synthesizer vital parts
#include "sfsystem.h"
#include "settings.h"
#include "bufsystem.h"
#include "bansystem.h"
#include "drvinit.h"
#include "ksdapi.h"

BOOL WINAPI DllMain(HINSTANCE hinstDLL, DWORD fdwReason, LPVOID lpvReserved)
{
	if (fdwReason == DLL_PROCESS_ATTACH){
		hinst = hinstDLL;
		NtDelayExecution = (NDE)GetProcAddress(LoadLibrary(L"ntdll"), "NtDelayExecution");
		DisableThreadLibraryCalls(hinstDLL);
	}
	else if (fdwReason == DLL_PROCESS_DETACH)
	{
		DisconnectNamedPipe(hPipe);
		DoStopClient();
	}
	return TRUE;
}

LONG_PTR DoDriverConfiguration() {
	TCHAR configuratorapp[MAX_PATH];
	if (SUCCEEDED(SHGetFolderPath(NULL, CSIDL_SYSTEMX86, NULL, 0, configuratorapp)))
	{
		PathAppend(configuratorapp, _T("\\keppysynth\\KeppySynthConfigurator.exe"));
		ShellExecute(NULL, L"open", configuratorapp, L"/AST", NULL, SW_SHOWNORMAL);
		return DRVCNF_OK;
	}
	return DRVCNF_CANCEL;
}

STDAPI_(LONG_PTR) DriverProc(DWORD_PTR dwDriverId, HDRVR hdrvr, UINT uMsg, LPARAM lParam1, LPARAM lParam2)
{
	switch (uMsg) {
	case DRV_QUERYCONFIGURE:
		return DRV_CANCEL;
	case DRV_CONFIGURE:
		return DoDriverConfiguration();
	case DRV_LOAD:
		return DRV_OK;
	case DRV_REMOVE:
		return DRV_OK;
	case DRV_OPEN:
		KSDevice = hdrvr;
		return DRV_OK;
	case DRV_CLOSE:
		return DRV_OK;
	default:
		return DRV_OK;
	}
}

DWORD modGetCaps(INT_PTR uDeviceID, MIDIOUTCAPS* capsPtr, DWORD capsSize) {
	try {
		MIDIOUTCAPSA * myCapsA;
		MIDIOUTCAPSW * myCapsW;
		MIDIOUTCAPS2A * myCaps2A;
		MIDIOUTCAPS2W * myCaps2W;
		
		WORD maximumvoices = 0xFFFF;
		WORD maximumnotes = 0xFFFF;
		DWORD CapsSupport = MIDICAPS_VOLUME;
		DWORD Technology = NULL;

		WORD VID = 0x0000;
		WORD PID = 0x0000;
		CHAR SynthName[MAXPNAMELEN];
		WCHAR SynthNameW[MAXPNAMELEN];

		HKEY hKey;
		long lResult;
		DWORD dwType = REG_DWORD;
		DWORD dwSize = sizeof(DWORD);
		DWORD dwSizeA = sizeof(SynthName);
		DWORD dwSizeW = sizeof(SynthNameW);

		lResult = RegOpenKeyEx(HKEY_CURRENT_USER, L"Software\\Keppy's Synthesizer\\Configuration", 0, KEY_ALL_ACCESS, &hKey);
		RegQueryValueEx(hKey, L"SynthType", NULL, &dwType, (LPBYTE)&SynthType, &dwSize);
		RegQueryValueEx(hKey, L"DebugMode", NULL, &dwType, (LPBYTE)&ManagedSettings.DebugMode, &dwSize);
		RegQueryValueEx(hKey, L"VID", NULL, &dwType, (LPBYTE)&VID, &dwSize);
		RegQueryValueEx(hKey, L"PID", NULL, &dwType, (LPBYTE)&PID, &dwSize);

		dwType = REG_SZ;
		RegQueryValueExA(hKey, "SynthName", NULL, &dwType, (LPBYTE)&SynthName, &dwSizeA);
		RegQueryValueExW(hKey, L"SynthName", NULL, &dwType, (LPBYTE)&SynthNameW, &dwSizeW);
		RegCloseKey(hKey);

		if (SynthType > MOD_SWSYNTH)
			Technology = MOD_SWSYNTH;
		else Technology = SynthNamesTypes[SynthType];

		if (ManagedSettings.DebugMode && (!BannedSystemProcess() | !BlackListSystem())) CreateConsole();

		if (strlen(SynthName) < 1 || isspace(SynthName[0])) {
			ZeroMemory(SynthName, MAXPNAMELEN);
			strncpy(SynthName, "Keppy's Synthesizer\0", MAXPNAMELEN);
		}

		if (wcslen(SynthNameW) < 1 || iswspace(SynthNameW[0])) {
			ZeroMemory(SynthNameW, MAXPNAMELEN);
			wcsncpy(SynthNameW, L"Keppy's Synthesizer\0", MAXPNAMELEN);
		}

		PrintToConsole(FOREGROUND_BLUE, 1, "Sharing MIDI caps with application...");

		const GUID CLSIDKEPSYNTH = { 0x210CE0E8, 0x6837, 0x448E, { 0xB1, 0x3F, 0x09, 0xFE, 0x71, 0xE7, 0x44, 0xEC } };

		// Not yet
		// CapsSupport |= MIDICAPS_STREAM;

		switch (capsSize) {
		case (sizeof(MIDIOUTCAPSA)):
			myCapsA = (MIDIOUTCAPSA *)capsPtr;
			memcpy(myCapsA->szPname, SynthName, sizeof(SynthName));
			myCapsA->dwSupport = CapsSupport;
			myCapsA->wChannelMask = 0xffff;
			myCapsA->wMid = VID;
			myCapsA->wNotes = maximumnotes;
			myCapsA->wPid = PID;
			myCapsA->wTechnology = Technology;
			myCapsA->wVoices = maximumvoices;
			myCapsA->vDriverVersion = 0x0501;
			PrintToConsole(FOREGROUND_BLUE, 1, "Done sharing caps. (MIDIOUTCAPSA)");
			return MMSYSERR_NOERROR;

		case (sizeof(MIDIOUTCAPSW)):
			myCapsW = (MIDIOUTCAPSW *)capsPtr;
			memcpy(myCapsW->szPname, SynthNameW, sizeof(SynthNameW));
			myCapsW->dwSupport = CapsSupport;
			myCapsW->wChannelMask = 0xffff;
			myCapsW->wMid = VID;
			myCapsW->wNotes = maximumnotes;
			myCapsW->wPid = PID;
			myCapsW->wTechnology = Technology;
			myCapsW->wVoices = maximumvoices;
			myCapsW->vDriverVersion = 0x0501;
			PrintToConsole(FOREGROUND_BLUE, 1, "Done sharing caps. (MIDIOUTCAPSW)");
			return MMSYSERR_NOERROR;

		case (sizeof(MIDIOUTCAPS2A)):
			myCaps2A = (MIDIOUTCAPS2A *)capsPtr;
			memcpy(myCaps2A->szPname, SynthName, sizeof(SynthName));
			myCaps2A->ManufacturerGuid = CLSIDKEPSYNTH;
			myCaps2A->NameGuid = CLSIDKEPSYNTH;
			myCaps2A->ProductGuid = CLSIDKEPSYNTH;
			myCaps2A->dwSupport = CapsSupport;
			myCaps2A->wChannelMask = 0xffff;
			myCaps2A->wMid = VID;
			myCaps2A->wNotes = maximumnotes;
			myCaps2A->wPid = PID;
			myCaps2A->wTechnology = Technology;
			myCaps2A->wVoices = maximumvoices;
			myCaps2A->vDriverVersion = 0x0501;
			PrintToConsole(FOREGROUND_BLUE, 1, "Done sharing caps. (MIDIOUTCAPS2A)");
			return MMSYSERR_NOERROR;

		case (sizeof(MIDIOUTCAPS2W)):
			myCaps2W = (MIDIOUTCAPS2W *)capsPtr;
			memcpy(myCaps2W->szPname, SynthNameW, sizeof(SynthNameW));
			myCaps2W->ManufacturerGuid = CLSIDKEPSYNTH;
			myCaps2W->NameGuid = CLSIDKEPSYNTH;
			myCaps2W->ProductGuid = CLSIDKEPSYNTH;
			myCaps2W->dwSupport = CapsSupport;
			myCaps2W->wChannelMask = 0xffff;
			myCaps2W->wMid = VID;
			myCaps2W->wNotes = maximumnotes;
			myCaps2W->wPid = PID;
			myCaps2W->wTechnology = Technology;
			myCaps2W->wVoices = maximumvoices;
			myCaps2W->vDriverVersion = 0x0501;
			PrintToConsole(FOREGROUND_BLUE, 1, "Done sharing caps. (MIDIOUTCAPS2W)");
			return MMSYSERR_NOERROR;

		default:
			return MMSYSERR_NOTENABLED;
		}
	}
	catch (...) {
		CrashMessage(L"MIDICapsException");
		ExitThread(0);
		throw;
	}
}

LONG DoOpenClient() {
	DoStartClient();
	DoResetClient();
	SetPriorityClass(GetCurrentProcess(), processPriority);
	return MMSYSERR_NOERROR;
}

STDAPI_(DWORD) modMessage(INT_PTR uDeviceID, UINT uMsg, DWORD_PTR dwUser, DWORD_PTR dwParam1, DWORD_PTR dwParam2){
	MIDIHDR* IIMidiHdr;
	DWORD retval = MMSYSERR_NOERROR;

	switch (uMsg) {
	case MODM_DATA:
		// Parse the data lol
		return _PrsData(uMsg, dwParam1, dwParam2);
	case MODM_LONGDATA: // case MODM_STRMDATA:
		// Reference the MIDIHDR
		IIMidiHdr = (MIDIHDR*)dwParam1;

		if (!IIMidiHdr || (sizeof(IIMidiHdr->lpData) > LONGMSG_MAXSIZE)) return MMSYSERR_INVALPARAM;	// The buffer doesn't exist, invalid 
		if (!(IIMidiHdr->dwFlags & MHDR_PREPARED)) return MIDIERR_UNPREPARED;							// The buffer is not prepared
		if (IIMidiHdr->dwFlags & MHDR_INQUEUE) return MIDIERR_STILLPLAYING;								// The buffer is already being played

		// Mark the buffer as in queue
		IIMidiHdr->dwFlags &= ~MHDR_DONE;
		IIMidiHdr->dwFlags |= MHDR_INQUEUE;

		// Do the stuff with it, if it's not to be ignored
		if (!ManagedSettings.IgnoreSysEx) SendLongToBASSMIDI(IIMidiHdr->lpData, sizeof(IIMidiHdr->lpData));
		// It has to be ignored, send info to console
		else PrintToConsole(FOREGROUND_RED, (DWORD)IIMidiHdr->lpData, "Ignored SysEx MIDI event.");

		// Mark the buffer as done
		IIMidiHdr->dwFlags &= ~MHDR_INQUEUE;
		IIMidiHdr->dwFlags |= MHDR_DONE;

		// Tell the app that the buffer has been played
		DriverCallback(KSCallback, KSFlags, KSDevice, MOM_DONE, KSInstance, dwParam1, 0);
		return MMSYSERR_NOERROR;
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

		return MMSYSERR_NOTSUPPORTED;
	case MODM_OPEN:
		// Parse callback and instance
		KSCallback = reinterpret_cast<MIDIOPENDESC*>(dwParam1)->dwCallback;
		KSInstance = reinterpret_cast<MIDIOPENDESC*>(dwParam1)->dwInstance;
		KSFlags = HIWORD(static_cast<DWORD>(dwParam2));

		// Open the driver
		retval = DoOpenClient();

		// Tell the app that the driver is ready
		DriverCallback(KSCallback, KSFlags, KSDevice, MOM_OPEN, KSInstance, 0, 0);
		return retval;
	case MODM_PREPARE:
		// Reference the MIDIHDR
		IIMidiHdr = (MIDIHDR*)dwParam1;

		if (!IIMidiHdr || sizeof(IIMidiHdr->lpData) > LONGMSG_MAXSIZE) return MMSYSERR_INVALPARAM;			// The buffer doesn't exist or is too big, invalid parameter

		// Lock the MIDIHDR buffer, to prevent the MIDI app from accidentally writing to it
		if (!VirtualLock(IIMidiHdr->lpData, sizeof(IIMidiHdr->lpData)))
			return MMSYSERR_NOMEM;

		// Mark the buffer as prepared, and say that everything is oki-doki
		IIMidiHdr->dwFlags |= MHDR_PREPARED;
		return MMSYSERR_NOERROR;
	case MODM_UNPREPARE:
		// Reference the MIDIHDR
		IIMidiHdr = (MIDIHDR*)dwParam1;

		// Check if the MIDIHDR buffer is valid
		if (!IIMidiHdr) return MMSYSERR_INVALPARAM;								// The buffer doesn't exist, invalid parameter
		if (!(IIMidiHdr->dwFlags & MHDR_PREPARED)) return MMSYSERR_NOERROR;		// Already unprepared, everything is fine
		if (IIMidiHdr->dwFlags & MHDR_INQUEUE) return MIDIERR_STILLPLAYING;		// The buffer is currently being played from the driver, cannot unprepare

		IIMidiHdr->dwFlags &= ~MHDR_PREPARED;									// Mark the buffer as unprepared

		// Unlock the buffer, and say that everything is oki-doki
		VirtualUnlock(IIMidiHdr->lpData, sizeof(IIMidiHdr->lpData));
		return MMSYSERR_NOERROR;
	case MODM_GETNUMDEVS:
		// Return "1" if the process isn't blacklisted, otherwise the driver doesn't exist OwO
		return BlackListInit();
	case MODM_GETDEVCAPS:
		// Return KS' caps to the app
		return modGetCaps(uDeviceID, reinterpret_cast<MIDIOUTCAPS*>(dwParam1), static_cast<DWORD>(dwParam2));
	case MODM_GETVOLUME:
		// Tell the app the current output volume of the driver
		*(LONG*)dwParam1 = static_cast<LONG>(sound_out_volume_float * 0xFFFF);
		return MMSYSERR_NOERROR;
	case MODM_SETVOLUME: 
		// The app isn't allowed to set the volume, everything's fine anyway
		return MMSYSERR_NOERROR;
	case MODM_RESET:
		DoResetClient();
		return MMSYSERR_NOERROR;
	case MODM_STOP:
		DoResetClient();
		DriverCallback(KSCallback, KSFlags, KSDevice, MOM_DONE, KSInstance, 0, 0);
		return MMSYSERR_NOERROR;
	case MODM_CLOSE:
		// The driver is sleeping now (Sort of), tell the app about this and that everything is oki-doki
		DriverCallback(KSCallback, KSFlags, KSDevice, MOM_CLOSE, KSInstance, 0, 0);
		return MMSYSERR_NOERROR;
	default:
		return MMSYSERR_NOERROR;
	}
}