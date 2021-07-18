#pragma once

// Quickest fix for Windows 10 2004+, I don't want to bother making clean code for this garbage project smh
// pino

#include <devguid.h>
#include <newdev.h>
#include <setupapi.h>
#include <Devguid.h>
#include <RegStr.h>

#ifdef DEFINE_DEVPROPKEY
#undef DEFINE_DEVPROPKEY
#endif
#define DEFINE_DEVPROPKEY(name, l, w1, w2, b1, b2, b3, b4, b5, b6, b7, b8, pid) EXTERN_C const DEVPROPKEY DECLSPEC_SELECTANY name = { { l, w1, w2, { b1, b2,  b3,  b4,  b5,  b6,  b7,  b8 } }, pid }

#define S2(x)	#x						// Convert to string
#define S1(x)	S2(x)					// Convert to string
#define FU		_T(__FUNCTION__)		// Function
#define LI		_T(S1(__LINE__))		// Line
#define FI		_T(__FILE__)			// File

#define DERROR(y)				ShakraError(y, FU, FI, LI)
#define DFERROR(x, y)			x.ThrowFatalError(y)
#define DLOG(x, y)				x.Log(y, FU, FI, LI)

// Copied from devpkey.h in the WinDDK
DEFINE_DEVPROPKEY(DEVPKEY_Device_BusReportedDeviceDesc, 0x540b947e, 0x8b40, 0x45bc, 0xa8, 0xa2, 0x6a, 0x0b, 0x89, 0x4c, 0xbd, 0xa2, 4);	// DEVPROP_TYPE_STRING
DEFINE_DEVPROPKEY(DEVPKEY_Device_ContainerId, 0x8c7ed206, 0x3f8a, 0x4827, 0xb3, 0xab, 0xae, 0x9e, 0x1f, 0xae, 0xfc, 0x6c, 2);			// DEVPROP_TYPE_GUID
DEFINE_DEVPROPKEY(DEVPKEY_Device_FriendlyName, 0xa45c254e, 0xdf1c, 0x4efd, 0x80, 0x20, 0x67, 0xd1, 0x46, 0xa8, 0x50, 0xe0, 14);			// DEVPROP_TYPE_STRING
DEFINE_DEVPROPKEY(DEVPKEY_DeviceDisplay_Category, 0x78c34fc8, 0x104a, 0x4aca, 0x9e, 0xa4, 0x52, 0x4d, 0x52, 0x99, 0x6e, 0x57, 0x5a);	// DEVPROP_TYPE_STRING_LIST
DEFINE_DEVPROPKEY(DEVPKEY_Device_LocationInfo, 0xa45c254e, 0xdf1c, 0x4efd, 0x80, 0x20, 0x67, 0xd1, 0x46, 0xa8, 0x50, 0xe0, 15);			// DEVPROP_TYPE_STRING
DEFINE_DEVPROPKEY(DEVPKEY_Device_Manufacturer, 0xa45c254e, 0xdf1c, 0x4efd, 0x80, 0x20, 0x67, 0xd1, 0x46, 0xa8, 0x50, 0xe0, 13);			// DEVPROP_TYPE_STRING
DEFINE_DEVPROPKEY(DEVPKEY_Device_SecuritySDS, 0xa45c254e, 0xdf1c, 0x4efd, 0x80, 0x20, 0x67, 0xd1, 0x46, 0xa8, 0x50, 0xe0, 26);			// DEVPROP_TYPE_SECURITY_DESCRIPTOR_STRING

// IsWoW64Process
typedef WINBASEAPI BOOL(WINAPI* fIW64P)(_In_ HANDLE, _Out_ PBOOL);

const GUID DevGUID = GUID_DEVCLASS_MEDIA;
const wchar_t DEVICE_NAME_MEDIA[] = L"MEDIA";
const wchar_t DEVICE_DESCRIPTION[] = L"OmniMIDI for Windows NT";
const wchar_t DRIVER_PROVIDER_NAME[] = L"Keppy's Software";
const wchar_t DRIVER_CLASS_PROP_DRIVER_DESC[] = L"DriverDesc";
const wchar_t DRIVER_CLASS_PROP_PROVIDER_NAME[] = L"ProviderName";
const wchar_t DRIVER_CLASS_SUBKEY_DRIVERS[] = L"Drivers";
const wchar_t DRIVER_CLASS_PROP_SUBCLASSES[] = L"SubClasses";
const wchar_t DRIVER_CLASS_SUBCLASSES[] = L"midi";
const wchar_t DRIVER_SUBCLASS_SUBKEYS[] = L"midi\\OmniMIDI.dll";
const wchar_t DRIVER_SUBCLASS_PROP_DRIVER[] = L"Driver";
const wchar_t DRIVER_SUBCLASS_PROP_DESCRIPTION[] = L"Description";
const wchar_t DRIVER_SUBCLASS_PROP_ALIAS[] = L"Alias";
const wchar_t SHAKRA_DRIVER_NAME[] = L"OmniMIDI.dll";

