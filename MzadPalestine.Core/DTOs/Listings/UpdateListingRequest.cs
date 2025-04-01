using Microsoft.AspNetCore.Http;

namespace MzadPalestine.Core.DTOs.Listings;

public class UpdateListingRequest
{
    public string Title { get; set; } = null!;
    public string Description { get; set; } = null!;
    public decimal StartingPrice { get; set; }
    public int CategoryId { get; set; }
    public int LocationId { get; set; }
    public bool IsAuction { get; set; }
    public DateTime? AuctionStartDate { get; set; }
    public DateTime? AuctionEndDate { get; set; }
}