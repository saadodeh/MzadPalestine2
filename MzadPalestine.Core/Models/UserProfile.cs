using MzadPalestine.Core.Models.Common;

namespace MzadPalestine.Core.Models;

public class UserProfile : BaseEntity
{
    public string UserId { get; set; } = string.Empty;
    public string? Bio { get; set; }
    public string? AvatarUrl { get; set; }
    public string? PhoneNumber { get; set; }
    public string? Address { get; set; }
    public bool IsVerified { get; set; }
    public DateTime? LastLoginDate { get; set; }
    public string? PreferredLanguage { get; set; }
    public string? TimeZone { get; set; }
    
    // Navigation properties
    public virtual ApplicationUser User { get; set; } = null!;
}
