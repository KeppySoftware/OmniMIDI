#define STRICT

#include "sound_out.h"

//#define HAVE_KS_HEADERS

#include <stdint.h>
#include <windows.h>
#include <shlobj.h>
#ifdef _M_ARM64
// XAudio2 ARM64 is only available on Windows 10+
// So let's compile straight for the Windows 10 SDK
#include <XAudio2.h>
#else
// Use redist version for x86/x64
#include "inc\XAudio2.h"
#endif
#include <assert.h>
#include <mmdeviceapi.h>
#include <vector>
#ifdef HAVE_KS_HEADERS
#include <ks.h>
#include <ksmedia.h>
#endif

#pragma comment ( lib, "winmm.lib" )

class XAudio2Output;

void XAudio2DeviceChanged(XAudio2Output*);

class XAudio2Output : public sound_out
{
	class XAudio2_BufferNotify : public IXAudio2VoiceCallback
	{
	public:
		HANDLE hBufferEndEvent;

		XAudio2_BufferNotify() {
			hBufferEndEvent = NULL;
			hBufferEndEvent = CreateEvent(NULL, FALSE, FALSE, NULL);
			assert(hBufferEndEvent != NULL);
		}

		~XAudio2_BufferNotify() {
			CloseHandle(hBufferEndEvent);
			hBufferEndEvent = NULL;
		}

		STDMETHOD_(void, OnBufferEnd) (void* pBufferContext) {
			assert(hBufferEndEvent != NULL);
			SetEvent(hBufferEndEvent);
			XAudio2Output* psnd = (XAudio2Output*)pBufferContext;
			if (psnd) psnd->OnBufferEnd();
		}


		// dummies:
		STDMETHOD_(void, OnVoiceProcessingPassStart) (UINT32 BytesRequired) {}
		STDMETHOD_(void, OnVoiceProcessingPassEnd) () {}
		STDMETHOD_(void, OnStreamEnd) () {}
		STDMETHOD_(void, OnBufferStart) (void* pBufferContext) {}
		STDMETHOD_(void, OnLoopEnd) (void* pBufferContext) {}
		STDMETHOD_(void, OnVoiceError) (void* pBufferContext, HRESULT Error) {};
	};

	void OnBufferEnd()
	{
		LONG buffer_read_cursor = this->buffer_read_cursor;
		samples_played += samples_in_buffer[buffer_read_cursor];
		this->buffer_read_cursor = (buffer_read_cursor + 1) % num_frames;
	}

	BOOL GetFolderPath(const GUID FolderID, const int CSIDL, wchar_t* P, size_t PS) {
#ifdef XP
		if (typedef HRESULT(WINAPI* SHGKP)(REFKNOWNFOLDERID, DWORD, HANDLE, PWSTR*); true) {
			SHGKP SHGetKnownFolderPath = (SHGKP)GetProcAddress(GetModuleHandle(L"shell32"), "SHGetKnownFolderPath");

			if (SHGetKnownFolderPath) {
#endif
				PWSTR Dir;

				if (SUCCEEDED(SHGetKnownFolderPath(FolderID, 0, NULL, &Dir))) {
					swprintf_s(P, PS, L"%s", Dir);
					CoTaskMemFree(Dir);
					return TRUE;
				}

				CoTaskMemFree(Dir);
#ifdef XP
			}
			else {
				LPWSTR Dir;

				if (SUCCEEDED(SHGetFolderPath(NULL, CSIDL, NULL, SHGFP_TYPE_CURRENT, Dir))) {
					swprintf_s(P, PS, L"%s", Dir);
					return TRUE;
				}
			}
		}
#endif

		return FALSE;
	}

	void* hwnd;
	bool			loaded;
	volatile bool   device_changed;
	unsigned        reopen_count;
	unsigned        sample_rate, bytes_per_sample, max_samples_per_frame, num_frames;
	unsigned short  nch;
	volatile LONG   buffer_read_cursor;
	LONG            buffer_write_cursor;

	volatile UINT64 samples_played;

	uint8_t* sample_buffer;
	UINT64* samples_in_buffer;

	IXAudio2* xaud;
	IXAudio2MasteringVoice* mVoice; // listener
	IXAudio2SourceVoice* sVoice; // sound source
	XAUDIO2_VOICE_STATE     vState;

	HMODULE XALib = nullptr;
	typedef NTSTATUS(NTAPI* XA2C)(_Outptr_ IXAudio2** ppXAudio2, UINT32 Flags, XAUDIO2_PROCESSOR XAudio2Processor);
	XA2C XA2Create = 0;

public:
	XAudio2Output()
	{
#ifndef _M_ARM64
		// If on x86/x64, load the library and its function using LoadLibrary/GetProcAddress
		
		wchar_t SysDir[MAX_PATH] = { 0 };
		wchar_t DLLPath[MAX_PATH] = { 0 };
		WIN32_FIND_DATA FD = { 0 };

		if (XALib == nullptr) {
			// XAudio2_9 can not be found, this is Windows 8.1 or older
			if (!(XALib = LoadLibrary(L"XAudio2_9")))
			{
				// Try to get XAudio2_9_win7 from OmniMIDI's folder
				if (GetFolderPath(FOLDERID_System, CSIDL_SYSTEM, SysDir, sizeof(SysDir))) {
					swprintf_s(DLLPath, MAX_PATH, L"%s\\OmniMIDI\\%s\0", SysDir, L"XAudio2_9_win7.dll");

					// Found it?
					if (FindFirstFile(DLLPath, &FD) != INVALID_HANDLE_VALUE)
					{
						// Yes, load it
						XALib = LoadLibrary(DLLPath);

						// If it failed, return.
						if (!XALib) return;
					}
				}
				// Something went wrong, GetFolderPath failed
				else return;
			}

			XA2Create = (XA2C)GetProcAddress(XALib, "XAudio2Create");

			if (!XA2Create)
				return;
		}
#endif

		this->loaded = true;

		reopen_count = 0;
		device_changed = false;

		xaud = NULL;
		mVoice = NULL;
		sVoice = NULL;
		sample_buffer = NULL;
		ZeroMemory(&vState, sizeof(vState));
	}

