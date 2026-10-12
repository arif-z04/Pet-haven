using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PetHaven.Api.DTOs.Common;
using PetHaven.Api.DTOs.Orders;
using PetHaven.Api.Services.Interfaces;

namespace PetHaven.Api.Controllers;

[Authorize]
[Route("api/[controller]")]
public class OrdersController : BaseApiController
{
    private readonly IOrderService _orderService;

    public OrdersController(IOrderService orderService)
    {
        _orderService = orderService;
    }

    /// <summary>
    /// Place a new order with transactional inventory reduction and coupon calculation
    /// </summary>
    [HttpPost]
    [ProducesResponseType(typeof(ApiResponse<OrderDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse<OrderDto>), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> CreateOrder([FromBody] CreateOrderDto dto)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ApiResponse<OrderDto>.Fail("Invalid checkout information.", 400));
        }

        var userId = GetCurrentUserId();
        var result = await _orderService.CreateOrderAsync(userId, dto);
        return StatusCode(result.StatusCode, result);
    }

    /// <summary>
    /// Retrieve customer's order history with pagination
    /// </summary>
    [HttpGet]
    [ProducesResponseType(typeof(ApiResponse<PagedResult<OrderDto>>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetMyOrders([FromQuery] PaginationParams pagination)
    {
        var userId = GetCurrentUserId();
        var result = await _orderService.GetCustomerOrdersAsync(userId, pagination);
        return Ok(result);
    }

    /// <summary>
    /// Retrieve a specific order by ID
    /// </summary>
    [HttpGet("{id:long}")]
    [ProducesResponseType(typeof(ApiResponse<OrderDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse<OrderDto>), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetOrderById(ulong id)
    {
        var userId = GetCurrentUserId();
        var result = await _orderService.GetOrderByIdAsync(userId, id, isAdmin: false);
        return StatusCode(result.StatusCode, result);
    }

    /// <summary>
    /// Cancel a pending or confirmed order and restore item inventory
    /// </summary>
    [HttpPost("{id:long}/cancel")]
    [ProducesResponseType(typeof(ApiResponse<OrderDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse<OrderDto>), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ApiResponse<OrderDto>), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> CancelOrder(ulong id)
    {
        var userId = GetCurrentUserId();
        var result = await _orderService.CancelOrderAsync(userId, id);
        return StatusCode(result.StatusCode, result);
    }
}
