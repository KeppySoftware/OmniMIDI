# KSDirect API (KSDAPI) documentation
## What's KSDAPI?
KSDAPI is a group of calls, that allows you to bypass the Windows Multimedia API, thus getting rid of the lag caused by its slow buffer system.

## How can I implement it in my program?
It's quite easy actually.

Here's a generic example by [Sono (MarcuzD)](https://github.com/MarcuzD), on how to make use of the KSDirect API.
It works just like WinMM would, we'll see later the differences between using WinMM as usual, and using WinMM together with KSDirect:
```c
...
#define ALLOK 0;
#define NOTOK 1;

MMRESULT(_stdcall*mmOutOpen)(LPHMIDIOUT, lphmo, UINT uDeviceID, DWORD_PTR dwCallback, DWORD_PTR dwCallbackInstance, DWORD dwFlags) = 0;
MMRESULT(_stdcall*mmOutClose)(HMIDIOUT hmo) = 0;
MMRESULT(_stdcall*mmOutShortMsg)(HMIDIOUT hmo, DWORD dwMsg) = 0;
UINT(_stdcall*mmOutGetErrorTextA)(MMRESULT mmrError, LPTSTR, lpText, UINT cchText) = 0;

MMRESULT(*KShortMsg)(DWORD msg) = 0;
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
MMRESULT _stdcall _KOutShortMsg(HMIDIOUT hmo, DWORD msg) {
    // Pass the MIDI event to the KSDirect call, and return the WinMM result
    return KShortMsg(msg);
}
```

As you can see, we're basically replacing the loaded **midiOutShortMsg** call with our own call, **_KOutShortMsg**, which redirects the messages from WinMM directly to (*lol*) the KSDirect API calls.

## Is it mandatory for me to implement these features, to use Keppy's Synthesizer?
Of course not! These calls are a thing for people who care about low latency and performance.
The driver will work fine with the default WinMM => modMessage system too.
It'll be slower when playing Black MIDIs, and the latency will also be higher, but it'll work just fine.

## What functions are available?
As of April 7th 2018, these are the functions available in the KSDirect API.
The **"NoBuf"** calls bypass the built-in buffer in Keppy's Synthesizer, and directly send the events to the events processing system.
### **SendDirectData/SendDirectDataNoBuf**
Allows you to send MIDI events to the driver. The available arguments are:

- *DWORD dwMsg*: The MIDI event to send to the driver.
```c
MMRESULT(*KShortMsg)(DWORD msg) = 0;
KShortMsg = (void*)GetProcAddress(GetModuleHandle("keppysynth"), "SendDirectData"); // Or SendDirectDataNoBuf
```
### **SendDirectLongData/SendDirectLongDataNoBuf**
Allows you to send SysEx events to the driver. The available arguments are:

- *LPMIDIHDR lpMidiOutHdr*: The MIDIHDR to send to the driver.
```c
MMRESULT(*KLongMsg)(LPMIDIHDR lpMidiOutHdr) = 0;
KLongMsg = (void*)GetProcAddress(GetModuleHandle("keppysynth"), "SendDirectLongData"); // Or SendDirectLongDataNoBuf
```