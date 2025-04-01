using System.ComponentModel.DataAnnotations;

namespace MzadPalestine.Core.DTOs.Admin;

public class BanUserDto
{
    [Required]
    public string UserId { get; set; } = string.Empty;

    [Required]
    [StringLength(500)]
    public string Reason { get; set; } = string.Empty;

    public DateTime? BanExpiration { get; set; }
}