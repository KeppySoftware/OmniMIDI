
#ifndef _NTFUNCS_H
#define _NTFUNCS_H

#include <Windows.h>
#include <cassert>

namespace NT {
	class Funcs {
	private:
		unsigned int (WINAPI* pNtDelayExecution)(unsigned char, signed long long*) = nullptr;
		unsigned int (WINAPI* pNtQuerySystemTime)(signed long long*) = nullptr;
		bool Ready = false;

	public:
		Funcs() {
			auto mod = GetModuleHandleA("ntdll");
			assert(mod != 0);

			if (!mod)
				return;

			auto v1 = (unsigned int (WINAPI*)(unsigned char, signed long long*))GetProcAddress(mod, "NtDelayExecution");
			assert(v1 != 0);

			if (v1 == nullptr)
				return;

			pNtDelayExecution = v1;

			auto v2 = (unsigned int (WINAPI*)(signed long long*))GetProcAddress(mod, "NtQuerySystemTime");
			assert(v2 != 0);

			if (v2 == nullptr)
				return;

			pNtQuerySystemTime = v2;

			Ready = true;
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