namespace PetHaven.Api.DTOs.PetTypes;

public class PetTypeDto
{
    public uint Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Slug { get; set; } = string.Empty;
    public string? Description { get; set; }
    public string? IconUrl { get; set; }
    public int SortOrder { get; set; }
    public string Status { get; set; } = string.Empty;
    public int ProductCount { get; set; }
}
