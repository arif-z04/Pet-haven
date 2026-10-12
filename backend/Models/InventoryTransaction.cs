using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PetHaven.Api.Models;

[Table("inventory_transactions")]
public class InventoryTransaction
{
    [Key]
    [Column("id")]
    public ulong Id { get; set; }

    [Required]
    [Column("product_id")]
    public ulong ProductId { get; set; }

    [Column("variant_id")]
    public ulong? VariantId { get; set; }

    [Required]
    [MaxLength(20)]
    [Column("transaction_type")]
    public string TransactionType { get; set; } = "purchase"; // purchase, sale, adjustment, return

    [Required]
    [Column("quantity")]
    public int Quantity { get; set; }

    [Required]
    [Column("previous_stock")]
    public int PreviousStock { get; set; }

    [Required]
    [Column("new_stock")]
    public int NewStock { get; set; }

    [Column("reference_order_id")]
    public ulong? ReferenceOrderId { get; set; }

    [MaxLength(255)]
    [Column("note")]
    public string? Note { get; set; }

    [Column("created_at")]
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    // Navigation properties
    [ForeignKey(nameof(ProductId))]
    public virtual Product Product { get; set; } = null!;

    [ForeignKey(nameof(VariantId))]
    public virtual ProductVariant? Variant { get; set; }

    [ForeignKey(nameof(ReferenceOrderId))]
    public virtual Order? ReferenceOrder { get; set; }
}
