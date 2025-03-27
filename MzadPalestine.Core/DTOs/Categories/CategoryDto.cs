namespace MzadPalestine.Core.DTOs.Categories;

public class CategoryDto
{
    public int Id { get; set; }
    public string Name { get; set; } = null!;
    public string? Description { get; set; }
    public int? ParentId { get; set; }
    public string? ImageUrl { get; set; }
    public int DisplayOrder { get; set; }
    public int ItemsCount { get; set; }
    public int ListingCount { get; set; }
    public List<CategoryDto> Children { get; set; } = new();
}

public class CategoryTreeDto : CategoryDto
{
    public int Level { get; set; }
    public string FullPath { get; set; } = null!;
    public List<CategoryTreeDto> Ancestors { get; set; } = new();
    public new List<CategoryTreeDto> Children { get; set; } = new();
}
