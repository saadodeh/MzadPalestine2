using MzadPalestine.Core.DTOs.Bids;
using MzadPalestine.Core.DTOs.Listings;
using MzadPalestine.Core.DTOs.Users;

namespace MzadPalestine.Core.DTOs.Auctions;

public class AuctionDetailsDto
{
    public int Id { get; set; }
    public ListingDto Listing { get; set; } = null!;
    public DateTime StartTime { get; set; }
    public DateTime EndTime { get; set; }
    public decimal ReservePrice { get; set; }
    public decimal CurrentBid { get; set; }
    public decimal BidIncrement { get; set; }
    public string Status { get; set; } = string.Empty;
    public string? ImageUrl { get; set; }
    public UserDto? Winner { get; set; }
    public List<MzadPalestine.Core.DTOs.Bids.BidDto> RecentBids { get; set; } = new();
    public int TotalBids { get; set; }
    public int WatchCount { get; set; }
    public bool IsWatched { get; set; }
    public bool HasAutoBid { get; set; }
    public decimal? AutoBidLimit { get; set; }
}