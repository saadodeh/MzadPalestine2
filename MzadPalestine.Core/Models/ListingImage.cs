using MzadPalestine.Core.Models.Common;

namespace MzadPalestine.Core.Models;

public class ListingImage : Image
{
    public string? ThumbnailUrl { get; set; }
    public bool IsMain { get; set; }
    public int DisplayOrder { get; set; }
    
    // Navigation properties
    public virtual Listing Listing { get; set; } = null!;

    // Map Url property from base class
    public string ImageUrl
    {
        get => Url;
        set => Url = value;
    }
}
