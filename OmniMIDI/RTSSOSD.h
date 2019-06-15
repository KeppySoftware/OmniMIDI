#pragma once
#include <RTSSSharedMemory.h>

// Check used to see if system is allowed to use OSD
static BOOL CanUseOSD = FALSE;

BOOL IsOSDAvailable() {
	if (CanUseOSD) {
		HANDLE hMapFile = OpenFileMapping(FILE_MAP_READ, FALSE, L"RTSSSharedMemoryV2");

		if (hMapFile) {
			CloseHandle(hMapFile);
			return TRUE;
		}
	}

	return FALSE;
}

BOOL UpdateOSD(LPCSTR lpText) {
	BOOL bResult = FALSE;

	if (CanUseOSD) {
		HANDLE hMapFile = OpenFileMapping(FILE_MAP_ALL_ACCESS, FALSE, L"RTSSSharedMemoryV2");

		if (hMapFile)
		{
			LPVOID pMapAddr = MapViewOfFile(hMapFile, FILE_MAP_ALL_ACCESS, 0, 0, 0);
			LPRTSS_SHARED_MEMORY pMem = (LPRTSS_SHARED_MEMORY)pMapAddr;

			if (pMem)
			{
				if ((pMem->dwSignature == 'RTSS') && (pMem->dwVersion >= 0x00020000))
				{
					for (DWORD dwPass = 0; dwPass < 2; dwPass++)
					{
						for (DWORD dwEntry = 1; dwEntry < pMem->dwOSDArrSize; dwEntry++)
						{
							RTSS_SHARED_MEMORY::LPRTSS_SHARED_MEMORY_OSD_ENTRY pEntry = (RTSS_SHARED_MEMORY::LPRTSS_SHARED_MEMORY_OSD_ENTRY)((LPBYTE)pMem + pMem->dwOSDArrOffset + dwEntry * pMem->dwOSDEntrySize);

							if (dwPass)
							{
								if (!strlen(pEntry->szOSDOwner))
									strcpy_s(pEntry->szOSDOwner, sizeof(pEntry->szOSDOwner), "RTSSSharedMemorySample");
							}

							if (!strcmp(pEntry->szOSDOwner, "RTSSSharedMemorySample"))
							{
								if (pMem->dwVersion >= 0x00020007)
								{
									if (pMem->dwVersion >= 0x0002000e)
									{
										DWORD dwBusy = _interlockedbittestandset(&pMem->dwBusy, 0);

										if (!dwBusy)
										{
											strncpy_s(pEntry->szOSDEx, sizeof(pEntry->szOSDEx), lpText, sizeof(pEntry->szOSDEx) - 1);
											pMem->dwBusy = 0;
										}
									}
									else strncpy_s(pEntry->szOSDEx, sizeof(pEntry->szOSDEx), lpText, sizeof(pEntry->szOSDEx) - 1);

								}
								else strncpy_s(pEntry->szOSD, sizeof(pEntry->szOSD), lpText, sizeof(pEntry->szOSD) - 1);

								pMem->dwOSDFrame++;
								bResult = TRUE;
								break;
							}
						}

						if (bResult) break;
					}
				}
				UnmapViewOfFile(pMapAddr);
			}
			CloseHandle(hMapFile);
		}
	}

	return bResult;
}

void ReleaseOSD() {
	CanUseOSD = FALSE;

	HANDLE hMapFile = OpenFileMapping(FILE_MAP_ALL_ACCESS, FALSE, L"RTSSSharedMemoryV2");

	if (hMapFile)
	{
		LPVOID pMapAddr = MapViewOfFile(hMapFile, FILE_MAP_ALL_ACCESS, 0, 0, 0);

		LPRTSS_SHARED_MEMORY pMem = (LPRTSS_SHARED_MEMORY)pMapAddr;

		if (pMem)
		{
			if ((pMem->dwSignature == 'RTSS') && (pMem->dwVersion >= 0x00020000))
			{
				for (DWORD dwEntry = 1; dwEntry < pMem->dwOSDArrSize; dwEntry++)
				{
					RTSS_SHARED_MEMORY::LPRTSS_SHARED_MEMORY_OSD_ENTRY pEntry = (RTSS_SHARED_MEMORY::LPRTSS_SHARED_MEMORY_OSD_ENTRY)((LPBYTE)pMem + pMem->dwOSDArrOffset + dwEntry * pMem->dwOSDEntrySize);

					if (!strcmp(pEntry->szOSDOwner, "RTSSSharedMemorySample"))
					{
						memset(pEntry, 0, pMem->dwOSDEntrySize);
						pMem->dwOSDFrame++;
					}
				}
			}

			UnmapViewOfFile(pMapAddr);
		}

		CloseHandle(hMapFile);
	}
}