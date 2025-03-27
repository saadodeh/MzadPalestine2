using MzadPalestine.Core.Models.Common;

namespace MzadPalestine.Core.Models;

public class AuditLog : BaseEntity
{
    public string UserId { get; set; } = string.Empty;
    public string Type { get; set; } = string.Empty;
    public string TableName { get; set; } = string.Empty;
    public DateTime DateTime { get; set; }
    public string? OldValues { get; set; }
    public string? NewValues { get; set; }
    public string? AffectedColumns { get; set; }
    public string PrimaryKey { get; set; } = string.Empty;
    
    // Navigation properties
    public virtual ApplicationUser User { get; set; } = null!;
}
