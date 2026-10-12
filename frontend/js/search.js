/**
 * PawHaven BD - Live Autocomplete Header & Mobile Search Controller
 */

document.addEventListener("DOMContentLoaded", () => {
  function setupSearch(inputId, dropdownId, formId) {
    const searchInput = document.getElementById(inputId);
    const searchDropdown = document.getElementById(dropdownId);
    const searchForm = document.getElementById(formId);

    if (!searchInput || !searchDropdown) return;

    let debounceTimer = null;

    searchInput.addEventListener("input", (e) => {
      const query = e.target.value.trim();
      clearTimeout(debounceTimer);

      if (query.length < 2) {
        searchDropdown.classList.add("hidden");
        searchDropdown.innerHTML = "";
        return;
      }

      debounceTimer = setTimeout(async () => {
        searchDropdown.classList.remove("hidden");
        searchDropdown.innerHTML = `
          <div class="p-4 text-center text-slate-500 text-xs flex items-center justify-center gap-2">
            <div class="w-4 h-4 border-2 border-teal-600 border-t-transparent rounded-full animate-spin"></div>
            Searching pet supplies...
          </div>
        `;

        try {
          const res = await window.api.search(query, { pageSize: 5 });
          if (!res.success || !res.data || res.data.items.length === 0) {
            searchDropdown.innerHTML = `
              <div class="p-4 text-center text-slate-500 text-xs">
                No products found matching "<span class="font-semibold text-slate-700">${query}</span>".
              </div>
            `;
            return;
          }

          const items = res.data.items;
          let html = `<div class="p-2 divide-y divide-slate-100 max-h-72 overflow-y-auto">`;

          items.forEach(item => {
            const imgUrl = window.resolveImageUrl(item.primaryImageUrl, item.name);
            html += `
              <a href="product.html?id=${item.id}" class="flex items-center gap-3 p-2 hover:bg-teal-50/60 rounded-xl transition group">
                <div class="w-11 h-11 bg-slate-50 rounded-lg p-1 flex items-center justify-center shrink-0 border border-slate-100">
                  <img src="${imgUrl}" alt="${item.name}" class="max-w-full max-h-full object-contain">
                </div>
                <div class="flex-1 min-w-0">
                  <p class="text-[10px] font-semibold text-teal-600 uppercase tracking-wider truncate">${item.categoryName || 'Pet Care'}</p>
                  <p class="text-xs font-bold text-slate-800 truncate group-hover:text-teal-700">${item.name}</p>
                  <p class="text-xs font-extrabold text-slate-900 mt-0.5">${window.formatPrice(item.basePrice)}</p>
                </div>
                <svg class="w-4 h-4 text-slate-300 group-hover:text-teal-600 shrink-0" fill="none" stroke="currentColor" viewBox="0 0 24 24"><path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M9 5l7 7-7 7"/></svg>
              </a>
            `;
          });

          html += `
            </div>
            <div class="p-2.5 bg-slate-50 border-t border-slate-100 text-center rounded-b-xl">
              <a href="shop.html?search=${encodeURIComponent(query)}" class="text-xs font-bold text-teal-700 hover:text-teal-800 flex items-center justify-center gap-1">
                View all results for "${query}"
                <svg class="w-3.5 h-3.5" fill="none" stroke="currentColor" viewBox="0 0 24 24"><path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M14 5l7 7m0 0l-7 7m7-7H3"/></svg>
              </a>
            </div>
          `;

          searchDropdown.innerHTML = html;
        } catch (err) {
          searchDropdown.innerHTML = `<div class="p-4 text-center text-red-500 text-xs">Failed to execute search.</div>`;
        }
      }, 300);
    });

    if (searchForm) {
      searchForm.addEventListener("submit", (e) => {
        e.preventDefault();
        const query = searchInput.value.trim();
        if (query) {
          window.location.href = `shop.html?search=${encodeURIComponent(query)}`;
        }
      });
    }

    // Dismiss on outside click
    document.addEventListener("click", (e) => {
      if (!searchInput.contains(e.target) && !searchDropdown.contains(e.target)) {
        searchDropdown.classList.add("hidden");
      }
    });
  }

  // Setup desktop header search
  setupSearch("header-search-input", "header-search-dropdown", "header-search-form");

  // Setup mobile dedicated search
  setupSearch("mobile-search-input", "mobile-search-dropdown", "mobile-search-form");
});
