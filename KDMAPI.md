<p align="center">
  <img src="https://i.imgur.com/iFLDs6C.png">
  <br />
  A group of functions, that allows you to bypass the Windows Multimedia API, thus getting rid of the lag caused by its slow buffer system.
</p>

## How can I implement it in my program?
It's quite easy actually.

Here's a generic example by [Sono (MarcuzD)](https://github.com/MarcuzD), on how to make use of the Keppy's Direct MIDI API.<br />
It works just like WinMM would, we'll see later the differences between using WinMM as usual, and using WinMM together with Keppy's Direct MIDI:
```c
...
#define ALLOK 0;
#define NOTOK 1;

MMRESULT(WINAPI*mmOutOpen)(LPHMIDIOUT, lphmo, UINT uDeviceID, DWORD_PTR dwCallback, DWORD_PTR dwCallbackInstance, DWORD dwFlags) = 0;
MMRESULT(WINAPI*mmOutClose)(HMIDIOUT hmo) = 0;
MMRESULT(WINAPI*mmOutShortMsg)(HMIDIOUT hmo, DWORD dwMsg) = 0;
UINT(WINAPI*mmOutGetErrorTextA)(MMRESULT mmrError, LPTSTR, lpText, UINT cchText) = 0;

MMRESULT(WINAPI*KShortMsg)(DWORD msg) = 0;
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
As of June 5th 2018, these are the functions available in the Keppy's Direct MIDI API.<br />
The **"NoBuf"** calls bypass the built-in buffer in OmniMIDI, and directly send the events to the events processing system.<br />
### **InitializeKDMAPIStream**<br />
It initializes the driver, its stream and all its required threads. There are no arguments.

```c
void(WINAPI*KDMInit)() = 0;
KDMInit = (void*)GetProcAddress(GetModuleHandle("OmniMIDI"), "InitializeKDMAPIStream");
```
<hr />

### **TerminateKMDAPIStream**
It tells the driver to wrap up its stuff and to leave! There are no arguments.

```c
void(WINAPI*KDMStop)() = 0;
KDMStop = (void*)GetProcAddress(GetModuleHandle("OmniMIDI"), "TerminateKDMAPIStream");
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
It returns the version of the Keppy's Direct MIDI API, as a string. There are no arguments available.
Used by OmniMIDI Configurator.

```c
char const*(WINAPI*KDMAPIVer)() = 0;
KDMAPIVer = (void*)GetProcAddress(GetModuleHandle("OmniMIDI"), "ReturnKDMAPIVer");
```
<hr />

### **IsKSDAPIAvailable**
A generic check, useful for people who want to see if KSDAPI v1.2+ is available.<br />
You NEED to call this function at least once, in order to switch the KSDAPI status value in the debug window to active.<br />
There are no arguments available, and you have to manually catch the exception, if the function isn't available.

```c
BOOL(WINAPI*KDMAPIStatus)() = 0;
KDMAPIStatus = (void*)GetProcAddress(GetModuleHandle("OmniMIDI"), "IsKDMAPIAvailable");
```
<hr />

### **ChangeDriverSettings**
Allows developers to change the driver's settings from within the app, rather than asking the user to change them in the configurator.<br/>
Sending **0/nullptr** will make it fallback to the settings from the registry.<br />
The available arguments are:

- `const Settings* Struct`: A pointer to your struct.
- `DWORD StructSize`: The size of the struct.
```c
VOID(WINAPI*KDMChangeSettings)(const Settings* Struct, DWORD StructSize) = 0;
KDMChangeSettings = (void*)GetProcAddress(GetModuleHandle("OmniMIDI"), "ChangeDriverSettings");
...
	Settings MySettings;
	
	// Change your settings
	Settings.MaxVoices = 4;
	Settings.AudioFrequency = 22050;
	Settings.LiveChanges = TRUE;
	
	// Push the settings to the driver
	KDMChangeSettings(&MySettings, sizeof(MySettings));
	
	// Now make the driver fallback to the settings from the registry
	KDMChangeSettings(0, 0);
...
```
You can get the code for the struct from **"val.h"**: [Click here!](https://github.com/KeppySoftware/OmniMIDI/blob/master/OmniMIDI/val.h)
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
	printf("Current rendering time: %d\n", DebugInfoFromDriver->RenderingTime); 
...
```
You can get the code for the struct from **"val.h"**: [Click here!](https://github.com/KeppySoftware/OmniMIDI/blob/master/OmniMIDI/val.h)
<hr />

### **LoadCustomSoundFontsList**
Allows developers to load their own custom SoundFonts or SoundFonts lists.<br />
The available arguments are:

- `const TCHAR* Directory`: A pointer to the unicode char array, containing the path.
```c
VOID(WINAPI*KDMLoadCustomSFList)(const TCHAR* Directory) = 0;
KDMLoadCustomSFList = (void*)GetProcAddress(GetModuleHandle("OmniMIDI"), "LoadCustomSoundFontsList");
...
	TCHAR Directory[MAX_PATH];
	wcscpy_s(Directory, MAX_PATH, _TEXT("C:\\MySF.sf2"));
	
	// Forward it to the driver
	KDMLoadCustomSFList(&Directory);
...
```
<hr />

### **SendDirectData/SendDirectDataNoBuf**
Allows you to send MIDI events to the driver.<br />
The available arguments are:

- `DWORD dwMsg`: The MIDI event to send to the driver.
```c
MMRESULT(WINAPI*KShortMsg)(DWORD msg) = 0;
KShortMsg = (void*)GetProcAddress(GetModuleHandle("OmniMIDI"), "SendDirectData"); // Or SendDirectDataNoBuf
```
<hr />

### **SendDirectLongData/SendDirectLongDataNoBuf**
Allows you to send MIDIHDR/System Exclusive events to the driver.<br />
Both functions do the same thing. SendDirectLongDataNoBuf directly calls SendDirectLongData. I left NoBuf for retrocompatibility purpose with old patches.<br />
You can handle the preparation of the buffer through **PrepareLongData**/**UnprepareLongData**.<br />
The available arguments are:

- `MIDIHDR* IIMidiHdr`: The pointer to the MIDIHDR.
```c
MMRESULT(WINAPI*KLongMsg)(MIDIHDR* IIMidiHdr) = 0;
KLongMsg = (void*)GetProcAddress(GetModuleHandle("OmniMIDI"), "SendDirectLongData"); // Or SendDirectLongDataNoBuf
```
<hr />

### **PrepareLongData**
Allows you to prepare the MIDIHDR buffer, before sending it to the driver through SendDirectLongData/SendDirectLongDataNoBuf.<br />
Once a buffer is prepared, it becomes read-only.<br />
The available arguments are:

- `MIDIHDR* IIMidiHdr`: The pointer to the MIDIHDR.
```c
MMRESULT(WINAPI*KPrepLongMsg)(MIDIHDR* IIMidiHdr) = 0;
KPrepLongMsg = (void*)GetProcAddress(GetModuleHandle("OmniMIDI"), "PrepareLongData");
```
<hr />

### **UnprepareLongData**
Allows you to unprepare the MIDIHDR buffer, to make it writable again.<br />
It is **MANDATORY** to unprepare a MIDIHDR before editing it, since PrepareLongData locks its data.<br />
The available arguments are:

- `MIDIHDR* IIMidiHdr`: The pointer to the MIDIHDR.
```c
MMRESULT(WINAPI*KUnprepLongMsg)(MIDIHDR* IIMidiHdr) = 0;
KUnprepLongMsg = (void*)GetProcAddress(GetModuleHandle("OmniMIDI"), "UnprepareLongData");
```