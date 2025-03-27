using MzadPalestine.Core.Models.Common;

namespace MzadPalestine.Core.Models;

public class SubscriptionPlan : BaseEntity
{
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public decimal Price { get; set; }
    public int DurationInDays { get; set; }
    public int MaxListings { get; set; }
    public int MaxAuctions { get; set; }
    public bool FeaturedListings { get; set; }
    public bool PrioritySupport { get; set; }
    public bool IsActive { get; set; } = true;
    public ICollection<Subscription> Subscriptions { get; set; } = new List<Subscription>();
}
