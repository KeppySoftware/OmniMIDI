#include "stdafx.h"

#include "sound_out.h"

//#define HAVE_KS_HEADERS

#define STRICT
#include <windows.h>

#ifdef HAVE_KS_HEADERS
#include <ks.h>
#include <ksmedia.h>
#endif

#pragma comment ( lib, "winmm.lib" )

class sound_out_i_waveout : public sound_out
{
	struct PACKET
	{
		WAVEHDR header;
		HANDLE  sync;
	};

	void          * hwnd;

	char          * buffer;

	unsigned        n_packets, cur_packet;

	PACKET        * packets;

	HWAVEOUT        hWo;

	bool            paused;

	unsigned        reopen_count;

	unsigned        sample_rate, nch, max_samples_per_frame, num_frames;

	volatile long   buffered_count;

public:
	sound_out_i_waveout()
	{
		buffer = 0;
		packets = 0;
		hWo = 0;
		paused = false;
		reopen_count = 0;
		buffered_count = 0;
	}

	virtual ~sound_out_i_waveout()
	{
		close();
	}

	virtual const char* open( void * hwnd, unsigned sample_rate, unsigned nch, unsigned max_samples_per_frame, unsigned num_frames )
	{
		this->hwnd = hwnd;
		this->sample_rate = sample_rate;
		this->nch = nch;
		this->max_samples_per_frame = max_samples_per_frame;
		this->num_frames = num_frames;

		unsigned bytes_per_frame = max_samples_per_frame * 2 * nch;

		{
			enum {
				min_packet_samples = 128,
				max_packet_bytes = 0x2000,
			};

			n_packets = num_frames;
			if ( n_packets < 4 ) n_packets = 4;

			unsigned buf_size_samples = max_samples_per_frame * n_packets * nch;

			unsigned buffer_size_bytes = buf_size_samples * 2;
			buffer = ( char * ) LocalAlloc( LMEM_FIXED | LMEM_ZEROINIT, buffer_size_bytes );

			packets = new PACKET[ n_packets ];
			memset( packets, 0, sizeof( PACKET ) * n_packets );

			cur_packet = 0;
		}

#ifdef HAVE_KS_HEADERS
		WAVEFORMATEXTENSIBLE wfx;
		wfx.Format.wFormatTag = WAVE_FORMAT_EXTENSIBLE;
		wfx.Format.nChannels = nch; //1;
		wfx.Format.nSamplesPerSec = sample_rate;
		wfx.Format.nBlockAlign = 2 * nch; //2;
		wfx.Format.nAvgBytesPerSec = wfx.Format.nSamplesPerSec * wfx.Format.nBlockAlign;
		wfx.Format.wBitsPerSample = 16;
		wfx.Format.cbSize = sizeof(WAVEFORMATEXTENSIBLE)-sizeof(WAVEFORMATEX);
		wfx.Samples.wValidBitsPerSample = 16;
		wfx.SubFormat = KSDATAFORMAT_SUBTYPE_PCM;
		wfx.dwChannelMask = nch == 2 ? KSAUDIO_SPEAKER_STEREO : KSAUDIO_SPEAKER_MONO;
#else
		WAVEFORMATEX wfx;
		wfx.wFormatTag = WAVE_FORMAT_PCM;
		wfx.nChannels = nch; //1;
		wfx.nSamplesPerSec = sample_rate;
		wfx.nBlockAlign = 2 * nch; //2;
		wfx.nAvgBytesPerSec = wfx.nSamplesPerSec * wfx.nBlockAlign;
		wfx.wBitsPerSample = 16;
		wfx.cbSize = 0;
#endif
		
		MMRESULT mr = waveOutOpen( & hWo, WAVE_MAPPER, ( WAVEFORMATEX * ) & wfx, ( DWORD_PTR ) &sound_out_i_waveout::g_callback, ( DWORD_PTR ) this, CALLBACK_FUNCTION );

		if ( mr )
		{
			return "Opening output device";
		}

		for ( unsigned i = 0; i < n_packets; ++i )
		{
			packets[ i ].sync = CreateEvent( NULL, TRUE, TRUE, NULL );

			memset( & packets[ i ].header, 0, sizeof( WAVEHDR ) );
			packets[ i ].header.lpData = buffer + i * bytes_per_frame;
		}

		if ( ! paused ) waveOutRestart( hWo );

		buffered_count = 0;

		return NULL;
	}

