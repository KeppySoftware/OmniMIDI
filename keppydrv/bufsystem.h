/*
Keppy's Driver buffer system
*/

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

#define EVBUFF_SIZE_OLD 32768
static struct evbuf_t_old evbuf_old[EVBUFF_SIZE_OLD];
static UINT evbwpoint_old = 0;
static UINT evbrpoint_old = 0;
static UINT evbsysexpoint_old;

#define EVBUFF_SIZE 32768
static struct evbuf_t evbuf[EVBUFF_SIZE];
static UINT  evbwpoint = 0;
static UINT  evbrpoint = 0;
static volatile LONG evbcount = 0;
static UINT evbsysexpoint;

int bmsyn_buf_check_old(void){
	int retval;
	EnterCriticalSection(&mim_section);
	retval = (evbrpoint_old != evbwpoint_old) ? ~0 : 0;
	LeaveCriticalSection(&mim_section);
	return retval;
}

int bmsyn_buf_check(void){
	int retval;
	EnterCriticalSection(&mim_section);
	retval = evbcount;
	LeaveCriticalSection(&mim_section);
	return retval;
}

int bmsyn_play_some_data_old(void){
	UINT uMsg;
	DWORD_PTR	dwParam1;
	DWORD_PTR   dwParam2;

	UINT evbpoint_old;
	int exlen;
	char *sysexbuffer;
	int played;

	played = 0;
	if (!bmsyn_buf_check_old()){
		played = ~0;
		return played;
	}
	do{
		EnterCriticalSection(&mim_section);

		evbpoint_old = evbrpoint_old;
		if (++evbrpoint_old >= EVBUFF_SIZE_OLD)
			evbrpoint_old -= EVBUFF_SIZE_OLD;

		uMsg = evbuf_old[evbpoint_old].uMsg;
		dwParam1 = evbuf_old[evbpoint_old].dwParam1;
		dwParam2 = evbuf_old[evbpoint_old].dwParam2;
		exlen = evbuf_old[evbpoint_old].exlen;
		sysexbuffer = evbuf_old[evbpoint_old].sysexbuffer;

		LeaveCriticalSection(&mim_section);
		switch (uMsg) {
		case MODM_DATA:
			dwParam2 = dwParam1 & 0xF0;
			exlen = (dwParam2 >= 0xF8 && dwParam2 <= 0xFF) ? 1 : ((dwParam2 == 0xC0 || dwParam2 == 0xD0) ? 2 : 3);
			BASS_MIDI_StreamEvents(hStream, BASS_MIDI_EVENTS_RAW, &dwParam1, exlen);
			break;
		case MODM_LONGDATA:
			BASS_MIDI_StreamEvents(hStream, BASS_MIDI_EVENTS_RAW, sysexbuffer, exlen);
			free(sysexbuffer);
			break;
		}
	} while (bmsyn_buf_check_old());
	return played;
}

int bmsyn_play_some_data(void){
	UINT uDeviceID;
	UINT uMsg;
	DWORD_PTR	dwParam1;
	DWORD_PTR   dwParam2;

	UINT evbpoint;
	int exlen;
	unsigned char *sysexbuffer;
	int played;

	played = 0;
	if (!bmsyn_buf_check()){
		played = ~0;
		return played;
	}
	do{
		EnterCriticalSection(&mim_section);
		evbpoint = evbrpoint;
		if (++evbrpoint >= EVBUFF_SIZE)
			evbrpoint -= EVBUFF_SIZE;

		uDeviceID = evbuf[evbpoint].uDeviceID;
		uMsg = evbuf[evbpoint].uMsg;
		dwParam1 = evbuf[evbpoint].dwParam1;
		dwParam2 = evbuf[evbpoint].dwParam2;
		exlen = evbuf[evbpoint].exlen;
		sysexbuffer = evbuf[evbpoint].sysexbuffer;

		LeaveCriticalSection(&mim_section);
		switch (uMsg) {
		case MODM_DATA:
			dwParam2 = dwParam1 & 0xF0;
			exlen = (dwParam2 >= 0xF8 && dwParam2 <= 0xFF) ? 1 : ((dwParam2 == 0xC0 || dwParam2 == 0xD0) ? 2 : 3);
			BASS_MIDI_StreamEvents(hStream, BASS_MIDI_EVENTS_RAW, &dwParam1, exlen);
			break;
		case MODM_LONGDATA:
#ifdef DEBUG
			FILE * logfile;
			logfile = fopen("c:\\dbglog2.log", "at");
			if (logfile != NULL) {
				for (int i = 0; i < exlen; i++)
					fprintf(logfile, "%x ", sysexbuffer[i]);
				fprintf(logfile, "\n");
			}
			fclose(logfile);
#endif
			BASS_MIDI_StreamEvents(hStream, BASS_MIDI_EVENTS_RAW, sysexbuffer, exlen);
			free(sysexbuffer);
			break;
		}
	} while (InterlockedDecrement(&evbcount));
	return played;
}

void modmdata_old(UINT evbpoint, UINT uMsg, DWORD_PTR dwParam1, DWORD_PTR dwParam2, int exlen, char *sysexbufferold) {
	EnterCriticalSection(&mim_section);
	evbpoint = evbwpoint_old;
	if (++evbwpoint_old >= EVBUFF_SIZE_OLD)
		evbwpoint_old -= EVBUFF_SIZE_OLD;
	evbuf_old[evbpoint].uMsg = uMsg;
	evbuf_old[evbpoint].dwParam1 = dwParam1;
	evbuf_old[evbpoint].dwParam2 = dwParam2;
	evbuf_old[evbpoint].exlen = exlen;
	evbuf_old[evbpoint].sysexbuffer = sysexbufferold;
	LeaveCriticalSection(&mim_section);
}

void modmdata(UINT evbpoint, UINT uMsg, UINT uDeviceID, DWORD_PTR dwParam1, DWORD_PTR dwParam2, int exlen, unsigned char *sysexbuffer) {
	EnterCriticalSection(&mim_section);
	evbpoint = evbwpoint;
	if (++evbwpoint >= EVBUFF_SIZE)
		evbwpoint -= EVBUFF_SIZE;
	evbuf[evbpoint].uDeviceID = uDeviceID;
	evbuf[evbpoint].uMsg = uMsg;
	evbuf[evbpoint].dwParam1 = dwParam1;
	evbuf[evbpoint].dwParam2 = dwParam2;
	evbuf[evbpoint].exlen = exlen;
	evbuf[evbpoint].sysexbuffer = sysexbuffer;
	LeaveCriticalSection(&mim_section);
	if (InterlockedIncrement(&evbcount) >= EVBUFF_SIZE) {
		do
		{

		} while (evbcount >= EVBUFF_SIZE);
	}
}