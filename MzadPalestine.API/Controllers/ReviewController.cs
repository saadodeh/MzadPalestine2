using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MzadPalestine.Core.DTOs.Reviews;
using MzadPalestine.Core.Interfaces.Services;
using MzadPalestine.Core.Models.Common;

namespace MzadPalestine.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ReviewController : ControllerBase
{
    private readonly IReviewService _reviewService;
    private readonly ICurrentUserService _currentUserService;

    public ReviewController(IReviewService reviewService, ICurrentUserService currentUserService)
    {
        _reviewService = reviewService;
        _currentUserService = currentUserService;
    }

    [Authorize]
    [HttpPost("add")]
    public async Task<IActionResult> AddReview([FromBody] CreateReviewDto createReviewDto)
    {
        var userId = _currentUserService.GetUserId();
        var result = await _reviewService.CreateReviewAsync(userId, createReviewDto);
        
        if (!result.Succeeded)
            return BadRequest(result.Message);

        return CreatedAtAction(nameof(GetUserReviews), new { userId }, result.Data);
    }

    [Authorize]
    [HttpPut("update/{reviewId}")]
    public async Task<IActionResult> UpdateReview(int reviewId, [FromBody] UpdateReviewDto updateReviewDto)
    {
        var userId = _currentUserService.GetUserId();
        var result = await _reviewService.UpdateReviewAsync(userId, reviewId, updateReviewDto);
        
        if (!result.Succeeded)
            return BadRequest(result.Message);

        return Ok(result.Data);
    }

    [Authorize]
    [HttpDelete("delete/{reviewId}")]
    public async Task<IActionResult> DeleteReview(int reviewId)
    {
        var userId = _currentUserService.GetUserId();
        var result = await _reviewService.DeleteReviewAsync(userId, reviewId);
        
        if (!result.Succeeded)
            return BadRequest(result.Message);

        return Ok(new { message = "Review deleted successfully" });
    }

    [HttpGet("user/{userId}")]
    public async Task<IActionResult> GetUserReviews(string userId, [FromQuery] PaginationParams parameters)
    {
        var result = await _reviewService.GetUserReviewsAsync(userId, parameters);
        return Ok(result);
    }

    [HttpGet("product/{productId}")]
    public async Task<IActionResult> GetProductReviews(int productId, [FromQuery] PaginationParams parameters)
    {
        var result = await _reviewService.GetProductReviewsAsync(productId, parameters);
        return Ok(result);
    }
}
