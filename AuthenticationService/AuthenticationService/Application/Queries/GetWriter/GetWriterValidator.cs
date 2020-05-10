using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AuthenticationService.Application.Queries.GetWriter
{
    public class GetCustomerValidator : AbstractValidator<GetWriterRequest>
    {
        public GetCustomerValidator()
        {
            RuleFor(x => x.EditorId).NotEmpty().WithMessage("EditorId is Required");
        }

    }
}
