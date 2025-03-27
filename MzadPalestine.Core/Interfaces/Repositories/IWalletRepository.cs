using MzadPalestine.Core.Models;

namespace MzadPalestine.Core.Interfaces.Repositories;

public interface IWalletRepository
{
    Task<Wallet> GetByUserIdAsync(string userId);
    Task<Wallet> CreateAsync(Wallet wallet);
    Task<Wallet> UpdateAsync(Wallet wallet);
    Task<WalletTransaction> AddTransactionAsync(WalletTransaction transaction);
    Task<IEnumerable<WalletTransaction>> GetTransactionHistoryAsync(string userId, int page = 1, int pageSize = 10);
    Task<decimal> GetBalanceAsync(string userId);
    Task<bool> HasSufficientBalanceAsync(string userId, decimal amount);
}
