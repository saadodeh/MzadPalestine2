namespace MzadPalestine.Core.DTOs.Wallet;

public class WalletTransactionDto
{
    public int Id { get; set; }
    public decimal Amount { get; set; }
    public string Type { get; set; } = string.Empty; // Deposit, Withdrawal, Transfer
    public string Status { get; set; } = string.Empty; // Pending, Completed, Failed
    public string? Description { get; set; }
    public string? ReferenceId { get; set; }
    public DateTime CreatedAt { get; set; }
}
