using MzadPalestine.Core.Models;

namespace MzadPalestine.Core.Interfaces.Repositories;

public interface ISubscriptionRepository : IGenericRepository<Subscription>
{
    Task<IEnumerable<Subscription>> GetActiveSubscriptionsAsync();
    Task<IEnumerable<Subscription>> GetUserSubscriptionsAsync(string userId);
    Task<Subscription?> GetActiveUserSubscriptionAsync(string userId);
}
