<p align="center">
  <img src="https://i.imgur.com/iFLDs6C.png">
  <br />
  A group of functions, that allows you to bypass the Windows Multimedia API, thus getting rid of the lag caused by its slow buffer system.
</p>

## How can I implement it in my program?
It's quite easy actually.

You can either import the functions through the header and library files here: [DeveloperContent folder](https://github.com/KeppySoftware/OmniMIDI/tree/master/DeveloperContent)<br />
Or you can use this generic example by [Sono (MarcuzD)](https://github.com/SonoSooS), on how to make use of the Keppy's Direct MIDI API with LoadLibrary and GetProcAddress.<br />
It works just like WinMM would, we'll see later the differences between using WinMM as usual, and using WinMM together with Keppy's Direct MIDI:
```c
...
#define ALLOK 0;
#define NOTOK 1;

MMRESULT(WINAPI*mmOutOpen)(LPHMIDIOUT, lphmo, UINT uDeviceID, DWORD_PTR dwCallback, DWORD_PTR dwCallbackInstance, DWORD dwFlags) = 0;
MMRESULT(WINAPI*mmOutClose)(HMIDIOUT hmo) = 0;
MMRESULT(WINAPI*mmOutShortMsg)(HMIDIOUT hmo, DWORD dwMsg) = 0;
UINT(WINAPI*mmOutGetErrorTextA)(MMRESULT mmrError, LPTSTR, lpText, UINT cchText) = 0;

VOID(WINAPI*KShortMsg)(DWORD msg) = 0;
...

...
int SetupWinMM() {
    // Check if WinMM is already in memory
    HDMODULE mm= GetModuleHandle("winmm");
    
    // It's not, load it manually from the system folder
    if (!mm) mm = LoadLibrary("winmm");
    
    // Something went wrong, return the error
    if (!mm) return GetLastError();

    // Load the MIDI functions from it
    mmOutOpen = (void*)GetProcAddress(mm, "midiOutOpen");
    if (!mmOutOpen) return GetLastError();
    mmOutClose = (void*)GetProcAddress(mm, "midiOutClose");
    if (!mmOutClose) return GetLastError();
    mmOutShortMsg = (void*)GetProcAddress(mm, "midiOutShortMsg");
    if (!mmOutShortMsg) return GetLastError();
    mmOutGetErrorTextA = (void*)GetProcAddress(mm, "midiOutGetErrorTextA");
    if (!mmOutGetErrorTextA) return GetLastError();
    
    // Everything's okay, continue
    return ALLOK;
}
...
```

Let's initialize WinMM:
```c
...
int mmstatus = SetupWinMM();
if (mmstatus) {
    // The program failed to initialize WinMM, close it.
    printf("Failed to initialize Windows Multimedia: %i\n", mmstatus);
    exit(0);
}

MMRESULT mmres = mmOutOpen(&hmo, 1, 0, 0, CALLBACK_NULL);
if (mmres != MMSYSERR_NOERROR) {
    // Device 1 doesn't exist or failed to initialize, let's initialize Microsoft GS instead
    mmOutGetErrorTextA(mmres, buf, sizeof(buf));
    printf("OutOpen (%1) %s\n", mmres, buf);
    mmres = mmOutOpen(&hmo, 0, 0, 0, CALLBACK_NULL);
}
if (mmres != MMSYSERR_NOERROR) {
    // Microsoft GS also failed to initialize, close the app
    mmOutGetErrorTextA(mmres, buf, sizeof(buf));
    printf("OutOpen (%1) %s\n", mmres, buf);

    return NOTOK; 
}

// Check if OmniMIDI is loaded into memory, and initialize the Keppy's Direct MIDI calls
KShortMsg = (void*)GetProcAddress(GetModuleHandle("OmniMIDI"), "SendDirectData");
if (KShortMsg) {
   // Replace default WinMM function with the one from the application itself
    puts("KDMAPI initialized.");
    mmOutShortMsg = _KOutShortMsg;
}
...
```

And here's the function from the application itself:
```c
MMRESULT WINAPI _KOutShortMsg(HMIDIOUT hmo, DWORD msg) {
    // Pass the MIDI event to the Keppy's Direct MIDI call, and return the WinMM result
    return KShortMsg(msg);
}
```

As you can see, we're basically replacing the loaded **midiOutShortMsg** call with our own call, **_KOutShortMsg**, which redirects the messages from WinMM directly to (*lol*) the Keppy's Direct MIDI API calls.

## Is it mandatory for me to implement these features, to use OmniMIDI?<br />
Of course not! These calls are a thing for people who care about low latency and performance.<br />
The driver will work fine with the default WinMM => modMessage system too.<br />
It'll be slower when playing Black MIDIs, and the latency will also be higher, but it'll work just fine.

## What functions are available?
As of June 25th, 2021, these are the functions available in the Keppy's Direct MIDI API.<br />
The **"NoBuf"** calls bypass the built-in buffer in OmniMIDI, and directly send the events to the events processing system.<br />
All the functions whose name starts with the **"int_"** nomenclature are not supposed to use by developers. They're internal functions used by WinMMWRP.<br />
### **InitializeKDMAPIStream**<br />
It initializes the driver, its stream and all its required threads. There are no arguments.<br />
The function returns TRUE if everything goes well, or else returns FALSE.

```c
BOOL(WINAPI*KDMInit)() = 0;
KDMInit = (void*)GetProcAddress(GetModuleHandle("OmniMIDI"), "InitializeKDMAPIStream");
...
	if (!KDMInit()) {
		printf("OmniMIDI failed to initialize!");
	}
...
```
<hr />

### **TerminateKMDAPIStream**
It tells the driver to wrap up its stuff and to leave! There are no arguments.

```c
BOOL(WINAPI*KDMStop)() = 0;
KDMStop = (void*)GetProcAddress(GetModuleHandle("OmniMIDI"), "TerminateKDMAPIStream");
...
	if (!KDMStop()) {
		printf("OmniMIDI failed to terminate!");
	}
...
```
<hr />

### **ResetKDMAPIStream**
Resets the MIDI channels. There are no arguments.

```c
void(WINAPI*KDMReset)() = 0;
KDMReset = (void*)GetProcAddress(GetModuleHandle("OmniMIDI"), "ResetKDMAPIStream");
```
<hr />

### **ReturnKDMAPIVer**
It returns the version of the Keppy's Direct MIDI API in 4 separate DWORDs.<br />
The function returns TRUE if everything goes well, or else returns FALSE.

```c
BOOL(WINAPI*KDMAPIVer)(LPDWORD Major, LPDWORD Minor, LPDWORD Build, LPDWORD Revision) = 0;
KDMAPIVer = (void*)GetProcAddress(GetModuleHandle("OmniMIDI"), "ReturnKDMAPIVer");
...
	DWORD Major, Minor, Build, Rev;
	
	if (KDMAPIVer(&Major, &Minor, &Build, &Rev)) {
		// Output example: "KDMAPI version: 1.30.1.0"
		printf("KDMAPI version: %d.%d.%d.%d", Major, Minor, Build, Rev);
	}
	else printf("The function failed.");
...
```
<hr />

### **IsKDMAPIAvailable**
A generic check, useful for people who want to see if KDMAPI v1.2+ is available.<br />
You NEED to call this function at least once, in order to switch the KDMAPI status value in the debug window to active.<br />

```c
BOOL(WINAPI*KDMAPIStatus)() = 0;
KDMAPIStatus = (void*)GetProcAddress(GetModuleHandle("OmniMIDI"), "IsKDMAPIAvailable");
...
	if (!KDMAPIStatus()) {
		printf("KDMAPI is unavailable.");
		exit(0x0);
	}
...
```
<hr />

### **DriverSettings**
Allows developers to get the current settings, or change them from within the app, rather than asking the user to change them in the configurator.<br/>
If the function succeeds, it'll return TRUE, or else it'll return FALSE.<br />
The available arguments are:

- `DWORD Setting`: The setting you want to change, you can find all the valid values in the header.
- `DWORD Mode`: OM_SET if you want to set the setting, or OM_GET if you want to get its current value.
- `LPVOID Value`: A pointer to the value
- `UINT cbValue`: The size of the object. *(sizeof(Value))*
```c
BOOL(WINAPI*KDMDriverSettings)(DWORD Setting, DWORD Mode, LPVOID Value, UINT cbValue) = 0;
KDMDriverSettings = (void*)GetProcAddress(GetModuleHandle("OmniMIDI"), "DriverSettings");
...
	DWORD Voices = 10;
	DWORD Frequency = 0;

	// Tell the driver that you're going to manage the settings from now on
	KDMDriverSettings(0, OM_MANAGE, nullptr, 0);

	// I want to change the voices
	KDMDriverSettings(OM_MAXVOICES, OM_SET, &Voices, sizeof(Voices));
	
	// Now I want to get the current frequency
	if (KDMDriverSettings(OM_AUDIOFREQ, OM_GET, &Frequency, sizeof(Frequency)))
	{	
		// "The frequency is now 44100Hz!"
		printf("The frequency is now %dHz", Frequency);
	}

	// Stop managing the settings
	KDMDriverSettings(0, OM_LEAVE, nullptr, 0);
...
```
<hr />

### **GetDriverDebugInfo**
Allows developers to get the driver's current rendering time and the voices that are currently active in the audio stream.<br />

```c
DebugInfo*(WINAPI*KDMGetDebugInfo)() = 0;
KDMGetDebugInfo = (void*)GetProcAddress(GetModuleHandle("OmniMIDI"), "GetDriverDebugInfo");
...
	DebugInfo* DebugInfoFromDriver;
	DebugInfoFromDriver = KDMGetDebugInfo();
	
	//Do something with the info
	if (!DebugInfoFromDriver)
		printf("Current rendering time: %d\n", DebugInfoFromDriver->RenderingTime); 
	else
		printf("Failed to get pointer to DebugInfo."); 	
...
```
You can get the code for the struct from **"val.h"**: [Click here!](https://github.com/KeppySoftware/OmniMIDI/blob/master/OmniMIDI/Values.h)
<hr />

### **LoadCustomSoundFontsList**
Allows developers to load their own custom SoundFonts or SoundFonts lists.<br />
The available arguments are:

- `TCHAR* Directory`: A pointer to the unicode char array, containing the path.
```c
BOOL(WINAPI*KDMLoadCustomSFList)(TCHAR* Directory) = 0;
KDMLoadCustomSFList = (void*)GetProcAddress(GetModuleHandle("OmniMIDI"), "LoadCustomSoundFontsList");
...
	TCHAR Directory[MAX_PATH];
	wcscpy_s(Directory, MAX_PATH, _TEXT("C:\\MySF.sf2"));
	
	// Forward it to the driver
	if (!KDMLoadCustomSFList(&Directory)) {
		printf("Failed to load SoundFont!");
	}
...
```
<hr />

### **SendCustomEvent**
Allows you to send a custom BASSMIDI event to the driver.<br />
You can learn more about it here: http://www.un4seen.com/doc/#bassmidi/BASS_MIDI_StreamEvent.html<br />
The available arguments are:

- `DWORD eventtype`: The type of event you want to send.
- `DWORD chan`: The target MIDI channel.
- `DWORD param`: The parameters to send to the driver.
```c
BOOL(WINAPI*KSendCustomEvent)(DWORD eventtype, DWORD chan, DWORD param) = 0;
KSendCustomEvent = (void*)GetProcAddress(GetModuleHandle("OmniMIDI"), "SendCustomEvent");
```
<hr />

### **SendDirectData**
Allows you to send MIDI events to the short events buffer of the driver.<br />
This function, unlike midiOutShortMsg, does NOT return a value, and it will fail quietly.
The available arguments are:

- `DWORD dwMsg`: The MIDI event to send to the driver.
```c
VOID(WINAPI*KShortMsg)(DWORD msg) = 0;
KShortMsg = (void*)GetProcAddress(GetModuleHandle("OmniMIDI"), "SendDirectData"); // Or SendDirectDataNoBuf
...
	KShortMsg(0x0);
...
```
<hr />

### **SendDirectDataNoBuf**
Allows you to send MIDI events directly to the driver's MIDI engine, bypassing its buffer.<br />
Doing so will break the last running status, so you'll have to keep track of the last event yourself.<br />
It's a MMRESULT function, but it always returns the same value no matter what. *(MMSYSERR_NOERROR)*<br />
The available arguments are:

- `DWORD dwMsg`: The MIDI event to send to the driver.
```c
MMRESULT(WINAPI*KShortMsgNoBuf)(DWORD msg) = 0;
KShortMsgNoBuf = (void*)GetProcAddress(GetModuleHandle("OmniMIDI"), "SendDirectDataNoBuf"); // Or SendDirectDataNoBuf
...
	KShortMsgNoBuf(0x0);
...
```
<hr />

### **SendDirectLongData/SendDirectLongDataNoBuf**
Allows you to send MIDIHDR/System Exclusive events to the driver.<br />
You can handle the preparation of the buffer through **PrepareLongData**/**UnprepareLongData**.<br />
The available arguments are:

- `MIDIHDR* IIMidiHdr`: The pointer to the MIDIHDR.
- `UINT IIMidiHdrSize`: Size of the MIDIHDR struct passed to IIMidiHdr.
```c
MMRESULT(WINAPI*KLongMsg)(MIDIHDR* IIMidiHdr, UINT IIMidiHdrSize) = 0;
KLongMsg = (void*)GetProcAddress(GetModuleHandle("OmniMIDI"), "SendDirectLongData");
...
	MMRESULT res = KLongMsg(&IIMidiHdr, IIMidiHdrSize);
	if (res) {
		printf("Something went wrong, it's supposed to return 0.\nReturned value: %d", res);
	}
...
```
<hr />

### **SendDirectLongDataNoBuf**
Allows you to send MIDIHDR/System Exclusive events to the driver.<br />
Both functions do the same thing. SendDirectLongDataNoBuf directly calls SendDirectLongData. I left NoBuf for retrocompatibility purpose with old patches.<br />
You can handle the preparation of the buffer through **PrepareLongData**/**UnprepareLongData**.<br />
The available arguments are:

- `LPSTR MidiHdrData`: The pointer to the data buffer.
- `DWORD MidiHdrDataLen`: Length of the data buffer.
```c
MMRESULT(WINAPI*KLongMsg)(MIDIHDR* IIMidiHdr) = 0;
KLongMsg = (void*)GetProcAddress(GetModuleHandle("OmniMIDI"), "SendDirectLongDataNoBuf");
...
	char DataBuf[32768];
...
	// Something with the data buffer...
...
	MMRESULT res = KLongMsgNoBuf(&DataBuf, sizeof(DataBuf));
	if (res) {
		printf("Something went wrong, it's supposed to return 0.\nReturned value: %d", res);
	}
...
```
<hr />

### **PrepareLongData**
Allows you to prepare the MIDIHDR buffer, before sending it to the driver through SendDirectLongData/SendDirectLongDataNoBuf.<br />
Once a buffer is prepared, it becomes read-only.<br />
The available arguments are:

- `MIDIHDR* IIMidiHdr`: The pointer to the MIDIHDR.
- `UINT IIMidiHdrSize`: Size of the MIDIHDR struct passed to IIMidiHdr.
```c
MMRESULT(WINAPI*KPrepLongMsg)(MIDIHDR* IIMidiHdr, UINT IIMidiHdrSize) = 0;
KPrepLongMsg = (void*)GetProcAddress(GetModuleHandle("OmniMIDI"), "PrepareLongData");
...
	MMRESULT res = KPrepLongMsg(&IIMidiHdr, IIMidiHdrSize);
	if (res) {
		printf("Something went wrong, it's supposed to return 0.\nReturned value: %d", res);
	}
...
```
<hr />

### **UnprepareLongData**
Allows you to unprepare the MIDIHDR buffer, to make it writable again.<br />
It is **MANDATORY** to unprepare a MIDIHDR before editing it, since PrepareLongData locks its data.<br />
The available arguments are:

- `MIDIHDR* IIMidiHdr`: The pointer to the MIDIHDR.
- `UINT IIMidiHdrSize`: Size of the MIDIHDR struct passed to IIMidiHdr.
```c
MMRESULT(WINAPI*KUnprepLongMsg)(MIDIHDR* IIMidiHdr, UINT IIMidiHdrSize) = 0;
KUnprepLongMsg = (void*)GetProcAddress(GetModuleHandle("OmniMIDI"), "UnprepareLongData");
...
	MMRESULT res = KUnprepLongMsg(&IIMidiHdr, IIMidiHdrSize);
	if (res) {
		printf("Something went wrong, it's supposed to return 0.\nReturned value: %d", res);
	}
...
```
<hr />

### **timeGetTime64**
A 64-bit version of the timeGetTime function from the Windows Multimedia API.<br />
It has the same precision, but doesn't rollback after reaching UINT_MAX.<br />
The available arguments are:
```c
DWORD64(WINAPI*TGT64)() = 0;
TGT64 = (void*)GetProcAddress(GetModuleHandle("OmniMIDI"), "timeGetTime64");
...
	DWORD64 time = TGT64();
	// Do something with its value... I guess?
...
```
<hr />

### **int_ICF/int_RCF**
These functions are meant to be used by the Windows Multimedia Wrapper, and serve no purpose for KDMAPI developers.