<p align="center">
  <img src="https://i.imgur.com/noVfUG6.png">
  <br />
  A reboot of the original <a href="https://github.com/kode54/BASSMIDI-Driver">BASSMIDI Driver by Kode54</a>, with more features.
</p>

## F.A.Q.

### Was it really necessary to create a complete separate fork of BASSMIDI Driver?
I feel like it was necessary, yes.

### Couldn't you just edit the driver on the existing repository?
True that, I could've just done that. But I honestly didn't want to ruin the original driver.<br />
The driver was born back in 2015, when a friend of mine wanted a version of BASSMIDI Driver with higher polyphony, but then I started working on it more and more, to the point where most of the original source code got replaced by mine.<br />
I really didn't want to ruin kode54's original source code, so I decided to create my own repository. (While still giving credits to kode54, of course.)<br />
Oh, and of course, the driver wouldn't be where it is now, without kode54's help from behind the scenes. He helped me a lot with some issues I was having with some parts of his code. (Which I eventually replaced, but still.)<br />

### Do you feel like your driver is complete now?
Tough question... I honestly have no idea. I mean, there's always room for improvement.<br/>
But I feel like I have nothing else to add to it at this point, I'm literally out of ideas.<br/>
If you're a programmer, and you have some ideas on how to improve or expand the driver's functionalities, please hit me up or send a pull requests with the edits.

### Didn't you stop updating it?
Yes, but I still do small updates from time to time when needed, and I also do updates on request.<br/>
I've received numerous donations from people that don't want the driver to be abandoned, and I'm really thankful to all of them for their support!

### Ok ok, enough of your story... What's so special about your driver that makes it different from the others out there?
Good question. The driver has unique features, such as:
- Automatic rendering recovery. The driver will **always** try to give you the best audio quality, no matter what MIDI you're trying to play.
- Spartan user interface, no *"fancy graphics"* which can distract the user from the original purpose of the driver, and designed for people who aims for *features* more than for *style*.
- The ability to use up to **4 cores/threads**, to ensure each function is executed at its best. Each core hosts a vital part of the driver: The first thread hosts the settings loader, the debug info writer etcetera, the second hosts the MIDI event parser, the third hosts the audio render and the fourth hosts the ASIO driver (When using the ASIO engine).
- Constant updates, to keep the driver fresh and always up-to-date to users requests.

It's meant for [professional people](#what-do-you-mean-by-for-professional-use) who wants a lot of settings to change almost every behaviour of the program.

### What do you mean by "for professional use"?
I'll be honest, when I programmed the interface of the driver, I made it to make it familiar for DAW experts or people who know how to use advanced programs.<br />
I've seen newbies getting angry at me after changing one settings, complaining that the driver kept crashing their apps while they were playing MIDIs/working on projects.<br /><br />
If you want something easy to use, I strongly recommend [VirtualMIDISynth 2.x](http://coolsoft.altervista.org/en/virtualmidisynth) by Claudio Nicora.<br />
His driver is definitely more stable than mine, and it's easier to use too. Go check it out.

### Can I use your program's source code for my program?
Sure, as long as you follow the [LICENSE](LICENSE.txt).

### Keppy's Direct MIDI API for developers
You can access the Keppy's Direct MIDI API from here: [Keppy's Direct MIDI API Documentation](https://github.com/KeppySoftware/OmniMIDI/tree/master/DeveloperContent/KDMAPI.md)<br/>
You can also access the source code for the Windows Multimedia Wrapper here: [WinMMWRP on GitHub](https://github.com/KeppySoftware/WinMMWRP)<br/>
Python bindings are available as well, get them from PyPI: [kdmapi](https://pypi.org/project/kdmapi/) (maintained by [SebaUbuntu](https://github.com/SebaUbuntu), source code [here](https://github.com/SebaUbuntu/kdmapi))

Here's a list of applications that currently have *native* support for the Keppy's Direct MIDI API:
- mmidi by Sono, the first third-party project to feature my API at all: _N/A_
- Chikara by Kaydax, a PFA clone that uses Vulkan, and aims to be the best performing MIDI player available: https://github.com/Kaydax/Chikara
- Kiva by Arduano, a multipurpose MIDI player with different graphic styles: https://github.com/arduano/Kiva
- Zenith by Arduano, a multipurpose MIDI render with different graphic styles: https://arduano.github.io/Zenith-MIDI/
- giradischi by SebaUbuntu, a simple Python + Qt6 MIDI player supporting multiple APIs, KDMAPI being one of them: https://github.com/SebaUbuntu/giradischi

### Can you make a WinMM patch for other drivers too?
There's a patch available for VirtualMIDISynth. You can get it here: https://github.com/KeppySoftware/WinMMWRP/releases/tag/4.2A

### Minimum system requirements for MIDI playback on x86/x64 systems
The minimum requirements for this synthesizer to work are the following:
- A SSE2-capable x86 CPU running at 1.5GHz
- 1024MB of RAM
- DirectX 9 capable sound card or better
- Windows Vista SP2 or greater *(Server versions are supported too)*

### Minimum system requirements for MIDI playback on ARM64 systems
The minimum requirements for this synthesizer to work are the following:
- Qualcomm® Snapdragon™ 835, or any ARM® Cortex-A57 based chip running at 2GHz or more
- 1536MB of RAM *(Required by Windows)*
- Any sound device supported by Windows 10 ARM64 *(Qualcomm® Aqstic™ or aptX™ DACs are recommended)*
- Windows 10 Spring Creators Update 2018

### Recommended system requirements for studio environments
For the best experience, it's recommended to run the synthesizer on a PC with the following specifications:
- AMD Ryzen 9 5900X
- 32GB of RAM *(3600MHz)*
- Realtek ALC1220 with ASIO4ALL or Native Instruments Komplete Audio 6 (or another dedicated ASIO-capable hardware interface)
- Windows 10 Pro 21H2
- OmniMapper and Windows Multimedia Wrapper for DAWs _(Both included in the driver's configurator, for easy installation)_

### Requirements for compiling the source code
To compile (and test) the synthesizer, you need:
- Microsoft Visual Studio 2022
- Inno Setup 6.1+ (It's recommended to install Inno Script Studio and the Inno Setup Pack)
- Inno Downloader Plugin
- Microsoft Windows SDK 10.0.22000

## ASIO support details
You can read the lists here: [OmniMIDIASIOSupportList folder on GitHub](https://github.com/KeppySoftware/OmniMIDI/tree/master/OmniMIDIASIOSupportList)
<br />
**WARNING**: Since I can not test all the ASIO devices available on the market (Mainly because they're not cheap), if you have one, please... Test it with OmniMIDI, then send me an e-mail about it to [kaleidonkep99@outlook.com](mailto:kaleidonkep99@outlook.com).

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
Octokit by GitHub Inc.: https://developer.github.com/v3/libraries/
