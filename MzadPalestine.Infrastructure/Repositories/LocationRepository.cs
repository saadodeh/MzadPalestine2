using MzadPalestine.Core.DTOs.Locations;
using MzadPalestine.Core.Interfaces;
using MzadPalestine.Core.Models;
using MzadPalestine.Infrastructure.Data;

namespace MzadPalestine.Infrastructure.Repositories;

public class LocationRepository : GenericRepository<Location>, ILocationRepository
{
    public LocationRepository(ApplicationDbContext context) : base(context)
    {
    }

    public Task<List<LocationDto>> GetAllLocationsAsync( )
    {
        throw new NotImplementedException();
    }

    public Task<int> GetListingCountAsync(int locationId)
    {
        throw new NotImplementedException();
    }

    public Task<LocationDto?> GetLocationByIdAsync(int id)
    {
        throw new NotImplementedException();
    }

    public Task<LocationDetailsDto?> GetLocationDetailsAsync(int id)
    {
        throw new NotImplementedException();
    }

    public Task<List<LocationDto>> GetLocationsByParentIdAsync(int? parentId)
    {
        throw new NotImplementedException();
    }

    public Task<LocationTreeDto?> GetLocationTreeAsync(int id)
    {
        throw new NotImplementedException();
    }

    public Task<Location?> GetLocationWithChildrenAsync(int id)
    {
        throw new NotImplementedException();
    }

    public Task<bool> LocationExistsAsync(int id)
    {
        throw new NotImplementedException();
    }
}