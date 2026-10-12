/**
 * PawHaven BD - Shopping Cart Controller
 */

class CartPageController {
  constructor() {
    this.itemsContainer = document.getElementById("cart-items-container");
    this.subtotalElement = document.getElementById("cart-subtotal");
    this.discountElement = document.getElementById("cart-discount");
    this.deliveryElement = document.getElementById("cart-delivery");
    this.totalElement = document.getElementById("cart-grand-total");
    this.couponInput = document.getElementById("coupon-code-input");
    this.applyCouponBtn = document.getElementById("apply-coupon-btn");
    this.couponStatusElement = document.getElementById("coupon-status-msg");
    this.deliverySelect = document.getElementById("cart-delivery-region");

    this.appliedDiscount = 0;
    this.appliedCouponCode = null;
    this.deliveryFee = 80; // 80 BDT Dhaka inside default, 150 BDT outside

    this.init();
  }

  async init() {
    if (this.deliverySelect) {
      this.deliverySelect.addEventListener("change", (e) => {
        this.deliveryFee = e.target.value === "outside" ? 150 : 80;
        this.updateFinancialSummary();
      });
    }

    if (this.applyCouponBtn) {
      this.applyCouponBtn.addEventListener("click", () => this.handleApplyCoupon());
    }

    await this.renderCart();
  }

