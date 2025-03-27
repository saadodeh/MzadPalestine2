using MzadPalestine.Core.Models;
using MzadPalestine.Core.Models.Common;
using MzadPalestine.Core.Models.Enums;

namespace MzadPalestine.Core.Interfaces;

public interface IInvoiceRepository : IGenericRepository<Invoice>
{
    Task<Invoice?> GetInvoiceWithDetailsAsync(int invoiceId);
    Task<PagedList<Invoice>> GetUserInvoicesAsync(string userId, PaginationParams parameters);
    Task<bool> UpdateInvoiceStatusAsync(int invoiceId, InvoiceStatus status);
    Task<bool> IsInvoiceValidAsync(int invoiceId, string userId);
    Task<decimal> GetTotalInvoicesAmountAsync(string userId);
    Task<Dictionary<InvoiceStatus, int>> GetInvoiceStatisticsAsync();
}
