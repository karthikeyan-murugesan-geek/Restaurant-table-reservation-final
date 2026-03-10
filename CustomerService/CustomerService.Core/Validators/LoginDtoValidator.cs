using System;
namespace CustomerService.Core.Validators
{
    using CustomerService.Core.Models;
    using FluentValidation;

    public class LoginDtoValidator : AbstractValidator<LoginDto>
    {
        public LoginDtoValidator()
        {
            RuleFor(x => x.UserName)
                .NotEmpty().WithMessage("Username is required")
                .MinimumLength(3)
                .MaximumLength(50);

            RuleFor(x => x.Password)
                .NotEmpty().WithMessage("Password is required")
                .MinimumLength(8)
                .MaximumLength(100);
        }
    }
}

