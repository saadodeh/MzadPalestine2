using MzadPalestine.Core.Models.Common;

namespace MzadPalestine.Core.Models;

public class Subscription : BaseEntity
{
    public string UserId { get; set; } = string.Empty;
    public string Plan { get; set; } = string.Empty;
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public DateTime RenewalDate { get; set; }
    public SubscriptionStatus Status { get; set; }

    // Navigation properties
    public virtual ApplicationUser User { get; set; } = null!;
}

public enum SubscriptionStatus
{
    Active,
    Canceled,
    Expired
}
