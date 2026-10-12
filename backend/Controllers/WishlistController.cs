using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PetHaven.Api.DTOs.Common;
using PetHaven.Api.DTOs.Wishlist;
using PetHaven.Api.Services.Interfaces;

namespace PetHaven.Api.Controllers;

[Authorize]
[Route("api/[controller]")]
public class WishlistController : BaseApiController
{
    private readonly IWishlistService _wishlistService;

    public WishlistController(IWishlistService wishlistService)
    {
        _wishlistService = wishlistService;
    }

    /// <summary>
    /// Retrieve authenticated customer's wishlist items
    /// </summary>
    [HttpGet]
    [ProducesResponseType(typeof(ApiResponse<WishlistDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetWishlist()
    {
        var userId = GetCurrentUserId();
        var result = await _wishlistService.GetWishlistAsync(userId);
        return Ok(result);
    }

    /// <summary>
    /// Add a product to the customer's wishlist
    /// </summary>
    [HttpPost("{productId:long}")]
    [ProducesResponseType(typeof(ApiResponse<bool>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse<bool>), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> AddToWishlist(ulong productId)
    {
        var userId = GetCurrentUserId();
        var result = await _wishlistService.AddToWishlistAsync(userId, productId);
        return StatusCode(result.StatusCode, result);
    }

    /// <summary>
    /// Remove a product from the customer's wishlist
    /// </summary>
    [HttpDelete("{productId:long}")]
    [ProducesResponseType(typeof(ApiResponse<bool>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse<bool>), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> RemoveFromWishlist(ulong productId)
    {
        var userId = GetCurrentUserId();
        var result = await _wishlistService.RemoveFromWishlistAsync(userId, productId);
        return StatusCode(result.StatusCode, result);
    }
}
