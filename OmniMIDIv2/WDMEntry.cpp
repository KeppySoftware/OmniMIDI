/*

	OmniMIDI v15+ (Rewrite) for Windows NT

	This file contains the required code to run the driver under Windows 7 SP1 and later.
	This file is useful only if you want to compile the driver under Windows, it's not needed for Linux/macOS porting.

*/

#include "WDMEntry.h"

typedef OmniMIDI::SynthModule* (*rInitModule)();
typedef void(*rStopModule)();

static ErrorSystem::WinErr WDMErr;
static WinDriver::DriverCallback* fDriverCallback = nullptr;
static WinDriver::DriverComponent* DriverComponent = nullptr;
static WinDriver::DriverMask* DriverMask = nullptr;
static OmniMIDI::WDMSettings* WDMSettings = nullptr;
static OmniMIDI::SynthModule* SynthModule = nullptr;
static HMODULE extModule = nullptr;

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

				if ((ret = DriverComponent->SetLibraryHandle(hModule)) == true) {
					DriverMask = new WinDriver::DriverMask;
					fDriverCallback = new WinDriver::DriverCallback;
					WDMSettings = new OmniMIDI::WDMSettings;

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
				if (WDMSettings) delete WDMSettings;

				ret = DriverComponent->UnsetLibraryHandle();
				delete DriverComponent;
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

	void freeAudioRender() {
		if (extModule) {
			if (SynthModule) {
				auto fM = (rStopModule)GetProcAddress(extModule, "freeModule");
				fM();
			}

			FreeLibrary(extModule);
			extModule = nullptr;
			SynthModule = nullptr;
		}
		else if (SynthModule != nullptr)
		{
			delete SynthModule;
			SynthModule = nullptr;
		}

		if (WDMSettings != nullptr)
		{
			delete WDMSettings;
			WDMSettings = nullptr;
		}

		SynthModule = new OmniMIDI::SynthModule;
	}

	BOOL getAudioRender() {
		BOOL good = TRUE;

		if (WDMSettings != nullptr)
			delete WDMSettings;

		if (SynthModule != nullptr)
			delete SynthModule;

		WDMSettings = new OmniMIDI::WDMSettings;
		switch (WDMSettings->Renderer) {
		case EXTERNAL:
			extModule = LoadLibraryA(WDMSettings->CustomRenderer.c_str());

			if (extModule) {
				auto iM = (rInitModule)GetProcAddress(extModule, "initModule");

				if (iM) {
					SynthModule = iM();

					if (!SynthModule)
						good = FALSE;
				}
				else good = FALSE;
			}
			else good = FALSE;
			break;
		case BASSMIDI:
			SynthModule = new OmniMIDI::BASSSynth;
			break;
		case FLUIDSYNTH:
			SynthModule = new OmniMIDI::FluidSynth;
			break;
		case XSYNTH:
			SynthModule = new OmniMIDI::XSynth;
			break;
		case TINYSF:
			SynthModule = new OmniMIDI::TinySFSynth;
			break;
		default:
			good = FALSE;
			break;
		}

		if (!good) freeAudioRender();
		else LOG(WDMErr, "Renderer: %d\nCustom renderer: %s", 
			WDMSettings->Renderer, 
			WDMSettings->CustomRenderer.c_str());

		return good;
	}


	__declspec(dllexport)
	MMRESULT WINAPI modMessage(UINT DeviceID, UINT Message, DWORD_PTR UserPointer, DWORD_PTR Param1, DWORD_PTR Param2) {
		switch (Message) {
		case MODM_DATA:
			return (!SynthModule->PlayShortEvent((DWORD)Param1)) ? MMSYSERR_NOERROR : MMSYSERR_INVALPARAM;

		case MODM_LONGDATA:
			fDriverCallback->CallbackFunction(MOM_DONE, (DWORD)Param1, 0);
			return (!SynthModule->PlayLongEvent(((LPMIDIHDR)Param1)->lpData, ((LPMIDIHDR)Param1)->dwBytesRecorded)) ? MMSYSERR_NOERROR : MMSYSERR_INVALPARAM;

		case MODM_RESET:
			return (!SynthModule->PlayShortEvent(0x0101FF) == SYNTH_OK) ? MMSYSERR_NOERROR : MMSYSERR_INVALPARAM;

		case MODM_OPEN:
			freeAudioRender();
			if (getAudioRender()) {
				if (!SynthModule->LoadSynthModule()) {
					NERROR(WDMErr, "Unable to initialize the synthesizer module!\n\nPress OK to continue without audio.", true);

					if (!SynthModule->UnloadSynthModule()) 
						FNERROR(WDMErr, "UnloadSynthModule failed in MODM_OPEN!\n\nABORT!!!");

					freeAudioRender();
					return MMSYSERR_NOMEM;
				}

				if (!SynthModule->StartSynthModule()) {
					NERROR(WDMErr, "Unable to start up the synthesizer module!\n\nPress OK to continue without audio.", true);

					if (fDriverCallback->PrepareCallbackFunction((LPMIDIOPENDESC)Param1, (DWORD)Param2))
					{
						fDriverCallback->CallbackFunction(MOM_OPEN, 0, 0);
						return MMSYSERR_NOERROR;
					}

					freeAudioRender();
					return MMSYSERR_NOTENABLED;
				}

				LOG(WDMErr, "Synth ID: %x", SynthModule->SynthID());

				fDriverCallback->PrepareCallbackFunction((LPMIDIOPENDESC)Param1, (DWORD)Param2);
				fDriverCallback->CallbackFunction(MOM_OPEN, 0, 0);

				return MMSYSERR_NOERROR;
			}

			NERROR(WDMErr, "Failed to initialize synthesizer.", true);
			return MMSYSERR_NOMEM;

		case MODM_CLOSE:
			if (SynthModule->StopSynthModule()) {
				if (SynthModule->UnloadSynthModule()) {
					fDriverCallback->CallbackFunction(MOM_CLOSE, 0, 0);
					fDriverCallback->ClearCallbackFunction();
					freeAudioRender();
					return MMSYSERR_NOERROR;
				}
			}

			LOG(WDMErr, "Failed to free synthesizer.");
			return MMSYSERR_ERROR;

		case MODM_GETNUMDEVS:
			return WDMSettings->IsBlacklistedProcess() ? 0 : 1;

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
		freeAudioRender();
		if (getAudioRender()) {
			if (!SynthModule->LoadSynthModule()) {
				NERROR(WDMErr, "Unable to initialize the synthesizer module!\n\nPress OK to continue without audio.", true);

				if (!SynthModule->UnloadSynthModule())
					FNERROR(WDMErr, "UnloadSynthModule failed in MODM_OPEN!\n\nABORT!!!");

				freeAudioRender();
				return 0;
			}

			if (!SynthModule->StartSynthModule()) {
				NERROR(WDMErr, "Unable to start up the synthesizer module!\n\nPress OK to continue without audio.", true);
				freeAudioRender();
				return 0;
			}

			LOG(WDMErr, "Synth ID: %x", SynthModule->SynthID());

			return 1;
		}

		NERROR(WDMErr, "Failed to initialize synthesizer.", true);
		return 0;
	}

	__declspec(dllexport)
	int KDMAPI TerminateKDMAPIStream() {
		if (SynthModule->StopSynthModule()) {
			if (SynthModule->UnloadSynthModule()) {
				freeAudioRender();
				return 1;
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
		return SynthModule->SettingsManager(setting, (bool)mode, value, (size_t)cbValue);
	}
}