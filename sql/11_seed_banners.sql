-- ============================================================================
-- 11_seed_banners.sql
-- Seed Data for Homepage Hero Banners & Promotional Campaigns
-- Compatible with MySQL 8.0+ / MariaDB 10.5+
-- ============================================================================

USE pet_ecommerce_db;

INSERT INTO banners (
    id, title, subtitle, image_url, button_text, button_link, 
    position, sort_order, is_active, start_date, end_date
) VALUES
(1, 'Pure Nourishment for Your Furry Royals', 
 'Explore Royal Canin breed and clinical nutrition formulas with same-day Dhaka delivery.', 
 '/assets/banners/hero-royal-canin.webp', 'Shop Royal Canin', '/brand/royal-canin', 
 'home_hero_slider', 1, TRUE, '2026-01-01 00:00:00', '2026-12-31 23:59:59'),

(2, 'Say Goodbye to Cat Odors Forever!', 
 'Premium Kit Cat SoyaClump & Reflex Lavender bentonite litters starting from only 1150 BDT.', 
 '/assets/banners/hero-cat-litter.webp', 'Explore Litters', '/category/cat-litter-hygiene', 
 'home_hero_slider', 2, TRUE, '2026-01-01 00:00:00', '2026-12-31 23:59:59'),

(3, 'Nurture Healthy Singing Birds & Parrots', 
 'Versele-Laga Belgian seeds and Beaphar vitamin supplements for radiant feathers.', 
 '/assets/banners/hero-bird-care.webp', 'Discover Bird Feed', '/category/bird-food-seeds', 
 'home_hero_slider', 3, TRUE, '2026-01-01 00:00:00', '2026-12-31 23:59:59'),

(4, 'Flat 10% Off on First Order!', 
 'Use coupon code WELCOME10 at checkout on orders over 1000 BDT.', 
 '/assets/banners/strip-welcome-coupon.webp', 'Claim Discount', '/shop', 
 'home_promo_strip', 1, TRUE, '2026-01-01 00:00:00', '2026-12-31 23:59:59'),

(5, 'Rabbit & Small Pet Paradise', 
 'First-cut fragrant Timothy Hay and Cuni Complete fiber-rich pellets in stock now.', 
 '/assets/banners/sidebar-rabbit-care.webp', 'Shop Rabbits', '/category/rabbit-food-hay', 
 'category_sidebar', 1, TRUE, '2026-01-01 00:00:00', '2026-12-31 23:59:59')
ON DUPLICATE KEY UPDATE title=VALUES(title);

SELECT 'Promotional banners seeded successfully!' AS status;
