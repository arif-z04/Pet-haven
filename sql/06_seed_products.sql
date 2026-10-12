-- ============================================================================
-- 06_seed_products.sql
-- Seed Data for 40+ Master Products & Gallery Images
-- Covers: Cats, Dogs, Birds, Rabbits, and Small Pets
-- Compatible with MySQL 8.0+ / MariaDB 10.5+
-- ============================================================================

USE pet_ecommerce_db;

-- ----------------------------------------------------------------------------
-- 1. SEED PRODUCTS
-- ----------------------------------------------------------------------------
INSERT INTO products (
    id, category_id, brand_id, pet_type_id, name, slug, 
    short_description, description, 
    base_price, compare_at_price, cost_price, 
    sku, barcode, stock_quantity, low_stock_threshold, weight, 
    status, is_featured, target_life_stage, target_breed_size, food_type, health_concern
) VALUES
-- ==================== CAT PRODUCTS (ID 1-16) ====================
(1, 101, 1, 1, 'Royal Canin Mother & Babycat Dry Food 2kg', 'royal-canin-mother-babycat-2kg',
 'Nutritional solution for pregnant cats and 1st stage kittens (1 to 4 months old).',
 'Features an ultra-soft kibble specifically designed to facilitate transition to solid food. Enriched with DHA to support cerebral development and immune defense complexes.',
 2950.00, 3200.00, 2400.00, 'RC-CAT-BABY-2KG', '8901234500011', 45, 10, 2.00,
 'active', TRUE, 'kitten_puppy', 'all', 'dry', 'Digestion'),

(2, 101, 1, 1, 'Royal Canin Kitten Dry Food 2kg', 'royal-canin-kitten-2kg',
 'Balanced dry food for second age kittens up to 12 months old.',
 'Optimal protein, vitamin, and mineral balance including vitamin D and calcium to foster healthy bone and muscle development.',
 2850.00, 3100.00, 2300.00, 'RC-CAT-KIT-2KG', '8901234500028', 60, 10, 2.00,
 'active', TRUE, 'kitten_puppy', 'all', 'dry', 'Digestion'),

(3, 101, 1, 1, 'Royal Canin Hairball Care Adult Dry Cat Food 2kg', 'royal-canin-hairball-care-2kg',
 'Specifically formulated to naturally stimulate the elimination of hairballs.',
 'Rich in psyllium mucilage and micronized dietary fiber to double the natural transit of swallowed hairs in the feces.',
 3100.00, 3400.00, 2550.00, 'RC-CAT-HB-2KG', '8901234500035', 30, 8, 2.00,
 'active', FALSE, 'adult', 'all', 'dry', 'Hairball'),

(4, 101, 2, 1, 'Reflex Plus Adult Cat Food with Salmon 1.5kg', 'reflex-plus-salmon-adult-1-5kg',
 'Super premium food containing Xylo-oligosaccharides (XOS) for adult felines.',
 'Rich in natural omega-3 and omega-6 fatty acids to nourish silky skin and radiant fur while strengthening urinary tract immunity.',
 1650.00, 1850.00, 1300.00, 'REF-CAT-SAL-15KG', '8901234500042', 80, 15, 1.50,
 'active', TRUE, 'adult', 'all', 'dry', 'Skin & Coat'),

(5, 101, 2, 1, 'Reflex Plus Kitten Food with Chicken 1.5kg', 'reflex-plus-kitten-chicken-1-5kg',
 'Super premium high-protein chicken recipe designed for growing kittens.',
 'Provides balanced vitamins and minerals to foster rapid muscular growth, vision development, and healthy immune resistance.',
 1700.00, 1900.00, 1350.00, 'REF-CAT-KIT-15KG', '8901234500059', 75, 15, 1.50,
 'active', TRUE, 'kitten_puppy', 'all', 'dry', 'Digestion'),

