using Microsoft.EntityFrameworkCore;
using PetHaven.Api.Data;
using PetHaven.Api.DTOs.Common;
using PetHaven.Api.DTOs.Coupons;
using PetHaven.Api.DTOs.Orders;
using PetHaven.Api.Models;
using PetHaven.Api.Services.Interfaces;

namespace PetHaven.Api.Services.Implementations;

public class OrderService : IOrderService
{
    private readonly AppDbContext _context;
    private readonly ICouponService _couponService;
    private readonly ILogger<OrderService> _logger;

    public OrderService(AppDbContext context, ICouponService couponService, ILogger<OrderService> logger)
    {
        _context = context;
        _couponService = couponService;
        _logger = logger;
    }

    public async Task<ApiResponse<OrderDto>> CreateOrderAsync(ulong userId, CreateOrderDto dto)
    {
        // 1. Validate the customer
        var user = await _context.Users.FindAsync(userId);
        if (user == null || user.Status != "active")
        {
            return ApiResponse<OrderDto>.Fail("Customer account is inactive or not found.", 403);
        }

        // Determine order items (either from request or from active cart)
        List<(ulong ProductId, ulong? VariantId, int Quantity)> itemsToProcess = new();

        if (dto.Items != null && dto.Items.Count > 0)
        {
            itemsToProcess = dto.Items.Select(i => (i.ProductId, i.VariantId, i.Quantity)).ToList();
        }
        else
        {
            var cart = await _context.Carts.Include(c => c.Items).FirstOrDefaultAsync(c => c.UserId == userId);
            if (cart == null || !cart.Items.Any())
            {
                return ApiResponse<OrderDto>.Fail("Cannot checkout: cart is empty and no items were specified.", 400);
            }
            itemsToProcess = cart.Items.Select(i => (i.ProductId, i.VariantId, i.Quantity)).ToList();
        }

        // Begin DB Transaction for ACID atomicity
        using var transaction = await _context.Database.BeginTransactionAsync();

        try
        {
            decimal calculatedSubtotal = 0.00m;
            var processedOrderItems = new List<OrderItem>();
            var inventoryLogs = new List<InventoryTransaction>();

            // 2. Validate products, 3. Validate variants, 4. Check stock, 5. Calculate subtotal
            foreach (var itemReq in itemsToProcess)
            {
                var product = await _context.Products.FindAsync(itemReq.ProductId);
                if (product == null || product.Status != "active")
                {
                    await transaction.RollbackAsync();
                    return ApiResponse<OrderDto>.Fail($"Product ID {itemReq.ProductId} is not available.", 400);
                }

                ProductVariant? variant = null;
                if (itemReq.VariantId.HasValue)
                {
                    variant = await _context.ProductVariants
                        .FirstOrDefaultAsync(v => v.Id == itemReq.VariantId.Value && v.ProductId == itemReq.ProductId);

                    if (variant == null || variant.Status != "active")
                    {
                        await transaction.RollbackAsync();
                        return ApiResponse<OrderDto>.Fail($"Variant ID {itemReq.VariantId} for product '{product.Name}' is not available.", 400);
                    }
                }

                int availableStock = variant != null ? variant.StockQuantity : product.StockQuantity;
                if (availableStock < itemReq.Quantity)
                {
                    await transaction.RollbackAsync();
                    return ApiResponse<OrderDto>.Fail($"Insufficient stock for '{product.Name}'. Available: {availableStock}, Requested: {itemReq.Quantity}.", 400);
                }

                decimal unitPrice = variant != null ? variant.Price : product.BasePrice;
                string sku = variant != null ? variant.Sku : product.Sku;
                string? variantName = variant?.VariantName;
                decimal itemTotal = unitPrice * itemReq.Quantity;

                calculatedSubtotal += itemTotal;

                // 11. Reduce stock & 12. Create inventory transaction log
                int prevStock = availableStock;
                int newStock = availableStock - itemReq.Quantity;

                if (variant != null)
                {
                    variant.StockQuantity = newStock;
                    variant.UpdatedAt = DateTime.UtcNow;
                }
                else
                {
                    product.StockQuantity = newStock;
                    product.UpdatedAt = DateTime.UtcNow;
                }

                processedOrderItems.Add(new OrderItem
                {
                    ProductId = product.Id,
                    VariantId = variant?.Id,
                    ProductName = product.Name,
                    VariantName = variantName,
                    Sku = sku,
                    UnitPrice = unitPrice,
                    Quantity = itemReq.Quantity,
                    TotalPrice = itemTotal,
                    CreatedAt = DateTime.UtcNow
                });

                inventoryLogs.Add(new InventoryTransaction
                {
                    ProductId = product.Id,
                    VariantId = variant?.Id,
                    TransactionType = "sale",
                    Quantity = -itemReq.Quantity,
                    PreviousStock = prevStock,
                    NewStock = newStock,
                    Note = "Stock deducted during checkout",
                    CreatedAt = DateTime.UtcNow
                });
            }

            // 6. Apply valid coupon
            decimal discountAmount = 0.00m;
            Coupon? validatedCoupon = null;

            if (!string.IsNullOrWhiteSpace(dto.CouponCode))
            {
                var couponVal = await _couponService.ValidateCouponAsync(userId, new ValidateCouponDto
                {
                    Code = dto.CouponCode,
                    OrderAmount = calculatedSubtotal
                });

                if (couponVal.Success && couponVal.Data != null && couponVal.Data.IsValid)
                {
                    discountAmount = couponVal.Data.CalculatedDiscount;
                    validatedCoupon = await _context.Coupons.FirstOrDefaultAsync(c => c.Code == dto.CouponCode.Trim());
                    if (validatedCoupon != null)
                    {
                        validatedCoupon.TimesUsed += 1;
                    }
                }
                else
                {
                    await transaction.RollbackAsync();
                    return ApiResponse<OrderDto>.Fail($"Invalid coupon: {couponVal.Data?.Message ?? "Validation failed"}", 400);
                }
            }

            // 7. Calculate delivery charge (80 BDT Dhaka inside, 150 BDT outside)
            decimal deliveryCharge = 80.00m;
            if (!dto.ShippingDivision.Trim().Equals("Dhaka", StringComparison.OrdinalIgnoreCase))
            {
                deliveryCharge = 150.00m;
            }

            // 8. Calculate final total
            decimal finalTotal = Math.Max(0, (calculatedSubtotal - discountAmount) + deliveryCharge);

            // Generate unique Order Number: ORD-YYYYMM-RANDOM
            var orderNumber = $"ORD-{DateTime.UtcNow:yyyyMM}-{Random.Shared.Next(10000, 99999)}";

            // 9. Create order entity
            var order = new Order
            {
                UserId = userId,
                OrderNumber = orderNumber,
                AddressId = dto.AddressId,
                ShippingFullName = dto.ShippingFullName.Trim(),
                ShippingPhone = dto.ShippingPhone.Trim(),
                ShippingDivision = dto.ShippingDivision.Trim(),
                ShippingDistrict = dto.ShippingDistrict.Trim(),
                ShippingArea = dto.ShippingArea.Trim(),
                ShippingAddressLine = dto.ShippingAddressLine.Trim(),
                ShippingPostalCode = dto.ShippingPostalCode?.Trim(),
                Subtotal = calculatedSubtotal,
                DiscountAmount = discountAmount,
                DeliveryCharge = deliveryCharge,
                TotalAmount = finalTotal,
                CouponCode = validatedCoupon?.Code,
                PaymentMethod = dto.PaymentMethod,
                PaymentStatus = dto.PaymentMethod == "cash_on_delivery" ? "pending" : "paid",
                OrderStatus = "pending",
                CustomerNote = dto.CustomerNote,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };

            _context.Orders.Add(order);
            await _context.SaveChangesAsync();

            // 10. Link Order Items to newly generated Order ID
            foreach (var oi in processedOrderItems)
            {
                oi.OrderId = order.Id;
                _context.OrderItems.Add(oi);
            }

            // Link inventory transaction to order id
            foreach (var log in inventoryLogs)
            {
                log.ReferenceOrderId = order.Id;
                log.Note = $"Order #{order.OrderNumber} placement";
                _context.InventoryTransactions.Add(log);
            }

            // Record coupon usage if applicable
            if (validatedCoupon != null)
            {
                _context.CouponUsages.Add(new CouponUsage
                {
                    CouponId = validatedCoupon.Id,
                    UserId = userId,
                    OrderId = order.Id,
                    DiscountApplied = discountAmount,
                    UsedAt = DateTime.UtcNow
                });
            }

            // Clear user's shopping cart upon successful order
            var userCart = await _context.Carts.Include(c => c.Items).FirstOrDefaultAsync(c => c.UserId == userId);
            if (userCart != null && userCart.Items.Any())
            {
                _context.CartItems.RemoveRange(userCart.Items);
            }

            // Send in-app notification
            _context.Notifications.Add(new Notification
            {
                UserId = userId,
                Title = "Order Placed Successfully!",
                Message = $"Your order #{order.OrderNumber} has been received and is being processed.",
                Type = "order_update",
                ReferenceId = order.Id,
                IsRead = false,
                CreatedAt = DateTime.UtcNow
            });

            await _context.SaveChangesAsync();

            // 13. Commit everything inside database transaction
            await transaction.CommitAsync();

            return await GetOrderByIdAsync(userId, order.Id, isAdmin: false);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Transaction failed while creating order for user {UserId}", userId);
            await transaction.RollbackAsync();
            return ApiResponse<OrderDto>.Fail("An unexpected error occurred during checkout. Please try again.", 500);
        }
    }

