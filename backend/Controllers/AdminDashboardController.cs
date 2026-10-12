using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PetHaven.Api.DTOs.Common;
using PetHaven.Api.DTOs.Dashboard;
using PetHaven.Api.Services.Interfaces;

namespace PetHaven.Api.Controllers;

[Authorize(Roles = "admin,staff")]
[Route("api/admin/dashboard")]
public class AdminDashboardController : BaseApiController
{
    private readonly IDashboardService _dashboardService;

    public AdminDashboardController(IDashboardService dashboardService)
    {
        _dashboardService = dashboardService;
    }

    /// <summary>
    /// Retrieve operational metrics, inventory warnings, top sellers, and revenue graphs (Admin & Staff)
    /// </summary>
    [HttpGet("stats")]
    [ProducesResponseType(typeof(ApiResponse<DashboardStatsDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetDashboardStats()
    {
        var result = await _dashboardService.GetDashboardStatsAsync();
        return Ok(result);
    }
}
