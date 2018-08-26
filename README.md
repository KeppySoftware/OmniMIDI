<p align="center">
  <img src="https://i.imgur.com/noVfUG6.png">
  <br />
  A fork of the original <a href="https://github.com/kode54/BASSMIDI-Driver">BASSMIDI Driver by Kode54</a>, with more features.
</p>

## Features compared to other software synthesizers
Click here: [Features compared to other software synthesizers](#features-compared-to-other-software-synthesizers)

## Keppy's Software Updates Server on Discord
I also post my updates on Discord, to keep everyone up-to-date 24/7!
Join now here: https://discord.gg/jUaHPrP

## Keppy's Direct MIDI API for developers
You can access the Keppy's Direct MIDI API from here: [Keppy's Direct MIDI API Documentation](KDMAPI.md)

## Reviews
### [Gingeas](https://www.youtube.com/user/gingeas), famous YouTuber who uploads Black MIDI videos:
_"Extremely high utility driver.<br />
I believe that you can make good audio with any driver (even TiMidity and BASSMIDI), but the driver changes the effort needed.<br />
I could spend 30 min - 2 hours making perfect audio in BASSMIDI that can be done with OmniMIDI in maybe 5 - 20 minutes due to its streamlined efficiency as well as its tools.<br />
Even though OmniMIDI by far has the highest performance out of all drivers I've used, the big selling point to the synth is the utility from its tools, such as the debug window and the SoundFont manager system."_

### [Frozen Snow](https://www.youtube.com/user/WeimTime007), Professional Composer and Arranger for animation, games and video:
_"A one of a kind workflow enhancing, lightweight and feature packed synthesizer that helps me focus on creativity.<br />
<br />
On the surface, this synthesizer is a fantastic performer for beginners and the more demanding users. It allows for fast Black MIDI and Transcription playback with High Quality Soundfonts. However, delving deeper, as a content creator, who works with MIDI data a lot, on a deadline, you'll find that this is more than your average synth. Allow me to explain why.<br />
<br />
Having something that can help me focus on creating without having to switch outputs constantly is really important. especially when you work with multiple instances of various DAWs and sometimes a MIDI player all at the same time.
My workflow usually consists of routing all my MIDI channels to plugin VSTs; for handling MIDI events like mod wheel etc better. Having big sample libraries and VSTs can sometimes take a big chunk out of my time, which slows me down. Having a sound ready to go from that MIDI channel, on a per app basis, with different soundfonts to suit my particular need for that program, may it be Black MIDI, Orchestrations or other, with the efficiency and quality that this synth has, is crucial.<br />
<br />
The aforementioned alone, I'd pay gold for. But to top it all off, there's some awesome additional features such as output to WAV, selecting output devices like the ASIO of my audio interface and changing the voice count on the fly directly within its settings, while not requiring an app to close, nor a restart is amazing. The low latency that gets introduced on some VSTs, is nonexistent between not one but all my programs and my devices.<br />
<br />
Take all of that, and you get a synthesizer that's a powerhouse for creative freedom allowing me to get my work out to my clients without the hustle of feeling burdened. It's the fastest performing and most feature packed of them all. In fact, I can't get it out of my current workflow at this point anymore, it's a must have for any creator who requires a tool that can aid in compositions, while also packing a bucket load of extra stuff.  Whenever it's simple MIDI playback or full-fledged composing, this synth pulls it off."_

## What's so special about your driver that makes it different from the others out there?
This driver has unique features, such as:
- Automatic rendering recovery. The driver will **always** try to give you the best audio quality, no matter what MIDI you're trying to play.
- Spartan user interface, no *"fancy graphics"* which can distract the user from the original purpose of the driver, and designed for people who aims for *features* more than for *style*.
- The ability to use up to **4 cores/threads**, to ensure each function is executed at its best. Each core hosts a vital part of the driver: The first thread hosts the settings loader, the debug info writer etcetera, the second hosts the MIDI event parser, the third hosts the audio render and the fourth hosts the ASIO driver (When using the ASIO angine).
- Constant updates, to keep the driver fresh and always up-to-date to users requests.

It's meant for [professional people](#what-do-you-mean-by-for-professional-use) who wants a lot of settings to change almost every behaviour of the program.

