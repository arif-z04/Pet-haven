# Contributing to PawHaven BD

Thank you for your interest in contributing to **PawHaven BD**! We welcome contributions ranging from bug fixes and documentation enhancements to major feature additions.

---

## 🧭 Code of Conduct

Please be respectful and considerate in all interactions within this project community.

---

## 🛠️ Development Workflow

### 1. Fork & Clone
1. Fork the repository on GitHub.
2. Clone your fork locally:
   ```bash
   git clone https://github.com/<your-username>/PetShop.git
   cd PetShop
   ```

### 2. Branching Strategy
Create a meaningful branch name following conventional prefixes:
- `feature/description` for new features
- `fix/description` for bug fixes
- `docs/description` for documentation improvements
- `refactor/description` for code refactoring

```bash
git checkout -b feature/awesome-feature
```

### 3. Setting Up the Environment
1. Ensure you have the prerequisites installed:
   - .NET 10 SDK (or .NET 8 SDK)
   - MySQL 8.0+ or MariaDB 10.5+
   - Python 3.8+ (for testing)
2. Deploy the database schema using `sql/setup_database.sh` or `sql/run_all.sql`.
3. Set your local connection string in `backend/appsettings.Development.json` or through the `MYSQL_CONNECTION_STRING` environment variable.
4. Verify the backend builds cleanly:
   ```bash
   dotnet build backend
   ```

### 4. Code Standards & Style
- **C# / Backend:**
  - Follow standard .NET naming conventions (PascalCase for classes, methods, properties; camelCase for parameters and local variables).
  - Use strongly typed DTOs and maintain controller-service separation.
  - Keep sensitive settings out of committed files.
- **Frontend:**
  - Write standard vanilla JavaScript ES Modules.
  - Follow Tailwind CSS utility patterns for responsiveness.
  - Ensure minimum 44×44px tap targets for mobile usability.
- **SQL:**
  - Keep SQL scripts idempotent using `IF NOT EXISTS` and `ON DUPLICATE KEY UPDATE` where appropriate.

### 5. Running Tests
Before submitting a pull request, run the test suite to ensure no regressions:
```bash
python3 tests/test_suite.py
```

### 6. Commit Messages
We encourage [Conventional Commits](https://www.conventionalcommits.org/):
- `feat: add product comparison view`
- `fix: resolve cart total rounding edge case`
- `docs: update setup instructions in README`
- `refactor: optimize EF Core query in OrderService`

---

## 📬 Submitting a Pull Request

1. Push your branch to GitHub:
   ```bash
   git push origin feature/awesome-feature
   ```
2. Open a Pull Request against the `main` branch.
3. Fill out the pull request template with context, changes made, and verification steps.
4. Address any review feedback promptly.