    public async Task<ApiResponse<PagedResult<OrderDto>>> GetCustomerOrdersAsync(ulong userId, PaginationParams pagination)
    {
        var query = _context.Orders
            .AsNoTracking()
            .Include(o => o.User)
            .Include(o => o.OrderItems)
            .Where(o => o.UserId == userId)
            .OrderByDescending(o => o.CreatedAt);

        var totalCount = await query.CountAsync();
        var orders = await query
            .Skip((pagination.PageNumber - 1) * pagination.PageSize)
            .Take(pagination.PageSize)
            .Select(o => MapOrderToDto(o))
            .ToListAsync();

        return ApiResponse<PagedResult<OrderDto>>.Ok(new PagedResult<OrderDto>(orders, totalCount, pagination.PageNumber, pagination.PageSize));
    }

    public async Task<ApiResponse<OrderDto>> GetOrderByIdAsync(ulong userId, ulong orderId, bool isAdmin = false)
    {
        var query = _context.Orders
            .AsNoTracking()
            .Include(o => o.User)
            .Include(o => o.OrderItems)
            .Where(o => o.Id == orderId);

        if (!isAdmin)
        {
            query = query.Where(o => o.UserId == userId);
        }

        var order = await query.FirstOrDefaultAsync();
        if (order == null)
        {
            return ApiResponse<OrderDto>.Fail("Order not found.", 404);
        }

        return ApiResponse<OrderDto>.Ok(MapOrderToDto(order));
    }

