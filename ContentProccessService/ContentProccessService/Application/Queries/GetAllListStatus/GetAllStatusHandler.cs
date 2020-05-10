using ContentProccessService.Entities;
using ContentProccessService.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace ContentProccessService.Application.Queries.GetAllListStatus
{
    public class GetAllStatusHandler : IRequestHandler<GetAllStatusRequest, List<StatusModelsReturn>>
    {
        private readonly ContentoDbContext Context;

        public GetAllStatusHandler(ContentoDbContext context)
        {
            Context = context;
        }
        public async Task<List<StatusModelsReturn>> Handle(GetAllStatusRequest request, CancellationToken cancellationToken)
        {
            return  await Context.StatusTasks.AsNoTracking().Select(x=>
            new StatusModelsReturn
            {
                Id = x.Id,
                Name = x.Name
            }).ToListAsync();
            
        }
    }
}
