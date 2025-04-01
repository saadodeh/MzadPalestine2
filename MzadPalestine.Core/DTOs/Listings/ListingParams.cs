using MzadPalestine.Core.Models.Common;
using MzadPalestine.Core.Models.Enums;

namespace MzadPalestine.Core.DTOs.Listings;

public class ListingParams : PaginationParams
{
    public string? SearchTerm { get; set; }
    public int? CategoryId { get; set; }
    public int? LocationId { get; set; }
    public ListingStatus? Status { get; set; }
    public string? SellerId { get; set; }
    public bool? IsAuction { get; set; }
    public decimal? MinPrice { get; set; }
    public decimal? MaxPrice { get; set; }
    public string? SortBy { get; set; }
    public bool IsDescending { get; set; } = true;
}