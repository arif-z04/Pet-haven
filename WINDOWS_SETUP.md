# 🪟 Windows Setup & Execution Guide — PawHaven BD

Welcome to **PawHaven BD**! This guide is specifically designed for **Windows users** (Windows 10 / Windows 11) to help you get the entire project installed, configured, and running smoothly in minutes.

---

## ⚡ 3-Minute Express Setup (Quick Start)

If you already have **.NET 10 SDK** (or .NET 8) and **MySQL / XAMPP** installed:

1. **Initialize the Database:**
   Double-click [`sql/setup_database.bat`](sql/setup_database.bat)  
   *(Or in PowerShell: run `.\sql\setup_database.ps1`)*

2. **Verify Database Password:**
   Open [`backend/appsettings.Development.json`](backend/appsettings.Development.json) and make sure your MySQL password is set (if using XAMPP, leave `Password=` empty).

3. **Launch the Whole App:**
   Double-click [`start_all_windows.bat`](start_all_windows.bat)  
   *This automatically opens both the Backend API and Frontend Storefront in separate terminal windows and launches your browser!*

---

## 📋 System Requirements & Prerequisites

You will need the following tools on your Windows machine:

| Software | Version | Purpose | Download Link / Command |
| :--- | :--- | :--- | :--- |
| **.NET SDK** | 10.0 (or 8.0) | Compiles and runs the Web API backend | [Download .NET SDK](https://dotnet.microsoft.com/download) |
| **MySQL Server** | 8.0+ or MariaDB / XAMPP | Stores relational store data | [Download XAMPP (Easiest)](https://www.apachefriends.org/) or [MySQL Installer](https://dev.mysql.com/downloads/installer/) |
| **Web Browser** | Edge, Chrome, Brave, Firefox | Runs the storefront and Swagger UI | Pre-installed |
| **Python** *(Optional)* | 3.8+ | Runs the 33-point automated test suite | [Download Python](https://www.python.org/downloads/) |

### Quick Install via Windows Package Manager (`winget`):
You can install all dependencies from an **Administrator PowerShell** prompt:
```powershell
winget install Microsoft.DotNet.SDK.10
winget install ApacheFriends.Xampp.8.2
winget install Python.Python.3.11
```

---

## 🛠️ Step 1: Setting Up MySQL on Windows

Choose **Option A (XAMPP - Easiest)** or **Option B (MySQL Community Server)**:

### Option A: Using XAMPP (Recommended for Beginners)
1. Install and open the **XAMPP Control Panel**.
2. Click the **Start** button next to **MySQL** (it will turn green and display port `3306`).
3. By default, XAMPP sets the MySQL user to `root` with **no password**.
4. In this repository, double-click [`sql/setup_database.bat`](sql/setup_database.bat).
5. When prompted:
   - Username: Press `Enter` (defaults to `root`)
   - Do you have a password?: Press `N` (or `Enter`)
6. The script will automatically locate `C:\xampp\mysql\bin\mysql.exe`, create `pet_ecommerce_db`, and populate all 25 tables, views, triggers, and sample data.

### Option B: Using Official MySQL Community Server / MySQL Installer
1. Open the **Windows Services** menu (`services.msc`) and verify that `MySQL80` (or `MySQL`) is **Running**.
2. Double-click [`sql/setup_database.bat`](sql/setup_database.bat).
3. When prompted:
   - Username: Press `Enter` (defaults to `root`)
   - Do you have a password?: Press `Y` and enter your MySQL root password.
4. The script will deploy the entire database schema and seed data.

---

## ⚙️ Step 2: Configure Database Connection

Open [`backend/appsettings.Development.json`](backend/appsettings.Development.json) in Notepad, VS Code, or Visual Studio:

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=127.0.0.1;Port=3306;Database=pet_ecommerce_db;User=root;Password=YOUR_PASSWORD_HERE;CharSet=utf8mb4;"
  }
}
```

- **If using XAMPP:** Leave the password blank: `Password=;`
- **If using MySQL Server with password:** Replace `YOUR_PASSWORD_HERE` with your root password: `Password=MyPassword123;`

> 💡 **Tip without editing files:** You can also set a PowerShell environment variable before launching the backend:
> ```powershell
> $env:MYSQL_CONNECTION_STRING="Server=127.0.0.1;Port=3306;Database=pet_ecommerce_db;User=root;Password=YourPassword;CharSet=utf8mb4;"
> ```

---

## 🚀 Step 3: Running the Application

Choose your favorite way to run the project on Windows:

### Method 1: The 1-Click Master Launcher (Easiest)
Double-click [`start_all_windows.bat`](start_all_windows.bat) in the root of the project:
1. It opens a command window running the **ASP.NET Core Web API** on `http://localhost:5208`.
2. It opens a command window serving the **Frontend Storefront** on `http://localhost:3000`.
3. It launches your default web browser and opens both the Storefront and the Swagger API documentation.

---

### Method 2: Running with Visual Studio 2022 / 2026
1. Double-click [`PetShop.sln`](PetShop.sln) to open the solution in Visual Studio.
2. Ensure the startup project is set to **PetHaven.Api**.
3. Select the **http** profile from the run dropdown (starts on port `5208`).
4. Press `F5` (Start Debugging) or `Ctrl + F5` (Start Without Debugging).
5. The Swagger documentation will open automatically at `http://localhost:5208/swagger`.
6. To run the frontend, double-click [`run_frontend.bat`](run_frontend.bat).

---

### Method 3: Running with Visual Studio Code
1. Open the project folder in VS Code (`File` > `Open Folder...`).
2. VS Code will automatically detect the pre-configured [`.vscode/launch.json`](.vscode/launch.json) and [`.vscode/tasks.json`](.vscode/tasks.json).
3. Press `F5` to build and launch the .NET API.
4. To view the storefront:
   - Install the **Live Server** extension (recommended prompt will appear).
   - Right-click [`frontend/index.html`](frontend/index.html) and select **Open with Live Server** (runs on `http://127.0.0.1:5500`, which is already enabled in the backend CORS settings).

---

### Method 4: Running via Windows Terminal / PowerShell CLI

**Window 1 (Backend API):**
```powershell
cd backend
dotnet run
```
*The API is now live at `http://localhost:5208`.*

**Window 2 (Frontend Storefront):**
```powershell
cd frontend
python -m http.server 3000
# Or using npx:
# npx serve . -p 3000
```
*The storefront is now live at `http://localhost:3000`.*

---

## 🌐 URLs & Verification

Once both services are running:

| Service | Local URL | Description |
| :--- | :--- | :--- |
| **Storefront** | `http://localhost:3000` | Full mobile-first shopping experience |
| **Swagger UI** | `http://localhost:5208/swagger` | Interactive API documentation & testing |
| **Backend Health** | `http://localhost:5208/api/products` | Raw JSON product catalog endpoint |

### Demo Accounts for Instant Testing:
On the login page ([`frontend/login.html`](frontend/login.html)), click any of the **1-Click Demo Fill** buttons:
- **Admin:** `admin@pawhaven.bd` / `Admin123!` (or `admin@pethavenbd.com` / `Password@123`)
- **Staff:** `staff@pawhaven.bd` / `Staff123!` (or `farhana.support@pethavenbd.com` / `Password@123`)
- **Customer:** `customer@pawhaven.bd` / `Customer123!` (or `zayan.chowdhury@example.com` / `Password@123`)

---

## 🧪 Running the Automated Test Suite on Windows

To run the full 33-point operational test suite:

1. Make sure both the backend (port `5208`) and frontend (port `3000`) are running.
2. Double-click [`run_tests.bat`](run_tests.bat)  
   *(Or in PowerShell: `python tests\test_suite.py`)*
3. You will see a detailed green passing matrix for all 8 test domains!

---

## ❓ Windows Troubleshooting & FAQ

### 1. `'dotnet' is not recognized as an internal or external command`
- **Cause:** .NET SDK is not installed or not in your Windows `PATH`.
- **Solution:** Download and install the [.NET 10 SDK](https://dotnet.microsoft.com/download), then close and reopen your Command Prompt or Terminal.

### 2. `'mysql' is not recognized as an internal or external command`
- **Cause:** MySQL CLI is not added to your system `PATH`.
- **Solution:** Our [`sql/setup_database.bat`](sql/setup_database.bat) script automatically scans standard Windows locations (`C:\xampp\mysql\bin\mysql.exe`, `C:\Program Files\MySQL\MySQL Server 8.0\bin\mysql.exe`). If yours is located elsewhere, add its folder to your Windows Environment Variables:
  1. Press `Windows Key + R`, type `sysdm.cpl`, press `Enter`.
  2. Go to **Advanced** > **Environment Variables**.
  3. Under **System Variables**, edit `Path` and add `C:\xampp\mysql\bin` (or your MySQL bin directory).

### 3. `Unable to connect to any of the specified MySQL hosts`
- **Cause:** The MySQL database server is not running.
- **Solution:**
  - If using XAMPP: Open XAMPP Control Panel and click **Start** on MySQL.
  - If using MySQL Windows Service: Open `services.msc`, locate `MySQL80`, and click **Start**.

### 4. `Authentication with password failed for user 'root'`
- **Cause:** The password in [`backend/appsettings.Development.json`](backend/appsettings.Development.json) does not match your MySQL root password.
- **Solution:** Open `appsettings.Development.json` and set your exact password in `Password=YOUR_PASSWORD;`. If you use XAMPP, ensure it is empty (`Password=;`).

### 5. `Port 5208 or Port 3306 is already in use`
- **Solution:** Find the conflicting process in Command Prompt:
  ```cmd
  netstat -ano | findstr :5208
  ```
  Note the PID (last column) and terminate it:
  ```cmd
  taskkill /PID <PID> /F
  ```

### 6. `CORS policy error when opening HTML files`
- **Cause:** Double-clicking `index.html` opens it with the `file:///` protocol instead of `http://`, which blocks AJAX/fetch requests in modern browsers.
- **Solution:** Use [`run_frontend.bat`](run_frontend.bat), `start_all_windows.bat`, or VS Code Live Server to serve files over HTTP at `http://localhost:3000` or `http://127.0.0.1:5500`.

### 7. PowerShell: `File cannot be loaded because running scripts is disabled`
- **Cause:** Windows PowerShell ExecutionPolicy restriction.
- **Solution:** Run this command in PowerShell to allow the script for the current session:
  ```powershell
  Set-ExecutionPolicy -Scope Process -ExecutionPolicy Bypass
  ```
  Then run `.\sql\setup_database.ps1`.
