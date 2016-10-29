#define use_ie6
#define use_dotnetfx40
#define use_wic
#define use_msiproduct
#define vc

#define ProductName "Keppy's Synthesizer"
#define Version '4.0.4.0'

[Setup]
AllowCancelDuringInstall=False
AppContact=kaleidonkep99@outlook.com
AppCopyright=Copyright (c) 2011-2017 Brad Miller, Chris Moeller and Riccardo Loi. All rights reserved.
AppId={{950DEC78-2D12-4917-BE69-CB04FE84B21F}
AppName=Keppy's Synthesizer
AppPublisher=KaleidonKep99
AppPublisherURL=https://github.com/KaleidonKep99/Keppy-s-Synthesizer
AppSupportPhone=+393511888475
AppSupportURL=https://github.com/KaleidonKep99/Keppy-s-Synthesizer/issues
AppUpdatesURL=https://github.com/KaleidonKep99/Keppy-s-Synthesizer/releases
AppVersion={#Version}
AppComments=User-mode MIDI driver based on the BASS libraries
ArchitecturesAllowed=x86 x64
ArchitecturesInstallIn64BitMode=x64
CloseApplications=yes
Compression=bzip
CompressionThreads=2
CreateAppDir=False
DefaultGroupName=Keppy's Synthesizer
ExtraDiskSpaceRequired=6
InternalCompressLevel=ultra64
LicenseFile=nsislicense.txt
MinVersion=0,6.0.6001sp1
OutputBaseFilename=KeppysSynthSetup
SetupIconFile=midiicon.ico
ShowLanguageDialog=no
SolidCompression=yes
TimeStampsInUTC=True
UninstallDisplayIcon={syswow64}\keppysynth\KeppySynthConfigurator.exe
UninstallDisplayName=Keppy's Synthesizer {#Version} (Uninstall only)
UninstallDisplaySize=3241947
UninstallFilesDir={sys}\keppysynth\
VersionInfoCompany=KaleidonKep99
VersionInfoCopyright=Copyright (c) 2011-2017 Brad Miller, Chris Moeller and Riccardo Loi. All rights reserved.
VersionInfoDescription=User-mode MIDI driver based on the BASS libraries
VersionInfoProductName=Keppy's Synthesizer
VersionInfoProductTextVersion={#Version}
VersionInfoTextVersion=User-mode MIDI driver based on the BASS libraries
VersionInfoVersion={#Version}
UsePreviousSetupType=False
FlatComponentsList=False
AlwaysShowGroupOnReadyPage=True
AlwaysShowDirOnReadyPage=True
WizardImageFile=compiler:WizModernImage-IS.bmp
WizardSmallImageFile=compiler:WizModernSmallImage-IS.bmp

[Files]
; 64-bit OS
Source: "output\64\keppysynth.dll"; DestDir: "{sys}\keppysynth"; DestName: "keppysynth.dll"; Flags: replacesameversion ignoreversion; Check: Is64BitInstallMode
Source: "output\KeppySynthConfigurator.exe"; DestDir: "{syswow64}\keppysynth"; DestName: "KeppySynthConfigurator.exe"; Flags: replacesameversion ignoreversion; Check: Is64BitInstallMode
Source: "output\KeppySynthDebugWindow.exe"; DestDir: "{syswow64}\keppysynth"; DestName: "KeppySynthDebugWindow.exe"; Flags: replacesameversion ignoreversion; Check: Is64BitInstallMode
Source: "output\KeppySynthMixerWindow.exe"; DestDir: "{syswow64}\keppysynth"; DestName: "KeppySynthMixerWindow.exe"; Flags: replacesameversion ignoreversion; Check: Is64BitInstallMode
Source: "output\keppysynth.dll"; DestDir: "{syswow64}\keppysynth"; DestName: "keppysynth.dll"; Flags: replacesameversion ignoreversion; Check: Is64BitInstallMode
Source: "output\KSDriverRegister.exe"; DestDir: "{syswow64}\keppysynth"; DestName: "KSDriverRegister.exe"; Flags: replacesameversion ignoreversion; Check: Is64BitInstallMode
Source: "output\midioutsetter32.exe"; DestDir: "{syswow64}\keppysynth"; DestName: "midioutsetter32.exe"; Flags: replacesameversion ignoreversion; Check: Is64BitInstallMode
Source: "output\midioutsetter64.exe"; DestDir: "{syswow64}\keppysynth"; DestName: "midioutsetter64.exe"; Flags: replacesameversion ignoreversion; Check: Is64BitInstallMode
Source: "output\sfpacker.exe"; DestDir: "{syswow64}\keppysynth"; DestName: "sfpacker.exe"; Flags: replacesameversion ignoreversion; Check: Is64BitInstallMode
Source: "output\sfzguide.txt"; DestDir: "{syswow64}\keppysynth"; DestName: "sfzguide.txt"; Flags: replacesameversion ignoreversion; Check: Is64BitInstallMode

; 32-bit OS
Source: "output\KeppySynthConfigurator.exe"; DestDir: "{sys}\keppysynth"; DestName: "KeppySynthConfigurator.exe"; Flags: replacesameversion ignoreversion; Check: not Is64BitInstallMode
Source: "output\KeppySynthDebugWindow.exe"; DestDir: "{sys}\keppysynth"; DestName: "KeppySynthDebugWindow.exe"; Flags: replacesameversion ignoreversion; Check: not Is64BitInstallMode
Source: "output\keppysynth.dll"; DestDir: "{sys}\keppysynth"; DestName: "keppysynth.dll"; Flags: replacesameversion ignoreversion; Check: not Is64BitInstallMode
Source: "output\KSDriverRegister.exe"; DestDir: "{sys}\keppysynth"; DestName: "KSDriverRegister.exe"; Flags: replacesameversion ignoreversion; Check: not Is64BitInstallMode
Source: "output\midioutsetter32.exe"; DestDir: "{sys}\keppysynth"; DestName: "midioutsetter32.exe"; Flags: replacesameversion ignoreversion; Check: not Is64BitInstallMode
Source: "output\sfpacker.exe"; DestDir: "{sys}\keppysynth"; DestName: "sfpacker.exe"; Flags: replacesameversion ignoreversion; Check: not Is64BitInstallMode
Source: "output\sfzguide.txt"; DestDir: "{sys}\keppysynth"; DestName: "sfzguide.txt"; Flags: replacesameversion ignoreversion; Check: not Is64BitInstallMode

; 64-bit libs
Source: "external_packages\lib64\bass.dll"; DestDir: "{sys}\keppysynth"; DestName: "bass.dll"; Flags: replacesameversion ignoreversion; Check: Is64BitInstallMode
Source: "external_packages\lib64\bass_mpc.dll"; DestDir: "{sys}\keppysynth"; DestName: "bass_mpc.dll"; Flags: replacesameversion ignoreversion; Check: Is64BitInstallMode
Source: "external_packages\lib64\bass_vst.dll"; DestDir: "{sys}\keppysynth"; DestName: "bass_vst.dll"; Flags: replacesameversion ignoreversion; Check: Is64BitInstallMode
Source: "external_packages\lib64\bassenc.dll"; DestDir: "{sys}\keppysynth"; DestName: "bassenc.dll"; Flags: replacesameversion ignoreversion; Check: Is64BitInstallMode
Source: "external_packages\lib64\bassflac.dll"; DestDir: "{sys}\keppysynth"; DestName: "bassflac.dll"; Flags: replacesameversion ignoreversion; Check: Is64BitInstallMode
Source: "external_packages\lib64\bassmidi.dll"; DestDir: "{sys}\keppysynth"; DestName: "bassmidi.dll"; Flags: replacesameversion ignoreversion; Check: Is64BitInstallMode
Source: "external_packages\lib64\bassopus.dll"; DestDir: "{sys}\keppysynth"; DestName: "bassopus.dll"; Flags: replacesameversion ignoreversion; Check: Is64BitInstallMode
Source: "external_packages\lib64\basswv.dll"; DestDir: "{sys}\keppysynth"; DestName: "basswv.dll"; Flags: replacesameversion ignoreversion; Check: Is64BitInstallMode
Source: "external_packages\lib\bass.dll"; DestDir: "{syswow64}\keppysynth"; DestName: "bass.dll"; Flags: replacesameversion ignoreversion; Check: Is64BitInstallMode
Source: "external_packages\lib\bass_mpc.dll"; DestDir: "{syswow64}\keppysynth"; DestName: "bass_mpc.dll"; Flags: replacesameversion ignoreversion; Check: Is64BitInstallMode
Source: "external_packages\lib\bass_vst.dll"; DestDir: "{syswow64}\keppysynth"; DestName: "bass_vst.dll"; Flags: replacesameversion ignoreversion; Check: Is64BitInstallMode
Source: "external_packages\lib\bassenc.dll"; DestDir: "{syswow64}\keppysynth"; DestName: "bassenc.dll"; Flags: replacesameversion ignoreversion; Check: Is64BitInstallMode
Source: "external_packages\lib\bassflac.dll"; DestDir: "{syswow64}\keppysynth"; DestName: "bassflac.dll"; Flags: replacesameversion ignoreversion; Check: Is64BitInstallMode
Source: "external_packages\lib\bassmidi.dll"; DestDir: "{syswow64}\keppysynth"; DestName: "bassmidi.dll"; Flags: replacesameversion ignoreversion; Check: Is64BitInstallMode
Source: "external_packages\lib\bassopus.dll"; DestDir: "{syswow64}\keppysynth"; DestName: "bassopus.dll"; Flags: replacesameversion ignoreversion; Check: Is64BitInstallMode
Source: "external_packages\lib\basswv.dll"; DestDir: "{syswow64}\keppysynth"; DestName: "basswv.dll"; Flags: replacesameversion ignoreversion; Check: Is64BitInstallMode

; 32-bit libs
Source: "external_packages\lib\bass.dll"; DestDir: "{sys}\keppysynth"; DestName: "bass.dll"; Flags: replacesameversion ignoreversion; Check: not Is64BitInstallMode
Source: "external_packages\lib\bass_mpc.dll"; DestDir: "{sys}\keppysynth"; DestName: "bass_mpc.dll"; Flags: replacesameversion ignoreversion; Check: not Is64BitInstallMode
Source: "external_packages\lib\bass_vst.dll"; DestDir: "{sys}\keppysynth"; DestName: "bass_vst.dll"; Flags: replacesameversion ignoreversion; Check: not Is64BitInstallMode
Source: "external_packages\lib\bassenc.dll"; DestDir: "{sys}\keppysynth"; DestName: "bassenc.dll"; Flags: replacesameversion ignoreversion; Check: not Is64BitInstallMode
Source: "external_packages\lib\bassflac.dll"; DestDir: "{sys}\keppysynth"; DestName: "bassflac.dll"; Flags: replacesameversion ignoreversion; Check: not Is64BitInstallMode
Source: "external_packages\lib\bassmidi.dll"; DestDir: "{sys}\keppysynth"; DestName: "bassmidi.dll"; Flags: replacesameversion ignoreversion; Check: not Is64BitInstallMode
Source: "external_packages\lib\bassopus.dll"; DestDir: "{sys}\keppysynth"; DestName: "bassopus.dll"; Flags: replacesameversion ignoreversion; Check: not Is64BitInstallMode
Source: "external_packages\lib\basswv.dll"; DestDir: "{sys}\keppysynth"; DestName: "basswv.dll"; Flags: replacesameversion ignoreversion; Check: not Is64BitInstallMode

; Generic for all the OSes
Source: "LICENSE.TXT"; DestDir: "{%USERPROFILE}\Keppy's Synthesizer"; Flags: replacesameversion ignoreversion;
Source: "dxwebsetup.exe"; DestDir: "{tmp}"; DestName: "dxwebsetup.exe"; Flags: replacesameversion ignoreversion; MinVersion: 0,5.01sp3; Tasks: dx9redist
Source: "output\keppymididrv.defaultblacklist"; DestDir: "{win}"; Flags: replacesameversion ignoreversion; MinVersion: 0,5.01sp3

[Dirs]
; 64-bit OS
Name: "{sys}\keppysynth"; Attribs: system; Check: Is64BitInstallMode
Name: "{syswow64}\keppysynth"; Attribs: system; Check: Is64BitInstallMode
; 32-bit OS
Name: "{sys}\keppysynth"; Attribs: system; Check: not Is64BitInstallMode    

[Icons]
; 64-bit OS
Name: "{group}\Configure Keppy's Synthesizer"; Filename: "{syswow64}\keppysynth\KeppySynthConfigurator.exe"; Check: Is64BitInstallMode
Name: "{group}\Change advanced settings"; Filename: "{syswow64}\keppysynth\KeppySynthConfigurator.exe"; Parameters: "/AST"; WorkingDir: "{app}"; Check: Is64BitInstallMode
Name: "{group}\Soundfont packer by Kode54"; Filename: "{syswow64}\keppysynth\sfpacker.exe"; WorkingDir: "{app}"; Check: Is64BitInstallMode
; 32-bit OS
Name: "{group}\Configure Keppy's Synthesizer"; Filename: "{sys}\keppysynth\KeppySynthConfigurator.exe"; WorkingDir: "{app}"; Check: not Is64BitInstallMode
Name: "{group}\Change advanced settings"; Filename: "{sys}\keppysynth\KeppySynthConfigurator.exe"; Parameters: "/AST"; WorkingDir: "{app}"; Check: not Is64BitInstallMode
Name: "{group}\Soundfont packer by Kode54"; Filename: "{sys}\keppysynth\sfpacker.exe"; WorkingDir: "{app}"; Check: Is64BitInstallMode   
; Generic for all the OSes
Name: "{group}\Uninstall the driver"; Filename: "{uninstallexe}"
; Other
Name: "{userdesktop}\Keppy's Synthesizer Configurator"; Filename: "{syswow64}\keppysynth\KeppySynthConfigurator.exe"; IconFilename: "{syswow64}\keppysynth\KeppySynthConfigurator.exe"; Check: Is64BitInstallMode; Tasks: desktopicon
Name: "{userdesktop}\Keppy's Synthesizer Configurator"; Filename: "{sys}\keppysynth\KeppySynthConfigurator.exe"; IconFilename: "{sys}\keppysynth\KeppySynthConfigurator.exe"; Check: not Is64BitInstallMode; Tasks: desktopicon
Name: "{userappdata}\Microsoft\Internet Explorer\Quick Launch\Keppy's Synthesizer Configurator"; Filename: "{syswow64}\keppysynth\KeppySynthConfigurator.exe"; IconFilename: "{syswow64}\keppysynth\KeppySynthConfigurator.exe"; Check: Is64BitInstallMode; Tasks: quicklaunchicon
Name: "{userappdata}\Microsoft\Internet Explorer\Quick Launch\Keppy's Synthesizer Configurator"; Filename: "{sys}\keppysynth\KeppySynthConfigurator.exe"; IconFilename: "{sys}\keppysynth\KeppySynthConfigurator.exe"; Check: not Is64BitInstallMode; Tasks: quicklaunchicon

[Languages]
Name: en; MessagesFile: "compiler:Default.isl"
Name: de; MessagesFile: "compiler:Default.isl"

[Tasks]
Name: "desktopicon"; Description: "{cm:CreateDesktopIcon}"; GroupDescription: "{cm:AdditionalIcons}"
Name: "quicklaunchicon"; Description: "{cm:CreateQuickLaunchIcon}"; GroupDescription: "{cm:AdditionalIcons}"; Flags: unchecked
Name: "dx9redist"; Description: "Install DirectX 9.0c Redistributable (June 2010)"; GroupDescription: "Required runtimes"; Flags: checkedonce

[Registry]
; Normal settings
Root: "HKCU"; Subkey: "Software\Keppy's Synthesizer"; ValueType: dword; ValueName: "buffull"; ValueData: "0"; Flags: uninsdeletekey; Permissions: everyone-full
Root: "HKCU"; Subkey: "Software\Keppy's Synthesizer"; ValueType: dword; ValueName: "currentcpuusage0"; ValueData: "0"; Flags: uninsdeletekey; Permissions: everyone-full
Root: "HKCU"; Subkey: "Software\Keppy's Synthesizer"; ValueType: dword; ValueName: "currentvoices0"; ValueData: "0"; Flags: uninsdeletekey; Permissions: everyone-full
Root: "HKCU"; Subkey: "Software\Keppy's Synthesizer"; ValueType: dword; ValueName: "int"; ValueData: "0"; Flags: uninsdeletekey; Permissions: everyone-full
Root: "HKCU"; Subkey: "Software\Keppy's Synthesizer\Settings"; ValueType: dword; ValueName: "32bit"; ValueData: "1"; Flags: uninsdeletekey createvalueifdoesntexist
Root: "HKCU"; Subkey: "Software\Keppy's Synthesizer\Settings"; ValueType: dword; ValueName: "allhotkeys"; ValueData: "0"; Flags: uninsdeletekey createvalueifdoesntexist
Root: "HKCU"; Subkey: "Software\Keppy's Synthesizer\Settings"; ValueType: dword; ValueName: "autopanic"; ValueData: "1"; Flags: uninsdeletekey createvalueifdoesntexist
Root: "HKCU"; Subkey: "Software\Keppy's Synthesizer\Settings"; ValueType: dword; ValueName: "autoupdatecheck"; ValueData: "1"; Flags: uninsdeletekey createvalueifdoesntexist
Root: "HKCU"; Subkey: "Software\Keppy's Synthesizer\Settings"; ValueType: dword; ValueName: "buflen"; ValueData: "30"; Flags: uninsdeletekey createvalueifdoesntexist
Root: "HKCU"; Subkey: "Software\Keppy's Synthesizer\Settings"; ValueType: dword; ValueName: "cpu"; ValueData: "75"; Flags: uninsdeletekey createvalueifdoesntexist
Root: "HKCU"; Subkey: "Software\Keppy's Synthesizer\Settings"; ValueType: dword; ValueName: "debugmode"; ValueData: "0"; Flags: uninsdeletekey createvalueifdoesntexist
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
Root: "HKCU"; Subkey: "Software\Keppy's Synthesizer\Settings"; ValueType: dword; ValueName: "rco"; ValueData: "1"; Flags: createvalueifdoesntexist uninsdeletekey
Root: "HKCU"; Subkey: "Software\Keppy's Synthesizer\Settings"; ValueType: dword; ValueName: "sfdisableconf"; ValueData: "0"; Flags: createvalueifdoesntexist uninsdeletekey
Root: "HKCU"; Subkey: "Software\Keppy's Synthesizer\Settings"; ValueType: dword; ValueName: "sinc"; ValueData: "0"; Flags: createvalueifdoesntexist uninsdeletekey
Root: "HKCU"; Subkey: "Software\Keppy's Synthesizer\Settings"; ValueType: dword; ValueName: "sndbfvalue"; ValueData: "100"; Flags: createvalueifdoesntexist uninsdeletekey
Root: "HKCU"; Subkey: "Software\Keppy's Synthesizer\Settings"; ValueType: dword; ValueName: "softwaremode"; ValueData: "1"; Flags: createvalueifdoesntexist uninsdeletekey
Root: "HKCU"; Subkey: "Software\Keppy's Synthesizer\Settings"; ValueType: dword; ValueName: "sysresetignore"; ValueData: "0"; Flags: createvalueifdoesntexist uninsdeletekey
Root: "HKCU"; Subkey: "Software\Keppy's Synthesizer\Settings"; ValueType: dword; ValueName: "sysexignore"; ValueData: "0"; Flags: createvalueifdoesntexist uninsdeletekey
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
Root: "HKCU"; Subkey: "Software\Keppy's Synthesizer\Watchdog"; ValueType: string; ValueName: "bit"; Flags: createvalueifdoesntexist uninsdeletekey
Root: "HKCU"; Subkey: "Software\Keppy's Synthesizer\Watchdog"; ValueType: string; ValueName: "currentapp"; Flags: createvalueifdoesntexist uninsdeletekey
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
Root: "HKLM"; Subkey: "Software\Microsoft\Windows NT\CurrentVersion\Drivers32"; ValueType: none; ValueName: "midi9"; Flags: dontcreatekey deletevalue uninsdeletevalue; Check: Is64BitInstallMode
Root: "HKLM"; Subkey: "Software\Wow6432Node\Microsoft\Windows NT\CurrentVersion\Drivers32"; ValueType: none; ValueName: "midi9"; Flags: dontcreatekey deletevalue uninsdeletevalue; Check: Is64BitInstallMode

; 32-bit OS
Root: "HKLM"; Subkey: "Software\Microsoft\Windows NT\CurrentVersion\Drivers32"; ValueType: none; ValueName: "midi9"; Flags: dontcreatekey deletevalue uninsdeletevalue; Check: not Is64BitInstallMode

[InstallDelete]
Type: filesandordirs; Name: "{syswow64}\keppydrv\"; Check: Is64BitInstallMode
Type: filesandordirs; Name: "{sys}\keppydrv\"; Check: Is64BitInstallMode
Type: filesandordirs; Name: "{sys}\keppydrv\"; Check: not Is64BitInstallMode
Type: filesandordirs; Name: "{syswow64}\keppysynth\"; Check: Is64BitInstallMode
Type: filesandordirs; Name: "{sys}\keppysynth\"; Check: Is64BitInstallMode
Type: filesandordirs; Name: "{sys}\keppysynth\"; Check: not Is64BitInstallMode

[UninstallDelete]
Type: filesandordirs; Name: "{syswow64}\keppydrv\"; Check: Is64BitInstallMode
Type: filesandordirs; Name: "{sys}\keppydrv\"; Check: Is64BitInstallMode
Type: filesandordirs; Name: "{sys}\keppydrv\"; Check: not Is64BitInstallMode
Type: filesandordirs; Name: "{syswow64}\keppysynth\"; Check: Is64BitInstallMode
Type: filesandordirs; Name: "{sys}\keppysynth\"; Check: Is64BitInstallMode
Type: filesandordirs; Name: "{sys}\keppysynth\"; Check: not Is64BitInstallMode

[Run]
Filename: "{syswow64}\keppysynth\KeppySynthConfigurator.exe"; Flags: runascurrentuser postinstall waituntilidle; Description: "Run the configurator, to set up soundfonts"; StatusMsg: "Run the configurator, to set up soundfonts"; Check: Is64BitInstallMode
Filename: "{sys}\keppysynth\KeppySynthConfigurator.exe"; Flags: runascurrentuser postinstall waituntilidle; Description: "Run the configurator, to set up soundfonts"; StatusMsg: "Run the configurator, to set up soundfonts"; Check: not Is64BitInstallMode
Filename: "{syswow64}\keppysynth\KeppySynthConfigurator.exe"; Parameters: "/ASP"; Flags: waituntilterminated runascurrentuser; Description: "Moving stuff from ""LocalAppdata"" to ""UserProfile""..."; StatusMsg: "Moving stuff from ""LocalAppdata"" to ""UserProfile""..."; Check: Is64BitInstallMode
Filename: "{sys}\keppysynth\KeppySynthConfigurator.exe"; Parameters: "/ASP"; Flags: waituntilterminated runascurrentuser; Description: "Moving stuff from ""LocalAppdata"" to ""UserProfile""..."; StatusMsg: "Moving stuff from ""LocalAppdata"" to ""UserProfile""..."; Check: not Is64BitInstallMode
Filename: "{syswow64}\keppysynth\KSDriverRegister.exe"; Parameters: "/register"; Flags: waituntilterminated; Description: "AREDRV"; StatusMsg: "Registering driver..."; Check: Is64BitInstallMode
Filename: "{sys}\keppysynth\KSDriverRegister.exe"; Parameters: "/register"; Flags: waituntilterminated; Description: "AREDRV"; StatusMsg: "Registering driver..."; Check: not Is64BitInstallMode
Filename: "http://frozensnowproductions.com/"; Flags: shellexec postinstall runasoriginaluser nowait unchecked; Description: "Visit Frozen Snow Productions"; StatusMsg: "Visit Frozen Snow Productions"
Filename: "{tmp}\dxwebsetup.exe"; Parameters: "/q /r:n"; Flags: waituntilterminated; Description: "DXINSTALL"; StatusMsg: "Installing DirectX Redistributable (Jun 2010), please wait..."; Tasks: dx9redist

[UninstallRun]
Filename: "{syswow64}\keppysynth\KSDriverRegister.exe"; Parameters: "/unregister"; Flags: waituntilterminated; StatusMsg: "Unregistering driver..."; Check: Is64BitInstallMode
Filename: "{sys}\keppysynth\KSDriverRegister.exe"; Parameters: "/unregister"; Flags: waituntilterminated; StatusMsg: "Unregistering driver..."; Check: not Is64BitInstallMode

[Messages]
WindowsVersionNotSupported=Keppy's Synthesizer support for Windows XP ended on October 29th, 2010.%n%nIf you want to get further updates, please update to Windows Vista or newer.

[Code]
// shared code for installing the products
#include "scripts\products.iss"
// helper functions
#include "scripts\products\stringversion.iss"
#include "scripts\products\winversion.iss"
#include "scripts\products\fileversion.iss"
#include "scripts\products\dotnetfxversion.iss"


#ifdef use_dotnetfx40
#include "scripts\products\dotnetfx40client.iss"
#include "scripts\products\dotnetfx40full.iss"
#endif

#ifdef use_wic
#include "scripts\products\wic.iss"
#endif

#ifdef use_msiproduct
#include "scripts\products\msiproduct.iss"
#endif

#ifdef vc
#include "scripts\products\vcredist.iss"
#endif

function InitializeSetup(): boolean;

  var ErrorCode: Integer;

begin

  // Kill the watchdog before installing
  ShellExec('open','taskkill.exe','/f /im KeppySynthWatchdog.exe','',SW_HIDE,ewNoWait,ErrorCode);
  ShellExec('open','tskill.exe',' KeppySynthWatchdog.exe','',SW_HIDE,ewNoWait,ErrorCode);

	// initialize windows version
	initwinversion();

#ifdef use_msi40
	msi45('4.0'); // min allowed version is 4.0
#endif

#ifdef use_wic
	wic();
#endif

	// if no .netfx 4.0 is found, install the client (smallest)
#ifdef use_dotnetfx40
	if (not netfxinstalled(NetFx40Client, '') and not netfxinstalled(NetFx40Full, '')) then
		dotnetfx40client();
#endif

#ifdef vc
	vcredist2010();
	vcredist2013();
#endif

	Result := true;
end;

function InitializeUninstall(): Boolean;

  var ErrorCode: Integer;

begin

  // Kill the watchdog before uninstalling
  ShellExec('open','taskkill.exe','/f /im KeppySynthWatchdog.exe','',SW_HIDE,ewNoWait,ErrorCode);
  ShellExec('open','tskill.exe',' KeppySynthWatchdog.exe','',SW_HIDE,ewNoWait,ErrorCode);
 	Result := true;

end;