using FluentValidation;
using Web.Models.Catalogs;

namespace Web.Validators
{
    public class CourseCreateInputValidator : AbstractValidator<CourseCreateInput>
    {
        public CourseCreateInputValidator()
        {
            RuleFor(_ => _.Name).NotEmpty().WithMessage("İsim alanı boş olamaz");
            RuleFor(_ => _.Description).NotEmpty().WithMessage("Açıklama alanı boş olamaz");
            RuleFor(_ => _.Feature.Duration).InclusiveBetween(1, int.MaxValue).WithMessage("Süre alanı boş olamaz");
            RuleFor(_ => _.Price).NotEmpty().WithMessage("Fiyat alanı boş olamaz").ScalePrecision(2, 6).WithMessage("Hatalı format");
            RuleFor(_ => _.CategoryId).NotEmpty().WithMessage("Kategori alanı seçiniz");
        }
    }
}
