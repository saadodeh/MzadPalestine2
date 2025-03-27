namespace MzadPalestine.Core.DTOs.Listings;

public class PlaceBidDto
{
    public int AuctionId { get; set; }
    public decimal Amount { get; set; }
    public bool IsAutoBid { get; set; }
    public decimal? MaxAutoBidAmount { get; set; }
}
