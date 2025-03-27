using MzadPalestine.Core.Models;

namespace MzadPalestine.Core.Interfaces.Repositories;

public interface ITagRepository : IGenericRepository<Tag>
{
    Task<IEnumerable<Tag>> GetTagsByListingIdAsync(int listingId);
    Task<IEnumerable<Tag>> GetPopularTagsAsync(int count);
}
