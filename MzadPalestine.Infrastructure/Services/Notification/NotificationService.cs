using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using MzadPalestine.Core.DTOs.Notifications;
using MzadPalestine.Core.Interfaces.Services;
using MzadPalestine.Core.Models;
using MzadPalestine.Core.Models.Common;
using MzadPalestine.Infrastructure.Data;
using MzadPalestine.Infrastructure.SignalR;

namespace MzadPalestine.Infrastructure.Services.Notification;

public class NotificationService : INotificationService
{
    private readonly ApplicationDbContext _context;
    private readonly IHubContext<NotificationHub> _hubContext;
    private readonly IUserConnectionManager _userConnectionManager;

    public NotificationService(
        ApplicationDbContext context ,
        IHubContext<NotificationHub> hubContext ,
        IUserConnectionManager userConnectionManager)
    {
        _context = context;
        _hubContext = hubContext;
        _userConnectionManager = userConnectionManager;
    }

    public async Task<Result<bool>> SendNotificationAsync(SendNotificationDto notificationDto)
    {
        try
        {
            var notification = new Core.Models.Notification
            {
                Title = notificationDto.Title ,
                Message = notificationDto.Message ,
                UserId = notificationDto.UserId ,
                Type = (Core.Models.NotificationType)notificationDto.Type ,
                ReferenceId = notificationDto.ReferenceId ,
                CreatedAt = DateTime.UtcNow ,
                IsRead = false ,
                Status = NotificationStatus.Unread.ToString()
            };

            await _context.Notifications.AddAsync(notification);
            await _context.SaveChangesAsync();

            // إرسال الإشعار في الوقت الفعلي
            var connections = _userConnectionManager.GetUserConnections(notificationDto.UserId);
            if (connections != null && connections.Any())
            {
                await _hubContext.Clients.Clients(connections)
                    .SendAsync("ReceiveNotification" , notification);
            }

            return Result<bool>.CreateSuccess(true);
        }
        catch (Exception ex)
        {
            return Result<bool>.Failure($"فشل في إرسال الإشعار: {ex.Message}");
        }
    }

    public async Task<Result<bool>> SendNotificationAsync(string userId , string title , string message , string? route = null)
    {
        var notificationDto = new SendNotificationDto
        {
            UserId = userId ,
            Title = title ,
            Message = message ,
            ReferenceId = route ,
            Type = NotificationType.General
        };

        return await SendNotificationAsync(notificationDto);
    }

    public async Task<PagedResult<NotificationDto>> GetUserNotificationsAsync(string userId , PaginationParams parameters)
    {
        var query = _context.Notifications
            .Where(n => n.UserId == userId)
            .OrderByDescending(n => n.CreatedAt);

        var totalCount = await query.CountAsync();
        var notifications = await query
            .Skip((parameters.PageNumber - 1) * parameters.PageSize)
            .Take(parameters.PageSize)
            .Select(n => new NotificationDto
            {
                Id = n.Id ,
                Title = n.Title ,
                Message = n.Message ,
                Type = (NotificationType)n.Type ,
                CreatedAt = n.CreatedAt ,
                IsRead = n.IsRead ,
                ReferenceId = n.ReferenceId
            })
            .ToListAsync();

        return new PagedResult<NotificationDto>
        {
            Items = notifications ,
            TotalCount = totalCount ,
            PageNumber = parameters.PageNumber ,
            PageSize = parameters.PageSize
        };
    }

    public async Task<Result<bool>> MarkNotificationAsReadAsync(string userId , int notificationId)
    {
        try
        {
            var notification = await _context.Notifications
                .FirstOrDefaultAsync(n => n.Id == notificationId && n.UserId == userId);

            if (notification == null)
            {
                return Result<bool>.Failure("الإشعار غير موجود");
            }

            notification.IsRead = true;
            notification.Status = NotificationStatus.Read.ToString();
            await _context.SaveChangesAsync();

            return Result<bool>.CreateSuccess(true);
        }
        catch (Exception ex)
        {
            return Result<bool>.Failure($"فشل في تحديث حالة الإشعار: {ex.Message}");
        }
    }

    public async Task<int> GetUnreadNotificationCountAsync(string userId)
    {
        return await _context.Notifications
            .CountAsync(n => n.UserId == userId && !n.IsRead);
    }

    public async Task MarkAllNotificationsAsReadAsync(string userId)
    {
        var notifications = await _context.Notifications
            .Where(n => n.UserId == userId && !n.IsRead)
            .ToListAsync();

        foreach (var notification in notifications)
        {
            notification.IsRead = true;
            notification.Status = NotificationStatus.Read.ToString();
        }

        await _context.SaveChangesAsync();
    }
}