// Thread classes for WTL
// Copyright (C) Till Krullmann.
//
// The use and distribution terms for this software are covered by the
// Common Public License 1.0 (http://opensource.org/licenses/cpl.php)
// which can be found in the file CPL.TXT at the root of this distribution.
// By using this software in any fashion, you are agreeing to be bound by
// the terms of this license. You must not remove this notice, or
// any other, from this software.

#pragma once

#ifndef _ATL_MIN_CRT
#include <process.h>
#endif



template <bool t_bManaged>
class CThreadT
{
protected:
	HANDLE m_hThread;
	DWORD m_dwThreadId;

public:
	CThreadT(HANDLE hThread = NULL, DWORD dwThreadId = 0)
		: m_hThread(hThread), m_dwThreadId(dwThreadId)
	{
#if WINVER >= 0x0502
		if ( m_hThread != NULL && m_dwThreadId == 0 )
			m_dwThreadId = ::GetThreadId(m_hThread);
#endif
	}


	CThreadT(const CThreadT& otherThread)
	{
		m_dwThreadId = otherThread.m_dwThreadId;
		if ( t_bManaged && otherThread.m_hThread != NULL )
		{
			HANDLE hProcess = GetCurrentProcess();
			DuplicateHandle( hProcess, otherThread.m_hThread, hProcess, &m_hThread,
				0, FALSE, DUPLICATE_SAME_ACCESS );
			ATLASSERT( m_hThread != NULL );
		}
		else
			m_hThread = otherThread.m_hThread;
	}


	~CThreadT()
	{
		if ( t_bManaged && m_hThread != NULL )
			CloseHandle(m_hThread);
	}


	static CThreadT Create(LPTHREAD_START_ROUTINE pThreadProc, LPVOID pParam = NULL,
		DWORD dwCreationFlags = 0, LPSECURITY_ATTRIBUTES pSecurityAttr = NULL,
		DWORD dwStackSize = 0)
	{
		DWORD dwThreadId = 0;
#ifdef _ATL_MIN_CRT
		HANDLE hThread = CreateThread(pSecurityAttr, dwStackSize, pThreadProc, pParam,
			dwCreationFlags, &dwThreadId);
#else
		HANDLE hThread = (HANDLE) _beginthreadex(pSecurityAttr, dwStackSize,
			(unsigned (__stdcall*)(void*)) pThreadProc,
			pParam, dwCreationFlags, (unsigned*) &dwThreadId);
#endif
		return CThreadT(hThread, dwThreadId);
	}


	static CThreadT Open(DWORD dwThreadId, DWORD dwDesiredAccess = THREAD_ALL_ACCESS,
		BOOL bInheritHandle = FALSE)
	{
		HANDLE hThread = OpenThread(dwDesiredAccess, bInheritHandle, dwThreadId);
		return CThreadT(hThread, dwThreadId);
	}



	DWORD GetId() const
	{
		return m_dwThreadId;
	}


	virtual HANDLE GetHandle() const
	{
		return m_hThread;
	}


	int GetPriority() const
	{
		ATLASSERT( m_hThread != NULL );
		return GetThreadPriority(m_hThread);
	}


	BOOL SetPriority(int nPriority)
	{
		ATLASSERT( m_hThread != NULL );
		return SetThreadPriority(m_hThread, nPriority);
	}


	DWORD GetExitCode() const
	{
		ATLASSERT( m_hThread != NULL );
		DWORD dwExitCode = 0;
		if ( GetExitCodeThread(m_hThread, &dwExitCode) )
			return dwExitCode;
		else
			return (DWORD) -1;
	}


	BOOL GetThreadTimes(LPFILETIME pCreationTime, LPFILETIME pExitTime, LPFILETIME pKernelTime,
		LPFILETIME pUserTime) const
	{
		ATLASSERT( m_hThread != NULL );
		return ::GetThreadTimes(m_hThread, pCreationTime, pExitTime, pKernelTime, pUserTime);
	}


#if _WIN32_WINNT >= 0x0501
	BOOL IsIOPending() const
	{
		ATLASSERT( m_hThread != NULL );
		BOOL bIOPending = FALSE;
		GetThreadIOPendingFlag(m_hThread, &bIOPending);
		return bIOPending;
	}
#endif

	
	DWORD Resume()
	{
		ATLASSERT( m_hThread != NULL );
		return ResumeThread(m_hThread);
	}


