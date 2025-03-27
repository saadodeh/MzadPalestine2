using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MzadPalestine.Core.DTOs.Auctions;
using MzadPalestine.Core.Interfaces.Services;
using MzadPalestine.Core.Models.Common;

namespace MzadPalestine.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuctionController : ControllerBase
{
    private readonly IAuctionService _auctionService;
    private readonly ICurrentUserService _currentUserService;

    public AuctionController(IAuctionService auctionService, ICurrentUserService currentUserService)
    {
        _auctionService = auctionService;
        _currentUserService = currentUserService;
    }

    [Authorize]
    [HttpPost("create")]
    public async Task<IActionResult> CreateAuction([FromBody] CreateAuctionDto createAuctionDto)
    {
        var userId = _currentUserService.GetUserId();
        var result = await _auctionService.CreateAuctionAsync(userId, createAuctionDto);
        
        if (!result.Succeeded)
            return BadRequest(result.Message);

        return CreatedAtAction(nameof(GetAuction), new { auctionId = result.Data.Id }, result.Data);
    }

    [Authorize]
    [HttpPut("update/{auctionId}")]
    public async Task<IActionResult> UpdateAuction(int auctionId, [FromBody] UpdateAuctionDto updateAuctionDto)
    {
        var userId = _currentUserService.GetUserId();
        var result = await _auctionService.UpdateAuctionAsync(userId, auctionId, updateAuctionDto);
        
        if (!result.Succeeded)
            return BadRequest(result.Message);

        return Ok(result.Data);
    }

    [Authorize]
    [HttpDelete("delete/{auctionId}")]
    public async Task<IActionResult> DeleteAuction(int auctionId)
    {
        var userId = _currentUserService.GetUserId();
        var result = await _auctionService.DeleteAuctionAsync(userId, auctionId);
        
        if (!result.Succeeded)
            return BadRequest(result.Message);

        return Ok(new { message = "Auction deleted successfully" });
    }

    [HttpGet("all")]
    public async Task<IActionResult> GetAllAuctions([FromQuery] PaginationParams parameters)
    {
        var result = await _auctionService.GetAllAuctionsAsync(parameters);
        return Ok(result);
    }

    [HttpGet("{auctionId}")]
    public async Task<IActionResult> GetAuction(int auctionId)
    {
        var result = await _auctionService.GetAuctionByIdAsync(auctionId);
        
        if (!result.Succeeded)
            return NotFound(result.Message);

        return Ok(result.Data);
    }

    [HttpGet("search")]
    public async Task<IActionResult> SearchAuctions([FromQuery] string searchTerm, [FromQuery] PaginationParams parameters)
    {
        var result = await _auctionService.SearchAuctionsAsync(searchTerm, parameters);
        return Ok(result);
    }

    [Authorize]
    [HttpPut("close/{auctionId}")]
    public async Task<IActionResult> CloseAuction(int auctionId)
    {
        var userId = _currentUserService.GetUserId();
        var result = await _auctionService.CloseAuctionAsync(userId, auctionId);
        
        if (!result.Succeeded)
            return BadRequest(result.Message);

        return Ok(new { message = "Auction closed successfully" });
    }
}
