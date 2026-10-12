using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PetHaven.Api.DTOs.Common;
using PetHaven.Api.DTOs.Products;
using PetHaven.Api.Services.Interfaces;

namespace PetHaven.Api.Controllers;

[Authorize]
[Route("api/[controller]")]
public class RecommendationsController : BaseApiController
{
    private readonly IRecommendationService _recommendationService;

    public RecommendationsController(IRecommendationService recommendationService)
    {
        _recommendationService = recommendationService;
    }

    /// <summary>
    /// Retrieve personalized product recommendations tailored to the customer's pets and order history
    /// </summary>
    [HttpGet]
    [ProducesResponseType(typeof(ApiResponse<List<ProductDto>>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetRecommendations([FromQuery] int limit = 8, [FromQuery] ulong? petId = null)
    {
        var userId = GetCurrentUserId();
        var result = await _recommendationService.GetRecommendationsAsync(userId, limit, petId);
        return Ok(result);
    }
}
