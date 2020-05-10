using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BatchjobService.Application.Command.DeleteFanpage
{
    public class DeleteFanpageCommand : IRequest<Unit>
    {
        public int id { get; set; }
    }
}
