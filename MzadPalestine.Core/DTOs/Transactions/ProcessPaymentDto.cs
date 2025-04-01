using System;

namespace MzadPalestine.Core.DTOs.Transactions;

public class ProcessPaymentDto
{
    public string PaymentMethodId { get; set; } = string.Empty;
    public decimal Amount { get; set; }
    public string Currency { get; set; } = "USD";
    public string Description { get; set; } = string.Empty;
    public int? AuctionId { get; set; }
    public string? ReferenceNumber { get; set; }
}