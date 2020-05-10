using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ContentProccessService.Application.Commands.ApproveRejectContent
{
    public class ApproveRejectContentValidator:AbstractValidator<ApproveRejectContentRequest>
    {
        public ApproveRejectContentValidator()
        {
            RuleFor(x => x.IdTask).NotNull().WithMessage("Id Task is required");
        }
    }
}
