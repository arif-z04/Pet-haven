using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PetHaven.Api.Models;

[Table("pet_profiles")]
public class PetProfile
{
    [Key]
    [Column("id")]
    public ulong Id { get; set; }

    [Required]
    [Column("user_id")]
    public ulong UserId { get; set; }

    [Required]
    [Column("pet_type_id")]
    public uint PetTypeId { get; set; }

    [Required]
    [MaxLength(100)]
    [Column("name")]
    public string Name { get; set; } = string.Empty;

    [MaxLength(100)]
    [Column("breed")]
    public string? Breed { get; set; }

    [Required]
    [MaxLength(10)]
    [Column("gender")]
    public string Gender { get; set; } = "unknown"; // male, female, unknown

    [Column("date_of_birth")]
    public DateTime? DateOfBirth { get; set; }

    [Column("weight", TypeName = "decimal(5,2)")]
    public decimal? Weight { get; set; }

    [MaxLength(500)]
    [Column("profile_image_url")]
    public string? ProfileImageUrl { get; set; }

    [MaxLength(255)]
    [Column("dietary_preferences")]
    public string? DietaryPreferences { get; set; }

    [Column("allergies_or_medical_notes")]
    public string? AllergiesOrMedicalNotes { get; set; }

    [Column("notes")]
    public string? Notes { get; set; }

    [Column("created_at")]
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    [Column("updated_at")]
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

    // Navigation properties
    [ForeignKey(nameof(UserId))]
    public virtual User User { get; set; } = null!;

    [ForeignKey(nameof(PetTypeId))]
    public virtual PetType PetType { get; set; } = null!;
}
