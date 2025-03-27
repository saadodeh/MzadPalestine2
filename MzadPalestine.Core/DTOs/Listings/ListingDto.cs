using MzadPalestine.Core.Models.Enums;

namespace MzadPalestine.Core.DTOs.Listings;

public class ListingDto
{
    public int Id { get; set; }
    public string Title { get; set; } = null!;
    public string Description { get; set; } = null!;
    public decimal StartingPrice { get; set; }
    public string CategoryName { get; set; } = null!;
    public string LocationName { get; set; } = null!;
    public ListingStatus Status { get; set; }
    public List<string> ImageUrls { get; set; } = new();
    public string? ContactPhone { get; set; }
    public bool IsAuction { get; set; }
    public DateTime CreatedAt { get; set; }
    
    // Seller information
    public string SellerId { get; set; } = null!;
    public string SellerName { get; set; } = null!;
    public string? SellerProfilePicture { get; set; }
    public double SellerRating { get; set; }
    
    // Auction specific properties
    public AuctionDto? Auction { get; set; }
}
