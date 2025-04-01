using System;

namespace MzadPalestine.Core.DTOs.Bids;

public class BidDto
{
    public int Id { get; set; }
    public int AuctionId { get; set; }
    public string BidderId { get; set; }
    public string UserName { get; set; }
    public decimal Amount { get; set; }
    public bool IsAutoBid { get; set; }
    public decimal? MaxAutoBidAmount { get; set; }
    public DateTime CreatedAt { get; set; }
    public bool IsCancelled { get; set; }
}