# Prompt: Mobile-First Smartphone Responsive Overhaul & Strict Operational Testing for PawHaven BD

---

## 📌 Context & Role Definition
You are the **Senior Mobile Frontend Architect** and **Principal E-Commerce QA Automation Engineer** for **PawHaven BD**—a pet e-commerce platform built with **HTML5, CSS3, Vanilla JavaScript (ES modules)**, and **Tailwind CSS**, connected to an **ASP.NET Core Web API (port 5208)** running on **MySQL/MariaDB**.

Your primary mission is twofold:
1. **Mobile-First UX/UI Overhaul:** Transform the entire storefront into a smartphone-optimized, touch-friendly, high-converting mobile web application that looks and behaves like an app across all smartphone viewports (360px to 430px) without breaking tablet and desktop layouts.
2. **Strict Operational Testing:** Rigorously verify and test 100% of all user flows, forms, cart operations, coupons, order placement, and API interactions under simulated mobile conditions and real network responses.

---

## 📱 Part 1: Smartphone Responsive Architecture & UX Guidelines

### 1. Viewport Matrix & Breakpoints
The mobile experience must render without horizontal layout overflow (`overflow-x: hidden`), broken grids, or truncated critical texts on:
* **Small Phones (360px – 375px):** Samsung Galaxy S-series (360px), iPhone SE / iPhone mini (375px).
* **Standard Phones (390px – 414px):** iPhone 12/13/14/15/16 Pro (393px), Google Pixel 7/8 (412px), iPhone Plus/Max (414px–430px).
* **Foldables & Small Tablets (600px – 768px):** Ensure smooth transition between single-column and multi-column layouts.

### 2. Mobile Navigation & Touch Ergonomics
* **Sticky Bottom Navigation Bar:**
  * Implement a sticky bottom bar (`fixed bottom-0 left-0 right-0 z-40 bg-white/95 backdrop-blur-md border-t border-slate-200/80`) visible only on mobile screens (`lg:hidden`).
  * Include 5 touch-optimized navigation tabs with labels and minimum 48×48px tap targets:
    1. **Home** (`index.html`) — Home icon.
    2. **Shop / Categories** (`shop.html`) — Grid / catalog icon.
    3. **Search** — Focuses the search input or opens an instant full-screen search sheet.
    4. **Cart** (`cart.html`) — Cart icon with dynamic badge count pill.
    5. **Account** (`account.html` or `login.html`) — User avatar / sign-in icon.
  * Add bottom safe-area padding (`pb-safe` or `pb-16` / `pb-20` on `<main>`) to prevent footer content from being covered by the sticky bottom bar or mobile system gesture navigation bars.
* **Sticky Compact Header:**
  * Compress header vertical padding on mobile (`py-2.5 px-4`).
  * Include brand logo, mobile drawer toggle button, and quick wishlist/cart counters.
  * Provide a compact search bar with debounced (300ms) autocomplete dropdown that fits within mobile screen bounds.
* **Slide-Over Mobile Drawer:**
  * Smooth slide-in from the left with backdrop dimming (`bg-slate-900/60`).
  * Include species navigation with icons (🐱 Cats, 🐶 Dogs, 🦜 Birds, 🐰 Rabbits, 🐹 Small Pets).
  * Direct links to Order Tracking, My Pets, Blog, Helpline, and Sign In / Sign Out.

### 3. Touch Optimization & Form Inputs
* **Tap Targets:** All clickable links, buttons, counter steppers, and checkboxes must have a minimum bounding tap area of **44×44px**.
* **iOS Zoom Prevention:** Set `font-size: 16px` (`text-base` or minimum `1rem`) on all mobile `<input>`, `<select>`, and `<textarea>` elements to prevent automatic mobile Safari viewport zooming upon focus.
* **Touch Carousels & Horizontal Scrolling:**
  * Convert species buttons, category pills, and thumbnail galleries into horizontally swipeable ribbons with `-webkit-overflow-scrolling: touch;` and hidden scrollbars (`scrollbar-none`).
