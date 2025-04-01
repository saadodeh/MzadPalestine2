using MzadPalestine.Core.Models;
using MzadPalestine.Core.Models.Common;
using MzadPalestine.Core.Models.Enums;

namespace MzadPalestine.Core.Interfaces.Repositories;

public interface IListingRepository : IGenericRepository<Listing>
{
    Task<PagedList<Listing>> GetPagedListingsAsync(int pageNumber, int pageSize, string? searchTerm = null, int? categoryId = null, int? locationId = null, ListingStatus? status = null);
    Task<Listing?> GetListingDetailsAsync(int id);
    Task<IEnumerable<Listing>> GetUserListingsAsync(string userId);
    Task<IEnumerable<Listing>> GetActiveListingsAsync();
    Task<IEnumerable<Listing>> GetListingsByCategoryAsync(string categoryId);
    Task<IEnumerable<Listing>> SearchListingsAsync(string searchTerm);
    Task<bool> IsListingOwnerAsync(string listingId, string userId);
    Task<bool> DeleteAsync(Listing listing);
}
