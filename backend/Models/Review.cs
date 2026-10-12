using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PetHaven.Api.Models;

[Table("reviews")]
public class Review
{
    [Key]
    [Column("id")]
    public ulong Id { get; set; }

    [Required]
    [Column("user_id")]
    public ulong UserId { get; set; }

    [Required]
    [Column("product_id")]
    public ulong ProductId { get; set; }

    [Column("order_id")]
    public ulong? OrderId { get; set; }

    [Required]
    [Range(1, 5)]
    [Column("rating")]
    public byte Rating { get; set; }

    [MaxLength(150)]
    [Column("title")]
    public string? Title { get; set; }

    [Column("comment")]
    public string? Comment { get; set; }

    [Required]
    [MaxLength(20)]
    [Column("status")]
    public string Status { get; set; } = "pending"; // pending, approved, rejected

    [Column("created_at")]
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    [Column("updated_at")]
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

    // Navigation properties
    [ForeignKey(nameof(UserId))]
    public virtual User User { get; set; } = null!;

    [ForeignKey(nameof(ProductId))]
    public virtual Product Product { get; set; } = null!;

    [ForeignKey(nameof(OrderId))]
    public virtual Order? Order { get; set; }
}