  async renderCart() {
    if (!this.itemsContainer) return;

    this.itemsContainer.innerHTML = `
      <div class="py-12 text-center">
        <div class="w-8 h-8 border-4 border-teal-500 border-t-transparent rounded-full animate-spin mx-auto mb-3"></div>
        <p class="text-sm text-slate-500">Loading your shopping cart...</p>
      </div>
    `;

    let items = [];
    let subtotal = 0;

    if (window.AuthManager && window.AuthManager.isAuthenticated()) {
      try {
        const res = await window.api.getCart();
        if (res.success && res.data) {
          items = res.data.items || [];
          subtotal = res.data.subtotal || 0;
        }
      } catch (err) {
        console.error("Cart fetch error:", err);
      }
    } else {
      // Guest local cart: resolve details from API
      const localCart = JSON.parse(localStorage.getItem("pawhaven_local_cart") || "[]");
      if (localCart.length > 0) {
        for (const item of localCart) {
          try {
            const pRes = await window.api.getProduct(item.productId);
            if (pRes.success && pRes.data) {
              const p = pRes.data;
              const v = item.variantId ? p.variants?.find(v => v.id == item.variantId) : null;
              const unitPrice = v ? v.price : p.basePrice;
              const sku = v ? v.sku : p.sku;
              const variantName = v ? v.variantName : null;

              items.push({
                id: item.productId,
                productId: item.productId,
                variantId: item.variantId,
                productName: p.name,
                productSlug: p.slug,
                variantName: variantName,
                sku: sku,
                imageUrl: p.primaryImageUrl,
                unitPrice: unitPrice,
                quantity: item.quantity,
                availableStock: v ? v.stockQuantity : p.stockQuantity
              });
              subtotal += unitPrice * item.quantity;
            }
          } catch (e) {
            console.warn("Failed resolving item for guest cart:", e);
          }
        }
      }
    }

    this.cartItems = items;
    this.rawSubtotal = subtotal;

    if (items.length === 0) {
      this.itemsContainer.innerHTML = `
        <div class="bg-white rounded-2xl border border-slate-200/80 p-12 text-center">
          <div class="w-20 h-20 bg-teal-50 rounded-full flex items-center justify-center mx-auto mb-4 text-teal-600">
            <svg class="w-10 h-10" fill="none" stroke="currentColor" viewBox="0 0 24 24"><path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M16 11V7a4 4 0 00-8 0v4M5 9h14l1 12H4L5 9z"/></svg>
          </div>
          <h3 class="text-xl font-bold text-slate-800 mb-2">Your Shopping Cart is Empty</h3>
          <p class="text-sm text-slate-500 mb-8 max-w-sm mx-auto">Explore our high-quality pet nutrition, delicious treats, and fun toys!</p>
          <a href="shop.html" class="inline-flex items-center gap-2 px-6 py-3 rounded-xl bg-teal-600 hover:bg-teal-700 text-white font-bold text-sm shadow-md transition">
            Start Shopping Now
            <svg class="w-4 h-4" fill="none" stroke="currentColor" viewBox="0 0 24 24"><path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M14 5l7 7m0 0l-7 7m7-7H3"/></svg>
          </a>
        </div>
      `;
      this.updateFinancialSummary();
      return;
    }

    let html = `<div class="bg-white rounded-2xl border border-slate-200/80 divide-y divide-slate-100 overflow-hidden shadow-sm">`;

    items.forEach(item => {
      const imgUrl = window.resolveImageUrl(item.imageUrl, item.productName);
      const lineTotal = item.unitPrice * item.quantity;

      html += `
        <div class="p-4 sm:p-6 flex flex-col sm:flex-row items-start sm:items-center justify-between gap-4">
          <div class="flex items-center gap-4 flex-1 min-w-0">
            <a href="product.html?id=${item.productId}" class="w-16 h-16 sm:w-20 sm:h-20 bg-slate-50 rounded-xl p-2 shrink-0 border border-slate-100 flex items-center justify-center">
              <img src="${imgUrl}" alt="${item.productName}" class="max-w-full max-h-full object-contain">
            </a>
            <div class="min-w-0 flex-1">
              <a href="product.html?id=${item.productId}" class="font-bold text-slate-900 hover:text-teal-600 text-sm sm:text-base line-clamp-1 transition">
                ${item.productName}
              </a>
              ${item.variantName ? `<p class="text-xs text-teal-700 font-semibold bg-teal-50 px-2 py-0.5 rounded-md inline-block mt-1">Option: ${item.variantName}</p>` : ''}
              <p class="text-xs text-slate-400 mt-1">SKU: ${item.sku || 'N/A'}</p>
              <p class="text-sm font-extrabold text-slate-900 sm:hidden mt-2">${window.formatPrice(item.unitPrice)}</p>
            </div>
          </div>

          <!-- Quantity Stepper & Price -->
          <div class="flex items-center justify-between w-full sm:w-auto sm:gap-8 pt-2 sm:pt-0 border-t sm:border-0 border-slate-100">
            <div class="flex items-center border border-slate-200 rounded-xl overflow-hidden bg-white">
              <button onclick="window.cartPage.changeQuantity(${item.id || item.productId}, ${item.quantity - 1})" class="w-10 h-10 min-w-[40px] min-h-[40px] flex items-center justify-center text-slate-500 hover:bg-slate-100 transition font-bold text-base" aria-label="Decrease quantity">−</button>
              <span class="w-10 text-center font-bold text-sm text-slate-800">${item.quantity}</span>
              <button onclick="window.cartPage.changeQuantity(${item.id || item.productId}, ${item.quantity + 1})" class="w-10 h-10 min-w-[40px] min-h-[40px] flex items-center justify-center text-slate-500 hover:bg-slate-100 transition font-bold text-base" aria-label="Increase quantity">+</button>
            </div>

            <div class="text-right min-w-[90px]">
              <span class="text-base font-extrabold text-slate-900 block">${window.formatPrice(lineTotal)}</span>
              <span class="text-xs text-slate-400 hidden sm:block">${window.formatPrice(item.unitPrice)} each</span>
            </div>

            <button onclick="window.cartPage.removeItem(${item.id || item.productId})" class="text-slate-400 hover:text-red-500 w-10 h-10 min-w-[40px] min-h-[40px] flex items-center justify-center rounded-xl hover:bg-red-50 transition" title="Remove Item" aria-label="Remove Item">
              <svg class="w-5 h-5" fill="none" stroke="currentColor" viewBox="0 0 24 24"><path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M19 7l-.867 12.142A2 2 0 0116.138 21H7.862a2 2 0 01-1.995-1.858L5 7m5 4v6m4-6v6m1-10V4a1 1 0 00-1-1h-4a1 1 0 00-1 1v3M4 7h16"/></svg>
            </button>
          </div>
        </div>
      `;
    });

    html += `</div>`;
    this.itemsContainer.innerHTML = html;
    this.updateFinancialSummary();
  }

