using MzadPalestine.Core.Models.Common;
using MzadPalestine.Core.Models.Enums;

namespace MzadPalestine.Core.Models;

public class WalletTransaction : BaseEntity
{
    public int Id { get; set; }
    public string UserId { get; set; } = string.Empty;
    public decimal Amount { get; set; }
    public TransactionType Type { get; set; }
    public TransactionStatus Status { get; set; }
    public string Description { get; set; } = string.Empty;
    public int? PaymentId { get; set; }
    public DateTime CreatedAt { get; set; }

    // العلاقات
    public ApplicationUser User { get; set; }
    public Payment Payment { get; set; }
    public int WalletId { get; set; }  // تأكد من وجود هذا الحقل
    public virtual Wallet Wallet { get; set; }
}
