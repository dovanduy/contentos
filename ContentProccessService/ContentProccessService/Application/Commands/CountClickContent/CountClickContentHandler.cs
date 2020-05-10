using ContentProccessService.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace ContentProccessService.Application.Commands.CountClickContent
{
    public class CountClickContentHandler : IRequestHandler<CountClickContentCommands>
    {
        private readonly ContentoDbContext contentodbContext;

        public CountClickContentHandler(ContentoDbContext contentodbContext)
        {
            this.contentodbContext = contentodbContext;
        }
        public async Task<Unit> Handle(CountClickContentCommands request, CancellationToken cancellationToken)
        {
            //contentodbContext.Personalizations.Where(x => request.Tags.Contains(x.IdTag) && x.IdUser == request.IdUser).ToList().ForEach(x => x.TimeInteraction  += 1);
            var checl = await contentodbContext.UsersInteractions.FirstOrDefaultAsync(x => x.IdTask == request.IdTask && x.IdUser == request.IdUser);
            var checkStatic = contentodbContext.Statistics.FirstOrDefault(x => x.IdTask == request.IdTask && x.CreatedDate.GetValueOrDefault().DayOfYear == DateTime.UtcNow.DayOfYear);
            if (checkStatic == null)
            {
                var statics = new Statistics
                {
                    IdTask = request.IdTask,
                    CreatedDate = DateTime.UtcNow,
                    Views = 0
                };
                contentodbContext.Attach(statics);
                contentodbContext.Statistics.Add(statics);
                await contentodbContext.SaveChangesAsync(cancellationToken);
            }
            if (checl == null && request.IdUser != 0)
            {
                var userInter = new UsersInteractions
                {
                    IdTask = request.IdTask,
                    IdUser = request.IdUser,
                    Interaction = 0
                };
                contentodbContext.Attach(userInter);
                contentodbContext.UsersInteractions.Add(userInter);
                await contentodbContext.SaveChangesAsync(cancellationToken);
            }
            contentodbContext.Statistics.FirstOrDefault(x => x.IdTask == request.IdTask && x.CreatedDate.GetValueOrDefault().DayOfYear == DateTime.UtcNow.DayOfYear).Views += 1;
            if (request.IdUser != 0)
            {
                contentodbContext.UsersInteractions.FirstOrDefault(x => x.IdTask == request.IdTask && x.IdUser == request.IdUser).Interaction += 1;
            }
            await contentodbContext.SaveChangesAsync(cancellationToken);
            return Unit.Value;
        }
    }
}
