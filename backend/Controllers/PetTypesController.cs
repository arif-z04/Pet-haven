using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PetHaven.Api.Data;
using PetHaven.Api.DTOs.Common;
using PetHaven.Api.DTOs.PetTypes;
using PetHaven.Api.DTOs.Products;
using PetHaven.Api.Services.Interfaces;

namespace PetHaven.Api.Controllers;

[Route("api/pet-types")]
public class PetTypesController : BaseApiController
{
    private readonly AppDbContext _context;
    private readonly IProductService _productService;

    public PetTypesController(AppDbContext context, IProductService productService)
    {
        _context = context;
        _productService = productService;
    }

    /// <summary>
    /// Retrieve all pet classifications (Cat, Dog, Bird, Rabbit, Small Pets)
    /// </summary>
    [HttpGet]
    [ProducesResponseType(typeof(ApiResponse<List<PetTypeDto>>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetPetTypes()
    {
        var petTypes = await _context.PetTypes
            .AsNoTracking()
            .Where(pt => pt.Status == "active")
            .Include(pt => pt.Products)
            .OrderBy(pt => pt.SortOrder)
            .Select(pt => new PetTypeDto
            {
                Id = pt.Id,
                Name = pt.Name,
                Slug = pt.Slug,
                Description = pt.Description,
                IconUrl = pt.IconUrl,
                SortOrder = pt.SortOrder,
                Status = pt.Status,
                ProductCount = pt.Products.Count
            })
            .ToListAsync();

        return Ok(ApiResponse<List<PetTypeDto>>.Ok(petTypes));
    }

    /// <summary>
    /// Retrieve products classified by pet type ID
    /// </summary>
    [HttpGet("{id:int}/products")]
    [ProducesResponseType(typeof(ApiResponse<PagedResult<ProductDto>>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetPetTypeProducts(uint id, [FromQuery] ProductQueryParameters queryParams)
    {
        queryParams.PetTypeId = id;
        var result = await _productService.GetProductsAsync(queryParams);
        return Ok(result);
    }
}
