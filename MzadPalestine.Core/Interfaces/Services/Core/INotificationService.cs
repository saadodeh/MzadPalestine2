using System.Threading.Tasks;

using MzadPalestine.Core.DTOs.Notifications;
using MzadPalestine.Core.Models;
using MzadPalestine.Core.Models.Common;

namespace MzadPalestine.Core.Interfaces.Services;

public interface INotificationService
{
    Task<Result<bool>> SendNotificationAsync(SendNotificationDto notificationDto);
    Task<Result<bool>> SendNotificationAsync(string userId , string title , string message , string? route = null);
    Task<PagedResult<NotificationDto>> GetUserNotificationsAsync(string userId , PaginationParams parameters);
    Task<Result<bool>> MarkNotificationAsReadAsync(string userId , int notificationId);
    Task<int> GetUnreadNotificationCountAsync(string userId);
    Task MarkAllNotificationsAsReadAsync(string userId);
}