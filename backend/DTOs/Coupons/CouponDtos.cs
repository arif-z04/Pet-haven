using System.ComponentModel.DataAnnotations;

namespace PetHaven.Api.DTOs.Coupons;

public class ValidateCouponDto
{
    [Required]
    public string Code { get; set; } = string.Empty;

    [Required]
    [Range(0, 1000000)]
    public decimal OrderAmount { get; set; }
}

public class CouponValidationResultDto
{
    public bool IsValid { get; set; }
    public string Message { get; set; } = string.Empty;
    public string? Code { get; set; }
    public string? DiscountType { get; set; }
    public decimal DiscountValue { get; set; }
    public decimal CalculatedDiscount { get; set; }
    public decimal FinalAmount { get; set; }
}
