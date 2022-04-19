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

#define DERROR(y)				ShakraError(y, FU, FI, LI)
#define DLOG(y)					ShakraError(y, FU, FI, LI)

// Define the maximum midiX
#define MIN_DEVICEID			1
#define MAX_DEVICEID			10
#define CLEANUP_DEVICEID		32

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
const wchar_t DEVICE_DESCRIPTION[] = L"OmniMIDI for Windows NT\0";
const wchar_t DRIVER_PROVIDER_NAME[] = L"Keppy's Software\0";
const wchar_t DRIVER_CLASS_PROP_DRIVER_DESC[] = L"DriverDesc";
const wchar_t DRIVER_CLASS_PROP_PROVIDER_NAME[] = L"ProviderName";
const wchar_t DRIVER_CLASS_SUBKEY_DRIVERS[] = L"Drivers";
const wchar_t DRIVER_CLASS_PROP_SUBCLASSES[] = L"SubClasses";
const wchar_t DRIVER_CLASS_SUBCLASSES[] = L"midi";
const wchar_t DRIVER_SUBCLASS_SUBKEYS[] = L"midi\\OmniMIDI.dll\0";
const wchar_t DRIVER_SUBCLASS_PROP_DRIVER[] = L"Driver";
const wchar_t DRIVER_SUBCLASS_PROP_DESCRIPTION[] = L"Description";
const wchar_t DRIVER_SUBCLASS_PROP_ALIAS[] = L"Alias";
const wchar_t OMOLD_DRIVER_NAME[] = L"OmniMIDI.dll";
const wchar_t SHAKRA_DRIVER_NAME[] = L"OmniMIDI.dll\0";
const wchar_t WDMAUD_DRIVER_NAME[] = L"wdmaud.drv";

static bool DriverBusy = false;

// MIDI REG
const wchar_t MIDI_REGISTRY_ENTRY_TEMPLATE[] = L"midi%d\0";

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

#ifdef DEBUGV
	MessageBox(NULL, Buf, L"Shakra - Debug message", MB_OK | MB_SYSTEMMODAL | MB_ICONWARNING);
#endif

	free(Buf);
}

