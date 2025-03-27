using MzadPalestine.Core.DTOs.Users;

namespace MzadPalestine.Core.DTOs.Support;

public class TicketReplyDto
{
    public int Id { get; set; }
    public string Message { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; }
    public UserDto User { get; set; } = null!;
    public List<string>? Attachments { get; set; }
}
