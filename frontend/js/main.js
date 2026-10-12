/**
 * PawHaven BD - Global Utilities, UI Helpers, and Core Components
 */

// Toast notification helper
window.showToast = function(message, type = "success", duration = 3500) {
  let container = document.getElementById("toast-container");
  if (!container) {
    container = document.createElement("div");
    container.id = "toast-container";
    document.body.appendChild(container);
  }

  const toast = document.createElement("div");
  toast.className = `toast toast-${type}`;

  let iconSvg = "";
  if (type === "success") {
    iconSvg = `<svg class="w-5 h-5 text-emerald-500 shrink-0 mt-0.5" fill="none" stroke="currentColor" viewBox="0 0 24 24"><path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M9 12l2 2 4-4m6 2a9 9 0 11-18 0 9 9 0 0118 0z"/></svg>`;
  } else if (type === "error") {
    iconSvg = `<svg class="w-5 h-5 text-red-500 shrink-0 mt-0.5" fill="none" stroke="currentColor" viewBox="0 0 24 24"><path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M12 8v4m0 4h.01M21 12a9 9 0 11-18 0 9 9 0 0118 0z"/></svg>`;
  } else if (type === "warning") {
    iconSvg = `<svg class="w-5 h-5 text-amber-500 shrink-0 mt-0.5" fill="none" stroke="currentColor" viewBox="0 0 24 24"><path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M12 9v2m0 4h.01m-6.938 4h13.856c1.54 0 2.502-1.667 1.732-3L13.732 4c-.77-1.333-2.694-1.333-3.464 0L3.34 16c-.77 1.333.192 3 1.732 3z"/></svg>`;
  } else {
    iconSvg = `<svg class="w-5 h-5 text-teal-500 shrink-0 mt-0.5" fill="none" stroke="currentColor" viewBox="0 0 24 24"><path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M13 16h-1v-4h-1m1-4h.01M21 12a9 9 0 11-18 0 9 9 0 0118 0z"/></svg>`;
  }

  toast.innerHTML = `
    ${iconSvg}
    <div class="flex-1 text-sm font-medium text-slate-800 leading-snug">${message}</div>
    <button class="text-slate-400 hover:text-slate-600 ml-2" onclick="this.parentElement.remove()">
      <svg class="w-4 h-4" fill="none" stroke="currentColor" viewBox="0 0 24 24"><path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M6 18L18 6M6 6l12 12"/></svg>
    </button>
  `;

  container.appendChild(toast);
  requestAnimationFrame(() => toast.classList.add("show"));

  setTimeout(() => {
    toast.classList.remove("show");
    setTimeout(() => toast.remove(), 300);
  }, duration);
};

// Format BDT currency
window.formatPrice = function(amount) {
  if (amount === null || amount === undefined || isNaN(amount)) return "৳ 0";
  return `৳ ${Number(amount).toLocaleString('en-IN', { minimumFractionDigits: 0, maximumFractionDigits: 0 })}`;
};

// Render star ratings
window.renderRatingStars = function(rating, reviewCount = null) {
  const rounded = Math.round((rating || 0) * 2) / 2;
  let starsHtml = "";

  for (let i = 1; i <= 5; i++) {
    if (i <= rounded) {
      starsHtml += `<svg class="w-4 h-4 fill-amber-400 text-amber-400" viewBox="0 0 20 20"><path d="M9.049 2.927c.3-.921 1.603-.921 1.902 0l1.07 3.292a1 1 0 00.95.69h3.462c.969 0 1.371 1.24.588 1.81l-2.8 2.034a1 1 0 00-.364 1.118l1.07 3.292c.3.921-.755 1.688-1.54 1.118l-2.8-2.034a1 1 0 00-1.175 0l-2.8 2.034c-.784.57-1.838-.197-1.539-1.118l1.07-3.292a1 1 0 00-.364-1.118L2.98 8.72c-.783-.57-.38-1.81.588-1.81h3.461a1 1 0 00.951-.69l1.07-3.292z"/></svg>`;
    } else {
      starsHtml += `<svg class="w-4 h-4 fill-slate-200 text-slate-200" viewBox="0 0 20 20"><path d="M9.049 2.927c.3-.921 1.603-.921 1.902 0l1.07 3.292a1 1 0 00.95.69h3.462c.969 0 1.371 1.24.588 1.81l-2.8 2.034a1 1 0 00-.364 1.118l1.07 3.292c.3.921-.755 1.688-1.54 1.118l-2.8-2.034a1 1 0 00-1.175 0l-2.8 2.034c-.784.57-1.838-.197-1.539-1.118l1.07-3.292a1 1 0 00-.364-1.118L2.98 8.72c-.783-.57-.38-1.81.588-1.81h3.461a1 1 0 00.951-.69l1.07-3.292z"/></svg>`;
    }
  }

  const countStr = reviewCount !== null ? `<span class="text-xs text-slate-500 font-medium ml-1">(${reviewCount})</span>` : "";
  return `<div class="rating-stars inline-flex items-center">${starsHtml}${countStr}</div>`;
};

