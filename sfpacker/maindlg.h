// maindlg.h : interface of the CMainDlg class
//
/////////////////////////////////////////////////////////////////////////////

#if !defined(AFX_MAINDLG_H__6920296A_4C3F_11D1_AA9A_000000000000__INCLUDED_)
#define AFX_MAINDLG_H__6920296A_4C3F_11D1_AA9A_000000000000__INCLUDED_

#if _MSC_VER >= 1000
#pragma once
#endif // _MSC_VER >= 1000

#include <iostream>
#include <fstream> 
#include "utf8conv.h"
#include "DropFileTarget.h"



#define BASSDEF(f) (WINAPI *f)	// define the BASS/BASSMIDI functions as pointers
#define BASSMIDIDEF(f) (WINAPI *f)
#define LOADBASSFUNCTION(f) *((void**)&f)=GetProcAddress(bass,#f)
#define LOADBASSMIDIFUNCTION(f) *((void**)&f)=GetProcAddress(bassmidi,#f)
#include "../external_packages/bass.h"
#include "../external_packages/bassmidi.h"
using namespace std;
using namespace utf8util;


const struct
{
	TCHAR* descr;
	TCHAR* cmdline;
	DWORD flags;
}
packers[] = {
	{ L"FLAC",                     L"flac --best -",                  0 },
	{ L"LAME (V2)",                L"lame -V 2 -",                    0 },
	{ L"Musepack (Q5)",            L"mpcenc --quality 5 - -",         0 },
	{ L"Musepack (Q6)",            L"mpcenc --quality 6 - -",         0 },
	{ L"Musepack (Q7)",            L"mpcenc --quality 7 - -",         0 },
	{ L"Opus",                     L"opusenc --raw-chan 1 - -",       BASS_MIDI_PACK_NOHEAD },
	{ L"WavPack (lossless)",       L"wavpack -h -",                   0 },
	{ L"WavPack (lossy, HQ)",      L"wavpack -hb384 -",               0 },
	{ L"WavPack (lossy, average)", L"wavpack -hb256 -",               0 },
	{ L"WavPack (lossy, low)",     L"wavpack -hb128 -",               0 },
	{ L"Vorbis (Q3)",              L"oggenc2 -q 3 -",                 0 }
};

unsigned int lossy_ctypes[]={0x10002, 0x10005, 0x10a00,0x10500, 0x11200};

#define BUFSIZE 4096

class CProcessThread : public CThreadImpl<CProcessThread>
{
	HWND m_hWndParent;
	int m_compress_type;
	TCHAR * user_encoderdir;
	TCHAR * sf_path;

public:
	CProcessThread( HWND hWndParent, int compress_type, TCHAR * pEncoderDir, TCHAR * pPath )
		: m_hWndParent( hWndParent ), m_compress_type( compress_type )
	{
		user_encoderdir = _tcsdup( pEncoderDir );
		sf_path = _tcsdup( pPath );
	}
	~CProcessThread()
	{
		free( sf_path );
		free( user_encoderdir );
	}

