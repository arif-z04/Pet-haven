using System.ComponentModel.DataAnnotations;

namespace PetHaven.Api.DTOs.Pets;

public class CreatePetProfileDto
{
    [Required]
    public uint PetTypeId { get; set; }

    [Required]
    [MaxLength(100)]
    public string Name { get; set; } = string.Empty;

    [MaxLength(100)]
    public string? Breed { get; set; }

    [Required]
    [MaxLength(10)]
    public string Gender { get; set; } = "unknown"; // male, female, unknown

    public DateTime? DateOfBirth { get; set; }

    [Range(0, 200)]
    public decimal? Weight { get; set; }

    public string? ProfileImageUrl { get; set; }

    [MaxLength(255)]
    public string? DietaryPreferences { get; set; }

    public string? AllergiesOrMedicalNotes { get; set; }

    public string? Notes { get; set; }
}

public class UpdatePetProfileDto : CreatePetProfileDto
{
}

public class PetProfileDto
{
    public ulong Id { get; set; }
    public ulong UserId { get; set; }
    public uint PetTypeId { get; set; }
    public string PetTypeName { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public string? Breed { get; set; }
    public string Gender { get; set; } = string.Empty;
    public DateTime? DateOfBirth { get; set; }
    public decimal? Weight { get; set; }
    public string? ProfileImageUrl { get; set; }
    public string? DietaryPreferences { get; set; }
    public string? AllergiesOrMedicalNotes { get; set; }
    public string? Notes { get; set; }
    public int? AgeInMonths
    {
        get
        {
            if (!DateOfBirth.HasValue) return null;
            var today = DateTime.UtcNow;
            return ((today.Year - DateOfBirth.Value.Year) * 12) + (today.Month - DateOfBirth.Value.Month);
        }
    }
}
