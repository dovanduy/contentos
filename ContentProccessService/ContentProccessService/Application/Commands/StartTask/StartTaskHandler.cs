using ContentProccessService.Entities;
using ContentProccessService.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace ContentProccessService.Application.Commands.StartTask
{
    public class StartTaskHandler : IRequestHandler<StartTaskCommand, TasksViewModel>
    {
        private readonly ContentoDbContext _context;

        public StartTaskHandler(ContentoDbContext context)
        {
            _context = context;
        }
        public async Task<TasksViewModel> Handle(StartTaskCommand request, CancellationToken cancellationToken)
        {
            var transaction = _context.Database.BeginTransaction();
            try
            {
                var newContent = new Contents
                {
                    CreatedDate = DateTime.UtcNow,
                    Version = 1,
                    IdTask = request.IdTask,
                    IsActive = true
                };
                _context.Contents.Add(newContent);
                var upTask = _context.Tasks.AsNoTracking().Include(y => y.Contents).FirstOrDefault(x => x.Id == request.IdTask);
                upTask.Status = 2;
                upTask.StartDate = DateTime.UtcNow;
                upTask.ModifiedDate = DateTime.UtcNow;
                _context.Attach(upTask);
                _context.Entry(upTask).State = EntityState.Modified;
                await _context.SaveChangesAsync(cancellationToken);
                // get detail task
                var task = await _context.Tasks.AsNoTracking().Include(i => i.Contents).FirstOrDefaultAsync(x => x.Id == request.IdTask);
                var content = task.Contents.Where(x => x.IsActive == true).FirstOrDefault();
                var edtId = _context.Campaigns.Find(task.IdCampaign).IdEditor;
                var campaign = _context.Campaigns.Find(task.IdCampaign).Title;
                var lstTag = new List<TagsViewModel>();
                var lstTags = _context.TasksTags.Where(x => x.IdTask == request.IdTask).ToList();
                foreach (var item in lstTags)
                {
                    var tag = new TagsViewModel();
                    tag.Name = _context.Tags.FirstOrDefault(x => x.Id == item.IdTag).Name;
                    tag.Id = item.IdTag;
                    lstTag.Add(tag);
                }
                var user = _context.Users.FirstOrDefault(x => x.Id == task.IdWritter);
                var Writter = new UsersModels
                {
                    Id = task.IdWritter,
                    Name = user.FirstName + " " + user.LastName
                };
                var Status = new StatusModels
                {
                    Id = task.Status,
                    Name = _context.StatusTasks.FirstOrDefault(x => x.Id == task.Status).Name,
                    Color = _context.StatusTasks.FirstOrDefault(x => x.Id == task.Status).Color
                };
                var edtn = _context.Users.FirstOrDefault(x => x.Id == edtId);
                var Editor = new UsersModels
                {
                    Id = edtId,
                    Name = edtn.FirstName + " "+edtn.LastName
                };
                var Content = new ContentModels
                {
                    Id = content.Id,
                    Content = content.TheContent,
                    Name = content.Name
                };
                var taskView = new TasksViewModel()
                {
                    Title = task.Title,
                    Deadline = task.Deadline,
                    PublishTime = task.PublishTime,
                    Writer = Writter,
                    Description = task.Description,
                    Status = Status,
                    StartedDate = task.StartDate,
                    Editor = Editor,
                    Content = Content,
                    Id = task.Id,
                    Tags = lstTag,
                    Campaign = campaign
                };
                transaction.Commit();
                return taskView;
                
            }
            catch (Exception)
            {
                transaction.Rollback();
                return null;
            }
            
            
        }
    }
}
