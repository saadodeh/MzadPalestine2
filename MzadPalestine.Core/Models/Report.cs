using MzadPalestine.Core.Models.Common;
using MzadPalestine.Core.Models.Enums;

namespace MzadPalestine.Core.Models;

public class Report : BaseEntity
{
    public int Id { get; set; }
    public string Title { get; set; }
    public string ReporterId { get; set; }
    public string ReportedUserId { get; set; }
    public string ResolvedById { get; set; }
    public string Description { get; set; }
    public DateTime CreatedAt { get; set; }
    public ReportStatus Status { get; set; }

    // العلاقات
    public ApplicationUser Reporter { get; set; }
    public ApplicationUser ResolvedBy { get; set; }
    public Listing Listing { get; set; }
    public int ListingId { get; set; }
}

public enum ReportStatus
{
    Pending,
    Reviewed,
    Resolved,
    Dismissed
}
