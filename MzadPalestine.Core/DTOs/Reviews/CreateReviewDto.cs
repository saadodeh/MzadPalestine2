using System.ComponentModel.DataAnnotations;

namespace MzadPalestine.Core.DTOs.Reviews;

public class CreateReviewDto
{
    [Required]
    public int ListingId { get; set; }

    [Required]
    [Range(1, 5)]
    public int Rating { get; set; }

    [Required]
    [StringLength(500)]
    public string Comment { get; set; } = string.Empty;
}