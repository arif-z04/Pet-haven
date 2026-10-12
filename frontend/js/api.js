/**
 * PawHaven BD - Centralized API Client
 * Manages all communication with the ASP.NET Core Web API backend
 */

const API_BASE_URL = "http://localhost:5208/api";

class ApiClient {
  constructor(baseUrl) {
    this.baseUrl = baseUrl;
  }

  /**
   * Helper to retrieve stored JWT token
   */
  getToken() {
    return localStorage.getItem("pawhaven_token") || null;
  }

  /**
   * Core request wrapper with automatic authentication headers,
   * error boundary handling, and structured JSON parsing.
   */
  async request(endpoint, options = {}) {
    const url = `${this.baseUrl}${endpoint}`;
    const headers = {
      "Content-Type": "application/json",
      ...(options.headers || {})
    };

    const token = this.getToken();
    if (token) {
      headers["Authorization"] = `Bearer ${token}`;
    }

    const config = {
      ...options,
      headers
    };

    try {
      const response = await fetch(url, config);

      // Handle 204 No Content
      if (response.status === 204) {
        return { success: true, statusCode: 204, data: null };
      }

      const json = await response.json();

      if (!response.ok) {
        // Handle 401 Unauthorized
        if (response.status === 401) {
          // Token expired or invalid
          console.warn("API 401 Unauthorized encountered:", endpoint);
          // If token was present but expired, notify auth state
          if (token && window.AuthManager) {
            window.AuthManager.handleSessionExpired();
          }
        }
        return {
          success: false,
          statusCode: response.status,
          message: json.message || "Request failed.",
          errors: json.errors || []
        };
      }

      return json;
    } catch (error) {
      console.error(`Network or API Error on [${config.method || "GET"} ${url}]:`, error);
      return {
        success: false,
        statusCode: 0,
        message: "Unable to connect to the server. Please ensure the backend is running.",
        errors: [error.message]
      };
    }
  }

  // ==================== AUTHENTICATION ====================
  async register(data) {
    return this.request("/auth/register", {
      method: "POST",
      body: JSON.stringify(data)
    });
  }

  async login(credentials) {
    return this.request("/auth/login", {
      method: "POST",
      body: JSON.stringify(credentials)
    });
  }

  async logout() {
    return this.request("/auth/logout", {
      method: "POST"
    });
  }

  // ==================== PRODUCTS ====================
  async getProducts(params = {}) {
    const query = new URLSearchParams();
    Object.entries(params).forEach(([key, val]) => {
      if (val !== null && val !== undefined && val !== "") {
        query.append(key, val);
      }
    });
    const queryString = query.toString() ? `?${query.toString()}` : "";
    return this.request(`/products${queryString}`);
  }

  async getProduct(id) {
    return this.request(`/products/${id}`);
  }

  async getProductBySlug(slug) {
    return this.request(`/products/slug/${encodeURIComponent(slug)}`);
  }

  // ==================== TAXONOMY & LOOKUPS ====================
  async getCategories() {
    return this.request("/categories");
  }

  async getCategory(id) {
    return this.request(`/categories/${id}`);
  }

  async getCategoryProducts(id, params = {}) {
    const query = new URLSearchParams(params).toString();
    return this.request(`/categories/${id}/products${query ? `?${query}` : ""}`);
  }

  async getBrands() {
    return this.request("/brands");
  }

  async getBrandProducts(id, params = {}) {
    const query = new URLSearchParams(params).toString();
    return this.request(`/brands/${id}/products${query ? `?${query}` : ""}`);
  }

  async getPetTypes() {
    return this.request("/pet-types");
  }

  async getPetTypeProducts(id, params = {}) {
    const query = new URLSearchParams(params).toString();
    return this.request(`/pet-types/${id}/products${query ? `?${query}` : ""}`);
  }

  // ==================== CART ====================
  async getCart() {
    return this.request("/cart");
  }

