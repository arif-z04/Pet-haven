using Microsoft.AspNetCore.Mvc;
using PetHaven.Api.DTOs.Common;
using PetHaven.Api.DTOs.Products;
using PetHaven.Api.Services.Interfaces;

namespace PetHaven.Api.Controllers;

[Route("api/[controller]")]
public class SearchController : BaseApiController
{
    private readonly IProductService _productService;

    public SearchController(IProductService productService)
    {
        _productService = productService;
    }

    /// <summary>
    /// Global product search across title, description, and SKU with pagination
    /// </summary>
    [HttpGet]
    [ProducesResponseType(typeof(ApiResponse<PagedResult<ProductDto>>), StatusCodes.Status200OK)]
    public async Task<IActionResult> Search([FromQuery] string? q, [FromQuery] PaginationParams pagination)
    {
        if (string.IsNullOrWhiteSpace(q))
        {
            return Ok(ApiResponse<PagedResult<ProductDto>>.Ok(new PagedResult<ProductDto>(new List<ProductDto>(), 0, pagination.PageNumber, pagination.PageSize)));
        }

        var result = await _productService.SearchProductsAsync(q.Trim(), pagination);
        return Ok(result);
    }
}