	virtual ~XAudio2Output()
	{
		close();

#ifndef _M_ARM64
		if (XALib)
		{
			FreeLibrary(XALib);
			XALib = nullptr;
			XA2Create = NULL;
		}
#endif
	}

	void OnDeviceChanged()
	{
		device_changed = true;
	}

	virtual bool IsLoaded() { return this->loaded; }

	virtual const char* OpenStream(void* hwnd, unsigned sample_rate, unsigned short nch, unsigned short bps, unsigned max_samples_per_frame, unsigned num_frames)
	{
		if (!this->loaded)
			return "XAudio2 failed to load";

		this->hwnd = hwnd;
		this->sample_rate = sample_rate;
		this->nch = nch;
		this->max_samples_per_frame = max_samples_per_frame;
		this->num_frames = num_frames;
		this->bytes_per_sample = bps;

		WAVEFORMATEX wfx;

		switch (bps)
		{
		case 4:
			wfx.wFormatTag = WAVE_FORMAT_IEEE_FLOAT;
			break;
		default:
		case 1:
		case 2:
		case 3:
			wfx.wFormatTag = WAVE_FORMAT_ADPCM;
			break;
		}

		wfx.nChannels = nch; //1;
		wfx.nSamplesPerSec = sample_rate;
		wfx.nBlockAlign = bytes_per_sample * nch; //2;
		wfx.nAvgBytesPerSec = wfx.nSamplesPerSec * wfx.nBlockAlign;
		wfx.wBitsPerSample = bps * 8;
		wfx.cbSize = 0;

#ifndef _M_ARM64
		HRESULT hr = XA2Create(&xaud, 0, 0);
#else
		HRESULT hr = XAudio2Create(&xaud, 0);
#endif
		if (FAILED(hr)) return "XAudio2Create failed";

		hr = xaud->CreateMasteringVoice(
			&mVoice,
			nch,
			sample_rate,
			0,
			NULL,
			NULL);
		if (FAILED(hr)) return "xaud::CreateMasteringVoice failed";

		hr = xaud->CreateSourceVoice(&sVoice, &wfx, 0, 1.0f);
		if (FAILED(hr)) return "xaud::CreateSourceVoice failed";

		hr = sVoice->Start(0);
		if (FAILED(hr)) return "xaud::Start failed";

		device_changed = false;
		buffer_read_cursor = 0;
		buffer_write_cursor = 0;
		samples_played = 0;
		sample_buffer = new uint8_t[max_samples_per_frame * num_frames * bytes_per_sample];
		samples_in_buffer = new UINT64[num_frames];
		memset(samples_in_buffer, 0, sizeof(UINT64) * num_frames);

		return NULL;
	}

	void close()
	{
		if (sVoice) {
			sVoice->Stop(0);
			sVoice->DestroyVoice();
			sVoice = NULL;
		}

		if (mVoice) {
			mVoice->DestroyVoice();
			mVoice = NULL;
		}

		if (xaud) {
			xaud->Release();
			xaud = NULL;
		}

		delete[] sample_buffer;
		sample_buffer = NULL;
		delete[] samples_in_buffer;
		samples_in_buffer = NULL;
	}

	virtual const char* WriteFrame(void* buffer, unsigned num_samples)
	{
		if (!this->loaded)
			return "XAudio2 not loaded";

		if (device_changed)
		{
			close();
			reopen_count = 5;
			device_changed = false;
			return 0;
		}

		if (reopen_count)
		{
			if (!--reopen_count)
			{
				const char* err = OpenStream(hwnd, sample_rate, nch, bytes_per_sample, max_samples_per_frame, num_frames);
				if (err)
				{
					reopen_count = 60 * 5;
					return err;
				}
			}
			else return 0;
		}

		for (;;) {
			sVoice->GetState(&vState, XAUDIO2_VOICE_NOSAMPLESPLAYED);
			if (vState.BuffersQueued < num_frames)
				break;
		}

		samples_in_buffer[buffer_write_cursor] = num_samples / nch;
		XAUDIO2_BUFFER buf = { 0 };
		unsigned num_bytes = num_samples * bytes_per_sample;
		buf.AudioBytes = num_bytes;
		buf.pAudioData = sample_buffer + max_samples_per_frame * buffer_write_cursor * bytes_per_sample;
		buf.pContext = this;
		buffer_write_cursor = (buffer_write_cursor + 1) % num_frames;
		memcpy((void*)buf.pAudioData, buffer, num_bytes);

		if (sVoice->SubmitSourceBuffer(&buf) == S_OK)
			return 0;

		close();
		reopen_count = 60 * 5;

		return 0;
	}
};

void XAudio2DeviceChanged(XAudio2Output* p_instance)
{
	p_instance->OnDeviceChanged();
}

sound_out* CreateXAudio2Stream()
{
	return new XAudio2Output;
}
