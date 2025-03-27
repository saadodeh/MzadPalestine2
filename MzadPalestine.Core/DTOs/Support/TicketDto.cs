using MzadPalestine.Core.DTOs.Users;

namespace MzadPalestine.Core.DTOs.Support;

public class TicketDto
{
    public int Id { get; set; }
    public string Subject { get; set; } = string.Empty;
    public string Message { get; set; } = string.Empty;
    public string Priority { get; set; } = string.Empty;
    public string Status { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; }
    public UserDto User { get; set; } = null!;
    public List<TicketReplyDto> Replies { get; set; } = new();
    public List<string>? Attachments { get; set; }
}
