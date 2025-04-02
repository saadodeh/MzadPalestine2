using Microsoft.EntityFrameworkCore;
using MzadPalestine.Core.Interfaces.Repositories;
using MzadPalestine.Core.Models;
using MzadPalestine.Core.Models.Common;
using MzadPalestine.Core.Models.Enums;
using MzadPalestine.Infrastructure.Data;

namespace MzadPalestine.Infrastructure.Repositories;

/// <summary>
/// Repository for managing listing-related database operations
/// </summary>
public class ListingRepository : GenericRepository<Listing>, IListingRepository
{
    private readonly ApplicationDbContext _context;

    public ListingRepository(ApplicationDbContext context) : base(context)
    {
        _context = context;
    }

    /// <summary>
    /// Retrieves a paged list of listings with optional filtering
    /// </summary>
    public async Task<PagedList<Listing>> GetPagedListingsAsync(
        int pageNumber,
        int pageSize,
        string? searchTerm = null,
        int? categoryId = null,
        int? locationId = null,
        ListingStatus? status = null)
    {
        var query = _context.Listings
            .Include(l => l.Category)
            .Include(l => l.Images)
            .Include(l => l.Location)
            .Where(l => !l.IsDeleted);

        query = ApplyFilters(query, searchTerm, categoryId, locationId, status);
        query = query.OrderByDescending(l => l.CreatedAt);

        return await PagedList<Listing>.CreateAsync(query, pageNumber, pageSize);
    }

    /// <summary>
    /// Retrieves listings for a specific user
    /// </summary>
    public async Task<IEnumerable<Listing>> GetUserListingsAsync(string userId)
    {
        return await _context.Listings
            .Where(l => l.SellerId == userId && !l.IsDeleted)
            .Include(l => l.Images)
            .Include(l => l.Category)
            .OrderByDescending(l => l.CreatedAt)
            .ToListAsync();
    }

    /// <summary>
    /// Retrieves all active listings
    /// </summary>
    public async Task<IEnumerable<Listing>> GetActiveListingsAsync()
    {
        return await _context.Listings
            .Where(l => !l.IsDeleted && l.Status == ListingStatus.Active)
            .Include(l => l.Images)
            .Include(l => l.Category)
            .OrderByDescending(l => l.CreatedAt)
            .ToListAsync();
    }

    /// <summary>
    /// Retrieves listings by category
    /// </summary>
    public async Task<IEnumerable<Listing>> GetListingsByCategoryAsync(string categoryId)
    {
        if (!int.TryParse(categoryId, out int id))
        {
            return new List<Listing>();
        }

        return await _context.Listings
            .Where(l => l.CategoryId == id && !l.IsDeleted && l.Status == ListingStatus.Active)
            .Include(l => l.Images)
            .Include(l => l.Category)
            .OrderByDescending(l => l.CreatedAt)
            .ToListAsync();
    }

    /// <summary>
    /// Checks if a user is the owner of a listing
    /// </summary>
    public async Task<bool> IsListingOwnerAsync(string listingId, string userId)
    {
        if (!int.TryParse(listingId, out int id))
        {
            return false;
        }

        var listing = await _context.Listings
            .FirstOrDefaultAsync(l => l.Id == id && !l.IsDeleted);
        return listing?.SellerId == userId;
    }

    /// <summary>
    /// Soft deletes a listing
    /// </summary>
    public async Task<bool> DeleteAsync(Listing listing)
    {
        listing.IsDeleted = true;
        listing.DeletedAt = DateTime.UtcNow;
        _context.Listings.Update(listing);
        return await _context.SaveChangesAsync() > 0;
    }

    /// <summary>
    /// Retrieves detailed information for a specific listing
    /// </summary>
    public async Task<Listing?> GetListingDetailsAsync(int id)
    {
        return await _context.Listings
            .Include(l => l.Images)
            .Include(l => l.Seller)
            .Include(l => l.Category)
            .Include(l => l.Location)
            .FirstOrDefaultAsync(l => l.Id == id && !l.IsDeleted);
    }

    /// <summary>
    /// Searches listings based on a search term
    /// </summary>
    public async Task<IEnumerable<Listing>> SearchListingsAsync(string searchTerm)
    {
        return await _context.Listings
            .Where(l => !l.IsDeleted &&
                       (l.Title.Contains(searchTerm) ||
                        l.Description.Contains(searchTerm)))
            .Include(l => l.Category)
            .Include(l => l.Images)
            .OrderByDescending(l => l.CreatedAt)
            .ToListAsync();
    }

    /// <summary>
    /// Applies filters to the listing query
    /// </summary>
    private static IQueryable<Listing> ApplyFilters(
        IQueryable<Listing> query,
        string? searchTerm,
        int? categoryId,
        int? locationId,
        ListingStatus? status)
    {
        if (!string.IsNullOrEmpty(searchTerm))
        {
            query = query.Where(l => l.Title.Contains(searchTerm) || 
                                    l.Description.Contains(searchTerm));
        }

        if (categoryId.HasValue)
        {
            query = query.Where(l => l.CategoryId == categoryId);
        }

        if (locationId.HasValue)
        {
            query = query.Where(l => l.LocationId == locationId);
        }

        if (status.HasValue)
        {
            query = query.Where(l => l.Status == status);
        }

        return query;
    }
}
