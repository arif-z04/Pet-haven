-- ============================================================================
-- 01_create_database.sql
-- Database Initialization for Full-Stack Pet E-Commerce Platform
-- Compatible with MySQL 8.0+ / MariaDB 10.5+
-- ============================================================================

-- Drop database if existing (uncomment if a clean rebuild is desired)
-- DROP DATABASE IF EXISTS pet_ecommerce_db;

-- Create the database with full Unicode support (supports Bengali, emojis, international text)
CREATE DATABASE IF NOT EXISTS pet_ecommerce_db
  CHARACTER SET utf8mb4
  COLLATE utf8mb4_unicode_ci;

-- Switch to the newly created database context
USE pet_ecommerce_db;

-- Display verification
SELECT 'Database pet_ecommerce_db initialized successfully with utf8mb4 character set' AS status;
