using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MzadPalestine.Core.DTOs.Bids;
using MzadPalestine.Core.Interfaces.Services;
using MzadPalestine.Core.Models.Common;

namespace MzadPalestine.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class BidController : ControllerBase
{
    private readonly IAuctionService _auctionService;
    private readonly ICurrentUserService _currentUserService;

    public BidController(IAuctionService auctionService, ICurrentUserService currentUserService)
    {
        _auctionService = auctionService;
        _currentUserService = currentUserService;
    }

    [Authorize]
    [HttpPost("place")]
    public async Task<IActionResult> PlaceBid([FromBody] PlaceBidDto placeBidDto)
    {
        var userId = _currentUserService.GetUserId();
        var result = await _auctionService.PlaceBidAsync(userId, placeBidDto);
        
        if (!result.Succeeded)
            return BadRequest(result.Message);

        return Ok(result.Data);
    }

    [HttpGet("auction/{auctionId}")]
    public async Task<IActionResult> GetAuctionBids(int auctionId, [FromQuery] PaginationParams parameters)
    {
        var result = await _auctionService.GetAuctionBidsAsync(auctionId, parameters);
        return Ok(result);
    }

    [Authorize]
    [HttpGet("user/{userId}")]
    public async Task<IActionResult> GetUserBids(string userId, [FromQuery] PaginationParams parameters)
    {
        var currentUserId = _currentUserService.GetUserId();
        if (currentUserId != userId)
            return Forbid();

        var result = await _auctionService.GetUserBidsAsync(userId, parameters);
        return Ok(result);
    }

    [Authorize]
    [HttpDelete("cancel/{bidId}")]
    public async Task<IActionResult> CancelBid(int bidId)
    {
        var userId = _currentUserService.GetUserId();
        var result = await _auctionService.CancelBidAsync(userId, bidId);
        
        if (!result.Succeeded)
            return BadRequest(result.Message);

        return Ok(new { message = "Bid cancelled successfully" });
    }

    [HttpGet("winning/{auctionId}")]
    public async Task<IActionResult> GetWinningBid(int auctionId)
    {
        var result = await _auctionService.GetWinningBidAsync(auctionId);
        
        if (!result.Succeeded)
            return NotFound(result.Message);

        return Ok(result.Data);
    }
}