static bool DriverBusy = false;

// MIDI REG
const wchar_t MIDI_REGISTRY_ENTRY_TEMPLATE[] = L"midi%d";

static const int ShaBufSize = 2048;
static const int ShaSZBufSize = sizeof(wchar_t) * ShaBufSize;

static bool isWow64Process() {
	BOOL IsUnderWOW64 = false;
	const fIW64P IW64P = (fIW64P)GetProcAddress(GetModuleHandle(L"kernel32"), "IsWow64Process");

	if (IW64P)
		if (!IW64P(GetCurrentProcess(), &IsUnderWOW64))
			return false;

	return IsUnderWOW64;
}

void ShakraError(wchar_t* Msg, wchar_t* Position, wchar_t* File, wchar_t* Line) 
{
	wchar_t* Buf;

	Buf = (wchar_t*)malloc(ShaSZBufSize);
	swprintf_s(Buf, ShaBufSize, L"DEBUG MSG FROM %s.\n\nFile: %s\nLine: %s\n\nMessage: %s", Position, File, Line, Msg);

	OutputDebugString(Buf);

#ifdef _DEBUG
	MessageBox(NULL, Buf, L"Shakra - Debug message", MB_OK | MB_SYSTEMMODAL | MB_ICONWARNING);
#endif

	free(Buf);
}

