@echo off
setlocal enabledelayedexpansion

title PawHaven BD - Test Suite (Windows)

echo ============================================================================
echo   PawHaven BD - Operational & Mobile Responsive Verification Suite
echo ============================================================================
echo.

where python >nul 2>&1
if %errorlevel% equ 0 (
    python "%~dp0tests\test_suite.py"
    goto :Done
)

where python3 >nul 2>&1
if %errorlevel% equ 0 (
    python3 "%~dp0tests\test_suite.py"
    goto :Done
)

echo [ERROR] Python 3 was not found on your system!
echo Please install Python 3 from https://www.python.org/ or the Microsoft Store.
echo.

:Done
echo.
pause
