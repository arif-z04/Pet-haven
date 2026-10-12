using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PetHaven.Api.Models;

[Table("wishlist_items")]
public class WishlistItem
{
    [Key]
    [Column("id")]
    public ulong Id { get; set; }

    [Required]
    [Column("wishlist_id")]
    public ulong WishlistId { get; set; }

    [Required]
    [Column("product_id")]
    public ulong ProductId { get; set; }

    [Column("created_at")]
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    // Navigation properties
    [ForeignKey(nameof(WishlistId))]
    public virtual Wishlist Wishlist { get; set; } = null!;

    [ForeignKey(nameof(ProductId))]
    public virtual Product Product { get; set; } = null!;
}