// Fallback high-resolution SVG placeholder generator for pet products
window.getProductImagePlaceholder = function(title = "Pet Product") {
  const cleanTitle = encodeURIComponent((title || "Pet Product").slice(0, 24));
  return `data:image/svg+xml;utf8,<svg xmlns="http://www.w3.org/2000/svg" width="400" height="400" viewBox="0 0 400 400"><rect width="400" height="400" fill="%23f1f5f9"/><circle cx="200" cy="180" r="70" fill="%23ccfbf1"/><path d="M175,170 C165,150 155,170 170,185 C185,170 175,150 175,170 Z M225,170 C215,150 205,170 220,185 C235,170 225,150 225,170 Z M200,195 C190,195 185,210 200,215 C215,210 210,195 200,195 Z" fill="%230d9488"/><text x="200" y="300" font-family="sans-serif" font-size="16" font-weight="bold" fill="%230f766e" text-anchor="middle">${cleanTitle}</text></svg>`;
};

// Safe Image Url resolver (handles backend paths or SVG fallback)
window.resolveImageUrl = function(url, title = "Product") {
  if (!url) return window.getProductImagePlaceholder(title);
  if (url.startsWith("http://") || url.startsWith("https://") || url.startsWith("data:")) {
    return url;
  }
  // If it's a relative asset that might not exist on disk, return the graceful placeholder
  return window.getProductImagePlaceholder(title);
};

// Update Cart Badge Count
window.updateCartBadge = async function() {
  const badge = document.getElementById("cart-badge-count");
  const mobileBadge = document.getElementById("mobile-bottom-cart-badge");
  
  function applyCount(count) {
    if (badge) {
      badge.textContent = count;
      badge.classList.toggle("hidden", count === 0);
    }
    if (mobileBadge) {
      mobileBadge.textContent = count;
      mobileBadge.classList.toggle("hidden", count === 0);
    }
  }

  if (window.AuthManager && window.AuthManager.isAuthenticated()) {
    try {
      const res = await window.api.getCart();
      if (res.success && res.data) {
        applyCount(res.data.totalItems || 0);
        return;
      }
    } catch (e) {
      console.warn("Cart badge fetch failed:", e);
    }
  }

  // Local storage fallback for unauthenticated cart counter
  const localCart = JSON.parse(localStorage.getItem("pawhaven_local_cart") || "[]");
  const count = localCart.reduce((acc, item) => acc + (item.quantity || 1), 0);
  applyCount(count);
};

// Update Wishlist Badge Count
window.updateWishlistBadge = async function() {
  const badge = document.getElementById("wishlist-badge-count");
  if (!badge) return;

  if (window.AuthManager && window.AuthManager.isAuthenticated()) {
    try {
      const res = await window.api.getWishlist();
      if (res.success && res.data) {
        const count = res.data.items?.length || 0;
        badge.textContent = count;
        badge.classList.toggle("hidden", count === 0);
        return;
      }
    } catch (e) {
      console.warn("Wishlist badge fetch failed:", e);
    }
  }

  const localWishlist = JSON.parse(localStorage.getItem("pawhaven_local_wishlist") || "[]");
  const count = localWishlist.length;
  badge.textContent = count;
  badge.classList.toggle("hidden", count === 0);
};

