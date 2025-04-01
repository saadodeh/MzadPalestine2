using MzadPalestine.Core.Models;
using MzadPalestine.Core.Models.Common;

namespace MzadPalestine.Core.Interfaces;

public interface IWatchlistRepository : IGenericRepository<Watchlist>
{
    Task<PagedList<Watchlist>> GetUserWatchlistAsync(string userId, PaginationParams parameters);
    Task<bool> IsListingInWatchlistAsync(string userId, int listingId);
    Task<bool> ToggleWatchlistAsync(string userId, int listingId);
    Task<int> GetWatchlistCountForListingAsync(int listingId);
}
