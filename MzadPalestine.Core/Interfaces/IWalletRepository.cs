using MzadPalestine.Core.Models;
using MzadPalestine.Core.Models.Common;
using MzadPalestine.Core.Models.Enums;

namespace MzadPalestine.Core.Interfaces;

public interface IWalletRepository : IGenericRepository<Wallet>
{
    Task<Wallet?> GetUserWalletAsync(string userId);
    Task<decimal> GetBalanceAsync(string userId);
    Task<bool> UpdateBalanceAsync(string userId, decimal amount);
    Task<IEnumerable<WalletTransaction>> GetTransactionHistoryAsync(string userId);
    Task<PagedList<WalletTransaction>> GetPagedTransactionHistoryAsync(string userId, PaginationParams parameters);
    Task<WalletTransaction?> GetTransactionByIdAsync(string transactionId);
    Task<bool> CreateTransactionAsync(WalletTransaction transaction);
    Task<bool> HasSufficientBalanceAsync(string userId, decimal amount);
    Task<IEnumerable<WalletTransaction>> GetUserTransactionsAsync(string userId, DateTime? startDate = null, DateTime? endDate = null, TransactionType? type = null);
    Task<WalletTransaction> AddTransactionAsync(string userId, decimal amount, TransactionType type, string description);
}