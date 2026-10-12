-- ============================================================================
-- 16_test_queries.sql
-- Comprehensive Verification Queries for Backend Developers & QA
-- Compatible with MySQL 8.0+ / MariaDB 10.5+
-- ============================================================================

USE pet_ecommerce_db;

-- ----------------------------------------------------------------------------
-- 1. TEST QUERY: All Products with Primary Images and Brand Names
-- ----------------------------------------------------------------------------
SELECT 
    p.id,
    p.sku,
    p.name,
    pt.name AS pet_type,
    c.name AS category,
    b.name AS brand,
    p.base_price,
    p.compare_at_price,
    p.stock_quantity,
    p.status,
    img.image_url AS primary_image
FROM products p
JOIN pet_types pt ON p.pet_type_id = pt.id
JOIN categories c ON p.category_id = c.id
LEFT JOIN brands b ON p.brand_id = b.id
LEFT JOIN product_images img ON p.id = img.product_id AND img.is_primary = TRUE
ORDER BY p.id ASC
LIMIT 10;

-- ----------------------------------------------------------------------------
-- 2. TEST QUERY: Products by Category (e.g. Cat Food subcategories)
-- ----------------------------------------------------------------------------
SELECT 
    p.id,
    p.name,
    c.name AS category_name,
    p.base_price,
    p.stock_quantity
FROM products p
JOIN categories c ON p.category_id = c.id
WHERE c.slug IN ('cat-food', 'dry-cat-food', 'wet-cat-food', 'cat-treats')
   OR c.parent_id = 10
ORDER BY p.base_price ASC;

-- ----------------------------------------------------------------------------
-- 3. TEST QUERY: Products by Pet Type (e.g. Dog Products)
-- ----------------------------------------------------------------------------
SELECT 
    p.id,
    p.name,
    b.name AS brand,
    p.target_life_stage,
    p.base_price,
    p.stock_quantity
FROM products p
JOIN pet_types pt ON p.pet_type_id = pt.id
LEFT JOIN brands b ON p.brand_id = b.id
WHERE pt.slug = 'dog'
  AND p.status = 'active';

-- ----------------------------------------------------------------------------
-- 4. TEST QUERY: Products by Brand (e.g. Royal Canin)
-- ----------------------------------------------------------------------------
SELECT 
    p.id,
    p.sku,
    p.name,
    p.base_price,
    p.stock_quantity
FROM products p
JOIN brands b ON p.brand_id = b.id
WHERE b.slug = 'royal-canin'
ORDER BY p.base_price DESC;

-- ----------------------------------------------------------------------------
-- 5. TEST QUERY: Discounted Products with Savings Percentage
-- (Uses view: discounted_products)
-- ----------------------------------------------------------------------------
SELECT 
    product_name,
    brand_name,
    original_price,
    sale_price,
    discount_amount,
    CONCAT(discount_percentage, '%') AS discount_percentage
FROM discounted_products;

-- ----------------------------------------------------------------------------
-- 6. TEST QUERY: Low-Stock Products Needing Restock Alert
-- (Uses view: low_stock_products)
-- ----------------------------------------------------------------------------
SELECT 
    sku,
    product_name,
    brand_name,
    stock_quantity,
    low_stock_threshold,
    inventory_status
FROM low_stock_products;

-- ----------------------------------------------------------------------------
-- 7. TEST QUERY: Out-of-Stock Products
-- ----------------------------------------------------------------------------
SELECT 
    p.id,
    p.sku,
    p.name,
    b.name AS brand,
    p.stock_quantity,
    p.status
FROM products p
LEFT JOIN brands b ON p.brand_id = b.id
WHERE p.stock_quantity = 0 OR p.status = 'out_of_stock';

-- ----------------------------------------------------------------------------
-- 8. TEST QUERY: Highest-Rated Products
-- (Uses view: product_rating_summary)
-- ----------------------------------------------------------------------------
SELECT 
    product_name,
    total_approved_reviews,
    average_rating,
    five_star_count
FROM product_rating_summary
WHERE total_approved_reviews > 0
ORDER BY average_rating DESC, total_approved_reviews DESC;

-- ----------------------------------------------------------------------------
-- 9. TEST QUERY: Best-Selling Products by Revenue and Volume
-- (Uses view: best_selling_products)
-- ----------------------------------------------------------------------------
SELECT 
    product_name,
    brand_name,
    pet_type,
    total_units_sold,
    CONCAT(total_sales_revenue, ' BDT') AS total_revenue
