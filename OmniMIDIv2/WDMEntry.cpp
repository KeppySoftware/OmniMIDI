/*
OmniMIDI v15+ (Rewrite) for Windows NT

This file contains the required code to run the driver under Windows 7 SP1 and later.
This file is useful only if you want to compile the driver under Windows, it's not needed for Linux/macOS porting.
*/

#include "WDMEntry.h"

static ErrorSystem::WinErr WDMErr;
static WinDriver::DriverCallback* fDriverCallback;
static WinDriver::DriverComponent* DriverComponent;
static WinDriver::DriverMask* DriverMask;
static OmniMIDI::SynthModule* SynthModule;

#define SYNTH	FluidSynth

extern "C" {
	__declspec(dllexport)
	BOOL APIENTRY DllMain(HMODULE hModule, DWORD ReasonForCall, LPVOID lpReserved)
	{
		BOOL ret = FALSE;
		switch (ReasonForCall)
		{
		case DLL_PROCESS_ATTACH:
			if (!DriverComponent) {
				DriverComponent = new WinDriver::DriverComponent;

				if ((ret = DriverComponent->SetLibraryHandle(hModule)) == TRUE) {
					DriverMask = new WinDriver::DriverMask;
					fDriverCallback = new WinDriver::DriverCallback;

					// Allocate a generic dummy synth for now
					SynthModule = new OmniMIDI::SynthModule;
				}
			}

			break;

		case DLL_PROCESS_DETACH:
			if (DriverComponent) {
				delete SynthModule;
				delete fDriverCallback;
				delete DriverMask;

				ret = DriverComponent->UnsetLibraryHandle();
			}
			break;

		case DLL_THREAD_ATTACH:
		case DLL_THREAD_DETACH:
			break;
		}
		return ret;
	}

	__declspec(dllexport)
		LRESULT WINAPI DriverProc(DWORD DriverIdentifier, HDRVR DriverHandle, UINT Message, LONG Param1, LONG Param2) {
		switch (Message) {
		case DRV_OPEN:
			return DriverComponent->SetDriverHandle(DriverHandle);
		case DRV_CLOSE:
			return DriverComponent->UnsetDriverHandle();

		case DRV_LOAD:
		case DRV_FREE:
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

	MMRESULT WINAPI pmidiOutShortMsg(HMIDIOUT dev, DWORD ev) {

	}

	__declspec(dllexport)
	MMRESULT WINAPI modMessage(UINT DeviceID, UINT Message, DWORD_PTR UserPointer, DWORD_PTR Param1, DWORD_PTR Param2) {
		LPMIDIHDR hdr = nullptr;
		MMRESULT r = MMSYSERR_NOERROR;

		switch (Message) {
		case MODM_DATA:
			return (SynthModule->PlayShortEvent((DWORD)Param1) == SYNTH_OK) ? MMSYSERR_NOERROR : MMSYSERR_INVALPARAM;

		case MODM_LONGDATA:
			hdr = ((LPMIDIHDR)Param1);
			r = SynthModule->PlayLongEvent(hdr->lpData, hdr->dwBytesRecorded);
			fDriverCallback->CallbackFunction(MOM_DONE, (DWORD)Param1, 0);
			return (r == SYNTH_OK) ? MMSYSERR_NOERROR : MMSYSERR_INVALPARAM;

		case MODM_RESET:
			return (SynthModule->PlayShortEvent(0x010101FF) == SYNTH_OK) ? MMSYSERR_NOERROR : MMSYSERR_INVALPARAM;

		case MODM_OPEN:
			if (SynthModule->SynthID() == EMPTYMODULE) {
				delete SynthModule;
				SynthModule = new OmniMIDI::SYNTH;

				if (SynthModule->LoadSynthModule()) {
					if (SynthModule->StartSynthModule()) {
						if (fDriverCallback->PrepareCallbackFunction((LPMIDIOPENDESC)Param1, (DWORD)Param2))
						{
							fDriverCallback->CallbackFunction(MOM_OPEN, 0, 0);
							return MMSYSERR_NOERROR;
						}
					}
					if (!SynthModule->StopSynthModule()) { FNERROR(WDMErr, "StopBASSSynth failed in MODM_OPEN!\n\nABORT!!!"); }
				}
				if (!SynthModule->UnloadSynthModule()) { FNERROR(WDMErr, "UnloadBASSSynth failed in MODM_OPEN!\n\nABORT!!!"); }
			}
			else return MMSYSERR_ALLOCATED;

			NERROR(WDMErr, "MODM_OPEN failed.", true);
			return MMSYSERR_NOMEM;

		case MODM_CLOSE:
		{
			if (SynthModule->SynthID() != EMPTYMODULE) {
				if (SynthModule->StopSynthModule()) {
					if (SynthModule->UnloadSynthModule()) {
						fDriverCallback->CallbackFunction(MOM_CLOSE, 0, 0);
						if (fDriverCallback->ClearCallbackFunction())
						{
							delete SynthModule;
							SynthModule = new OmniMIDI::SynthModule;

							return MMSYSERR_NOERROR;
						}
					}
				}
			} 
			else return MMSYSERR_NOERROR;

			LOG(WDMErr, "MODM_CLOSE failed.");
			return MMSYSERR_ALLOCATED;
		}

		case MODM_GETNUMDEVS:
			return 1;

		case MODM_GETDEVCAPS:
			return DriverMask->GiveCaps(DeviceID, (PVOID)Param1, (DWORD)Param2);

		case DRVM_INIT:
		case DRVM_EXIT:
		case DRVM_ENABLE:
		case DRVM_DISABLE:
		case MODM_CACHEPATCHES:
		case MODM_CACHEDRUMPATCHES:
		case DRV_QUERYDEVICEINTERFACESIZE:
		case DRV_QUERYDEVICEINTERFACE:
			return MMSYSERR_NOERROR;

		case MODM_SETVOLUME:
		case MODM_GETVOLUME:
		default:
			return MMSYSERR_NOTSUPPORTED;
		}
	}

	__declspec(dllexport)
	int KDMAPI IsKDMAPIAvailable() {
		return 1;
	}

	__declspec(dllexport)
	int KDMAPI InitializeKDMAPIStream() {
		if (SynthModule->SynthID() == EMPTYMODULE) {
			delete SynthModule;
			SynthModule = new OmniMIDI::SYNTH;

			if (SynthModule->LoadSynthModule()) {
				if (SynthModule->StartSynthModule()) {
					return 1;
				}
				SynthModule->StopSynthModule();
			}
			SynthModule->UnloadSynthModule();
		}

		LOG(WDMErr, "InitializeKDMAPIStream failed.");
		return 0;
	}

	__declspec(dllexport)
	int KDMAPI TerminateKDMAPIStream() {
		if (SynthModule->SynthID() != EMPTYMODULE) {
			if (SynthModule->StopSynthModule()) {
				if (SynthModule->UnloadSynthModule()) {
					delete SynthModule;
					SynthModule = new OmniMIDI::SynthModule;

					return 1;
				}
			}
		}

		LOG(WDMErr, "TerminateKDMAPIStream failed.");
		return 0;
	}

	__declspec(dllexport)
	void KDMAPI ResetKDMAPIStream() {
		SynthModule->PlayShortEvent(0x010101FF);
	}

	__declspec(dllexport)
	void KDMAPI SendDirectData(unsigned int ev) {
		SynthModule->PlayShortEvent(ev);
	}

	__declspec(dllexport)
	void KDMAPI SendDirectDataNoBuf(unsigned int) {
		// Unsupported.
	}

	__declspec(dllexport)
	int KDMAPI SendCustomEvent(unsigned int evt, unsigned int chan, unsigned int param) {
		return SynthModule->TalkToSynthDirectly(evt, chan, param);
	}

	__declspec(dllexport)
	int KDMAPI DriverSettings(unsigned int setting, unsigned int mode, void* value, unsigned int cbValue) {
		if (!SynthModule)
			return 0;

		return SynthModule->SettingsManager(setting, (bool)mode, value, (size_t)cbValue);
	}
}