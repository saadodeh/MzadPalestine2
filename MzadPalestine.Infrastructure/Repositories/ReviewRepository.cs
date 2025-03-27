using Microsoft.EntityFrameworkCore;
using MzadPalestine.Core.Interfaces.Repositories;
using MzadPalestine.Core.Models;
using MzadPalestine.Infrastructure.Data;

namespace MzadPalestine.Infrastructure.Repositories;

public class ReviewRepository : GenericRepository<Review>, IReviewRepository
{
    private readonly ApplicationDbContext _context;

    public ReviewRepository(ApplicationDbContext context) : base(context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Review>> GetUserReviewsAsync(int userId)
    {
        return await _context.Reviews
            .Include(r => r.ReviewerId)
            .Include(r => r.Reviewer)
            .Include(r => r.Auction)
            .Where(r => r.ReviewerId == userId)
            .OrderByDescending(r => r.CreatedAt)
            .ToListAsync();
    }

    public async Task<double> GetUserAverageRatingAsync(int userId)
    {
        var reviews = await _context.Reviews
            .Include(r => r.ReviewerId)
            .Include(r => r.Reviewer)
            .Include(r => r.Auction)
            .Where(r => r.ReviewerId == userId)
            .ToListAsync();

        if (!reviews.Any())
            return 0;

        return reviews.Average(r => r.Rating);
    }

    public async Task<IEnumerable<Review>> GetAuctionReviewsAsync(int auctionId)
    {
        return await _context.Reviews
            .Include(r => r.ReviewerId)
            .Include(r => r.Reviewer)
            .Include(r => r.Auction)
            .Where(r => r.AuctionId == auctionId)
            .OrderByDescending(r => r.CreatedAt)
            .ToListAsync();
    }

    public async Task<bool> HasUserReviewedAuctionAsync(int userId, int auctionId)
    {
        return await _context.Reviews
            .AnyAsync(r => r.ReviewerId == userId && r.AuctionId == auctionId);
    }

    public async Task<Review?> GetUserAuctionReviewAsync(int userId, int auctionId)
    {
        return await _context.Reviews
            .Include(r => r.ReviewerId)
            .Include(r => r.Reviewer)
            .Include(r => r.Auction)
            .FirstOrDefaultAsync(r => r.ReviewerId == userId && r.AuctionId == auctionId);
    }

    public Task<IEnumerable<Review>> GetAuctionReviewsAsync(string auctionId)
    {
        throw new NotImplementedException();
    }

    public Task<bool> HasUserReviewedAuctionAsync(string userId, string auctionId)
    {
        throw new NotImplementedException();
    }

    public Task<Review?> GetUserAuctionReviewAsync(string userId, string auctionId)
    {
        throw new NotImplementedException();
    }

    public Task<IEnumerable<Review>> GetUserReviewsAsync(string userId)
    {
        throw new NotImplementedException();
    }

    public Task<double> GetUserAverageRatingAsync(string userId)
    {
        throw new NotImplementedException();
    }
}
