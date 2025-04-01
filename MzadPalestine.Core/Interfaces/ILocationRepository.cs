using MzadPalestine.Core.DTOs.Locations;
using MzadPalestine.Core.Models;

namespace MzadPalestine.Core.Interfaces;

public interface ILocationRepository : IRepository<Location>
{
    Task<LocationDto?> GetLocationByIdAsync(int id);
    Task<List<LocationDto>> GetAllLocationsAsync();
    Task<List<LocationDto>> GetLocationsByParentIdAsync(int? parentId);
    Task<LocationDetailsDto?> GetLocationDetailsAsync(int id);
    Task<LocationTreeDto?> GetLocationTreeAsync(int id);
    Task<int> GetListingCountAsync(int locationId);
    Task<bool> LocationExistsAsync(int id);
    Task<Location?> GetLocationWithChildrenAsync(int id);
}