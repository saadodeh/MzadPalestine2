namespace MzadPalestine.Core.DTOs.Payments;

public class PaymentResultDto
{
    public bool Success { get; set; }
    public string TransactionId { get; set; } = string.Empty;
    public decimal Amount { get; set; }
    public string PaymentMethod { get; set; } = string.Empty;
}
