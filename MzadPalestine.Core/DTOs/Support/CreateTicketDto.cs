using System.ComponentModel.DataAnnotations;

namespace MzadPalestine.Core.DTOs.Support;

public class CreateTicketDto
{
    [Required]
    [StringLength(255)]
    public string Subject { get; set; } = string.Empty;

    [Required]
    [StringLength(2000)]
    public string Message { get; set; } = string.Empty;

    public TicketPriority Priority { get; set; } = TicketPriority.Medium;

    public List<string>? Attachments { get; set; }
}

public enum TicketPriority
{
    Low,
    Medium,
    High
}
