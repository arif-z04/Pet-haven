using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PetHaven.Api.Models;

[Table("product_questions")]
public class ProductQuestion
{
    [Key]
    [Column("id")]
    public ulong Id { get; set; }

    [Required]
    [Column("product_id")]
    public ulong ProductId { get; set; }

    [Required]
    [Column("user_id")]
    public ulong UserId { get; set; }

    [Required]
    [Column("question")]
    public string Question { get; set; } = string.Empty;

    [Column("answer")]
    public string? Answer { get; set; }

    [Column("answered_by")]
    public ulong? AnsweredBy { get; set; }

    [Column("answered_at")]
    public DateTime? AnsweredAt { get; set; }

    [Required]
    [MaxLength(20)]
    [Column("status")]
    public string Status { get; set; } = "pending"; // pending, answered, rejected

    [Column("created_at")]
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    [Column("updated_at")]
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

    // Navigation properties
    [ForeignKey(nameof(ProductId))]
    public virtual Product Product { get; set; } = null!;

    [ForeignKey(nameof(UserId))]
    public virtual User User { get; set; } = null!;

    [ForeignKey(nameof(AnsweredBy))]
    public virtual User? Answerer { get; set; }
}
