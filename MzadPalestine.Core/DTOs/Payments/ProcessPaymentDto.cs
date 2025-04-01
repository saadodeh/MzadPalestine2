namespace MzadPalestine.Core.DTOs.Payments;

public class ProcessPaymentDto
{
    public decimal Amount { get; set; }
    public string Currency { get; set; } = string.Empty;
    public string PaymentMethodId { get; set; } = string.Empty;
}