* **Filters Drawer on Mobile (`shop.html`):**
  * On mobile devices, hide the desktop sidebar and provide a prominent floating or top bar "Filters" button with active badge count.
  * Slide open a dedicated bottom sheet or right drawer containing pet species, price ranges, and checkboxes.
  * Include sticky bottom "Apply Filters" and "Reset All" buttons inside the drawer.

---

## 🧪 Part 2: Strict Operational Testing Matrix

Execute and verify the following operational tests sequentially against the running backend (`http://localhost:5208`) and frontend (`http://localhost:3000`). Every single test must pass without console errors or layout breaks.

### Test Domain 1: Authentication & User State
* [ ] **TC-1.1: 1-Click Demo Fill:** In `login.html`, click "Customer", "Staff", and "Admin" buttons. Verify form fields populate instantly.
* [ ] **TC-1.2: Customer Login:** Submit login with `customer@pawhaven.bd` / `Customer123!`. Verify:
  * API returns HTTP 200 with JWT token and user profile.
  * JWT is saved in `localStorage.pawhaven_token`.
  * Header immediately reflects user name/initial avatar.
  * User is redirected to `account.html` (or `?redirect=` target).
* [ ] **TC-1.3: New Account Registration:** In `register.html`, submit a new user with valid name, email, phone, and password. Verify validation, successful creation in MariaDB, and instant session start.
* [ ] **TC-1.4: Sign Out Flow:** Click "Sign Out" from mobile drawer or account dropdown. Verify token is purged, header switches back to "Sign In", and cart badge updates.

### Test Domain 2: Search & Catalog Discovery
* [ ] **TC-2.1: Live Autocomplete:** Type "cat", "royal", or "litter" into the mobile search bar. Verify:
  * Dropdown opens within 300ms with thumbnail, title, category, and price.
  * Tapping a suggestion navigates directly to `product.html?id=...`.
  * Tapping outside dismisses the dropdown.
* [ ] **TC-2.2: Species & Category Filters:** In `shop.html`, select "Cats", then "Dry Cat Food". Verify:
  * Products grid dynamically reloads only matching feline dry food products.
  * Product counter updates accurately (e.g., "Showing 1–12 of 16 products").
  * Active filter chips appear and can be cleared individually.
* [ ] **TC-2.3: Sorting Dropdown:** Change sorting to "Price: Low to High", then "Price: High to Low", then "Most Popular". Verify products order updates immediately without reloading the page.
* [ ] **TC-2.4: Pagination:** Navigate to Page 2. Verify page scrolls smoothly to top and renders the next page of items.

### Test Domain 3: Product Details & Variant Selection
* [ ] **TC-3.1: Detail Page Rendering:** Open `product.html?id=2` (Royal Canin Kitten). Verify image, title, rating stars, reviews count, and stock indicator load from API.
* [ ] **TC-3.2: Mobile Image Gallery:** Tap secondary thumbnail images. Verify main image changes immediately without page jump.
* [ ] **TC-3.3: Variant Switcher:** Tap different package size/weight variant buttons (e.g., 400g vs 2kg). Verify:
  * Unit price dynamically updates.
  * SKU code updates.
  * Stock availability indicator updates.
* [ ] **TC-3.4: Quantity Stepper:** Test `−` and `+` buttons. Verify value cannot drop below 1 or exceed 99.
* [ ] **TC-3.5: Add to Cart:** Tap "Add to Cart". Verify:
  * Toast notification "Product added to cart!" appears with checkmark icon.
  * Header and bottom navigation cart badge increments by selected quantity.

