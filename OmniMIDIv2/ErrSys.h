/*

	OmniMIDI v15+ (Rewrite) for Windows NT

	This file contains the required code to run the driver under Windows 7 SP1 and later.
	This file is useful only if you want to compile the driver under Windows, it's not needed for Linux/macOS porting.

*/

#ifndef _ERRSYS_H
#define _ERRSYS_H

#include <Windows.h>
#include <tchar.h>
#include <string>
#include <string_view>
#include <stdexcept>
#include "SrcLoc.h"

#define S2(x)					#x
#define S1(x)					S2(x)
#define HERE					OmniMIDI::source_location::current()

#define NERROR(x, y, z, ...)	x.ThrowError(y, HERE, z, 0, __VA_ARGS__)
#define FNERROR(x, y)			x.ThrowFatalError(y, HERE)

#define NERRORW(x, y, z, ...)	x.ThrowErrorW(y, HERE, z, 0, __VA_ARGS__)
#define FNERRORW(x, y, ...)		x.ThrowFatalErrorW(y, HERE)

#if _DEBUG
#define LOG(x, y, ...)			x.Log(y, HERE, 0, __VA_ARGS__)
#define LOGV(x, y, ...)			x.Log(S1(y), HERE, 0, __VA_ARGS__)
#define LOGW(x, y, ...)			x.LogW(y, HERE, 0, __VA_ARGS__)
#define LOGVW(x, y, ...)		x.LogW(S1(y), HERE, 0, __VA_ARGS__)
#else
#define LOG(x, y, ...)			NULL
#define LOGV(x, y, ...)			NULL
#endif

namespace ErrorSystem {
	class WinErr {
	private:
		static const int BufSize = 2048;
		static const int SZBufSize = sizeof(char) * BufSize;

	public:
		void Log(const char* Error, const OmniMIDI::source_location& location, int dummy, ...);
		void ThrowError(const char* Error, const OmniMIDI::source_location& location, bool IsSeriousError, int dummy, ...);
		void ThrowFatalError(const char* Error, const OmniMIDI::source_location& location);

		void LogW(const wchar_t* Error, const OmniMIDI::source_location& location, int dummy, ...);
		void ThrowErrorW(const wchar_t* Error, const OmniMIDI::source_location& location, bool IsSeriousError, int dummy, ...);
		void ThrowFatalErrorW(const wchar_t* Error, const OmniMIDI::source_location& location);
	};
}

#endif