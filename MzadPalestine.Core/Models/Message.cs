using MzadPalestine.Core.Models.Common;

namespace MzadPalestine.Core.Models;

public class Message : BaseEntity
{
    public string SenderId { get; set; } = null!;
    public string ReceiverId { get; set; } = null!;
    public string Content { get; set; } = null!;
    public bool IsRead { get; set; }
    public int? ListingId { get; set; }
    
    // Navigation properties
    public virtual ApplicationUser Sender { get; set; } = null!;
    public virtual ApplicationUser Receiver { get; set; } = null!;
    public virtual Listing? Listing { get; set; }
}
