/*
* 
	OmniMIDI v15+ (Rewrite) for Windows NT

	This file contains the required code to run the driver under Windows 7 SP1 and later.
	This file is useful only if you want to compile the driver under Windows, it's not needed for Linux/macOS porting.

*/

#include "pch.h"
#include "WDMDrv.h"

#ifdef _WIN32

bool WinDriver::DriverMask::ChangeSettings(short NMID, short NPID, short NT, short NS) {
	// TODO: Check if the values contain valid technologies/support flags

	this->ManufacturerID = NMID;
	this->ProductID = NPID;
	this->Technology = NT;
	this->Support = NS;

	return true;
}

unsigned long WinDriver::DriverMask::GiveCaps(UINT DeviceIdentifier, PVOID CapsPointer, DWORD CapsSize) {
	wchar_t DevName[MAXPNAMELEN] = { 0 };
	MIDIOUTCAPSA CapsA;
	MIDIOUTCAPSW CapsW;
	MIDIOUTCAPS2A Caps2A;
	MIDIOUTCAPS2W Caps2W;
	size_t WCSTSRetVal;

	// Why would this happen? Stupid MIDI app dev smh
	if (CapsPointer == nullptr)
	{
		NERROR(MaskErr, L"A null pointer has been passed to the function. The driver can't share its info with the application.", false);
		return MMSYSERR_INVALPARAM;
	}

	if (CapsSize == 0) {
		NERROR(MaskErr, L"CapsSize has a value of 0, how is the driver supposed to determine the subtype of the struct?", false);
		return MMSYSERR_INVALPARAM;
	}

	swprintf_s(DevName, MAXPNAMELEN, this->TemplateName);

	// I have to support all this s**t or else it won't work in some apps smh
	switch (CapsSize) {
	case (sizeof(MIDIOUTCAPSA)):
		wcstombs_s(&WCSTSRetVal, CapsA.szPname, sizeof(CapsA.szPname), DevName, MAXPNAMELEN);
		CapsA.dwSupport = this->Support;
		CapsA.wChannelMask = 0xFFFF;
		CapsA.wMid = this->ManufacturerID;
		CapsA.wPid = this->ProductID;
		CapsA.wTechnology = this->Technology;
		CapsA.wNotes = 65535;
		CapsA.wVoices = 65535;
		CapsA.vDriverVersion = MAKEWORD(6, 2);
		memcpy((LPMIDIOUTCAPSA)CapsPointer, &CapsA, min(CapsSize, sizeof(CapsA)));
		break;

	case (sizeof(MIDIOUTCAPSW)):
		wcsncpy_s(CapsW.szPname, DevName, MAXPNAMELEN);
		CapsW.dwSupport = this->Support;
		CapsW.wChannelMask = 0xFFFF;
		CapsW.wMid = this->ManufacturerID;
		CapsW.wPid = this->ProductID;
		CapsW.wTechnology = this->Technology;
		CapsW.wNotes = 65535;
		CapsW.wVoices = 65535;
		CapsW.vDriverVersion = MAKEWORD(6, 2);
		memcpy((LPMIDIOUTCAPSW)CapsPointer, &CapsW, min(CapsSize, sizeof(CapsW)));
		break;

	case (sizeof(MIDIOUTCAPS2A)):
		wcstombs_s(&WCSTSRetVal, Caps2A.szPname, sizeof(Caps2A.szPname), DevName, MAXPNAMELEN);
		Caps2A.dwSupport = this->Support;
		Caps2A.wChannelMask = 0xFFFF;
		Caps2A.wMid = this->ManufacturerID;
		Caps2A.wPid = this->ProductID;
		Caps2A.wTechnology = this->Technology;
		Caps2A.wNotes = 65535;
		Caps2A.wVoices = 65535;
		Caps2A.vDriverVersion = MAKEWORD(6, 2);
		memcpy((LPMIDIOUTCAPS2A)CapsPointer, &Caps2A, min(CapsSize, sizeof(Caps2A)));
		break;

	case (sizeof(MIDIOUTCAPS2W)):
		wcsncpy_s(Caps2W.szPname, DevName, MAXPNAMELEN);
		Caps2W.dwSupport = this->Support;
		Caps2W.wChannelMask = 0xFFFF;
		Caps2W.wMid = this->ManufacturerID;
		Caps2W.wPid = this->ProductID;
		Caps2W.wTechnology = this->Technology;
		Caps2W.wNotes = 65535;
		Caps2W.wVoices = 65535;
		Caps2W.vDriverVersion = MAKEWORD(6, 2);
		memcpy((LPMIDIOUTCAPS2W)CapsPointer, &Caps2W, min(CapsSize, sizeof(Caps2W)));
		break;

	default:
		// ???????
		NERROR(MaskErr, L"Size passed to CapsSize does not match any valid MIDIOUTCAPS struct.", false);
		return MMSYSERR_INVALPARAM;

	}

	return MMSYSERR_NOERROR;
}

