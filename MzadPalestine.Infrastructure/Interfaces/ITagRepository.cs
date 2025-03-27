using MzadPalestine.Core.Models;

namespace MzadPalestine.Core.Interfaces;

public interface ITagRepository : IGenericRepository<Tag>
{
    Task<IEnumerable<Tag>> GetPopularTagsAsync(int count);
    Task<IEnumerable<Tag>> SearchTagsAsync(string searchTerm);
    Task<IEnumerable<Tag>> GetListingTagsAsync(int listingId);
    Task AddTagToListingAsync(int listingId, int tagId);
    Task RemoveTagFromListingAsync(int listingId, int tagId);
}
