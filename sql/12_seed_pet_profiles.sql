-- ============================================================================
-- 12_seed_pet_profiles.sql
-- Seed Data for "My Pet" Profiles (Recommendation Support)
-- Compatible with MySQL 8.0+ / MariaDB 10.5+
-- ============================================================================

USE pet_ecommerce_db;

INSERT INTO pet_profiles (
    id, user_id, pet_type_id, name, breed, gender, date_of_birth, weight, 
    profile_image_url, dietary_preferences, allergies_or_medical_notes, notes
) VALUES
-- User 4: Zayan Chowdhury's Persian Cat
(1, 4, 1, 'Milo', 'Persian Longhair', 'male', '2024-05-15', 4.20,
 '/assets/pets/milo-persian.webp', 'Reflex Plus Salmon and Me-O Creamy treats', 
 'Prone to frequent hairballs; requires daily psyllium or malt paste.', 
 'Indoor companion, very fluffy, dislikes dry chicken kibble.'),

-- User 5: Nusrat Jahan's Kitten
(2, 5, 1, 'Luna', 'British Shorthair', 'female', '2025-08-10', 2.10,
 '/assets/pets/luna-bsh.webp', 'Royal Canin Kitten and Whiskas Tuna pouches', 
 'No known allergies. Currently completing kitten vaccination cycles.', 
 'High-energy kitten, loves climbing scratch posts and chasing feather wands.'),

-- User 6: Kazi Mahfuzur Rahman's German Shepherd
(3, 6, 2, 'Rocky', 'German Shepherd', 'male', '2023-02-20', 32.50,
 '/assets/pets/rocky-gsd.webp', 'Drools Focus Adult and JerHigh Milky bones', 
 'Mild hip sensitivity during cold weather; benefits from glucosamine.', 
 'Guard dog, trained for obedience, exercises twice daily.'),

-- User 7: Anika Tabassum's Budgie
(4, 7, 3, 'Rio', 'Budgerigar', 'male', '2024-11-01', 0.04,
 '/assets/pets/rio-budgie.webp', 'Versele-Laga Prestige Budgies and yellow millet', 
 'Requires cuttlefish bone for beak grooming and calcium supplementation.', 
 'Extremely vocal morning singer, friendly and steps up on fingers.'),

-- User 8: Shakil Hasan's Rabbit
(5, 8, 4, 'Snowball', 'Netherland Dwarf', 'female', '2024-06-12', 1.10,
 '/assets/pets/snowball-rabbit.webp', 'Sun-cured Timothy Hay and Cuni Complete pellets', 
 'Sensitive gastrointestinal cecum; avoid high-sugar treats.', 
 'Litter box trained, free roams in playpen, adores head pets.')
ON DUPLICATE KEY UPDATE name=VALUES(name);

SELECT 'Sample "My Pet" user profiles seeded successfully!' AS status;