  async addToCart(productId, variantId = null, quantity = 1) {
    return this.request("/cart/items", {
      method: "POST",
      body: JSON.stringify({ productId, variantId, quantity })
    });
  }

  async updateCartItem(itemId, quantity) {
    return this.request(`/cart/items/${itemId}`, {
      method: "PUT",
      body: JSON.stringify({ quantity })
    });
  }

  async removeCartItem(itemId) {
    return this.request(`/cart/items/${itemId}`, {
      method: "DELETE"
    });
  }

  async clearCart() {
    return this.request("/cart", {
      method: "DELETE"
    });
  }

  // ==================== WISHLIST ====================
  async getWishlist() {
    return this.request("/wishlist");
  }

  async addToWishlist(productId) {
    return this.request(`/wishlist/${productId}`, {
      method: "POST"
    });
  }

  async removeFromWishlist(productId) {
    return this.request(`/wishlist/${productId}`, {
      method: "DELETE"
    });
  }

  // ==================== ORDERS & CHECKOUT ====================
  async validateCoupon(code, orderAmount) {
    return this.request("/coupons/validate", {
      method: "POST",
      body: JSON.stringify({ code, orderAmount })
    });
  }

  async createOrder(orderPayload) {
    return this.request("/orders", {
      method: "POST",
      body: JSON.stringify(orderPayload)
    });
  }

  async getMyOrders(params = {}) {
    const query = new URLSearchParams(params).toString();
    return this.request(`/orders${query ? `?${query}` : ""}`);
  }

  async getOrder(id) {
    return this.request(`/orders/${id}`);
  }

  async cancelOrder(id) {
    return this.request(`/orders/${id}/cancel`, {
      method: "POST"
    });
  }

  // ==================== "MY PET" PROFILES ====================
  async getMyPets() {
    return this.request("/pets");
  }

  async getPet(id) {
    return this.request(`/pets/${id}`);
  }

  async createPet(data) {
    return this.request("/pets", {
      method: "POST",
      body: JSON.stringify(data)
    });
  }

  async updatePet(id, data) {
    return this.request(`/pets/${id}`, {
      method: "PUT",
      body: JSON.stringify(data)
    });
  }

  async deletePet(id) {
    return this.request(`/pets/${id}`, {
      method: "DELETE"
    });
  }

  // ==================== RECOMMENDATIONS ====================
  async getRecommendations(limit = 8, petId = null) {
    const query = petId ? `/recommendations?limit=${limit}&petId=${petId}` : `/recommendations?limit=${limit}`;
    return this.request(query);
  }

  // ==================== REVIEWS ====================
  async getProductReviews(productId) {
    return this.request(`/products/${productId}/reviews`);
  }

  async createReview(productId, reviewData) {
    return this.request(`/products/${productId}/reviews`, {
      method: "POST",
      body: JSON.stringify(reviewData)
    });
  }

  // ==================== SEARCH & CONTENT ====================
  async search(query, params = {}) {
    const queryParams = new URLSearchParams({ q: query, ...params }).toString();
    return this.request(`/search?${queryParams}`);
  }

  async getBanners(position = null) {
    const query = position ? `?position=${encodeURIComponent(position)}` : "";
    return this.request(`/banners${query}`);
  }

  async getBlogPosts(params = {}) {
    const query = new URLSearchParams(params).toString();
    return this.request(`/blog${query ? `?${query}` : ""}`);
  }

  async getBlogPost(slug) {
    return this.request(`/blog/${encodeURIComponent(slug)}`);
  }

  async getNotifications(unreadOnly = false) {
    return this.request(`/notifications?unreadOnly=${unreadOnly}`);
  }

  async markNotificationRead(id) {
    return this.request(`/notifications/${id}/read`, {
      method: "PUT"
    });
  }

  // ==================== ADMIN ====================
  async getAdminDashboardStats() {
    return this.request("/admin/dashboard/stats");
  }
}

// Global API instance available to all pages
window.api = new ApiClient(API_BASE_URL);
