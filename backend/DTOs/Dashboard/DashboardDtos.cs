using PetHaven.Api.DTOs.Orders;
using PetHaven.Api.DTOs.Products;

namespace PetHaven.Api.DTOs.Dashboard;

public class DashboardStatsDto
{
    public int TotalUsers { get; set; }
    public int TotalCustomers { get; set; }
    public int TotalOrders { get; set; }
    public decimal TotalSales { get; set; }
    public int PendingOrders { get; set; }
    public int DeliveredOrders { get; set; }
    public int LowStockProductsCount { get; set; }
    public int OutOfStockProductsCount { get; set; }
    public List<BestSellingItemDto> BestSellingProducts { get; set; } = new();
    public List<OrderDto> RecentOrders { get; set; } = new();
    public List<DailySalesSummaryDto> SalesSummary { get; set; } = new();
}

public class BestSellingItemDto
{
    public ulong ProductId { get; set; }
    public string ProductName { get; set; } = string.Empty;
    public string Sku { get; set; } = string.Empty;
    public int TotalUnitsSold { get; set; }
    public decimal TotalRevenue { get; set; }
}

public class DailySalesSummaryDto
{
    public string Date { get; set; } = string.Empty;
    public int OrderCount { get; set; }
    public decimal Revenue { get; set; }
}
