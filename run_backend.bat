@echo off
setlocal enabledelayedexpansion

title PawHaven BD - Backend API (.NET)

echo ============================================================================
echo   PawHaven BD - Starting ASP.NET Core Web API
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

cd /d "%~dp0backend"

echo Restoring dependencies and starting server on http://localhost:5208 ...
echo Press Ctrl+C at any time to stop the server.
echo.

dotnet run
if %errorlevel% neq 0 (
    echo.
    echo [ERROR] The backend failed to start.
    echo Please check if MySQL is running and your connection string in
    echo backend\appsettings.json or backend\appsettings.Development.json is correct.
    echo.
    pause
)
