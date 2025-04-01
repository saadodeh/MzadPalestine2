namespace MzadPalestine.Core.DTOs.Reports;

public class AddReportCommentDto
{
    public string ReportId { get; set; } = null!;
    public string Comment { get; set; } = null!;
}