void __stdcall DriverRegistration(HWND HWND, HINSTANCE HinstanceDLL, LPSTR CommandLine, DWORD CmdShow) {
	// Used for registration
	LSTATUS DrvWOW64 = ERROR_SUCCESS, Drv32 = ERROR_SUCCESS;
	HKEY DeviceRegKey, DriverSubKey, DriversSubKey, Drivers32Key, DriversWOW64Key;
	HDEVINFO DeviceInfo;
	SP_DEVINFO_DATA DeviceInfoData;
	DEVPROPTYPE DevicePropertyType;
	wchar_t szBuffer[4096];
	DWORD dwSize, dwPropertyRegDataType, dummy;
	DWORD configClass = CONFIGFLAG_MANUAL_INSTALL | CONFIGFLAG_NEEDS_FORCED_CONFIG;
	DWORD sztype = REG_SZ;

	// Drivers32 Shakra
	wchar_t Drv[255], Alias[255];
	wchar_t Buf32[255];
	wchar_t Buf64[255];
	wchar_t ShakraKey[255];
	bool OnlyDrivers32 = true;

	// What's the first MIDIx slot available? We'll check later.
	bool MIDISlots[9] = { true, true, true, true, true, true, true, true, true };
	std::vector<std::wstring> Drvs;

	// We need to register, woohoo
	if (_stricmp(CommandLine, "RegisterDrv") == 0 || _stricmp(CommandLine, "RegisterDrvS") == 0 ||
		_stricmp(CommandLine, "UnregisterDrv") == 0 || _stricmp(CommandLine, "UnregisterDrvS") == 0) {
		// Check the argument sent by RunDLL32, and see what the heck it wants from us
		bool Registration = (_stricmp(CommandLine, "RegisterDrv") == 0 || _stricmp(CommandLine, "RegisterDrvS") == 0);
		bool Silent = (_stricmp(CommandLine, "RegisterDrvS") == 0 || _stricmp(CommandLine, "UnregisterDrvS") == 0);

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

			// NEW MIDI SLOT DETECTION FROM OMNIMIDI 15.x+

			// Open the PnP settings of the device index
			HKEY DevCheckKey = SetupDiOpenDevRegKey(DeviceInfo, &DeviceInfoData, DICS_FLAG_GLOBAL, NULL, DIREG_DRV, KEY_READ);
			TCHAR achKey[255];
			DWORD cbName = 255;

			// ---------------------------------------
			// I know this code looks like a mess but
			// it's literally the only way to prevent
			// Microsoft from messing around with the
			// MIDI slots.					    - kep
			// ---------------------------------------

			// Open the Drivers subkey
			if (!RegOpenKeyExW(DevCheckKey, L"Drivers", NULL, KEY_READ, &DevCheckKey))
			{
				// Open the midi subkey
				if (!RegOpenKeyExW(DevCheckKey, L"midi", NULL, KEY_READ, &DevCheckKey))
				{
					// Get the first subkey inside the "midi" key (it's always one only)
					if (!RegEnumKeyExW(DevCheckKey, 0, achKey, &cbName, NULL, NULL, NULL, NULL))
					{
						// Open the subkey you got, and check the Alias
						if (!RegOpenKeyExW(DevCheckKey, achKey, NULL, KEY_READ, &DevCheckKey))
						{
							DWORD ABufSize = sizeof(Alias), BBufSize = sizeof(Buf32);

							// Loop until you find the Alias
							for (int MIDIEntry = MIN_DEVICEID; MIDIEntry < CLEANUP_DEVICEID; MIDIEntry++)
							{
								ZeroMemory(ShakraKey, sizeof(ShakraKey));
								swprintf_s(ShakraKey, sizeof(ShakraKey), (wchar_t*)MIDI_REGISTRY_ENTRY_TEMPLATE, MIDIEntry);

								ZeroMemory(Buf32, sizeof(Buf32));
								if (!RegQueryValueExW(DevCheckKey, L"Driver", 0, &sztype, (LPBYTE)&Drv, &BBufSize))
								{
									DLOG(L"Got driver");

									// Check if the driver matches OM
									if (_wcsicmp(Buf32, SHAKRA_DRIVER_NAME) == 0)
									{
										// It does! We're in.
										if (Registration)
										{
											DERROR(L"The driver is already registered!");
											RegCloseKey(DevCheckKey);
											return;
										}
										else {
											OnlyDrivers32 = false;
											break;
										}
									}
									else
									{
										DLOG(L"Driver is not OmniMIDI");

										ZeroMemory(Buf32, sizeof(Buf32));
										ZeroMemory(Alias, sizeof(Alias));
										if (!RegQueryValueExW(DevCheckKey, L"Alias", 0, &sztype, (LPBYTE)&Alias, &ABufSize))
										{
											DLOG(Alias);

											auto Position = Drvs.begin() + MIDIEntry - 1;

											ZeroMemory(Buf32, sizeof(Buf32));
											if (!RegQueryValueExW(DevCheckKey, L"Description", 0, &sztype, (LPBYTE)&Buf32, &BBufSize))
												Drvs.insert(Position, Buf32);

											DLOG(Buf32);
										}

										// It doesn't, we're not in
										if (MIDIEntry < MAX_DEVICEID)
											MIDISlots[MIDIEntry - 1] = false;
									}
								}
							}
						}
					}
				}
			}

			// Close the key
			RegCloseKey(DevCheckKey);

			if (!OnlyDrivers32)
				break;
		}

		if (Registration)
		{
			// Open Drivers32 for WOW64 key
			if (RegCreateKeyEx(HKEY_LOCAL_MACHINE, L"SOFTWARE\\Microsoft\\Windows NT\\CurrentVersion\\Drivers32", 0, NULL, REG_OPTION_NON_VOLATILE, KEY_ALL_ACCESS | KEY_WOW64_32KEY, NULL, &DriversWOW64Key, &dummy) != ERROR_SUCCESS) {
				DERROR(L"RegCreateKeyEx failed to open the Drivers32 key from the WOW64 hive, unable to (un)register the driver.");
				return;
			}

#ifdef _WIN64
			// Open Drivers32 key
			if (RegCreateKeyEx(HKEY_LOCAL_MACHINE, L"SOFTWARE\\Microsoft\\Windows NT\\CurrentVersion\\Drivers32", 0, NULL, REG_OPTION_NON_VOLATILE, KEY_ALL_ACCESS, NULL, &Drivers32Key, &dummy) != ERROR_SUCCESS) {
				DERROR(L"RegCreateKeyEx failed to open the Drivers32 key, unable to (un)register the driver.");
				return;
			}
#endif

			// Create the info for the device
			if (!SetupDiCreateDeviceInfo(DeviceInfo, DEVICE_NAME_MEDIA, &DevGUID, DEVICE_DESCRIPTION, NULL, DICD_GENERATE_ID, &DeviceInfoData))
			{
				SetupDiDestroyDeviceInfoList(DeviceInfo);
				DERROR(L"SetupDiCreateDeviceInfo failed, unable to register the driver.");
				return;
			}

			// Register the info you created to the PnP system
			if (!SetupDiRegisterDeviceInfo(DeviceInfo, &DeviceInfoData, NULL, NULL, NULL, NULL)) {
				SetupDiDestroyDeviceInfoList(DeviceInfo);
				DERROR(L"SetupDiRegisterDeviceInfo failed, unable to register the driver.");
				return;
			}

			// Populate the device settings
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

			// Remove old values
			for (int MIDIEntry = MIN_DEVICEID; MIDIEntry < CLEANUP_DEVICEID; MIDIEntry++)
			{
				DWORD BufSize = sizeof(Buf32);

				ZeroMemory(Buf32, BufSize);

				ZeroMemory(ShakraKey, sizeof(ShakraKey));
				swprintf_s(ShakraKey, sizeof(ShakraKey), (wchar_t*)MIDI_REGISTRY_ENTRY_TEMPLATE, MIDIEntry);

				DrvWOW64 = RegQueryValueExW(DriversWOW64Key, ShakraKey, 0, &sztype, (LPBYTE)&Buf32, &BufSize);
				if (DrvWOW64 == ERROR_SUCCESS)
				{
					if (_wcsicmp(Buf32, SHAKRA_DRIVER_NAME) == 0 || _wcsicmp(Buf32, OMOLD_DRIVER_NAME) == 0)
						RegDeleteValueW(DriversWOW64Key, ShakraKey);
				}
			}

#ifdef _WIN64
			for (int MIDIEntry = MIN_DEVICEID; MIDIEntry < CLEANUP_DEVICEID; MIDIEntry++)
			{
				DWORD BufSize = sizeof(Buf64);

				ZeroMemory(Buf64, BufSize);

				ZeroMemory(ShakraKey, sizeof(ShakraKey));
				swprintf_s(ShakraKey, sizeof(ShakraKey), (wchar_t*)MIDI_REGISTRY_ENTRY_TEMPLATE, MIDIEntry);

				Drv32 = RegQueryValueExW(Drivers32Key, ShakraKey, 0, &sztype, (LPBYTE)&Buf64, &BufSize);
				if (Drv32 == ERROR_SUCCESS)
				{
					if (_wcsicmp(Buf64, SHAKRA_DRIVER_NAME) == 0 || _wcsicmp(Buf64, OMOLD_DRIVER_NAME) == 0)
						RegDeleteValueW(Drivers32Key, ShakraKey);
				}
			}
#endif

			bool Pass = false;
			int FinalID = 0;
			for (int MIDIEntry = 0; MIDIEntry < MAX_DEVICEID; MIDIEntry++)
			{
				// If the slot isn't available, quit.
				if (!MIDISlots[MIDIEntry]) continue;

				// Slot is empty! Register.
				else {
					Pass = true;
					FinalID = MIDIEntry + 1;
					break;
				}
			}

			if (Pass) {
				// Everything went fine, let's finish registering the device.
				ZeroMemory(ShakraKey, sizeof(ShakraKey));
				swprintf_s(ShakraKey, MIDI_REGISTRY_ENTRY_TEMPLATE, FinalID);

				if (RegSetValueEx(DriversWOW64Key, ShakraKey, NULL, REG_SZ, (LPBYTE)SHAKRA_DRIVER_NAME, sizeof(SHAKRA_DRIVER_NAME)) != ERROR_SUCCESS)
				{
					DERROR(L"RegSetValueEx failed to write the MIDI alias to Drivers32 (x86), unable to register the driver.");
					return;
				}

#ifdef _WIN64
				if (RegSetValueEx(Drivers32Key, ShakraKey, NULL, REG_SZ, (LPBYTE)SHAKRA_DRIVER_NAME, sizeof(SHAKRA_DRIVER_NAME)) != ERROR_SUCCESS)
				{
					DERROR(L"RegSetValueEx failed to write the MIDI alias to Drivers32 (x64), unable to register the driver.");
					return;
				}
#endif

				if (RegSetValueExW(DriverSubKey, DRIVER_SUBCLASS_PROP_ALIAS, NULL, REG_SZ, (LPBYTE)ShakraKey, sizeof(ShakraKey)) != ERROR_SUCCESS)
				{
					DERROR(L"RegSetValueEx failed to write DRIVER_SUBCLASS_PROP_ALIAS32, unable to register the driver.");
					return;
				}
			}
			// Failed to register, let's delete the virtual device.
			else {
				wchar_t F[] = L"Slot free or no info";

				SetupDiRemoveDevice(DeviceInfo, &DeviceInfoData);
				RegDeleteValueW(DriversWOW64Key, ShakraKey);
#ifdef _WIN64
				RegDeleteValueW(Drivers32Key, ShakraKey);
#endif
				ZeroMemory(Buf32, sizeof(Buf32));
				swprintf_s(Buf32, sizeof(Buf32),
					L"No MIDI slots available for OmniMIDI to register properly!\nDown below you can find a list of which slots are available at the moment.\n\nPress OK to quit.\n\nmidi1: %s\nmidi2: %s\nmidi3: %s\nmidi4: %s\nmidi5: %s\nmidi6: %s\nmidi7: %s\nmidi8: %s\nmidi9: %s",
					MIDISlots[0] ? F : Drvs[0].c_str(), MIDISlots[1] ? F : Drvs[1].c_str(), MIDISlots[2] ? F : Drvs[2].c_str(), MIDISlots[3] ? F : Drvs[3].c_str(),
					MIDISlots[4] ? F : Drvs[4].c_str(), MIDISlots[5] ? F : Drvs[5].c_str(), MIDISlots[6] ? F : Drvs[6].c_str(), MIDISlots[7] ? F : Drvs[7].c_str(),
					MIDISlots[8] ? F : Drvs[8].c_str());

				MessageBox(HWND, Buf32, L"OmniMIDI - ERROR", MB_OK | MB_ICONERROR);
			}

			SetupDiDestroyDeviceInfoList(DeviceInfo);

			RegCloseKey(Drivers32Key);
#ifdef _WIN64
			RegCloseKey(DriversWOW64Key);
#endif
			RegCloseKey(DeviceRegKey);
			RegCloseKey(DriversSubKey);

			if (!Silent && Pass) MessageBox(HWND, L"Driver successfully registered!", L"OmniMIDI - Success", MB_OK | MB_ICONINFORMATION);
		}
		else
		{
			bool ShowMsg = false;

			// Open Drivers32 for WOW64 key
			if (RegCreateKeyEx(HKEY_LOCAL_MACHINE, L"SOFTWARE\\Microsoft\\Windows NT\\CurrentVersion\\Drivers32", 0, NULL, REG_OPTION_NON_VOLATILE, KEY_ALL_ACCESS | KEY_WOW64_32KEY, NULL, &DriversWOW64Key, &dummy) != ERROR_SUCCESS) {
				DERROR(L"RegCreateKeyEx failed to open the Drivers32 key from the WOW64 hive, unable to (un)register the driver.");
				return;
			}

#ifdef _WIN64
			// Open Drivers32 key
			if (RegCreateKeyEx(HKEY_LOCAL_MACHINE, L"SOFTWARE\\Microsoft\\Windows NT\\CurrentVersion\\Drivers32", 0, NULL, REG_OPTION_NON_VOLATILE, KEY_ALL_ACCESS, NULL, &Drivers32Key, &dummy) != ERROR_SUCCESS) {
				DERROR(L"RegCreateKeyEx failed to open the Drivers32 key, unable to (un)register the driver.");
				return;
			}
#endif

			if (!OnlyDrivers32)
			{
				if (!SetupDiRemoveDevice(DeviceInfo, &DeviceInfoData))
				{
					if (GetLastError() != ERROR_FILE_NOT_FOUND)
					{
						DERROR(L"SetupDiRemoveDevice failed, unable to unregister the driver.");
						return;
					}
					else MessageBox(HWND, L"The virtual OmniMIDI device has been removed already.", L"OmniMIDI - Information", MB_OK | MB_ICONINFORMATION);
				
					SetupDiDestroyDeviceInfoList(DeviceInfo);
				}
			}

			// Remove old values
			for (int MIDIEntry = MIN_DEVICEID; MIDIEntry < CLEANUP_DEVICEID; MIDIEntry++)
			{
				DWORD BufSize = sizeof(Buf32);

				ZeroMemory(Buf32, BufSize);

				ZeroMemory(ShakraKey, sizeof(ShakraKey));
				swprintf_s(ShakraKey, sizeof(ShakraKey), (wchar_t*)MIDI_REGISTRY_ENTRY_TEMPLATE, MIDIEntry);

				DrvWOW64 = RegQueryValueExW(DriversWOW64Key, ShakraKey, 0, &sztype, (LPBYTE)&Buf32, &BufSize);
				if (DrvWOW64 == ERROR_SUCCESS)
				{
					if (_wcsicmp(Buf32, SHAKRA_DRIVER_NAME) == 0 || _wcsicmp(Buf32, OMOLD_DRIVER_NAME) == 0)
						RegDeleteValueW(DriversWOW64Key, ShakraKey);
				}
			}

			RegFlushKey(DriversWOW64Key);
			RegCloseKey(DriversWOW64Key);

#ifdef _WIN64
			for (int MIDIEntry = MIN_DEVICEID; MIDIEntry < CLEANUP_DEVICEID; MIDIEntry++)
			{
				DWORD BufSize = sizeof(Buf64);

				ZeroMemory(Buf64, BufSize);

				ZeroMemory(ShakraKey, sizeof(ShakraKey));
				swprintf_s(ShakraKey, sizeof(ShakraKey), (wchar_t*)MIDI_REGISTRY_ENTRY_TEMPLATE, MIDIEntry);

				Drv32 = RegQueryValueExW(Drivers32Key, ShakraKey, 0, &sztype, (LPBYTE)&Buf64, &BufSize);
				if (Drv32 == ERROR_SUCCESS)
				{
					if (_wcsicmp(Buf64, SHAKRA_DRIVER_NAME) == 0 || _wcsicmp(Buf64, OMOLD_DRIVER_NAME) == 0)
						RegDeleteValueW(Drivers32Key, ShakraKey);
				}
			}

			RegFlushKey(Drivers32Key);
			RegCloseKey(Drivers32Key);
#endif

			SetupDiDestroyDeviceInfoList(DeviceInfo);

			if (ShowMsg && !Silent)
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