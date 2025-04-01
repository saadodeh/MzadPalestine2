using System.ComponentModel.DataAnnotations;

namespace MzadPalestine.Core.DTOs.Messages;

public class SendMessageDto
{
    [Required]
    public string RecipientId { get; set; } = string.Empty;

    [Required]
    [StringLength(1000, MinimumLength = 1)]
    public string Content { get; set; } = string.Empty;

    public string? Subject { get; set; }
}