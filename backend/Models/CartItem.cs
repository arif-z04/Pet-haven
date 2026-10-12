using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PetHaven.Api.Models;

[Table("cart_items")]
public class CartItem
{
    [Key]
    [Column("id")]
    public ulong Id { get; set; }

    [Required]
    [Column("cart_id")]
    public ulong CartId { get; set; }

    [Required]
    [Column("product_id")]
    public ulong ProductId { get; set; }

    [Column("variant_id")]
    public ulong? VariantId { get; set; }

    [Required]
    [Column("quantity")]
    public int Quantity { get; set; } = 1;

    [Column("created_at")]
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    [Column("updated_at")]
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

    // Navigation properties
    [ForeignKey(nameof(CartId))]
    public virtual Cart Cart { get; set; } = null!;

    [ForeignKey(nameof(ProductId))]
    public virtual Product Product { get; set; } = null!;

    [ForeignKey(nameof(VariantId))]
    public virtual ProductVariant? Variant { get; set; }
}
