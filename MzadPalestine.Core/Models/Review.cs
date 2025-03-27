using MzadPalestine.Core.Models;
using MzadPalestine.Core.Models.Common;

public class Review : BaseEntity
{
    public int Id { get; set; }

    // المستخدم الذي قام بكتابة المراجعة
    public int ReviewerId { get; set; }
    public virtual ApplicationUser Reviewer { get; set; }

    // المستخدم الذي تم تقييمه
    public string RevieweeId { get; set; }
    public virtual ApplicationUser Reviewee { get; set; }

    // في حالة كان التقييم مرتبطًا بمزاد معين
    public int? AuctionId { get; set; }
    public virtual Auction Auction { get; set; }

    // التقييم يجب أن يكون بين 1 و 5
    public int Rating { get; set; }

    // النص المرافق للتقييم
    public string Comment { get; set; }

    // تاريخ إنشاء المراجعة
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    // التحقق من صحة التقييم
    public void ValidateRating()
    {
        if (Rating < 1 || Rating > 5)
        {
            throw new ArgumentOutOfRangeException(nameof(Rating), "Rating must be between 1 and 5.");
        }
    }
}