// Global Quick View Modal
window.openQuickView = async function(productId) {
  let modal = document.getElementById("quick-view-modal");
  if (!modal) {
    modal = document.createElement("div");
    modal.id = "quick-view-modal";
    modal.className = "fixed inset-0 z-50 flex items-center justify-center p-4 hidden";
    modal.innerHTML = `
      <div class="fixed inset-0 bg-slate-900/60 backdrop-blur-sm" onclick="window.closeQuickView()"></div>
      <div class="relative bg-white rounded-2xl max-w-2xl w-full p-6 shadow-2xl z-10 max-h-[90vh] overflow-y-auto" id="quick-view-content">
        <div class="flex items-center justify-center py-12"><div class="w-8 h-8 border-4 border-teal-500 border-t-transparent rounded-full animate-spin"></div></div>
      </div>
    `;
    document.body.appendChild(modal);
  }

  modal.classList.remove("hidden");
  const content = document.getElementById("quick-view-content");
  content.innerHTML = `<div class="flex items-center justify-center py-12"><div class="w-8 h-8 border-4 border-teal-500 border-t-transparent rounded-full animate-spin"></div></div>`;

  try {
    const res = await window.api.getProduct(productId);
    if (!res.success || !res.data) {
      content.innerHTML = `<div class="p-6 text-center text-red-600">Failed to load product details.</div>`;
      return;
    }

    const p = res.data;
    const imgUrl = window.resolveImageUrl(p.primaryImageUrl, p.name);

    content.innerHTML = `
      <button class="absolute top-4 right-4 text-slate-400 hover:text-slate-600 p-1" onclick="window.closeQuickView()">
        <svg class="w-6 h-6" fill="none" stroke="currentColor" viewBox="0 0 24 24"><path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M6 18L18 6M6 6l12 12"/></svg>
      </button>

      <div class="grid grid-cols-1 md:grid-cols-2 gap-6 pt-2">
        <div class="aspect-square bg-slate-50 rounded-xl overflow-hidden border border-slate-100 flex items-center justify-center">
          <img src="${imgUrl}" alt="${p.name}" class="w-full h-full object-contain p-4">
        </div>

        <div class="flex flex-col">
          <span class="text-xs font-bold text-teal-600 uppercase tracking-wider mb-1">${p.categoryName || 'Pet Care'}</span>
          <h3 class="text-xl font-bold text-slate-900 leading-snug mb-2">${p.name}</h3>
          
          <div class="mb-3">${window.renderRatingStars(p.averageRating, p.reviewCount)}</div>

          <div class="flex items-baseline gap-3 mb-4">
            <span class="text-2xl font-extrabold text-slate-900">${window.formatPrice(p.basePrice)}</span>
            ${p.compareAtPrice ? `<span class="text-sm text-slate-400 line-through">${window.formatPrice(p.compareAtPrice)}</span>` : ''}
            ${p.discountPercentage ? `<span class="px-2 py-0.5 text-xs font-bold rounded-md bg-red-50 text-red-600">Save ${p.discountPercentage}%</span>` : ''}
          </div>

          <p class="text-sm text-slate-600 mb-6 line-clamp-3">${p.shortDescription || 'High quality nutrition and care supplies tested for complete pet vitality.'}</p>

          <div class="mt-auto space-y-3">
            <a href="product.html?id=${p.id}" class="w-full text-center block bg-teal-600 hover:bg-teal-700 text-white font-semibold py-3 px-4 rounded-xl transition shadow-sm">
              View Full Details
            </a>
          </div>
        </div>
      </div>
    `;
  } catch (err) {
    content.innerHTML = `<div class="p-6 text-center text-red-600">Error loading product.</div>`;
  }
};

window.closeQuickView = function() {
  const modal = document.getElementById("quick-view-modal");
  if (modal) modal.classList.add("hidden");
};

// Global Add to Cart wrapper
window.handleAddToCart = async function(productId, variantId = null, quantity = 1) {
  if (window.AuthManager && window.AuthManager.isAuthenticated()) {
    const res = await window.api.addToCart(productId, variantId, quantity);
    if (res.success) {
      window.showToast("Product added to cart!", "success");
      window.updateCartBadge();
    } else {
      window.showToast(res.message || "Failed to add to cart.", "error");
    }
  } else {
    // Guest cart storage in localStorage
    const cart = JSON.parse(localStorage.getItem("pawhaven_local_cart") || "[]");
    const existing = cart.find(i => i.productId == productId && i.variantId == variantId);
    if (existing) {
      existing.quantity += quantity;
    } else {
      cart.push({ productId, variantId, quantity });
    }
    localStorage.setItem("pawhaven_local_cart", JSON.stringify(cart));
    window.showToast("Product added to cart!", "success");
    window.updateCartBadge();
  }
};

