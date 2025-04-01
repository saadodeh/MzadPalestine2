using MediatR;
using MzadPalestine.Core.DTOs.Bids;

namespace MzadPalestine.Core.CQRS.Commands.Auctions;

public record PlaceBidCommand(PlaceBidDto Model, string UserId) : IRequest<BidDto>;
