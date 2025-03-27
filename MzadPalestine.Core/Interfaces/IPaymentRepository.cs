using MzadPalestine.Core.Models;
using MzadPalestine.Core.Models.Common;
using MzadPalestine.Core.Models.Enums;

namespace MzadPalestine.Core.Interfaces;

public interface IPaymentRepository : IGenericRepository<Payment>
{
    Task<Payment?> GetPaymentWithDetailsAsync(int paymentId);
    Task<PagedList<Payment>> GetUserPaymentsAsync(string userId, PaginationParams parameters);
    Task<bool> UpdatePaymentStatusAsync(int paymentId, PaymentStatus status);
    Task<bool> IsPaymentValidAsync(int paymentId, string userId);
    Task<decimal> GetTotalPaymentsAsync(string userId);
    Task<Dictionary<PaymentStatus, int>> GetPaymentStatisticsAsync();
}
