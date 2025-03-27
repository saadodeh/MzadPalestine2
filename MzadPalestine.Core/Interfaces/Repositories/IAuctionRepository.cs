using MzadPalestine.Core.Models;
using MzadPalestine.Core.Models.Common;

namespace MzadPalestine.Core.Interfaces.Repositories;

public interface IAuctionRepository : IGenericRepository<Auction>
{
    Task<IEnumerable<Auction>> GetUserAuctionsAsync(string userId);
    Task<IEnumerable<Auction>> GetListingAuctionsAsync(string listingId);
    Task<decimal> GetTotalUserAuctionsAmountAsync(string userId);
    Task<IEnumerable<Auction>> GetWonAuctionsAsync(string userId);
    Task<bool> HasActiveBidsAsync(string auctionId);
    Task<decimal> GetHighestBidAmountAsync(string auctionId);
    Task<bool> IsUserHighestBidderAsync(string userId, string auctionId);
    Task<bool> HasUserBidAsync(int auctionId, string userId);
    Task<Auction> GetAuctionWithDetailsAsync(int id);
    Task<bool> IsHighestBidderAsync(int auctionId, string userId);
    Task<ApplicationUser> GetWinnerAsync(int auctionId);
}
