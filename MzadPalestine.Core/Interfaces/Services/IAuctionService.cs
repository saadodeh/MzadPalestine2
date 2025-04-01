using MzadPalestine.Core.DTOs.Auctions;
using MzadPalestine.Core.DTOs.Bids;
using MzadPalestine.Core.Models.Common;

namespace MzadPalestine.Core.Interfaces.Services;

public interface IAuctionService
{
    Task<Result<AuctionDto>> CreateAuctionAsync(string userId, CreateAuctionDto createAuctionDto);
    Task<Result<AuctionDto>> UpdateAuctionAsync(string userId, int auctionId, UpdateAuctionDto updateAuctionDto);
    Task<Result<bool>> DeleteAuctionAsync(string userId, int auctionId);
    Task<Result<AuctionDto>> GetAuctionByIdAsync(int id);
    Task<PagedList<AuctionDto>> GetAllAuctionsAsync(PaginationParams parameters);
    Task<PagedList<AuctionDto>> SearchAuctionsAsync(string searchTerm, PaginationParams parameters);
    Task<IEnumerable<AuctionDto>> GetActiveAuctionsAsync(
        int? categoryId = null,
        int? locationId = null,
        string? searchTerm = null,
        string? userId = null,
        int page = 1,
        int pageSize = 10);
    Task<Result<BidDto>> PlaceBidAsync(string userId, PlaceBidDto model);
    Task<Result<bool>> CancelBidAsync(string userId, int bidId);
    Task<bool> ExtendAuctionTimeAsync(int auctionId, TimeSpan extension);
    Task<bool> CloseAuctionAsync(int auctionId);
    Task ProcessEndedAuctionsAsync();
    Task<PagedList<BidDto>> GetAuctionBidsAsync(int auctionId, PaginationParams parameters);
    Task<PagedList<BidDto>> GetUserBidsAsync(string userId, PaginationParams parameters);
    Task<Result<BidDto>> GetWinningBidAsync(int auctionId);
}
