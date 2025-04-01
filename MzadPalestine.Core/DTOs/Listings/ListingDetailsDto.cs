using MzadPalestine.Core.DTOs.Auctions;
using MzadPalestine.Core.Models.Enums;

namespace MzadPalestine.Core.DTOs.Listings;

public class ListingDetailsDto
{
    public int Id { get; set; }
    public string Title { get; set; } = null!;
    public string Description { get; set; } = null!;
    public decimal StartingPrice { get; set; }
    public ListingStatus Status { get; set; }
    public int CategoryId { get; set; }
    public string? CategoryName { get; set; }
    public int LocationId { get; set; }
    public string? LocationName { get; set; }
    public string SellerId { get; set; } = null!;
    public string? SellerName { get; set; }
    public double SellerRating { get; set; }
    public List<string> Images { get; set; } = new();
    public DateTime CreatedAt { get; set; }
    public AuctionDto? Auction { get; set; }
}