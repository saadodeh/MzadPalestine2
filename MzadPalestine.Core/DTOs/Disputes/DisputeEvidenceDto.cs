using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace MzadPalestine.Core.DTOs.Disputes;

public class DisputeEvidenceDto
{
    public int Id { get; set; }
    public int DisputeId { get; set; }

    [Required]
    [MaxLength(500)]
    public string Description { get; set; } = string.Empty;

    public string? FileUrl { get; set; }
    public DateTime CreatedAt { get; set; }
    public string CreatedBy { get; set; } = string.Empty;
}

public class AddDisputeEvidenceDto
{
    [Required]
    [MaxLength(500)]
    public string Description { get; set; } = string.Empty;

    [Required]
    public IFormFile File { get; set; } = null!;
    
    [Required]
    [MaxLength(50)]
    public string EvidenceType { get; set; } = string.Empty;
}