using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PetHaven.Api.Models;

[Table("coupon_usages")]
public class CouponUsage
{
    [Key]
    [Column("id")]
    public ulong Id { get; set; }

    [Required]
    [Column("coupon_id")]
    public uint CouponId { get; set; }

    [Required]
    [Column("user_id")]
    public ulong UserId { get; set; }

    [Required]
    [Column("order_id")]
    public ulong OrderId { get; set; }

    [Required]
    [Column("discount_applied", TypeName = "decimal(10,2)")]
    public decimal DiscountApplied { get; set; }

    [Column("used_at")]
    public DateTime UsedAt { get; set; } = DateTime.UtcNow;

    // Navigation properties
    [ForeignKey(nameof(CouponId))]
    public virtual Coupon Coupon { get; set; } = null!;

    [ForeignKey(nameof(UserId))]
    public virtual User User { get; set; } = null!;

    [ForeignKey(nameof(OrderId))]
    public virtual Order Order { get; set; } = null!;
}