FROM best_selling_products;

-- ----------------------------------------------------------------------------
-- 10. TEST QUERY: Customer Orders & Delivery Details
-- ----------------------------------------------------------------------------
SELECT 
    o.order_number,
    u.name AS customer_name,
    o.shipping_phone,
    o.shipping_division,
    o.shipping_district,
    o.shipping_area,
    o.order_status,
    o.payment_method,
    o.payment_status,
    o.total_amount,
    o.created_at
FROM orders o
JOIN users u ON o.user_id = u.id
ORDER BY o.created_at DESC;

-- ----------------------------------------------------------------------------
-- 11. TEST QUERY: Order Line Items and Totals Breakdown for a Specific Order
-- ----------------------------------------------------------------------------
SELECT 
    o.order_number,
    oi.product_name,
    COALESCE(oi.variant_name, 'Standard') AS variant_option,
    oi.sku,
    oi.unit_price,
    oi.quantity,
    oi.total_price AS item_subtotal,
    o.subtotal AS order_subtotal,
    o.discount_amount AS coupon_discount,
    o.delivery_charge,
    o.total_amount AS final_invoice_total
FROM orders o
JOIN order_items oi ON o.id = oi.order_id
WHERE o.order_number = 'ORD-202610-0001';

-- ----------------------------------------------------------------------------
-- 12. TEST QUERY: Coupon Validation Engine Simulation
-- Validates if coupon 'WELCOME10' is valid for user_id = 4 with cart_amount = 2500
-- ----------------------------------------------------------------------------
SELECT 
    c.code,
    c.discount_type,
    c.discount_value,
    CASE 
        WHEN c.is_active = FALSE THEN 'INACTIVE_COUPON'
        WHEN NOW() NOT BETWEEN c.start_date AND c.expiry_date THEN 'EXPIRED_OR_NOT_STARTED'
        WHEN 2500.00 < c.min_order_amount THEN 'MIN_ORDER_REQUIREMENT_NOT_MET'
        WHEN c.usage_limit IS NOT NULL AND c.times_used >= c.usage_limit THEN 'MAX_TOTAL_USAGE_EXCEEDED'
        WHEN (SELECT COUNT(*) FROM coupon_usages cu WHERE cu.coupon_id = c.id AND cu.user_id = 4) >= c.per_user_limit THEN 'PER_USER_LIMIT_EXCEEDED'
        ELSE 'VALID'
    END AS coupon_validation_status,
    CASE 
        WHEN c.discount_type = 'percentage' THEN 
            LEAST((2500.00 * (c.discount_value / 100)), COALESCE(c.max_discount_amount, 2500.00))
        ELSE c.discount_value
    END AS estimated_discount_bdt
FROM coupons c
WHERE c.code = 'WELCOME10';

-- ----------------------------------------------------------------------------
-- 13. TEST QUERY: Inventory Audit Trail for Product ID 4 (Reflex Salmon)
-- ----------------------------------------------------------------------------
SELECT 
    it.id,
    it.created_at,
    p.name AS product_name,
    it.transaction_type,
    it.quantity AS stock_change,
    it.previous_stock,
    it.new_stock,
    it.reference_order_id,
    it.note
FROM inventory_transactions it
JOIN products p ON it.product_id = p.id
WHERE p.id = 4
ORDER BY it.created_at DESC;

-- ----------------------------------------------------------------------------
-- 14. TEST QUERY: Personalized Recommendation Engine for "My Pet"
-- Given Milo (User 4's Adult Persian Cat prone to Hairballs), recommend items:
-- ----------------------------------------------------------------------------
SELECT 
    p.id,
    p.name,
    b.name AS brand,
    p.target_life_stage,
    p.food_type,
    p.health_concern,
    p.base_price,
    p.stock_quantity
FROM products p
LEFT JOIN brands b ON p.brand_id = b.id
WHERE p.pet_type_id = 1 -- Cat
  AND (p.target_life_stage IN ('adult', 'all'))
  AND (p.health_concern = 'Hairball' OR p.food_type IN ('dry', 'treat'))
  AND p.status = 'active'
ORDER BY (p.health_concern = 'Hairball') DESC, p.base_price ASC
LIMIT 5;

SELECT 'All 14 test queries executed successfully!' AS status;
