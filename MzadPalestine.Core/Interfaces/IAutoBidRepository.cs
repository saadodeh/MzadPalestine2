using MzadPalestine.Core.Models;

namespace MzadPalestine.Core.Interfaces;

public interface IAutoBidRepository
{
    Task<AutoBid> AddAsync(AutoBid autoBid);
    Task<AutoBid> GetAutoBidAsync(string userId, int auctionId);
    Task<IEnumerable<AutoBid>> GetUserAutoBidsAsync(string userId);
    Task<IEnumerable<AutoBid>> GetAuctionAutoBidsAsync(int auctionId);
    Task<bool> UpdateAsync(AutoBid autoBid);
    Task<bool> DeleteAsync(int id);
}