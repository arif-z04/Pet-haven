using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PetHaven.Api.Models;

[Table("product_images")]
public class ProductImage
{
    [Key]
    [Column("id")]
    public ulong Id { get; set; }

    [Required]
    [Column("product_id")]
    public ulong ProductId { get; set; }

    [Required]
    [MaxLength(500)]
    [Column("image_url")]
    public string ImageUrl { get; set; } = string.Empty;

    [MaxLength(255)]
    [Column("alt_text")]
    public string? AltText { get; set; }

    [Column("sort_order")]
    public int SortOrder { get; set; }

    [Column("is_primary")]
    public bool IsPrimary { get; set; }

    [Column("created_at")]
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    // Navigation property
    [ForeignKey(nameof(ProductId))]
    public virtual Product Product { get; set; } = null!;
}
