using MzadPalestine.Core.DTOs.Auctions;
using MzadPalestine.Core.DTOs.Bids;
using MzadPalestine.Core.Interfaces;
using MzadPalestine.Core.Interfaces.Services;
using MzadPalestine.Core.Models;
using MzadPalestine.Core.Models.Common;

namespace MzadPalestine.Infrastructure.Services;

public class AuctionService : IAuctionService
{
    private readonly IUnitOfWork _unitOfWork;

    public AuctionService(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<Auction> CreateAuctionAsync(Auction auction)
    {
        await _unitOfWork.Auctions.AddAsync(auction);
        await _unitOfWork.SaveChangesAsync();
        return auction;
    }

    public async Task<Auction> GetAuctionByIdAsync(int id)
    {
        return await _unitOfWork.Auctions.GetByIdAsync(id);
    }

    public async Task<IEnumerable<Auction>> GetActiveAuctionsAsync( )
    {
        return await _unitOfWork.Auctions.GetActiveAuctionsAsync();
    }

    public async Task<bool> UpdateAuctionAsync(Auction auction)
    {
        await _unitOfWork.Auctions.UpdateAsync(auction);
        await _unitOfWork.SaveChangesAsync();
        return true;
    }

    public async Task<bool> DeleteAuctionAsync(int id)
    {
        var auction = await GetAuctionByIdAsync(id);
        if (auction == null) return false;

        await _unitOfWork.Auctions.DeleteAsync(auction);
        await _unitOfWork.SaveChangesAsync();
        return true;
    }

    public Task<Result<AuctionDto>> CreateAuctionAsync(string userId , CreateAuctionDto createAuctionDto)
    {
        throw new NotImplementedException();
    }

    public Task<Result<AuctionDto>> UpdateAuctionAsync(string userId , int auctionId , UpdateAuctionDto updateAuctionDto)
    {
        throw new NotImplementedException();
    }

    public Task<Result<bool>> DeleteAuctionAsync(string userId , int auctionId)
    {
        throw new NotImplementedException();
    }

    Task<Result<AuctionDto>> IAuctionService.GetAuctionByIdAsync(int id)
    {
        throw new NotImplementedException();
    }

    public Task<PagedList<AuctionDto>> GetAllAuctionsAsync(PaginationParams parameters)
    {
        throw new NotImplementedException();
    }

    public Task<PagedList<AuctionDto>> SearchAuctionsAsync(string searchTerm , PaginationParams parameters)
    {
        throw new NotImplementedException();
    }

    public Task<IEnumerable<AuctionDto>> GetActiveAuctionsAsync(int? categoryId = null , int? locationId = null , string? searchTerm = null , string? userId = null , int page = 1 , int pageSize = 10)
    {
        throw new NotImplementedException();
    }

    public Task<Result<BidDto>> PlaceBidAsync(string userId , PlaceBidDto model)
    {
        throw new NotImplementedException();
    }

    public Task<Result<bool>> CancelBidAsync(string userId , int bidId)
    {
        throw new NotImplementedException();
    }

    public Task<bool> ExtendAuctionTimeAsync(int auctionId , TimeSpan extension)
    {
        throw new NotImplementedException();
    }

    public Task<bool> CloseAuctionAsync(int auctionId)
    {
        throw new NotImplementedException();
    }

    public Task ProcessEndedAuctionsAsync( )
    {
        throw new NotImplementedException();
    }

    public Task<PagedList<BidDto>> GetAuctionBidsAsync(int auctionId , PaginationParams parameters)
    {
        throw new NotImplementedException();
    }

    public Task<PagedList<BidDto>> GetUserBidsAsync(string userId , PaginationParams parameters)
    {
        throw new NotImplementedException();
    }

    public Task<Result<BidDto>> GetWinningBidAsync(int auctionId)
    {
        throw new NotImplementedException();
    }
}