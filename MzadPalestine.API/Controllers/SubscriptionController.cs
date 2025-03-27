using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MzadPalestine.Core.DTOs.Subscriptions;
using MzadPalestine.Core.Interfaces.Services;
using MzadPalestine.Core.Models.Common;

namespace MzadPalestine.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class SubscriptionController : ControllerBase
{
    private readonly ISubscriptionService _subscriptionService;
    private readonly ICurrentUserService _currentUserService;

    public SubscriptionController(ISubscriptionService subscriptionService, ICurrentUserService currentUserService)
    {
        _subscriptionService = subscriptionService;
        _currentUserService = currentUserService;
    }

    [HttpPost("subscribe")]
    public async Task<IActionResult> Subscribe([FromBody] CreateSubscriptionDto subscriptionDto)
    {
        var userId = _currentUserService.GetUserId();
        var result = await _subscriptionService.CreateSubscriptionAsync(userId, subscriptionDto);
        
        if (!result.Succeeded)
            return BadRequest(result.Message);

        return Ok(result.Data);
    }

    [HttpPut("cancel/{subscriptionId}")]
    public async Task<IActionResult> CancelSubscription(int subscriptionId)
    {
        var userId = _currentUserService.GetUserId();
        var result = await _subscriptionService.CancelSubscriptionAsync(userId, subscriptionId);
        
        if (!result.Succeeded)
            return BadRequest(result.Message);

        return Ok(new { message = "Subscription cancelled successfully" });
    }

    [HttpGet("user/{userId}")]
    public async Task<IActionResult> GetUserSubscription(string userId)
    {
        var currentUserId = _currentUserService.GetUserId();
        if (currentUserId != userId)
            return Forbid();

        var result = await _subscriptionService.GetUserSubscriptionAsync(userId);
        
        if (!result.Succeeded)
            return NotFound(result.Message);

        return Ok(result.Data);
    }

    [HttpGet("plans")]
    public async Task<IActionResult> GetSubscriptionPlans()
    {
        var plans = await _subscriptionService.GetSubscriptionPlansAsync();
        return Ok(plans);
    }

    [HttpGet("history/{userId}")]
    public async Task<IActionResult> GetSubscriptionHistory(string userId, [FromQuery] PaginationParams parameters)
    {
        var currentUserId = _currentUserService.GetUserId();
        if (currentUserId != userId && !User.IsInRole("Admin"))
            return Forbid();

        var result = await _subscriptionService.GetSubscriptionHistoryAsync(userId, parameters);
        return Ok(result);
    }
}
