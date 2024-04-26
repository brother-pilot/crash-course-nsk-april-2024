using FluentValidation;
using Market.DTO;

namespace Market.Validators;

public class SearchProductRequestDtoValidator : AbstractValidator<SearchProductRequestDto>
{
    public SearchProductRequestDtoValidator()
    {
        RuleFor(s => s.ProductName).Length(1, 100)
            .When(s => s.ProductName != null);
        RuleFor(s => s.Skip).GreaterThanOrEqualTo(0);
        RuleFor(s => s.Take).InclusiveBetween(1, 100);
    }
}