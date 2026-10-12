using Microsoft.EntityFrameworkCore;
using PetHaven.Api.Data;
using PetHaven.Api.DTOs.Common;
using PetHaven.Api.DTOs.Products;
using PetHaven.Api.Models;
using PetHaven.Api.Services.Interfaces;

namespace PetHaven.Api.Services.Implementations;

public class ProductService : IProductService
{
    private readonly AppDbContext _context;
    private readonly ILogger<ProductService> _logger;

    public ProductService(AppDbContext context, ILogger<ProductService> logger)
    {
        _context = context;
        _logger = logger;
    }

    public async Task<ApiResponse<PagedResult<ProductDto>>> GetProductsAsync(ProductQueryParameters parameters)
    {
        var query = _context.Products
            .AsNoTracking()
            .Include(p => p.Category)
            .Include(p => p.Brand)
            .Include(p => p.PetType)
            .Include(p => p.Images)
            .Include(p => p.Reviews)
            .Where(p => p.Status == "active");

        // 1. Filter by Category / Subcategory
        if (parameters.CategoryId.HasValue)
        {
            var catId = parameters.CategoryId.Value;
            var subCategoryIds = await _context.Categories
                .Where(c => c.ParentId == catId || c.Id == catId)
                .Select(c => c.Id)
                .ToListAsync();

            query = query.Where(p => subCategoryIds.Contains(p.CategoryId));
        }
        else if (!string.IsNullOrWhiteSpace(parameters.CategorySlug))
        {
            var category = await _context.Categories.FirstOrDefaultAsync(c => c.Slug == parameters.CategorySlug.Trim());
            if (category != null)
            {
                var subCategoryIds = await _context.Categories
                    .Where(c => c.ParentId == category.Id || c.Id == category.Id)
                    .Select(c => c.Id)
                    .ToListAsync();

                query = query.Where(p => subCategoryIds.Contains(p.CategoryId));
            }
        }

        // 2. Filter by Pet Type
        if (parameters.PetTypeId.HasValue)
        {
            query = query.Where(p => p.PetTypeId == parameters.PetTypeId.Value);
        }
        else if (!string.IsNullOrWhiteSpace(parameters.PetTypeSlug))
        {
            query = query.Where(p => p.PetType.Slug == parameters.PetTypeSlug.Trim());
        }

        // 3. Filter by Brand
        if (parameters.BrandId.HasValue)
        {
            query = query.Where(p => p.BrandId == parameters.BrandId.Value);
        }
        else if (!string.IsNullOrWhiteSpace(parameters.BrandSlug))
        {
            query = query.Where(p => p.Brand != null && p.Brand.Slug == parameters.BrandSlug.Trim());
        }

        // 4. Price range
        if (parameters.MinPrice.HasValue)
        {
            query = query.Where(p => p.BasePrice >= parameters.MinPrice.Value);
        }
        if (parameters.MaxPrice.HasValue)
        {
            query = query.Where(p => p.BasePrice <= parameters.MaxPrice.Value);
        }

        // 5. Stock availability
        if (parameters.InStockOnly == true)
        {
            query = query.Where(p => p.StockQuantity > 0);
        }

        // 6. Discount / On sale
        if (parameters.OnSaleOnly == true)
        {
            query = query.Where(p => p.CompareAtPrice != null && p.CompareAtPrice > p.BasePrice);
        }

        // 7. Featured only
        if (parameters.FeaturedOnly == true)
        {
            query = query.Where(p => p.IsFeatured);
        }

        // 8. Life stage, food type, health concern
        if (!string.IsNullOrWhiteSpace(parameters.LifeStage))
        {
            query = query.Where(p => p.TargetLifeStage == parameters.LifeStage || p.TargetLifeStage == "all");
        }
        if (!string.IsNullOrWhiteSpace(parameters.FoodType))
        {
            query = query.Where(p => p.FoodType == parameters.FoodType);
        }
        if (!string.IsNullOrWhiteSpace(parameters.HealthConcern))
        {
            query = query.Where(p => p.HealthConcern != null && p.HealthConcern.Contains(parameters.HealthConcern));
        }

        // 9. Search keyword
        if (!string.IsNullOrWhiteSpace(parameters.Search))
        {
            var search = parameters.Search.Trim().ToLower();
            query = query.Where(p => p.Name.ToLower().Contains(search) || 
                                     (p.ShortDescription != null && p.ShortDescription.ToLower().Contains(search)) ||
                                     p.Sku.ToLower().Contains(search));
        }

        // 10. Rating filter
        if (parameters.MinRating.HasValue)
        {
            query = query.Where(p => p.Reviews.Where(r => r.Status == "approved").Any() &&
                                     p.Reviews.Where(r => r.Status == "approved").Average(r => (double)r.Rating) >= parameters.MinRating.Value);
        }

        // Sorting
        query = parameters.SortBy?.ToLowerInvariant() switch
        {
            "price_asc" or "pricelowtohigh" => query.OrderBy(p => p.BasePrice),
            "price_desc" or "pricehightolow" => query.OrderByDescending(p => p.BasePrice),
            "rating" => query.OrderByDescending(p => p.Reviews.Where(r => r.Status == "approved").Average(r => (double?)r.Rating) ?? 0),
            "popularity" => query.OrderByDescending(p => p.OrderItems.Sum(oi => oi.Quantity)),
            _ => query.OrderByDescending(p => p.CreatedAt) // newest default
        };

        var totalCount = await query.CountAsync();

        var products = await query
            .Skip((parameters.PageNumber - 1) * parameters.PageSize)
            .Take(parameters.PageSize)
            .Select(p => MapToDto(p))
            .ToListAsync();

        var pagedResult = new PagedResult<ProductDto>(products, totalCount, parameters.PageNumber, parameters.PageSize);
        return ApiResponse<PagedResult<ProductDto>>.Ok(pagedResult);
    }

