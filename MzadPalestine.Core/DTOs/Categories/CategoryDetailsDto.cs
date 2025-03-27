public class CategoryDetailsDto
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public string ImageUrl { get; set; }
    public int? ParentId { get; set; }
    public string ParentName { get; set; }
    public int DisplayOrder { get; set; }
    public int ListingCount { get; set; }
    public List<CategoryDto> Children { get; set; }
}