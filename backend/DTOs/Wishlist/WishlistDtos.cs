namespace PetHaven.Api.DTOs.Wishlist;

public class WishlistDto
{
    public ulong Id { get; set; }
    public List<WishlistItemDto> Items { get; set; } = new();
}

public class WishlistItemDto
{
    public ulong Id { get; set; }
    public ulong ProductId { get; set; }
    public string ProductName { get; set; } = string.Empty;
    public string ProductSlug { get; set; } = string.Empty;
    public decimal BasePrice { get; set; }
    public decimal? CompareAtPrice { get; set; }
    public string? ImageUrl { get; set; }
    public bool InStock { get; set; }
    public DateTime AddedAt { get; set; }
}
