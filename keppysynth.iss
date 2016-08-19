[Setup]
AllowCancelDuringInstall=False
AppContact=kaleidonkep99@outlook.com
AppCopyright=Copyright (c) 2011-2016 Brad Miller, Chris Moeller and Riccardo Loi. All rights reserved.
AppId={{950DEC78-2D12-4917-BE69-CB04FE84B21F}
AppName=Keppy's Synthesizer
AppPublisher=KaleidonKep99
AppPublisherURL=https://github.com/KaleidonKep99/Keppy-s-Synthesizer
AppSupportPhone=+393511888475
AppSupportURL=https://github.com/KaleidonKep99/Keppy-s-Synthesizer/issues
AppUpdatesURL=https://github.com/KaleidonKep99/Keppy-s-Synthesizer/releases
AppVersion=4.0.1.0
ArchitecturesAllowed=x86 x64
ArchitecturesInstallIn64BitMode=x64
Compression=bzip
CompressionThreads=2
CreateAppDir=False
DefaultGroupName=Keppy's Synthesizer
ExtraDiskSpaceRequired=6
InternalCompressLevel=ultra64
LicenseFile=nsislicense.txt
MinVersion=0,5.01.2600sp3
OutputBaseFilename=KeppysSynthSetup
OutputDir=..\Keppy-s-Driver
SetupIconFile=midiicon.ico
ShowLanguageDialog=no
SolidCompression=yes
TimeStampsInUTC=True
UninstallDisplayIcon={sys}\keppysynth\KeppySynthConfigurator.exe
UninstallDisplayName=Keppy's Synthesizer (Uninstall only)
UninstallDisplaySize=5
UninstallFilesDir={sys}\keppysynth\
VersionInfoCompany=KaleidonKep99
VersionInfoCopyright=Copyright (c) 2011-2016 Brad Miller, Chris Moeller and Riccardo Loi. All rights reserved.
VersionInfoDescription=User-mode MIDI driver for Windows XP SP3 (SP2 for x64) and newer
VersionInfoProductName=Keppy's Synthesizer
VersionInfoProductTextVersion=4.0.1.0
VersionInfoTextVersion=User-mode MIDI driver for Windows XP SP3 (SP2 for x64) and newer
VersionInfoVersion=4.0.1.0
UsePreviousSetupType=False
FlatComponentsList=False
AlwaysShowGroupOnReadyPage=True
AlwaysShowDirOnReadyPage=True

[Files]
; 64-bit OS
Source: "external_packages\lib64\bass.dll"; DestDir: "{sys}\keppysynth"; DestName: "bass.dll"; Flags: replacesameversion ignoreversion; Check: Is64BitInstallMode
Source: "external_packages\lib64\bassenc.dll"; DestDir: "{sys}\keppysynth"; DestName: "bassenc.dll"; Flags: replacesameversion ignoreversion; Check: Is64BitInstallMode
Source: "external_packages\lib64\bassmidi.dll"; DestDir: "{sys}\keppysynth"; DestName: "bassmidi.dll"; Flags: replacesameversion ignoreversion; Check: Is64BitInstallMode
Source: "external_packages\lib64\bass_vst.dll"; DestDir: "{sys}\keppysynth"; DestName: "bass_vst.dll"; Flags: replacesameversion ignoreversion; Check: Is64BitInstallMode
Source: "external_packages\lib\bass.dll"; DestDir: "{syswow64}\keppysynth"; DestName: "bass.dll"; Flags: replacesameversion ignoreversion; Check: Is64BitInstallMode
Source: "external_packages\lib\bassenc.dll"; DestDir: "{syswow64}\keppysynth"; DestName: "bassenc.dll"; Flags: replacesameversion ignoreversion; Check: Is64BitInstallMode
Source: "external_packages\lib\bassmidi.dll"; DestDir: "{syswow64}\keppysynth"; DestName: "bassmidi.dll"; Flags: replacesameversion ignoreversion; Check: Is64BitInstallMode
Source: "external_packages\lib\bass_vst.dll"; DestDir: "{syswow64}\keppysynth"; DestName: "bass_vst.dll"; Flags: replacesameversion ignoreversion; Check: Is64BitInstallMode
Source: "output\64\keppysynth.dll"; DestDir: "{sys}\keppysynth"; DestName: "keppysynth.dll"; Flags: replacesameversion ignoreversion; Check: Is64BitInstallMode
Source: "output\KeppySynthConfigurator.exe"; DestDir: "{syswow64}\keppysynth"; DestName: "KeppySynthConfigurator.exe"; Flags: replacesameversion ignoreversion; Check: Is64BitInstallMode
Source: "output\KeppySynthDebugWindow.exe"; DestDir: "{syswow64}\keppysynth"; DestName: "KeppySynthDebugWindow.exe"; Flags: replacesameversion ignoreversion; Check: Is64BitInstallMode
Source: "output\KeppySynthWatchdog.exe"; DestDir: "{syswow64}\keppysynth"; DestName: "KeppySynthWatchdog.exe"; Flags: replacesameversion ignoreversion; Check: Is64BitInstallMode
Source: "output\keppysynth.dll"; DestDir: "{syswow64}\keppysynth"; DestName: "keppysynth.dll"; Flags: replacesameversion ignoreversion; Check: Is64BitInstallMode
Source: "output\midioutsetter32.exe"; DestDir: "{syswow64}\keppysynth"; DestName: "midioutsetter32.exe"; Flags: replacesameversion ignoreversion; Check: Is64BitInstallMode
Source: "output\midioutsetter64.exe"; DestDir: "{syswow64}\keppysynth"; DestName: "midioutsetter64.exe"; Flags: replacesameversion ignoreversion; Check: Is64BitInstallMode
Source: "output\sfpacker.exe"; DestDir: "{syswow64}\keppysynth"; DestName: "sfpacker.exe"; Flags: replacesameversion ignoreversion; Check: Is64BitInstallMode
Source: "output\sfzguide.txt"; DestDir: "{syswow64}\keppysynth"; DestName: "sfzguide.txt"; Flags: replacesameversion ignoreversion; Check: Is64BitInstallMode
; 32-bit OS
Source: "external_packages\lib\bass.dll"; DestDir: "{sys}\keppysynth"; DestName: "bass.dll"; Flags: replacesameversion ignoreversion; Check: not Is64BitInstallMode
Source: "external_packages\lib\bassenc.dll"; DestDir: "{sys}\keppysynth"; DestName: "bassenc.dll"; Flags: replacesameversion ignoreversion; Check: not Is64BitInstallMode
Source: "external_packages\lib\bassmidi.dll"; DestDir: "{sys}\keppysynth"; DestName: "bassmidi.dll"; Flags: replacesameversion ignoreversion; Check: not Is64BitInstallMode
Source: "external_packages\lib\bass_vst.dll"; DestDir: "{sys}\keppysynth"; DestName: "bass_vst.dll"; Flags: replacesameversion ignoreversion; Check: not Is64BitInstallMode
Source: "output\KeppySynthConfigurator.exe"; DestDir: "{sys}\keppysynth"; DestName: "KeppySynthConfigurator.exe"; Flags: replacesameversion ignoreversion; Check: not Is64BitInstallMode
Source: "output\KeppySynthDebugWindow.exe"; DestDir: "{sys}\keppysynth"; DestName: "KeppySynthDebugWindow.exe"; Flags: replacesameversion ignoreversion; Check: not Is64BitInstallMode
Source: "output\KeppySynthWatchdog.exe"; DestDir: "{sys}\keppysynth"; DestName: "KeppySynthWatchdog.exe"; Flags: replacesameversion ignoreversion; Check: not Is64BitInstallMode
Source: "output\keppysynth.dll"; DestDir: "{sys}\keppysynth"; DestName: "keppysynth.dll"; Flags: replacesameversion ignoreversion; Check: not Is64BitInstallMode
Source: "output\midioutsetter32.exe"; DestDir: "{sys}\keppysynth"; DestName: "midioutsetter32.exe"; Flags: replacesameversion ignoreversion; Check: not Is64BitInstallMode
Source: "output\sfpacker.exe"; DestDir: "{sys}\keppysynth"; DestName: "sfpacker.exe"; Flags: replacesameversion ignoreversion; Check: not Is64BitInstallMode
Source: "output\sfzguide.txt"; DestDir: "{sys}\keppysynth"; DestName: "sfzguide.txt"; Flags: replacesameversion ignoreversion; Check: not Is64BitInstallMode
; Generic for all the OSes
Source: "dxwebsetup.exe"; DestDir: "{tmp}"; DestName: "dxwebsetup.exe"; Flags: replacesameversion ignoreversion; MinVersion: 0,5.01sp3
Source: "output\keppymididrv.defaultblacklist"; DestDir: "{win}"; Flags: replacesameversion ignoreversion; MinVersion: 0,5.01sp3

[Dirs]
; 64-bit OS
Name: "{sys}\keppydrv"; Attribs: system; Check: Is64BitInstallMode
Name: "{syswow64}\keppydrv"; Attribs: system; Check: Is64BitInstallMode
; 32-bit OS
Name: "{sys}\keppydrv"; Attribs: system; Check: not Is64BitInstallMode    

[Icons]
; 64-bit OS
Name: "{group}\Configure Keppy's Synthesizer"; Filename: "{syswow64}\keppysynth\KeppySynthConfigurator.exe"; WorkingDir: "{app}"; Check: Is64BitInstallMode
Name: "{group}\Change advanced settings"; Filename: "{syswow64}\keppysynth\KeppySynthConfigurator.exe"; Parameters: "/AT"; WorkingDir: "{app}"; Check: Is64BitInstallMode
Name: "{group}\Soundfont packer by Kode54"; Filename: "{syswow64}\keppysynth\sfpacker.exe"; WorkingDir: "{app}"; Check: Is64BitInstallMode
; 32-bit OS
Name: "{group}\Configure Keppy's Synthesizer"; Filename: "{sys}\keppysynth\KeppySynthConfigurator.exe"; WorkingDir: "{app}"; Check: not Is64BitInstallMode
Name: "{group}\Change advanced settings"; Filename: "{sys}\keppysynth\KeppySynthConfigurator.exe"; Parameters: "/AT"; WorkingDir: "{app}"; Check: not Is64BitInstallMode
Name: "{group}\Soundfont packer by Kode54"; Filename: "{sys}\keppysynth\sfpacker.exe"; WorkingDir: "{app}"; Check: Is64BitInstallMode   
; Generic for all the OSes
Name: "{group}\Uninstall the driver"; Filename: "{uninstallexe}"

[Languages]
Name: en; MessagesFile: "compiler:Default.isl"
Name: it; MessagesFile: "compiler:Languages\Italian.isl"
Name: nl; MessagesFile: "compiler:Languages\Dutch.isl"
Name: de; MessagesFile: "compiler:Languages\German.isl"
Name: jp; MessagesFile: "compiler:Languages\Japanese.isl"

[Registry]
; Normal settings
Root: "HKCU"; Subkey: "Software\Keppy's Synthesizer"; ValueType: dword; ValueName: "buffull"; ValueData: "0"; Flags: uninsdeletekey; Permissions: everyone-full
Root: "HKCU"; Subkey: "Software\Keppy's Synthesizer"; ValueType: dword; ValueName: "currentcpuusage0"; ValueData: "0"; Flags: uninsdeletekey; Permissions: everyone-full
Root: "HKCU"; Subkey: "Software\Keppy's Synthesizer"; ValueType: dword; ValueName: "currentvoices0"; ValueData: "0"; Flags: uninsdeletekey; Permissions: everyone-full
Root: "HKCU"; Subkey: "Software\Keppy's Synthesizer"; ValueType: dword; ValueName: "int"; ValueData: "0"; Flags: uninsdeletekey; Permissions: everyone-full
Root: "HKCU"; Subkey: "Software\Keppy's Synthesizer\Settings"; ValueType: dword; ValueName: "autoupdatecheck"; ValueData: "1"; Flags: uninsdeletekey createvalueifdoesntexist
Root: "HKCU"; Subkey: "Software\Keppy's Synthesizer\Settings"; ValueType: dword; ValueName: "buflen"; ValueData: "30"; Flags: uninsdeletekey createvalueifdoesntexist
Root: "HKCU"; Subkey: "Software\Keppy's Synthesizer\Settings"; ValueType: dword; ValueName: "cpu"; ValueData: "75"; Flags: uninsdeletekey createvalueifdoesntexist
Root: "HKCU"; Subkey: "Software\Keppy's Synthesizer\Settings"; ValueType: dword; ValueName: "defaultsflist"; ValueData: "1"; Flags: uninsdeletekey createvalueifdoesntexist
Root: "HKCU"; Subkey: "Software\Keppy's Synthesizer\Settings"; ValueType: dword; ValueName: "encmode"; ValueData: "0"; Flags: createvalueifdoesntexist uninsdeletekey
Root: "HKCU"; Subkey: "Software\Keppy's Synthesizer\Settings"; ValueType: dword; ValueName: "extra8lists"; ValueData: "0"; Flags: createvalueifdoesntexist uninsdeletekey
Root: "HKCU"; Subkey: "Software\Keppy's Synthesizer\Settings"; ValueType: dword; ValueName: "firstrun"; ValueData: "1"; Flags: createvalueifdoesntexist uninsdeletekey
Root: "HKCU"; Subkey: "Software\Keppy's Synthesizer\Settings"; ValueType: dword; ValueName: "frequency"; ValueData: "44100"; Flags: createvalueifdoesntexist uninsdeletekey
Root: "HKCU"; Subkey: "Software\Keppy's Synthesizer\Settings"; ValueType: dword; ValueName: "legacybuf"; ValueData: "0"; Flags: uninsdeletekey dontcreatekey
Root: "HKCU"; Subkey: "Software\Keppy's Synthesizer\Settings"; ValueType: dword; ValueName: "midivolumeoverride"; ValueData: "0"; Flags: createvalueifdoesntexist uninsdeletekey
Root: "HKCU"; Subkey: "Software\Keppy's Synthesizer\Settings"; ValueType: dword; ValueName: "newevbuffvalue"; ValueData: "16384"; Flags: createvalueifdoesntexist uninsdeletekey
Root: "HKCU"; Subkey: "Software\Keppy's Synthesizer\Settings"; ValueType: dword; ValueName: "nodx8"; ValueData: "1"; Flags: uninsdeletekey dontcreatekey
Root: "HKCU"; Subkey: "Software\Keppy's Synthesizer\Settings"; ValueType: dword; ValueName: "nofloat"; ValueData: "1"; Flags: createvalueifdoesntexist uninsdeletekey
Root: "HKCU"; Subkey: "Software\Keppy's Synthesizer\Settings"; ValueType: dword; ValueName: "nofx"; ValueData: "1"; Flags: createvalueifdoesntexist uninsdeletekey
Root: "HKCU"; Subkey: "Software\Keppy's Synthesizer\Settings"; ValueType: dword; ValueName: "noteoff"; ValueData: "0"; Flags: createvalueifdoesntexist uninsdeletekey
Root: "HKCU"; Subkey: "Software\Keppy's Synthesizer\Settings"; ValueType: dword; ValueName: "oldbuffersystem"; ValueData: "0"; Flags: createvalueifdoesntexist uninsdeletekey
Root: "HKCU"; Subkey: "Software\Keppy's Synthesizer\Settings"; ValueType: dword; ValueName: "polyphony"; ValueData: "500"; Flags: createvalueifdoesntexist uninsdeletekey
Root: "HKCU"; Subkey: "Software\Keppy's Synthesizer\Settings"; ValueType: dword; ValueName: "preload"; ValueData: "1"; Flags: createvalueifdoesntexist uninsdeletekey
Root: "HKCU"; Subkey: "Software\Keppy's Synthesizer\Settings"; ValueType: dword; ValueName: "sfdisableconf"; ValueData: "0"; Flags: createvalueifdoesntexist uninsdeletekey
Root: "HKCU"; Subkey: "Software\Keppy's Synthesizer\Settings"; ValueType: dword; ValueName: "sinc"; ValueData: "0"; Flags: createvalueifdoesntexist uninsdeletekey
Root: "HKCU"; Subkey: "Software\Keppy's Synthesizer\Settings"; ValueType: dword; ValueName: "sndbfvalue"; ValueData: "100"; Flags: createvalueifdoesntexist uninsdeletekey
Root: "HKCU"; Subkey: "Software\Keppy's Synthesizer\Settings"; ValueType: dword; ValueName: "softwaremode"; ValueData: "1"; Flags: createvalueifdoesntexist uninsdeletekey
Root: "HKCU"; Subkey: "Software\Keppy's Synthesizer\Settings"; ValueType: dword; ValueName: "sysresetignore"; ValueData: "0"; Flags: createvalueifdoesntexist uninsdeletekey
Root: "HKCU"; Subkey: "Software\Keppy's Synthesizer\Settings"; ValueType: dword; ValueName: "tracks"; ValueData: "16"; Flags: createvalueifdoesntexist uninsdeletekey
Root: "HKCU"; Subkey: "Software\Keppy's Synthesizer\Settings"; ValueType: dword; ValueName: "vmsemu"; ValueData: "0"; Flags: createvalueifdoesntexist uninsdeletekey
Root: "HKCU"; Subkey: "Software\Keppy's Synthesizer\Settings"; ValueType: dword; ValueName: "volume"; ValueData: "10000"; Flags: createvalueifdoesntexist uninsdeletekey
Root: "HKCU"; Subkey: "Software\Keppy's Synthesizer\Settings"; ValueType: dword; ValueName: "volumehotkeys"; ValueData: "1"; Flags: createvalueifdoesntexist uninsdeletekey
Root: "HKCU"; Subkey: "Software\Keppy's Synthesizer\Settings"; ValueType: dword; ValueName: "volumemon"; ValueData: "0"; Flags: createvalueifdoesntexist uninsdeletekey
Root: "HKCU"; Subkey: "Software\Keppy's Synthesizer\Settings"; ValueType: dword; ValueName: "xaudiodisabled"; ValueData: "0"; Flags: createvalueifdoesntexist uninsdeletekey

;Channels volume
Root: "HKCU"; Subkey: "Software\Keppy's Synthesizer\Channels"; ValueType: dword; ValueName: "ch1"; ValueData: "100"; Flags: createvalueifdoesntexist uninsdeletekey
Root: "HKCU"; Subkey: "Software\Keppy's Synthesizer\Channels"; ValueType: dword; ValueName: "ch2"; ValueData: "100"; Flags: createvalueifdoesntexist uninsdeletekey
Root: "HKCU"; Subkey: "Software\Keppy's Synthesizer\Channels"; ValueType: dword; ValueName: "ch3"; ValueData: "100"; Flags: createvalueifdoesntexist uninsdeletekey
Root: "HKCU"; Subkey: "Software\Keppy's Synthesizer\Channels"; ValueType: dword; ValueName: "ch4"; ValueData: "100"; Flags: createvalueifdoesntexist uninsdeletekey
Root: "HKCU"; Subkey: "Software\Keppy's Synthesizer\Channels"; ValueType: dword; ValueName: "ch5"; ValueData: "100"; Flags: createvalueifdoesntexist uninsdeletekey
Root: "HKCU"; Subkey: "Software\Keppy's Synthesizer\Channels"; ValueType: dword; ValueName: "ch6"; ValueData: "100"; Flags: createvalueifdoesntexist uninsdeletekey
Root: "HKCU"; Subkey: "Software\Keppy's Synthesizer\Channels"; ValueType: dword; ValueName: "ch7"; ValueData: "100"; Flags: createvalueifdoesntexist uninsdeletekey
Root: "HKCU"; Subkey: "Software\Keppy's Synthesizer\Channels"; ValueType: dword; ValueName: "ch8"; ValueData: "100"; Flags: createvalueifdoesntexist uninsdeletekey
Root: "HKCU"; Subkey: "Software\Keppy's Synthesizer\Channels"; ValueType: dword; ValueName: "ch9"; ValueData: "100"; Flags: createvalueifdoesntexist uninsdeletekey
Root: "HKCU"; Subkey: "Software\Keppy's Synthesizer\Channels"; ValueType: dword; ValueName: "ch10"; ValueData: "100"; Flags: createvalueifdoesntexist uninsdeletekey
Root: "HKCU"; Subkey: "Software\Keppy's Synthesizer\Channels"; ValueType: dword; ValueName: "ch11"; ValueData: "100"; Flags: createvalueifdoesntexist uninsdeletekey
Root: "HKCU"; Subkey: "Software\Keppy's Synthesizer\Channels"; ValueType: dword; ValueName: "ch12"; ValueData: "100"; Flags: createvalueifdoesntexist uninsdeletekey
Root: "HKCU"; Subkey: "Software\Keppy's Synthesizer\Channels"; ValueType: dword; ValueName: "ch13"; ValueData: "100"; Flags: createvalueifdoesntexist uninsdeletekey
Root: "HKCU"; Subkey: "Software\Keppy's Synthesizer\Channels"; ValueType: dword; ValueName: "ch14"; ValueData: "100"; Flags: createvalueifdoesntexist uninsdeletekey
Root: "HKCU"; Subkey: "Software\Keppy's Synthesizer\Channels"; ValueType: dword; ValueName: "ch15"; ValueData: "100"; Flags: createvalueifdoesntexist uninsdeletekey
Root: "HKCU"; Subkey: "Software\Keppy's Synthesizer\Channels"; ValueType: dword; ValueName: "ch16"; ValueData: "100"; Flags: createvalueifdoesntexist uninsdeletekey

;Watchdog              
Root: "HKCU"; Subkey: "Software\Keppy's Synthesizer\Watchdog"; ValueType: dword; ValueName: "closewatchdog"; ValueData: "1"; Flags: createvalueifdoesntexist uninsdeletekey
Root: "HKCU"; Subkey: "Software\Keppy's Synthesizer\Watchdog"; ValueType: dword; ValueName: "rel1"; ValueData: "0"; Flags: createvalueifdoesntexist uninsdeletekey
Root: "HKCU"; Subkey: "Software\Keppy's Synthesizer\Watchdog"; ValueType: dword; ValueName: "rel2"; ValueData: "0"; Flags: createvalueifdoesntexist uninsdeletekey
Root: "HKCU"; Subkey: "Software\Keppy's Synthesizer\Watchdog"; ValueType: dword; ValueName: "rel3"; ValueData: "0"; Flags: createvalueifdoesntexist uninsdeletekey
Root: "HKCU"; Subkey: "Software\Keppy's Synthesizer\Watchdog"; ValueType: dword; ValueName: "rel4"; ValueData: "0"; Flags: createvalueifdoesntexist uninsdeletekey
Root: "HKCU"; Subkey: "Software\Keppy's Synthesizer\Watchdog"; ValueType: dword; ValueName: "rel5"; ValueData: "0"; Flags: createvalueifdoesntexist uninsdeletekey
Root: "HKCU"; Subkey: "Software\Keppy's Synthesizer\Watchdog"; ValueType: dword; ValueName: "rel6"; ValueData: "0"; Flags: createvalueifdoesntexist uninsdeletekey
Root: "HKCU"; Subkey: "Software\Keppy's Synthesizer\Watchdog"; ValueType: dword; ValueName: "rel7"; ValueData: "0"; Flags: createvalueifdoesntexist uninsdeletekey
Root: "HKCU"; Subkey: "Software\Keppy's Synthesizer\Watchdog"; ValueType: dword; ValueName: "rel8"; ValueData: "0"; Flags: createvalueifdoesntexist uninsdeletekey
Root: "HKCU"; Subkey: "Software\Keppy's Synthesizer\Watchdog"; ValueType: dword; ValueName: "runwd"; ValueData: "0"; Flags: createvalueifdoesntexist uninsdeletekey
Root: "HKCU"; Subkey: "Software\Keppy's Synthesizer\Watchdog"; ValueType: dword; ValueName: "watchdog"; ValueData: "1"; Flags: createvalueifdoesntexist uninsdeletekey

; 64-bit OS
Root: "HKLM"; Subkey: "Software\Microsoft\Windows NT\CurrentVersion\Drivers32"; ValueType: string; ValueName: "midi9"; ValueData: "keppysynth\keppysynth.dll"; Flags: uninsdeletevalue dontcreatekey; Check: Is64BitInstallMode
Root: "HKLM"; Subkey: "Software\Wow6432Node\Microsoft\Windows NT\CurrentVersion\Drivers32"; ValueType: string; ValueName: "midi9"; ValueData: "keppysynth\keppysynth.dll"; Flags: uninsdeletevalue dontcreatekey; Check: Is64BitInstallMode

; 32-bit OS
Root: "HKLM"; Subkey: "Software\Microsoft\Windows NT\CurrentVersion\Drivers32"; ValueType: string; ValueName: "midi9"; ValueData: "keppysynth\keppysynth.dll"; Flags: uninsdeletevalue dontcreatekey; Check: not Is64BitInstallMode

[InstallDelete]
Type: filesandordirs; Name: "{syswow64}\keppydrv"; Check: Is64BitInstallMode
Type: filesandordirs; Name: "{sys}\keppydrv"; Check: Is64BitInstallMode
Type: filesandordirs; Name: "{sys}\keppydrv"; Check: not Is64BitInstallMode
Type: files; Name: "{syswow64}\keppydrv\KeppyDriverConfigurator.exe"; Check: Is64BitInstallMode
Type: files; Name: "{syswow64}\keppydrv\KeppyDriverWatchdog.exe"; Check: Is64BitInstallMode
Type: files; Name: "{syswow64}\keppydrv\bass.dll"; Check: Is64BitInstallMode
Type: files; Name: "{syswow64}\keppydrv\bass_mpc.dll"; Check: Is64BitInstallMode
Type: files; Name: "{syswow64}\keppydrv\bassenc.dll"; Check: Is64BitInstallMode
Type: files; Name: "{syswow64}\keppydrv\bassflac.dll"; Check: Is64BitInstallMode
Type: files; Name: "{syswow64}\keppydrv\bassmidi.dll"; Check: Is64BitInstallMode
Type: files; Name: "{syswow64}\keppydrv\bassopus.dll"; Check: Is64BitInstallMode
Type: files; Name: "{syswow64}\keppydrv\basswv.dll"; Check: Is64BitInstallMode
Type: files; Name: "{syswow64}\keppydrv\keppydrv.dll"; Check: Is64BitInstallMode
Type: files; Name: "{syswow64}\keppydrv\keppydrvcfg.exe"; Check: Is64BitInstallMode
Type: files; Name: "{syswow64}\keppydrv\sfpacker.exe"; Check: Is64BitInstallMode
Type: files; Name: "{syswow64}\keppysynth\KeppySynthConfigurator.exe"; Check: Is64BitInstallMode
Type: files; Name: "{syswow64}\keppysynth\KeppySynthWatchdog.exe"; Check: Is64BitInstallMode
Type: files; Name: "{syswow64}\keppysynth\bass.dll"; Check: Is64BitInstallMode
Type: files; Name: "{syswow64}\keppysynth\bass_mpc.dll"; Check: Is64BitInstallMode
Type: files; Name: "{syswow64}\keppysynth\bassenc.dll"; Check: Is64BitInstallMode
Type: files; Name: "{syswow64}\keppysynth\bassflac.dll"; Check: Is64BitInstallMode
Type: files; Name: "{syswow64}\keppysynth\bassmidi.dll"; Check: Is64BitInstallMode
Type: files; Name: "{syswow64}\keppysynth\bassopus.dll"; Check: Is64BitInstallMode
Type: files; Name: "{syswow64}\keppysynth\basswv.dll"; Check: Is64BitInstallMode
Type: files; Name: "{syswow64}\keppysynth\keppysynth.dll"; Check: Is64BitInstallMode
Type: files; Name: "{syswow64}\keppysynth\keppysynthcfg.exe"; Check: Is64BitInstallMode
Type: files; Name: "{syswow64}\keppysynth\sfpacker.exe"; Check: Is64BitInstallMode
Type: files; Name: "{sys}\keppydrv\KeppyDriverConfigurator.exe"; Check: not Is64BitInstallMode
Type: files; Name: "{sys}\keppydrv\KeppyDriverWatchdog.exe"; Check: not Is64BitInstallMode
Type: files; Name: "{sys}\keppysynth\bass.dll"; Check: Is64BitInstallMode
Type: files; Name: "{sys}\keppysynth\bass.dll"; Check: not Is64BitInstallMode
Type: files; Name: "{sys}\keppysynth\bass_mpc.dll"; Check: Is64BitInstallMode
Type: files; Name: "{sys}\keppysynth\bass_mpc.dll"; Check: not Is64BitInstallMode
Type: files; Name: "{sys}\keppysynth\bassenc.dll"; Check: Is64BitInstallMode
Type: files; Name: "{sys}\keppysynth\bassenc.dll"; Check: not Is64BitInstallMode
Type: files; Name: "{sys}\keppysynth\bassflac.dll"; Check: Is64BitInstallMode
Type: files; Name: "{sys}\keppysynth\bassflac.dll"; Check: not Is64BitInstallMode
Type: files; Name: "{sys}\keppysynth\bassmidi.dll"; Check: Is64BitInstallMode
Type: files; Name: "{sys}\keppysynth\bassmidi.dll"; Check: not Is64BitInstallMode
Type: files; Name: "{sys}\keppysynth\bassopus.dll"; Check: Is64BitInstallMode
Type: files; Name: "{sys}\keppysynth\bassopus.dll"; Check: not Is64BitInstallMode
Type: files; Name: "{sys}\keppysynth\basswv.dll"; Check: Is64BitInstallMode
Type: files; Name: "{sys}\keppysynth\basswv.dll"; Check: not Is64BitInstallMode
Type: files; Name: "{sys}\keppysynth\keppysynth.dll"; Check: Is64BitInstallMode
Type: files; Name: "{sys}\keppysynth\keppysynth.dll"; Check: not Is64BitInstallMode
Type: files; Name: "{sys}\keppydrv\bass.dll"; Check: Is64BitInstallMode
Type: files; Name: "{sys}\keppydrv\bass.dll"; Check: not Is64BitInstallMode
Type: files; Name: "{sys}\keppydrv\bass_mpc.dll"; Check: Is64BitInstallMode
Type: files; Name: "{sys}\keppydrv\bass_mpc.dll"; Check: not Is64BitInstallMode
Type: files; Name: "{sys}\keppydrv\bassenc.dll"; Check: Is64BitInstallMode
Type: files; Name: "{sys}\keppydrv\bassenc.dll"; Check: not Is64BitInstallMode
Type: files; Name: "{sys}\keppydrv\bassflac.dll"; Check: Is64BitInstallMode
Type: files; Name: "{sys}\keppydrv\bassflac.dll"; Check: not Is64BitInstallMode
Type: files; Name: "{sys}\keppydrv\bassmidi.dll"; Check: Is64BitInstallMode
Type: files; Name: "{sys}\keppydrv\bassmidi.dll"; Check: not Is64BitInstallMode
Type: files; Name: "{sys}\keppydrv\bassopus.dll"; Check: Is64BitInstallMode
Type: files; Name: "{sys}\keppydrv\bassopus.dll"; Check: not Is64BitInstallMode
Type: files; Name: "{sys}\keppydrv\basswv.dll"; Check: Is64BitInstallMode
Type: files; Name: "{sys}\keppydrv\basswv.dll"; Check: not Is64BitInstallMode
Type: files; Name: "{sys}\keppydrv\keppydrv.dll"; Check: Is64BitInstallMode
Type: files; Name: "{sys}\keppydrv\keppydrv.dll"; Check: not Is64BitInstallMode
Type: files; Name: "{sys}\keppydrv\keppydrvcfg.exe"; Check: Is64BitInstallMode
Type: files; Name: "{sys}\keppydrv\keppydrvcfg.exe"; Check: not Is64BitInstallMode
Type: files; Name: "{sys}\keppydrv\sfpacker.exe"; Check: not Is64BitInstallMode

[UninstallDelete]
Type: files; Name: "{syswow64}\keppydrv\KeppyDriverConfigurator.exe"; Check: Is64BitInstallMode
Type: files; Name: "{syswow64}\keppydrv\KeppyDriverWatchdog.exe"; Check: Is64BitInstallMode
Type: files; Name: "{syswow64}\keppydrv\bass.dll"; Check: Is64BitInstallMode
Type: files; Name: "{syswow64}\keppydrv\bass_mpc.dll"; Check: Is64BitInstallMode
Type: files; Name: "{syswow64}\keppydrv\bassenc.dll"; Check: Is64BitInstallMode
Type: files; Name: "{syswow64}\keppydrv\bassflac.dll"; Check: Is64BitInstallMode
Type: files; Name: "{syswow64}\keppydrv\bassmidi.dll"; Check: Is64BitInstallMode
Type: files; Name: "{syswow64}\keppydrv\bassopus.dll"; Check: Is64BitInstallMode
Type: files; Name: "{syswow64}\keppydrv\basswv.dll"; Check: Is64BitInstallMode
Type: files; Name: "{syswow64}\keppydrv\keppydrv.dll"; Check: Is64BitInstallMode
Type: files; Name: "{syswow64}\keppydrv\keppydrvcfg.exe"; Check: Is64BitInstallMode
Type: files; Name: "{syswow64}\keppydrv\sfpacker.exe"; Check: Is64BitInstallMode
Type: files; Name: "{syswow64}\keppysynth\KeppySynthConfigurator.exe"; Check: Is64BitInstallMode
Type: files; Name: "{syswow64}\keppysynth\KeppySynthWatchdog.exe"; Check: Is64BitInstallMode
Type: files; Name: "{syswow64}\keppysynth\bass.dll"; Check: Is64BitInstallMode
Type: files; Name: "{syswow64}\keppysynth\bass_mpc.dll"; Check: Is64BitInstallMode
Type: files; Name: "{syswow64}\keppysynth\bassenc.dll"; Check: Is64BitInstallMode
Type: files; Name: "{syswow64}\keppysynth\bassflac.dll"; Check: Is64BitInstallMode
Type: files; Name: "{syswow64}\keppysynth\bassmidi.dll"; Check: Is64BitInstallMode
Type: files; Name: "{syswow64}\keppysynth\bassopus.dll"; Check: Is64BitInstallMode
Type: files; Name: "{syswow64}\keppysynth\basswv.dll"; Check: Is64BitInstallMode
Type: files; Name: "{syswow64}\keppysynth\keppysynth.dll"; Check: Is64BitInstallMode
Type: files; Name: "{syswow64}\keppysynth\keppysynthcfg.exe"; Check: Is64BitInstallMode
Type: files; Name: "{syswow64}\keppysynth\sfpacker.exe"; Check: Is64BitInstallMode
Type: files; Name: "{sys}\keppydrv\KeppyDriverConfigurator.exe"; Check: not Is64BitInstallMode
Type: files; Name: "{sys}\keppydrv\KeppyDriverWatchdog.exe"; Check: not Is64BitInstallMode
Type: files; Name: "{sys}\keppysynth\bass.dll"; Check: Is64BitInstallMode
Type: files; Name: "{sys}\keppysynth\bass.dll"; Check: not Is64BitInstallMode
Type: files; Name: "{sys}\keppysynth\bass_mpc.dll"; Check: Is64BitInstallMode
Type: files; Name: "{sys}\keppysynth\bass_mpc.dll"; Check: not Is64BitInstallMode
Type: files; Name: "{sys}\keppysynth\bassenc.dll"; Check: Is64BitInstallMode
Type: files; Name: "{sys}\keppysynth\bassenc.dll"; Check: not Is64BitInstallMode
Type: files; Name: "{sys}\keppysynth\bassflac.dll"; Check: Is64BitInstallMode
Type: files; Name: "{sys}\keppysynth\bassflac.dll"; Check: not Is64BitInstallMode
Type: files; Name: "{sys}\keppysynth\bassmidi.dll"; Check: Is64BitInstallMode
Type: files; Name: "{sys}\keppysynth\bassmidi.dll"; Check: not Is64BitInstallMode
Type: files; Name: "{sys}\keppysynth\bassopus.dll"; Check: Is64BitInstallMode
Type: files; Name: "{sys}\keppysynth\bassopus.dll"; Check: not Is64BitInstallMode
Type: files; Name: "{sys}\keppysynth\basswv.dll"; Check: Is64BitInstallMode
Type: files; Name: "{sys}\keppysynth\basswv.dll"; Check: not Is64BitInstallMode
Type: files; Name: "{sys}\keppysynth\keppysynth.dll"; Check: Is64BitInstallMode
Type: files; Name: "{sys}\keppysynth\keppysynth.dll"; Check: not Is64BitInstallMode
Type: files; Name: "{sys}\keppydrv\bass.dll"; Check: Is64BitInstallMode
Type: files; Name: "{sys}\keppydrv\bass.dll"; Check: not Is64BitInstallMode
Type: files; Name: "{sys}\keppydrv\bass_mpc.dll"; Check: Is64BitInstallMode
Type: files; Name: "{sys}\keppydrv\bass_mpc.dll"; Check: not Is64BitInstallMode
Type: files; Name: "{sys}\keppydrv\bassenc.dll"; Check: Is64BitInstallMode
Type: files; Name: "{sys}\keppydrv\bassenc.dll"; Check: not Is64BitInstallMode
Type: files; Name: "{sys}\keppydrv\bassflac.dll"; Check: Is64BitInstallMode
Type: files; Name: "{sys}\keppydrv\bassflac.dll"; Check: not Is64BitInstallMode
Type: files; Name: "{sys}\keppydrv\bassmidi.dll"; Check: Is64BitInstallMode
Type: files; Name: "{sys}\keppydrv\bassmidi.dll"; Check: not Is64BitInstallMode
Type: files; Name: "{sys}\keppydrv\bassopus.dll"; Check: Is64BitInstallMode
Type: files; Name: "{sys}\keppydrv\bassopus.dll"; Check: not Is64BitInstallMode
Type: files; Name: "{sys}\keppydrv\basswv.dll"; Check: Is64BitInstallMode
Type: files; Name: "{sys}\keppydrv\basswv.dll"; Check: not Is64BitInstallMode
Type: files; Name: "{sys}\keppydrv\keppydrv.dll"; Check: Is64BitInstallMode
Type: files; Name: "{sys}\keppydrv\keppydrv.dll"; Check: not Is64BitInstallMode
Type: files; Name: "{sys}\keppydrv\keppydrvcfg.exe"; Check: Is64BitInstallMode
Type: files; Name: "{sys}\keppydrv\keppydrvcfg.exe"; Check: not Is64BitInstallMode
Type: files; Name: "{sys}\keppydrv\sfpacker.exe"; Check: not Is64BitInstallMode

[Run]
Filename: "{syswow64}\keppysynth\KeppySynthConfigurator.exe"; Flags: postinstall runascurrentuser nowait; Description: "Run the configurator, to set up soundfonts"; StatusMsg: "Run the configurator, to set up soundfonts"; Check: Is64BitInstallMode
Filename: "{sys}\keppysynth\KeppySynthConfigurator.exe"; Flags: postinstall runascurrentuser nowait; Description: "Run the configurator, to set up soundfonts"; StatusMsg: "Run the configurator, to set up soundfonts"; Check: not Is64BitInstallMode
Filename: "{syswow64}\keppysynth\KeppySynthConfigurator.exe"; Parameters: "/AS"; Flags: runascurrentuser nowait; Description: "Moving stuff from ""LocalAppdata"" to ""UserProfile""..."; StatusMsg: "Moving stuff from ""LocalAppdata"" to ""UserProfile""..."; Check: Is64BitInstallMode
Filename: "{sys}\keppysynth\KeppySynthConfigurator.exe"; Parameters: "/AS"; Flags: runascurrentuser nowait; Description: "Moving stuff from ""LocalAppdata"" to ""UserProfile""..."; StatusMsg: "Moving stuff from ""LocalAppdata"" to ""UserProfile""..."; Check: not Is64BitInstallMode
Filename: "http://frozensnowy.com/"; Flags: shellexec postinstall runasoriginaluser nowait unchecked; Description: "Visit Frozen Snow Productions"; StatusMsg: "Visit Frozen Snow Productions"
Filename: "{tmp}\dxwebsetup.exe"; Parameters: "/q"; Flags: waituntilterminated; Description: "DXINSTALL"; StatusMsg: "Installing DirectX Redistributable (Jun 2010), please wait..."

[Messages]
AboutSetupTitle=About the driver
ApplicationsFound2=The driver's files are locked by some programs or by Windows itself.%n%nIt is recommended to close the following programs and/or restart Windows to solve the issue.
ApplicationsFound=The driver's files are locked by some programs or by Windows itself.%n%nIt is recommended to close the following programs and/or restart Windows to solve the issue.
CannotContinue=The installation cannot continue.%n%nClick Cancel to exit.
CannotContinue=The setup cannot continue. Click Cancel to exit.
CloseApplications=Close them immediately!
ConfirmUninstall=Are you sure you want to uninstall the driver? Be sure to select another MIDI device on your programs, before uninstalling.
ConfirmUninstall=Before uninstalling, be sure to set your MIDI programs to the default synth (Microsoft GS Wavetable Synth), to avoid crashes.%n%nPress Yes to continue, or no to abort the uninstallation.
DontCloseApplications=Nah, leave them opened. (I'll restart later to finish the installation)
ErrorRestartingComputer=Unable to restart the computer. Please do this manually.
ExitSetupMessage=You didn't finished the installation. If you exit now, the driver will not be installed!%n%nYou may run the setup again at another time to complete the installation.%n%nAre you sure you want to exit?
FinishedLabel=The driver has been succesfully installed. Please configure your MIDI programs to allow them to use it, and add some soundfonts to the soundfonts lists!
FinishedLabelNoIcons=The driver has been succesfully installed.
SetupAppTitle=Keppy's Driver Installer
UninstallAppFullTitle=Keppy's Driver Uninstaller
WelcomeLabel1=Keppy's Driver Installation Wizard
WelcomeLabel2=The setup will install the driver on your computer.%n%nIt is recommended that you close all other applications before continuing.
