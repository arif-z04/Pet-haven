using Microsoft.EntityFrameworkCore;
using PetHaven.Api.Data;
using PetHaven.Api.DTOs.Common;
using PetHaven.Api.DTOs.Wishlist;
using PetHaven.Api.Models;
using PetHaven.Api.Services.Interfaces;

namespace PetHaven.Api.Services.Implementations;

public class WishlistService : IWishlistService
{
    private readonly AppDbContext _context;

    public WishlistService(AppDbContext context)
    {
        _context = context;
    }

    public async Task<ApiResponse<WishlistDto>> GetWishlistAsync(ulong userId)
    {
        var wishlist = await GetOrCreateUserWishlistAsync(userId);
        var dto = new WishlistDto
        {
            Id = wishlist.Id,
            Items = wishlist.Items.Select(i => new WishlistItemDto
            {
                Id = i.Id,
                ProductId = i.ProductId,
                ProductName = i.Product.Name,
                ProductSlug = i.Product.Slug,
                BasePrice = i.Product.BasePrice,
                CompareAtPrice = i.Product.CompareAtPrice,
                ImageUrl = i.Product.Images.FirstOrDefault(img => img.IsPrimary)?.ImageUrl ?? i.Product.Images.FirstOrDefault()?.ImageUrl,
                InStock = i.Product.StockQuantity > 0,
                AddedAt = i.CreatedAt
            }).ToList()
        };

        return ApiResponse<WishlistDto>.Ok(dto);
    }

    public async Task<ApiResponse<bool>> AddToWishlistAsync(ulong userId, ulong productId)
    {
        var wishlist = await GetOrCreateUserWishlistAsync(userId);
        var product = await _context.Products.FindAsync(productId);
        if (product == null)
        {
            return ApiResponse<bool>.Fail("Product not found.", 404);
        }

        var alreadyExists = wishlist.Items.Any(i => i.ProductId == productId);
        if (alreadyExists)
        {
            return ApiResponse<bool>.Ok(true, "Product is already in wishlist.");
        }

        var item = new WishlistItem
        {
            WishlistId = wishlist.Id,
            ProductId = productId,
            CreatedAt = DateTime.UtcNow
        };

        _context.WishlistItems.Add(item);
        await _context.SaveChangesAsync();

        return ApiResponse<bool>.Ok(true, "Product added to wishlist.");
    }

    public async Task<ApiResponse<bool>> RemoveFromWishlistAsync(ulong userId, ulong productId)
    {
        var wishlist = await GetOrCreateUserWishlistAsync(userId);
        var item = wishlist.Items.FirstOrDefault(i => i.ProductId == productId);
        if (item == null)
        {
            return ApiResponse<bool>.Fail("Item not found in wishlist.", 404);
        }

        _context.WishlistItems.Remove(item);
        await _context.SaveChangesAsync();

        return ApiResponse<bool>.Ok(true, "Product removed from wishlist.");
    }

    private async Task<Wishlist> GetOrCreateUserWishlistAsync(ulong userId)
    {
        var wishlist = await _context.Wishlists
            .Include(w => w.Items)
                .ThenInclude(i => i.Product)
                    .ThenInclude(p => p.Images)
            .FirstOrDefaultAsync(w => w.UserId == userId);

        if (wishlist == null)
        {
            wishlist = new Wishlist { UserId = userId, CreatedAt = DateTime.UtcNow, UpdatedAt = DateTime.UtcNow };
            _context.Wishlists.Add(wishlist);
            await _context.SaveChangesAsync();
        }

        return wishlist;
    }
}
