namespace MzadPalestine.Core.DTOs.Locations;

public class LocationDto
{
    public int Id { get; set; }
    public string Name { get; set; } = null!;
    public string? NameAr { get; set; }
    public string? Description { get; set; }
    public int? ParentId { get; set; }
    public double? Latitude { get; set; }
    public double? Longitude { get; set; }
    public string? Type { get; set; }
    public int DisplayOrder { get; set; }
    public int ItemsCount { get; set; }
    public List<LocationDto> Children { get; set; } = new();
}

public class LocationTreeDto : LocationDto
{
    public int Level { get; set; }
    public string FullPath { get; set; } = null!;
    public List<LocationTreeDto> Ancestors { get; set; } = new();
    public new List<LocationTreeDto> Children { get; set; } = new();
}
