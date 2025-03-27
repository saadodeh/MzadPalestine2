using MediatR;
using MzadPalestine.Core.DTOs.Auth;

namespace MzadPalestine.Core.CQRS.Commands.Auth;

public record LoginCommand(LoginDto Model) : IRequest<AuthResponseDto>;
