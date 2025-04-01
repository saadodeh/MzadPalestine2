using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MzadPalestine.Core.DTOs.Disputes;
using MzadPalestine.Core.Interfaces.Services;
using MzadPalestine.Core.Models.Common;

namespace MzadPalestine.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class DisputeController : ControllerBase
{
    private readonly IDisputeService _disputeService;
    private readonly ICurrentUserService _currentUserService;

    public DisputeController(IDisputeService disputeService, ICurrentUserService currentUserService)
    {
        _disputeService = disputeService;
        _currentUserService = currentUserService;
    }

    [HttpPost("submit")]
    public async Task<IActionResult> SubmitDispute([FromBody] CreateDisputeDto disputeDto)
    {
        var userId = _currentUserService.GetUserId();
        var result = await _disputeService.CreateDisputeAsync(userId, disputeDto);
        
        if (!result.Succeeded)
            return BadRequest(result.Message);

        return CreatedAtAction(nameof(GetDispute), new { disputeId = result.Data.Id }, result.Data);
    }

    [HttpGet("user/{userId}")]
    public async Task<IActionResult> GetUserDisputes(string userId, [FromQuery] PaginationParams parameters)
    {
        var currentUserId = _currentUserService.GetUserId();
        if (currentUserId != userId && !User.IsInRole("Admin"))
            return Forbid();

        var result = await _disputeService.GetUserDisputesAsync(userId, parameters);
        return Ok(result);
    }

    [HttpGet("{disputeId}")]
    public async Task<IActionResult> GetDispute(int disputeId)
    {
        var userId = _currentUserService.GetUserId();
        var result = await _disputeService.GetDisputeByIdAsync(userId, disputeId);
        
        if (!result.Succeeded)
            return NotFound(result.Message);

        return Ok(result.Data);
    }

    [HttpPut("update/{disputeId}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> UpdateDisputeStatus(int disputeId, [FromBody] ResolveDisputeDto resolveDto)
    {
        var adminId = _currentUserService.GetUserId();
        var result = await _disputeService.ResolveDisputeAsync(adminId, disputeId, resolveDto);
        
        if (!result.Succeeded)
            return BadRequest(result.Message);

        return Ok(result.Data);
    }

    [HttpPost("evidence/{disputeId}")]
    public async Task<IActionResult> AddDisputeEvidence(int disputeId, [FromForm] AddDisputeEvidenceRequestDto evidenceDto)
    {
        var userId = _currentUserService.GetUserId();
        var result = await _disputeService.AddDisputeEvidenceAsync(userId, disputeId, evidenceDto);
        
        if (!result.Succeeded)
            return BadRequest(result.Message);

        return Ok(result.Data);
    }

    [HttpGet("auction/{auctionId}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> GetAuctionDisputes(int auctionId, [FromQuery] PaginationParams parameters)
    {
        var result = await _disputeService.GetAuctionDisputesAsync(auctionId, parameters);
        return Ok(result);
    }
}
