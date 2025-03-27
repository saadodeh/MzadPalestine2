using Microsoft.Extensions.Logging;
using MzadPalestine.Core.Interfaces.Repositories;
using MzadPalestine.Core.Interfaces.Services;
using MzadPalestine.Core.Models;
//using NotificationType = MzadPalestine.Core.Models.NotificationType;

namespace MzadPalestine.Infrastructure.Services.Notification;

public class NotificationService : INotificationService
{
    private readonly INotificationRepository _notificationRepository;
    private readonly ILogger<NotificationService> _logger;
    private readonly IUserConnectionManager _userConnectionManager;

    public NotificationService(
        INotificationRepository notificationRepository,
        ILogger<NotificationService> logger,
        IUserConnectionManager userConnectionManager)
    {
        _notificationRepository = notificationRepository;
        _logger = logger;
        _userConnectionManager = userConnectionManager;
    }

    public async Task NotifyAuctionEndedAsync(int auctionId, string? winnerId)
    {
        var notification = new MzadPalestine.Core.Models.Notification
        {
            Title = "المزاد انتهى",
            Message = winnerId != null 
                ? "تم إنهاء المزاد بنجاح" 
                : "انتهى المزاد بدون عروض",
            UserId = winnerId ?? string.Empty,
            Type = (MzadPalestine.Core.Models.NotificationType)NotificationType.AuctionEnded,
            ReferenceId = auctionId.ToString()
        };

        await _notificationRepository.CreateAsync(notification);
        if (winnerId != null)
        {
            await _userConnectionManager.SendToUserAsync(winnerId, notification, notification.Type.ToString());
        }
    }

    public async Task NotifyPaymentRequiredAsync(int auctionId, string userId)
    {
        var notification = new MzadPalestine.Core.Models.Notification
        {
            Title = "مطلوب الدفع",
            Message = "يرجى إكمال عملية الدفع للمزاد الذي فزت به",
            UserId = userId,
            Type = (MzadPalestine.Core.Models.NotificationType)NotificationType.PaymentRequired,
            ReferenceId = auctionId.ToString()
        };

        await _notificationRepository.CreateAsync(notification);
        await _userConnectionManager.SendToUserAsync(userId, notification, notification.Type.ToString());
    }

    public async Task NotifyPaymentReceivedAsync(int auctionId, string userId)
    {
        var notification = new MzadPalestine.Core.Models.Notification
        {
            Title = "تم استلام الدفع",
            Message = "تم استلام الدفع بنجاح للمزاد",
            UserId = userId,
            Type = (MzadPalestine.Core.Models.NotificationType)NotificationType.PaymentReceived,
            ReferenceId = auctionId.ToString()
        };

        await _notificationRepository.CreateAsync(notification);
        await _userConnectionManager.SendToUserAsync(userId, notification, notification.Type.ToString());
    }

    public async Task NotifyMessageReceivedAsync(string userId, string senderId, string message)
    {
        var notification = new MzadPalestine.Core.Models.Notification
        {
            Title = "رسالة جديدة",
            Message = message,
            UserId = userId,
            Type = (MzadPalestine.Core.Models.NotificationType)NotificationType.Message,
            ReferenceId = senderId
        };

        await _notificationRepository.CreateAsync(notification);
        await _userConnectionManager.SendToUserAsync(userId, notification, notification.Type.ToString());
    }

    public async Task NotifyListingReportedAsync(int listingId, string userId, string reason)
    {
        var notification = new MzadPalestine.Core.Models.Notification
        {
            Title = "تم الإبلاغ عن القائمة",
            Message = $"تم الإبلاغ عن القائمة الخاصة بك. السبب: {reason}",
            UserId = userId,
            Type = (MzadPalestine.Core.Models.NotificationType)NotificationType.ListingReported,
            ReferenceId = listingId.ToString()
        };

        await _notificationRepository.CreateAsync(notification);
        await _userConnectionManager.SendToUserAsync(userId, notification, notification.Type.ToString());
    }

    public async Task NotifyBidPlacedAsync(int auctionId, string userId, decimal amount)
    {
        var notification = new MzadPalestine.Core.Models.Notification
        {
            Title = "تم وضع مزايدة",
            Message = $"تم وضع مزايدة جديدة بقيمة {amount:C}",
            UserId = userId,
            Type = (MzadPalestine.Core.Models.NotificationType)NotificationType.BidPlaced,
            ReferenceId = auctionId.ToString()
        };

        await _notificationRepository.CreateAsync(notification);
        await _userConnectionManager.SendToUserAsync(userId, notification, notification.Type.ToString());
    }

    public async Task NotifyBidOutbidAsync(int auctionId, string userId, decimal newAmount)
    {
        var notification = new MzadPalestine.Core.Models.Notification
        {
            Title = "تم تجاوز مزايدتك",
            Message = $"تم تجاوز مزايدتك. المزايدة الجديدة هي {newAmount:C}",
            UserId = userId,
            Type = (MzadPalestine.Core.Models.NotificationType)NotificationType.BidOutbid,
            ReferenceId = auctionId.ToString()
        };

        await _notificationRepository.CreateAsync(notification);
        await _userConnectionManager.SendToUserAsync(userId, notification, notification.Type.ToString());
    }

    public async Task NotifyAuctionEndingSoonAsync(int auctionId)
    {
        var notification = new MzadPalestine.Core.Models.Notification
        {
            Title = "المزاد ينتهي قريباً",
            Message = "المزاد سينتهي خلال ساعة واحدة",
            Type = (MzadPalestine.Core.Models.NotificationType)NotificationType.AuctionEndingSoon,
            ReferenceId = auctionId.ToString()
        };

        // TODO: Get all bidders and notify them
        // For now, we'll just create the notification without sending it
        await _notificationRepository.CreateAsync(notification);
    }
}
