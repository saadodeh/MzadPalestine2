using MzadPalestine.Core.Models;
using MzadPalestine.Core.Models.Common;

namespace MzadPalestine.Core.Interfaces;

public interface ITagRepository : IGenericRepository<Tag>
{
    Task<IEnumerable<Tag>> GetPopularTagsAsync(int count);
    Task<IEnumerable<Tag>> GetTagsByListingIdAsync(int listingId);
    Task<PagedList<Tag>> SearchTagsAsync(string searchTerm, PaginationParams parameters);
    Task<bool> AddTagsToListingAsync(int listingId, IEnumerable<string> tags);
    Task<bool> RemoveTagsFromListingAsync(int listingId, IEnumerable<string> tags);
    Task<Dictionary<string, int>> GetTagStatisticsAsync();
}