bool WinDriver::DriverCallback::PrepareCallbackFunction(MIDIOPENDESC* OpInfStruct, DWORD CallbackMode) {
	// Save the pointer's address to memory
	this->WMMHandle = OpInfStruct->hMidi;

	// Check if the app wants the driver to do callbacks
	if (CallbackMode != CALLBACK_NULL) {
		if (OpInfStruct->dwCallback != 0) {
			this->Callback = OpInfStruct->dwCallback;
			this->CallbackMode = CallbackMode;
			this->Instance = OpInfStruct->dwInstance;
		}

		// If callback mode is specified but no callback address is specified, abort the initialization
		else {
			NERROR(CallbackErr, L"No memory address has been specified for the callback function.", false);
			return false;
		}
	}

	return true;
}

bool WinDriver::DriverCallback::ClearCallbackFunction() {
	this->WMMHandle = nullptr;
	this->Callback = 0;
	this->CallbackMode = CALLBACK_NULL;
	this->Instance = 0;

	return true;
}

void WinDriver::DriverCallback::CallbackFunction(DWORD Message, DWORD_PTR Arg1, DWORD_PTR Arg2) {
	WMMC Callback = nullptr;
	int ReturnMessage = 0;

	switch (this->CallbackMode & CALLBACK_TYPEMASK) {

	case CALLBACK_FUNCTION:	// Use a custom function to notify the app
		// Use the app's custom function to send the callback
		(*(WMMC)this->Callback)((HMIDIOUT)(this->WMMHandle), Message, this->Instance, Arg1, Arg2);
		break;

	case CALLBACK_EVENT:	// Set an event to notify the app
		ReturnMessage = SetEvent((HANDLE)(this->Callback));
		break;

	case CALLBACK_THREAD:	// Send a message to a thread to notify the app
		ReturnMessage = PostThreadMessage((DWORD)(this->Callback), Message, Arg1, Arg2);
		break;

	case CALLBACK_WINDOW:	// Send a message to the app's main window
		ReturnMessage = PostMessage((HWND)(this->Callback), Message, Arg1, Arg2);
		break;

	default:				// stub
		break;

	}
}

bool WinDriver::DriverComponent::SetDriverHandle(HDRVR Handle) {
	// The app tried to initialize the driver with no pointer?
	if (Handle == nullptr) {
		NERROR(DrvErr, L"A null pointer has been passed to the function.", true);
		return false;
	}

	// We already have the same pointer in memory.
	if (this->DrvHandle == Handle) {
		LOG(DrvErr, L"We already have the handle stored in memory. The app has Alzheimer's I guess?");
		return true;
	}

	// A pointer is already stored in the variable, UnSetDriverHandle hasn't been called
	if (this->DrvHandle != nullptr) {
		NERROR(DrvErr, L"DrvHandle has been set in a previous call and not freed.", false);
		return false;
	}

	// All good, save the pointer to a local variable and return true
	this->DrvHandle = Handle;
	return true;
}

bool WinDriver::DriverComponent::UnsetDriverHandle() {
	// Warn through stdout if the app is trying to free the driver twice
	if (this->DrvHandle == nullptr)
		LOG(DrvErr, L"The application called UnsetDriverHandle even though there's no handle set. Bad design?");

	// Free the driver by setting the local variable to nullptr, then return true
	this->DrvHandle = nullptr;
	return true;
}

bool WinDriver::DriverComponent::OpenDriver(MIDIOPENDESC* OpInfStruct, DWORD CallbackMode, DWORD_PTR CookedPlayerAddress) {
	if (OpInfStruct->hMidi == nullptr) {
		NERROR(DrvErr, L"No valid HMIDI pointer has been specified.", false);
		return false;
	}

	// Check if the app wants a cooked player
	if (CallbackMode & 0x00000002L) {
		if (CookedPlayerAddress == NULL) {
			NERROR(DrvErr, L"No memory address has been specified for the MIDI_IO_COOKED player.", false);
			return false;
		}
		// stub
	}

	// Everything is hunky-dory, proceed
	return true;
}

bool WinDriver::DriverComponent::CloseDriver() {
	// stub
	return true;
}

#endif