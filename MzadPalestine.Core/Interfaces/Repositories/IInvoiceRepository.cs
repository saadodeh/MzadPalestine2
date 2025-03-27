using MzadPalestine.Core.Models;

namespace MzadPalestine.Core.Interfaces.Repositories;

public interface IInvoiceRepository : IGenericRepository<Invoice>
{
    Task<IEnumerable<Invoice>> GetUserInvoicesAsync(string userId);
    Task<IEnumerable<Invoice>> GetAuctionInvoicesAsync(string auctionId);
    Task<decimal> GetTotalUserInvoicesAmountAsync(string userId);
}
