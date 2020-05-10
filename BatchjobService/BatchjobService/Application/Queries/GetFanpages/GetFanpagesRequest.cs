using BatchjobService.Models;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BatchjobService.Application.Queries.GetFanpages
{
    public class GetFanpagesRequest : IRequest<List<FanpageViewModel>>
    {
        public int Id { get; set; }
    }
}
