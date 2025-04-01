using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using MzadPalestine.Core.Interfaces.Repositories;
using MzadPalestine.Core.Models;
using MzadPalestine.Core.Models.Common;
using MzadPalestine.Core.Models.Enums;
using MzadPalestine.Infrastructure.Data;

namespace MzadPalestine.Infrastructure.Repositories;

public class AuctionRepository : GenericRepository<Auction>, IAuctionRepository
{
    private readonly ApplicationDbContext _context;
    private readonly DbSet<Auction> _dbSet;

    public AuctionRepository(ApplicationDbContext context) : base(context)
    {
        _context = context;
        _dbSet = context.Set<Auction>();
    }

    public async Task<Auction?> GetAuctionDetailsAsync(int auctionId)
    {
        return await _dbSet
            .Include(a => a.Listing)
                .ThenInclude(l => l.Images)
            .Include(a => a.Listing)
                .ThenInclude(l => l.Category)
            .Include(a => a.Listing)
                .ThenInclude(l => l.Location)
            .Include(a => a.Listing)
                .ThenInclude(l => l.User)
            .Include(a => a.Bids.OrderByDescending(b => b.Amount))
                .ThenInclude(b => b.User)
            .FirstOrDefaultAsync(a => a.Id == auctionId);
    }

    public new async Task<IEnumerable<Auction>> AddRangeAsync(IEnumerable<Auction> entities)
    {
        await base.AddRangeAsync(entities);
        return entities;
    }

    public new async Task DeleteAsync(Auction entity)
    {
        base.Remove(entity);
        await Task.CompletedTask;
    }

    public new async Task DeleteRangeAsync(IEnumerable<Auction> entities)
    {
        base.RemoveRange(entities);
        await Task.CompletedTask;
    }

    public new async Task<bool> ExistsAsync(Expression<Func<Auction, bool>> predicate)
    {
        return await _dbSet.AnyAsync(predicate);
    }

    public async Task<bool> UpdateAsync(Auction auction)
    {
        _context.Entry(auction).State = EntityState.Modified;
        return true;
    }

    public async Task<PagedList<Auction>> GetUserAuctionsAsync(string userId, PaginationParams parameters)
    {
        var query = _dbSet
            .Include(a => a.Listing)
            .Include(a => a.Bids)
                .ThenInclude(b => b.Bidder)
            .Where(a => a.Listing.UserId == userId)
            .OrderByDescending(a => a.CreatedAt);

        return await PagedList<Auction>.CreateAsync(query, parameters.PageNumber, parameters.PageSize);
    }

    public async Task<IEnumerable<Auction>> GetActiveAuctionsAsync()
    {
        var now = DateTime.UtcNow;
        return await _dbSet
            .Include(a => a.Listing)
                .ThenInclude(l => l.Category)
            .Include(a => a.Bids)
                .ThenInclude(b => b.Bidder)
            .Where(a => a.StartDate <= now && a.EndDate > now && a.Status == AuctionStatus.Open)
            .OrderBy(a => a.EndDate)
            .ToListAsync();
    }

    public async Task<IEnumerable<Auction>> GetEndingSoonAuctionsAsync(int hours)
    {
        var now = DateTime.UtcNow;
        var threshold = now.AddHours(hours);
        return await _dbSet
            .Include(a => a.Listing)
                .ThenInclude(l => l.Category)
            .Include(a => a.Bids)
                .ThenInclude(b => b.Bidder)
            .Where(a => a.EndDate > now && a.EndDate <= threshold && a.Status == AuctionStatus.Open)
            .OrderBy(a => a.EndDate)
            .ToListAsync();
    }

    public async Task<bool> HasActiveBidsAsync(int auctionId)
    {
        return await _dbSet
            .Include(a => a.Bids)
            .AnyAsync(a => a.Id == auctionId && a.Bids.Any());
    }

    public async Task<decimal> GetHighestBidAsync(int auctionId)
    {
        var auction = await _dbSet
            .Include(a => a.Bids)
            .FirstOrDefaultAsync(a => a.Id == auctionId);

        return auction?.Bids.Max(b => b.Amount) ?? 0;
    }
}
