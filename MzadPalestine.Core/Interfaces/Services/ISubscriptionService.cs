using MzadPalestine.Core.DTOs.Subscriptions;
using MzadPalestine.Core.Models.Common;

namespace MzadPalestine.Core.Interfaces.Services;

public interface ISubscriptionService
{
    Task<Result<object>> CreateSubscriptionAsync(string userId, CreateSubscriptionDto subscriptionDto);
    Task<Result<object>> CancelSubscriptionAsync(string userId, int subscriptionId);
    Task<Result<object>> GetUserSubscriptionAsync(string userId);
    Task<Result<object>> GetAllSubscriptionPlansAsync();
    Task<Result<object>> GetSubscriptionByIdAsync(int subscriptionId);
    Task<PagedList<SubscriptionDto>> GetSubscriptionHistoryAsync(string userId, PaginationParams parameters);
    Task<IEnumerable<SubscriptionPlanDto>> GetSubscriptionPlansAsync();
}