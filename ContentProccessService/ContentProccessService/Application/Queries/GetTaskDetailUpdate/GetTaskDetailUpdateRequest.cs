using ContentProccessService.Models;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ContentProccessService.Application.Queries.GetTaskDetailUpdate
{
    public class GetTaskDetailUpdateRequest : IRequest<TasksViewModelReturn>
    {
        public int IdTask { get; set; }
    }
}
