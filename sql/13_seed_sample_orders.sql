-- ============================================================================
-- 13_seed_sample_orders.sql
-- Seed Data for Orders, Order Items, Reviews, Q&A, Blog, Carts & Audit Trails
-- Compatible with MySQL 8.0+ / MariaDB 10.5+
-- ============================================================================

USE pet_ecommerce_db;

-- ----------------------------------------------------------------------------
-- 1. SEED ORDERS
-- ----------------------------------------------------------------------------
INSERT INTO orders (
    id, user_id, order_number, address_id,
    shipping_full_name, shipping_phone, shipping_division, shipping_district, shipping_area, 
    shipping_address_line, shipping_postal_code,
    subtotal, discount_amount, delivery_charge, total_amount, coupon_code,
    payment_method, payment_status, order_status, payment_transaction_id, customer_note, admin_note,
    created_at
) VALUES
(1, 4, 'ORD-202610-0001', 1,
 'Zayan Chowdhury', '+8801812345671', 'Dhaka', 'Dhaka', 'Dhanmondi',
 'House 42, Road 7/A, Flat B4', '1209',
 3240.00, 300.00, 80.00, 3020.00, 'WELCOME10',
 'bKash', 'paid', 'delivered', 'TRX-BK-9823412', 'Please ring the bell on arrival.', 'Delivered safely via RedX.',
 '2026-10-01 10:30:00'),

(2, 5, 'ORD-202610-0002', 3,
 'Nusrat Jahan', '+8801812345672', 'Dhaka', 'Dhaka', 'Gulshan 2',
 'Plot 15, Road 103, Apartment 5A', '1212',
 3400.00, 100.00, 80.00, 3380.00, 'MEOW100',
 'cash_on_delivery', 'paid', 'delivered', NULL, 'Call before delivery.', 'Cash collected in full by courier.',
 '2026-10-03 14:15:00'),

(3, 6, 'ORD-202610-0003', 5,
 'Kazi Mahfuzur Rahman', '+8801812345673', 'Dhaka', 'Dhaka', 'Uttara',
 'Sector 11, Road 18, House 09', '1230',
 4050.00, 0.00, 80.00, 4130.00, NULL,
 'Nagad', 'paid', 'shipped', 'TRX-NG-5521908', 'Leave with building security guard if not home.', 'Dispatched via Pathao Courier.',
 '2026-10-10 11:45:00'),

(4, 7, 'ORD-202610-0004', 6,
 'Anika Tabassum', '+8801812345674', 'Dhaka', 'Dhaka', 'Mirpur DOHS',
 'Avenue 4, Road 9, House 412', '1216',
 1590.00, 80.00, 80.00, 1590.00, 'FREESHIP80',
 'online_payment', 'paid', 'processing', 'TRX-SSL-1002931', 'Urgent bird food requirement.', 'Packaging in warehouse.',
 '2026-10-11 16:20:00'),

(5, 8, 'ORD-202610-0005', 7,
 'Shakil Hasan', '+8801812345675', 'Chattogram', 'Chattogram', 'Panchlaish',
 'Holding 14/B, Equity Point, Nasirabad Housing Society', '4000',
 3550.00, 0.00, 150.00, 3700.00, NULL,
 'cash_on_delivery', 'pending', 'confirmed', NULL, 'Deliver between 10am to 5pm.', 'Order verified via phone call.',
 '2026-10-12 09:10:00'),

(6, 9, 'ORD-202610-0006', 8,
 'Sadia Afrin', '+8801812345676', 'Sylhet', 'Sylhet', 'Zindabazar',
 'Green Villa, Shahi Eidgah Road, Kumarpara', '3100',
 2200.00, 0.00, 150.00, 2350.00, NULL,
 'cash_on_delivery', 'failed', 'cancelled', NULL, 'Ordered by mistake.', 'Cancelled per customer phone request.',
 '2026-10-08 15:00:00'),

