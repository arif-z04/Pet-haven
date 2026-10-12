/**
 * PawHaven BD - Wishlist Controller
 */

class WishlistPageController {
  constructor() {
    this.container = document.getElementById("wishlist-items-container");
    this.init();
  }

  async init() {
    await this.renderWishlist();
  }

  async renderWishlist() {
    if (!this.container) return;

    this.container.innerHTML = `
      <div class="col-span-full py-16 text-center">
        <div class="w-8 h-8 border-4 border-teal-500 border-t-transparent rounded-full animate-spin mx-auto mb-3"></div>
        <p class="text-sm text-slate-500">Loading your saved items...</p>
      </div>
    `;

    let items = [];

    if (window.AuthManager && window.AuthManager.isAuthenticated()) {
      try {
        const res = await window.api.getWishlist();
        if (res.success && res.data) {
          items = res.data.items || [];
        }
      } catch (e) {
        console.error("Wishlist fetch error:", e);
      }
    } else {
      const localWishlist = JSON.parse(localStorage.getItem("pawhaven_local_wishlist") || "[]");
      for (const pid of localWishlist) {
        const pRes = await window.api.getProduct(pid);
        if (pRes.success && pRes.data) {
          items.push({
            productId: pRes.data.id,
            productName: pRes.data.name,
            productSlug: pRes.data.slug,
            basePrice: pRes.data.basePrice,
            compareAtPrice: pRes.data.compareAtPrice,
            imageUrl: pRes.data.primaryImageUrl,
            inStock: pRes.data.stockQuantity > 0
          });
        }
      }
    }

    if (items.length === 0) {
      this.container.innerHTML = `
        <div class="col-span-full bg-white rounded-2xl border border-slate-200/80 p-12 text-center">
          <div class="w-20 h-20 bg-teal-50 rounded-full flex items-center justify-center mx-auto mb-4 text-teal-600">
            <svg class="w-10 h-10" fill="none" stroke="currentColor" viewBox="0 0 24 24"><path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M4.318 6.318a4.5 4.5 0 000 6.364L12 20.364l7.682-7.682a4.5 4.5 0 00-6.364-6.364L12 7.636l-1.318-1.318a4.5 4.5 0 00-6.364 0z"/></svg>
          </div>
          <h3 class="text-xl font-bold text-slate-800 mb-2">Your Wishlist is Empty</h3>
          <p class="text-sm text-slate-500 mb-8 max-w-sm mx-auto">Bookmark items you love while browsing our pet store to purchase them later!</p>
          <a href="shop.html" class="inline-flex items-center gap-2 px-6 py-3 rounded-xl bg-teal-600 hover:bg-teal-700 text-white font-bold text-sm shadow-md transition">
            Explore All Products
          </a>
        </div>
      `;
      return;
    }

    this.container.innerHTML = items.map(item => {
      const imgUrl = window.resolveImageUrl(item.imageUrl, item.productName);

      return `
        <div class="bg-white rounded-2xl border border-slate-200/80 p-4 flex flex-col justify-between shadow-sm hover:shadow-lg transition">
          <div class="aspect-square bg-slate-50 rounded-xl p-3 flex items-center justify-center mb-4 relative">
            <img src="${imgUrl}" alt="${item.productName}" class="max-h-full max-w-full object-contain">
            <button onclick="window.wishlistPage.removeItem(${item.productId})" class="absolute top-2 right-2 text-slate-400 hover:text-red-500 p-1 rounded-full bg-white shadow-sm" title="Remove">
              <svg class="w-4 h-4" fill="none" stroke="currentColor" viewBox="0 0 24 24"><path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M6 18L18 6M6 6l12 12"/></svg>
            </button>
          </div>

          <div>
            <span class="text-xs font-bold text-teal-600 uppercase tracking-wider block mb-1">${item.inStock ? 'In Stock' : 'Out of Stock'}</span>
            <a href="product.html?id=${item.productId}" class="font-bold text-slate-800 hover:text-teal-600 text-sm line-clamp-2 mb-2 block">
              ${item.productName}
            </a>
            <div class="flex items-baseline gap-2 mb-4">
              <span class="text-lg font-black text-slate-900">${window.formatPrice(item.basePrice)}</span>
              ${item.compareAtPrice ? `<span class="text-xs text-slate-400 line-through">${window.formatPrice(item.compareAtPrice)}</span>` : ''}
            </div>
          </div>

          <div class="pt-2">
            <button onclick="window.wishlistPage.moveToCart(${item.productId})" class="w-full py-2.5 px-3 rounded-xl bg-teal-600 hover:bg-teal-700 text-white font-bold text-xs text-center transition flex items-center justify-center gap-1.5 shadow-sm">
              <svg class="w-4 h-4" fill="none" stroke="currentColor" viewBox="0 0 24 24"><path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M16 11V7a4 4 0 00-8 0v4M5 9h14l1 12H4L5 9z"/></svg>
              Move to Cart
            </button>
          </div>
        </div>
      `;
    }).join("");
  }

  async removeItem(productId) {
    if (window.AuthManager && window.AuthManager.isAuthenticated()) {
      const res = await window.api.removeFromWishlist(productId);
      if (res.success) {
        window.showToast("Removed from wishlist.", "info");
        await this.renderWishlist();
        window.updateWishlistBadge();
      }
    } else {
      let localWishlist = JSON.parse(localStorage.getItem("pawhaven_local_wishlist") || "[]");
      localWishlist = localWishlist.filter(id => id != productId);
      localStorage.setItem("pawhaven_local_wishlist", JSON.stringify(localWishlist));
      window.showToast("Removed from wishlist.", "info");
      await this.renderWishlist();
      window.updateWishlistBadge();
    }
  }

  async moveToCart(productId) {
    await window.handleAddToCart(productId, null, 1);
    await this.removeItem(productId);
  }
}

document.addEventListener("DOMContentLoaded", () => {
  if (document.getElementById("wishlist-items-container")) {
    window.wishlistPage = new WishlistPageController();
  }
});
