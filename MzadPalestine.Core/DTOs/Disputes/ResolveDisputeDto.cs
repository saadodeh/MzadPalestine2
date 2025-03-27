using System.ComponentModel.DataAnnotations;

namespace MzadPalestine.Core.DTOs.Disputes;

public class ResolveDisputeDto
{
    [Required]
    public string Resolution { get; set; } = string.Empty;

    [Required]
    public DisputeResolution ResolutionType { get; set; }
}

public enum DisputeResolution
{
    RefundBuyer,
    ReleaseFunds,
    BanSeller,
    BanBuyer,
    Other
}