(7, 10, 'ORD-202610-0007', 9,
 'Arif Billah', '+8801812345677', 'Rajshahi', 'Rajshahi', 'Kazihata',
 'House 88, Near Medical College Gate', '6000',
 980.00, 0.00, 150.00, 1130.00, NULL,
 'cash_on_delivery', 'refunded', 'returned', NULL, 'Returning item.', 'Customer received package but opted for larger leash.',
 '2026-10-04 12:00:00')
ON DUPLICATE KEY UPDATE order_number=VALUES(order_number);

-- ----------------------------------------------------------------------------
-- 2. SEED ORDER ITEMS (Snapshots of purchased goods)
-- ----------------------------------------------------------------------------
INSERT INTO order_items (
    id, order_id, product_id, variant_id, product_name, variant_name, sku, unit_price, quantity, total_price
) VALUES
-- Items for Order 1
(1, 1, 4, NULL, 'Reflex Plus Adult Cat Food with Salmon 1.5kg', NULL, 'REF-CAT-SAL-15KG', 1650.00, 1, 1650.00),
(2, 1, 10, NULL, 'Me-O Creamy Cat Treats Salmon Flavor (4 x 15g)', NULL, 'MEO-TRT-SAL-60G', 220.00, 2, 440.00),
(3, 1, 13, NULL, 'Reflex Lavender Scented Bentonite Cat Litter 10L', NULL, 'REF-LTR-LAV-10L', 1150.00, 1, 1150.00),

-- Items for Order 2
(4, 2, 2, 2, 'Royal Canin Kitten Dry Food 2kg', '2kg Bag', 'RC-CAT-KIT-2KG-VAR', 2850.00, 1, 2850.00),
(5, 2, 8, NULL, 'Whiskas Tuna in Jelly Wet Cat Food Pouch 85g', NULL, 'WHK-WET-TUNA-85G', 110.00, 5, 550.00),

-- Items for Order 3
(6, 3, 19, 20, 'Drools Focus Adult Super Premium Dog Food 4kg', '4kg Bag', 'DRL-DOG-FOC-4KG-VAR', 2950.00, 1, 2950.00),
(7, 3, 22, NULL, 'JerHigh Milky Dog Chew Sticks 100g', NULL, 'JH-DOG-MILK-100G', 260.00, 2, 520.00),
(8, 3, 25, NULL, 'Bioline Neem Oil Anti-Flea & Tick Dog Shampoo 250ml', NULL, 'BIO-DOG-SHMP-250ML', 580.00, 1, 580.00),

-- Items for Order 4
(9, 4, 29, 26, 'Versele-Laga Prestige Budgies Daily Seed Mix 1kg', '1kg Pack', 'VL-BRD-BUDG-1KG-VAR', 650.00, 2, 1300.00),
(10, 4, 34, NULL, 'Natural Marine Cuttlefish Bone with Metal Cage Holder (Twin Pack)', NULL, 'MED-BRD-CUTTLE-2P', 290.00, 1, 290.00),

-- Items for Order 5
(11, 5, 35, 29, 'Versele-Laga Cuni Complete Adult Rabbit Pellets 1.75kg', '1.75kg Standard Bag', 'VL-RAB-CUNI-175KG-VAR', 1850.00, 1, 1850.00),
(12, 5, 36, NULL, 'Sun-Cured Green Timothy Hay for Rabbits 1kg', NULL, 'HAY-RAB-TIM-1KG', 850.00, 2, 1700.00),

-- Items for Order 6
(13, 6, 14, NULL, 'Enclosed Hooded Cat Litter Box with Carbon Filter', NULL, 'ACC-LTR-BOX-ENCL', 2200.00, 1, 2200.00),

-- Items for Order 7
(14, 7, 28, NULL, 'Heavy-Duty 5-Meter Retractable Dog Walking Leash', NULL, 'ACC-DOG-LSH-5M', 980.00, 1, 980.00)
ON DUPLICATE KEY UPDATE product_name=VALUES(product_name);

