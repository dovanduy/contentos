using ContentProccessService.Entities;
using ContentProccessService.Models;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace ContentProccessService.Application.Commands.CreateListTaskChannel
{
    public class CreateListTaskChannelHandler : IRequestHandler<CreateListTaskChannelRequest, List<TaskChannelModelRespone>>
    {
        private readonly ContentoDbContext contentodbContext;

        public CreateListTaskChannelHandler(ContentoDbContext contentodbContext)
        {
            this.contentodbContext = contentodbContext;
        }

        public async Task<List<TaskChannelModelRespone>> Handle(CreateListTaskChannelRequest request, CancellationToken cancellationToken)
        {
            List<TaskChannelModelRespone> Listtaskschannel = new List<TaskChannelModelRespone>();
            foreach (var item in request.ListTaskChannel)
            {
                var taskchannel = new TasksFanpages
                {
                    IdFanpage = item.IdChannel,
                    IdTask = item.IdTask,
                    ModifiedDate = DateTime.UtcNow,
                    CreatedDate = DateTime.UtcNow,
                };
                contentodbContext.Attach(taskchannel);
                //contentodbContext.TasksChannels.Add(taskchannel);
                await contentodbContext.SaveChangesAsync(cancellationToken);
                TaskChannelModelRespone TaskChannels = new TaskChannelModelRespone
                {
                    //id = taskchannel.id,
                    IdChannel = taskchannel.IdFanpage,
                    IdTask = taskchannel.IdTask
                };
                Listtaskschannel.Add(TaskChannels);

            }
            return Listtaskschannel;
        }

    }
}
