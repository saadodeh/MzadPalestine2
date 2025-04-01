using MzadPalestine.Core.Models.Enums;
using MzadPalestine.Core.DTOs.Bids;

namespace MzadPalestine.Core.DTOs.Auctions;

public class AuctionDto
{
    public int Id { get; set; }
    public int ListingId { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public decimal ReservePrice { get; set; }
    public decimal CurrentPrice { get; set; }
    public AuctionStatus Status { get; set; }
    public decimal MinimumBidIncrement { get; set; }
    
    public string? WinningBidderId { get; set; }
    public string? WinningBidderName { get; set; }
    public int BidCount { get; set; }
    public List<BidDto> RecentBids { get; set; } = new();
    public bool HasReservePrice { get; set; }
    public bool ReservePriceMet { get; set; }
    public TimeSpan TimeRemaining { get; set; }
}