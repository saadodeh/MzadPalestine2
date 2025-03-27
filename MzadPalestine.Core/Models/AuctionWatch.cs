using MzadPalestine.Core.Models.Common;

namespace MzadPalestine.Core.Models;

public class AuctionWatch : BaseEntity
{
    public int AuctionId { get; set; }
    public string UserId { get; set; } = string.Empty;
    public bool NotifyOnNewBid { get; set; }
    public bool NotifyOnEndingSoon { get; set; }
    public bool NotifyOnPriceChange { get; set; }
    
    // Navigation properties
    public virtual Auction Auction { get; set; } = null!;
    public virtual ApplicationUser User { get; set; } = null!;
}
