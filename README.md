# Keppy's Driver
A fork of the original [BASSMIDI Driver by Kode54] (https://github.com/kode54/BASSMIDI-Driver), with new functions.

## What's the difference between the original driver and your fork?
I optimized this fork by doing these things:
- Blacklist in the driver, to prevent strange processes from blocking it
- Modifiable audio frequency
- Modifiable buffer size
- Note off event and sound effects disabler settings
- Better UI for the configurator

## Required software
You need this software installed before attempting to run the setup:
- [Microsoft Visual C++ 2013] (https://www.microsoft.com/en-us/download/details.aspx?id=40784) (Install the 64-bit version too if you're using a 64-bit O.S.)
- [Microsoft .NET Framework 4.0] (https://www.microsoft.com/en-US/download/details.aspx?id=17718)
- [XP: Windows Imaging Component] (https://www.microsoft.com/en-us/download/details.aspx?id=32) (Install the 64-bit version too if you're using a 64-bit O.S.)

The DirectX Web Installer is already included with the driver setup, so there's no need to install it separately.

## Minimum system requirements for standard MIDIs playback
To use the driver, you need at least:
- A dual-core CPU (Hyper-Threading) running at 1.0GHz (With SSE2 and CMPXCHG16b instructions support)
- 256MB of RAM
- Windows XP SP3 [SP2 for 64-bit] or greater (Server versions are supported too)

## Recommended system requirements for smooth Black MIDIs playback
To use the driver, you need at least:
- A quad-core CPU running at 2.4GHz (x86_64 compliant)
- 4096MB of RAM
- Windows 7 SP1 or greater (Server versions are supported too)

## Minimum system requirements for driver compiling
To compile (and test) the driver, you need:
- Microsoft Visual Studio 2013
- Inno Setup 5.5.6 (It's recommended to install Inno Setup Studio and the Inno Setup Pack)
- A dual-core CPU (Hyper-Threading) running at 1.0GHz (With SSE2 and CMPXCHG16b instructions support, for Microsoft Visual Studio 2013)
- 768MB of RAM (for both soundfonts and Microsoft Visual Studio 2013)
- Windows 7 SP1 or greater (Otherwise, no VS2013 for you)

## Does it support Windows 10?
Yes it does. I'm constantly optimizing the driver for it... I had to upgrade from W7 to W10 to do this.

## Are you trying to overshadow the original project?
Absolutely not, Kode54's driver is better than mine. (His code is cleaner, and way more stable)

You can download it from here: https://github.com/kode54/BASSMIDI-Driver