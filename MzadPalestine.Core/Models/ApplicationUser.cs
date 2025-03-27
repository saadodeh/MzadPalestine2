using Microsoft.AspNetCore.Identity;
using MzadPalestine.Core.Models.Enums;
using MzadPalestine.Core.Models;

public class ApplicationUser : IdentityUser
{
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
    public UserRole Role { get; set; }
    public bool IsVerified { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public string? ProfilePictureUrl { get; set; }
    public bool IsActive { get; set; } = true;
    public bool IsDeleted { get; set; }
    public DateTime? DeletedAt { get; set; }

    // Navigation properties
    public virtual ICollection<Listing> Listings { get; set; } = new List<Listing>();
    public virtual ICollection<Bid> Bids { get; set; } = new List<Bid>();
    public virtual ICollection<Review> Reviews { get; set; } = new List<Review>(); // «·„—«Ã⁄«  «· Ì ﬁ«„ »Â« «·„” Œœ„
    public virtual ICollection<Review> ReceivedReviews { get; set; } = new List<Review>(); // «·„—«Ã⁄«  «· Ì  ·ﬁ«Â« «·„” Œœ„

    // „—«Ã⁄ («·„—«Ã⁄«  «· Ì  ·ﬁ«Â« «·„” Œœ„)
    public ICollection<Review> Reviewed { get; set; } // ›Ì Õ«· ﬂ«‰ ·œÌﬂ Œ’«∆’ „Œ ·›… · ﬁ”Ì„ «·„—«Ã⁄« .

    public virtual ICollection<IdentityUserRole<string>> UserRoles { get; set; } = new List<IdentityUserRole<string>>();

    public virtual Wallet? Wallet { get; set; }
}
