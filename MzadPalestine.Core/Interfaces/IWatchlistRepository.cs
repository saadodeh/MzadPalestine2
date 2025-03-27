using MzadPalestine.Core.Models;
using MzadPalestine.Core.Models.Common;

namespace MzadPalestine.Core.Interfaces;

public interface IWatchlistRepository : IGenericRepository<Watchlist>
{
    Task<PagedList<Auction>> GetUserWatchlistAsync(string userId, PaginationParams parameters);
    Task<bool> IsInWatchlistAsync(string userId, int auctionId);
    Task AddToWatchlistAsync(string userId, int auctionId);
    Task RemoveFromWatchlistAsync(string userId, int auctionId);
    Task<int> GetWatchlistCountAsync(string userId);
    Task<IEnumerable<string>> GetWatchingUsersAsync(int auctionId);
    Task RemoveExpiredWatchlistItemsAsync();
    Task<bool> HasReachedWatchlistLimitAsync(string userId);
    Task<Dictionary<int, int>> GetWatchlistStatisticsAsync(string userId);
}
