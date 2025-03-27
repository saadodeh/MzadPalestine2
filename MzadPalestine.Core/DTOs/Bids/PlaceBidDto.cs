using System.ComponentModel.DataAnnotations;

namespace MzadPalestine.Core.DTOs.Bids;

public class PlaceBidDto
{
    [Required]
    public int AuctionId { get; set; }

    [Required]
    [Range(0.01, double.MaxValue)]
    public decimal Amount { get; set; }

    public bool IsAutoBid { get; set; }

    [Range(0.01, double.MaxValue)]
    public decimal? MaxAutoBidAmount { get; set; }
}
