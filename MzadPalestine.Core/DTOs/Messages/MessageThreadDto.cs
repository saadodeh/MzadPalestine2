namespace MzadPalestine.Core.DTOs.Messages;

public class MessageThreadDto
{
    public string UserId { get; set; } = string.Empty;
    public string UserName { get; set; } = string.Empty;
    public string? UserAvatar { get; set; }
    public string? LastMessage { get; set; }
    public DateTime? LastMessageDate { get; set; }
    public int UnreadCount { get; set; }
}
