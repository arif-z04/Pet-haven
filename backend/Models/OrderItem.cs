using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PetHaven.Api.Models;

[Table("order_items")]
public class OrderItem
{
    [Key]
    [Column("id")]
    public ulong Id { get; set; }

    [Required]
    [Column("order_id")]
    public ulong OrderId { get; set; }

    [Required]
    [Column("product_id")]
    public ulong ProductId { get; set; }

    [Column("variant_id")]
    public ulong? VariantId { get; set; }

    [Required]
    [MaxLength(255)]
    [Column("product_name")]
    public string ProductName { get; set; } = string.Empty;

    [MaxLength(100)]
    [Column("variant_name")]
    public string? VariantName { get; set; }

    [Required]
    [MaxLength(64)]
    [Column("sku")]
    public string Sku { get; set; } = string.Empty;

    [Required]
    [Column("unit_price", TypeName = "decimal(10,2)")]
    public decimal UnitPrice { get; set; }

    [Required]
    [Column("quantity")]
    public int Quantity { get; set; }

    [Required]
    [Column("total_price", TypeName = "decimal(10,2)")]
    public decimal TotalPrice { get; set; }

    [Column("created_at")]
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    // Navigation properties
    [ForeignKey(nameof(OrderId))]
    public virtual Order Order { get; set; } = null!;

    [ForeignKey(nameof(ProductId))]
    public virtual Product Product { get; set; } = null!;

    [ForeignKey(nameof(VariantId))]
    public virtual ProductVariant? Variant { get; set; }
}
