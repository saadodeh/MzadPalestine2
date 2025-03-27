using MzadPalestine.Core.Models;
using MzadPalestine.Core.Models.Common;

namespace MzadPalestine.Core.Interfaces;

public interface ILocationRepository : IGenericRepository<Location>
{
    Task<Location?> GetLocationWithChildrenAsync(int locationId);
    Task<IEnumerable<Location>> GetRootLocationsAsync();
    Task<IEnumerable<Location>> GetLocationsByParentIdAsync(int parentId);
    Task<bool> HasListingsAsync(int locationId);
    Task<bool> IsValidLocationPathAsync(int[] locationPath);
    Task<Dictionary<string, int>> GetLocationStatisticsAsync();
}
