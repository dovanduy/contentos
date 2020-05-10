using BatchjobService.Entities;
using BatchjobService.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace BatchjobService.Application.Queries.GetListFanpageByTags
{
    public class GetListFanpageByTagsHandler : IRequestHandler<GetListFanpageByTagsRequest, Dictionary<string, List<int>>>
    {
        private readonly ContentoDbContext _context;

        public GetListFanpageByTagsHandler(ContentoDbContext context)
        {
            _context = context;
        }
        public async Task<Dictionary<string, List<int>>> Handle(GetListFanpageByTagsRequest request, CancellationToken cancellationToken)
        {
            var fanpages = await _context.Fanpages
                .Include(i => i.IdChannelNavigation)
                .Include(i => i.FanpagesTags).ThenInclude(FanpagesTags => FanpagesTags.IdTagNavigation)
                .Where(w => w.IdCustomer == null || w.IdCustomer == request.idCustomer)
                .ToListAsync();

            Dictionary<string, List<int>> result = new Dictionary<string, List<int>>();
            List<int> lstContento = new List<int>();
            List<int> lstFB = new List<int>();
            List<int> lstWP = new List<int>();

            foreach (var fanpage in fanpages)
            {
                bool flag = false;
                foreach(var tag in fanpage.FanpagesTags)
                {
                    if (request.lstTags.Contains(tag.IdTag))
                    {
                        flag = true;
                        break;
                    }
                }

                if (!flag)
                {
                    continue;
                }

                switch (fanpage.IdChannel)
                {
                    case 1:
                        lstContento.Add(fanpage.Id);
                        break;
                    case 2:
                        lstFB.Add(fanpage.Id);
                        break;
                    case 3:
                        lstWP.Add(fanpage.Id);
                        break;
                }
            }

            result.Add("Contento", lstContento);
            result.Add("Facebook", lstFB);
            result.Add("Wordpress", lstWP);

            return result;
        }
    }
}
