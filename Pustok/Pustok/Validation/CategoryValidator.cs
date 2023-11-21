using FluentValidation;
using Pustok.Database.DomainModels;
using Pustok.ViewModels.Product;

namespace Pustok.Validation
{
    public class CategoryValidator : AbstractValidator<CategoryAddResponseViewModel>
    {
        public CategoryValidator()
        {
            RuleFor(n => n.Name).NotEmpty()
                .WithMessage("Please enter category Name")
                .Length(3, 15);
            
        }
    }
}
