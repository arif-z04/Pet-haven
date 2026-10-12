using Microsoft.EntityFrameworkCore;
using PetHaven.Api.Data;
using PetHaven.Api.DTOs.Common;
using PetHaven.Api.DTOs.Coupons;
using PetHaven.Api.Services.Interfaces;

namespace PetHaven.Api.Services.Implementations;

public class CouponService : ICouponService
{
    private readonly AppDbContext _context;

    public CouponService(AppDbContext context)
    {
        _context = context;
    }

    public async Task<ApiResponse<CouponValidationResultDto>> ValidateCouponAsync(ulong userId, ValidateCouponDto dto)
    {
        var code = dto.Code.Trim().ToUpperInvariant();
        var coupon = await _context.Coupons.FirstOrDefaultAsync(c => c.Code == code);

        if (coupon == null)
        {
            return ApiResponse<CouponValidationResultDto>.Fail("Coupon code does not exist.", 404);
        }

        // 1. Active status check
        if (!coupon.IsActive)
        {
            return ApiResponse<CouponValidationResultDto>.Ok(new CouponValidationResultDto
            {
                IsValid = false,
                Message = "This coupon is currently inactive."
            });
        }

        // 2. Date range & Expiry check
        var now = DateTime.UtcNow;
        if (now < coupon.StartDate || now > coupon.ExpiryDate)
        {
            return ApiResponse<CouponValidationResultDto>.Ok(new CouponValidationResultDto
            {
                IsValid = false,
                Message = "This coupon has expired or is not yet active."
            });
        }

        // 3. Minimum order amount check
        if (dto.OrderAmount < coupon.MinOrderAmount)
        {
            return ApiResponse<CouponValidationResultDto>.Ok(new CouponValidationResultDto
            {
                IsValid = false,
                Message = $"Minimum order amount of {coupon.MinOrderAmount:N2} BDT is required to apply this coupon."
            });
        }

        // 4. Platform-wide usage limit check
        if (coupon.UsageLimit.HasValue && coupon.TimesUsed >= coupon.UsageLimit.Value)
        {
            return ApiResponse<CouponValidationResultDto>.Ok(new CouponValidationResultDto
            {
                IsValid = false,
                Message = "This coupon has reached its maximum total redemption limit."
            });
        }

        // 5. Per-user usage limit check
        if (userId > 0)
        {
            var userRedemptions = await _context.CouponUsages
                .CountAsync(cu => cu.CouponId == coupon.Id && cu.UserId == userId);

            if (userRedemptions >= coupon.PerUserLimit)
            {
                return ApiResponse<CouponValidationResultDto>.Ok(new CouponValidationResultDto
                {
                    IsValid = false,
                    Message = $"You have already reached the maximum usage limit ({coupon.PerUserLimit}) for this coupon."
                });
            }
        }

        // Calculate discount amount
        decimal discountCalculated = 0.00m;
        if (coupon.DiscountType == "percentage")
        {
            discountCalculated = dto.OrderAmount * (coupon.DiscountValue / 100m);
            if (coupon.MaxDiscountAmount.HasValue && discountCalculated > coupon.MaxDiscountAmount.Value)
            {
                discountCalculated = coupon.MaxDiscountAmount.Value;
            }
        }
        else // fixed_amount
        {
            discountCalculated = coupon.DiscountValue;
        }

        discountCalculated = Math.Min(discountCalculated, dto.OrderAmount);
        var finalAmount = Math.Max(0, dto.OrderAmount - discountCalculated);

        return ApiResponse<CouponValidationResultDto>.Ok(new CouponValidationResultDto
        {
            IsValid = true,
            Message = "Coupon applied successfully!",
            Code = coupon.Code,
            DiscountType = coupon.DiscountType,
            DiscountValue = coupon.DiscountValue,
            CalculatedDiscount = Math.Round(discountCalculated, 2),
            FinalAmount = Math.Round(finalAmount, 2)
        });
    }
}
