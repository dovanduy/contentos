using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ContentProccessService.Application.Commands.CreateTasks
{
    public class CreateTasksValidator : AbstractValidator<CreateTasksRequest>
    {
        public CreateTasksValidator()
        {
        
        }
    }
}