### Test Domain 4: Cart Management & Promotional Coupons
* [ ] **TC-4.1: Line Items Table:** Open `cart.html` on mobile viewport (375px). Verify card layout is vertically responsive with no horizontal clipping.
* [ ] **TC-4.2: Quantity Modification:** Increment and decrement items in cart. Verify line totals and subtotal update in real-time.
* [ ] **TC-4.3: Item Deletion:** Remove an item. Verify item fades out, subtotal recalculates, and badge count decreases.
* [ ] **TC-4.4: Delivery Fee Calculation:** Switch between "Inside Dhaka (৳ 80)" and "Outside Dhaka (৳ 150)". Verify estimated shipping and grand total update automatically.
* [ ] **TC-4.5: Valid Coupon Application:**
  * Enter coupon code `PAW10` or `WELCOME10` on an order over ৳ 1,000. Verify green confirmation message appears and 10% discount is deducted from grand total.
* [ ] **TC-4.6: Invalid Coupon Rejection:**
  * Enter an expired or non-existent coupon (e.g. `INVALID99`). Verify error message is rendered without breaking form state.

### Test Domain 5: Multi-Step Checkout & Order Placement
* [ ] **TC-5.1: Address Completion:** In `checkout.html`, fill in Bangladeshi delivery fields (Division, District, Thana/Area, Street Address, Mobile Phone).
* [ ] **TC-5.2: Payment Methods:** Select between **Cash on Delivery (COD)**, **bKash**, and **Nagad**. Verify radio highlights properly.
* [ ] **TC-5.3: Order Placement:** Click "Confirm & Place Order". Verify:
  * API call `POST /api/orders` executes with user payload.
  * Server responds with HTTP 201 Created and new Order Number (`ORD-202610-XXXX`).
  * User is redirected to `order-details.html?id=...`.
  * Shopping cart is cleared.

### Test Domain 6: Order Tracking & Status Stepper
* [ ] **TC-6.1: Delivery Progress Stepper:** Open `order-details.html?id=...`. Verify 4-stage stepper (Placed → Confirmed → Shipped → Delivered) reflects the actual database status.
* [ ] **TC-6.2: Line Items & Invoice Breakdown:** Verify item thumbnails, quantities, subtotal, discount, delivery fee, and total amount match the placed order.
* [ ] **TC-6.3: Pending Order Cancellation:** If order status is `pending`, verify "Cancel Order" button is visible. Click it, confirm prompt, and verify status updates to `cancelled`.

### Test Domain 7: "My Pet" Profiles & Recommendation Engine
* [ ] **TC-7.1: Add Pet Modal:** In `my-pets.html`, click "+ Add New Pet". Verify modal opens centered on mobile without cutting off inputs.
* [ ] **TC-7.2: Pet Profile Creation:** Enter pet name, species (Cat, Dog, Bird, Rabbit), breed, and weight. Submit form. Verify pet card appears in grid.
* [ ] **TC-7.3: Dynamic Recommendations:** Click "View Recommended Products" on the pet card. Verify the recommendations grid below updates with veterinary formulas matching that pet's species.

---

## 🛠️ Part 3: Expected Implementation Tasks

When executing this prompt, carry out the following work:
1. **Audit All 17 HTML Files:** Verify every page contains `<meta name="viewport" content="width=device-width, initial-scale=1.0">` and clean responsive utility classes (`w-full`, `max-w-full`, `overflow-hidden`).
2. **Implement Universal Mobile Sticky Bottom Navigation:** Inject the bottom bar in `frontend/components/layout.js` so it automatically protects and serves every page.
3. **Verify Mobile Filter Drawer:** Ensure `shop.html` filter drawer operates smoothly with touch backdrop dismiss and lock body scroll when open.
4. **Conduct Responsive Touch Audits:** Check that buttons, inputs, steppers, and modals fit neatly on 360px and 375px screens with zero horizontal overflow.
5. **Run End-to-End API Verifications:** Perform curl / automated tests across the auth, products, coupons, and orders endpoints to ensure 100% operational integrity.
</USER_REQUEST>
<ADDITIONAL_METADATA>
The current local time is: 2026-10-12T17:48:55+06:00.
</ADDITIONAL_METADATA>