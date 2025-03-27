using MzadPalestine.Core.Models.Enums;

namespace MzadPalestine.Core.Interfaces.Services;

public interface ICurrentUserService
{
    string? UserId { get; }
    string? UserName { get; }
    string? Email { get; }
    bool IsAuthenticated { get; }
    bool IsAdmin { get; }
    UserRole? Role { get; }
    IEnumerable<string> Roles { get; }
    string? IpAddress { get; }
    string? UserAgent { get; }
}