	DWORD Run()
	{
		TCHAR encoder_string[MAX_PATH] = {0};
		TCHAR packed_fname[MAX_PATH] = {0};
		TCHAR unpacked_fname[MAX_PATH] = {0};
		BASS_MIDI_FONTINFO info;
		HSOUNDFONT sf2 = BASS_MIDI_FontInit(sf_path,BASS_UNICODE);
		BASS_MIDI_FontGetInfo(sf2,&info);

		_tcscpy(packed_fname,sf_path);
		_tcscpy(unpacked_fname,sf_path);
		PathRemoveExtension(packed_fname);
		lstrcat(packed_fname,L".sf2pack");
		PathRemoveExtension(unpacked_fname);
		lstrcat(unpacked_fname,L".sf2");

		if (info.samtype == 0) //pack!
		{

			int sel = m_compress_type;
			DWORD       fileAttr;
			_tcscpy(encoder_string, user_encoderdir);
			_tcscat(encoder_string, packers[sel].cmdline);
			if(!BASS_MIDI_FontPack(sf2,packed_fname,encoder_string,BASS_UNICODE|packers[sel].flags))
			{
				MessageBox(m_hWndParent,L"SoundFont packing failed",L"Error",MB_ICONSTOP);
				BASS_MIDI_FontFree(sf2);
				return 1;
			}
			MessageBox(m_hWndParent,L"SoundFont packing succeeded",L"Success!",MB_ICONINFORMATION);
		}
		else //depack!
		{
			for (int i=0;i<_countof(lossy_ctypes);i++)
			{
				if (info.samtype == lossy_ctypes[i])
				{
					int iResponse = MessageBox(m_hWndParent,L"The file you loaded seems to be a lossy compressed file.\nUsing unpacked SoundFonts from lossy sources is not recommended. Sure you want to unpack?",L"WARNING",MB_YESNO|MB_ICONINFORMATION);
					if (iResponse == IDNO)
					{
						BASS_MIDI_FontFree(sf2);
						return 1;
					}
				}
			}
			if (GetFileAttributes(unpacked_fname) != 0xFFFFFFFF)
			{
				int iResponse = MessageBox(m_hWndParent,L"The unpacked file already exists. Are you sure\nyou want to unpack over it?", L"WARNING",MB_YESNO|MB_ICONINFORMATION);
				if (iResponse == IDNO)
				{
					BASS_MIDI_FontFree(sf2);
					return 1;
				}
			}
			if (!BASS_MIDI_FontUnpack(sf2,unpacked_fname,BASS_UNICODE)) {
				MessageBox(m_hWndParent,L"SoundFont unpacking failed",L"Error",MB_ICONSTOP);
				BASS_MIDI_FontFree(sf2);
				return 1;
			}
			MessageBox(m_hWndParent,L"SoundFont unpacking succeeded",L"Success!",MB_ICONINFORMATION);
			if(info.samtype == 0x10900)
			{
				int iResponse = MessageBox(m_hWndParent,L"You seemed to have unpacked a lossless compressed file.\nWant to delete the packed file and backups? (if they exist)",L"Delete unneeded files?",MB_YESNO|MB_ICONINFORMATION);
				if (iResponse == IDYES)
				{
					BASS_MIDI_FontFree(sf2);
					DeleteFile(packed_fname);
					return 0;
				}
			}

		}
		BASS_MIDI_FontFree(sf2);

		return 0;
	}
};

class CMainDlg : public CDialogImpl<CMainDlg>, public CDropFileTarget<CMainDlg>, public CMessageFilter
{
	HMODULE bass, bassmidi;
	CComboBox compress_type;
	CStatic   compress_str, filename, directoryname;
	CButton  pack_sf, exit;
	CProcessThread * process_thread;
	TCHAR user_encoderdir[MAX_PATH];
	
public:
	enum { IDD = IDD_DIALOG };
	enum { TIMERID = 1337L };

	CMainDlg() : process_thread( NULL )
	{
	}

	virtual BOOL PreTranslateMessage(MSG* pMsg)
	{
		return ::IsDialogMessage(m_hWnd, pMsg);
	}

	BEGIN_MSG_MAP(CMainDlg)
		MESSAGE_HANDLER(WM_INITDIALOG, OnInitDialog)
		COMMAND_ID_HANDLER(ID_PACK, OnPack)
		COMMAND_ID_HANDLER(IDCANCEL, OnCancel)
		COMMAND_ID_HANDLER(IDC_BROWSE, OnBrowse)
		MESSAGE_HANDLER(WM_TIMER, OnTimer)
		CHAIN_MSG_MAP(CDropFileTarget<CMainDlg>)
	END_MSG_MAP()


