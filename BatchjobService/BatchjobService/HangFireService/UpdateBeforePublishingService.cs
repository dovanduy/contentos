using BatchjobService.Entities;
using Hangfire;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BatchjobService.HangFireService
{
    public interface IUpdateBeforePublishingService
    {
        void UpdateStatusBeforePublishing(int id , DateTime time, List<int> lstTag);
    }
    public class UpdateBeforePublishingService : IUpdateBeforePublishingService
    {
        private readonly ContentoDbContext _context;
        public UpdateBeforePublishingService(ContentoDbContext contentodbContext)
        {
            _context = contentodbContext;
        }
        public void UpdateStatusBeforePublishing(int id, DateTime time, List<int> lstTag)
        {
            var upTask = _context.Tasks.FirstOrDefault(x => x.Id == id);
            _context.Entry(upTask).Collection(p => p.TasksTags).Load();

            List<TasksTags> tags = new List<TasksTags>();

            foreach(var item in lstTag)
            {
                tags.Add(new TasksTags { IdTag = item , IdTask = upTask.Id});
            }

            if(upTask.Status <= 6)
            {
                if (upTask.Status == 6)
                {
                    var taskFanpages = _context.TasksFanpages.Where(w => w.IdTask == id).ToList();
                    foreach (var item in taskFanpages)
                    {
                        BackgroundJob.Delete(item.IdJob);
                    }
                    _context.RemoveRange(taskFanpages);
                }
               upTask.Status = 6;
               upTask.PublishTime = time;
               _context.Update(upTask);
               _context.RemoveRange(upTask.TasksTags);
               _context.TasksTags.AddRange(tags);
               _context.SaveChanges();
            }
        }
    }
}
