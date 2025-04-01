using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MzadPalestine.Core.DTOs.Locations;
using MzadPalestine.Core.Interfaces.Services;
using MzadPalestine.Core.Models.Common;

namespace MzadPalestine.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class LocationsController : ControllerBase
{
    private readonly ILocationService _locationService;
    private readonly ICurrentUserService _currentUserService;

    public LocationsController(ILocationService locationService, ICurrentUserService currentUserService)
    {
        _locationService = locationService;
        _currentUserService = currentUserService;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<LocationDto>>> GetLocations()
    {
        var locations = await _locationService.GetLocationsAsync();
        return Ok(locations);
    }

    [HttpGet("tree")]
    public async Task<ActionResult<IEnumerable<LocationTreeDto>>> GetLocationTree()
    {
        var locationTree = await _locationService.GetLocationTreeAsync();
        return Ok(locationTree);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<LocationDetailsDto>> GetLocation(int id)
    {
        var location = await _locationService.GetLocationByIdAsync(id);
        if (location == null)
            return NotFound();

        return Ok(location);
    }

    [HttpGet("children/{parentId}")]
    public async Task<ActionResult<IEnumerable<LocationDto>>> GetChildLocations(int parentId)
    {
        var children = await _locationService.GetChildLocationsAsync(parentId);
        return Ok(children);
    }

    [HttpGet("search")]
    public async Task<ActionResult<IEnumerable<LocationDto>>> SearchLocations([FromQuery] string searchTerm)
    {
        var locations = await _locationService.SearchLocationsAsync(searchTerm);
        return Ok(locations);
    }

    [Authorize(Roles = "Admin")]
    [HttpPost]
    public async Task<ActionResult<LocationDto>> CreateLocation([FromBody] CreateLocationDto createLocationDto)
    {
        var result = await _locationService.CreateLocationAsync(createLocationDto);

        if (!result.Succeeded)
            return BadRequest(result.Message);

        return CreatedAtAction(nameof(GetLocation), new { id = result.Data.Id }, result.Data);
    }

    [Authorize(Roles = "Admin")]
    [HttpPut("{id}")]
    public async Task<ActionResult<LocationDto>> UpdateLocation(int id, [FromBody] UpdateLocationDto updateLocationDto)
    {
        var result = await _locationService.UpdateLocationAsync(id, updateLocationDto);

        if (!result.Succeeded)
            return BadRequest(result.Message);

        return Ok(result.Data);
    }

    [Authorize(Roles = "Admin")]
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteLocation(int id)
    {
        var result = await _locationService.DeleteLocationAsync(id);

        if (!result.Succeeded)
            return BadRequest(result.Message);

        return NoContent();
    }

    [Authorize(Roles = "Admin")]
    [HttpPatch("{id}/toggle-status")]
    public async Task<IActionResult> ToggleLocationStatus(int id)
    {
        var result = await _locationService.ToggleLocationStatusAsync(id);

        if (!result.Succeeded)
            return BadRequest(result.Message);

        return Ok(new { message = "Location status updated successfully" });
    }
}
