using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PetHaven.Api.DTOs.Cart;
using PetHaven.Api.DTOs.Common;
using PetHaven.Api.Services.Interfaces;

namespace PetHaven.Api.Controllers;

[Authorize]
[Route("api/[controller]")]
public class CartController : BaseApiController
{
    private readonly ICartService _cartService;

    public CartController(ICartService cartService)
    {
        _cartService = cartService;
    }

    /// <summary>
    /// Retrieve the authenticated customer's current shopping cart
    /// </summary>
    [HttpGet]
    [ProducesResponseType(typeof(ApiResponse<CartDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetCart()
    {
        var userId = GetCurrentUserId();
        var result = await _cartService.GetCartAsync(userId);
        return Ok(result);
    }

    /// <summary>
    /// Add an item (product or variant) to the cart
    /// </summary>
    [HttpPost("items")]
    [ProducesResponseType(typeof(ApiResponse<CartDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse<CartDto>), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> AddToCart([FromBody] AddToCartDto dto)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ApiResponse<CartDto>.Fail("Invalid cart payload.", 400));
        }

        var userId = GetCurrentUserId();
        var result = await _cartService.AddToCartAsync(userId, dto);
        return StatusCode(result.StatusCode, result);
    }

    /// <summary>
    /// Update the quantity of a cart line item
    /// </summary>
    [HttpPut("items/{id:long}")]
    [ProducesResponseType(typeof(ApiResponse<CartDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse<CartDto>), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ApiResponse<CartDto>), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> UpdateCartItem(ulong id, [FromBody] UpdateCartItemDto dto)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ApiResponse<CartDto>.Fail("Invalid quantity payload.", 400));
        }

        var userId = GetCurrentUserId();
        var result = await _cartService.UpdateCartItemAsync(userId, id, dto);
        return StatusCode(result.StatusCode, result);
    }

    /// <summary>
    /// Remove an item from the shopping cart
    /// </summary>
    [HttpDelete("items/{id:long}")]
    [ProducesResponseType(typeof(ApiResponse<CartDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse<CartDto>), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> RemoveCartItem(ulong id)
    {
        var userId = GetCurrentUserId();
        var result = await _cartService.RemoveCartItemAsync(userId, id);
        return StatusCode(result.StatusCode, result);
    }

    /// <summary>
    /// Clear all items from the shopping cart
    /// </summary>
    [HttpDelete]
    [ProducesResponseType(typeof(ApiResponse<bool>), StatusCodes.Status200OK)]
    public async Task<IActionResult> ClearCart()
    {
        var userId = GetCurrentUserId();
        var result = await _cartService.ClearCartAsync(userId);
        return Ok(result);
    }
}
