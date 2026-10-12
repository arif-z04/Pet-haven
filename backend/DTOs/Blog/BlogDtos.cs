using System.ComponentModel.DataAnnotations;

namespace PetHaven.Api.DTOs.Blog;

public class BlogPostDto
{
    public ulong Id { get; set; }
    public uint CategoryId { get; set; }
    public string CategoryName { get; set; } = string.Empty;
    public ulong AuthorId { get; set; }
    public string AuthorName { get; set; } = string.Empty;
    public uint? PetTypeId { get; set; }
    public string? PetTypeName { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Slug { get; set; } = string.Empty;
    public string Content { get; set; } = string.Empty;
    public string? Excerpt { get; set; }
    public string? FeaturedImage { get; set; }
    public string Status { get; set; } = string.Empty;
    public DateTime? PublishedAt { get; set; }
    public DateTime CreatedAt { get; set; }
}

public class CreateBlogPostDto
{
    [Required]
    public uint CategoryId { get; set; }

    public uint? PetTypeId { get; set; }

    [Required]
    [MaxLength(255)]
    public string Title { get; set; } = string.Empty;

    [Required]
    public string Content { get; set; } = string.Empty;

    [MaxLength(500)]
    public string? Excerpt { get; set; }

    public string? FeaturedImage { get; set; }

    public string Status { get; set; } = "draft";
}

public class UpdateBlogPostDto : CreateBlogPostDto
{
}
