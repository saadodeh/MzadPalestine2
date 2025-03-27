using MzadPalestine.Core.Models;

namespace MzadPalestine.Core.Interfaces.Repositories;

public interface IListingRepository : IGenericRepository<Listing>
{
    Task<IEnumerable<Listing>> GetUserListingsAsync(string userId);
    Task<IEnumerable<Listing>> GetActiveListingsAsync();
    Task<IEnumerable<Listing>> GetListingsByCategoryAsync(string categoryId);
    Task<IEnumerable<Listing>> SearchListingsAsync(string searchTerm);
    Task<bool> IsListingOwnerAsync(string listingId, string userId);
}
