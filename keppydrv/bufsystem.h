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

static const char sysex_gm_reset[] = { 0xF0, 0x7E, 0x7F, 0x09, 0x01, 0xF7 };
static const char sysex_gs_reset[] = { 0xF0, 0x41, 0x10, 0x42, 0x12, 0x40, 0x00, 0x7F, 0x00, 0x41, 0xF7 };
static const char sysex_xg_reset[] = { 0xF0, 0x43, 0x10, 0x4C, 0x00, 0x00, 0x7E, 0x00, 0xF7 };

static void ResetDrumChannels()
{
	static const BYTE part_to_ch[16] = { 9, 0, 1, 2, 3, 4, 5, 6, 7, 8, 10, 11, 12, 13, 14, 15 };

	unsigned i;

	memset(drum_channels, 0, sizeof(drum_channels));
	drum_channels[9] = 1;

	memcpy(gs_part_to_ch, part_to_ch, sizeof(gs_part_to_ch));

	if (hStream)
	{
		for (i = 0; i < 16; ++i)
		{
			BASS_MIDI_StreamEvent(hStream, i, MIDI_EVENT_DRUMS, drum_channels[i]);
		}
	}
}

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
			exlen = (dwParam2 == 0xC0 || dwParam2 == 0xD0) ? 2 : 3;
			BASS_MIDI_StreamEvents(hStream, BASS_MIDI_EVENTS_RAW, &dwParam1, exlen);
			if (dwParam2 == 0xB0 && (dwParam1 & 0xFF00) == 0)
			{
				if ((dwParam1 & 0xFF0000) == 0x7F0000) drum_channels[dwParam1 & 0x0F] = 1;
				else if ((dwParam1 & 0xFF0000) == 0x790000) drum_channels[dwParam1 & 0x0F] = 0;
			}
			else if (dwParam2 == 0xC0)
			{
				BASS_MIDI_StreamEvent(hStream, dwParam1 & 0x0F, MIDI_EVENT_DRUMS, drum_channels[dwParam1 & 0x0F]);
			}
			break;
		case MODM_LONGDATA:
			BASS_MIDI_StreamEvents(hStream, BASS_MIDI_EVENTS_RAW, sysexbuffer, exlen);
			if ((exlen == _countof(sysex_gm_reset) && !memcmp(sysexbuffer, sysex_gm_reset, _countof(sysex_gm_reset))) ||
				(exlen == _countof(sysex_gs_reset) && !memcmp(sysexbuffer, sysex_gs_reset, _countof(sysex_gs_reset))) ||
				(exlen == _countof(sysex_xg_reset) && !memcmp(sysexbuffer, sysex_xg_reset, _countof(sysex_xg_reset)))) {
				ResetDrumChannels();
			}
			else if (exlen == 11 &&
				sysexbuffer[0] == (char)0xF0 && sysexbuffer[1] == 0x41 && sysexbuffer[3] == 0x42 &&
				sysexbuffer[4] == 0x12 && sysexbuffer[5] == 0x40 && (sysexbuffer[6] & 0xF0) == 0x10 &&
				sysexbuffer[10] == (char)0xF7)
			{
				if (sysexbuffer[7] == 2)
				{
					// GS MIDI channel to part assign
					gs_part_to_ch[sysexbuffer[6] & 15] = sysexbuffer[8];
				}
				else if (sysexbuffer[7] == 0x15)
				{
					// GS part to rhythm allocation
					unsigned int drum_channel = gs_part_to_ch[sysexbuffer[6] & 15];
					if (drum_channel < 16)
					{
						drum_channels[drum_channel] = sysexbuffer[8];
					}
				}
			}
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
			BASS_MIDI_StreamEvents(hStream, BASS_MIDI_EVENTS_RAW, sysexbuffer, exlen);
			free(sysexbuffer);
			break;
		}
	} while (InterlockedDecrement(&evbcount));
	return played;
}

bool modmdata_old(UINT evbpoint, UINT uMsg, DWORD_PTR dwParam1, DWORD_PTR dwParam2, int exlen, char *sysexbufferold) {
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
	return MMSYSERR_NOERROR;
}

bool modmdata(UINT evbpoint, UINT uMsg, UINT uDeviceID, DWORD_PTR dwParam1, DWORD_PTR dwParam2, int exlen, unsigned char *sysexbuffer) {
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
	return MMSYSERR_NOERROR;
}

bool longmodmdata_old(MIDIHDR *IIMidiHdr, DWORD_PTR dwParam1, DWORD_PTR dwParam2, int exlen, char *sysexbuffer) {
	IIMidiHdr = (MIDIHDR *)dwParam1;
	if (!(IIMidiHdr->dwFlags & MHDR_PREPARED)) return MIDIERR_UNPREPARED;
	IIMidiHdr->dwFlags &= ~MHDR_DONE;
	IIMidiHdr->dwFlags |= MHDR_INQUEUE;
	IIMidiHdr = (MIDIHDR *)dwParam1;
	exlen = (int)IIMidiHdr->dwBufferLength;
	if (NULL == (sysexbuffer = (char *)malloc(exlen * sizeof(char)))){
		return MMSYSERR_NOMEM;
	}
	else{
		memcpy(sysexbuffer, IIMidiHdr->lpData, exlen);
	}
	IIMidiHdr->dwFlags &= ~MHDR_INQUEUE;
	IIMidiHdr->dwFlags |= MHDR_DONE;
}

bool longmodmdata(MIDIHDR *IIMidiHdr, UINT uDeviceID, DWORD_PTR dwParam1, DWORD_PTR dwParam2, int exlen, unsigned char *sysexbuffer) {
	IIMidiHdr = (MIDIHDR *)dwParam1;
	if (!(IIMidiHdr->dwFlags & MHDR_PREPARED)) return MIDIERR_UNPREPARED;
	IIMidiHdr->dwFlags &= ~MHDR_DONE;
	IIMidiHdr->dwFlags |= MHDR_INQUEUE;
	exlen = (int)IIMidiHdr->dwBufferLength;
	if (NULL == (sysexbuffer = (unsigned char *)malloc(exlen * sizeof(unsigned char)))){
		return MMSYSERR_NOMEM;
	}
	else{
		memcpy(sysexbuffer, IIMidiHdr->lpData, exlen);
	}
	IIMidiHdr->dwFlags &= ~MHDR_INQUEUE;
	IIMidiHdr->dwFlags |= MHDR_DONE;
}