using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PetHaven.Api.Data;
using PetHaven.Api.DTOs.Common;
using PetHaven.Api.DTOs.Reviews;
using PetHaven.Api.Models;

namespace PetHaven.Api.Controllers;

[Route("api")]
public class ReviewsController : BaseApiController
{
    private readonly AppDbContext _context;

    public ReviewsController(AppDbContext context)
    {
        _context = context;
    }

    /// <summary>
    /// Retrieve all approved reviews for a given product
    /// </summary>
    [HttpGet("products/{productId:long}/reviews")]
    [ProducesResponseType(typeof(ApiResponse<List<ReviewDto>>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetProductReviews(ulong productId)
    {
        var reviews = await _context.Reviews
            .AsNoTracking()
            .Where(r => r.ProductId == productId && r.Status == "approved")
            .Include(r => r.User)
            .OrderByDescending(r => r.CreatedAt)
            .Select(r => new ReviewDto
            {
                Id = r.Id,
                ProductId = r.ProductId,
                UserId = r.UserId,
                UserName = r.User.Name,
                Rating = r.Rating,
                Title = r.Title,
                Comment = r.Comment,
                Status = r.Status,
                CreatedAt = r.CreatedAt
            })
            .ToListAsync();

        return Ok(ApiResponse<List<ReviewDto>>.Ok(reviews));
    }

    /// <summary>
    /// Submit a product review (Authenticated customer)
    /// </summary>
    [Authorize]
    [HttpPost("products/{productId:long}/reviews")]
    [ProducesResponseType(typeof(ApiResponse<ReviewDto>), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ApiResponse<ReviewDto>), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ApiResponse<ReviewDto>), StatusCodes.Status409Conflict)]
    public async Task<IActionResult> CreateReview(ulong productId, [FromBody] CreateReviewDto dto)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ApiResponse<ReviewDto>.Fail("Invalid review rating or text.", 400));
        }

        var userId = GetCurrentUserId();
        var product = await _context.Products.FindAsync(productId);
        if (product == null)
        {
            return NotFound(ApiResponse<ReviewDto>.Fail("Product not found.", 404));
        }

        var alreadyReviewed = await _context.Reviews.AnyAsync(r => r.ProductId == productId && r.UserId == userId);
        if (alreadyReviewed)
        {
            return Conflict(ApiResponse<ReviewDto>.Fail("You have already submitted a review for this product.", 409));
        }

        // Verify if user ordered this product for verified badge
        var verifiedOrder = await _context.OrderItems
            .Where(oi => oi.ProductId == productId && oi.Order.UserId == userId)
            .Select(oi => (ulong?)oi.OrderId)
            .FirstOrDefaultAsync();

        var review = new Review
        {
            UserId = userId,
            ProductId = productId,
            OrderId = verifiedOrder,
            Rating = dto.Rating,
            Title = dto.Title?.Trim(),
            Comment = dto.Comment?.Trim(),
            Status = "approved", // Automatically approve or set to pending for moderation
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };

        _context.Reviews.Add(review);
        await _context.SaveChangesAsync();

        var user = await _context.Users.FindAsync(userId);
        var reviewDto = new ReviewDto
        {
            Id = review.Id,
            ProductId = review.ProductId,
            UserId = review.UserId,
            UserName = user?.Name ?? "Customer",
            Rating = review.Rating,
            Title = review.Title,
            Comment = review.Comment,
            Status = review.Status,
            CreatedAt = review.CreatedAt
        };

        return StatusCode(StatusCodes.Status201Created, ApiResponse<ReviewDto>.Created(reviewDto, "Review submitted successfully."));
    }

    /// <summary>
    /// Edit an existing review (Owner or Admin)
    /// </summary>
    [Authorize]
    [HttpPut("reviews/{id:long}")]
    [ProducesResponseType(typeof(ApiResponse<ReviewDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse<ReviewDto>), StatusCodes.Status403Forbidden)]
    [ProducesResponseType(typeof(ApiResponse<ReviewDto>), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> UpdateReview(ulong id, [FromBody] UpdateReviewDto dto)
    {
        var review = await _context.Reviews.Include(r => r.User).FirstOrDefaultAsync(r => r.Id == id);
        if (review == null)
        {
            return NotFound(ApiResponse<ReviewDto>.Fail("Review not found.", 404));
        }

        var userId = GetCurrentUserId();
        if (review.UserId != userId && !IsAdminOrStaff())
        {
            return StatusCode(StatusCodes.Status403Forbidden, ApiResponse<ReviewDto>.Fail("You cannot edit this review.", 403));
        }

        review.Rating = dto.Rating;
        review.Title = dto.Title?.Trim();
        review.Comment = dto.Comment?.Trim();
        review.UpdatedAt = DateTime.UtcNow;

        await _context.SaveChangesAsync();

        var reviewDto = new ReviewDto
        {
            Id = review.Id,
            ProductId = review.ProductId,
            UserId = review.UserId,
            UserName = review.User.Name,
            Rating = review.Rating,
            Title = review.Title,
            Comment = review.Comment,
            Status = review.Status,
            CreatedAt = review.CreatedAt
        };

        return Ok(ApiResponse<ReviewDto>.Ok(reviewDto, "Review updated successfully."));
    }

    /// <summary>
    /// Delete a review (Owner or Admin)
    /// </summary>
    [Authorize]
    [HttpDelete("reviews/{id:long}")]
    [ProducesResponseType(typeof(ApiResponse<bool>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse<bool>), StatusCodes.Status403Forbidden)]
    [ProducesResponseType(typeof(ApiResponse<bool>), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> DeleteReview(ulong id)
    {
        var review = await _context.Reviews.FindAsync(id);
        if (review == null)
        {
            return NotFound(ApiResponse<bool>.Fail("Review not found.", 404));
        }

        var userId = GetCurrentUserId();
        if (review.UserId != userId && !IsAdminOrStaff())
        {
            return StatusCode(StatusCodes.Status403Forbidden, ApiResponse<bool>.Fail("You cannot delete this review.", 403));
        }

        _context.Reviews.Remove(review);
        await _context.SaveChangesAsync();

        return Ok(ApiResponse<bool>.Ok(true, "Review deleted successfully."));
    }
}
