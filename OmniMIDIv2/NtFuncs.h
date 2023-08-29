
#ifndef _NTFUNCS_H
#define _NTFUNCS_H

#include <Windows.h>
#include <cassert>

namespace NT {
	class Funcs {
	private:
		HMODULE ntdll = nullptr;
		bool LL = false;
		unsigned int (WINAPI* pNtDelayExecution)(unsigned char, signed long long*) = nullptr;
		unsigned int (WINAPI* pNtQuerySystemTime)(signed long long*) = nullptr;

	public:
		Funcs() {
			ntdll = GetModuleHandleA("ntdll");
			
			if (ntdll) {
				// ... How?
				LL = true;
				ntdll = LoadLibraryA("ntdll");
			}

			assert(ntdll != 0);
			if (!ntdll)
				return;

			auto v1 = (unsigned int (WINAPI*)(unsigned char, signed long long*))GetProcAddress(ntdll, "NtDelayExecution");
			assert(v1 != 0);

			if (v1 == nullptr)
				return;

			pNtDelayExecution = v1;

			auto v2 = (unsigned int (WINAPI*)(signed long long*))GetProcAddress(ntdll, "NtQuerySystemTime");
			assert(v2 != 0);

			if (v2 == nullptr)
				return;

			pNtQuerySystemTime = v2;
		}

		~Funcs() {
			if (LL) {
				if (!FreeLibrary(ntdll))
					throw;

				ntdll = nullptr;
			}
		}

		unsigned int uSleep(signed long long v) {
			return pNtDelayExecution(0, &v);
		}

		unsigned int querySystemTime(signed long long* v) {
			return pNtQuerySystemTime(v);
		}
	};
}

#endif