using MzadPalestine.Core.Models.Common;

namespace MzadPalestine.Core.Models;

public class ListingTag : BaseEntity
{
    public int ListingId { get; set; }
    public int TagId { get; set; }

    // Navigation properties
    public virtual Listing Listing { get; set; } = null!;
    public virtual Tag Tag { get; set; } = null!;
}
