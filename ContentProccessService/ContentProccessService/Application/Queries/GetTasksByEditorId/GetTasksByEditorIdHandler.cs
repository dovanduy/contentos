using ContentProccessService.Entities;
using ContentProccessService.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace ContentProccessService.Application.Queries.GetTasksByEditorId
{
    public class GetTasksByEditorIdHandler : IRequestHandler<GetTasksByEditorIdRequest, List<TasksViewByEditorModel>>
    {
        private readonly ContentoDbContext Context;

        public GetTasksByEditorIdHandler(ContentoDbContext context)
        {
            Context = context;
        }

        public async Task<List<TasksViewByEditorModel>> Handle(GetTasksByEditorIdRequest request, CancellationToken cancellationToken)
        {
            List<TasksViewByEditorModel> Tasks = new List<TasksViewByEditorModel>();
            var ls = await Context.Tasks.AsNoTracking()
                .Include(t => t.IdCampaignNavigation)
                .Include(g=>g.StatusNavigation)
                .Include(f => f.Contents)
                .Where(t => t.IdCampaignNavigation.IdEditor == request.IdEditor)
                .Where(t => t.Status == 3)
                .OrderBy(x => x.PublishTime)
                .ToListAsync();
            
            foreach (var item in ls)
            {
                var Status = new StatusTaskModels
                {
                    Name = item.StatusNavigation.Name,
                    Color = item.StatusNavigation.Color,
                    Id = item.StatusNavigation.Id
                };
                var Campaign = new CampaignModels
                {
                    Id = item.IdCampaignNavigation.Id,
                    Name = item.IdCampaignNavigation.Title
                };
                Tasks.Add(new TasksViewByEditorModel
                {
                    Id = item.Id,
                    //Description = item.Description,
                    Campaign = Campaign,
                    ModifiedDate = item.Contents.FirstOrDefault(x=>x.IsActive == true).ModifiedDate,
                    Deadline = item.Deadline,
                    Title = item.Title,
                    Status = Status
                });
            }
            return Tasks;
        }


    }
}