    public async Task<ApiResponse<OrderDto>> CancelOrderAsync(ulong userId, ulong orderId)
    {
        using var transaction = await _context.Database.BeginTransactionAsync();
        try
        {
            var order = await _context.Orders
                .Include(o => o.OrderItems)
                .FirstOrDefaultAsync(o => o.Id == orderId && o.UserId == userId);

            if (order == null)
            {
                return ApiResponse<OrderDto>.Fail("Order not found.", 404);
            }

            if (order.OrderStatus != "pending" && order.OrderStatus != "confirmed")
            {
                return ApiResponse<OrderDto>.Fail($"Cannot cancel order in '{order.OrderStatus}' status.", 400);
            }

            order.OrderStatus = "cancelled";
            order.UpdatedAt = DateTime.UtcNow;

            // Restore stocks
            foreach (var item in order.OrderItems)
            {
                if (item.VariantId.HasValue)
                {
                    var variant = await _context.ProductVariants.FindAsync(item.VariantId.Value);
                    if (variant != null)
                    {
                        var prevStock = variant.StockQuantity;
                        variant.StockQuantity += item.Quantity;
                        _context.InventoryTransactions.Add(new InventoryTransaction
                        {
                            ProductId = item.ProductId,
                            VariantId = variant.Id,
                            TransactionType = "adjustment",
                            Quantity = item.Quantity,
                            PreviousStock = prevStock,
                            NewStock = variant.StockQuantity,
                            ReferenceOrderId = order.Id,
                            Note = $"Order #{order.OrderNumber} cancellation stock return"
                        });
                    }
                }
                else
                {
                    var product = await _context.Products.FindAsync(item.ProductId);
                    if (product != null)
                    {
                        var prevStock = product.StockQuantity;
                        product.StockQuantity += item.Quantity;
                        _context.InventoryTransactions.Add(new InventoryTransaction
                        {
                            ProductId = product.Id,
                            TransactionType = "adjustment",
                            Quantity = item.Quantity,
                            PreviousStock = prevStock,
                            NewStock = product.StockQuantity,
                            ReferenceOrderId = order.Id,
                            Note = $"Order #{order.OrderNumber} cancellation stock return"
                        });
                    }
                }
            }

            await _context.SaveChangesAsync();
            await transaction.CommitAsync();

            return await GetOrderByIdAsync(userId, orderId, isAdmin: false);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to cancel order {OrderId}", orderId);
            await transaction.RollbackAsync();
            return ApiResponse<OrderDto>.Fail("Failed to cancel order.", 500);
        }
    }

