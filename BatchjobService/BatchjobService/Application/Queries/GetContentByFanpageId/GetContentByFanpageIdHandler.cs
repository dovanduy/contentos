using BatchjobService.Entities;
using BatchjobService.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace BatchjobService.Application.Queries.GetContentByFanpageId
{
    public class GetContentByFanpageIdHandler : IRequestHandler<GetContentByFanpageIdRequest, List<ContentModel>>
    {
        private readonly ContentoDbContext _context;

        public GetContentByFanpageIdHandler(ContentoDbContext context)
        {
            _context = context;
        }
        public async Task<List<ContentModel>> Handle(GetContentByFanpageIdRequest request, CancellationToken cancellationToken)
        {
            var fanpages = await _context.TasksFanpages.AsNoTracking().Include(i => i.IdTaskNavigation)
                .ThenInclude(IdTaskNavigation => IdTaskNavigation.Contents)
                .Where(w => w.IdFanpage == request.id).ToListAsync();

            var tags = _context.TasksTags.AsNoTracking().Include(i => i.IdTagNavigation);

            List<ContentModel> result = new List<ContentModel>();

            foreach(var fanpage in fanpages)
            {
                ContentModel model = new ContentModel();
                var tagsFanpage = tags.Where(w => w.IdTask == fanpage.IdTask);

                model.id = fanpage.IdTaskNavigation.Id;
                model.name = fanpage.IdTaskNavigation.Contents.FirstOrDefault(f => f.IsActive == true).Name;
                model.publish_time = fanpage.IdTaskNavigation.PublishTime;
                model.isAds = fanpage.IdTaskNavigation.IsAds;

                List<Tag> lstTag = new List<Tag>();
                foreach(var tag in tagsFanpage)
                {
                    lstTag.Add(new Tag { id = tag.IdTag, name = tag.IdTagNavigation.Name });
                }

                model.listTag = lstTag;

                result.Add(model);
            }

            return result;
        }
    }
}
