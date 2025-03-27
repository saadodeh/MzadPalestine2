using MzadPalestine.Core.Models.Common;
using MzadPalestine.Core.Models.Enums;

namespace MzadPalestine.Core.Models;

public class CustomerSupportTicket : BaseEntity
{
    public int Id { get; set; }
    public string UserId { get; set; }
    public string Subject { get; set; }
    public string Description { get; set; }
    public TicketStatus Status { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
    public string? AdminId { get; set; }
    public DateTime? ClosedAt { get; set; }
    public ApplicationUser User { get; set; } = null!;
    public ApplicationUser? Admin { get; set; }
    public ICollection< Message> Messages { get; set; } = new List<Message>();
}

public enum TicketPriority
{
    Low,
    Medium,
    High
}

public enum TicketStatus
{
    Open,
    Resolved,
    Closed
}
