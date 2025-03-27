namespace MzadPalestine.Core.DTOs.Reviews;

public class ReviewDto
{
    public int Id { get; set; }
    public int Rating { get; set; }
    public string Comment { get; set; } = string.Empty;
    public string ReviewerId { get; set; } = string.Empty;
    public string ReviewerName { get; set; } = string.Empty;
    public string ReviewedUserId { get; set; } = string.Empty;
    public int? AuctionId { get; set; }
    public string? AuctionTitle { get; set; }
    public DateTime CreatedAt { get; set; }
}
