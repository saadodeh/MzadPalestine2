using MzadPalestine.Core.Models;
using MzadPalestine.Core.Models.Common;

namespace MzadPalestine.Core.Interfaces;

public interface INotificationRepository : IGenericRepository<Notification>
{
    Task<PagedList<Notification>> GetUserNotificationsAsync(string userId, PaginationParams parameters);
    Task<int> GetUnreadNotificationsCountAsync(string userId);
    Task MarkAsReadAsync(int notificationId);
    Task MarkAllAsReadAsync(string userId);
    Task<bool> IsNotificationRecipientAsync(string userId, int notificationId);
    Task<IEnumerable<Notification>> GetRecentNotificationsAsync(string userId, int count);
    Task<Notification?> GetNotificationWithDetailsAsync(int notificationId);
    Task DeleteOldNotificationsAsync(int daysOld);
    Task<bool> HasUserEnabledNotificationTypeAsync(string userId, NotificationType type);
    Task UpdateUserNotificationPreferencesAsync(string userId, IEnumerable<NotificationType> enabledTypes);
}