	LRESULT OnTimer(UINT /*uMsg*/, WPARAM wParam, LPARAM /*lParam*/, BOOL& /*bHandled*/)
	{
		if ( wParam == TIMERID )
		{
			if ( process_thread && process_thread->Join( 10 ) == WAIT_OBJECT_0 )
			{
				delete process_thread;
				process_thread = NULL;
				KillTimer( TIMERID );
				pack_sf.EnableWindow();
				exit.EnableWindow();
			}
		}

		return 0;
	}

	LRESULT OnInitDialog(UINT /*uMsg*/, WPARAM /*wParam*/, LPARAM /*lParam*/, BOOL& /*bHandled*/)
	{
		// center the dialog on the screen
		CenterWindow();
		// register object for message filtering
		CMessageLoop* pLoop = _Module.GetMessageLoop();
		pLoop->AddMessageFilter(this);

		RegisterDropTarget();

		filename = GetDlgItem(IDC_FILENAME);
		compress_str = GetDlgItem(IDC_COMPSTR);
		pack_sf = GetDlgItem(ID_PACK);
		exit = GetDlgItem(IDCANCEL);
		compress_type = GetDlgItem(IDC_COMPTYPE);
		directoryname = GetDlgItem(IDC_PATH);

		for (int i=0;i<_countof(packers);i++)compress_type.AddString((TCHAR*)packers[i].descr);
		compress_type.SetCurSel(0);

		TCHAR installpath[1024] = {0};
		TCHAR basspath[1024] = {0};
		TCHAR bassmidipath[1024] = {0};
		TCHAR bassflacpath[1024] = {0};
		TCHAR basswvpath[1024] = {0};
		GetModuleFileName(GetModuleHandle(0), installpath, 1024);
		PathRemoveFileSpec(installpath);
		lstrcat(basspath,installpath);
		lstrcat(basspath,L"\\bass.dll");
		if (!(bass=LoadLibrary(basspath))) {
			OutputDebugString(L"Failed to load BASS DLL!");
			return FALSE;
		}
		lstrcat(bassmidipath,installpath);
		lstrcat(bassmidipath,L"\\bassmidi.dll");
		if (!(bassmidi=LoadLibrary(bassmidipath))) {
			OutputDebugString(L"Failed to load BASSMIDI DLL!");
			return FALSE;
		}
		/* "load" all the BASS functions that are to be used */
		OutputDebugString(L"Loading BASS functions....");

		LOADBASSFUNCTION(BASS_ErrorGetCode);
		LOADBASSFUNCTION(BASS_SetConfig);
		LOADBASSFUNCTION(BASS_Init);
		LOADBASSFUNCTION(BASS_Free);
		LOADBASSFUNCTION(BASS_GetInfo);
		LOADBASSFUNCTION(BASS_StreamFree);
		LOADBASSFUNCTION(BASS_PluginLoad);
		LOADBASSFUNCTION(BASS_PluginGetInfo);
		LOADBASSMIDIFUNCTION(BASS_MIDI_FontInit);
		LOADBASSMIDIFUNCTION(BASS_MIDI_FontLoad);
		LOADBASSMIDIFUNCTION(BASS_MIDI_FontGetInfo);
		LOADBASSMIDIFUNCTION(BASS_MIDI_FontPack);
		LOADBASSMIDIFUNCTION(BASS_MIDI_FontUnpack);
		LOADBASSMIDIFUNCTION(BASS_MIDI_FontFree);

		BASS_SetConfig(BASS_CONFIG_UPDATEPERIOD,0);
		BASS_Init(0,44100,0,0,NULL);
		

		WIN32_FIND_DATA fd;
		HANDLE fh;
		TCHAR pluginpath[MAX_PATH] = {0};
		lstrcat(pluginpath,installpath);
		lstrcat(pluginpath,L"\\bass*.dll");
		int installpathlength=lstrlen(installpath)+1;
		fh=FindFirstFile(pluginpath,&fd);
		if (fh!=INVALID_HANDLE_VALUE) {
			do {
				HPLUGIN plug;
				pluginpath[installpathlength]=0;
				lstrcat(pluginpath,fd.cFileName);
				plug=BASS_PluginLoad((char*)pluginpath,BASS_UNICODE);
			} while (FindNextFile(fh,&fd));
			FindClose(fh);
		}

	   load_settings();

		return TRUE;
	}

