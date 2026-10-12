# Pet Haven BD - Backend REST API Specification & Developer Guide

A production-grade **ASP.NET Core Web API** backend engineered for the **Pet Haven BD** e-commerce platform. It communicates directly with the normalized MySQL 8+ database located in `/sql`.

---

## 1. Tech Stack & Architecture

- **Framework:** ASP.NET Core Web API (.NET 10 / .NET 8 compatible)
- **Database Provider:** Entity Framework Core with `Pomelo.EntityFrameworkCore.MySql`
- **Database:** MySQL 8+ / MariaDB 10.5+ (`pet_ecommerce_db`)
- **Authentication:** JSON Web Tokens (JWT) with HMAC-SHA256 signature
- **Password Security:** Salted BCrypt password hashing (`BCrypt.Net-Next`)
- **API Documentation:** Swagger / OpenAPI with interactive JWT Bearer authorization
- **Design Pattern:** Clean layered architecture (Controllers -> Services -> EF Core Data Context -> MySQL Database)

---

## 2. Directory Structure

```
backend/
├── Controllers/
│   ├── BaseApiController.cs           # Common claims extractor (UserId, Role, Admin check)
│   ├── AuthController.cs               # Register, Login, Refresh, Logout
│   ├── ProductsController.cs           # Product catalog, multi-criteria filtering, CRUD
│   ├── CategoriesController.cs         # Hierarchical category tree & subcategories
│   ├── BrandsController.cs             # Pet brands & brand product queries
│   ├── PetTypesController.cs           # Species (Cat, Dog, Bird, Rabbit, Small Pets)
│   ├── CartController.cs               # Customer cart additions, updates, removals
│   ├── WishlistController.cs           # Customer bookmarks & stock status
│   ├── OrdersController.cs             # Transactional checkout & order history
│   ├── AdminOrdersController.cs        # Order queues & workflow status transitions
│   ├── CouponsController.cs            # Promotional code validation engine
│   ├── ReviewsController.cs            # Star ratings, reviews & moderation
│   ├── PetsController.cs               # "My Pet" profile management
│   ├── RecommendationsController.cs    # Rule-based pet-profile product recommendations
│   ├── BlogController.cs               # Pet care guides & veterinary articles
│   ├── BannersController.cs            # Homepage sliders & promo campaigns
│   ├── NotificationsController.cs      # In-app customer order & restock alerts
│   ├── SearchController.cs             # Global keyword search across catalog
│   └── AdminDashboardController.cs     # Executive sales summary & inventory alerts
├── Models/                             # 25 Normalized Entity Framework Core entities
├── DTOs/                               # Strongly typed request/response transfer contracts
│   ├── Common/                         # ApiResponse<T>, PagedResult<T>, PaginationParams
│   ├── Auth/
│   ├── Products/
│   ├── Categories/
│   ├── Brands/
│   ├── PetTypes/
│   ├── Cart/
│   ├── Wishlist/
│   ├── Orders/
│   ├── Coupons/
│   ├── Reviews/
│   ├── Pets/
│   ├── Blog/
│   ├── Banners/
│   ├── Notifications/
│   └── Dashboard/
├── Data/
│   └── AppDbContext.cs                 # EF Core DbContext with foreign keys & indexes
├── Services/
│   ├── Interfaces/                     # Service abstractions (IProductService, IOrderService, etc.)
│   └── Implementations/                # Business logic & ACID transaction executions
├── Authentication/
│   ├── IJwtTokenGenerator.cs           # Access & refresh token generator
│   └── JwtTokenGenerator.cs
├── Configuration/
│   └── AppOptions.cs                   # Strongly typed JwtOptions & CorsOptions
├── Middleware/
│   └── ExceptionHandlingMiddleware.cs  # Centralized exception interception & error envelopes
├── Helpers/
│   └── PasswordHasher.cs               # BCrypt password hashing & verification
├── appsettings.json                    # Configuration file (Connection string, JWT, CORS)
├── appsettings.Development.json        # Verbose development logging & local overrides
└── Program.cs                          # Pipeline configuration, DI, JWT, and Swagger
```

---

## 3. Database Configuration & Connection String

