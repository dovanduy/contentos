
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ContentProccessService.Application.Commands.UpdateTaskChannel
{
    public class UpdateTaskChannelValidator : AbstractValidator<UpdateTaskChannelRequest>
    {
        public UpdateTaskChannelValidator()
        {
            RuleFor(x => x.IdTaskChannel).NotEmpty().WithMessage("Task is missing");
        }
    }
}
