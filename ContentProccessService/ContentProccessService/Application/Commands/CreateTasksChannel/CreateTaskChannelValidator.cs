
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ContentProccessService.Application.Commands.CreateTasksChannel
{
    public class CreateTaskChannelValidator : AbstractValidator<CreateTaskChannelRequest>
    {
        public CreateTaskChannelValidator()
        {
            RuleFor(x => x.IdTask).NotEmpty().WithMessage("Task is missing");
            RuleFor(x => x.IdChannel).NotEmpty().WithMessage("Channel is missing");
        }
    }
}
