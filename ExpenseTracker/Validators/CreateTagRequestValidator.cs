using ExpenseTracker.Contracts.V1.Requests;
using FluentValidation;

namespace ExpenseTracker.Validators
{
    public class CreateTagRequestValidator : AbstractValidator<CreateTagRequest>
    {
        public CreateTagRequestValidator()
        {
            RuleFor(x => x.Name).NotEmpty().Matches("^[a-zA-Z0-9 ]*$");
        }
    }
}