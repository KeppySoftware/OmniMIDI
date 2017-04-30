/*
Keppy's Synthesizer WinMM replacement
*/

BOOL WINAPI DriverCallback(DWORD_PTR dwCallBack, DWORD uFlags, HDRVR hDev,
	DWORD wMsg, DWORD_PTR dwUser, DWORD_PTR dwParam1,
	DWORD_PTR dwParam2)
{
	BOOL ret = FALSE;

	if (!dwCallBack)
		return ret;

	switch (uFlags & DCB_TYPEMASK) {
	case DCB_NULL:
		/* Native returns FALSE = no notification, not TRUE */
		return ret;
	case DCB_WINDOW:
		ret = PostMessageA((HWND)dwCallBack, wMsg, (WPARAM)hDev, dwParam1);
		break;
	case DCB_TASK: /* aka DCB_THREAD */
		ret = PostThreadMessageA(dwCallBack, wMsg, (WPARAM)hDev, dwParam1);
		break;
	case DCB_FUNCTION:
		((LPDRVCALLBACK)dwCallBack)(hDev, wMsg, dwUser, dwParam1, dwParam2);
		ret = TRUE;
		break;
	case DCB_EVENT:
		ret = SetEvent((HANDLE)dwCallBack);
		break;
	}

	return ret;
}

UINT WINAPI midiInClose(HMIDIIN hMidiIn)
{
	return MMSYSERR_NOERROR;
}

UINT WINAPI midiInReset(HMIDIIN hMidiIn)
{
	return MMSYSERR_NOERROR;
}

UINT WINAPI midiInGetDevCapsA(UINT_PTR uDeviceID, LPMIDIINCAPSA lpCaps, UINT uSize)
{
	return MMSYSERR_NOERROR;
}

UINT WINAPI midiInGetDevCapsW(UINT_PTR uDeviceID, LPMIDIINCAPSW lpCaps, UINT uSize)
{
	return MMSYSERR_NOERROR;
}

UINT WINAPI midiInPrepareHeader(HMIDIIN hMidiIn,
	MIDIHDR* lpMidiInHdr, UINT uSize)
{
	return MMSYSERR_NOERROR;
}

UINT WINAPI midiInUnprepareHeader(HMIDIIN hMidiIn,
	MIDIHDR* lpMidiInHdr, UINT uSize)
{
	return MMSYSERR_NOERROR;
}

UINT WINAPI midiInAddBuffer(HMIDIIN hMidiIn,
	MIDIHDR* lpMidiInHdr, UINT uSize)
{
	return MMSYSERR_NOERROR;
}

UINT WINAPI midiInGetNumDevs(void)
{
	return MMSYSERR_NOERROR;
}

UINT WINAPI midiInOpen(HMIDIIN* lphMidiIn, UINT uDeviceID,
	DWORD_PTR dwCallback, DWORD_PTR dwInstance, DWORD dwFlags)
{
	return MMSYSERR_NOERROR;
}

UINT WINAPI midiInStart(HMIDIIN hMidiIn)
{
	return MMSYSERR_NOERROR;
}

UINT WINAPI midiInStop(HMIDIIN hMidiIn)
{
	return MMSYSERR_NOERROR;
}

UINT WINAPI midiOutClose(HMIDIOUT hMidiOut)
{
	return MMSYSERR_NOERROR;
}

UINT WINAPI midiOutGetDevCapsW(UINT_PTR uDeviceID, LPMIDIOUTCAPSW lpCaps,
	UINT uSize)
{
	return MMSYSERR_NOERROR;
}

UINT WINAPI midiOutGetNumDevs(VOID)
{
	return MMSYSERR_NOERROR;
}

UINT WINAPI midiOutOpen(LPHMIDIOUT lphMidiOut, UINT uDeviceID,
	DWORD_PTR dwCallback, DWORD_PTR dwInstance, DWORD dwFlags)
{
	struct Driver *driver = &drivers[uDeviceID];
	int clientNum;
	DoCallback(uDeviceID, clientNum, MOM_OPEN, 0, 0);
	return MMSYSERR_NOERROR;
}

UINT WINAPI midiOutReset(HMIDIOUT hMidiOut)
{
	reset_synth = 1;
	ResetSynth(0);
	return MMSYSERR_NOERROR;
}

