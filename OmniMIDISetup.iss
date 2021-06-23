#pragma include __INCLUDE__ + ";" + ReadReg(HKLM, "Software\Mitrich Software\Inno Download Plugin", "InstallDir")
#include <idp.iss>

#define use_ie6
#define use_dotnetfx40
#define use_wic
#define use_msiproduct

#define Author "Keppy's Software"
#define Configurator "OmniMIDIConfigurator"
#define Copyright "Copyright (c) 2021 Keppy's Software. All rights reserved."
#define DebugWindow "OmniMIDIDebugWindow"
#define Description "OmniMIDI, User-Mode Windows MIDI Driver"
#define DevEnum "OmniMIDIDevEnum"
#define DriverRegister "OmniMIDIDriverRegister"
#define Email 'kaleidonkep99@outlook.com'
#define Icon "Branding\OMSetupICO.ico"
#define InstallDir "OmniMIDI"
#define Link 'https://github.com/KaleidonKep99/Keppy-s-Synthesizer'
#define MixerWindow "OmniMIDIMixerWindow"
#define OutputName "OmniMIDISetup"
#define ProductName "OmniMIDI"
#define Version '14.4.0.0'
                       
#define MIDIMapper 'OmniMapper'
#define lib32 'external_packages\lib'
#define lib64 'external_packages\lib64'
#define libARM64 'external_packages\libARM64'
#define outputdir32 'output'
#define outputdir64 'output\64'
#define outputdirARM64 'output\ARM64'

