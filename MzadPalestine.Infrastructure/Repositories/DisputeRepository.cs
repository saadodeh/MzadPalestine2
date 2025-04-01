using Microsoft.EntityFrameworkCore;
using MzadPalestine.Core.Interfaces.Repositories;
using MzadPalestine.Core.Models;
using MzadPalestine.Core.Models.Common;
using MzadPalestine.Infrastructure.Data;
using System.Linq.Expressions;

namespace MzadPalestine.Infrastructure.Repositories;

public class DisputeRepository : GenericRepository<Dispute>, IDisputeRepository
{
    private readonly ApplicationDbContext _context;

    public DisputeRepository(ApplicationDbContext context) : base(context)
    {
        _context = context;
    }

    public async Task<PagedList<Dispute>> GetUserDisputesAsync(string userId , PaginationParams parameters)
    {
        var query = _context.Disputes
            .Include(d => d.User)
            .Include(d => d.Auction)
            .Where(d => d.UserId == userId)
            .OrderByDescending(d => d.CreatedAt);

        return await PagedList<Dispute>.CreateAsync(query , parameters.PageNumber , parameters.PageSize);
    }

    public async Task<IEnumerable<Dispute>> GetDisputesByAuctionAsync(int auctionId)
    {
        return await _context.Disputes
            .Where(d => d.AuctionId == auctionId)
            .Include(d => d.User)
            .OrderByDescending(d => d.CreatedAt)
            .ToListAsync();
    }

    public async Task<PagedList<Dispute>> GetOpenDisputesAsync(PaginationParams parameters)
    {
        var query = _context.Disputes
            .Include(d => d.User)
            .Include(d => d.Auction)
            .Where(d => !d.IsResolved)
            .OrderByDescending(d => d.CreatedAt);

        return await PagedList<Dispute>.CreateAsync(query , parameters.PageNumber , parameters.PageSize);
    }

    public async Task<bool> HasUserDisputedAuctionAsync(string userId , int auctionId)
    {
        return await _context.Disputes
            .AnyAsync(d => d.UserId == userId && d.AuctionId == auctionId);
    }

    Task<IEnumerable<Dispute>> Core.Interfaces.IRepository<Dispute>.AddRangeAsync(IEnumerable<Dispute> entities)
    {
        throw new NotImplementedException();
    }

    public Task DeleteAsync(Dispute entity)
    {
        throw new NotImplementedException();
    }

    public Task DeleteRangeAsync(IEnumerable<Dispute> entities)
    {
        throw new NotImplementedException();
    }

    public Task<bool> ExistsAsync(Expression<Func<Dispute , bool>> predicate)
    {
        throw new NotImplementedException();
    }
}
