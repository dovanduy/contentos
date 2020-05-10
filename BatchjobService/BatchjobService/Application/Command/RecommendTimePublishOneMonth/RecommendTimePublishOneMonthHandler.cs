using BatchjobService.Entities;
using BatchjobService.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace BatchjobService.Application.Command.RecommendTimePublishOneMonth
{
    public class RecommendTimePublishOneMonthHandler : IRequestHandler<RecommendTimePublishOneMonthRequest, List<RecommendsTimeModels>>
    {
        private readonly ContentoDbContext _context;

        public RecommendTimePublishOneMonthHandler(ContentoDbContext context)
        {
            _context = context;
        }
        public async Task<List<RecommendsTimeModels>> Handle(RecommendTimePublishOneMonthRequest request, CancellationToken cancellationToken)
        {
            var retTime = new List<RecommendsTimeModels>();

            foreach (var item in request.IdFanpages)
            {
                var lstInterDay = _context
                    .FanpagesInteraction
                    .Where(x => x.IdFanpages == item 
                    && x.CreatedDate.GetValueOrDefault().Hour == 23 
                    && x.CreatedDate.GetValueOrDefault().DayOfYear >= DateTime.UtcNow.AddMonths(-1).DayOfYear 
                    && x.CreatedDate.GetValueOrDefault().DayOfYear < DateTime.UtcNow.DayOfYear)
                    .OrderByDescending(x => x.Interaction).FirstOrDefault();
                var lstInter = await _context
                    .FanpagesInteraction
                    .Include(x => x.IdFanpagesNavigation)
                    .AsNoTracking()
                    .Where(x => x.IdFanpages == item
                    && x.CreatedDate.GetValueOrDefault().DayOfYear >= DateTime.UtcNow.AddMonths(-1).DayOfYear && x.CreatedDate.GetValueOrDefault().DayOfYear < DateTime.UtcNow.DayOfYear)
                    .OrderBy(x => x.CreatedDate).ToListAsync();

                for (int i = 0; i < lstInter.Count; i++)
                {
                    if (lstInter[i].CreatedDate.GetValueOrDefault().Hour == 23)
                    {
                        var interBefore = _context
                            .FanpagesInteraction
                            .Where(x => x.IdFanpages == item
                            && x.CreatedDate.GetValueOrDefault().DayOfYear == lstInter[i].CreatedDate.GetValueOrDefault().AddDays(1).DayOfYear)
                            .OrderBy(x => x.CreatedDate).FirstOrDefault();
                        lstInter[i].Interaction = (interBefore.Interaction ?? 0) - (lstInter[i].Interaction ?? 0);
                    }
                    else if (i == (lstInter.Count - 1))
                    {
                        lstInter.RemoveAt(i);
                    }
                    else
                    {
                        lstInter[i].Interaction = (lstInter[i + 1].Interaction ?? 0) - (lstInter[i].Interaction ?? 0);
                    }

                }

                var newLst = lstInter.OrderByDescending(x => x.Interaction).ToList();

                var bestTimeInter = newLst.Where(x => x.Interaction == newLst.FirstOrDefault().Interaction).ToList();
                if (bestTimeInter.Count > 1)
                {
                    foreach (var item3 in bestTimeInter)
                    {
                        if (lstInterDay.CreatedDate == item3.CreatedDate)
                        {
                            var RtModel = new RecommendsTimeModels
                            {
                                FanpageName = item3.IdFanpagesNavigation.Name,
                                PublishTime = item3.CreatedDate.GetValueOrDefault().AddDays(7).TimeOfDay.ToString().Substring(0, 5)+" " + item3.CreatedDate.GetValueOrDefault().AddDays(7).DayOfWeek.ToString()
                            };
                            retTime.Add(RtModel);
                            break;
                        }
                    }
                }
                else
                {
                    var oneBest = newLst.FirstOrDefault();
                    var RtModel = new RecommendsTimeModels
                    {
                        FanpageName = oneBest.IdFanpagesNavigation.Name,
                        PublishTime = oneBest.CreatedDate.GetValueOrDefault().AddDays(7).TimeOfDay.ToString().Substring(0, 5)+" " +  oneBest.CreatedDate.GetValueOrDefault().AddDays(7).DayOfWeek.ToString()
                    };
                    retTime.Add(RtModel);
                }

            }
            return retTime;
        }
    }
}
