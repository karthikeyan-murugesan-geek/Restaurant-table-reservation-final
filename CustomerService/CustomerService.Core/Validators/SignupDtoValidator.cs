using System;
using CustomerService.Core.Models;
using FluentValidation;

namespace CustomerService.Core.Validators
{
    public class SignupDtoValidator : AbstractValidator<SignupDto>
    {
        public SignupDtoValidator()
        {
            Include(new LoginDtoValidator());

            RuleFor(x => x.Role)
                .NotEmpty()
                .Must(role => role == Enums.UserRole.Customer || role == Enums.UserRole.Manager)
                .WithMessage("Role must be Customer or Manager");

            RuleFor(x => x.MobileNumber)
                .Matches(@"^\+?[1-9]\d{7,14}$")
                .When(x => !string.IsNullOrEmpty(x.MobileNumber))
                .WithMessage("Invalid mobile number");
        }
    }
}

