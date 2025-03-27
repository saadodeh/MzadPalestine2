using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MzadPalestine.Core.DTOs.Notifications;
using MzadPalestine.Core.Interfaces.Services;
using MzadPalestine.Core.Models.Common;

namespace MzadPalestine.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class NotificationController : ControllerBase
{
    private readonly INotificationService _notificationService;
    private readonly ICurrentUserService _currentUserService;

    public NotificationController(INotificationService notificationService, ICurrentUserService currentUserService)
    {
        _notificationService = notificationService;
        _currentUserService = currentUserService;
    }

    [HttpPost("send")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> SendNotification([FromBody] SendNotificationDto notificationDto)
    {
        var result = await _notificationService.SendNotificationAsync(notificationDto);
        
        if (!result.Succeeded)
            return BadRequest(result.Message);

        return Ok(new { message = "Notification sent successfully" });
    }

    [HttpGet("user/{userId}")]
    public async Task<IActionResult> GetUserNotifications(string userId, [FromQuery] PaginationParams parameters)
    {
        var currentUserId = _currentUserService.GetUserId();
        if (currentUserId != userId)
            return Forbid();

        var result = await _notificationService.GetUserNotificationsAsync(userId, parameters);
        return Ok(result);
    }

    [HttpPut("mark-read/{notificationId}")]
    public async Task<IActionResult> MarkNotificationAsRead(int notificationId)
    {
        var userId = _currentUserService.GetUserId();
        var result = await _notificationService.MarkNotificationAsReadAsync(userId, notificationId);
        
        if (!result.Succeeded)
            return BadRequest(result.Message);

        return Ok(new { message = "Notification marked as read" });
    }

    [HttpGet("unread-count")]
    public async Task<IActionResult> GetUnreadNotificationCount()
    {
        var userId = _currentUserService.GetUserId();
        var count = await _notificationService.GetUnreadNotificationCountAsync(userId);
        return Ok(new { count });
    }

    [HttpPut("mark-all-read")]
    public async Task<IActionResult> MarkAllNotificationsAsRead()
    {
        var userId = _currentUserService.GetUserId();
        await _notificationService.MarkAllNotificationsAsReadAsync(userId);
        return Ok(new { message = "All notifications marked as read" });
    }
}
