using BatchjobService.Entities;
using BatchjobService.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace BatchjobService.Application.Queries.GetDetailFanpage
{
    public class GetDetailFanpageHandler : IRequestHandler<GetDetailFanpageRequest, EditViewModel>
    {
        private readonly ContentoDbContext _context;

        public GetDetailFanpageHandler(ContentoDbContext context)
        {
            _context = context;
        }

        public async Task<EditViewModel> Handle(GetDetailFanpageRequest request, CancellationToken cancellationToken)
        {
            Fanpages fanpage = await _context.Fanpages.AsNoTracking()
                .Include(x=>x.IdChannelNavigation)
                .Include(x=>x.FanpagesTags).ThenInclude(FanpagesTags=> FanpagesTags.IdTagNavigation)
                .FirstOrDefaultAsync(x=>x.Id == request.Id);

            //_context.Entry(fanpage).Reference(p => p.IdChannelNavigation).Load();

            EditViewModel model = new EditViewModel();

            model.Id = fanpage.Id;
            model.Name = fanpage.Name;
            model.Channel = fanpage.IdChannelNavigation.Id;
            if (fanpage.IdCustomer != null)
            {
                var customer = _context.Users.Find(fanpage.IdCustomer);
                model.Customer = customer.Id;
            }
            var lstTag = new List<int>();
            var returnTags = new List<TagModel>();
            if (fanpage.FanpagesTags.Count != 0)
            {
                foreach (var item in fanpage.FanpagesTags)
                {
                    var returnTag = new TagModel
                    {

                        Id = item.IdTag,
                        Name = item.IdTagNavigation.Name
                    };
                    returnTags.Add(returnTag);
                    lstTag.Add(item.IdTag);
                }
            }
            model.Tags = returnTags;
            model.TagId = lstTag;
            model.ModifiedDate = fanpage.ModifiedDate;
            model.Token = fanpage.Token;

            return model;
        }
    }
}