-- ----------------------------------------------------------------------------
-- 3. SEED COUPON USAGES
-- ----------------------------------------------------------------------------
INSERT INTO coupon_usages (id, coupon_id, user_id, order_id, discount_applied, used_at) VALUES
(1, 1, 4, 1, 300.00, '2026-10-01 10:30:00'),
(2, 2, 5, 2, 100.00, '2026-10-03 14:15:00'),
(3, 4, 7, 4, 80.00, '2026-10-11 16:20:00')
ON DUPLICATE KEY UPDATE discount_applied=VALUES(discount_applied);

-- ----------------------------------------------------------------------------
-- 4. SEED INVENTORY TRANSACTIONS (Audit Trail)
-- ----------------------------------------------------------------------------
INSERT INTO inventory_transactions (
    id, product_id, variant_id, transaction_type, quantity, previous_stock, new_stock, reference_order_id, note
) VALUES
-- Initial shipment received into warehouse
(1, 4, NULL, 'purchase', 81, 0, 81, NULL, 'Initial wholesale shipment from Reflex Distributor'),
(2, 10, NULL, 'purchase', 142, 0, 142, NULL, 'Wholesale batch received from Perfect Companion'),
(3, 13, NULL, 'purchase', 86, 0, 86, NULL, 'Container shipment received from port'),

-- Stock reductions for Order 1
(4, 4, NULL, 'sale', -1, 81, 80, 1, 'Order #ORD-202610-0001 fulfillment'),
(5, 10, NULL, 'sale', -2, 142, 140, 1, 'Order #ORD-202610-0001 fulfillment'),
(6, 13, NULL, 'sale', -1, 86, 85, 1, 'Order #ORD-202610-0001 fulfillment'),

-- Stock reduction and return restoration for Order 7
(7, 28, NULL, 'sale', -1, 50, 49, 7, 'Order #ORD-202610-0007 fulfillment'),
(8, 28, NULL, 'return', 1, 49, 50, 7, 'Customer return processed in warehouse')
ON DUPLICATE KEY UPDATE note=VALUES(note);

-- ----------------------------------------------------------------------------
-- 5. SEED REVIEWS AND RATINGS
-- ----------------------------------------------------------------------------
INSERT INTO reviews (
    id, user_id, product_id, order_id, rating, title, comment, status, created_at
) VALUES
(1, 4, 4, 1, 5, 'Best cat food for shiny coat!', 
 'My Persian cat Milo was shedding excessively. Within 3 weeks of Reflex Plus Salmon, his coat is super glossy and soft. Highly recommended!', 
 'approved', '2026-10-05 18:20:00'),

(2, 4, 13, 1, 5, 'Zero odor and great clumping!', 
 'Reflex lavender litter has solid clumping and absorbs ammonia smell immediately. Easy to scoop.', 
 'approved', '2026-10-06 09:15:00'),

(3, 5, 2, 2, 5, 'Kitten loves it, easy on tummy', 
 'My British Shorthair kitten transitioned smoothly from mother milk to this Royal Canin kibble without diarrhea.', 
 'approved', '2026-10-07 11:30:00'),

(4, 5, 8, 2, 4, 'Great hydration pouch', 
 'Juicy wet tuna chunks that my kitten licks clean every morning. Price is also very reasonable.', 
 'approved', '2026-10-08 14:00:00'),

(5, 6, 19, 3, 5, 'High protein, dog is energetic and muscular', 
 'Drools Focus is a great alternative to expensive imported brands. My GSD Rocky has solid stools and lean muscle.', 
 'approved', '2026-10-11 21:00:00'),

(6, 7, 29, 4, 5, 'Clean Belgian seeds, zero dust', 
 'Very fresh and diverse seed mix. My budgie Rio eats almost all seeds without wastage.', 
 'approved', '2026-10-12 11:00:00')
ON DUPLICATE KEY UPDATE rating=VALUES(rating);