(6, 101, 3, 1, 'Whiskas Adult Ocean Fish Dry Cat Food 1.2kg', 'whiskas-ocean-fish-1-2kg',
 'Savory crunchy pockets bursting with real ocean fish flavor and essential nutrients.',
 'Provides complete and balanced nutrition with taurine for heart and eyesight wellness, plus zinc and omega-6.',
 980.00, 1100.00, 780.00, 'WHK-CAT-OF-12KG', '8901234500066', 110, 20, 1.20,
 'active', FALSE, 'adult', 'all', 'dry', 'None'),

(7, 101, 5, 1, 'Me-O Adult Seafood Dry Cat Food 1.2kg', 'me-o-adult-seafood-1-2kg',
 'Nutritionally complete seafood formula enriched with taurine and low sodium.',
 'Formulated to reduce feline lower urinary tract disease (FLUTD) risks while maintaining dental cleanliness through crisp kibble.',
 820.00, 920.00, 650.00, 'MEO-CAT-SF-12KG', '8901234500073', 95, 20, 1.20,
 'active', FALSE, 'adult', 'all', 'dry', 'Urinary'),

(8, 102, 3, 1, 'Whiskas Tuna in Jelly Wet Cat Food Pouch 85g', 'whiskas-tuna-jelly-pouch-85g',
 'Juicy tender tuna chunks swimming in savory appetizing jelly for active felines.',
 'High moisture content promotes proper hydration and kidney health in everyday feeding routines.',
 110.00, 125.00, 85.00, 'WHK-WET-TUNA-85G', '8901234500080', 250, 40, 0.08,
 'active', TRUE, 'adult', 'all', 'wet', 'None'),

(9, 102, 7, 1, 'Sheba Melty Chicken Breast Gravy Cat Pouch 70g', 'sheba-chicken-breast-pouch-70g',
 'Gourmet hand-shredded chicken breast cooked in delicate veloute gravy.',
 'Ultra-palatable delicacy packed with high animal proteins for fussy or recovering cats.',
 150.00, 175.00, 115.00, 'SHB-WET-CHK-70G', '8901234500097', 180, 30, 0.07,
 'active', FALSE, 'adult', 'all', 'wet', 'None'),

(10, 103, 5, 1, 'Me-O Creamy Cat Treats Salmon Flavor (4 x 15g)', 'me-o-creamy-treats-salmon-60g',
 'Lickable puree cat treat sticks enriched with green tea extract and taurine.',
 'Perfect for bonding, training rewards, and administering crushed tablets or supplements effortlessly.',
 220.00, 250.00, 165.00, 'MEO-TRT-SAL-60G', '8901234500103', 140, 25, 0.06,
 'active', TRUE, 'all', 'all', 'treat', 'None'),

(11, 103, 12, 1, 'Kit Cat Purr Puree Chicken & Smoked Fish (4 x 15g)', 'kit-cat-purr-puree-chicken-smoked-fish',
 'Grain-free smooth treat fortified with omega 3 & 6, prebiotics, and zero pork or lard.',
 'Irresistible creamy puree that soothes picky feline eaters and aids gastrointestinal flora.',
 240.00, 270.00, 180.00, 'KTC-TRT-CK-60G', '8901234500110', 120, 20, 0.06,
 'active', FALSE, 'all', 'all', 'treat', 'Digestion'),

(12, 111, 12, 1, 'Kit Cat SoyaClump Biodegradable Tofu Litter 7L', 'kit-cat-soyaclump-tofu-litter-7l',
 'Eco-friendly, flushable soybean residue cat litter with instantaneous clumping.',
 'Virtually 99.9% dust-free, gentle on kitten paws, and offers 3x superior odor absorption without toxic chemicals.',
 1250.00, 1400.00, 950.00, 'KTC-LTR-TOFU-7L', '8901234500127', 65, 12, 2.80,
 'active', TRUE, 'all', 'all', 'not_applicable', 'None'),