	void load_settings()
	{		
		CRegKeyEx reg;
		CRegKeyEx subkey;
		TCHAR encoder_path[MAX_PATH] = {0};
		
		long lRet;
		ULONG size;
		//synthlist.GetLBText(selection,device_name.GetBuffer(n));
		lRet = reg.Create(HKEY_CURRENT_USER, L"Software\\sfpacker");
		if (lRet == ERROR_SUCCESS){
			lRet = reg.QueryStringValue(L"path",NULL,&size);
			if (lRet == ERROR_SUCCESS) {
				reg.QueryStringValue(L"path",encoder_path,&size);
			}
			else //create new string, gulp! set it to app dir
			{
				TCHAR installpath[MAX_PATH] = {0};
				GetModuleFileName(GetModuleHandle(0), installpath, MAX_PATH);
				PathRemoveFileSpec(installpath);
				_tcscpy(encoder_path,installpath);
				reg.SetStringValue(L"path",encoder_path);
			}
			reg.Close();
		}
		directoryname.SetWindowText(encoder_path);
		_tcscpy(user_encoderdir, encoder_path);
		_tcscat(user_encoderdir, _T("\\"));
	}

	void ProcessFile(LPCTSTR lpszPath)
	{
		filename.SetWindowText(lpszPath);
		TCHAR new_fname[MAX_PATH] = {0};
		const TCHAR * ext = _tcsrchr( lpszPath, _T('.') );
		if ( ext ) ext++;
		if ( !_tcsicmp( ext, _T("sf2")) || !_tcsicmp( ext, _T("sf2pack")) )
		{
			BASS_MIDI_FONTINFO info;
			HSOUNDFONT sf2 = BASS_MIDI_FontInit(lpszPath,BASS_UNICODE);
			const TCHAR* compression;
			BASS_MIDI_FontGetInfo(sf2,&info);

			if ( ((!_tcsicmp( ext, _T("sf2")) && info.samtype != 0)) ||  (!_tcsicmp( ext, _T("sf2pack")) && info.samtype == 0) ) //must rename file on packing/depacking
			{
			    BASS_MIDI_FontFree(sf2);
				int iResponse = MessageBox(L"The file you loaded is not what the extension suggests. Rename the file?",L"WARNING",MB_YESNO|MB_ICONINFORMATION);
				if (iResponse == IDYES)
				{
					_tcscpy(new_fname,lpszPath);
					PathRemoveExtension(new_fname);

					if (!_tcsicmp( ext, L"sf2"))
					{

						lstrcat(new_fname,L".sf2pack");
					}
					else
					{
						lstrcat(new_fname,L".sf2");
					}
					int res = MoveFile(lpszPath,new_fname);
					if (!res)  
					{
						MessageBox(L"Failed to rename the file",L"Error",MB_ICONSTOP);
						return;
					}
					MessageBox(L"File renamed. Now you can convert the file",L"Success",MB_ICONINFORMATION);
					ProcessFile(new_fname);
				}
			}

			wstring utf8 = utf16_from_utf8(info.name);
			switch (info.samtype)
			{
			default:
				compression = L"Unsupported";
				break;
			case -1:
				compression = L"Unknown";
				break;
			case 0:
				compression = L"None";
				break;
			case 0x10002:
				compression = L"Vorbis";
				break;
			case 0x10003:
				compression = L"MP1";
				break;
			case 0x10004:
				compression = L"MP2";
				break;
			case 0x10005:
				compression = L"MP3";
				break;
			case 0x10900:
				compression = L"FLAC";
				break;
			case 0x10901:
				compression = L"OggFLAC";
				break;
			case 0x10500:
				compression = L"WavPack";
				break;
			case 0x10a00:
				compression = L"Musepack";
				break;
			case 0x11200:
				compression = L"Opus";
				break;
			}
			BASS_MIDI_FontFree(sf2);

			if (info.samtype == 0){
				pack_sf.SetWindowText(L"Pack!");
				compress_type.EnableWindow(true);
			}
			else
			{
				pack_sf.SetWindowText(L"Unpack!");
				compress_type.EnableWindow(false);
			}
			compress_str.SetWindowText(compression);
		}	
	}