    public async Task<ApiResponse<ProductDetailDto>> GetProductByIdAsync(ulong id)
    {
        var product = await _context.Products
            .AsNoTracking()
            .Include(p => p.Category)
            .Include(p => p.Brand)
            .Include(p => p.PetType)
            .Include(p => p.Images.OrderBy(i => i.SortOrder))
            .Include(p => p.Variants.Where(v => v.Status == "active"))
            .Include(p => p.Reviews.Where(r => r.Status == "approved"))
            .FirstOrDefaultAsync(p => p.Id == id);

        if (product == null)
        {
            return ApiResponse<ProductDetailDto>.Fail("Product not found.", 404);
        }

        return ApiResponse<ProductDetailDto>.Ok(MapToDetailDto(product));
    }

    public async Task<ApiResponse<ProductDetailDto>> GetProductBySlugAsync(string slug)
    {
        var product = await _context.Products
            .AsNoTracking()
            .Include(p => p.Category)
            .Include(p => p.Brand)
            .Include(p => p.PetType)
            .Include(p => p.Images.OrderBy(i => i.SortOrder))
            .Include(p => p.Variants.Where(v => v.Status == "active"))
            .Include(p => p.Reviews.Where(r => r.Status == "approved"))
            .FirstOrDefaultAsync(p => p.Slug == slug.Trim());

        if (product == null)
        {
            return ApiResponse<ProductDetailDto>.Fail("Product not found.", 404);
        }

        return ApiResponse<ProductDetailDto>.Ok(MapToDetailDto(product));
    }

    public async Task<ApiResponse<ProductDetailDto>> CreateProductAsync(CreateProductDto dto)
    {
        var slug = GenerateSlug(dto.Name);
        var existingSlug = await _context.Products.AnyAsync(p => p.Slug == slug);
        if (existingSlug)
        {
            slug = $"{slug}-{DateTime.UtcNow.Ticks % 10000}";
        }

        var existingSku = await _context.Products.AnyAsync(p => p.Sku == dto.Sku.Trim());
        if (existingSku)
        {
            return ApiResponse<ProductDetailDto>.Fail("SKU must be unique.", 409);
        }

        var product = new Product
        {
            CategoryId = dto.CategoryId,
            BrandId = dto.BrandId,
            PetTypeId = dto.PetTypeId,
            Name = dto.Name.Trim(),
            Slug = slug,
            ShortDescription = dto.ShortDescription,
            Description = dto.Description,
            BasePrice = dto.BasePrice,
            CompareAtPrice = dto.CompareAtPrice,
            CostPrice = dto.CostPrice,
            Sku = dto.Sku.Trim(),
            Barcode = dto.Barcode?.Trim(),
            StockQuantity = dto.StockQuantity,
            LowStockThreshold = dto.LowStockThreshold,
            Weight = dto.Weight,
            Status = dto.Status,
            IsFeatured = dto.IsFeatured,
            TargetLifeStage = dto.TargetLifeStage,
            TargetBreedSize = dto.TargetBreedSize,
            FoodType = dto.FoodType,
            HealthConcern = dto.HealthConcern,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };

        if (dto.ImageUrls != null && dto.ImageUrls.Count > 0)
        {
            for (int i = 0; i < dto.ImageUrls.Count; i++)
            {
                product.Images.Add(new ProductImage
                {
                    ImageUrl = dto.ImageUrls[i],
                    SortOrder = i + 1,
                    IsPrimary = i == 0
                });
            }
        }

        _context.Products.Add(product);
        await _context.SaveChangesAsync();

        return await GetProductByIdAsync(product.Id);
    }

