using MzadPalestine.Core.Models.Enums;

namespace MzadPalestine.Core.DTOs.Listings;

public class AuctionDto
{
    public int Id { get; set; }
    public int ListingId { get; set; }
    public DateTime StartTime { get; set; }
    public DateTime EndTime { get; set; }
    public decimal ReservePrice { get; set; }
    public decimal CurrentBid { get; set; }
    public AuctionStatus Status { get; set; }
    public decimal MinimumBidIncrement { get; set; }
    
    public string? WinningBidderId { get; set; }
    public string? WinningBidderName { get; set; }
    public int TotalBids { get; set; }
    public List<BidDto> RecentBids { get; set; } = new();
    public bool HasReservePrice { get; set; }
    public bool ReservePriceMet { get; set; }
    public TimeSpan TimeRemaining { get; set; }
}
