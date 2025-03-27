namespace MzadPalestine.Core.DTOs.Users;

public class UpdateUserDto
{
    public string FirstName { get; set; } = null!;
    public string LastName { get; set; } = null!;
    public string PhoneNumber { get; set; } = null!;
    public string? ProfilePictureUrl { get; set; }
    public bool ReceiveEmailNotifications { get; set; } = true;
    public bool ReceiveSmsNotifications { get; set; } = true;
    public string? Bio { get; set; }
    public string? Location { get; set; }
    public string? Website { get; set; }
    public Dictionary<string, bool> Preferences { get; set; } = new();
}