(13, 111, 2, 1, 'Reflex Lavender Scented Bentonite Cat Litter 10L', 'reflex-lavender-bentonite-litter-10l',
 'Compact clumping natural white bentonite litter infused with soothing lavender scent.',
 'Locks urine odors tightly within seconds and produces hard, easy-to-scoop clumps.',
 1150.00, 1300.00, 880.00, 'REF-LTR-LAV-10L', '8901234500134', 85, 15, 8.50,
 'active', FALSE, 'all', 'all', 'not_applicable', 'None'),

(14, 113, NULL, 1, 'Enclosed Hooded Cat Litter Box with Carbon Filter', 'enclosed-hooded-cat-litter-box',
 'Spacious privacy litter box with flap door, internal step-mat, and replaceable charcoal filter.',
 'Prevents litter scatter, contains urine sprays, and includes a heavy-duty matching sifting scoop.',
 2200.00, 2500.00, 1600.00, 'ACC-LTR-BOX-ENCL', '8901234500141', 25, 5, 1.80,
 'active', FALSE, 'all', 'all', 'not_applicable', 'None'),

(15, 12, NULL, 1, 'Multi-Level Sisal Cat Scratching Post with Hammock', 'multi-level-sisal-cat-scratching-post',
 'Sturdy 90cm feline activity tree featuring natural sisal scratch columns and plush perch.',
 'Saves furniture from clawing damage while offering high-vantage rest and playtime entertainment.',
 3450.00, 3900.00, 2500.00, 'TOY-CAT-TREE-90CM', '8901234500158', 18, 4, 4.50,
 'active', TRUE, 'all', 'all', 'not_applicable', 'None'),

(16, 14, 10, 1, 'Beaphar Malt Paste Anti-Hairball for Cats 100g', 'beaphar-malt-paste-100g',
 'Dual-action Dutch malt paste that encapsulates swallowed hair and assists easy passage.',
 'Includes BIO-MOS prebiotic oligosaccharides to strengthen gut microflora and ease digestion.',
 1350.00, 1550.00, 1020.00, 'BEA-CAT-MALT-100G', '8901234500165', 40, 10, 0.12,
 'active', FALSE, 'all', 'all', 'not_applicable', 'Hairball'),

-- ==================== DOG PRODUCTS (ID 17-28) ====================
(17, 201, 1, 2, 'Royal Canin Maxi Puppy Dry Dog Food 4kg', 'royal-canin-maxi-puppy-4kg',
 'Tailored nutrition for large breed puppies (adult weight 26-44kg) up to 15 months old.',
 'Moderate energy level to ensure harmonious skeletal development without excess weight gain during critical growth phases.',
 4450.00, 4800.00, 3600.00, 'RC-DOG-MXPUP-4KG', '8901234500172', 32, 6, 4.00,
 'active', TRUE, 'kitten_puppy', 'large', 'dry', 'Joint Care'),

(18, 201, 1, 2, 'Royal Canin Medium Adult Dry Dog Food 4kg', 'royal-canin-medium-adult-4kg',
 'Formula designed to support natural defenses and optimal vitality for adult dogs (11 to 25kg).',
 'Enriched with EPA-DHA omega fatty acids for luminous coat condition and high digestibility proteins.',
 4300.00, 4650.00, 3500.00, 'RC-DOG-MEDAD-4KG', '8901234500189', 28, 6, 4.00,
 'active', FALSE, 'adult', 'medium', 'dry', 'None'),

(19, 201, 4, 2, 'Drools Focus Adult Super Premium Dog Food 4kg', 'drools-focus-adult-dog-4kg',
 'No corn, no wheat, and no soy kibble crafted with 100% real chicken and wholesome brown rice.',
 'Formulated with glucosamine and chondroitin sulfate to maintain joint flexibility and mobility.',
 2950.00, 3300.00, 2350.00, 'DRL-DOG-FOC-4KG', '8901234500196', 50, 10, 4.00,
 'active', TRUE, 'adult', 'all', 'dry', 'Joint Care'),

