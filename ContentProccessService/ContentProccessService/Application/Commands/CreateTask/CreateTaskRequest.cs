using ContentProccessService.Entities;
using ContentProccessService.Models;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ContentProccessService.Application.Commands.CreateTask
{
    public class CreateTaskRequest :IRequest<TasksViewModel>
    {
        public CreateTaskModel Task { get; set; }
    }
}
