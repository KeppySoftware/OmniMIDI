/*

	OmniMIDI v15+ (Rewrite) for Windows NT

	This file contains the required code to run the driver under Windows 7 SP1 and later.
	This file is useful only if you want to compile the driver under Windows, it's not needed for Linux/macOS porting.

*/

#include <Windows.h>

#ifndef _EVBUF_T_H
#define _EVBUF_T_H

#define EvBuf				EvBuf_t

namespace OmniMIDI {
	struct Ev {
		unsigned int Event;
		unsigned int Align[15];
	};

	// Sono's buffer
	class EvBuf_t {
	private:
		Ev* Buffer;
		size_t Size{ 0 };

		// Written by reader thread
		alignas(64) volatile size_t	ReadHead { 0 };
		alignas(64) size_t WriteHeadCached { 0 };

		// Written by writer thread
		alignas(64) volatile size_t	WriteHead { 0 };
		alignas(64) size_t ReadHeadCached { 0 };

#ifdef _STATSDEV
		size_t EventsSent{ 0 };
		size_t EventsSkipped{ 0 };
#endif

	public:
		EvBuf_t(size_t ReqSize) {
			Buffer = new Ev[ReqSize];
			Size = ReqSize;
		}

		~EvBuf_t() {
			Size = 0;
			ReadHead = 0;
			WriteHead = 0;
			ReadHeadCached = 0;
			WriteHeadCached = 0;
#ifdef _STATSDEV
			EventsSent = 0;
			EventsSkipped = 0;
#endif
			delete[] Buffer;
		}

		void GetStats() {
#ifdef _STATSDEV
			char asdf[1024] = {};
			snprintf(asdf, sizeof(asdf), "%llu of %llu events skipped", EventsSkipped, EventsSent);
			MessageBoxA(NULL, asdf, "", 0);
#else
			// Absolutely nothing.
#endif
		}

		bool Push(unsigned int ev) {
			size_t LocalWriteHead = WriteHead;
			size_t NextWriteHead = LocalWriteHead + 1;

			if (NextWriteHead >= Size)
				NextWriteHead = 0;

			if (NextWriteHead == ReadHeadCached)
			{
				ReadHeadCached = ReadHead;
				if (NextWriteHead == ReadHeadCached) {
#ifdef _STATSDEV
					EventsSkipped++;
#endif
					return false;
				}
			}

			Buffer[LocalWriteHead].Event = ev;
			WriteHead = NextWriteHead;

			return true;
		}

		bool Pop(unsigned int& ev) {
			size_t LocalReadHead = ReadHead;
			if (LocalReadHead == WriteHeadCached)
			{
				WriteHeadCached = WriteHead;
				if (LocalReadHead == WriteHeadCached)
					return false;
			}

			size_t NextReadHead = LocalReadHead + 1;
			if (NextReadHead >= Size)
				NextReadHead = 0;

			ev = Buffer[LocalReadHead].Event;
			ReadHead = NextReadHead;

			return true;
		}

		void Peek(unsigned int& ev) {
			size_t NextReadHead = ReadHead + 1;
			if (NextReadHead >= Size)
				NextReadHead = 0;

			ev = Buffer[NextReadHead].Event;
		}
	};
}

#endif