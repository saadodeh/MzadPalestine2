using MzadPalestine.Core.Models.Common;

namespace MzadPalestine.Core.Models;

public class DisputeEvidence : BaseEntity
{
    public int DisputeId { get; set; }
    public string Description { get; set; } = string.Empty;
    public string FileUrl { get; set; } = string.Empty;
    public string FileType { get; set; } = string.Empty;
    public string SubmittedById { get; set; } = string.Empty;
    
    // Navigation properties
    public virtual Dispute Dispute { get; set; } = null!;
    public virtual ApplicationUser SubmittedBy { get; set; } = null!;
}
