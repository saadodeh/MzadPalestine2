using MzadPalestine.Core.Models.Enums;

namespace MzadPalestine.Core.DTOs.Users;

public class UserDto
{
    public string Id { get; set; } = null!;
    public string FirstName { get; set; } = null!;
    public string LastName { get; set; } = null!;
    public string FullName => $"{FirstName} {LastName}";
    public string Email { get; set; } = null!;
    public string PhoneNumber { get; set; } = null!;
    public string? ProfilePictureUrl { get; set; }
    public UserRole Role { get; set; }
    public bool IsVerified { get; set; }
    public DateTime CreatedAt { get; set; }
    public bool IsActive { get; set; }
    public double Rating { get; set; }
    public int TotalListings { get; set; }
    public int ActiveListings { get; set; }
    public int CompletedAuctions { get; set; }
    public int WonAuctions { get; set; }
}
