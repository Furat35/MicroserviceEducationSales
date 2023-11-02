using FluentValidation;
using Web.Models.Discounts;

namespace Web.Validators
{
    public class DiscountApplyInputValidator : AbstractValidator<DiscountApplyInput>
    {
        public DiscountApplyInputValidator()
        {
            RuleFor(_ => _.Code).NotEmpty().WithMessage("İndirim kupon alanı boş olamaz");
        }
    }
}
