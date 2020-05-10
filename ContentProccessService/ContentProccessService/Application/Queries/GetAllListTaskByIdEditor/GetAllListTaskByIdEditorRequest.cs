using ContentProccessService.Models;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ContentProccessService.Application.Queries.GetAllListTaskByIdEditor
{
    public class GetAllListTaskByIdEditorRequest : IRequest<List<TasksViewByEditorModel>>
    {
        public int IdEditor { get; set; }
    }
}
