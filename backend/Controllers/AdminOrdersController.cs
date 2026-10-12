using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PetHaven.Api.DTOs.Common;
using PetHaven.Api.DTOs.Orders;
using PetHaven.Api.Services.Interfaces;

namespace PetHaven.Api.Controllers;

[Authorize(Roles = "admin,staff")]
[Route("api/admin/orders")]
public class AdminOrdersController : BaseApiController
{
    private readonly IOrderService _orderService;

    public AdminOrdersController(IOrderService orderService)
    {
        _orderService = orderService;
    }

    /// <summary>
    /// Retrieve all platform orders with status filtering and pagination (Admin & Staff)
    /// </summary>
    [HttpGet]
    [ProducesResponseType(typeof(ApiResponse<PagedResult<OrderDto>>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAllOrders([FromQuery] PaginationParams pagination, [FromQuery] string? status)
    {
        var result = await _orderService.GetAdminOrdersAsync(pagination, status);
        return Ok(result);
    }

    /// <summary>
    /// Retrieve complete order details by order ID (Admin & Staff)
    /// </summary>
    [HttpGet("{id:long}")]
    [ProducesResponseType(typeof(ApiResponse<OrderDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse<OrderDto>), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetOrderById(ulong id)
    {
        var result = await _orderService.GetOrderByIdAsync(userId: 0, orderId: id, isAdmin: true);
        return StatusCode(result.StatusCode, result);
    }

    /// <summary>
    /// Update workflow order status and payment status (Admin & Staff)
    /// </summary>
    [HttpPut("{id:long}/status")]
    [ProducesResponseType(typeof(ApiResponse<OrderDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse<OrderDto>), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ApiResponse<OrderDto>), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> UpdateOrderStatus(ulong id, [FromBody] UpdateOrderStatusDto dto)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ApiResponse<OrderDto>.Fail("Invalid status update payload.", 400));
        }

        var result = await _orderService.UpdateOrderStatusAsync(id, dto);
        return StatusCode(result.StatusCode, result);
    }
}
