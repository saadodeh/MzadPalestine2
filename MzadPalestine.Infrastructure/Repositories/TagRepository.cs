using Microsoft.EntityFrameworkCore;
using MzadPalestine.Core.Interfaces.Repositories;
using MzadPalestine.Core.Models;
using MzadPalestine.Infrastructure.Data;

namespace MzadPalestine.Infrastructure.Repositories;

public class TagRepository : GenericRepository<Tag>, ITagRepository
{
    private readonly ApplicationDbContext _context;

    public TagRepository(ApplicationDbContext context) : base(context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Tag>> GetTagsByListingIdAsync(int listingId)
    {
        return await _context.Tags
            .Include(t => t.ListingTags)
            .Where(t => t.ListingTags.Any(lt => lt.ListingId == listingId))
            .ToListAsync();
    }

    public async Task<IEnumerable<Tag>> GetPopularTagsAsync(int count)
    {
        return await _context.Tags
            .Include(t => t.ListingTags)
            .OrderByDescending(t => t.ListingTags.Count)
            .Take(count)
            .ToListAsync();
    }

    public async Task<IEnumerable<Tag>> SearchTagsAsync(string searchTerm)
    {
        return await _context.Tags
            .Where(t => t.Name.Contains(searchTerm))
            .OrderBy(t => t.Name)
            .ToListAsync();
    }

    public async Task<IEnumerable<Tag>> GetListingTagsAsync(int listingId)
    {
        var listing = await _context.Listings
            .Include(l => l.Tags)
            .FirstOrDefaultAsync(l => l.Id == listingId);

        return listing?.Tags ?? new List<Tag>();
    }

    public async Task AddTagToListingAsync(int listingId, int tagId)
    {
        var listing = await _context.Listings.FindAsync(listingId);
        var tag = await _context.Tags.FindAsync(tagId);

        if (listing != null && tag != null)
        {
            listing.Tags.Add(tag);
            await _context.SaveChangesAsync();
        }
    }

    public async Task RemoveTagFromListingAsync(int listingId, int tagId)
    {
        var listing = await _context.Listings
            .Include(l => l.Tags)
            .FirstOrDefaultAsync(l => l.Id == listingId);
        var tag = await _context.Tags.FindAsync(tagId);

        if (listing != null && tag != null)
        {
            listing.Tags.Remove(tag);
            await _context.SaveChangesAsync();
        }
    }
}
