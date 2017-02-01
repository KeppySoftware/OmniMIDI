# Keppy's Synthesizer: A MIDI driver for professional use
<p align="center">
  <a href="http://www.softpedia.com/get/Multimedia/Audio/Audio-Mixers-Synthesizers/Keppys-Synthesizer.shtml#status"><img src="http://s1.softpedia-static.com/_img/sp100clean.png" /></a>
  <br>
  A fork of the original <a href="https://github.com/kode54/BASSMIDI-Driver">BASSMIDI Driver by Kode54</a>, with new functions.
</p>

## What's so special about your driver that makes it different from the others out there?
This driver has unique features, such as:
- Automatic rendering recovery. The driver will **always** try to give you the best audio quality, no matter what MIDI you're trying to play.
- Spartan user interface, no "fancy graphics" which can distract the user from the original purpose of the driver, and designed for people who aims for features more than for style.
- The ability to use up to 3 cores/threads, to ensure each function is executed at its best. Each core hosts a vital part of the driver: The first thread hosts the settings loader, the debug info writer etcetera, the second hosts the MIDI event parser, and the third hosts the audio render.
- [Constant updates] (#why-do-i-get-updates-every-1-2-days), to keep the driver fresh and always up-to-date to users requests.

It's meant for [professional people] (#what-do-you-mean-by-for-professional-use) who wants a lot of settings to change almost every behaviour of the program.

## What do you mean by "for professional use"?
Certain functions in this driver are not suited for newbies or for people that don't know how to use programs well.
<br>
Changing one single driver function could break the entire audio stream, and if you're not an advanced user, you could have troubles on getting it working again.
<br>
My advice for such people is to download CoolSoft VirtualMIDISynth driver from there: [Click me] (http://coolsoft.altervista.org/en/virtualmidisynth)
<br>
It's free, easy-to-use for newbies, and doesn't get updated every 1-2 nanoseconds. (Unlike mine)

## Why do I get updates every 1-2 days???
As I mentioned before, I always try to keep my driver fresh and up-to-date to today's users standards.
If you're not okay with this (again), you can always switch to [CoolSoft VirtualMIDISynth] (http://coolsoft.altervista.org/en/virtualmidisynth).

## Can I use your program's source code for my program?
Sure you can, but there are a few "rules" you need to follow.

What you can do:
- Take parts of the code, and use it on your apps. (As long as you credit me, BASS.NET and Un4seen.)
- Share the code on websites outside of the GitHub world. (Again, same as before.)
- Create ports of the driver for other operating systems, such as Linux, Mac OS X, Amiga etc... Any other O.S. other than Windows. (See down below for further explanations.)

What you can't do:
- Clone the source code of the driver, and change its name to "(Your name)'s Synthesizer", without actually doing any change to its source code. I mean, why would you do that?
- Create ports of the driver in different programming languages, but with Windows support. There's already a Windows version, which is this one.

## Required software
You need this software installed before attempting to run the setup:
- [Microsoft Visual C++ 2010] (https://www.microsoft.com/en-us/download/details.aspx?id=5555) and [Microsoft Visual C++ 2013] (https://www.microsoft.com/en-us/download/details.aspx?id=40784) (Install the 64-bit version too if you're using a 64-bit O.S.)
- [Microsoft .NET Framework 4.0] (https://www.microsoft.com/en-US/download/details.aspx?id=17718)
- [Windows XP: Windows Imaging Component] (https://www.microsoft.com/en-us/download/details.aspx?id=32) (Install the 64-bit version too if you're using a 64-bit O.S.)

The DirectX Web Installer is already included with the driver setup, so there's no need to install it separately.

## Minimum system requirements for standard MIDIs playback
To use the driver, you need at least:
- A dual-core CPU (Hyper-Threading) running at 1.0GHz (With MMX instruction set support)
- 256MB of RAM
- Windows Vista SP1 or greater (Server versions are supported too)

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
Yes it does.

## Are you trying to overshadow the original project?
Absolutely not, Kode54's driver is better than mine. (His code is cleaner, and way more stable)

You can download it from here: https://github.com/kode54/BASSMIDI-Driver
