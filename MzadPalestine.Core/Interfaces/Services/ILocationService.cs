using MzadPalestine.Core.DTOs.Locations;
using MzadPalestine.Core.Models.Common;

namespace MzadPalestine.Core.Interfaces.Services;

public interface ILocationService
{
    Task<IEnumerable<LocationDto>> GetLocationsAsync();
    Task<LocationDetailsDto?> GetLocationByIdAsync(int id);
    Task<IEnumerable<LocationTreeDto>> GetLocationTreeAsync();
    Task<Result<LocationDto>> CreateLocationAsync(CreateLocationDto createLocationDto);
    Task<Result<LocationDto>> UpdateLocationAsync(int id, UpdateLocationDto updateLocationDto);
    Task<Result<bool>> DeleteLocationAsync(int id);
    Task<Result<bool>> ToggleLocationStatusAsync(int id);
    Task<IEnumerable<LocationDto>> GetChildLocationsAsync(int parentId);
    Task<IEnumerable<LocationDto>> SearchLocationsAsync(string searchTerm);
}