namespace MzadPalestine.Core.DTOs.Wallet;

public class WalletDto
{
    public decimal Balance { get; set; }
    public decimal PendingBalance { get; set; }
    public decimal TotalDeposited { get; set; }
    public decimal TotalWithdrawn { get; set; }
    public List<WalletTransactionDto> RecentTransactions { get; set; } = new();
}
