using ContentProccessService.Models;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ContentProccessService.Application.Commands.CreateListTaskChannel
{
    public class CreateListTaskChannelValidator : AbstractValidator<TaskChannelModelRespone>
    {
        public CreateListTaskChannelValidator()
        {
            RuleFor(x => x.IdTask).NotEmpty().WithMessage("Task is missing");
            RuleFor(x => x.IdChannel).NotEmpty().WithMessage("Channel is missing");
        }
    }


}

