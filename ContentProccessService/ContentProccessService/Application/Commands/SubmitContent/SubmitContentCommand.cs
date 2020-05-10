using ContentProccessService.Models;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ContentProccessService.Application.Commands.SubmitContent
{
    public class SubmitContentCommand : IRequest
    {
        public int IdTask { get; set; }
        public int IdContent { get; set; }
        public string Name { get; set; }
        public string Content { get; set; }

    }
}
