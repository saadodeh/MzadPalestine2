using MzadPalestine.Core.DTOs.Bids;
using MzadPalestine.Core.Interfaces;
using MzadPalestine.Core.Interfaces.Services;
using MzadPalestine.Core.Models;

namespace MzadPalestine.Infrastructure.Services;

public class BidService : IBidService
{
    private readonly IUnitOfWork _unitOfWork;

    public BidService(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<Bid> PlaceBidAsync(string userId , int auctionId , decimal amount)
    {
        var bid = new Bid
        {
            BidderId = userId ,
            AuctionId = auctionId ,
            Amount = amount ,
            CreatedAt = DateTime.UtcNow
        };

        await _unitOfWork.Bids.AddAsync(bid);
        await _unitOfWork.SaveChangesAsync();
        return bid;
    }

    public async Task<AutoBid> SetAutoBidAsync(string userId , int auctionId , decimal maxAmount)
    {
        var autoBid = new AutoBid
        {
            BidderId = userId ,
            AuctionId = auctionId ,
            MaxAmount = maxAmount ,
            IsActive = true ,
            CreatedAt = DateTime.UtcNow
        };

        await _unitOfWork.AutoBids.AddAsync(autoBid);
        await _unitOfWork.SaveChangesAsync();
        return autoBid;
    }

    public async Task<bool> CancelAutoBidAsync(string userId , int auctionId)
    {
        var autoBid = await _unitOfWork.AutoBids.GetAutoBidAsync(userId , auctionId);
        if (autoBid == null) return false;

        autoBid.IsActive = false;
        await _unitOfWork.SaveChangesAsync();
        return true;
    }

    public async Task<IEnumerable<BidDto>> GetUserBidsAsync(string userId)
    {
        var bids = await _unitOfWork.Bids.GetUserBidsAsync(userId);
        return bids.Select(b => new BidDto
        {
            Id = b.Id ,
            AuctionId = b.AuctionId ,
            Amount = b.Amount ,
            CreatedAt = b.CreatedAt
        });
    }

    public async Task<IEnumerable<BidDto>> GetAuctionBidsAsync(int auctionId)
    {
        var bids = await _unitOfWork.Bids.GetAuctionBidsAsync(auctionId);
        return bids.Select(b => new BidDto
        {
            Id = b.Id ,
            BidderId = b.BidderId ,
            Amount = b.Amount ,
            CreatedAt = b.CreatedAt
        });
    }
}