// Global Add to Wishlist wrapper
window.handleToggleWishlist = async function(productId) {
  if (window.AuthManager && window.AuthManager.isAuthenticated()) {
    const res = await window.api.addToWishlist(productId);
    if (res.success) {
      window.showToast("Added to your wishlist!", "success");
      window.updateWishlistBadge();
    } else {
      window.showToast(res.message || "Could not update wishlist.", "error");
    }
  } else {
    const wishlist = JSON.parse(localStorage.getItem("pawhaven_local_wishlist") || "[]");
    const index = wishlist.indexOf(productId);
    if (index > -1) {
      wishlist.splice(index, 1);
      window.showToast("Removed from wishlist.", "info");
    } else {
      wishlist.push(productId);
      window.showToast("Added to your wishlist!", "success");
    }
    localStorage.setItem("pawhaven_local_wishlist", JSON.stringify(wishlist));
    window.updateWishlistBadge();
  }
};

// Render universal Product Card HTML string
window.renderProductCard = function(product) {
  const p = product;
  const imgUrl = window.resolveImageUrl(p.primaryImageUrl, p.name);
  const isOutOfStock = p.stockQuantity <= 0;
  const isLowStock = p.stockQuantity > 0 && p.stockQuantity <= 5;
  const hasDiscount = p.compareAtPrice && p.compareAtPrice > p.basePrice;

  return `
    <div class="product-card group relative bg-white rounded-2xl border border-slate-200/80 shadow-sm hover:shadow-xl hover:border-teal-500/40 transition-all duration-300 flex flex-col overflow-hidden">
      <!-- Card Image & Floating Badges -->
      <div class="product-card-image-wrap relative aspect-square bg-slate-50 flex items-center justify-center p-4">
        <a href="product.html?id=${p.id}" class="w-full h-full flex items-center justify-center">
          <img src="${imgUrl}" alt="${p.name}" loading="lazy" class="max-h-full max-w-full object-contain">
        </a>

        <!-- Badges -->
        <div class="absolute top-3 left-3 flex flex-col gap-1.5 z-10 pointer-events-none">
          ${hasDiscount ? `<span class="badge-sale px-2.5 py-1 text-[11px] font-extrabold rounded-lg uppercase tracking-wide">-${p.discountPercentage}% OFF</span>` : ''}
          ${p.isFeatured && !hasDiscount ? `<span class="badge-featured px-2.5 py-1 text-[11px] font-extrabold rounded-lg uppercase tracking-wide">Featured</span>` : ''}
          ${isOutOfStock ? `<span class="bg-slate-800 text-white px-2.5 py-1 text-[11px] font-extrabold rounded-lg uppercase tracking-wide">Out of Stock</span>` : ''}
        </div>

        <!-- Floating Quick Action Buttons -->
        <div class="absolute top-3 right-3 flex flex-col gap-2 opacity-0 group-hover:opacity-100 transition-opacity duration-200 z-10">
          <button onclick="window.handleToggleWishlist(${p.id})" title="Add to Wishlist" class="w-9 h-9 rounded-full bg-white text-slate-600 hover:text-red-500 hover:bg-red-50 shadow-md flex items-center justify-center transition">
            <svg class="w-4 h-4" fill="none" stroke="currentColor" viewBox="0 0 24 24"><path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M4.318 6.318a4.5 4.5 0 000 6.364L12 20.364l7.682-7.682a4.5 4.5 0 00-6.364-6.364L12 7.636l-1.318-1.318a4.5 4.5 0 00-6.364 0z"/></svg>
          </button>
          <button onclick="window.openQuickView(${p.id})" title="Quick Preview" class="w-9 h-9 rounded-full bg-white text-slate-600 hover:text-teal-600 hover:bg-teal-50 shadow-md flex items-center justify-center transition">
            <svg class="w-4 h-4" fill="none" stroke="currentColor" viewBox="0 0 24 24"><path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M15 12a3 3 0 11-6 0 3 3 0 016 0z"/><path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M2.458 12C3.732 7.943 7.523 5 12 5c4.478 0 8.268 2.943 9.542 7-1.274 4.057-5.064 7-9.542 7-4.477 0-8.268-2.943-9.542-7z"/></svg>
          </button>
        </div>
      </div>

      <!-- Card Content -->
      <div class="p-4 flex flex-col flex-1">
        <div class="flex items-center justify-between text-xs text-slate-400 font-medium mb-1.5">
          <span class="truncate">${p.brandName || p.petTypeName || 'Pet Supplies'}</span>
          <span class="text-teal-600 font-semibold uppercase text-[10px] tracking-wider">${p.categoryName || ''}</span>
        </div>

        <a href="product.html?id=${p.id}" class="font-bold text-slate-800 text-sm hover:text-teal-600 line-clamp-2 mb-2 transition leading-snug" title="${p.name}">
          ${p.name}
        </a>

        <div class="mb-3">${window.renderRatingStars(p.averageRating, p.reviewCount)}</div>

        <!-- Price Section -->
        <div class="mt-auto pt-2 flex items-baseline gap-2 mb-3">
          <span class="text-lg font-black text-slate-900">${window.formatPrice(p.basePrice)}</span>
          ${hasDiscount ? `<span class="text-xs text-slate-400 line-through">${window.formatPrice(p.compareAtPrice)}</span>` : ''}
        </div>

        <!-- CTA Buttons -->
        ${isOutOfStock ? `
          <button disabled class="w-full py-2.5 px-3 rounded-xl bg-slate-100 text-slate-400 font-bold text-xs uppercase cursor-not-allowed">
            Out of Stock
          </button>
        ` : `
          <a href="product.html?id=${p.id}" class="w-full py-2.5 px-3 rounded-xl bg-slate-900 hover:bg-teal-600 text-white font-bold text-xs text-center transition flex items-center justify-center gap-1.5 shadow-sm">
            <span>Choose Options</span>
            <svg class="w-3.5 h-3.5" fill="none" stroke="currentColor" viewBox="0 0 24 24"><path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M14 5l7 7m0 0l-7 7m7-7H3"/></svg>
          </a>
        `}
      </div>
    </div>
  `;
};

