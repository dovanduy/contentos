
using ContentProccessService.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace ContentProccessService.Application.Commands.UpdateTaskChannel
{
    public class UpdateTaskChannelHandler : IRequestHandler<UpdateTaskChannelRequest, TasksFanpages>
    {
        private readonly ContentoDbContext _context;

        public UpdateTaskChannelHandler(ContentoDbContext context)
        {
            _context = context;
        }

        public async Task<TasksFanpages> Handle(UpdateTaskChannelRequest request, CancellationToken cancellationToken)
        {
            //TasksChannels taskchannel = _context.TasksFanpages.Find(request.IdTaskChannel);
            //_context.TasksFanpages.Remove(taskchannel);
            TasksFanpages taskchannel = new TasksFanpages();
            await _context.SaveChangesAsync(cancellationToken);
            return taskchannel;
        }
    }
}

