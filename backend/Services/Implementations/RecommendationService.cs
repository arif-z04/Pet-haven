using Microsoft.EntityFrameworkCore;
using PetHaven.Api.Data;
using PetHaven.Api.DTOs.Common;
using PetHaven.Api.DTOs.Products;
using PetHaven.Api.Models;
using PetHaven.Api.Services.Interfaces;

namespace PetHaven.Api.Services.Implementations;

public class RecommendationService : IRecommendationService
{
    private readonly AppDbContext _context;

    public RecommendationService(AppDbContext context)
    {
        _context = context;
    }

    public async Task<ApiResponse<List<ProductDto>>> GetRecommendationsAsync(ulong userId, int limit = 8, ulong? petId = null)
    {
        var recommendedProducts = new List<Product>();

        // 1. Rule 1: Inspect user's pet profile(s) from "My Pet"
        var petProfiles = await _context.PetProfiles
            .AsNoTracking()
            .Where(p => p.UserId == userId)
            .OrderByDescending(p => p.CreatedAt)
            .ToListAsync();

        if (petProfiles.Count > 0)
        {
            var primaryPet = (petId.HasValue ? petProfiles.FirstOrDefault(p => p.Id == petId.Value) : null) ?? petProfiles.First();
            var petTypeId = primaryPet.PetTypeId;

            // Determine life stage from pet age
            string targetLifeStage = "all";
            if (primaryPet.DateOfBirth.HasValue)
            {
                var today = DateTime.UtcNow;
                var ageInMonths = ((today.Year - primaryPet.DateOfBirth.Value.Year) * 12) + (today.Month - primaryPet.DateOfBirth.Value.Month);
                if (ageInMonths <= 12)
                {
                    targetLifeStage = "kitten_puppy";
                }
                else if (ageInMonths >= 84) // 7+ years
                {
                    targetLifeStage = "senior";
                }
                else
                {
                    targetLifeStage = "adult";
                }
            }

            var petMatchingQuery = _context.Products
                .AsNoTracking()
                .Include(p => p.Category)
                .Include(p => p.Brand)
                .Include(p => p.PetType)
                .Include(p => p.Images)
                .Include(p => p.Reviews)
                .Where(p => p.Status == "active" && p.StockQuantity > 0 && p.PetTypeId == petTypeId);

            // Filter by life stage or health concern if pet profile specified
            if (!string.IsNullOrWhiteSpace(primaryPet.AllergiesOrMedicalNotes) && primaryPet.AllergiesOrMedicalNotes.Contains("Hairball", StringComparison.OrdinalIgnoreCase))
            {
                petMatchingQuery = petMatchingQuery.Where(p => p.HealthConcern != null && p.HealthConcern.Contains("Hairball") || (p.TargetLifeStage == targetLifeStage || p.TargetLifeStage == "all"));
            }
            else
            {
                petMatchingQuery = petMatchingQuery.Where(p => p.TargetLifeStage == targetLifeStage || p.TargetLifeStage == "all");
            }

            var petProducts = await petMatchingQuery
                .OrderByDescending(p => p.IsFeatured)
                .Take(limit)
                .ToListAsync();

            recommendedProducts.AddRange(petProducts);
        }

        // 2. Rule 2: Inspect previously purchased categories if more recommendations needed
        if (recommendedProducts.Count < limit)
        {
            var previousOrderCategoryIds = await _context.OrderItems
                .Where(oi => oi.Order.UserId == userId)
                .Select(oi => oi.Product.CategoryId)
                .Distinct()
                .Take(3)
                .ToListAsync();

            if (previousOrderCategoryIds.Count > 0)
            {
                var existingIds = recommendedProducts.Select(p => p.Id).ToList();
                var categoryProducts = await _context.Products
                    .AsNoTracking()
                    .Include(p => p.Category)
                    .Include(p => p.Brand)
                    .Include(p => p.PetType)
                    .Include(p => p.Images)
                    .Include(p => p.Reviews)
                    .Where(p => p.Status == "active" && p.StockQuantity > 0 && !existingIds.Contains(p.Id))
                    .Where(p => previousOrderCategoryIds.Contains(p.CategoryId))
                    .Take(limit - recommendedProducts.Count)
                    .ToListAsync();

                recommendedProducts.AddRange(categoryProducts);
            }
        }

        // 3. Rule 3: Fallback to top-selling & featured popular items
        if (recommendedProducts.Count < limit)
        {
            var existingIds = recommendedProducts.Select(p => p.Id).ToList();
            var popularProducts = await _context.Products
                .AsNoTracking()
                .Include(p => p.Category)
                .Include(p => p.Brand)
                .Include(p => p.PetType)
                .Include(p => p.Images)
                .Include(p => p.Reviews)
                .Where(p => p.Status == "active" && p.StockQuantity > 0 && !existingIds.Contains(p.Id))
                .OrderByDescending(p => p.IsFeatured)
                .ThenByDescending(p => p.CreatedAt)
                .Take(limit - recommendedProducts.Count)
                .ToListAsync();

            recommendedProducts.AddRange(popularProducts);
        }

        var result = recommendedProducts.Select(p => MapToDto(p)).ToList();
        return ApiResponse<List<ProductDto>>.Ok(result, "Personalized recommendations generated.");
    }

    private static ProductDto MapToDto(Product p)
    {
        var approvedReviews = p.Reviews?.Where(r => r.Status == "approved").ToList();
        var avgRating = approvedReviews != null && approvedReviews.Count > 0 ? approvedReviews.Average(r => (double)r.Rating) : 0.0;
        var primaryImg = p.Images?.FirstOrDefault(i => i.IsPrimary)?.ImageUrl ?? p.Images?.FirstOrDefault()?.ImageUrl;
        decimal? discountPercent = null;

        if (p.CompareAtPrice.HasValue && p.CompareAtPrice > p.BasePrice)
        {
            discountPercent = Math.Round(((p.CompareAtPrice.Value - p.BasePrice) / p.CompareAtPrice.Value) * 100, 1);
        }

        return new ProductDto
        {
            Id = p.Id,
            CategoryId = p.CategoryId,
            CategoryName = p.Category?.Name ?? string.Empty,
            BrandId = p.BrandId,
            BrandName = p.Brand?.Name,
            PetTypeId = p.PetTypeId,
            PetTypeName = p.PetType?.Name ?? string.Empty,
            Name = p.Name,
            Slug = p.Slug,
            ShortDescription = p.ShortDescription,
            BasePrice = p.BasePrice,
            CompareAtPrice = p.CompareAtPrice,
            DiscountPercentage = discountPercent,
            Sku = p.Sku,
            StockQuantity = p.StockQuantity,
            Status = p.Status,
            IsFeatured = p.IsFeatured,
            PrimaryImageUrl = primaryImg,
            AverageRating = Math.Round(avgRating, 1),
            ReviewCount = approvedReviews?.Count ?? 0,
            TargetLifeStage = p.TargetLifeStage,
            FoodType = p.FoodType,
            HealthConcern = p.HealthConcern
        };
    }
}
