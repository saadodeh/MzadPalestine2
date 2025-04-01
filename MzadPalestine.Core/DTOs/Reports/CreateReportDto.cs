namespace MzadPalestine.Core.DTOs.Reports;

public class CreateReportDto
{
    public string Title { get; set; } = null!;
    public string Description { get; set; } = null!;
    public string? ReportedItemId { get; set; }
    public string? ReportedUserId { get; set; }
    public string ReportType { get; set; } = null!;
}