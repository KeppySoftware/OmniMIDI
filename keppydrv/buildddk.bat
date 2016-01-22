@ECHO OFF
if "%DDK_INC_PATH%" EQU "" GOTO ERROR
build /cwg 
goto END
:ERROR
echo.
echo This scripts needs to be run from a Windows Driver kit console (WINDDK)
echo (Check Vindows Driver kits, Build Environments)
:END
