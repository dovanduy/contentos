using BatchjobService.Entities;
using BatchjobService.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;

namespace BatchjobService.Application.Queries.Test
{
    public class TestHandler : IRequestHandler<TestRequest, List<AlgorithmDataBeforeModel>>
    {
        private readonly ContentoDbContext _context;
        public TestHandler(ContentoDbContext context)
        {
            _context = context;
        }
        public async Task<List<AlgorithmDataBeforeModel>> Handle(TestRequest request, CancellationToken cancellationToken)
        {
            var listIduser = await _context.UsersInteractions.Select(x => x.IdUser).Distinct().ToListAsync();
            var lstAlori = new List<AlgorithmDataBeforeModel>();
            var lstTwoMonth = new List<AlgorithmDataBeforeModel>();
            var lstTest = new List<ListTaskModel>();
            foreach (var test in listIduser)
            {
                var userInterId = await _context.UsersInteractions.Where(x => x.IdUser == test).Select(x => new ListTaskModel
                {
                    IdUser = x.IdUser,
                    IdTask = _context.UsersInteractions.Where(z => z.IdUser == x.IdUser).Select(z => new TaskInterModel { Id = z.IdTask, Interaction = z.Interaction ?? 0 }).ToList()
                }).FirstOrDefaultAsync();
                lstTest.Add(userInterId);
            }
            var lstTagsum = new List<int>();
            foreach (var item in lstTest)
            {
                var lstTagChoose = _context.Personalizations.Where(x => x.IdUser == item.IdUser && x.IsChosen == true).Select(x => x.IdTag).ToList();
                var listTaskTwoMonth = GetTaskTwoMonth(item.IdTask);
                var lstOutTwoMonth = item.IdTask.Except(listTaskTwoMonth).ToList();
                if (listTaskTwoMonth.Count != 0)
                {
                    foreach (var item1 in listTaskTwoMonth)
                    {
                        var lstTag = _context.TasksTags.Where(x => x.IdTask == item1.Id).Select(x => x.IdTag).ToList();
                        var listfinal = lstTag.Intersect(lstTagChoose).ToList();
                        foreach (var item2 in listfinal)
                        {
                            if (lstTwoMonth.Any(x => x.IdTag == item2 && x.IdUser == item.IdUser))
                            {
                                lstTwoMonth.Where(x => x.IdTag == item2 && x.IdUser == item.IdUser).FirstOrDefault().TimeInTeraction += item1.Interaction > 0 ? item1.Interaction*0.9 : 0;
                            }
                            else
                            {
                                var Alori = new AlgorithmDataBeforeModel { IdUser = item.IdUser };
                                Alori.IdTag = item2;
                                Alori.TimeInTeraction = Alori.TimeInTeraction + item1.Interaction*0.9;
                                lstTwoMonth.Add(Alori);
                            }

                        }

                    }
                }
                foreach (var item1 in lstOutTwoMonth)
                {
                    var lstTag = _context.TasksTags.Where(x => x.IdTask == item1.Id).Select(x => x.IdTag).ToList();
                    var listfinal = lstTag.Intersect(lstTagChoose).ToList();
                    foreach (var item2 in listfinal)
                    {
                        if (lstTwoMonth.Any(x => x.IdTag == item2 && x.IdUser == item.IdUser))
                        {
                            lstTwoMonth.Where(x => x.IdTag == item2 && x.IdUser == item.IdUser).FirstOrDefault().TimeInTeraction += item1.Interaction > 0 ? item1.Interaction * 0.1 : 0;
                        }
                        else
                        {
                            var Alori = new AlgorithmDataBeforeModel { IdUser = item.IdUser };
                            Alori.IdTag = item2;
                            Alori.TimeInTeraction = Alori.TimeInTeraction + item1.Interaction*0.1;
                            lstTwoMonth.Add(Alori);
                        }

                    }
                }
            }

            return lstTwoMonth;
        }
        public List<TaskInterModel> GetTaskTwoMonth(List<TaskInterModel> ListTags)
        {
            var lstTaskTwo = new List<TaskInterModel>();
            foreach (var item in ListTags)
            {
                var task = _context.Tasks.FirstOrDefault(x => x.Id == item.Id
                && x.Status == 7
                && x.Contents.Any(t => t.IsActive == true)
                && x.TasksFanpages.Any(t => t.IdFanpage == 1));
                var test = DateTime.UtcNow.AddMonths(-2);
                if (task != null)
                {
                    if (task.PublishTime >= DateTime.UtcNow.AddMonths(-2) && task.PublishTime < DateTime.UtcNow)
                    {
                        var newTask = new TaskInterModel()
                        {
                            Id = item.Id,
                            Interaction = item.Interaction
                        };
                        lstTaskTwo.Add(newTask);
                    }
                }

            }
            return lstTaskTwo;
        }

    }
}
