using ContentProccessService.Entities;
using ContentProccessService.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace ContentProccessService.Application.Queries.GetTaskByCampaignIdInteraction
{
    public class GetTaskByCampaignIdInteractionHandler : IRequestHandler<GetTaskByCampaignIdInteractionRequest, List<ListTaskStaticModel>>
    {
        private readonly ContentoDbContext _context;
        public GetTaskByCampaignIdInteractionHandler(ContentoDbContext contentodbContext)
        {
            _context = contentodbContext;
        }
        public async Task<List<ListTaskStaticModel>> Handle(GetTaskByCampaignIdInteractionRequest request, CancellationToken cancellationToken)
        {
            var lstIdTask = await _context.Tasks.AsNoTracking().Where(x => x.IdCampaign == request.IdCampaign).Select(x=>x.Id).ToListAsync();
            var lstTask = await _context.Statistics.Include(x => x.IdTaskNavigation).ThenInclude(IdTaskNavigation => IdTaskNavigation.Contents).Where(x => lstIdTask.Contains(x.IdTask)).ToListAsync();
            var lstTaskView = new List<ListTaskStaticModel>();
            foreach (var item in lstTask)
            {
                if (lstTaskView.Any(x => x.IdTask == item.IdTaskNavigation.Id))
                {
                    lstTaskView.Where(x => x.IdTask == item.IdTaskNavigation.Id).FirstOrDefault().View += item.Views ?? 0;
                }
                else
                {
                    var taskView = new ListTaskStaticModel();
                    taskView.Title = item.IdTaskNavigation.Contents.FirstOrDefault(x => x.IsActive == true).Name;
                    taskView.View += item.Views ?? 0;
                    taskView.IdTask = item.IdTaskNavigation.Id;
                    lstTaskView.Add(taskView);
                }
            }
            return lstTaskView.OrderByDescending(x => x.View).ToList();
        }
    }
}
