using MzadPalestine.Core.DTOs.Listings;

namespace MzadPalestine.Core.Interfaces.Services;

public interface IAuctionService
{
    Task<AuctionDto?> GetAuctionByIdAsync(int id);
    Task<IEnumerable<AuctionDto>> GetActiveAuctionsAsync(
        int? categoryId = null,
        int? locationId = null,
        string? searchTerm = null,
        string? userId = null,
        int page = 1,
        int pageSize = 10);
    Task<BidDto> PlaceBidAsync(PlaceBidDto model, string userId);
    Task<bool> CancelBidAsync(int bidId, string userId);
    Task<bool> ExtendAuctionTimeAsync(int auctionId, TimeSpan extension);
    Task<bool> CloseAuctionAsync(int auctionId);
    Task ProcessEndedAuctionsAsync();
}
