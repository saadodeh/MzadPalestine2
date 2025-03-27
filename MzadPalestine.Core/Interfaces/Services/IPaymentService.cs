using MzadPalestine.Core.Models;

namespace MzadPalestine.Core.Interfaces.Services;

public interface IPaymentService
{
    Task<string> InitiatePaymentAsync(int amount, string currency);
    Task<bool> ProcessPaymentAsync(string paymentMethodId, string currency);
    Task<bool> RefundPaymentAsync(string transactionId);
    Task<bool> VerifyPaymentAsync(string transactionId);
    Task ProcessPendingPaymentsAsync();
}
