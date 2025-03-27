using Microsoft.EntityFrameworkCore;
using MzadPalestine.Core.Interfaces.Repositories;
using MzadPalestine.Core.Models;
using MzadPalestine.Infrastructure.Data;

namespace MzadPalestine.Infrastructure.Repositories;

public class BidRepository : GenericRepository<Bid>, IBidRepository
{
    private readonly ApplicationDbContext _context;

    public BidRepository(ApplicationDbContext context) : base(context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Bid>> GetAuctionBidsAsync(int auctionId)
    {
        return await _context.Bids
            .Include(b => b.Bidder)
            .Where(b => b.AuctionId == auctionId)
            .OrderByDescending(b => b.Amount)
            .ToListAsync();
    }

    public async Task<Bid?> GetHighestBidAsync(int auctionId)
    {
        return await _context.Bids
            .Include(b => b.Bidder)
            .Where(b => b.AuctionId == auctionId)
            .OrderByDescending(b => b.Amount)
            .FirstOrDefaultAsync();
    }

    public async Task<IEnumerable<Bid>> GetUserBidsAsync(string userId)
    {
        return await _context.Bids
            .Include(b => b.Auction)
            .Include(b => b.Bidder)
            .Where(b => b.BidderId == userId)
            .OrderByDescending(b => b.CreatedAt)
            .ToListAsync();
    }

    public async Task<decimal> GetTotalBidsAmountAsync(string userId)
    {
        return await _context.Bids
            .Where(b => b.BidderId == userId)
            .SumAsync(b => b.Amount);
    }

    public Task<IEnumerable<Bid>> GetAuctionBidsAsync(string auctionId)
    {
        throw new NotImplementedException();
    }

    public Task<Bid?> GetHighestBidAsync(string auctionId)
    {
        throw new NotImplementedException();
    }

    public async Task<Bid> GetBidWithDetailsAsync(int bidId)
    {
        return await _context.Bids
            .Include(b => b.Auction)
            .Include(b => b.Bidder)
            .FirstOrDefaultAsync(b => b.Id == bidId);
    }

    public async Task<bool> IsBidderAsync(int bidId, string userId)
    {
        var bid = await _context.Bids.FindAsync(bidId);
        return bid?.BidderId == userId;
    }
}
