using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MzadPalestine.Core.DTOs.Listings;
using MzadPalestine.Core.Interfaces.Services;
using MzadPalestine.Core.Models.Common;

namespace MzadPalestine.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ProductController : ControllerBase
{
    private readonly IListingService _listingService;
    private readonly ICurrentUserService _currentUserService;

    public ProductController(IListingService listingService , ICurrentUserService currentUserService)
    {
        _listingService = listingService;
        _currentUserService = currentUserService;
    }

    [Authorize]
    [HttpPost("create")]
    public async Task<IActionResult> CreateProduct([FromBody] CreateListingDto createListingDto)
    {
        var userId = _currentUserService.GetUserId();
        var result = await _listingService.CreateListingAsync(createListingDto , userId);

        if (result == null)
            return BadRequest("Failed to create listing");

        return CreatedAtAction(nameof(GetProduct) , new { productId = result.Id } , result);
    }

    [Authorize]
    [HttpPut("update/{productId}")]
    public async Task<IActionResult> UpdateProduct(int productId , [FromBody] UpdateListingDto updateListingDto)
    {
        var userId = _currentUserService.GetUserId();
        var result = await _listingService.UpdateListingAsync(productId , updateListingDto , userId);

        if (!result)
            return BadRequest("Failed to update listing");

        return Ok("Listing updated successfully");
    }

    [Authorize]
    [HttpDelete("delete/{productId}")]
    public async Task<IActionResult> DeleteProduct(int productId)
    {
        var userId = _currentUserService.GetUserId();
        var result = await _listingService.DeleteListingAsync(productId , userId);

        if (!result)
            return BadRequest("Failed to delete listing");

        return Ok("Listing deleted successfully");
    }

    [HttpGet("all")]
    public async Task<IActionResult> GetAllProducts([FromQuery] PaginationParams parameters)
    {
        var result = await _listingService.GetListingsAsync(page: parameters.PageNumber , pageSize: parameters.PageSize);
        return Ok(result);
    }

    [HttpGet("{productId}")]
    public async Task<IActionResult> GetProduct(int productId)
    {
        var result = await _listingService.GetListingByIdAsync(productId);

        if (result == null)
            return NotFound("Listing not found");

        return Ok(result);
    }

    [HttpGet("search")]
    public async Task<IActionResult> SearchProducts([FromQuery] string searchTerm , [FromQuery] PaginationParams parameters)
    {
        var result = await _listingService.GetListingsAsync(searchTerm: searchTerm , page: parameters.PageNumber , pageSize: parameters.PageSize);
        return Ok(result);
    }
}
