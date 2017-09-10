<p align="center">
  <img src="http://i.imgur.com/gZwDTi0.png">
  <br />
  A fork of the original <a href="https://github.com/kode54/BASSMIDI-Driver">BASSMIDI Driver by Kode54</a>, with new functions.
</p>

## Features compared to other MIDI synthesizers
Click here: [Features compared to other MIDI drivers](#features-compared-to-other-midi-drivers)

## Keppy's Updates Server on Discord
I also post my updates on Discord, to keep everyone up-to-date 24/7!
Join now here: https://discord.gg/jUaHPrP

## Reviews
### [Gingeas](https://www.youtube.com/user/gingeas), famous YouTuber who uploads Black MIDI videos:
_"Extremely high utility driver.<br />
I believe that you can make good audio with any driver (even TiMidity and BASSMIDI), but the driver changes the effort needed.<br />
I could spend 30 min - 2 hours making perfect audio in BASSMIDI that can be done with Keppy's Synthesizer in maybe 5 - 20 minutes due to its streamlined efficiency as well as its tools.<br />
Even though Keppy's Synthesizer by far has the highest performance out of all drivers I've used, the big selling point to the synth is the utility from its tools, such as the debug window and the SoundFont manager system."_

## What's so special about your driver that makes it different from the others out there?
This driver has unique features, such as:
- Automatic rendering recovery. The driver will **always** try to give you the best audio quality, no matter what MIDI you're trying to play.
- Spartan user interface, no *"fancy graphics"* which can distract the user from the original purpose of the driver, and designed for people who aims for *features* more than for *style*.
- The ability to use up to **4 cores/threads**, to ensure each function is executed at its best. Each core hosts a vital part of the driver: The first thread hosts the settings loader, the debug info writer etcetera, the second hosts the MIDI event parser, the third hosts the audio render and the fourth hosts the ASIO driver (When using the ASIO angine).
- Constant updates, to keep the driver fresh and always up-to-date to users requests.

It's meant for [professional people](#what-do-you-mean-by-for-professional-use) who wants a lot of settings to change almost every behaviour of the program.

## What do you mean by "for professional use"?
Certain functions in this driver are not suited for newbies or for people that don't know how to use programs well.
<br />
Changing one single driver function could break the entire audio stream, and if you're not an advanced user, you could have troubles on getting it working again.
<br />
My advice for such people is to download CoolSoft VirtualMIDISynth driver from there: [Click me](http://coolsoft.altervista.org/en/virtualmidisynth)
<br />
It's free, easy-to-use for newbies, and doesn't get updated every 1-2 nanoseconds. (Unlike mine)

## How can I get rid of the annoying SmartScreen block screen and stop Chrome from warning me not to download your driver?
You can install my self-signed certificate by using the two files here: https://github.com/KaleidonKep99/Keppy-s-Synthesizer/tree/master/installcert<br />
I made the sign myself, no one has it except me. So you can trust add it to your trusted certificates by using the small installcert.exe utility.<br />
*(Be sure that KeppyCert.cer is in the same folder as InstallCert.exe)*

## Can I use your program's source code for my program?
Sure you can, but there are a few "rules" you need to follow.

What you can do:
- Take parts of the code, and use it on your apps. (As long as you credit me, BASS.NET and Un4seen.)
- Share the code on websites outside of the GitHub world. (Again, same as before.)
- Create ports of the driver for other operating systems, such as Linux, Mac OS X, Amiga etc... Any other O.S. other than Windows. (See down below for further explanations.)

What you can't do:
- Clone the source code of the driver, and change its name to "(Your name)'s Synthesizer", without actually doing any change to its source code. I mean, why would you do that?
- Create ports of the driver in different programming languages, but with Windows support. There's already a Windows version, which is this one.

## I'm here just for the WinMM patch. How can I get it without downloading your driver?
I'm sad that you don't want to download my driver...
<br />
But anyway, here it is. [Click me for the direct download](https://github.com/KaleidonKep99/Keppy-s-Synthesizer/raw/master/KeppySynthConfigurator/KeppySynthConfigurator/Resources/WinMMPatch.zip)


# Requirements for the driver
## Required software
You need this software installed before attempting to run the setup:
- [Microsoft Visual C++ 2010](https://www.microsoft.com/en-us/download/details.aspx?id=5555) (Install the 64-bit version too if you're using a 64-bit O.S.. Mandatory for BASS_VST to work.)
- [Microsoft .NET Framework 4.0](https://www.microsoft.com/en-US/download/details.aspx?id=17718)
- [Windows XP: Windows Imaging Component](https://www.microsoft.com/en-us/download/details.aspx?id=32) (Install the 64-bit version too if you're using a 64-bit O.S.)

The DirectX Web Installer is already included with the driver setup, so there's no need to install it separately.

## Minimum system requirements for standard MIDIs playback
To use the driver, you need at least:
- A dual-core CPU (Hyper-Threading) running at 1.0GHz (With MMX instruction set support)
- 256MB of RAM
- Windows Vista SP1 or greater (Server versions are supported too)

## Recommended system requirements for smooth Black MIDIs playback
To use the driver, you need at least:
- A quad-core CPU running at 2.4GHz (x86_64 compliant)
- 4GB of RAM
- Windows 10 with WinMM patch (Included in the driver's configurator, for easy installation)

## Minimum system requirements for driver compiling
To compile (and test) the driver, you need:
- Microsoft Visual Studio 2017
- Inno Setup 5.5.6 (It's recommended to install Inno Script Studio and the Inno Setup Pack)
- A dual-core CPU (Hyper-Threading) running at 1.0GHz (With SSE2 and CMPXCHG16b instructions support, for Microsoft Visual Studio 2017)
- 768MB of RAM (for both soundfonts and Microsoft Visual Studio 2017)
- Windows 7 SP1 or greater (Otherwise, no VS2017 for you)


# ASIO support details
## Supported ASIO devices
- ASIO4ALL *(Almost everything works fine, but closing the MIDI app might get it stuck. Task Manager is required to terminate it.)*
- FL Studio ASIO *(Almost everything works fine, but changing the buffer size from its control panel leads to a corrupted audio output. Restarting the app fixes the issue.)*
- USB Audio ASIO Driver *(Works fine, but has more latency than local software ASIO drivers.)*
- FlexASIO *(Almost everything works fine, except the pitch of the output isn't right.)*
- Voicemeeter Virtual ASIO *(Will throw a BASS_ERROR_NOTAVAIL exception but still work)*
- JACK ASIO Driver
## Unsupported ASIO devices
- ASIO2WASAPI *(Throws a BASS_ERROR_UNKNOWN exception)*
- Realtek ASIO *(Throws a BASS_ERROR_UNKNOWN exception)*
## Untested ASIO devices
- ASIO Digidesign Driver
- ASIO Digidesign Driver Mbox2
- Avid Mbox ASIO
- Avid Mbox Mini ASIO
- Avid Mbox Pro ASIO
- Digital Design Dance Rack ASIO

**WARNING**: Since I can not test all the ASIO devices available on the market (Mainly because they're not cheap), if you have one, please... Test it with Keppy's Synthesizer, then send me an e-mail about it to [kaleidonkep99@outlook.com](mailto:kaleidonkep99@outlook.com).

# Features compared to other MIDI drivers
<p align="center">

📋 | Keppy's Synthesizer | VirtualMIDISynth 1.x | VirtualMIDISynth 2.x | BASSMIDI Driver
------------ | ------------- | ------------- | ------------- | -------------
Easy-to-use configurator | ✔️ | ✔️ | ✔️ | ❌
Smaller memory footprint | ✔️ | ❌ | ✔️ | ❌
Support for Windows XP | ❌ | ✔️ | ✔️ | ✔️
Mixer for easy volume<br />changes per MIDI channel | ✔️ | ✔️ | ✔️ | ❌
Real-time debug information<br>about the audio stream | ✔️ | ❌ | ✔️ | ❌
Only one MIDI out port<br />with multiple SoundFont lists | ✔️ | ❌ | ❌ | ❌
User has full<br />control over the audio<br />stream | ✔️ | ❌ | ❌ | ❌
Updatable without<br />admin permissions | ✔️ | ❌ | ❌ | ❌
Uses a separate process<br />for audio rendering | ❌ | ❌ | ✔️ | ❌
Able to achieve extremely<br />low latencies | ✔️ | ❌ | ❌ | ⚠️
Wide choice of audio engines,<br />from DirectSound to WASAPI | ✔️ | ❌ | ❌ | ⚠️
Suitable for day-to-day<br />music playback | ✔️<span>*</span> | ✔️ | ✔️ | ✔️
Suitable for professional<br />music production | ✔️ | ❌ | ❌ | ❌

</p>

✔️: Supported<br />
⚠️: Partially supported, or implemented in a different way<br />
❌: Unsupported
###### * Might require some advanced computer knowledge

<p align="center">
  <a href="http://www.softpedia.com/get/Multimedia/Audio/Audio-Mixers-Synthesizers/Keppys-Synthesizer.shtml#status"><img src="http://s1.softpedia-static.com/_img/sp100free.png?1" /></a>
</p>