(20, 201, 6, 2, 'SmartHeart Adult Dog Food Roast Beef Flavor 3kg', 'smartheart-adult-beef-3kg',
 'Nutritionally balanced dry dog kibble with savory roast beef aroma.',
 'Contains DHA & choline for brain function, calcium & phosphorus for dental strength, and omega-3.',
 1550.00, 1750.00, 1200.00, 'SMH-DOG-BEEF-3KG', '8901234500202', 70, 15, 3.00,
 'active', FALSE, 'adult', 'all', 'dry', 'None'),

(21, 202, 8, 2, 'Purina Pro Plan Adult Savor Chicken & Rice Canned Dog Food 368g', 'purina-pro-plan-savor-chicken-368g',
 'Real chicken chunks in rich, savory gravy designed for balanced everyday nourishment.',
 'Promotes lean muscle mass and optimum fluid intake with optimal 23 essential vitamins and minerals.',
 420.00, 480.00, 320.00, 'PUR-DOG-CAN-368G', '8901234500219', 90, 20, 0.40,
 'active', FALSE, 'adult', 'all', 'wet', 'None'),

(22, 21, 11, 2, 'JerHigh Milky Dog Chew Sticks 100g', 'jerhigh-milky-treats-100g',
 'Premium human-grade chicken meat dog treats with wholesome calcium and milk aroma.',
 'Ideal training reward for puppies and adults, promoting oral stimulation and dental hygiene.',
 260.00, 300.00, 195.00, 'JH-DOG-MILK-100G', '8901234500226', 150, 25, 0.10,
 'active', TRUE, 'all', 'all', 'treat', 'Dental'),

(23, 21, 4, 2, 'Drools Absolute Calcium Dog Dental Bone 400g', 'drools-calcium-bone-400g',
 'Enriched calcium dental bone with calibrated phosphorus ratio for jaw and skeletal resilience.',
 'Effectively scrapes away plaque and tartar buildup while entertaining dogs for hours.',
 480.00, 550.00, 370.00, 'DRL-DOG-CALC-400G', '8901234500233', 85, 15, 0.45,
 'active', FALSE, 'all', 'all', 'treat', 'Dental'),

(24, 22, NULL, 2, 'Heavy Duty Indestructible Natural Rubber Dog Ball Toy', 'heavy-duty-rubber-dog-ball',
 'Puncture-resistant high-bounce natural rubber fetch ball with treat-dispensing hollow core.',
 'Withstands vigorous chewers, massages gums safely, and floats in water for outdoor play.',
 550.00, 650.00, 360.00, 'TOY-DOG-BALL-MED', '8901234500240', 60, 12, 0.25,
 'active', FALSE, 'all', 'medium', 'not_applicable', 'None'),

(25, 23, 13, 2, 'Bioline Neem Oil Anti-Flea & Tick Dog Shampoo 250ml', 'bioline-anti-flea-tick-shampoo-250ml',
 'Herbal antiseptic shampoo formulated with pure natural neem extract and citronella oils.',
 'Soothes inflamed insect bites, eliminates stubborn fleas and ticks, and leaves a fresh herbal scent.',
 580.00, 680.00, 420.00, 'BIO-DOG-SHMP-250ML', '8901234500257', 55, 10, 0.30,
 'active', TRUE, 'all', 'all', 'not_applicable', 'Skin & Coat'),

(26, 24, 10, 2, 'Beaphar Fiprotec Spot-On Flea & Tick Treatment for Dogs (1 Pipette)', 'beaphar-fiprotec-dog-spot-on',
 'Veterinary topical fipronil spot-on solution protecting against fleas for up to 5 weeks.',
 'Kills fleas, deer ticks, and biting lice rapidly on contact before they transmit harmful blood parasites.',
 750.00, 850.00, 560.00, 'BEA-DOG-FIP-1P', '8901234500264', 70, 15, 0.05,
 'active', FALSE, 'all', 'medium', 'not_applicable', 'None'),

