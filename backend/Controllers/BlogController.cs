using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PetHaven.Api.Data;
using PetHaven.Api.DTOs.Blog;
using PetHaven.Api.DTOs.Common;
using PetHaven.Api.Models;

namespace PetHaven.Api.Controllers;

[Route("api/[controller]")]
public class BlogController : BaseApiController
{
    private readonly AppDbContext _context;

    public BlogController(AppDbContext context)
    {
        _context = context;
    }

    /// <summary>
    /// Retrieve published pet-care guides and educational articles
    /// </summary>
    [HttpGet]
    [ProducesResponseType(typeof(ApiResponse<List<BlogPostDto>>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetBlogPosts([FromQuery] uint? categoryId, [FromQuery] uint? petTypeId)
    {
        var query = _context.BlogPosts
            .AsNoTracking()
            .Where(b => b.Status == "published")
            .Include(b => b.Category)
            .Include(b => b.Author)
            .Include(b => b.PetType)
            .AsQueryable();

        if (categoryId.HasValue)
        {
            query = query.Where(b => b.CategoryId == categoryId.Value);
        }

        if (petTypeId.HasValue)
        {
            query = query.Where(b => b.PetTypeId == petTypeId.Value);
        }

        var posts = await query
            .OrderByDescending(b => b.PublishedAt)
            .Select(b => MapPostToDto(b))
            .ToListAsync();

        return Ok(ApiResponse<List<BlogPostDto>>.Ok(posts));
    }

    /// <summary>
    /// Retrieve a single blog post by SEO slug
    /// </summary>
    [HttpGet("{slug}")]
    [ProducesResponseType(typeof(ApiResponse<BlogPostDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse<BlogPostDto>), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetBlogPostBySlug(string slug)
    {
        var post = await _context.BlogPosts
            .AsNoTracking()
            .Include(b => b.Category)
            .Include(b => b.Author)
            .Include(b => b.PetType)
            .FirstOrDefaultAsync(b => b.Slug == slug.Trim() && b.Status == "published");

        if (post == null)
        {
            return NotFound(ApiResponse<BlogPostDto>.Fail("Blog article not found.", 404));
        }

        return Ok(ApiResponse<BlogPostDto>.Ok(MapPostToDto(post)));
    }

    /// <summary>
    /// Create a new pet guide or article (Admin & Staff)
    /// </summary>
    [Authorize(Roles = "admin,staff")]
    [HttpPost]
    [ProducesResponseType(typeof(ApiResponse<BlogPostDto>), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ApiResponse<BlogPostDto>), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> CreateBlogPost([FromBody] CreateBlogPostDto dto)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ApiResponse<BlogPostDto>.Fail("Invalid article payload.", 400));
        }

        var authorId = GetCurrentUserId();
        var slug = GenerateSlug(dto.Title);
        var existingSlug = await _context.BlogPosts.AnyAsync(b => b.Slug == slug);
        if (existingSlug)
        {
            slug = $"{slug}-{DateTime.UtcNow.Ticks % 10000}";
        }

        var post = new BlogPost
        {
            CategoryId = dto.CategoryId,
            AuthorId = authorId,
            PetTypeId = dto.PetTypeId,
            Title = dto.Title.Trim(),
            Slug = slug,
            Content = dto.Content,
            Excerpt = dto.Excerpt,
            FeaturedImage = dto.FeaturedImage,
            Status = dto.Status,
            PublishedAt = dto.Status == "published" ? DateTime.UtcNow : null,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };

        _context.BlogPosts.Add(post);
        await _context.SaveChangesAsync();

        var created = await _context.BlogPosts
            .Include(b => b.Category)
            .Include(b => b.Author)
            .Include(b => b.PetType)
            .FirstAsync(b => b.Id == post.Id);

        return StatusCode(StatusCodes.Status201Created, ApiResponse<BlogPostDto>.Created(MapPostToDto(created), "Article published successfully."));
    }

    private static BlogPostDto MapPostToDto(BlogPost b) => new()
    {
        Id = b.Id,
        CategoryId = b.CategoryId,
        CategoryName = b.Category?.Name ?? string.Empty,
        AuthorId = b.AuthorId,
        AuthorName = b.Author?.Name ?? "Staff Writer",
        PetTypeId = b.PetTypeId,
        PetTypeName = b.PetType?.Name,
        Title = b.Title,
        Slug = b.Slug,
        Content = b.Content,
        Excerpt = b.Excerpt,
        FeaturedImage = b.FeaturedImage,
        Status = b.Status,
        PublishedAt = b.PublishedAt,
        CreatedAt = b.CreatedAt
    };

    private static string GenerateSlug(string text)
    {
        var slug = text.ToLowerInvariant().Trim();
        slug = System.Text.RegularExpressions.Regex.Replace(slug, @"[^a-z0-9\s-]", "");
        slug = System.Text.RegularExpressions.Regex.Replace(slug, @"\s+", " ").Trim();
        return slug.Replace(" ", "-");
    }
}
