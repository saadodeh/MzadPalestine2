using System.ComponentModel.DataAnnotations;

namespace MzadPalestine.Core.DTOs.Wallet;

public class DepositRequest
{
    [Required]
    [Range(0.01, double.MaxValue, ErrorMessage = "Amount must be greater than 0")]
    public decimal Amount { get; set; }

    [Required(ErrorMessage = "Payment method is required")]
    public string PaymentMethodId { get; set; } = string.Empty;
}

public partial class WithdrawRequest
{
    [Required]
    [Range(0.01, double.MaxValue, ErrorMessage = "Amount must be greater than 0")]
    public decimal Amount { get; set; }

    [Required(ErrorMessage = "Bank account information is required")]
    public string BankAccount { get; set; } = string.Empty;

    [Required]
    public string PaymentMethod { get; set; } = string.Empty;

    [Required]
    public string PaymentDetails { get; set; } = string.Empty;
}

public class TransferRequest
{
    [Required]
    [Range(0.01, double.MaxValue, ErrorMessage = "Amount must be greater than 0")]
    public decimal Amount { get; set; }

    [Required(ErrorMessage = "Recipient ID is required")]
    public string RecipientId { get; set; } = string.Empty;
}