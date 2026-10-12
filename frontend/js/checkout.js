/**
 * PawHaven BD - Checkout & Transaction Processing Controller
 */

class CheckoutController {
  constructor() {
    this.form = document.getElementById("checkout-form");
    this.itemsPreviewContainer = document.getElementById("checkout-items-preview");
    this.subtotalElement = document.getElementById("checkout-subtotal");
    this.discountElement = document.getElementById("checkout-discount");
    this.deliveryElement = document.getElementById("checkout-delivery");
    this.totalElement = document.getElementById("checkout-total");
    this.divisionSelect = document.getElementById("shipping-division");
    this.placeOrderBtn = document.getElementById("place-order-btn");

    this.appliedDiscount = 0;
    this.appliedCouponCode = null;
    this.deliveryFee = 80;

    this.init();
  }

  async init() {
    // If not logged in, prompt user or allow checkout
    const user = window.AuthManager ? window.AuthManager.getUser() : null;
    if (user) {
      const nameInput = document.getElementById("customer-name");
      const emailInput = document.getElementById("customer-email");
      const phoneInput = document.getElementById("customer-phone");
      if (nameInput) nameInput.value = user.name || "";
      if (emailInput) emailInput.value = user.email || "";
      if (phoneInput && user.phone) phoneInput.value = user.phone;
    }

    // Check saved coupon from cart
    const savedCoupon = sessionStorage.getItem("pawhaven_applied_coupon");
    if (savedCoupon) {
      try {
        const c = JSON.parse(savedCoupon);
        this.appliedDiscount = c.calculatedDiscount || 0;
        this.appliedCouponCode = c.code || null;
      } catch (e) {}
    }

    if (this.divisionSelect) {
      this.divisionSelect.addEventListener("change", (e) => {
        this.deliveryFee = e.target.value === "Dhaka" ? 80 : 150;
        this.updateFinancialSummary();
      });
    }

    if (this.form) {
      this.form.addEventListener("submit", (e) => this.handleSubmitOrder(e));
    }

    await this.loadCheckoutItems();
  }

  async loadCheckoutItems() {
    if (!this.itemsPreviewContainer) return;

    let items = [];
    let subtotal = 0;

    if (window.AuthManager && window.AuthManager.isAuthenticated()) {
      const res = await window.api.getCart();
      if (res.success && res.data) {
        items = res.data.items || [];
        subtotal = res.data.subtotal || 0;
      }
    } else {
      const localCart = JSON.parse(localStorage.getItem("pawhaven_local_cart") || "[]");
      for (const item of localCart) {
        const pRes = await window.api.getProduct(item.productId);
        if (pRes.success && pRes.data) {
          const p = pRes.data;
          const v = item.variantId ? p.variants?.find(v => v.id == item.variantId) : null;
          const unitPrice = v ? v.price : p.basePrice;
          items.push({
            productId: item.productId,
            variantId: item.variantId,
            productName: p.name,
            variantName: v ? v.variantName : null,
            unitPrice: unitPrice,
            quantity: item.quantity,
            totalPrice: unitPrice * item.quantity
          });
          subtotal += unitPrice * item.quantity;
        }
      }
    }

    this.checkoutItems = items;
    this.rawSubtotal = subtotal;

    if (items.length === 0) {
      this.itemsPreviewContainer.innerHTML = `<p class="text-sm text-slate-500 py-4 text-center">No items to checkout. <a href="shop.html" class="text-teal-600 font-bold">Go to shop</a></p>`;
      if (this.placeOrderBtn) this.placeOrderBtn.disabled = true;
      return;
    }

    let html = `<div class="divide-y divide-slate-100 max-h-60 overflow-y-auto pr-1">`;
    items.forEach(item => {
      html += `
        <div class="py-2.5 flex items-center justify-between text-sm">
          <div class="min-w-0 flex-1 pr-2">
            <p class="font-bold text-slate-800 truncate">${item.productName}</p>
            <p class="text-xs text-slate-400">Qty: ${item.quantity} ${item.variantName ? `• ${item.variantName}` : ''}</p>
          </div>
          <span class="font-extrabold text-slate-900 shrink-0">${window.formatPrice(item.unitPrice * item.quantity)}</span>
        </div>
      `;
    });
    html += `</div>`;

    this.itemsPreviewContainer.innerHTML = html;
    this.updateFinancialSummary();
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
  }

  async handleSubmitOrder(e) {
    e.preventDefault();

    if (!window.AuthManager || !window.AuthManager.isAuthenticated()) {
      window.showToast("Please sign in or create an account to finalize your order.", "warning");
      setTimeout(() => {
        window.location.href = `login.html?redirect=checkout.html`;
      }, 1000);
      return;
    }

    const name = document.getElementById("customer-name")?.value.trim();
    const phone = document.getElementById("customer-phone")?.value.trim();
    const division = document.getElementById("shipping-division")?.value.trim();
    const district = document.getElementById("shipping-district")?.value.trim();
    const area = document.getElementById("shipping-area")?.value.trim();
    const addressLine = document.getElementById("shipping-address")?.value.trim();
    const postalCode = document.getElementById("shipping-postal")?.value.trim();
    const customerNote = document.getElementById("customer-note")?.value.trim();
    const paymentMethod = document.querySelector("input[name='payment-method']:checked")?.value || "cash_on_delivery";

    if (!name || !phone || !division || !district || !area || !addressLine) {
      window.showToast("Please complete all required shipping fields.", "warning");
      return;
    }

    const payload = {
      shippingFullName: name,
      shippingPhone: phone,
      shippingDivision: division,
      shippingDistrict: district,
      shippingArea: area,
      shippingAddressLine: addressLine,
      shippingPostalCode: postalCode,
      couponCode: this.appliedCouponCode,
      paymentMethod: paymentMethod,
      customerNote: customerNote
    };

    if (this.placeOrderBtn) {
      this.placeOrderBtn.disabled = true;
      this.placeOrderBtn.innerHTML = `
        <div class="w-5 h-5 border-2 border-white border-t-transparent rounded-full animate-spin"></div>
        Processing Order...
      `;
    }

    try {
      const res = await window.api.createOrder(payload);

      if (res.success && res.data) {
        // Clear local storage cart state
        localStorage.removeItem("pawhaven_local_cart");
        sessionStorage.removeItem("pawhaven_applied_coupon");
        window.updateCartBadge();

        window.showToast("Order placed successfully! Redirecting...", "success");
        setTimeout(() => {
          window.location.href = `order-details.html?id=${res.data.id}&orderNumber=${res.data.orderNumber}`;
        }, 1200);
      } else {
        window.showToast(res.message || "Failed to place order.", "error");
        if (this.placeOrderBtn) {
          this.placeOrderBtn.disabled = false;
          this.placeOrderBtn.innerHTML = `Confirm & Place Order`;
        }
      }
    } catch (err) {
      window.showToast("Network error submitting order.", "error");
      if (this.placeOrderBtn) {
        this.placeOrderBtn.disabled = false;
        this.placeOrderBtn.innerHTML = `Confirm & Place Order`;
      }
    }
  }
}

document.addEventListener("DOMContentLoaded", () => {
  if (document.getElementById("checkout-form")) {
    window.checkoutPage = new CheckoutController();
  }
});
