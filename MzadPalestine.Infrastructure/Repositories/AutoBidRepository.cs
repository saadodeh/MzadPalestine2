using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using MzadPalestine.Core.Interfaces;
using MzadPalestine.Core.Models;
using MzadPalestine.Infrastructure.Data;
using Polly;
using System.Data.SqlClient;

namespace MzadPalestine.Infrastructure.Repositories;

public class AutoBidRepository : GenericRepository<AutoBid>, IAutoBidRepository
{
    private readonly ApplicationDbContext _context;
    private readonly ILogger<AutoBidRepository> _logger;
    private readonly IAsyncPolicy _retryPolicy;

    public AutoBidRepository(ApplicationDbContext context, ILogger<AutoBidRepository> logger) : base(context)
    {
        _context = context;
        _logger = logger;
        _retryPolicy = Policy
            .Handle<SqlException>()
            .Or<DbUpdateException>()
            .WaitAndRetryAsync(3, retryAttempt =>
                TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)),
                onRetry: (exception, timeSpan, retryCount, context) =>
                {
                    _logger.LogWarning($"Attempt {retryCount} of 3 failed with exception: {exception.Message}. Retrying in {timeSpan.TotalSeconds} seconds.");
                });
    }

    public async Task<AutoBid?> GetAutoBidAsync(string userId, int auctionId)
    {
        try
        {
            return await _retryPolicy.ExecuteAsync(async () =>
                await _context.AutoBids
                    .FirstOrDefaultAsync(ab => ab.BidderId == userId && ab.AuctionId == auctionId && ab.IsActive));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Error getting auto bid for user {userId} and auction {auctionId}");
            throw;
        }
    }

    public async Task<IEnumerable<AutoBid>> GetActiveAutoBidsForAuctionAsync(int auctionId)
    {
        try
        {
            return await _retryPolicy.ExecuteAsync(async () =>
                await _context.AutoBids
                    .Where(ab => ab.AuctionId == auctionId && ab.IsActive)
                    .OrderByDescending(ab => ab.MaxAmount)
                    .ToListAsync());
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Error getting active auto bids for auction {auctionId}");
            throw;
        }
    }

    public async Task<IEnumerable<AutoBid>> GetUserAutoBidsAsync(string userId)
    {
        try
        {
            return await _retryPolicy.ExecuteAsync(async () =>
                await _context.AutoBids
                    .Where(ab => ab.BidderId == userId)
                    .OrderByDescending(ab => ab.CreatedAt)
                    .ToListAsync());
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Error getting user auto bids for user {userId}");
            throw;
        }
    }

    public async Task<IEnumerable<AutoBid>> GetAuctionAutoBidsAsync(int auctionId)
    {
        try
        {
            return await _retryPolicy.ExecuteAsync(async () =>
                await _context.AutoBids
                    .Where(ab => ab.AuctionId == auctionId)
                    .OrderByDescending(ab => ab.CreatedAt)
                    .ToListAsync());
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Error getting auction auto bids for auction {auctionId}");
            throw;
        }
    }

    public async Task<bool> UpdateAsync(AutoBid autoBid)
    {
        try
        {
            await _retryPolicy.ExecuteAsync(async () =>
            {
                _context.AutoBids.Update(autoBid);
                await _context.SaveChangesAsync();
                return true;
            });
            return true;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Error updating auto bid {autoBid.Id}");
            return false;
        }
    }

    public async Task<bool> DeleteAsync(int id)
    {
        try
        {
            return await _retryPolicy.ExecuteAsync(async () =>
            {
                var autoBid = await _context.AutoBids.FindAsync(id);
                if (autoBid == null) return false;

                _context.AutoBids.Remove(autoBid);
                await _context.SaveChangesAsync();
                return true;
            });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Error deleting auto bid {id}");
            return false;
        }
    }
}