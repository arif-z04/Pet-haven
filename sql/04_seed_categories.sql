-- ============================================================================
-- 04_seed_categories.sql
-- Seed Data for Pet Types and Hierarchical Categories
-- Compatible with MySQL 8.0+ / MariaDB 10.5+
-- ============================================================================

USE pet_ecommerce_db;

-- ----------------------------------------------------------------------------
-- 1. SEED PET TYPES
-- ----------------------------------------------------------------------------
INSERT INTO pet_types (id, name, slug, description, icon_url, sort_order, status) VALUES
(1, 'Cat', 'cat', 'Feline food, litter, healthcare, toys and accessories', '/assets/icons/pets/cat.svg', 1, 'active'),
(2, 'Dog', 'dog', 'Canine dry/wet food, chew toys, grooming and health supplies', '/assets/icons/pets/dog.svg', 2, 'active'),
(3, 'Bird', 'bird', 'Bird seeds, cages, supplements, perches and toys', '/assets/icons/pets/bird.svg', 3, 'active'),
(4, 'Rabbit', 'rabbit', 'Timothy hay, pellets, chew toys, cages and grooming products', '/assets/icons/pets/rabbit.svg', 4, 'active'),
(5, 'Other Small Pets', 'other-small-pets', 'Supplies for hamsters, guinea pigs, turtles and other companions', '/assets/icons/pets/small-pet.svg', 5, 'active')
ON DUPLICATE KEY UPDATE name=VALUES(name);

-- ----------------------------------------------------------------------------
-- 2. SEED TOP-LEVEL ROOT CATEGORIES (parent_id = NULL)
-- ----------------------------------------------------------------------------
INSERT INTO categories (id, parent_id, pet_type_id, name, slug, description, image_url, icon_url, sort_order, status) VALUES
(1, NULL, 1, 'Cat Supplies', 'cat-supplies', 'All essentials and premium products for cats', '/assets/categories/cat-main.webp', '/assets/icons/cat.svg', 1, 'active'),
(2, NULL, 2, 'Dog Supplies', 'dog-supplies', 'Complete range of nutrition, toys and care for dogs', '/assets/categories/dog-main.webp', '/assets/icons/dog.svg', 2, 'active'),
(3, NULL, 3, 'Bird Supplies', 'bird-supplies', 'Nutritious seeds, spacious cages and bird care', '/assets/categories/bird-main.webp', '/assets/icons/bird.svg', 3, 'active'),
(4, NULL, 4, 'Rabbit Supplies', 'rabbit-supplies', 'Hay, premium feed, and comfortable housing for rabbits', '/assets/categories/rabbit-main.webp', '/assets/icons/rabbit.svg', 4, 'active'),
(5, NULL, 5, 'Small Pet Supplies', 'small-pet-supplies', 'Care essentials for hamsters, guinea pigs and other small pets', '/assets/categories/small-pets-main.webp', '/assets/icons/small-pet.svg', 5, 'active')
ON DUPLICATE KEY UPDATE name=VALUES(name);

-- ----------------------------------------------------------------------------
-- 3. SEED SECOND-LEVEL CATEGORIES
-- ----------------------------------------------------------------------------
INSERT INTO categories (id, parent_id, pet_type_id, name, slug, description, image_url, icon_url, sort_order, status) VALUES
-- Cat Subcategories
(10, 1, 1, 'Cat Food', 'cat-food', 'Nutrition formulas tailored for kittens and adult felines', '/assets/categories/cat-food.webp', NULL, 1, 'active'),
(11, 1, 1, 'Cat Litter & Hygiene', 'cat-litter-hygiene', 'Odor-control bentonite, tofu litters and hygiene trays', '/assets/categories/cat-litter.webp', NULL, 2, 'active'),
(12, 1, 1, 'Cat Toys & Scratchers', 'cat-toys-scratchers', 'Interactive laser toys, balls, teaser wands and sisal scratch posts', '/assets/categories/cat-toys.webp', NULL, 3, 'active'),
(13, 1, 1, 'Cat Grooming', 'cat-grooming', 'Deshedding combs, tear stain removers and deodorizing sprays', '/assets/categories/cat-grooming.webp', NULL, 4, 'active'),
(14, 1, 1, 'Cat Healthcare & Supplements', 'cat-healthcare', 'Dewormers, multivitamins, hairball paste and flea treatments', '/assets/categories/cat-healthcare.webp', NULL, 5, 'active'),

-- Dog Subcategories
(20, 2, 2, 'Dog Food', 'dog-food', 'Balanced kibble and wet cans for all puppy and dog breeds', '/assets/categories/dog-food.webp', NULL, 1, 'active'),
(21, 2, 2, 'Dog Treats & Chews', 'dog-treats-chews', 'Calcium dental sticks, jerkies, raw-hides and biscuits', '/assets/categories/dog-treats.webp', NULL, 2, 'active'),
(22, 2, 2, 'Dog Toys & Training', 'dog-toys-training', 'Durable chew toys, squeakers, tug ropes and clickers', '/assets/categories/dog-toys.webp', NULL, 3, 'active'),
(23, 2, 2, 'Dog Grooming', 'dog-grooming', 'Medicated shampoos, conditioners, brushes and nail trimmers', '/assets/categories/dog-grooming.webp', NULL, 4, 'active'),
(24, 2, 2, 'Dog Healthcare & Protection', 'dog-healthcare', 'Tick & flea spot-ons, joint supplements and immune boosters', '/assets/categories/dog-healthcare.webp', NULL, 5, 'active'),
(25, 2, 2, 'Leashes, Collars & Harnesses', 'dog-leashes-collars', 'Reflective walking harnesses, retractable leashes and collars', '/assets/categories/dog-accessories.webp', NULL, 6, 'active'),

