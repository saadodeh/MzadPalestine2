using MzadPalestine.Core.Models.Common;

namespace MzadPalestine.Core.Models;

public class Notification : BaseEntity
{
    public int Id { get; set; }
    public string Title { get; set; }
    public string Message { get; set; }
    public string ReferenceId { get; set; }
    public NotificationType Type { get; set; }
    public string UserId { get; set; }
    public DateTime CreatedAt { get; set; }
    public bool IsRead { get; set; }
    public string Status { get; set; }

    // Navigation properties
    public virtual ApplicationUser User { get; set; } = null!;
}

public enum NotificationType
{
    Bid,
    Payment,
    General
}

public enum NotificationStatus
{
    Unread,
    Read
}
