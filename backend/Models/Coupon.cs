using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PetHaven.Api.Models;

[Table("coupons")]
public class Coupon
{
    [Key]
    [Column("id")]
    public uint Id { get; set; }

    [Required]
    [MaxLength(50)]
    [Column("code")]
    public string Code { get; set; } = string.Empty;

    [MaxLength(255)]
    [Column("description")]
    public string? Description { get; set; }

    [Required]
    [MaxLength(20)]
    [Column("discount_type")]
    public string DiscountType { get; set; } = "percentage"; // percentage, fixed_amount

    [Required]
    [Column("discount_value", TypeName = "decimal(10,2)")]
    public decimal DiscountValue { get; set; }

    [Column("min_order_amount", TypeName = "decimal(10,2)")]
    public decimal MinOrderAmount { get; set; } = 0.00m;

    [Column("max_discount_amount", TypeName = "decimal(10,2)")]
    public decimal? MaxDiscountAmount { get; set; }

    [Required]
    [Column("start_date")]
    public DateTime StartDate { get; set; }

    [Required]
    [Column("expiry_date")]
    public DateTime ExpiryDate { get; set; }

    [Column("usage_limit")]
    public int? UsageLimit { get; set; }

    [Column("per_user_limit")]
    public int PerUserLimit { get; set; } = 1;

    [Column("times_used")]
    public int TimesUsed { get; set; } = 0;

    [Column("is_active")]
    public bool IsActive { get; set; } = true;

    [Column("created_at")]
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    [Column("updated_at")]
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

    // Navigation properties
    public virtual ICollection<CouponUsage> Usages { get; set; } = new List<CouponUsage>();
}