UINT WINAPI midiOutSetVolume(HMIDIOUT hMidiOut, DWORD dwVolume)
{
	return MMSYSERR_NOERROR;
}

UINT WINAPI midiOutShortMsg(HMIDIOUT hMidiOut, DWORD dwMsg)
{
	try {
		UINT evbpoint;
		EnterCriticalSection(&mim_section);
		evbpoint = evbwpoint;
		if (++evbwpoint >= newevbuffvalue)
			evbwpoint -= newevbuffvalue;
		evbuf[evbpoint].uMsg = MODM_DATA;
		evbuf[evbpoint].dwParam1 = dwMsg;
		LeaveCriticalSection(&mim_section);
		if (vms2emu == 1) {
			if (InterlockedIncrement(&evbcount) >= newevbuffvalue) {
				do
				{
					if (debugmode) {
						std::cout << "Buffer is full, slowing down..." << std::endl << std::flush;;
					}
				} while (evbcount >= newevbuffvalue);
			}
		}
		return MMSYSERR_NOERROR;
	}
	catch (...) {
		return MMSYSERR_NOERROR;
	}
}

UINT WINAPI midiOutLongMsg(HMIDIOUT hMidiOut,
	MIDIHDR* lpMidiOutHdr, UINT uSize)
{
	try {
		UINT evbpoint;
		EnterCriticalSection(&mim_section);
		evbpoint = evbwpoint;
		if (++evbwpoint >= newevbuffvalue)
			evbwpoint -= newevbuffvalue;
		evbuf[evbpoint].uMsg = MODM_LONGDATA;
		evbuf[evbpoint].dwParam1 = (DWORD_PTR)lpMidiOutHdr;
		LeaveCriticalSection(&mim_section);
		if (vms2emu == 1) {
			if (InterlockedIncrement(&evbcount) >= newevbuffvalue) {
				do
				{
					if (debugmode) {
						std::cout << "Buffer is full, slowing down..." << std::endl << std::flush;;
					}
				} while (evbcount >= newevbuffvalue);
			}
		}
		return MMSYSERR_NOERROR;
	}
	catch (...) {
		return MMSYSERR_NOERROR;
	}
}

DWORD WINAPI timeGetTime(void)
{
	return GetTickCount();
}

UINT WINAPI timeBeginPeriod(UINT wPeriod)
{
	return MMSYSERR_NOERROR;
}

UINT WINAPI timeEndPeriod(UINT wPeriod)
{
	if (wPeriod < 1 || wPeriod > 65535)
		return TIMERR_NOCANDO;

	if (wPeriod > 1)
	{

	}

	return 0;
}

UINT WINAPI mixerOpen(LPHMIXER lphMix, UINT uDeviceID, DWORD_PTR dwCallback,
	DWORD_PTR dwInstance, DWORD fdwOpen)
{
	return MMSYSERR_NOERROR;
}

UINT WINAPI mixerClose(HMIXER hMix)
{
	return MMSYSERR_NOERROR;
}

UINT WINAPI mixerGetControlDetailsA(HMIXEROBJ hmix, LPMIXERCONTROLDETAILS lpmcdA,
	DWORD fdwDetails)
{
	return MMSYSERR_NOERROR;
}

UINT WINAPI mixerGetDevCapsA(UINT_PTR uDeviceID, LPMIXERCAPSA lpCaps, UINT uSize)
{
	return MMSYSERR_NOERROR;
}

UINT WINAPI mixerGetLineControlsA(HMIXEROBJ hmix, LPMIXERLINECONTROLSA lpmlcA,
	DWORD fdwControls)
{
	return MMSYSERR_NOERROR;
}

UINT WINAPI mixerGetLineInfoW(HMIXEROBJ hmix, LPMIXERLINEW lpmliW, DWORD fdwInfo)
{
	return MMSYSERR_NOERROR;
}

UINT WINAPI mixerGetLineInfoA(HMIXEROBJ hmix, LPMIXERLINEA lpmliA,
	DWORD fdwInfo)
{
	return MMSYSERR_NOERROR;
}

UINT WINAPI mixerSetControlDetails(HMIXEROBJ hmix, LPMIXERCONTROLDETAILS lpmcd,
	DWORD fdwDetails)
{
	return MMSYSERR_NOERROR;
}

UINT WINAPI mixerGetNumDevs(void)
{
	return MMSYSERR_NOERROR;
}