[Setup]
AllowCancelDuringInstall=False
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
ArchitecturesAllowed=x86 x64 arm64
ArchitecturesInstallIn64BitMode=x64 arm64
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
SetupMutex=OmniMIDISetup,KeppySoftware\OmniMIDISetup
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
VersionInfoTextVersion={#Description}
VersionInfoVersion={#Version}
WizardStyle=modern
WizardImageFile=scripts\image.bmp
WizardSmallImageFile=scripts\smallimage.bmp
DisableWelcomePage=False

[Files]
; 64-bit OS
Source: "{#outputdir64}\{#DevEnum}.exe"; DestDir: "{sys}\{#InstallDir}"; DestName: "{#DevEnum}.exe"; Flags: replacesameversion ignoreversion restartreplace; Check: WindowsAMD64
Source: "{#outputdir64}\{#InstallDir}.dll"; DestDir: "{sys}"; DestName: "{#InstallDir}.dll"; Flags: replacesameversion ignoreversion restartreplace; Check: WindowsAMD64
Source: "{#outputdir64}\{#MIDIMapper}.dll"; DestDir: "{sys}\{#InstallDir}"; DestName: "{#MIDIMapper}.dll"; Flags: replacesameversion ignoreversion restartreplace; Check: WindowsAMD64

; ARM64 OS
; Source: "{#outputdirARM64}\{#DevEnum}.exe"; DestDir: "{win}\Sysnative\{#InstallDir}"; DestName: "{#DevEnum}.exe"; Flags: replacesameversion ignoreversion restartreplace; Check: WindowsARM64
Source: "{#outputdirARM64}\{#InstallDir}.dll"; DestDir: "{win}\Sysnative"; DestName: "{#InstallDir}.dll"; Flags: replacesameversion ignoreversion restartreplace; Check: WindowsARM64
Source: "{#outputdirARM64}\{#MIDIMapper}.dll"; DestDir: "{win}\Sysnative\{#InstallDir}"; DestName: "{#MIDIMapper}.dll"; Flags: replacesameversion ignoreversion restartreplace; Check: WindowsARM64

; 32-bit files for AMD64 and ARM64
Source: "{#outputdir32}\{#Configurator}.exe"; DestDir: "{syswow64}\{#InstallDir}"; DestName: "{#Configurator}.exe"; Flags: replacesameversion ignoreversion; Check: Windows64
Source: "{#outputdir32}\{#DebugWindow}.exe"; DestDir: "{syswow64}\{#InstallDir}"; DestName: "{#DebugWindow}.exe"; Flags: replacesameversion ignoreversion; Check: Windows64
Source: "{#outputdir32}\{#DevEnum}.exe"; DestDir: "{syswow64}\{#InstallDir}"; DestName: "{#DevEnum}.exe"; Flags: replacesameversion ignoreversion; Check: Windows64
Source: "{#outputdir32}\{#MixerWindow}.exe"; DestDir: "{syswow64}\{#InstallDir}"; DestName: "{#MixerWindow}.exe"; Flags: replacesameversion ignoreversion; Check: Windows64
Source: "{#outputdir32}\{#InstallDir}.dll"; DestDir: "{syswow64}"; DestName: "{#InstallDir}.dll"; Flags: replacesameversion ignoreversion restartreplace; Check: Windows64
Source: "{#outputdir32}\{#MIDIMapper}.dll"; DestDir: "{syswow64}\{#InstallDir}"; DestName: "{#MIDIMapper}.dll"; Flags: replacesameversion ignoreversion restartreplace; Check: Windows64
Source: "{#outputdir32}\{#DriverRegister}.exe"; DestDir: "{syswow64}\{#InstallDir}"; DestName: "{#DriverRegister}.exe"; Flags: replacesameversion ignoreversion; Check: Windows64
Source: "{#outputdir32}\sfzguide.txt"; DestDir: "{syswow64}\{#InstallDir}"; DestName: "sfzguide.txt"; Flags: replacesameversion ignoreversion; Check: Windows64

; 32-bit files for IA32 OS
Source: "{#outputdir32}\{#Configurator}.exe"; DestDir: "{sys}\{#InstallDir}"; DestName: "{#Configurator}.exe"; Flags: replacesameversion ignoreversion; Check: not WindowsAMD64
Source: "{#outputdir32}\{#DebugWindow}.exe"; DestDir: "{sys}\{#InstallDir}"; DestName: "{#DebugWindow}.exe"; Flags: replacesameversion ignoreversion; Check: not WindowsAMD64
Source: "{#outputdir32}\{#DevEnum}.exe"; DestDir: "{sys}\{#InstallDir}"; DestName: "{#DevEnum}.exe"; Flags: replacesameversion ignoreversion; Check: not WindowsAMD64
Source: "{#outputdir32}\{#MixerWindow}.exe"; DestDir: "{sys}\{#InstallDir}"; DestName: "{#MixerWindow}.exe"; Flags: replacesameversion ignoreversion; Check: not WindowsAMD64
Source: "{#outputdir32}\{#InstallDir}.dll"; DestDir: "{sys}"; DestName: "{#InstallDir}.dll"; Flags: replacesameversion ignoreversion restartreplace; Check: not WindowsAMD64
Source: "{#outputdir32}\{#MIDIMapper}.dll"; DestDir: "{sys}\{#InstallDir}"; DestName: "{#MIDIMapper}.dll"; Flags: replacesameversion ignoreversion restartreplace; Check: not WindowsAMD64
Source: "{#outputdir32}\{#DriverRegister}.exe"; DestDir: "{sys}\{#InstallDir}"; DestName: "{#DriverRegister}.exe"; Flags: replacesameversion ignoreversion; Check: not WindowsAMD64
Source: "{#outputdir32}\sfzguide.txt"; DestDir: "{sys}\{#InstallDir}"; DestName: "sfzguide.txt"; Flags: replacesameversion ignoreversion; Check: not WindowsAMD64

; 64-bit libs
Source: "{#lib64}\bass.dll"; DestDir: "{sys}\{#InstallDir}"; DestName: "bass.dll"; Flags: replacesameversion ignoreversion; Check: WindowsAMD64
Source: "{#lib64}\bassmidi.dll"; DestDir: "{sys}\{#InstallDir}"; DestName: "bassmidi.dll"; Flags: replacesameversion ignoreversion; Check: WindowsAMD64
Source: "{#lib64}\opt\bass.dll"; DestDir: "{sys}\{#InstallDir}\opt"; DestName: "bass.dll"; Flags: replacesameversion ignoreversion; Check: WindowsAMD64
Source: "{#lib64}\opt\bassmidi.dll"; DestDir: "{sys}\{#InstallDir}\opt"; DestName: "bassmidi.dll"; Flags: replacesameversion ignoreversion; Check: WindowsAMD64
Source: "{#lib64}\bassopus.dll"; DestDir: "{sys}\{#InstallDir}"; DestName: "bassopus.dll"; Flags: replacesameversion ignoreversion; Check: WindowsAMD64
Source: "{#lib64}\basswv.dll"; DestDir: "{sys}\{#InstallDir}"; DestName: "basswv.dll"; Flags: replacesameversion ignoreversion; Check: WindowsAMD64
Source: "{#lib64}\bassasio.dll"; DestDir: "{sys}\{#InstallDir}"; DestName: "bassasio.dll"; Flags: replacesameversion ignoreversion; Check: WindowsAMD64
Source: "{#lib64}\bassflac.dll"; DestDir: "{sys}\{#InstallDir}"; DestName: "bassflac.dll"; Flags: replacesameversion ignoreversion; Check: WindowsAMD64
Source: "{#lib64}\bassenc.dll"; DestDir: "{sys}\{#InstallDir}"; DestName: "bassenc.dll"; Flags: replacesameversion ignoreversion; Check: WindowsAMD64
Source: "{#lib64}\bass_vst.dll"; DestDir: "{sys}\{#InstallDir}"; DestName: "bass_vst.dll"; Flags: replacesameversion ignoreversion; Check: WindowsAMD64
Source: "{#lib64}\basswasapi.dll"; DestDir: "{sys}\{#InstallDir}"; DestName: "basswasapi.dll"; Flags: replacesameversion ignoreversion; Check: WindowsAMD64

; ARM64 libs
; Alternative MIDI Mapper isn't available for ARM64
Source: "{#libarm64}\bass.dll"; DestDir: "{sys}\{#InstallDir}"; DestName: "bass.dll"; Flags: replacesameversion ignoreversion; Check: WindowsARM64
; Source: "{#libarm64}\basswv.dll"; DestDir: "{sys}\{#InstallDir}"; DestName: "basswv.dll"; Flags: replacesameversion ignoreversion; Check: WindowsARM64
; Source: "{#libarm64}\bassasio.dll"; DestDir: "{sys}\{#InstallDir}"; DestName: "bassasio.dll"; Flags: replacesameversion ignoreversion; Check: WindowsARM64
; Source: "{#libarm64}\bassflac.dll"; DestDir: "{sys}\{#InstallDir}"; DestName: "bassflac.dll"; Flags: replacesameversion ignoreversion; Check: WindowsARM64
; Source: "{#libarm64}\bassenc.dll"; DestDir: "{sys}\{#InstallDir}"; DestName: "bassenc.dll"; Flags: replacesameversion ignoreversion; Check: WindowsARM64
Source: "{#libarm64}\bassmidi.dll"; DestDir: "{sys}\{#InstallDir}"; DestName: "bassmidi.dll"; Flags: replacesameversion ignoreversion; Check: WindowsARM64
; Source: "{#libarm64}\basswasapi.dll"; DestDir: "{sys}\{#InstallDir}"; DestName: "basswasapi.dll"; Flags: replacesameversion ignoreversion; Check: WindowsARM64
; LoudMax isn't available for ARM64

; 32-bit libs for AMD64 and ARM64
Source: "{#lib32}\bass.dll"; DestDir: "{syswow64}\{#InstallDir}"; DestName: "bass.dll"; Flags: replacesameversion ignoreversion; Check: Windows64
Source: "{#lib32}\bassmidi.dll"; DestDir: "{syswow64}\{#InstallDir}"; DestName: "bassmidi.dll"; Flags: replacesameversion ignoreversion; Check: Windows64
Source: "{#lib32}\opt\bass.dll"; DestDir: "{syswow64}\{#InstallDir}\opt"; DestName: "bass.dll"; Flags: replacesameversion ignoreversion; Check: Windows64
Source: "{#lib32}\opt\bassmidi.dll"; DestDir: "{syswow64}\{#InstallDir}\opt"; DestName: "bassmidi.dll"; Flags: replacesameversion ignoreversion; Check: Windows64
Source: "{#lib32}\bassopus.dll"; DestDir: "{syswow64}\{#InstallDir}"; DestName: "bassopus.dll"; Flags: replacesameversion ignoreversion; Check: Windows64
Source: "{#lib32}\basswv.dll"; DestDir: "{syswow64}\{#InstallDir}"; DestName: "basswv.dll"; Flags: replacesameversion ignoreversion; Check: Windows64
Source: "{#lib32}\bassasio.dll"; DestDir: "{syswow64}\{#InstallDir}"; DestName: "bassasio.dll"; Flags: replacesameversion ignoreversion; Check: Windows64
Source: "{#lib32}\bassflac.dll"; DestDir: "{syswow64}\{#InstallDir}"; DestName: "bassflac.dll"; Flags: replacesameversion ignoreversion; Check: Windows64
Source: "{#lib32}\bassenc.dll"; DestDir: "{syswow64}\{#InstallDir}"; DestName: "bassenc.dll"; Flags: replacesameversion ignoreversion; Check: Windows64
Source: "{#lib32}\bass_vst.dll"; DestDir: "{syswow64}\{#InstallDir}"; DestName: "bass_vst.dll"; Flags: replacesameversion ignoreversion; Check: Windows64
Source: "{#lib32}\basswasapi.dll"; DestDir: "{syswow64}\{#InstallDir}"; DestName: "basswasapi.dll"; Flags: replacesameversion ignoreversion; Check: Windows64
Source: "output\{#InstallDir}.dbl"; DestDir: "{syswow64}\{#InstallDir}"; Flags: replacesameversion ignoreversion; Check: Windows64

; 32-bit libs for IA32
Source: "{#lib32}\bass.dll"; DestDir: "{sys}\{#InstallDir}"; DestName: "bass.dll"; Flags: replacesameversion ignoreversion; Check: not WindowsAMD64
Source: "{#lib32}\bassmidi.dll"; DestDir: "{sys}\{#InstallDir}"; DestName: "bassmidi.dll"; Flags: replacesameversion ignoreversion; Check: not WindowsAMD64
Source: "{#lib32}\opt\bass.dll"; DestDir: "{sys}\{#InstallDir}\opt"; DestName: "bass.dll"; Flags: replacesameversion ignoreversion; Check: not WindowsAMD64
Source: "{#lib32}\opt\bassmidi.dll"; DestDir: "{sys}\{#InstallDir}\opt"; DestName: "bassmidi.dll"; Flags: replacesameversion ignoreversion; Check: not WindowsAMD64
Source: "{#lib32}\bassopus.dll"; DestDir: "{sys}\{#InstallDir}"; DestName: "bassopus.dll"; Flags: replacesameversion ignoreversion; Check: not WindowsAMD64
Source: "{#lib32}\basswv.dll"; DestDir: "{sys}\{#InstallDir}"; DestName: "basswv.dll"; Flags: replacesameversion ignoreversion; Check: not WindowsAMD64
Source: "{#lib32}\bassasio.dll"; DestDir: "{sys}\{#InstallDir}"; DestName: "bassasio.dll"; Flags: replacesameversion ignoreversion; Check: not WindowsAMD64
Source: "{#lib32}\bassflac.dll"; DestDir: "{sys}\{#InstallDir}"; DestName: "bassflac.dll"; Flags: replacesameversion ignoreversion; Check: not WindowsAMD64
Source: "{#lib32}\bassenc.dll"; DestDir: "{sys}\{#InstallDir}"; DestName: "bassenc.dll"; Flags: replacesameversion ignoreversion; Check: not WindowsAMD64
Source: "{#lib32}\bass_vst.dll"; DestDir: "{sys}\{#InstallDir}"; DestName: "bass_vst.dll"; Flags: replacesameversion ignoreversion; Check: not WindowsAMD64
Source: "{#lib32}\basswasapi.dll"; DestDir: "{sys}\{#InstallDir}"; DestName: "basswasapi.dll"; Flags: replacesameversion ignoreversion; Check: not WindowsAMD64
Source: "output\{#InstallDir}.dbl"; DestDir: "{sys}\{#InstallDir}"; Flags: replacesameversion ignoreversion; Check: not WindowsAMD64

[Dirs]
; 64-bit OS
Name: "{sys}\{#InstallDir}"; Permissions: everyone-readexec; Flags: setntfscompression; Check: Windows64
Name: "{syswow64}\{#InstallDir}";  Permissions: everyone-readexec; Flags: setntfscompression; Check: Windows64
; 32-bit OS
Name: "{sys}\{#InstallDir}"; Permissions: everyone-readexec; Flags: setntfscompression; Check: WindowsIA32

[Icons]
; 64-bit OS
Name: "{group}\Configure {#ProductName}"; Filename: "{syswow64}\{#InstallDir}\{#Configurator}.exe"; WorkingDir: "{app}"; IconFilename: "{syswow64}\{#InstallDir}\{#Configurator}.exe"; Check: Windows64
Name: "{group}\Open the {#ProductName}'s mixer"; Filename: "{syswow64}\{#InstallDir}\{#MixerWindow}.exe"; WorkingDir: "{app}"; IconFilename: "{syswow64}\{#InstallDir}\{#MixerWindow}.exe"; Check: Windows64
Name: "{group}\Open the {#ProductName}'s debug window"; Filename: "{syswow64}\{#InstallDir}\{#DebugWindow}.exe"; WorkingDir: "{app}"; IconFilename: "{syswow64}\{#InstallDir}\{#DebugWindow}.exe"; Check: Windows64
Name: "{group}\Change advanced settings"; Filename: "{syswow64}\{#InstallDir}\{#Configurator}.exe"; WorkingDir: "{app}"; IconFilename: "{syswow64}\{#InstallDir}\{#Configurator}.exe"; Parameters: "/AST"; Check: Windows64

; 32-bit OS
Name: "{group}\Configure {#ProductName}"; Filename: "{sys}\{#InstallDir}\{#Configurator}.exe"; WorkingDir: "{app}"; IconFilename: "{sys}\{#InstallDir}\{#Configurator}.exe"; Check: WindowsIA32
Name: "{group}\Open the {#ProductName}'s mixer"; Filename: "{sys}\{#InstallDir}\{#MixerWindow}.exe"; WorkingDir: "{app}"; IconFilename: "{sys}\{#InstallDir}\{#MixerWindow}.exe"; Check: WindowsIA32
Name: "{group}\Open the {#ProductName}'s debug window"; Filename: "{sys}\{#InstallDir}\{#DebugWindow}.exe"; WorkingDir: "{app}"; IconFilename: "{sys}\{#InstallDir}\{#DebugWindow}.exe"; Check: WindowsIA32
Name: "{group}\Change advanced settings"; Filename: "{sys}\{#InstallDir}\{#Configurator}.exe"; WorkingDir: "{app}"; IconFilename: "{sys}\{#InstallDir}\{#Configurator}.exe"; Parameters: "/AST"; Check: WindowsIA32

; Other
Name: "{userdesktop}\{#ProductName} Configurator"; Filename: "{syswow64}\{#InstallDir}\{#Configurator}.exe"; IconFilename: "{syswow64}\{#InstallDir}\{#Configurator}.exe"; Tasks: desktopicon; Check: Is64BitInstallMode
Name: "{userdesktop}\{#ProductName} Configurator"; Filename: "{sys}\{#InstallDir}\{#Configurator}.exe"; IconFilename: "{sys}\{#InstallDir}\{#Configurator}.exe"; Tasks: desktopicon; Check: WindowsIA32
Name: "{userappdata}\Microsoft\Internet Explorer\Quick Launch\{#ProductName} Configurator"; Filename: "{syswow64}\{#InstallDir}\{#Configurator}.exe"; IconFilename: "{syswow64}\{#InstallDir}\{#Configurator}.exe"; Tasks: quicklaunchicon; Check: Is64BitInstallMode
Name: "{userappdata}\Microsoft\Internet Explorer\Quick Launch\{#ProductName} Configurator"; Filename: "{sys}\{#InstallDir}\{#Configurator}.exe"; IconFilename: "{sys}\{#InstallDir}\{#Configurator}.exe"; Tasks: quicklaunchicon; Check: WindowsIA32

[Languages]
Name: en; MessagesFile: "compiler:Default.isl"
Name: de; MessagesFile: "compiler:Default.isl"

[Tasks]
Name: "desktopicon"; Description: "{cm:CreateDesktopIcon}"; GroupDescription: "{cm:AdditionalIcons}"
Name: "quicklaunchicon"; Description: "{cm:CreateQuickLaunchIcon}"; GroupDescription: "{cm:AdditionalIcons}"; Flags: unchecked

[InstallDelete]
Type: filesandordirs; Name: "{syswow64}\{#InstallDir}\amidimap.cpl"; Check: Is64BitInstallMode
Type: filesandordirs; Name: "{sys}\{#InstallDir}\amidimap.cpl"
Type: filesandordirs; Name: "{syswow64}\{#InstallDir}\"; Check: Is64BitInstallMode
Type: filesandordirs; Name: "{sys}\{#InstallDir}\"
Type: filesandordirs; Name: "{syswow64}\keppydrv\"; Check: Is64BitInstallMode
Type: filesandordirs; Name: "{sys}\keppydrv\"
Type: filesandordirs; Name: "{syswow64}\keppysynth\"; Check: Is64BitInstallMode
Type: filesandordirs; Name: "{sys}\keppysynth\"
Type: filesandordirs; Name: "{group}\"
Type: files; Name: "{syswow64}\{#InstallDir}.dll"
Type: files; Name: "{sys}\{#InstallDir}.dll"
Type: files; Name: "{syswow64}\{#InstallDir}\{#InstallDir}.dll"
Type: files; Name: "{sys}\{#InstallDir}\{#InstallDir}.dll"
Type: files; Name: "{tmp}\LoudMax.dll"
Type: files; Name: "{tmp}\LoudMax64.dll"

[UninstallDelete]
Type: filesandordirs; Name: "{syswow64}\{#InstallDir}\amidimap.cpl"; Check: Is64BitInstallMode
Type: filesandordirs; Name: "{sys}\{#InstallDir}\amidimap.cpl"
Type: filesandordirs; Name: "{syswow64}\{#InstallDir}\"; Check: Is64BitInstallMode
Type: filesandordirs; Name: "{sys}\{#InstallDir}\"
Type: filesandordirs; Name: "{syswow64}\keppydrv\"; Check: Is64BitInstallMode
Type: filesandordirs; Name: "{sys}\keppydrv\"
Type: filesandordirs; Name: "{syswow64}\keppysynth\"; Check: Is64BitInstallMode
Type: filesandordirs; Name: "{sys}\keppysynth\"
Type: filesandordirs; Name: "{group}\"
Type: files; Name: "{syswow64}\{#InstallDir}.dll"
Type: files; Name: "{sys}\{#InstallDir}.dll"
Type: files; Name: "{syswow64}\{#InstallDir}\{#InstallDir}.dll"
Type: files; Name: "{sys}\{#InstallDir}\{#InstallDir}.dll"
Type: files; Name: "{tmp}\LoudMax.dll"
Type: files; Name: "{tmp}\LoudMax64.dll"

[Run]
Filename: "{syswow64}\{#InstallDir}\{#Configurator}.exe"; Flags: runascurrentuser postinstall waituntilidle; Description: "Run the configurator, to set up soundfonts"; StatusMsg: "Run the configurator, to set up soundfonts"; Check: Windows64
Filename: "{sys}\{#InstallDir}\{#Configurator}.exe"; Flags: runascurrentuser postinstall waituntilidle; Description: "Run the configurator, to set up soundfonts"; StatusMsg: "Run the configurator, to set up soundfonts"; Check: WindowsIA32
Filename: "{syswow64}\{#InstallDir}\{#DriverRegister}.exe"; Parameters: "/register"; Flags: waituntilterminated; StatusMsg: "Registering driver..."; Check: Windows64
Filename: "{sys}\{#InstallDir}\{#DriverRegister}.exe"; Parameters: "/register"; Flags: waituntilterminated; StatusMsg: "Registering driver..."; Check: WindowsIA32

// Create symlink
Filename: "cmd.exe"; Parameters: "/C mklink {syswow64}\{#InstallDir}\{#InstallDir}.dll {syswow64}\{#InstallDir}.dll"; Flags: waituntilterminated; StatusMsg: "Symlinking for KDMAPI..."; Check: WindowsAMD64
Filename: "cmd.exe"; Parameters: "/C mklink {sys}\{#InstallDir}\{#InstallDir}.dll {sys}\{#InstallDir}.dll"; Flags: waituntilterminated; StatusMsg: "Symlinking for KDMAPI..."

// Write registry keys
Filename: "{syswow64}\{#InstallDir}\{#Configurator}.exe"; Parameters: "/prepare"; Flags: runasoriginaluser waituntilterminated; StatusMsg: "Writing required registry keys..."; Check: Windows64
Filename: "{sys}\{#InstallDir}\{#Configurator}.exe"; Parameters: "/prepare"; Flags: runasoriginaluser waituntilterminated; StatusMsg: "Writing required registry keys..."; Check: WindowsIA32

[UninstallRun]
Filename: "{syswow64}\{#InstallDir}\{#DriverRegister}.exe"; Parameters: "/umidimap"; Flags: waituntilterminated; StatusMsg: "Unregistering MIDI Mapper..."; Check: Windows64
Filename: "{sys}\{#InstallDir}\{#DriverRegister}.exe"; Parameters: "/umidimap"; Flags: waituntilterminated; StatusMsg: "Unregistering MIDI Mapper..."; Check: WindowsIA32
Filename: "{syswow64}\{#InstallDir}\{#DriverRegister}.exe"; Parameters: "/unregister"; Flags: waituntilterminated; StatusMsg: "Unregistering driver..."; Check: Windows64
Filename: "{sys}\{#InstallDir}\{#DriverRegister}.exe"; Parameters: "/unregister"; Flags: waituntilterminated; StatusMsg: "Unregistering driver..."; Check: WindowsIA32

[Messages]
WindowsVersionNotSupported={#ProductName} support for Windows XP ended on October 29th, 2016.%n%nIf you want to get further updates, please update to Windows Vista or newer.
ExitSetupMessage=The MIDI driver hasn't been installed yet.%n%nAre you sure you want to quit?
SetupWindowTitle=Setup - %1
BeveledLabel=OmniMIDI for Windows NT

WindowsVersionNotSupported={#ProductName} support for Windows XP ended on October 29th, 2016.%n%nIf you want to get further updates, please update to Windows Vista or newer.
ExitSetupMessage={#ProductName} hasn't been installed yet.%n%nAre you sure you want to quit?
SetupWindowTitle={#ProductName} - User-mode MIDI driver for Windows ({#Version})
WelcomeLabel1=Welcome to the [name] Installer
WelcomeLabel2=This will install [name/ver] on your computer.%n%nIt is recommended that you close all other applications before continuing.
SetupAppTitle={#ProductName} Installer

[Code]
function Windows64(): Boolean;
begin
  Result := IsWin64;
end;

function WindowsARM64(): Boolean;
begin
  Result := (IsWin64 and IsARM64);
end;

function WindowsAMD64(): Boolean;
begin
  Result := (IsWin64 and (not IsARM64));
end;

function WindowsIA32(): Boolean;
begin
  Result := (not IsWin64);
end;

function IsWindowsVersionOrNewer(Major, Minor: Integer): boolean;
var
  Version: TWindowsVersion;
begin
  GetWindowsVersionEx(Version);
  Result :=
    (Version.Major > Major) or
    ((Version.Major = Major) and (Version.Minor >= Minor));
end;

function IsWindowsVistaOrNewer(): boolean;
begin
  Result := IsWindowsVersionOrNewer(6, 0);
end;

function Framework40IsNotInstalled(): boolean;
var
  bSuccess: Boolean;
  installVer: String;
begin
  Result := True;

  bSuccess := RegQueryStringValue(HKLM, 'Software\Microsoft\NET Framework Setup\NDP\v4\Full', 'TargetVersion', installVer);
  if (True = bSuccess) and (installVer = '4.0.0') then begin
    Result := False;
  end;
end;

function Framework45IsNotInstalled(): boolean;
var
  bSuccess: Boolean;
  regVersion: Cardinal;
begin
  Result := True;

  bSuccess := RegQueryDWordValue(HKLM, 'Software\Microsoft\NET Framework Setup\NDP\v4\Full', 'Release', regVersion);
  if (True = bSuccess) and (regVersion >= 378389) then begin
    Result := False;
  end;
end;

procedure InitializeWizard;
begin
  if IsWindowsVistaOrNewer() then begin
    if Framework45IsNotInstalled() then begin
      idpAddFile('http://go.microsoft.com/fwlink/?LinkId=397707', ExpandConstant('{tmp}\NetFramework45Installer.exe'));
      idpDownloadAfter(wpPreparing);
    end;
  end else begin
    if Framework40IsNotInstalled() then begin
      idpAddFile('http://go.microsoft.com/fwlink/?linkid=182805', ExpandConstant('{tmp}\NetFramework40Installer.exe'));
      idpDownloadAfter(wpPreparing);
    end;
  end;
end;

procedure InstallFramework;
var
  StatusText: string;
  ResultCode: Integer;
begin
  if IsWindowsVistaOrNewer() then begin
    StatusText := WizardForm.StatusLabel.Caption;
    WizardForm.StatusLabel.Caption := 'Please wait while OmniMIDI installs .NET 4.5.2 for you...';
    WizardForm.ProgressGauge.Style := npbstMarquee;
    try
      if not Exec(ExpandConstant('{tmp}\NetFramework45Installer.exe'), '/passive /norestart', '', SW_SHOW, ewWaitUntilTerminated, ResultCode) then
      begin
        MsgBox('OmniIMDI failed to install .NET Framework 4.5.2!' + #13#10 + 'Error code: ' + IntToStr(ResultCode) + '.' + #13#10 + #13#10 + 'Please try to install the framework manually.', mbError, MB_OK);
      end;
    finally
      WizardForm.StatusLabel.Caption := StatusText;
      WizardForm.ProgressGauge.Style := npbstNormal;

      DeleteFile(ExpandConstant('{tmp}\NetFramework45Installer.exe'));
    end;
  end else begin
    StatusText := WizardForm.StatusLabel.Caption;
    WizardForm.StatusLabel.Caption := 'Please wait while OmniMIDI installs .NET 4.0.3 for you...';
    WizardForm.ProgressGauge.Style := npbstMarquee;
    try
      if not Exec(ExpandConstant('{tmp}\NetFramework40Installer.exe'), '/passive /norestart', '', SW_SHOW, ewWaitUntilTerminated, ResultCode) then
      begin
        MsgBox('OmniIMDI failed to install .NET Framework 4.0.3!' + #13#10 + 'Error code: ' + IntToStr(ResultCode) + '.' + #13#10 + #13#10 + 'Please try to install the framework manually.', mbError, MB_OK);
      end;
    finally
      WizardForm.StatusLabel.Caption := StatusText;
      WizardForm.ProgressGauge.Style := npbstNormal;

      DeleteFile(ExpandConstant('{tmp}\NetFramework40Installer.exe'));
    end;
  end;
end;

procedure CurStepChanged(CurStep: TSetupStep);
begin
  case CurStep of
    ssInstall:
      begin
      if IsWindowsVistaOrNewer() then begin
        if Framework45IsNotInstalled() then begin
          InstallFramework();
        end;
      end else begin
        if Framework40IsNotInstalled() then begin
          InstallFramework();
        end;
      end;
    end;
  end;
end;
