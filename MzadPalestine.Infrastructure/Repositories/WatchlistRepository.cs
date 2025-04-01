using Microsoft.EntityFrameworkCore;
using MzadPalestine.Core.Interfaces;
using MzadPalestine.Core.Models;
using MzadPalestine.Core.Models.Common;
using MzadPalestine.Infrastructure.Data;

namespace MzadPalestine.Infrastructure.Repositories;

public class WatchlistRepository : GenericRepository<Watchlist>, IWatchlistRepository
{
    private readonly ApplicationDbContext _context;

    public WatchlistRepository(ApplicationDbContext context) : base(context)
    {
        _context = context;
    }

    public async Task<PagedList<Watchlist>> GetUserWatchlistAsync(string userId, PaginationParams parameters)
    {
        var query = _context.Watchlists
            .Include(w => w.Listing)
                .ThenInclude(l => l!.Images)
            .Include(w => w.Listing)
                .ThenInclude(l => l!.User)
            .Where(w => w.UserId == userId)
            .OrderByDescending(w => w.CreatedAt);

        return await PagedList<Watchlist>.CreateAsync(query, parameters.PageNumber, parameters.PageSize);
    }

    public async Task<bool> IsListingInWatchlistAsync(string userId, int listingId)
    {
        return await _context.Watchlists
            .AnyAsync(w => w.UserId == userId && w.ListingId == listingId);
    }

    public async Task<bool> ToggleWatchlistAsync(string userId, int listingId)
    {
        var watchlistItem = await _context.Watchlists
            .FirstOrDefaultAsync(w => w.UserId == userId && w.ListingId == listingId);

        if (watchlistItem != null)
        {
            _context.Watchlists.Remove(watchlistItem);
            await _context.SaveChangesAsync();
            return false; // Item was removed
        }

        watchlistItem = new Watchlist
        {
            UserId = userId,
            ListingId = listingId,
            CreatedAt = DateTime.UtcNow
        };

        await _context.Watchlists.AddAsync(watchlistItem);
        await _context.SaveChangesAsync();
        return true; // Item was added
    }

    public async Task<int> GetWatchlistCountForListingAsync(int listingId)
    {
        return await _context.Watchlists
            .CountAsync(w => w.ListingId == listingId);
    }
}