### Where to configure the Connection String:
The connection string is configured in **`backend/appsettings.json`** (and overridden by **`backend/appsettings.Development.json`** or environment variables).

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=localhost;Port=3306;Database=pet_ecommerce_db;User=root;Password=YOUR_PASSWORD;CharSet=utf8mb4;TreatTinyAsBoolean=true;"
  }
}
```

### Connection Parameter Breakdown:
| Parameter | Description | Default / Example |
| :--- | :--- | :--- |
| **Server** | Hostname or IP address of your MySQL instance | `localhost` or `127.0.0.1` |
| **Port** | MySQL TCP port | `3306` |
| **Database** | Database name created from the SQL scripts | `pet_ecommerce_db` |
| **User** | MySQL username with CRUD & DDL privileges | `root` or `pet_user` |
| **Password** | Password for the MySQL user | `YOUR_SECURE_PASSWORD` |
| **CharSet** | Character set ensuring Bengali & UTF-8 emojis | `utf8mb4` |

### Environment Variable Alternative:
To keep credentials out of source control in production or CI/CD pipelines, define an environment variable:
```bash
export ConnectionStrings__DefaultConnection="Server=127.0.0.1;Port=3306;Database=pet_ecommerce_db;User=myuser;Password=mypassword;CharSet=utf8mb4;"
```

---

## 4. How to Initialize MySQL & Seed Data

Ensure MySQL is running on your machine, then execute the master script from the project root:

```bash
# Option 1: Automated shell script
cd sql
./setup_database.sh

# Option 2: MySQL client execution
mysql -u root -p < sql/run_all.sql
```

This sets up all 25 tables, performance indexes, 42 products, variants, brands, categories, coupons, and test orders.

---

## 5. How to Run the Backend API

1. Navigate to the backend directory:
   ```bash
   cd backend
   ```
2. Restore NuGet dependencies and build the project:
   ```bash
   dotnet restore
   dotnet build
   ```
3. Run the development server:
   ```bash
   dotnet run
   ```
4. The API will start on:
   - **HTTP:** `http://localhost:5208`
   - **HTTPS:** `https://localhost:7080`

---

## 6. Interactive Testing with Swagger UI

1. Open your browser and navigate to:
   ```
   http://localhost:5208/swagger
   ```
2. **Authenticate in Swagger:**
   - Call `POST /api/auth/login` with sample credentials:
     - **Admin Email:** `admin@pethavenbd.com`
     - **Customer Email:** `zayan.chowdhury@example.com`
     - **Password:** `Secret@1234` (or `Password@123`)
   - Copy the returned `token` from the response JSON.
   - Click the **Authorize** button at the top right of Swagger UI.
   - Enter `Bearer <YOUR_COPIED_TOKEN>` and click **Authorize**.
   - All protected customer and admin endpoints are now unlocked for testing.

---

## 7. Available API Endpoints Summary

### Authentication (`/api/auth`)
- `POST /api/auth/register`: Register new customer with auto-created cart and wishlist.
- `POST /api/auth/login`: Authenticate and receive JWT access token.
- `POST /api/auth/refresh`: Refresh expired access token.
- `POST /api/auth/logout`: Terminate session.

### Product Catalog (`/api/products`)
- `GET /api/products`: Filter products by `categoryId`, `petTypeId`, `brandId`, `minPrice`, `maxPrice`, `inStockOnly`, `onSaleOnly`, `featuredOnly`, `lifeStage`, `foodType`, `healthConcern`, and `sortBy` (`newest`, `popularity`, `price_asc`, `price_desc`, `rating`). **Includes pagination (`pageNumber`, `pageSize`).**
- `GET /api/products/{id}`: Detailed product view including gallery images and active variants.
- `GET /api/products/slug/{slug}`: SEO-friendly product details.
- `POST /api/products`: Create product *(Admin & Staff)*.
- `PUT /api/products/{id}`: Update product *(Admin & Staff)*.
- `DELETE /api/products/{id}`: Soft-delete/archive product *(Admin)*.

### Taxonomies & Lookups
- `GET /api/categories`: Hierarchical category tree (Root -> Category -> Subcategory).
- `GET /api/categories/{id}/products`: Products within category.
- `GET /api/brands`: Active pet brands with product counts.
- `GET /api/brands/{id}/products`: Products manufactured by brand.
- `GET /api/pet-types`: Pet species (Cat, Dog, Bird, Rabbit, Small Pets).
- `GET /api/pet-types/{id}/products`: Products for pet species.

### Cart & Wishlist (`/api/cart`, `/api/wishlist`)
- `GET /api/cart`: Current authenticated customer's cart.
- `POST /api/cart/items`: Add product/variant to cart with stock validation.
- `PUT /api/cart/items/{id}`: Update item quantity.
- `DELETE /api/cart/items/{id}`: Remove line item.
- `DELETE /api/cart`: Clear cart.
- `GET /api/wishlist`: Retrieve bookmarked products.
- `POST /api/wishlist/{productId}`: Add product to wishlist.
- `DELETE /api/wishlist/{productId}`: Remove product from wishlist.

