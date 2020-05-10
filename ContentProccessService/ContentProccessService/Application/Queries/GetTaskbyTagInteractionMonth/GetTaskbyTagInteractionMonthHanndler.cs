using ContentProccessService.Entities;
using ContentProccessService.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace ContentProccessService.Application.Queries.GetTaskbyTagInteractionMonth
{
    public class GetTaskbyTagInteractionMonthHanndler : IRequestHandler<GetTaskbyTagInteractionMonthRequest, List<StaticsDetailModel>>
    {
        private readonly ContentoDbContext _context;
        public GetTaskbyTagInteractionMonthHanndler(ContentoDbContext contentodbContext)
        {
            _context = contentodbContext;
        }
        public async Task<List<StaticsDetailModel>> Handle(GetTaskbyTagInteractionMonthRequest request, CancellationToken cancellationToken)
        {

            var lstIdTask = _context.Tasks.Include(x => x.TasksTags)
                .Where(x => x.TasksTags.Any(y => y.IdTag == request.Id)).Select(x => x.Id).ToList();
            var lstTask = await _context.Statistics
                .Include(x => x.IdTaskNavigation)
                .ThenInclude(IdTaskNavigation => IdTaskNavigation.Contents)
                .Where(x => x.CreatedDate >= request.StartDate && x.CreatedDate < request.EndDate && lstIdTask.Contains(x.IdTask)).ToListAsync();
            var lstTaskView = new List<StaticsDetailModel>();
            foreach (var item in lstTask)
            {
                if (lstTaskView.Any(x => x.Date.GetValueOrDefault().DayOfYear == item.CreatedDate.GetValueOrDefault().DayOfYear))
                {
                    lstTaskView.FirstOrDefault(x => x.Date.GetValueOrDefault().DayOfYear == item.CreatedDate.GetValueOrDefault().DayOfYear).TimeInTeraction += item.Views ?? 0;
                }
                else
                {
                    var taskView = new StaticsDetailModel();
                    taskView.Date = item.CreatedDate;
                    taskView.TimeInTeraction += item.Views ?? 0;
                    lstTaskView.Add(taskView);
                }
            }
            var newlistreturn = lstTaskView.OrderBy(x => x.Date).ToList();
            if (lstTaskView.Count != 7)
            {
                for (int i = 0; i < 7; i++)
                {
                    if (!newlistreturn.Any(x => x.Date.GetValueOrDefault().DayOfYear == DateTime.UtcNow.AddDays(i - 7).DayOfYear))
                    {
                        var statiscs = new StaticsDetailModel();
                        statiscs.Date = DateTime.UtcNow.AddDays(i - 7);
                        statiscs.TimeInTeraction = 0;
                        newlistreturn.Add(statiscs);
                    }
                }

            }
            return newlistreturn.OrderBy(x => x.Date).Take(10).ToList();

        }
    }
}
