using Microsoft.EntityFrameworkCore;
using MzadPalestine.Core.Interfaces.Repositories;
using MzadPalestine.Core.Models;
using MzadPalestine.Infrastructure.Data;

namespace MzadPalestine.Infrastructure.Repositories;

public class DisputeRepository : GenericRepository<Dispute>, IDisputeRepository
{
    private readonly ApplicationDbContext _context;

    public DisputeRepository(ApplicationDbContext context) : base(context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Dispute>> GetUserDisputesAsync(string userId)
    {
        return await _context.Disputes
            .Include(d => d.User)
            .Include(d => d.Auction)
            .Where(d => d.UserId == userId)
            .OrderByDescending(d => d.CreatedAt)
            .ToListAsync();
    }

    public async Task<IEnumerable<Dispute>> GetAuctionDisputesAsync(int auctionId)
    {
        return await _context.Disputes
            .Where(d => d.AuctionId == auctionId)
            .Include(d => d.User)
            .ToListAsync();
    }

    public async Task<bool> HasUserDisputedAuctionAsync(string userId, int auctionId)
    {
        return await _context.Disputes
            .AnyAsync(d => d.UserId == userId && d.AuctionId == auctionId);
    }
}
