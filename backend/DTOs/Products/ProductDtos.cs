using System.ComponentModel.DataAnnotations;
using PetHaven.Api.DTOs.Common;

namespace PetHaven.Api.DTOs.Products;

public class ProductQueryParameters : PaginationParams
{
    public uint? CategoryId { get; set; }
    public string? CategorySlug { get; set; }
    public uint? PetTypeId { get; set; }
    public string? PetTypeSlug { get; set; }
    public uint? BrandId { get; set; }
    public string? BrandSlug { get; set; }
    public decimal? MinPrice { get; set; }
    public decimal? MaxPrice { get; set; }
    public double? MinRating { get; set; }
    public bool? InStockOnly { get; set; }
    public bool? OnSaleOnly { get; set; }
    public bool? FeaturedOnly { get; set; }
    public string? Search { get; set; }
    
    // Recommendation-related filters
    public string? LifeStage { get; set; }
    public string? FoodType { get; set; }
    public string? HealthConcern { get; set; }

    // Sorting: newest, price_asc, price_desc, rating, popularity
    public string? SortBy { get; set; } = "newest";
}

public class ProductDto
{
    public ulong Id { get; set; }
    public uint CategoryId { get; set; }
    public string CategoryName { get; set; } = string.Empty;
    public uint? BrandId { get; set; }
    public string? BrandName { get; set; }
    public uint PetTypeId { get; set; }
    public string PetTypeName { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public string Slug { get; set; } = string.Empty;
    public string? ShortDescription { get; set; }
    public decimal BasePrice { get; set; }
    public decimal? CompareAtPrice { get; set; }
    public decimal? DiscountPercentage { get; set; }
    public string Sku { get; set; } = string.Empty;
    public int StockQuantity { get; set; }
    public bool InStock => StockQuantity > 0;
    public string Status { get; set; } = string.Empty;
    public bool IsFeatured { get; set; }
    public string? PrimaryImageUrl { get; set; }
    public double AverageRating { get; set; }
    public int ReviewCount { get; set; }
    public string TargetLifeStage { get; set; } = string.Empty;
    public string FoodType { get; set; } = string.Empty;
    public string? HealthConcern { get; set; }
}

public class ProductDetailDto : ProductDto
{
    public string? Description { get; set; }
    public string? Barcode { get; set; }
    public decimal? Weight { get; set; }
    public string TargetBreedSize { get; set; } = string.Empty;
    public List<ProductImageDto> Images { get; set; } = new();
    public List<ProductVariantDto> Variants { get; set; } = new();
}

public class ProductImageDto
{
    public ulong Id { get; set; }
    public string ImageUrl { get; set; } = string.Empty;
    public string? AltText { get; set; }
    public int SortOrder { get; set; }
    public bool IsPrimary { get; set; }
}

public class ProductVariantDto
{
    public ulong Id { get; set; }
    public string VariantName { get; set; } = string.Empty;
    public string Sku { get; set; } = string.Empty;
    public decimal Price { get; set; }
    public decimal? CompareAtPrice { get; set; }
    public int StockQuantity { get; set; }
    public decimal? Weight { get; set; }
    public string Status { get; set; } = string.Empty;
}

public class CreateProductDto
{
    [Required]
    public uint CategoryId { get; set; }

    public uint? BrandId { get; set; }

    [Required]
    public uint PetTypeId { get; set; }

    [Required]
    [MaxLength(255)]
    public string Name { get; set; } = string.Empty;

    [MaxLength(500)]
    public string? ShortDescription { get; set; }

    public string? Description { get; set; }

    [Required]
    [Range(0, 1000000)]
    public decimal BasePrice { get; set; }

    [Range(0, 1000000)]
    public decimal? CompareAtPrice { get; set; }

    [Range(0, 1000000)]
    public decimal? CostPrice { get; set; }

    [Required]
    [MaxLength(64)]
    public string Sku { get; set; } = string.Empty;

    [MaxLength(64)]
    public string? Barcode { get; set; }

    [Range(0, 100000)]
    public int StockQuantity { get; set; }

    [Range(0, 10000)]
    public int LowStockThreshold { get; set; } = 5;

    public decimal? Weight { get; set; }

    public string Status { get; set; } = "active";
    public bool IsFeatured { get; set; }
    public string TargetLifeStage { get; set; } = "all";
    public string TargetBreedSize { get; set; } = "all";
    public string FoodType { get; set; } = "not_applicable";
    public string? HealthConcern { get; set; }

    public List<string>? ImageUrls { get; set; }
}

public class UpdateProductDto : CreateProductDto
{
}
