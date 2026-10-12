# 🐾 PawHaven BD — Pet Care E-Commerce & Pet Profile Platform

[![.NET Version](https://img.shields.io/badge/.NET-10.0-512BD4?logo=dotnet)](https://dotnet.microsoft.com/)
[![ASP.NET Core](https://img.shields.io/badge/ASP.NET_Core-Web_API-512BD4?logo=dotnet)](https://learn.microsoft.com/en-us/aspnet/core/)
[![Database](https://img.shields.io/badge/Database-MySQL_8%2B_%2F_MariaDB-4479A1?logo=mysql&logoColor=white)](https://www.mysql.com/)
[![Frontend](https://img.shields.io/badge/Frontend-Vanilla_JS_%2B_Tailwind_CSS-38B2AC?logo=tailwind-css&logoColor=white)](https://tailwindcss.com/)
[![License](https://img.shields.io/badge/License-MIT-green.svg)](LICENSE)
[![Tests Status](https://img.shields.io/badge/Test_Matrix-33%2F33_Passed-success)](#-testing--qa-automation)

**PawHaven BD** is a modern, high-performance, full-stack pet care e-commerce web platform tailored for pet parents. It combines an **ASP.NET Core Web API** backend connected to **MySQL / MariaDB**, with an ultra-responsive, mobile-first **HTML5 / Vanilla JavaScript (ES Modules) / Tailwind CSS** storefront.

The platform provides species-tailored browsing (Cats, Dogs, Birds, Rabbits, Small Pets), dynamic pet profile management with rule-based veterinary recommendations, live autocomplete search, coupons, and an ACID-compliant 13-step transactional checkout system with inventory tracking.

---

## 📑 Table of Contents

- [Key Features](#-key-features)
- [System Architecture](#-system-architecture)
- [Repository Structure](#-repository-structure)
- [Tech Stack](#-tech-stack)
- [Prerequisites](#-prerequisites)
- [Getting Started](#-getting-started)
  - [1. Database Initialization](#1-database-initialization)
  - [2. Backend API Setup](#2-backend-api-setup)
  - [3. Frontend Storefront Setup](#3-frontend-storefront-setup)
- [Interactive Swagger API Documentation](#-interactive-swagger-api-documentation)
- [Demo User Accounts](#-demo-user-accounts)
- [Testing & QA Automation](#-testing--qa-automation)
- [Configuration & Environment Variables](#-configuration--environment-variables)
- [Contributing](#-contributing)
- [License](#-license)

---

## ✨ Key Features

### 🛒 Customer Storefront
- **Species & Category Navigation:** Hierarchical taxonomies covering pet species (🐱 Cats, 🐶 Dogs, 🦜 Birds, 🐰 Rabbits, 🐹 Small Pets) and detailed subcategories.
- **Dynamic Catalog & Filtering:** Real-time multi-facet filtering by category, brand, pet type, price range, stock availability, life stage, food type, and dietary health concerns.
- **Live Search & Autocomplete:** Instant, debounced (300ms) catalog keyword search matching product titles, descriptions, and SKUs.
- **Cart & Wishlist Operations:** Client-synchronized cart and bookmarking workflows with live badge counts and stock limits.
- **Promotional Coupons Engine:** Real-time coupon validation enforcing minimum spends, active dates, and per-user redemption limits.
- **13-Step Transactional Checkout:** ACID-compliant order placement validating stock, snapshotting immutable line items, logging inventory transactions, and applying delivery rates.
- **"My Pet" Profiles & Smart Recommendations:** Registered pet profiles (name, species, birth date, weight, dietary needs, allergies) power personalized life-stage and dietary recommendations.
- **Verified Buyer Reviews:** Rating submission and review moderation with verified purchaser detection.
- **Mobile-First UX:** 44×44px touch targets, sticky mobile bottom navigation, slide-over species drawer, swipeable ribbons, and responsive filter sheets.

### 🛡️ Admin & Operational Capabilities
- **Role-Based Access Control (RBAC):** Distinct permission levels (`Admin`, `Staff`, `Customer`) enforced via JWT authentication.
- **Order Management Queue:** State machine transitions (`pending` → `confirmed` → `processing` → `shipped` → `delivered` / `cancelled`) with automatic stock restoration upon cancellation.
- **Executive Analytics:** Dashboard endpoints delivering metrics for gross sales, order volume, recent orders, inventory restock alerts, and 7-day revenue breakdowns.

---

## 🏛️ System Architecture

```text
┌────────────────────────────────────────────────────────┐
│           PawHaven BD Frontend (Port 3000)             │
│   Vanilla JS (ES Modules) • Tailwind CSS • HTML5       │
│   Touch-Optimized UI • Sticky Mobile Nav • Drawer      │
└───────────────────────────┬────────────────────────────┘
                            │ RESTful JSON over HTTP
                            ▼
┌────────────────────────────────────────────────────────┐
│         ASP.NET Core Web API (Port 5208)               │
│  JWT Authentication • Exception Middleware • Swagger   │
│  Controllers ──► Services ──► EF Core DbContext        │
└───────────────────────────┬────────────────────────────┘
                            │ MySqlConnector / EF Core
                            ▼
┌────────────────────────────────────────────────────────┐
│          MySQL 8+ / MariaDB 10.5+ Database             │
│  25 Normalized Tables • Triggers • Views • Indexes     │
└────────────────────────────────────────────────────────┘
```

---

## 📁 Repository Structure

```text
.
├── backend/                  # ASP.NET Core Web API (.NET 10)
│   ├── Authentication/       # JWT Token generator & claim helpers
│   ├── Configuration/        # Strongly-typed configuration options
│   ├── Controllers/          # REST API endpoints (18 controllers)
│   ├── Data/                 # EF Core AppDbContext & mappings
│   ├── DTOs/                 # Data Transfer Objects & API envelopes
│   ├── Helpers/              # Password hashing (BCrypt) & utility classes
│   ├── Middleware/           # Global exception handling middleware
│   ├── Models/               # 25 relational domain entities
│   ├── Services/             # Business logic layer (Interfaces & Implementations)
│   ├── appsettings.json      # API runtime configuration
│   ├── appsettings.Example.json # Example configuration template
│   └── PetHaven.Api.csproj   # C# project definition
├── frontend/                 # Web Storefront Client
│   ├── assets/               # Images, icons, and font assets
│   ├── components/           # Reusable HTML layout components (Header, Mobile Nav)
│   ├── css/                  # Custom styling & scrollbar classes
│   ├── js/                   # Modular ES6 JavaScript
│   │   ├── api.js            # Centralized API fetch client with JWT interceptor
│   │   ├── auth.js           # Auth manager & local session lifecycle
│   │   ├── cart.js           # Cart state & line item calculations
│   │   └── ...               # Domain modules (checkout, pets, products, search)
│   ├── index.html            # Storefront homepage
│   ├── shop.html             # Product catalog & filter drawer
│   ├── product.html          # Product detail & review section
│   └── ...                   # Pages (cart, checkout, account, orders, my-pets, blog)
├── sql/                      # Normalized MySQL / MariaDB Schema & Data
│   ├── 01_create_database.sql # Database creation script
│   ├── 02_create_tables.sql   # 25 normalized tables with constraints
│   ├── 03_create_indexes.sql  # Composite and performance indexes
│   ├── 04-13_seed_*.sql       # Comprehensive seed data (categories, brands, products, users)
│   ├── 14_views.sql           # Storefront & analytical database views
│   ├── 15_triggers.sql        # Automated inventory audit triggers
│   ├── 16_test_queries.sql    # Verification queries
│   ├── run_all.sql            # Master execution script
│   └── setup_database.sh      # Automated bash database loader
└── tests/                    # Operational Verification & QA
    ├── test_suite.py         # 33-point automated integration test suite
    └── reports/              # Test result JSON output reports
```

---

## 💻 Tech Stack

| Layer | Technologies |
| :--- | :--- |
| **Backend API** | ASP.NET Core Web API (.NET 10), C# 13, Pomelo / MySQL EF Core |
| **Authentication** | JSON Web Tokens (JWT) with HMAC-SHA256, BCrypt password hashing |
| **Database** | MySQL 8.0+ or MariaDB 10.5+ (`pet_ecommerce_db`) |
| **Frontend** | Semantic HTML5, Vanilla JavaScript (ES6 Modules), Tailwind CSS |
| **Documentation** | Swagger / OpenAPI 3.0 UI |
| **Testing** | Python 3 Automated Test Suite (HTTP client, touch layout matrix) |

---

## ⚙️ Prerequisites

Before running PawHaven BD locally, ensure you have the following installed:

- **[.NET 10 SDK](https://dotnet.microsoft.com/download)** (or .NET 8 SDK)
- **[MySQL Server 8.0+](https://dev.mysql.com/downloads/)** or **[MariaDB 10.5+](https://mariadb.org/)**
- **[Python 3.8+](https://www.python.org/)** *(required only for running the test suite)*
- A local HTTP static server (e.g., Python `http.server`, Node `npx serve`, or VS Code Live Server)

---

## 🚀 Getting Started

### 1. Database Initialization

Ensure your MySQL/MariaDB server is running. Then deploy the complete schema and seed data using one of the following methods:

**Method A: Automated Shell Script (Linux / macOS)**
```bash
cd sql
./setup_database.sh
```

**Method B: MySQL Client CLI (Cross-Platform)**
```bash
# From the repository root:
mysql -u root -p < sql/run_all.sql
```

This creates the `pet_ecommerce_db` database, configures all 25 tables, triggers, views, indexes, and loads sample catalog data (products, brands, categories, coupons, and user accounts).

---

### 2. Backend API Setup

1. Configure your database connection in `backend/appsettings.json` (or set environment variables):
   ```json
   {
     "ConnectionStrings": {
       "DefaultConnection": "Server=127.0.0.1;Port=3306;Database=pet_ecommerce_db;User=root;Password=YOUR_DATABASE_PASSWORD;CharSet=utf8mb4;"
     }
   }
   ```
   > 💡 **Tip:** You can also set the `MYSQL_CONNECTION_STRING` or `ConnectionStrings__DefaultConnection` environment variable without modifying files.

2. Build and start the backend API:
   ```bash
   cd backend
   dotnet restore
   dotnet run
   ```

3. The API will start on:
   - **HTTP:** `http://localhost:5208`
   - **Swagger UI:** `http://localhost:5208/swagger`

---

### 3. Frontend Storefront Setup

The frontend is built using standard web technologies with no compilation or build step required. Simply serve the `frontend/` directory with any static web server:

**Using Python:**
```bash
cd frontend
python3 -m http.server 3000
```

**Using Node.js (`serve`):**
```bash
npx serve frontend -p 3000
```

Open your browser and navigate to:
```text
http://localhost:3000
```

---

## 📖 Interactive Swagger API Documentation

When the backend is running, access the interactive Swagger UI at:
```text
http://localhost:5208/swagger
```

### Authenticating in Swagger
1. Call `POST /api/auth/login` using one of the demo accounts listed below.
2. Copy the returned `token` from the response JSON.
3. Click the **Authorize** button at the top right of Swagger UI.
4. Enter `Bearer <YOUR_TOKEN>` and click **Authorize**.
5. All protected customer and administrative endpoints will now be accessible for interactive testing.

---

## 👥 Demo User Accounts

The database comes pre-seeded with ready-to-use demo accounts for all roles:

| Role | Email | Password | Permissions |
| :--- | :--- | :--- | :--- |
| **Platform Administrator** | `admin@pethavenbd.com` | `Password@123` | Full administrative control, catalog CRUD, sales dashboard |
| **Operations / Staff** | `farhana.support@pethavenbd.com` | `Password@123` | Order status processing, inventory management |
| **Customer** | `zayan.chowdhury@example.com` | `Password@123` | Cart, wishlist, orders, pet profiles |
| **Customer (Demo Fill)** | `customer@pawhaven.bd` | `Customer123!` | Storefront 1-click quick login button |

> 🔑 On the login page (`login.html`), convenient **1-Click Demo Fill** buttons are available to instantly test Admin, Staff, or Customer personas.

---

## 🧪 Testing & QA Automation

The repository includes a comprehensive automated verification suite (`tests/test_suite.py`) covering 33 end-to-end operational test cases:

```bash
# Ensure both backend (port 5208) and frontend (port 3000) are running, then run:
python3 tests/test_suite.py
```

### Verified Test Domains:
- **Domain 1: Authentication & User State** (Demo fill buttons, login, register, token issuance, session purge)
- **Domain 2: Search & Discovery** (Debounced autocomplete, empty state fallbacks, catalog querying)
- **Domain 3: Catalog Browsing & Species Taxonomy** (Categories, pet types, product details, stock limits)
- **Domain 4: Cart & Persistent Operations** (Add to cart, quantity steppers, removals, subtotal calculations)
- **Domain 5: Promotional Coupons** (Valid discount codes, invalid codes, minimum order threshold checks)
- **Domain 6: 13-Step Transactional Checkout** (Order placement, inventory reduction, cancellation rollback)
- **Domain 7: "My Pet" Profiles & Dynamic Recommendations** (Pet registration, profile lookup, tailored feed)
- **Domain 8: Smartphone Touch Ergonomics** (44px tap targets, mobile drawer, touch ribbons, bottom navigation)

---

## 🔧 Configuration & Environment Variables

The backend can be configured via `appsettings.json`, `appsettings.Development.json`, or environment variables:

| Variable | Description | Default |
| :--- | :--- | :--- |
| `ConnectionStrings__DefaultConnection` | MySQL connection string | `Server=127.0.0.1;Port=3306;...` |
| `MYSQL_CONNECTION_STRING` | Fallback MySQL connection string | None |
| `Jwt__SecretKey` | HMAC-SHA256 Secret Key (≥ 32 bytes) | Configured in appsettings |
| `Jwt__Issuer` | JWT Token Issuer | `PetHavenApi` |
| `Jwt__Audience` | JWT Token Audience | `PetHavenClient` |
| `Jwt__ExpiryInMinutes` | Access token lifespan | `120` |
| `Jwt__RefreshTokenExpiryInDays` | Refresh token lifespan | `14` |
| `ASPNETCORE_URLS` | Hosting URLs / ports | `http://localhost:5208` |

See [`.env.example`](.env.example) and [`backend/appsettings.Example.json`](backend/appsettings.Example.json) for configuration templates.

---

## 🤝 Contributing

Contributions are welcome! Please follow these steps:

1. Fork the repository.
2. Create a feature branch: `git checkout -b feature/your-feature-name`.
3. Commit your changes following conventional commit syntax: `git commit -m "feat: add your feature"`.
4. Push to your branch: `git push origin feature/your-feature-name`.
5. Open a Pull Request.

Please see [CONTRIBUTING.md](CONTRIBUTING.md) for details on code style and guidelines.

---

## 📄 License

This project is licensed under the MIT License — see the [LICENSE](LICENSE) file for details.
