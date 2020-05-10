using ContentProccessService.Entities;
using ContentProccessService.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace ContentProccessService.Application.Queries.GetListTaksByCampaignId
{
    public class GetListTasksByCampaignIdHandler : IRequestHandler<GetListTasksByCampaignIdRequest, List<TasksViewModel>>
    {
        private readonly ContentoDbContext _context;
        public GetListTasksByCampaignIdHandler(ContentoDbContext contentodbContext)
        {
            _context = contentodbContext;
        }
        public async Task<List<TasksViewModel>> Handle(GetListTasksByCampaignIdRequest request, CancellationToken cancellationToken)
        {
            var task = await _context.Tasks.AsNoTracking()
                .Include(x=>x.StatusNavigation)
                .Include(x=>x.IdWritterNavigation)
                .Where(x => x.IdCampaign == request.IdCampaign)
                .OrderByDescending(x => x.CreatedDate)
                .ToListAsync();

            var lstTask = new List<TasksViewModel>();
            foreach (var item in task)
            {
                var Writter = new UsersModels
                {
                    Id = item.IdWritterNavigation.Id,
                    Name = item.IdWritterNavigation.FirstName + " " + item.IdWritterNavigation.LastName
                };
                var Status = new StatusModels
                {
                    Id = item.Status,
                    Name = item.StatusNavigation.Name,
                    Color = item.StatusNavigation.Color,
                };
                var taskView = new TasksViewModel()
                {
                    Title = item.Title,
                    Deadline = item.Deadline,
                    PublishTime = item.PublishTime,
                    Writer = Writter,
                    //Description = item.Description,
                    Status = Status,
                    StartedDate = item.StartDate,
                    Id = item.Id
                };
                lstTask.Add(taskView);
            }

            return lstTask;
        }
    }
}
