using MzadPalestine.Core.Models;
using MzadPalestine.Core.Models.Common;

namespace MzadPalestine.Core.Interfaces;

public interface IAuctionRepository : IGenericRepository<Auction>
{
    Task<Auction?> GetAuctionWithDetailsAsync(int auctionId);
    Task<PagedList<Auction>> GetActiveAuctionsAsync(PaginationParams parameters);
    Task<PagedList<Auction>> GetUserAuctionsAsync(string userId, PaginationParams parameters);
    Task<PagedList<Auction>> SearchAuctionsAsync(string searchTerm, PaginationParams parameters);
    Task<IEnumerable<Auction>> GetEndingSoonAuctionsAsync(int hours);
    Task<IEnumerable<Auction>> GetFeaturedAuctionsAsync();
    Task<IEnumerable<Auction>> GetRelatedAuctionsAsync(int auctionId, int count);
    Task<bool> HasActiveBidsAsync(int auctionId);
    Task<decimal> GetCurrentBidAsync(int auctionId);
    Task<int> GetBidCountAsync(int auctionId);
    Task UpdateCurrentBidAsync(int auctionId, decimal amount);
    Task<bool> IsUserHighestBidderAsync(int auctionId, string userId);
}
