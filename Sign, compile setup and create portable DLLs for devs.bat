@echo off

@echo Signing libs...
signtool.exe sign /fd SHA512 /ac sign/KeppyCert.cer /f sign/KeppyCert.pfx /p 74bDxvxs9LhuQyqe /t "http://timestamp.verisign.com/scripts/timestamp.dll" /v output/OmniMIDIDebugWindow.exe
signtool.exe sign /fd SHA512 /ac sign/KeppyCert.cer /f sign/KeppyCert.pfx /p 74bDxvxs9LhuQyqe /t "http://timestamp.verisign.com/scripts/timestamp.dll" /v output/OmniMIDIMixerWindow.exe
signtool.exe sign /fd SHA512 /ac sign/KeppyCert.cer /f sign/KeppyCert.pfx /p 74bDxvxs9LhuQyqe /t "http://timestamp.verisign.com/scripts/timestamp.dll" /v output/OmniMIDIDriverRegister.exe
signtool.exe sign /fd SHA512 /ac sign/KeppyCert.cer /f sign/KeppyCert.pfx /p 74bDxvxs9LhuQyqe /t "http://timestamp.verisign.com/scripts/timestamp.dll" /v output/OmniMIDIConfigurator.exe

@echo Compiling setup...
"C:\Program Files (x86)\Inno Setup 5\iscc.exe" "OmniMIDISetup.iss"
@echo Compiling update...
"C:\Program Files (x86)\Inno Setup 5\iscc.exe" "OmniMIDIUpdate.iss"

@echo Signing setup and update...
signtool.exe sign /fd SHA512 /ac sign/KeppyCert.cer /f sign/KeppyCert.pfx /p 74bDxvxs9LhuQyqe /t "http://timestamp.verisign.com/scripts/timestamp.dll" /v output/OmniMIDISetup.exe
signtool.exe sign /fd SHA512 /ac sign/KeppyCert.cer /f sign/KeppyCert.pfx /p 74bDxvxs9LhuQyqe /t "http://timestamp.verisign.com/scripts/timestamp.dll" /v output/OmniMIDIUpdate.exe

@echo Creating portable package for developers...
7za a Output\OmniMIDIDev.zip DeveloperContent\
7za a Output\OmniMIDIDev.zip Output\OmniMIDI.dll
7za a Output\OmniMIDIDev.zip Output\64\OmniMIDI.dll