using Microsoft.AspNetCore.Http;

public class UpdateCategoryRequest
{
    public string Name { get; set; }
    public string Description { get; set; }
    public int? ParentId { get; set; }
    public int DisplayOrder { get; set; }
    public IFormFile Image { get; set; }
}