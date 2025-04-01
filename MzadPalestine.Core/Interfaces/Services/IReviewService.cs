using MzadPalestine.Core.DTOs.Reviews;
using MzadPalestine.Core.Models.Common;

namespace MzadPalestine.Core.Interfaces.Services;

public interface IReviewService
{
    Task<Result<ReviewDto>> CreateReviewAsync(string userId, CreateReviewDto createReviewDto);
    Task<Result<ReviewDto>> UpdateReviewAsync(string userId, int reviewId, UpdateReviewDto updateReviewDto);
    Task<Result<bool>> DeleteReviewAsync(string userId, int reviewId);
    Task<Result<ReviewDto>> GetReviewByIdAsync(int reviewId);
    Task<Result<IEnumerable<ReviewDto>>> GetUserReviewsAsync(string userId, PaginationParams parameters);
    Task<Result<IEnumerable<ReviewDto>>> GetProductReviewsAsync(int productId, PaginationParams parameters);
}