    public async Task<ApiResponse<PagedResult<OrderDto>>> GetAdminOrdersAsync(PaginationParams pagination, string? status = null)
    {
        var query = _context.Orders
            .AsNoTracking()
            .Include(o => o.User)
            .Include(o => o.OrderItems)
            .AsQueryable();

        if (!string.IsNullOrWhiteSpace(status))
        {
            query = query.Where(o => o.OrderStatus == status.Trim());
        }

        query = query.OrderByDescending(o => o.CreatedAt);

        var totalCount = await query.CountAsync();
        var orders = await query
            .Skip((pagination.PageNumber - 1) * pagination.PageSize)
            .Take(pagination.PageSize)
            .Select(o => MapOrderToDto(o))
            .ToListAsync();

        return ApiResponse<PagedResult<OrderDto>>.Ok(new PagedResult<OrderDto>(orders, totalCount, pagination.PageNumber, pagination.PageSize));
    }

    public async Task<ApiResponse<OrderDto>> UpdateOrderStatusAsync(ulong orderId, UpdateOrderStatusDto dto)
    {
        var order = await _context.Orders.Include(o => o.OrderItems).FirstOrDefaultAsync(o => o.Id == orderId);
        if (order == null)
        {
            return ApiResponse<OrderDto>.Fail("Order not found.", 404);
        }

        order.OrderStatus = dto.OrderStatus;
        if (!string.IsNullOrWhiteSpace(dto.PaymentStatus))
        {
            order.PaymentStatus = dto.PaymentStatus;
        }
        if (!string.IsNullOrWhiteSpace(dto.AdminNote))
        {
            order.AdminNote = dto.AdminNote;
        }
        order.UpdatedAt = DateTime.UtcNow;

        await _context.SaveChangesAsync();
        return await GetOrderByIdAsync(order.UserId, order.Id, isAdmin: true);
    }

    private static OrderDto MapOrderToDto(Order o) => new()
    {
        Id = o.Id,
        OrderNumber = o.OrderNumber,
        UserId = o.UserId,
        CustomerName = o.User?.Name ?? o.ShippingFullName,
        CustomerEmail = o.User?.Email ?? string.Empty,
        ShippingFullName = o.ShippingFullName,
        ShippingPhone = o.ShippingPhone,
        ShippingDivision = o.ShippingDivision,
        ShippingDistrict = o.ShippingDistrict,
        ShippingArea = o.ShippingArea,
        ShippingAddressLine = o.ShippingAddressLine,
        ShippingPostalCode = o.ShippingPostalCode,
        Subtotal = o.Subtotal,
        DiscountAmount = o.DiscountAmount,
        DeliveryCharge = o.DeliveryCharge,
        TotalAmount = o.TotalAmount,
        CouponCode = o.CouponCode,
        PaymentMethod = o.PaymentMethod,
        PaymentStatus = o.PaymentStatus,
        OrderStatus = o.OrderStatus,
        PaymentTransactionId = o.PaymentTransactionId,
        CustomerNote = o.CustomerNote,
        AdminNote = o.AdminNote,
        CreatedAt = o.CreatedAt,
        Items = o.OrderItems.Select(oi => new OrderItemDto
        {
            Id = oi.Id,
            ProductId = oi.ProductId,
            VariantId = oi.VariantId,
            ProductName = oi.ProductName,
            VariantName = oi.VariantName,
            Sku = oi.Sku,
            UnitPrice = oi.UnitPrice,
            Quantity = oi.Quantity,
            TotalPrice = oi.TotalPrice
        }).ToList()
    };
}
