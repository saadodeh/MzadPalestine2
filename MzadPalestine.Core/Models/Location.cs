using MzadPalestine.Core.Models.Common;

namespace MzadPalestine.Core.Models;

public class Location : BaseEntity
{
    public int Id { get; set; }
    public string Name { get; set; } = null!;
    public string Description { get; set; }
    public int? ParentLocationId { get; set; }
    public double? Latitude { get; set; }
    public double? Longitude { get; set; }
    
    // Navigation properties
    public virtual Location? ParentLocation { get; set; }
    public virtual ICollection<Location> Children { get; set; } = new List<Location>();
    public virtual ICollection<Listing> Listings { get; set; } = new List<Listing>();
}
