using MzadPalestine.Core.DTOs.Payments;
using MzadPalestine.Core.DTOs.Transactions;
using MzadPalestine.Core.Models.Common;

namespace MzadPalestine.Core.Interfaces.Services;

public interface IPaymentService
{
    Task<Result<PaymentResultDto>> ProcessPaymentAsync(string userId, TransactionPaymentDto paymentDto);
    Task<Result<RefundResultDto>> RefundPaymentAsync(string userId, string transactionId);
    Task<Result<TransactionDto>> GetTransactionByIdAsync(string transactionId);
    Task<PagedList<TransactionDto>> GetUserTransactionsAsync(string userId, PaginationParams parameters);
}
