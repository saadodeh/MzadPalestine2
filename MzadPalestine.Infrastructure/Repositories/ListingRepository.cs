using Microsoft.EntityFrameworkCore;
using MzadPalestine.Core.Interfaces.Repositories;
using MzadPalestine.Core.Models;
using MzadPalestine.Core.Models.Common;
using MzadPalestine.Core.Models.Enums;
using MzadPalestine.Infrastructure.Data;

namespace MzadPalestine.Infrastructure.Repositories;

public class ListingRepository : GenericRepository<Listing>, IListingRepository
{
    private readonly ApplicationDbContext _context;

    public ListingRepository(ApplicationDbContext context) : base(context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Listing>> GetUserListingsAsync(string userId)
    {
        return await _context.Listings
            .Where(l => l.UserId == userId)
            .Include(l => l.Images)
            .OrderByDescending(l => l.CreatedAt)
            .ToListAsync();
    }

    public async Task<IEnumerable<Listing>> GetActiveListingsAsync()
    {
        return await _context.Listings
            .Where(l => !l.IsDeleted && l.Status == ListingStatus.Active)
            .OrderByDescending(l => l.CreatedAt)
            .ToListAsync();
    }

    public async Task<IEnumerable<Listing>> GetListingsByCategoryAsync(int categoryId)
    {
        return await _context.Listings
            .Where(l => l.CategoryId == categoryId && !l.IsDeleted && l.Status == ListingStatus.Active)
            .OrderByDescending(l => l.CreatedAt)
            .ToListAsync();
    }

    public async Task<IEnumerable<Listing>> SearchListingsAsync(string searchTerm)
    {
        return await _context.Listings
            .Where(l => l.Title.Contains(searchTerm) || l.Description.Contains(searchTerm))
            .Where(l => !l.IsDeleted && l.Status == ListingStatus.Active)
            .OrderByDescending(l => l.CreatedAt)
            .ToListAsync();
    }

    public async Task<bool> IsOwnerAsync(int listingId, string userId)
    {
        var listing = await _context.Listings
            .FirstOrDefaultAsync(l => l.Id == listingId);
        
        return listing?.UserId == userId;
    }

    public Task<IEnumerable<Listing>> GetListingsByCategoryAsync(string categoryId)
    {
        throw new NotImplementedException();
    }

    public Task<bool> IsListingOwnerAsync(string listingId, string userId)
    {
        throw new NotImplementedException();
    }
}
