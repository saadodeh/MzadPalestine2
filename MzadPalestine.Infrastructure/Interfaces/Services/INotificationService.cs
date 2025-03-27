namespace MzadPalestine.Core.Interfaces.Services;

public interface INotificationService
{
    Task NotifyBidPlacedAsync(int auctionId, string bidderId, decimal amount);
    Task NotifyBidOutbidAsync(int auctionId, string bidderId, decimal newAmount);
    Task NotifyAuctionEndingSoonAsync(int auctionId);
    Task NotifyAuctionEndedAsync(int auctionId, string? winnerId);
    Task NotifyPaymentRequiredAsync(int auctionId, string userId);
    Task NotifyPaymentReceivedAsync(int auctionId, string userId);
    Task NotifyMessageReceivedAsync(string senderId, string receiverId, string message);
    Task NotifyListingReportedAsync(int listingId, string reporterId, string reason);
}
