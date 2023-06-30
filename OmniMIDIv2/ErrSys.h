/*
Shakra Driver component for Windows
This .h file contains the required code to run the driver under Windows 8.1 and later.

This file is useful only if you want to compile the driver under Windows, it's not needed for Linux/macOS porting.
*/

#ifndef _ERRSYS_H
#define _ERRSYS_H

#pragma once

#define S2(x)				#x								// Convert to string
#define S1(x)				S2(x)							// Convert to string
#define FU					__FUNCTION__					// Function
#define LI					S1(__LINE__)					// Line
#define FI					__FILE__						// File

#define NERROR(x, y, z)		x.ThrowError(y, FU, FI, LI, z)
#define FNERROR(x, y)		x.ThrowFatalError(y)

#define NERRORW(x, y, z)	x.ThrowErrorW(y, _T(FU), _T(FI), _T(LI), z)
#define FNERRORW(x, y)		x.ThrowFatalErrorW(y)

#if _DEBUG
#define LOG(x, y)		x.Log(y, FU, FI, LI)
#define LOGV(x, y)		x.Log(S1(y), FU, FI, LI)
#define LOGW(x, y)		x.LogW(y, _T(FU), _T(FI), _T(LI))
#define LOGVW(x, y)		x.LogW(S1(y), _T(FU), _T(FI), _T(LI))
#else
#define LOG(x, y)		NULL
#define LOGV(x, y)		NULL
#endif

#include <Windows.h>
#include <tchar.h>
#include <string>

namespace ErrorSystem {
	class WinErr {
	private:
		static const int BufSize = 2048;
		static const int SZBufSize = sizeof(char) * BufSize;

	public:
		void Log(const char* Error, const char* Position, const char* File, const char* Line);
		void ThrowError(const char* Error, const char* Position, const char* File, const char* Line, bool IsSeriousError);
		void ThrowFatalError(const char* Error);

		void LogW(const wchar_t* Error, const wchar_t* Position, const wchar_t* File, const wchar_t* Line);
		void ThrowErrorW(const wchar_t* Error, const wchar_t* Position, const wchar_t* File, const wchar_t* Line, bool IsSeriousError);
		void ThrowFatalErrorW(const wchar_t* Error);
	};
}

#endif