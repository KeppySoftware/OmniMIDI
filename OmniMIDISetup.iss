#define use_ie6
#define use_dotnetfx40
#define use_wic
#define use_msiproduct

#define Author "KaleidonKep99"
#define Icon "Branding\OMSetupICO.ico"
#define Configurator "OmniMIDIConfigurator"
#define Copyright 'Copyright (c) 2011 Brad Miller, Chris Moeller and Riccardo Loi. All rights reserved.'
#define DebugWindow "OmniMIDIDebugWindow"
#define Description "OmniMIDI, User-Mode Windows MIDI Driver"
#define DriverRegister "OmniMIDIDriverRegister"
#define Email 'kaleidonkep99@outlook.com'
#define InstallDir "OmniMIDI"
#define Link 'https://github.com/KaleidonKep99/Keppy-s-Synthesizer'
#define MixerWindow "OmniMIDIMixerWindow"
#define OutputName "OmniMIDISetup"
#define ProductName "OmniMIDI"
#define Version '6.0.8.0'

#define lib32 'external_packages\lib'
#define lib64 'external_packages\lib64'
#define outputdir32 'output'
#define outputdir64 'output\64'

[Setup]
AllowCancelDuringInstall=True
AlwaysShowDirOnReadyPage=True
AlwaysShowGroupOnReadyPage=True
AppComments={#Description}     
AppContact={#Email}
AppCopyright={#Copyright}
AppId={{950DEC78-2D12-4917-BE69-CB04FE84B21F}
AppName={#ProductName}
AppPublisher={#Author}
AppPublisherURL={#Link}
AppSupportURL={#Link}/issues
AppUpdatesURL={#Link}/releases
AppVersion={#Version}
ArchitecturesAllowed=x86 x64
ArchitecturesInstallIn64BitMode=x64
CloseApplications=yes
Compression=lzma2/ultra64
CompressionThreads=2
CreateAppDir=False
DefaultGroupName={#ProductName}
DisableDirPage=auto
FlatComponentsList=False
InternalCompressLevel=ultra64
LanguageDetectionMethod=none
LicenseFile=license.txt
MinVersion=0,6.0.6001sp2
OutputBaseFilename={#OutputName}
SetupIconFile={#Icon}
ShowLanguageDialog=no
SolidCompression=yes
TimeStampsInUTC=True
UninstallDisplayIcon={syswow64}\{#InstallDir}\OmniMIDI.exe
UninstallDisplayName={#ProductName} (Uninstall only)
UninstallDisplaySize=8241947
UninstallFilesDir={syswow64}\{#InstallDir}
UsePreviousSetupType=False
VersionInfoCompany={#Author}
VersionInfoCopyright={#Copyright}
VersionInfoDescription={#Description}
VersionInfoProductName={#ProductName}
VersionInfoProductTextVersion={#Version}
VersionInfoTextVersion={#Description}
VersionInfoVersion={#Version}
WizardImageFile=scripts\image.bmp
WizardSmallImageFile=scripts\smallimage.bmp

[Files]
; 64-bit OS
Source: "{#outputdir64}\{#InstallDir}.dll"; DestDir: "{sys}\{#InstallDir}"; DestName: "{#InstallDir}.dll"; Flags: replacesameversion ignoreversion restartreplace; Check: Is64BitInstallMode
Source: "{#outputdir32}\{#Configurator}.exe"; DestDir: "{syswow64}\{#InstallDir}"; DestName: "{#Configurator}.exe"; Flags: replacesameversion ignoreversion; Check: Is64BitInstallMode
Source: "{#outputdir32}\{#DebugWindow}.exe"; DestDir: "{syswow64}\{#InstallDir}"; DestName: "{#DebugWindow}.exe"; Flags: replacesameversion ignoreversion; Check: Is64BitInstallMode
Source: "{#outputdir32}\{#MixerWindow}.exe"; DestDir: "{syswow64}\{#InstallDir}"; DestName: "{#MixerWindow}.exe"; Flags: replacesameversion ignoreversion; Check: Is64BitInstallMode
Source: "{#outputdir32}\{#InstallDir}.dll"; DestDir: "{syswow64}\{#InstallDir}"; DestName: "{#InstallDir}.dll"; Flags: replacesameversion ignoreversion restartreplace; Check: Is64BitInstallMode
Source: "{#outputdir32}\{#DriverRegister}.exe"; DestDir: "{syswow64}\{#InstallDir}"; DestName: "{#DriverRegister}.exe"; Flags: replacesameversion ignoreversion; Check: Is64BitInstallMode
Source: "{#outputdir32}\midioutsetter32.exe"; DestDir: "{syswow64}\{#InstallDir}"; DestName: "midioutsetter32.exe"; Flags: replacesameversion ignoreversion; Check: Is64BitInstallMode
Source: "{#outputdir32}\midioutsetter64.exe"; DestDir: "{syswow64}\{#InstallDir}"; DestName: "midioutsetter64.exe"; Flags: replacesameversion ignoreversion; Check: Is64BitInstallMode
Source: "{#outputdir32}\sfzguide.txt"; DestDir: "{syswow64}\{#InstallDir}"; DestName: "sfzguide.txt"; Flags: replacesameversion ignoreversion; Check: Is64BitInstallMode

; 32-bit OS
Source: "{#outputdir32}\{#Configurator}.exe"; DestDir: "{sys}\{#InstallDir}"; DestName: "{#Configurator}.exe"; Flags: replacesameversion ignoreversion; Check: not Is64BitInstallMode
Source: "{#outputdir32}\{#DebugWindow}.exe"; DestDir: "{sys}\{#InstallDir}"; DestName: "{#DebugWindow}.exe"; Flags: replacesameversion ignoreversion; Check: not Is64BitInstallMode
Source: "{#outputdir32}\{#MixerWindow}.exe"; DestDir: "{sys}\{#InstallDir}"; DestName: "{#MixerWindow}.exe"; Flags: replacesameversion ignoreversion; Check: not Is64BitInstallMode
Source: "{#outputdir32}\{#InstallDir}.dll"; DestDir: "{sys}\{#InstallDir}"; DestName: "{#InstallDir}.dll"; Flags: replacesameversion ignoreversion restartreplace; Check: not Is64BitInstallMode
Source: "{#outputdir32}\{#DriverRegister}.exe"; DestDir: "{sys}\{#InstallDir}"; DestName: "{#DriverRegister}.exe"; Flags: replacesameversion ignoreversion; Check: not Is64BitInstallMode
Source: "{#outputdir32}\midioutsetter32.exe"; DestDir: "{sys}\{#InstallDir}"; DestName: "midioutsetter32.exe"; Flags: replacesameversion ignoreversion; Check: not Is64BitInstallMode
Source: "{#outputdir32}\sfzguide.txt"; DestDir: "{sys}\{#InstallDir}"; DestName: "sfzguide.txt"; Flags: replacesameversion ignoreversion; Check: not Is64BitInstallMode

; 64-bit libs
Source: "{#lib64}\amidimap.cpl"; DestDir: "{sys}\{#InstallDir}"; DestName: "amidimap.cpl"; Flags: uninsrestartdelete comparetimestamp restartreplace; Check: Is64BitInstallMode
Source: "{#lib64}\bass.dll"; DestDir: "{sys}\{#InstallDir}"; DestName: "bass.dll"; Flags: replacesameversion ignoreversion; Check: Is64BitInstallMode
Source: "{#lib64}\bassasio.dll"; DestDir: "{sys}\{#InstallDir}"; DestName: "bassasio.dll"; Flags: replacesameversion ignoreversion; Check: Is64BitInstallMode
Source: "{#lib64}\bass_mpc.dll"; DestDir: "{sys}\{#InstallDir}"; DestName: "bass_mpc.dll"; Flags: replacesameversion ignoreversion; Check: Is64BitInstallMode
Source: "{#lib64}\bassenc.dll"; DestDir: "{sys}\{#InstallDir}"; DestName: "bassenc.dll"; Flags: replacesameversion ignoreversion; Check: Is64BitInstallMode
Source: "{#lib64}\bassflac.dll"; DestDir: "{sys}\{#InstallDir}"; DestName: "bassflac.dll"; Flags: replacesameversion ignoreversion; Check: Is64BitInstallMode
Source: "{#lib64}\bassmidi.dll"; DestDir: "{sys}\{#InstallDir}"; DestName: "bassmidi.dll"; Flags: replacesameversion ignoreversion; Check: Is64BitInstallMode
Source: "{#lib64}\bassopus.dll"; DestDir: "{sys}\{#InstallDir}"; DestName: "bassopus.dll"; Flags: replacesameversion ignoreversion; Check: Is64BitInstallMode
Source: "{#lib64}\basswv.dll"; DestDir: "{sys}\{#InstallDir}"; DestName: "basswv.dll"; Flags: replacesameversion ignoreversion; Check: Is64BitInstallMode
Source: "{#lib32}\amidimap.cpl"; DestDir: "{syswow64}\{#InstallDir}"; DestName: "amidimap.cpl"; Flags: uninsrestartdelete comparetimestamp restartreplace; Check: Is64BitInstallMode
Source: "{#lib32}\bass.dll"; DestDir: "{syswow64}\{#InstallDir}"; DestName: "bass.dll"; Flags: replacesameversion ignoreversion; Check: Is64BitInstallMode
Source: "{#lib32}\bassasio.dll"; DestDir: "{syswow64}\{#InstallDir}"; DestName: "bassasio.dll"; Flags: replacesameversion ignoreversion; Check: Is64BitInstallMode
Source: "{#lib32}\bass_mpc.dll"; DestDir: "{syswow64}\{#InstallDir}"; DestName: "bass_mpc.dll"; Flags: replacesameversion ignoreversion; Check: Is64BitInstallMode
Source: "{#lib32}\bassenc.dll"; DestDir: "{syswow64}\{#InstallDir}"; DestName: "bassenc.dll"; Flags: replacesameversion ignoreversion; Check: Is64BitInstallMode
Source: "{#lib32}\bassflac.dll"; DestDir: "{syswow64}\{#InstallDir}"; DestName: "bassflac.dll"; Flags: replacesameversion ignoreversion; Check: Is64BitInstallMode
Source: "{#lib32}\bassmidi.dll"; DestDir: "{syswow64}\{#InstallDir}"; DestName: "bassmidi.dll"; Flags: replacesameversion ignoreversion; Check: Is64BitInstallMode
Source: "{#lib32}\bassopus.dll"; DestDir: "{syswow64}\{#InstallDir}"; DestName: "bassopus.dll"; Flags: replacesameversion ignoreversion; Check: Is64BitInstallMode
Source: "{#lib32}\basswv.dll"; DestDir: "{syswow64}\{#InstallDir}"; DestName: "basswv.dll"; Flags: replacesameversion ignoreversion; Check: Is64BitInstallMode
Source: "output\{#InstallDir}.dbl"; DestDir: "{syswow64}\{#InstallDir}"; Flags: replacesameversion ignoreversion; Check: Is64BitInstallMode

; 32-bit libs
Source: "{#lib32}\amidimap.cpl"; DestDir: "{sys}\{#InstallDir}"; DestName: "amidimap.cpl"; Flags: uninsrestartdelete comparetimestamp restartreplace; Check: not Is64BitInstallMode
Source: "{#lib32}\bass.dll"; DestDir: "{sys}\{#InstallDir}"; DestName: "bass.dll"; Flags: replacesameversion ignoreversion; Check: not Is64BitInstallMode
Source: "{#lib32}\bassasio.dll"; DestDir: "{sys}\{#InstallDir}"; DestName: "bassasio.dll"; Flags: replacesameversion ignoreversion; Check: not Is64BitInstallMode
Source: "{#lib32}\bass_mpc.dll"; DestDir: "{sys}\{#InstallDir}"; DestName: "bass_mpc.dll"; Flags: replacesameversion ignoreversion; Check: not Is64BitInstallMode
Source: "{#lib32}\bassenc.dll"; DestDir: "{sys}\{#InstallDir}"; DestName: "bassenc.dll"; Flags: replacesameversion ignoreversion; Check: not Is64BitInstallMode
Source: "{#lib32}\bassflac.dll"; DestDir: "{sys}\{#InstallDir}"; DestName: "bassflac.dll"; Flags: replacesameversion ignoreversion; Check: not Is64BitInstallMode
Source: "{#lib32}\bassmidi.dll"; DestDir: "{sys}\{#InstallDir}"; DestName: "bassmidi.dll"; Flags: replacesameversion ignoreversion; Check: not Is64BitInstallMode
Source: "{#lib32}\bassopus.dll"; DestDir: "{sys}\{#InstallDir}"; DestName: "bassopus.dll"; Flags: replacesameversion ignoreversion; Check: not Is64BitInstallMode
Source: "{#lib32}\basswv.dll"; DestDir: "{sys}\{#InstallDir}"; DestName: "basswv.dll"; Flags: replacesameversion ignoreversion; Check: not Is64BitInstallMode
Source: "output\{#InstallDir}.dbl"; DestDir: "{sys}\{#InstallDir}"; Flags: replacesameversion ignoreversion; Check: not Is64BitInstallMode

; Generic for all the OSes
Source: "LICENSE.TXT"; DestDir: "{%USERPROFILE}\{#ProductName}"; Flags: replacesameversion ignoreversion

[Dirs]
; 64-bit OS
Name: "{sys}\{#InstallDir}"; Attribs: system; Permissions: everyone-full; Check: Is64BitInstallMode
Name: "{syswow64}\{#InstallDir}"; Attribs: system; Permissions: everyone-full; Check: Is64BitInstallMode
; 32-bit OS
Name: "{sys}\{#InstallDir}"; Attribs: system; Permissions: everyone-full; Check: not Is64BitInstallMode

[Icons]
; 64-bit OS
Name: "{group}\Configure {#ProductName}"; Filename: "{syswow64}\{#InstallDir}\{#Configurator}.exe"; WorkingDir: "{app}"; IconFilename: "{syswow64}\{#InstallDir}\{#Configurator}.exe"; Check: Is64BitInstallMode
Name: "{group}\Open the {#ProductName}'s mixer"; Filename: "{syswow64}\{#InstallDir}\{#MixerWindow}.exe"; WorkingDir: "{app}"; IconFilename: "{syswow64}\{#InstallDir}\{#MixerWindow}.exe"; Check: Is64BitInstallMode
Name: "{group}\Open the {#ProductName}'s debug window"; Filename: "{syswow64}\{#InstallDir}\{#DebugWindow}.exe"; WorkingDir: "{app}"; IconFilename: "{syswow64}\{#InstallDir}\{#DebugWindow}.exe"; Check: Is64BitInstallMode
Name: "{group}\Change advanced settings"; Filename: "{syswow64}\{#InstallDir}\{#Configurator}.exe"; WorkingDir: "{app}"; IconFilename: "{syswow64}\{#InstallDir}\{#Configurator}.exe"; Parameters: "/AST"; Check: Is64BitInstallMode
; 32-bit OS
Name: "{group}\Configure {#ProductName}"; Filename: "{sys}\{#InstallDir}\{#Configurator}.exe"; WorkingDir: "{app}"; IconFilename: "{sys}\{#InstallDir}\{#Configurator}.exe"; Check: not Is64BitInstallMode
Name: "{group}\Open the {#ProductName}'s mixer"; Filename: "{sys}\{#InstallDir}\{#MixerWindow}.exe"; WorkingDir: "{app}"; IconFilename: "{sys}\{#InstallDir}\{#MixerWindow}.exe"; Check: not Is64BitInstallMode
Name: "{group}\Open the {#ProductName}'s debug window"; Filename: "{sys}\{#InstallDir}\{#DebugWindow}.exe"; WorkingDir: "{app}"; IconFilename: "{sys}\{#InstallDir}\{#DebugWindow}.exe"; Check: not Is64BitInstallMode
Name: "{group}\Change advanced settings"; Filename: "{sys}\{#InstallDir}\{#Configurator}.exe"; WorkingDir: "{app}"; IconFilename: "{sys}\{#InstallDir}\{#Configurator}.exe"; Parameters: "/AST"; Check: not Is64BitInstallMode
; Other
Name: "{userdesktop}\{#ProductName} Configurator"; Filename: "{syswow64}\{#InstallDir}\{#Configurator}.exe"; IconFilename: "{syswow64}\{#InstallDir}\{#Configurator}.exe"; Tasks: desktopicon; Check: Is64BitInstallMode
Name: "{userdesktop}\{#ProductName} Configurator"; Filename: "{sys}\{#InstallDir}\{#Configurator}.exe"; IconFilename: "{sys}\{#InstallDir}\{#Configurator}.exe"; Tasks: desktopicon; Check: not Is64BitInstallMode
Name: "{userappdata}\Microsoft\Internet Explorer\Quick Launch\{#ProductName} Configurator"; Filename: "{syswow64}\{#InstallDir}\{#Configurator}.exe"; IconFilename: "{syswow64}\{#InstallDir}\{#Configurator}.exe"; Tasks: quicklaunchicon; Check: Is64BitInstallMode
Name: "{userappdata}\Microsoft\Internet Explorer\Quick Launch\{#ProductName} Configurator"; Filename: "{sys}\{#InstallDir}\{#Configurator}.exe"; IconFilename: "{sys}\{#InstallDir}\{#Configurator}.exe"; Tasks: quicklaunchicon; Check: not Is64BitInstallMode

[Languages]
Name: en; MessagesFile: "compiler:Default.isl"
Name: de; MessagesFile: "compiler:Default.isl"

[Tasks]
Name: "registerassociation"; Description: "Associate SoundFont files with the synthesizer"; GroupDescription: "Additional settings:"; Flags: unchecked
Name: "desktopicon"; Description: "{cm:CreateDesktopIcon}"; GroupDescription: "{cm:AdditionalIcons}"
Name: "quicklaunchicon"; Description: "{cm:CreateQuickLaunchIcon}"; GroupDescription: "{cm:AdditionalIcons}"; Flags: unchecked

[Registry]
; Normal settings
Root: "HKCU"; Subkey: "Software\{#ProductName}\Configuration"; ValueType: dword; ValueName: "AlternativeCPU"; ValueData: "0"; Flags: createvalueifdoesntexist uninsdeletekey
Root: "HKCU"; Subkey: "Software\{#ProductName}\Configuration"; ValueType: dword; ValueName: "AudioBitDepth"; ValueData: "1"; Flags: createvalueifdoesntexist uninsdeletekey
Root: "HKCU"; Subkey: "Software\{#ProductName}\Configuration"; ValueType: dword; ValueName: "AudioFrequency"; ValueData: "48000"; Flags: createvalueifdoesntexist uninsdeletekey
Root: "HKCU"; Subkey: "Software\{#ProductName}\Configuration"; ValueType: dword; ValueName: "AudioOutput"; ValueData: "0"; Flags: createvalueifdoesntexist uninsdeletekey
Root: "HKCU"; Subkey: "Software\{#ProductName}\Configuration"; ValueType: dword; ValueName: "BufferLength"; ValueData: "30"; Flags: createvalueifdoesntexist uninsdeletekey
Root: "HKCU"; Subkey: "Software\{#ProductName}\Configuration"; ValueType: dword; ValueName: "CapFramerate"; ValueData: "0"; Flags: createvalueifdoesntexist uninsdeletekey
Root: "HKCU"; Subkey: "Software\{#ProductName}\Configuration"; ValueType: dword; ValueName: "CurrentEngine"; ValueData: "3"; Flags: createvalueifdoesntexist uninsdeletekey
Root: "HKCU"; Subkey: "Software\{#ProductName}\Configuration"; ValueType: dword; ValueName: "DebugMode"; ValueData: "0"; Flags: createvalueifdoesntexist uninsdeletekey
Root: "HKCU"; Subkey: "Software\{#ProductName}\Configuration"; ValueType: dword; ValueName: "DefaultSFList"; ValueData: "1"; Flags: createvalueifdoesntexist uninsdeletekey
Root: "HKCU"; Subkey: "Software\{#ProductName}\Configuration"; ValueType: dword; ValueName: "EnableSFX"; ValueData: "1"; Flags: createvalueifdoesntexist uninsdeletekey
Root: "HKCU"; Subkey: "Software\{#ProductName}\Configuration"; ValueType: dword; ValueName: "EvBufferMultRatio"; ValueData: "1"; Flags: createvalueifdoesntexist uninsdeletekey
Root: "HKCU"; Subkey: "Software\{#ProductName}\Configuration"; ValueType: dword; ValueName: "EvBufferSize"; ValueData: "4096"; Flags: createvalueifdoesntexist uninsdeletekey
Root: "HKCU"; Subkey: "Software\{#ProductName}\Configuration"; ValueType: dword; ValueName: "Extra8Lists"; ValueData: "0"; Flags: createvalueifdoesntexist uninsdeletekey
Root: "HKCU"; Subkey: "Software\{#ProductName}\Configuration"; ValueType: dword; ValueName: "FastHotkeys"; ValueData: "0"; Flags: createvalueifdoesntexist uninsdeletekey
Root: "HKCU"; Subkey: "Software\{#ProductName}\Configuration"; ValueType: dword; ValueName: "HyperPlayback"; ValueData: "0"; Flags: createvalueifdoesntexist uninsdeletekey
Root: "HKCU"; Subkey: "Software\{#ProductName}\Configuration"; ValueType: dword; ValueName: "IgnoreNotesBetweenVel"; ValueData: "0"; Flags: createvalueifdoesntexist uninsdeletekey
Root: "HKCU"; Subkey: "Software\{#ProductName}\Configuration"; ValueType: dword; ValueName: "IgnoreSysEx"; ValueData: "0"; Flags: createvalueifdoesntexist uninsdeletekey
Root: "HKCU"; Subkey: "Software\{#ProductName}\Configuration"; ValueType: dword; ValueName: "IgnoreSysReset"; ValueData: "0"; Flags: createvalueifdoesntexist uninsdeletekey
Root: "HKCU"; Subkey: "Software\{#ProductName}\Configuration"; ValueType: dword; ValueName: "KSDAPIEnabled"; ValueData: "1"; Flags: createvalueifdoesntexist uninsdeletekey
Root: "HKCU"; Subkey: "Software\{#ProductName}\Configuration"; ValueType: dword; ValueName: "MaxRenderingTime"; ValueData: "75"; Flags: createvalueifdoesntexist uninsdeletekey
Root: "HKCU"; Subkey: "Software\{#ProductName}\Configuration"; ValueType: dword; ValueName: "MaxVoices"; ValueData: "500"; Flags: createvalueifdoesntexist uninsdeletekey
Root: "HKCU"; Subkey: "Software\{#ProductName}\Configuration"; ValueType: dword; ValueName: "MonoRendering"; ValueData: "0"; Flags: createvalueifdoesntexist uninsdeletekey
Root: "HKCU"; Subkey: "Software\{#ProductName}\Configuration"; ValueType: dword; ValueName: "NoteOff1"; ValueData: "0"; Flags: createvalueifdoesntexist uninsdeletekey
Root: "HKCU"; Subkey: "Software\{#ProductName}\Configuration"; ValueType: dword; ValueName: "NotesCatcherWithAudio"; ValueData: "0"; Flags: createvalueifdoesntexist uninsdeletekey
Root: "HKCU"; Subkey: "Software\{#ProductName}\Configuration"; ValueType: dword; ValueName: "OutputVolume"; ValueData: "10000"; Flags: createvalueifdoesntexist uninsdeletekey
Root: "HKCU"; Subkey: "Software\{#ProductName}\Configuration"; ValueType: dword; ValueName: "PID"; ValueData: "10"; Flags: createvalueifdoesntexist uninsdeletekey
Root: "HKCU"; Subkey: "Software\{#ProductName}\Configuration"; ValueType: dword; ValueName: "PreloadSoundFonts"; ValueData: "1"; Flags: createvalueifdoesntexist uninsdeletekey
Root: "HKCU"; Subkey: "Software\{#ProductName}\Configuration"; ValueType: dword; ValueName: "SincInter"; ValueData: "0"; Flags: createvalueifdoesntexist uninsdeletekey
Root: "HKCU"; Subkey: "Software\{#ProductName}\Configuration"; ValueType: dword; ValueName: "SleepStates"; ValueData: "1"; Flags: createvalueifdoesntexist uninsdeletekey
Root: "HKCU"; Subkey: "Software\{#ProductName}\Configuration"; ValueType: dword; ValueName: "SynthType"; ValueData: "4"; Flags: createvalueifdoesntexist uninsdeletekey
Root: "HKCU"; Subkey: "Software\{#ProductName}\Configuration"; ValueType: dword; ValueName: "SynthType"; ValueData: "4"; Flags: createvalueifdoesntexist uninsdeletekey
Root: "HKCU"; Subkey: "Software\{#ProductName}\Configuration"; ValueType: dword; ValueName: "VID"; ValueData: "65535"; Flags: createvalueifdoesntexist uninsdeletekey
Root: "HKCU"; Subkey: "Software\{#ProductName}\Configuration"; ValueType: dword; ValueName: "VolumeMonitor"; ValueData: "0"; Flags: createvalueifdoesntexist uninsdeletekey
Root: "HKCU"; Subkey: "Software\{#ProductName}\Configuration"; ValueType: string; ValueName: "ASIOOutput"; ValueData: "FL Studio ASIO"; Flags: createvalueifdoesntexist uninsdeletekey
Root: "HKCU"; Subkey: "Software\{#ProductName}\Configuration"; ValueType: string; ValueName: "SynthName"; ValueData: "OmniMIDI"; Flags: createvalueifdoesntexist uninsdeletekey
Root: "HKCU"; Subkey: "Software\{#ProductName}\Settings"; Flags: uninsdeletekey deletekey

;Override instruments
Root: "HKCU"; Subkey: "Software\{#ProductName}\ChanOverride"; ValueType: dword; ValueName: "overrideinstruments"; ValueData: "0"; Flags: createvalueifdoesntexist uninsdeletekey

;Channels volume
Root: "HKCU"; Subkey: "Software\{#ProductName}\Channels"; ValueType: dword; ValueName: "ch1"; ValueData: "100"; Flags: createvalueifdoesntexist uninsdeletekey
Root: "HKCU"; Subkey: "Software\{#ProductName}\Channels"; ValueType: dword; ValueName: "ch2"; ValueData: "100"; Flags: createvalueifdoesntexist uninsdeletekey
Root: "HKCU"; Subkey: "Software\{#ProductName}\Channels"; ValueType: dword; ValueName: "ch3"; ValueData: "100"; Flags: createvalueifdoesntexist uninsdeletekey
Root: "HKCU"; Subkey: "Software\{#ProductName}\Channels"; ValueType: dword; ValueName: "ch4"; ValueData: "100"; Flags: createvalueifdoesntexist uninsdeletekey
Root: "HKCU"; Subkey: "Software\{#ProductName}\Channels"; ValueType: dword; ValueName: "ch5"; ValueData: "100"; Flags: createvalueifdoesntexist uninsdeletekey
Root: "HKCU"; Subkey: "Software\{#ProductName}\Channels"; ValueType: dword; ValueName: "ch6"; ValueData: "100"; Flags: createvalueifdoesntexist uninsdeletekey
Root: "HKCU"; Subkey: "Software\{#ProductName}\Channels"; ValueType: dword; ValueName: "ch7"; ValueData: "100"; Flags: createvalueifdoesntexist uninsdeletekey
Root: "HKCU"; Subkey: "Software\{#ProductName}\Channels"; ValueType: dword; ValueName: "ch8"; ValueData: "100"; Flags: createvalueifdoesntexist uninsdeletekey
Root: "HKCU"; Subkey: "Software\{#ProductName}\Channels"; ValueType: dword; ValueName: "ch9"; ValueData: "100"; Flags: createvalueifdoesntexist uninsdeletekey
Root: "HKCU"; Subkey: "Software\{#ProductName}\Channels"; ValueType: dword; ValueName: "ch10"; ValueData: "100"; Flags: createvalueifdoesntexist uninsdeletekey
Root: "HKCU"; Subkey: "Software\{#ProductName}\Channels"; ValueType: dword; ValueName: "ch11"; ValueData: "100"; Flags: createvalueifdoesntexist uninsdeletekey
Root: "HKCU"; Subkey: "Software\{#ProductName}\Channels"; ValueType: dword; ValueName: "ch12"; ValueData: "100"; Flags: createvalueifdoesntexist uninsdeletekey
Root: "HKCU"; Subkey: "Software\{#ProductName}\Channels"; ValueType: dword; ValueName: "ch13"; ValueData: "100"; Flags: createvalueifdoesntexist uninsdeletekey
Root: "HKCU"; Subkey: "Software\{#ProductName}\Channels"; ValueType: dword; ValueName: "ch14"; ValueData: "100"; Flags: createvalueifdoesntexist uninsdeletekey
Root: "HKCU"; Subkey: "Software\{#ProductName}\Channels"; ValueType: dword; ValueName: "ch15"; ValueData: "100"; Flags: createvalueifdoesntexist uninsdeletekey
Root: "HKCU"; Subkey: "Software\{#ProductName}\Channels"; ValueType: dword; ValueName: "ch16"; ValueData: "100"; Flags: createvalueifdoesntexist uninsdeletekey

;Watchdog
Root: "HKCU"; Subkey: "Software\{#ProductName}\Watchdog"; ValueType: string; ValueName: "bit"; Flags: dontcreatekey deletevalue uninsdeletevalue
Root: "HKCU"; Subkey: "Software\{#ProductName}\Watchdog"; ValueType: string; ValueName: "currentapp"; Flags: dontcreatekey deletevalue uninsdeletevalue
Root: "HKCU"; Subkey: "Software\{#ProductName}\Watchdog"; ValueType: dword; ValueName: "closewatchdog"; ValueData: "1"; Flags: dontcreatekey deletevalue uninsdeletevalue
Root: "HKCU"; Subkey: "Software\{#ProductName}\Watchdog"; ValueType: dword; ValueName: "rel1"; ValueData: "0"; Flags: createvalueifdoesntexist uninsdeletekey
Root: "HKCU"; Subkey: "Software\{#ProductName}\Watchdog"; ValueType: dword; ValueName: "rel2"; ValueData: "0"; Flags: createvalueifdoesntexist uninsdeletekey
Root: "HKCU"; Subkey: "Software\{#ProductName}\Watchdog"; ValueType: dword; ValueName: "rel3"; ValueData: "0"; Flags: createvalueifdoesntexist uninsdeletekey
Root: "HKCU"; Subkey: "Software\{#ProductName}\Watchdog"; ValueType: dword; ValueName: "rel4"; ValueData: "0"; Flags: createvalueifdoesntexist uninsdeletekey
Root: "HKCU"; Subkey: "Software\{#ProductName}\Watchdog"; ValueType: dword; ValueName: "rel5"; ValueData: "0"; Flags: createvalueifdoesntexist uninsdeletekey
Root: "HKCU"; Subkey: "Software\{#ProductName}\Watchdog"; ValueType: dword; ValueName: "rel6"; ValueData: "0"; Flags: createvalueifdoesntexist uninsdeletekey
Root: "HKCU"; Subkey: "Software\{#ProductName}\Watchdog"; ValueType: dword; ValueName: "rel7"; ValueData: "0"; Flags: createvalueifdoesntexist uninsdeletekey
Root: "HKCU"; Subkey: "Software\{#ProductName}\Watchdog"; ValueType: dword; ValueName: "rel8"; ValueData: "0"; Flags: createvalueifdoesntexist uninsdeletekey
Root: "HKCU"; Subkey: "Software\{#ProductName}\Watchdog"; ValueType: dword; ValueName: "runwd"; ValueData: "0"; Flags: dontcreatekey deletevalue uninsdeletevalue
Root: "HKCU"; Subkey: "Software\{#ProductName}\Watchdog"; ValueType: dword; ValueName: "watchdog"; ValueData: "1"; Flags: dontcreatekey deletevalue uninsdeletevalue

[InstallDelete]
Type: filesandordirs; Name: "{syswow64}\{#InstallDir}\"; Check: Is64BitInstallMode
Type: filesandordirs; Name: "{sys}\{#InstallDir}\"
Type: filesandordirs; Name: "{syswow64}\keppydrv\"; Check: Is64BitInstallMode
Type: filesandordirs; Name: "{sys}\keppydrv\"
Type: filesandordirs; Name: "{syswow64}\keppysynth\"; Check: Is64BitInstallMode
Type: filesandordirs; Name: "{sys}\keppysynth\"
Type: filesandordirs; Name: "{group}\"
Type: files; Name: "{syswow64}\{#InstallDir}\bass_fx.dll"; Check: Is64BitInstallMode
Type: files; Name: "{sys}\{#InstallDir}\bass_fx.dll"
Type: files; Name: "{tmp}\LoudMax.dll"
Type: files; Name: "{tmp}\LoudMax64.dll"

[UninstallDelete]
Type: filesandordirs; Name: "{syswow64}\{#InstallDir}\"; Check: Is64BitInstallMode
Type: filesandordirs; Name: "{sys}\{#InstallDir}\"Type: filesandordirs; Name: "{syswow64}\keppydrv\"; Check: Is64BitInstallMode
Type: filesandordirs; Name: "{sys}\keppydrv\"
Type: filesandordirs; Name: "{syswow64}\keppysynth\"; Check: Is64BitInstallMode
Type: filesandordirs; Name: "{sys}\keppysynth\"
Type: filesandordirs; Name: "{group}\"Type: files; Name: "{syswow64}\{#InstallDir}\bass_fx.dll"; Check: Is64BitInstallMode
Type: files; Name: "{sys}\{#InstallDir}\bass_fx.dll"
Type: files; Name: "{tmp}\LoudMax.dll"
Type: files; Name: "{tmp}\LoudMax64.dll"

[Run]
Filename: "{syswow64}\{#InstallDir}\{#Configurator}.exe"; Flags: runascurrentuser postinstall waituntilidle; Description: "Run the configurator, to set up soundfonts"; StatusMsg: "Run the configurator, to set up soundfonts"; Check: Is64BitInstallMode
Filename: "{sys}\{#InstallDir}\{#Configurator}.exe"; Flags: runascurrentuser postinstall waituntilidle; Description: "Run the configurator, to set up soundfonts"; StatusMsg: "Run the configurator, to set up soundfonts"; Check: not Is64BitInstallMode
Filename: "{syswow64}\{#InstallDir}\{#DriverRegister}.exe"; Parameters: "/register"; Flags: waituntilterminated; StatusMsg: "Registering driver..."; Check: Is64BitInstallMode
Filename: "{sys}\{#InstallDir}\{#DriverRegister}.exe"; Parameters: "/register"; Flags: waituntilterminated; StatusMsg: "Registering driver..."; Check: not Is64BitInstallMode
Filename: "{syswow64}\{#InstallDir}\{#Configurator}.exe"; Parameters: "/toomni"; Flags: waituntilterminated runascurrentuser; StatusMsg: "Updating folders to OmniMIDI..."; Check: Is64BitInstallMode
Filename: "{sys}\{#InstallDir}\{#Configurator}.exe"; Parameters: "/toomni"; Flags: waituntilterminated runascurrentuser; StatusMsg: "Updating folders to OmniMIDI..."; Check: not Is64BitInstallMode

Filename: "{syswow64}\{#InstallDir}\{#DriverRegister}.exe"; Parameters: "/associate"; Flags: waituntilterminated; StatusMsg: "Registering associations..."; Check: Is64BitInstallMode; Tasks: registerassociation
Filename: "{sys}\{#InstallDir}\{#DriverRegister}.exe"; Parameters: "/associate"; Flags: waituntilterminated; StatusMsg: "Registering associations..."; Check: not Is64BitInstallMode; Tasks: registerassociation

[UninstallRun]
Filename: "{syswow64}\{#InstallDir}\{#DriverRegister}.exe"; Parameters: "/umidimap"; Flags: waituntilterminated; StatusMsg: "Unregistering MIDI Mapper..."; Check: Is64BitInstallMode
Filename: "{sys}\{#InstallDir}\{#DriverRegister}.exe"; Parameters: "/umidimap"; Flags: waituntilterminated; StatusMsg: "Unregistering MIDI Mapper..."; Check: not Is64BitInstallMode
Filename: "{syswow64}\{#InstallDir}\{#DriverRegister}.exe"; Parameters: "/unregister"; Flags: waituntilterminated; StatusMsg: "Unregistering driver..."; Check: Is64BitInstallMode
Filename: "{sys}\{#InstallDir}\{#DriverRegister}.exe"; Parameters: "/unregister"; Flags: waituntilterminated; StatusMsg: "Unregistering driver..."; Check: not Is64BitInstallMode

[Messages]
WindowsVersionNotSupported={#ProductName} support for Windows XP ended on October 29th, 2016.%n%nIf you want to get further updates, please update to Windows Vista or newer.
ExitSetupMessage=The MIDI driver hasn't been installed yet.%n%nAre you sure you want to quit?
SetupWindowTitle=Setup - %1 {#Version}

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

procedure CurUninstallStepChanged(CurUninstallStep: TUninstallStep);
begin
  if CurUninstallStep = usUninstall then
  begin
    if MsgBox('Do you want to delete your SoundFont lists too?', mbConfirmation, MB_YESNO or MB_DEFBUTTON2) = IDYES then 
    begin
        DelTree(ExpandConstant('{%USERPROFILE}\OmniMIDI'), True, True, True);
        MsgBox('Your data has been deleted.', mbInformation, MB_OK);    
    end;
  end;
end;

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