# Keppy's Driver
A fork of the original [BASSMIDI Driver by Kode54] (https://github.com/kode54/BASSMIDI-Driver), with new functions.

## What's the difference between the original driver and your fork?
I optimized this fork by doing these things:
- Blacklist in the driver, to prevent strange processes from blocking it
- Modifiable audio frequency
- Modifiable samples per frame value
- Note off event and sound effects disabler settings
- Better UI for the configurator

## Minimum system requirements for standard MIDIs playback
To use the driver, you need at least:
- A CPU running at 1.2GHz (No special CPU instructions are needed, just plain IA32)
- 768MB of RAM
- [Microsoft Visual C++ 2013] (https://www.microsoft.com/en-us/download/details.aspx?id=40784) & .NET Framework 2.0
- [DirectX 9.0c with XAudio 2.x] (http://www.microsoft.com/en-us/download/details.aspx?id=35) (Integrated in the installer)
- Windows Vista SP2 or greater

## Recommended system requirements for smooth Black MIDIs playback
To use the driver, you need at least:
- A quad-core CPU running at 3.0GHz (x86_64 compliant)
- 4096MB of RAM
- [Microsoft Visual C++ 2013] (https://www.microsoft.com/en-us/download/details.aspx?id=40784) & .NET Framework 2.0
- [DirectX 9.0c with XAudio 2.x] (http://www.microsoft.com/en-us/download/details.aspx?id=35) (Integrated in the installer)
- Windows 7 SP1 or greater

## Minimum system requirements for driver compiling
To compile (and test) the driver, you need:
- Microsoft Visual Studio 2013
- Inno Setup 5.5.6 (It's recommended to install Inno Setup Studio and the Inno Setup Pack)
- A CPU that supports CMPXCHG16b and SSE2, running at 1033MHz (For Microsoft Visual Studio 2013)
- 768MB of RAM (for both soundfonts and Microsoft Visual Studio 2013)
- Windows 7 SP1 or greater (Otherwise, no VS2013 for you)

## Are you trying to overshadow the original project?
Absolutely no, Kode54's driver is better than mine tho. (His code is more cleaner, and way more stable)

You can download it from there here: https://github.com/kode54/BASSMIDI-Driver

## About the Windows 10 support
INFO: Windows 10 will NOT be supported until Microsoft doesn't fix the problem with the MIDI-Mapper.
There's already a patch for Windows 8 and 8.1, so I'll give support for them.

And for support, I mean bug fixes and other things like that.
The driver itself works on Windows 10, but it's really glitchy.