using FluentValidation;
using Pustok.Database.DomainModels;

namespace Pustok.Validation
{
    public class CategoryValidator : AbstractValidator<Category>
    {
        public CategoryValidator()
        {
            RuleFor(n => n.Name).NotEmpty()
                .WithMessage("Please enter category Name")
                .Length(3, 15);
            
        }
    }
}
