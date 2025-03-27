using System.ComponentModel.DataAnnotations;

namespace MzadPalestine.Core.DTOs.Disputes;

public class CreateDisputeDto
{
    [Required]
    public int AuctionId { get; set; }

    [Required]
    [StringLength(1000)]
    public string Reason { get; set; } = string.Empty;

    public List<string>? EvidenceUrls { get; set; }
}
