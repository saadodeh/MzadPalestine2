using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MzadPalestine.Core.DTOs.Messages;
using MzadPalestine.Core.Interfaces.Services;
using MzadPalestine.Core.Models.Common;

namespace MzadPalestine.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class MessageController : ControllerBase
{
    private readonly IMessageService _messageService;
    private readonly ICurrentUserService _currentUserService;

    public MessageController(IMessageService messageService, ICurrentUserService currentUserService)
    {
        _messageService = messageService;
        _currentUserService = currentUserService;
    }

    [HttpPost("send")]
    public async Task<IActionResult> SendMessage([FromBody] SendMessageDto messageDto)
    {
        var userId = _currentUserService.GetUserId();
        var result = await _messageService.SendMessageAsync(userId, messageDto);
        
        if (!result.Succeeded)
            return BadRequest(result.Message);

        return Ok(result.Data);
    }

    [HttpGet("inbox/{userId}")]
    public async Task<IActionResult> GetInboxMessages(string userId, [FromQuery] PaginationParams parameters)
    {
        var currentUserId = _currentUserService.GetUserId();
        if (currentUserId != userId)
            return Forbid();

        var result = await _messageService.GetInboxMessagesAsync(userId, parameters);
        return Ok(result);
    }

    [HttpGet("sent/{userId}")]
    public async Task<IActionResult> GetSentMessages(string userId, [FromQuery] PaginationParams parameters)
    {
        var currentUserId = _currentUserService.GetUserId();
        if (currentUserId != userId)
            return Forbid();

        var result = await _messageService.GetSentMessagesAsync(userId, parameters);
        return Ok(result);
    }

    [HttpDelete("delete/{messageId}")]
    public async Task<IActionResult> DeleteMessage(int messageId)
    {
        var userId = _currentUserService.GetUserId();
        var result = await _messageService.DeleteMessageAsync(userId, messageId);
        
        if (!result.Succeeded)
            return BadRequest(result.Message);

        return Ok(new { message = "Message deleted successfully" });
    }

    [HttpGet("conversation/{otherUserId}")]
    public async Task<IActionResult> GetConversation(string otherUserId, [FromQuery] PaginationParams parameters)
    {
        var userId = _currentUserService.GetUserId();
        var result = await _messageService.GetConversationAsync(userId, otherUserId, parameters);
        return Ok(result);
    }

    [HttpPut("mark-read/{messageId}")]
    public async Task<IActionResult> MarkMessageAsRead(int messageId)
    {
        var userId = _currentUserService.GetUserId();
        var result = await _messageService.MarkMessageAsReadAsync(userId, messageId);
        
        if (!result.Succeeded)
            return BadRequest(result.Message);

        return Ok(new { message = "Message marked as read" });
    }
}
