using FluentValidation;
using Pustok.Database.DomainModels;
using Pustok.ViewModels.Product;

namespace Pustok.Validation
{
    public class CategoryValidator : AbstractValidator<CategoryAddResponseViewModel>
    {
        public CategoryValidator()
        {
            RuleFor(n => n.Name).NotNull()
                .When(x => x.Name == null).WithMessage("Please enter category Name")
                .Length(3, 15).WithMessage("Name can't be less than 3 character");
            
        }
    }
}
