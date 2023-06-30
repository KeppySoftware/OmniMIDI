/*

	OmniMIDI v15+ (Rewrite) for Windows NT

	This file contains the required code to run the driver under Windows 7 SP1 and later.
	This file is useful only if you want to compile the driver under Windows, it's not needed for Linux/macOS porting.

*/

#include "pch.h"
#include <basswasapi.h>

DWORD(WINAPI* BASS_WASAPI_GetVersion)() = 0;
BOOL(WINAPI* BASS_WASAPI_SetNotify)(WASAPINOTIFYPROC* proc, void* user) = 0;
BOOL(WINAPI* BASS_WASAPI_GetDeviceInfo)(DWORD device, BASS_WASAPI_DEVICEINFO* info) = 0;
float (WINAPI* BASS_WASAPI_GetDeviceLevel)(DWORD device, int chan) = 0;
BOOL(WINAPI* BASS_WASAPI_SetDevice)(DWORD device) = 0;
DWORD(WINAPI* BASS_WASAPI_GetDevice)() = 0;
DWORD(WINAPI* BASS_WASAPI_CheckFormat)(DWORD device, DWORD freq, DWORD chans, DWORD flags) = 0;
BOOL(WINAPI* BASS_WASAPI_Init)(int device, DWORD freq, DWORD chans, DWORD flags, float buffer, float period, WASAPIPROC* proc, void* user) = 0;
BOOL(WINAPI* BASS_WASAPI_Free)() = 0;
BOOL(WINAPI* BASS_WASAPI_GetInfo)(BASS_WASAPI_INFO* info) = 0;
float (WINAPI* BASS_WASAPI_GetCPU)() = 0;
BOOL(WINAPI* BASS_WASAPI_Lock)(BOOL lock) = 0;
BOOL(WINAPI* BASS_WASAPI_Start)() = 0;
BOOL(WINAPI* BASS_WASAPI_Stop)(BOOL reset) = 0;
BOOL(WINAPI* BASS_WASAPI_IsStarted)() = 0;
BOOL(WINAPI* BASS_WASAPI_SetVolume)(DWORD mode, float volume) = 0;
float (WINAPI* BASS_WASAPI_GetVolume)(DWORD mode) = 0;
BOOL(WINAPI* BASS_WASAPI_SetMute)(DWORD mode, BOOL mute) = 0;
BOOL(WINAPI* BASS_WASAPI_GetMute)(DWORD mode) = 0;
DWORD(WINAPI* BASS_WASAPI_PutData)(void* buffer, DWORD length) = 0;
DWORD(WINAPI* BASS_WASAPI_GetData)(void* buffer, DWORD length) = 0;
DWORD(WINAPI* BASS_WASAPI_GetLevel)() = 0;
BOOL(WINAPI* BASS_WASAPI_GetLevelEx)(float* levels, float length, DWORD flags) = 0;