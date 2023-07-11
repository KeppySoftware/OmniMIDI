/*
	BASSASIO 1.4 C/C++ header file
	Copyright (c) 2005-2019 Un4seen Developments Ltd.

	See the BASSASIO.CHM file for more detailed documentation
*/

#ifndef BASSASIO_H
#define BASSASIO_H

#include <wtypes.h>

#define BASSASIOVERSION 0x104	// API version

// error codes returned by BASS_ASIO_ErrorGetCode
#define BASS_OK				0	// all is OK
#define BASS_ERROR_FILEOPEN	2	// can't open the file
#define BASS_ERROR_DRIVER	3	// can't find a free/valid driver
#define BASS_ERROR_HANDLE	5	// invalid handle
#define BASS_ERROR_FORMAT	6	// unsupported sample format
#define BASS_ERROR_INIT		8	// BASS_ASIO_Init has not been successfully called
#define BASS_ERROR_START	9	// BASS_ASIO_Start has/hasn't been called
#define BASS_ERROR_ALREADY	14	// already initialized/started
#define BASS_ERROR_NOCHAN	18	// no channels are enabled
#define BASS_ERROR_ILLPARAM	20	// an illegal parameter was specified
#define BASS_ERROR_DEVICE	23	// illegal device number
#define BASS_ERROR_NOTAVAIL	37	// not available
#define BASS_ERROR_UNKNOWN	-1	// some other mystery error

// BASS_ASIO_Init flags
#define BASS_ASIO_THREAD	1 // host driver in dedicated thread
#define BASS_ASIO_JOINORDER	2 // order joined channels by when they were joined

// device info structure
typedef struct {
	const char *name;	// description
	const char *driver;	// driver
} BASS_ASIO_DEVICEINFO;

typedef struct {
	char name[32];	// driver name
	DWORD version;	// driver version
	DWORD inputs;	// number of inputs
	DWORD outputs;	// number of outputs
	DWORD bufmin;	// minimum buffer length
	DWORD bufmax;	// maximum buffer length
	DWORD bufpref;	// preferred/default buffer length
	int bufgran;	// buffer length granularity
	DWORD initflags; // BASS_ASIO_Init "flags" parameter
} BASS_ASIO_INFO;

typedef struct {
	DWORD group;
	DWORD format;	// sample format (BASS_ASIO_FORMAT_xxx)
	char name[32];	// channel name
} BASS_ASIO_CHANNELINFO;

// sample formats
#define BASS_ASIO_FORMAT_16BIT		16 // 16-bit integer
#define BASS_ASIO_FORMAT_24BIT		17 // 24-bit integer
#define BASS_ASIO_FORMAT_32BIT		18 // 32-bit integer
#define BASS_ASIO_FORMAT_FLOAT		19 // 32-bit floating-point
#define BASS_ASIO_FORMAT_DSD_LSB	32 // DSD (LSB 1st)
#define BASS_ASIO_FORMAT_DSD_MSB	33 // DSD (MSB 1st)
#define BASS_ASIO_FORMAT_DITHER		0x100 // flag: apply dither when converting from floating-point to integer

// BASS_ASIO_ChannelReset flags
#define BASS_ASIO_RESET_ENABLE	1 // disable channel
#define BASS_ASIO_RESET_JOIN	2 // unjoin channel
#define BASS_ASIO_RESET_PAUSE	4 // unpause channel
#define BASS_ASIO_RESET_FORMAT	8 // reset sample format to native format
#define BASS_ASIO_RESET_RATE	16 // reset sample rate to device rate
#define BASS_ASIO_RESET_VOLUME	32 // reset volume to 1.0
#define BASS_ASIO_RESET_JOINED	0x10000 // apply to joined channels too

// BASS_ASIO_ChannelIsActive return values
#define BASS_ASIO_ACTIVE_DISABLED	0
#define BASS_ASIO_ACTIVE_ENABLED	1
#define BASS_ASIO_ACTIVE_PAUSED		2

typedef DWORD (CALLBACK ASIOPROC)(BOOL input, DWORD channel, void *buffer, DWORD length, void *user);
/* ASIO channel callback function.
input  : Input? else output
channel: Channel number
buffer : Buffer containing the sample data
length : Number of bytes
user   : The 'user' parameter given when calling BASS_ASIO_ChannelEnable
RETURN : The number of bytes written (ignored with input channels) */

