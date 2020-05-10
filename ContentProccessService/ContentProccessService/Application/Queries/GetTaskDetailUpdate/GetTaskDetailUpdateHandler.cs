using ContentProccessService.Entities;
using ContentProccessService.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace ContentProccessService.Application.Queries.GetTaskDetailUpdate
{
    public class GetTaskDetailUpdateHandler : IRequestHandler<GetTaskDetailUpdateRequest, TasksViewModelReturn>
    {
        private readonly ContentoDbContext _context;
        public GetTaskDetailUpdateHandler(ContentoDbContext contentodbContext)
        {
            _context = contentodbContext;
        }
        public async Task<TasksViewModelReturn> Handle(GetTaskDetailUpdateRequest request, CancellationToken cancellationToken)
        {
            var task = await _context.Tasks.AsNoTracking()
                .Include(x=>x.TasksTags).ThenInclude(TasksTags=> TasksTags.IdTagNavigation)
                .Include(x=> x.IdWritterNavigation)
                .Include(x=>x.StatusNavigation)
                .Include(x=>x.IdCampaignNavigation).ThenInclude(IdCampaignNavigation=> IdCampaignNavigation.IdEditorNavigation)
                .FirstOrDefaultAsync(x => x.Id == request.IdTask);
            var edtId = task.IdCampaignNavigation.IdEditorNavigation.Id;
            var lstTag = new List<TagsViewModel>();
            //var lstTags = _context.TasksTags.Include(x=>x.IdTagNavigation).Where(x => x.IdTask == request.IdTask).ToList();
            var lstTagid = new List<int>();
            foreach (var item in task.TasksTags)
            {
                var tag = new TagsViewModel();
                tag.Name = item.IdTagNavigation.Name;
                tag.Id = item.IdTag;
                lstTagid.Add(item.IdTag);
                lstTag.Add(tag);
            } 
            var Writter = new UsersModels
            {
                Id = task.IdWritter,
                Name = task.IdWritterNavigation.FirstName + " " + task.IdWritterNavigation.LastName
            };
            var Status = new StatusModels
            {
                Id = task.Status,
                Name = task.StatusNavigation.Name,
                Color = task.StatusNavigation.Color
            };
            var taskView = new TasksViewModelReturn()
            {
                Title = task.Title,
                Deadline = task.Deadline,
                PublishTime = task.PublishTime,
                Writer = Writter,
                Description = task.Description,
                Status = Status,
                StartedDate = task.StartDate,
                Id = task.Id,
                Tags = lstTagid,
                TagFull = lstTag

            };

            return taskView;
        }
    }
}