// Document initialization
document.addEventListener("DOMContentLoaded", () => {
  window.updateCartBadge();
  window.updateWishlistBadge();

  // Mobile menu toggle listeners
  const mobileMenuBtn = document.getElementById("mobile-menu-btn");
  const mobileDrawer = document.getElementById("mobile-drawer");
  const mobileDrawerClose = document.getElementById("mobile-drawer-close");
  const mobileDrawerBackdrop = document.getElementById("mobile-drawer-backdrop");

  function openMobileDrawer() {
    if (mobileDrawer) {
      mobileDrawer.classList.remove("hidden");
      document.body.classList.add("overflow-hidden");
    }
  }

  function closeMobileDrawer() {
    if (mobileDrawer) {
      mobileDrawer.classList.add("hidden");
      document.body.classList.remove("overflow-hidden");
    }
  }

  if (mobileMenuBtn && mobileDrawer) {
    mobileMenuBtn.addEventListener("click", openMobileDrawer);
  }
  if (mobileDrawerClose && mobileDrawer) {
    mobileDrawerClose.addEventListener("click", closeMobileDrawer);
  }
  if (mobileDrawerBackdrop && mobileDrawer) {
    mobileDrawerBackdrop.addEventListener("click", closeMobileDrawer);
  }

  // Header & Mobile Search Input redirects
  const searchForm = document.getElementById("header-search-form");
  const searchInput = document.getElementById("header-search-input");
  if (searchForm && searchInput) {
    searchForm.addEventListener("submit", (e) => {
      e.preventDefault();
      const query = searchInput.value.trim();
      if (query) {
        window.location.href = `shop.html?search=${encodeURIComponent(query)}`;
      }
    });
  }

  const mobileSearchForm = document.getElementById("mobile-search-form");
  const mobileSearchInput = document.getElementById("mobile-search-input");
  if (mobileSearchForm && mobileSearchInput) {
    mobileSearchForm.addEventListener("submit", (e) => {
      e.preventDefault();
      const query = mobileSearchInput.value.trim();
      if (query) {
        window.location.href = `shop.html?search=${encodeURIComponent(query)}`;
      }
    });
  }
});