  async changeQuantity(itemId, newQty) {
    if (newQty <= 0) {
      await this.removeItem(itemId);
      return;
    }

    if (window.AuthManager && window.AuthManager.isAuthenticated()) {
      const res = await window.api.updateCartItem(itemId, newQty);
      if (res.success) {
        await this.renderCart();
        window.updateCartBadge();
      } else {
        window.showToast(res.message || "Could not update quantity.", "error");
      }
    } else {
      const localCart = JSON.parse(localStorage.getItem("pawhaven_local_cart") || "[]");
      const item = localCart.find(i => i.productId == itemId);
      if (item) {
        item.quantity = newQty;
        localStorage.setItem("pawhaven_local_cart", JSON.stringify(localCart));
        await this.renderCart();
        window.updateCartBadge();
      }
    }
  }

  async removeItem(itemId) {
    if (window.AuthManager && window.AuthManager.isAuthenticated()) {
      const res = await window.api.removeCartItem(itemId);
      if (res.success) {
        window.showToast("Item removed from cart.", "info");
        await this.renderCart();
        window.updateCartBadge();
      }
    } else {
      let localCart = JSON.parse(localStorage.getItem("pawhaven_local_cart") || "[]");
      localCart = localCart.filter(i => i.productId != itemId);
      localStorage.setItem("pawhaven_local_cart", JSON.stringify(localCart));
      window.showToast("Item removed from cart.", "info");
      await this.renderCart();
      window.updateCartBadge();
    }
  }

  async handleApplyCoupon() {
    if (!this.couponInput) return;
    const code = this.couponInput.value.trim();
    if (!code) {
      window.showToast("Please enter a promotional coupon code.", "warning");
      return;
    }

    if (!this.rawSubtotal || this.rawSubtotal <= 0) {
      window.showToast("Your cart is empty.", "warning");
      return;
    }

    if (this.couponStatusElement) {
      this.couponStatusElement.innerHTML = `<span class="text-teal-600 text-xs">Validating coupon with server...</span>`;
    }

    try {
      const res = await window.api.validateCoupon(code, this.rawSubtotal);
      if (res.success && res.data && res.data.isValid) {
        this.appliedDiscount = res.data.calculatedDiscount;
        this.appliedCouponCode = res.data.code;
        sessionStorage.setItem("pawhaven_applied_coupon", JSON.stringify(res.data));

        if (this.couponStatusElement) {
          this.couponStatusElement.innerHTML = `<span class="text-emerald-600 font-bold text-xs">✓ Coupon ${res.data.code} applied: -${window.formatPrice(this.appliedDiscount)}</span>`;
        }
        window.showToast(`Coupon applied! Saved ${window.formatPrice(this.appliedDiscount)}`, "success");
      } else {
        this.appliedDiscount = 0;
        this.appliedCouponCode = null;
        sessionStorage.removeItem("pawhaven_applied_coupon");

        const msg = res.data?.message || res.message || "Invalid or expired coupon.";
        if (this.couponStatusElement) {
          this.couponStatusElement.innerHTML = `<span class="text-red-500 font-semibold text-xs">✗ ${msg}</span>`;
        }
        window.showToast(msg, "error");
      }
      this.updateFinancialSummary();
    } catch (err) {
      window.showToast("Coupon validation server request failed.", "error");
    }
  }

  updateFinancialSummary() {
    const subtotal = this.rawSubtotal || 0;
    const discount = this.appliedDiscount || 0;
    const delivery = subtotal > 0 ? this.deliveryFee : 0;
    const grandTotal = Math.max(0, (subtotal - discount) + delivery);

    if (this.subtotalElement) this.subtotalElement.textContent = window.formatPrice(subtotal);
    if (this.discountElement) this.discountElement.textContent = `-${window.formatPrice(discount)}`;
    if (this.deliveryElement) this.deliveryElement.textContent = window.formatPrice(delivery);
    if (this.totalElement) this.totalElement.textContent = window.formatPrice(grandTotal);

    // Save active checkout total
    sessionStorage.setItem("pawhaven_checkout_subtotal", subtotal.toString());
    sessionStorage.setItem("pawhaven_checkout_delivery", delivery.toString());
    sessionStorage.setItem("pawhaven_checkout_total", grandTotal.toString());
  }
}

document.addEventListener("DOMContentLoaded", () => {
  if (document.getElementById("cart-items-container")) {
    window.cartPage = new CartPageController();
  }
});
