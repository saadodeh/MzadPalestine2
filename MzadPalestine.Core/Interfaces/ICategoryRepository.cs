using MzadPalestine.Core.Models;
using MzadPalestine.Core.Models.Common;

namespace MzadPalestine.Core.Interfaces;

public interface ICategoryRepository : IGenericRepository<Category>
{
    Task<Category?> GetCategoryWithChildrenAsync(int categoryId);
    Task<IEnumerable<Category>> GetMainCategoriesAsync();
    Task<IEnumerable<Category>> GetSubCategoriesAsync(int parentId);
    Task<IEnumerable<Category>> GetCategoryPathAsync(int categoryId);
    Task<bool> HasListingsAsync(int categoryId);
    Task<int> GetListingsCountAsync(int categoryId);
    Task<bool> IsCategoryUsedInActiveAuctionsAsync(int categoryId);
    Task<PagedList<Category>> SearchCategoriesAsync(string searchTerm, PaginationParams parameters);
    Task<bool> IsSlugUniqueAsync(string slug);
    Task UpdateCategoryOrderAsync(int categoryId, int order);
}
