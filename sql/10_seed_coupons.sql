-- ============================================================================
-- 10_seed_coupons.sql
-- Seed Data for Promotional Coupons and Discount Campaigns
-- Compatible with MySQL 8.0+ / MariaDB 10.5+
-- ============================================================================

USE pet_ecommerce_db;

INSERT INTO coupons (
    id, code, description, discount_type, discount_value, min_order_amount, 
    max_discount_amount, start_date, expiry_date, usage_limit, per_user_limit, times_used, is_active
) VALUES
(1, 'WELCOME10', 'Welcome 10% discount for first-time orders over 1000 BDT', 
 'percentage', 10.00, 1000.00, 300.00, '2026-01-01 00:00:00', '2026-12-31 23:59:59', 1000, 1, 15, TRUE),

(2, 'MEOW100', 'Flat 100 BDT discount on feline products and food orders over 1200 BDT', 
 'fixed_amount', 100.00, 1200.00, NULL, '2026-01-01 00:00:00', '2026-12-31 23:59:59', 500, 1, 28, TRUE),

(3, 'PETLOVER15', 'Mega Pet Lover 15% discount for bulk orders over 3000 BDT', 
 'percentage', 15.00, 3000.00, 600.00, '2026-06-01 00:00:00', '2026-12-31 23:59:59', 200, 2, 8, TRUE),

(4, 'FREESHIP80', 'Flat 80 BDT delivery discount on orders above 1500 BDT', 
 'fixed_amount', 80.00, 1500.00, NULL, '2026-01-01 00:00:00', '2026-12-31 23:59:59', 500, 3, 42, TRUE),

(5, 'PAW500', 'Flat 500 BDT discount for large purchases over 5000 BDT', 
 'fixed_amount', 500.00, 5000.00, NULL, '2026-01-01 00:00:00', '2026-12-31 23:59:59', 100, 1, 4, TRUE),

-- Expired coupon for testing business logic & test queries
(6, 'SUMMERSAVE', 'Expired summer promotional campaign', 
 'percentage', 20.00, 1000.00, 400.00, '2025-06-01 00:00:00', '2025-08-31 23:59:59', 100, 1, 87, FALSE)
ON DUPLICATE KEY UPDATE code=VALUES(code);

SELECT 'Promotional coupons seeded successfully!' AS status;
