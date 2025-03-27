using FluentValidation;
using MzadPalestine.Core.DTOs.Listings;

namespace MzadPalestine.Core.Validators.Listings;

public class CreateListingDtoValidator : AbstractValidator<CreateListingDto>
{
    public CreateListingDtoValidator()
    {
        RuleFor(x => x.Title)
            .NotEmpty().WithMessage("عنوان المنتج مطلوب")
            .Length(10, 100).WithMessage("عنوان المنتج يجب أن يكون بين 10 و 100 حرف");

        RuleFor(x => x.Description)
            .NotEmpty().WithMessage("وصف المنتج مطلوب")
            .MinimumLength(20).WithMessage("وصف المنتج يجب أن يكون 20 حرف على الأقل");

        RuleFor(x => x.StartingPrice)
            .GreaterThan(0).WithMessage("السعر الابتدائي يجب أن يكون أكبر من صفر");

        RuleFor(x => x.CategoryId)
            .GreaterThan(0).WithMessage("الفئة مطلوبة");

        RuleFor(x => x.LocationId)
            .GreaterThan(0).WithMessage("الموقع مطلوب");

        RuleFor(x => x.ContactPhone)
            .Matches(@"^((?:\+?970|00970|0)(?:2|4|8|9)[0-9]{7})$")
            .When(x => !string.IsNullOrEmpty(x.ContactPhone))
            .WithMessage("رقم الهاتف غير صالح");

        When(x => x.IsAuction, () =>
        {
            RuleFor(x => x.StartTime)
                .NotNull().WithMessage("وقت بدء المزاد مطلوب")
                .GreaterThan(DateTime.UtcNow).WithMessage("وقت بدء المزاد يجب أن يكون في المستقبل");

            RuleFor(x => x.EndTime)
                .NotNull().WithMessage("وقت انتهاء المزاد مطلوب")
                .GreaterThan(x => x.StartTime).WithMessage("وقت انتهاء المزاد يجب أن يكون بعد وقت البدء");

            RuleFor(x => x.ReservePrice)
                .GreaterThanOrEqualTo(x => x.StartingPrice)
                .When(x => x.ReservePrice.HasValue)
                .WithMessage("السعر المحجوز يجب أن يكون أكبر من أو يساوي السعر الابتدائي");

            RuleFor(x => x.MinimumBidIncrement)
                .NotNull().WithMessage("الحد الأدنى للزيادة في المزايدة مطلوب")
                .GreaterThan(0).WithMessage("الحد الأدنى للزيادة في المزايدة يجب أن يكون أكبر من صفر");
        });

        RuleFor(x => x.ImageUrls)
            .NotEmpty().WithMessage("يجب إضافة صورة واحدة على الأقل")
            .Must(x => x == null || x.Count <= 10).WithMessage("الحد الأقصى للصور هو 10 صور");
    }
}
