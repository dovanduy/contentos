using ContentProccessService.Models;
using MediatR;
using System.Collections.Generic;

namespace ContentProccessService.Application.Queries.GetTasksByEditorId
{
    public class GetTasksByEditorIdRequest : IRequest<List<TasksViewByEditorModel>>
    {
        public int IdEditor { get; set; }
    }
}
