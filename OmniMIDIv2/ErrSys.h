/*
Shakra Driver component for Windows
This .h file contains the required code to run the driver under Windows 8.1 and later.

This file is useful only if you want to compile the driver under Windows, it's not needed for Linux/macOS porting.
*/

#ifndef _ERRSYS_H
#define _ERRSYS_H

#pragma once

#define S2(x)			#x								// Convert to string
#define S1(x)			S2(x)							// Convert to string
#define FU				_T(__FUNCTION__)				// Function
#define LI				_T(S1(__LINE__))				// Line
#define FI				_T(__FILE__)					// File

#define NERROR(x, y, z)	x.ThrowError(y, FU, FI, LI, z)
#define FNERROR(x, y)	x.ThrowFatalError(y)

#if _DEBUG
#define LOG(x, y)		x.Log(y, FU, FI, LI)
#define LOGV(x, y)		x.Log(_T(S1(y)), FU, FI, LI)
#else
#define LOG(x, y)		NULL
#define LOGV(x, y)		NULL
#endif

#include <Windows.h>
#include <tchar.h>
#include <string>

using namespace std;

namespace ErrorSystem {
	class WinErr {
	private:
		static const int BufSize = 2048;
		static const int SZBufSize = sizeof(wchar_t) * BufSize;

	public:
		void Log(const wchar_t* Error, const wchar_t* Position, const wchar_t* File, const wchar_t* Line);
		void ThrowError(const wchar_t* Error, const wchar_t* Position, const wchar_t* File, const wchar_t* Line, bool IsSeriousError);
		void ThrowFatalError(const wchar_t* Error);
	};
}

#endif