using System.ComponentModel.DataAnnotations;
using MzadPalestine.Core.Models;

namespace MzadPalestine.Core.DTOs.Notifications;

public class SendNotificationDto
{
    [Required]
    public string UserId { get; set; } = string.Empty;

    [Required]
    public string Title { get; set; } = string.Empty;

    [Required]
    public string Message { get; set; } = string.Empty;

    [Required]
    public NotificationType Type { get; set; }

    public string? ReferenceId { get; set; }
}