using ContentProccessService.Entities;
using ContentProccessService.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace ContentProccessService.Application.Queries.GetStatusPublish
{

    public class GetStatusPublishHandler : IRequestHandler<GetStatusPublishRequest, List<StatusModelsReturn>>
    {
        private readonly ContentoDbContext Context;

        public GetStatusPublishHandler(ContentoDbContext context)
        {
            Context = context;
        }
        public async Task<List<StatusModelsReturn>> Handle(GetStatusPublishRequest request, CancellationToken cancellationToken)
        {
            return await Context.StatusTasks.AsNoTracking().Where(x => x.Id == 5 || x.Id == 6 || x.Id == 7 || x.Id == 8).Select(x =>
           new StatusModelsReturn
           {
               Id = x.Id,
               Name = x.Name
           }).ToListAsync();

        }
    }
}
