using MzadPalestine.Core.Models;
using MzadPalestine.Core.Models.Common;

namespace MzadPalestine.Core.Interfaces;

public interface ISubscriptionRepository : IGenericRepository<Subscription>
{
    Task<Subscription?> GetUserActiveSubscriptionAsync(string userId);
    Task<PagedList<Subscription>> GetUserSubscriptionHistoryAsync(string userId, PaginationParams parameters);
    Task<IEnumerable<Subscription>> GetExpiringSubscriptionsAsync(int daysThreshold);
    Task<bool> HasActiveSubscriptionAsync(string userId);
}
