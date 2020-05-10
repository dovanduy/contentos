using ContentProccessService.Entities;
using ContentProccessService.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace ContentProccessService.Application.Queries.GetTags
{
    public class GetTagHandler : IRequestHandler<GetTagRequest, IEnumerable<TagViewModel>>
    {
        private readonly ContentoDbContext _context;
        public GetTagHandler(ContentoDbContext contentodbContext)
        {
            _context = contentodbContext;
        }

        public async Task<IEnumerable<TagViewModel>> Handle(GetTagRequest request, CancellationToken cancellationToken)
        {
            var tags = await _context.Tags.AsNoTracking().Select(x=> new TagViewModel
            {
                id = x.Id,
                name = x.Name
            }).ToListAsync();
            return tags;
        }
    }
}