	int do_fileops(TCHAR* sf_path)
	{
		pack_sf.EnableWindow(FALSE);
		exit.EnableWindow(FALSE);
		process_thread = new CProcessThread( m_hWnd, compress_type.GetCurSel(), user_encoderdir, sf_path );
		SetTimer( TIMERID, 100 );
		return 0;
	}

	LRESULT OnPack(WORD /*wNotifyCode*/, WORD /*wID*/, HWND /*hWndCtl*/, BOOL& /*bHandled*/)
	{
	    TCHAR sf_path[MAX_PATH] ={0};
		filename.GetWindowText(sf_path, sizeof(sf_path));   
		do_fileops(sf_path);
		return 0;
	}

	LRESULT OnBrowse(WORD /*wNotifyCode*/, WORD /*wID*/, HWND /*hWndCtl*/, BOOL& /*bHandled*/)
	{
		TCHAR folderpath[MAX_PATH];
		BROWSEINFO bi;
		memset(&bi, 0, sizeof(bi));
		int retVal = false;
		bi.ulFlags   = BIF_USENEWUI;
		bi.lpszTitle = L"Select the directory that holds your encoders...";
		::OleInitialize(NULL);
		LPITEMIDLIST pIDL = ::SHBrowseForFolder(&bi);

		if(pIDL != NULL)
		{
			// Create a buffer to store the path, then 
			// get the path.
			TCHAR buffer[_MAX_PATH] = {'\0'};
			if(::SHGetPathFromIDList(pIDL, buffer) != 0)
			{
				// Set the string value.
				_tcscpy(folderpath,buffer);
				retVal = true;
			}

			// free the item id list
			CoTaskMemFree(pIDL);
		}
		::OleUninitialize();
		if (retVal)
		{
			CRegKeyEx reg;
			CRegKeyEx subkey;
			TCHAR encoder_path[MAX_PATH] = {0};
			long lRet;
			ULONG size;
			lRet = reg.Create(HKEY_CURRENT_USER, L"Software\\sfpacker");
			if (lRet == ERROR_SUCCESS){
				lRet = reg.QueryStringValue(L"path",NULL,&size);
				if (lRet == ERROR_SUCCESS) {
					reg.SetStringValue(L"path",folderpath);
					directoryname.SetWindowText(folderpath);
				}
				reg.Close();
			}
			_tcscpy(user_encoderdir, folderpath);
			_tcscat(user_encoderdir, _T("\\"));
		}
		return 0;
	}

	LRESULT OnCancel(WORD /*wNotifyCode*/, WORD wID, HWND /*hWndCtl*/, BOOL& /*bHandled*/)
	{
		if (process_thread)
		{
			MessageBox(L"Please wait for the current job to complete.", L"Warning",MB_ICONINFORMATION);
			return 0;
		}
		CloseDialog(wID);
		return 0;
	}

	void CloseDialog(int nVal)
	{
		if (process_thread) process_thread->Join();
		delete process_thread;
		DestroyWindow();
		::PostQuitMessage(nVal);
	}
};


/////////////////////////////////////////////////////////////////////////////

//{{AFX_INSERT_LOCATION}}
// Microsoft Developer Studio will insert additional declarations immediately before the previous line.

#endif // !defined(AFX_MAINDLG_H__6920296A_4C3F_11D1_AA9A_000000000000__INCLUDED_)
