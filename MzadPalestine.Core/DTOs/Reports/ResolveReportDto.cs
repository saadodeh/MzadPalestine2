using System.ComponentModel.DataAnnotations;

namespace MzadPalestine.Core.DTOs.Reports;

public class ResolveReportDto
{
    [Required]
    public int ReportId { get; set; }

    [Required]
    [StringLength(500)]
    public string Resolution { get; set; } = string.Empty;

    [Required]
    public bool IsResolved { get; set; }

    public string? AdminNotes { get; set; }

    public bool TakeAction { get; set; }

    public string? ActionType { get; set; }
}