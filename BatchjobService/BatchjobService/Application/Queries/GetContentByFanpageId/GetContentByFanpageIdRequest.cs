using BatchjobService.Models;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BatchjobService.Application.Queries.GetContentByFanpageId
{
    public class GetContentByFanpageIdRequest : IRequest<List<ContentModel>>
    {
        public int id { get; set; }
    }
}
