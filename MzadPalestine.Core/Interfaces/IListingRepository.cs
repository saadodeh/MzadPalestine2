using MzadPalestine.Core.Models;
using MzadPalestine.Core.Models.Common;
using MzadPalestine.Core.Models.Enums;

namespace MzadPalestine.Core.Interfaces;

public interface IListingRepository : IGenericRepository<Listing>
{
    Task<Listing?> GetListingWithDetailsAsync(int listingId);
    Task<PagedList<Listing>> GetUserListingsAsync(string userId, PaginationParams parameters);
    Task<PagedList<Listing>> SearchListingsAsync(string searchTerm, PaginationParams parameters);
    Task<PagedList<Listing>> GetListingsByCategoryAsync(int categoryId, PaginationParams parameters);
    Task<PagedList<Listing>> GetListingsByLocationAsync(int locationId, PaginationParams parameters);
    Task<IEnumerable<Listing>> GetFeaturedListingsAsync();
    Task<IEnumerable<Listing>> GetRelatedListingsAsync(int listingId, int count);
    Task<bool> IsListingOwnedByUserAsync(int listingId, string userId);
    Task<bool> HasActiveAuctionAsync(int listingId);
    Task UpdateListingStatusAsync(int listingId, ListingStatus status);
    Task<int> GetUserListingsCountAsync(string userId);
    Task<Dictionary<ListingStatus, int>> GetListingStatisticsAsync();
}
