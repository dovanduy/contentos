using ContentProccessService.Models;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ContentProccessService.Application.Commands.UpdatetTaskEditor
{
    public class UpdateTaskEditorCommand : IRequest<ReturnUpdateTaskModel>
    {
        public int IdTask { get; set; }
        public int IdWriter { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime? Deadline { get; set; }
        public DateTime? PublishTime { get; set; }
        public List<int> Tags { get; set; }
    }
}
