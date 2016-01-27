# Keppy's Driver
A fork of the original [BASSMIDI Driver by Kode54] (https://github.com/kode54/BASSMIDI-Driver), with new functions.

## What's the difference between the original driver and your fork?
I optimized this fork by doing these things:
- Blacklist in the driver, to prevent strange processes from blocking it
- Modifiable audio frequency
- Modifiable samples per frame value
- Note off event and sound effects disabler settings
- Better UI for the configurator

## System requirements
To use the driver, you need at least:
- A CPU running at 900MHz (No special CPU instructions are needed, just plain IA32)
- 512MB of RAM
- [Microsoft Visual C++ 2013] (https://www.microsoft.com/en-us/download/details.aspx?id=40784) & .NET Framework 2.0
- [DirectX 9.0c with XAudio 2.x] (http://www.microsoft.com/en-us/download/details.aspx?id=35) (Integrated in the installer)
- Windows XP SP3 (SP2 for the 64-bit edition) or greater

INFO: Windows 10 will NOT be supported until Microsoft doesn't fix the problem with the MIDI-Mapper.
There's already a patch for Windows 8 and 8.1, so I'll give support for them.

To compile (and test) the driver, you need:
- Microsoft Visual Studio 2013
- Inno Setup 5.5.6 (It's recommended to install Inno Setup Studio and the Inno Setup Pack)
- A CPU that supports CMPXCHG16b and SSE2, running at 1033MHz (For Microsoft Visual Studio 2013)
- 768MB of RAM (for both soundfonts and Microsoft Visual Studio 2013)
- Windows 7 SP1 or greater (Otherwise, no VS2013 for you)

## Are you trying to overshadow the original project?
Absolutely no, Kode54's driver is better than mine tho. (His code is more cleaner, and way more stable)

You can download it from there here: https://github.com/kode54/BASSMIDI-Driver
