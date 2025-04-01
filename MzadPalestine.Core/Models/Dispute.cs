using MzadPalestine.Core.Models.Common;

namespace MzadPalestine.Core.Models;

public class Dispute : BaseEntity
{
    public string UserId { get; set; } = string.Empty;
    public int AuctionId { get; set; }
    public string Reason { get; set; } = string.Empty;
    public DisputeStatus Status { get; set; }
    public DateTime CreatedAt { get; set; }
    public string? ResolvedById { get; set; }
    public bool IsResolved { get; set; }

    // Navigation properties
    public virtual ApplicationUser User { get; set; } = null!;
    public virtual ApplicationUser? ResolvedBy { get; set; }
    public virtual Auction Auction { get; set; } = null!;
}

public enum DisputeStatus
{
    Open,
    Resolved,
    Closed
}
