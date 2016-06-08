[Setup]
AllowCancelDuringInstall=False
AppContact=kaleidonkep99@outlook.com
AppCopyright=Copyright (c) 2011-2016 Brad Miller, Chris Moeller and Riccardo Loi. All rights reserved.
AppId={{950DEC78-2D12-4917-BE69-CB04FE84B21F}
AppName=Keppy's Driver
AppPublisher=Keppy Studios
AppPublisherURL=http://keppystudios.com
AppSupportPhone=+393511888475
AppSupportURL=mailto:kaleidonkep99@outlook.com
AppUpdatesURL=https://github.com/KaleidonKep99/Keppy-s-MIDI-Driver/releases
AppVersion=3.2.0.5
ArchitecturesAllowed=x86 x64
ArchitecturesInstallIn64BitMode=x64
Compression=bzip
CompressionThreads=2
CreateAppDir=False
DefaultGroupName=Keppy's Driver
ExtraDiskSpaceRequired=6
InternalCompressLevel=ultra64
LicenseFile=nsislicense.txt
MinVersion=0,6.0.6000
OutputBaseFilename=KeppysDriverSetup
OutputDir=..\Keppy-s-Driver
SetupIconFile=midiicon.ico
ShowLanguageDialog=no
SolidCompression=yes
TimeStampsInUTC=True
UninstallDisplayIcon={sys}\keppydrv\keppydrvcfg.exe
UninstallDisplayName=Keppy's Driver (Uninstall only)
UninstallDisplaySize=5
UninstallFilesDir={sys}\keppydrv\
VersionInfoCompany=Keppy Studios
VersionInfoCopyright=Copyright (c) 2011-2016 Brad Miller, Chris Moeller and Riccardo Loi. All rights reserved.
VersionInfoDescription=User-mode MIDI driver for Windows Vista and newer
VersionInfoProductName=Keppy's Driver
VersionInfoProductTextVersion=3.2.0.5
VersionInfoTextVersion=User-mode MIDI driver for Windows Vista and newer
VersionInfoVersion=3.2.0.5
UsePreviousSetupType=False
FlatComponentsList=False
AlwaysShowGroupOnReadyPage=True
AlwaysShowDirOnReadyPage=True

[Files]
; 64-bit OS
Source: "external_packages\lib64\bass.dll"; DestDir: "{sys}\keppydrv"; DestName: "bass.dll"; Flags: replacesameversion ignoreversion; Check: Is64BitInstallMode
Source: "external_packages\lib64\bass_mpc.dll"; DestDir: "{sys}\keppydrv"; DestName: "bass_mpc.dll"; Flags: replacesameversion ignoreversion; Check: Is64BitInstallMode
Source: "external_packages\lib64\bassenc.dll"; DestDir: "{sys}\keppydrv"; DestName: "bassenc.dll"; Flags: replacesameversion ignoreversion; Check: Is64BitInstallMode
Source: "external_packages\lib64\bassflac.dll"; DestDir: "{sys}\keppydrv"; DestName: "bassflac.dll"; Flags: replacesameversion ignoreversion; Check: Is64BitInstallMode
Source: "external_packages\lib64\bassmidi.dll"; DestDir: "{sys}\keppydrv"; DestName: "bassmidi.dll"; Flags: replacesameversion ignoreversion; Check: Is64BitInstallMode
Source: "external_packages\lib64\bassopus.dll"; DestDir: "{sys}\keppydrv"; DestName: "bassopus.dll"; Flags: replacesameversion ignoreversion; Check: Is64BitInstallMode
Source: "external_packages\lib64\basswv.dll"; DestDir: "{sys}\keppydrv"; DestName: "basswv.dll"; Flags: replacesameversion ignoreversion; Check: Is64BitInstallMode
Source: "external_packages\lib\bass.dll"; DestDir: "{syswow64}\keppydrv"; DestName: "bass.dll"; Flags: replacesameversion ignoreversion; Check: Is64BitInstallMode
Source: "external_packages\lib\bass_mpc.dll"; DestDir: "{syswow64}\keppydrv"; DestName: "bass_mpc.dll"; Flags: replacesameversion ignoreversion; Check: Is64BitInstallMode
Source: "external_packages\lib\bassenc.dll"; DestDir: "{syswow64}\keppydrv"; DestName: "bassenc.dll"; Flags: replacesameversion ignoreversion; Check: Is64BitInstallMode
Source: "external_packages\lib\bassflac.dll"; DestDir: "{syswow64}\keppydrv"; DestName: "bassflac.dll"; Flags: replacesameversion ignoreversion; Check: Is64BitInstallMode
Source: "external_packages\lib\bassmidi.dll"; DestDir: "{syswow64}\keppydrv"; DestName: "bassmidi.dll"; Flags: replacesameversion ignoreversion; Check: Is64BitInstallMode
Source: "external_packages\lib\bassopus.dll"; DestDir: "{syswow64}\keppydrv"; DestName: "bassopus.dll"; Flags: replacesameversion ignoreversion; Check: Is64BitInstallMode
Source: "external_packages\lib\basswv.dll"; DestDir: "{syswow64}\keppydrv"; DestName: "basswv.dll"; Flags: replacesameversion ignoreversion; Check: Is64BitInstallMode
Source: "output\64\keppydrv.dll"; DestDir: "{sys}\keppydrv"; DestName: "keppydrv.dll"; Flags: replacesameversion ignoreversion; Check: Is64BitInstallMode
Source: "output\KeppyDriverConfigurator.exe"; DestDir: "{syswow64}\keppydrv"; DestName: "KeppyDriverConfigurator.exe"; Flags: replacesameversion ignoreversion; Check: Is64BitInstallMode
Source: "output\keppydrv.dll"; DestDir: "{syswow64}\keppydrv"; DestName: "keppydrv.dll"; Flags: replacesameversion ignoreversion; Check: Is64BitInstallMode
Source: "output\midioutsetter32.exe"; DestDir: "{syswow64}\keppydrv"; DestName: "midioutsetter32.exe"; Flags: replacesameversion ignoreversion; Check: Is64BitInstallMode
Source: "output\midioutsetter64.exe"; DestDir: "{syswow64}\keppydrv"; DestName: "midioutsetter64.exe"; Flags: replacesameversion ignoreversion; Check: Is64BitInstallMode
Source: "output\sfpacker.exe"; DestDir: "{syswow64}\keppydrv"; DestName: "sfpacker.exe"; Flags: replacesameversion ignoreversion; Check: Is64BitInstallMode
Source: "output\sfzguide.txt"; DestDir: "{syswow64}\keppydrv"; DestName: "sfzguide.txt"; Flags: replacesameversion ignoreversion; Check: Is64BitInstallMode
; 32-bit OS
Source: "external_packages\lib\bass.dll"; DestDir: "{sys}\keppydrv"; DestName: "bass.dll"; Flags: replacesameversion ignoreversion; Check: not Is64BitInstallMode
Source: "external_packages\lib\bass_mpc.dll"; DestDir: "{sys}\keppydrv"; DestName: "bass_mpc.dll"; Flags: replacesameversion ignoreversion; Check: not Is64BitInstallMode
Source: "external_packages\lib\bassenc.dll"; DestDir: "{sys}\keppydrv"; DestName: "bassenc.dll"; Flags: replacesameversion ignoreversion; Check: not Is64BitInstallMode
Source: "external_packages\lib\bassflac.dll"; DestDir: "{sys}\keppydrv"; DestName: "bassflac.dll"; Flags: replacesameversion ignoreversion; Check: not Is64BitInstallMode
Source: "external_packages\lib\bassmidi.dll"; DestDir: "{sys}\keppydrv"; DestName: "bassmidi.dll"; Flags: replacesameversion ignoreversion; Check: not Is64BitInstallMode
Source: "external_packages\lib\bassopus.dll"; DestDir: "{sys}\keppydrv"; DestName: "bassopus.dll"; Flags: replacesameversion ignoreversion; Check: not Is64BitInstallMode
Source: "external_packages\lib\basswv.dll"; DestDir: "{sys}\keppydrv"; DestName: "basswv.dll"; Flags: replacesameversion ignoreversion; Check: not Is64BitInstallMode
Source: "output\KeppyDriverConfigurator.exe"; DestDir: "{sys}\keppydrv"; DestName: "KeppyDriverConfigurator.exe"; Flags: replacesameversion ignoreversion; Check: not Is64BitInstallMode
Source: "output\keppydrv.dll"; DestDir: "{sys}\keppydrv"; DestName: "keppydrv.dll"; Flags: replacesameversion ignoreversion; Check: not Is64BitInstallMode
Source: "output\midioutsetter32.exe"; DestDir: "{sys}\keppydrv"; DestName: "midioutsetter32.exe"; Flags: replacesameversion ignoreversion; Check: not Is64BitInstallMode
Source: "output\sfpacker.exe"; DestDir: "{sys}\keppydrv"; DestName: "sfpacker.exe"; Flags: replacesameversion ignoreversion; Check: not Is64BitInstallMode
Source: "output\sfzguide.txt"; DestDir: "{sys}\keppydrv"; DestName: "sfzguide.txt"; Flags: replacesameversion ignoreversion; Check: not Is64BitInstallMode
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
Name: "{group}\Configure Keppy's Driver"; Filename: "{syswow64}\keppydrv\KeppyDriverConfigurator.exe"; WorkingDir: "{app}"; Check: Is64BitInstallMode
Name: "{group}\Change advanced settings"; Filename: "{syswow64}\keppydrv\KeppyDriverConfigurator.exe"; Parameters: "-advancedtab"; WorkingDir: "{app}"; Check: Is64BitInstallMode
Name: "{group}\Soundfont packer by Kode54"; Filename: "{syswow64}\keppydrv\sfpacker.exe"; WorkingDir: "{app}"; Check: Is64BitInstallMode
; 32-bit OS
Name: "{group}\Configure Keppy's Driver"; Filename: "{sys}\keppydrv\KeppyDriverConfigurator.exe"; WorkingDir: "{app}"; Check: not Is64BitInstallMode
Name: "{group}\Change advanced settings"; Filename: "{sys}\keppydrv\KeppyDriverConfigurator.exe"; Parameters: "-advancedtab"; WorkingDir: "{app}"; Check: not Is64BitInstallMode
Name: "{group}\Soundfont packer by Kode54"; Filename: "{sys}\keppydrv\sfpacker.exe"; WorkingDir: "{app}"; Check: Is64BitInstallMode   
; Generic for all the OSes
Name: "{group}\Uninstall the driver"; Filename: "{uninstallexe}"

[Registry]
; Normal settings
Root: "HKCU"; Subkey: "Software\Keppy's Driver"; ValueType: dword; ValueName: "currentcpuusage0"; ValueData: "0"; Flags: uninsdeletekey; Permissions: everyone-full
Root: "HKCU"; Subkey: "Software\Keppy's Driver"; ValueType: dword; ValueName: "currentvoices0"; ValueData: "0"; Flags: uninsdeletekey; Permissions: everyone-full
Root: "HKCU"; Subkey: "Software\Keppy's Driver"; ValueType: dword; ValueName: "int"; ValueData: "0"; Flags: uninsdeletekey; Permissions: everyone-full
Root: "HKCU"; Subkey: "Software\Keppy's Driver\Settings"; ValueType: dword; ValueName: "buflen"; ValueData: "40"; Flags: uninsdeletekey createvalueifdoesntexist
Root: "HKCU"; Subkey: "Software\Keppy's Driver\Settings"; ValueType: dword; ValueName: "cpu"; ValueData: "75"; Flags: uninsdeletekey createvalueifdoesntexist
Root: "HKCU"; Subkey: "Software\Keppy's Driver\Settings"; ValueType: dword; ValueName: "defaultsflist"; ValueData: "1"; Flags: uninsdeletekey createvalueifdoesntexist
Root: "HKCU"; Subkey: "Software\Keppy's Driver\Settings"; ValueType: dword; ValueName: "encmode"; ValueData: "0"; Flags: createvalueifdoesntexist uninsdeletekey
Root: "HKCU"; Subkey: "Software\Keppy's Driver\Settings"; ValueType: dword; ValueName: "firstrun"; ValueData: "1"; Flags: createvalueifdoesntexist uninsdeletekey
Root: "HKCU"; Subkey: "Software\Keppy's Driver\Settings"; ValueType: dword; ValueName: "frequency"; ValueData: "44100"; Flags: createvalueifdoesntexist uninsdeletekey
Root: "HKCU"; Subkey: "Software\Keppy's Driver\Settings"; ValueType: dword; ValueName: "nodx8"; ValueData: "1"; Flags: createvalueifdoesntexist uninsdeletekey
Root: "HKCU"; Subkey: "Software\Keppy's Driver\Settings"; ValueType: dword; ValueName: "nofloat"; ValueData: "1"; Flags: createvalueifdoesntexist uninsdeletekey
Root: "HKCU"; Subkey: "Software\Keppy's Driver\Settings"; ValueType: dword; ValueName: "nofx"; ValueData: "1"; Flags: createvalueifdoesntexist uninsdeletekey
Root: "HKCU"; Subkey: "Software\Keppy's Driver\Settings"; ValueType: dword; ValueName: "noteoff"; ValueData: "0"; Flags: createvalueifdoesntexist uninsdeletekey
Root: "HKCU"; Subkey: "Software\Keppy's Driver\Settings"; ValueType: dword; ValueName: "polyphony"; ValueData: "512"; Flags: createvalueifdoesntexist uninsdeletekey
Root: "HKCU"; Subkey: "Software\Keppy's Driver\Settings"; ValueType: dword; ValueName: "preload"; ValueData: "1"; Flags: createvalueifdoesntexist uninsdeletekey
Root: "HKCU"; Subkey: "Software\Keppy's Driver\Settings"; ValueType: dword; ValueName: "sinc"; ValueData: "0"; Flags: createvalueifdoesntexist uninsdeletekey
Root: "HKCU"; Subkey: "Software\Keppy's Driver\Settings"; ValueType: dword; ValueName: "sndbfvalue"; ValueData: "100"; Flags: createvalueifdoesntexist uninsdeletekey
Root: "HKCU"; Subkey: "Software\Keppy's Driver\Settings"; ValueType: dword; ValueName: "softwaremode"; ValueData: "1"; Flags: createvalueifdoesntexist uninsdeletekey
Root: "HKCU"; Subkey: "Software\Keppy's Driver\Settings"; ValueType: dword; ValueName: "sysresetignore"; ValueData: "0"; Flags: createvalueifdoesntexist uninsdeletekey
Root: "HKCU"; Subkey: "Software\Keppy's Driver\Settings"; ValueType: dword; ValueName: "tracks"; ValueData: "16"; Flags: createvalueifdoesntexist uninsdeletekey
Root: "HKCU"; Subkey: "Software\Keppy's Driver\Settings"; ValueType: dword; ValueName: "vmsemu"; ValueData: "0"; Flags: createvalueifdoesntexist uninsdeletekey
Root: "HKCU"; Subkey: "Software\Keppy's Driver\Settings"; ValueType: dword; ValueName: "volume"; ValueData: "10000"; Flags: createvalueifdoesntexist uninsdeletekey
Root: "HKCU"; Subkey: "Software\Keppy's Driver\Settings"; ValueType: dword; ValueName: "xaudiodisabled"; ValueData: "0"; Flags: createvalueifdoesntexist uninsdeletekey
;3D effects
Root: "HKCU"; Subkey: "Software\Keppy's Driver\Effects"; ValueType: dword; ValueName: "chorusfx"; ValueData: "0"; Flags: uninsdeletekey deletekey dontcreatekey
Root: "HKCU"; Subkey: "Software\Keppy's Driver\Effects"; ValueType: dword; ValueName: "chorusfxnum"; ValueData: "0"; Flags: uninsdeletekey deletekey dontcreatekey
Root: "HKCU"; Subkey: "Software\Keppy's Driver\Effects"; ValueType: dword; ValueName: "chorusglobalvalue"; ValueData: "0"; Flags: uninsdeletekey deletekey dontcreatekey
Root: "HKCU"; Subkey: "Software\Keppy's Driver\Effects"; ValueType: dword; ValueName: "compressorfx"; ValueData: "0"; Flags: uninsdeletekey deletekey dontcreatekey
Root: "HKCU"; Subkey: "Software\Keppy's Driver\Effects"; ValueType: dword; ValueName: "compressorfxnum"; ValueData: "0"; Flags: uninsdeletekey deletekey dontcreatekey
Root: "HKCU"; Subkey: "Software\Keppy's Driver\Effects"; ValueType: dword; ValueName: "distortionfx"; ValueData: "0"; Flags: uninsdeletekey deletekey dontcreatekey
Root: "HKCU"; Subkey: "Software\Keppy's Driver\Effects"; ValueType: dword; ValueName: "distortionfxnum"; ValueData: "0"; Flags: uninsdeletekey deletekey dontcreatekey
Root: "HKCU"; Subkey: "Software\Keppy's Driver\Effects"; ValueType: dword; ValueName: "echofx"; ValueData: "0"; Flags: uninsdeletekey deletekey dontcreatekey
Root: "HKCU"; Subkey: "Software\Keppy's Driver\Effects"; ValueType: dword; ValueName: "echofxnum"; ValueData: "0"; Flags: uninsdeletekey deletekey dontcreatekey
Root: "HKCU"; Subkey: "Software\Keppy's Driver\Effects"; ValueType: dword; ValueName: "flangerfx"; ValueData: "0"; Flags: uninsdeletekey deletekey dontcreatekey
Root: "HKCU"; Subkey: "Software\Keppy's Driver\Effects"; ValueType: dword; ValueName: "flangerfxnum"; ValueData: "0"; Flags: uninsdeletekey deletekey dontcreatekey
Root: "HKCU"; Subkey: "Software\Keppy's Driver\Effects"; ValueType: dword; ValueName: "garglefx"; ValueData: "0"; Flags: uninsdeletekey deletekey dontcreatekey
Root: "HKCU"; Subkey: "Software\Keppy's Driver\Effects"; ValueType: dword; ValueName: "garglefxnum"; ValueData: "0"; Flags: uninsdeletekey deletekey dontcreatekey
Root: "HKCU"; Subkey: "Software\Keppy's Driver\Effects"; ValueType: dword; ValueName: "reverbfx"; ValueData: "0"; Flags: uninsdeletekey deletekey dontcreatekey
Root: "HKCU"; Subkey: "Software\Keppy's Driver\Effects"; ValueType: dword; ValueName: "reverbfxnum"; ValueData: "0"; Flags: uninsdeletekey deletekey dontcreatekey
Root: "HKCU"; Subkey: "Software\Keppy's Driver\Effects"; ValueType: dword; ValueName: "reverbglobalvalue"; ValueData: "0"; Flags: uninsdeletekey deletekey dontcreatekey
Root: "HKCU"; Subkey: "Software\Keppy's Driver\Effects"; ValueType: dword; ValueName: "sittingfx"; ValueData: "0"; Flags: uninsdeletekey deletekey dontcreatekey
Root: "HKCU"; Subkey: "Software\Keppy's Driver\Effects"; ValueType: dword; ValueName: "sittingfxnum"; ValueData: "0"; Flags: uninsdeletekey deletekey dontcreatekey
Root: "HKCU"; Subkey: "Software\Keppy's Driver\Effects"; ValueType: dword; ValueName: "transpose"; ValueData: "0"; Flags: uninsdeletekey deletekey dontcreatekey
; 64-bit OS
Root: "HKLM"; Subkey: "Software\Microsoft\Windows NT\CurrentVersion\Drivers32"; ValueType: string; ValueName: "midi9"; ValueData: "keppydrv\keppydrv.dll"; Flags: uninsdeletevalue dontcreatekey; Check: Is64BitInstallMode
Root: "HKLM"; Subkey: "Software\Wow6432Node\Microsoft\Windows NT\CurrentVersion\Drivers32"; ValueType: string; ValueName: "midi9"; ValueData: "keppydrv\keppydrv.dll"; Flags: uninsdeletevalue dontcreatekey; Check: Is64BitInstallMode
; 32-bit OS
Root: "HKLM"; Subkey: "Software\Microsoft\Windows NT\CurrentVersion\Drivers32"; ValueType: string; ValueName: "midi9"; ValueData: "keppydrv\keppydrv.dll"; Flags: uninsdeletevalue dontcreatekey; Check: not Is64BitInstallMode

[InstallDelete]
Type: files; Name: "{syswow64}\keppydrv\KeppyDriverConfigurator.exe"; Check: Is64BitInstallMode
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
Type: files; Name: "{sys}\keppydrv\KeppyDriverConfigurator.exe"; Check: Is64BitInstallMode
Type: files; Name: "{sys}\keppydrv\KeppyDriverConfigurator.exe"; Check: Is64BitInstallMode
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
Type: files; Name: "{sys}\keppydrv\KeppyDriverConfigurator.exe"; Check: Is64BitInstallMode
Type: files; Name: "{sys}\keppydrv\KeppyDriverConfigurator.exe"; Check: Is64BitInstallMode
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
Filename: "{syswow64}\keppydrv\KeppyDriverConfigurator.exe"; Flags: postinstall runascurrentuser nowait; Description: "Run the configurator, to set up soundfonts"; StatusMsg: "Run the configurator, to set up soundfonts"; Check: Is64BitInstallMode
Filename: "{sys}\keppydrv\KeppyDriverConfigurator.exe"; Flags: postinstall runascurrentuser nowait; Description: "Run the configurator, to set up soundfonts"; StatusMsg: "Run the configurator, to set up soundfonts"; Check: not Is64BitInstallMode
Filename: "http://keppystudios.com/keppy-s-steinway-piano.html"; Flags: shellexec postinstall runasoriginaluser nowait unchecked; Description: "Download Keppy Steinway Piano"; StatusMsg: "Download Keppy Steinway Piano";
Filename: "http://keppystudios.com/"; Flags: shellexec postinstall runasoriginaluser nowait unchecked; Description: "Visit Keppy Studios"; StatusMsg: "Visit Keppy Studios";
Filename: "http://frozensnowy.com/"; Flags: shellexec postinstall runasoriginaluser nowait unchecked; Description: "Visit Frozen Snow Productions"; StatusMsg: "Visit Frozen Snow Productions";
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
