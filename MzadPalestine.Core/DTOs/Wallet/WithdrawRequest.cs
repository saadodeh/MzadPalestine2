using System.ComponentModel.DataAnnotations;

namespace MzadPalestine.Core.DTOs.Wallet;

public class WithdrawRequest
{
    [Required]
    [Range(1, double.MaxValue)]
    public decimal Amount { get; set; }

    [Required]
    public string PaymentMethod { get; set; } = string.Empty;

    [Required]
    public string PaymentDetails { get; set; } = string.Empty;
}
