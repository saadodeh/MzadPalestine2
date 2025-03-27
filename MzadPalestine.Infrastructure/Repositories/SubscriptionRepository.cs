using Microsoft.EntityFrameworkCore;
using MzadPalestine.Core.Interfaces.Repositories;
using MzadPalestine.Core.Models;
using MzadPalestine.Core.Models.Common;
using MzadPalestine.Infrastructure.Data;

namespace MzadPalestine.Infrastructure.Repositories;

public class SubscriptionRepository : GenericRepository<Subscription>, ISubscriptionRepository
{
    private readonly ApplicationDbContext _context;

    public SubscriptionRepository(ApplicationDbContext context) : base(context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Subscription>> GetActiveSubscriptionsAsync()
    {
        return await _context.Subscriptions
            .Include(s => s.User)
            .Where(s => s.EndDate > DateTime.UtcNow)
            .OrderByDescending(s => s.CreatedAt)
            .ToListAsync();
    }

    public async Task<IEnumerable<Subscription>> GetUserSubscriptionsAsync(string userId)
    {
        return await _context.Subscriptions
            .Include(s => s.User)
            .Where(s => s.UserId == userId)
            .OrderByDescending(s => s.CreatedAt)
            .ToListAsync();
    }

    public async Task<Subscription?> GetActiveUserSubscriptionAsync(string userId)
    {
        return await _context.Subscriptions
            .Include(s => s.User)
            .Where(s => s.UserId == userId && s.EndDate > DateTime.UtcNow)
            .OrderByDescending(s => s.CreatedAt)
            .FirstOrDefaultAsync();
    }

    public async Task<Subscription?> GetUserActiveSubscriptionAsync(string userId)
    {
        return await _context.Subscriptions
            .FirstOrDefaultAsync(s => 
                s.UserId == userId && 
                s.Status == SubscriptionStatus.Active &&
                s.EndDate > DateTime.UtcNow);
    }

    public async Task<PagedList<Subscription>> GetUserSubscriptionHistoryAsync(string userId, PaginationParams parameters)
    {
        var query = _context.Subscriptions
            .Where(s => s.UserId == userId)
            .OrderByDescending(s => s.StartDate);

        return await PagedList<Subscription>.CreateAsync(query, parameters.PageNumber, parameters.PageSize);
    }

    public async Task<IEnumerable<Subscription>> GetExpiringSubscriptionsAsync(int daysThreshold)
    {
        var thresholdDate = DateTime.UtcNow.AddDays(daysThreshold);
        return await _context.Subscriptions
            .Where(s => 
                s.Status == SubscriptionStatus.Active &&
                s.EndDate <= thresholdDate &&
                s.EndDate > DateTime.UtcNow)
            .OrderBy(s => s.EndDate)
            .ToListAsync();
    }

    public async Task<bool> HasActiveSubscriptionAsync(string userId)
    {
        return await _context.Subscriptions
            .AnyAsync(s => 
                s.UserId == userId && 
                s.Status == SubscriptionStatus.Active &&
                s.EndDate > DateTime.UtcNow);
    }
}
