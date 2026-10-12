-- ============================================================================
-- 05_seed_brands.sql
-- Seed Data for Global and Popular Bangladeshi Market Pet Care Brands
-- Compatible with MySQL 8.0+ / MariaDB 10.5+
-- ============================================================================

USE pet_ecommerce_db;

INSERT INTO brands (id, name, slug, description, logo_url, website_url, is_featured, status) VALUES
(1, 'Royal Canin', 'royal-canin', 'Pioneering scientific nutrition tailored by pet breed, age, and health conditions.', '/assets/brands/royal-canin.webp', 'https://www.royalcanin.com', TRUE, 'active'),
(2, 'Reflex Plus', 'reflex-plus', 'Super premium dry food formulated with XOS prebiotics for optimal feline and canine gut health.', '/assets/brands/reflex-plus.webp', 'https://www.reflexpetfood.com', TRUE, 'active'),
(3, 'Whiskas', 'whiskas', 'Global brand offering delicious dry food and nutrient-packed gravy pouches for felines.', '/assets/brands/whiskas.webp', 'https://www.whiskas.com', TRUE, 'active'),
(4, 'Drools', 'drools', 'Real chicken and egg based nutritional kibbles and dental treats for active dogs and cats.', '/assets/brands/drools.webp', 'https://www.drools.com', TRUE, 'active'),
(5, 'Me-O', 'me-o', 'Thailand-made favorite offering complete cat nutrition, crunchy bites, and creamy treats.', '/assets/brands/me-o.webp', 'https://www.perfectcompanion.com', TRUE, 'active'),
(6, 'SmartHeart', 'smartheart', 'High-quality balanced dry diets and treats for dogs, birds, rabbits, and rodents.', '/assets/brands/smartheart.webp', 'https://www.perfectcompanion.com', TRUE, 'active'),
(7, 'Sheba', 'sheba', 'Luxurious slow-cooked wet food cuts and fillets in irresistible gravies for discerning cats.', '/assets/brands/sheba.webp', 'https://www.sheba.com', FALSE, 'active'),
(8, 'Purina Pro Plan', 'purina-pro-plan', 'Advanced nutrition supported by veterinary science and targeted vitality formulas.', '/assets/brands/purina-pro-plan.webp', 'https://www.purina.com', TRUE, 'active'),
(9, 'Versele-Laga', 'versele-laga', 'Belgian premium seed blends, specialized pellets, and hay for birds, rabbits, and rodents.', '/assets/brands/versele-laga.webp', 'https://www.versele-laga.com', TRUE, 'active'),
(10, 'Beaphar', 'beaphar', 'Pharmaceutical-grade pet healthcare, dewormers, parasite sprays, and multivitamins from the Netherlands.', '/assets/brands/beaphar.webp', 'https://www.beaphar.com', TRUE, 'active'),
(11, 'JerHigh', 'jerhigh', 'Human-grade real chicken meat dog treats enriched with calcium, glucosamine, and vitamin E.', '/assets/brands/jerhigh.webp', 'https://www.jerhigh.com', FALSE, 'active'),
(12, 'Kit Cat', 'kit-cat', 'Natural grain-free wet foods, pure lickable treats, and biodegradable soybean clumping litter.', '/assets/brands/kit-cat.webp', 'https://www.kitcat.com.sg', TRUE, 'active'),
(13, 'Bioline', 'bioline', 'German technology grooming, dental hygiene, flea & tick drops, and catnip sprays.', '/assets/brands/bioline.webp', 'https://www.bioline.de', FALSE, 'active'),
(14, 'Lara Pet Food', 'lara-pet-food', 'Crispy and savory chunk recipes with high acceptance for playful cats.', '/assets/brands/lara.webp', 'https://www.versele-laga.com/lara', FALSE, 'active'),
(15, 'Vitakraft', 'vitakraft', 'German expert manufacturer of enriched seed sticks, rodent crackers, and rabbit treats.', '/assets/brands/vitakraft.webp', 'https://www.vitakraft.com', FALSE, 'active')
ON DUPLICATE KEY UPDATE name=VALUES(name);

SELECT 'Pet care brands seeded successfully!' AS status;
