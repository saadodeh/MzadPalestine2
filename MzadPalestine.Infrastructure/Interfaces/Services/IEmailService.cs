namespace MzadPalestine.Core.Interfaces.Services;

public interface IEmailService
{
    Task SendEmailAsync(string to, string subject, string body, bool isHtml = true);
    Task SendWelcomeEmailAsync(string to, string userName, string confirmationLink);
    Task SendPasswordResetEmailAsync(string to, string resetLink);
    Task SendAuctionWonEmailAsync(string to, string userName, int auctionId, decimal winningBid);
    Task SendBidOutbidEmailAsync(string to, string userName, int auctionId, decimal newBid);
    Task SendAuctionEndingSoonEmailAsync(string to, string userName, int auctionId, TimeSpan timeRemaining);
}
