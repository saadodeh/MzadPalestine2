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

    public ProductController(IListingService listingService, ICurrentUserService currentUserService)
    {
        _listingService = listingService;
        _currentUserService = currentUserService;
    }

    [Authorize]
    [HttpPost("create")]
    public async Task<IActionResult> CreateProduct([FromBody] CreateListingDto createListingDto)
    {
        var userId = _currentUserService.GetUserId();
        var result = await _listingService.CreateListingAsync(userId, createListingDto);
        
        if (!result.Succeeded)
            return BadRequest(result.Message);

        return CreatedAtAction(nameof(GetProduct), new { productId = result.Data.Id }, result.Data);
    }

    [Authorize]
    [HttpPut("update/{productId}")]
    public async Task<IActionResult> UpdateProduct(int productId, [FromBody] UpdateListingDto updateListingDto)
    {
        var userId = _currentUserService.GetUserId();
        var result = await _listingService.UpdateListingAsync(userId, productId, updateListingDto);
        
        if (!result.Succeeded)
            return BadRequest(result.Message);

        return Ok(result.Data);
    }

    [Authorize]
    [HttpDelete("delete/{productId}")]
    public async Task<IActionResult> DeleteProduct(int productId)
    {
        var userId = _currentUserService.GetUserId();
        var result = await _listingService.DeleteListingAsync(userId, productId);
        
        if (!result.Succeeded)
            return BadRequest(result.Message);

        return Ok(new { message = "Product deleted successfully" });
    }

    [HttpGet("all")]
    public async Task<IActionResult> GetAllProducts([FromQuery] PaginationParams parameters)
    {
        var result = await _listingService.GetAllListingsAsync(parameters);
        return Ok(result);
    }

    [HttpGet("{productId}")]
    public async Task<IActionResult> GetProduct(int productId)
    {
        var result = await _listingService.GetListingByIdAsync(productId);
        
        if (!result.Succeeded)
            return NotFound(result.Message);

        return Ok(result.Data);
    }

    [HttpGet("search")]
    public async Task<IActionResult> SearchProducts([FromQuery] string searchTerm, [FromQuery] PaginationParams parameters)
    {
        var result = await _listingService.SearchListingsAsync(searchTerm, parameters);
        return Ok(result);
    }
}
