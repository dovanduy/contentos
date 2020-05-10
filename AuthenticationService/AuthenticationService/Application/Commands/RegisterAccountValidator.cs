using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AuthenticationService.Application.Commands
{
    public class RegisterAccountValidator : AbstractValidator<RegisterAccountCommands>
    {
        public RegisterAccountValidator()
        {
            RuleFor(x => x.Email)
            .NotEmpty()
            .WithMessage("Email is required.")
            .EmailAddress()
            .WithMessage("Invalid email format.");
            RuleFor(x => x.Password).NotEmpty().WithMessage("Please chooser or add new customers");
            RuleFor(x => x.Password).MinimumLength(8).WithMessage("Please input passowrd > 8 character");
            RuleFor(x => x.Password).MaximumLength(32).WithMessage("Please input passowrd < 32 character");
            RuleFor(x => x.Password).Matches("^(?=.*[a-z])(?=.*[A-Z])(?=.*\\d)(?=.*[#$^+=!*()@%&]).{8,32}$").WithMessage("Minimum 8 characters atleast 1 Alphabet, 1 Number and 1 Special Character");
            RuleFor(x => x.FirstName).NotEmpty().WithMessage("Please enter fullname");
            RuleFor(x => x.LastName).NotEmpty().WithMessage("Please enter lastname");

        }
    }
}
