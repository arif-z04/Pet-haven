@echo off
setlocal enabledelayedexpansion

title PawHaven BD - Database Setup (Windows)

echo ============================================================================
echo   PawHaven BD - MySQL Database Initialization Script for Windows
echo ============================================================================
echo.

rem Navigate to the sql directory where this script is located
cd /d "%~dp0"

set "MYSQL_BIN="

rem 1. Check if mysql is already in PATH
where mysql >nul 2>&1
if %errorlevel% equ 0 (
    set "MYSQL_BIN=mysql"
    goto :FoundMySQL
)

rem 2. Check if mariadb is in PATH
where mariadb >nul 2>&1
if %errorlevel% equ 0 (
    set "MYSQL_BIN=mariadb"
    goto :FoundMySQL
)

rem 3. Search common Windows installation paths
if exist "C:\Program Files\MySQL\MySQL Server 8.4\bin\mysql.exe" (
    set "MYSQL_BIN=C:\Program Files\MySQL\MySQL Server 8.4\bin\mysql.exe"
    goto :FoundMySQL
)
if exist "C:\Program Files\MySQL\MySQL Server 8.0\bin\mysql.exe" (
    set "MYSQL_BIN=C:\Program Files\MySQL\MySQL Server 8.0\bin\mysql.exe"
    goto :FoundMySQL
)
if exist "C:\Program Files\MySQL\MySQL Server 5.7\bin\mysql.exe" (
    set "MYSQL_BIN=C:\Program Files\MySQL\MySQL Server 5.7\bin\mysql.exe"
    goto :FoundMySQL
)
if exist "C:\xampp\mysql\bin\mysql.exe" (
    set "MYSQL_BIN=C:\xampp\mysql\bin\mysql.exe"
    goto :FoundMySQL
)
if exist "C:\laragon\bin\mysql\mysql.exe" (
    set "MYSQL_BIN=C:\laragon\bin\mysql\mysql.exe"
    goto :FoundMySQL
)

:NotFound
echo [ERROR] MySQL or MariaDB client was not found on your system!
echo.
echo Please ensure MySQL is installed:
echo   - If using XAMPP, MySQL is located at C:\xampp\mysql\bin\mysql.exe
echo   - If using MySQL Installer, install MySQL Server and Command Line Tools
echo   - Or add MySQL bin folder to your Windows System PATH.
echo.
pause
exit /b 1

:FoundMySQL
echo [OK] Using MySQL client at:
echo      !MYSQL_BIN!
echo.

set "DB_USER=root"
set /p "USER_INPUT=Enter MySQL Username [default: root]: "
if not "!USER_INPUT!"=="" set "DB_USER=!USER_INPUT!"

echo.
echo Is a password required for user '!DB_USER!'?
echo (If using XAMPP or default local MariaDB, usually NO password is required)
set /p "HAS_PASS=Do you have a password? (Y/N) [default: N]: "

if /i "!HAS_PASS!"=="Y" (
    echo.
    echo Please enter the password when prompted below:
    echo -------------------------------------------------------------
    "!MYSQL_BIN!" -u !DB_USER! -p < run_all.sql
) else (
    echo.
    echo Connecting without password...
    echo -------------------------------------------------------------
    "!MYSQL_BIN!" -u !DB_USER! < run_all.sql
)

if %errorlevel% neq 0 (
    echo.
    echo ============================================================================
    echo [ERROR] Database setup failed!
    echo Possible causes:
    echo   1. MySQL service is not running (Start it via Windows Services or XAMPP)
    echo   2. Incorrect username or password
    echo   3. Port 3306 is blocked or using a non-standard port
    echo ============================================================================
    echo.
    pause
    exit /b 1
)

echo.
echo ============================================================================
echo [SUCCESS] Database 'pet_ecommerce_db' initialized and seeded successfully!
echo ============================================================================
echo.
pause
