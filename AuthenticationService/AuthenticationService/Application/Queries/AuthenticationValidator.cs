using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AuthenticationService.Application.Queries
{
    public class AuthenticationValidator : AbstractValidator<AuthenticationRequest>
    {
        public AuthenticationValidator()
        {
            RuleFor(x => x.Email).NotEmpty().WithMessage("Email is Required");
            RuleFor(x => x.Email).EmailAddress().WithMessage("Email is use format example@gmail.com");
            RuleFor(x => x.Password).NotEmpty().WithMessage("Password is Required");
        }

    }
}
