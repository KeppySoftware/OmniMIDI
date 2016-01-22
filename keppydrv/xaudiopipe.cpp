#define STRICT
#ifndef _WIN32_WINNT
#define _WIN32_WINNT _WIN32_WINNT_WINXP
#endif

#include "sound_out.h"

//#define HAVE_KS_HEADERS

#include <stdint.h>
#include <windows.h>
#include <XAudio2.h>
#include <assert.h>
#include <mmdeviceapi.h>
#include <vector>
#ifdef HAVE_KS_HEADERS
#include <ks.h>
#include <ksmedia.h>
#endif


#pragma comment ( lib, "winmm.lib" )

class sound_out_i_xaudio2;

void xaudio2_device_changed( sound_out_i_xaudio2 * );

class XAudio2_Device_Notifier : public IMMNotificationClient
{
	volatile LONG registered;
	IMMDeviceEnumerator *pEnumerator;

	CRITICAL_SECTION lock;
	std::vector<sound_out_i_xaudio2*> instances;

public:
	XAudio2_Device_Notifier() : registered( 0 )
	{
		InitializeCriticalSection( &lock );
	}
	~XAudio2_Device_Notifier()
	{
		DeleteCriticalSection( &lock );
	}

	ULONG STDMETHODCALLTYPE AddRef()
	{
		return 1;
	}

	ULONG STDMETHODCALLTYPE Release()
	{
		return 1;
	}

	HRESULT STDMETHODCALLTYPE QueryInterface( REFIID riid, VOID **ppvInterface )
	{
		if (IID_IUnknown == riid)
		{
			*ppvInterface = (IUnknown*)this;
		}
		else if (__uuidof(IMMNotificationClient) == riid)
		{
			*ppvInterface = (IMMNotificationClient*)this;
		}
		else
		{
			*ppvInterface = NULL;
			return E_NOINTERFACE;
		}
		return S_OK;
	}

	HRESULT STDMETHODCALLTYPE OnDefaultDeviceChanged( EDataFlow flow, ERole role, LPCWSTR pwstrDeviceId )
	{
		if ( flow == eRender )
		{
			EnterCriticalSection( &lock );
			for ( std::vector<sound_out_i_xaudio2*>::iterator it = instances.begin(); it < instances.end(); ++it )
			{
				xaudio2_device_changed( *it );
			}
			LeaveCriticalSection( &lock );
		}

		return S_OK;
	}

	HRESULT STDMETHODCALLTYPE OnDeviceAdded( LPCWSTR pwstrDeviceId ) { return S_OK; }
	HRESULT STDMETHODCALLTYPE OnDeviceRemoved( LPCWSTR pwstrDeviceId ) { return S_OK; }
	HRESULT STDMETHODCALLTYPE OnDeviceStateChanged( LPCWSTR pwstrDeviceId, DWORD dwNewState ) { return S_OK; }
	HRESULT STDMETHODCALLTYPE OnPropertyValueChanged( LPCWSTR pwstrDeviceId, const PROPERTYKEY key ) { return S_OK; }

	void do_register(sound_out_i_xaudio2 * p_instance)
	{
		if ( InterlockedIncrement( &registered ) == 1 )
		{
			pEnumerator = NULL;
			HRESULT hr = CoCreateInstance( __uuidof( MMDeviceEnumerator ), NULL, CLSCTX_INPROC_SERVER, __uuidof( IMMDeviceEnumerator ), ( void** ) &pEnumerator );
			if ( SUCCEEDED( hr ) )
			{
				pEnumerator->RegisterEndpointNotificationCallback( this );
				registered = true;
			}
		}

		EnterCriticalSection( &lock );
		instances.push_back( p_instance );
		LeaveCriticalSection( &lock );
	}

	void do_unregister( sound_out_i_xaudio2 * p_instance )
	{
		if ( InterlockedDecrement( &registered ) == 0 )
		{
			if (pEnumerator)
			{
				pEnumerator->UnregisterEndpointNotificationCallback( this );
				pEnumerator->Release();
				pEnumerator = NULL;
			}
			registered = false;
		}

		EnterCriticalSection( &lock );
		for ( std::vector<sound_out_i_xaudio2*>::iterator it = instances.begin(); it < instances.end(); ++it )
		{
			if ( *it == p_instance )
			{
				instances.erase( it );
				break;
			}
		}
		LeaveCriticalSection( &lock );
	}
} g_notifier;

class sound_out_i_xaudio2 : public sound_out
{
	class XAudio2_BufferNotify : public IXAudio2VoiceCallback
	{
	public:
		HANDLE hBufferEndEvent;

		XAudio2_BufferNotify() {
			hBufferEndEvent = NULL;
			hBufferEndEvent = CreateEvent( NULL, FALSE, FALSE, NULL );
			assert( hBufferEndEvent != NULL );
		}

		~XAudio2_BufferNotify() {
			CloseHandle( hBufferEndEvent );
			hBufferEndEvent = NULL;
		}

