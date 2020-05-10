using BatchjobService.Entities;
using Hangfire;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace BatchjobService.Application.Command.DeleteFanpage
{
    public class DeleteFanpageHandler : IRequestHandler<DeleteFanpageCommand, Unit>
    {
        private readonly ContentoDbContext _context;

        public DeleteFanpageHandler(ContentoDbContext context)
        {
            _context = context;
        }

        public async Task<Unit> Handle(DeleteFanpageCommand request, CancellationToken cancellationToken)
        {
            Fanpages fanpage = _context.Fanpages.Find(request.id);

            var fanpagesInteraction = _context.FanpagesInteraction.Where(w => w.IdFanpages == request.id).ToList();

            _context.Entry(fanpage).Collection(r => r.FanpagesTags).Load();
            _context.Entry(fanpage).Collection(r => r.TasksFanpages).Load();

            foreach(var item in fanpage.TasksFanpages)
            {
                BackgroundJob.Delete(item.IdJob);
            }

            _context.RemoveRange(fanpage.TasksFanpages);
            _context.RemoveRange(fanpage.FanpagesTags);
            _context.RemoveRange(fanpagesInteraction);
            _context.Remove(fanpage);

            await _context.SaveChangesAsync();

            return Unit.Value;
        }
    }
}
