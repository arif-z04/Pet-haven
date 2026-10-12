using System.ComponentModel.DataAnnotations;

namespace PetHaven.Api.DTOs.Reviews;

public class CreateReviewDto
{
    [Required]
    [Range(1, 5, ErrorMessage = "Rating must be between 1 and 5 stars.")]
    public byte Rating { get; set; }

    [MaxLength(150)]
    public string? Title { get; set; }

    [MaxLength(2000)]
    public string? Comment { get; set; }
}

public class UpdateReviewDto
{
    [Required]
    [Range(1, 5)]
    public byte Rating { get; set; }

    [MaxLength(150)]
    public string? Title { get; set; }

    [MaxLength(2000)]
    public string? Comment { get; set; }
}

public class ReviewDto
{
    public ulong Id { get; set; }
    public ulong ProductId { get; set; }
    public ulong UserId { get; set; }
    public string UserName { get; set; } = string.Empty;
    public byte Rating { get; set; }
    public string? Title { get; set; }
    public string? Comment { get; set; }
    public string Status { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; }
}
