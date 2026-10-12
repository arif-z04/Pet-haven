using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PetHaven.Api.Data;
using PetHaven.Api.DTOs.Brands;
using PetHaven.Api.DTOs.Common;
using PetHaven.Api.DTOs.Products;
using PetHaven.Api.Services.Interfaces;

namespace PetHaven.Api.Controllers;

[Route("api/[controller]")]
public class BrandsController : BaseApiController
{
    private readonly AppDbContext _context;
    private readonly IProductService _productService;

    public BrandsController(AppDbContext context, IProductService productService)
    {
        _context = context;
        _productService = productService;
    }

    /// <summary>
    /// Retrieve all active brands
    /// </summary>
    [HttpGet]
    [ProducesResponseType(typeof(ApiResponse<List<BrandDto>>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetBrands()
    {
        var brands = await _context.Brands
            .AsNoTracking()
            .Where(b => b.Status == "active")
            .Include(b => b.Products)
            .OrderBy(b => b.Name)
            .Select(b => new BrandDto
            {
                Id = b.Id,
                Name = b.Name,
                Slug = b.Slug,
                Description = b.Description,
                LogoUrl = b.LogoUrl,
                WebsiteUrl = b.WebsiteUrl,
                IsFeatured = b.IsFeatured,
                Status = b.Status,
                ProductCount = b.Products.Count
            })
            .ToListAsync();

        return Ok(ApiResponse<List<BrandDto>>.Ok(brands));
    }

    /// <summary>
    /// Retrieve products by brand ID
    /// </summary>
    [HttpGet("{id:int}/products")]
    [ProducesResponseType(typeof(ApiResponse<PagedResult<ProductDto>>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetBrandProducts(uint id, [FromQuery] ProductQueryParameters queryParams)
    {
        queryParams.BrandId = id;
        var result = await _productService.GetProductsAsync(queryParams);
        return Ok(result);
    }
}
