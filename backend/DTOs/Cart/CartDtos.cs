using System.ComponentModel.DataAnnotations;

namespace PetHaven.Api.DTOs.Cart;

public class CartDto
{
    public ulong Id { get; set; }
    public List<CartItemDto> Items { get; set; } = new();
    public int TotalItems => Items.Sum(i => i.Quantity);
    public decimal Subtotal => Items.Sum(i => i.TotalPrice);
}

public class CartItemDto
{
    public ulong Id { get; set; }
    public ulong ProductId { get; set; }
    public string ProductName { get; set; } = string.Empty;
    public string ProductSlug { get; set; } = string.Empty;
    public string? ImageUrl { get; set; }
    public ulong? VariantId { get; set; }
    public string? VariantName { get; set; }
    public string Sku { get; set; } = string.Empty;
    public decimal UnitPrice { get; set; }
    public int Quantity { get; set; }
    public decimal TotalPrice => UnitPrice * Quantity;
    public int AvailableStock { get; set; }
}

public class AddToCartDto
{
    [Required]
    public ulong ProductId { get; set; }

    public ulong? VariantId { get; set; }

    [Required]
    [Range(1, 100, ErrorMessage = "Quantity must be between 1 and 100.")]
    public int Quantity { get; set; } = 1;
}

public class UpdateCartItemDto
{
    [Required]
    [Range(1, 100, ErrorMessage = "Quantity must be between 1 and 100.")]
    public int Quantity { get; set; }
}
