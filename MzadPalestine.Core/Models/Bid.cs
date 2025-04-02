using MzadPalestine.Core.Models.Common;

namespace MzadPalestine.Core.Models;

public class Bid : BaseEntity
{
    public int AuctionId { get; set; }
    public string BidderId { get; set; } = string.Empty;
    public decimal Amount { get; set; }
    public bool IsAutoBid { get; set; }
    public decimal? MaxAutoBidAmount { get; set; }
    public bool IsCancelled { get; set; }
    public byte[] RowVersion { get; set; } = Array.Empty<byte>();
    
    // Navigation properties
    public virtual Auction Auction { get; set; } = null!;
    public virtual ApplicationUser Bidder { get; set; } = null!;
}
