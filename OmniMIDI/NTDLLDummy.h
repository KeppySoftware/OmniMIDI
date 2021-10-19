// NTDLL dummy

#pragma once

extern "C" NTSTATUS WINAPI NtSetTimerResolution(IN ULONG a1, IN BOOLEAN a2, OUT PULONG a3);
extern "C" NTSTATUS WINAPI NtQueryTimerResolution(OUT PULONG a1, OUT PULONG a2, OUT PULONG a3);
extern "C" NTSTATUS WINAPI NtDelayExecution(BOOLEAN a1, INT64* a2);
extern "C" NTSTATUS WINAPI NtQuerySystemTime(QWORD* a1);