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

    public async Task<IEnumerable<Review>> GetUserReviewsAsync(string userId)
    {
        return await _context.Reviews
            .Include(r => r.ReviewerId)
            .Include(r => r.Reviewer)
            .Include(r => r.Auction)
            .Where(r => r.ReviewerId == userId)
            .OrderByDescending(r => r.CreatedAt)
            .ToListAsync();
    }

    public async Task<double> GetUserAverageRatingAsync(string userId)
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

    public async Task<bool> HasUserReviewedAuctionAsync(string userId, int auctionId)
    {
        return await _context.Reviews
            .AnyAsync(r => r.ReviewerId == userId && r.AuctionId == auctionId);
    }

    public async Task<Review?> GetUserAuctionReviewAsync(string userId, int auctionId)
    {
        return await _context.Reviews
            .Include(r => r.ReviewerId)
            .Include(r => r.Reviewer)
            .Include(r => r.Auction)
            .FirstOrDefaultAsync(r => r.ReviewerId == userId && r.AuctionId == auctionId);
    }

    public Task<IEnumerable<Review>> GetAuctionReviewsAsync(string auctionId)
    {
        return GetAuctionReviewsAsync(int.Parse(auctionId));
    }

    public Task<bool> HasUserReviewedAuctionAsync(string userId, string auctionId)
    {
        return HasUserReviewedAuctionAsync(userId, int.Parse(auctionId));
    }

    public Task<Review?> GetUserAuctionReviewAsync(string userId, string auctionId)
    {
        return GetUserAuctionReviewAsync(userId, int.Parse(auctionId));
    }
}
