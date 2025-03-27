using System.ComponentModel.DataAnnotations;

namespace MzadPalestine.Core.DTOs.Auctions;

public class UpdateAuctionDto
{
    [Required]
    public DateTime StartTime { get; set; }

    [Required]
    public DateTime EndTime { get; set; }

    [Required]
    [Range(0.01, double.MaxValue)]
    public decimal ReservePrice { get; set; }

    [Required]
    [Range(0.01, double.MaxValue)]
    public decimal BidIncrement { get; set; }

    public string? ImageUrl { get; set; }
}
