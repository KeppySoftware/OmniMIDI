/*
Keppy's Driver, a fork of BASSMIDI Driver
*/

#define STRICT

#if __DMC__
unsigned long _beginthreadex(void *security, unsigned stack_size,
	unsigned(__stdcall *start_address)(void *), void *arglist,
	unsigned initflag, unsigned *thrdaddr);
void _endthreadex(unsigned retval);
#endif

#define _CRT_SECURE_CPP_OVERLOAD_STANDARD_NAMES 1
#define _CRT_SECURE_NO_WARNINGS 1
#include <assert.h>
#include <atlbase.h>
#include <atlstr.h>
#include <stdarg.h>
#include <stdio.h>
#include <stdlib.h>
#include <windows.h>
#include <process.h>
#include <tchar.h>
#include <limits>
#include <vector>
#include <signal.h>
#include <list>
#include <sstream>
#include <string>
#include <shlobj.h>
#include <fstream>
#include <iostream>
#include <winbase.h>
#include <string.h>
#include <comdef.h>

#define BASSDEF(f) (WINAPI *f)	// define the BASS/BASSMIDI functions as pointers
#define BASSMIDIDEF(f) (WINAPI *f)	
#define BASSENCDEF(f) (WINAPI *f)	
#define LOADBASSFUNCTION(f) *((void**)&f)=GetProcAddress(bass,#f)
#define LOADBASSMIDIFUNCTION(f) *((void**)&f)=GetProcAddress(bassmidi,#f)
#define LOADBASSENCFUNCTION(f) *((void**)&f)=GetProcAddress(bassenc,#f)

#include <bass.h>
#include <bassmidi.h>
#include <bassenc.h>

struct evbuf_t_old{
	UINT uMsg;
	DWORD	dwParam1;
	DWORD	dwParam2;
	int exlen;
	char *sysexbuffer;
};

struct evbuf_t{
	UINT uDeviceID;
	UINT   uMsg;
	DWORD_PTR	dwParam1;
	DWORD_PTR	dwParam2;
	int exlen;
	unsigned char *sysexbuffer;
};

#define EVBUFF_SIZE_OLD 0x80000
static struct evbuf_t_old evbuf_old[EVBUFF_SIZE_OLD];
static UINT evbwpoint_old = 0;
static UINT evbrpoint_old = 0;
static UINT evbsysexpoint_old;

#define EVBUFF_SIZE 0x3FFFC
static struct evbuf_t evbuf[EVBUFF_SIZE];
static UINT  evbwpoint = 0;
static UINT  evbrpoint = 0;
static volatile LONG evbcount = 0;
static UINT evbsysexpoint;

// Old buffer system from BASSMIDI Driver 1.x
int bmsyn_buf_check_old(CRITICAL_SECTION mim2){
	int retval;
	EnterCriticalSection(&mim2);
	retval = (evbrpoint_old != evbwpoint_old) ? ~0 : 0;
	LeaveCriticalSection(&mim2);
	return retval;
}

int bmsyn_play_some_data_old(HSTREAM stream, CRITICAL_SECTION mim){
	UINT uMsg;
	DWORD_PTR	dwParam1;
	DWORD_PTR   dwParam2;

	UINT evbpoint;
	int exlen;
	char *sysexbuffer;
	int played;

	played = 0;
	if (!bmsyn_buf_check_old(mim)){
		played = ~0;
		return played;
	}
	do{
		EnterCriticalSection(&mim);
		evbpoint = evbrpoint_old;
		if (++evbrpoint_old >= EVBUFF_SIZE_OLD)
			evbrpoint_old -= EVBUFF_SIZE_OLD;

		uMsg = evbuf_old[evbpoint].uMsg;
		dwParam1 = evbuf_old[evbpoint].dwParam1;
		dwParam2 = evbuf_old[evbpoint].dwParam2;
		exlen = evbuf_old[evbpoint].exlen;
		sysexbuffer = evbuf_old[evbpoint].sysexbuffer;

		LeaveCriticalSection(&mim);
		switch (uMsg) {
		case MODM_DATA:
			dwParam2 = dwParam1 & 0xF0;
			exlen = (dwParam2 >= 0xF8 && dwParam2 <= 0xFF) ? 1 : ((dwParam2 == 0xC0 || dwParam2 == 0xD0) ? 2 : 3);
			BASS_MIDI_StreamEvents(stream, BASS_MIDI_EVENTS_RAW, &dwParam1, exlen);
			break;
		case MODM_LONGDATA:
			BASS_MIDI_StreamEvents(stream, BASS_MIDI_EVENTS_RAW, sysexbuffer, exlen);
			free(sysexbuffer);
			break;
		}
	} while (bmsyn_buf_check_old(mim));
	return played;
}