(27, 25, NULL, 2, 'Reflective No-Pull Breathable Mesh Dog Harness', 'reflective-no-pull-dog-harness',
 'Ergonomic chest harness with reinforced alloy front D-ring to safely discourage leash pulling.',
 'Features 3M high-visibility luminous stitching and quick-release safety side buckles.',
 1250.00, 1450.00, 820.00, 'ACC-DOG-HARN-M', '8901234500271', 45, 10, 0.35,
 'active', TRUE, 'all', 'medium', 'not_applicable', 'None'),

(28, 25, NULL, 2, 'Heavy-Duty 5-Meter Retractable Dog Walking Leash', 'retractable-dog-leash-5m',
 'One-button brake & lock mechanism with tangle-free 360-degree nylon ribbon lead.',
 'Built with anti-slip rubberized grip and durable zinc snap hook supporting dogs up to 30kg.',
 980.00, 1150.00, 650.00, 'ACC-DOG-LSH-5M', '8901234500288', 50, 10, 0.40,
 'active', FALSE, 'all', 'all', 'not_applicable', 'None'),

-- ==================== BIRD PRODUCTS (ID 29-34) ====================
(29, 30, 9, 3, 'Versele-Laga Prestige Budgies Daily Seed Mix 1kg', 'versele-laga-budgies-seed-1kg',
 'Premium Belgian seed mixture containing yellow millet, red millet, and peeled oats for budgerigars.',
 'Ensures healthy plumage, energetic singing, and optimal avian digestive wellness.',
 650.00, 750.00, 480.00, 'VL-BRD-BUDG-1KG', '8901234500295', 75, 15, 1.00,
 'active', TRUE, 'all', 'all', 'dry', 'None'),

(30, 30, 9, 3, 'Versele-Laga Prestige Cockatiels Premium Seed Mix 1kg', 'versele-laga-cockatiel-seed-1kg',
 'Wholesome seed blend tailored for cockatiels and medium parakeets, fortified with striped sunflower seeds.',
 'Enriched with essential amino acids and clean canary grass seed for vibrant crest feathers.',
 750.00, 850.00, 560.00, 'VL-BRD-CKTL-1KG', '8901234500301', 65, 15, 1.00,
 'active', FALSE, 'all', 'all', 'dry', 'None'),

(31, 31, NULL, 3, 'Spacious Wrought Iron Bird Cage with Perches & Feeders (18x18x24")', 'spacious-iron-bird-cage-medium',
 'Electrostatic rust-resistant iron bird home with slide-out cleaning tray and dual feeders.',
 'Features safe 1cm bar spacing, two natural wood perches, and secure escape-proof door latches.',
 2850.00, 3200.00, 2100.00, 'CAG-BRD-IRON-M', '8901234500318', 20, 5, 3.80,
 'active', TRUE, 'all', 'all', 'not_applicable', 'None'),

(32, 32, NULL, 3, 'Natural Wooden Hanging Ladder & Swing Bird Toy', 'natural-wood-bird-ladder-swing',
 'Handcrafted pesticide-free wooden ladder with colorful beads, metal bells, and climbing rungs.',
 'Encourages foot muscle exercise, relieves cage boredom, and stimulates natural bird curiosity.',
 380.00, 450.00, 240.00, 'TOY-BRD-LAD-SWG', '8901234500325', 90, 20, 0.15,
 'active', FALSE, 'all', 'all', 'not_applicable', 'None'),

(33, 33, 10, 3, 'Beaphar Vinka Liquid Multivitamin for Birds 50ml', 'beaphar-vinka-bird-vitamins-50ml',
 'Dutch high-potency water-soluble drops loaded with 12 essential vitamins (A, C, D3, E, B-complex).',
 'Aids molt condition, stimulates appetite, and strengthens reproductive performance during breeding.',
 680.00, 780.00, 490.00, 'BEA-BRD-VINKA-50ML', '8901234500332', 45, 10, 0.08,
 'active', FALSE, 'all', 'all', 'not_applicable', 'None'),

