-- ============================================================================
-- 08_seed_users.sql
-- Seed Data for Platform Accounts: Admins, Operations Staff, and Customers
-- Compatible with MySQL 8.0+ / MariaDB 10.5+
-- ============================================================================

USE pet_ecommerce_db;

-- Standard bcrypt password hash representing 'Password@123'
-- Cost factor 10 hash: $2y$10$92IXUNpkjO0rOQ5byMi.Ye4oKoEa3Ro9llC/.og/at2.uheWG/igi

INSERT INTO users (
    id, name, email, phone, password_hash, role, status, last_login_at
) VALUES
-- Platform Administrators
(1, 'Tanvir Ahmed', 'admin@pethavenbd.com', '+8801711000001', 
 '$2y$10$92IXUNpkjO0rOQ5byMi.Ye4oKoEa3Ro9llC/.og/at2.uheWG/igi', 'admin', 'active', '2026-10-10 09:30:00'),

-- Customer Support & Inventory Staff
(2, 'Farhana Yasmin', 'farhana.support@pethavenbd.com', '+8801711000002', 
 '$2y$10$92IXUNpkjO0rOQ5byMi.Ye4oKoEa3Ro9llC/.og/at2.uheWG/igi', 'staff', 'active', '2026-10-11 11:15:00'),
(3, 'Rafiqul Islam', 'rafiq.ops@pethavenbd.com', '+8801711000003', 
 '$2y$10$92IXUNpkjO0rOQ5byMi.Ye4oKoEa3Ro9llC/.og/at2.uheWG/igi', 'staff', 'active', '2026-10-12 08:45:00'),

-- Customers (Dhaka & Regional)
(4, 'Zayan Chowdhury', 'zayan.chowdhury@example.com', '+8801812345671', 
 '$2y$10$92IXUNpkjO0rOQ5byMi.Ye4oKoEa3Ro9llC/.og/at2.uheWG/igi', 'customer', 'active', '2026-10-11 18:20:00'),
(5, 'Nusrat Jahan', 'nusrat.jahan@example.com', '+8801812345672', 
 '$2y$10$92IXUNpkjO0rOQ5byMi.Ye4oKoEa3Ro9llC/.og/at2.uheWG/igi', 'customer', 'active', '2026-10-12 14:10:00'),
(6, 'Kazi Mahfuzur Rahman', 'kazi.mahfuz@example.com', '+8801812345673', 
 '$2y$10$92IXUNpkjO0rOQ5byMi.Ye4oKoEa3Ro9llC/.og/at2.uheWG/igi', 'customer', 'active', '2026-10-09 20:40:00'),
(7, 'Anika Tabassum', 'anika.tabassum@example.com', '+8801812345674', 
 '$2y$10$92IXUNpkjO0rOQ5byMi.Ye4oKoEa3Ro9llC/.og/at2.uheWG/igi', 'customer', 'active', '2026-10-10 16:55:00'),
(8, 'Shakil Hasan', 'shakil.hasan@example.com', '+8801812345675', 
 '$2y$10$92IXUNpkjO0rOQ5byMi.Ye4oKoEa3Ro9llC/.og/at2.uheWG/igi', 'customer', 'active', '2026-10-08 12:00:00'),
(9, 'Sadia Afrin', 'sadia.afrin@example.com', '+8801812345676', 
 '$2y$10$92IXUNpkjO0rOQ5byMi.Ye4oKoEa3Ro9llC/.og/at2.uheWG/igi', 'customer', 'active', '2026-10-11 22:30:00'),
(10, 'Arif Billah', 'arif.billah@example.com', '+8801812345677', 
 '$2y$10$92IXUNpkjO0rOQ5byMi.Ye4oKoEa3Ro9llC/.og/at2.uheWG/igi', 'customer', 'active', '2026-10-05 10:15:00')
ON DUPLICATE KEY UPDATE name=VALUES(name);

SELECT 'Sample administrators, staff, and customers seeded successfully!' AS status;
