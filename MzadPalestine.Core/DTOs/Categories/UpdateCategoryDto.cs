using System.ComponentModel.DataAnnotations;

namespace MzadPalestine.Core.DTOs.Categories;

public class UpdateCategoryDto
{
    [Required]
    [StringLength(100)]
    public string Name { get; set; } = string.Empty;

    [StringLength(500)]
    public string? Description { get; set; }

    public int? ParentId { get; set; }

    public string? IconUrl { get; set; }
}
