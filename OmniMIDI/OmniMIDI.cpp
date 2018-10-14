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
#include "OmniMIDI.h"

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

// Predefined sleep values, useful for redundancy
#define _DBGWAIT NTSleep(-10000)											// Normal wait
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

// OmniMIDI vital parts
#include "sfsystem.h"
#include "bufsystem.h"
#include "settings.h"
#include "bansystem.h"
#include "drvinit.h"
#include "kdmapi.h"

extern "C" BOOL APIENTRY DllMain(HANDLE hinstDLL, DWORD fdwReason, LPVOID lpvReserved)
{
	switch (fdwReason) {
	case DLL_PROCESS_ATTACH:
		hinst = (HINSTANCE)hinstDLL;
		NtDelayExecution = (NDE)GetProcAddress(LoadLibrary(L"ntdll"), "NtDelayExecution");
		DisableThreadLibraryCalls((HMODULE)hinstDLL);
		break;
	case DLL_PROCESS_DETACH: break;
	case DLL_THREAD_ATTACH: break;
	case DLL_THREAD_DETACH: break;
	}

	return TRUE;
}

LONG_PTR DoDriverConfiguration() {
	TCHAR configuratorapp[MAX_PATH];
	if (SUCCEEDED(SHGetFolderPath(NULL, CSIDL_SYSTEMX86, NULL, 0, configuratorapp)))
	{
		PathAppend(configuratorapp, _T("\\OmniMIDI\\OmniMIDIConfigurator.exe"));
		ShellExecute(NULL, L"open", configuratorapp, L"/AST", NULL, SW_SHOWNORMAL);
		return DRVCNF_OK;
	}
	return DRVCNF_CANCEL;
}

STDAPI_(LONG_PTR) DriverProc(DWORD_PTR dwDriverId, HDRVR hdrvr, UINT uMsg, LPARAM lParam1, LPARAM lParam2)
{
	switch (uMsg) {
	case DRV_QUERYCONFIGURE:
		return DRV_OK;
	case DRV_CONFIGURE:
		return DoDriverConfiguration();
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
		return DRV_OK;
	default:
		return DRV_OK;
	}
}

DWORD modGetCaps(PVOID capsPtr, DWORD capsSize) {
	try {
		// Create temp caps
		static MIDIOUTCAPS2 MIDICaps = { 0 };
		
		// Initialize values
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
		if (SynthType >= (SizeOfArray(SynthNamesTypes)))
			Technology = MOD_MIDIPORT;
		// Else, load the requested value
		else Technology = SynthNamesTypes[SynthType];

		// If the debug mode is enabled, and the process isn't banned, create the debug log
		if (ManagedSettings.DebugMode && (!BannedSystemProcess() | !BlackListSystem())) 
			CreateConsole();

		// If the synthname length is less than 1, or if it's just a space, use the default name
		if (wcslen(SynthNameW) < 1 || (wcslen(SynthNameW) == 1 && iswspace(SynthNameW[0]))) {
			ZeroMemory(SynthNameW, MAXPNAMELEN);
			wcsncpy(SynthNameW, L"OmniMIDI\0", MAXPNAMELEN);
		}

		PrintToConsole(FOREGROUND_BLUE, 1, "Sharing MIDI device caps with application...");

		// Dummy GUID associated with OM
		const GUID OMCLSID = { 0x210CE0E8, 0x6837, 0x448E, { 0xB1, 0x3F, 0x09, 0xFE, 0x71, 0xE7, 0x44, 0xEC } };

		// Not yet
		// CapsSupport |= MIDICAPS_STREAM;

		// Prepare the caps item
		if (!MIDICaps.wMid) {
			memcpy(MIDICaps.szPname, SynthNameW, sizeof(SynthNameW));
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
			PrintToConsole(FOREGROUND_BLUE, 1, "Done sharing MIDI device caps.");
		}

		// Copy the item to the app's caps
		memcpy(capsPtr, &MIDICaps, min(sizeof(MIDICaps), capsSize));
		return MMSYSERR_NOERROR;
	}
	catch (...) {
		CrashMessage(L"MIDICapsException");
	}
}

LONG DoOpenClient() {
	// Start the driver
	DoStartClient();
	DoResetClient();
	return MMSYSERR_NOERROR;
}

STDAPI_(DWORD) modMessage(UINT uDeviceID, UINT uMsg, DWORD_PTR dwUser, DWORD_PTR dwParam1, DWORD_PTR dwParam2){
	MIDIHDR* IIMidiHdr;
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

		return MMSYSERR_NOTSUPPORTED;
	case MODM_OPEN:
		// The driver doesn't support stream mode
		if ((DWORD)dwParam2 & MIDI_IO_COOKED) return MMSYSERR_NOTENABLED;

		if (!AlreadyInitializedViaKDMAPI || !bass_initialized) {
			// Parse callback and instance
			OMCallback = ((MIDIOPENDESC*)dwParam1)->dwCallback;
			OMInstance = ((MIDIOPENDESC*)dwParam1)->dwInstance;
			OMFlags = HIWORD((DWORD)dwParam2);

			// Open the driver
			RetVal = DoOpenClient();

			// Tell the app that the driver is ready
			DriverCallback(OMCallback, OMFlags, OMDevice, MOM_OPEN, OMInstance, 0, 0);
			return RetVal;
		}
		// The driver is already being used through KDMAPI
		return MMSYSERR_ALLOCATED;
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
		*(LONG*)dwParam1 = (LONG)(sound_out_volume_float * 0xFFFF);
		return MMSYSERR_NOERROR;
	case MODM_SETVOLUME: 
		// The app isn't allowed to set the volume, everything's fine anyway
		return MMSYSERR_NOERROR;
	case MODM_RESET:
		// Stop all the current active voices
		DoResetClient();
		return MMSYSERR_NOERROR;
	case MODM_STOP:
		// Stop all the current active voices and send the callback to the app
		DoResetClient();
		DriverCallback(OMCallback, OMFlags, OMDevice, MOM_DONE, OMInstance, 0, 0);
		return MMSYSERR_NOERROR;
	case MODM_CLOSE:
		if (!AlreadyInitializedViaKDMAPI && bass_initialized) {
			// The app wants us to close the driver
			// Only close the stream if the user has chosen to
			if (CloseStreamMidiOutClose) DoStopClient();
			DriverCallback(OMCallback, OMFlags, OMDevice, MOM_CLOSE, OMInstance, 0, 0);		
		}
		return MMSYSERR_NOERROR;
	case DRV_QUERYDEVICEINTERFACESIZE:
		// Maximum longmsg size, 64kB
		*(LONG*)dwParam1 = 65535;
		return MMSYSERR_NOERROR;
	case DRV_QUERYDEVICEINTERFACE:
		// The app is asking for the driver's name, let's give it to them
		memcpy((VOID*)dwParam1, SynthNameW, sizeof(SynthNameW));
		*(LONG*)dwParam2 = 65535;
		return MMSYSERR_NOERROR;
	}
}