/*
OmniMIDI v15+ (Rewrite) for Windows NT

This file contains the required code to run the driver under Windows 7 SP1 and later.
This file is useful only if you want to compile the driver under Windows, it's not needed for Linux/macOS porting.
*/

#include "ErrSys.h"

#ifdef _WIN32

void ErrorSystem::WinErr::Log(const char* Message, const char* Position, const char* File, const char* Line) {
	char* Buf = new char[SZBufSize];

	sprintf_s(Buf, BufSize, "Howdy from \"%s\".\n\nFile: %s\nLine: %s\n\nMessage: %s", Position, File, Line, Message);

	OutputDebugStringA(Buf);

#ifdef _DEBUG
	MessageBoxA(NULL, Buf, "OmniMIDI - DEBUG", MB_OK | MB_SYSTEMMODAL | MB_ICONWARNING);
#endif

	delete[] Buf;
}

void ErrorSystem::WinErr::ThrowError(const char* Error, const char* Position, const char* File, const char* Line, bool IsSeriousError) {
	int GLE = GetLastError();
	char* Buf = nullptr;
	LPSTR GLEBuf = nullptr;

	if (!Error) {
		size_t MsgBufSize = FormatMessageA(
			FORMAT_MESSAGE_FROM_SYSTEM |
			FORMAT_MESSAGE_IGNORE_INSERTS |
			FORMAT_MESSAGE_ALLOCATE_BUFFER,
			NULL, GLE,
			MAKELANGID(LANG_NEUTRAL, SUBLANG_DEFAULT),
			(LPSTR)&GLEBuf, 0, NULL);

		if (MsgBufSize != 0)
		{
			MessageBoxA(NULL, GLEBuf, "OmniMIDI - ERROR", IsSeriousError ? MB_ICONERROR : MB_ICONWARNING | MB_OK | MB_SYSTEMMODAL);
			LocalFree(GLEBuf);
		}
	}
	else {
		Buf = new char[SZBufSize];
#ifdef _DEBUG
		sprintf_s(Buf, BufSize, "An error has occured in the \"%s\" function!\n\nFile: %s\nLine: %s\n\nError: %s", Position, File, Line, Error);
#else
		sprintf_s(Buf, BufSize, "An error has occured in the \"%s\" function!\n\nError: %s", Position, Error);
#endif

		MessageBoxA(NULL, Buf, "OmniMIDI - ERROR", IsSeriousError ? MB_ICONERROR : MB_ICONWARNING | MB_OK | MB_SYSTEMMODAL);

		delete[] Buf;
	}
}

void ErrorSystem::WinErr::ThrowFatalError(const char* Error) {
	char* Buf = new char[SZBufSize];

	sprintf_s(Buf, BufSize, "A fatal error has occured from which the driver is unable to recover!\n\nError: %s", Error);

	MessageBoxA(NULL, Buf, "OmniMIDI - FATAL ERROR", MB_ICONERROR | MB_OK | MB_SYSTEMMODAL);

	delete[] Buf;

	throw ::GetLastError();
}

#endif