using BatchjobService.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace BatchjobService.Application.Queries.GetTaskFanpageByContentId
{
    public class GetTaskFanpageByContentIdHandler : IRequestHandler<GetTaskFanpageByContentIdRequest, Dictionary<string, List<int>>>
    {
        private readonly ContentoDbContext _context;

        public GetTaskFanpageByContentIdHandler(ContentoDbContext context)
        {
            _context = context;
        }
        public async Task<Dictionary<string, List<int>>> Handle(GetTaskFanpageByContentIdRequest request, CancellationToken cancellationToken)
        {
            var content = _context.Contents.FirstOrDefault(w => w.Id == request.Id && w.IsActive == true);
            var task = _context.Tasks.FirstOrDefault(x => x.Id == content.IdTask);
            var fanpages = await _context.TasksFanpages.Where(w => w.IdTask == task.Id).Include(i => i.IdFanpageNavigation).ToListAsync();

            Dictionary<string, List<int>> result = new Dictionary<string, List<int>>();
            List<int> lstContento = new List<int>();
            List<int> lstFB = new List<int>();
            List<int> lstWP = new List<int>();

            foreach (var item in fanpages)
            {
                switch (item.IdFanpageNavigation.IdChannel)
                {
                    case 1: lstContento.Add(item.IdFanpage);
                        break;
                    case 2:
                        lstFB.Add(item.IdFanpage);
                        break;
                    case 3:
                        lstWP.Add(item.IdFanpage);
                        break;
                }
            }

            result.Add("Contento", lstContento);
            result.Add("Facebook",lstFB);
            result.Add("Wordpress",lstWP);

            return result;
        }
    }
}
