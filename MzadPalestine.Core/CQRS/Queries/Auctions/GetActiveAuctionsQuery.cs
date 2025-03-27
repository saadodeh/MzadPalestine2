using MediatR;
using MzadPalestine.Core.DTOs.Listings;

namespace MzadPalestine.Core.CQRS.Queries.Auctions;

public record GetActiveAuctionsQuery(
    int? CategoryId = null,
    int? LocationId = null,
    string? SearchTerm = null,
    string? UserId = null,
    int Page = 1,
    int PageSize = 10
) : IRequest<IEnumerable<AuctionDto>>;
