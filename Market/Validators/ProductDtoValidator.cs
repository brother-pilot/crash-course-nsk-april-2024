using FluentValidation;
using Market.DTO;
using Market.Models;

namespace Market.Validators;

public class ProductDtoValidator : AbstractValidator<ProductDto>
{
    public ProductDtoValidator()
    {
        RuleFor(s => s.Id).NotEmpty();
        RuleFor(s => s.SellerId).NotEmpty();
        RuleFor(s => s.Description).NotEmpty();
        RuleFor(s => s.Name).NotEmpty();

        
        RuleFor(s => s.Name).Length(3, 100);
        RuleFor(s => s.Description).Length(1, 1000)
            .When(s => s.Description != null);
        RuleFor(s => s.PriceInRubles).GreaterThan(0);
    }
}