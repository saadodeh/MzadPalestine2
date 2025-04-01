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
    private readonly MzadPalestine.Core.Interfaces.Services.ICurrentUserService _currentUserService;

    public WatchlistController(IWatchlistService watchlistService, MzadPalestine.Core.Interfaces.Services.ICurrentUserService currentUserService)
    {
        _watchlistService = watchlistService;
        _currentUserService = currentUserService;
    }

    [HttpPost("add")]
    public async Task<IActionResult> AddToWatchlist([FromBody] AddToWatchlistDto watchlistDto)
    {
        var result = await _watchlistService.AddToWatchlistAsync(_currentUserService.UserId, watchlistDto.AuctionId);

        if (!result.Succeeded)
            return BadRequest(result.Message);

        return Ok(new { message = "Added to watchlist successfully" });
    }

    [HttpDelete("remove/{auctionId}")]
    public async Task<IActionResult> RemoveFromWatchlist(int auctionId)
    {
        var result = await _watchlistService.RemoveFromWatchlistAsync(_currentUserService.UserId, auctionId);

        if (!result.Succeeded)
            return BadRequest(result.Message);

        return Ok(new { message = "Removed from watchlist successfully" });
    }

    [HttpGet("user/{userId}")]
    public async Task<IActionResult> GetUserWatchlist(string userId, [FromQuery] PaginationParams parameters)
    {
        if (_currentUserService.UserId != userId)
            return Forbid();

        var result = await _watchlistService.GetUserWatchlistAsync(userId, parameters);
        return Ok(result);
    }

    [HttpGet("check/{auctionId}")]
    public async Task<IActionResult> CheckWatchlistStatus(int auctionId)
    {
        var result = await _watchlistService.CheckWatchlistStatusAsync(_currentUserService.UserId, auctionId);
        return Ok(result);
    }
}
