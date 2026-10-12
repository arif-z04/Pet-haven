@echo off
setlocal enabledelayedexpansion

title PawHaven BD - Frontend Storefront (Port 3000)

echo ============================================================================
echo   PawHaven BD - Starting Frontend Static Server
echo ============================================================================
echo.

cd /d "%~dp0frontend"

rem 1. Check if Python is available
where python >nul 2>&1
if %errorlevel% equ 0 (
    echo [OK] Serving frontend using Python on http://localhost:3000 ...
    echo Opening browser in 2 seconds...
    start "" cmd /c "timeout /t 2 /nobreak >nul && start http://localhost:3000"
    python -m http.server 3000
    goto :Done
)

rem 2. Check if Python3 is available (Windows Store alias)
where python3 >nul 2>&1
if %errorlevel% equ 0 (
    echo [OK] Serving frontend using Python3 on http://localhost:3000 ...
    start "" cmd /c "timeout /t 2 /nobreak >nul && start http://localhost:3000"
    python3 -m http.server 3000
    goto :Done
)

rem 3. Check if npx is available
where npx >nul 2>&1
if %errorlevel% equ 0 (
    echo [OK] Serving frontend using npx serve on http://localhost:3000 ...
    start "" cmd /c "timeout /t 2 /nobreak >nul && start http://localhost:3000"
    npx serve . -p 3000
    goto :Done
)

rem 4. Fallback: Launch directly in default browser
echo [INFO] Neither Python nor Node/npx was found.
echo Opening index.html directly in your default browser...
echo.
echo NOTE: For best experience and API communication, serving over HTTP
echo (e.g. via VS Code Live Server or Python) is recommended.
start "" "%~dp0frontend\index.html"

:Done
