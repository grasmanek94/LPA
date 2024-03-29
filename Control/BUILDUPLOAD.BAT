@echo off

IF not DEFINED AVR32_HOME GOTO :NOTINSTALLED

SET PATH=%AVR32_HOME%\bin;%PATH%
SET LANG=C
SET mypath=%~dp0
SET robotloader_cmd_exe="%AVR32_HOME%\RobotLoader\robotloader_cmd.exe"
SET robotloader_path="%AVR32_HOME%\RobotLoader\"
SET project_path=%mypath:~0,-1%\

cd /D "%project_path%"

for /f "delims=\" %%a in ("%project_path%") do SET project_name=%%~nxa
SET hexfile="%project_path%%project_name%.hex"

echo ------------------- clean -------------------
make -f makefilevs clean

echo ------------------- build -------------------
make -f makefilevs all 
if not ERRORLEVEL 0 GOTO :BUILDERROR

echo ------------------- info  -------------------
echo Using Project name    : %project_name%
echo Using Project path    : %project_path%
echo Using Project HEX File: %hexfile%
echo Using Robotloader cmd : %robotloader_path%

echo ------------------- upload ------------------
cd /D "%robotloader_path%"
%robotloader_cmd_exe% -c -hex=%hexfile%

echo ---------------------------------------------
GOTO :EOF

:NOTINSTALLED
	echo ERROR Cannot begin build
	echo Please run INSTALL BAT in RP6 Environment directory
	goto :EOF

:BUILDERROR
	echo Build errors occured
	echo You must fix the errors before upload can begin
	goto :EOF