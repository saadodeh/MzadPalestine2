using MzadPalestine.Core.Models;

namespace MzadPalestine.Core.Interfaces.Repositories;

public interface IBidRepository : IGenericRepository<Bid>
{
    Task<IEnumerable<Bid>> GetAuctionBidsAsync(string auctionId);
    Task<Bid?> GetHighestBidAsync(string auctionId);
    Task<IEnumerable<Bid>> GetUserBidsAsync(string userId);
    Task<decimal> GetTotalBidsAmountAsync(string userId);
}
