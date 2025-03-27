using MediatR;
using MzadPalestine.Core.DTOs.Listings;

namespace MzadPalestine.Core.CQRS.Queries.Listings;

public record GetListingByIdQuery(int Id) : IRequest<ListingDto?>;
