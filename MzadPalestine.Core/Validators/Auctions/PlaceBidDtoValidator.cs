using FluentValidation;
using MzadPalestine.Core.DTOs.Listings;

namespace MzadPalestine.Core.Validators.Auctions;

public class PlaceBidDtoValidator : AbstractValidator<PlaceBidDto>
{
    public PlaceBidDtoValidator()
    {
        RuleFor(x => x.AuctionId)
            .GreaterThan(0).WithMessage("معرف المزاد غير صالح");

        RuleFor(x => x.Amount)
            .GreaterThan(0).WithMessage("قيمة المزايدة يجب أن تكون أكبر من صفر");

        When(x => x.IsAutoBid, () =>
        {
            RuleFor(x => x.MaxAutoBidAmount)
                .NotNull().WithMessage("الحد الأقصى للمزايدة التلقائية مطلوب")
                .GreaterThan(x => x.Amount).WithMessage("الحد الأقصى للمزايدة التلقائية يجب أن يكون أكبر من قيمة المزايدة");
        });
    }
}
