using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PetHaven.Api.Models;

[Table("products")]
public class Product
{
    [Key]
    [Column("id")]
    public ulong Id { get; set; }

    [Required]
    [Column("category_id")]
    public uint CategoryId { get; set; }

    [Column("brand_id")]
    public uint? BrandId { get; set; }

    [Required]
    [Column("pet_type_id")]
    public uint PetTypeId { get; set; }

    [Required]
    [MaxLength(255)]
    [Column("name")]
    public string Name { get; set; } = string.Empty;

    [Required]
    [MaxLength(255)]
    [Column("slug")]
    public string Slug { get; set; } = string.Empty;

    [MaxLength(500)]
    [Column("short_description")]
    public string? ShortDescription { get; set; }

    [Column("description")]
    public string? Description { get; set; }

    [Required]
    [Column("base_price", TypeName = "decimal(10,2)")]
    public decimal BasePrice { get; set; }

    [Column("compare_at_price", TypeName = "decimal(10,2)")]
    public decimal? CompareAtPrice { get; set; }

    [Column("cost_price", TypeName = "decimal(10,2)")]
    public decimal? CostPrice { get; set; }

    [Required]
    [MaxLength(64)]
    [Column("sku")]
    public string Sku { get; set; } = string.Empty;

    [MaxLength(64)]
    [Column("barcode")]
    public string? Barcode { get; set; }

    [Column("stock_quantity")]
    public int StockQuantity { get; set; }

    [Column("low_stock_threshold")]
    public int LowStockThreshold { get; set; } = 5;

    [Column("weight", TypeName = "decimal(8,2)")]
    public decimal? Weight { get; set; }

    [Required]
    [MaxLength(20)]
    [Column("status")]
    public string Status { get; set; } = "active"; // active, inactive, draft, archived

    [Column("is_featured")]
    public bool IsFeatured { get; set; }

    [Required]
    [MaxLength(20)]
    [Column("target_life_stage")]
    public string TargetLifeStage { get; set; } = "all"; // all, kitten_puppy, adult, senior

    [Required]
    [MaxLength(20)]
    [Column("target_breed_size")]
    public string TargetBreedSize { get; set; } = "all"; // all, small, medium, large, giant

    [Required]
    [MaxLength(20)]
    [Column("food_type")]
    public string FoodType { get; set; } = "not_applicable"; // dry, wet, treat, raw, not_applicable

    [MaxLength(100)]
    [Column("health_concern")]
    public string? HealthConcern { get; set; }

    [Column("created_at")]
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    [Column("updated_at")]
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

    // Navigation properties
    [ForeignKey(nameof(CategoryId))]
    public virtual Category Category { get; set; } = null!;

    [ForeignKey(nameof(BrandId))]
    public virtual Brand? Brand { get; set; }

    [ForeignKey(nameof(PetTypeId))]
    public virtual PetType PetType { get; set; } = null!;

    public virtual ICollection<ProductImage> Images { get; set; } = new List<ProductImage>();
    public virtual ICollection<ProductVariant> Variants { get; set; } = new List<ProductVariant>();
    public virtual ICollection<Review> Reviews { get; set; } = new List<Review>();
    public virtual ICollection<ProductQuestion> Questions { get; set; } = new List<ProductQuestion>();
    public virtual ICollection<OrderItem> OrderItems { get; set; } = new List<OrderItem>();
    public virtual ICollection<InventoryTransaction> InventoryTransactions { get; set; } = new List<InventoryTransaction>();
}
