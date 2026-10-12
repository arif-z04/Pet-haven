using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PetHaven.Api.Models;

[Table("product_variants")]
public class ProductVariant
{
    [Key]
    [Column("id")]
    public ulong Id { get; set; }

    [Required]
    [Column("product_id")]
    public ulong ProductId { get; set; }

    [Required]
    [MaxLength(100)]
    [Column("variant_name")]
    public string VariantName { get; set; } = string.Empty;

    [Required]
    [MaxLength(64)]
    [Column("sku")]
    public string Sku { get; set; } = string.Empty;

    [Required]
    [Column("price", TypeName = "decimal(10,2)")]
    public decimal Price { get; set; }

    [Column("compare_at_price", TypeName = "decimal(10,2)")]
    public decimal? CompareAtPrice { get; set; }

    [Column("cost_price", TypeName = "decimal(10,2)")]
    public decimal? CostPrice { get; set; }

    [Column("stock_quantity")]
    public int StockQuantity { get; set; }

    [Column("weight", TypeName = "decimal(8,2)")]
    public decimal? Weight { get; set; }

    [Required]
    [MaxLength(20)]
    [Column("status")]
    public string Status { get; set; } = "active";

    [Column("created_at")]
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    [Column("updated_at")]
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

    // Navigation properties
    [ForeignKey(nameof(ProductId))]
    public virtual Product Product { get; set; } = null!;

    public virtual ICollection<OrderItem> OrderItems { get; set; } = new List<OrderItem>();
    public virtual ICollection<InventoryTransaction> InventoryTransactions { get; set; } = new List<InventoryTransaction>();
}
