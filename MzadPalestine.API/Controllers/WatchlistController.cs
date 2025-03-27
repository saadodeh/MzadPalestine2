using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MzadPalestine.Core.DTOs.Watchlists;
using MzadPalestine.Core.Interfaces.Services;
using MzadPalestine.Core.Models.Common;

namespace MzadPalestine.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class WatchlistController : ControllerBase
{
    private readonly IWatchlistService _watchlistService;
    private readonly ICurrentUserService _currentUserService;

    public WatchlistController(IWatchlistService watchlistService, ICurrentUserService currentUserService)
    {
        _watchlistService = watchlistService;
        _currentUserService = currentUserService;
    }

    [HttpPost("add")]
    public async Task<IActionResult> AddToWatchlist([FromBody] AddToWatchlistDto watchlistDto)
    {
        var userId = _currentUserService.GetUserId();
        var result = await _watchlistService.AddToWatchlistAsync(userId, watchlistDto.AuctionId);
        
        if (!result.Succeeded)
            return BadRequest(result.Message);

        return Ok(new { message = "Added to watchlist successfully" });
    }

    [HttpDelete("remove/{auctionId}")]
    public async Task<IActionResult> RemoveFromWatchlist(int auctionId)
    {
        var userId = _currentUserService.GetUserId();
        var result = await _watchlistService.RemoveFromWatchlistAsync(userId, auctionId);
        
        if (!result.Succeeded)
            return BadRequest(result.Message);

        return Ok(new { message = "Removed from watchlist successfully" });
    }

    [HttpGet("user/{userId}")]
    public async Task<IActionResult> GetUserWatchlist(string userId, [FromQuery] PaginationParams parameters)
    {
        var currentUserId = _currentUserService.GetUserId();
        if (currentUserId != userId)
            return Forbid();

        var result = await _watchlistService.GetUserWatchlistAsync(userId, parameters);
        return Ok(result);
    }

    [HttpGet("check/{auctionId}")]
    public async Task<IActionResult> CheckWatchlistStatus(int auctionId)
    {
        var userId = _currentUserService.GetUserId();
        var isWatched = await _watchlistService.IsInWatchlistAsync(userId, auctionId);
        return Ok(new { isWatched });
    }
}
