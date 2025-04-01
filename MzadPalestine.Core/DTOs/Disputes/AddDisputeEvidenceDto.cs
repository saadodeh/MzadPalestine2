using Microsoft.AspNetCore.Http;

namespace MzadPalestine.Core.DTOs.Disputes;

public class AddDisputeEvidenceRequestDto
{
    public string Description { get; set; } = string.Empty;
    public IFormFile? File { get; set; }
    public string EvidenceType { get; set; } = string.Empty;
}