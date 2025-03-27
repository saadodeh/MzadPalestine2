namespace MzadPalestine.Core.DTOs.Listings;

public class BidDto
{
    public int Id { get; set; }
    public int AuctionId { get; set; }
    public string BidderId { get; set; } = null!;
    public string BidderName { get; set; } = null!;
    public string? BidderProfilePicture { get; set; }
    public decimal Amount { get; set; }
    public DateTime BidTime { get; set; }
    public bool IsWinning { get; set; }
    public bool IsAutoBid { get; set; }
}
