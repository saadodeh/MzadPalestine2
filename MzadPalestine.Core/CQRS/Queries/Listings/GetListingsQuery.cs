using MediatR;
using MzadPalestine.Core.DTOs.Listings;

namespace MzadPalestine.Core.CQRS.Queries.Listings;

public record GetListingsQuery(
    int? CategoryId = null,
    int? LocationId = null,
    string? SearchTerm = null,
    bool? IsAuction = null,
    string? UserId = null,
    int Page = 1,
    int PageSize = 10
) : IRequest<IEnumerable<ListingDto>>;
