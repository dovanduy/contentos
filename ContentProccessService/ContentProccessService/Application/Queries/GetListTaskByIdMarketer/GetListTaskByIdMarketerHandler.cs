using ContentProccessService.Application.Dtos;
using ContentProccessService.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace ContentProccessService.Application.Queries.GetListTaskByIdMarketer
{
    public class GetListTaskByIdMarketerHandler : IRequestHandler<GetListTaskByIdMarketerRequest, List<TasksViewModel>>
    {
        private readonly ContentoDbContext _context;
        public GetListTaskByIdMarketerHandler(ContentoDbContext contentodbContext)
        {
            _context = contentodbContext;
        }
        public async Task<List<TasksViewModel>> Handle(GetListTaskByIdMarketerRequest request, CancellationToken cancellationToken)
        {
            var lstIdCampaign = await _context.Campaigns.AsNoTracking()
                .Include(x => x.Tasks).ThenInclude(Tasks=> Tasks.StatusNavigation)
                .Include(x=> x.Tasks).ThenInclude(Tasks=> Tasks.IdWritterNavigation)
                .Where(x => x.IdMarketer == request.IdMartketer )
                .Select(x => new
                {
                    x,
                    Tasks = x.Tasks.Where(i => i.Status == 5 || i.Status == 6 ||i.Status == 7 || i.Status == 8 ).OrderBy(i=>i.PublishTime).ToList()
                })
                .ToListAsync();
            var lstTask = new List<TasksViewModel>();
            foreach (var item in lstIdCampaign)
            {
                foreach (var itemtask in item.Tasks)
                {
                    var Writter = new UsersModels
                    {
                        Id = itemtask.IdWritterNavigation.Id,
                        Name = itemtask.IdWritterNavigation.FirstName + " " +itemtask.IdWritterNavigation.LastName
                    };
                    var Status = new StatusModels
                    {
                        Id = itemtask.Status,
                        Name = itemtask.StatusNavigation.Name,
                        Color = itemtask.StatusNavigation.Color
                    };
                    var taskView = new TasksViewModel()
                    {
                        Title = itemtask.Title,
                        Deadline = itemtask.Deadline,
                        PublishTime = itemtask.PublishTime,
                        Writer = Writter,
                        //Description = itemtask.Description,
                        Status = Status,
                        StartedDate = itemtask.StartDate,
                        Id = itemtask.Id
                    };
                    lstTask.Add(taskView);
                }

                
            }
            return lstTask;
        }
    }
}
