using ContentProccessService.Entities;
using ContentProccessService.Models;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ContentProccessService.Application.Commands.ApproveRejectContent
{
    public class ApproveRejectContentRequest : IRequest<bool>
    {
        public int IdTask { get; set; }
        public int IdContent { get; set; }
        public string Comments { get; set; }
        public bool Button { get; set; }
        public string Name { get; set; }
    }
   
}
