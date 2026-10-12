using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PetHaven.Api.Models;

[Table("wishlists")]
public class Wishlist
{
    [Key]
    [Column("id")]
    public ulong Id { get; set; }

    [Required]
    [Column("user_id")]
    public ulong UserId { get; set; }

    [Column("created_at")]
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    [Column("updated_at")]
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

    // Navigation properties
    [ForeignKey(nameof(UserId))]
    public virtual User User { get; set; } = null!;

    public virtual ICollection<WishlistItem> Items { get; set; } = new List<WishlistItem>();
}
