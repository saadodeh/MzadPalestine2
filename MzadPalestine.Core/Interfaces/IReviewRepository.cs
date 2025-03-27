using MzadPalestine.Core.Models;
using MzadPalestine.Core.Models.Common;

namespace MzadPalestine.Core.Interfaces;

public interface IReviewRepository : IGenericRepository<Review>
{
    Task<PagedList<Review>> GetUserReviewsAsync(string userId, PaginationParams parameters);
    Task<PagedList<Review>> GetListingReviewsAsync(int listingId, PaginationParams parameters);
    Task<PagedList<Review>> GetSellerReviewsAsync(string sellerId, PaginationParams parameters);
    Task<double> GetAverageRatingAsync(string userId);
    Task<bool> HasUserReviewedListingAsync(string userId, int listingId);
    Task<bool> HasUserReviewedSellerAsync(string userId, string sellerId);
    Task<Review?> GetReviewWithDetailsAsync(int reviewId);
    Task<int> GetUserReviewsCountAsync(string userId);
    Task<Dictionary<int, int>> GetRatingDistributionAsync(string userId);
}
