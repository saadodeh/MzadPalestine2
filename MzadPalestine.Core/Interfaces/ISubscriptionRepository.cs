using MzadPalestine.Core.Models;
using MzadPalestine.Core.Models.Common;

namespace MzadPalestine.Core.Interfaces;

public interface ISubscriptionRepository : IGenericRepository<Subscription>
{
    Task<Subscription?> GetActiveSubscriptionAsync(string userId);
    Task<PagedList<Subscription>> GetUserSubscriptionHistoryAsync(string userId, PaginationParams parameters);
    Task<bool> HasActiveSubscriptionAsync(string userId);
    Task<DateTime?> GetSubscriptionEndDateAsync(string userId);
    Task<IEnumerable<SubscriptionPlan>> GetAvailablePlansAsync();
    Task<bool> IsValidPlanAsync(int planId);
    Task CancelSubscriptionAsync(string userId);
    Task<bool> IsSubscriptionExpiredAsync(string userId);
    Task<IEnumerable<Subscription>> GetExpiringSubscriptionsAsync(int daysThreshold);
    Task<Dictionary<string, int>> GetSubscriptionStatisticsAsync();
}
