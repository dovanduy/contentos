using ContentProccessService.Entities;
using ContentProccessService.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace ContentProccessService.Application.Queries.GetStatisticsOneMonth
{
    public class GetStatisticsOneMonthHandler : IRequestHandler<GetStatisticsOneMonthRequest, List<StatisticMonthReturnModel>>
    {
        private readonly ContentoDbContext _context;
        public GetStatisticsOneMonthHandler(ContentoDbContext contentodbContext)
        {
            _context = contentodbContext;
        }
        public async Task<List<StatisticMonthReturnModel>> Handle(GetStatisticsOneMonthRequest request, CancellationToken cancellationToken)
        {

            
            var lstTasks = await _context.Statistics
                .Where(x => x.CreatedDate.GetValueOrDefault() >= DateTime.UtcNow.AddMonths(-1) && x.CreatedDate < DateTime.UtcNow)
                .ToListAsync();
            var lstTagInter = new List<StatisticsModelMonth>();
            var lstTagInterReturn = new List<StatisticMonthReturnModel>();
            var lstweek = new List<WeekStatics>();
            for (int i = 0; i < 4; i++)
            {
                var week = new WeekStatics();
                if (i == 3)
                {
                    week.StratDate = DateTime.UtcNow.AddMonths(-1).AddDays(7 * i);
                    week.EndDate = DateTime.UtcNow.AddDays(-1);
                }
                else
                {
                    week.StratDate = DateTime.UtcNow.AddMonths(-1).AddDays(7 * i);
                    week.EndDate = week.StratDate.AddDays(7);
                }
               
              
                lstweek.Add(week);
            }
            foreach (var item in lstTasks)
            {
                var lstTag = _context.TasksTags.Include(x => x.IdTagNavigation).Where(x => x.IdTask == item.IdTask).Select(x => x.IdTagNavigation).ToList();
                foreach (var item2 in lstTag)
                {
                    if (lstTagInter.Any(x => x.IdTags == item2.Id && x.StratDate >= item.CreatedDate && x.EndDate < item.CreatedDate))
                    {
                        lstTagInter.Where(x => x.IdTags == item2.Id && x.StratDate >= item.CreatedDate && x.EndDate < item.CreatedDate).FirstOrDefault().TimeInTeraction += item.Views ?? 0;
                    }
                    else
                    {
                        var Alori = new StatisticsModelMonth();
                        Alori.IdTags = item2.Id;
                        foreach (var item3 in lstweek)
                        {
                            if ( item.CreatedDate >= item3.StratDate && item.CreatedDate < item3.EndDate  )
                            {
                                Alori.StratDate = item3.StratDate;
                                Alori.EndDate = item3.EndDate;
                            }
                        }
                        Alori.TimeInTeraction += item.Views ?? 0;
                        lstTagInter.Add(Alori);
                    }
                }

            }
            foreach (var item in lstTagInter)
            {
                var testst = item;
                var test = lstTagInterReturn.Where(x => x.StratDate == item.StratDate && x.EndDate == item.EndDate).FirstOrDefault();
                if (test != null)
                {
                    var test2 = test.TimeInteraction.ToArray();
                    switch (item.IdTags)
                    {
                        case 1:
                            {

                                test2[0] += item.TimeInTeraction;
                                test.TimeInteraction = test2.ToList();
                                break;
                            }
                        case 2:
                            {
                                test2[1] += item.TimeInTeraction;
                                test.TimeInteraction = test2.ToList();
                                break;
                            }
                        case 3:
                            {

                                test2[2] += item.TimeInTeraction;
                                test.TimeInteraction = test2.ToList();
                                break;
                            }
                        case 4:
                            {

                                test2[3] += item.TimeInTeraction;
                                test.TimeInteraction = test2.ToList();
                                break;
                            }
                        case 5:
                            {

                                test2[4] += item.TimeInTeraction;
                                test.TimeInteraction = test2.ToList();
                                break;
                            }
                        case 6:
                            {

                                test2[5] += item.TimeInTeraction;
                                test.TimeInteraction = test2.ToList();
                                break;
                            }
                        case 7:
                            {

                                test2[6] += item.TimeInTeraction;
                                test.TimeInteraction = test2.ToList();
                                break;
                            }
                        case 8:
                            {

                                test2[7] += item.TimeInTeraction;
                                test.TimeInteraction = test2.ToList();
                                break;
                            }
                        case 9:
                            {

                                test2[8] += item.TimeInTeraction;
                                test.TimeInteraction = test2.ToList();
                                break;
                            }
                        case 10:
                            {

                                test2[9] += item.TimeInTeraction;
                                test.TimeInteraction = test2.ToList();
                                break;
                            }

                        default:
                            {
                                break;
                            }
                    }

                }
                else
                {
                    var statiscs = new StatisticMonthReturnModel();
                    statiscs.StratDate = item.StratDate;
                    statiscs.EndDate = item.EndDate;
                    statiscs.Date = item.StratDate.Day.ToString() + " - " + item.EndDate.ToString("dd-MM-yyyy");
                    int[] arr = new int[10];
                    switch (item.IdTags)
                    {
                        case 1:
                            {

                                arr[0] += item.TimeInTeraction;
                                break;
                            }
                        case 2:
                            {
                                arr[1] += item.TimeInTeraction;
                                break;
                            }
                        case 3:
                            {

                                arr[2] += item.TimeInTeraction;
                                break;
                            }
                        case 4:
                            {

                                arr[3] += item.TimeInTeraction;
                                break;
                            }
                        case 5:
                            {

                                arr[4] += item.TimeInTeraction;
                                break;
                            }
                        case 6:
                            {

                                arr[5] += item.TimeInTeraction;
                                break;
                            }
                        case 7:
                            {

                                arr[6] += item.TimeInTeraction;
                                break;
                            }
                        case 8:
                            {

                                arr[7] += item.TimeInTeraction;
                                break;
                            }
                        case 9:
                            {

                                arr[8] += item.TimeInTeraction;
                                break;
                            }
                        case 10:
                            {

                                arr[9] += item.TimeInTeraction;
                                break;
                            }

                        default:
                            {
                                break;
                            }
                    }
                    statiscs.TimeInteraction = arr.ToList();
                    lstTagInterReturn.Add(statiscs);

                }

            }
            var newlistreturn = lstTagInterReturn.OrderBy(x => x.StratDate).ToList();
            if (lstTagInterReturn.Count != 4)
            {
                foreach (var item5 in lstweek)
                {
                    if (!newlistreturn.Any(x=>x.StratDate == item5.StratDate))
                    {
                        var statiscs = new StatisticMonthReturnModel();
                        statiscs.StratDate = item5.StratDate;
                        statiscs.EndDate = item5.EndDate;
                        statiscs.Date = item5.StratDate.Day.ToString() + " - " + item5.EndDate.ToString("dd-MM-yyyy");
                        int[] arr = new int[10];
                        statiscs.TimeInteraction = arr.ToList();
                        newlistreturn.Add(statiscs);
                    }
                }


            }

            return newlistreturn.OrderBy(x => x.StratDate).ToList();
        }
     

    }
}
