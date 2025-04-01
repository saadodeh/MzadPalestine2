namespace MzadPalestine.Core.DTOs.Reports;

public class UpdateReportStatusDto
{
    public string ReportId { get; set; } = null!;
    public string Status { get; set; } = null!;
    public string? Resolution { get; set; }
}