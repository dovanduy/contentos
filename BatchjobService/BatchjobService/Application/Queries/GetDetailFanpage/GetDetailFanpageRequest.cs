using BatchjobService.Models;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BatchjobService.Application.Queries.GetDetailFanpage
{
    public class GetDetailFanpageRequest : IRequest<EditViewModel>
    { 
        public int Id { get; set; }
    }
}
