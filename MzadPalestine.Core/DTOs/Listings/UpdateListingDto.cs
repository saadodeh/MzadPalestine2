using System.ComponentModel.DataAnnotations;

namespace MzadPalestine.Core.DTOs.Listings;

public class UpdateListingDto
{
    [Required(ErrorMessage = "عنوان المنتج مطلوب")]
    [StringLength(255, MinimumLength = 3, 
        ErrorMessage = "يجب أن يكون العنوان بين 3 و 255 حرف")]
    public string Title { get; set; } = string.Empty;

    [Required(ErrorMessage = "وصف المنتج مطلوب")]
    [StringLength(2000, MinimumLength = 10, 
        ErrorMessage = "يجب أن يكون الوصف بين 10 و 2000 حرف")]
    public string Description { get; set; } = string.Empty;

    [Required(ErrorMessage = "السعر مطلوب")]
    [Range(0.01, 1000000, ErrorMessage = "يجب أن يكون السعر بين 0.01 و 1,000,000")]
    public decimal Price { get; set; }

    [Required(ErrorMessage = "الفئة مطلوبة")]
    [Range(1, int.MaxValue, ErrorMessage = "الرجاء اختيار فئة صحيحة")]
    public int CategoryId { get; set; }

    [Required(ErrorMessage = "الموقع مطلوب")]
    [Range(1, int.MaxValue, ErrorMessage = "الرجاء اختيار موقع صحيح")]
    public int LocationId { get; set; }

    [Url(ErrorMessage = "الرجاء إدخال رابط صورة صحيح")]
    public string? ImageUrl { get; set; }

    [MaxLength(10, ErrorMessage = "لا يمكن إضافة أكثر من 10 وسوم")]
    public List<string> Tags { get; set; } = new();
}
