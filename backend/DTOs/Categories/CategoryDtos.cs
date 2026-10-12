namespace PetHaven.Api.DTOs.Categories;

public class CategoryDto
{
    public uint Id { get; set; }
    public uint? ParentId { get; set; }
    public uint? PetTypeId { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Slug { get; set; } = string.Empty;
    public string? Description { get; set; }
    public string? ImageUrl { get; set; }
    public string? IconUrl { get; set; }
    public int SortOrder { get; set; }
    public string Status { get; set; } = string.Empty;
    public int ProductCount { get; set; }
    public List<CategoryDto> SubCategories { get; set; } = new();
}
