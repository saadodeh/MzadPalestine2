using MzadPalestine.Core.DTOs.Bids;
using MzadPalestine.Core.Models;

namespace MzadPalestine.Core.Interfaces.Services;

public interface IBidService
{
    Task<Bid> PlaceBidAsync(string userId, int auctionId, decimal amount);
    Task<AutoBid> SetAutoBidAsync(string userId, int auctionId, decimal maxAmount);
    Task<bool> CancelAutoBidAsync(string userId, int auctionId);
    Task<IEnumerable<BidDto>> GetUserBidsAsync(string userId);
    Task<IEnumerable<BidDto>> GetAuctionBidsAsync(int auctionId);
}