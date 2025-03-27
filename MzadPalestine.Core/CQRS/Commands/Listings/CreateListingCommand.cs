using MediatR;
using MzadPalestine.Core.DTOs.Listings;

namespace MzadPalestine.Core.CQRS.Commands.Listings;

public record CreateListingCommand(CreateListingDto Model, string UserId) : IRequest<ListingDto>;