(34, 33, NULL, 3, 'Natural Marine Cuttlefish Bone with Metal Cage Holder (Twin Pack)', 'natural-cuttlefish-bone-twin',
 'Pure natural ocean cuttlebone providing organic calcium and micro-minerals for caged birds.',
 'Trims overgrown beaks naturally while providing crucial calcium for solid eggshells.',
 290.00, 350.00, 180.00, 'MED-BRD-CUTTLE-2P', '8901234500349', 110, 25, 0.12,
 'active', TRUE, 'all', 'all', 'not_applicable', 'Dental'),

-- ==================== RABBIT PRODUCTS (ID 35-39) ====================
(35, 40, 9, 4, 'Versele-Laga Cuni Complete Adult Rabbit Pellets 1.75kg', 'versele-laga-cuni-complete-1-75kg',
 'All-in-one extruded extruded rabbit pellets formulated without whole grains to prevent selective feeding.',
 'Rich in long unground fibers and timothy hay to ensure dental wear and optimal cecal flora.',
 1850.00, 2100.00, 1420.00, 'VL-RAB-CUNI-175KG', '8901234500356', 35, 8, 1.75,
 'active', TRUE, 'adult', 'all', 'dry', 'Dental'),

(36, 40, NULL, 4, 'Sun-Cured Green Timothy Hay for Rabbits 1kg', 'sun-cured-timothy-hay-1kg',
 'Premium high-fiber, low-protein aromatic timothy hay harvested at early bloom stage.',
 'Essential daily roughage promoting continuous digestive gut motility and preventing rabbit trichobezoars.',
 850.00, 980.00, 600.00, 'HAY-RAB-TIM-1KG', '8901234500363', 60, 12, 1.00,
 'active', TRUE, 'all', 'all', 'dry', 'Digestion'),

(37, 41, NULL, 4, 'No-Drip Ballpoint Hanging Rabbit Water Bottle 500ml', 'no-drip-rabbit-water-bottle-500ml',
 'Leak-proof stainless steel twin-ball drinking nozzle with easy screw-on cage bracket.',
 'Ensures clean and uncontaminated hydration without cage bedding soaking.',
 420.00, 500.00, 270.00, 'ACC-RAB-BTL-500ML', '8901234500370', 80, 15, 0.20,
 'active', FALSE, 'all', 'all', 'not_applicable', 'None'),

(38, 41, NULL, 4, 'Foldable Indoor Rabbit Playpen & Exercise Enclosure (8 Panels)', 'foldable-rabbit-playpen-8p',
 'Heavy-gauge powder-coated wire fence providing 16 sq. ft of safe free-roaming exercise area.',
 'Sets up in seconds without tools and folds completely flat for travel and storage.',
 2750.00, 3100.00, 1950.00, 'ACC-RAB-PEN-8P', '8901234500387', 22, 5, 4.20,
 'active', FALSE, 'all', 'all', 'not_applicable', 'None'),

(39, 42, 15, 4, 'Vitakraft Carotties Crunch Treats for Rabbits 50g', 'vitakraft-carotties-rabbit-treats-50g',
 'Crunchy carrot-infused reward rings enriched with carotene, vitamins, and minerals.',
 'Encourages nibbling instincts while reinforcing positive human-bunny bonding.',
 320.00, 380.00, 210.00, 'VTK-RAB-CARROT-50G', '8901234500394', 70, 15, 0.05,
 'active', FALSE, 'all', 'all', 'treat', 'None'),

-- ==================== SMALL PET PRODUCTS (ID 40-42) ====================
(40, 50, 9, 5, 'Versele-Laga Crispy Muesli Hamster & Gerbil Food 1kg', 'versele-laga-crispy-muesli-hamster-1kg',
 'Gourmet varied muesli blend containing puffed grains, crunchy seeds, and vegetable flakes.',
 'Enriched with "Happy & Healthy" pellets containing all essential vitamins for small rodents.',
 720.00, 820.00, 510.00, 'VL-PET-HAMST-1KG', '8901234500400', 50, 10, 1.00,
 'active', TRUE, 'all', 'small', 'dry', 'None'),

