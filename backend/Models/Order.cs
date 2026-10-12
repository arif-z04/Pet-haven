using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PetHaven.Api.Models;

[Table("orders")]
public class Order
{
    [Key]
    [Column("id")]
    public ulong Id { get; set; }

    [Required]
    [Column("user_id")]
    public ulong UserId { get; set; }

    [Required]
    [MaxLength(32)]
    [Column("order_number")]
    public string OrderNumber { get; set; } = string.Empty;

    [Column("address_id")]
    public ulong? AddressId { get; set; }

    // Shipping Snapshot
    [Required]
    [MaxLength(100)]
    [Column("shipping_full_name")]
    public string ShippingFullName { get; set; } = string.Empty;

    [Required]
    [MaxLength(20)]
    [Column("shipping_phone")]
    public string ShippingPhone { get; set; } = string.Empty;

    [Required]
    [MaxLength(50)]
    [Column("shipping_division")]
    public string ShippingDivision { get; set; } = string.Empty;

    [Required]
    [MaxLength(50)]
    [Column("shipping_district")]
    public string ShippingDistrict { get; set; } = string.Empty;

    [Required]
    [MaxLength(100)]
    [Column("shipping_area")]
    public string ShippingArea { get; set; } = string.Empty;

    [Required]
    [Column("shipping_address_line")]
    public string ShippingAddressLine { get; set; } = string.Empty;

    [MaxLength(20)]
    [Column("shipping_postal_code")]
    public string? ShippingPostalCode { get; set; }

    // Financial calculations (in BDT)
    [Required]
    [Column("subtotal", TypeName = "decimal(10,2)")]
    public decimal Subtotal { get; set; }

    [Column("discount_amount", TypeName = "decimal(10,2)")]
    public decimal DiscountAmount { get; set; } = 0.00m;

    [Column("delivery_charge", TypeName = "decimal(10,2)")]
    public decimal DeliveryCharge { get; set; } = 0.00m;

    [Required]
    [Column("total_amount", TypeName = "decimal(10,2)")]
    public decimal TotalAmount { get; set; }

    [MaxLength(50)]
    [Column("coupon_code")]
    public string? CouponCode { get; set; }

    // Payment & Status
    [Required]
    [MaxLength(30)]
    [Column("payment_method")]
    public string PaymentMethod { get; set; } = "cash_on_delivery";

    [Required]
    [MaxLength(20)]
    [Column("payment_status")]
    public string PaymentStatus { get; set; } = "pending";

    [Required]
    [MaxLength(20)]
    [Column("order_status")]
    public string OrderStatus { get; set; } = "pending";

    [MaxLength(100)]
    [Column("payment_transaction_id")]
    public string? PaymentTransactionId { get; set; }

    [Column("customer_note")]
    public string? CustomerNote { get; set; }

    [Column("admin_note")]
    public string? AdminNote { get; set; }

    [Column("created_at")]
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    [Column("updated_at")]
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

    // Navigation properties
    [ForeignKey(nameof(UserId))]
    public virtual User User { get; set; } = null!;

    [ForeignKey(nameof(AddressId))]
    public virtual CustomerAddress? Address { get; set; }

    public virtual ICollection<OrderItem> OrderItems { get; set; } = new List<OrderItem>();
    public virtual ICollection<CouponUsage> CouponUsages { get; set; } = new List<CouponUsage>();
    public virtual ICollection<InventoryTransaction> InventoryTransactions { get; set; } = new List<InventoryTransaction>();
}
