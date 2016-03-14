#include "stdafx.h"

#include "sound_config.h"

#include <windows.h>
#include <tchar.h>

#include <commctrl.h>

#include "../config/config.h"

#include "../resource.h"

class sound_config
{
	sound_config_t * config;

public:
	bool run( void * hinstance, void * hwnd, sound_config_t * config )
	{
		this->config = config;
		
		return !! DialogBoxParam( ( HINSTANCE ) hinstance, MAKEINTRESOURCE( IDD_SOUND_CONFIG ), ( HWND ) hwnd, g_dlg_proc, ( LPARAM ) this );
	}

private:
	static INT_PTR CALLBACK g_dlg_proc( HWND w, UINT msg, WPARAM wp, LPARAM lp )
	{
		sound_config * p_this;
	
		if(	msg == WM_INITDIALOG )
		{
			p_this = ( sound_config * ) lp;
			SetWindowLongPtr( w, DWLP_USER, ( LONG_PTR ) p_this );
		}
		else
			p_this = reinterpret_cast< sound_config * >( GetWindowLongPtr( w, DWLP_USER ) );
	
		return p_this ? p_this->DlgProc( w, msg, wp, lp ) : FALSE;
	}

public:
	INT_PTR DlgProc( HWND w, UINT msg, WPARAM wp, LPARAM lp )
	{
		switch ( msg )
		{
		case WM_INITDIALOG:
			{
				static const unsigned default_rates[] = { 8000, 11025, 16000, 22050, 32000, 44100, 48000 };

				HWND wc = GetDlgItem( w, IDC_COMBO_SAMPLE_RATE );

				bool selected = false;

				for ( unsigned i = 0; i < ARRAYSIZE( default_rates ); ++i )
				{
					std::tostringstream fmt;

					fmt << default_rates[ i ];
					SendMessage( wc, CB_ADDSTRING, 0, ( LPARAM ) ( const TCHAR * ) fmt.str().c_str() );

					if ( default_rates[ i ] == config->sample_rate )
					{
						SendMessage( wc, CB_SETCURSEL, i, 0 );
						selected = true;
					}
				}

				if ( ! selected )
				{
					std::tostringstream fmt;
					fmt << config->sample_rate;
					SetWindowText( wc, fmt.str().c_str() );
				}

				SendDlgItemMessage( w, IDC_EFFECTS, BM_SETCHECK, config->effects_enabled ? BST_CHECKED : BST_UNCHECKED, 0 );

				wc = GetDlgItem( w, IDC_SLIDER_BASS );
				SendMessage( wc, TBM_SETRANGE, 0, MAKELONG( 0, 255 ) );
				SendMessage( wc, TBM_SETPOS, 1, config->bass );

				wc = GetDlgItem( w, IDC_SLIDER_TREBLE );
				SendMessage( wc, TBM_SETRANGE, 0, MAKELONG( 0, 255 ) );
				SendMessage( wc, TBM_SETPOS, 1, config->treble );

				wc = GetDlgItem( w, IDC_SLIDER_ECHO_DEPTH );
				SendMessage( wc, TBM_SETRANGE, 0, MAKELONG( 0, 255 ) );
				SendMessage( wc, TBM_SETPOS, 1, config->echo_depth );
			}
			break;

		case WM_COMMAND:
			switch ( wp )
			{
			case IDOK:
				config->effects_enabled = SendDlgItemMessage( w, IDC_EFFECTS, BM_GETCHECK, 0, 0 );
				config->bass = SendDlgItemMessage( w, IDC_SLIDER_BASS, TBM_GETPOS, 0, 0 );
				config->treble = SendDlgItemMessage( w, IDC_SLIDER_TREBLE, TBM_GETPOS, 0, 0 );
				config->echo_depth = SendDlgItemMessage( w, IDC_SLIDER_ECHO_DEPTH, TBM_GETPOS, 0, 0 );
				{
					TCHAR temp[ 8 ];
					unsigned len = GetDlgItemText( w, IDC_COMBO_SAMPLE_RATE, temp, 8 );
					if ( len > 3 && len < 6 )
					{
						unsigned rate;
						std::tstring str( temp );
						std::tistringstream fmt( str );
						fmt >> rate;
						if ( rate >= 6000 && rate <= 48000 )
							config->sample_rate = rate;
						else if ( rate < 6000 )
							config->sample_rate = 6000;
						else
							config->sample_rate = 48000;
					}
				}
				EndDialog( w, 1 );
				break;

			case IDCANCEL:
				EndDialog( w, 0 );
				break;
			}
			break;
		}

		return FALSE;
	}
};

bool do_sound_config( void * hinstance, void * hwnd, sound_config_t * config )
{
	sound_config sc;

	return sc.run( hinstance, hwnd, config );
}