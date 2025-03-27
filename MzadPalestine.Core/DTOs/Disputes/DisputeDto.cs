using MzadPalestine.Core.DTOs.Users;

namespace MzadPalestine.Core.DTOs.Disputes;

public class DisputeDto
{
    public int Id { get; set; }
    public int AuctionId { get; set; }
    public string Reason { get; set; } = string.Empty;
    public string Status { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; }
    public UserDto User { get; set; } = null!;
    public UserDto? ResolvedBy { get; set; }
    public List<string>? EvidenceUrls { get; set; }
}
