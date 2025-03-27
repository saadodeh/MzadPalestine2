using MzadPalestine.Core.Models.Common;

namespace MzadPalestine.Core.DTOs.Reviews;

public class ReviewParams : PaginationParams
{
    public DateTime? StartDate { get; set; }
    public DateTime? EndDate { get; set; }
    public int? MinRating { get; set; }
    public int? MaxRating { get; set; }
}
