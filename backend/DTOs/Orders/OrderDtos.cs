using System.ComponentModel.DataAnnotations;

namespace PetHaven.Api.DTOs.Orders;

public class CreateOrderDto
{
    public ulong? AddressId { get; set; }

    [Required]
    [MaxLength(100)]
    public string ShippingFullName { get; set; } = string.Empty;

    [Required]
    [MaxLength(20)]
    public string ShippingPhone { get; set; } = string.Empty;

    [Required]
    [MaxLength(50)]
    public string ShippingDivision { get; set; } = string.Empty;

    [Required]
    [MaxLength(50)]
    public string ShippingDistrict { get; set; } = string.Empty;

    [Required]
    [MaxLength(100)]
    public string ShippingArea { get; set; } = string.Empty;

    [Required]
    public string ShippingAddressLine { get; set; } = string.Empty;

    public string? ShippingPostalCode { get; set; }

    public string? CouponCode { get; set; }

    [Required]
    public string PaymentMethod { get; set; } = "cash_on_delivery";

    public string? CustomerNote { get; set; }

    // Can order directly with items or pull from active cart
    public List<OrderItemRequestDto>? Items { get; set; }
}

public class OrderItemRequestDto
{
    [Required]
    public ulong ProductId { get; set; }

    public ulong? VariantId { get; set; }

    [Required]
    [Range(1, 100)]
    public int Quantity { get; set; }
}

public class OrderDto
{
    public ulong Id { get; set; }
    public string OrderNumber { get; set; } = string.Empty;
    public ulong UserId { get; set; }
    public string CustomerName { get; set; } = string.Empty;
    public string CustomerEmail { get; set; } = string.Empty;

    public string ShippingFullName { get; set; } = string.Empty;
    public string ShippingPhone { get; set; } = string.Empty;
    public string ShippingDivision { get; set; } = string.Empty;
    public string ShippingDistrict { get; set; } = string.Empty;
    public string ShippingArea { get; set; } = string.Empty;
    public string ShippingAddressLine { get; set; } = string.Empty;
    public string? ShippingPostalCode { get; set; }

    public decimal Subtotal { get; set; }
    public decimal DiscountAmount { get; set; }
    public decimal DeliveryCharge { get; set; }
    public decimal TotalAmount { get; set; }
    public string? CouponCode { get; set; }

    public string PaymentMethod { get; set; } = string.Empty;
    public string PaymentStatus { get; set; } = string.Empty;
    public string OrderStatus { get; set; } = string.Empty;
    public string? PaymentTransactionId { get; set; }
    public string? CustomerNote { get; set; }
    public string? AdminNote { get; set; }
    public DateTime CreatedAt { get; set; }

    public List<OrderItemDto> Items { get; set; } = new();
}

public class OrderItemDto
{
    public ulong Id { get; set; }
    public ulong ProductId { get; set; }
    public ulong? VariantId { get; set; }
    public string ProductName { get; set; } = string.Empty;
    public string? VariantName { get; set; }
    public string Sku { get; set; } = string.Empty;
    public decimal UnitPrice { get; set; }
    public int Quantity { get; set; }
    public decimal TotalPrice { get; set; }
}

public class UpdateOrderStatusDto
{
    [Required]
    public string OrderStatus { get; set; } = string.Empty; // pending, confirmed, processing, shipped, delivered, cancelled, returned

    public string? PaymentStatus { get; set; }

    public string? AdminNote { get; set; }
}
