using MzadPalestine.Core.Models.Common;

namespace MzadPalestine.Core.Models;

public class ListingImage : BaseEntity
{
    public int ListingId { get; set; }
    public string ImageUrl { get; set; } = string.Empty;
    public string? ThumbnailUrl { get; set; }
    public bool IsMain { get; set; }
    public int DisplayOrder { get; set; }
    
    // Navigation properties
    public virtual Listing Listing { get; set; } = null!;
}
