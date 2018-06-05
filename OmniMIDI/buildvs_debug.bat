@ECHO OFF
if "%INCLUDE%" EQU "" GOTO ERROR
@cl /Od /Zi /MTd /EHsc /DUNICODE /D_UNICODE /LDd /I "..\external_packages" /L"..\external_packages\lib" bassmididrv.cpp dsound.cpp sound_out_dsound.cpp sound_out_xaudio2.cpp kernel32.lib user32.lib Shlwapi.lib advapi32.lib winmm.lib Ole32.lib uuid.lib bassmididrv.def
goto END
:ERROR
echo.
echo This scripts needs to be run from a Visual studio command line 
echo (Check Visual Studio tools, Visual studio comand line in the programs menu)
:END
