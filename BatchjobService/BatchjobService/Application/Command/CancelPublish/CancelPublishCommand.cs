using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BatchjobService.Application.Command.CancelPublish
{
    public class CancelPublishCommand : IRequest<string>
    {
        public int taskId { get; set; }
    }
}
