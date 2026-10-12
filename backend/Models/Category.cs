using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PetHaven.Api.Models;

[Table("categories")]
public class Category
{
    [Key]
    [Column("id")]
    public uint Id { get; set; }

    [Column("parent_id")]
    public uint? ParentId { get; set; }

    [Column("pet_type_id")]
    public uint? PetTypeId { get; set; }

    [Required]
    [MaxLength(100)]
    [Column("name")]
    public string Name { get; set; } = string.Empty;

    [Required]
    [MaxLength(120)]
    [Column("slug")]
    public string Slug { get; set; } = string.Empty;

    [Column("description")]
    public string? Description { get; set; }

    [MaxLength(255)]
    [Column("image_url")]
    public string? ImageUrl { get; set; }

    [MaxLength(255)]
    [Column("icon_url")]
    public string? IconUrl { get; set; }

    [Column("sort_order")]
    public int SortOrder { get; set; }

    [MaxLength(20)]
    [Column("status")]
    public string Status { get; set; } = "active";

    [Column("created_at")]
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    [Column("updated_at")]
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

    // Navigation properties
    [ForeignKey(nameof(ParentId))]
    public virtual Category? Parent { get; set; }
    public virtual ICollection<Category> SubCategories { get; set; } = new List<Category>();

    [ForeignKey(nameof(PetTypeId))]
    public virtual PetType? PetType { get; set; }

    public virtual ICollection<Product> Products { get; set; } = new List<Product>();
}
