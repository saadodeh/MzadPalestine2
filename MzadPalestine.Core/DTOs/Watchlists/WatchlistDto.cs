using System;

namespace MzadPalestine.Core.DTOs.Watchlists;

public class WatchlistDto
{
    public int Id { get; set; }
    public int ListingId { get; set; }
    public string UserId { get; set; } = string.Empty;
    public DateTime AddedAt { get; set; }
    public string ListingTitle { get; set; } = string.Empty;
    public decimal CurrentPrice { get; set; }
    public DateTime? AuctionEndDate { get; set; }
    public string? ListingThumbnailUrl { get; set; }
}