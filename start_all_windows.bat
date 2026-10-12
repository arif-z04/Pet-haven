@echo off
chcp 65001 >nul
setlocal enabledelayedexpansion

title PawHaven BD - Master 1-Click Launcher (Windows)

echo ============================================================================
echo   🐾 PawHaven BD - Master 1-Click Launcher for Windows
echo ============================================================================
echo.

rem Check if dotnet is installed
where dotnet >nul 2>&1
if %errorlevel% neq 0 (
    echo [ERROR] .NET SDK is not installed or not in your PATH!
    echo Please install the .NET SDK from:
    echo   https://dotnet.microsoft.com/download
    echo.
    pause
    exit /b 1
)

echo [1/3] Launching ASP.NET Core API server in a new window...
start "PawHaven API (Port 5208)" cmd /k "cd /d ""%~dp0"" && run_backend.bat"

echo [2/3] Launching Frontend static server in a new window...
start "PawHaven Storefront (Port 3000)" cmd /k "cd /d ""%~dp0"" && run_frontend.bat"

echo [3/3] Opening endpoints in your default browser...
timeout /t 3 /nobreak >nul
start http://localhost:5208/swagger
start http://localhost:3000

echo.
echo ============================================================================
echo [SUCCESS] Both backend and frontend services have been launched!
echo.
echo   - Storefront UI : http://localhost:3000
echo   - Swagger API   : http://localhost:5208/swagger
echo.
echo To stop the services, simply close the opened Command Prompt windows.
echo ============================================================================
echo.
pause
