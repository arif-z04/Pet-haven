/**
 * PawHaven BD - Customer Dashboard Controller
 */

class AccountController {
  constructor() {
    this.user = window.AuthManager ? window.AuthManager.getUser() : null;
    this.init();
  }

  async init() {
    if (!window.AuthManager || !window.AuthManager.isAuthenticated()) {
      window.location.href = `login.html?redirect=account.html`;
      return;
    }

    this.renderUserInfo();
    await this.loadRecentOrders();
    await this.loadNotifications();
  }

  renderUserInfo() {
    const nameEl = document.getElementById("profile-name");
    const emailEl = document.getElementById("profile-email");
    const phoneEl = document.getElementById("profile-phone");
    const initialEl = document.getElementById("profile-initial");

    if (nameEl) nameEl.textContent = this.user?.name || "Customer";
    if (emailEl) emailEl.textContent = this.user?.email || "";
    if (phoneEl) phoneEl.textContent = this.user?.phone || "No phone linked";
    if (initialEl) initialEl.textContent = (this.user?.name || "U").charAt(0).toUpperCase();
  }

  async loadRecentOrders() {
    const container = document.getElementById("account-recent-orders");
    if (!container) return;

    container.innerHTML = `
      <div class="py-6 text-center">
        <div class="w-6 h-6 border-2 border-teal-500 border-t-transparent rounded-full animate-spin mx-auto mb-2"></div>
        <p class="text-xs text-slate-500">Loading order history...</p>
      </div>
    `;

    try {
      const res = await window.api.getMyOrders({ pageNumber: 1, pageSize: 5 });
      if (!res.success || !res.data || res.data.items.length === 0) {
        container.innerHTML = `
          <div class="p-8 text-center bg-slate-50 rounded-xl">
            <p class="text-sm text-slate-500 mb-3">You haven't placed any orders yet.</p>
            <a href="shop.html" class="inline-block px-4 py-2 rounded-lg bg-teal-600 text-white font-bold text-xs">Start Shopping</a>
          </div>
        `;
        return;
      }

      const orders = res.data.items;
      let html = `<div class="divide-y divide-slate-100">`;

      orders.forEach(o => {
        let statusBadge = "";
        switch (o.orderStatus?.toLowerCase()) {
          case "delivered":
            statusBadge = `<span class="px-2.5 py-1 text-xs font-bold rounded-full bg-emerald-50 text-emerald-700">Delivered</span>`;
            break;
          case "shipped":
            statusBadge = `<span class="px-2.5 py-1 text-xs font-bold rounded-full bg-blue-50 text-blue-700">Shipped</span>`;
            break;
          case "processing":
          case "confirmed":
            statusBadge = `<span class="px-2.5 py-1 text-xs font-bold rounded-full bg-amber-50 text-amber-700">In Progress</span>`;
            break;
          case "cancelled":
            statusBadge = `<span class="px-2.5 py-1 text-xs font-bold rounded-full bg-red-50 text-red-700">Cancelled</span>`;
            break;
          default:
            statusBadge = `<span class="px-2.5 py-1 text-xs font-bold rounded-full bg-slate-100 text-slate-700">Pending</span>`;
        }

        html += `
          <div class="py-4 flex flex-col sm:flex-row sm:items-center justify-between gap-3">
            <div>
              <div class="flex items-center gap-2 mb-1">
                <a href="order-details.html?id=${o.id}" class="font-bold text-slate-900 hover:text-teal-600 text-sm">#${o.orderNumber}</a>
                ${statusBadge}
              </div>
              <p class="text-xs text-slate-400">Placed on ${new Date(o.createdAt).toLocaleDateString()} • ${o.items?.length || 1} items</p>
            </div>
            <div class="flex items-center justify-between sm:justify-end gap-4">
              <span class="font-extrabold text-slate-900 text-sm sm:text-base">${window.formatPrice(o.totalAmount)}</span>
              <a href="order-details.html?id=${o.id}" class="px-3 py-1.5 rounded-lg border border-slate-200 text-slate-700 hover:bg-slate-50 font-bold text-xs">
                View Details
              </a>
            </div>
          </div>
        `;
      });

      html += `</div>`;
      container.innerHTML = html;
    } catch (e) {
      container.innerHTML = `<div class="p-4 text-center text-red-500 text-xs">Failed to load orders.</div>`;
    }
  }

  async loadNotifications() {
    const container = document.getElementById("account-notifications-list");
    if (!container) return;

    try {
      const res = await window.api.getNotifications(false);
      if (!res.success || !res.data || res.data.length === 0) {
        container.innerHTML = `<p class="text-xs text-slate-400 py-4 text-center">No notifications at this time.</p>`;
        return;
      }

      container.innerHTML = res.data.slice(0, 5).map(n => `
        <div class="p-3 rounded-xl ${n.isRead ? 'bg-slate-50' : 'bg-teal-50/50 border border-teal-100'} flex items-start gap-3">
          <div class="w-2 h-2 rounded-full ${n.isRead ? 'bg-slate-300' : 'bg-teal-500'} mt-1.5 shrink-0"></div>
          <div class="flex-1">
            <h4 class="font-bold text-slate-800 text-xs">${n.title}</h4>
            <p class="text-xs text-slate-500 mt-0.5">${n.message}</p>
            <span class="text-[10px] text-slate-400 mt-1 block">${new Date(n.createdAt).toLocaleDateString()}</span>
          </div>
        </div>
      `).join("");
    } catch (e) {
      console.warn("Notifications error:", e);
    }
  }
}

document.addEventListener("DOMContentLoaded", () => {
  if (document.getElementById("profile-name")) {
    window.accountPage = new AccountController();
  }
});
