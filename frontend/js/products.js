/**
 * PawHaven BD - Products & Shop Catalog Controller
 */

class ProductCatalogController {
  constructor() {
    this.container = document.getElementById("shop-products-grid");
    this.paginationContainer = document.getElementById("shop-pagination");
    this.countElement = document.getElementById("shop-product-count");
    this.activeFiltersContainer = document.getElementById("active-filters-pills");
    
    // Filter controls
    this.petTypeInputs = document.querySelectorAll("input[name='filter-pet-type']");
    this.categoryInputs = document.querySelectorAll("input[name='filter-category']");
    this.brandInputs = document.querySelectorAll("input[name='filter-brand']");
    this.minPriceInput = document.getElementById("filter-min-price");
    this.maxPriceInput = document.getElementById("filter-max-price");
    this.inStockCheckbox = document.getElementById("filter-in-stock");
    this.onSaleCheckbox = document.getElementById("filter-on-sale");
    this.sortSelect = document.getElementById("shop-sort-select");

    this.currentPage = 1;
    this.pageSize = 12;

    this.init();
  }

  init() {
    // Read initial state from URL query parameters
    const urlParams = new URLSearchParams(window.location.search);
    if (urlParams.has("page")) this.currentPage = parseInt(urlParams.get("page")) || 1;
    if (urlParams.has("category")) this.selectedCategory = urlParams.get("category");
    if (urlParams.has("petType")) this.selectedPetType = urlParams.get("petType");
    if (urlParams.has("brand")) this.selectedBrand = urlParams.get("brand");
    if (urlParams.has("search")) this.searchQuery = urlParams.get("search");
    if (urlParams.has("sortBy")) {
      this.sortBy = urlParams.get("sortBy");
      if (this.sortSelect) this.sortSelect.value = this.sortBy;
    }

    this.bindEvents();
    this.loadCatalog();
  }

  bindEvents() {
    if (this.sortSelect) {
      this.sortSelect.addEventListener("change", (e) => {
        this.sortBy = e.target.value;
        this.currentPage = 1;
        this.loadCatalog();
      });
    }

    const filterForm = document.getElementById("shop-filters-form");
    if (filterForm) {
      filterForm.addEventListener("submit", (e) => {
        e.preventDefault();
        this.currentPage = 1;
        this.loadCatalog();
      });
    }

    const clearFiltersBtn = document.getElementById("clear-all-filters-btn");
    if (clearFiltersBtn) {
      clearFiltersBtn.addEventListener("click", () => {
        window.location.href = "shop.html";
      });
    }
  }

  getQueryParams() {
    const params = {
      pageNumber: this.currentPage,
      pageSize: this.pageSize,
      sortBy: this.sortSelect ? this.sortSelect.value : (this.sortBy || "newest")
    };

    const urlParams = new URLSearchParams(window.location.search);

    // Categories
    const checkedCat = document.querySelector("input[name='filter-category']:checked");
    if (checkedCat) {
      params.categorySlug = checkedCat.value;
    } else if (urlParams.has("category")) {
      params.categorySlug = urlParams.get("category");
    }

    // Pet Types
    const checkedPet = document.querySelector("input[name='filter-pet-type']:checked");
    if (checkedPet) {
      params.petTypeSlug = checkedPet.value;
    } else if (urlParams.has("petType")) {
      params.petTypeSlug = urlParams.get("petType");
    }

    // Brands
    const checkedBrand = document.querySelector("input[name='filter-brand']:checked");
    if (checkedBrand) {
      params.brandSlug = checkedBrand.value;
    } else if (urlParams.has("brand")) {
      params.brandSlug = urlParams.get("brand");
    }

    // Search query
    if (urlParams.has("search")) {
      params.search = urlParams.get("search");
    }

    // Price
    if (this.minPriceInput && this.minPriceInput.value) params.minPrice = this.minPriceInput.value;
    if (this.maxPriceInput && this.maxPriceInput.value) params.maxPrice = this.maxPriceInput.value;

    // Availability & Deals
    if (this.inStockCheckbox && this.inStockCheckbox.checked) params.inStockOnly = true;
    if (this.onSaleCheckbox && this.onSaleCheckbox.checked) params.onSaleOnly = true;

    return params;
  }

