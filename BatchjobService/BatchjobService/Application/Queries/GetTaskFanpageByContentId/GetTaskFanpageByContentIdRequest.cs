using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BatchjobService.Application.Queries.GetTaskFanpageByContentId
{
    public class GetTaskFanpageByContentIdRequest : IRequest<Dictionary<string, List<int>>>
    {
        public int Id { get; set; }
    }
}
