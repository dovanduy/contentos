using ContentProccessService.Models;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ContentProccessService.Application.Commands.CreateTasks
{
    public class CreateTasksRequest :IRequest<List<TasksViewModel>>
    {
        public List<CreateTaskModel> tasks { get; set; }
    }
}
