using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PetHaven.Api.Models;

[Table("blog_posts")]
public class BlogPost
{
    [Key]
    [Column("id")]
    public ulong Id { get; set; }

    [Required]
    [Column("category_id")]
    public uint CategoryId { get; set; }

    [Required]
    [Column("author_id")]
    public ulong AuthorId { get; set; }

    [Column("pet_type_id")]
    public uint? PetTypeId { get; set; }

    [Required]
    [MaxLength(255)]
    [Column("title")]
    public string Title { get; set; } = string.Empty;

    [Required]
    [MaxLength(255)]
    [Column("slug")]
    public string Slug { get; set; } = string.Empty;

    [Required]
    [Column("content")]
    public string Content { get; set; } = string.Empty;

    [MaxLength(500)]
    [Column("excerpt")]
    public string? Excerpt { get; set; }

    [MaxLength(500)]
    [Column("featured_image")]
    public string? FeaturedImage { get; set; }

    [Required]
    [MaxLength(20)]
    [Column("status")]
    public string Status { get; set; } = "draft"; // draft, published, archived

    [Column("published_at")]
    public DateTime? PublishedAt { get; set; }

    [Column("created_at")]
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    [Column("updated_at")]
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

    // Navigation properties
    [ForeignKey(nameof(CategoryId))]
    public virtual BlogCategory Category { get; set; } = null!;

    [ForeignKey(nameof(AuthorId))]
    public virtual User Author { get; set; } = null!;

    [ForeignKey(nameof(PetTypeId))]
    public virtual PetType? PetType { get; set; }
}
