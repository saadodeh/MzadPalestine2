using System.ComponentModel.DataAnnotations;

namespace MzadPalestine.Core.DTOs.Admin;

public class DeleteAuctionDto
{
    [Required]
    public int AuctionId { get; set; }

    [Required]
    [StringLength(500)]
    public string Reason { get; set; } = string.Empty;

    public bool NotifyUser { get; set; } = true;
}