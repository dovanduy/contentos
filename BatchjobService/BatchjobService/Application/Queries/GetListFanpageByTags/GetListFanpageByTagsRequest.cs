using BatchjobService.Models;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BatchjobService.Application.Queries.GetListFanpageByTags
{
    public class GetListFanpageByTagsRequest : IRequest<Dictionary<string, List<int>>>
    {
        public List<int> lstTags { get; set; }

        public int idCustomer { get; set; }
    }
}
