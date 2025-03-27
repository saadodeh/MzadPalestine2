using MzadPalestine.Core.Models;
using MzadPalestine.Core.Models.Common;

namespace MzadPalestine.Core.Interfaces;

public interface IDisputeRepository : IGenericRepository<Dispute>
{
    Task<PagedList<Dispute>> GetUserDisputesAsync(string userId, PaginationParams parameters);
    Task<PagedList<Dispute>> GetOpenDisputesAsync(PaginationParams parameters);
    Task<IEnumerable<Dispute>> GetDisputesByAuctionAsync(int auctionId);
}
