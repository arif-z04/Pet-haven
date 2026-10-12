<#
.SYNOPSIS
    PawHaven BD - Database Setup PowerShell Script for Windows
.DESCRIPTION
    Initializes and seeds pet_ecommerce_db using MySQL CLI on Windows.
#>

[CmdletBinding()]
param(
    [string]$Username = "root",
    [switch]$PromptPassword
)

Write-Host "============================================================================" -ForegroundColor Cyan
Write-Host "  PawHaven BD - MySQL Database Initialization Script (PowerShell)         " -ForegroundColor Cyan
Write-Host "============================================================================" -ForegroundColor Cyan
Write-Host ""

$ScriptDir = Split-Path -Parent $MyInvocation.MyCommand.Path
Set-Location -Path $ScriptDir

# Locate MySQL executable
$mysqlExe = Get-Command mysql -ErrorAction SilentlyContinue
if (-not $mysqlExe) {
    $mysqlExe = Get-Command mariadb -ErrorAction SilentlyContinue
}

$commonPaths = @(
    "C:\Program Files\MySQL\MySQL Server 8.4\bin\mysql.exe",
    "C:\Program Files\MySQL\MySQL Server 8.0\bin\mysql.exe",
    "C:\Program Files\MySQL\MySQL Server 5.7\bin\mysql.exe",
    "C:\xampp\mysql\bin\mysql.exe",
    "C:\laragon\bin\mysql\mysql.exe"
)

if (-not $mysqlExe) {
    foreach ($p in $commonPaths) {
        if (Test-Path $p) {
            $mysqlExe = $p
            break
        }
    }
}

if (-not $mysqlExe) {
    Write-Host "[ERROR] MySQL or MariaDB command line client was not found!" -ForegroundColor Red
    Write-Host "Please ensure MySQL is installed (e.g. via XAMPP at C:\xampp\mysql\bin\mysql.exe)" -ForegroundColor Yellow
    Write-Host "or add the MySQL bin folder to your System PATH environment variable." -ForegroundColor Yellow
    exit 1
}

$exePath = if ($mysqlExe -is [System.Management.Automation.CommandInfo]) { $mysqlExe.Source } else { $mysqlExe }
Write-Host "[OK] Using MySQL client at: $exePath" -ForegroundColor Green
Write-Host ""

$userInput = Read-Host "Enter MySQL Username [default: $Username]"
if (-not [string]::IsNullOrWhiteSpace($userInput)) {
    $Username = $userInput.Trim()
}

$hasPassword = $false
$pwdChoice = Read-Host "Is a password required for user '$Username'? (Y/N) [default: N]"
if ($pwdChoice -match '^[Yy]') {
    $hasPassword = $true
}

$sqlFile = Join-Path $ScriptDir "run_all.sql"

try {
    if ($hasPassword) {
        Write-Host "Please enter your password when prompted below:" -ForegroundColor Yellow
        & $exePath -u $Username -p --default-character-set=utf8mb4 < $sqlFile
    } else {
        Write-Host "Executing database deployment without password..." -ForegroundColor Cyan
        & $exePath -u $Username --default-character-set=utf8mb4 < $sqlFile
    }

    if ($LASTEXITCODE -eq 0) {
        Write-Host ""
        Write-Host "============================================================================" -ForegroundColor Green
        Write-Host "[SUCCESS] Database 'pet_ecommerce_db' initialized and seeded successfully!" -ForegroundColor Green
        Write-Host "============================================================================" -ForegroundColor Green
    } else {
        throw "MySQL execution returned exit code $LASTEXITCODE"
    }
} catch {
    Write-Host ""
    Write-Host "[ERROR] Database deployment failed: $_" -ForegroundColor Red
    Write-Host "Check that:" -ForegroundColor Yellow
    Write-Host "  1. MySQL service is running on 127.0.0.1:3306" -ForegroundColor Yellow
    Write-Host "  2. Your username and password are correct" -ForegroundColor Yellow
    exit 1
}
