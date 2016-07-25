/*
Keppy's Driver buffer system
*/

int bmsyn_buf_check(void){
	int retval;
	EnterCriticalSection(&mim_section);
	retval = (evbrpoint != evbwpoint) ? ~0 : 0;
	LeaveCriticalSection(&mim_section);
	return retval;
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
	int buffullno;
	int buffullye;
	played = 0;
	buffull = 0;

	if (!bmsyn_buf_check()){
		played = ~0;
		return played;
	}
	do{
		EnterCriticalSection(&mim_section);
		evbpoint = evbrpoint;

		if (++evbrpoint >= newevbuffvalue) {
			evbrpoint -= newevbuffvalue;
			buffull = 1;
			LeaveCriticalSection(&mim_section);
			return played;
		}

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
	} while (bmsyn_buf_check());
	return played;
}

bool modmdata(UINT evbpoint, UINT uMsg, UINT uDeviceID, DWORD_PTR dwParam1, DWORD_PTR dwParam2, int exlen, unsigned char *sysexbuffer) {
	EnterCriticalSection(&mim_section);
	evbpoint = evbwpoint;
	if (++evbwpoint >= newevbuffvalue) {
		evbwpoint -= newevbuffvalue;
		LeaveCriticalSection(&mim_section);
		return MMSYSERR_NOERROR;
	}
	evbuf[evbpoint].uDeviceID = uDeviceID;
	evbuf[evbpoint].uMsg = uMsg;
	evbuf[evbpoint].dwParam1 = dwParam1;
	evbuf[evbpoint].dwParam2 = dwParam2;
	evbuf[evbpoint].exlen = exlen;
	evbuf[evbpoint].sysexbuffer = sysexbuffer;
	LeaveCriticalSection(&mim_section);
	return MMSYSERR_NOERROR;
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

void AudioRender(int bassoutput) {
	try {
		if (bassoutput == -1) {
			BASS_ChannelUpdate(hStream, 0);
		}
		else {
			decoded = BASS_ChannelGetData(hStream, sndbf, BASS_DATA_FLOAT + newsndbfvalue * sizeof(float));
			if (encmode == 1) {

			}
			else if (encmode == 0) {
				if (decoded < 0) {

				}
				if (evbrpoint >= newevbuffvalue | evbwpoint >= newevbuffvalue) {
					for (unsigned i = 0, j = 0.0f / sizeof(float); i < j; i++) {
						sndbf[i] *= sound_out_volume_float;
					}
					sound_driver->write_frame(sndbf, 0.0f / sizeof(float), true);
				}
				else {
					for (unsigned i = 0, j = decoded / sizeof(float); i < j; i++) {
						sndbf[i] *= sound_out_volume_float;
					}
					sound_driver->write_frame(sndbf, decoded / sizeof(float), true);
				}
			}
		}
	}
	catch (int e) {
		crashhandler(e);
	}
}