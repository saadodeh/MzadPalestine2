using MzadPalestine.Core.Models;
using MzadPalestine.Core.Models.Common;

namespace MzadPalestine.Core.Interfaces;

public interface IInvoiceRepository : IGenericRepository<Invoice>
{
    Task<PagedList<Invoice>> GetUserInvoicesAsync(string userId, PaginationParams parameters);
    Task<IEnumerable<Invoice>> GetUnpaidInvoicesAsync();
    Task<IEnumerable<Invoice>> GetOverdueInvoicesAsync();
}