void __stdcall DriverRegistration(HWND HWND, HINSTANCE HinstanceDLL, LPSTR CommandLine, DWORD CmdShow) {
	// Check the argument sent by RunDLL32, and see what the heck it wants from us
	bool Registration = (_stricmp(CommandLine, "RegisterDrv") == 0), UnregisterPass = false;

	// Used for registration
	HKEY DeviceRegKey, DriverSubKey, DriversSubKey, Drivers32Key, DriversWOW64Key;
	HDEVINFO DeviceInfo;
	SP_DEVINFO_DATA DeviceInfoData;
	DEVPROPTYPE DevicePropertyType;
	wchar_t szBuffer[4096];
	DWORD dwSize, dwPropertyRegDataType, dummy;
	DWORD configClass = CONFIGFLAG_MANUAL_INSTALL | CONFIGFLAG_NEEDS_FORCED_CONFIG;

	// Drivers32 Shakra
	wchar_t Buf[255];
	wchar_t ShakraKey[255];
	DWORD bufSize = sizeof(Buf);
	DWORD shakraSize = sizeof(ShakraKey);
	bool OnlyDrivers32 = true;

	// We need to register, woohoo
	if (_stricmp(CommandLine, "RegisterDrv") == 0 || _stricmp(CommandLine, "UnregisterDrv") == 0) {
		// If user is not an admin, abort.
		if (!IsUserAnAdmin())
		{
			DERROR(L"You can not manage the driver without administration rights.");
			return;
		}

		// If it's running under WoW64 emulation, tell the user to use the x64 DLL under RunDLL32 instead
		if (isWow64Process())
		{
			DERROR(L"You can not register the driver using the 32-bit library under 64-bit Windows.\n\nPlease use the 64-bit library.");
			return;
		}

		// Open Drivers32 key
		if (RegCreateKeyEx(HKEY_LOCAL_MACHINE, L"SOFTWARE\\Microsoft\\Windows NT\\CurrentVersion\\Drivers32", 0, NULL, REG_OPTION_NON_VOLATILE, KEY_ALL_ACCESS, NULL, &Drivers32Key, &dummy) != ERROR_SUCCESS) {
			DERROR(L"RegCreateKeyEx failed to open the Drivers32 key, unable to (un)register the driver.");
			return;
		}

#ifdef _WIN64
		// Open Drivers32 for WOW64 key
		if (RegCreateKeyEx(HKEY_LOCAL_MACHINE, L"SOFTWARE\\Microsoft\\Windows NT\\CurrentVersion\\Drivers32", 0, NULL, REG_OPTION_NON_VOLATILE, KEY_ALL_ACCESS | KEY_WOW64_32KEY, NULL, &DriversWOW64Key, &dummy) != ERROR_SUCCESS) {
			DERROR(L"RegCreateKeyEx failed to open the Drivers32 key from the WOW64 hive, unable to (un)register the driver.");
			return;
		}
#endif

		DeviceInfo = SetupDiGetClassDevs(&DevGUID, NULL, NULL, 0);

		if (DeviceInfo == INVALID_HANDLE_VALUE)
		{
			DERROR(L"SetupDiGetClassDevs returned a NULL struct, unable to register the driver.");
			return;
		}

		// Die Windows 10
		for (int DeviceIndex = 0; ; DeviceIndex++) {
			ZeroMemory(&DeviceInfoData, sizeof(SP_DEVINFO_DATA));
			ZeroMemory(&szBuffer, sizeof(szBuffer));
			DeviceInfoData.cbSize = sizeof(SP_DEVINFO_DATA);

			if (!SetupDiEnumDeviceInfo(DeviceInfo, DeviceIndex, &DeviceInfoData))
				break;

			if (SetupDiGetDeviceRegistryProperty(
				DeviceInfo,
				&DeviceInfoData,
				SPDRP_DEVICEDESC,
				&dwPropertyRegDataType,
				(BYTE*)szBuffer,
				sizeof(szBuffer),
				&dwSize))
			{
				if (_wcsicmp(szBuffer, DEVICE_DESCRIPTION) == 0)
				{
					if (Registration)
					{
						DERROR(L"The driver is already registered!");
						return;
					}
					else
					{
						UnregisterPass = true;
						OnlyDrivers32 = false;
						break;
					}
				}
			}
		}

		// Normal check for 8.1 and lower
		for (int MIDIEntry = 1; MIDIEntry < 32; MIDIEntry++)
		{
#ifdef _WIN64
			int Step = 0;

			if (Step > 1) break;
#endif

			ZeroMemory(Buf, sizeof(Buf));
			ZeroMemory(ShakraKey, sizeof(ShakraKey));

			swprintf_s(ShakraKey, MIDI_REGISTRY_ENTRY_TEMPLATE, MIDIEntry);

#ifdef _WIN64
			if (Step == 0)
			{
#endif
				if (RegQueryValueExW(Drivers32Key, ShakraKey, 0, 0, reinterpret_cast<LPBYTE>(&Buf), &bufSize) == ERROR_SUCCESS)
				{
					if (_wcsicmp(Buf, DRIVER_SUBCLASS_SUBKEYS) == 0)
					{
						UnregisterPass = true;
#ifdef _WIN64
						Step++;
						MIDIEntry = 1;
#else
						break;
#endif
					}
				}
				else continue;
#ifdef _WIN64
			}
			else
			{
				if (RegQueryValueExW(DriversWOW64Key, ShakraKey, 0, 0, reinterpret_cast<LPBYTE>(&Buf), &bufSize) == ERROR_SUCCESS)
				{
					if (_wcsicmp(Buf, DRIVER_SUBCLASS_SUBKEYS) == 0)
					{
						UnregisterPass = true;
						Step++;
					}
				}
				else continue;
			}
#endif
		}

		if (!Registration && !UnregisterPass)
		{
			DERROR(L"The driver is not registered!");
			return;
		}

		if (Registration)
		{
			if (!SetupDiCreateDeviceInfo(DeviceInfo, DEVICE_NAME_MEDIA, &DevGUID, DEVICE_DESCRIPTION, NULL, DICD_GENERATE_ID, &DeviceInfoData))
			{
				SetupDiDestroyDeviceInfoList(DeviceInfo);
				DERROR(L"SetupDiCreateDeviceInfo failed, unable to register the driver.");
				return;
			}

			if (!SetupDiRegisterDeviceInfo(DeviceInfo, &DeviceInfoData, NULL, NULL, NULL, NULL)) {
				SetupDiDestroyDeviceInfoList(DeviceInfo);
				DERROR(L"SetupDiRegisterDeviceInfo failed, unable to register the driver.");
				return;
			}

			if (!SetupDiSetDeviceRegistryProperty(DeviceInfo, &DeviceInfoData, SPDRP_CONFIGFLAGS, (BYTE*)&configClass, sizeof(configClass))) {
				SetupDiDestroyDeviceInfoList(DeviceInfo);
				DERROR(L"SetupDiSetDeviceRegistryProperty failed, unable to register the driver.");
				return;
			}

			if (!SetupDiSetDeviceRegistryProperty(DeviceInfo, &DeviceInfoData, SPDRP_MFG, (BYTE*)&DRIVER_PROVIDER_NAME, sizeof(DRIVER_PROVIDER_NAME))) {
				SetupDiDestroyDeviceInfoList(DeviceInfo);
				DERROR(L"SetupDiSetDeviceRegistryProperty failed, unable to register the driver.");
				return;
			}

			DeviceRegKey = SetupDiCreateDevRegKey(DeviceInfo, &DeviceInfoData, DICS_FLAG_GLOBAL, 0, DIREG_DRV, NULL, NULL);
			SetupDiDestroyDeviceInfoList(DeviceInfo);

			if (DeviceRegKey == INVALID_HANDLE_VALUE)
			{
				DERROR(L"SetupDiCreateDevRegKey returned a NULL registry key, unable to register the driver.");
				return;
			}

			if (RegSetValueEx(DeviceRegKey, DRIVER_CLASS_PROP_DRIVER_DESC, NULL, REG_SZ, (LPBYTE)DEVICE_DESCRIPTION, sizeof(DEVICE_DESCRIPTION)) != ERROR_SUCCESS)
			{
				DERROR(L"RegSetValueEx failed to write DRIVER_CLASS_PROP_DRIVER_DESC, unable to register the driver.");
				return;
			}

			if (RegSetValueEx(DeviceRegKey, DRIVER_CLASS_PROP_PROVIDER_NAME, NULL, REG_SZ, (LPBYTE)DRIVER_PROVIDER_NAME, sizeof(DRIVER_PROVIDER_NAME)) != ERROR_SUCCESS)
			{
				DERROR(L"RegSetValueEx failed to write DRIVER_CLASS_PROP_PROVIDER_NAME, unable to register the driver.");
				return;
			}

			if (RegCreateKeyEx(DeviceRegKey, DRIVER_CLASS_SUBKEY_DRIVERS, NULL, NULL, 0, KEY_ALL_ACCESS, NULL, &DriversSubKey, NULL) != ERROR_SUCCESS)
			{
				DERROR(L"RegSetValueEx failed to write DRIVER_CLASS_SUBKEY_DRIVERS, unable to register the driver.");
				return;
			}

			if (RegSetValueEx(DriversSubKey, DRIVER_CLASS_PROP_SUBCLASSES, NULL, REG_SZ, (LPBYTE)DRIVER_CLASS_SUBCLASSES, sizeof(DRIVER_CLASS_SUBCLASSES)) != ERROR_SUCCESS)
			{
				DERROR(L"RegSetValueEx failed to write DRIVER_CLASS_PROP_SUBCLASSES, unable to register the driver.");
				return;
			}

			if (RegCreateKeyEx(DriversSubKey, DRIVER_SUBCLASS_SUBKEYS, NULL, NULL, 0, KEY_ALL_ACCESS, NULL, &DriverSubKey, NULL) != ERROR_SUCCESS)
			{
				DERROR(L"RegSetValueEx failed to write DRIVER_SUBCLASS_SUBKEYS, unable to register the driver.");
				return;
			}

			if (RegSetValueEx(DriverSubKey, DRIVER_SUBCLASS_PROP_DRIVER, NULL, REG_SZ, (LPBYTE)SHAKRA_DRIVER_NAME, sizeof(SHAKRA_DRIVER_NAME)) != ERROR_SUCCESS)
			{
				DERROR(L"RegSetValueEx failed to write DRIVER_SUBCLASS_PROP_DRIVER, unable to register the driver.");
				return;
			}

			if (RegSetValueEx(DriverSubKey, DRIVER_SUBCLASS_PROP_DESCRIPTION, NULL, REG_SZ, (LPBYTE)DEVICE_DESCRIPTION, sizeof(DEVICE_DESCRIPTION)) != ERROR_SUCCESS)
			{
				DERROR(L"RegSetValueEx failed to write DRIVER_SUBCLASS_PROP_DESCRIPTION, unable to register the driver.");
				return;
			}

			for (int MIDIEntry = 1; MIDIEntry < 32; MIDIEntry++)
			{
				LSTATUS Drv32 = ERROR_SUCCESS, DrvWOW64 = ERROR_SUCCESS;

				ZeroMemory(Buf, bufSize);
				ZeroMemory(ShakraKey, shakraSize);

				swprintf_s(ShakraKey, MIDI_REGISTRY_ENTRY_TEMPLATE, MIDIEntry);

				Drv32 = RegQueryValueExW(Drivers32Key, ShakraKey, 0, 0, reinterpret_cast<LPBYTE>(&Buf), &bufSize);
#ifdef _WIN64
				DrvWOW64 = RegQueryValueExW(DriversWOW64Key, ShakraKey, 0, 0, reinterpret_cast<LPBYTE>(&Buf), &bufSize);
#endif

				if (Drv32 != ERROR_SUCCESS || DrvWOW64 != ERROR_SUCCESS)
				{
					if (Drv32 != ERROR_SUCCESS)
					{
						if (RegSetValueEx(Drivers32Key, ShakraKey, NULL, REG_SZ, (LPBYTE)SHAKRA_DRIVER_NAME, sizeof(SHAKRA_DRIVER_NAME)) != ERROR_SUCCESS)
						{
							DERROR(L"RegSetValueEx failed to write the MIDI alias to Drivers32, unable to register the driver.");
							return;
						}
					}

#ifdef _WIN64
					if (DrvWOW64 != ERROR_SUCCESS)
					{
						if (RegSetValueEx(DriversWOW64Key, ShakraKey, NULL, REG_SZ, (LPBYTE)SHAKRA_DRIVER_NAME, sizeof(SHAKRA_DRIVER_NAME)) != ERROR_SUCCESS)
						{
							DERROR(L"RegSetValueEx failed to write the MIDI alias to Drivers32 in the WOW64 hive, unable to register the driver.");
							return;
						}
					}
#endif

					if (RegSetValueEx(DriverSubKey, DRIVER_SUBCLASS_PROP_ALIAS, NULL, REG_SZ, (LPBYTE)ShakraKey, shakraSize) != ERROR_SUCCESS)
					{
						DERROR(L"RegSetValueEx failed to write DRIVER_SUBCLASS_PROP_ALIAS, unable to register the driver.");
						return;
					}

					break;
				}
				else
				{
					if (_wcsicmp(Buf, DRIVER_SUBCLASS_SUBKEYS) == 0)
					{
						DERROR(L"The driver has been already registered!");
						return;
					}
				}
			}

			RegCloseKey(Drivers32Key);
#ifdef _WIN64
			RegCloseKey(DriversWOW64Key);
#endif
			RegCloseKey(DeviceRegKey);
			RegCloseKey(DriversSubKey);

			MessageBox(HWND, L"Driver successfully registered!", L"OmniMIDI - Success", MB_OK | MB_ICONINFORMATION);
		}
		else
		{
			if (!OnlyDrivers32)
			{
				if (!SetupDiRemoveDevice(DeviceInfo, &DeviceInfoData))
				{
					SetupDiDestroyDeviceInfoList(DeviceInfo);
					DERROR(L"SetupDiRemoveDevice failed, unable to unregister the driver.");
					return;
				}
			}

			for (int MIDIEntry = 1; MIDIEntry < 32; MIDIEntry++)
			{
				ZeroMemory(Buf, sizeof(Buf));
				ZeroMemory(ShakraKey, sizeof(ShakraKey));

				swprintf_s(ShakraKey, MIDI_REGISTRY_ENTRY_TEMPLATE, MIDIEntry);

				if (RegQueryValueExW(Drivers32Key, ShakraKey, 0, 0, reinterpret_cast<LPBYTE>(&Buf), &bufSize) == ERROR_SUCCESS)
				{
					if (_wcsicmp(Buf, DRIVER_SUBCLASS_SUBKEYS) == 0)
					{
						RegDeleteValue(Drivers32Key, ShakraKey);
					}
				}

#ifdef _WIN64
				if (RegQueryValueExW(DriversWOW64Key, ShakraKey, 0, 0, reinterpret_cast<LPBYTE>(&Buf), &bufSize) == ERROR_SUCCESS)
				{
					if (_wcsicmp(Buf, DRIVER_SUBCLASS_SUBKEYS) == 0)
					{
						RegDeleteValue(DriversWOW64Key, ShakraKey);
					}
				}
#endif
			}

			RegCloseKey(Drivers32Key);
#ifdef _WIN64
			RegCloseKey(DriversWOW64Key);
#endif
			SetupDiDestroyDeviceInfoList(DeviceInfo);

			MessageBox(HWND, L"Driver successfully unregistered!", L"OmniMIDI - Success", MB_OK | MB_ICONINFORMATION);
		}

		return;
	}

	// I have no idea?
	else {
		MessageBoxA(
			HWND,
			"RunDLL32 sent an empty or unrecognized command line.\n\nThe driver doesn't know what to do, so press OK to quit.",
			"OmniMIDI", MB_ICONERROR | MB_OK);
		return;
	}
}