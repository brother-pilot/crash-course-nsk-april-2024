using FluentValidation;
using Market.Models;

namespace Market.DTO
{
    public class ProductValidator : AbstractValidator<Product>
    {
        public ProductValidator()
        {
            RuleFor(s => s.Id).NotEmpty();
            RuleFor(s => s.SellerId).NotEmpty();
            RuleFor(s => s.Description).NotEmpty();
            RuleFor(s => s.Name).NotEmpty();
            RuleFor(s => s.Description.Length).LessThanOrEqualTo(1000).When(s=>s.Description!=null);
        }
    }
}