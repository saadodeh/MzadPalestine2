using MzadPalestine.Core.Models.Common;
using MzadPalestine.Core.Models.Enums;

namespace MzadPalestine.Core.Models;

public class Invoice : BaseEntity
{
    public string UserId { get; set; } = string.Empty;
    public int AuctionId { get; set; }
    public decimal Amount { get; set; }
    public DateTime DueDate { get; set; }
    public Enums.InvoiceStatus Status { get; set; }
    public DateTime IssuedAt { get; set; }

    // Navigation properties
    public virtual ApplicationUser User { get; set; } = null!;
    public virtual Auction Auction { get; set; } = null!;
}
