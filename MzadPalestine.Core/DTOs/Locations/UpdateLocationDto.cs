using System.ComponentModel.DataAnnotations;

namespace MzadPalestine.Core.DTOs.Locations;

public class UpdateLocationDto
{
    [Required]
    [StringLength(100)]
    public string Name { get; set; } = string.Empty;

    [StringLength(500)]
    public string? Description { get; set; }

    public int? ParentId { get; set; }

    public double? Latitude { get; set; }

    public double? Longitude { get; set; }
}
