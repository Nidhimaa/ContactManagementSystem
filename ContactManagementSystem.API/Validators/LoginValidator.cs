using ContactManagementSystem.DTOs;
using FluentValidation;

namespace ContactManagementSystem.API.Validators
{
    public class LoginValidator : AbstractValidator<LoginDTO>
    {
        public LoginValidator()
        {
            // EMAIL
            RuleFor(x => x.Email)
                .NotEmpty().WithMessage("Email is required.")
                .MinimumLength(4).WithMessage("Email must be at least 4 characters.")
                .MaximumLength(50).WithMessage("Email must not exceed 50 characters.")
                .Matches(@"^[a-zA-Z0-9]+([._]?[a-zA-Z0-9]+)*@gmail\.com$")
                .WithMessage("Email must be valid Gmail and contain only letters, numbers, @ and .");
        }
    }
}