  async loadCatalog() {
    if (!this.container) return;

    // Render skeleton placeholders
    this.container.innerHTML = Array(8).fill(0).map(() => `
      <div class="bg-white rounded-2xl border border-slate-200/80 p-4 flex flex-col gap-3">
        <div class="aspect-square rounded-xl skeleton"></div>
        <div class="h-4 w-1/3 rounded skeleton"></div>
        <div class="h-5 w-4/5 rounded skeleton"></div>
        <div class="h-4 w-1/2 rounded skeleton"></div>
        <div class="h-8 w-full rounded-xl skeleton mt-auto"></div>
      </div>
    `).join("");

    const params = this.getQueryParams();

    try {
      const res = await window.api.getProducts(params);

      if (!res.success || !res.data || res.data.items.length === 0) {
        this.container.innerHTML = `
          <div class="col-span-full py-16 text-center">
            <div class="w-16 h-16 bg-slate-100 rounded-full flex items-center justify-center mx-auto mb-4 text-slate-400">
              <svg class="w-8 h-8" fill="none" stroke="currentColor" viewBox="0 0 24 24"><path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M9.172 16.172a4 4 0 015.656 0M9 10h.01M15 10h.01M21 12a9 9 0 11-18 0 9 9 0 0118 0z"/></svg>
            </div>
            <h3 class="text-lg font-bold text-slate-800 mb-1">No products match your criteria</h3>
            <p class="text-sm text-slate-500 mb-6 max-w-sm mx-auto">Try clearing selected filters or searching with different keywords.</p>
            <button onclick="window.location.href='shop.html'" class="px-5 py-2.5 rounded-xl bg-teal-600 hover:bg-teal-700 text-white font-semibold text-sm transition">
              Reset All Filters
            </button>
          </div>
        `;
        if (this.countElement) this.countElement.textContent = "0 products found";
        if (this.paginationContainer) this.paginationContainer.innerHTML = "";
        return;
      }

      const paged = res.data;
      if (this.countElement) {
        this.countElement.textContent = `Showing ${(paged.pageNumber - 1) * paged.pageSize + 1}–${Math.min(paged.pageNumber * paged.pageSize, paged.totalCount)} of ${paged.totalCount} products`;
      }

      this.container.innerHTML = paged.items.map(p => window.renderProductCard(p)).join("");
      this.renderPagination(paged);
    } catch (err) {
      console.error("Failed to load catalog:", err);
      this.container.innerHTML = `<div class="col-span-full py-12 text-center text-red-600">Failed to connect to backend server. Please check your network.</div>`;
    }
  }

  renderPagination(paged) {
    if (!this.paginationContainer) return;
    if (paged.totalPages <= 1) {
      this.paginationContainer.innerHTML = "";
      return;
    }

    let html = `<div class="flex items-center gap-1.5 justify-center py-6">`;

    // Previous Button
    html += `
      <button ${!paged.hasPrevious ? 'disabled' : ''} onclick="window.catalogController.changePage(${paged.pageNumber - 1})" class="p-2 rounded-xl border border-slate-200 text-slate-600 hover:bg-teal-50 hover:text-teal-700 disabled:opacity-40 disabled:pointer-events-none transition">
        <svg class="w-5 h-5" fill="none" stroke="currentColor" viewBox="0 0 24 24"><path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M15 19l-7-7 7-7"/></svg>
      </button>
    `;

    for (let i = 1; i <= paged.totalPages; i++) {
      if (i === paged.pageNumber) {
        html += `<span class="w-10 h-10 rounded-xl bg-teal-600 text-white font-bold flex items-center justify-center text-sm shadow-sm">${i}</span>`;
      } else {
        html += `<button onclick="window.catalogController.changePage(${i})" class="w-10 h-10 rounded-xl border border-slate-200 text-slate-700 hover:bg-teal-50 hover:text-teal-700 font-semibold text-sm transition">${i}</button>`;
      }
    }

    // Next Button
    html += `
      <button ${!paged.hasNext ? 'disabled' : ''} onclick="window.catalogController.changePage(${paged.pageNumber + 1})" class="p-2 rounded-xl border border-slate-200 text-slate-600 hover:bg-teal-50 hover:text-teal-700 disabled:opacity-40 disabled:pointer-events-none transition">
        <svg class="w-5 h-5" fill="none" stroke="currentColor" viewBox="0 0 24 24"><path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M9 5l7 7-7 7"/></svg>
      </button>
    `;

    html += `</div>`;
    this.paginationContainer.innerHTML = html;
  }

  changePage(page) {
    this.currentPage = page;
    this.loadCatalog();
    window.scrollTo({ top: 0, behavior: 'smooth' });
  }
}

document.addEventListener("DOMContentLoaded", () => {
  if (document.getElementById("shop-products-grid")) {
    window.catalogController = new ProductCatalogController();
  }
});
