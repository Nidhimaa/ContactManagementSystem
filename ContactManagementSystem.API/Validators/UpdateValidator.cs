using ContactManagementSystem.DTOs;
using FluentValidation;

namespace ContactManagementSystem.API.Validators
{
    public class UpdateValidator : AbstractValidator<UpdateContactDTO>
    {
        public UpdateValidator()
        {
            // NAME
            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("Name is required.")
                .MinimumLength(4).WithMessage("Name must be at least 4 characters.")
                .MaximumLength(25).WithMessage("Name must not exceed 25 characters.")
                .Matches(@"^[a-zA-Z\s]+$")
                .WithMessage("Name must contain only alphabets.");

            // MOBILE
            RuleFor(x => x.MobileNo)
                .NotEmpty().WithMessage("Mobile number is required.")
                .Matches(@"^[6-9]\d{9}$")
                .WithMessage("Mobile number must be a valid number.")
                .Must(m => m.Distinct().Count() > 1)
                .WithMessage("Mobile number cannot contain all identical digits.");

            // ADDRESS → Alphabets only
            RuleFor(x => x.Address)
                .NotEmpty().WithMessage("Address is required.")
                .MinimumLength(4).WithMessage("Address must be at least 4 characters.")
                .MaximumLength(100).WithMessage("Address must not exceed 100 characters.")
                .Matches(@"^[a-zA-Z0-9\s]+$")
                .WithMessage("Address must contain only alphabets.");
        }
    }
}