		STDMETHOD_( void, OnBufferEnd ) ( void *pBufferContext ) {
			assert( hBufferEndEvent != NULL );
			SetEvent( hBufferEndEvent );
			sound_out_i_xaudio2 * psnd = ( sound_out_i_xaudio2 * ) pBufferContext;
			if ( psnd ) psnd->OnBufferEnd();
		}


		// dummies:
		STDMETHOD_( void, OnVoiceProcessingPassStart ) ( UINT32 BytesRequired ) {}
		STDMETHOD_( void, OnVoiceProcessingPassEnd ) () {}
		STDMETHOD_( void, OnStreamEnd ) () {}
		STDMETHOD_( void, OnBufferStart ) ( void *pBufferContext ) {}
		STDMETHOD_( void, OnLoopEnd ) ( void *pBufferContext ) {}
		STDMETHOD_( void, OnVoiceError ) ( void *pBufferContext, HRESULT Error ) {};
	};

	void OnBufferEnd()
	{
		InterlockedDecrement( &buffered_count );
		LONG buffer_read_cursor = this->buffer_read_cursor;
		samples_played += samples_in_buffer[ buffer_read_cursor ];
		this->buffer_read_cursor = ( buffer_read_cursor + 1 ) % num_frames;
	}

	void          * hwnd;
	bool            paused;
	volatile bool   device_changed;
	unsigned        reopen_count;
	unsigned        sample_rate, bytes_per_sample, max_samples_per_frame, num_frames;
	unsigned short  nch;
	volatile LONG   buffered_count;
	volatile LONG   buffer_read_cursor;
	LONG            buffer_write_cursor;

	volatile UINT64 samples_played;

	uint8_t       * sample_buffer;
	UINT64        * samples_in_buffer;

	IXAudio2               *xaud;
	IXAudio2MasteringVoice *mVoice; // listener
	IXAudio2SourceVoice    *sVoice; // sound source
	XAUDIO2_VOICE_STATE     vState;
	XAudio2_BufferNotify    notify; // buffer end notification
public:
	sound_out_i_xaudio2()
	{
		paused = false;
		reopen_count = 0;
		buffered_count = 0;
		device_changed = false;

		xaud = NULL;
		mVoice = NULL;
		sVoice = NULL;
		sample_buffer = NULL;
		ZeroMemory( &vState, sizeof( vState ) );

		g_notifier.do_register( this );
	}

	virtual ~sound_out_i_xaudio2()
	{
		g_notifier.do_unregister( this );

		close();
	}

	void OnDeviceChanged()
	{
		device_changed = true;
	}

	virtual const char* open( void * hwnd, unsigned sample_rate, unsigned short nch, bool floating_point, unsigned max_samples_per_frame, unsigned num_frames )
	{
		this->hwnd = hwnd;
		this->sample_rate = sample_rate;
		this->nch = nch;
		this->max_samples_per_frame = max_samples_per_frame;
		this->num_frames = num_frames;
		bytes_per_sample = floating_point ? 4 : 2;

#ifdef HAVE_KS_HEADERS
		WAVEFORMATEXTENSIBLE wfx;
		wfx.Format.wFormatTag = WAVE_FORMAT_EXTENSIBLE;
		wfx.Format.nChannels = nch; //1;
		wfx.Format.nSamplesPerSec = sample_rate;
		wfx.Format.nBlockAlign = bytes_per_sample * nch; //2;
		wfx.Format.nAvgBytesPerSec = wfx.Format.nSamplesPerSec * wfx.Format.nBlockAlign;
		wfx.Format.wBitsPerSample = floating_point ? 32 : 16;
		wfx.Format.cbSize = sizeof(WAVEFORMATEXTENSIBLE)-sizeof(WAVEFORMATEX);
		wfx.Samples.wValidBitsPerSample = wfx.Format.wBitsPerSample;
		wfx.SubFormat = floating_point ? KSDATAFORMAT_SUBTYPE_IEEE_FLOAT : KSDATAFORMAT_SUBTYPE_PCM;
		wfx.dwChannelMask = nch == 2 ? KSAUDIO_SPEAKER_STEREO : KSAUDIO_SPEAKER_MONO;
#else
		WAVEFORMATEX wfx;
		wfx.wFormatTag = floating_point ? WAVE_FORMAT_IEEE_FLOAT : WAVE_FORMAT_PCM;
		wfx.nChannels = nch; //1;
		wfx.nSamplesPerSec = sample_rate;
		wfx.nBlockAlign = bytes_per_sample * nch; //2;
		wfx.nAvgBytesPerSec = wfx.nSamplesPerSec * wfx.nBlockAlign;
		wfx.wBitsPerSample = floating_point ? 32 : 16;
		wfx.cbSize = 0;
#endif
        HRESULT hr = XAudio2Create( &xaud, 0 );
		if (FAILED(hr)) return "Creating XAudio2 interface";
		hr = xaud->CreateMasteringVoice(
			&mVoice,
			nch,
			sample_rate,
			0,
			NULL,
			NULL );
		if (FAILED(hr)) return "Creating XAudio2 mastering voice";
		hr = xaud->CreateSourceVoice( &sVoice, &wfx, 0, 4.0f, &notify );
		if (FAILED(hr)) return "Creating XAudio2 source voice";
		hr = sVoice->Start( 0 );
		if (FAILED(hr)) return "Starting XAudio2 voice";
		hr = sVoice->SetFrequencyRatio((float)1.0f);
		if (FAILED(hr)) return "Setting XAudio2 voice frequency ratio";
		device_changed = false;
		buffered_count = 0;
		buffer_read_cursor = 0;
		buffer_write_cursor = 0;
		samples_played = 0;
		sample_buffer = new uint8_t[ max_samples_per_frame * num_frames * bytes_per_sample ];
		samples_in_buffer = new UINT64[ num_frames ];
		memset( samples_in_buffer, 0, sizeof( UINT64 ) * num_frames );
		return NULL;
	}

