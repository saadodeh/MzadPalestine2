using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MzadPalestine.Core.DTOs.Listings;
using MzadPalestine.Core.Interfaces.Repositories;
using MzadPalestine.Core.Interfaces.Services;
using MzadPalestine.Core.Models;
using MzadPalestine.Core.Models.Common;
using MzadPalestine.Core.Models.Enums;

namespace MzadPalestine.API.Controllers;

public class ListingsController : BaseApiController
{
    private readonly IListingRepository _listingRepository;
    private readonly ICurrentUserService _currentUserService;
    private readonly IFileStorageService _fileStorageService;
    private readonly ICacheService _cacheService;

    public ListingsController(
        IListingRepository listingRepository,
        ICurrentUserService currentUserService,
        IFileStorageService fileStorageService,
        ICacheService cacheService)
    {
        _listingRepository = listingRepository;
        _currentUserService = currentUserService;
        _fileStorageService = fileStorageService;
        _cacheService = cacheService;
    }

    [HttpGet]
    public async Task<ActionResult<PagedList<ListingDto>>> GetListings(
        [FromQuery] ListingParams parameters)
    {
        var cacheKey = $"listings_{parameters}";
        var cachedResult = await _cacheService.GetAsync<PagedList<ListingDto>>(cacheKey);

        if (cachedResult != null)
        {
            return cachedResult;
        }

        var listings = await _listingRepository.GetPagedListingsAsync(
            parameters.PageNumber,
            parameters.PageSize,
            parameters.SearchTerm,
            parameters.CategoryId,
            parameters.LocationId,
            parameters.Status);

        var result = new PagedList<ListingDto>(
            listings.Items.Select(l => new ListingDto
            {
                Id = l.Id,
                Title = l.Title,
                Description = l.Description,
                StartingPrice = l.StartingPrice,
                Status = l.Status,
                CategoryId = l.CategoryId,
                CategoryName = l.Category?.Name,
                LocationId = l.LocationId,
                LocationName = l.Location?.Name,
                SellerId = l.SellerId,
                SellerName = $"{l.Seller?.FirstName} {l.Seller?.LastName}",
                Images = l.Images.Select(i => i.Url).ToList(),
                CreatedAt = l.CreatedAt
            }).ToList(),
            listings.TotalCount,
            listings.CurrentPage,
            listings.PageSize);

        await _cacheService.SetAsync(cacheKey, result, TimeSpan.FromMinutes(5));

        return result;
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<ListingDetailsDto>> GetListing(int id)
    {
        var cacheKey = $"listing_{id}";
        var cachedResult = await _cacheService.GetAsync<ListingDetailsDto>(cacheKey);

        if (cachedResult != null)
        {
            return cachedResult;
        }

        var listing = await _listingRepository.GetListingDetailsAsync(id);

        if (listing == null)
        {
            return NotFound();
        }

        var result = new ListingDetailsDto
        {
            Id = listing.Id,
            Title = listing.Title,
            Description = listing.Description,
            StartingPrice = listing.StartingPrice,
            Status = listing.Status,
            CategoryId = listing.CategoryId,
            CategoryName = listing.Category?.Name,
            LocationId = listing.LocationId,
            LocationName = listing.Location?.Name,
            SellerId = listing.SellerId,
            SellerName = $"{listing.Seller?.FirstName} {listing.Seller?.LastName}",
            SellerRating = listing.Seller?.Rating ?? 0,
            Images = listing.Images.Select(i => i.Url).ToList(),
            CreatedAt = listing.CreatedAt,
            Auction = listing.Auction != null ? new AuctionDto
            {
                Id = listing.Auction.Id,
                StartDate = listing.Auction.StartDate,
                EndDate = listing.Auction.EndDate,
                CurrentPrice = listing.Auction.CurrentPrice,
                Status = listing.Auction.Status,
                BidCount = listing.Auction.Bids.Count
            } : null
        };

        await _cacheService.SetAsync(cacheKey, result, TimeSpan.FromMinutes(5));

        return result;
    }

    [Authorize]
    [HttpPost]
    public async Task<ActionResult<ListingDto>> CreateListing([FromForm] CreateListingRequest request)
    {
        var listing = new Listing
        {
            Title = request.Title,
            Description = request.Description,
            StartingPrice = request.StartingPrice,
            CategoryId = request.CategoryId,
            LocationId = request.LocationId,
            SellerId = _currentUserService.UserId!,
            Status = ListingStatus.Pending,
            CreatedAt = DateTime.UtcNow
        };

        if (request.Images != null && request.Images.Any())
        {
            foreach (var image in request.Images)
            {
                var imageUrl = await _fileStorageService.SaveFileAsync(image, "listings");
                listing.Images.Add(new ListingImage { Url = imageUrl });
            }
        }

        await _listingRepository.AddAsync(listing);

        return CreatedAtAction(nameof(GetListing), new { id = listing.Id }, new ListingDto
        {
            Id = listing.Id,
            Title = listing.Title,
            Description = listing.Description,
            StartingPrice = listing.StartingPrice,
            Status = listing.Status,
            CategoryId = listing.CategoryId,
            LocationId = listing.LocationId,
            SellerId = listing.SellerId,
            Images = listing.Images.Select(i => i.Url).ToList(),
            CreatedAt = listing.CreatedAt
        });
    }

    [Authorize]
    [HttpPut("{id}")]
    public async Task<ActionResult<ListingDto>> UpdateListing(int id, UpdateListingRequest request)
    {
        var listing = await _listingRepository.GetByIdAsync(id);

        if (listing == null)
        {
            return NotFound();
        }

        if (listing.SellerId != _currentUserService.UserId)
        {
            return Forbid();
        }

        if (listing.Status != ListingStatus.Pending && listing.Status != ListingStatus.Draft)
        {
            return BadRequest("Cannot update an active or completed listing");
        }

        listing.Title = request.Title;
        listing.Description = request.Description;
        listing.StartingPrice = request.StartingPrice;
        listing.CategoryId = request.CategoryId;
        listing.LocationId = request.LocationId;

        await _listingRepository.UpdateAsync(listing);

        // Invalidate cache
        await _cacheService.RemoveAsync($"listing_{id}");

        return new ListingDto
        {
            Id = listing.Id,
            Title = listing.Title,
            Description = listing.Description,
            StartingPrice = listing.StartingPrice,
            Status = listing.Status,
            CategoryId = listing.CategoryId,
            LocationId = listing.LocationId,
            SellerId = listing.SellerId,
            Images = listing.Images.Select(i => i.Url).ToList(),
            CreatedAt = listing.CreatedAt
        };
    }

    [Authorize]
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteListing(int id)
    {
        var listing = await _listingRepository.GetByIdAsync(id);

        if (listing == null)
        {
            return NotFound();
        }

        if (listing.SellerId != _currentUserService.UserId && !_currentUserService.IsAdmin)
        {
            return Forbid();
        }

        if (listing.Status == ListingStatus.Active)
        {
            return BadRequest("Cannot delete an active listing");
        }

        // Delete images
        foreach (var image in listing.Images)
        {
            await _fileStorageService.DeleteFileAsync(image.Url);
        }

        await _listingRepository.DeleteAsync(listing);

        // Invalidate cache
        await _cacheService.RemoveAsync($"listing_{id}");

        return NoContent();
    }

    [Authorize]
    [HttpPost("{id}/images")]
    public async Task<ActionResult<IEnumerable<string>>> AddListingImages(int id, [FromForm] IFormFileCollection images)
    {
        var listing = await _listingRepository.GetByIdAsync(id);

        if (listing == null)
        {
            return NotFound();
        }

        if (listing.SellerId != _currentUserService.UserId)
        {
            return Forbid();
        }

        var imageUrls = new List<string>();

        foreach (var image in images)
        {
            var imageUrl = await _fileStorageService.SaveFileAsync(image, "listings");
            listing.Images.Add(new ListingImage { Url = imageUrl });
            imageUrls.Add(imageUrl);
        }

        await _listingRepository.UpdateAsync(listing);

        // Invalidate cache
        await _cacheService.RemoveAsync($"listing_{id}");

        return imageUrls;
    }

    [Authorize]
    [HttpDelete("{id}/images/{imageId}")]
    public async Task<IActionResult> DeleteListingImage(int id, int imageId)
    {
        var listing = await _listingRepository.GetByIdAsync(id);

        if (listing == null)
        {
            return NotFound();
        }

        if (listing.SellerId != _currentUserService.UserId)
        {
            return Forbid();
        }

        var image = listing.Images.FirstOrDefault(i => i.Id == imageId);

        if (image == null)
        {
            return NotFound();
        }

        await _fileStorageService.DeleteFileAsync(image.Url);
        listing.Images.Remove(image);

        await _listingRepository.UpdateAsync(listing);

        // Invalidate cache
        await _cacheService.RemoveAsync($"listing_{id}");

        return NoContent();
    }
}
