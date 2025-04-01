using System.Collections.Generic;
using System.Threading.Tasks;
using MzadPalestine.Core.Models;
using MzadPalestine.Core.Models.Common;
using MzadPalestine.Core.Models.Enums;

namespace MzadPalestine.Core.Interfaces;

public interface IListingRepository : IRepository<Listing>
{
    Task<PagedList<Listing>> GetListingsAsync(PaginationParams parameters);
    Task<PagedList<Listing>> GetPagedListingsAsync(
        int pageNumber ,
        int pageSize ,
        string? searchTerm = null ,
        int? categoryId = null ,
        int? locationId = null ,
        ListingStatus? status = null);
    Task<Listing?> GetListingByIdAsync(int id);
    Task<IEnumerable<Listing>> GetListingsByUserIdAsync(string userId);
    Task<IEnumerable<Listing>> GetActiveListingsAsync( );
    Task<IEnumerable<Listing>> GetFeaturedListingsAsync( );
    Task<bool> IsListingOwnerAsync(int listingId , string userId);
    Task<bool> UpdateListingStatusAsync(int listingId , bool isActive);
    Task<IEnumerable<Listing>> SearchListingsAsync(string searchTerm);
    Task<int> GetTotalListingsCountAsync( );
    Task<Listing?> GetListingDetailsAsync(int id);
}