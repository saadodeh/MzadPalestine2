using MzadPalestine.Core.Models;
using MzadPalestine.Core.Models.Common;

namespace MzadPalestine.Core.Interfaces;

public interface IBidRepository : IGenericRepository<Bid>
{
    Task<PagedList<Bid>> GetAuctionBidsAsync(int auctionId, PaginationParams parameters);
    Task<PagedList<Bid>> GetUserBidsAsync(string userId, PaginationParams parameters);
    Task<Bid?> GetHighestBidAsync(int auctionId);
    Task<Bid?> GetLastBidAsync(int auctionId);
    Task<bool> HasUserBidOnAuctionAsync(string userId, int auctionId);
    Task<IEnumerable<Bid>> GetAutoBidsForAuctionAsync(int auctionId);
    Task<decimal> GetMaxAutoBidAmountAsync(string userId, int auctionId);
    Task<bool> IsValidBidAmountAsync(int auctionId, decimal amount);
}
