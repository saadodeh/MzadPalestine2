using MzadPalestine.Core.Models;
using MzadPalestine.Core.Models.Common;

namespace MzadPalestine.Core.Interfaces;

public interface IAuctionRepository : IGenericRepository<Auction>
{
    Task<Auction?> GetAuctionDetailsAsync(int auctionId);
    Task<bool> UpdateAsync(Auction auction);
    Task<PagedList<Auction>> GetUserAuctionsAsync(string userId, PaginationParams parameters);
    Task<IEnumerable<Auction>> GetActiveAuctionsAsync();
    Task<IEnumerable<Auction>> GetEndingSoonAuctionsAsync(int hours);
    Task<bool> HasActiveBidsAsync(int auctionId);
    Task<decimal> GetHighestBidAsync(int auctionId);
}