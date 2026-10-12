-- ============================================================================
-- 14_views.sql
-- Analytical and Storefront SQL Views
-- Compatible with MySQL 8.0+ / MariaDB 10.5+
-- ============================================================================

USE pet_ecommerce_db;

-- ----------------------------------------------------------------------------
-- 1. VIEW: available_products
-- Active products currently in stock, joined with brand, category, pet type,
-- and primary thumbnail for lightning-fast catalog querying.
-- ----------------------------------------------------------------------------
DROP VIEW IF EXISTS available_products;
CREATE VIEW available_products AS
SELECT 
    p.id AS product_id,
    p.name AS product_name,
    p.slug AS product_slug,
    p.sku,
    pt.name AS pet_type,
    c.name AS category_name,
    b.name AS brand_name,
    p.base_price,
    p.compare_at_price,
    p.stock_quantity,
    p.is_featured,
    p.target_life_stage,
    p.food_type,
    p.health_concern,
    img.image_url AS primary_image_url
FROM products p
JOIN pet_types pt ON p.pet_type_id = pt.id
JOIN categories c ON p.category_id = c.id
LEFT JOIN brands b ON p.brand_id = b.id
LEFT JOIN product_images img ON p.id = img.product_id AND img.is_primary = TRUE
WHERE p.status = 'active' 
  AND p.stock_quantity > 0;

-- ----------------------------------------------------------------------------
-- 2. VIEW: low_stock_products
-- Identifies items that need immediate inventory reordering
-- ----------------------------------------------------------------------------
DROP VIEW IF EXISTS low_stock_products;
CREATE VIEW low_stock_products AS
SELECT 
    p.id AS product_id,
    p.sku,
    p.name AS product_name,
    b.name AS brand_name,
    c.name AS category_name,
    p.stock_quantity,
    p.low_stock_threshold,
    CASE 
        WHEN p.stock_quantity = 0 THEN 'OUT_OF_STOCK'
        WHEN p.stock_quantity <= p.low_stock_threshold THEN 'CRITICAL_LOW_STOCK'
        ELSE 'NORMAL'
    END AS inventory_status,
    p.cost_price,
    p.base_price
FROM products p
LEFT JOIN brands b ON p.brand_id = b.id
JOIN categories c ON p.category_id = c.id
WHERE p.stock_quantity <= p.low_stock_threshold
ORDER BY p.stock_quantity ASC;

-- ----------------------------------------------------------------------------
-- 3. VIEW: discounted_products
-- Products featuring active markdown deals with calculated savings
-- ----------------------------------------------------------------------------
DROP VIEW IF EXISTS discounted_products;
CREATE VIEW discounted_products AS
SELECT 
    p.id AS product_id,
    p.name AS product_name,
    p.slug AS product_slug,
    b.name AS brand_name,
    c.name AS category_name,
    p.base_price AS sale_price,
    p.compare_at_price AS original_price,
    (p.compare_at_price - p.base_price) AS discount_amount,
    ROUND(((p.compare_at_price - p.base_price) / p.compare_at_price) * 100, 1) AS discount_percentage,
    p.stock_quantity
FROM products p
LEFT JOIN brands b ON p.brand_id = b.id
JOIN categories c ON p.category_id = c.id
WHERE p.status = 'active'
  AND p.compare_at_price IS NOT NULL
  AND p.compare_at_price > p.base_price
ORDER BY discount_percentage DESC;

-- ----------------------------------------------------------------------------
-- 4. VIEW: best_selling_products
-- Products ranked by units sold and revenue from fulfilled/confirmed orders
-- ----------------------------------------------------------------------------
DROP VIEW IF EXISTS best_selling_products;
CREATE VIEW best_selling_products AS
SELECT 
    p.id AS product_id,
    p.name AS product_name,
    p.sku,
    b.name AS brand_name,
    pt.name AS pet_type,
    COALESCE(SUM(oi.quantity), 0) AS total_units_sold,
    COALESCE(SUM(oi.total_price), 0.00) AS total_sales_revenue,
    COUNT(DISTINCT oi.order_id) AS order_occurrences
FROM products p
LEFT JOIN brands b ON p.brand_id = b.id
JOIN pet_types pt ON p.pet_type_id = pt.id
LEFT JOIN order_items oi ON p.id = oi.product_id
LEFT JOIN orders o ON oi.order_id = o.id AND o.order_status NOT IN ('cancelled', 'returned')
GROUP BY p.id, p.name, p.sku, b.name, pt.name
HAVING total_units_sold > 0
ORDER BY total_units_sold DESC, total_sales_revenue DESC;

-- ----------------------------------------------------------------------------
-- 5. VIEW: product_rating_summary
-- Aggregated customer review metrics and star distributions
-- ----------------------------------------------------------------------------
DROP VIEW IF EXISTS product_rating_summary;
CREATE VIEW product_rating_summary AS
SELECT 
    p.id AS product_id,
    p.name AS product_name,
    COUNT(r.id) AS total_approved_reviews,
    COALESCE(ROUND(AVG(r.rating), 2), 0.00) AS average_rating,
    SUM(CASE WHEN r.rating = 5 THEN 1 ELSE 0 END) AS five_star_count,
    SUM(CASE WHEN r.rating = 4 THEN 1 ELSE 0 END) AS four_star_count,
    SUM(CASE WHEN r.rating = 3 THEN 1 ELSE 0 END) AS three_star_count,
    SUM(CASE WHEN r.rating = 2 THEN 1 ELSE 0 END) AS two_star_count,
    SUM(CASE WHEN r.rating = 1 THEN 1 ELSE 0 END) AS one_star_count
FROM products p
LEFT JOIN reviews r ON p.id = r.product_id AND r.status = 'approved'
GROUP BY p.id, p.name;

-- ----------------------------------------------------------------------------
-- 6. VIEW: order_summary
-- Executive order book combining customer profiles, financial totals, and statuses
-- ----------------------------------------------------------------------------
DROP VIEW IF EXISTS order_summary;
CREATE VIEW order_summary AS
SELECT 
    o.id AS order_id,
    o.order_number,
    o.created_at AS order_date,
    u.id AS customer_id,
    u.name AS customer_name,
    u.email AS customer_email,
    o.shipping_phone,
    CONCAT(o.shipping_area, ', ', o.shipping_district, ', ', o.shipping_division) AS shipping_destination,
    COUNT(oi.id) AS line_item_count,
    COALESCE(SUM(oi.quantity), 0) AS total_items_quantity,
    o.subtotal,
    o.discount_amount,
    o.delivery_charge,
    o.total_amount,
    o.payment_method,
    o.payment_status,
    o.order_status
FROM orders o
JOIN users u ON o.user_id = u.id
LEFT JOIN order_items oi ON o.id = oi.order_id
GROUP BY 
    o.id, o.order_number, o.created_at, u.id, u.name, u.email, 
    o.shipping_phone, o.shipping_area, o.shipping_district, o.shipping_division,
    o.subtotal, o.discount_amount, o.delivery_charge, o.total_amount,
    o.payment_method, o.payment_status, o.order_status
ORDER BY o.created_at DESC;

SELECT 'Storefront and operational analytical views created successfully!' AS status;
