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

static NT::Funcs NTFuncs;
static signed long long TickStart = 0;

extern "C" {
	__declspec(dllexport)
	BOOL APIENTRY DllMain(HMODULE hModule, DWORD ReasonForCall, LPVOID lpReserved)
	{
		BOOL ret = FALSE;
		switch (ReasonForCall)
		{
		case DLL_PROCESS_ATTACH:
			if (!TickStart) {
				if (!(NTFuncs.querySystemTime(&TickStart) == 0)) {
					OutputDebugStringA("Failed to parse starting tick through NtQuerySystemTime! OmniMIDI will not load.");
					return FALSE;
				}
			}

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
				DriverComponent = nullptr;
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
		if (extModule != nullptr) {
			if (SynthModule) {
				auto fM = (rStopModule)GetProcAddress(extModule, "freeModule");
				fM();
			}

			FreeLibrary(extModule);
			extModule = nullptr;
			SynthModule = nullptr;
		}
		
		if (SynthModule != nullptr)
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

	bool getAudioRender() {
		bool good = true;

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
						good = false;
				}
				else good = false;
			}
			else good = false;
			break;
		case BASSMIDI:
#ifndef _M_ARM
			SynthModule = new OmniMIDI::BASSSynth;
#else
			NERROR(WDMErr, "BASSMIDI is not available on ARM Thumb-2.", true);
			good = false;
#endif
			break;
		case FLUIDSYNTH:
			SynthModule = new OmniMIDI::FluidSynth;
			break;

		case XSYNTH:
#ifdef _M_AMD64
			SynthModule = new OmniMIDI::XSynth;
#else
			NERROR(WDMErr, "XSynth is only available on AMD64 platforms.", true);
			good = false;
#endif
			break;

		case TINYSF:
			SynthModule = new OmniMIDI::TinySFSynth;
			break;
		default:
			good = false;
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
			return (SynthModule->PlayShortEvent((DWORD)Param1) == SYNTH_OK) ? MMSYSERR_NOERROR : MMSYSERR_INVALPARAM;

		case MODM_LONGDATA:
			fDriverCallback->CallbackFunction(MOM_DONE, (DWORD)Param1, 0);
			return (SynthModule->PlayLongEvent(((LPMIDIHDR)Param1)->lpData, ((LPMIDIHDR)Param1)->dwBytesRecorded) == SYNTH_OK) ? MMSYSERR_NOERROR : MMSYSERR_INVALPARAM;

		case MODM_RESET:
			return (SynthModule->PlayShortEvent(0x0101FF) == SYNTH_OK) ? MMSYSERR_NOERROR : MMSYSERR_INVALPARAM;

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
				fDriverCallback->CallbackFunction(MOM_CLOSE, 0, 0);
				fDriverCallback->ClearCallbackFunction();
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
	unsigned int KDMAPI SendDirectLongData(MIDIHDR* IIMidiHdr, UINT IIMidiHdrSize) {
		fDriverCallback->CallbackFunction(MOM_DONE, (DWORD_PTR)IIMidiHdr, 0);
		return SynthModule->PlayLongEvent(IIMidiHdr->lpData, IIMidiHdr->dwBytesRecorded) == SYNTH_OK ? MMSYSERR_NOERROR : MMSYSERR_INVALPARAM;
	}

	__declspec(dllexport)
	unsigned int KDMAPI PrepareLongData(MIDIHDR* IIMidiHdr, UINT IIMidiHdrSize) {
		if (!IIMidiHdr || sizeof(IIMidiHdr->lpData) > 65536) return MMSYSERR_INVALPARAM;	// The buffer doesn't exist or is too big, invalid parameter

		// Mark the buffer as prepared, and return MMSYSERR_NOERROR
		IIMidiHdr->dwFlags |= MHDR_PREPARED;

		fDriverCallback->CallbackFunction(MOM_DONE, (DWORD_PTR)IIMidiHdr, 0);
		return MMSYSERR_NOERROR;
	}

	__declspec(dllexport)
	unsigned int KDMAPI UnprepareLongData(MIDIHDR* IIMidiHdr, UINT IIMidiHdrSize) {
		if (!IIMidiHdr) return MMSYSERR_INVALPARAM;								// The buffer doesn't exist, invalid parameter
		if (!(IIMidiHdr->dwFlags & MHDR_PREPARED)) return MMSYSERR_NOERROR;		// Already unprepared, everything is fine
		if (IIMidiHdr->dwFlags & MHDR_INQUEUE) return MIDIERR_STILLPLAYING;		// The buffer is currently being played from the driver, cannot unprepare

		// Mark the buffer as unprepared
		IIMidiHdr->dwFlags &= ~MHDR_PREPARED;

		fDriverCallback->CallbackFunction(MOM_DONE, (DWORD_PTR)IIMidiHdr, 0);
		return MMSYSERR_NOERROR;
	}

	__declspec(dllexport)
	int KDMAPI InitializeCallbackFeatures(HMIDI OMHM, DWORD_PTR OMCB, DWORD_PTR OMI, DWORD_PTR OMU, DWORD OMCM) {
		MIDIOPENDESC MidiP;

		MidiP.hMidi = OMHM;
		MidiP.dwCallback = OMCB;
		MidiP.dwInstance = OMI;

		if (!fDriverCallback->PrepareCallbackFunction(&MidiP, OMCM))
			return FALSE;

		fDriverCallback->CallbackFunction(MOM_OPEN, 0, 0);
		return TRUE;
	}

	__declspec(dllexport)
	void KDMAPI RunCallbackFunction(DWORD msg, DWORD_PTR p1, DWORD_PTR p2) {
		fDriverCallback->CallbackFunction(msg, p1, p2);
	}

	__declspec(dllexport)
	int KDMAPI SendCustomEvent(unsigned int evt, unsigned int chan, unsigned int param) {
		return SynthModule->TalkToSynthDirectly(evt, chan, param);
	}

	__declspec(dllexport)
	int KDMAPI DriverSettings(unsigned int setting, unsigned int mode, void* value, unsigned int cbValue) {
		return SynthModule->SettingsManager(setting, (bool)mode, value, (size_t)cbValue);
	}

	__declspec(dllexport)
	unsigned long long KDMAPI timeGetTime64() {
		signed long long CurrentTime;
		NTFuncs.querySystemTime(&CurrentTime);
		return (unsigned long long)((CurrentTime) - TickStart) / 10000.0;
	}
}