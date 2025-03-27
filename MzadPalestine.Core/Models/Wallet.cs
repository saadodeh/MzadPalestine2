using MzadPalestine.Core.Models.Common;

namespace MzadPalestine.Core.Models;

public class Wallet : BaseEntity
{
    public int Id { get; set; }
    public int UserId { get; set; } 
    public decimal Balance { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
    public byte[] RowVersion { get; set; }  // إضافة للتعامل مع التزامن
    public bool IsActive { get; set; } = true;

    // Navigation properties
    public virtual ApplicationUser User { get; set; } = null!;
    public virtual ICollection<WalletTransaction> Transactions { get; set; } = new List<WalletTransaction>();
}
