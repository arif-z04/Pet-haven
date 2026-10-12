-- ============================================================================
-- 03_create_indexes.sql
-- Performance Indexes and Search Optimization
-- Compatible with MySQL 8.0+ / MariaDB 10.5+
-- ============================================================================

USE pet_ecommerce_db;

-- ----------------------------------------------------------------------------
-- 1. USERS & ADDRESSES INDEXES
-- ----------------------------------------------------------------------------
CREATE INDEX idx_users_role_status ON users (role, status);
CREATE INDEX idx_users_created_at ON users (created_at);

CREATE INDEX idx_addresses_user_id ON customer_addresses (user_id);
CREATE INDEX idx_addresses_division_district ON customer_addresses (division, district);

-- ----------------------------------------------------------------------------
-- 2. CATEGORIES & BRANDS INDEXES
-- ----------------------------------------------------------------------------
CREATE INDEX idx_categories_parent_id ON categories (parent_id);
CREATE INDEX idx_categories_pet_type_id ON categories (pet_type_id);
CREATE INDEX idx_categories_status_sort ON categories (status, sort_order);

CREATE INDEX idx_brands_status_featured ON brands (status, is_featured);

-- ----------------------------------------------------------------------------
-- 3. PRODUCTS INDEXES
-- Optimized for catalog filtering, recommendation engine, search, and sorting
-- ----------------------------------------------------------------------------
-- Single-column lookups
CREATE INDEX idx_products_name ON products (name);
CREATE INDEX idx_products_category_id ON products (category_id);
CREATE INDEX idx_products_brand_id ON products (brand_id);
CREATE INDEX idx_products_pet_type_id ON products (pet_type_id);
CREATE INDEX idx_products_status ON products (status);
CREATE INDEX idx_products_base_price ON products (base_price);
CREATE INDEX idx_products_stock_quantity ON products (stock_quantity);

-- Composite indexes for common storefront queries
CREATE INDEX idx_products_catalog_filter ON products (status, pet_type_id, category_id);
CREATE INDEX idx_products_featured ON products (status, is_featured, created_at DESC);
CREATE INDEX idx_products_price_sort ON products (status, base_price);

-- Recommendation index for 'My Pet' profile matching
CREATE INDEX idx_products_recommendation ON products (pet_type_id, target_life_stage, food_type);

-- Fulltext search index for keyword searches (e.g. "Royal Canin Kitten Salmon")
CREATE FULLTEXT INDEX ft_products_search ON products (name, short_description);

-- ----------------------------------------------------------------------------
-- 4. PRODUCT IMAGES & VARIANTS INDEXES
-- ----------------------------------------------------------------------------
CREATE INDEX idx_product_images_product_primary ON product_images (product_id, is_primary);
CREATE INDEX idx_product_variants_product_status ON product_variants (product_id, status);

-- ----------------------------------------------------------------------------
-- 5. ORDERS & ORDER ITEMS INDEXES
-- ----------------------------------------------------------------------------
CREATE INDEX idx_orders_user_created ON orders (user_id, created_at DESC);
CREATE INDEX idx_orders_order_status ON orders (order_status);
CREATE INDEX idx_orders_payment_status ON orders (payment_status);
CREATE INDEX idx_orders_created_at ON orders (created_at);
CREATE INDEX idx_orders_status_date ON orders (order_status, created_at);

CREATE INDEX idx_order_items_order_id ON order_items (order_id);
CREATE INDEX idx_order_items_product_id ON order_items (product_id);
CREATE INDEX idx_order_items_sku ON order_items (sku);

-- ----------------------------------------------------------------------------
-- 6. CARTS & WISHLISTS INDEXES
-- ----------------------------------------------------------------------------
CREATE INDEX idx_cart_items_cart_id ON cart_items (cart_id);
CREATE INDEX idx_cart_items_product_variant ON cart_items (product_id, variant_id);

CREATE INDEX idx_wishlist_items_wishlist_id ON wishlist_items (wishlist_id);
CREATE INDEX idx_wishlist_items_product_id ON wishlist_items (product_id);

-- ----------------------------------------------------------------------------
-- 7. REVIEWS & Q&A INDEXES
-- ----------------------------------------------------------------------------
CREATE INDEX idx_reviews_product_status ON reviews (product_id, status, rating);
CREATE INDEX idx_reviews_user_id ON reviews (user_id);

CREATE INDEX idx_questions_product_status ON product_questions (product_id, status);

-- ----------------------------------------------------------------------------
-- 8. COUPONS & USAGES INDEXES
-- ----------------------------------------------------------------------------
CREATE INDEX idx_coupons_active_dates ON coupons (is_active, start_date, expiry_date);
CREATE INDEX idx_coupon_usages_coupon_user ON coupon_usages (coupon_id, user_id);

-- ----------------------------------------------------------------------------
-- 9. NOTIFICATIONS & INVENTORY TRANSACTIONS
-- ----------------------------------------------------------------------------
CREATE INDEX idx_notifications_user_unread ON notifications (user_id, is_read, created_at DESC);

CREATE INDEX idx_inventory_product_date ON inventory_transactions (product_id, created_at DESC);
CREATE INDEX idx_inventory_type ON inventory_transactions (transaction_type);

-- ----------------------------------------------------------------------------
-- 10. BLOG & BANNERS INDEXES
-- ----------------------------------------------------------------------------
CREATE INDEX idx_blog_posts_category_status ON blog_posts (category_id, status, published_at DESC);
CREATE FULLTEXT INDEX ft_blog_posts_search ON blog_posts (title, content);

CREATE INDEX idx_banners_position_active ON banners (position, is_active, sort_order);

SELECT 'All performance and operational indexes created successfully!' AS status;
