using System.ComponentModel.DataAnnotations;

namespace MzadPalestine.Core.DTOs.Messages;

public class SendMessageRequest
{
    [Required]
    public string RecipientId { get; set; } = string.Empty;

    [Required]
    [StringLength(1000)]
    public string Content { get; set; } = string.Empty;
}
