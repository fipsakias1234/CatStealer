using CatStealer.Application.DTOs;
using FluentValidation;

namespace CatStealer.Application.Cats.Validators
{
    public class CatStealerValidator : AbstractValidator<AddCatRequestDTO>, ICatStealerValidator
    {
        public CatStealerValidator()
        {
            RuleFor(x => x.NumberOfCatsToAdd).InclusiveBetween(1, 100).WithMessage("The number of cats to add must be between 1 and 100.");

        }
    }

    public interface ICatStealerValidator : IValidator<AddCatRequestDTO> { }
}
