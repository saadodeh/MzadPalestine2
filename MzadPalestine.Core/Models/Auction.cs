using MzadPalestine.Core.Models.Common;
using MzadPalestine.Core.Models.Enums;

namespace MzadPalestine.Core.Models;

public class Auction : BaseEntity
{
    public int ListingId { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public decimal CurrentPrice { get; set; }
    public decimal MinimumBidIncrement { get; set; }
    public AuctionStatus Status { get; set; }
    public string? WinnerId { get; set; }
    public PaymentStatus PaymentStatus { get; set; }
    public string? PaymentTransactionId { get; set; }

    // العلاقات
    public Listing Listing { get; set; }
    public ApplicationUser Winner { get; set; }
    public ICollection<Bid> Bids { get; set; } = new List<Bid>();
    public ICollection<AutoBid> AutoBids { get; set; } = new List<AutoBid>();
    public ICollection<Review> Reviews { get; set; } = new List<Review>();
    public ICollection<AuctionWatch> AuctionWatches { get; set; } = new List<AuctionWatch>();
    public ICollection<Invoice> Invoices { get; set; } = new List<Invoice>();
}
