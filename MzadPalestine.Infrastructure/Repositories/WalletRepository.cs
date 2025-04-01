using Microsoft.EntityFrameworkCore;
using MzadPalestine.Core.Interfaces;
using MzadPalestine.Core.Interfaces.Repositories;
using MzadPalestine.Core.Models;
using MzadPalestine.Core.Models.Common;
using MzadPalestine.Core.Models.Enums;
using MzadPalestine.Infrastructure.Data;

namespace MzadPalestine.Infrastructure.Repositories;

public class WalletRepository : BaseRepository<Wallet>, IWalletRepository
{
    private readonly ApplicationDbContext _context;

    public WalletRepository(ApplicationDbContext context) : base(context)
    {
        _context = context;
    }

    public async Task<Wallet> GetByUserIdAsync(int userId)
    {
        var wallet = await _context.Wallets
            .FirstOrDefaultAsync(w => w.UserId == userId);

        if (wallet == null)
        {
            wallet = new Wallet { UserId = userId , Balance = 0 };
            _context.Wallets.Add(wallet);
            await _context.SaveChangesAsync();
        }

        return wallet;
    }

    public async Task<Wallet> CreateAsync(Wallet wallet)
    {
        _context.Wallets.Add(wallet);
        await _context.SaveChangesAsync();
        return wallet;
    }

    public new async Task<Wallet> UpdateAsync(Wallet wallet)
    {
        _context.Entry(wallet).State = EntityState.Modified;
        await _context.SaveChangesAsync();
        return wallet;
    }

    public async Task<WalletTransaction> AddTransactionAsync(WalletTransaction transaction)
    {
        var wallet = await GetByUserIdAsync(transaction.UserId);

        if (transaction.Type == TransactionType.Debit && wallet.Balance < transaction.Amount)
        {
            throw new InvalidOperationException("Insufficient balance");
        }

        wallet.Balance += transaction.Type == TransactionType.Credit ? transaction.Amount : -transaction.Amount;

        _context.WalletTransactions.Add(transaction);
        await _context.SaveChangesAsync();

        return transaction;
    }

    public async Task<IEnumerable<WalletTransaction>> GetTransactionHistoryAsync(int userId , int page = 1 , int pageSize = 10)
    {
        return await _context.WalletTransactions
            .Where(t => t.UserId == userId)
            .OrderByDescending(t => t.CreatedAt)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();
    }

    public async Task<decimal> GetBalanceAsync(string userId)
    {
        var wallet = await GetByUserIdAsync(userId);
        return wallet.Balance;
    }

    public async Task<bool> HasSufficientBalanceAsync(string userId , decimal amount)
    {
        var balance = await GetBalanceAsync(userId);
        return balance >= amount;
    }

    public async Task<Wallet?> GetUserWalletAsync(int userId)
    {
        return await _dbSet
            .Include(w => w.Transactions.OrderByDescending(t => t.CreatedAt).Take(10))
            .FirstOrDefaultAsync(w => w.UserId == userId);
    }

    public async Task<WalletTransaction> AddTransactionAsync(int userId , decimal amount , TransactionType type , string description)
    {
        var wallet = await _dbSet
            .Include(w => w.Transactions)
            .FirstOrDefaultAsync(w => w.UserId == userId);

        if (wallet == null)
        {
            throw new InvalidOperationException("Wallet not found");
        }

        var transaction = new WalletTransaction
        {
            Id = wallet.Id ,
            Amount = amount ,
            Type = type ,
            Status = TransactionStatus.Completed ,
            CreatedAt = DateTime.UtcNow
        };

        wallet.Transactions.Add(transaction);

        // Update wallet balance
        if (type == TransactionType.Credit)
        {
            wallet.Balance += amount;
        }
        else
        {
            if (wallet.Balance < amount)
            {
                throw new InvalidOperationException("Insufficient balance");
            }
            wallet.Balance -= amount;
        }

        await _context.SaveChangesAsync();
        return transaction;
    }

    public async Task<IEnumerable<WalletTransaction>> GetUserTransactionsAsync(
        int userId ,
        DateTime? startDate = null ,
        DateTime? endDate = null ,
        TransactionType? type = null)
    {
        var wallet = await _dbSet
            .Include(w => w.Transactions)
            .FirstOrDefaultAsync(w => w.UserId == userId);

        if (wallet == null)
        {
            return Enumerable.Empty<WalletTransaction>();
        }

        var query = wallet.Transactions.AsQueryable();

        if (startDate.HasValue)
        {
            query = query.Where(t => t.CreatedAt >= startDate);
        }

        if (endDate.HasValue)
        {
            query = query.Where(t => t.CreatedAt <= endDate);
        }

        if (type.HasValue)
        {
            query = query.Where(t => t.Type == type);
        }

        return query.OrderByDescending(t => t.CreatedAt);
    }

    public async Task<decimal> GetTotalTransactionsAmountAsync(
        int userId ,
        TransactionType type ,
        DateTime? startDate = null ,
        DateTime? endDate = null)
    {
        var wallet = await _dbSet
            .Include(w => w.Transactions)
            .FirstOrDefaultAsync(w => w.UserId == userId);

        if (wallet == null)
        {
            return 0;
        }

        var query = wallet.Transactions
            .Where(t => t.Type == type);

        if (startDate.HasValue)
        {
            query = query.Where(t => t.CreatedAt >= startDate);
        }

        if (endDate.HasValue)
        {
            query = query.Where(t => t.CreatedAt <= endDate);
        }

        return query.Sum(t => t.Amount);
    }

    public Task<Wallet> GetByUserIdAsync(string userId)
    {
        throw new NotImplementedException();
    }

    public Task<IEnumerable<WalletTransaction>> GetTransactionHistoryAsync(string userId , int page = 1 , int pageSize = 10)
    {
        throw new NotImplementedException();
    }

    public Task<Wallet?> GetUserWalletAsync(string userId)
    {
        throw new NotImplementedException();
    }

    public Task<bool> UpdateBalanceAsync(string userId , decimal amount)
    {
        throw new NotImplementedException();
    }

    public Task<IEnumerable<WalletTransaction>> GetTransactionHistoryAsync(string userId)
    {
        throw new NotImplementedException();
    }

    public Task<PagedList<WalletTransaction>> GetPagedTransactionHistoryAsync(string userId , PaginationParams parameters)
    {
        throw new NotImplementedException();
    }

    public Task<WalletTransaction?> GetTransactionByIdAsync(string transactionId)
    {
        throw new NotImplementedException();
    }

    public Task<bool> CreateTransactionAsync(WalletTransaction transaction)
    {
        throw new NotImplementedException();
    }

    public Task<IEnumerable<WalletTransaction>> GetUserTransactionsAsync(string userId , DateTime? startDate = null , DateTime? endDate = null , TransactionType? type = null)
    {
        throw new NotImplementedException();
    }

    public Task<WalletTransaction> AddTransactionAsync(string userId , decimal amount , TransactionType type , string description)
    {
        throw new NotImplementedException();
    }
}
