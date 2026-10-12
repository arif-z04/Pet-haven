-- ============================================================================
-- 09_seed_addresses.sql
-- Seed Data for Customer Addresses (Bangladesh Geographies)
-- Compatible with MySQL 8.0+ / MariaDB 10.5+
-- ============================================================================

USE pet_ecommerce_db;

INSERT INTO customer_addresses (
    id, user_id, full_name, phone, division, district, area, address_line, postal_code, is_default, address_type
) VALUES
-- Addresses for Zayan Chowdhury (user_id = 4)
(1, 4, 'Zayan Chowdhury', '+8801812345671', 'Dhaka', 'Dhaka', 'Dhanmondi', 
 'House 42, Road 7/A, Flat B4', '1209', TRUE, 'home'),
(2, 4, 'Zayan Chowdhury (Office)', '+8801812345671', 'Dhaka', 'Dhaka', 'Kawran Bazar', 
 'BDBL Bhaban, Level 8, 12 Kazi Nazrul Islam Ave', '1215', FALSE, 'office'),

-- Addresses for Nusrat Jahan (user_id = 5)
(3, 5, 'Nusrat Jahan', '+8801812345672', 'Dhaka', 'Dhaka', 'Gulshan 2', 
 'Plot 15, Road 103, Apartment 5A', '1212', TRUE, 'home'),
(4, 5, 'Nusrat Jahan', '+8801812345672', 'Dhaka', 'Dhaka', 'Banani', 
 'House 28, Road 11, Block D', '1213', FALSE, 'office'),

-- Addresses for Kazi Mahfuzur Rahman (user_id = 6)
(5, 6, 'Kazi Mahfuzur Rahman', '+8801812345673', 'Dhaka', 'Dhaka', 'Uttara', 
 'Sector 11, Road 18, House 09', '1230', TRUE, 'home'),

-- Addresses for Anika Tabassum (user_id = 7)
(6, 7, 'Anika Tabassum', '+8801812345674', 'Dhaka', 'Dhaka', 'Mirpur DOHS', 
 'Avenue 4, Road 9, House 412', '1216', TRUE, 'home'),

-- Addresses for Shakil Hasan (user_id = 8)
(7, 8, 'Shakil Hasan', '+8801812345675', 'Chattogram', 'Chattogram', 'Panchlaish', 
 'Holding 14/B, Equity Point, Nasirabad Housing Society', '4000', TRUE, 'home'),

-- Addresses for Sadia Afrin (user_id = 9)
(8, 9, 'Sadia Afrin', '+8801812345676', 'Sylhet', 'Sylhet', 'Zindabazar', 
 'Green Villa, Shahi Eidgah Road, Kumarpara', '3100', TRUE, 'home'),

-- Addresses for Arif Billah (user_id = 10)
(9, 10, 'Arif Billah', '+8801812345677', 'Rajshahi', 'Rajshahi', 'Kazihata', 
 'House 88, Near Medical College Gate', '6000', TRUE, 'home')
ON DUPLICATE KEY UPDATE full_name=VALUES(full_name);

SELECT 'Customer delivery addresses seeded successfully!' AS status;
