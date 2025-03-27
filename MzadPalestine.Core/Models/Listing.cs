using MzadPalestine.Core.Models.Common;
using MzadPalestine.Core.Models.Enums;
using MzadPalestine.Core.Interfaces.Common;

namespace MzadPalestine.Core.Models;

public class Listing : BaseEntity, ISoftDelete
{
    public int Id { get; set; }
    public string Title { get; set; }
    public string Description { get; set; }
    public decimal StartingPrice { get; set; }
    public decimal ReservePrice { get; set; }
    public string UserId { get; set; }
    public int CategoryId { get; set; }
    public int LocationId { get; set; }
    public DateTime CreatedAt { get; set; }
    public ListingStatus Status { get; set; }
    public bool IsAuction { get; set; }
    public bool IsDeleted { get; set; }
    public DateTime? DeletedAt { get; set; }
    public string? DeletedBy { get; set; }
        // «·Œ’«∆’ «·√Œ—Ï
        public string SellerId { get; set; }  // ﬁœ  ﬂÊ‰ „‰ «·‰Ê⁄ string √Ê int° Õ”» ÂÌﬂ· „‘—Ê⁄ﬂ
        public virtual ApplicationUser Seller { get; set; }  // Ì„À· «·„” Œœ„ («·»«∆⁄
    // Navigation properties
    public ApplicationUser User { get; set; }
    public Category Category { get; set; }
    public Location Location { get; set; }
    public ICollection<Tag> Tags { get; set; } = new List<Tag>();
    public ICollection<Image> Images { get; set; } = new List<Image>();
    public ICollection<Report> Reports { get; set; } = new List<Report>();
    public Auction Auction { get; set; }
}
