-- ============================================================================
-- 07_seed_product_variants.sql
-- Seed Data for Multi-Option Product Variants (Weights, Sizes, Scents)
-- Compatible with MySQL 8.0+ / MariaDB 10.5+
-- ============================================================================

USE pet_ecommerce_db;

INSERT INTO product_variants (
    id, product_id, variant_name, sku, price, compare_at_price, cost_price, stock_quantity, weight, status
) VALUES
-- Product 2: Royal Canin Kitten Dry Food
(1, 2, '400g Pouch', 'RC-CAT-KIT-400G', 650.00, 720.00, 500.00, 50, 0.40, 'active'),
(2, 2, '2kg Bag', 'RC-CAT-KIT-2KG-VAR', 2850.00, 3100.00, 2300.00, 40, 2.00, 'active'),
(3, 2, '4kg Value Bag', 'RC-CAT-KIT-4KG', 5400.00, 5900.00, 4400.00, 25, 4.00, 'active'),
(4, 2, '10kg Breeder Pack', 'RC-CAT-KIT-10KG', 12500.00, 13800.00, 10200.00, 10, 10.00, 'active'),

-- Product 4: Reflex Plus Adult Salmon
(5, 4, '1.5kg Bag', 'REF-CAT-SAL-15KG-VAR', 1650.00, 1850.00, 1300.00, 45, 1.50, 'active'),
(6, 4, '8kg Economy Pack', 'REF-CAT-SAL-8KG', 6800.00, 7400.00, 5400.00, 20, 8.00, 'active'),
(7, 4, '15kg Mega Pack', 'REF-CAT-SAL-15KG-MEGA', 11900.00, 12900.00, 9600.00, 12, 15.00, 'active'),

-- Product 6: Whiskas Ocean Fish Dry Food
(8, 6, '450g Trial Pack', 'WHK-CAT-OF-450G', 390.00, 440.00, 310.00, 80, 0.45, 'active'),
(9, 6, '1.2kg Everyday Pack', 'WHK-CAT-OF-12KG-VAR', 980.00, 1100.00, 780.00, 60, 1.20, 'active'),
(10, 6, '3kg Family Pack', 'WHK-CAT-OF-3KG', 2350.00, 2600.00, 1850.00, 30, 3.00, 'active'),
(11, 6, '7kg Mega Saver', 'WHK-CAT-OF-7KG', 5100.00, 5600.00, 4100.00, 15, 7.00, 'active'),

-- Product 12: Kit Cat SoyaClump Tofu Litter
(12, 12, 'Original Scent 7L', 'KTC-LTR-TOFU-ORIG-7L', 1250.00, 1400.00, 950.00, 35, 2.80, 'active'),
(13, 12, 'Green Tea 7L', 'KTC-LTR-TOFU-TEA-7L', 1250.00, 1400.00, 950.00, 40, 2.80, 'active'),
(14, 12, 'Activated Charcoal 7L', 'KTC-LTR-TOFU-CHAR-7L', 1280.00, 1450.00, 980.00, 25, 2.80, 'active'),
(15, 12, 'Coffee Aroma 7L', 'KTC-LTR-TOFU-COF-7L', 1250.00, 1400.00, 950.00, 20, 2.80, 'active'),

-- Product 17: Royal Canin Maxi Puppy Dog Food
(16, 17, '4kg Bag', 'RC-DOG-MXPUP-4KG-VAR', 4450.00, 4800.00, 3600.00, 20, 4.00, 'active'),
(17, 17, '10kg Breeder Sack', 'RC-DOG-MXPUP-10KG', 9800.00, 10800.00, 7900.00, 12, 10.00, 'active'),
(18, 17, '15kg Giant Sack', 'RC-DOG-MXPUP-15KG', 13800.00, 15000.00, 11200.00, 8, 15.00, 'active'),

-- Product 19: Drools Focus Adult Super Premium Dog Food
(19, 19, '1.2kg Bag', 'DRL-DOG-FOC-12KG', 950.00, 1100.00, 760.00, 40, 1.20, 'active'),
(20, 19, '4kg Bag', 'DRL-DOG-FOC-4KG-VAR', 2950.00, 3300.00, 2350.00, 30, 4.00, 'active'),
(21, 19, '12kg Jumbo Pack', 'DRL-DOG-FOC-12KG-JUM', 7800.00, 8600.00, 6200.00, 14, 12.00, 'active'),

-- Product 27: Reflective No-Pull Dog Harness
(22, 27, 'Size Small (Chest 40-55cm)', 'ACC-DOG-HARN-S', 1100.00, 1250.00, 720.00, 15, 0.25, 'active'),
(23, 27, 'Size Medium (Chest 55-70cm)', 'ACC-DOG-HARN-M-VAR', 1250.00, 1450.00, 820.00, 25, 0.35, 'active'),
(24, 27, 'Size Large (Chest 70-90cm)', 'ACC-DOG-HARN-L', 1400.00, 1600.00, 920.00, 20, 0.45, 'active'),
(25, 27, 'Size XL (Chest 85-110cm)', 'ACC-DOG-HARN-XL', 1550.00, 1750.00, 1020.00, 10, 0.55, 'active'),

-- Product 29: Versele-Laga Prestige Budgies Daily Seed Mix
(26, 29, '1kg Pack', 'VL-BRD-BUDG-1KG-VAR', 650.00, 750.00, 480.00, 40, 1.00, 'active'),
(27, 29, '4kg Breeder Bucket', 'VL-BRD-BUDG-4KG', 2400.00, 2750.00, 1800.00, 15, 4.00, 'active'),

-- Product 35: Versele-Laga Cuni Complete Adult Rabbit Pellets
(28, 35, '500g Fresh Pack', 'VL-RAB-CUNI-500G', 650.00, 750.00, 480.00, 30, 0.50, 'active'),
(29, 35, '1.75kg Standard Bag', 'VL-RAB-CUNI-175KG-VAR', 1850.00, 2100.00, 1420.00, 25, 1.75, 'active'),
(30, 35, '8kg Value Sack', 'VL-RAB-CUNI-8KG', 6900.00, 7800.00, 5300.00, 8, 8.00, 'active')
ON DUPLICATE KEY UPDATE variant_name=VALUES(variant_name);

SELECT '30 product variants seeded successfully!' AS status;
