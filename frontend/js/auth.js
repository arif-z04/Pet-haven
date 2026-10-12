/**
 * PawHaven BD - Authentication & Session State Manager
 */

class AuthManager {
  constructor() {
    this.tokenKey = "pawhaven_token";
    this.userKey = "pawhaven_user";
    this.expiresKey = "pawhaven_expires";
  }

  isAuthenticated() {
    const token = localStorage.getItem(this.tokenKey);
    const expires = localStorage.getItem(this.expiresKey);
    if (!token) return false;

    if (expires) {
      const expiryDate = new Date(expires);
      if (expiryDate <= new Date()) {
        this.clearSession();
        return false;
      }
    }
    return true;
  }

  getUser() {
    try {
      const userStr = localStorage.getItem(this.userKey);
      return userStr ? JSON.parse(userStr) : null;
    } catch {
      return null;
    }
  }

  getToken() {
    return localStorage.getItem(this.tokenKey);
  }

  saveSession(authResponse) {
    if (!authResponse || !authResponse.token) return;
    localStorage.setItem(this.tokenKey, authResponse.token);
    localStorage.setItem(this.userKey, JSON.stringify(authResponse.user));
    if (authResponse.expiresAt) {
      localStorage.setItem(this.expiresKey, authResponse.expiresAt);
    }
    this.updateHeaderAuthUI();
  }

  clearSession() {
    localStorage.removeItem(this.tokenKey);
    localStorage.removeItem(this.userKey);
    localStorage.removeItem(this.expiresKey);
    this.updateHeaderAuthUI();
    if (window.updateCartBadge) window.updateCartBadge();
    if (window.updateWishlistBadge) window.updateWishlistBadge();
  }

  async logout() {
    try {
      if (window.api) {
        await window.api.logout();
      }
    } catch (e) {
      console.error("Logout request failed:", e);
    } finally {
      this.clearSession();
      if (window.showToast) {
        window.showToast("You have been signed out successfully.", "info");
      }
      setTimeout(() => {
        window.location.href = "login.html";
      }, 500);
    }
  }

  handleSessionExpired() {
    this.clearSession();
    if (window.showToast) {
      window.showToast("Your session has expired. Please sign in again.", "warning");
    }
  }

  requireAuth(redirectUrl = null) {
    if (!this.isAuthenticated()) {
      const current = redirectUrl || window.location.pathname + window.location.search;
      window.location.href = `login.html?redirect=${encodeURIComponent(current)}`;
      return false;
    }
    return true;
  }

