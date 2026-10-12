using Microsoft.AspNetCore.Mvc;
using PetHaven.Api.DTOs.Common;
using PetHaven.Api.DTOs.Coupons;
using PetHaven.Api.Services.Interfaces;

namespace PetHaven.Api.Controllers;

[Route("api/[controller]")]
public class CouponsController : BaseApiController
{
    private readonly ICouponService _couponService;

    public CouponsController(ICouponService couponService)
    {
        _couponService = couponService;
    }

    /// <summary>
    /// Validate promotional coupon against cart rules, user usage limits, and expiration dates
    /// </summary>
    [HttpPost("validate")]
    [ProducesResponseType(typeof(ApiResponse<CouponValidationResultDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse<CouponValidationResultDto>), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ApiResponse<CouponValidationResultDto>), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> ValidateCoupon([FromBody] ValidateCouponDto dto)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ApiResponse<CouponValidationResultDto>.Fail("Invalid coupon payload.", 400));
        }

        var userId = GetCurrentUserId();
        var result = await _couponService.ValidateCouponAsync(userId, dto);
        return StatusCode(result.StatusCode, result);
    }
}
