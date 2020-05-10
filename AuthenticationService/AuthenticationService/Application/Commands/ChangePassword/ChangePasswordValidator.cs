using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AuthenticationService.Application.Commands.ChangePassword
{
    public class ChangePasswordValidator : AbstractValidator<ChangePasswordCommands>
    {
        public ChangePasswordValidator()
        {
            RuleFor(x => x.Email)
           .NotEmpty()
           .WithMessage("Email is required.")
           .EmailAddress()
           .WithMessage("Invalid email format.");
            RuleFor(x => x.OldPassword).NotEmpty().WithMessage("OldPassword is required.");
            RuleFor(x => x.NewPassword).MinimumLength(8).WithMessage("Please input passowrd > 8 character");
            RuleFor(x => x.NewPassword).MaximumLength(32).WithMessage("Please input passowrd < 32 character");
            RuleFor(x => x.NewPassword).Matches("^(?=.*[a-z])(?=.*[A-Z])(?=.*\\d)(?=.*[#$^+=!*()@%&]).{8,32}$").WithMessage("Minimum 8 characters atleast 1 Alphabet, 1 Number and 1 Special Character");
        }
    }
}
