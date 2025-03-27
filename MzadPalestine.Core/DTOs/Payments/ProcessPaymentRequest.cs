using System.ComponentModel.DataAnnotations;

namespace MzadPalestine.Core.DTOs.Payments;

public class ProcessPaymentRequest
{
    public string PaymentMethodId { get; set; } = string.Empty;
    public bool UseWalletBalance { get; set; }
}
