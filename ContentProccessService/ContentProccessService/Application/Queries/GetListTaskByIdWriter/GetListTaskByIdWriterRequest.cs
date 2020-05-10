using ContentProccessService.Models;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ContentProccessService.Application.Queries.GetListTaskByIdWriter
{
    public class GetListTaskByIdWriterRequest :IRequest<List<TasksViewByEditorModel>>
    {
        public int IdWriter { get; set; }
    }
}
