using MzadPalestine.Core.Models;

namespace MzadPalestine.Core.Interfaces.Repositories;

public interface IWatchlistRepository : IGenericRepository<Watchlist>
{
    Task<IEnumerable<Watchlist>> GetUserWatchlistAsync(string userId);
    Task<bool> IsListingInWatchlistAsync(string userId, int listingId);
    Task<Watchlist?> GetWatchlistItemAsync(string userId, int listingId);
}
