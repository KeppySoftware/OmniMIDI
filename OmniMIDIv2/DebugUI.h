/*

	OmniMIDI v15+ (Rewrite) for Windows NT

	This file contains the required code to run the driver under Windows 7 SP1 and later.
	This file is useful only if you want to compile the driver under Windows, it's not needed for Linux/macOS porting.

*/

#ifndef _DEBUGUI_H
#define _DEBUGUI_H

#pragma once

#include <Windows.h>
#include <guiddef.h>
#include <ShlObj_core.h>
#include <strsafe.h>
#include <d3d11.h>
#include <tchar.h>

namespace OmniMIDI {
	class DebugUI {
	private:
		WNDCLASS wc;
		HWND win;

		static LRESULT CALLBACK WndProc(HWND hwnd, UINT message, WPARAM wparam, LPARAM lparam)
		{
			switch (message)
			{
			case WM_DESTROY:
				PostQuitMessage(0);
				break;
			default:
				return DefWindowProc(hwnd, message, wparam, lparam);
			}
			return 0;
		}

	public:
		DebugUI() {
			LPCWSTR cn = L"OMv2 Debug Window";

			wc = { };

			wc.lpfnWndProc = WndProc;
			wc.hInstance = 0;
			wc.lpszClassName = cn;

			wc.style = CS_HREDRAW | CS_VREDRAW;

			if (!RegisterClass(&wc))
				MessageBox(NULL, L"Could not register class", L"Error", MB_OK);

			win = CreateWindow(cn, 0, WS_POPUP, 0, 0, GetSystemMetrics(SM_CXSCREEN), GetSystemMetrics(SM_CYSCREEN), 0, 0, 0, 0);

			ShowWindow(win, SW_RESTORE);
		}

		~DebugUI() {
			DestroyWindow(win);
			DeleteObject(win);
		}
	};
}

#endif