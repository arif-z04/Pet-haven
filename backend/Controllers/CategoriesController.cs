using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PetHaven.Api.Data;
using PetHaven.Api.DTOs.Categories;
using PetHaven.Api.DTOs.Common;
using PetHaven.Api.DTOs.Products;
using PetHaven.Api.Services.Interfaces;

namespace PetHaven.Api.Controllers;

[Route("api/[controller]")]
public class CategoriesController : BaseApiController
{
    private readonly AppDbContext _context;
    private readonly IProductService _productService;

    public CategoriesController(AppDbContext context, IProductService productService)
    {
        _context = context;
        _productService = productService;
    }

    /// <summary>
    /// Retrieve all hierarchical categories with nested subcategories
    /// </summary>
    [HttpGet]
    [ProducesResponseType(typeof(ApiResponse<List<CategoryDto>>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetCategories()
    {
        var rootCategories = await _context.Categories
            .AsNoTracking()
            .Where(c => c.ParentId == null && c.Status == "active")
            .OrderBy(c => c.SortOrder)
            .Include(c => c.SubCategories.Where(sc => sc.Status == "active").OrderBy(sc => sc.SortOrder))
                .ThenInclude(sc => sc.SubCategories.Where(ssc => ssc.Status == "active").OrderBy(ssc => ssc.SortOrder))
            .Include(c => c.Products)
            .ToListAsync();

        var dtoList = rootCategories.Select(c => MapCategoryToDto(c)).ToList();
        return Ok(ApiResponse<List<CategoryDto>>.Ok(dtoList));
    }

    /// <summary>
    /// Retrieve a single category by ID
    /// </summary>
    [HttpGet("{id:int}")]
    [ProducesResponseType(typeof(ApiResponse<CategoryDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse<CategoryDto>), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetCategoryById(uint id)
    {
        var category = await _context.Categories
            .AsNoTracking()
            .Include(c => c.SubCategories.Where(sc => sc.Status == "active"))
            .Include(c => c.Products)
            .FirstOrDefaultAsync(c => c.Id == id);

        if (category == null)
        {
            return NotFound(ApiResponse<CategoryDto>.Fail("Category not found.", 404));
        }

        return Ok(ApiResponse<CategoryDto>.Ok(MapCategoryToDto(category)));
    }

    /// <summary>
    /// Retrieve paginated products belonging to a category
    /// </summary>
    [HttpGet("{id:int}/products")]
    [ProducesResponseType(typeof(ApiResponse<PagedResult<ProductDto>>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetCategoryProducts(uint id, [FromQuery] ProductQueryParameters queryParams)
    {
        queryParams.CategoryId = id;
        var result = await _productService.GetProductsAsync(queryParams);
        return Ok(result);
    }

    private static CategoryDto MapCategoryToDto(Models.Category c) => new()
    {
        Id = c.Id,
        ParentId = c.ParentId,
        PetTypeId = c.PetTypeId,
        Name = c.Name,
        Slug = c.Slug,
        Description = c.Description,
        ImageUrl = c.ImageUrl,
        IconUrl = c.IconUrl,
        SortOrder = c.SortOrder,
        Status = c.Status,
        ProductCount = c.Products?.Count ?? 0,
        SubCategories = c.SubCategories?.Select(sc => MapCategoryToDto(sc)).ToList() ?? new()
    };
}
