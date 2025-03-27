using MzadPalestine.Core.Models.Common;

namespace MzadPalestine.Core.Models;

public class Tag : BaseEntity
{
    public string Name { get; set; } = string.Empty;

    // Navigation properties
    public virtual ICollection<Listing> Listings { get; set; } = new List<Listing>();
    public virtual ICollection<ListingTag> ListingTags { get; set; } = new List<ListingTag>();
}
