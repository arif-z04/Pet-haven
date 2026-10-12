using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PetHaven.Api.DTOs.Common;
using PetHaven.Api.DTOs.Products;
using PetHaven.Api.Services.Interfaces;

namespace PetHaven.Api.Controllers;

[Route("api/[controller]")]
public class ProductsController : BaseApiController
{
    private readonly IProductService _productService;

    public ProductsController(IProductService productService)
    {
        _productService = productService;
    }

    /// <summary>
    /// Retrieve paginated products with multi-criteria filtering and sorting
    /// </summary>
    [HttpGet]
    [ProducesResponseType(typeof(ApiResponse<PagedResult<ProductDto>>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetProducts([FromQuery] ProductQueryParameters queryParams)
    {
        var result = await _productService.GetProductsAsync(queryParams);
        return Ok(result);
    }

    /// <summary>
    /// Retrieve single product details by internal numeric ID
    /// </summary>
    [HttpGet("{id:long}")]
    [ProducesResponseType(typeof(ApiResponse<ProductDetailDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse<ProductDetailDto>), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetProductById(ulong id)
    {
        var result = await _productService.GetProductByIdAsync(id);
        return StatusCode(result.StatusCode, result);
    }

    /// <summary>
    /// Retrieve single product details by SEO slug
    /// </summary>
    [HttpGet("slug/{slug}")]
    [ProducesResponseType(typeof(ApiResponse<ProductDetailDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse<ProductDetailDto>), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetProductBySlug(string slug)
    {
        var result = await _productService.GetProductBySlugAsync(slug);
        return StatusCode(result.StatusCode, result);
    }

    /// <summary>
    /// Create a new product (Admin and Staff only)
    /// </summary>
    [Authorize(Roles = "admin,staff")]
    [HttpPost]
    [ProducesResponseType(typeof(ApiResponse<ProductDetailDto>), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ApiResponse<ProductDetailDto>), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ApiResponse<ProductDetailDto>), StatusCodes.Status409Conflict)]
    public async Task<IActionResult> CreateProduct([FromBody] CreateProductDto dto)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ApiResponse<ProductDetailDto>.Fail("Invalid product payload.", 400));
        }

        var result = await _productService.CreateProductAsync(dto);
        return StatusCode(result.StatusCode, result);
    }

    /// <summary>
    /// Update existing product details (Admin and Staff only)
    /// </summary>
    [Authorize(Roles = "admin,staff")]
    [HttpPut("{id:long}")]
    [ProducesResponseType(typeof(ApiResponse<ProductDetailDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse<ProductDetailDto>), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> UpdateProduct(ulong id, [FromBody] UpdateProductDto dto)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ApiResponse<ProductDetailDto>.Fail("Invalid product update payload.", 400));
        }

        var result = await _productService.UpdateProductAsync(id, dto);
        return StatusCode(result.StatusCode, result);
    }

    /// <summary>
    /// Delete or archive a product (Admin only)
    /// </summary>
    [Authorize(Roles = "admin")]
    [HttpDelete("{id:long}")]
    [ProducesResponseType(typeof(ApiResponse<bool>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse<bool>), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> DeleteProduct(ulong id)
    {
        var result = await _productService.DeleteProductAsync(id);
        return StatusCode(result.StatusCode, result);
    }
}
