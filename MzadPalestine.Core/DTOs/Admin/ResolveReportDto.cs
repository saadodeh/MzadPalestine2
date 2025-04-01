using System.ComponentModel.DataAnnotations;

namespace MzadPalestine.Core.DTOs.Admin;

public class ResolveReportDto
{
    [Required]
    public int ReportId { get; set; }

    [Required]
    [StringLength(500)]
    public string Resolution { get; set; } = string.Empty;

    public bool NotifyReporter { get; set; } = true;
    public bool NotifyReportedUser { get; set; } = true;

    [StringLength(500)]
    public string? AdminNotes { get; set; }
}