using Microsoft.EntityFrameworkCore;
using PetHaven.Api.Data;
using PetHaven.Api.DTOs.Common;
using PetHaven.Api.DTOs.Dashboard;
using PetHaven.Api.DTOs.Orders;
using PetHaven.Api.Services.Interfaces;

namespace PetHaven.Api.Services.Implementations;

public class DashboardService : IDashboardService
{
    private readonly AppDbContext _context;

    public DashboardService(AppDbContext context)
    {
        _context = context;
    }

    public async Task<ApiResponse<DashboardStatsDto>> GetDashboardStatsAsync()
    {
        var totalUsers = await _context.Users.CountAsync();
        var totalCustomers = await _context.Users.CountAsync(u => u.Role == "customer");
        var totalOrders = await _context.Orders.CountAsync();
        var totalSales = await _context.Orders
            .Where(o => o.OrderStatus != "cancelled" && o.OrderStatus != "returned")
            .SumAsync(o => o.TotalAmount);

        var pendingOrders = await _context.Orders.CountAsync(o => o.OrderStatus == "pending");
        var deliveredOrders = await _context.Orders.CountAsync(o => o.OrderStatus == "delivered");

        var lowStockCount = await _context.Products.CountAsync(p => p.StockQuantity <= p.LowStockThreshold && p.StockQuantity > 0);
        var outOfStockCount = await _context.Products.CountAsync(p => p.StockQuantity == 0 || p.Status == "out_of_stock");

        // Best selling products
        var bestSelling = await _context.OrderItems
            .Where(oi => oi.Order.OrderStatus != "cancelled" && oi.Order.OrderStatus != "returned")
            .GroupBy(oi => new { oi.ProductId, oi.ProductName, oi.Sku })
            .Select(g => new BestSellingItemDto
            {
                ProductId = g.Key.ProductId,
                ProductName = g.Key.ProductName,
                Sku = g.Key.Sku,
                TotalUnitsSold = g.Sum(x => x.Quantity),
                TotalRevenue = g.Sum(x => x.TotalPrice)
            })
            .OrderByDescending(x => x.TotalUnitsSold)
            .Take(5)
            .ToListAsync();

        // Recent 5 orders
        var recentOrders = await _context.Orders
            .AsNoTracking()
            .Include(o => o.User)
            .Include(o => o.OrderItems)
            .OrderByDescending(o => o.CreatedAt)
            .Take(5)
            .Select(o => new OrderDto
            {
                Id = o.Id,
                OrderNumber = o.OrderNumber,
                UserId = o.UserId,
                CustomerName = o.User != null ? o.User.Name : o.ShippingFullName,
                CustomerEmail = o.User != null ? o.User.Email : "",
                ShippingFullName = o.ShippingFullName,
                ShippingPhone = o.ShippingPhone,
                ShippingDivision = o.ShippingDivision,
                ShippingDistrict = o.ShippingDistrict,
                ShippingArea = o.ShippingArea,
                ShippingAddressLine = o.ShippingAddressLine,
                Subtotal = o.Subtotal,
                DiscountAmount = o.DiscountAmount,
                DeliveryCharge = o.DeliveryCharge,
                TotalAmount = o.TotalAmount,
                CouponCode = o.CouponCode,
                PaymentMethod = o.PaymentMethod,
                PaymentStatus = o.PaymentStatus,
                OrderStatus = o.OrderStatus,
                CreatedAt = o.CreatedAt,
                Items = o.OrderItems.Select(oi => new OrderItemDto
                {
                    Id = oi.Id,
                    ProductId = oi.ProductId,
                    ProductName = oi.ProductName,
                    VariantName = oi.VariantName,
                    Sku = oi.Sku,
                    UnitPrice = oi.UnitPrice,
                    Quantity = oi.Quantity,
                    TotalPrice = oi.TotalPrice
                }).ToList()
            })
            .ToListAsync();

        // Daily sales summary for past 7 days
        var pastWeek = DateTime.UtcNow.Date.AddDays(-7);
        var salesData = await _context.Orders
            .Where(o => o.CreatedAt >= pastWeek && o.OrderStatus != "cancelled" && o.OrderStatus != "returned")
            .GroupBy(o => o.CreatedAt.Date)
            .Select(g => new DailySalesSummaryDto
            {
                Date = g.Key.ToString("yyyy-MM-dd"),
                OrderCount = g.Count(),
                Revenue = g.Sum(o => o.TotalAmount)
            })
            .OrderBy(s => s.Date)
            .ToListAsync();

        var stats = new DashboardStatsDto
        {
            TotalUsers = totalUsers,
            TotalCustomers = totalCustomers,
            TotalOrders = totalOrders,
            TotalSales = totalSales,
            PendingOrders = pendingOrders,
            DeliveredOrders = deliveredOrders,
            LowStockProductsCount = lowStockCount,
            OutOfStockProductsCount = outOfStockCount,
            BestSellingProducts = bestSelling,
            RecentOrders = recentOrders,
            SalesSummary = salesData
        };

        return ApiResponse<DashboardStatsDto>.Ok(stats);
    }
}
