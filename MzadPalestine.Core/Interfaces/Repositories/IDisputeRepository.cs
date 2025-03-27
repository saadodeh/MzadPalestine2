using MzadPalestine.Core.Models;

namespace MzadPalestine.Core.Interfaces.Repositories;

public interface IDisputeRepository : IGenericRepository<Dispute>
{
    Task<IEnumerable<Dispute>> GetUserDisputesAsync(string userId);
    Task<IEnumerable<Dispute>> GetAuctionDisputesAsync(int auctionId);
    Task<bool> HasUserDisputedAuctionAsync(string userId, int auctionId);
}
