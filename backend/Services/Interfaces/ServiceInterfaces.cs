using PetHaven.Api.DTOs.Auth;
using PetHaven.Api.DTOs.Cart;
using PetHaven.Api.DTOs.Common;
using PetHaven.Api.DTOs.Coupons;
using PetHaven.Api.DTOs.Dashboard;
using PetHaven.Api.DTOs.Orders;
using PetHaven.Api.DTOs.Products;
using PetHaven.Api.DTOs.Wishlist;

namespace PetHaven.Api.Services.Interfaces;

public interface IAuthService
{
    Task<ApiResponse<AuthResponseDto>> RegisterAsync(RegisterDto dto);
    Task<ApiResponse<AuthResponseDto>> LoginAsync(LoginDto dto);
    Task<ApiResponse<AuthResponseDto>> RefreshTokenAsync(RefreshTokenDto dto);
    Task<ApiResponse<bool>> LogoutAsync(ulong userId);
}

public interface IProductService
{
    Task<ApiResponse<PagedResult<ProductDto>>> GetProductsAsync(ProductQueryParameters parameters);
    Task<ApiResponse<ProductDetailDto>> GetProductByIdAsync(ulong id);
    Task<ApiResponse<ProductDetailDto>> GetProductBySlugAsync(string slug);
    Task<ApiResponse<ProductDetailDto>> CreateProductAsync(CreateProductDto dto);
    Task<ApiResponse<ProductDetailDto>> UpdateProductAsync(ulong id, UpdateProductDto dto);
    Task<ApiResponse<bool>> DeleteProductAsync(ulong id);
    Task<ApiResponse<PagedResult<ProductDto>>> SearchProductsAsync(string query, PaginationParams pagination);
}

public interface ICartService
{
    Task<ApiResponse<CartDto>> GetCartAsync(ulong userId);
    Task<ApiResponse<CartDto>> AddToCartAsync(ulong userId, AddToCartDto dto);
    Task<ApiResponse<CartDto>> UpdateCartItemAsync(ulong userId, ulong itemId, UpdateCartItemDto dto);
    Task<ApiResponse<CartDto>> RemoveCartItemAsync(ulong userId, ulong itemId);
    Task<ApiResponse<bool>> ClearCartAsync(ulong userId);
}

public interface IWishlistService
{
    Task<ApiResponse<WishlistDto>> GetWishlistAsync(ulong userId);
    Task<ApiResponse<bool>> AddToWishlistAsync(ulong userId, ulong productId);
    Task<ApiResponse<bool>> RemoveFromWishlistAsync(ulong userId, ulong productId);
}

public interface IOrderService
{
    Task<ApiResponse<OrderDto>> CreateOrderAsync(ulong userId, CreateOrderDto dto);
    Task<ApiResponse<PagedResult<OrderDto>>> GetCustomerOrdersAsync(ulong userId, PaginationParams pagination);
    Task<ApiResponse<OrderDto>> GetOrderByIdAsync(ulong userId, ulong orderId, bool isAdmin = false);
    Task<ApiResponse<OrderDto>> CancelOrderAsync(ulong userId, ulong orderId);
    Task<ApiResponse<PagedResult<OrderDto>>> GetAdminOrdersAsync(PaginationParams pagination, string? status = null);
    Task<ApiResponse<OrderDto>> UpdateOrderStatusAsync(ulong orderId, UpdateOrderStatusDto dto);
}

public interface ICouponService
{
    Task<ApiResponse<CouponValidationResultDto>> ValidateCouponAsync(ulong userId, ValidateCouponDto dto);
}

public interface IRecommendationService
{
    Task<ApiResponse<List<ProductDto>>> GetRecommendationsAsync(ulong userId, int limit = 8, ulong? petId = null);
}

public interface IDashboardService
{
    Task<ApiResponse<DashboardStatsDto>> GetDashboardStatsAsync();
}
