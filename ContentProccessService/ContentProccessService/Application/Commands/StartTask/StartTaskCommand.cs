using ContentProccessService.Models;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ContentProccessService.Application.Commands.StartTask
{
    public class StartTaskCommand : IRequest<TasksViewModel>
    {
        public int IdTask { get; set; }
    }
}
