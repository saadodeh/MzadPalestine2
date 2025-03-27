namespace MzadPalestine.Core.DTOs.Reviews;

public class UserReviewStatsDto
{
    public double AverageRating { get; set; }
    public Dictionary<int, int> RatingDistribution { get; set; } = new();
}
