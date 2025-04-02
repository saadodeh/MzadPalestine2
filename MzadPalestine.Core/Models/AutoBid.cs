using MzadPalestine.Core.Models.Common;

namespace MzadPalestine.Core.Models;

public class AutoBid : BaseEntity
{
    public int AuctionId { get; set; }
    public string BidderId { get; set; } = string.Empty;
    public decimal MaxAmount { get; set; }
    public bool IsActive { get; set; }

    // Navigation properties
    public virtual Auction Auction { get; set; } = null!;
    public virtual ApplicationUser Bidder { get; set; } = null!;
}

public enum AutoBidStatus
{
    Active,
    Disabled
}
