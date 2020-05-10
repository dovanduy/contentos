using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AuthenticationService.Application.Queries.GetCustomer
{
    public class GetCustomerValidator : AbstractValidator<GetCustomerBasicRequest>
    {
        public GetCustomerValidator()
        {
            RuleFor(x => x.MarketerId).NotEmpty().WithMessage("MarketerId is Required");
        }

    }
}
