using MzadPalestine.Core.Models;

namespace MzadPalestine.Core.Interfaces;

public interface IBidRepository
{
    Task<Bid> AddAsync(Bid bid);
    Task<IEnumerable<Bid>> GetUserBidsAsync(string userId);
    Task<IEnumerable<Bid>> GetAuctionBidsAsync(int auctionId);
    Task<Bid> GetByIdAsync(int id);
    Task<bool> UpdateAsync(Bid bid);
    Task<bool> DeleteAsync(int id);
}