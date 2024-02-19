using Core.DTOs;
using FluentValidation;

namespace NLayer.Service.Validations
{
    public class ProductDtoValidator : AbstractValidator<ProductCreateDto> // kimi validate edeceğiz ProductDto onu yazdık
    {
        // validation larımızın yapıldığı yerdir
        public ProductDtoValidator()
        {
            // PropertyName name dediğimizde Name ismi direk olarak buraya geliyor
            RuleFor(x => x.Name).NotNull().WithMessage("{PropertyName} is required").NotEmpty().WithMessage("{PropertyName} is required");

            // Price default olarak 0 dırbunun için NotNull NotEmpity işe yaramaz double int float null olamaz
            // bunun için dahil edeceğimiz aralığı belirtiriz InclusiveBetween
            RuleFor(x => x.Price).InclusiveBetween(1, int.MaxValue).WithMessage("{PropertyName} must be greater 0");
            RuleFor(x => x.Stock).InclusiveBetween(1, int.MaxValue).WithMessage("{PropertyName} must be greater 0");
            RuleFor(x => x.CategoryId).InclusiveBetween(1, int.MaxValue).WithMessage("{PropertyName} must be greater 0");
        }


    }
}