-- ----------------------------------------------------------------------------
-- 6. SEED PRODUCT QUESTIONS (Q&A)
-- ----------------------------------------------------------------------------
INSERT INTO product_questions (
    id, product_id, user_id, question, answer, answered_by, answered_at, status
) VALUES
(1, 1, 5, 
 'Can I feed this to a 2-month-old kitten that was just separated from its mother?',
 'Yes, absolutely! Royal Canin Mother & Babycat is specially formulated for kittens from 1 to 4 months old. You can also rehydrate the kibble with warm water to make a soft porridge.',
 2, '2026-10-02 10:45:00', 'answered'),

(2, 12, 4, 
 'Can this Kit Cat SoyaClump tofu litter really be flushed down standard toilets in Dhaka?',
 'Yes, it is 100% natural biodegradable pea and soybean fiber. It dissolves quickly in water. Flush one or two clumps at a time to prevent pipe congestion.',
 3, '2026-10-04 15:30:00', 'answered'),

(3, 27, 6, 
 'What size harness should I choose for an adult German Shepherd weighing 32kg?',
 'For a 32kg German Shepherd, we recommend Size Large (L) which fits chest girths from 70cm to 90cm. You can also measure the widest part of your dog chest with a measuring tape.',
 2, '2026-10-09 11:20:00', 'answered'),

(4, 36, 8, 
 'Is this Timothy Hay green and fragrant, or brown and dusty?',
 'This is fresh sun-cured early bloom first-cut Timothy Hay. It is vibrant green, fragrant, long-stemmed, and dust-sifted for small herbivores.',
 3, '2026-10-11 17:00:00', 'answered')
ON DUPLICATE KEY UPDATE question=VALUES(question);

-- ----------------------------------------------------------------------------
-- 7. SEED NOTIFICATIONS
-- ----------------------------------------------------------------------------
INSERT INTO notifications (
    id, user_id, title, message, type, reference_id, is_read, read_at, created_at
) VALUES
(1, 4, 'Order Delivered Successfully!', 'Your order #ORD-202610-0001 has been safely delivered to Dhanmondi.', 'order_update', 1, TRUE, '2026-10-04 11:00:00', '2026-10-04 10:45:00'),
(2, 5, 'Order Shipped!', 'Your order #ORD-202610-0002 is on the way with our rider.', 'order_update', 2, TRUE, '2026-10-06 08:30:00', '2026-10-05 16:20:00'),
(3, 6, 'Order Dispatched', 'Your order #ORD-202610-0003 has been handed over to courier.', 'order_update', 3, FALSE, NULL, '2026-10-11 09:00:00'),
(4, 4, 'Exclusive Coupon for Milo!', 'Use code PETLOVER15 for 15% off on your next cat food order.', 'promotion', 3, FALSE, NULL, '2026-10-12 12:00:00')
ON DUPLICATE KEY UPDATE title=VALUES(title);

-- ----------------------------------------------------------------------------
-- 8. SEED CONTACT MESSAGES
-- ----------------------------------------------------------------------------
INSERT INTO contact_messages (
    id, name, email, phone, subject, message, status, admin_reply, replied_by, replied_at
) VALUES
(1, 'Tanvir Mahmud', 'tanvir.m@example.com', '+8801755123456', 
 'Urgent Delivery in Dhanmondi', 
 'Do you provide express 3-hour emergency delivery in Dhanmondi for sick cat prescription food?', 
 'replied', 
 'Hello Tanvir, yes! For emergency prescription orders inside Dhanmondi and Gulshan, we offer express delivery via Pathao Parcel. Please contact our hotline directly at 01711-000002.', 
 2, '2026-10-08 14:30:00'),

(2, 'Farzana Akter', 'farzana.a@example.com', '+8801755987654', 
 'Versele-Laga Cuni Complete Restock Inquiry', 
 'When will the 8kg breeder sack of Versele-Laga Cuni Complete be back in stock?', 
 'read', NULL, NULL, NULL)
ON DUPLICATE KEY UPDATE subject=VALUES(subject);

