using MzadPalestine.Core.Models.Common;

namespace MzadPalestine.Core.Models;

public class Category : BaseEntity
{
    public int Id { get; set; }
    public string Name { get; set; } = null!;
    public string Description { get; set; }
    public int? ParentId { get; set; }
    public string? ImageUrl { get; set; }
    public int DisplayOrder { get; set; }
    
    // Navigation properties
    public virtual Category? Parent { get; set; }
    public virtual ICollection<Category> Children { get; set; } = new List<Category>();
    public virtual ICollection<Listing> Listings { get; set; } = new List<Listing>();
}
