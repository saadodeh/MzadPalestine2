using Microsoft.EntityFrameworkCore;
using MzadPalestine.Core.Interfaces.Repositories;
using MzadPalestine.Core.Models;
using MzadPalestine.Core.Models.Common;
using MzadPalestine.Core.Models.Enums;
using System.Linq.Expressions;
using MzadPalestine.Infrastructure.Data;

namespace MzadPalestine.Infrastructure.Repositories;

public class AuctionRepository : GenericRepository<Auction>, IAuctionRepository
{
    private readonly ApplicationDbContext _context;

    public AuctionRepository(ApplicationDbContext context) : base(context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Auction>> GetActiveAuctionsAsync()
    {
        var now = DateTime.UtcNow;
        return await _context.Auctions
            .Include(a => a.Listing)
                .ThenInclude(l => l.Category)
            .Include(a => a.Bids)
                .ThenInclude(b => b.Bidder)
            .Where(a => a.StartDate <= now && a.EndDate > now && a.Status == AuctionStatus.Open)
            .OrderBy(a => a.EndDate)
            .ToListAsync();
    }

    public async Task<IEnumerable<Auction>> GetEndingSoonAuctionsAsync(int hours)
    {
        var now = DateTime.UtcNow;
        var threshold = now.AddHours(hours);
        return await _context.Auctions
            .Include(a => a.Listing)
                .ThenInclude(l => l.Category)
            .Include(a => a.Bids)
                .ThenInclude(b => b.Bidder)
            .Where(a => a.EndDate > now && a.EndDate <= threshold && a.Status == AuctionStatus.Open)
            .OrderBy(a => a.EndDate)
            .ToListAsync();
    }

    public async Task<Auction?> GetAuctionDetailsAsync(int auctionId)
    {
        return await _context.Auctions
            .Include(a => a.Listing)
                .ThenInclude(l => l.Images)
            .Include(a => a.Listing)
                .ThenInclude(l => l.Category)
            .Include(a => a.Listing)
                .ThenInclude(l => l.Location)
            .Include(a => a.Listing)
                .ThenInclude(l => l.User)
            .Include(a => a.Bids.OrderByDescending(b => b.Amount))
                .ThenInclude(b => b.User) // Â‰« Ì„ﬂ‰ﬂ «” »œ«· b.User »√Ì… ⁄·«ﬁ… „— »ÿ… »‹‹ Bid ≈–« ·“„ «·√„—
            .FirstOrDefaultAsync(a => a.Id == auctionId);
    }


    public async Task<IEnumerable<Auction>> GetUserParticipatedAuctionsAsync(int userId)
    {
        return await _context.Auctions
            .Include(a => a.Listing)
            .Include(a => a.Bids)
            .Where(a => a.Bids.Any(b => b.Id == userId))
            .OrderByDescending(a => a.EndDate)
            .ToListAsync();
    }

    public async Task<IEnumerable<Auction>> GetUserWonAuctionsAsync(int userId)
    {
        return await _context.Auctions
            .Include(a => a.Listing)
            .Include(a => a.Bids)
            .Where(a => a.WinnerId == userId)
            .OrderByDescending(a => a.EndDate)
            .ToListAsync();
    }

    public async Task<PagedList<Auction>> GetPagedAuctionsAsync(PaginationParams paginationParams)
    {
        var query = _context.Auctions
            .Include(a => a.Listing)
            .Include(a => a.Bids)
            .AsNoTracking();

        return await PagedList<Auction>.CreateAsync(query, paginationParams.PageNumber, paginationParams.PageSize);
    }

    public async Task<IEnumerable<Auction>> GetUserAuctionsAsync(string userId)
    {
        return await _context.Auctions
            .Include(a => a.Listing)
            .Include(a => a.Bids)
                .ThenInclude(b => b.Bidder)
            .Where(a => a.Listing.UserId == userId)
            .OrderByDescending(a => a.EndDate)
            .ToListAsync();
    }

    public async Task<IEnumerable<Auction>> GetListingAuctionsAsync(int listingId)
    {
        return await _context.Auctions
            .Include(a => a.Bids)
            .Where(a => a.ListingId == listingId)
            .OrderByDescending(a => a.EndDate)
            .ToListAsync();
    }

    public async Task<decimal> GetTotalUserAuctionsAmountAsync(int userId)
    {
        return await _context.Auctions
            .Where(a => a.WinnerId == userId)
            .SumAsync(a => a.WinnerId); // «› —÷ √‰ Amount ÂÊ «·Œ«’Ì… «· Ì  „À· «·„»·€
    }


    public async Task<IEnumerable<Auction>> GetWonAuctionsAsync(int userId)
    {
        return await _context.Auctions
            .Include(a => a.Listing)
            .Include(a => a.Bids)
            .Where(a => a.WinnerId == userId)
            .OrderByDescending(a => a.EndDate)
            .ToListAsync();
    }

    public async Task<bool> HasActiveBidsAsync(int auctionId)
    {
        return await _context.Bids.AnyAsync(b => b.AuctionId == auctionId);
    }

    public async Task<decimal> GetHighestBidAmountAsync(int auctionId)
    {
        var highestBid = await _context.Bids
            .Where(b => b.AuctionId == auctionId)
            .OrderByDescending(b => b.Amount)
            .FirstOrDefaultAsync();

        return highestBid?.Amount ?? 0;
    }

    public async Task<bool> IsUserHighestBidderAsync(int auctionId, string userId)
    {
        var highestBid = await _context.Bids
            .Where(b => b.AuctionId == auctionId)
            .OrderByDescending(b => b.Amount)
            .FirstOrDefaultAsync();

        return highestBid?.BidderId == userId;
    }

    public async Task<bool> HasUserBidAsync(int auctionId, string userId)
    {
        return await _context.Bids
            .AnyAsync(b => b.AuctionId == auctionId && b.BidderId == userId);
    }

    public async Task<IEnumerable<Auction>> GetActiveAuctionsAsync(PaginationParams paginationParams)
    {
        var query = _context.Auctions
            .Where(a => a.Status == AuctionStatus.Open && a.EndDate > DateTime.UtcNow)
            .OrderByDescending(a => a.EndDate);

        var skip = (paginationParams.PageNumber - 1) * paginationParams.PageSize;
        return await query
            .Skip(skip)
            .Take(paginationParams.PageSize)
            .ToListAsync();
    }

    public async Task<IEnumerable<Auction>> GetCompletedAuctionsAsync(PaginationParams paginationParams)
    {
        var query = _context.Auctions
            .Where(a => a.Status == AuctionStatus.Closed)
            .OrderByDescending(a => a.EndDate);

        var skip = (paginationParams.PageNumber - 1) * paginationParams.PageSize;
        return await query
            .Skip(skip)
            .Take(paginationParams.PageSize)
            .ToListAsync();
    }

    public void RemoveRange(IEnumerable<Auction> entities)
    {
        _context.Auctions.RemoveRange(entities);
    }

    public async Task<bool> ExistsAsync(int id)
    {
        return await _context.Auctions.AnyAsync(a => a.Id == id);
    }

    public IQueryable<Auction> GetQueryable()
    {
        return _context.Auctions;
    }

    public async Task<IEnumerable<Auction>> GetAllWithIncludesAsync(params Expression<Func<Auction, object>>[] includes)
    {
        var query = _context.Auctions.AsQueryable();
        foreach (var include in includes)
        {
            query = query.Include(include);
        }
        return await query.ToListAsync();
    }

    public async Task<PagedList<Auction>> GetPagedWithIncludesAsync(PaginationParams paginationParams, params Expression<Func<Auction, object>>[] includes)
    {
        var query = _context.Auctions.AsQueryable();
        foreach (var include in includes)
        {
            query = query.Include(include);
        }
        return await PagedList<Auction>.CreateAsync(query, paginationParams.PageNumber, paginationParams.PageSize);
    }

    public async Task SaveChangesAsync()
    {
        await _context.SaveChangesAsync();
    }

    public Task<IEnumerable<Auction>> GetListingAuctionsAsync(string listingId)
    {
        throw new NotImplementedException();
    }

    public Task<bool> HasActiveBidsAsync(string auctionId)
    {
        throw new NotImplementedException();
    }

    public Task<decimal> GetHighestBidAmountAsync(string auctionId)
    {
        throw new NotImplementedException();
    }

    public Task<bool> IsUserHighestBidderAsync(string userId, string auctionId)
    {
        throw new NotImplementedException();
    }

    public async Task<Auction> GetAuctionWithDetailsAsync(int id)
    {
        return await _context.Auctions
            .Include(a => a.Listing)
                .ThenInclude(l => l.Category)
            .Include(a => a.Bids)
                .ThenInclude(b => b.Bidder)
            .FirstOrDefaultAsync(a => a.Id == id);
    }

    public async Task<bool> IsSellerAsync(int auctionId, string userId)
    {
        var auction = await _context.Auctions
            .Include(a => a.Listing)
            .FirstOrDefaultAsync(a => a.Id == auctionId);

        return auction?.Listing?.UserId == userId;
    }

    public async Task<IEnumerable<Auction>> GetSellerAuctionsAsync(string userId)
    {
        return await _context.Auctions
            .Include(a => a.Listing)
            .Where(a => a.Listing.UserId == userId)
            .ToListAsync();
    }

    public async Task<bool> IsHighestBidderAsync(int auctionId, string userId)
    {
        var highestBid = await GetHighestBidAsync(auctionId);
        return highestBid?.BidderId == userId;
    }

    public async Task<ApplicationUser> GetWinnerAsync(int auctionId)
    {
        var highestBid = await _context.Bids
            .Include(b => b.Bidder)
            .Where(b => b.AuctionId == auctionId)
            .OrderByDescending(b => b.Amount)
            .FirstOrDefaultAsync();

        return highestBid?.Bidder;
    }

    public async Task<IEnumerable<Bid>> GetBidHistoryAsync(int auctionId)
    {
        return await _context.Bids
            .Include(b => b.Bidder)
            .Where(b => b.AuctionId == auctionId)
            .OrderByDescending(b => b.CreatedAt)
            .ToListAsync();
    }

    public async Task<bool> HasBidFromUserAsync(int auctionId, string userId)
    {
        return await _context.Bids
            .AnyAsync(b => b.AuctionId == auctionId && b.BidderId == userId);
    }

    public async Task<Bid> GetLastBidAsync(int auctionId)
    {
        return await _context.Bids
            .Include(b => b.Bidder)
            .Where(b => b.AuctionId == auctionId)
            .OrderByDescending(b => b.CreatedAt)
            .FirstOrDefaultAsync();
    }

    public async Task<bool> HasBidsAsync(int auctionId)
    {
        return await _context.Bids.AnyAsync(b => b.AuctionId == auctionId);
    }

    public async Task<bool> IsAuctionWinnerAsync(int auctionId, int userId)
    {
        var auction = await _context.Auctions
            .FirstOrDefaultAsync(a => a.Id == auctionId);
        return auction?.WinnerId == userId;
    }

    public async Task<bool> HasUserBidInAuctionAsync(int auctionId, string userId)
    {
        return await _context.Bids
            .AnyAsync(b => b.AuctionId == auctionId && b.BidderId == userId);
    }

    public async Task<IEnumerable<Auction>> GetExpiredAuctionsAsync()
    {
        var now = DateTime.UtcNow;
        return await _context.Auctions
            .Include(a => a.Bids)
                .ThenInclude(b => b.Bidder)
            .Where(a => a.EndDate <= now && a.Status == AuctionStatus.Open)
            .ToListAsync();
    }

    public async Task<bool> HasWinningBidAsync(int auctionId)
    {
        var highestBid = await GetHighestBidAsync(auctionId);
        return highestBid != null;
    }

    public async Task<Bid> GetWinningBidAsync(int auctionId)
    {
        return await GetHighestBidAsync(auctionId);
    }

    public async Task<ApplicationUser> GetWinningBidderAsync(int auctionId)
    {
        var highestBid = await GetHighestBidAsync(auctionId);
        return highestBid?.Bidder;
    }

    public async Task<decimal> GetWinningBidAmountAsync(int auctionId)
    {
        var highestBid = await GetHighestBidAsync(auctionId);
        return highestBid?.Amount ?? 0;
    }

    public async Task<IEnumerable<Auction>> GetClosedAuctionsAsync()
    {
        return await _context.Auctions
            .Include(a => a.Listing)
            .Include(a => a.Bids)
                .ThenInclude(b => b.Bidder)
            .Where(a => a.Status == AuctionStatus.Closed)
            .OrderByDescending(a => a.EndDate)
            .ToListAsync();
    }

    public async Task<IEnumerable<Auction>> GetOpenAuctionsAsync()
    {
        return await _context.Auctions
            .Include(a => a.Listing)
            .Include(a => a.Bids)
                .ThenInclude(b => b.Bidder)
            .Where(a => a.Status == AuctionStatus.Open)
            .OrderBy(a => a.EndDate)
            .ToListAsync();
    }

    public async Task<IEnumerable<Bid>> GetUserBidsAsync(int auctionId, string userId)
    {
        return await _context.Bids
            .Include(b => b.Bidder)
            .Where(b => b.AuctionId == auctionId && b.BidderId == userId)
            .OrderByDescending(b => b.Amount)
            .ToListAsync();
    }

    public async Task<Bid> GetHighestBidAsync(int auctionId)
    {
        return await _context.Bids
            .Include(b => b.Bidder)
            .Where(b => b.AuctionId == auctionId)
            .OrderByDescending(b => b.Amount)
            .FirstOrDefaultAsync();
    }

    public async Task<IEnumerable<Auction>> GetUserBiddingAuctionsAsync(string userId)
    {
        return await _context.Auctions
            .Include(a => a.Listing)
            .Include(a => a.Bids)
                .ThenInclude(b => b.Bidder)
            .Where(a => a.Bids.Any(b => b.BidderId == userId))
            .OrderByDescending(a => a.EndDate)
            .ToListAsync();
    }

    public async Task<IEnumerable<Bid>> GetBidsForAuctionAsync(int auctionId)
    {
        return await _context.Bids
            .Include(b => b.Bidder)
            .Where(b => b.AuctionId == auctionId)
            .OrderByDescending(b => b.Amount)
            .ToListAsync();
    }

    public async Task<decimal> GetCurrentHighestBidAmountAsync(int auctionId)
    {
        var highestBid = await GetHighestBidAsync(auctionId);
        return highestBid?.Amount ?? 0;
    }

    public async Task<IEnumerable<Bid>> GetUserBidsForAuctionAsync(int auctionId, string userId)
    {
        return await _context.Bids
            .Include(b => b.Bidder)
            .Where(b => b.AuctionId == auctionId && b.BidderId == userId)
            .OrderByDescending(b => b.Amount)
            .ToListAsync();
    }

    public async Task<bool> IsAuctionEndedAsync(int auctionId)
    {
        var auction = await _context.Auctions
            .FirstOrDefaultAsync(a => a.Id == auctionId);
        
        return auction != null && auction.EndDate <= DateTime.UtcNow;
    }

    public Task<decimal> GetTotalUserAuctionsAmountAsync(string userId)
    {
        throw new NotImplementedException();
    }

    public Task<IEnumerable<Auction>> GetWonAuctionsAsync(string userId)
    {
        throw new NotImplementedException();
    }
}
