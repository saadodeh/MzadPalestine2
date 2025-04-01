using System.Security.Claims;
using MzadPalestine.Core.Models;

namespace MzadPalestine.Core.Interfaces.Services;

public interface ITokenService
{
    Task<(string AccessToken, string RefreshToken)> GenerateTokensAsync(ApplicationUser user);
    ClaimsPrincipal? ValidateToken(string token);
}