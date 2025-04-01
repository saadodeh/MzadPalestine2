using System.Security.Claims;

namespace MzadPalestine.Core.Interfaces.Services;

public interface ICurrentUserService
{
    string? UserId { get; }
    string? UserName { get; }
    string? Email { get; }
    bool IsAuthenticated { get; }
    ClaimsPrincipal? User { get; }
    bool IsAdmin { get; }
    bool IsInRole(string role);
    IEnumerable<string> GetUserRoles();
    IEnumerable<Claim> GetUserClaims();
    string? IpAddress { get; }
    string? UserAgent { get; }
    string GetUserId();
}