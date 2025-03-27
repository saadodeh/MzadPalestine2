namespace MzadPalestine.Core.DTOs.Listings;

public class CreateListingDto
{
    public string Title { get; set; } = null!;
    public string Description { get; set; } = null!;
    public decimal StartingPrice { get; set; }
    public int CategoryId { get; set; }
    public int LocationId { get; set; }
    public string? ContactPhone { get; set; }
    public bool IsAuction { get; set; }
    public List<string>? ImageUrls { get; set; }
    
    // Auction specific properties
    public DateTime? StartTime { get; set; }
    public DateTime? EndTime { get; set; }
    public decimal? ReservePrice { get; set; }
    public decimal? MinimumBidIncrement { get; set; }
}
