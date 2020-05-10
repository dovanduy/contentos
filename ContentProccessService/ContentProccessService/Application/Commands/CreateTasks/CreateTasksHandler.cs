using ContentProccessService.Entities;
using ContentProccessService.Models;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Transactions;

namespace ContentProccessService.Application.Commands.CreateTasks
{
    public class CreateTasksHandler : IRequestHandler<CreateTasksRequest, List<TasksViewModel>>
    {
        private readonly ContentoDbContext contentodbContext;
        public CreateTasksHandler(ContentoDbContext contentodbContext)
        {
            this.contentodbContext = contentodbContext;
        }
        public async Task<List<TasksViewModel>> Handle(CreateTasksRequest request, CancellationToken cancellationToken)
        {
            // Start a local transaction.
            var transaction = contentodbContext.Database.BeginTransaction();
            try
            {
                List<TasksViewModel> tasks = new List<TasksViewModel>();

                foreach (var item in request.tasks)
                {
                    var task = new Tasks
                    {
                        IdCampaign = item.IdCampaign,
                        IdWritter = item.IdWriter,
                        Deadline = item.Deadline,
                        Description = item.Description,
                        PublishTime = item.PublishTime,
                        Title = item.Title,
                        CreatedDate = DateTime.UtcNow,
                        //ModifiedDate = DateTime.UtcNow,
                        Status = 1
                    };
                    contentodbContext.Attach(task);
                    contentodbContext.Tasks.Add(task);
                    await contentodbContext.SaveChangesAsync(cancellationToken);
                    var taskModel = new TasksViewModel
                    {
                        Deadline = task.Deadline,
                        Id = task.Id,
                        Description = task.Description,
                        StartedDate = task.CreatedDate,
                        PublishTime = task.PublishTime,
                        Title = task.Title,
                    };
                    tasks.Add(taskModel);
                }
                var upStatus = contentodbContext.Campaigns.FirstOrDefault(y => y.Id == request.tasks.FirstOrDefault().IdCampaign);
                if (upStatus.Status == 1)
                {
                    upStatus.Status = 2;
                    contentodbContext.Attach(upStatus);
                    contentodbContext.Entry(upStatus).Property(x => x.Status).IsModified = true;
                    await contentodbContext.SaveChangesAsync(cancellationToken);
                }
                transaction.Commit();
                return tasks;
            }
            catch(Exception e)
            {
                transaction.Rollback();
                return null;
            }
           
            

           

           
        }
    }
}
