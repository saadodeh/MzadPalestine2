using System.ComponentModel.DataAnnotations;

namespace MzadPalestine.Core.DTOs.Locations;

public class LocationDetailsDto
{
    public int Id { get; set; }
    
    [Required]
    public string Name { get; set; } = null!;
    
    public int? ParentId { get; set; }
    
    public string? ParentName { get; set; }
    
    public string? Type { get; set; }
    
    public int ListingCount { get; set; }
    
    public List<LocationDto> Children { get; set; } = new();
}