
#ifndef _NTFUNCS_H
#define _NTFUNCS_H

#include <Windows.h>
#include <cassert>

namespace {
	static unsigned int (WINAPI* pNtDelayExecution)(unsigned char, signed long long*) = nullptr;

	bool PrepZwDelayExecution() {
		if (pNtDelayExecution)
			return true;

		auto mod = GetModuleHandleA("ntdll");
		assert(mod != 0);

		if (!mod)
			return false;

		auto v = (unsigned int (WINAPI*)(unsigned char, signed long long*))GetProcAddress(mod, "NtDelayExecution");
		assert(v != 0);

		if (v == nullptr)
			return false;

		pNtDelayExecution = v;
		return true;
	}

	unsigned int NtDelayExecution(signed long long v) {
		return pNtDelayExecution(0, &v);
	}
}

#endif