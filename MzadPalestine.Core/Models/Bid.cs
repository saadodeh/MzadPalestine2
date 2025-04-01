using MzadPalestine.Core.Models.Common;

namespace MzadPalestine.Core.Models;

public class Bid : BaseEntity
{
    public int Id { get; set; }
    public int AuctionId { get; set; }
    public string BidderId { get; set; }
    public string UserId { get; set; }
    public decimal Amount { get; set; }
    public DateTime CreatedAt { get; set; }
    public bool IsAutoBid { get; set; }
    public decimal? MaxAutoBidAmount { get; set; }
    public bool IsCancelled { get; set; }
    public byte[] RowVersion { get; set; }
    
    // Navigation properties
    public virtual Auction Auction { get; set; } = null!;
    public virtual ApplicationUser Bidder { get; set; } = null!;
    public object User { get; set; }
}