	DWORD Suspend()
	{
		ATLASSERT( m_hThread != NULL );
		return SuspendThread(m_hThread);
	}


	BOOL Terminate(DWORD dwExitCode = 0)
	{
		ATLASSERT( m_hThread != NULL );
		return TerminateThread(m_hThread, dwExitCode);
	}


	void Exit(DWORD dwExitCode = 0)
	{
		// Make sure this is only called from the thread that this object represents
		ATLASSERT( m_dwThreadId == ::GetCurrentThreadId() );

#ifdef _ATL_MIN_CRT
		ExitThread(dwExitCode);
#else
		_endthreadex(dwExitCode);
#endif
	}


	DWORD Join(DWORD dwWaitMilliseconds = INFINITE) const
	{
		ATLASSERT( m_hThread != NULL );
        return WaitForSingleObject(m_hThread, dwWaitMilliseconds);
	}
};


typedef CThreadT<true> CThread;
typedef CThreadT<false> CThreadHandle;





template <class T>
__declspec(naked) DWORD WINAPI _ThreadProcThunk(void*)
{
	__asm
	{
		pop		eax
		pop		ecx
		push	eax
		xor		eax, eax
		jmp T::Run
	}
}



template <class T, class TBase = CThread>
class CThreadImpl : public TBase
{
	typedef CThreadImpl<T, TBase> thisClass;

public:
	CThreadImpl(DWORD dwCreationFlags = 0)
		: TBase(NULL, NULL)
	{
		m_hThread = CreateThread(NULL, 0, (LPTHREAD_START_ROUTINE) _ThreadProcThunk<T>,
			this, dwCreationFlags, &m_dwThreadId);
	}
};





#include <atlwin.h>
#include <atlapp.h>

template <bool t_bManaged>
class CGuiThreadT : public CThreadT<t_bManaged>
{
	typedef CThreadT<t_bManaged> baseClass;

public:
	const _ATL_MSG* m_pCurrentMsg;

	CGuiThreadT(HANDLE hThread, DWORD dwThreadId)
		: baseClass(hThread, dwThreadId)
	{ }

	CGuiThreadT(const CThreadT& otherThread)
		: baseClass(otherThread)
	{ }

	BOOL PostThreadMessage(UINT uMsg, WPARAM wParam = 0, LPARAM lParam = 0L)
	{
		DWORD dwThreadId = GetId();
		ATLASSERT( dwThreadId != 0 );
		return ::PostThreadMessage( dwThreadId, uMsg, wParam, lParam );
	}
	
	BOOL PostQuitMessage()
	{
		return PostThreadMessage(WM_QUIT);
	}
};


typedef CGuiThreadT<true> CGuiThread;
typedef CGuiThreadT<false> CGuiThreadHandle;



template <class T, class TBase = CGuiThread>
class CGuiThreadImpl :
	public CThreadImpl<T, TBase>,
	public CMessageFilter,
	public CMessageMap
{
	DECLARE_EMPTY_MSG_MAP()

private:
	CAppModule* m_pModule;

public:
	CGuiThreadImpl(CAppModule* pModule, DWORD dwCreationFlags = 0)
		: m_pModule(pModule), CThreadImpl<T, TBase>(dwCreationFlags)
	{
		ATLASSERT( pModule != NULL );
	}

	BOOL InitializeThread()
	{
		return TRUE;
	}

	void CleanupThread(DWORD dwExitCode)
	{ }

	virtual BOOL PreTranslateMessage(MSG* pMsg)
	{
		if ( pMsg->hwnd != NULL )
			return FALSE;

		T* pT = static_cast<T*>(this);
		LRESULT lResult = 0;
		return pT->ProcessWindowMessage( NULL, pMsg->message, pMsg->wParam,
			pMsg->lParam, lResult, 0 );
	}

	DWORD Run()
	{
		CMessageLoop msgLoop;
		m_pModule->AddMessageLoop(&msgLoop);

		msgLoop.AddMessageFilter(this);

		T* pT = static_cast<T*>(this);
		if ( !pT->InitializeThread() )
		{
			m_pModule->RemoveMessageLoop();
			return 1;
		}

		DWORD dwExitCode = msgLoop.Run();

		pT->CleanupThread(dwExitCode);

		m_pModule->RemoveMessageLoop();

		return dwExitCode;
	}
};