typedef void (CALLBACK ASIONOTIFYPROC)(DWORD notify, void *user);
/* Driver notification callback function.
notify : The notification (BASS_ASIO_NOTIFY_xxx)
user   : The 'user' parameter given when calling BASS_ASIO_SetNotify */

// driver notifications
#define BASS_ASIO_NOTIFY_RATE	1 // sample rate change
#define BASS_ASIO_NOTIFY_RESET	2 // reset (reinitialization) request

// BASS_ASIO_ChannelGetLevel flags
#define BASS_ASIO_LEVEL_RMS		0x1000000

extern unsigned int (WINAPI *BASS_ASIO_GetVersion)();
extern int (WINAPI *BASS_ASIO_SetUnicode)(int unicode);
extern unsigned int (WINAPI *BASS_ASIO_ErrorGetCode)();
extern int (WINAPI *BASS_ASIO_GetDeviceInfo)(unsigned int device, BASS_ASIO_DEVICEINFO *info);
extern unsigned int (WINAPI *BASS_ASIO_AddDevice)(const GUID *clsid, const char *driver, const char *name);
extern int (WINAPI *BASS_ASIO_SetDevice)(unsigned int device);
extern unsigned int (WINAPI *BASS_ASIO_GetDevice)();
extern int (WINAPI *BASS_ASIO_Init)(int device, unsigned int flags);
extern int (WINAPI *BASS_ASIO_Free)();
extern int (WINAPI *BASS_ASIO_Lock)(int lock);
extern int (WINAPI *BASS_ASIO_SetNotify)(ASIONOTIFYPROC *proc, void *user);
extern int (WINAPI *BASS_ASIO_ControlPanel)();
extern int (WINAPI *BASS_ASIO_GetInfo)(BASS_ASIO_INFO *info);
extern int (WINAPI *BASS_ASIO_CheckRate)(double rate);
extern int (WINAPI *BASS_ASIO_SetRate)(double rate);
extern double (WINAPI *BASS_ASIO_GetRate)();
extern int (WINAPI *BASS_ASIO_Start)(unsigned int buflen, unsigned int threads);
extern int (WINAPI *BASS_ASIO_Stop)();
extern int (WINAPI *BASS_ASIO_IsStarted)();
extern unsigned int (WINAPI *BASS_ASIO_GetLatency)(int input);
extern float (WINAPI *BASS_ASIO_GetCPU)();
extern int (WINAPI *BASS_ASIO_Monitor)(int input, unsigned int output, unsigned int gain, unsigned int state, unsigned int pan);
extern int (WINAPI *BASS_ASIO_SetDSD)(int dsd);
extern int (WINAPI *BASS_ASIO_Future)(unsigned int selector, void *param);
 
extern int (WINAPI *BASS_ASIO_ChannelGetInfo)(int input, unsigned int channel, BASS_ASIO_CHANNELINFO *info);
extern int (WINAPI *BASS_ASIO_ChannelReset)(int input, int channel, unsigned int flags);
extern int (WINAPI *BASS_ASIO_ChannelEnable)(int input, unsigned int channel, ASIOPROC *proc, void *user);
extern int (WINAPI *BASS_ASIO_ChannelEnableMirror)(unsigned int channel, int input2, unsigned int channel2);
extern int (WINAPI *BASS_ASIO_ChannelEnableBASS)(int input, unsigned int channel, unsigned int handle, int join);
extern int (WINAPI *BASS_ASIO_ChannelJoin)(int input, unsigned int channel, int channel2);
extern int (WINAPI *BASS_ASIO_ChannelPause)(int input, unsigned int channel);
extern unsigned int (WINAPI *BASS_ASIO_ChannelIsActive)(int input, unsigned int channel);
extern int (WINAPI *BASS_ASIO_ChannelSetFormat)(int input, unsigned int channel, unsigned int format);
extern unsigned int (WINAPI *BASS_ASIO_ChannelGetFormat)(int input, unsigned int channel);
extern int (WINAPI *BASS_ASIO_ChannelSetRate)(int input, unsigned int channel, double rate);
extern double (WINAPI *BASS_ASIO_ChannelGetRate)(int input, unsigned int channel);
extern int (WINAPI *BASS_ASIO_ChannelSetVolume)(int input, int channel, float volume);
extern float (WINAPI *BASS_ASIO_ChannelGetVolume)(int input, int channel);
extern float (WINAPI *BASS_ASIO_ChannelGetLevel)(int input, unsigned int channel);

#endif
