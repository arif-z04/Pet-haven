/**
 * PawHaven BD - Universal Header, Mega Menu, Mobile Drawer, Sticky Bottom Bar & Footer
 */

(function() {
  const headerHtml = `
    <!-- Top Announcement Bar (Collapsible on mobile) -->
    <div class="bg-slate-900 text-white text-[11px] sm:text-xs py-1.5 sm:py-2 px-3 sm:px-4 border-b border-slate-800">
      <div class="max-w-7xl mx-auto flex items-center justify-between">
        <div class="flex items-center gap-2 sm:gap-4 overflow-hidden">
          <span class="flex items-center gap-1.5 text-teal-400 font-semibold truncate">
            <svg class="w-3.5 h-3.5 shrink-0" fill="none" stroke="currentColor" viewBox="0 0 24 24"><path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M5 13l4 4L19 7"/></svg>
            Same-day Delivery inside Dhaka
          </span>
          <span class="hidden md:inline text-slate-400">•</span>
          <span class="hidden md:inline text-slate-300">Free delivery on orders over ৳ 1,500 with code <strong class="text-amber-400">FREESHIP80</strong></span>
        </div>
        <div class="flex items-center gap-3 text-slate-300 shrink-0">
          <a href="contact.html" class="hover:text-white transition flex items-center gap-1">
            <svg class="w-3.5 h-3.5 text-teal-400 shrink-0" fill="none" stroke="currentColor" viewBox="0 0 24 24"><path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M3 5a2 2 0 012-2h3.28a1 1 0 01.948.684l1.498 4.493a1 1 0 01-.502 1.21l-2.257 1.13a11.042 11.042 0 005.516 5.516l1.13-2.257a1 1 0 011.21-.502l4.493 1.498a1 1 0 01.684.949V19a2 2 0 01-2 2h-1C9.716 21 3 14.284 3 6V5z"/></svg>
            <span class="hidden xs:inline">Helpline:</span> +880 1711-000001
          </a>
        </div>
      </div>
    </div>

    <!-- Main Header -->
    <div class="bg-white border-b border-slate-200/80 sticky top-0 z-40 shadow-sm backdrop-blur-md bg-white/95">
      <div class="max-w-7xl mx-auto px-3 sm:px-6 lg:px-8 py-2.5 sm:py-3.5 flex items-center justify-between gap-2 sm:gap-4">
        
        <!-- Mobile Menu Trigger -->
        <button id="mobile-menu-btn" class="lg:hidden p-2 text-slate-700 hover:text-teal-600 focus:outline-none rounded-xl hover:bg-slate-100 min-w-[44px] min-h-[44px] flex items-center justify-center transition" aria-label="Open mobile menu">
          <svg class="w-6 h-6" fill="none" stroke="currentColor" viewBox="0 0 24 24"><path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M4 6h16M4 12h16M4 18h16"/></svg>
        </button>

        <!-- Brand Logo -->
        <a href="index.html" class="flex items-center gap-2 sm:gap-2.5 shrink-0 group">
          <div class="w-9 h-9 sm:w-10 sm:h-10 rounded-xl bg-teal-600 flex items-center justify-center text-white shadow-md shadow-teal-500/20 group-hover:bg-teal-700 transition">
            <svg class="w-5 h-5 sm:w-6 sm:h-6" fill="currentColor" viewBox="0 0 24 24">
              <path d="M12 2C6.48 2 2 6.48 2 12s4.48 10 10 10 10-4.48 10-10S17.52 2 12 2zm0 18c-4.41 0-8-3.59-8-8s3.59-8 8-8 8 3.59 8 8-3.59 8-8 8zm-2-9c.83 0 1.5-.67 1.5-1.5S10.83 8 10 8s-1.5.67-1.5 1.5S9.17 11 10 11zm4 0c.83 0 1.5-.67 1.5-1.5S14.83 8 14 8s-1.5.67-1.5 1.5.67 1.5 1.5 1.5zm-2 5.5c2.33 0 4.31-1.46 5.11-3.5H6.89c.8 2.04 2.78 3.5 5.11 3.5z"/>
            </svg>
          </div>
          <div>
            <span class="text-lg sm:text-xl font-extrabold text-slate-900 tracking-tight block leading-tight">PawHaven<span class="text-teal-600">.bd</span></span>
            <span class="text-[9px] sm:text-[10px] text-slate-400 font-bold tracking-widest uppercase block">Pet Care & Supplies</span>
          </div>
        </a>

        <!-- Desktop Global Search Bar with Autocomplete Dropdown -->
        <div class="hidden md:flex flex-1 max-w-xl mx-4 relative">
          <form id="header-search-form" class="w-full relative">
            <input 
              id="header-search-input"
              type="text" 
              placeholder="Search food, treats, litter, toys or accessories..." 
              autocomplete="off"
              class="w-full pl-11 pr-24 py-2.5 rounded-full border border-slate-200 bg-slate-50 text-sm focus:bg-white text-slate-800 placeholder-slate-400 transition"
            />
            <svg class="w-5 h-5 text-slate-400 absolute left-4 top-1/2 -translate-y-1/2" fill="none" stroke="currentColor" viewBox="0 0 24 24"><path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M21 21l-6-6m2-5a7 7 0 11-14 0 7 7 0 0114 0z"/></svg>
            <button type="submit" class="absolute right-1.5 top-1/2 -translate-y-1/2 px-4 py-1.5 rounded-full bg-teal-600 hover:bg-teal-700 text-white font-semibold text-xs transition">
              Search
            </button>
          </form>

          <!-- Autocomplete Search Dropdown -->
          <div id="header-search-dropdown" class="absolute left-0 right-0 top-full mt-2 bg-white rounded-2xl shadow-2xl border border-slate-100 hidden z-50 overflow-hidden"></div>
        </div>

        <!-- Header Action Controls -->
        <div class="flex items-center gap-1 sm:gap-3 shrink-0">
          
          <!-- Mobile Search Toggle Button -->
          <button id="mobile-search-toggle-btn" class="md:hidden p-2 text-slate-600 hover:text-teal-600 rounded-xl hover:bg-slate-50 min-w-[44px] min-h-[44px] flex items-center justify-center transition" aria-label="Search">
            <svg class="w-5 h-5" fill="none" stroke="currentColor" viewBox="0 0 24 24"><path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M21 21l-6-6m2-5a7 7 0 11-14 0 7 7 0 0114 0z"/></svg>
          </button>

          <!-- Wishlist Badge -->
          <a href="wishlist.html" class="p-2 text-slate-600 hover:text-teal-600 relative transition rounded-xl hover:bg-slate-50 min-w-[44px] min-h-[44px] flex items-center justify-center" title="Wishlist">
            <svg class="w-5 h-5 sm:w-6 sm:h-6" fill="none" stroke="currentColor" viewBox="0 0 24 24"><path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M4.318 6.318a4.5 4.5 0 000 6.364L12 20.364l7.682-7.682a4.5 4.5 0 00-6.364-6.364L12 7.636l-1.318-1.318a4.5 4.5 0 00-6.364 0z"/></svg>
            <span id="wishlist-badge-count" class="absolute top-1 right-1 w-4 h-4 sm:w-5 sm:h-5 rounded-full bg-amber-500 text-white text-[10px] sm:text-[11px] font-extrabold flex items-center justify-center hidden shadow-sm">0</span>
          </a>

          <!-- Cart Badge -->
          <a href="cart.html" class="p-2 text-slate-600 hover:text-teal-600 relative transition rounded-xl hover:bg-slate-50 min-w-[44px] min-h-[44px] flex items-center justify-center" title="Cart">
            <svg class="w-5 h-5 sm:w-6 sm:h-6" fill="none" stroke="currentColor" viewBox="0 0 24 24"><path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M16 11V7a4 4 0 00-8 0v4M5 9h14l1 12H4L5 9z"/></svg>
            <span id="cart-badge-count" class="absolute top-1 right-1 w-4 h-4 sm:w-5 sm:h-5 rounded-full bg-teal-600 text-white text-[10px] sm:text-[11px] font-extrabold flex items-center justify-center hidden shadow-sm">0</span>
          </a>

          <!-- User Authentication Section (Dynamically updated by auth.js) -->
          <div id="header-auth-container" class="hidden sm:block">
            <a href="login.html" class="flex items-center gap-1.5 text-xs sm:text-sm font-semibold text-slate-700 hover:text-teal-600 transition px-2.5 py-1.5 rounded-xl border border-slate-200 hover:border-teal-500">
              <svg class="w-4 h-4 text-slate-500" fill="none" stroke="currentColor" viewBox="0 0 24 24"><path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M16 7a4 4 0 11-8 0 4 4 0 018 0zM12 14a7 7 0 00-7 7h14a7 7 0 00-7 7z"/></svg>
              <span>Sign In</span>
            </a>
          </div>

        </div>
      </div>

      <!-- Mobile Search Expandable Bar (Visible on mobile toggle) -->
      <div id="mobile-search-row" class="hidden md:hidden px-3 pb-3 pt-1 border-t border-slate-100 bg-white">
        <form id="mobile-search-form" class="relative">
          <input 
            id="mobile-search-input"
            type="text" 
            placeholder="Search food, treats, litter, toys..." 
            autocomplete="off"
            class="w-full pl-10 pr-20 py-2.5 rounded-xl border border-slate-200 bg-slate-50 text-sm focus:bg-white text-slate-800 placeholder-slate-400 transition"
          />
          <svg class="w-4 h-4 text-slate-400 absolute left-3.5 top-1/2 -translate-y-1/2" fill="none" stroke="currentColor" viewBox="0 0 24 24"><path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M21 21l-6-6m2-5a7 7 0 11-14 0 7 7 0 0114 0z"/></svg>
          <button type="submit" class="absolute right-1.5 top-1/2 -translate-y-1/2 px-3 py-1.5 rounded-lg bg-teal-600 text-white font-bold text-xs">
            Search
          </button>
          <div id="mobile-search-dropdown" class="absolute left-0 right-0 top-full mt-2 bg-white rounded-2xl shadow-2xl border border-slate-100 hidden z-50 overflow-hidden"></div>
        </form>
      </div>

      <!-- Desktop Navigation Bar & Mega Menu -->
      <nav class="hidden lg:block border-t border-slate-100 bg-white">
        <div class="max-w-7xl mx-auto px-4 sm:px-6 lg:px-8">
          <ul class="flex items-center gap-1 text-sm font-bold text-slate-700">
            <li><a href="index.html" class="inline-block py-3 px-3 hover:text-teal-600 transition">Home</a></li>
            <li><a href="shop.html" class="inline-block py-3 px-3 hover:text-teal-600 transition">Shop All</a></li>
            <li><a href="shop.html?petType=cat" class="inline-block py-3 px-3 hover:text-teal-600 transition">Cats 🐱</a></li>
            <li><a href="shop.html?petType=dog" class="inline-block py-3 px-3 hover:text-teal-600 transition">Dogs 🐶</a></li>
            <li><a href="shop.html?petType=bird" class="inline-block py-3 px-3 hover:text-teal-600 transition">Birds 🦜</a></li>
            <li><a href="shop.html?petType=rabbit" class="inline-block py-3 px-3 hover:text-teal-600 transition">Rabbits 🐰</a></li>
            <li><a href="brands.html" class="inline-block py-3 px-3 hover:text-teal-600 transition">Brands</a></li>
            <li><a href="my-pets.html" class="inline-flex items-center gap-1 py-3 px-3 text-teal-700 hover:text-teal-800 transition font-extrabold bg-teal-50/70 rounded-lg my-1">My Pet Care</a></li>
            <li><a href="blog.html" class="inline-block py-3 px-3 hover:text-teal-600 transition">Pet Care Blog</a></li>
            <li class="ml-auto"><a href="shop.html?onSaleOnly=true" class="inline-flex items-center gap-1.5 py-3 px-3 text-red-600 hover:text-red-700 transition font-extrabold"><span class="w-2 h-2 rounded-full bg-red-500 animate-ping"></span>Flash Deals</a></li>
          </ul>
        </div>
      </nav>
    </div>

    <!-- Mobile Slide-Over Navigation Drawer -->
    <div id="mobile-drawer" class="fixed inset-0 z-50 hidden">
      <div id="mobile-drawer-backdrop" class="fixed inset-0 bg-slate-900/60 backdrop-blur-sm transition-opacity"></div>
      <div class="relative w-4/5 max-w-xs bg-white h-full shadow-2xl z-10 flex flex-col p-5 overflow-y-auto">
        <div class="flex items-center justify-between pb-4 border-b border-slate-100">
          <div class="flex items-center gap-2">
            <div class="w-8 h-8 rounded-lg bg-teal-600 flex items-center justify-center text-white font-bold text-sm">P</div>
            <span class="font-extrabold text-slate-900 text-base">PawHaven BD</span>
          </div>
          <button id="mobile-drawer-close" class="p-2 text-slate-400 hover:text-slate-600 rounded-lg min-w-[44px] min-h-[44px] flex items-center justify-center" aria-label="Close menu">
            <svg class="w-5 h-5" fill="none" stroke="currentColor" viewBox="0 0 24 24"><path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M6 18L18 6M6 6l12 12"/></svg>
          </button>
        </div>

        <!-- Quick Pet Selector in Drawer -->
        <div class="py-3 border-b border-slate-100">
          <p class="text-[10px] font-bold uppercase tracking-wider text-slate-400 mb-2">Shop By Companion</p>
          <div class="grid grid-cols-4 gap-2 text-center text-xs font-bold text-slate-700">
            <a href="shop.html?petType=cat" class="p-2 rounded-xl bg-slate-50 hover:bg-teal-50">🐱<span class="block text-[10px] mt-0.5">Cats</span></a>
            <a href="shop.html?petType=dog" class="p-2 rounded-xl bg-slate-50 hover:bg-teal-50">🐶<span class="block text-[10px] mt-0.5">Dogs</span></a>
            <a href="shop.html?petType=bird" class="p-2 rounded-xl bg-slate-50 hover:bg-teal-50">🦜<span class="block text-[10px] mt-0.5">Birds</span></a>
            <a href="shop.html?petType=rabbit" class="p-2 rounded-xl bg-slate-50 hover:bg-teal-50">🐰<span class="block text-[10px] mt-0.5">Rabbits</span></a>
          </div>
        </div>

        <ul class="py-3 space-y-1 text-sm font-bold text-slate-700">
          <li><a href="index.html" class="flex items-center gap-2.5 py-2.5 px-3 rounded-xl hover:bg-slate-50"><span>🏠</span> Home</a></li>
          <li><a href="shop.html" class="flex items-center gap-2.5 py-2.5 px-3 rounded-xl hover:bg-slate-50"><span>🛍️</span> All Products Catalog</a></li>
          <li><a href="shop.html?onSaleOnly=true" class="flex items-center gap-2.5 py-2.5 px-3 rounded-xl text-red-600 hover:bg-red-50"><span>⚡</span> Flash Deals & Discounts</a></li>
          <li><a href="brands.html" class="flex items-center gap-2.5 py-2.5 px-3 rounded-xl hover:bg-slate-50"><span>🏷️</span> Authorized Brands</a></li>
          <li><a href="my-pets.html" class="flex items-center gap-2.5 py-2.5 px-3 rounded-xl text-teal-700 hover:bg-teal-50"><span>🐾</span> My Pet Profiles</a></li>
          <li><a href="orders.html" class="flex items-center gap-2.5 py-2.5 px-3 rounded-xl hover:bg-slate-50"><span>📦</span> Track My Orders</a></li>
          <li><a href="blog.html" class="flex items-center gap-2.5 py-2.5 px-3 rounded-xl hover:bg-slate-50"><span>📖</span> Pet Care Knowledge Hub</a></li>
          <li><a href="contact.html" class="flex items-center gap-2.5 py-2.5 px-3 rounded-xl hover:bg-slate-50"><span>📞</span> Helpline & Contact</a></li>
        </ul>

        <div id="mobile-drawer-auth-container" class="mt-auto pt-4 border-t border-slate-100 text-xs">
          <a href="login.html" class="block w-full text-center py-2.5 px-4 rounded-xl bg-teal-600 text-white font-bold mb-2">
            Sign In / Register
          </a>
          <p class="text-[11px] text-slate-400 text-center">Helpline: +880 1711-000001</p>
        </div>
      </div>
    </div>

    <!-- Mobile Sticky Bottom Navigation Bar (Visible only on lg:hidden) -->
    <nav id="mobile-bottom-nav" class="lg:hidden fixed bottom-0 left-0 right-0 z-40 bg-white/95 backdrop-blur-md border-t border-slate-200/80 shadow-[0_-4px_20px_rgba(0,0,0,0.06)] px-2 py-1 flex items-center justify-around">
      <!-- 1. Home -->
      <a href="index.html" class="flex flex-col items-center justify-center p-1 text-slate-600 hover:text-teal-600 focus:text-teal-600 transition min-w-[56px] min-h-[48px]">
        <svg class="w-5 h-5 mb-0.5" fill="none" stroke="currentColor" viewBox="0 0 24 24"><path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M3 12l2-2m0 0l7-7 7 7M5 10v10a1 1 0 001 1h3m10-11l2 2m-2-2v10a1 1 0 01-1 1h-3m-6 0a1 1 0 001-1v-4a1 1 0 011-1h2a1 1 0 011 1v4a1 1 0 001 1m-6 0h6"/></svg>
        <span class="text-[10px] font-bold">Home</span>
      </a>

      <!-- 2. Shop/Categories -->
      <a href="shop.html" class="flex flex-col items-center justify-center p-1 text-slate-600 hover:text-teal-600 focus:text-teal-600 transition min-w-[56px] min-h-[48px]">
        <svg class="w-5 h-5 mb-0.5" fill="none" stroke="currentColor" viewBox="0 0 24 24"><path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M4 6a2 2 0 012-2h2a2 2 0 012 2v2a2 2 0 01-2 2H6a2 2 0 01-2-2V6zM14 6a2 2 0 012-2h2a2 2 0 012 2v2a2 2 0 01-2 2h-2a2 2 0 01-2-2V6zM4 16a2 2 0 012-2h2a2 2 0 012 2v2a2 2 0 01-2 2H6a2 2 0 01-2-2v-2zM14 16a2 2 0 012-2h2a2 2 0 012 2v2a2 2 0 01-2 2h-2a2 2 0 01-2-2v-2z"/></svg>
        <span class="text-[10px] font-bold">Shop</span>
      </a>

      <!-- 3. Search Trigger -->
      <button id="mobile-bottom-search-btn" class="flex flex-col items-center justify-center p-1 text-slate-600 hover:text-teal-600 focus:text-teal-600 transition min-w-[56px] min-h-[48px]">
        <svg class="w-5 h-5 mb-0.5" fill="none" stroke="currentColor" viewBox="0 0 24 24"><path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M21 21l-6-6m2-5a7 7 0 11-14 0 7 7 0 0114 0z"/></svg>
        <span class="text-[10px] font-bold">Search</span>
      </button>

      <!-- 4. Cart -->
      <a href="cart.html" class="flex flex-col items-center justify-center p-1 text-slate-600 hover:text-teal-600 focus:text-teal-600 transition min-w-[56px] min-h-[48px] relative">
        <div class="relative">
          <svg class="w-5 h-5 mb-0.5" fill="none" stroke="currentColor" viewBox="0 0 24 24"><path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M16 11V7a4 4 0 00-8 0v4M5 9h14l1 12H4L5 9z"/></svg>
          <span id="mobile-bottom-cart-badge" class="absolute -top-1.5 -right-2.5 w-4 h-4 rounded-full bg-teal-600 text-white text-[9px] font-extrabold flex items-center justify-center hidden shadow-sm">0</span>
        </div>
        <span class="text-[10px] font-bold">Cart</span>
      </a>

      <!-- 5. Account -->
      <a href="account.html" class="flex flex-col items-center justify-center p-1 text-slate-600 hover:text-teal-600 focus:text-teal-600 transition min-w-[56px] min-h-[48px]">
        <svg class="w-5 h-5 mb-0.5" fill="none" stroke="currentColor" viewBox="0 0 24 24"><path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M16 7a4 4 0 11-8 0 4 4 0 018 0zM12 14a7 7 0 00-7 7h14a7 7 0 00-7 7z"/></svg>
        <span class="text-[10px] font-bold">Account</span>
      </a>
    </nav>
  `;

  const footerHtml = `
    <!-- Comprehensive Footer -->
    <footer class="bg-slate-900 text-white pt-12 pb-24 lg:pb-8 border-t border-slate-800">
      <div class="max-w-7xl mx-auto px-4 sm:px-6 lg:px-8">
        <div class="grid grid-cols-1 md:grid-cols-2 lg:grid-cols-5 gap-8 lg:gap-10 pb-10 border-b border-slate-800">
          
          <!-- Column 1: Brand Info -->
          <div class="lg:col-span-2">
            <div class="flex items-center gap-2.5 mb-4">
              <div class="w-9 h-9 rounded-xl bg-teal-600 flex items-center justify-center text-white font-bold">
                <svg class="w-5 h-5" fill="currentColor" viewBox="0 0 24 24"><path d="M12 2C6.48 2 2 6.48 2 12s4.48 10 10 10 10-4.48 10-10S17.52 2 12 2zm0 18c-4.41 0-8-3.59-8-8s3.59-8 8-8 8 3.59 8 8-3.59 8-8 8z"/></svg>
              </div>
              <span class="text-xl sm:text-2xl font-extrabold text-white tracking-tight">PawHaven<span class="text-teal-400">.bd</span></span>
            </div>
            <p class="text-xs sm:text-sm text-slate-400 leading-relaxed max-w-sm mb-5">
              Bangladesh's trusted destination for 100% authentic pet food, veterinary supplements, hygiene litters, and playful accessories.
            </p>
            <div class="flex flex-wrap items-center gap-2 text-slate-400">
              <span class="text-[11px] bg-slate-800 px-2.5 py-1 rounded-lg border border-slate-700">🚚 Same-day Dhaka</span>
              <span class="text-[11px] bg-slate-800 px-2.5 py-1 rounded-lg border border-slate-700">🛡️ 100% Genuine</span>
              <span class="text-[11px] bg-slate-800 px-2.5 py-1 rounded-lg border border-slate-700">💳 Cash On Delivery</span>
            </div>
          </div>

          <!-- Column 2: Quick Links -->
          <div>
            <h4 class="font-bold text-white text-xs sm:text-sm mb-3 sm:mb-4 uppercase tracking-wider">Shop by Pet</h4>
            <ul class="space-y-2 text-xs sm:text-sm text-slate-400">
              <li><a href="shop.html?petType=cat" class="hover:text-teal-400 transition">Cat Supplies</a></li>
              <li><a href="shop.html?petType=dog" class="hover:text-teal-400 transition">Dog Food & Chews</a></li>
              <li><a href="shop.html?petType=bird" class="hover:text-teal-400 transition">Bird Seeds & Cages</a></li>
              <li><a href="shop.html?petType=rabbit" class="hover:text-teal-400 transition">Rabbit Hay & Pellets</a></li>
              <li><a href="shop.html?category=cat-litter-hygiene" class="hover:text-teal-400 transition">Odor-Free Litters</a></li>
            </ul>
          </div>

          <!-- Column 3: Customer Care -->
          <div>
            <h4 class="font-bold text-white text-xs sm:text-sm mb-3 sm:mb-4 uppercase tracking-wider">Customer Care</h4>
            <ul class="space-y-2 text-xs sm:text-sm text-slate-400">
              <li><a href="orders.html" class="hover:text-teal-400 transition">Track Your Order</a></li>
              <li><a href="contact.html" class="hover:text-teal-400 transition">Shipping & Delivery</a></li>
              <li><a href="contact.html" class="hover:text-teal-400 transition">Return & Refund Policy</a></li>
              <li><a href="my-pets.html" class="hover:text-teal-400 transition">My Pet Profiles</a></li>
              <li><a href="about.html" class="hover:text-teal-400 transition">About PawHaven</a></li>
            </ul>
          </div>

          <!-- Column 4: Hotline & Payment -->
          <div>
            <h4 class="font-bold text-white text-xs sm:text-sm mb-3 sm:mb-4 uppercase tracking-wider">Contact & Support</h4>
            <p class="text-xs sm:text-sm text-slate-400 mb-1.5">Dhanmondi, Dhaka 1209, Bangladesh</p>
            <p class="text-sm font-bold text-teal-400 mb-3">+880 1711-000001</p>
            <p class="text-[11px] text-slate-500 mb-2">Supported Payment Methods:</p>
            <div class="flex items-center gap-1.5 flex-wrap">
              <span class="px-2 py-0.5 bg-slate-800 text-[10px] font-bold rounded border border-slate-700 text-slate-300">bKash</span>
              <span class="px-2 py-0.5 bg-slate-800 text-[10px] font-bold rounded border border-slate-700 text-slate-300">Nagad</span>
              <span class="px-2 py-0.5 bg-slate-800 text-[10px] font-bold rounded border border-slate-700 text-slate-300">Cash on Delivery</span>
            </div>
          </div>
        </div>

        <!-- Bottom Copyright -->
        <div class="pt-6 flex flex-col sm:flex-row items-center justify-between text-xs text-slate-500 gap-3">
          <p>© 2026 PawHaven BD. Authentic pet nutrition for every companion.</p>
          <div class="flex gap-4">
            <a href="about.html" class="hover:text-slate-400 transition">Privacy Policy</a>
            <a href="about.html" class="hover:text-slate-400 transition">Terms of Service</a>
            <a href="contact.html" class="hover:text-slate-400 transition">Contact Us</a>
          </div>
        </div>
      </div>
    </footer>
  `;

  document.addEventListener("DOMContentLoaded", () => {
    const headerPlaceholder = document.getElementById("site-header");
    if (headerPlaceholder) {
      headerPlaceholder.innerHTML = headerHtml;
    }

    const footerPlaceholder = document.getElementById("site-footer");
    if (footerPlaceholder) {
      footerPlaceholder.innerHTML = footerHtml;
    }

    // Mobile Search Bar Toggle
    const mobileSearchBtn = document.getElementById("mobile-search-toggle-btn");
    const mobileSearchRow = document.getElementById("mobile-search-row");
    const mobileBottomSearchBtn = document.getElementById("mobile-bottom-search-btn");

    function toggleMobileSearch() {
      if (mobileSearchRow) {
        mobileSearchRow.classList.toggle("hidden");
        const input = document.getElementById("mobile-search-input");
        if (!mobileSearchRow.classList.contains("hidden") && input) {
          input.focus();
        }
      }
    }

    if (mobileSearchBtn) {
      mobileSearchBtn.addEventListener("click", toggleMobileSearch);
    }
    if (mobileBottomSearchBtn) {
      mobileBottomSearchBtn.addEventListener("click", () => {
        if (mobileSearchRow && mobileSearchRow.classList.contains("hidden")) {
          toggleMobileSearch();
        } else {
          const input = document.getElementById("mobile-search-input");
          if (input) input.focus();
        }
        window.scrollTo({ top: 0, behavior: "smooth" });
      });
    }
  });
})();
