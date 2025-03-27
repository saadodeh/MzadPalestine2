namespace MzadPalestine.Core.DTOs.Auth;

public class AuthResponseDto
{
    public bool IsSuccess { get; set; }
    public string? Message { get; set; }
    public string? Token { get; set; }
    public string? RefreshToken { get; set; }
    public DateTime? TokenExpiration { get; set; }
    public string? UserId { get; set; }
    public string? Email { get; set; }
    public string? UserName { get; set; }
    public List<string>? Roles { get; set; }
}
