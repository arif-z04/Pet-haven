-- ============================================================================
-- 15_triggers.sql
-- Automated Inventory Adjustments and Stock Audit Triggers
-- Compatible with MySQL 8.0+ / MariaDB 10.5+
-- ============================================================================

USE pet_ecommerce_db;

DELIMITER //

-- ----------------------------------------------------------------------------
-- 1. TRIGGER: trg_prevent_negative_stock
-- Ensures stock quantity never drops below zero at the database layer
-- ----------------------------------------------------------------------------
DROP TRIGGER IF EXISTS trg_prevent_negative_stock //
CREATE TRIGGER trg_prevent_negative_stock
BEFORE UPDATE ON products
FOR EACH ROW
BEGIN
    IF NEW.stock_quantity < 0 THEN
        SIGNAL SQLSTATE '45000'
            SET MESSAGE_TEXT = 'Operation aborted: product stock quantity cannot fall below zero.';
    END IF;
END //

-- ----------------------------------------------------------------------------
-- 2. TRIGGER: trg_after_order_item_insert
-- Automatically deducts inventory and logs audit trail when order item is created
-- ----------------------------------------------------------------------------
DROP TRIGGER IF EXISTS trg_after_order_item_insert //
CREATE TRIGGER trg_after_order_item_insert
AFTER INSERT ON order_items
FOR EACH ROW
BEGIN
    DECLARE v_prev_stock INT;
    DECLARE v_new_stock INT;

    -- Retrieve current product stock
    SELECT stock_quantity INTO v_prev_stock 
    FROM products 
    WHERE id = NEW.product_id;

    SET v_new_stock = v_prev_stock - NEW.quantity;

    -- Update product stock
    UPDATE products 
    SET stock_quantity = v_new_stock 
    WHERE id = NEW.product_id;

    -- Update variant stock if variant purchased
    IF NEW.variant_id IS NOT NULL THEN
        UPDATE product_variants 
        SET stock_quantity = stock_quantity - NEW.quantity 
        WHERE id = NEW.variant_id;
    END IF;

    -- Insert automated inventory transaction audit record
    INSERT INTO inventory_transactions (
        product_id, variant_id, transaction_type, quantity, 
        previous_stock, new_stock, reference_order_id, note
    ) VALUES (
        NEW.product_id, 
        NEW.variant_id, 
        'sale', 
        -NEW.quantity, 
        v_prev_stock, 
        v_new_stock, 
        NEW.order_id, 
        CONCAT('Automatic stock deduction for order #', NEW.order_id)
    );
END //

-- ----------------------------------------------------------------------------
-- 3. TRIGGER: trg_after_order_status_update
-- Restores inventory if an order transitions into 'cancelled' or 'returned'
-- ----------------------------------------------------------------------------
DROP TRIGGER IF EXISTS trg_after_order_status_update //
CREATE TRIGGER trg_after_order_status_update
AFTER UPDATE ON orders
FOR EACH ROW
BEGIN
    -- Only trigger when order is freshly cancelled or returned from an active state
    IF (NEW.order_status IN ('cancelled', 'returned')) 
       AND (OLD.order_status NOT IN ('cancelled', 'returned')) THEN

        -- Temporary cursor simulation via INSERT / UPDATE logic
        -- Restore product master stock
        UPDATE products p
        JOIN order_items oi ON p.id = oi.product_id
        SET p.stock_quantity = p.stock_quantity + oi.quantity
        WHERE oi.order_id = NEW.id;

        -- Restore variant stock if applicable
        UPDATE product_variants pv
        JOIN order_items oi ON pv.id = oi.variant_id
        SET pv.stock_quantity = pv.stock_quantity + oi.quantity
        WHERE oi.order_id = NEW.id;

        -- Insert return inventory audit records
        INSERT INTO inventory_transactions (
            product_id, variant_id, transaction_type, quantity, 
            previous_stock, new_stock, reference_order_id, note
        )
        SELECT 
            oi.product_id, 
            oi.variant_id, 
            CASE WHEN NEW.order_status = 'cancelled' THEN 'adjustment' ELSE 'return' END,
            oi.quantity,
            p.stock_quantity - oi.quantity,
            p.stock_quantity,
            NEW.id,
            CONCAT('Automatic stock restoration for ', NEW.order_status, ' order #', NEW.order_number)
        FROM order_items oi
        JOIN products p ON oi.product_id = p.id
        WHERE oi.order_id = NEW.id;

    END IF;
END //

-- ----------------------------------------------------------------------------
-- 4. TRIGGER: trg_after_coupon_usage_insert
-- Automatically increments the redemption counter on the coupon master record
-- ----------------------------------------------------------------------------
DROP TRIGGER IF EXISTS trg_after_coupon_usage_insert //
CREATE TRIGGER trg_after_coupon_usage_insert
AFTER INSERT ON coupon_usages
FOR EACH ROW
BEGIN
    UPDATE coupons 
    SET times_used = times_used + 1 
    WHERE id = NEW.coupon_id;
END //

DELIMITER ;

SELECT 'Database inventory triggers created successfully!' AS status;
