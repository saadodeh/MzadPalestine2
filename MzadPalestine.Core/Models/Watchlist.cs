using MzadPalestine.Core.Models.Common;

namespace MzadPalestine.Core.Models;

public class Watchlist : BaseEntity
{
    public string UserId { get; set; } = string.Empty;
    public int ListingId { get; set; }
    public DateTime AddedAt { get; set; }

    // Navigation properties
    public virtual ApplicationUser User { get; set; } = null!;
    public virtual Listing Listing { get; set; } = null!;
}
