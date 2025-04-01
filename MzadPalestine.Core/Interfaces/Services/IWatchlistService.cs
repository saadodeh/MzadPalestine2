using MzadPalestine.Core.DTOs.Watchlists;
using MzadPalestine.Core.Models.Common;

namespace MzadPalestine.Core.Interfaces.Services;

public interface IWatchlistService
{
    Task<WatchlistDto?> GetWatchlistItemAsync(int listingId, string userId);
    Task<PagedList<WatchlistDto>> GetUserWatchlistAsync(string userId, PaginationParams parameters);
    Task<Result<bool>> AddToWatchlistAsync(string userId, int listingId);
    Task<Result<bool>> RemoveFromWatchlistAsync(string userId, int listingId);
    Task<bool> IsListingInWatchlistAsync(int listingId, string userId);
    Task<int> GetWatchlistCountForListingAsync(int listingId);
    Task<bool> CheckWatchlistStatusAsync(string userId, int auctionId);
}