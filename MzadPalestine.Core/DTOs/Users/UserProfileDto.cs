using MzadPalestine.Core.DTOs.Auctions;
using MzadPalestine.Core.DTOs.Listings;
using MzadPalestine.Core.DTOs.Reviews;
using MzadPalestine.Core.DTOs.Users;

namespace MzadPalestine.Core.DTOs.Users;

public class UserProfileDto
{
    public UserDto User { get; set; } = null!;
    public List<ListingDto> ActiveListings { get; set; } = new();
    public List<ListingDto> SoldListings { get; set; } = new();
    public List<AuctionDto> WonAuctions { get; set; } = new();
    public List<ReviewDto> Reviews { get; set; } = new();
    public UserStatsDto Stats { get; set; } = null!;
}

public class UserStatsDto
{
    public int TotalListings { get; set; }
    public int ActiveListings { get; set; }
    public int SoldListings { get; set; }
    public int WonAuctions { get; set; }
    public int TotalReviews { get; set; }
    public decimal AverageRating { get; set; }
}
