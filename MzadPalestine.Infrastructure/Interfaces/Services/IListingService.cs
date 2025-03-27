using MzadPalestine.Core.DTOs.Listings;

namespace MzadPalestine.Core.Interfaces.Services;

public interface IListingService
{
    Task<ListingDto> CreateListingAsync(CreateListingDto model, string userId);
    Task<ListingDto?> GetListingByIdAsync(int id);
    Task<IEnumerable<ListingDto>> GetListingsAsync(
        int? categoryId = null,
        int? locationId = null,
        string? searchTerm = null,
        bool? isAuction = null,
        string? userId = null,
        int page = 1,
        int pageSize = 10);
    Task<bool> UpdateListingAsync(int id, CreateListingDto model, string userId);
    Task<bool> DeleteListingAsync(int id, string userId);
    Task<bool> ReportListingAsync(int id, string reason, string userId);
}
