using BatchjobService.Entities;
using Hangfire;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace BatchjobService.Application.Command.CancelPublish
{
    public class CancelPublishHandler : IRequestHandler<CancelPublishCommand, string>
    {
        private readonly ContentoDbContext _context;

        public CancelPublishHandler(ContentoDbContext context)
        {
            _context = context;
        }

        public async Task<string> Handle(CancelPublishCommand request, CancellationToken cancellationToken)
        {
            var taskFanpages = _context.TasksFanpages.Where(w => w.IdTask == request.taskId).ToList();
            var task = _context.Tasks.Find(request.taskId);

            if(task.Status == 7)
            {
                return "";
            }

            foreach (var item in taskFanpages)
            {
                BackgroundJob.Delete(item.IdJob);
            }

            _context.RemoveRange(taskFanpages);

            task.Status = 8;

            _context.Update(task);
            await _context.SaveChangesAsync();

            return "success";
        }
    }
}