(41, 51, NULL, 5, 'Dust-Free Natural Aspen Wood Shavings Bedding 1kg', 'natural-aspen-wood-bedding-1kg',
 '100% kiln-dried aspen wood flakes free from aromatic phenols and toxic pine/cedar oils.',
 'High absorbency with non-allergenic comfort for hamsters, guinea pigs, and hedgehogs.',
 450.00, 520.00, 310.00, 'BED-PET-ASPEN-1KG', '8901234500417', 90, 15, 1.00,
 'active', FALSE, 'all', 'small', 'not_applicable', 'None'),

(42, 51, NULL, 5, 'Silent Spinner Hamster Exercise Wheel with Stand 17cm', 'silent-spinner-hamster-wheel-17cm',
 'Whisper-quiet ball-bearing running wheel with solid running surface to protect delicate tiny feet.',
 'Can be freestanding on the floor or attached directly to cage wire bars.',
 650.00, 750.00, 430.00, 'TOY-PET-WHEEL-17CM', '8901234500424', 40, 8, 0.28,
 'active', TRUE, 'all', 'small', 'not_applicable', 'None')
ON DUPLICATE KEY UPDATE name=VALUES(name);

-- ----------------------------------------------------------------------------
-- 2. SEED PRODUCT IMAGES (Placeholder URLs for frontend rendering)
-- ----------------------------------------------------------------------------
INSERT INTO product_images (product_id, image_url, alt_text, sort_order, is_primary) VALUES
-- Cat Images
(1, '/assets/products/royal-canin-mother-babycat-1.webp', 'Royal Canin Mother and Babycat Front', 1, TRUE),
(1, '/assets/products/royal-canin-mother-babycat-2.webp', 'Royal Canin Mother and Babycat Kibble Detail', 2, FALSE),
(2, '/assets/products/royal-canin-kitten-1.webp', 'Royal Canin Kitten Dry Food 2kg', 1, TRUE),
(3, '/assets/products/royal-canin-hairball-1.webp', 'Royal Canin Hairball Care Package', 1, TRUE),
(4, '/assets/products/reflex-plus-salmon-adult-1.webp', 'Reflex Plus Adult Cat Food Salmon', 1, TRUE),
(5, '/assets/products/reflex-plus-kitten-1.webp', 'Reflex Plus Kitten Food Chicken', 1, TRUE),
(6, '/assets/products/whiskas-ocean-fish-1.webp', 'Whiskas Ocean Fish Adult Cat Food', 1, TRUE),
(7, '/assets/products/me-o-seafood-1.webp', 'Me-O Adult Seafood Dry Cat Food', 1, TRUE),
(8, '/assets/products/whiskas-tuna-pouch-1.webp', 'Whiskas Tuna Wet Cat Food Pouch 85g', 1, TRUE),
(9, '/assets/products/sheba-chicken-pouch-1.webp', 'Sheba Melty Chicken Breast Gravy Pouch', 1, TRUE),
(10, '/assets/products/me-o-creamy-treat-1.webp', 'Me-O Creamy Cat Treats Salmon 4 Pack', 1, TRUE),
(11, '/assets/products/kit-cat-purr-puree-1.webp', 'Kit Cat Purr Puree Chicken and Fish', 1, TRUE),
(12, '/assets/products/kit-cat-soya-litter-1.webp', 'Kit Cat SoyaClump Tofu Litter 7L', 1, TRUE),
(13, '/assets/products/reflex-bentonite-litter-1.webp', 'Reflex Lavender Bentonite Cat Litter 10L', 1, TRUE),
(14, '/assets/products/hooded-litter-box-1.webp', 'Enclosed Hooded Cat Litter Box with Scoop', 1, TRUE),
(15, '/assets/products/cat-tree-sisal-1.webp', 'Multi Level Sisal Cat Scratching Tree', 1, TRUE),
(16, '/assets/products/beaphar-malt-paste-1.webp', 'Beaphar Malt Paste Anti-Hairball Tube', 1, TRUE),

