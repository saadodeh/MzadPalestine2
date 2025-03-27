using Microsoft.EntityFrameworkCore;
using MzadPalestine.Core.Interfaces.Repositories;
using MzadPalestine.Core.Models;
using MzadPalestine.Infrastructure.Data;

namespace MzadPalestine.Infrastructure.Repositories;

public class WatchlistRepository : GenericRepository<Watchlist>, IWatchlistRepository
{
    private readonly ApplicationDbContext _context;

    public WatchlistRepository(ApplicationDbContext context) : base(context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Watchlist>> GetUserWatchlistAsync(string userId)
    {
        return await _context.Watchlists
            .Include(w => w.Listing)
                .ThenInclude(l => l!.Images)
            .Include(w => w.Listing)
                .ThenInclude(l => l!.User)
            .Where(w => w.UserId == userId)
            .OrderByDescending(w => w.CreatedAt)
            .ToListAsync();
    }

    public async Task<bool> IsListingInWatchlistAsync(string userId, int listingId)
    {
        return await _context.Watchlists
            .AnyAsync(w => w.UserId == userId && w.ListingId == listingId);
    }

    public async Task<Watchlist?> GetWatchlistItemAsync(string userId, int listingId)
    {
        return await _context.Watchlists
            .Include(w => w.Listing)
            .FirstOrDefaultAsync(w => w.UserId == userId && w.ListingId == listingId);
    }
}
