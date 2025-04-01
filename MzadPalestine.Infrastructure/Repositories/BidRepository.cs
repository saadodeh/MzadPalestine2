using Microsoft.EntityFrameworkCore;
using MzadPalestine.Core.Interfaces;
using MzadPalestine.Core.Interfaces.Repositories;
using MzadPalestine.Core.Models;
using MzadPalestine.Infrastructure.Data;
using System.Linq.Expressions;

namespace MzadPalestine.Infrastructure.Repositories;

public class BidRepository : GenericRepository<Bid>, Core.Interfaces.Repositories.IBidRepository, Core.Interfaces.IBidRepository
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

    public async Task<bool> IsBidderAsync(int bidId , string userId)
    {
        var bid = await _context.Bids.FindAsync(bidId);
        return bid?.BidderId == userId;
    }

    public async Task<IEnumerable<Bid>> AddRangeAsync(IEnumerable<Bid> entities)
    {
        await _context.Bids.AddRangeAsync(entities);
        return entities;
    }

    public Task DeleteAsync(Bid entity)
    {
        _context.Bids.Remove(entity);
        return Task.CompletedTask;
    }

    public Task DeleteRangeAsync(IEnumerable<Bid> entities)
    {
        _context.Bids.RemoveRange(entities);
        return Task.CompletedTask;
    }

    public Task<bool> ExistsAsync(Expression<Func<Bid, bool>> predicate)
    {
        return _context.Bids.AnyAsync(predicate);
    }

    async Task<bool> Core.Interfaces.IBidRepository.DeleteAsync(int id)
    {
        var bid = await _context.Bids.FindAsync(id);
        if (bid == null) return false;
        _context.Bids.Remove(bid);
        return true;
    }

    async Task<bool> Core.Interfaces.IBidRepository.UpdateAsync(Bid bid)
    {
        var existingBid = await _context.Bids.FindAsync(bid.Id);
        if (existingBid == null) return false;
        _context.Entry(existingBid).CurrentValues.SetValues(bid);
        return true;
    }
}
