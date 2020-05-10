using BatchjobService.Entities;
using BatchjobService.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace BatchjobService.Application.Queries.GetFanpages
{
    public class GetFanpagesHandler : IRequestHandler<GetFanpagesRequest, List<FanpageViewModel>>
    {
        private readonly ContentoDbContext _context;

        public GetFanpagesHandler(ContentoDbContext context)
        {
            _context = context;
        }

        public async Task<List<FanpageViewModel>> Handle(GetFanpagesRequest request, CancellationToken cancellationToken)
        {
            var fanpages = await _context.Fanpages
                .Include(i => i.IdChannelNavigation)
                .Include(i => i.FanpagesTags).ThenInclude(FanpagesTags=> FanpagesTags.IdTagNavigation)
                .Where(w => w.IdMarketer == request.Id || w.IdMarketer == null).ToListAsync();

            List<FanpageViewModel> listFanpages = new List<FanpageViewModel>();

            if (fanpages == null)
            {
                return listFanpages;
            }

            foreach (var fanpage in fanpages)
            {
                FanpageViewModel model = new FanpageViewModel();

                model.Id = fanpage.Id;
                model.Name = fanpage.Name;
                model.Channel = new Channel { Id = fanpage.IdChannelNavigation.Id, Name = fanpage.IdChannelNavigation.Name };
                if(fanpage.IdCustomer != null)
                {
                    var customer = _context.Users.Find(fanpage.IdCustomer);
                    model.Customer = new Customer { Id = customer.Id, Name = customer.FirstName + " " + customer.LastName };
                }
                else
                {
                    model.Customer = new Customer { Id = 0, Name = "" };
                }
                model.ModifiedDate = fanpage.ModifiedDate;
                var lstTag = new List<int>();
                var returnTags = new List<TagModel>();
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

                model.Token = fanpage.Token;

                model.Tags = returnTags;
                model.TagId = lstTag;
                listFanpages.Add(model);
            }

            return listFanpages;
        }
    }
}