### Orders & Checkout (`/api/orders`, `/api/admin/orders`)
- `POST /api/orders`: **13-Step Transactional Checkout:**
  1. Validates customer account status.
  2. Validates product availability.
  3. Validates selected variant.
  4. Checks stock limits.
  5. Computes accurate subtotal.
  6. Validates and applies coupon rules.
  7. Calculates delivery charge (80 BDT Dhaka, 150 BDT outside Dhaka).
  8. Calculates final grand total.
  9. Creates order entity with unique order number.
  10. Creates immutable line items snapshots.
  11. Reduces warehouse stock quantities.
  12. Records inventory transaction audit logs.
  13. Commits transaction atomically (rolls back automatically on any error).
- `GET /api/orders`: Paginated personal order history.
- `GET /api/orders/{id}`: Order line items and delivery status.
- `POST /api/orders/{id}/cancel`: Cancels pending orders and restores warehouse inventory.
- `GET /api/admin/orders`: Platform-wide order queue *(Admin & Staff)*.
- `PUT /api/admin/orders/{id}/status`: Transition status (`confirmed`, `processing`, `shipped`, `delivered`, `cancelled`) *(Admin & Staff)*.

### Promotional Coupons (`/api/coupons`)
- `POST /api/coupons/validate`: Evaluates coupon code, date range, minimum cart amount, and per-user redemption limits.

### Reviews & Ratings (`/api/reviews`)
- `GET /api/products/{productId}/reviews`: Approved star ratings (1–5) and customer feedback.
- `POST /api/products/{productId}/reviews`: Submit rating with verified buyer badge detection.
- `PUT /api/reviews/{id}`: Edit review.
- `DELETE /api/reviews/{id}`: Delete review.

### "My Pet" Profile (`/api/pets`)
- `GET /api/pets`: Registered pets (name, breed, birth date, weight, dietary needs, allergies).
- `POST /api/pets`: Register a new pet profile.
- `GET /api/pets/{id}`: Pet details.
- `PUT /api/pets/{id}`: Update pet profile.
- `DELETE /api/pets/{id}`: Remove pet profile.

### Rule-Based Recommendations (`/api/recommendations`)
- `GET /api/recommendations`: Evaluates customer's registered pets (`pet_profiles`), calculates their life stage from date of birth, factors in health flags (e.g. hairball or joint care), examines previous order categories, and ranks recommendations.

### Educational Blog (`/api/blog`)
- `GET /api/blog`: Published pet care guides.
- `GET /api/blog/{slug}`: Article detail.
- `POST /api/blog`: Publish new veterinary article *(Admin & Staff)*.

### Promotional Banners (`/api/banners`)
- `GET /api/banners`: Active homepage sliders and promotional campaign strips.

### Search (`/api/search`)
- `GET /api/search?q={query}`: Multi-field keyword search across titles, descriptions, and SKUs.

### Admin Dashboard Analytics (`/api/admin/dashboard`)
- `GET /api/admin/dashboard/stats`: High-level metrics:
  - Total users and customers
  - Total sales revenue and order counts
  - Pending and delivered orders
  - Low-stock and out-of-stock product warnings
  - Best-selling products by volume and revenue
  - Recent orders
  - Past 7-day daily sales revenue breakdown

---

## 8. Standard JSON API Response Envelope

Every endpoint returns a consistent JSON envelope:

### Success Response (`200 OK`, `201 Created`):
```json
{
  "success": true,
  "message": "Resource created successfully",
  "data": { ... },
  "statusCode": 201
}
```

### Error Response (`400 Bad Request`, `401 Unauthorized`, `404 Not Found`, `409 Conflict`, `500 Server Error`):
```json
{
  "success": false,
  "message": "Insufficient stock for product.",
  "errors": [
    "Available stock is 2, requested 5."
  ],
  "statusCode": 400
}
```

---

## 9. Frontend Integration Guide (React / Next.js / Vue)

### Base Axios Setup
```typescript
import axios from 'axios';

const api = axios.create({
  baseURL: 'http://localhost:5208/api',
  headers: {
    'Content-Type': 'application/json',
  },
});

// Attach JWT access token to requests
api.interceptors.request.use((config) => {
  const token = localStorage.getItem('token');
  if (token) {
    config.headers.Authorization = `Bearer ${token}`;
  }
  return config;
});

export default api;
```

### Example: Fetching Filtered Cat Products
```typescript
const fetchProducts = async (page = 1) => {
  const response = await api.get('/products', {
    params: {
      petTypeSlug: 'cat',
      categorySlug: 'cat-food',
      minPrice: 500,
      maxPrice: 3000,
      sortBy: 'popularity',
      pageNumber: page,
      pageSize: 20
    }
  });
  return response.data.data; // Returns PagedResult<ProductDto>
};
```
