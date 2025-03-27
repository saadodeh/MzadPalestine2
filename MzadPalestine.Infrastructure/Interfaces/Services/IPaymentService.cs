namespace MzadPalestine.Core.Interfaces.Services;

public interface IPaymentService
{
    Task<string> InitiatePaymentAsync(int auctionId, string userId);
    Task<bool> ProcessPaymentAsync(string paymentId, string gatewayResponse);
    Task<bool> RefundPaymentAsync(string paymentId);
    Task<bool> VerifyPaymentAsync(string paymentId);
    Task ProcessPendingPaymentsAsync();
}
