[Setup]
AllowCancelDuringInstall=False
AllowNoIcons=True
AppContact=kaleidonkep99@outlook.com
AppCopyright=Copyright (c) 2011-2016 Brad Miller, Chris Moeller and Riccardo Loi. All rights reserved.
AppId={{950DEC78-2D12-4917-BE69-CB04FE84B21F}
AppName=Keppy's Driver 3.1
AppPublisher=Keppy Studios
AppPublisherURL=http://keppystudios.com
AppSupportPhone=+393511888475
AppSupportURL=mailto:kaleidonkep99@outlook.com
AppUpdatesURL=https://github.com/KaleidonKep99/Keppy-s-MIDI-Driver/releases
AppVersion=3.1
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
OutputDir="C:\Users\KaleidonKep99\Desktop"
SetupIconFile=midiicon.ico
ShowLanguageDialog=no
ShowTasksTreeLines=True
ShowUndisplayableLanguages=True
SolidCompression=yes
TimeStampsInUTC=True
UninstallDisplayIcon={sys}\keppydrv\keppydrvcfg.exe
UninstallDisplayName=Keppy's Driver 3.1
UninstallDisplaySize=5
UninstallFilesDir={sys}
VersionInfoCompany=Keppy Studios
VersionInfoCopyright=Copyright (c) 2011-2016 Brad Miller, Chris Moeller and Riccardo Loi. All rights reserved.
VersionInfoDescription=User-mode MIDI driver for Windows Vista and newer
VersionInfoProductName=Keppy's Driver 3.1
VersionInfoProductTextVersion=3.1.2.0
VersionInfoProductVersion=3.1
VersionInfoTextVersion=User-mode MIDI driver for Windows Vista and newer
VersionInfoVersion=3.1.2.0

[Files]
; 64-bit OS
Source: "output\64\bass.dll"; DestDir: "{sys}\keppydrv"; DestName: "bass.dll"; Flags: replacesameversion ignoreversion; MinVersion: 0,5.01sp3; Check: Is64BitInstallMode
Source: "output\64\bass_mpc.dll"; DestDir: "{sys}\keppydrv"; DestName: "bass_mpc.dll"; Flags: replacesameversion ignoreversion; Check: Is64BitInstallMode
Source: "output\64\bassflac.dll"; DestDir: "{sys}\keppydrv"; DestName: "bassflac.dll"; Flags: replacesameversion ignoreversion; Check: Is64BitInstallMode
Source: "output\64\bassmidi.dll"; DestDir: "{sys}\keppydrv"; DestName: "bassmidi.dll"; Flags: replacesameversion ignoreversion; Check: Is64BitInstallMode
Source: "output\64\bassenc.dll"; DestDir: "{sys}\keppydrv"; DestName: "bassenc.dll"; Flags: replacesameversion ignoreversion; Check: Is64BitInstallMode
Source: "output\64\basswasapi.dll"; DestDir: "{sys}\keppydrv"; DestName: "basswasapi.dll"; Flags: replacesameversion ignoreversion; Check: Is64BitInstallMode
Source: "output\64\bassopus.dll"; DestDir: "{sys}\keppydrv"; DestName: "bassopus.dll"; Flags: replacesameversion ignoreversion; Check: Is64BitInstallMode
Source: "output\64\basswv.dll"; DestDir: "{sys}\keppydrv"; DestName: "basswv.dll"; Flags: replacesameversion ignoreversion; Check: Is64BitInstallMode
Source: "output\64\keppydrv.dll"; DestDir: "{sys}\keppydrv"; DestName: "keppydrv.dll"; Flags: replacesameversion ignoreversion; MinVersion: 0,5.01sp3; Check: Is64BitInstallMode
Source: "output\bass.dll"; DestDir: "{syswow64}\keppydrv"; DestName: "bass.dll"; Flags: replacesameversion ignoreversion; MinVersion: 0,5.01sp3; Check: Is64BitInstallMode
Source: "output\bass_mpc.dll"; DestDir: "{syswow64}\keppydrv"; DestName: "bass_mpc.dll"; Flags: replacesameversion ignoreversion; Check: Is64BitInstallMode
Source: "output\bassflac.dll"; DestDir: "{syswow64}\keppydrv"; DestName: "bassflac.dll"; Flags: replacesameversion ignoreversion; Check: Is64BitInstallMode
Source: "output\bassmidi.dll"; DestDir: "{syswow64}\keppydrv"; DestName: "bassmidi.dll"; Flags: replacesameversion ignoreversion; Check: Is64BitInstallMode
Source: "output\bassenc.dll"; DestDir: "{syswow64}\keppydrv"; DestName: "bassenc.dll"; Flags: replacesameversion ignoreversion; Check: Is64BitInstallMode
Source: "output\basswasapi.dll"; DestDir: "{syswow64}\keppydrv"; DestName: "basswasapi.dll"; Flags: replacesameversion ignoreversion; Check: Is64BitInstallMode
Source: "output\bassopus.dll"; DestDir: "{syswow64}\keppydrv"; DestName: "bassopus.dll"; Flags: replacesameversion ignoreversion; Check: Is64BitInstallMode
Source: "output\basswv.dll"; DestDir: "{syswow64}\keppydrv"; DestName: "basswv.dll"; Flags: replacesameversion ignoreversion; Check: Is64BitInstallMode
Source: "output\keppydrv.dll"; DestDir: "{syswow64}\keppydrv"; DestName: "keppydrv.dll"; Flags: replacesameversion ignoreversion; MinVersion: 0,5.01sp3; Check: Is64BitInstallMode
Source: "output\KeppyDriverConfigurator.exe"; DestDir: "{syswow64}\keppydrv"; DestName: "KeppyDriverConfigurator.exe"; Flags: replacesameversion ignoreversion; MinVersion: 0,5.01sp3; Check: Is64BitInstallMode
Source: "output\sfpacker.exe"; DestDir: "{syswow64}\keppydrv"; DestName: "sfpacker.exe"; Flags: replacesameversion ignoreversion; MinVersion: 0,5.01sp3; Check: Is64BitInstallMode
Source: "output\sfzguide.txt"; DestDir: "{syswow64}\keppydrv"; DestName: "sfzguide.txt"; Flags: replacesameversion ignoreversion; MinVersion: 0,5.01sp3; Check: Is64BitInstallMode
; 32-bit OS
Source: "output\bass.dll"; DestDir: "{sys}\keppydrv"; DestName: "bass.dll"; Flags: replacesameversion ignoreversion; MinVersion: 0,5.01sp3; Check: not Is64BitInstallMode
Source: "output\bass_mpc.dll"; DestDir: "{sys}\keppydrv"; DestName: "bass_mpc.dll"; Flags: replacesameversion ignoreversion; MinVersion: 0,5.01sp3; Check: not Is64BitInstallMode
Source: "output\bassflac.dll"; DestDir: "{sys}\keppydrv"; DestName: "bassflac.dll"; Flags: replacesameversion ignoreversion; MinVersion: 0,5.01sp3; Check: not Is64BitInstallMode
Source: "output\bassmidi.dll"; DestDir: "{sys}\keppydrv"; DestName: "bassmidi.dll"; Flags: replacesameversion ignoreversion; MinVersion: 0,5.01sp3; Check: not Is64BitInstallMode
Source: "output\bassenc.dll"; DestDir: "{sys}\keppydrv"; DestName: "bassenc.dll"; Flags: replacesameversion ignoreversion; MinVersion: 0,5.01sp3; Check: not Is64BitInstallMode
Source: "output\basswasapi.dll"; DestDir: "{sys}\keppydrv"; DestName: "basswasapi.dll"; Flags: replacesameversion ignoreversion; MinVersion: 0,5.01sp3; Check: not Is64BitInstallMode
Source: "output\bassopus.dll"; DestDir: "{sys}\keppydrv"; DestName: "bassopus.dll"; Flags: replacesameversion ignoreversion; MinVersion: 0,5.01sp3; Check: not Is64BitInstallMode
Source: "output\basswv.dll"; DestDir: "{sys}\keppydrv"; DestName: "basswv.dll"; Flags: replacesameversion ignoreversion; MinVersion: 0,5.01sp3; Check: not Is64BitInstallMode
Source: "output\keppydrv.dll"; DestDir: "{sys}\keppydrv"; DestName: "keppydrv.dll"; Flags: replacesameversion ignoreversion; MinVersion: 0,5.01sp3; Check: not Is64BitInstallMode
Source: "output\KeppyDriverConfigurator.exe"; DestDir: "{sys}\keppydrv"; DestName: "KeppyDriverConfigurator.exe"; Flags: replacesameversion ignoreversion; MinVersion: 0,5.01sp3; Check: not Is64BitInstallMode
Source: "output\sfpacker.exe"; DestDir: "{sys}\keppydrv"; DestName: "sfpacker.exe"; Flags: replacesameversion ignoreversion; MinVersion: 0,5.01sp3; Check: not Is64BitInstallMode
Source: "output\sfzguide.txt"; DestDir: "{sys}\keppydrv"; DestName: "sfzguide.txt"; Flags: replacesameversion ignoreversion; MinVersion: 0,5.01sp3; Check: not Is64BitInstallMode
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
Name: "{group}\Soundfont packer by Kode54"; Filename: "{syswow64}\keppydrv\sfpacker.exe"; WorkingDir: "{app}"; Check: Is64BitInstallMode
; 32-bit OS
Name: "{group}\Configure Keppy's Driver"; Filename: "{sys}\keppydrv\KeppyDriverConfigurator.exe"; WorkingDir: "{app}"; Check: not Is64BitInstallMode
Name: "{group}\Soundfont packer by Kode54"; Filename: "{sys}\keppydrv\sfpacker.exe"; WorkingDir: "{app}"; Check: Is64BitInstallMode   
; Generic for all the OSes
Name: "{group}\Uninstall the driver"; Filename: "{uninstallexe}"

[Registry]
; Normal settings
Root: "HKCU"; Subkey: "Software\Keppy's Driver"; ValueType: dword; ValueName: "int"; ValueData: "0"; Flags: uninsdeletekey; Permissions: everyone-full
Root: "HKCU"; Subkey: "Software\Keppy's Driver"; ValueType: dword; ValueName: "currentvoices0"; ValueData: "0"; Flags: uninsdeletekey; Permissions: everyone-full
Root: "HKCU"; Subkey: "Software\Keppy's Driver"; ValueType: dword; ValueName: "currentcpuusage0"; ValueData: "0"; Flags: uninsdeletekey; Permissions: everyone-full
Root: "HKCU"; Subkey: "Software\Keppy's Driver\Settings"; ValueType: dword; ValueName: "volume"; ValueData: "10000"; Flags: createvalueifdoesntexist uninsdeletekey
Root: "HKCU"; Subkey: "Software\Keppy's Driver\Settings"; ValueType: dword; ValueName: "buflen"; ValueData: "40"; Flags: uninsdeletekey createvalueifdoesntexist
Root: "HKCU"; Subkey: "Software\Keppy's Driver\Settings"; ValueType: dword; ValueName: "preload"; ValueData: "1"; Flags: createvalueifdoesntexist uninsdeletekey
Root: "HKCU"; Subkey: "Software\Keppy's Driver\Settings"; ValueType: dword; ValueName: "cpu"; ValueData: "75"; Flags: uninsdeletekey createvalueifdoesntexist
Root: "HKCU"; Subkey: "Software\Keppy's Driver\Settings"; ValueType: dword; ValueName: "nofloat"; ValueData: "1"; Flags: createvalueifdoesntexist uninsdeletekey
Root: "HKCU"; Subkey: "Software\Keppy's Driver\Settings"; ValueType: dword; ValueName: "nofx"; ValueData: "1"; Flags: createvalueifdoesntexist uninsdeletekey
Root: "HKCU"; Subkey: "Software\Keppy's Driver\Settings"; ValueType: dword; ValueName: "nodx8"; ValueData: "1"; Flags: createvalueifdoesntexist uninsdeletekey
Root: "HKCU"; Subkey: "Software\Keppy's Driver\Settings"; ValueType: dword; ValueName: "noteoff"; ValueData: "0"; Flags: createvalueifdoesntexist uninsdeletekey
Root: "HKCU"; Subkey: "Software\Keppy's Driver\Settings"; ValueType: dword; ValueName: "sinc"; ValueData: "0"; Flags: createvalueifdoesntexist uninsdeletekey
Root: "HKCU"; Subkey: "Software\Keppy's Driver\Settings"; ValueType: dword; ValueName: "softwaremode"; ValueData: "1"; Flags: createvalueifdoesntexist uninsdeletekey
Root: "HKCU"; Subkey: "Software\Keppy's Driver\Settings"; ValueType: dword; ValueName: "tracks"; ValueData: "16"; Flags: createvalueifdoesntexist uninsdeletekey
Root: "HKCU"; Subkey: "Software\Keppy's Driver\Settings"; ValueType: dword; ValueName: "sysresetignore"; ValueData: "0"; Flags: createvalueifdoesntexist uninsdeletekey
Root: "HKCU"; Subkey: "Software\Keppy's Driver\Settings"; ValueType: dword; ValueName: "encmode"; ValueData: "0"; Flags: createvalueifdoesntexist uninsdeletekey
Root: "HKCU"; Subkey: "Software\Keppy's Driver\Settings"; ValueType: dword; ValueName: "polyphony"; ValueData: "512"; Flags: createvalueifdoesntexist uninsdeletekey
Root: "HKCU"; Subkey: "Software\Keppy's Driver\Settings"; ValueType: dword; ValueName: "frequency"; ValueData: "44100"; Flags: createvalueifdoesntexist uninsdeletekey
Root: "HKCU"; Subkey: "Software\Keppy's Driver\Settings"; ValueType: dword; ValueName: "xaudiodisabled"; ValueData: "0"; Flags: createvalueifdoesntexist uninsdeletekey
Root: "HKCU"; Subkey: "Software\Keppy's Driver\Settings"; ValueType: dword; ValueName: "firstrun"; ValueData: "1"; Flags: createvalueifdoesntexist uninsdeletekey
Root: "HKCU"; Subkey: "Software\Keppy's Driver\Settings"; ValueType: dword; ValueName: "3d"; ValueData: "1"; Flags: createvalueifdoesntexist uninsdeletekey
;3D effects
Root: "HKCU"; Subkey: "Software\Keppy's Driver\Effects"; ValueType: dword; ValueName: "chorusfx"; ValueData: "0"; Flags: uninsdeletekey deletekey dontcreatekey
Root: "HKCU"; Subkey: "Software\Keppy's Driver\Effects"; ValueType: dword; ValueName: "chorusfxnum"; ValueData: "2"; Flags: uninsdeletekey deletekey dontcreatekey
Root: "HKCU"; Subkey: "Software\Keppy's Driver\Effects"; ValueType: dword; ValueName: "compressorfx"; ValueData: "0"; Flags: uninsdeletekey deletekey dontcreatekey
Root: "HKCU"; Subkey: "Software\Keppy's Driver\Effects"; ValueType: dword; ValueName: "compressorfxnum"; ValueData: "4"; Flags: uninsdeletekey deletekey dontcreatekey
Root: "HKCU"; Subkey: "Software\Keppy's Driver\Effects"; ValueType: dword; ValueName: "distortionfx"; ValueData: "0"; Flags: uninsdeletekey deletekey dontcreatekey
Root: "HKCU"; Subkey: "Software\Keppy's Driver\Effects"; ValueType: dword; ValueName: "distortionfxnum"; ValueData: "6"; Flags: uninsdeletekey deletekey dontcreatekey
Root: "HKCU"; Subkey: "Software\Keppy's Driver\Effects"; ValueType: dword; ValueName: "echofx"; ValueData: "0"; Flags: uninsdeletekey deletekey dontcreatekey
Root: "HKCU"; Subkey: "Software\Keppy's Driver\Effects"; ValueType: dword; ValueName: "echofxnum"; ValueData: "7"; Flags: uninsdeletekey deletekey dontcreatekey
Root: "HKCU"; Subkey: "Software\Keppy's Driver\Effects"; ValueType: dword; ValueName: "flangerfx"; ValueData: "0"; Flags: uninsdeletekey deletekey dontcreatekey
Root: "HKCU"; Subkey: "Software\Keppy's Driver\Effects"; ValueType: dword; ValueName: "flangerfxnum"; ValueData: "3"; Flags: uninsdeletekey deletekey dontcreatekey
Root: "HKCU"; Subkey: "Software\Keppy's Driver\Effects"; ValueType: dword; ValueName: "garglefx"; ValueData: "0"; Flags: uninsdeletekey deletekey dontcreatekey
Root: "HKCU"; Subkey: "Software\Keppy's Driver\Effects"; ValueType: dword; ValueName: "garglefxnum"; ValueData: "5"; Flags: uninsdeletekey deletekey dontcreatekey
Root: "HKCU"; Subkey: "Software\Keppy's Driver\Effects"; ValueType: dword; ValueName: "reverbfx"; ValueData: "0"; Flags: uninsdeletekey deletekey dontcreatekey
Root: "HKCU"; Subkey: "Software\Keppy's Driver\Effects"; ValueType: dword; ValueName: "reverbfxnum"; ValueData: "1"; Flags: uninsdeletekey deletekey dontcreatekey
Root: "HKCU"; Subkey: "Software\Keppy's Driver\Effects"; ValueType: dword; ValueName: "sittingfx"; ValueData: "0"; Flags: uninsdeletekey deletekey dontcreatekey
Root: "HKCU"; Subkey: "Software\Keppy's Driver\Effects"; ValueType: dword; ValueName: "sittingfxnum"; ValueData: "8"; Flags: uninsdeletekey deletekey dontcreatekey
Root: "HKCU"; Subkey: "Software\Keppy's Driver\Effects"; ValueType: dword; ValueName: "reverbglobalvalue"; ValueData: "0"; Flags: uninsdeletekey deletekey dontcreatekey
Root: "HKCU"; Subkey: "Software\Keppy's Driver\Effects"; ValueType: dword; ValueName: "chorusglobalvalue"; ValueData: "0"; Flags: uninsdeletekey deletekey dontcreatekey
Root: "HKCU"; Subkey: "Software\Keppy's Driver\Effects"; ValueType: dword; ValueName: "transpose"; ValueData: "100"; Flags: uninsdeletekey deletekey dontcreatekey
; 64-bit OS
Root: "HKLM"; Subkey: "Software\Microsoft\Windows NT\CurrentVersion\Drivers32"; ValueType: string; ValueName: "midi9"; ValueData: "keppydrv\keppydrv.dll"; Flags: uninsdeletevalue dontcreatekey; Check: Is64BitInstallMode
Root: "HKLM"; Subkey: "Software\Wow6432Node\Microsoft\Windows NT\CurrentVersion\Drivers32"; ValueType: string; ValueName: "midi9"; ValueData: "keppydrv\keppydrv.dll"; Flags: uninsdeletevalue dontcreatekey; Check: Is64BitInstallMode
; 32-bit OS
Root: "HKLM"; Subkey: "Software\Microsoft\Windows NT\CurrentVersion\Drivers32"; ValueType: string; ValueName: "midi9"; ValueData: "keppydrv\keppydrv.dll"; Flags: uninsdeletevalue dontcreatekey; Check: not Is64BitInstallMode

[InstallDelete]
Type: files; Name: "{sys}\keppydrv\bass.dll"; Check: Is64BitInstallMode
Type: files; Name: "{sys}\keppydrv\bassmidi.dll"; Check: Is64BitInstallMode
Type: files; Name: "{sys}\keppydrv\bassenc.dll"; Check: Is64BitInstallMode
Type: files; Name: "{sys}\keppydrv\basswasapi.dll"; Check: Is64BitInstallMode
Type: files; Name: "{sys}\keppydrv\bassopus.dll"; Check: Is64BitInstallMode
Type: files; Name: "{sys}\keppydrv\bassflac.dll"; Check: Is64BitInstallMode
Type: files; Name: "{sys}\keppydrv\bass_mpc.dll"; Check: Is64BitInstallMode
Type: files; Name: "{sys}\keppydrv\basswv.dll"; Check: Is64BitInstallMode
Type: files; Name: "{sys}\keppydrv\keppydrvcfg.exe"; Check: Is64BitInstallMode
Type: files; Name: "{sys}\keppydrv\KeppyDriverConfigurator.exe"; Check: Is64BitInstallMode
Type: files; Name: "{sys}\keppydrv\keppydrv.dll"; Check: Is64BitInstallMode
Type: files; Name: "{syswow64}\keppydrv\bass.dll"; Check: Is64BitInstallMode
Type: files; Name: "{syswow64}\keppydrv\bassmidi.dll"; Check: Is64BitInstallMode
Type: files; Name: "{syswow64}\keppydrv\bassenc.dll"; Check: Is64BitInstallMode
Type: files; Name: "{syswow64}\keppydrv\basswasapi.dll"; Check: Is64BitInstallMode
Type: files; Name: "{syswow64}\keppydrv\bassopus.dll"; Check: Is64BitInstallMode
Type: files; Name: "{syswow64}\keppydrv\bassflac.dll"; Check: Is64BitInstallMode
Type: files; Name: "{syswow64}\keppydrv\bass_mpc.dll"; Check: Is64BitInstallMode
Type: files; Name: "{syswow64}\keppydrv\basswv.dll"; Check: Is64BitInstallMode
Type: files; Name: "{syswow64}\keppydrv\keppydrv.dll"; Check: Is64BitInstallMode
Type: files; Name: "{syswow64}\keppydrv\keppydrvcfg.exe"; Check: Is64BitInstallMode
Type: files; Name: "{syswow64}\keppydrv\KeppyDriverConfigurator.exe"; Check: Is64BitInstallMode
Type: files; Name: "{syswow64}\keppydrv\sfpacker.exe"; Check: Is64BitInstallMode
Type: files; Name: "{sys}\keppydrv\bass.dll"; Check: not Is64BitInstallMode
Type: files; Name: "{sys}\keppydrv\bassmidi.dll"; Check: not Is64BitInstallMode
Type: files; Name: "{sys}\keppydrv\bassenc.dll"; Check: not Is64BitInstallMode
Type: files; Name: "{sys}\keppydrv\basswasapi.dll"; Check: not Is64BitInstallMode
Type: files; Name: "{sys}\keppydrv\bassopus.dll"; Check: not Is64BitInstallMode
Type: files; Name: "{sys}\keppydrv\bassflac.dll"; Check: not Is64BitInstallMode
Type: files; Name: "{sys}\keppydrv\bass_mpc.dll"; Check: not Is64BitInstallMode
Type: files; Name: "{sys}\keppydrv\basswv.dll"; Check: not Is64BitInstallMode
Type: files; Name: "{sys}\keppydrv\keppydrv.dll"; Check: not Is64BitInstallMode
Type: files; Name: "{sys}\keppydrv\keppydrvcfg.exe"; Check: not Is64BitInstallMode
Type: files; Name: "{sys}\keppydrv\KeppyDriverConfigurator.exe"; Check: Is64BitInstallMode
Type: files; Name: "{sys}\keppydrv\sfpacker.exe"; Check: not Is64BitInstallMode

[UninstallDelete]
Type: files; Name: "{sys}\keppydrv\bass.dll"; Check: Is64BitInstallMode
Type: files; Name: "{sys}\keppydrv\bassmidi.dll"; Check: Is64BitInstallMode
Type: files; Name: "{sys}\keppydrv\bassenc.dll"; Check: Is64BitInstallMode
Type: files; Name: "{sys}\keppydrv\basswasapi.dll"; Check: Is64BitInstallMode
Type: files; Name: "{sys}\keppydrv\bassopus.dll"; Check: Is64BitInstallMode
Type: files; Name: "{sys}\keppydrv\bassflac.dll"; Check: Is64BitInstallMode
Type: files; Name: "{sys}\keppydrv\bass_mpc.dll"; Check: Is64BitInstallMode
Type: files; Name: "{sys}\keppydrv\basswv.dll"; Check: Is64BitInstallMode
Type: files; Name: "{sys}\keppydrv\keppydrvcfg.exe"; Check: Is64BitInstallMode
Type: files; Name: "{sys}\keppydrv\KeppyDriverConfigurator.exe"; Check: Is64BitInstallMode
Type: files; Name: "{sys}\keppydrv\keppydrv.dll"; Check: Is64BitInstallMode
Type: files; Name: "{syswow64}\keppydrv\bass.dll"; Check: Is64BitInstallMode
Type: files; Name: "{syswow64}\keppydrv\bassmidi.dll"; Check: Is64BitInstallMode
Type: files; Name: "{syswow64}\keppydrv\bassenc.dll"; Check: Is64BitInstallMode
Type: files; Name: "{syswow64}\keppydrv\basswasapi.dll"; Check: Is64BitInstallMode
Type: files; Name: "{syswow64}\keppydrv\bassopus.dll"; Check: Is64BitInstallMode
Type: files; Name: "{syswow64}\keppydrv\bassflac.dll"; Check: Is64BitInstallMode
Type: files; Name: "{syswow64}\keppydrv\bass_mpc.dll"; Check: Is64BitInstallMode
Type: files; Name: "{syswow64}\keppydrv\basswv.dll"; Check: Is64BitInstallMode
Type: files; Name: "{syswow64}\keppydrv\keppydrv.dll"; Check: Is64BitInstallMode
Type: files; Name: "{syswow64}\keppydrv\keppydrvcfg.exe"; Check: Is64BitInstallMode
Type: files; Name: "{syswow64}\keppydrv\KeppyDriverConfigurator.exe"; Check: Is64BitInstallMode
Type: files; Name: "{syswow64}\keppydrv\sfpacker.exe"; Check: Is64BitInstallMode
Type: files; Name: "{sys}\keppydrv\bass.dll"; Check: not Is64BitInstallMode
Type: files; Name: "{sys}\keppydrv\bassmidi.dll"; Check: not Is64BitInstallMode
Type: files; Name: "{sys}\keppydrv\bassenc.dll"; Check: not Is64BitInstallMode
Type: files; Name: "{sys}\keppydrv\basswasapi.dll"; Check: not Is64BitInstallMode
Type: files; Name: "{sys}\keppydrv\bassopus.dll"; Check: not Is64BitInstallMode
Type: files; Name: "{sys}\keppydrv\bassflac.dll"; Check: not Is64BitInstallMode
Type: files; Name: "{sys}\keppydrv\bass_mpc.dll"; Check: not Is64BitInstallMode
Type: files; Name: "{sys}\keppydrv\basswv.dll"; Check: not Is64BitInstallMode
Type: files; Name: "{sys}\keppydrv\keppydrv.dll"; Check: not Is64BitInstallMode
Type: files; Name: "{sys}\keppydrv\keppydrvcfg.exe"; Check: not Is64BitInstallMode
Type: files; Name: "{sys}\keppydrv\KeppyDriverConfigurator.exe"; Check: Is64BitInstallMode
Type: files; Name: "{sys}\keppydrv\sfpacker.exe"; Check: not Is64BitInstallMode

[Run]
Filename: "{tmp}\dxwebsetup.exe"; Parameters: "/q"; Flags: waituntilterminated; Description: "DirectX installer"; StatusMsg: "Installing DirectX runtime (XAudio2)..."
Filename: "{syswow64}\keppydrv\KeppyDriverConfigurator.exe"; Flags: postinstall runascurrentuser nowait; Description: "Run the configurator, to set up soundfonts"; StatusMsg: "Run the configurator, to set up soundfonts"; Check: Is64BitInstallMode
Filename: "{sys}\keppydrv\KeppyDriverConfigurator.exe"; Flags: postinstall runascurrentuser nowait; Description: "Run the configurator, to set up soundfonts"; StatusMsg: "Run the configurator, to set up soundfonts"; Check: not Is64BitInstallMode

[Messages]
SetupAppTitle=Keppy's Driver Installer
UninstallAppFullTitle=Keppy's Driver Uninstaller
ErrorRestartingComputer=Unable to restart the computer. Please do this manually.
AboutSetupTitle=About the driver
CannotContinue=The installation cannot continue.%n%nClick Cancel to exit.
ExitSetupMessage=You didn't finished the installation. If you exit now, the driver will not be installed!%n%nYou may run the setup again at another time to complete the installation.%n%nAre you sure you want to exit?
FinishedLabel=The driver has been succesfully installed. Please configure your MIDI programs to allow them to use it, and add some soundfonts to the soundfonts lists!
FinishedLabelNoIcons=The driver has been succesfully installed.
WelcomeLabel1=Keppy's Driver Installation Wizard
WelcomeLabel2=The setup will install the driver on your computer.%n%nIt is recommended that you close all other applications before continuing.
ConfirmUninstall=Before uninstalling, be sure to set your MIDI programs to the default synth (Microsoft GS Wavetable Synth), to avoid crashes.%n%nPress Yes to continue, or no to abort the uninstallation.