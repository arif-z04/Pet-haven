using Microsoft.EntityFrameworkCore;
using PetHaven.Api.Data;
using PetHaven.Api.DTOs.Cart;
using PetHaven.Api.DTOs.Common;
using PetHaven.Api.Models;
using PetHaven.Api.Services.Interfaces;

namespace PetHaven.Api.Services.Implementations;

public class CartService : ICartService
{
    private readonly AppDbContext _context;

    public CartService(AppDbContext context)
    {
        _context = context;
    }

    public async Task<ApiResponse<CartDto>> GetCartAsync(ulong userId)
    {
        var cart = await GetOrCreateUserCartAsync(userId);
        return ApiResponse<CartDto>.Ok(MapCartToDto(cart));
    }

    public async Task<ApiResponse<CartDto>> AddToCartAsync(ulong userId, AddToCartDto dto)
    {
        var cart = await GetOrCreateUserCartAsync(userId);
        var product = await _context.Products.FindAsync(dto.ProductId);
        if (product == null || product.Status != "active")
        {
            return ApiResponse<CartDto>.Fail("Product is not available.", 404);
        }

        ProductVariant? variant = null;
        if (dto.VariantId.HasValue)
        {
            variant = await _context.ProductVariants.FirstOrDefaultAsync(v => v.Id == dto.VariantId.Value && v.ProductId == dto.ProductId);
            if (variant == null || variant.Status != "active")
            {
                return ApiResponse<CartDto>.Fail("Product variant is not available.", 404);
            }
        }

        var availableStock = variant != null ? variant.StockQuantity : product.StockQuantity;
        if (availableStock < dto.Quantity)
        {
            return ApiResponse<CartDto>.Fail($"Insufficient stock. Available: {availableStock}", 400);
        }

        var existingItem = cart.Items.FirstOrDefault(i => i.ProductId == dto.ProductId && i.VariantId == dto.VariantId);
        if (existingItem != null)
        {
            var newTotalQty = existingItem.Quantity + dto.Quantity;
            if (availableStock < newTotalQty)
            {
                return ApiResponse<CartDto>.Fail($"Cannot add more. Maximum available stock is {availableStock}.", 400);
            }
            existingItem.Quantity = newTotalQty;
            existingItem.UpdatedAt = DateTime.UtcNow;
        }
        else
        {
            var newItem = new CartItem
            {
                CartId = cart.Id,
                ProductId = dto.ProductId,
                VariantId = dto.VariantId,
                Quantity = dto.Quantity,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };
            _context.CartItems.Add(newItem);
        }

        await _context.SaveChangesAsync();
        var refreshedCart = await GetOrCreateUserCartAsync(userId);
        return ApiResponse<CartDto>.Ok(MapCartToDto(refreshedCart), "Item added to cart.");
    }

    public async Task<ApiResponse<CartDto>> UpdateCartItemAsync(ulong userId, ulong itemId, UpdateCartItemDto dto)
    {
        var cart = await GetOrCreateUserCartAsync(userId);
        var item = cart.Items.FirstOrDefault(i => i.Id == itemId);
        if (item == null)
        {
            return ApiResponse<CartDto>.Fail("Cart item not found.", 404);
        }

        var product = await _context.Products.FindAsync(item.ProductId);
        ProductVariant? variant = item.VariantId.HasValue ? await _context.ProductVariants.FindAsync(item.VariantId.Value) : null;
        var availableStock = variant != null ? variant.StockQuantity : product?.StockQuantity ?? 0;

        if (availableStock < dto.Quantity)
        {
            return ApiResponse<CartDto>.Fail($"Insufficient stock. Maximum available: {availableStock}", 400);
        }

        item.Quantity = dto.Quantity;
        item.UpdatedAt = DateTime.UtcNow;
        await _context.SaveChangesAsync();

        var refreshedCart = await GetOrCreateUserCartAsync(userId);
        return ApiResponse<CartDto>.Ok(MapCartToDto(refreshedCart), "Cart item updated.");
    }

    public async Task<ApiResponse<CartDto>> RemoveCartItemAsync(ulong userId, ulong itemId)
    {
        var cart = await GetOrCreateUserCartAsync(userId);
        var item = cart.Items.FirstOrDefault(i => i.Id == itemId);
        if (item == null)
        {
            return ApiResponse<CartDto>.Fail("Cart item not found.", 404);
        }

        _context.CartItems.Remove(item);
        await _context.SaveChangesAsync();

        var refreshedCart = await GetOrCreateUserCartAsync(userId);
        return ApiResponse<CartDto>.Ok(MapCartToDto(refreshedCart), "Item removed from cart.");
    }

    public async Task<ApiResponse<bool>> ClearCartAsync(ulong userId)
    {
        var cart = await GetOrCreateUserCartAsync(userId);
        if (cart.Items.Any())
        {
            _context.CartItems.RemoveRange(cart.Items);
            await _context.SaveChangesAsync();
        }
        return ApiResponse<bool>.Ok(true, "Cart cleared successfully.");
    }

    private async Task<Cart> GetOrCreateUserCartAsync(ulong userId)
    {
        var cart = await _context.Carts
            .Include(c => c.Items)
                .ThenInclude(i => i.Product)
                    .ThenInclude(p => p.Images)
            .Include(c => c.Items)
                .ThenInclude(i => i.Variant)
            .FirstOrDefaultAsync(c => c.UserId == userId);

        if (cart == null)
        {
            cart = new Cart { UserId = userId, CreatedAt = DateTime.UtcNow, UpdatedAt = DateTime.UtcNow };
            _context.Carts.Add(cart);
            await _context.SaveChangesAsync();
        }

        return cart;
    }

    private static CartDto MapCartToDto(Cart cart)
    {
        return new CartDto
        {
            Id = cart.Id,
            Items = cart.Items.Select(i =>
            {
                var price = i.Variant != null ? i.Variant.Price : i.Product.BasePrice;
                var sku = i.Variant != null ? i.Variant.Sku : i.Product.Sku;
                var stock = i.Variant != null ? i.Variant.StockQuantity : i.Product.StockQuantity;
                var primaryImg = i.Product.Images.FirstOrDefault(img => img.IsPrimary)?.ImageUrl ?? i.Product.Images.FirstOrDefault()?.ImageUrl;

                return new CartItemDto
                {
                    Id = i.Id,
                    ProductId = i.ProductId,
                    ProductName = i.Product.Name,
                    ProductSlug = i.Product.Slug,
                    ImageUrl = primaryImg,
                    VariantId = i.VariantId,
                    VariantName = i.Variant?.VariantName,
                    Sku = sku,
                    UnitPrice = price,
                    Quantity = i.Quantity,
                    AvailableStock = stock
                };
            }).ToList()
        };
    }
}
