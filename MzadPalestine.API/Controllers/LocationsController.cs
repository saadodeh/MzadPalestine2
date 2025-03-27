using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MzadPalestine.Core.DTOs.Locations;
using MzadPalestine.Core.Interfaces.Repositories;
using MzadPalestine.Core.Interfaces.Services;
using MzadPalestine.Core.Models;

namespace MzadPalestine.API.Controllers;

public class LocationsController : BaseApiController
{
    private readonly ILocationRepository _locationRepository;
    private readonly ICacheService _cacheService;
    private readonly ICurrentUserService _currentUserService;

    public LocationsController(
        ILocationRepository locationRepository,
        ICacheService cacheService,
        ICurrentUserService currentUserService)
    {
        _locationRepository = locationRepository;
        _cacheService = cacheService;
        _currentUserService = currentUserService;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<LocationDto>>> GetLocations()
    {
        var cacheKey = "locations_all";
        var cachedResult = await _cacheService.GetAsync<IEnumerable<LocationDto>>(cacheKey);

        if (cachedResult != null)
        {
            return Ok(cachedResult);
        }

        var locations = await _locationRepository.GetAllAsync();

        var result = locations.Select(l => new LocationDto
        {
            Id = l.Id,
            Name = l.Name,
            ParentId = l.ParentId,
            Type = l.Type,
            ListingCount = l.Listings.Count
        }).ToList();

        await _cacheService.SetAsync(cacheKey, result, TimeSpan.FromHours(24));

        return Ok(result);
    }

    [HttpGet("tree")]
    public async Task<ActionResult<IEnumerable<LocationTreeDto>>> GetLocationTree()
    {
        var cacheKey = "locations_tree";
        var cachedResult = await _cacheService.GetAsync<IEnumerable<LocationTreeDto>>(cacheKey);

        if (cachedResult != null)
        {
            return Ok(cachedResult);
        }

        var locations = await _locationRepository.GetAllAsync();
        var rootLocations = locations.Where(l => !l.ParentId.HasValue)
            .OrderBy(l => l.Name);

        var result = rootLocations.Select(l => BuildLocationTree(l, locations)).ToList();

        await _cacheService.SetAsync(cacheKey, result, TimeSpan.FromHours(24));

        return Ok(result);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<LocationDetailsDto>> GetLocation(int id)
    {
        var location = await _locationRepository.GetByIdAsync(id);

        if (location == null)
        {
            return NotFound();
        }

        return new LocationDetailsDto
        {
            Id = location.Id,
            Name = location.Name,
            ParentId = location.ParentId,
            ParentName = location.Parent?.Name,
            Type = location.Type,
            ListingCount = location.Listings.Count,
            Children = location.Children.Select(c => new LocationDto
            {
                Id = c.Id,
                Name = c.Name,
                ParentId = c.ParentId,
                Type = c.Type,
                ListingCount = c.Listings.Count
            }).ToList()
        };
    }

    [Authorize(Roles = "Admin")]
    [HttpPost]
    public async Task<ActionResult<LocationDto>> CreateLocation(CreateLocationRequest request)
    {
        var location = new Location
        {
            Name = request.Name,
            ParentId = request.ParentId,
            Type = request.Type,
            CreatedAt = DateTime.UtcNow
        };

        await _locationRepository.AddAsync(location);

        // Invalidate cache
        await _cacheService.RemoveAsync("locations_all");
        await _cacheService.RemoveAsync("locations_tree");

        return CreatedAtAction(nameof(GetLocation), new { id = location.Id }, new LocationDto
        {
            Id = location.Id,
            Name = location.Name,
            ParentId = location.ParentId,
            Type = location.Type
        });
    }

    [Authorize(Roles = "Admin")]
    [HttpPut("{id}")]
    public async Task<ActionResult<LocationDto>> UpdateLocation(int id, UpdateLocationRequest request)
    {
        var location = await _locationRepository.GetByIdAsync(id);

        if (location == null)
        {
            return NotFound();
        }

        location.Name = request.Name;
        location.ParentId = request.ParentId;
        location.Type = request.Type;

        await _locationRepository.UpdateAsync(location);

        // Invalidate cache
        await _cacheService.RemoveAsync("locations_all");
        await _cacheService.RemoveAsync("locations_tree");

        return new LocationDto
        {
            Id = location.Id,
            Name = location.Name,
            ParentId = location.ParentId,
            Type = location.Type
        };
    }

    [Authorize(Roles = "Admin")]
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteLocation(int id)
    {
        var location = await _locationRepository.GetByIdAsync(id);

        if (location == null)
        {
            return NotFound();
        }

        if (location.Listings.Any())
        {
            return BadRequest("لا يمكن حذف الموقع لوجود مزادات مرتبطة به");
        }

        if (location.Children.Any())
        {
            return BadRequest("لا يمكن حذف الموقع لوجود مواقع فرعية");
        }

        await _locationRepository.DeleteAsync(location);

        // Invalidate cache
        await _cacheService.RemoveAsync("locations_all");
        await _cacheService.RemoveAsync("locations_tree");

        return NoContent();
    }

    private LocationTreeDto BuildLocationTree(Location location, IEnumerable<Location> allLocations)
    {
        var children = allLocations
            .Where(l => l.ParentId == location.Id)
            .OrderBy(l => l.Name)
            .Select(l => BuildLocationTree(l, allLocations))
            .ToList();

        return new LocationTreeDto
        {
            Id = location.Id,
            Name = location.Name,
            Type = location.Type,
            ListingCount = location.Listings.Count,
            Children = children
        };
    }
}
