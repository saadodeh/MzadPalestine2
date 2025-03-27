using Microsoft.EntityFrameworkCore;
using MzadPalestine.Core.Interfaces.Repositories;
using MzadPalestine.Core.Models;
using MzadPalestine.Infrastructure.Data;

namespace MzadPalestine.Infrastructure.Repositories;

public class InvoiceRepository : GenericRepository<Invoice>, IInvoiceRepository
{
    private readonly ApplicationDbContext _context;

    public InvoiceRepository(ApplicationDbContext context) : base(context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Invoice>> GetUserInvoicesAsync(string userId)
    {
        return await _context.Invoices
            .Include(i => i.User)
            .Include(i => i.Auction)
            .Where(i => i.UserId == userId)
            .OrderByDescending(i => i.CreatedAt)
            .ToListAsync();
    }

    public async Task<IEnumerable<Invoice>> GetAuctionInvoicesAsync(string auctionId)
    {
        return await _context.Invoices
            .Include(i => i.User)
            .Include(i => i.Auction)
            .Where(i => i.AuctionId == auctionId)
            .OrderByDescending(i => i.CreatedAt)
            .ToListAsync();
    }

    public async Task<decimal> GetTotalUserInvoicesAmountAsync(string userId)
    {
        return await _context.Invoices
            .Where(i => i.UserId == userId)
            .SumAsync(i => i.Amount);
    }
}