## What do you mean by "for professional use"?
This driver is not meant to be used by beginners. You mess up a setting, and the entire driver might go nuts at you.
<br />
If you're not an advanced user, you could have troubles on getting it working again.
<br />
My advice for such people is to download CoolSoft VirtualMIDISynth driver from here: [Click me](http://coolsoft.altervista.org/en/virtualmidisynth)
<br />
It's free, easy-to-use for beginners, and it usually doesn't get as many updates as this synthesizer.

# Requirements for the driver
## Required software
You need this software installed before attempting to run the setup:
- [Microsoft .NET Framework 4.0](https://www.microsoft.com/en-US/download/details.aspx?id=17718)

## Minimum system requirements for standard MIDIs playback
The minimum requirements for this synthesizer to work are the following:
- A CPU running at 2.0GHz (With SSE instruction set support)
- 512MB of RAM
- Windows Vista SP2 or greater (Server versions are supported too)

## Recommended system requirements for studio environments
For the best experience, it's recommended to run the synthesizer on a PC with the following specifications:
- A octa-core CPU running at 3GHz (Ryzen is recommended)
- 32GB of RAM
- Creative SoundBlaster ZxR, Realtek ALC889A (or better), or a dedicated ASIO hardware interface
- Windows 10 Pro for Workstations
- Alternative MIDI Mapper _(Included in the driver's configurator, for easy installation)_

## Recommended system requirements for smooth Black MIDIs playback
For the best experience, it's recommended to run the synthesizer on a PC with the following specifications:
- A quad-core CPU running at 2.4GHz (x86_64 compliant)
- 4GB of RAM
- Windows 10 Home/Pro
- MIDI player with WinMM patch _(The patch is included in the driver's configurator, for easy installation)_

## Requirements for compiling the source code
To compile (and test) the synthesizer, you need:
- Microsoft Visual Studio 2017 (or newer)
- Inno Setup 5.5.6 (It's recommended to install Inno Script Studio and the Inno Setup Pack)
- Microsoft DirectX SDK
- Microsoft Windows 10 SDK (17133.1)
## How can I get rid of the annoying SmartScreen block screen and stop Chrome from warning me not to download your driver?
You can install my self-signed certificate by using the two files here: https://github.com/KaleidonKep99/OmniMIDI/tree/master/InstallCert<br />
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
But anyway, here is the version without the KSDirect API. [Click me for the direct download](https://github.com/KeppySoftware/OmniMIDI/raw/master/OmniMIDIConfigurator/OmniMIDIConfigurator/Resources/VanillaWinMM.zip)

# ASIO support details
## Supported ASIO devices
- ASIO4ALL *(No issues.)*
- FL Studio ASIO *(No issues, but you can NOT change the buffer size without restarting the stream.)*
- FlexASIO *(Only one issue, the pitch of the output isn't right.)*
- JACK ASIO Driver *(No issues.)*
- Native Instruments Komplete Audio 6 *(No issues.)*
- USB Audio ASIO Driver *(No issues.)*
- Voicemeeter Virtual ASIO *(Will throw a BASS_ERROR_NOTAVAIL exception but still work)*
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

**WARNING**: Since I can not test all the ASIO devices available on the market (Mainly because they're not cheap), if you have one, please... Test it with OmniMIDI, then send me an e-mail about it to [kaleidonkep99@outlook.com](mailto:kaleidonkep99@outlook.com).

# Features compared to other software synthesizers
:pencil: | OmniMIDI | VirtualMIDISynth 1.x | VirtualMIDISynth 2.x | BASSMIDI Driver
------------ | ------------- | ------------- | ------------- | -------------
Easy-to-use configurator | ✔️ | ✔️ | ✔️ | ❌
Smaller memory footprint | ✔️ | ❌ | ❌ | ❌
Support for Windows XP | ❌ | ✔️ | ✔️ | ✔️
Suitable for day-to-day<br />music playback | ✔️<span>*</span> | ✔️ | ✔️ | ✔️
Suitable for professional<br />music production | ✔️ | ❌ | ❌ | ❌
Mixer for easy volume<br />changes per MIDI channel | ✔️ | ✔️ | ✔️ | ❌
Ability to change <br />channel instruments | ✔️ | ❌ | ❌ | ❌
Real-time debug information<br />about the audio stream | ✔️ | ❌ | ✔️ | ❌
Only one MIDI out port<br />with multiple SoundFont lists | ✔️ | ❌ | ❌ | ❌
User has full<br />control over the audio<br />stream | ✔️ | ❌ | ❌ | ❌
Updatable without<br />admin permissions | ✔️ | ❌ | ❌ | ❌
Uses a separate process<br />for audio rendering | ❌ | ❌ | ✔️ | ❌
Able to achieve extremely<br />low latencies | ✔️ | ❌ | ❌ | ⚠️
Wide choice of audio engines,<br />from DirectSound to WASAPI | ✔️ | ❌ | ❌ | ⚠️

<p align="center">
  ✔️: Supported<br />
  ⚠️: Partially supported, or implemented in a different way<br />
  ❌: Unsupported<br />
  <sub>* Might require some advanced computer knowledge</sub>
</p>

<p align="center">
  <a href="http://www.softpedia.com/get/Multimedia/Audio/Audio-Mixers-Synthesizers/Keppys-Synthesizer.shtml#status"><img src="http://s1.softpedia-static.com/_img/sp100free.png?1" /></a>
</p>

# Credits
BASSMIDI driver by Kode54 and mudlord: https://github.com/kode54/BASSMIDI-Driver
<br />
HtmlAgilityPack by Simon Mourier: https://www.nuget.org/packages/HtmlAgilityPack/
<br />
BASS libraries by Un4seen (Ian Luck): http://www.un4seen.com/
<br />
BASS.NET wrapper by radio42: http://bass.radio42.com/
<br />
Costura.Fody by Simon Cropp: https://github.com/Fody
<br />
Discord RPC wrapper by nostrenz: https://github.com/nostrenz
