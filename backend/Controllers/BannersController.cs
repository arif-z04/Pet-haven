using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PetHaven.Api.Data;
using PetHaven.Api.DTOs.Banners;
using PetHaven.Api.DTOs.Common;

namespace PetHaven.Api.Controllers;

[Route("api/[controller]")]
public class BannersController : BaseApiController
{
    private readonly AppDbContext _context;

    public BannersController(AppDbContext context)
    {
        _context = context;
    }

    /// <summary>
    /// Retrieve all active homepage and promotional banners
    /// </summary>
    [HttpGet]
    [ProducesResponseType(typeof(ApiResponse<List<BannerDto>>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetBanners([FromQuery] string? position)
    {
        var now = DateTime.UtcNow;
        var query = _context.Banners
            .AsNoTracking()
            .Where(b => b.IsActive)
            .Where(b => b.StartDate == null || b.StartDate <= now)
            .Where(b => b.EndDate == null || b.EndDate >= now);

        if (!string.IsNullOrWhiteSpace(position))
        {
            query = query.Where(b => b.Position == position.Trim());
        }

        var banners = await query
            .OrderBy(b => b.SortOrder)
            .Select(b => new BannerDto
            {
                Id = b.Id,
                Title = b.Title,
                Subtitle = b.Subtitle,
                ImageUrl = b.ImageUrl,
                ButtonText = b.ButtonText,
                ButtonLink = b.ButtonLink,
                Position = b.Position,
                SortOrder = b.SortOrder,
                IsActive = b.IsActive
            })
            .ToListAsync();

        return Ok(ApiResponse<List<BannerDto>>.Ok(banners));
    }
}