    public async Task<ApiResponse<ProductDetailDto>> UpdateProductAsync(ulong id, UpdateProductDto dto)
    {
        var product = await _context.Products.Include(p => p.Images).FirstOrDefaultAsync(p => p.Id == id);
        if (product == null)
        {
            return ApiResponse<ProductDetailDto>.Fail("Product not found.", 404);
        }

        var skuConflict = await _context.Products.AnyAsync(p => p.Sku == dto.Sku.Trim() && p.Id != id);
        if (skuConflict)
        {
            return ApiResponse<ProductDetailDto>.Fail("SKU must be unique.", 409);
        }

        product.CategoryId = dto.CategoryId;
        product.BrandId = dto.BrandId;
        product.PetTypeId = dto.PetTypeId;
        product.Name = dto.Name.Trim();
        product.ShortDescription = dto.ShortDescription;
        product.Description = dto.Description;
        product.BasePrice = dto.BasePrice;
        product.CompareAtPrice = dto.CompareAtPrice;
        product.CostPrice = dto.CostPrice;
        product.Sku = dto.Sku.Trim();
        product.Barcode = dto.Barcode?.Trim();
        product.StockQuantity = dto.StockQuantity;
        product.LowStockThreshold = dto.LowStockThreshold;
        product.Weight = dto.Weight;
        product.Status = dto.Status;
        product.IsFeatured = dto.IsFeatured;
        product.TargetLifeStage = dto.TargetLifeStage;
        product.TargetBreedSize = dto.TargetBreedSize;
        product.FoodType = dto.FoodType;
        product.HealthConcern = dto.HealthConcern;
        product.UpdatedAt = DateTime.UtcNow;

        await _context.SaveChangesAsync();
        return await GetProductByIdAsync(product.Id);
    }

    public async Task<ApiResponse<bool>> DeleteProductAsync(ulong id)
    {
        var product = await _context.Products.FindAsync(id);
        if (product == null)
        {
            return ApiResponse<bool>.Fail("Product not found.", 404);
        }

        var hasOrders = await _context.OrderItems.AnyAsync(oi => oi.ProductId == id);
        if (hasOrders)
        {
            // Soft delete/archive to preserve historical records
            product.Status = "archived";
            product.UpdatedAt = DateTime.UtcNow;
            await _context.SaveChangesAsync();
            return ApiResponse<bool>.Ok(true, "Product has order history and was marked as archived.");
        }

        _context.Products.Remove(product);
        await _context.SaveChangesAsync();
        return ApiResponse<bool>.Ok(true, "Product deleted successfully.");
    }

    public async Task<ApiResponse<PagedResult<ProductDto>>> SearchProductsAsync(string query, PaginationParams pagination)
    {
        var parameters = new ProductQueryParameters
        {
            Search = query,
            PageNumber = pagination.PageNumber,
            PageSize = pagination.PageSize
        };
        return await GetProductsAsync(parameters);
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

    private static ProductDetailDto MapToDetailDto(Product p)
    {
        var baseDto = MapToDto(p);
        return new ProductDetailDto
        {
            Id = baseDto.Id,
            CategoryId = baseDto.CategoryId,
            CategoryName = baseDto.CategoryName,
            BrandId = baseDto.BrandId,
            BrandName = baseDto.BrandName,
            PetTypeId = baseDto.PetTypeId,
            PetTypeName = baseDto.PetTypeName,
            Name = baseDto.Name,
            Slug = baseDto.Slug,
            ShortDescription = baseDto.ShortDescription,
            Description = p.Description,
            BasePrice = baseDto.BasePrice,
            CompareAtPrice = baseDto.CompareAtPrice,
            DiscountPercentage = baseDto.DiscountPercentage,
            Sku = baseDto.Sku,
            Barcode = p.Barcode,
            StockQuantity = baseDto.StockQuantity,
            Weight = p.Weight,
            Status = baseDto.Status,
            IsFeatured = baseDto.IsFeatured,
            PrimaryImageUrl = baseDto.PrimaryImageUrl,
            AverageRating = baseDto.AverageRating,
            ReviewCount = baseDto.ReviewCount,
            TargetLifeStage = baseDto.TargetLifeStage,
            TargetBreedSize = p.TargetBreedSize,
            FoodType = baseDto.FoodType,
            HealthConcern = baseDto.HealthConcern,
            Images = p.Images?.Select(i => new ProductImageDto
            {
                Id = i.Id,
                ImageUrl = i.ImageUrl,
                AltText = i.AltText,
                SortOrder = i.SortOrder,
                IsPrimary = i.IsPrimary
            }).ToList() ?? new(),
            Variants = p.Variants?.Select(v => new ProductVariantDto
            {
                Id = v.Id,
                VariantName = v.VariantName,
                Sku = v.Sku,
                Price = v.Price,
                CompareAtPrice = v.CompareAtPrice,
                StockQuantity = v.StockQuantity,
                Weight = v.Weight,
                Status = v.Status
            }).ToList() ?? new()
        };
    }

    private static string GenerateSlug(string text)
    {
        var slug = text.ToLowerInvariant().Trim();
        slug = System.Text.RegularExpressions.Regex.Replace(slug, @"[^a-z0-9\s-]", "");
        slug = System.Text.RegularExpressions.Regex.Replace(slug, @"\s+", " ").Trim();
        slug = slug.Replace(" ", "-");
        return slug;
    }
}
