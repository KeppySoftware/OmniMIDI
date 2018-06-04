# KSDirect API (KSDAPI) documentation
## What's KSDAPI?
KSDAPI is a group of calls, that allows you to bypass the Windows Multimedia API, thus getting rid of the lag caused by its slow buffer system.

## How can I implement it in my program?
It's quite easy actually.

Here's a generic example by [Sono (MarcuzD)](https://github.com/MarcuzD), on how to make use of the KSDirect API.<br />
It works just like WinMM would, we'll see later the differences between using WinMM as usual, and using WinMM together with KSDirect:
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

// Check if Keppy's Synthesizer is loaded into memory, and initialize the KSDirect calls
KShortMsg = (void*)GetProcAddress(GetModuleHandle("keppysynth"), "SendDirectData");
if (KShortMsg) {
   // Replace default WinMM function with the one from the application itself
    puts("KSDAPI initialized.");
    mmOutShortMsg = _KOutShortMsg;
}
...
```

And here's the function from the application itself:
```c
MMRESULT WINAPI _KOutShortMsg(HMIDIOUT hmo, DWORD msg) {
    // Pass the MIDI event to the KSDirect call, and return the WinMM result
    return KShortMsg(msg);
}
```

As you can see, we're basically replacing the loaded **midiOutShortMsg** call with our own call, **_KOutShortMsg**, which redirects the messages from WinMM directly to (*lol*) the KSDirect API calls.

## Is it mandatory for me to implement these features, to use Keppy's Synthesizer?<br />
Of course not! These calls are a thing for people who care about low latency and performance.<br />
The driver will work fine with the default WinMM => modMessage system too.<br />
It'll be slower when playing Black MIDIs, and the latency will also be higher, but it'll work just fine.

## What functions are available?
As of May 1st 2018, these are the functions available in the KSDirect API.<br />
The **"NoBuf"** calls bypass the built-in buffer in Keppy's Synthesizer, and directly send the events to the events processing system.<br />
### **InitializeKSStream**<br />
It initializes the driver, its stream and all its required threads. There are no arguments.

```c
void(WINAPI*KSInit)() = 0;
KSInit = (void*)GetProcAddress(GetModuleHandle("keppysynth"), "InitializeKSStream");
```
<hr />

### **TerminateKSStream**
It tells the driver to wrap up its stuff and to leave! There are no arguments.

```c
void(WINAPI*KSStop)() = 0;
KSStop = (void*)GetProcAddress(GetModuleHandle("keppysynth"), "TerminateKSStream");
```
<hr />

### **ResetKSStream**
Resets the MIDI channels. There are no arguments.

```c
void(WINAPI*KSReset)() = 0;
KSReset = (void*)GetProcAddress(GetModuleHandle("keppysynth"), "ResetKSStream");
```
<hr />

### **ReturnKSDAPIVer**
It returns the version of the KSDirect API, as a string. There are no arguments available.
Used by Keppy's Synthesizer Configurator.

```c
char const*(WINAPI*KSDAPIVer)() = 0;
KSDAPIVer = (void*)GetProcAddress(GetModuleHandle("keppysynth"), "ReturnKSDAPIVer");
```
<hr />

### **IsKSDAPIAvailable**
A generic check, useful for people who want to see if KSDAPI v1.2+ is available.<br />
You NEED to call this function at least once, in order to switch the KSDAPI status value in the debug window to active.<br />
There are no arguments available, and you have to manually catch the exception, if the function isn't available.

```c
BOOL(WINAPI*KSDAPIStatus)() = 0;
KSDAPIStatus = (void*)GetProcAddress(GetModuleHandle("keppysynth"), "IsKSDAPIAvailable");
```
<hr />

### **ChangeDriverSettings**
Allows developers to change the driver's settings from within the app, rather than asking the user to change them in the configurator.<br/>
Sending **0/nullptr** will make it fallback to the settings from the registry.<br />
The available arguments are:

- `const Settings* Struct`: A pointer to your struct.
- `DWORD StructSize`: The size of the struct.
```c
VOID(WINAPI*KSChangeSettings)(const Settings* Struct, DWORD StructSize) = 0;
KSChangeSettings = (void*)GetProcAddress(GetModuleHandle("keppysynth"), "ChangeDriverSettings");
...
	Settings MySettings;
	
	// Change your settings
	Settings.MaxVoices = 4;
	Settings.AudioFrequency = 22050;
	Settings.LiveChanges = TRUE;
	
	// Push the settings to the driver
	KSChangeSettings(&MySettings, sizeof(MySettings));
	
	// Now make the driver fallback to the settings from the registry
	KSChangeSettings(0, 0);
