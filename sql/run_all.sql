-- ============================================================================
-- run_all.sql
-- Master Execution Script for Pet E-Commerce Database Setup
-- Usage in MySQL CLI:
--   mysql -u root -p < sql/run_all.sql
--   or inside MySQL shell:
--   SOURCE /path/to/sql/run_all.sql;
-- ============================================================================

SELECT '>>> 01: Initializing Database...' AS Step;
SOURCE 01_create_database.sql;

SELECT '>>> 02: Creating Relational Tables...' AS Step;
SOURCE 02_create_tables.sql;

SELECT '>>> 03: Creating Performance Indexes...' AS Step;
SOURCE 03_create_indexes.sql;

SELECT '>>> 04: Seeding Pet Types & Hierarchical Categories...' AS Step;
SOURCE 04_seed_categories.sql;

SELECT '>>> 05: Seeding Brands...' AS Step;
SOURCE 05_seed_brands.sql;

SELECT '>>> 06: Seeding Products & Gallery Images...' AS Step;
SOURCE 06_seed_products.sql;

SELECT '>>> 07: Seeding Product Variants...' AS Step;
SOURCE 07_seed_product_variants.sql;

SELECT '>>> 08: Seeding Users (Admins, Staff, Customers)...' AS Step;
SOURCE 08_seed_users.sql;

SELECT '>>> 09: Seeding Customer Delivery Addresses...' AS Step;
SOURCE 09_seed_addresses.sql;

SELECT '>>> 10: Seeding Promotional Coupons...' AS Step;
SOURCE 10_seed_coupons.sql;

SELECT '>>> 11: Seeding Homepage Banners...' AS Step;
SOURCE 11_seed_banners.sql;

SELECT '>>> 12: Seeding "My Pet" Profiles...' AS Step;
SOURCE 12_seed_pet_profiles.sql;

SELECT '>>> 13: Seeding Orders, Items, Reviews, Q&A, and Logs...' AS Step;
SOURCE 13_seed_sample_orders.sql;

SELECT '>>> 14: Creating Analytical & Storefront Views...' AS Step;
SOURCE 14_views.sql;

SELECT '>>> 15: Creating Inventory Triggers...' AS Step;
SOURCE 15_triggers.sql;

SELECT '>>> 16: Executing Verification & Analytical Queries...' AS Step;
SOURCE 16_test_queries.sql;

SELECT '=======================================================' AS Status;
SELECT 'PET HAVEN BD DATABASE FULLY DEPLOYED AND VERIFIED!' AS Status;
SELECT '=======================================================' AS Status;