	void close()
	{
		if ( hWo )
		{
			waveOutReset( hWo );

			for ( unsigned i = 0; i < n_packets; ++i )
			{
				if ( packets[ i ].header.dwFlags & WHDR_PREPARED )
				{
					waveOutUnprepareHeader( hWo, &packets[ i ].header, sizeof( WAVEHDR ) );
				}
				CloseHandle( packets[ i ].sync );
			}
			waveOutClose( hWo );
			hWo = 0;
		}

		if ( packets )
		{
			delete [] packets;
			packets = 0;
		}

		if ( buffer )
		{
			LocalFree( buffer );
			buffer = 0;
		}
	}

	virtual const char* write_frame( void * buffer, unsigned num_samples, bool wait )
	{
		if ( paused )
		{
			if ( wait ) Sleep( MulDiv( num_samples / nch, 1000, sample_rate ) );
			return 0;
		}

		if ( reopen_count )
		{
			if ( ! --reopen_count )
			{
				const char * err = open( hwnd, sample_rate, nch, max_samples_per_frame, num_frames );
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

		if ( WaitForSingleObject( packets[ cur_packet ].sync, wait ? INFINITE : 0 ) != WAIT_OBJECT_0 )
			return "Output packet not ready";

		if ( packets[ cur_packet ].header.dwFlags & WHDR_PREPARED )
		{
			waveOutUnprepareHeader( hWo, & packets[ cur_packet ].header, sizeof( WAVEHDR ) );
		}

		unsigned num_bytes = num_samples * 2;

		memcpy( packets[ cur_packet ].header.lpData, buffer, num_bytes );
		packets[ cur_packet ].header.dwBufferLength = num_bytes;
		packets[ cur_packet ].header.dwBytesRecorded = num_bytes;

		if ( ! waveOutPrepareHeader( hWo, & packets[ cur_packet ].header, sizeof( WAVEHDR ) ) )
		{
			if ( ! waveOutWrite( hWo, & packets[ cur_packet ].header, sizeof( WAVEHDR ) ) )
			{
				ResetEvent( packets[ cur_packet ].sync );

				cur_packet = ( cur_packet + 1 ) % n_packets;

				InterlockedIncrement( & buffered_count );

				return 0;
			}

			waveOutUnprepareHeader( hWo, & packets[ cur_packet ].header, sizeof( WAVEHDR ) );

			//return "waveOutWrite";
		}

		//return "waveOutPrepareHeader";

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
				if ( waveOutPause( hWo ) )
				{
					close();
					reopen_count = 60 * 5;
				}
			}
		}
		else
		{
			if ( paused )
			{
				paused = false;
				if ( waveOutRestart( hWo ) )
				{
					close();
					reopen_count = 60 * 5;
				}
			}
		}

		return 0;
	}

	virtual const char* set_ratio( double )
	{
		return NULL;
	}

	virtual double buffered()
	{
		return buffered_count;
	}

	static void CALLBACK g_callback( HWAVEOUT hwo, UINT uMsg, DWORD_PTR dwInstance, DWORD dwParam1, DWORD dwParam2 )
	{
		sound_out_i_waveout * p_this = reinterpret_cast< sound_out_i_waveout * >( dwInstance );
		if ( p_this )
		{
			p_this->callback( hwo, uMsg, dwParam1, dwParam2 );
		}
	}

public:
	void callback( HWAVEOUT hwo, UINT uMsg, DWORD dwParam1, DWORD dwParam2 )
	{
		if ( hwo != hWo ) return;

		if ( uMsg == WOM_DONE )
		{
			for ( unsigned i = 0; i < n_packets; ++i )
			{
				if ( dwParam1 == ( DWORD ) ( & packets[ i ].header ) )
				{
					InterlockedDecrement( & buffered_count );
					SetEvent( packets[ i ].sync );
					break;
				}
			}
		}
	}
};

sound_out * create_sound_out()
{
	return new sound_out_i_waveout;
}