...
```
You can get the code for the struct from **"val.h"**: [Click here!](https://github.com/KeppySoftware/Keppy-s-Synthesizer/blob/master/KeppySynth/val.h)
<hr />

### **GetDriverDebugInfo**
Allows developers to get the driver's current rendering time and the voices that are currently active in the audio stream.<br />

```c
DebugInfo*(WINAPI*KSGetDebugInfo)() = 0;
KSGetDebugInfo = (void*)GetProcAddress(GetModuleHandle("keppysynth"), "GetDriverDebugInfo");
...
	DebugInfo* DebugInfoFromDriver;
	DebugInfoFromDriver = KSGetDebugInfo();
	
	//Do something with the info
	printf("Current rendering time: %d\n", DebugInfoFromDriver->RenderingTime); 
...
```
You can get the code for the struct from **"val.h"**: [Click here!](https://github.com/KeppySoftware/Keppy-s-Synthesizer/blob/master/KeppySynth/val.h)
<hr />

### **LoadCustomSoundFontsList**
Allows developers to load their own custom SoundFonts or SoundFonts lists.<br />
The available arguments are:

- `const TCHAR* Directory`: A pointer to the unicode char array, containing the path.
```c
VOID(WINAPI*KSLoadCustomSFList)(const TCHAR* Directory) = 0;
KSLoadCustomSFList = (void*)GetProcAddress(GetModuleHandle("keppysynth"), "LoadCustomSoundFontsList");
...
	TCHAR Directory[MAX_PATH];
	wcscpy_s(Directory, MAX_PATH, _TEXT("C:\\MySF.sf2"));
	
	// Forward it to the driver
	KSLoadCustomSFList(&Directory);
...
```
<hr />

### **SendDirectData/SendDirectDataNoBuf**
Allows you to send MIDI events to the driver.<br />
The available arguments are:

- `DWORD dwMsg`: The MIDI event to send to the driver.
```c
MMRESULT(WINAPI*KShortMsg)(DWORD msg) = 0;
KShortMsg = (void*)GetProcAddress(GetModuleHandle("keppysynth"), "SendDirectData"); // Or SendDirectDataNoBuf
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
KLongMsg = (void*)GetProcAddress(GetModuleHandle("keppysynth"), "SendDirectLongData"); // Or SendDirectLongDataNoBuf
```
<hr />

### **PrepareLongData**
Allows you to prepare the MIDIHDR buffer, before sending it to the driver through SendDirectLongData/SendDirectLongDataNoBuf.<br />
Once a buffer is prepared, it becomes read-only.<br />
The available arguments are:

- `MIDIHDR* IIMidiHdr`: The pointer to the MIDIHDR.
```c
MMRESULT(WINAPI*KPrepLongMsg)(MIDIHDR* IIMidiHdr) = 0;
KPrepLongMsg = (void*)GetProcAddress(GetModuleHandle("keppysynth"), "PrepareLongData");
```
<hr />

### **UnprepareLongData**
Allows you to unprepare the MIDIHDR buffer, to make it writable again.<br />
It is **MANDATORY** to unprepare a MIDIHDR before editing it, since PrepareLongData locks its data.<br />
The available arguments are:

- `MIDIHDR* IIMidiHdr`: The pointer to the MIDIHDR.
```c
MMRESULT(WINAPI*KUnprepLongMsg)(MIDIHDR* IIMidiHdr) = 0;
KUnprepLongMsg = (void*)GetProcAddress(GetModuleHandle("keppysynth"), "UnprepareLongData");
```