-- Dog Images
(17, '/assets/products/royal-canin-maxi-puppy-1.webp', 'Royal Canin Maxi Puppy Bag', 1, TRUE),
(18, '/assets/products/royal-canin-medium-adult-1.webp', 'Royal Canin Medium Adult Dog Bag', 1, TRUE),
(19, '/assets/products/drools-focus-adult-1.webp', 'Drools Focus Adult Super Premium 4kg', 1, TRUE),
(20, '/assets/products/smartheart-roast-beef-1.webp', 'SmartHeart Adult Dog Food Roast Beef', 1, TRUE),
(21, '/assets/products/purina-pro-plan-savor-1.webp', 'Purina Pro Plan Savor Dog Canned Can', 1, TRUE),
(22, '/assets/products/jerhigh-milky-treats-1.webp', 'JerHigh Milky Dog Chew Sticks Bag', 1, TRUE),
(23, '/assets/products/drools-calcium-bone-1.webp', 'Drools Absolute Calcium Dog Bone Jar', 1, TRUE),
(24, '/assets/products/rubber-dog-ball-1.webp', 'Indestructible Rubber Dog Ball Fetch Toy', 1, TRUE),
(25, '/assets/products/bioline-neem-shampoo-1.webp', 'Bioline Neem Anti-Flea Shampoo Bottle', 1, TRUE),
(26, '/assets/products/beaphar-fiprotec-dog-1.webp', 'Beaphar Fiprotec Spot-on Pipette Box', 1, TRUE),
(27, '/assets/products/dog-harness-reflective-1.webp', 'Reflective No Pull Dog Walking Harness', 1, TRUE),
(28, '/assets/products/retractable-dog-leash-1.webp', 'Heavy Duty 5M Retractable Dog Leash', 1, TRUE),

-- Bird Images
(29, '/assets/products/versele-laga-budgie-1.webp', 'Versele-Laga Prestige Budgies Daily Seed 1kg', 1, TRUE),
(30, '/assets/products/versele-laga-cockatiel-1.webp', 'Versele-Laga Prestige Cockatiel Seed 1kg', 1, TRUE),
(31, '/assets/products/iron-bird-cage-1.webp', 'Spacious Iron Bird Cage with Perches', 1, TRUE),
(32, '/assets/products/bird-wood-ladder-1.webp', 'Natural Wooden Hanging Bird Ladder Toy', 1, TRUE),
(33, '/assets/products/beaphar-vinka-1.webp', 'Beaphar Vinka Bird Vitamin Drops Bottle', 1, TRUE),
(34, '/assets/products/cuttlefish-bone-1.webp', 'Natural Marine Cuttlefish Bone 2 Pack', 1, TRUE),

-- Rabbit & Small Pet Images
(35, '/assets/products/cuni-complete-rabbit-1.webp', 'Versele-Laga Cuni Complete Adult Rabbit Pellets', 1, TRUE),
(36, '/assets/products/timothy-hay-1.webp', 'Sun Cured Green Timothy Hay Bale', 1, TRUE),
(37, '/assets/products/rabbit-water-bottle-1.webp', 'No Drip Hanging Rabbit Water Bottle 500ml', 1, TRUE),
(38, '/assets/products/rabbit-playpen-1.webp', 'Foldable Indoor Rabbit Playpen 8 Panels', 1, TRUE),
(39, '/assets/products/vitakraft-carotties-1.webp', 'Vitakraft Carotties Crunch Treats Bag', 1, TRUE),
(40, '/assets/products/hamster-crispy-muesli-1.webp', 'Versele-Laga Crispy Muesli Hamster Food 1kg', 1, TRUE),
(41, '/assets/products/aspen-bedding-1.webp', 'Natural Aspen Wood Shavings Bedding 1kg', 1, TRUE),
(42, '/assets/products/silent-spinner-wheel-1.webp', 'Silent Spinner Hamster Wheel 17cm', 1, TRUE);

SELECT '42 master products and gallery images seeded successfully!' AS status;
