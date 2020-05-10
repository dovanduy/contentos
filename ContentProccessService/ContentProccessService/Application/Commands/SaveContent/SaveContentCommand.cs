using ContentProccessService.Models;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ContentProccessService.Application.Commands.SaveContent
{
    public class SaveContentCommand : IRequest<ContentsViewModel>
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Content { get; set; }
    }
}