	void close()
	{
		if( sVoice ) {
			if( !paused ) {
				sVoice->Stop( 0 );
			}
			sVoice->DestroyVoice();
			sVoice = NULL;
		}

		if( mVoice ) {
			mVoice->DestroyVoice();
			mVoice = NULL;
		}

		if( xaud ) {
			xaud->Release();
			xaud = NULL;
		}

		delete [] sample_buffer;
		sample_buffer = NULL;
		delete [] samples_in_buffer;
		samples_in_buffer = NULL;
	}

	virtual const char* write_frame( void * buffer, unsigned num_samples, bool wait )
	{
		if ( device_changed )
		{
			close();
			reopen_count = 5;
			device_changed = false;
			return 0;
		}

		if ( paused )
		{
			if ( wait ) Sleep( MulDiv( num_samples / nch, 1000, sample_rate ) );
			return 0;
		}

		if ( reopen_count )
		{
			if ( ! --reopen_count )
			{
				const char * err = open( hwnd, sample_rate, nch, bytes_per_sample == 4, max_samples_per_frame, num_frames );
				if ( err )
				{
					reopen_count = 60 * 5;
					return err;
				}
			}
			else
			{
				if ( wait ) Sleep( MulDiv( num_samples / nch, 1000, sample_rate ) );
				return 0;
			}
		}

		for (;;) {
			sVoice->GetState( &vState );
			assert( vState.BuffersQueued <= num_frames );
			if( vState.BuffersQueued < num_frames ) {
				if( vState.BuffersQueued == 0 ) {
					// buffers ran dry
				}
				// there is at least one free buffer
				break;
			} else {
				// wait for one buffer to finish playing
				const DWORD timeout_ms = ( max_samples_per_frame / nch ) * num_frames * 1000 / sample_rate;
				if ( WaitForSingleObject( notify.hBufferEndEvent, timeout_ms ) == WAIT_TIMEOUT )
				{
					// buffer has stalled, likely by the whole XAudio2 system failing, so we should tear it down and attempt to reopen it
					close();
					reopen_count = 5;

					return 0;
				}
			}
		}
		samples_in_buffer[ buffer_write_cursor ] = num_samples / nch;
		XAUDIO2_BUFFER buf = {0};
		unsigned num_bytes = num_samples * bytes_per_sample;
		buf.AudioBytes = num_bytes;
		buf.pAudioData = sample_buffer + max_samples_per_frame * buffer_write_cursor * bytes_per_sample;
		buf.pContext = this;
		buffer_write_cursor = ( buffer_write_cursor + 1 ) % num_frames;
		memcpy( ( void * ) buf.pAudioData, buffer, num_bytes );
		if( sVoice->SubmitSourceBuffer( &buf ) == S_OK )
		{
			InterlockedIncrement( &buffered_count );
			return 0;
		}

		close();
		reopen_count = 60 * 5;

		return 0;
	}

	virtual const char* pause( bool pausing )
	{
		if ( pausing )
		{
			if ( ! paused )
			{
				paused = true;
				if ( !reopen_count )
				{
					HRESULT hr = sVoice->Stop( 0 );
					if ( FAILED(hr) )
					{
						close();
						reopen_count = 60 * 5;
					}
				}
			}
		}
		else
		{
			if ( paused )
			{
				paused = false;
				if ( !reopen_count )
				{
					HRESULT hr = sVoice->Start( 0 );
					if ( FAILED(hr) )
					{
						close();
						reopen_count = 60 * 5;
					}
				}
			}
		}

		return 0;
	}

	virtual const char* set_ratio( double ratio )
	{
		if ( !reopen_count && FAILED( sVoice->SetFrequencyRatio( static_cast<float>(ratio) ) ) ) return "setting ratio";
		return 0;
	}

	virtual double buffered()
	{
		if ( reopen_count ) return 0.0;
		sVoice->GetState( &vState );
		double buffered_count = vState.BuffersQueued;
		INT64 samples_played = vState.SamplesPlayed - this->samples_played;
		buffered_count -= double( samples_played ) / double( max_samples_per_frame / nch );
		return buffered_count;
	}
};

void xaudio2_device_changed( sound_out_i_xaudio2 * p_instance )
{
	p_instance->OnDeviceChanged();
}

sound_out * create_sound_out_xaudio2()
{
	return new sound_out_i_xaudio2;
}
