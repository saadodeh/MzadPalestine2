using System.ComponentModel.DataAnnotations;

namespace MzadPalestine.Core.DTOs.Locations;

public class CreateLocationRequest
{
    [Required]
    [StringLength(100)]
    public string Name { get; set; } = string.Empty;

    [StringLength(500)]
    public string? Description { get; set; }

    public int? ParentId { get; set; }

    [Required]
    public double Latitude { get; set; }

    [Required]
    public double Longitude { get; set; }

    [StringLength(50)]
    public string? Type { get; set; }

    public bool IsActive { get; set; } = true;
}