namespace MzadPalestine.Core.DTOs.Wallet;

public class WalletDto
{
    public int Id { get; set; }
    public decimal Balance { get; set; }
    public decimal PendingBalance { get; set; }
    public decimal TotalDeposited { get; set; }
    public decimal TotalWithdrawn { get; set; }
    public bool IsActive { get; set; }
    public List<TransactionDto> RecentTransactions { get; set; } = new();
}
