using MzadPalestine.Core.Models.Common;

namespace MzadPalestine.Core.Models;

public class ReportComment : BaseEntity
{
    public string Content { get; set; } = string.Empty;
    public string UserId { get; set; } = string.Empty;
    public ApplicationUser User { get; set; } = null!;
    public int ReportId { get; set; }
    public Report Report { get; set; } = null!;
}
