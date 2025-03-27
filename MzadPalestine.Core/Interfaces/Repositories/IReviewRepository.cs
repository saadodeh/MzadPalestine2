using MzadPalestine.Core.Models;

namespace MzadPalestine.Core.Interfaces.Repositories;

public interface IReviewRepository : IGenericRepository<Review>
{
    Task<IEnumerable<Review>> GetUserReviewsAsync(string userId);
    Task<double> GetUserAverageRatingAsync(string userId);
    Task<IEnumerable<Review>> GetAuctionReviewsAsync(string auctionId);
    Task<bool> HasUserReviewedAuctionAsync(string userId, string auctionId);
    Task<Review?> GetUserAuctionReviewAsync(string userId, string auctionId);
}
