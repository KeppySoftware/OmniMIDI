/*
OmniMIDI v15+ (Rewrite) for Windows NT

This file contains the required code to run the driver under Windows 7 SP1 and later.
This file is useful only if you want to compile the driver under Windows, it's not needed for Linux/macOS porting.
*/

#include "WDMEntry.h"

static ErrorSystem::WinErr WDMErr;
static WinDriver::DriverCallback fDriverCallback;
static WinDriver::DriverComponent DriverComponent;
static WinDriver::DriverMask DriverMask;
static OmniMIDI::SynthModule SynthModule;

extern "C" __declspec(dllexport)
BOOL APIENTRY DllMain(HMODULE hModule, DWORD ReasonForCall, LPVOID lpReserved)
{
    switch (ReasonForCall)
    {
    case DLL_PROCESS_ATTACH:
    case DLL_THREAD_ATTACH:
    case DLL_THREAD_DETACH:
    case DLL_PROCESS_DETACH:
        break;
    }
    return TRUE;
}

extern "C" __declspec(dllexport)
LRESULT WINAPI DriverProc(DWORD DriverIdentifier, HDRVR DriverHandle, UINT Message, LONG Param1, LONG Param2) {
	switch (Message) {
	case DRV_LOAD:
		return DriverComponent.SetDriverHandle(DriverHandle);
	case DRV_FREE:
		return DriverComponent.UnsetDriverHandle();

	case DRV_OPEN:
	case DRV_CLOSE:
		return DRVCNF_OK;

	case DRV_QUERYCONFIGURE:
		return DRVCNF_CANCEL;

	case DRV_ENABLE:
	case DRV_DISABLE:
		return DRVCNF_OK;

	case DRV_INSTALL:
		return DRVCNF_OK;

	case DRV_REMOVE:
		return DRVCNF_OK;
	}

	return DefDriverProc(DriverIdentifier, DriverHandle, Message, Param1, Param2);
}

extern "C" __declspec(dllexport) 
MMRESULT WINAPI modMessage(UINT DeviceID, UINT Message, DWORD_PTR UserPointer, DWORD_PTR Param1, DWORD_PTR Param2) {
	LPMIDIHDR hdr = nullptr;
	MMRESULT r = MMSYSERR_NOERROR;

	switch (Message) {
	case MODM_DATA:
		return (SynthModule.PlayShortEvent((DWORD)Param1) == SYNTH_OK) ? MMSYSERR_NOERROR : MMSYSERR_INVALPARAM;

	case MODM_LONGDATA:
		hdr = ((LPMIDIHDR)Param1);
		r = SynthModule.PlayLongEvent(hdr->lpData, hdr->dwBytesRecorded);
		fDriverCallback.CallbackFunction(MOM_DONE, (DWORD)Param1, 0);
		return (r == SYNTH_OK) ? MMSYSERR_NOERROR : MMSYSERR_INVALPARAM;

	case MODM_RESET:
		return (SynthModule.PlayShortEvent(0x010101FF) == SYNTH_OK) ? MMSYSERR_NOERROR : MMSYSERR_INVALPARAM;

	case MODM_OPEN:
		if (SynthModule.LoadSynthModule()) {
			if (SynthModule.StartSynthModule()) {
				if (fDriverCallback.PrepareCallbackFunction((LPMIDIOPENDESC)Param1, (DWORD)Param2))
				{
					fDriverCallback.CallbackFunction(MOM_OPEN, 0, 0);
					return MMSYSERR_NOERROR;
				}
			}
			SynthModule.StopSynthModule();
		}
		SynthModule.UnloadSynthModule();

		LOG(WDMErr, L"MODM_OPEN failed.");
		return MMSYSERR_NOTENABLED;

	case MODM_CLOSE:
		if (SynthModule.StopSynthModule()) {
			if (SynthModule.UnloadSynthModule()) {
				fDriverCallback.CallbackFunction(MOM_CLOSE, 0, 0);
				if (fDriverCallback.ClearCallbackFunction())
				{
					return MMSYSERR_NOERROR;
				}
			}
		}

		LOG(WDMErr, L"MODM_CLOSE failed.");
		return MMSYSERR_ALLOCATED;

	case MODM_GETNUMDEVS:
		return 1;

	case MODM_GETDEVCAPS:
		return DriverMask.GiveCaps(DeviceID, (PVOID)Param1, (DWORD)Param2);

	case MODM_SETVOLUME:
	case MODM_GETVOLUME:
	case MODM_CACHEPATCHES:
	case MODM_CACHEDRUMPATCHES:
	case DRV_QUERYDEVICEINTERFACESIZE:
	case DRV_QUERYDEVICEINTERFACE:
		return MMSYSERR_NOERROR;

	default:
		return MMSYSERR_NOTSUPPORTED;
	}
}

extern "C" __declspec(dllexport)
int KDMAPI IsKDMAPIAvailable() {
	return 1;
}

extern "C" __declspec(dllexport)
int KDMAPI InitializeKDMAPIStream() {
	if (SynthModule.LoadSynthModule()) {
		if (SynthModule.StartSynthModule()) {
			return 1;
		}
		SynthModule.StopSynthModule();
	}
	SynthModule.UnloadSynthModule();

	LOG(WDMErr, L"InitializeKDMAPIStream failed.");
	return 0;
}

extern "C" __declspec(dllexport)
int KDMAPI TerminateKDMAPIStream() {
	if (SynthModule.StopSynthModule()) {
		if (SynthModule.UnloadSynthModule()) {
			return 1;
		}
	}

	LOG(WDMErr, L"TerminateKDMAPIStream failed.");
	return 0;
}

extern "C" __declspec(dllexport)
void KDMAPI ResetKDMAPIStream() {
	SynthModule.PlayShortEvent(0x010101FF);
}

extern "C" __declspec(dllexport)
void KDMAPI SendDirectData(unsigned int ev) {
	SynthModule.PlayShortEvent(ev);
}

extern "C" __declspec(dllexport)
void KDMAPI SendDirectDataNoBuf(unsigned int) {
	// Unsupported.
}

extern "C" __declspec(dllexport)
int KDMAPI SendCustomEvent(unsigned int evt, unsigned int chan, unsigned int param) {
	return SynthModule.TalkToBASSMIDI(evt, chan, param);
}