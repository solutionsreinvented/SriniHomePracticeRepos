@echo off
set fileToDelete=C:\path\to\file.txt

REM Check if the file is in use
net use "%fileToDelete%" >nul 2>&1
if %errorlevel% equ 0 (
    echo The file is in use. Killing the process...
    taskkill /F /IM process_name.exe /T
)

REM Delete the file
if exist "%fileToDelete%" (
    del "%fileToDelete%"
    echo File deleted successfully.
) else (
    echo File not found.
)

exit

C:\Windows\WinSxS\amd64_netfx-system.windows.forms_b03f5f7f11d50a3a_10.0.19041.1_none_6ffddb374fbda91c\System.Windows.Forms.dll