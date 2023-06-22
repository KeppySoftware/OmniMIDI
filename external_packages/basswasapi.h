/*
	BASSWASAPI 2.4 C/C++ header file
	Copyright (c) 2009-2020 Un4seen Developments Ltd.

	See the BASSWASAPI.CHM file for more detailed documentation
*/

#ifndef BASSWASAPI_H
#define BASSWASAPI_H

#include "bass.h"

#ifdef __cplusplus
extern "C" {
#endif

#ifndef BASSWASAPIDEF
#define BASSWASAPIDEF(f) WINAPI f
#endif

// Additional error codes returned by BASS_ErrorGetCode
#define BASS_ERROR_WASAPI			5000	// no WASAPI
#define BASS_ERROR_WASAPI_BUFFER	5001	// buffer size is invalid
#define BASS_ERROR_WASAPI_CATEGORY	5002	// can't set category
#define BASS_ERROR_WASAPI_DENIED	5003	// access denied

// Device info structure
typedef struct {
	const char *name;
	const char *id;
	DWORD type;
	DWORD flags;
	float minperiod;
	float defperiod;
	DWORD mixfreq;
	DWORD mixchans;
} BASS_WASAPI_DEVICEINFO;

typedef struct {
	DWORD initflags;
	DWORD freq;
	DWORD chans;
	DWORD format;
	DWORD buflen;
	float volmax;
	float volmin;
	float volstep;
} BASS_WASAPI_INFO;

// BASS_WASAPI_DEVICEINFO "type"
#define BASS_WASAPI_TYPE_NETWORKDEVICE	0
#define BASS_WASAPI_TYPE_SPEAKERS		1
#define BASS_WASAPI_TYPE_LINELEVEL		2
#define BASS_WASAPI_TYPE_HEADPHONES		3
#define BASS_WASAPI_TYPE_MICROPHONE		4
#define BASS_WASAPI_TYPE_HEADSET		5
#define BASS_WASAPI_TYPE_HANDSET		6
#define BASS_WASAPI_TYPE_DIGITAL		7
#define BASS_WASAPI_TYPE_SPDIF			8
#define BASS_WASAPI_TYPE_HDMI			9
#define BASS_WASAPI_TYPE_UNKNOWN		10

// BASS_WASAPI_DEVICEINFO flags
#define BASS_DEVICE_ENABLED		1
#define BASS_DEVICE_DEFAULT		2
#define BASS_DEVICE_INIT		4
#define BASS_DEVICE_LOOPBACK	8
#define BASS_DEVICE_INPUT		16
#define BASS_DEVICE_UNPLUGGED	32
#define BASS_DEVICE_DISABLED	64

// BASS_WASAPI_Init flags
#define BASS_WASAPI_EXCLUSIVE	1
#define BASS_WASAPI_AUTOFORMAT	2
#define BASS_WASAPI_BUFFER		4
#define BASS_WASAPI_EVENT		16
#define BASS_WASAPI_SAMPLES		32
#define BASS_WASAPI_DITHER		64
#define BASS_WASAPI_RAW			128
#define BASS_WASAPI_ASYNC		0x100

#define BASS_WASAPI_CATEGORY_MASK					0xf000
#define BASS_WASAPI_CATEGORY_OTHER					0x0000
#define BASS_WASAPI_CATEGORY_FOREGROUNDONLYMEDIA	0x1000
#define BASS_WASAPI_CATEGORY_BACKGROUNDCAPABLEMEDIA	0x2000
#define BASS_WASAPI_CATEGORY_COMMUNICATIONS			0x3000
#define BASS_WASAPI_CATEGORY_ALERTS					0x4000
#define BASS_WASAPI_CATEGORY_SOUNDEFFECTS			0x5000
#define BASS_WASAPI_CATEGORY_GAMEEFFECTS			0x6000
#define BASS_WASAPI_CATEGORY_GAMEMEDIA				0x7000
#define BASS_WASAPI_CATEGORY_GAMECHAT				0x8000
#define BASS_WASAPI_CATEGORY_SPEECH					0x9000
#define BASS_WASAPI_CATEGORY_MOVIE					0xa000
#define BASS_WASAPI_CATEGORY_MEDIA					0xb000

// BASS_WASAPI_INFO "format"
#define BASS_WASAPI_FORMAT_FLOAT	0
#define BASS_WASAPI_FORMAT_8BIT		1
#define BASS_WASAPI_FORMAT_16BIT	2
#define BASS_WASAPI_FORMAT_24BIT	3
#define BASS_WASAPI_FORMAT_32BIT	4

// BASS_WASAPI_Set/GetVolume modes
#define BASS_WASAPI_CURVE_DB		0
#define BASS_WASAPI_CURVE_LINEAR	1
#define BASS_WASAPI_CURVE_WINDOWS	2
#define BASS_WASAPI_VOL_SESSION		8

typedef DWORD (CALLBACK WASAPIPROC)(void *buffer, DWORD length, void *user);
/* WASAPI callback function.
buffer : Buffer containing the sample data
length : Number of bytes
user   : The 'user' parameter given when calling BASS_WASAPI_Init
RETURN : The number of bytes written (output devices), 0/1 = stop/continue (input devices) */

// Special WASAPIPROCs
#define WASAPIPROC_PUSH		(WASAPIPROC*)0		// push output
#define WASAPIPROC_BASS		(WASAPIPROC*)-1		// BASS channel

typedef void (CALLBACK WASAPINOTIFYPROC)(DWORD notify, DWORD device, void *user);
/* WASAPI device notification callback function.
notify : The notification (BASS_WASAPI_NOTIFY_xxx)
device : Device that the notification applies to
user   : The 'user' parameter given when calling BASS_WASAPI_SetNotify */

// Device notifications
#define BASS_WASAPI_NOTIFY_ENABLED		0
#define BASS_WASAPI_NOTIFY_DISABLED		1
#define BASS_WASAPI_NOTIFY_DEFOUTPUT	2
#define BASS_WASAPI_NOTIFY_DEFINPUT		3
#define BASS_WASAPI_NOTIFY_FAIL			0x100

DWORD BASSWASAPIDEF(BASS_WASAPI_GetVersion)();
BOOL BASSWASAPIDEF(BASS_WASAPI_SetNotify)(WASAPINOTIFYPROC *proc, void *user);
BOOL BASSWASAPIDEF(BASS_WASAPI_GetDeviceInfo)(DWORD device, BASS_WASAPI_DEVICEINFO *info);
float BASSWASAPIDEF(BASS_WASAPI_GetDeviceLevel)(DWORD device, int chan);
BOOL BASSWASAPIDEF(BASS_WASAPI_SetDevice)(DWORD device);
DWORD BASSWASAPIDEF(BASS_WASAPI_GetDevice)();
DWORD BASSWASAPIDEF(BASS_WASAPI_CheckFormat)(DWORD device, DWORD freq, DWORD chans, DWORD flags);
BOOL BASSWASAPIDEF(BASS_WASAPI_Init)(int device, DWORD freq, DWORD chans, DWORD flags, float buffer, float period, WASAPIPROC *proc, void *user);
BOOL BASSWASAPIDEF(BASS_WASAPI_Free)();
BOOL BASSWASAPIDEF(BASS_WASAPI_GetInfo)(BASS_WASAPI_INFO *info);
float BASSWASAPIDEF(BASS_WASAPI_GetCPU)();
BOOL BASSWASAPIDEF(BASS_WASAPI_Lock)(BOOL lock);
BOOL BASSWASAPIDEF(BASS_WASAPI_Start)();
BOOL BASSWASAPIDEF(BASS_WASAPI_Stop)(BOOL reset);
BOOL BASSWASAPIDEF(BASS_WASAPI_IsStarted)();
BOOL BASSWASAPIDEF(BASS_WASAPI_SetVolume)(DWORD mode, float volume);
float BASSWASAPIDEF(BASS_WASAPI_GetVolume)(DWORD mode);
BOOL BASSWASAPIDEF(BASS_WASAPI_SetMute)(DWORD mode, BOOL mute);
BOOL BASSWASAPIDEF(BASS_WASAPI_GetMute)(DWORD mode);
DWORD BASSWASAPIDEF(BASS_WASAPI_PutData)(void *buffer, DWORD length);
DWORD BASSWASAPIDEF(BASS_WASAPI_GetData)(void *buffer, DWORD length);
DWORD BASSWASAPIDEF(BASS_WASAPI_GetLevel)();
BOOL BASSWASAPIDEF(BASS_WASAPI_GetLevelEx)(float *levels, float length, DWORD flags);

#ifdef __cplusplus
}
#endif

#endif