void oldmoddatafunction(UINT uMsg, CRITICAL_SECTION mim3, DWORD_PTR dwParam1, DWORD_PTR dwParam2) {
	UINT evbpoint;
	int exlen;
	char *sysexbuffer;
	EnterCriticalSection(&mim3);
	evbpoint = evbwpoint_old;
	if (++evbwpoint_old >= EVBUFF_SIZE_OLD)
		evbwpoint_old -= EVBUFF_SIZE_OLD;
	evbuf_old[evbpoint].uMsg = uMsg;
	evbuf_old[evbpoint].dwParam1 = dwParam1;
	evbuf_old[evbpoint].dwParam2 = dwParam2;
	evbuf_old[evbpoint].exlen = exlen;
	evbuf_old[evbpoint].sysexbuffer = sysexbuffer;
	LeaveCriticalSection(&mim3);
}

// New buffer system from BASSMIDI Driver 4.x

int bmsyn_buf_check(CRITICAL_SECTION mim2){
	int retval;
	EnterCriticalSection(&mim2);
	retval = evbcount;
	LeaveCriticalSection(&mim2);
	return retval;
}

int bmsyn_play_some_data(HSTREAM stream, CRITICAL_SECTION mim){
	UINT uDeviceID;
	UINT uMsg;
	DWORD_PTR	dwParam1;
	DWORD_PTR   dwParam2;

	UINT evbpoint;
	int exlen;
	unsigned char *sysexbuffer;
	int played;

	played = 0;
	if (!bmsyn_buf_check(mim)){
		played = ~0;
		return played;
	}
	do{
		EnterCriticalSection(&mim);
		evbpoint = evbrpoint;
		if (++evbrpoint >= EVBUFF_SIZE)
			evbrpoint -= EVBUFF_SIZE;

		uDeviceID = evbuf[evbpoint].uDeviceID;
		uMsg = evbuf[evbpoint].uMsg;
		dwParam1 = evbuf[evbpoint].dwParam1;
		dwParam2 = evbuf[evbpoint].dwParam2;
		exlen = evbuf[evbpoint].exlen;
		sysexbuffer = evbuf[evbpoint].sysexbuffer;

		LeaveCriticalSection(&mim);
		switch (uMsg) {
		case MODM_DATA:
			dwParam2 = dwParam1 & 0xF0;
			exlen = (dwParam2 >= 0xF8 && dwParam2 <= 0xFF) ? 1 : ((dwParam2 == 0xC0 || dwParam2 == 0xD0) ? 2 : 3);
			BASS_MIDI_StreamEvents(stream, BASS_MIDI_EVENTS_RAW, &dwParam1, exlen);
			break;
		case MODM_LONGDATA:
			BASS_MIDI_StreamEvents(stream, BASS_MIDI_EVENTS_RAW, sysexbuffer, exlen);
			free(sysexbuffer);
			break;
		}
	} while (InterlockedDecrement(&evbcount));
	return played;
}

void moddatafunction(UINT uDeviceID, UINT uMsg, CRITICAL_SECTION mim3, DWORD_PTR dwParam1, DWORD_PTR dwParam2) {
	UINT evbpoint;
	int exlen;
	unsigned char *sysexbuffer;
	EnterCriticalSection(&mim3);
	evbpoint = evbwpoint;
	if (++evbwpoint >= EVBUFF_SIZE)
		evbwpoint -= EVBUFF_SIZE;
	evbuf[evbpoint].uDeviceID = uDeviceID;
	evbuf[evbpoint].uMsg = uMsg;
	evbuf[evbpoint].dwParam1 = dwParam1;
	evbuf[evbpoint].dwParam2 = dwParam2;
	evbuf[evbpoint].exlen = exlen;
	evbuf[evbpoint].sysexbuffer = sysexbuffer;
	LeaveCriticalSection(&mim3);
	if (InterlockedIncrement(&evbcount) >= EVBUFF_SIZE) {
		do
		{

		} while (evbcount >= EVBUFF_SIZE);
	}
}