  updateHeaderAuthUI() {
    const authContainer = document.getElementById("header-auth-container");
    const drawerAuthContainer = document.getElementById("mobile-drawer-auth-container");

    if (this.isAuthenticated()) {
      const user = this.getUser();
      const initial = user?.name ? user.name.charAt(0).toUpperCase() : "U";
      const displayName = user?.name ? user.name.split(" ")[0] : "Account";

      if (authContainer) {
        authContainer.innerHTML = `
          <div class="relative group">
            <button id="user-menu-btn" class="flex items-center gap-2 p-1.5 rounded-lg hover:bg-slate-100 transition text-slate-700">
              <span class="w-8 h-8 rounded-full bg-teal-600 text-white font-semibold flex items-center justify-center text-sm shadow-sm">
                ${initial}
              </span>
              <span class="text-sm font-medium hidden md:inline-block max-w-[100px] truncate">${displayName}</span>
              <svg class="w-4 h-4 text-slate-400 group-hover:rotate-180 transition-transform duration-200" fill="none" stroke="currentColor" viewBox="0 0 24 24">
                <path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M19 9l-7 7-7-7" />
              </svg>
            </button>
            
            <!-- Dropdown Menu -->
            <div class="absolute right-0 top-full pt-2 w-52 hidden group-hover:block hover:block z-50">
              <div class="bg-white rounded-xl shadow-xl border border-slate-100 py-2 divide-y divide-slate-100 text-sm">
                <div class="px-4 py-2.5">
                  <p class="font-semibold text-slate-900 truncate">${user?.name || "Customer"}</p>
                  <p class="text-xs text-slate-500 truncate">${user?.email || ""}</p>
                  <span class="inline-block mt-1 px-2 py-0.5 text-[10px] font-bold rounded-full bg-teal-50 text-teal-700 uppercase tracking-wider">${user?.role || "customer"}</span>
                </div>
                <div class="py-1">
                  <a href="account.html" class="flex items-center gap-2.5 px-4 py-2 text-slate-700 hover:bg-teal-50 hover:text-teal-700">
                    <svg class="w-4 h-4" fill="none" stroke="currentColor" viewBox="0 0 24 24"><path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M16 7a4 4 0 11-8 0 4 4 0 018 0zM12 14a7 7 0 00-7 7h14a7 7 0 00-7 7z"/></svg>
                    My Dashboard
                  </a>
                  <a href="orders.html" class="flex items-center gap-2.5 px-4 py-2 text-slate-700 hover:bg-teal-50 hover:text-teal-700">
                    <svg class="w-4 h-4" fill="none" stroke="currentColor" viewBox="0 0 24 24"><path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M16 11V7a4 4 0 00-8 0v4M5 9h14l1 12H4L5 9z"/></svg>
                    My Orders
                  </a>
                  <a href="my-pets.html" class="flex items-center gap-2.5 px-4 py-2 text-slate-700 hover:bg-teal-50 hover:text-teal-700">
                    <svg class="w-4 h-4" fill="none" stroke="currentColor" viewBox="0 0 24 24"><path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M4.318 6.318a4.5 4.5 0 000 6.364L12 20.364l7.682-7.682a4.5 4.5 0 00-6.364-6.364L12 7.636l-1.318-1.318a4.5 4.5 0 00-6.364 0z"/></svg>
                    My Pet Profiles
                  </a>
                </div>
                <div class="pt-1">
                  <button onclick="window.AuthManager.logout()" class="w-full text-left flex items-center gap-2.5 px-4 py-2 text-red-600 hover:bg-red-50">
                    <svg class="w-4 h-4" fill="none" stroke="currentColor" viewBox="0 0 24 24"><path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M17 16l4-4m0 0l-4-4m4 4H7m6 4v1a3 3 0 01-3 3H6a3 3 0 01-3-3V7a3 3 0 013-3h4a3 3 0 013 3v1"/></svg>
                    Sign Out
                  </button>
                </div>
              </div>
            </div>
          </div>
        `;
      }

      if (drawerAuthContainer) {
        drawerAuthContainer.innerHTML = `
          <div class="p-3 bg-slate-50 rounded-xl mb-2 flex items-center justify-between">
            <div class="min-w-0 pr-2">
              <p class="font-bold text-slate-900 truncate">${user?.name || "Customer"}</p>
              <p class="text-[10px] text-slate-500 truncate">${user?.email || ""}</p>
            </div>
            <span class="px-2 py-0.5 text-[9px] font-extrabold rounded-full bg-teal-100 text-teal-800 uppercase shrink-0">${user?.role || "customer"}</span>
          </div>
          <button onclick="window.AuthManager.logout()" class="w-full text-center py-2.5 px-4 rounded-xl bg-red-50 text-red-600 hover:bg-red-100 font-bold mb-2 transition">
            Sign Out
          </button>
          <p class="text-[11px] text-slate-400 text-center">Helpline: +880 1711-000001</p>
        `;
      }
    } else {
      if (authContainer) {
        authContainer.innerHTML = `
          <a href="login.html" class="flex items-center gap-1.5 text-sm font-semibold text-slate-700 hover:text-teal-600 transition px-2 py-1 rounded-lg">
            <svg class="w-5 h-5 text-slate-500" fill="none" stroke="currentColor" viewBox="0 0 24 24">
              <path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M16 7a4 4 0 11-8 0 4 4 0 018 0zM12 14a7 7 0 00-7 7h14a7 7 0 00-7 7z" />
            </svg>
            <span class="hidden sm:inline">Sign In</span>
          </a>
        `;
      }

      if (drawerAuthContainer) {
        drawerAuthContainer.innerHTML = `
          <a href="login.html" class="block w-full text-center py-2.5 px-4 rounded-xl bg-teal-600 text-white font-bold mb-2">
            Sign In / Register
          </a>
          <p class="text-[11px] text-slate-400 text-center">Helpline: +880 1711-000001</p>
        `;
      }
    }
  }
}

window.AuthManager = new AuthManager();
document.addEventListener("DOMContentLoaded", () => {
  window.AuthManager.updateHeaderAuthUI();
});