-- ----------------------------------------------------------------------------
-- 9. SEED BLOG CATEGORIES & ARTICLES
-- ----------------------------------------------------------------------------
INSERT INTO blog_categories (id, name, slug, description) VALUES
(1, 'Cat Care & Nutrition', 'cat-care-nutrition', 'Dietary advice, litter box training, and preventive feline health guides.'),
(2, 'Dog Health & Training', 'dog-health-training', 'Puppy socialization, tick prevention, and canine behavior management.'),
(3, 'Avian Care & Breeding', 'avian-care-breeding', 'Bird nutrition, cage sanitation, and seasonal molt maintenance.'),
(4, 'Small Pet Wellness', 'small-pet-wellness', 'Hay requirements, dental trimming, and bunny housing essentials.')
ON DUPLICATE KEY UPDATE name=VALUES(name);

INSERT INTO blog_posts (
    id, category_id, author_id, pet_type_id, title, slug, content, excerpt, featured_image, status, published_at
) VALUES
(1, 1, 1, 1, 
 'How to Choose Between Wet and Dry Food for Your Cat in Bangladesh', 
 'how-to-choose-between-wet-and-dry-cat-food',
 'Maintaining hydration is one of the biggest challenges for cat owners in Bangladesh during humid summers. While dry kibble provides dental cleaning and caloric balance, wet food gravy pouches supply critical moisture that protects felines from dangerous urinary tract obstructions and kidney stones...',
 'A comprehensive guide on balancing kibble crunch with wet food hydration for healthy Bengali felines.',
 '/assets/blog/cat-nutrition-guide.webp', 'published', '2026-09-15 10:00:00'),

(2, 2, 1, 2, 
 'Tick and Flea Prevention Guide for Dogs During the Monsoon Season', 
 'tick-and-flea-prevention-monsoon-dogs',
 'Monsoon in Bangladesh brings heavy moisture and skyrocketing tick populations. In this veterinary-reviewed article, we review topical fipronil spot-on treatments, medicated neem shampoos, and preventive home sanitizing steps to guard your dog against tick fever...',
 'Protect your canine companion from dangerous blood parasites and monsoon skin dermatitis.',
 '/assets/blog/dog-flea-prevention.webp', 'published', '2026-09-28 14:00:00'),

(3, 4, 1, 4, 
 'Why 80% of a Rabbit Diet Must Be Fragrant Timothy Hay', 
 'why-rabbits-need-80-percent-timothy-hay',
 'Unlike dogs or cats, rabbits possess continuously growing open-rooted teeth and specialized hindgut fermentation. Feeding too many pellets or carrots leads to fatal GI stasis and painful dental spurs. Discover why unlimited clean hay is your rabbit life-saver...',
 'Learn why hay is essential for dental wear and continuous gut motility in pet rabbits.',
 '/assets/blog/rabbit-hay-importance.webp', 'published', '2026-10-05 16:30:00')
ON DUPLICATE KEY UPDATE title=VALUES(title);

-- ----------------------------------------------------------------------------
-- 10. SEED SAMPLE ACTIVE CARTS & WISHLISTS
-- ----------------------------------------------------------------------------
-- User 4 active shopping cart
INSERT INTO carts (id, user_id, session_token) VALUES
(1, 4, NULL),
(2, 5, NULL)
ON DUPLICATE KEY UPDATE user_id=VALUES(user_id);

INSERT INTO cart_items (id, cart_id, product_id, variant_id, quantity) VALUES
(1, 1, 1, NULL, 1),
(2, 1, 11, NULL, 3),
(3, 2, 15, NULL, 1)
ON DUPLICATE KEY UPDATE quantity=VALUES(quantity);

-- User 4 and 5 active wishlists
INSERT INTO wishlists (id, user_id) VALUES
(1, 4),
(2, 5)
ON DUPLICATE KEY UPDATE user_id=VALUES(user_id);

INSERT INTO wishlist_items (id, wishlist_id, product_id) VALUES
(1, 1, 15),
(2, 1, 3),
(3, 2, 27)
ON DUPLICATE KEY UPDATE product_id=VALUES(product_id);

SELECT 'Sample orders, order items, audit transactions, reviews, questions, blogs, carts and wishlists seeded successfully!' AS status;
