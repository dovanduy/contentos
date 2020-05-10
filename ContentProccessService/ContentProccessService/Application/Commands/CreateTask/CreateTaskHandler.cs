using ContentProccessService.Entities;
using ContentProccessService.Models;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace ContentProccessService.Application.Commands.CreateTask
{
    public class CreateTaskHandler : IRequestHandler<CreateTaskRequest, TasksViewModel>
    {
        private readonly ContentoDbContext contentodbContext;
        public CreateTaskHandler(ContentoDbContext contentodbContext)
        {
            this.contentodbContext = contentodbContext;
        }
        public async Task<TasksViewModel> Handle(CreateTaskRequest request, CancellationToken cancellationToken)
        {
            //var task = new Tasks
            //{
            //    IdCampaign = request.Task.IdCampaign,
            //    IdWriter = request.Task.IdWriter,
            //    Description = request.Task.Description,
            //    Deadline = request.Task.Deadline,
            //    PublishTime = request.Task.PublishTime,
            //    Title = request.Task.Title,
            //    StartedDate = DateTime.UtcNow,
            //    ModifiedDate = DateTime.UtcNow,
            //    Status = 1
            //};
            //contentodbContext.Tasks.Add(task);
            //await contentodbContext.SaveChangesAsync();
            //return task;
            var transaction = contentodbContext.Database.BeginTransaction();
            try
            {
                var Tags = new List<TasksTags>();

                foreach (var item in request.Task.Tags)
                {
                    var tag = new TasksTags { IdTag = item, CreatedDate = DateTime.UtcNow };
                    Tags.Add(tag);
                }
                var task = new Tasks
                {
                    IdCampaign = request.Task.IdCampaign,
                    IdWritter = request.Task.IdWriter,
                    Deadline = request.Task.Deadline,
                    Description = request.Task.Description,
                    PublishTime = request.Task.PublishTime,
                    Title = request.Task.Title,
                    CreatedDate = DateTime.UtcNow,
                    TasksTags = Tags,
                    Status = 1
                };
                contentodbContext.Attach(task);
                contentodbContext.Tasks.Add(task);
                await contentodbContext.SaveChangesAsync(cancellationToken);
                var ReturnTags = new List<TagsViewModel>();

                foreach (var item in task.TasksTags)
                {
                    var tag = new TagsViewModel { Id = item.IdTag, Name = contentodbContext.Tags.Find(item.IdTag).Name };
                    ReturnTags.Add(tag);
                }
                var status = new StatusModels();

                status.Id = task.Status;
                status.Name = contentodbContext.StatusTasks.FirstOrDefault(x => x.Id == task.Status).Name;
                status.Color = contentodbContext.StatusTasks.FirstOrDefault(x => x.Id == task.Status).Color;

                var user = contentodbContext.Users.FirstOrDefault(x => x.Id == task.IdWritter);
                var writer = new UsersModels
                {
                    Id = task.IdWritter,

                    Name = user.FirstName + " " + user.LastName
                };
                var taskModel = new TasksViewModel
                {
                    Deadline = task.Deadline,
                    Writer = writer,
                    Id = task.Id,
                    Description = task.Description,
                    StartedDate = task.CreatedDate,
                    PublishTime = task.PublishTime,
                    Tags = ReturnTags,
                    Status = status,
                    Title = task.Title,
                    EmailWriter = contentodbContext.Accounts.FirstOrDefault(x => x.IdUserNavigation.Id == task.IdWritter).Email
                };
                var upStatus = contentodbContext.Campaigns.FirstOrDefault(y => y.Id == request.Task.IdCampaign);
                if (upStatus.Status == 1)
                {
                    upStatus.Status = 2;
                    upStatus.StartDate = DateTime.UtcNow;
                    contentodbContext.Attach(upStatus);
                    contentodbContext.Entry(upStatus).Property(x => x.Status).IsModified = true;
                    await contentodbContext.SaveChangesAsync(cancellationToken);
                }

                transaction.Commit();
                return taskModel;
            }
            catch (Exception e)
            {
                transaction.Rollback();
                return null;
            }
        }


    }
}