-- Bird Subcategories
(30, 3, 3, 'Bird Food & Seeds', 'bird-food-seeds', 'Specialized seed mixes for Budgies, Cockatiels, Finches and Parrots', '/assets/categories/bird-food.webp', NULL, 1, 'active'),
(31, 3, 3, 'Bird Cages & Feeders', 'bird-cages-feeders', 'Durable iron cages, breeding boxes, perches and automatic drinkers', '/assets/categories/bird-cages.webp', NULL, 2, 'active'),
(32, 3, 3, 'Bird Toys', 'bird-toys', 'Wooden hanging ladders, chewing bells, and colorful swings', '/assets/categories/bird-toys.webp', NULL, 3, 'active'),
(33, 3, 3, 'Bird Healthcare & Supplements', 'bird-healthcare', 'Cuttlefish bones, mineral blocks and water-soluble vitamins', '/assets/categories/bird-healthcare.webp', NULL, 4, 'active'),

-- Rabbit Subcategories
(40, 4, 4, 'Rabbit Food & Hay', 'rabbit-food-hay', 'First-cut Timothy hay, alfalfa grass and fiber-rich rabbit pellets', '/assets/categories/rabbit-food.webp', NULL, 1, 'active'),
(41, 4, 4, 'Rabbit Accessories & Cages', 'rabbit-accessories', 'Spacious playpens, ceramic food bowls and water sippers', '/assets/categories/rabbit-accessories.webp', NULL, 2, 'active'),
(42, 4, 4, 'Rabbit Healthcare & Grooming', 'rabbit-healthcare', 'Hairball digestive treats, nail clippers and brush sets', '/assets/categories/rabbit-healthcare.webp', NULL, 3, 'active'),

-- Other Small Pets Subcategories
(50, 5, 5, 'Small Pet Food', 'small-pet-food', 'Balanced nutrition for hamsters, guinea pigs and hedgehogs', '/assets/categories/small-pet-food.webp', NULL, 1, 'active'),
(51, 5, 5, 'Bedding & Habitats', 'small-pet-bedding', 'Dust-free wood shavings, paper bedding and exercise wheels', '/assets/categories/small-pet-bedding.webp', NULL, 2, 'active'),
(52, 5, 5, 'Small Pet Healthcare', 'small-pet-healthcare', 'Vitamin drops, chew blocks and sanitizing sprays', '/assets/categories/small-pet-healthcare.webp', NULL, 3, 'active')
ON DUPLICATE KEY UPDATE name=VALUES(name);

-- ----------------------------------------------------------------------------
-- 4. SEED THIRD-LEVEL SPECIFIC SUBCATEGORIES (Granular navigation)
-- ----------------------------------------------------------------------------
INSERT INTO categories (id, parent_id, pet_type_id, name, slug, description, image_url, icon_url, sort_order, status) VALUES
-- Under Cat Food (10)
(101, 10, 1, 'Dry Cat Food', 'dry-cat-food', 'Complete extruded kibble with taurine and omega fatty acids', '/assets/categories/dry-cat-food.webp', NULL, 1, 'active'),
(102, 10, 1, 'Wet Cat Food & Gravy', 'wet-cat-food', 'Hydrating pouches, pates, and cans in gravy and jelly', '/assets/categories/wet-cat-food.webp', NULL, 2, 'active'),
(103, 10, 1, 'Cat Treats & Puree Sticks', 'cat-treats', 'Creamy licking puree sachets, crunchy bites and freeze-dried meat', '/assets/categories/cat-treats.webp', NULL, 3, 'active'),

-- Under Cat Litter (11)
(111, 11, 1, 'Bentonite Clumping Litter', 'bentonite-cat-litter', 'Fast clumping volcanic clay litter with activated carbon', '/assets/categories/bentonite-litter.webp', NULL, 1, 'active'),
(112, 11, 1, 'Tofu & Eco Litter', 'tofu-cat-litter', 'Flushable, 100% biodegradable natural pea fiber and plant litters', '/assets/categories/tofu-litter.webp', NULL, 2, 'active'),
(113, 11, 1, 'Litter Trays & Scoops', 'litter-trays-scoops', 'Hooded litter boxes, open pans, sifting scoops and mats', '/assets/categories/litter-boxes.webp', NULL, 3, 'active'),

-- Under Dog Food (20)
(201, 20, 2, 'Dry Dog Food', 'dry-dog-food', 'Breed-specific and all-life-stage dry kibble for dogs', '/assets/categories/dry-dog-food.webp', NULL, 1, 'active'),
(202, 20, 2, 'Wet Dog Food', 'wet-dog-food', 'Rich loafs, chunks in gravy, and savory stews for dogs', '/assets/categories/wet-dog-food.webp', NULL, 2, 'active')
ON DUPLICATE KEY UPDATE name=VALUES(name);

SELECT 'Pet types and hierarchical categories seeded successfully!' AS status;
