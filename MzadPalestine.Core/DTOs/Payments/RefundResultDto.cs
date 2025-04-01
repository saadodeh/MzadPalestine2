using System.ComponentModel.DataAnnotations;

namespace MzadPalestine.Core.DTOs.Payments;

public class RefundResultDto
{
    [Required]
    public string TransactionId { get; set; } = string.Empty;

    [Required]
    public bool Success { get; set; }

    public string? RefundTransactionId { get; set; }

    public decimal Amount { get; set; }

    public string? Currency { get; set; }

    public string? Status { get; set; }

    public string? ErrorMessage { get; set; }

    public DateTime RefundDate { get; set; }
}